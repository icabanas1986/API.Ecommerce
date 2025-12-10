using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Ecommerce.DTOs.Pagos;
using API.Ecommerce.Services.Interfaces;

namespace API.Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PagosController : ControllerBase
    {
        private readonly IPagoService _service;
        public PagosController(IPagoService service) => _service = service;

        // GET /api/pagos?page=1&pageSize=20&clienteId=&pedidoId=&desde=&hasta=
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20,
                                                [FromQuery] int? clienteId = null, [FromQuery] int? pedidoId = null,
                                                [FromQuery] DateTime? desde = null, [FromQuery] DateTime? hasta = null)
        {
            var (items, total) = await _service.GetPagedAsync(page, pageSize, clienteId, pedidoId, desde, hasta);
            return Ok(new { items, total, page, pageSize });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _service.GetByIdAsync(id);
            return p == null ? NotFound() : Ok(p);
        }

        // POST /api/pagos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PagoCreateDto dto)
        {
            try
            {
                var creado = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = creado.Id }, creado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT /api/pagos/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] PagoUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var actualizado = await _service.UpdateAsync(dto);
            return actualizado == null ? NotFound() : Ok(actualizado);
        }

        // DELETE /api/pagos/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }

        // GET /api/pagos/pedido/10
        [HttpGet("pedido/{pedidoId:int}")]
        public async Task<IActionResult> GetByPedido(int pedidoId)
        {
            var items = await _service.GetByPedidoAsync(pedidoId);
            return Ok(items);
        }

        // GET /api/pagos/cliente/5
        [HttpGet("cliente/{clienteId:int}")]
        public async Task<IActionResult> GetByCliente(int clienteId)
        {
            var items = await _service.GetByClienteAsync(clienteId);
            return Ok(items);
        }
    }
}
