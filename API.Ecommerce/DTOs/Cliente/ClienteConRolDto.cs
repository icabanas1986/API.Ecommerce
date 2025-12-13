namespace API.Ecommerce.DTOs.Cliente
{
    public class ClienteConRolDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Email { get; set; }
        public int RolId { get; set; }
        public string RolNombre { get; set; }
    }
}
