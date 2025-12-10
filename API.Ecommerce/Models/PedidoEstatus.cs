using System.ComponentModel.DataAnnotations;

namespace API.Ecommerce.Models
{
    public class PedidoEstatus
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        // Opcional: Para asignar colores en el dashboard
        [MaxLength(20)]
        public string? ColorHex { get; set; }
    }
}
