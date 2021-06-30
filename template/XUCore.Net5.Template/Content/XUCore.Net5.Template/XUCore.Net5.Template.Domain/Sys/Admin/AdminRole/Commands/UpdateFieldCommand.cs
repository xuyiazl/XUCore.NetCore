using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.Domain.Sys.AdminRole
{
    /// <summary>
    /// 更新部分字段
    /// </summary>
    public class AdminRoleUpdateFieldCommand : CommandId<int, long>
    {
        /// <summary>
        /// 字段名
        /// </summary>
        [Required]
        public string Field { get; set; }
        /// <summary>
        /// 需要修改的值
        /// </summary>
        [Required]
        public string Value { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandIdValidator<AdminRoleUpdateFieldCommand, int, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Field).NotEmpty().WithMessage("字段名");
            }
        }

        public class Handler : CommandHandler<AdminRoleUpdateFieldCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminRoleUpdateFieldCommand request, CancellationToken cancellationToken)
            {
                switch (request.Field.ToLower())
                {
                    case "name":
                        return await db.UpdateAsync<AdminRoleEntity>(c => c.Id == request.Id, c => new AdminRoleEntity() { Name = request.Value, Updated_At = DateTime.Now });
                    default:
                        return 0;
                }
            }
        }
    }
}
