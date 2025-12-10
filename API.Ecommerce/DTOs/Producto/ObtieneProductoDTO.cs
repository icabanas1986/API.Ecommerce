using API.Ecommerce.DTOs.ImagenesProducto;

namespace API.Ecommerce.DTOs.Producto
{
    public class ObtieneProductoDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }
        public bool Activo { get; set; }
        public List<ObtenImagenesProductoDTO> Imagenes { get; set; }
    }
}
