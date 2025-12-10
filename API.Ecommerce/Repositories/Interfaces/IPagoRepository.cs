using API.Ecommerce.Models;

namespace API.Ecommerce.Repositories.Interfaces
{
    public interface IPagoRepository
    {
        Task<(IEnumerable<Pago> Items, int Total)> GetPagedAsync(int page, int pageSize, int? clienteId, int? pedidoId, DateTime? desde, DateTime? hasta);
        Task<Pago?> GetByIdAsync(int id);
        Task<Pago> AddAsync(Pago pago);
        Task<Pago> UpdateAsync(Pago pago);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Pago>> GetByPedidoAsync(int pedidoId);
        Task<IEnumerable<Pago>> GetByClienteAsync(int clienteId);
    }
}
