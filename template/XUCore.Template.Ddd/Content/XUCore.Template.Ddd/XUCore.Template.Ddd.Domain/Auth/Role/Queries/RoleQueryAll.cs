using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Template.Ddd.Domain.Core;

namespace XUCore.Template.Ddd.Domain.Auth.Role
{
    /// <summary>
    /// 查询所有角色命令
    /// </summary>
    public class RoleQueryAll : Command<IList<RoleDto>>
    {
        public class Validator : CommandValidator<RoleQueryAll>
        {
            public Validator()
            {

            }
        }

        public class Handler : CommandHandler<RoleQueryAll, IList<RoleDto>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<RoleDto>> Handle(RoleQueryAll request, CancellationToken cancellationToken)
            {
                var res = await db.Context.Role
                    .Where(c => c.Status == Status.Show)
                    .ProjectTo<RoleDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
