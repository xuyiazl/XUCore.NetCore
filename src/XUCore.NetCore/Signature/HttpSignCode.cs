using System;
using System.Collections.Generic;
using System.Text;

namespace XUCore.NetCore.Signature
{
    public class HttpSignCode
    {
        private static IDictionary<HttpSignSubCode, (string, string)> zhCn = new Dictionary<HttpSignSubCode, (string, string)> {
            { HttpSignSubCode.MissingParams,("1001","缺少签名参数")},
            { HttpSignSubCode.ParamsFail,("1002","签名参数不正确")},
            { HttpSignSubCode.RequestTimeout,("1003","请求超时")},
            { HttpSignSubCode.SignFail,("1004","签名错误")},
            { HttpSignSubCode.RequestFail,("1005","请求失败")},
        };

        public static (string, string) Message(HttpSignSubCode HttpSignSubCode)
        {
            return zhCn[HttpSignSubCode];
        }
    }
    public enum HttpSignSubCode
    {
        MissingParams, ParamsFail, RequestTimeout, SignFail, RequestFail
    }
}
