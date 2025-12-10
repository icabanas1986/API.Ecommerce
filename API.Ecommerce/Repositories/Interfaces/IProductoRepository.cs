using API.Ecommerce.Models;

namespace API.Ecommerce.Repositories.Interfaces
{
    public interface IProductoRepository
    {
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<Producto> ObtenerPorIdAsync(int id);
        Task<Producto> CrearAsync(Producto producto);
        Task ActualizarAsync(Producto producto);
        Task EliminarAsync(int id);
        Task<List<Producto>> ObtenerPorCategoriaIdAsync(int idCategoria);
    }
}
