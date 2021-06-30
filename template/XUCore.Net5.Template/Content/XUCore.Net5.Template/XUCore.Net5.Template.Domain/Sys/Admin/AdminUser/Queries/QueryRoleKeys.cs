using AutoMapper;
using XUCore.Net5.Template.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    /// <summary>
    /// 查询关联角色id集合
    /// </summary>
    public class AdminUserQueryRoleKeys : Command<IList<long>>
    {
        /// <summary>
        /// 管理员id
        /// </summary>
        [Required]
        public long AdminId { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<AdminUserQueryRoleKeys>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
            }
        }

        public class Handler : CommandHandler<AdminUserQueryRoleKeys, IList<long>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<long>> Handle(AdminUserQueryRoleKeys request, CancellationToken cancellationToken)
            {
                return await db.Context.AdminAuthUserRole
                    .Where(c => c.AdminId == request.AdminId)
                    .Select(c => c.RoleId)
                    .ToListAsync();
            }
        }
    }
}
