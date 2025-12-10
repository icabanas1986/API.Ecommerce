using API.Ecommerce.DTOs.ImagenesProducto;
using API.Ecommerce.DTOs.Producto;
using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using API.Ecommerce.Services.Interfaces;
using API.Ecommerce.Utils;
using API.Ecommerce.Utils.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Ecommerce.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IProductoImagenesRepository _productoImagenesRepository;
        private readonly ILogger<ProductoService> _logger;
        private readonly IImgServices _imgServices;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly Encrypt _encyrpt;
        public ProductoService(IProductoRepository productoRepository, IProductoImagenesRepository productoImagenesRepository,
            ILogger<ProductoService> logger, IImgServices imgServices, IHttpContextAccessor httpContext)
        {
            _productoRepository = productoRepository;
            _productoImagenesRepository = productoImagenesRepository;
            _logger = logger;
            _imgServices = imgServices;
            _HttpContextAccessor = httpContext;
            _encyrpt = new Encrypt();
        }

        public async Task<List<ObtieneProductoDTO>> ObtenerTodosAsync([FromServices] IWebHostEnvironment env)
        {
            List<string> rutas = new List<string>();
            List<ObtenImagenesProductoDTO> imagenes = new List<ObtenImagenesProductoDTO>();

            var productos = await _productoRepository.ObtenerTodosAsync();
            List<ObtieneProductoDTO> productosDTO = new List<ObtieneProductoDTO>();
            foreach (var product in productos)
            {
                //obtener las imagenes del producto
                var imagenesProducto = await _productoImagenesRepository.ObtenerImagenesPorProductoIdAsync(product.Id);
                //foreach (var s in imagenesProducto)
                //{
                //    rutas.Add(s.Url);
                //}
                //var imagenesB64 = await _imgServices.ImgToBase64(rutas);
                foreach (var img in imagenesProducto)
                {
                    imagenes.Add(new ObtenImagenesProductoDTO
                    {
                        Url = img.Url
                    });
                }
                productosDTO.Add(new ObtieneProductoDTO()
                {
                    IdProducto = product.Id,
                    Nombre = product.Nombre,
                    Descripcion = product.Descripcion,
                    Precio = product.Precio,
                    Stock = product.Stock,
                    IdCategoria = product.CategoriaId,
                    Activo = product.Activo,
                    Imagenes = imagenes
                });
                rutas = new List<string>();
                imagenes = new List<ObtenImagenesProductoDTO>();
            }
            return productosDTO;
        }


        public async Task<ObtieneProductoDTO> ObtenerPorIdAsync(int id, [FromServices] IWebHostEnvironment env)
        {
            List<string> rutas = new List<string>();
            List<ObtenImagenesProductoDTO> imagenes = new List<ObtenImagenesProductoDTO>();

            var productos = await _productoRepository.ObtenerPorIdAsync(id);
            if (productos == null)
            {
                return null;
            }
            //obtener las imagenes del producto
            var imagenesProducto = await _productoImagenesRepository.ObtenerImagenesPorProductoIdAsync(productos.Id);
            foreach (var img in imagenesProducto)
            {
                imagenes.Add(new ObtenImagenesProductoDTO
                {
                    Url = img.Url
                });
            }
            ObtieneProductoDTO productoDTO = new ObtieneProductoDTO()
            {
                IdProducto = productos.Id,
                Nombre = productos.Nombre,
                Descripcion = productos.Descripcion,
                Precio = productos.Precio,
                Stock = productos.Stock,
                IdCategoria = productos.CategoriaId,
                Imagenes = imagenes
            };
            return productoDTO;
        }

        public async Task<int> CrearAsync(RegistrerProductoDTO productoDTO, [FromServices] IWebHostEnvironment env)
        {

            List<ProductoImagen> productoImagen = new List<ProductoImagen>();
            Producto producto = new Producto()
            {
                Activo = productoDTO.Activo,
                CategoriaId = productoDTO.CategoriaID,
                Descripcion = productoDTO.Descripcion,
                Nombre = productoDTO.Nombre,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock,
            };

            var product = await _productoRepository.CrearAsync(producto);
            if (product == null)
            {
                return 0;
            }

            foreach (var url in productoDTO.Imagenes)
            {
                productoImagen.Add(new ProductoImagen()
                {
                    ProductoId = product.Id,
                    Url = url
                });
            }
            await _productoImagenesRepository.GuardarImagenesAsync(productoImagen);

            return producto.Id;
        }

        public async Task ActualizarAsync(UpdateProductoDTO productoDTO, [FromServices] IWebHostEnvironment env)
        {
            List<ProductoImagen> productoImagen = new List<ProductoImagen>();
            //Primero eliminamos las imagenes para despues insertar las nuevas si es que cambiaron
            var eliminaImagenes = await _productoImagenesRepository.EliminaImagenesPorIdProductoAsync(productoDTO.Id);
            if (!eliminaImagenes)
            {
                _logger.LogError("No se pudieron eliminar las imagenes del producto con Id: {ProductoId}", productoDTO.Id);
                throw new Exception("No se puede actualizar las imagenes");
            }
            Producto producto = new Producto()
            {
                Activo = productoDTO.Activo,
                CategoriaId = productoDTO.CategoriaID,
                Descripcion = productoDTO.Descripcion,
                Nombre = productoDTO.Nombre,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock,
                Id = productoDTO.Id
            };

            await _productoRepository.ActualizarAsync(producto);

            foreach (var url in productoDTO.Imagenes)
            {
                productoImagen.Add(new ProductoImagen()
                {
                    ProductoId = productoDTO.Id,
                    Url = url
                });
            }
            await _productoImagenesRepository.GuardarImagenesAsync(productoImagen);
        }

        public async Task EliminarAsync(int id)
        {
            //Se eliminan los producto imagenes asociados
            var borrarImagenes = await _productoImagenesRepository.EliminaImagenesPorIdProductoAsync(id);
            if (!borrarImagenes)
            {
                throw new Exception("No se pudieron eliminar las imagenes asociadas al producto.");
            }
            //Se elimina el producto
            await _productoRepository.EliminarAsync(id);
        }

        public async Task<List<ObtieneProductoDTO>> ObtenerPorCategoriaIdAsync(int categoriaId)
        {
            List<string> rutas = new List<string>();
            List<ObtenImagenesProductoDTO> imagenes = new List<ObtenImagenesProductoDTO>();
            var productos = await _productoRepository.ObtenerPorCategoriaIdAsync(categoriaId);
            List<ObtieneProductoDTO> productosDTO = new List<ObtieneProductoDTO>();
            foreach (var product in productos)
            {
                //obtener las imagenes del producto
                var imagenesProducto = await _productoImagenesRepository.ObtenerImagenesPorProductoIdAsync(product.Id);
                foreach (var img in imagenesProducto)
                {
                    imagenes.Add(new ObtenImagenesProductoDTO
                    {
                        Url = img.Url
                    });
                }
                productosDTO.Add(new ObtieneProductoDTO()
                {
                    IdProducto = product.Id,
                    Nombre = product.Nombre,
                    Descripcion = product.Descripcion,
                    Precio = product.Precio,
                    Stock = product.Stock,
                    IdCategoria = product.CategoriaId,
                    Activo = product.Activo,
                    Imagenes = imagenes
                });
                rutas = new List<string>();
                imagenes = new List<ObtenImagenesProductoDTO>();
            }
            return productosDTO;
        }
    }
}
