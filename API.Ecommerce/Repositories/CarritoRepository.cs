using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Repositories
{
    public class CarritoRepository : ICarritoRepository
    {
        private readonly ApplicationDbContext _context;

        public CarritoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Carrito?> ObtenerCarritoActivo(int clienteId)
        {
            try
            {
                return await _context.Carrito
                .Include(c => c.Items)
                .ThenInclude(i => i.Producto)
                .ThenInclude(b => b.Imagenes)
                .FirstOrDefaultAsync(c => c.ClienteId == clienteId && c.Activo);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task CrearAsync(Carrito carrito)
        {
            await _context.Carrito.AddAsync(carrito);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
