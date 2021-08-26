using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Data;

namespace XUCore.Template.Ddd.Domain.Auth.Role
{
    /// <summary>
    /// 删除导航命令
    /// </summary>
    public class RoleDeleteCommand : CommandIds<int, string>
    {
        public class Validator : CommandIdsValidator<RoleDeleteCommand, int, string>
        {
            public Validator()
            {
                AddIdsValidator();
            }
        }

        public class Handler : CommandHandler<RoleDeleteCommand, int>
        {
            private readonly IDefaultDbRepository db;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [UnitOfWork(DbType = typeof(IDefaultDbContext))]
            public override async Task<int> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
            {
                var res = await db.DeleteAsync<RoleEntity>(c => request.Ids.Contains(c.Id));

                if (res > 0)
                {
                    //删除关联的导航
                    await db.DeleteAsync<RoleMenuEntity>(c => request.Ids.Contains(c.RoleId));
                    //删除用户关联的角色
                    await db.DeleteAsync<UserRoleEntity>(c => request.Ids.Contains(c.RoleId));
                }

                return res;
            }
        }
    }
}
