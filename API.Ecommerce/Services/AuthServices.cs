using API.Ecommerce.Data.Repository;
using API.Ecommerce.DTOs.Cliente;
using API.Ecommerce.DTOs.Rol;
using API.Ecommerce.Models;
using API.Ecommerce.Models.Auth;
using API.Ecommerce.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace API.Ecommerce.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IAuthRepository _repository;
        private readonly IConfiguration _config;
        private readonly IRolService _rolServices;
        private readonly IClienteService _clienteService;

        public AuthServices(IAuthRepository repository, IConfiguration config, IRolService rolServices, IClienteService clienteService)
        {
            _repository = repository;
            _config = config;
            _rolServices = rolServices;
            _clienteService = clienteService;
        }

        /// <summary>
        /// Registra un nuevo cliente y su usuario de autenticación.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> RegisterClienteAsync(ClienteCreateDto dto)
        {
            Rol rolCreado = new Rol();
            if (await _repository.ObtieneUsuarioPorCorreo(dto.Correo))
                throw new Exception("El correo ya está registrado.");

            if(dto.rolId==0)
            {
                //Buscamos el rol de cliente, si no existe lo creamos
                var rolCliente = await _rolServices.ObtenerTodosAsync();
                if (!rolCliente.Any(r => r.Nombre == "Cliente"))
                {
                    RegistrerRolDTO rolDTO = new RegistrerRolDTO
                    {
                        Nombre = "Cliente"
                    };
                    rolCreado = await _rolServices.CrearAsync(rolDTO);
                }
                else
                {
                    rolCreado = rolCliente.First(r => r.Nombre == "Cliente");
                }
            }
            var usuario = new UsuariosAuth
            {
                Email = dto.Correo,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.password),
                RolId = dto.rolId==0?rolCreado.Id:dto.rolId
            };

            var idAuth = await _repository.RegistraUsuario(usuario);
            if (idAuth <= 0)
                throw new Exception("Error al registrar el usuario.");

            //Registramos el cliente

            var clienteCreado = await _clienteService.CreateAsync(dto, idAuth);
            if (clienteCreado == null)
                throw new Exception("Error al registrar el cliente.");

            return "Cliente registrado correctamente.";
        }

        /// <summary>
        /// Registra un nuevo usuario con rol diferente de cliente administrador y su usuario de autenticación.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> RegisterAsync(ClienteCreateDto dto)
        {
            if (await _repository.ObtieneUsuarioPorCorreo(dto.Correo))
                throw new Exception("El correo ya está registrado.");

            var usuario = new UsuariosAuth
            {
                Email = dto.Correo,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Contraseña),
                RolId = dto.rolId
            };

            var idAuth = await _repository.RegistraUsuario(usuario);
            if (idAuth <= 0)
                throw new Exception("Error al registrar el usuario.");

            //Registramos el cliente

            var clienteCreado = await _clienteService.CreateAsync(dto, idAuth);
            if (clienteCreado == null)
                throw new Exception("Error al registrar el cliente.");
            return "Usuario registrado correctamente.";
        }

        public async Task<ObtieneClienteDTO> LoginAsync(string email, string password)
        {
            ObtieneClienteDTO clienteDTO = new ObtieneClienteDTO();
            var usuario = await _repository.GetByEmailAsync(email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash))
                throw new Exception("Credenciales inválidas.");

            clienteDTO.token = GenerarToken(usuario);
            var cliente = await _clienteService.GetByEmailAsync(email);
            clienteDTO.nombre = cliente.Nombre;
            clienteDTO.apellidoP = cliente.ApellidoPaterno;
            clienteDTO.apellidoM = cliente.ApellidoMaterno;
            clienteDTO.idPerfil = usuario.Rol.Id.ToString();
            clienteDTO.Perfil = usuario.Rol.Nombre;
            clienteDTO.UserId = cliente.Id;
            return clienteDTO;
        }

        public async Task<bool> EliminaUsuario(int id)
        {
            return await _repository.EliminaUsuario(id);
        }

        public async Task<bool> ActualizaUsuario(ClienteUpdateDto dto)
        {
            var clienteExiste = await _clienteService.GetByIdAsync(dto.Id);
            Cliente cliente = new Cliente()
            {
                Id = dto.Id,
                Nombre = dto.Nombre,
                ApellidoPaterno = dto.ApellidoPaterno,
                ApellidoMaterno = dto.ApellidoMaterno,
                Telefono = dto.Telefono,
                Activo = dto.Activo
            };

            UsuariosAuth usuarios = new UsuariosAuth()
            {
                Id = clienteExiste.IdAuth,
                Email = dto.Correo,
                RolId = dto.rolId,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.password)
            };
            return await _repository.ActualizaUsuario(usuarios,cliente);
        }

        public async Task<List<ClienteConRolDto>> ObtenerUsuarios()
        {
            return await _repository.ObtenerUsuarios();
        }
        private string GenerarToken(UsuariosAuth usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim("id", usuario.Id.ToString()),
                new Claim(ClaimTypes.Role, usuario.Rol?.Nombre ?? "SinRol")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
