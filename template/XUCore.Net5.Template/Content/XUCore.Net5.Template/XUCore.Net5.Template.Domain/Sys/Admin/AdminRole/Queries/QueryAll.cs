using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Core;

namespace XUCore.Net5.Template.Domain.Sys.AdminRole
{
    /// <summary>
    /// 查询所有角色命令
    /// </summary>
    public class AdminRoleQueryAll : Command<IList<AdminRoleDto>>
    {
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<AdminRoleQueryAll>
        {
            public Validator()
            {

            }
        }

        public class Handler : CommandHandler<AdminRoleQueryAll, IList<AdminRoleDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<AdminRoleDto>> Handle(AdminRoleQueryAll request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthRole
                    .Where(c => c.Status == Status.Show)
                    .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
