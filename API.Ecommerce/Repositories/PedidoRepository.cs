using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationDbContext _context;

        public PedidoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CrearAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<List<Pedido>> ObtenerTodos()
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estatus)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }

        public async Task<List<Pedido>> ObtenerPorCliente(int clienteId)
        {
            return await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Estatus)
                .Where(p => p.ClienteId == clienteId)
                .OrderByDescending(p => p.Fecha)
                .ToListAsync();
        }

    }
}
