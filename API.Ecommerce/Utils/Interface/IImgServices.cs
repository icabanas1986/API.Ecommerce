namespace API.Ecommerce.Utils.Interface
{
    public interface IImgServices
    {
        Task<List<string>> GuardaImagenes(List<string> imagenesBase64, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor);

        Task<List<string>> ObtenerImagenesBase64(List<string> rutas, IWebHostEnvironment env);

        Task<List<string>> Base64ToImg(List<string> base64String, IWebHostEnvironment env);
        Task<List<string>> ImgToBase64(List<string> ruta);
    }
}
