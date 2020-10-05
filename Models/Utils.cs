using System;

namespace SecureNotepadServer.Models
{
    public static class Utils
    {
        public static byte[][] DivideIntoBlocks(byte[] source, int blocksNum, int blockSize)
        {
            var blocks = new byte[blocksNum][];
            int j = 0;

            for (int i = 0; i < source.Length; i += blockSize)
            {
                blocks[j] = new byte[blockSize];
                Array.Copy(source, i, blocks[j++], 0, blockSize);
            }
            return blocks;
        }
        public static byte[] GenerateByteArray(int size)
        {
            var bytes = new byte[size];
            new Random().NextBytes(bytes);
            return bytes;
        }
    }
}
