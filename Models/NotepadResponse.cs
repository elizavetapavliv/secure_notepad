using System.Numerics;

namespace SecureNotepadServer.Models
{
    public class NotepadResponse
    {
        public byte[] File { get; set; }
        public BigInteger RSAEncodedKey { get; set; }
        public BigInteger[] GMEncodedKey { get; set; }
        public NotepadResponse(byte[] file, BigInteger rsaEncodedKey)
        {
            File = file;
            RSAEncodedKey = rsaEncodedKey;
        }
        public NotepadResponse(byte[] file, BigInteger[] gmEncodedKey)
        {
            File = file;
            GMEncodedKey = gmEncodedKey;
        }
    }
}