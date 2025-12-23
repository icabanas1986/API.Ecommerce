using API.Ecommerce.Models;

namespace API.Ecommerce.Repositories.Interfaces
{
    public interface ICarritoRepository
    {
        Task<Carrito?> ObtenerCarritoActivo(int clienteId);
        Task CrearAsync(Carrito carrito);
        Task SaveChangesAsync();
    }
}
