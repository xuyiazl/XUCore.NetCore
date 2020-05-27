using System.Security.Cryptography;

namespace XUCore.NetCore.Jwt.Algorithms
{
    /// <summary>
    /// HMAC using SHA-384
    /// </summary>
    public sealed class HMACSHA384Algorithm : IJwtAlgorithm
    {
        /// <inheritdoc />
        public byte[] Sign(byte[] key, byte[] bytesToSign)
        {
            using (var sha = new HMACSHA384(key))
            {
                return sha.ComputeHash(bytesToSign);
            }
        }

        /// <inheritdoc />
        public string Name => JwtHashAlgorithm.HS384.ToString();

        /// <inheritdoc />
        public bool IsAsymmetric => false;
    }
}