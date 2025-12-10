using API.Ecommerce.DTOs.Pagos;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IPagoService
    {
        Task<(IEnumerable<PagoDto> Items, int Total)> GetPagedAsync(int page, int pageSize, int? clienteId, int? pedidoId, DateTime? desde, DateTime? hasta);
        Task<PagoDto?> GetByIdAsync(int id);
        Task<PagoDto> CreateAsync(PagoCreateDto dto);
        Task<PagoDto?> UpdateAsync(PagoUpdateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<PagoDto>> GetByPedidoAsync(int pedidoId);
        Task<IEnumerable<PagoDto>> GetByClienteAsync(int clienteId);
    }
}
