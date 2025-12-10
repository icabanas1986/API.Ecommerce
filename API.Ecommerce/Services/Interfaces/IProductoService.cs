using API.Ecommerce.DTOs.Producto;
using Microsoft.AspNetCore.Mvc;

namespace API.Ecommerce.Services.Interfaces
{
    public interface IProductoService
    {
        Task<List<ObtieneProductoDTO>> ObtenerTodosAsync([FromServices] IWebHostEnvironment env);
        Task<ObtieneProductoDTO> ObtenerPorIdAsync(int id, [FromServices] IWebHostEnvironment env);
        Task<int> CrearAsync(RegistrerProductoDTO productoDTO, [FromServices] IWebHostEnvironment env);
        Task ActualizarAsync(UpdateProductoDTO producto, [FromServices] IWebHostEnvironment env);
        Task EliminarAsync(int id);

        Task<List<ObtieneProductoDTO>> ObtenerPorCategoriaIdAsync(int categoriaId);
    }
}
