using API.Ecommerce.DTOs.Pedidos;
using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using API.Ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Services
{
    public class PedidoConsultaService : IPedidoConsultaService
    {
        private readonly IPedidoRepository _repo;
        private readonly ApplicationDbContext _context;

        public PedidoConsultaService(IPedidoRepository repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<List<PedidoListadoDTO>> ObtenerTodos()
        {
            var pedidos = await _repo.ObtenerTodos();
            return pedidos.Select(MapearListado).ToList();
        }

        public async Task<List<PedidoListadoDTO>> ObtenerPorCliente(int clienteId)
        {
            var pedidos = await _repo.ObtenerPorCliente(clienteId);
            return pedidos.Select(MapearListado).ToList();
        }

        public async Task<PedidoDetalleDTO?> ObtenerDetalle(int pedidoId)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estatus)
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido == null)
                return null;

            return new PedidoDetalleDTO
            {
                PedidoId = pedido.Id,
                Fecha = pedido.Fecha,
                Cliente = $"{pedido.Cliente.Nombre} {pedido.Cliente.ApellidoPaterno} {pedido.Cliente.ApellidoMaterno}",
                Estatus = pedido.Estatus.Nombre,
                Total = pedido.Total,
                Detalles = pedido.Detalles.Select(d => new PedidoDetalleItemDTO
                {
                    Producto = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Cantidad * d.PrecioUnitario
                }).ToList()
            };
        }

        private PedidoListadoDTO MapearListado(Pedido p)
        {
            return new PedidoListadoDTO
            {
                PedidoId = p.Id,
                Fecha = p.Fecha,
                Cliente = $"{p.Cliente.Nombre} {p.Cliente.ApellidoPaterno} {p.Cliente.ApellidoMaterno}",
                Estatus = p.Estatus.Nombre,
                Total = p.Total
            };
        }
    }
}
