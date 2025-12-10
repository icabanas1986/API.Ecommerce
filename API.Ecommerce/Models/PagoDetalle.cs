using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Ecommerce.Models
{
    public class PagoDetalle
    {
        public int Id { get; set; }

        public int PagoId { get; set; }
        public Pago Pago { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [MaxLength(50)]
        public string Metodo { get; set; } = string.Empty;

        [MaxLength(200)]
        public string? Referencia { get; set; }
    }
}
