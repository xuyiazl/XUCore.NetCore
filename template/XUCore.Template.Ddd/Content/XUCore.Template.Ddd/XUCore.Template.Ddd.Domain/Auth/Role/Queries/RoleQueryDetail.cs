using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;

namespace XUCore.Template.Ddd.Domain.Auth.Role
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
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<RoleDto> Handle(RoleQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.GetByIdAsync<RoleEntity, RoleDto>(request.Id, cancellationToken);

                return res;
            }
        }
    }
}