using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Ecommerce.DTOs.Categorias;
using API.Ecommerce.Models;
using API.Ecommerce.Services.Interfaces;

namespace API.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _service;
        public CategoriasController(ICategoriaService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.ObtenerCategorias());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cat = await _service.ObtenerPorId(id);
            if (cat == null) return NotFound();
            return Ok(cat);
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Create(CreaCategoriaDTO categoria)
        {
            await _service.CrearCategoria(categoria);
            return Ok(new { message = "Categoría creada correctamente" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ActualizaCategoriaDTO categoria)
        {
            if (id != categoria.IdCategoria) return BadRequest();
            await _service.ActualizarCategoria(categoria);
            return Ok(new { message = "Categoría actualizada" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.EliminarCategoria(id);
            return Ok(new { message = "Categoría eliminada" });
        }
    }
}
