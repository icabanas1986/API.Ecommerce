namespace API.Ecommerce.DTOs.Direcciones
{
    public class CreaDireccionDTO
    {
        public int IdCliente { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string Colonia { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; }
        public bool Activo { get; set; }
    }
}
