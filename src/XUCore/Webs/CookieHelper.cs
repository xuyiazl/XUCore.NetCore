using Microsoft.AspNetCore.Http;
using XUCore.Helpers;
using System;

namespace XUCore.Webs
{
    /// <summary>
    /// cookie有效期枚举
    /// </summary>
    public enum ExpiresType
    {
        ///<summary>
        /// 秒
        /// </summary>
        Seconds,
        ///<summary>
        /// 分
        /// </summary>
        Minutes,
        ///<summary>
        /// 时
        /// </summary>
        Hours,
        ///<summary>
        /// 天
        /// </summary>
        Days,
        ///<summary>
        /// 月
        /// </summary>
        Months,
        ///<summary>
        /// 年
        /// </summary>
        Years
    }

    /// <summary>
    /// Cookie 操作辅助类
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        public static void SetCookie(string cookieName, string value)
            => SetCookie(cookieName, value, string.Empty);

        /// <summary>
        /// 设置cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="domain"></param>
        public static void SetCookie(string cookieName, string value, string domain)
            => SetCookie(cookieName, value, string.Empty, 0, domain);

        /// <summary>
        /// 设置cookie 。eType: s 秒 m 分，h 时，d 天，M 月，y 年
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expiresType"></param>
        /// <param name="expires"></param>
        public static void SetCookie(string cookieName, string value, string expiresType, int expires)
            => SetCookie(cookieName, value, expiresType, expires, string.Empty);

        /// <summary>
        /// 设置cookie 。eType: s 秒 m 分，h 时，d 天，M 月，y 年
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expiresType"></param>
        /// <param name="expires"></param>
        /// <param name="domain"></param>
        public static void SetCookie(string cookieName, string value, string expiresType, int expires, string domain)
            => SetCookie(cookieName, value, expiresType, expires, true, domain);

        /// <summary>
        /// 设置cookie 。eType: s 秒 m 分，h 时，d 天，M 月，y 年
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expiresType"></param>
        /// <param name="expires"></param>
        /// <param name="httponly"></param>
        /// <param name="domain"></param>
        public static void SetCookie(string cookieName, string value, string expiresType, int expires, bool httponly, string domain)
            => SetCookie(Web.HttpContext, cookieName, value, expiresType, expires, httponly, domain);

        /// <summary>
        /// 设置cookie 。eType: s 秒 m 分，h 时，d 天，M 月，y 年
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="value">值</param>
        /// <param name="expirestype">有效期类型 s 秒 m 分，h 时，d 天，M 月，y 年</param>
        /// <param name="expires">有效期 分钟</param>
        /// <param name="httponly">客户端脚本访问</param>
        /// <param name="domain">域</param>
        public static void SetCookie(HttpContext context, string cookieName, string value, string expirestype, int expires, bool httponly, string domain)
        {
            CookieOptions options = new CookieOptions();

            switch (expirestype)
            {
                case "s":
                    options.Expires = DateTime.Now.AddSeconds(expires);
                    break;

                case "m":
                    options.Expires = DateTime.Now.AddMinutes(expires);
                    break;

                case "h":
                    options.Expires = DateTime.Now.AddHours(expires);
                    break;

                case "d":
                    options.Expires = DateTime.Now.AddDays(expires);
                    break;

                case "M":
                    options.Expires = DateTime.Now.AddMonths(expires);
                    break;

                case "Y":
                    options.Expires = DateTime.Now.AddYears(expires);
                    break;

                default:
                    break;
            }

            options.HttpOnly = httponly;

            if (!string.IsNullOrEmpty(domain))
                options.Domain = domain;

            context?.Response?.Cookies.Append(cookieName, Web.UrlEncode(value), options);
        }

        /// <summary>
        /// 设置cookie 。eType: s 秒 m 分，h 时，d 天，M 月，y 年
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expiresType"></param>
        /// <param name="expires"></param>
        public static void SetCookie(string cookieName, string value, ExpiresType expiresType, int expires)
            => SetCookie(cookieName, value, expiresType, expires, string.Empty);

