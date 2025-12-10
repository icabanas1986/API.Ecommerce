using System.Security.Cryptography;

namespace API.Ecommerce.Utils
{
    public class Encrypt
    {
        public async Task<string> Encrypta(string base64Image)
        {
            byte[] bytesImagen = Convert.FromBase64String(base64Image);
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var msEncrypt = new System.IO.MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(bytesImagen, 0, bytesImagen.Length);
                    }
                    byte[] bytesEncriptados = msEncrypt.ToArray();

                    // 3. Codificar los bytes encriptados a una nueva cadena Base64
                    string base64EncryptedImage = Convert.ToBase64String(bytesEncriptados);

                    Console.WriteLine("Cadena Base64 encriptada: " + base64EncryptedImage);
                    return base64EncryptedImage;
                }
            }
        }
    }
}
