using FluentValidation;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.Domain.Sys.AdminMenu
{
    /// <summary>
    /// 更新数据状态
    /// </summary>
    public class AdminMenuUpdateStatusCommand : CommandIds<int, long>
    {
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandIdsValidator<AdminMenuUpdateStatusCommand, int, long>
        {
            public Validator()
            {
                AddIdsValidator();

                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<AdminMenuUpdateStatusCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminMenuUpdateStatusCommand request, CancellationToken cancellationToken)
            {
                switch (request.Status)
                {
                    case Status.Show:
                        return await db.UpdateAsync<AdminMenuEntity>(c => request.Ids.Contains(c.Id), c => new AdminMenuEntity { Status = Status.Show, Updated_At = DateTime.Now });
                    case Status.SoldOut:
                        return await db.UpdateAsync<AdminMenuEntity>(c => request.Ids.Contains(c.Id), c => new AdminMenuEntity { Status = Status.SoldOut, Updated_At = DateTime.Now });
                    case Status.Trash:
                        return await db.UpdateAsync<AdminMenuEntity>(c => request.Ids.Contains(c.Id), c => new AdminMenuEntity { Status = Status.Trash, Deleted_At = DateTime.Now });
                    default:
                        return 0;
                }
            }
        }
    }
}
