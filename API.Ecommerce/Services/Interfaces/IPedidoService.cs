using API.Ecommerce.DTOs.Pedidos;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoDto?> GetByIdAsync(int id);
        Task<IEnumerable<PedidoDto>> GetByClienteAsync(int clienteId);
        Task<(IEnumerable<PedidoDto> Items, int Total)> GetPagedAsync(int page, int pageSize, DateTime? desde, DateTime? hasta, int? clienteId);
        Task<PedidoDto> CreateAsync(PedidoCreateDto dto);
        Task<PedidoDto?> UpdateAsync(PedidoUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
