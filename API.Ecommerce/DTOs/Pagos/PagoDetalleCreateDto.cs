namespace API.Ecommerce.DTOs.Pagos
{
    public class PagoDetalleCreateDto
    {
        public decimal Monto { get; set; }
        public string Metodo { get; set; } = string.Empty;
        public string? Referencia { get; set; }
    }
}
