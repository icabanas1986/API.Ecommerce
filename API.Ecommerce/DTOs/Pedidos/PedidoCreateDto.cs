namespace API.Ecommerce.DTOs.Pedidos
{
    public class PedidoCreateDto
    {
        public int ClienteId { get; set; }
        public int EstatusId { get; set; } = 1;
        public List<PedidoDetalleCreateDto> Detalles { get; set; } = new();
    }

    public class PedidoDetalleCreateDto
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
