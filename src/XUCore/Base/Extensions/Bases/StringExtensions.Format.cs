// ReSharper disable once CheckNamespace
using XUCore.Helpers;

namespace XUCore.Extensions
{
    /// <summary>
    /// 字符串(<see cref="string"/>) 扩展 - 格式化
    /// </summary>
    public static partial class StringExtensions
    {
        /// <summary>
        /// 加密车牌号
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        /// <param name="specialChar"></param>
        public static string EncryptPlateNumberOfChina(string plateNumber, char specialChar = '*') => Format.EncryptPlateNumberOfChina(plateNumber, specialChar);
        /// <summary>
        /// 加密汽车VIN
        /// </summary>
        /// <param name="vinCode">汽车VIN</param>
        /// <param name="specialChar"></param>
        public static string EncryptVinCode(string vinCode, char specialChar = '*') => Format.EncryptVinCode(vinCode, specialChar);
        /// <summary>
        /// 格式化金额
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="isEncrypt">是否加密。默认：false</param>
        public static string FormatMoney(this decimal money, bool isEncrypt = false) => Format.FormatMoney(money, isEncrypt);
        /// <summary>
        /// 加密手机号码信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="specialChar"></param>
        /// <returns></returns>
        public static string EncryptPhone(this string value, char specialChar = '*') => Format.EncryptPhone(value, specialChar);
        /// <summary>
        /// 加密邮箱信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="specialChar"></param>
        /// <returns></returns>
        public static string EncryptEmail(this string value, char specialChar = '*') => Format.EncryptEmail(value, specialChar);
        /// <summary>
        /// 加密敏感信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="specialChar"></param>
        /// <returns></returns>
        public static string EncryptSensitiveInfo(this string value, char specialChar = '*') => Format.EncryptSensitiveInfo(value, specialChar);
        /// <summary>
        /// 将传入的字符串中间部分字符替换成特殊字符
        /// </summary>
        /// <param name="value">需要替换的字符串</param>
        /// <param name="startLen">前保留长度</param>
        /// <param name="endLen">尾保留长度</param>
        /// <param name="specialChar">特殊字符</param>
        /// <returns>被特殊字符替换的字符串</returns>
        public static string EncryptString(this string value, int startLen = 4, int endLen = 4, char specialChar = '*')
            => Format.EncryptString(value, startLen, endLen, specialChar);
    }
}