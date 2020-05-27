using XUCore.NetCore.Jwt.Algorithms;
using XUCore.NetCore.Jwt.Serializers;
using XUCore.Extensions;
using System;
using System.Text;

namespace XUCore.NetCore.Jwt
{
    internal static class JwtVerifyExtensions
    {
        /// <summary>
        /// 验证token完整性和时效性
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        internal static bool VerifyToken(this string token, string secret, out Exception ex)
        {
            var urlEncoder = new JwtBase64UrlEncoder();
            var jsonNetSerializer = new JsonNetSerializer();
            var utcDateTimeProvider = new UtcDateTimeProvider();

            var jwt = new JwtParts(token);

            var payloadJson = urlEncoder.Decode(jwt.Payload).ToString(Encoding.UTF8);

            var crypto = urlEncoder.Decode(jwt.Signature);
            var decodedCrypto = crypto.ToBase64String();

            var alg = new HMACSHA256Algorithm();
            var bytesToSign = String.Concat(jwt.Header, ".", jwt.Payload).ToBytes(Encoding.UTF8);
            var signatureData = alg.Sign(secret.ToBytes(Encoding.UTF8), bytesToSign);
            var decodedSignature = signatureData.ToBase64String();

            var jwtValidator = new JwtValidator(jsonNetSerializer, utcDateTimeProvider);

            return jwtValidator.TryValidate(payloadJson, decodedCrypto, decodedSignature, out ex);
        }
    }
}