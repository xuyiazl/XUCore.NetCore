using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    /// <summary>
    /// 删除导航命令
    /// </summary>
    public class AdminUserDeleteCommand : CommandIds<int, long>
    {
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandIdsValidator<AdminUserDeleteCommand, int, long>
        {
            public Validator()
            {
                AddIdsValidator();
            }
        }

        public class Handler : CommandHandler<AdminUserDeleteCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserDeleteCommand request, CancellationToken cancellationToken)
            {
                var res = await db.DeleteAsync<AdminUserEntity>(c => request.Ids.Contains(c.Id));

                if (res > 0)
                {
                    //删除登录记录
                    await db.DeleteAsync<LoginRecordEntity>(c => request.Ids.Contains(c.AdminId));
                    //删除关联的角色
                    await db.DeleteAsync<AdminUserRoleEntity>(c => request.Ids.Contains(c.AdminId));
                }

                return res;
            }
        }
    }
}
