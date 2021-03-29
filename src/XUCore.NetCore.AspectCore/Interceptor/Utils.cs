using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using XUCore.Extensions;

namespace XUCore.NetCore.AspectCore.Interceptor
{
    internal static class Utils
    {
        public static string GetParamterKey(string Prefix, string Key, string ParamterKey, object[] paramters)
        {
            string key = $"{Prefix}{Key}";

            if (!string.IsNullOrEmpty(ParamterKey) && paramters != null)
            {
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
                    else
                        paramList.Add(param.SafeString());
                }

                if (paramList.Count > 0)
                    key += $"_{string.Format(ParamterKey, paramList.ToArray())}";
            }

            return key;
        }
    }
}
