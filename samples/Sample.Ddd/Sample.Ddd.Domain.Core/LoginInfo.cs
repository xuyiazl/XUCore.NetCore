using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Helpers;

namespace Sample.Ddd.Domain.Core
{
    /// <summary>
    /// 创建/修改/删除获取用户id记录的临时解决方案
    /// </summary>
    public static class LoginInfo
    {
        private const string userId = "__user_id__";
        public static string UserId => Web.HttpContext.User.Identity.GetValue<string>(userId);
    }
}
