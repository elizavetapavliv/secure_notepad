using System;
using System.IO;
using System.Numerics;

namespace SecureNotepadServer.Models
{
    public class SecureNotepad
    {
        private RSAPublicKey rsaKey;

        public SecureNotepad(RSAPublicKey rsaKey)
        {
            this.rsaKey = rsaKey;
        }
        public NotepadResponse EncodeFile(string fileName)
        {
            byte[] source;
            try
            {
                source = File.ReadAllBytes($"Files\\{fileName}");
            }
            catch
            { 
                throw new ArgumentException( $"Can't find file \"{fileName}\".", fileName);
            }

            var sessionKey = Utils.GenerateByteArray(16);      
            var encodedKey = BigInteger.ModPow(new BigInteger(sessionKey), rsaKey.Exp, rsaKey.Mod);

            CFB cfb = new CFB(sessionKey);
            var encodedFile = cfb.Encrypt(source);

            return new NotepadResponse(encodedFile, encodedKey);
        }
    }
}