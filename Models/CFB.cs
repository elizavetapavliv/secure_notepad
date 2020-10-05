using System.Linq;

namespace SecureNotepadServer.Models
{
    public class CFB
    {
        private IDEACipher idea;
        private const int BLOCK_SIZE = 8;
        public CFB(byte[] key)
        {
            idea = new IDEACipher(key);
        }
        public byte[] Encrypt(byte[] source)
        {
            var blocks = getBlocks(source); 
            var iv = Utils.GenerateByteArray(BLOCK_SIZE);

            var encrypted = Xor(idea.Encrypt(iv), blocks[0]);
            for (int i = 1; i < blocks.Length; i++)
            {
                var current = Xor(idea.Encrypt(encrypted), blocks[i]);
                encrypted = encrypted.Concat(current).ToArray();
            }
            return encrypted.Take(source.Length).ToArray();
        }

        private byte[][] getBlocks(byte[] source)
        {
            int rem = source.Length % BLOCK_SIZE;
            int blocksNum = source.Length / BLOCK_SIZE;
            if (rem != 0)
            {
                blocksNum++;
                source = source.Concat(new byte[BLOCK_SIZE - rem]).ToArray();
            }

            return Utils.DivideIntoBlocks(source, blocksNum, BLOCK_SIZE);
        }

        private byte[] Xor(byte[] a, byte[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] ^= b[i];
            }
            return a;
        }
    }
}
