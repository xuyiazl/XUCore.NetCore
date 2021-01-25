﻿using XUCore.Timing;
using System;

namespace XUCore.Helpers
{
    /// <summary>
    /// 时间操作
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// 日期
        /// </summary>
        private static DateTime? _dateTime;

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="dateTime">时间</param>
        public static void SetTime(DateTime? dateTime) => _dateTime = dateTime;

        /// <summary>
        /// 设置时间
        /// </summary>
        /// <param name="dateTime">时间</param>
        public static void SetTime(string dateTime) => _dateTime = Conv.ToDateOrNull(dateTime);

        /// <summary>
        /// 重置时间
        /// </summary>
        public static void Reset() => _dateTime = null;

        /// <summary>
        /// 获取当前日期时间
        /// </summary>
        public static DateTime GetDateTime() => _dateTime ?? DateTime.Now;

        /// <summary>
        /// 获取当前日期，不带时间
        /// </summary>
        public static DateTime GetDate() => GetDateTime().Date;

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        public static long GetUnixTimestamp() => GetUnixTimestamp(DateTime.Now);

        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <param name="time">时间</param>
        public static long GetUnixTimestamp(DateTime time) => UnixTime.ToTimestamp(time);

        /// <summary>
        /// 从Unix时间戳获取时间
        /// </summary>
        /// <param name="timestamp">Unix时间戳</param>
        public static DateTime GetTimeFromUnixTimestamp(long timestamp) => UnixTime.ToDateTime(timestamp, DateTimeKind.Local);
    }
}