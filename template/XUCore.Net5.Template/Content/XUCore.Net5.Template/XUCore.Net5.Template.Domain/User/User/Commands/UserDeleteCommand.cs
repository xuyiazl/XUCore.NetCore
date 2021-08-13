using AutoMapper;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Auth;
using XUCore.Net5.Template.Domain.Core.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;
using XUCore.NetCore.Data;

namespace XUCore.Net5.Template.Domain.User.User
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
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [UnitOfWork(typeof(INigelDbContext))]
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
