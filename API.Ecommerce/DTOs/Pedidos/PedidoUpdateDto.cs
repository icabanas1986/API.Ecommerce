namespace API.Ecommerce.DTOs.Pedidos
{
    public class PedidoUpdateDto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }

        public int EstatusId { get; set; }
    }
}
