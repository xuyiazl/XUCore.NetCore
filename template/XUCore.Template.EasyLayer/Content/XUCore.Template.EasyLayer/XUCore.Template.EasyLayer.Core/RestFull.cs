using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.NetCore;

namespace XUCore.Template.EasyLayer.Core
{
    /// <summary>
    /// 业务代码
    /// </summary>
    public enum SubCode
    {
        Success,
        Fail,
        ValidError,
        Unauthorized,
        Cancel,
        Repetition,
        Undefind,
        SoldOut,
    }

    /// <summary>
    /// 标准化返回
    /// </summary>
    public static class RestFull
    {
        private static IDictionary<SubCode, (string, string)> zhCn = new Dictionary<SubCode, (string, string)> {
            { SubCode.Success,("C000001","操作成功")},
            { SubCode.Fail,("C000002","操作失败")},
            { SubCode.ValidError,("C000003","验证失败")},
            { SubCode.Unauthorized,("C000004","操作无权限")},
            { SubCode.Cancel,("C000005","操作取消")},
            { SubCode.Repetition,("C000006","数据重复")},
            { SubCode.Undefind,("C000007","数据不存在")},
            { SubCode.SoldOut,("C000008","数据已下架")},
        };

        public static (string SubCode, string Message) GetMessage(SubCode subCode)
        {
            var item = zhCn[subCode];

            return (item.Item1, item.Item2);
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result<T> Success<T>(SubCode subCode = SubCode.Success, string message = "", T data = default)
        {
            var msg = GetMessage(subCode);

            return new Result<T>(StateCode.Success, msg.SubCode, message.IsEmpty() ? msg.Message : message, data)
            {
                ElapsedTime = -1,
                OperationTime = DateTime.Now
            };
        }
        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subCode"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Result<T> Fail<T>(SubCode subCode = SubCode.Fail, string message = "", T data = default)
        {
            var msg = GetMessage(subCode);

            return new Result<T>(StateCode.Fail, msg.SubCode, message.IsEmpty() ? msg.Message : message, data)
            {
                ElapsedTime = -1,
                OperationTime = DateTime.Now
            };
        }
    }
}
