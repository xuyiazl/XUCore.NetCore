namespace XUCore
{
    /// <summary>
    /// 正则
    /// </summary>
    public class RegexPatterns
    {
        /// <summary>
        /// 正整数
        /// </summary>
        public const string Number = @"^[0-9]+$";

        /// <summary>
        /// 整数
        /// </summary>
        public const string NumberSign = @"^[+-]?[0-9]+$";

        /// <summary>
        /// 中文
        /// </summary>
        public const string ZHCN = @"[\u4e00-\u9fa5]";

        /// <summary>
        /// 字母
        /// </summary>
        public const string Alpha = @"^[a-zA-Z]*$";

        /// <summary>
        /// 大写字母
        /// </summary>
        public const string AlphaUpperCase = @"^[A-Z]*$";

        /// <summary>
        /// 小写字母
        /// </summary>
        public const string AlphaLowerCase = @"^[a-z]*$";

        /// <summary>
        /// 字母+整数数字
        /// </summary>
        public const string AlphaNumeric = @"^[a-zA-Z0-9]*$";

        /// <summary>
        /// 字母+整数数字+空格
        /// </summary>
        public const string AlphaNumericSpace = @"^[a-zA-Z0-9 ]*$";

        /// <summary>
        /// 字母+整数数字+空格+（-）
        /// </summary>
        public const string AlphaNumericSpaceDash = @"^[a-zA-Z0-9 \-]*$";

        /// <summary>
        /// 字母+整数数字+空格+（-）+(_)
        /// </summary>
        public const string AlphaNumericSpaceDashUnderscore = @"^[a-zA-Z0-9 \-_]*$";

        /// <summary>
        /// 字母+整数数字+空格+（-）+(_)+（.）
        /// </summary>
        public const string AlphaNumericSpaceDashUnderscorePeriod = @"^[a-zA-Z0-9\. \-_]*$";

        /// <summary>
        /// 数字
        /// </summary>
        public const string Numeric = @"^\-?[0-9]*\.?[0-9]*$";

        /// <summary>
        /// 中文验证正则表达式
        /// </summary>
        public const string ChinesePattern = @"^[\u4e00-\u9fa5]+$";

        /// <summary>
        /// 身份证
        /// </summary>
        public const string IdentityCard = @"(^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}(?:\d|x|X)$)";

        /// <summary>
        /// Email
        /// </summary>
        public const string Email = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";

        /// <summary>
        /// Url
        /// </summary>
        public const string Url = @"^^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&%\$#_=]*)?$";

        /// <summary>
        /// 邮编
        /// </summary>
        public const string ZipCode = @"^[1-9]\d{5}$";

        /// <summary>
        /// 手机号码
        /// </summary>
        public const string MobilePhone = @"^1[0-9]{10}";

        /// <summary>
        /// 电话号码
        /// </summary>
        public const string TelPhone = @"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$";

        /// <summary>
        /// 微信号验证正则表达式
        /// </summary>
        public const string WechatNoPatter = @"^[a-zA-Z]([-_a-zA-Z0-9]{5,19})+$";

        /// <summary>
        /// 固话验证正则表达式
        /// </summary>
        public const string TelNoOfChinaPatter = @"^\d{3,4}-?\d{6,8}$";

        /// <summary>
        /// 车牌号验证正则表达式
        /// </summary>
        public const string PlateNumberOfChinaPatter =
            @"^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-Z0-9]{4}[A-Z0-9挂学警港澳]{1}$";

        /// <summary>
        /// 邮政编码验证正则表达式
        /// </summary>
        public const string PostalCodeOfChinaPatter = @"^\d{6}$";

        /// <summary>
        /// QQ验证正则表达式
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const string QQPatter = @"^[1-9][0-9]{4,10}$";
    }
}