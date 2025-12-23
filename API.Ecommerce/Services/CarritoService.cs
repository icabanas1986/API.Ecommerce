using API.Ecommerce.DTOs.Carrito;
using API.Ecommerce.Models;
using API.Ecommerce.Repositories.Interfaces;
using API.Ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using TPVY.API.Ecommerce.Data;

namespace API.Ecommerce.Services
{
    public class CarritoService : ICarritoService
    {
        private readonly ICarritoRepository _repo;
        private readonly IProductoImagenesRepository _imgRepo;
        private readonly IProductoRepository _productoRepo;
        private readonly ApplicationDbContext _context;

        public CarritoService(ICarritoRepository repo, ApplicationDbContext context,IProductoImagenesRepository imgRepo,IProductoRepository productoRepo)
        {
            _repo = repo;
            _context = context;
            _imgRepo = imgRepo;
            _productoRepo = productoRepo;
        }

        public async Task<Carrito> ObtenerOCrearCarrito(int clienteId)
        {
            var carrito = await _repo.ObtenerCarritoActivo(clienteId);

            if (carrito != null)
                return carrito;

            carrito = new Carrito
            {
                ClienteId = clienteId
            };

            await _repo.CrearAsync(carrito);
            await _repo.SaveChangesAsync();

            return carrito;
        }

        public async Task<CarritoResponseDTO> ObtenerCarrito(int clienteId)
        {
            List<string> imgs = new List<string>();
            var carrito = await ObtenerOCrearCarrito(clienteId);
            var carroDto =  MapearCarrito(carrito);
            foreach(var item in carroDto.Items)
            {
                var imagen = await _imgRepo.ObtenerImagenesPorProductoIdAsync(item.ProductoId);
                if (imagen != null)
                {
                    imgs = imagen.Select(i => i.Url).ToList();
                    carroDto.Items.FirstOrDefault(i => i.ProductoId == item.ProductoId).Imagenes = imgs;
                }
            }
            return carroDto;
        }

        public async Task<CarritoResponseDTO> AgregarProducto(AgregarProductoCarritoDTO dto)
        {
            var carrito = await ObtenerOCrearCarrito(dto.ClienteId);

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == dto.ProductoId && p.Activo);

            if (producto == null)
                throw new Exception("Producto no encontrado");

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == dto.ProductoId);

            if (item != null)
            {
                item.Cantidad += dto.Cantidad;
            }
            else
            {
                carrito.Items.Add(new CarritoItem
                {
                    ProductoId = producto.Id,
                    Cantidad = dto.Cantidad,
                    PrecioUnitario = producto.Precio
                });
            }

            await _repo.SaveChangesAsync();

            return MapearCarrito(carrito);
        }

        private CarritoResponseDTO MapearCarrito(Carrito carrito)
        {
            return new CarritoResponseDTO
            {
                CarritoId = carrito.Id,
                ClienteId = carrito.ClienteId,
                Items = carrito.Items.Select(i => new CarritoItemResponseDTO
                {
                    ProductoId = i.ProductoId,
                    NombreProducto = i.Producto.Nombre,
                    Cantidad = i.Cantidad,
                    PrecioUnitario = i.PrecioUnitario,
                    Subtotal = i.Cantidad * i.PrecioUnitario
                }).ToList(),
                Total = carrito.Items.Sum(i => i.Cantidad * i.PrecioUnitario)
            };
        }

        public async Task EliminarProducto(int clienteId, int productoId)
        {
            var carrito = await ObtenerOCrearCarrito(clienteId);

            var item = carrito.Items.FirstOrDefault(i => i.ProductoId == productoId);

            if (item != null)
            {
                carrito.Items.Remove(item);
                await _repo.SaveChangesAsync();
            }
        }

        public async Task VaciarCarrito(int clienteId)
        {
            var carrito = await ObtenerOCrearCarrito(clienteId);

            carrito.Items.Clear();
            await _repo.SaveChangesAsync();
        }
    }
}
