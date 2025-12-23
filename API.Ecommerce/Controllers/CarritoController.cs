using API.Ecommerce.DTOs.Carrito;
using API.Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Ecommerce.Controllers
{
    [ApiController]
    [Route("api/carrito")]
    public class CarritoController : ControllerBase
    {
        private readonly ICarritoService _service;

        public CarritoController(ICarritoService service)
        {
            _service = service;
        }

        [HttpGet("{clienteId}")]
        public async Task<IActionResult> Obtener(int clienteId)
        {
            var carrito = await _service.ObtenerCarrito(clienteId);
            return Ok(carrito);
        }

        [HttpPost("agregar")]
        public async Task<IActionResult> Agregar([FromBody] AgregarProductoCarritoDTO dto)
        {
            await _service.AgregarProducto(dto);
            return Ok();
        }

        [HttpDelete("eliminar")]
        public async Task<IActionResult> Eliminar(int clienteId, int productoId)
        {
            await _service.EliminarProducto(clienteId,productoId);
            return Ok();
        }

        [HttpDelete("vaciar/{clienteId}")]
        public async Task<IActionResult> Vaciar(int clienteId)
        {
            await _service.VaciarCarrito(clienteId);
            return Ok();
        }
    }
}
