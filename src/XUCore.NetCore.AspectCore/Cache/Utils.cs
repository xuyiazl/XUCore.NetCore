using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using XUCore.Extensions;
using XUCore.Serializer;

namespace XUCore.NetCore.AspectCore.Cache
{
    internal static class Utils
    {
        public static string GetParamterKey(string Prefix, string Key, string ParamterKey, object[] paramters)
        {
            string key = $"{Prefix}{Key}";

            if (string.IsNullOrEmpty(ParamterKey) || paramters == null)
                return key;

            var paramList = new List<string>();

            foreach (var param in paramters)
            {
                if (param == null)
                    paramList.Add(param.SafeString());
                else if (param.GetType() == typeof(CancellationToken))
                    continue;
                else if (param.GetType() == typeof(int[]))
                    paramList.Add(((int[])param).Join(","));
                else if (param.GetType() == typeof(long[]))
                    paramList.Add(((long[])param).Join(","));
                else if (param.GetType() == typeof(short[]))
                    paramList.Add(((short[])param).Join(","));
                else if (param.GetType() == typeof(string[]))
                    paramList.Add(((string[])param).Join(","));
                else if (param.GetType().BaseType == typeof(object))
                {
                    //如果是从对象中获取参数，则先转换成字典，再正则匹配替换（仅支持一层结构）
                    string tmp = ParamterKey;

                    var jsonObj = param.SafeString().ToObject<Dictionary<string, object>>();

                    Regex reg = new Regex(@"\{(\w+)\}");

                    foreach (Match m in reg.Matches(ParamterKey))
                    {
                        var field = m.Groups[1].Value;
                        if (!jsonObj.ContainsKey(field))
                            continue;

                        tmp = tmp.Replace("{" + field + "}", jsonObj[field].SafeString());
                    }

                    ParamterKey = $"{tmp}";
                }
                else
                    paramList.Add(param.SafeString());
            }

            if (paramList.Count > 0)
                key += $"{string.Format(ParamterKey, paramList.ToArray())}";
            else
                key += ParamterKey;

            return key;
        }
    }
}
