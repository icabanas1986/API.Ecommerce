using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using API.Ecommerce.Models;
using API.Ecommerce.Models.Auth;

namespace TPVY.API.Ecommerce.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public ApplicationDbContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configura la conexión manualmente solo en modo diseño
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cliente>()
    .HasOne(c => c.UsuarioAuth)
    .WithOne()
    .HasForeignKey<Cliente>(c => c.IdAuth)
    .OnDelete(DeleteBehavior.Restrict);

            // Relaciones
            modelBuilder.Entity<Rol>()
                .HasMany(r => r.Usuarios)
                .WithOne(u => u.Rol)
                .HasForeignKey(u => u.RolId);

            // Datos iniciales
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador" },
                new Rol { Id = 2, Nombre = "Vendedor" }
            );

            modelBuilder.Entity<Categoria>()
        .HasMany(c => c.Productos)
        .WithOne(p => p.Categoria)
        .HasForeignKey(p => p.CategoriaId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Categoria>().HasData(
        new Categoria { Id = 1, Nombre = "Lectores de Código de Barras" },
        new Categoria { Id = 2, Nombre = "Impresoras Térmicas" },
        new Categoria { Id = 3, Nombre = "Cajas Registradoras" }
            );

            modelBuilder.Entity<Pago>().Property(p => p.Monto).HasPrecision(18, 2);
            modelBuilder.Entity<PagoDetalle>().Property(d => d.Monto).HasPrecision(18, 2);

            modelBuilder.Entity<Pago>()
        .HasMany(p => p.Detalles)
        .WithOne(d => d.Pago)
        .HasForeignKey(d => d.PagoId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pedido>()
        .HasMany(p => p.Detalles)
        .WithOne(d => d.Pedido)
        .HasForeignKey(d => d.PedidoId)
        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Pedido>()
                .Property(p => p.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PedidoDetalle>()
                .Property(p => p.PrecioUnitario)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Pedido>()
        .HasOne(p => p.Estatus)
        .WithMany()
        .HasForeignKey(p => p.EstatusId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DireccionCliente>()
        .HasOne(d => d.Cliente)
        .WithMany(c => c.Direcciones)
        .HasForeignKey(d => d.ClienteId)
        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Carrito>()
    .HasMany(c => c.Items)
    .WithOne(i => i.Carrito)
    .HasForeignKey(i => i.CarritoId);

            modelBuilder.Entity<PedidoEstatus>().HasData(
       new PedidoEstatus { Id = 1, Nombre = "Pendiente", ColorHex = "#FFA500", Activo = true },
       new PedidoEstatus { Id = 2, Nombre = "Pagado", ColorHex = "#28A745", Activo = true },
       new PedidoEstatus { Id = 3, Nombre = "Cancelado", ColorHex = "#DC3545", Activo = true });
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<ProductoImagen> ProductoImagenes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<PedidoDetalle> PedidoDetalles { get; set; }

        public DbSet<UsuariosAuth> UsuariosAuth { get; set; }
        public DbSet<Rol> Roles { get; set; }

        public DbSet<Pago> Pagos { get; set; }
        public DbSet<PagoDetalle> PagoDetalles { get; set; }
        public DbSet<PedidoEstatus> PedidoEstatuses { get; set; }
        public DbSet<Carrito> Carrito
        {
            get; set;
        }
        public DbSet<CarritoItem> CarritoItem
        {
            get; set;
        }
        public DbSet<DireccionCliente> DireccionesCliente
        {
            get; set;
        }
    }
}
