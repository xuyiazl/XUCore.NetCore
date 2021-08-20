using AutoMapper;
using AutoMapper.QueryableExtensions;
using XUCore.Template.Ddd.Domain.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Template.Ddd.Domain.Core.Entities.User;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 查询一条记录命令
    /// </summary>
    public class UserQueryDetail : CommandId<UserDto, string>
    {
        public class Validator : CommandIdValidator<UserQueryDetail, UserDto, string>
        {
            public Validator()
            {
                AddIdValidator();
            }
        }

        public class Handler : CommandHandler<UserQueryDetail, UserDto>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<UserDto> Handle(UserQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.GetByIdAsync<UserEntity, UserDto>(request.Id, cancellationToken);

                return res;
            }
        }
    }
}
