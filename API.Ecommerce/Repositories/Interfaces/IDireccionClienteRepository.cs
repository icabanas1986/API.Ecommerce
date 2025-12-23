using API.Ecommerce.Models;

namespace API.Ecommerce.Repositories.Interfaces
{
    public interface IDireccionClienteRepository
    {
        Task<List<DireccionCliente>> GetByClienteIdAsync(int clienteId);
        Task<DireccionCliente?> GetByIdAsync(int id);
        Task<bool> AddAsync(DireccionCliente direccion);
        Task<bool> UpdateAsync(DireccionCliente direccion);
        Task<bool> DeleteAsync(int id);
        Task<bool> UpdateRangeAsync(List<DireccionCliente> direcciones);
        Task SaveChangesAsync();
    }
}
