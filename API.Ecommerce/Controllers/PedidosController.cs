using Microsoft.AspNetCore.Mvc;
using API.Ecommerce.DTOs.Pedidos;
using API.Ecommerce.Services.Interfaces;

namespace API.Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _service;

        public PedidosController(IPedidoService service)
        {
            _service = service;
        }

        [HttpPost("crear-desde-carrito")]
        public async Task<IActionResult> CrearDesdeCarrito([FromBody] CrearPedidoDTO dto)
        {
            var pedido = await _service.CrearDesdeCarrito(dto);
            return Ok(pedido);
        }
    }
}
