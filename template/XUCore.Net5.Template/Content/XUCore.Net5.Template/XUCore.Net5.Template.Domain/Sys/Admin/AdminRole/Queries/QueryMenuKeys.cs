using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Net5.Template.Domain.Core;

namespace XUCore.Net5.Template.Domain.Sys.AdminRole
{
    /// <summary>
    /// 查询角色关联的导航id集合
    /// </summary>
    public class AdminRoleQueryMenuKeys : Command<IList<long>>
    {
        /// <summary>
        /// 角色id
        /// </summary>
        [Required]
        public long RoleId { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandValidator<AdminRoleQueryMenuKeys>
        {
            public Validator()
            {
            }
        }


        public class Handler : CommandHandler<AdminRoleQueryMenuKeys, IList<long>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<long>> Handle(AdminRoleQueryMenuKeys request, CancellationToken cancellationToken)
            {
                return await db.Context.AdminAuthRoleMenus
                    .Where(c => c.RoleId == request.RoleId)
                    .OrderBy(c => c.MenuId)
                    .Select(c => c.MenuId)
                    .ToListAsync();
            }
        }
    }
}
