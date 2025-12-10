using API.Ecommerce.DTOs.Cliente;
using API.Ecommerce.Models.Auth;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IAuthServices
    {
        Task<string> RegisterAsync(ClienteCreateDto dto);
        Task<string> LoginAsync(string email, string password);

        Task<bool> EliminaUsuario(int id);

        Task<bool> ActualizaUsuario(int Id, string Nombre, string Email, int RolId, string Password);

        Task<List<UsuariosAuth>> ObtenerUsuarios();
        Task<string> RegisterClienteAsync(ClienteCreateDto dto);
    }
}
