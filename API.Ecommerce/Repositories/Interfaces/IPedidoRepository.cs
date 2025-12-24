using API.Ecommerce.Models;

namespace API.Ecommerce.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<List<Pedido>> ObtenerTodos();
        Task<List<Pedido>> ObtenerPorCliente(int clienteId);
        Task CrearAsync(Pedido pedido);
        Task SaveChangesAsync();
    }
}
