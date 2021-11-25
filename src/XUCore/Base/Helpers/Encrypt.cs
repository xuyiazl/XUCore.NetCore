using XUCore.Extensions;
using XUCore.Helpers.Internal;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using XUCore.Serializer;

namespace XUCore.Helpers
{
    /// <summary>
    /// 加密操作
    /// 说明：
    /// 1、AES加密整理自支付宝SDK
    /// 2、RSA加密采用 https://github.com/stulzq/DotnetCore.RSA/blob/master/DotnetCore.RSA/RSAHelper.cs
    /// </summary>
    public static class Encrypt
    {
        #region Md5加密

        /// <summary>
        /// Md5加密，返回16位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Md5By16(string value)
        {
            return Md5By16(value, Encoding.UTF8);
        }

        /// <summary>
        /// Md5加密，返回16位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Md5By16(string value, Encoding encoding)
        {
            return Md5(value, encoding, 4, 8);
        }

        /// <summary>
        /// Md5加密，返回32位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Md5By32(string value)
        {
            return Md5By32(value, Encoding.UTF8);
        }

        /// <summary>
        /// Md5加密，返回32位结果
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Md5By32(string value, Encoding encoding)
        {
            return Md5(value, encoding, null, null);
        }

        /// <summary>
        /// Md5加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private static string Md5(string value, Encoding encoding, int? startIndex, int? length)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            var md5 = MD5.Create();
            string result;
            try
            {
                var hash = md5.ComputeHash(encoding.GetBytes(value));
                result = startIndex == null
                    ? BitConverter.ToString(hash)
                    : BitConverter.ToString(hash, startIndex.SafeValue(), length.SafeValue());
            }
            finally
            {
                md5?.Clear();
                md5?.Dispose();
            }
            return result.Replace("-", "");
        }

        #endregion Md5加密

        #region DES加密

