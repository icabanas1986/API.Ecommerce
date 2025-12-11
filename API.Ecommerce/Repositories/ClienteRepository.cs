using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Repositories
{
    public class ClienteRepository :IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .AsNoTracking()
                .OrderByDescending(c => c.FechaRegistro)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Cliente> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search)
        {
            var query = _context.Clientes.AsNoTracking().Where(c=>c.UsuarioAuth.Rol.Nombre=="Cliente");

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(c =>
                    c.Nombre.ToLower().Contains(search) ||
                    (c.ApellidoPaterno != null && c.ApellidoPaterno.ToLower().Contains(search)) ||
                    (c.ApellidoMaterno != null && c.ApellidoMaterno.ToLower().Contains(search)) ||
                    (c.Telefono != null && c.Telefono.Contains(search))
                );
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.FechaRegistro)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Cliente?> GetByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<Cliente?> GetByEmailAsync(string email)
        {
            //return await _context.Clientes.FirstOrDefaultAsync(c => c.Correo == email);
            return new Cliente();
        }

        public async Task<Cliente> AddAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var c = await _context.Clientes.FindAsync(id);
            if (c == null) return false;
            _context.Clientes.Remove(c);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
