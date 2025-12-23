using API.Ecommerce.DTOs.Direcciones;
using API.Ecommerce.Models;
using API.Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DireccionClienteController : Controller
    {
        private readonly IDireccionClienteService _service;

        public DireccionClienteController(IDireccionClienteService service)
        {
            _service = service;
        }

        [HttpGet("{clienteId}")]
        public async Task<IActionResult> GetByCliente(int clienteId)
        {
            var result = await _service.ObtenerPorCliente(clienteId);
            return Ok(result);
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Crear(CreaDireccionDTO direccion)
        {
            await _service.CrearAsync(direccion);
            return Ok(new { message = "Dirección creada correctamente" });
        }

        [HttpPut]
        [Route("Actualizar")]
        public async Task<IActionResult> Actualizar(ActualizaDireccionDTO direccion)
        {
            await _service.ActualizarAsync(direccion);
            return Ok(new { message = "Dirección actualizada correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _service.EliminarAsync(id);
            return Ok(new { message = "Dirección eliminada correctamente" });
        }
    }
}
