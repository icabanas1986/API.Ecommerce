using API.Ecommerce.DTOs.Cliente;
using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using API.Ecommerce.Services.Interfaces;

namespace API.Ecommerce.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(IClienteRepository repo, ILogger<ClienteService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        private ClienteDto MapToDto(Cliente c) => new ClienteDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Apellidos = c.ApellidoPaterno,
            Telefono = c.Telefono,
            Activo = c.Activo,
            FechaRegistro = c.FechaRegistro
        };

        public async Task<IEnumerable<ClienteDto>> GetAllAsync()
        {
            var datos = await _repo.GetAllAsync();
            return datos.Select(MapToDto);
        }

        public async Task<(IEnumerable<ClienteDto> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search)
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, search);
            return (items.Select(MapToDto), total);
        }

        public async Task<ClienteDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            return c == null ? null : MapToDto(c);
        }

        public async Task<ClienteDto> CreateAsync(ClienteCreateDto dto, int idAuth)
        {
            var cliente = new Cliente
            {
                Nombre = dto.Nombre,
                ApellidoPaterno = dto.ApellidoPaterno,
                ApellidoMaterno = dto.ApellidoMaterno,
                Telefono = dto.Telefono,
                Activo = true,
                FechaRegistro = DateTime.UtcNow,
                IdAuth = idAuth
            };

            var creado = await _repo.AddAsync(cliente);
            return MapToDto(creado);
        }

        public async Task<ClienteDto?> UpdateAsync(ClienteUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null) return null;

            // validar email duplicado (otro cliente)
            var byEmail = await _repo.GetByEmailAsync(dto.Correo);
            if (byEmail != null && byEmail.Id != dto.Id)
                throw new InvalidOperationException($"El correo {dto.Correo} ya está en uso por otro cliente.");

            existing.Nombre = dto.Nombre;
            existing.ApellidoPaterno = dto.Apellidos;
            existing.Telefono = dto.Telefono;
            existing.Activo = dto.Activo;

            var actualizado = await _repo.UpdateAsync(existing);
            return MapToDto(actualizado);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
