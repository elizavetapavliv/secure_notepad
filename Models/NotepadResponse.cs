using System.Numerics;

namespace SecureNotepadServer.Models
{
    public class NotepadResponse
    {
        public byte[] File { get; set; }
        public BigInteger SessionKey { get; set; }
        public NotepadResponse(byte[] file, BigInteger sessionKey)
        {
            File = file;
            SessionKey = sessionKey;
        }
    }
}