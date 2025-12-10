using System.ComponentModel.DataAnnotations;

namespace API.Ecommerce.Models.Auth
{
    public class UsuariosAuth
    {
        public int Id { get; set; }

        [Required, MaxLength(150), EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int RolId { get; set; }
        public Rol? Rol { get; set; }
    }
}