        /// <summary>
        /// 设置cookie 。eType: s 秒 m 分，h 时，d 天，M 月，y 年
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expiresType"></param>
        /// <param name="expires"></param>
        /// <param name="domain"></param>
        public static void SetCookie(string cookieName, string value, ExpiresType expiresType, int expires, string domain)
            => SetCookie(cookieName, value, expiresType, expires, true, domain);

        /// <summary>
        /// 设置cookie 。eType: s 秒 m 分，h 时，d 天，M 月，y 年
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expiresType"></param>
        /// <param name="expires"></param>
        /// <param name="httponly"></param>
        /// <param name="domain"></param>
        public static void SetCookie(string cookieName, string value, ExpiresType expiresType, int expires, bool httponly, string domain)
            => SetCookie(Web.HttpContext, cookieName, value, expiresType, expires, httponly, domain);

        /// <summary>
        /// 设置cookie 。eType: s 秒 m 分，h 时，d 天，M 月，y 年
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="value">值</param>
        /// <param name="expiresType">有效期类型 s 秒 m 分，h 时，d 天，M 月，y 年</param>
        /// <param name="expires">有效期 分钟</param>
        /// <param name="httponly">客户端脚本访问</param>
        /// <param name="domain">域</param>
        public static void SetCookie(HttpContext context, string cookieName, string value, ExpiresType expiresType, int expires, bool httponly, string domain)
        {
            CookieOptions options = new CookieOptions();

            switch (expiresType)
            {
                case ExpiresType.Seconds:
                    options.Expires = DateTime.Now.AddSeconds(expires);
                    break;

                case ExpiresType.Minutes:
                    options.Expires = DateTime.Now.AddMinutes(expires);
                    break;

                case ExpiresType.Hours:
                    options.Expires = DateTime.Now.AddHours(expires);
                    break;

                case ExpiresType.Days:
                    options.Expires = DateTime.Now.AddDays(expires);
                    break;

                case ExpiresType.Months:
                    options.Expires = DateTime.Now.AddMonths(expires);
                    break;

                default:
                    options.Expires = DateTime.Now.AddYears(expires);
                    break;
            }

            options.HttpOnly = httponly;
            if (!string.IsNullOrEmpty(domain))
                options.Domain = domain;

            context?.Response?.Cookies.Append(cookieName, Web.UrlEncode(value), options);
        }

        /// <summary>
        /// 获取Cookie的value值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(string cookieName)
            => GetCookie(Web.HttpContext, cookieName);

        /// <summary>
        /// 获取Cookie的value值
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(HttpContext context, string cookieName)
        {
            if (context.Request.Cookies.TryGetValue(cookieName, out string value))
                return value;
            return string.Empty;
        }
        /// <summary>
        /// 获取Cookie的value值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static T GetCookie<T>(string cookieName)
            => GetCookie<T>(Web.HttpContext, cookieName);

        /// <summary>
        /// 获取Cookie的value值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context">Http上下文</param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static T GetCookie<T>(HttpContext context, string cookieName)
        {
            if (context.Request.Cookies.TryGetValue(cookieName, out string value))
                return Conv.To<T>(value);
            return default(T);
        }

        /// <summary>
        /// 删除cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void Delete(string cookieName)
            => Delete(Web.HttpContext, cookieName);

        /// <summary>
        /// 删除cookie
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="cookieName"></param>
        public static void Delete(HttpContext context, string cookieName)
        {
            context?.Response?.Cookies.Delete(cookieName);
        }

        /// <summary>
        /// 删除指定域下cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="cookieDomain"></param>
        public static void Delete(string cookieName, string cookieDomain)
            => Delete(Web.HttpContext, cookieName, cookieDomain);

        /// <summary>
        /// 删除指定域下cookie
        /// </summary>
        /// <param name="context">Http上下文</param>
        /// <param name="cookieName"></param>
        /// <param name="cookieDomain"></param>
        public static void Delete(HttpContext context, string cookieName, string cookieDomain)
        {
            context?.Response?.Cookies.Delete(cookieName, new CookieOptions { Domain = cookieDomain });
        }
    }
}