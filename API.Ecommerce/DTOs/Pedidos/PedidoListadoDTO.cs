namespace API.Ecommerce.DTOs.Pedidos
{
    public class PedidoListadoDTO
    {
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Estatus { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}
