using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace API.Ecommerce.DTOs.Cliente
{
    public class ClienteCreateDto
    {
        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ApellidoPaterno { get; set; }

        [MaxLength(100)]
        public string? ApellidoMaterno { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required, PasswordPropertyText]
        public string password { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? Telefono { get; set; }

        public string Contraseña { get; set; }
        public int rolId { get; set; }
    }
}
