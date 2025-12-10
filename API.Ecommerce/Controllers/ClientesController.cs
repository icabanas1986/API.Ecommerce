using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Ecommerce.DTOs;
using API.Ecommerce.DTOs.Cliente;
using API.Ecommerce.Services.Interfaces;

namespace API.Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // requiere token; si quieres público quita esta línea
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;
        private readonly IAuthServices _authServices;
        public ClientesController(IClienteService service, IAuthServices authServices)
        {
            _service = service;
            _authServices = authServices;
        }
        // GET /api/clientes?page=1&pageSize=20&search=juan
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? search = null)
        {
            var (items, total) = await _service.GetPagedAsync(page, pageSize, search);
            return Ok(new { items, total, page, pageSize });
        }

        // GET /api/clientes/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var c = await _service.GetByIdAsync(id);
            return c == null ? NotFound() : Ok(c);
        }

        // POST /api/clientes
        [HttpPost]
        [Authorize(Roles = "Administrador, Vendedor")] // opcional
        public async Task<IActionResult> Post([FromBody] ClienteCreateDto dto)
        {
            try
            {
                var creado = await _authServices.RegisterClienteAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = creado }, creado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT /api/clientes/5
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrador, Vendedor")]
        public async Task<IActionResult> Put(int id, [FromBody] ClienteUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            try
            {
                var actualizado = await _service.UpdateAsync(dto);
                return actualizado == null ? NotFound() : Ok(actualizado);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE /api/clientes/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
