using API.Ecommerce.DTOs.Categorias;

namespace API.Ecommerce.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<IEnumerable<ObtieneCategoriasDTO>> ObtenerCategorias();
        Task<ObtieneCategoriasDTO?> ObtenerPorId(int id);
        Task CrearCategoria(CreaCategoriaDTO categoria);
        Task ActualizarCategoria(ActualizaCategoriaDTO categoria);
        Task EliminarCategoria(int id);
    }
}
