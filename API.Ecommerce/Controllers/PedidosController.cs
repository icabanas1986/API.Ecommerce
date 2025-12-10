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

        [HttpGet]
        public async Task<IActionResult> GetPaged(
        int page = 1,
        int pageSize = 20,
        DateTime? desde = null,
        DateTime? hasta = null,
        int? clienteId = null)
        {
            var (items, total) = await _service.GetPagedAsync(page, pageSize, desde, hasta, clienteId);
            return Ok(new { items, total, page, pageSize });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p = await _service.GetByIdAsync(id);
            return p == null ? NotFound() : Ok(p);
        }

        [HttpGet("cliente/{clienteId:int}")]
        public async Task<IActionResult> GetByCliente(int clienteId)
        {
            var items = await _service.GetByClienteAsync(clienteId);
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PedidoCreateDto dto)
        {
            var creado = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = creado.Id }, creado);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PedidoUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest();

            var actualizado = await _service.UpdateAsync(dto);
            return actualizado == null ? NotFound() : Ok(actualizado);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.DeleteAsync(id)
                ? NoContent()
                : NotFound();
        }
    }
}
