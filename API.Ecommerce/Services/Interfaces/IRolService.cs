using API.Ecommerce.DTOs.Rol;
using API.Ecommerce.Models.Auth;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IRolService
    {
        Task<IEnumerable<Rol>> ObtenerTodosAsync();
        Task<Rol> ObtenerPorIdAsync(int id);
        Task<Rol> CrearAsync(RegistrerRolDTO rolDTO);
        Task ActualizarAsync(UpdateRolDTO rolDTO);
        Task EliminarAsync(int id);
    }
}
