using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Repositories
{
    public class DireccionClienteRepository : IDireccionClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public DireccionClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateRangeAsync(List<DireccionCliente> direcciones)
        {
            _context.DireccionesCliente.UpdateRange(direcciones);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<List<DireccionCliente>> GetByClienteIdAsync(int clienteId)
        => await _context.DireccionesCliente
            .Where(d => d.ClienteId == clienteId && d.Activo)
            .ToListAsync();

        public async Task<DireccionCliente?> GetByIdAsync(int id)
            => await _context.DireccionesCliente.FindAsync(id);

        public async Task<bool> AddAsync(DireccionCliente direccion)
        {
            _context.DireccionesCliente.Add(direccion);
            var res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> UpdateAsync(DireccionCliente direccion)
        {
            try
            {
                _context.DireccionesCliente.Update(direccion);
                var res = await _context.SaveChangesAsync();
                return res > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var res = false;
            var dir = await _context.DireccionesCliente.FindAsync(id);
            if (dir != null)
            {
                dir.Activo = false;
                var resu = await _context.SaveChangesAsync();
                res = resu > 0;
            }
            return res;
        }

    }
}
