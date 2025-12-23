using System.ComponentModel.DataAnnotations.Schema;

namespace API.Ecommerce.Models
{
    public class CarritoItem
    {
        public int Id { get; set; }

        public int CarritoId { get; set; }
        public Carrito Carrito { get; set; } = null!;

        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

        [NotMapped]
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
