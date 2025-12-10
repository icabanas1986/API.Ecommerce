using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly ApplicationDbContext _ctx;

        public PedidoRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<Pedido?> GetByIdAsync(int id)
        {
            return await _ctx.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles).ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Pedido>> GetByClienteAsync(int clienteId)
        {
            return await _ctx.Pedidos
                .Where(p => p.ClienteId == clienteId)
                .Include(p => p.Detalles)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Pedido> Items, int Total)> GetPagedAsync(int page, int pageSize, DateTime? desde, DateTime? hasta, int? clienteId)
        {
            var q = _ctx.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Detalles)
                .AsQueryable();

            if (clienteId.HasValue)
                q = q.Where(p => p.ClienteId == clienteId.Value);

            if (desde.HasValue)
                q = q.Where(p => p.Fecha >= desde.Value);

            if (hasta.HasValue)
                q = q.Where(p => p.Fecha <= hasta.Value);

            var total = await q.CountAsync();

            var items = await q
                .OrderByDescending(p => p.Fecha)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Pedido> AddAsync(Pedido pedido)
        {
            _ctx.Pedidos.Add(pedido);
            await _ctx.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido> UpdateAsync(Pedido pedido)
        {
            _ctx.Pedidos.Update(pedido);
            await _ctx.SaveChangesAsync();
            return pedido;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var pedido = await _ctx.Pedidos.FindAsync(id);
            if (pedido == null) return false;

            _ctx.Pedidos.Remove(pedido);
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
