using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    /// <summary>
    /// 更新数据状态
    /// </summary>
    public class MenuUpdateStatusCommand : CommandIds<int, string>
    {
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }

        public class Validator : CommandIdsValidator<MenuUpdateStatusCommand, int, string>
        {
            public Validator()
            {
                AddIdsValidator();

                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<MenuUpdateStatusCommand, int>
        {
            private readonly IDefaultDbRepository db;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }


            public override async Task<int> Handle(MenuUpdateStatusCommand request, CancellationToken cancellationToken)
            {
                switch (request.Status)
                {
                    case Status.Show:
                        return await db.UpdateAsync<MenuEntity>(c => request.Ids.Contains(c.Id), c => new MenuEntity { Status = Status.Show, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case Status.SoldOut:
                        return await db.UpdateAsync<MenuEntity>(c => request.Ids.Contains(c.Id), c => new MenuEntity { Status = Status.SoldOut, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case Status.Trash:
                        return await db.UpdateAsync<MenuEntity>(c => request.Ids.Contains(c.Id), c => new MenuEntity { Status = Status.Trash, DeletedAt = DateTime.Now, DeletedAtUserId = LoginInfo.UserId });
                    default:
                        return 0;
                }
            }
        }
    }
}
