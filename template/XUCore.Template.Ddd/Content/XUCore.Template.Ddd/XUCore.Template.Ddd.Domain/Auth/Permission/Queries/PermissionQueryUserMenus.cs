using FluentValidation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.NetCore.AspectCore.Cache;
using XUCore;
using System.Collections.Generic;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace XUCore.Template.Ddd.Domain.Auth.Permission
{
    public class PermissionQueryUserMenus : Command<IList<MenuEntity>>
    {
        public string UserId { get; set; }

        public class Validator : CommandValidator<PermissionQueryUserMenus>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("UserId");
            }
        }

        public class Handler : CommandHandler<PermissionQueryUserMenus, IList<MenuEntity>>
        {
            private readonly IDefaultDbRepository db;

            public Handler(IDefaultDbRepository db)
            {
                this.db = db;
            }

            [CacheMethod(Key = CacheKey.UserAuth, ParamterKey = "{0}", Seconds = CacheTime.Min3)]
            public override async Task<IList<MenuEntity>> Handle(PermissionQueryUserMenus request, CancellationToken cancellationToken)
            {
                var res =
                    await
                    (
                        from userRoles in db.Context.UserRole

                        join roleMenus in db.Context.RoleMenu on userRoles.RoleId equals roleMenus.RoleId

                        join menus in db.Context.Menu on roleMenus.MenuId equals menus.Id

                        where userRoles.UserId == request.UserId

                        select menus
                    )
                    .Distinct()
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
