using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Ecommerce.DTOs.Producto;
using API.Ecommerce.Models;
using API.Ecommerce.Services.Interfaces;

namespace API.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductoController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductoController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromServices] IWebHostEnvironment env)
        {
            var productos = await _productoService.ObtenerTodosAsync(env);
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromServices] IWebHostEnvironment env)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id, env);
            if (producto == null)
                return NotFound();
            return Ok(producto);
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Create(RegistrerProductoDTO producto, [FromServices] IWebHostEnvironment env)
        {
            if (producto.Imagenes == null || producto.Imagenes.Count == 0)
                return BadRequest("Debe enviar al menos una imagen.");

            var nuevoProducto = await _productoService.CrearAsync(producto, env);
            return CreatedAtAction(nameof(GetById), new { id = nuevoProducto }, nuevoProducto);
        }

        [HttpPut("actualizar")]
        public async Task<IActionResult> Update(UpdateProductoDTO producto, [FromServices] IWebHostEnvironment env)
        {
            await _productoService.ActualizarAsync(producto, env);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _productoService.EliminarAsync(id);
            return NoContent();
        }

    }
}
