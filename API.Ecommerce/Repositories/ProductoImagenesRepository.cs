using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Repositories
{
    public class ProductoImagenesRepository : IProductoImagenesRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoImagenesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> GuardarImagenesAsync(List<ProductoImagen> imagenes)
        {
            IEnumerable<string> urls;
            try
            {
                _context.ProductoImagenes.AddRange(imagenes);
                var result = await _context.SaveChangesAsync();
                urls = imagenes.Select(img => img.Url);
            }
            catch (Exception ex)
            {
                return await Task.FromException<IEnumerable<string>>(ex);
            }
            return await Task.FromResult(urls);
        }

        public Task<IEnumerable<ProductoImagen>> ObtenerImagenesPorProductoIdAsync(int productoId)
        {
            var imagenes = _context.ProductoImagenes.Where(img => img.ProductoId == productoId).AsEnumerable();
            return Task.FromResult(imagenes);
        }

        public Task<bool> EliminaImagenesPorIdProductoAsync(int productoId)
        {
            try
            {
                var imagenes = _context.ProductoImagenes.Where(img => img.ProductoId == productoId);
                _context.ProductoImagenes.RemoveRange(imagenes);
                _context.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromException<bool>(ex);
            }
        }
    }
}
