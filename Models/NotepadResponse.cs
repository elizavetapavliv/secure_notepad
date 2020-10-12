using System.Numerics;

namespace SecureNotepadServer.Models
{
    public class NotepadResponse
    {
        public byte[] File { get; set; }
        public byte[] IV { get; set; }
        public BigInteger RSAEncodedKey { get; set; }
        public BigInteger[] GMEncodedKey { get; set; }
        private NotepadResponse(byte[] file, byte[] iv)
        {
            File = file;
            IV = iv;
        }
        public NotepadResponse(byte[] file, byte[] iv, BigInteger rsaEncodedKey) : this(file, iv)
        {
            RSAEncodedKey = rsaEncodedKey;
        }
        public NotepadResponse(byte[] file, byte[] iv, BigInteger[] gmEncodedKey) : this(file, iv)
        {
            GMEncodedKey = gmEncodedKey;
        }
    }
}