using Microsoft.AspNetCore.Mvc;
using API.Ecommerce.DTOs.Cliente;
using API.Ecommerce.Services.Interfaces;

namespace API.Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authService;

        public AuthController(IAuthServices authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] ClienteCreateDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto.Email, dto.Password);
            return Ok(new { data = token });
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] ClienteUpdateDto dto)
        {
            var result = await _authService.ActualizaUsuario(dto);
            return Ok(new { message = result });
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authService.EliminaUsuario(id);
            if (!result)
                return NotFound(new { message = "Usuario no encontrado" });
            return Ok(new { message = "Usuario eliminado correctamente" });
        }

        [HttpGet("usuarios")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authService.ObtenerUsuarios();
            return Ok(users);
        }

        [HttpPost("registro-cliente")]
        public async Task<IActionResult> RegisterCliente([FromBody] ClienteCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Datos de cliente inválidos" });
            }
            if (dto.password != dto.Contraseña)
            {
                return BadRequest(new { message = "Las contraseñas no coinciden" });
            }
            var result = await _authService.RegisterClienteAsync(dto);
            return Ok(new { message = result });
        }

        // DTOs
        public record UserDto(int Id, string Nombre, string Email, int RolId, string Password);
        public record RegisterDto(string Nombre, string Email, string Password, int RolId);
        public record LoginDto(string Email, string Password);

    }
}
