using System.ComponentModel.DataAnnotations;

namespace API.Ecommerce.DTOs.Cliente
{
    public class ClienteUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Apellidos { get; set; }

        [Required, EmailAddress, MaxLength(150)]
        public string Correo { get; set; } = string.Empty;

        [MaxLength(30)]
        public string? Telefono { get; set; }

        [MaxLength(300)]
        public string? Direccion { get; set; }

        [MaxLength(50)]
        public string? RazonSocial { get; set; }

        public bool Activo { get; set; }
    }
}
