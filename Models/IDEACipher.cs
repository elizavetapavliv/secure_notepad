namespace SecureNotepadServer.Models
{
    public class IDEACipher
    {
        private byte[] key;
        private const int ROUNDS = 8;
        private uint m = ushort.MaxValue + 1;
        public IDEACipher(byte[] key)
        {
            this.key = key;
        }
        private ushort[] GenerateSubkeys()
        {
            var subkeys = new ushort[ROUNDS * 6 + 4];
            int j = 0;
            for (int i = 0; i < key.Length; i += 2) 
            {
                subkeys[j++] = ConcatBytes (key[i], key[i + 1]);
            }
            for (int i = 9; i < subkeys.Length + 1; i++)
            {
                subkeys[j++] = (ushort)((subkeys[(i - 8) % 8 == 0 ? i - 16 : i - 8] << 9 |
                    subkeys[(i - 7) % 8 <= 1 ? i - 15 : i - 7] >> 7) & 0xFFFF);
            }
            return subkeys;
        }
        public byte[] Encrypt(byte[] source)
        {
            var subkeys = GenerateSubkeys();
            const int blocksNum = 4;

            var blocksOfBlocks = Utils.DivideIntoBlocks(source, blocksNum, 2);
            var blocks = new ushort[blocksNum];

            for (int i = 0; i < blocksNum; i++)
            {
                blocks[i] = ConcatBytes(blocksOfBlocks[i][0], blocksOfBlocks[i][1]);
            }
            int j = 0;

            for (int i = 0; i < ROUNDS; i++)
            {
                ushort a = ModMul(blocks[0], subkeys[j++]);
                ushort b = ModAdd(blocks[1], subkeys[j++]);
                ushort c = ModAdd(blocks[2], subkeys[j++]);
                ushort d = ModMul(blocks[3], subkeys[j++]);
                ushort e = (ushort)(a ^ c);
                ushort f = (ushort)(b ^ d);
                ushort k = ModMul(e, subkeys[j++]);
                ushort g = ModMul(ModAdd(f, k), subkeys[j++]);
                ushort h = ModAdd(k, g);
                blocks[0] = (ushort)(a ^ g);
                blocks[1] = (ushort)(c ^ g);
                blocks[2] = (ushort)(b ^ h);
                blocks[3] = (ushort)(d ^ h);
            }
            blocks[0] = ModMul(blocks[0], subkeys[j++]);
            blocks[1] = ModAdd(blocks[2], subkeys[j++]);
            blocks[2] = ModAdd(blocks[1], subkeys[j++]);
            blocks[3] = ModMul(blocks[3], subkeys[j]);

            j = 0;
            var result = new byte[source.Length];
            for (int i = 0; i < source.Length; i += 2) 
            {
                result[i] = (byte)(blocks[j] >> 8);
                result[i + 1] = (byte)blocks[j++];
            }
            return result;
        }
        private ushort ConcatBytes(byte a, byte b)
        {
            return (ushort)(a << 8 | b);
        }
        private ushort ModMul(uint a, uint b)
        {
            if(a == 0)
            {
                a = m;
            }
            if (b == 0)
            {
                b = m;
            }
            return (ushort)(a * b % (m + 1));
        }
        private ushort ModAdd(ushort a, ushort b)
        {
            return (ushort)((a + b) % m);
        }
    }
}