namespace API.Ecommerce.DTOs.Pedidos
{
    public class PedidoDetalleItemDTO
    {
        public string Producto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}
