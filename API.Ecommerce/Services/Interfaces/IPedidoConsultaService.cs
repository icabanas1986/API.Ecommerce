using API.Ecommerce.DTOs.Pedidos;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IPedidoConsultaService
    {
        Task<List<PedidoListadoDTO>> ObtenerTodos();
        Task<List<PedidoListadoDTO>> ObtenerPorCliente(int clienteId);
        Task<PedidoDetalleDTO> ObtenerDetalle(int pedidoId);
    }
}
