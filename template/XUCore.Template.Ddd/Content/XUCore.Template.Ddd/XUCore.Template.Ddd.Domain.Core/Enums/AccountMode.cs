using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.Ddd.Domain.Core
{
    public enum AccountMode
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        UserName = 1,
        /// <summary>
        /// 手机号码
        /// </summary>
        [Description("手机号码")]
        Mobile = 2
    }
}
