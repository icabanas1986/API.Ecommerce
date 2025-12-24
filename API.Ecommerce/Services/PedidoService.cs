using API.Ecommerce.DTOs.Pedidos;
using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using API.Ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly ICarritoRepository _carritoRepo;
        private readonly IPedidoRepository _pedidoRepo;
        private readonly ApplicationDbContext _context;

        public PedidoService(
            ICarritoRepository carritoRepo,
            IPedidoRepository pedidoRepo,
            ApplicationDbContext context)
        {
            _carritoRepo = carritoRepo;
            _pedidoRepo = pedidoRepo;
            _context = context;
        }

        public async Task<PedidoResponseDTO> CrearDesdeCarrito(CrearPedidoDTO dto)
        {
            var carrito = await _carritoRepo.ObtenerCarritoActivo(dto.ClienteId);

            if (carrito == null || !carrito.Items.Any())
                throw new Exception("El carrito está vacío");

            // 🔹 Estatus inicial (Ej: Pendiente)
            var estatusInicial = await _context.PedidoEstatuses
                .FirstAsync(e => e.Nombre == "Pendiente");

            var pedido = new Pedido
            {
                ClienteId = dto.ClienteId,
                EstatusId = estatusInicial.Id,
                Total = carrito.Items.Sum(i => i.Cantidad * i.PrecioUnitario)
            };

            foreach (var item in carrito.Items)
            {
                pedido.Detalles.Add(new PedidoDetalle
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario
                });
            }

            // 🔹 Persistir pedido
            await _pedidoRepo.CrearAsync(pedido);

            // 🔹 Desactivar carrito
            carrito.Activo = false;

            await _pedidoRepo.SaveChangesAsync();

            return MapearPedido(pedido);
        }

        private PedidoResponseDTO MapearPedido(Pedido pedido)
        {
            return new PedidoResponseDTO
            {
                PedidoId = pedido.Id,
                Fecha = pedido.Fecha,
                Estatus = pedido.Estatus.Nombre,
                Total = pedido.Total,
                Detalles = pedido.Detalles.Select(d => new PedidoDetalleResponseDTO
                {
                    ProductoId = d.ProductoId,
                    NombreProducto = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Cantidad * d.PrecioUnitario
                }).ToList()
            };
        }
    }
}
