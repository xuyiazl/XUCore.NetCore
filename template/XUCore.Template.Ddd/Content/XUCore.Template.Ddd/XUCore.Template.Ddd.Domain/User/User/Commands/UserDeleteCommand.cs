using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using XUCore.Template.Ddd.Domain.Core.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Data;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 删除导航命令
    /// </summary>
    public class UserDeleteCommand : CommandIds<int, string>
    {
        public class Validator : CommandIdsValidator<UserDeleteCommand, int, string>
        {
            public Validator()
            {
                AddIdsValidator();
            }
        }

        public class Handler : CommandHandler<UserDeleteCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [UnitOfWork(DbType = typeof(IDefaultDbContext))]
            public override async Task<int> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
            {
                var res = await db.DeleteAsync<UserEntity>(c => request.Ids.Contains(c.Id));

                if (res > 0)
                {
                    //删除登录记录
                    await db.DeleteAsync<UserLoginRecordEntity>(c => request.Ids.Contains(c.UserId));
                    //删除关联的角色
                    await db.DeleteAsync<UserRoleEntity>(c => request.Ids.Contains(c.UserId));
                }

                return res;
            }
        }
    }
}
