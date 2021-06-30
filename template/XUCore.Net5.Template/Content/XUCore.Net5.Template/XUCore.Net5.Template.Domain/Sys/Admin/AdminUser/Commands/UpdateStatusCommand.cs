using AutoMapper;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using FluentValidation;
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
    /// 更新数据状态
    /// </summary>
    public class AdminUserUpdateStatusCommand : CommandIds<int, long>
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
        public class Validator : CommandIdsValidator<AdminUserUpdateStatusCommand, int, long>
        {
            public Validator()
            {
                AddIdsValidator();

                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<AdminUserUpdateStatusCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserUpdateStatusCommand request, CancellationToken cancellationToken)
            {
                switch (request.Status)
                {
                    case Status.Show:
                        return await db.UpdateAsync<AdminUserEntity>(c => request.Ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.Show, Updated_At = DateTime.Now });
                    case Status.SoldOut:
                        return await db.UpdateAsync<AdminUserEntity>(c => request.Ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.SoldOut, Updated_At = DateTime.Now });
                    case Status.Trash:
                        return await db.UpdateAsync<AdminUserEntity>(c => request.Ids.Contains(c.Id), c => new AdminUserEntity { Status = Status.Trash, Deleted_At = DateTime.Now });
                    default:
                        return 0;
                }
            }
        }
    }
}
