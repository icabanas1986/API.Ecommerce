namespace API.Ecommerce.DTOs.Pedidos
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        public int EstatusId { get; set; }
        public string EstatusNombre { get; set; } = string.Empty;
        public string? EstatusColor { get; set; }

        public List<PedidoDetalleDto> Detalles { get; set; } = new();
    }
}
