using API.Ecommerce.DTOs.Carrito;
using API.Ecommerce.Models;

namespace API.Ecommerce.Services.Interfaces
{
    public interface ICarritoService
    {
        Task<CarritoResponseDTO> ObtenerCarrito(int clienteId);
        Task<CarritoResponseDTO> AgregarProducto(AgregarProductoCarritoDTO dto);
        Task VaciarCarrito(int clienteId);
        Task EliminarProducto(int clienteId, int productoId);
    }
}
