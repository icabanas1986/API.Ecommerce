using Microsoft.AspNetCore.Mvc;
using API.Ecommerce.Services.Interfaces;

namespace API.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosTiendaController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly ICategoriaService _categoriaService;
        public ProductosTiendaController(IProductoService productoService, ICategoriaService categoriaService)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
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

        [HttpGet("categoria/{categoriaId}")]
        public async Task<IActionResult> GetByCategoriaId(int categoriaId)
        {
            var productos = await _productoService.ObtenerPorCategoriaIdAsync(categoriaId);
            return Ok(productos);
        }

        [HttpGet("categorias")]
        public async Task<IActionResult> GetCategorias()
        {
            var categorias = await _categoriaService.ObtenerCategorias();
            return Ok(categorias);
        }
    }
}
