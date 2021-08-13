using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Core;

namespace XUCore.Net5.Template.Domain.Auth.Role
{
    /// <summary>
    /// 查询一条角色记录命令
    /// </summary>
    public class RoleQueryDetail : CommandId<RoleDto, string>
    {
        public class Validator : CommandIdValidator<RoleQueryDetail, RoleDto, string>
        {
            public Validator()
            {
                AddIdValidator();
            }
        }

        public class Handler : CommandHandler<RoleQueryDetail, RoleDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<RoleDto> Handle(RoleQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.Context.Role
                    .Where(c => c.Id == request.Id)
                    .ProjectTo<RoleDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return res;
            }
        }
    }
}