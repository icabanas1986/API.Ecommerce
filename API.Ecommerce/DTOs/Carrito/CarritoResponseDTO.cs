namespace API.Ecommerce.DTOs.Carrito
{
    public class CarritoResponseDTO
    {
        public int CarritoId { get; set; }
        public int ClienteId { get; set; }
        public List<CarritoItemResponseDTO> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
