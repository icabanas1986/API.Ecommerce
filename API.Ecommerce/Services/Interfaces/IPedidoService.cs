using API.Ecommerce.DTOs.Pedidos;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponseDTO> CrearDesdeCarrito(CrearPedidoDTO dto);
    }
}
