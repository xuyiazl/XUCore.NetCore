using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Net5.Template.Domain.Core
{
    public static class SubCodeMessage
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

        public static (string, string) Message(SubCode subCode)
        {
            return zhCn[subCode];
        }
    }

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
}
