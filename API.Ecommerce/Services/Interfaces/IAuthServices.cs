using API.Ecommerce.DTOs.Cliente;
using API.Ecommerce.Models.Auth;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IAuthServices
    {
        Task<string> RegisterAsync(ClienteCreateDto dto);
        Task<ObtieneClienteDTO> LoginAsync(string email, string password);

        Task<bool> EliminaUsuario(int id);

        Task<bool> ActualizaUsuario(ClienteUpdateDto dto);

        Task<List<ClienteConRolDto>> ObtenerUsuarios();
        Task<string> RegisterClienteAsync(ClienteCreateDto dto);
    }
}
