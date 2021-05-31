﻿using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Text;
using XUCore.Extensions;
using XUCore.Serializer;
using XUCore.Serializer.Converters;

namespace XUCore.NetCore.MessagePack
{

    public static class MessagePackUtils
    {
        public static void FormatterJsonOptions(OutputFormatterWriteContext context, MessagePackFormatterOptions options)
        {
            if (options.JsonSerializerSettings != null && options.JsonSerializerSettings.ContractResolver?.GetType() == typeof(LimitPropsContractResolver))
            {
                var headers = context.HttpContext.Request.Headers;
                // col1,col2
                var fields = headers["limit-field"].SafeString().Split(",", true);
                // 指定输出字段 contain or match or equal
                // 忽略指定字段 ignore
                var limitMode = headers["limit-mode"].SafeString().ToLower();
                // 解析器 camelcase 小驼峰 ， default 默认大写
                var contractResolverMode = headers["limit-resolver"].SafeString().ToLower();
                // column1=col1,column2=col2
                var rename = headers["limit-field-rename"].SafeString().ToMap(',', '=', true, false, true);

                var dateFormat = headers["limit-date-format"].SafeString();
                var dateUnix = headers["limit-date-unix"].SafeString().ToLower();

                if (!dateFormat.IsEmpty())
                    options.JsonSerializerSettings.DateFormatString = dateFormat;
                else
                    options.JsonSerializerSettings.DateFormatString = string.Empty;

                options.JsonSerializerSettings.Converters.Clear();

                if (!dateUnix.IsEmpty() && dateUnix.Equals("true"))
                {
                    options.JsonSerializerSettings.Converters.Add(new DateTimeToUnixConverter());
                    options.JsonSerializerSettings.Converters.Add(new DateTimeNullToUnixConverter());
                }


                var _limitType = LimitPropsMode.Contains;

                switch (limitMode)
                {
                    case "contain":
                    case "match":
                    case "equal":
                        _limitType = LimitPropsMode.Contains;
                        break;
                    default:
                        _limitType = LimitPropsMode.Ignore;
                        break;
                }

                var _resolverMode = ResolverMode.Default;

                switch (contractResolverMode)
                {
                    case "camelcase":
                        _resolverMode = ResolverMode.CamelCase;
                        break;
                    default:
                        _resolverMode = ResolverMode.Default;
                        break;
                }

                options.JsonSerializerSettings.ContractResolver = new LimitPropsContractResolver(fields, _limitType, rename, _resolverMode);
            }
        }
    }
}
