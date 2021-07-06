﻿using FluentValidation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using Sample.Ddd.Domain.Core;
using XUCore.NetCore.AspectCore.Cache;

namespace Sample.Ddd.Domain.Sys.Permission
{
    public class PermissionQueryData : Command<PermissionViewModel>
    {
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<PermissionQueryData>
        {
            public Validator()
            {
            }
        }

        public class Handler : CommandHandler<PermissionQueryData, PermissionViewModel>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db)
            {
                this.db = db;
            }

            [CacheMethod(Key = CacheKey.AuthTables, Seconds = CacheTime.Day1)]
            public override async Task<PermissionViewModel> Handle(PermissionQueryData request, CancellationToken cancellationToken)
            {
                await Task.CompletedTask;

                return new PermissionViewModel
                {
                    Menus = db.Context.AdminAuthMenus.Where(c => c.Status == Status.Show).ToList(),
                    RoleMenus = db.Context.AdminAuthRoleMenus.ToList(),
                    UserRoles = db.Context.AdminAuthUserRole.ToList()
                };
            }
        }
    }
}
