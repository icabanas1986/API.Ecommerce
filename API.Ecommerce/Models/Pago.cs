using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Ecommerce.Models
{
    public class Pago
    {
        public int Id { get; set; }

        // Relación opcional con Pedido (un pago puede estar ligado a un pedido)
        public int? PedidoId { get; set; }
        public Pedido? Pedido { get; set; }

        // Relación opcional con Cliente (pagos sin pedido, por ejemplo abonos)
        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Monto { get; set; }

        [Required, MaxLength(50)]
        public string Metodo { get; set; } = string.Empty; // Ej: "Efectivo", "Tarjeta", "Transferencia"

        [MaxLength(200)]
        public string? Referencia { get; set; } // número de autorización, transacción, etc.

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Lista de detalles si necesitas desglosar (ej: pago dividido en tarjetas y efectivo)
        public List<PagoDetalle> Detalles { get; set; } = new();
    }
}
