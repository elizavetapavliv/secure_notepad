using System.Numerics;

namespace SecureNotepadServer.Models
{
    public class RSAPublicKey
    {
        public BigInteger Mod { get; set; }
        public BigInteger Exp { get; set; }
    }
}