        /// <summary>
        /// DES密钥，24位字符串
        /// </summary>
        private static string DesKey = "#s^un2ye21fcv%|f0XpR,+vh";

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <returns></returns>
        public static string DesEncrypt(object value)
        {
            return DesEncrypt(value, DesKey);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥，24位</param>
        /// <returns></returns>
        public static string DesEncrypt(object value, string key)
        {
            return DesEncrypt(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥，24位</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string DesEncrypt(object value, string key, Encoding encoding)
        {
            var text = value.SafeString();
            if (ValidateDes(text, key) == false)
            {
                return string.Empty;
            }
            using var transform = CreateDesProvider(key).CreateEncryptor();
            return GetEncryptResult(text, encoding, transform);
        }

        /// <summary>
        /// 验证Des加密参数
        /// </summary>
        /// <param name="text">待加密的文本</param>
        /// <param name="key">密钥，24位</param>
        /// <returns></returns>
        private static bool ValidateDes(string text, string key)
        {
            if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(key))
            {
                return false;
            }
            return key.Length == 24;
        }

        /// <summary>
        /// 创建Des加密服务提供程序
        /// </summary>
        /// <param name="key">密钥，24位</param>
        /// <returns></returns>
        private static TripleDES CreateDesProvider(string key)
        {
            var des = TripleDES.Create();
            des.Key = Encoding.ASCII.GetBytes(key);
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.PKCS7;
            return des;
        }

        /// <summary>
        /// 获取加密结果
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="transform">加密器</param>
        /// <returns></returns>
        private static string GetEncryptResult(string value, Encoding encoding, ICryptoTransform transform)
        {
            var bytes = encoding.GetBytes(value);
            var result = transform.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <returns></returns>
        public static string DesDecrypt(object value)
        {
            return DesDecrypt(value, DesKey);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥，24位</param>
        /// <returns></returns>
        public static string DesDecrypt(object value, string key)
        {
            return DesDecrypt(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥，24位</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string DesDecrypt(object value, string key, Encoding encoding)
        {
            var text = value.SafeString();
            if (!ValidateDes(text, key))
            {
                return string.Empty;
            }

            using var transform = CreateDesProvider(key).CreateDecryptor();
            return GetDecryptResult(text, encoding, transform);
        }

        /// <summary>
        /// 获取解密结果
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="transform">加密器</param>
        /// <returns></returns>
        private static string GetDecryptResult(string value, Encoding encoding, ICryptoTransform transform)
        {
            var bytes = Convert.FromBase64String(value);
            var result = transform.TransformFinalBlock(bytes, 0, bytes.Length);
            return encoding.GetString(result);
        }

        #endregion DES加密

        #region AES加密

        /// <summary>
        /// 128位0向量
        /// </summary>
        private static byte[] _iv;

        /// <summary>
        /// 128位0向量
        /// </summary>
        private static byte[] Iv
        {
            get
            {
                if (_iv == null)
                {
                    var size = 16;
                    _iv = new byte[size];
                    for (int i = 0; i < size; i++)
                    {
                        _iv[i] = 0;
                    }
                }
                return _iv;
            }
        }

        /// <summary>
        /// AES密钥
        /// </summary>
        private static string AesKey = "QaP1AF8utIarcBqdhYTZpVGbiNQ9M6IL";

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <returns></returns>
        public static string AesEncrypt(string value)
        {
            return AesEncrypt(value, AesKey);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string AesEncrypt(string value, string key)
        {
            return AesEncrypt(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="value">待加密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string AesEncrypt(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var rijndaelManaged = CreateRijndaelManaged(key);
            using var transform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
            return GetEncryptResult(value, encoding, transform);
        }

        /// <summary>
        /// 创建RijndaelManaged
        /// </summary>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        private static RijndaelManaged CreateRijndaelManaged(string key)
        {
            return new RijndaelManaged()
            {
                Key = Convert.FromBase64String(key),
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                IV = Iv
            };
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <returns></returns>
        public static string AesDecrypt(string value)
        {
            return AesDecrypt(value, AesKey);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string AesDecrypt(string value, string key)
        {
            return AesDecrypt(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="value">待解密的值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string AesDecrypt(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            var rijndaelManaged = CreateRijndaelManaged(key);
            using var transform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
            return GetDecryptResult(value, encoding, transform);
        }

        #endregion AES加密

        #region AES加密（兼容多语言的解密加密，微软自带的补位不友好）

        /// <summary>
        /// Aes加密（AES/CBC/PKCS7=>None）（兼容多语言的加密，微软自带的补位不友好）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="appSecret"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static string AesEncrypt<T>(T input, string appSecret, string appId)
        {
            if (input == null) return string.Empty;

            var json = input.ToJson(camelCase: true);

            return Encrypt.AesEncrypt(json, appSecret, appId);
        }
        /// <summary>
        /// Aes解密（AES/CBC/PKCS7=>None）（兼容多语言的解密，微软自带的补位不友好）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="appSecret"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        public static T AesDecrypt<T>(string input, string appSecret, ref string appId)
        {
            if (string.IsNullOrEmpty(input)) return default;

            var val = Encrypt.AesDecrypt(input, appSecret, ref appId);

            return val.ToObject<T>();
        }
        /// <summary>
        /// Aes解密方法（AES/CBC/PKCS7=>None）（兼容多语言的解密，微软自带的补位不友好）
        /// </summary>
        /// <param name="input">密文</param>
        /// <param name="appSecret"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static string AesDecrypt(string input, string appSecret, ref string appid)
        {
            //byte[] Key = Convert.FromBase64String(appSecret);
            byte[] Key = Encoding.UTF8.GetBytes(appSecret);
            //iv向量 从密钥获取16位
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            //解密字符串
            byte[] btmpMsg = AesDecrypt(input, Iv, Key);

            //消息体结构：  int[4](内容长度) + byte[](内容) + byte[](appid)

            //获取解密后的消息头长度=====》网关为C#开发，请注意大小端的问题
            int len = BitConverter.ToInt32(btmpMsg, 0);
            var msgLen = 4;
            //定义消息
            byte[] bMsg = new byte[len];
            //定义消息中appid长度
            byte[] bAppid = new byte[btmpMsg.Length - msgLen - len];
            //获取消息内容====需要去掉消息头从4位开始
            Array.Copy(btmpMsg, msgLen, bMsg, 0, len);
            //获取appid=====去掉消息头的4位，以及消息整体长度，剩下的就是appid长度
            Array.Copy(btmpMsg, msgLen + len, bAppid, 0, btmpMsg.Length - msgLen - len);
            //消息内容
            string oriMsg = Encoding.UTF8.GetString(bMsg);
            //appid内容
            appid = Encoding.UTF8.GetString(bAppid);

            return oriMsg;
        }
        /// <summary>
        /// Aes解密方法（AES/CBC/PKCS7=>None）（兼容多语言的解密，微软自带的补位不友好）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Iv"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static byte[] AesDecrypt(string input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.None,
                Key = Key,
                IV = Iv
            };
            var decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] baseStr = Convert.FromBase64String(input);

                    byte[] msg = new byte[baseStr.Length + 32 - baseStr.Length % 32];
                    Array.Copy(baseStr, msg, baseStr.Length);

                    cs.Write(baseStr, 0, baseStr.Length);
                }
                xBuff = Decode2(ms.ToArray());
            }
            return xBuff;
        }

        private static byte[] Decode2(byte[] decrypted)
        {
            int pad = (int)decrypted[^1];
            if (pad < 1 || pad > 32)
                pad = 0;
            byte[] res = new byte[decrypted.Length - pad];
            Array.Copy(decrypted, 0, res, 0, decrypted.Length - pad);
            return res;
        }

        /// <summary>
        /// Aes加密方法（AES/CBC/PKCS7=>None）（兼容多语言的加密，微软自带的补位不友好）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="appSecret"></param>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static string AesEncrypt(string input, string appSecret, string appid)
        {
            //byte[] Key = Convert.FromBase64String(appSecret);
            byte[] Key = Encoding.UTF8.GetBytes(appSecret);
            //iv向量 从密钥获取16位
            byte[] Iv = new byte[16];
            Array.Copy(Key, Iv, 16);
            //消息体
            byte[] btmpMsg = Encoding.UTF8.GetBytes(input);
            //消息长度
            byte[] bMsgLen = BitConverter.GetBytes(btmpMsg.Length);
            //appid
            byte[] bAppid = Encoding.UTF8.GetBytes(appid);
            //新的消息（消息长度+内容长度+appid长度）
            byte[] bMsg = new byte[bMsgLen.Length + btmpMsg.Length + bAppid.Length];

            //消息体结构：  int[4](内容长度) + byte[](内容) + byte[](appid)

            //头部消息。记录消息长度
            Array.Copy(bMsgLen, 0, bMsg, 0, bMsgLen.Length);
            //追加消息内容
            Array.Copy(btmpMsg, 0, bMsg, bMsgLen.Length, btmpMsg.Length);
            //追加appid在尾部
            Array.Copy(bAppid, 0, bMsg, bMsgLen.Length + btmpMsg.Length, bAppid.Length);
            //加密并补位
            return AesEncrypt(bMsg, Iv, Key);
        }

        /// <summary>
        /// Aes加密方法（AES/CBC/PKCS7=>None）（兼容多语言的加密，微软自带的补位不友好）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Iv"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        private static string AesEncrypt(byte[] input, byte[] Iv, byte[] Key)
        {
            var aes = new RijndaelManaged
            {
                //秘钥的大小，以位为单位
                KeySize = 256,
                //支持的块大小
                BlockSize = 128,
                //填充模式
                //aes.Padding = PaddingMode.PKCS7;
                Padding = PaddingMode.None,
                Mode = CipherMode.CBC,
                Key = Key,
                IV = Iv
            };
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;

            #region 自己进行PKCS7补位，用系统自己带的不行

            byte[] msg = new byte[input.Length + 32 - input.Length % 32];
            Array.Copy(input, msg, input.Length);
            byte[] pad = KCS7Encoder(input.Length);
            Array.Copy(pad, 0, msg, input.Length, pad.Length);

            #endregion

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    cs.Write(msg, 0, msg.Length);
                }
                xBuff = ms.ToArray();
            }

            return Convert.ToBase64String(xBuff);
        }
        /// <summary>
        /// KCS7补位
        /// </summary>
        /// <param name="text_length"></param>
        /// <returns></returns>
        private static byte[] KCS7Encoder(int text_length)
        {
            int block_size = 32;
            // 计算需要填充的位数
            int amount_to_pad = block_size - (text_length % block_size);
            if (amount_to_pad == 0)
            {
                amount_to_pad = block_size;
            }
            // 获得补位所用的字符
            char pad_chr = Chr(amount_to_pad);
            string tmp = "";
            for (int index = 0; index < amount_to_pad; index++)
            {
                tmp += pad_chr;
            }
            return Encoding.UTF8.GetBytes(tmp);
        }
        /// <summary>
        /// 将数字转化成ASCII码对应的字符，用于对明文进行补码
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private static char Chr(int a)
        {
            byte target = (byte)(a & 0xFF);
            return (char)target;
        }

        #endregion

        #region RSA签名

        /// <summary>
        /// RSA签名，采用 SHA1 算法
        /// </summary>
        /// <param name="value">待签名的值</param>
        /// <param name="key">私钥</param>
        /// <returns></returns>
        public static string RsaSign(string value, string key)
        {
            return RsaSign(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// RSA签名，采用 SHA1 算法
        /// </summary>
        /// <param name="value">待签名的值</param>
        /// <param name="key">私钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string RsaSign(string value, string key, Encoding encoding)
        {
            return RsaSign(value, key, encoding, RSAType.RSA);
        }

        /// <summary>
        /// RSA签名，采用 SHA256 算法
        /// </summary>
        /// <param name="value">待签名的值</param>
        /// <param name="key">私钥</param>
        /// <returns></returns>
        public static string Rsa2Sign(string value, string key)
        {
            return Rsa2Sign(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// RSA签名，采用 SHA256 算法
        /// </summary>
        /// <param name="value">待签名的值</param>
        /// <param name="key">私钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Rsa2Sign(string value, string key, Encoding encoding)
        {
            return RsaSign(value, key, encoding, RSAType.RSA2);
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="value">待签名的值</param>
        /// <param name="key">私钥</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="type">RSA算法类型</param>
        /// <returns></returns>
        private static string RsaSign(string value, string key, Encoding encoding, RSAType type)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var rsa = new RsaHelper(type, encoding, key);
            return rsa.Sign(value);
        }

        /// <summary>
        /// RSA验签，采用 SHA1 算法
        /// </summary>
        /// <param name="value">待验签的值</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        public static bool RsaVerify(string value, string publicKey, string sign)
        {
            return RsaVerify(value, publicKey, sign, Encoding.UTF8);
        }

        /// <summary>
        /// RSA验签，采用 SHA1 算法
        /// </summary>
        /// <param name="value">待验签的值</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="sign">签名</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static bool RsaVerify(string value, string publicKey, string sign, Encoding encoding)
        {
            return RsaVerify(value, publicKey, sign, encoding, RSAType.RSA);
        }

        /// <summary>
        /// RSA验签，采用 SHA256 算法
        /// </summary>
        /// <param name="value">待验签的值</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="sign">签名</param>
        /// <returns></returns>
        public static bool Rsa2Verify(string value, string publicKey, string sign)
        {
            return Rsa2Verify(value, publicKey, sign, Encoding.UTF8);
        }

        /// <summary>
        /// RSA验签，采用 SHA256 算法
        /// </summary>
        /// <param name="value">待验签的值</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="sign">签名</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static bool Rsa2Verify(string value, string publicKey, string sign, Encoding encoding)
        {
            return RsaVerify(value, publicKey, sign, encoding, RSAType.RSA2);
        }

        /// <summary>
        /// RSA验签
        /// </summary>
        /// <param name="value">待验签的值</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="sign">签名</param>
        /// <param name="encoding">字符编码</param>
        /// <param name="type">RSA算法类型</param>
        /// <returns></returns>
        private static bool RsaVerify(string value, string publicKey, string sign, Encoding encoding, RSAType type)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            var rsa = new RsaHelper(type, encoding, publicKey: publicKey);
            return rsa.Verify(value, sign);
        }

        #endregion RSA签名

        #region HmacMd5加密

        /// <summary>
        /// HMACMD5加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string HmacMd5(string value, string key)
        {
            return HmacMd5(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// HMACMD5加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string HmacMd5(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var md5 = new HMACMD5(encoding.GetBytes(key));
            var hash = md5.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        #endregion HmacMd5加密

        #region HmacSha1加密

        /// <summary>
        /// HMACSHA1加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string HmacSha1(string value, string key)
        {
            return HmacSha1(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// HMACSHA1加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string HmacSha1(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var sha1 = new HMACSHA1(encoding.GetBytes(key));
            var hash = sha1.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        #endregion HmacSha1加密

        #region HmacSha256加密

        /// <summary>
        /// HMACSHA256加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string HmacSha256(string value, string key)
        {
            return HmacSha256(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// HMACSHA256加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string HmacSha256(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var sha256 = new HMACSHA256(encoding.GetBytes(key));
            var hash = sha256.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        #endregion HmacSha256加密

        #region HmacSha384加密

        /// <summary>
        /// HMACSHA384加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string HmacSha384(string value, string key)
        {
            return HmacSha384(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// HMACSHA384加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string HmacSha384(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var sha384 = new HMACSHA384(encoding.GetBytes(key));
            var hash = sha384.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        #endregion HmacSha384加密

        #region HmacSha512加密

        /// <summary>
        /// HMACSHA512加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static string HmacSha512(string value, string key)
        {
            return HmacSha512(value, key, Encoding.UTF8);
        }

        /// <summary>
        /// HMACSHA512加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="key">密钥</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string HmacSha512(string value, string key, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value) || string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }
            var sha512 = new HMACSHA512(encoding.GetBytes(key));
            var hash = sha512.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        #endregion HmacSha512加密

        #region SHA1加密

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Sha1(string value)
        {
            return Sha1(value, Encoding.UTF8);
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Sha1(string value, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            using var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        #endregion SHA1加密

        #region SHA256加密

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Sha256(string value)
        {
            return Sha256(value, Encoding.UTF8);
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Sha256(string value, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        #endregion SHA256加密

        #region SHA384加密

        /// <summary>
        /// SHA384加密
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Sha384(string value)
        {
            return Sha384(value, Encoding.UTF8);
        }

        /// <summary>
        /// SHA384加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Sha384(string value, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            using var sha = SHA384.Create();
            var hash = sha.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        #endregion SHA384加密

        #region SHA512加密

        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Sha512(string value)
        {
            return Sha512(value, Encoding.UTF8);
        }

        /// <summary>
        /// SHA512加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Sha512(string value, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return string.Empty;
            }
            using var sha = SHA512.Create();
            var hash = sha.ComputeHash(encoding.GetBytes(value));
            return string.Join("", hash.ToList().Select(x => x.ToString("x2")).ToArray());
        }

        #endregion SHA512加密

        #region Base64加密

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Base64Encrypt(string value)
        {
            return Base64Encrypt(value, Encoding.UTF8);
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Base64Encrypt(string value, Encoding encoding)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : Convert.ToBase64String(encoding.GetBytes(value));
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string Base64Decrypt(string value)
        {
            return Base64Decrypt(value, Encoding.UTF8);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="encoding">字符编码</param>
        /// <returns></returns>
        public static string Base64Decrypt(string value, Encoding encoding)
        {
            return string.IsNullOrWhiteSpace(value)
                ? string.Empty
                : encoding.GetString(Convert.FromBase64String(value));
        }

        #endregion Base64加密
    }
}