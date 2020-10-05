using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecureNotepadServer.Models;

namespace SecureNotepadServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotepadController : ControllerBase
    { 
        [HttpPost ("rsa")]
        public void SendRSAKey([FromBody] RSAPublicKey rsaKey)
        {
            HttpContext.Session.SetString("RSA", JsonConvert.SerializeObject(rsaKey));
        }

        [HttpGet ("encode")]
        public NotepadResponse GetEncodedFile(string fileName)
        {
            return new SecureNotepad(JsonConvert.DeserializeObject<RSAPublicKey>(HttpContext.Session.GetString("RSA")))
                .EncodeFile(fileName);
        }
    }
}