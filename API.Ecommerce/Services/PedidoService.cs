using API.Ecommerce.DTOs.Pedidos;
using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using API.Ecommerce.Services.Interfaces;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _repo;
        private readonly ApplicationDbContext _ctx;

        public PedidoService(IPedidoRepository repo, ApplicationDbContext ctx)
        {
            _repo = repo;
            _ctx = ctx;
        }

        public PedidoDto Map(Pedido p)
        {
            return new PedidoDto
            {
                Id = p.Id,
                ClienteId = p.ClienteId,
                ClienteNombre = p.Cliente?.Nombre ?? "",
                Fecha = p.Fecha,
                Total = p.Total,
                EstatusId = p.EstatusId,
                EstatusNombre = p.Estatus?.Nombre ?? "",
                EstatusColor = p.Estatus?.ColorHex,
                Detalles = p.Detalles.Select(d => new PedidoDetalleDto
                {
                    Id = d.Id,
                    ProductoId = d.ProductoId,
                    ProductoNombre = d.Producto?.Nombre ?? "",
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                }).ToList()
            };
        }

        public async Task<PedidoDto?> GetByIdAsync(int id)
        {
            var pedido = await _repo.GetByIdAsync(id);
            return pedido == null ? null : Map(pedido);
        }

        public async Task<IEnumerable<PedidoDto>> GetByClienteAsync(int clienteId)
        {
            var pedidos = await _repo.GetByClienteAsync(clienteId);
            return pedidos.Select(Map);
        }

        public async Task<(IEnumerable<PedidoDto> Items, int Total)> GetPagedAsync(int page, int pageSize, DateTime? desde, DateTime? hasta, int? clienteId)
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, desde, hasta, clienteId);
            return (items.Select(Map), total);
        }

        public async Task<PedidoDto> CreateAsync(PedidoCreateDto dto)
        {
            var cliente = await _ctx.Clientes.FindAsync(dto.ClienteId);
            if (cliente == null)
                throw new InvalidOperationException("Cliente no encontrado");

            var pedido = new Pedido
            {
                ClienteId = dto.ClienteId,
                Fecha = DateTime.UtcNow
            };

            decimal total = 0;

            foreach (var item in dto.Detalles)
            {
                var producto = await _ctx.Productos.FindAsync(item.ProductoId);
                if (producto == null)
                    throw new InvalidOperationException("Producto no encontrado");

                var detalle = new PedidoDetalle
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = producto.Precio
                };
                pedido.Detalles.Add(detalle);

                total += detalle.Subtotal;
            }

            pedido.Total = total;

            var creado = await _repo.AddAsync(pedido);

            return Map(creado);
        }

        public async Task<PedidoDto?> UpdateAsync(PedidoUpdateDto dto)
        {
            var pedido = await _repo.GetByIdAsync(dto.Id);
            if (pedido == null) return null;

            pedido.ClienteId = dto.ClienteId;

            var actualizado = await _repo.UpdateAsync(pedido);
            return Map(actualizado);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
