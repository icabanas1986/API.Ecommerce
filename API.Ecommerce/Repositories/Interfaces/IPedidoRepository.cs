using API.Ecommerce.Models;

namespace API.Ecommerce.Repositories.Interfaces
{
    public interface IPedidoRepository
    {
        Task<Pedido?> GetByIdAsync(int id);
        Task<IEnumerable<Pedido>> GetByClienteAsync(int clienteId);
        Task<(IEnumerable<Pedido> Items, int Total)> GetPagedAsync(int page, int pageSize, DateTime? desde, DateTime? hasta, int? clienteId);
        Task<Pedido> AddAsync(Pedido pedido);
        Task<Pedido> UpdateAsync(Pedido pedido);
        Task<bool> DeleteAsync(int id);
    }
}
