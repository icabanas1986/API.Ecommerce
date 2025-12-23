namespace API.Ecommerce.Models
{
    public class Carrito
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<CarritoItem> Items { get; set; } = new List<CarritoItem>();
    }
}
