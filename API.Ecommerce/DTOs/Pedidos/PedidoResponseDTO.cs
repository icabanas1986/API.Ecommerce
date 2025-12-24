namespace API.Ecommerce.DTOs.Pedidos
{
    public class PedidoResponseDTO
    {
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Estatus { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public List<PedidoDetalleResponseDTO> Detalles { get; set; } = new();
    }
}
