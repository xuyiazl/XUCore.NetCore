using XUCore.Extensions;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace XUCore.Helpers
{
    /// <summary>
    /// 金额单位转换操作
    /// </summary>
    public static class AmountUnitConv
    {
        /// <summary>
        /// 分转元
        /// </summary>
        /// <param name="fen">分</param>
        public static decimal ToYuan(int fen) => Conv.ToDecimal((decimal)fen / 100, 2);

        /// <summary>
        /// 分转元
        /// </summary>
        /// <param name="fen">分</param>
        public static decimal ToYuan(int? fen) => fen == null ? 0 : Conv.ToDecimal((decimal)fen / 100, 2);

        /// <summary>
        /// 元转分
        /// </summary>
        /// <param name="yuan">元</param>
        public static int ToFen(decimal yuan) => Conv.ToInt(CutDecimalWithN(yuan, 2) * 100, 0);

        /// <summary>
        /// 元转分
        /// </summary>
        /// <param name="yuan">元</param>
        public static int ToFen(decimal? yuan) => yuan == null ? 0 : Conv.ToInt(CutDecimalWithN(yuan.Value, 2) * 100, 0);

        /// <summary>
        /// 截取保留N位小数且不进行四舍五入
        /// </summary>
        /// <param name="input">输入值</param>
        /// <param name="digits">小数位数</param>
        private static decimal CutDecimalWithN(decimal input, int digits)
        {
            var decimalStr = input.ToString(CultureInfo.InvariantCulture);
            var index = decimalStr.IndexOf(".", StringComparison.Ordinal);
            if (index == -1 || decimalStr.Length < index + digits + 1)
            {
                decimalStr = string.Format("{0:F" + digits + "}", input);
            }
            else
            {
                var length = index;
                if (digits != 0)
                    length = index + digits + 1;
                decimalStr = decimalStr.Substring(0, length);
            }
            return decimal.Parse(decimalStr);
        }

        /// <summary>
        /// 保留两位小数
        /// </summary>
        /// <param name="input">输入值</param>
        public static string ToN2String(decimal input) => $"{input:N2}";

        #region ToChinese(阿拉伯数字金额、中文大写互转)

        /// <summary>
        /// 把阿拉伯数字的金额转换为中文大写数字
        ///<example>Console.WriteLine("{0,14:N2}: {1}", x, ConvertToChinese(x));</example>
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToChinese(double x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", delegate (Match m) { return "负元 零壹贰叁肆伍陆柒捌玖       分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString(); });
        }

        /// <summary>
        /// 把阿拉伯数字的金额转换为中文大写数字
        /// </summary>
        ///<example>Console.WriteLine("{0,14:N2}: {1}", x, ConvertToChinese(x));</example>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string ToChinese(string x)
        {
            double money = x.ToDouble();
            return ToChinese(money);
        }

        #endregion ToChinese(阿拉伯数字金额、中文大写互转)
    }
}