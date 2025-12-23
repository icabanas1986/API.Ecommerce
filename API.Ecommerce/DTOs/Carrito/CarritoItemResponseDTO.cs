using API.Ecommerce.DTOs.ProductoImagen;

namespace API.Ecommerce.DTOs.Carrito
{
    public class CarritoItemResponseDTO
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public List<string> Imagenes { get; set; }
    }
}
