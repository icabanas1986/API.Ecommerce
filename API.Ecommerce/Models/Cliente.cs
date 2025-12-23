using API.Ecommerce.Models.Auth;
using System.ComponentModel.DataAnnotations;

namespace API.Ecommerce.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ApellidoPaterno { get; set; }

        [MaxLength(100)]
        public string? ApellidoMaterno { get; set; }

        [MaxLength(30)]
        public string? Telefono { get; set; }

        public bool Activo { get; set; } = true;
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public int IdAuth { get; set; }
        public UsuariosAuth? UsuarioAuth { get; set; }

        public ICollection<DireccionCliente> Direcciones { get; set; } = new List<DireccionCliente>();
    }
}
