using API.Ecommerce.DTOs.Categorias;
using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using API.Ecommerce.Services.Interfaces;

namespace API.Ecommerce.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ObtieneCategoriasDTO>> ObtenerCategorias()
        {
            var categorias = await _repository.GetAllAsync();
            return categorias.Select(c => new ObtieneCategoriasDTO
            {
                IdCategoria = c.Id,
                Nombre = c.Nombre,
                Descripcion = String.IsNullOrEmpty(c.Descripcion) ? "Sin descripción" : c.Descripcion
            });
        }
        public async Task<ObtieneCategoriasDTO?> ObtenerPorId(int id)
        {
            var categoria = await _repository.GetByIdAsync(id);
            if (categoria == null)
            {
                return null;
            }
            return new ObtieneCategoriasDTO
            {
                IdCategoria = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = String.IsNullOrEmpty(categoria.Descripcion) ? "Sin descripción" : categoria.Descripcion
            };
        }

        public async Task CrearCategoria(CreaCategoriaDTO categoriaDTO)
        {
            var categoria = new Categoria
            {
                Nombre = categoriaDTO.Nombre,
                Descripcion = categoriaDTO.Descripcion
            };
            await _repository.AddAsync(categoria);
            await _repository.SaveChangesAsync();
        }

        public async Task ActualizarCategoria(ActualizaCategoriaDTO categoriaDTO)
        {
            Categoria categoria = new Categoria()
            {
                Id = categoriaDTO.IdCategoria,
                Descripcion = categoriaDTO.Descripcion,
                Nombre = categoriaDTO.Nombre
            };

            await _repository.UpdateAsync(categoria);
            await _repository.SaveChangesAsync();
        }

        public async Task EliminarCategoria(int id)
        {
            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();
        }
    }
}
