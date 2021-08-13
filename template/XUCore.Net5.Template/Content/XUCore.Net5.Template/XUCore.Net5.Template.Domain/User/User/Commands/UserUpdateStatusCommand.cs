using AutoMapper;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.User;
using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.Domain.User.User
{
    /// <summary>
    /// 更新数据状态
    /// </summary>
    public class UserUpdateStatusCommand : CommandIds<int, string>
    {
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }

        public class Validator : CommandIdsValidator<UserUpdateStatusCommand, int, string>
        {
            public Validator()
            {
                AddIdsValidator();

                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<UserUpdateStatusCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }


            public override async Task<int> Handle(UserUpdateStatusCommand request, CancellationToken cancellationToken)
            {
                switch (request.Status)
                {
                    case Status.Show:
                        return await db.UpdateAsync<UserEntity>(c => request.Ids.Contains(c.Id), c => new UserEntity { Status = Status.Show, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case Status.SoldOut:
                        return await db.UpdateAsync<UserEntity>(c => request.Ids.Contains(c.Id), c => new UserEntity { Status = Status.SoldOut, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case Status.Trash:
                        return await db.UpdateAsync<UserEntity>(c => request.Ids.Contains(c.Id), c => new UserEntity { Status = Status.Trash, DeletedAt = DateTime.Now, DeletedAtUserId = LoginInfo.UserId });
                    default:
                        return 0;
                }
            }
        }
    }
}
