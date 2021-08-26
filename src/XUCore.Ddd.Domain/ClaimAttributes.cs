using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XUCore.Ddd.Domain
{
    /// <summary>
    /// Claim属性
    /// </summary>
    public class ClaimAttributes
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public const string UserId = "id";

        /// <summary>
        /// 认证授权用户Id
        /// </summary>
        public const string IdentityServerUserId = "sub";

        /// <summary>
        /// 用户名
        /// </summary>
        public const string UserName = "na";

        /// <summary>
        /// 姓名
        /// </summary>
        public const string UserNickName = "nn";

        /// <summary>
        /// 用户登录token
        /// </summary>
        public const string UserToken = "tk";

        /// <summary>
        /// 刷新有效期
        /// </summary>
        public const string RefreshExpires = "re";

        /// <summary>
        /// 租户Id
        /// </summary>
        public const string TenantId = "ti";

        /// <summary>
        /// 租户类型
        /// </summary>
        public const string TenantType = "tt";

        /// <summary>
        /// 数据隔离
        /// </summary>
        public const string DataIsolationType = "dit";
    }
}
