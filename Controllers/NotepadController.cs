using Microsoft.AspNetCore.Mvc;
using SecureNotepadServer.Models;
using SecureNotepadServer.Services;

namespace SecureNotepadServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotepadController : ControllerBase
    {
        private ISecureNotepad secureNotepad;
        public NotepadController(ISecureNotepad secureNotepad)
        {
            this.secureNotepad = secureNotepad;
        }
     
        [HttpPost("GMAlgorithm/{isGM}")]
        public void PostAlgorithm(bool isGM)
        {
            secureNotepad.IsGMAlgorithm = isGM;
        }

        [HttpPost ("publicKey")]
        public void PostPublicKey(PublicKey publicKey)
        {
            secureNotepad.PublicKey = publicKey;
        }

        [HttpGet ("encode/{fileName}")]
        public NotepadResponse GetEncodedFile(string fileName)
        {
            return secureNotepad.EncodeFile(fileName);
        }
    }
}