namespace API.Ecommerce.DTOs.Pagos
{
    public class PagoCreateDto
    {
        public int? PedidoId { get; set; }
        public int? ClienteId { get; set; }
        public decimal Monto { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public string? Referencia { get; set; }
        public List<PagoDetalleCreateDto>? Detalles { get; set; }
    }
}
