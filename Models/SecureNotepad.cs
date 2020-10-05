using Microsoft.AspNetCore.Http;
using SecureNotepadServer.Models;
using System;
using System.Collections;
using System.IO;
using System.Numerics;

namespace SecureNotepadServer.Services
{
    public class SecureNotepad : ISecureNotepad
    {
        public PublicKey PublicKey { get; set; }
        public bool IsGMAlgorithm { get; set; }

        private IHttpContextAccessor httpContextAccessor;
        public SecureNotepad(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            IsGMAlgorithm = false;
        }
        public NotepadResponse EncodeFile(string fileName)
        {
            var source = LoadDataFromFile(fileName);
            var sessionKey = GetSessionKey();
         
            var encodedFile = new CFB(sessionKey).Encrypt(source);

            NotepadResponse response;
            if (IsGMAlgorithm)
            {
                response = new NotepadResponse(encodedFile, EncodeWithGM(sessionKey));
            }
            else
            {
                response = new NotepadResponse(encodedFile, EncodeWithRSA(sessionKey));
            }
            return response;
        }

        private BigInteger EncodeWithRSA(byte[] sessionKey)
        {
            return BigInteger.ModPow(new BigInteger(sessionKey), PublicKey.X, PublicKey.N);
        }
        private BigInteger[] EncodeWithGM(byte[] sessionKey)
        {
            var bits = new BitArray(sessionKey);
            var N = PublicKey.N;
            var x = PublicKey.X;
            var length = sessionKey.Length;
            var result = new BigInteger[length];
            for (int i = 0; i < length; i++) 
            {
                var y = GenerateRandomBigInteger(N);
                result[i] = BigInteger.Multiply(BigInteger.Pow(y, 2), bits[i] ? x : 1);
            }
            return result;
        }

        private BigInteger GenerateRandomBigInteger(BigInteger N)
        {
            var length = N.ToByteArray().Length;
            BigInteger result;
            do
            {
                result = new BigInteger(Utils.GenerateByteArray(length)) % N;      
            } while (BigInteger.GreatestCommonDivisor(result, N) != 1);
            return result;
        }

        private byte[] GetSessionKey()
        {
            var sessionKey = httpContextAccessor.HttpContext.Session.Get("sessionKey");
            if (sessionKey == null)
            {
                sessionKey = Utils.GenerateByteArray(16);
                httpContextAccessor.HttpContext.Session.Set("sessionKey", sessionKey);
            }
            return sessionKey;
        }

        private byte[] LoadDataFromFile(string fileName)
        {
            byte[] source;
            try
            {
                source = File.ReadAllBytes($"Files\\{fileName}");
            }
            catch
            {
                throw new ArgumentException($"Can't find file \"{fileName}\".", fileName);
            }
            return source;
        }
    }
}