using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Template.FreeSql.Core.Auth
{
    /// <summary>
    /// 用户信息接口
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// 主键
        /// </summary>
        long Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 昵称
        /// </summary>
        string NickName { get; }
        /// <summary>
        /// 将登录的用户写入内存作为标记，处理强制重新获取jwt，模拟退出登录（可以使用redis）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        void SetLoginToken(long userId, string token);
        /// <summary>
        /// 删除登录标记，模拟退出
        /// </summary>
        void RemoveLoginToken();
        /// <summary>
        /// 验证token是否一致
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        bool VaildLoginToken(string token);
    }
}
