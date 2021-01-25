using System;

namespace XUCore.Helpers
{
    /// <summary>
    /// Unix时间操作
    /// </summary>
    public static class UnixTime
    {
        /// <summary>
        /// Unix纪元时间
        /// </summary>
        public static DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// 转换为Unix时间戳
        /// </summary>
        /// <param name="isContainMillisecond">是否包含毫秒</param>
        /// <returns></returns>
        public static long ToTimestamp(bool isContainMillisecond = true)
        {
            return ToTimestamp(DateTime.Now, isContainMillisecond);
        }

        /// <summary>
        /// 转换为Unix时间戳
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="isContainMillisecond">是否包含毫秒</param>
        /// <returns></returns>
        public static long ToTimestamp(DateTime dateTime, bool isContainMillisecond = true)
        {
            //return dateTime.Kind == DateTimeKind.Utc
            //    ? Convert.ToInt64((dateTime - EpochTime).TotalMilliseconds / (isContainMillisecond ? 1 : 1000))
            //    : Convert.ToInt64((TimeZoneInfo.ConvertTimeToUtc(dateTime) - EpochTime).TotalMilliseconds /
            //                      (isContainMillisecond ? 1 : 1000));
            if (isContainMillisecond)
                return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
            else
                return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
        }

        /// <summary>
        /// 将给定 Unix 时间戳 转换为 DateTime 时间。
        /// </summary>
        /// <param name="unixTimeStamp">Unix 时间戳。</param>
        /// <param name="dateTimeKind">Utc or Local</param>
        /// <returns></returns>
        public static DateTime ToDateTime(long unixTimeStamp, DateTimeKind dateTimeKind = DateTimeKind.Local)
        {
            if (unixTimeStamp.ToString().Length == 10)
                return dateTimeKind == DateTimeKind.Local ?
                    DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).LocalDateTime :
                    DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
            else
                return dateTimeKind == DateTimeKind.Local ?
                    DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).LocalDateTime :
                    DateTimeOffset.FromUnixTimeMilliseconds(unixTimeStamp).UtcDateTime;
        }
    }
}