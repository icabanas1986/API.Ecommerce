using API.Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Ecommerce.Controllers
{
    [ApiController]
    [Route("api/pedidos")]
    public class PedidoConsultaController : ControllerBase
    {
        private readonly IPedidoConsultaService _service;

        public PedidoConsultaController(IPedidoConsultaService service)
        {
            _service = service;
        }

        // 🛠 PANEL ADMIN
        [HttpGet("admin")]
        public async Task<IActionResult> ObtenerTodos()
        {
            return Ok(await _service.ObtenerTodos());
        }

        // 👤 PEDIDOS POR CLIENTE
        [HttpGet("cliente/{clienteId}")]
        public async Task<IActionResult> ObtenerPorCliente(int clienteId)
        {
            return Ok(await _service.ObtenerPorCliente(clienteId));
        }

        // 📦 DETALLE
        [HttpGet("{pedidoId}")]
        public async Task<IActionResult> Detalle(int pedidoId)
        {
            var pedido = await _service.ObtenerDetalle(pedidoId);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }
    }
}
