using System.ComponentModel.DataAnnotations;

namespace API.Ecommerce.Models
{
    public class DireccionCliente
    {
        public int Id { get; set; }

        [Required]
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Required, MaxLength(150)]
        public string Calle { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Colonia { get; set; }

        [MaxLength(10)]
        public string? CodigoPostal { get; set; }

        [MaxLength(100)]
        public string? Ciudad { get; set; }

        [MaxLength(100)]
        public string? Estado { get; set; }

        [MaxLength(100)]
        public string? Pais { get; set; }

        public bool EsPrincipal { get; set; } = false;

        public bool Activo { get; set; } = true;

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    }
}
