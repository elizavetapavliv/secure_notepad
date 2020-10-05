using System.Numerics;

namespace SecureNotepadServer.Models
{
    public class PublicKey
    {
        public BigInteger N { get; set; }
        public BigInteger X { get; set; }
    }
}