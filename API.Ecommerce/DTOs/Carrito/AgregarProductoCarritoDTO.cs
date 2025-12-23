namespace API.Ecommerce.DTOs.Carrito
{
    public class AgregarProductoCarritoDTO
    {
        public int ClienteId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
