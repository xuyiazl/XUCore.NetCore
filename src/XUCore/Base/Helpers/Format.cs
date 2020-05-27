namespace XUCore.Helpers
{
    /// <summary>
    /// 格式化操作
    /// </summary>
    public static class Format
    {
        /// <summary>
        /// 加密车牌号
        /// </summary>
        /// <param name="plateNumber">车牌号</param>
        /// <param name="specialChar"></param>
        public static string EncryptPlateNumberOfChina(string plateNumber, char specialChar = '*') => EncryptString(plateNumber, 2, 2, specialChar);

        /// <summary>
        /// 加密汽车VIN
        /// </summary>
        /// <param name="vinCode">汽车VIN</param>
        /// <param name="specialChar"></param>
        public static string EncryptVinCode(string vinCode, char specialChar = '*') => EncryptString(vinCode, 3, 3, specialChar);

        /// <summary>
        /// 格式化金额
        /// </summary>
        /// <param name="money">金额</param>
        /// <param name="isEncrypt">是否加密。默认：false</param>
        public static string FormatMoney(decimal money, bool isEncrypt = false) => isEncrypt ? "***" : $"{money:N2}";

        /// <summary>
        /// 加密手机号码信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="specialChar"></param>
        /// <returns></returns>
        public static string EncryptPhone(string value, char specialChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (Regexs.IsMatch(value, RegexPatterns.MobilePhone))
                return EncryptString(value, 3, 4, specialChar);

            return EncryptSensitiveInfo(value, specialChar);
        }
        /// <summary>
        /// 加密邮箱信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="specialChar"></param>
        /// <returns></returns>
        public static string EncryptEmail(string value, char specialChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return value;

            if (Regexs.IsMatch(value, RegexPatterns.Email))
            {
                int suffixLen = value.LastIndexOf('@');
                return $"{EncryptSensitiveInfo(value.Substring(0, suffixLen), specialChar)}{value.Substring(suffixLen)}";
            }

            return EncryptSensitiveInfo(value, specialChar);
        }
        /// <summary>
        /// 加密敏感信息
        /// </summary>
        /// <param name="value"></param>
        /// <param name="specialChar"></param>
        /// <returns></returns>
        public static string EncryptSensitiveInfo(string value, char specialChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return value;

            var len = value.Length;

            if (len == 1)
                return value;
            else if (len > 1 && len <= 4)
                return EncryptString(value, 1, 0, specialChar);
            else if (len > 4 && len <= 5)
                return EncryptString(value, 1, 1, specialChar);
            else if (len > 5 && len <= 8)
                return EncryptString(value, 2, 2, specialChar);
            else if (len > 8 && len <= 10)
                return EncryptString(value, 3, 3, specialChar);
            else
                return EncryptString(value, 4, 4, specialChar);
        }

        /// <summary>
        /// 将传入的字符串中间部分字符替换成特殊字符
        /// </summary>
        /// <param name="value">需要替换的字符串</param>
        /// <param name="startLen">前保留长度</param>
        /// <param name="endLen">尾保留长度</param>
        /// <param name="specialChar">特殊字符</param>
        /// <returns>被特殊字符替换的字符串</returns>
        public static string EncryptString(string value, int startLen = 4, int endLen = 4, char specialChar = '*')
        {
            if (string.IsNullOrEmpty(value)) return value;

            int len = value.Length - startLen - endLen;

            if (len <= 0) return value;

            string left = value.Substring(0, startLen);
            string right = value.Substring(value.Length - endLen);

            return $"{left}{"".PadLeft(len, specialChar)}{right}";

        }
    }
}