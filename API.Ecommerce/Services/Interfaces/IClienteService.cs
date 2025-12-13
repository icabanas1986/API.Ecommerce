using API.Ecommerce.DTOs.Cliente;
using API.Ecommerce.Models;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDto>> GetAllAsync();
        Task<(IEnumerable<ClienteDto> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search);
        Task<ClienteDto?> GetByIdAsync(int id);
        Task<ClienteDto> CreateAsync(ClienteCreateDto dto, int idAuth);
        Task<ClienteDto?> UpdateAsync(ClienteUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<Cliente?> GetByEmailAsync(string email);
    }
}
