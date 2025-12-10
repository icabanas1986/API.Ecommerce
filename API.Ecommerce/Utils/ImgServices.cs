using API.Ecommerce.Utils.Interface;
using System.Security.Cryptography;
using System.Text;

namespace API.Ecommerce.Utils
{
    public class ImgServices : IImgServices
    {
        private readonly ILogger<ImgServices> _logger;
        public ImgServices(ILogger<ImgServices> logger)
        {
            _logger = logger;
        }
        public async Task<List<string>> GuardaImagenes(List<string> imagenesBase64, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            List<string> urlsPublicas = new List<string>();

            try
            {
                string carpeta = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "imagenes_productos");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                foreach (var base64 in imagenesBase64)
                {
                    try
                    {
                        string base64Clean = base64.Contains(",")
                            ? base64.Split(',')[1]
                            : base64;

                        byte[] bytes = Convert.FromBase64String(base64Clean);

                        string nombreArchivo = $"{Guid.NewGuid()}.jpg";
                        string rutaArchivo = Path.Combine(carpeta, nombreArchivo);

                        await File.WriteAllBytesAsync(rutaArchivo, bytes);

                        // Construye la URL pública accesible desde el front
                        var request = httpContextAccessor.HttpContext.Request;
                        string baseUrl = $"{request.Scheme}://{request.Host}";
                        string urlPublica = $"/imagenes_productos/{nombreArchivo}";

                        urlsPublicas.Add(urlPublica);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error procesando una imagen base64.");
                        throw;
                    }
                }

                return urlsPublicas;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general guardando imágenes base64");
                throw;
            }
        }

        public async Task<List<string>> ObtenerImagenesBase64(List<string> rutas, IWebHostEnvironment env)
        {
            List<string> imagenesBase64 = new List<string>();

            try
            {
                foreach (var ruta in rutas)
                {
                    try
                    {
                        // Normalizar ruta
                        string rutaFisica = ruta;

                        // Si es ruta relativa tipo "/imagenes_productos/xx.jpg"
                        if (!Path.IsPathRooted(ruta))
                        {
                            rutaFisica = Path.Combine(env.ContentRootPath, ruta.TrimStart('/'));
                        }

                        if (!File.Exists(rutaFisica))
                        {
                            _logger.LogWarning("La imagen no existe en la ruta: {Ruta}", rutaFisica);
                            continue; // no lanzamos excepción, solo lo omitimos
                        }

                        byte[] bytes = await File.ReadAllBytesAsync(rutaFisica);

                        // Detectar MIME
                        string extension = Path.GetExtension(rutaFisica).ToLower();
                        string mime = extension switch
                        {
                            ".jpg" or ".jpeg" => "image/jpeg",
                            ".png" => "image/png",
                            ".gif" => "image/gif",
                            ".webp" => "image/webp",
                            ".bmp" => "image/bmp",
                            ".heic" => "image/heic",
                            _ => "application/octet-stream"
                        };

                        // Convertir a Base64 usable en UI
                        string base64 = Convert.ToBase64String(bytes);
                        string base64Final = $"data:{mime};base64,{base64}";

                        imagenesBase64.Add(base64Final);
                    }
                    catch (Exception exImg)
                    {
                        _logger.LogError(exImg, "Error procesando una imagen individual al convertirla a Base64.");
                        throw;
                    }
                }

                return imagenesBase64;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general al obtener las imágenes en Base64.");
                throw;
            }
        }

        public async Task<List<string>> Base64ToImg(List<string> base64String, IWebHostEnvironment env)
        {
            List<string> rutas = new List<string>();
            string archivo = string.Empty;
            try
            {
                foreach (var b64string in base64String)
                {
                    // Separar base64 si tiene encabezado
                    var partes = b64string.Split(',');
                    string base64 = partes.Length > 1 ? partes[1] : partes[0];

                    archivo = archivo + GenerarCodigoUnico() + ".jpg";
                    // Ruta física del directorio y archivo
                    var directorio = Path.Combine(env.WebRootPath ?? env.ContentRootPath, "Imagenes");
                    var rutaArchivo = Path.Combine(directorio, archivo);

                    // Crear directorio si no existe
                    if (!Directory.Exists(directorio))
                    {
                        Directory.CreateDirectory(directorio);
                    }

                    // Guardar imagen
                    byte[] bytesImagen = Convert.FromBase64String(base64);
                    File.WriteAllBytes(rutaArchivo, bytesImagen);
                    rutas.Add(rutaArchivo);
                }
                return rutas;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<string>> ImgToBase64(List<string> rutas)
        {
            List<string> imgsBase64 = new List<string>();
            try
            {
                foreach (var ruta in rutas)
                {
                    byte[] bytes = File.ReadAllBytes(ruta);
                    string base64 = Convert.ToBase64String(bytes);

                    // Detectar MIME por extensión
                    string extension = Path.GetExtension(ruta).ToLower().TrimStart('.');
                    string mime = "application/octet-stream";

                    if (extension == "jpg" || extension == "jpeg")
                        mime = "image/jpeg";
                    else if (extension == "png")
                        mime = "image/png";
                    else if (extension == "gif")
                        mime = "image/gif";
                    else if (extension == "bmp")
                        mime = "image/bmp";
                    imgsBase64.Add($"data:{mime};base64,{base64}");
                }
            }
            catch (Exception ex)
            {

                return null;
            }
            return imgsBase64;
        }

        public static string GenerarCodigoUnico(int longitud = 12)
        {
            const string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var bytesAleatorios = new byte[longitud];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytesAleatorios);
            }

            var sb = new StringBuilder();

            for (int i = 0; i < longitud; i++)
            {
                // Combina con tiempo actual para hacer menos repetible
                int index = (bytesAleatorios[i] + DateTime.UtcNow.Ticks.ToString()[i % DateTime.UtcNow.Ticks.ToString().Length]) % caracteres.Length;
                sb.Append(caracteres[index]);
            }

            return sb.ToString();
        }
    }
}
