using API.Ecommerce.DTOs.Cliente;
using API.Ecommerce.Models.Auth;

namespace API.Ecommerce.Data.Repository
{
    public interface IAuthRepository
    {
        Task<int> RegistraUsuario(UsuariosAuth auth);
        Task<UsuariosAuth> ObtieneUsuarioPorId(int id);
        Task<bool> ObtieneUsuarioPorCorreo(string correo);
        Task<UsuariosAuth?> GetByEmailAsync(string email);
        Task<bool> EliminaUsuario(int id);

        Task<bool> ActualizaUsuario(UsuariosAuth auth);
        Task<List<ClienteConRolDto>> ObtenerUsuarios();
    }
}
