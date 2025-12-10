using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Repositories
{
    public class PagoRepository: IPagoRepository
    {
        private readonly ApplicationDbContext _ctx;
        public PagoRepository(ApplicationDbContext ctx) => _ctx = ctx;

        public async Task<(IEnumerable<Pago> Items, int Total)> GetPagedAsync(int page, int pageSize, int? clienteId, int? pedidoId, DateTime? desde, DateTime? hasta)
        {
            var q = _ctx.Pagos.Include(p => p.Detalles).AsQueryable();

            if (clienteId.HasValue) q = q.Where(p => p.ClienteId == clienteId.Value);
            if (pedidoId.HasValue) q = q.Where(p => p.PedidoId == pedidoId.Value);
            if (desde.HasValue) q = q.Where(p => p.Fecha >= desde.Value);
            if (hasta.HasValue) q = q.Where(p => p.Fecha <= hasta.Value);

            var total = await q.CountAsync();
            var items = await q.OrderByDescending(p => p.Fecha)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToListAsync();

            return (items, total);
        }

        public async Task<Pago?> GetByIdAsync(int id) =>
       await _ctx.Pagos.Include(p => p.Detalles).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Pago> AddAsync(Pago pago)
        {
            _ctx.Pagos.Add(pago);
            await _ctx.SaveChangesAsync();
            return pago;
        }

        public async Task<Pago> UpdateAsync(Pago pago)
        {
            _ctx.Pagos.Update(pago);
            await _ctx.SaveChangesAsync();
            return pago;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _ctx.Pagos.FindAsync(id);
            if (p == null) return false;
            _ctx.Pagos.Remove(p);
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Pago>> GetByPedidoAsync(int pedidoId) =>
        await _ctx.Pagos.Where(p => p.PedidoId == pedidoId).Include(p => p.Detalles).ToListAsync();

        public async Task<IEnumerable<Pago>> GetByClienteAsync(int clienteId) =>
            await _ctx.Pagos.Where(p => p.ClienteId == clienteId).Include(p => p.Detalles).ToListAsync();
    }
}
