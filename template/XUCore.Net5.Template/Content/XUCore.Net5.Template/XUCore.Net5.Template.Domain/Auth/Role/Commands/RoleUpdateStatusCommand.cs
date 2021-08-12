using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Auth;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.Domain.Auth.Role
{
    /// <summary>
    /// 更新数据状态
    /// </summary>
    public class RoleUpdateStatusCommand : CommandIds<int, string>
    {
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }

        public class Validator : CommandIdsValidator<RoleUpdateStatusCommand, int, string>
        {
            public Validator()
            {
                AddIdsValidator();

                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<RoleUpdateStatusCommand, int>
        {
            private readonly ITaxDbRepository db;

            public Handler(ITaxDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }


            public override async Task<int> Handle(RoleUpdateStatusCommand request, CancellationToken cancellationToken)
            {
                switch (request.Status)
                {
                    case Status.Show:
                        return await db.UpdateAsync<RoleEntity>(c => request.Ids.Contains(c.Id), c => new RoleEntity { Status = Status.Show, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case Status.SoldOut:
                        return await db.UpdateAsync<RoleEntity>(c => request.Ids.Contains(c.Id), c => new RoleEntity { Status = Status.SoldOut, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case Status.Trash:
                        return await db.UpdateAsync<RoleEntity>(c => request.Ids.Contains(c.Id), c => new RoleEntity { Status = Status.Trash, DeletedAt = DateTime.Now, DeletedAtUserId = LoginInfo.UserId });
                    default:
                        return 0;
                }
            }
        }
    }
}
