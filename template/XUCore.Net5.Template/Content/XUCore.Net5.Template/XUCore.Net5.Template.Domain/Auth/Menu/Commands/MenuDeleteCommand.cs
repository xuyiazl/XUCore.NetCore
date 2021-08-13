using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Auth;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Data;

namespace XUCore.Net5.Template.Domain.Auth.Menu
{
    /// <summary>
    /// 删除导航命令
    /// </summary>
    public class MenuDeleteCommand : CommandIds<int, string>
    {
        public class Validator : CommandIdsValidator<MenuDeleteCommand, int, string>
        {
            public Validator()
            {
                AddIdsValidator();
            }
        }

        public class Handler : CommandHandler<MenuDeleteCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [UnitOfWork(typeof(INigelDbContext))]
            public override async Task<int> Handle(MenuDeleteCommand request, CancellationToken cancellationToken)
            {
                var res = await db.DeleteAsync<MenuEntity>(c => request.Ids.Contains(c.Id));

                if (res > 0)
                {
                    await db.DeleteAsync<RoleMenuEntity>(c => request.Ids.Contains(c.MenuId));
                }

                return res;
            }
        }
    }
}
