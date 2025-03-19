using H4SikkerhedWebAPI.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace H4SikkerhedWebAPI.Controllers
{
    [Route("api/encrypt")]
    [ApiController]
    public class EncryptionController : ControllerBase
    {
        [HttpPost]
        public IActionResult EncryptText([FromBody] EncryptionRequest request)
        {
            try
            {
                string encryptedText = Encrypt(request.PlainText, request.PublicKey);
                return Ok(new { EncryptedText = encryptedText });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private string Encrypt(string plainText, string publicKey)
        {
            using (RSA rsa = RSA.Create())
            {
                rsa.ImportFromPem(publicKey);
                byte[] encryptedBytes = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }
}
