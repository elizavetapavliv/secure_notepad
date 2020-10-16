using System.Linq;
using System.Numerics;

namespace SecureNotepadServer.Models
{
    public class NotepadResponse
    {
        public byte[] File { get; set; }
        public byte[] IV { get; set; }
        public string RSAEncodedKey { get; set; }
        public string[] GMEncodedKey { get; set; }
        private NotepadResponse(byte[] file, byte[] iv)
        {
            File = file;
            IV = iv;
        }
        public NotepadResponse(byte[] file, byte[] iv, BigInteger rsaEncodedKey) : this(file, iv)
        {
            RSAEncodedKey = rsaEncodedKey.ToString();
        }
        public NotepadResponse(byte[] file, byte[] iv, BigInteger[] gmEncodedKey) : this(file, iv)
        {
            GMEncodedKey = gmEncodedKey.Select(c => c.ToString()).ToArray();
        }
    }
}