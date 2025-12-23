using API.Ecommerce.DTOs.Direcciones;
using API.Ecommerce.Models;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IDireccionClienteService
    {
        Task<List<ObtieneDireccionDTO>> ObtenerPorCliente(int clienteId);
        Task<bool> CrearAsync(CreaDireccionDTO direccion);
        Task<bool> ActualizarAsync(ActualizaDireccionDTO direccion);
        Task EliminarAsync(int id);
    }
}
