using API.Ecommerce.DTOs.Direcciones;
using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;

namespace API.Ecommerce.Services
{
    public class DireccionClienteService : Services.Interfaces.IDireccionClienteService
    {
        private readonly IDireccionClienteRepository _repo;

        public DireccionClienteService(IDireccionClienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ObtieneDireccionDTO>> ObtenerPorCliente(int clienteId)
        {
            List<ObtieneDireccionDTO> obtieneDireccionDTOs = new List<ObtieneDireccionDTO>();
            try
            {
                var clientes = await _repo.GetByClienteIdAsync(clienteId);
                foreach (var direccion in clientes)
                {
                    obtieneDireccionDTOs.Add(new ObtieneDireccionDTO
                    {
                        IdDireccion = direccion.Id,
                        Calle = direccion.Calle,
                        Colonia = direccion.Colonia,
                        Ciudad = direccion.Ciudad,
                        Estado = direccion.Estado,
                        CodigoPostal = direccion.CodigoPostal,
                        Pais = direccion.Pais,
                        EsPrincipal = direccion.EsPrincipal,
                        Activo = direccion.Activo
                    });
                }
                return obtieneDireccionDTOs.OrderByDescending(z => z.EsPrincipal).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<bool> CrearAsync(CreaDireccionDTO direccion)
        {
            bool actualizado = false;
            try
            {
                DireccionCliente direccionCliente = new DireccionCliente
                {
                    ClienteId = direccion.IdCliente,
                    Calle = direccion.Calle,
                    Colonia = direccion.Colonia,
                    Ciudad = direccion.Ciudad,
                    Estado = direccion.Estado,
                    CodigoPostal = direccion.CodigoPostal,
                    Pais = direccion.Pais,
                    EsPrincipal = direccion.EsPrincipal,
                    Activo = direccion.Activo
                };
                if (direccionCliente.EsPrincipal)
                {
                    var existentes = await _repo.GetByClienteIdAsync(direccion.IdCliente);
                    foreach (var dir in existentes.Where(d => d.EsPrincipal))
                    {
                        dir.EsPrincipal = false;
                    }
                    await _repo.UpdateRangeAsync(existentes);
                }
                actualizado = await _repo.AddAsync(direccionCliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la dirección del cliente", ex);
            }
            return actualizado;
        }


        public async Task<bool> ActualizarAsync(ActualizaDireccionDTO dto)
        {
            // 1️⃣ Obtener la dirección existente (TRACKED)
            var direccionCliente = await _repo.GetByIdAsync(dto.Id);

            if (direccionCliente == null)
                throw new Exception("La dirección no existe");

            // 2️⃣ Si será principal, desmarcar las demás
            if (dto.EsPrincipal)
            {
                var existentes = await _repo.GetByClienteIdAsync(dto.IdCliente);

                foreach (var dir in existentes)
                {
                    dir.EsPrincipal = false;
                }
            }

            // 3️⃣ Actualizar propiedades
            direccionCliente.Calle = dto.Calle;
            direccionCliente.Colonia = dto.Colonia;
            direccionCliente.Ciudad = dto.Ciudad;
            direccionCliente.Estado = dto.Estado;
            direccionCliente.CodigoPostal = dto.CodigoPostal;
            direccionCliente.Pais = dto.Pais;
            direccionCliente.EsPrincipal = dto.EsPrincipal;
            direccionCliente.Activo = dto.Activo;

            // 4️⃣ Guardar cambios UNA sola vez
            await _repo.SaveChangesAsync();

            return true;
        }



        public async Task EliminarAsync(int id)
            => await _repo.DeleteAsync(id);
    }
}
