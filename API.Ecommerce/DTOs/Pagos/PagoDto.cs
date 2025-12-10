namespace API.Ecommerce.DTOs.Pagos
{
    public class PagoDto
    {
        public int Id { get; set; }
        public int? PedidoId { get; set; }
        public int? ClienteId { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public string? Referencia { get; set; }
        public DateTime Fecha { get; set; }
        public List<PagoDetalleDto> Detalles { get; set; } = new();
    }
}
