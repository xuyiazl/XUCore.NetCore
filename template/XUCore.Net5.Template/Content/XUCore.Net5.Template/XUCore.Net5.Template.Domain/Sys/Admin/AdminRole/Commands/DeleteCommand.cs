using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.Domain.Sys.AdminRole
{
    /// <summary>
    /// 删除导航命令
    /// </summary>
    public class AdminRoleDeleteCommand : CommandIds<int, long>
    {
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandIdsValidator<AdminRoleDeleteCommand, int, long>
        {
            public Validator()
            {
                AddIdsValidator();
            }
        }

        public class Handler : CommandHandler<AdminRoleDeleteCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminRoleDeleteCommand request, CancellationToken cancellationToken)
            {
                var res = await db.DeleteAsync<AdminRoleEntity>(c => request.Ids.Contains(c.Id));

                if (res > 0)
                {
                    //删除关联的导航
                    await db.DeleteAsync<AdminRoleMenuEntity>(c => request.Ids.Contains(c.RoleId));
                    //删除用户关联的角色
                    await db.DeleteAsync<AdminUserRoleEntity>(c => request.Ids.Contains(c.RoleId));
                }

                return res;
            }
        }
    }
}
