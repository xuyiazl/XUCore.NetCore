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

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    /// <summary>
    /// 查询导航记录命令
    /// </summary>
    public class MenuQueryDetail : CommandId<MenuDto, string>
    {
        public class Validator : CommandIdValidator<MenuQueryDetail, MenuDto, string>
        {
            public Validator()
            {
                AddIdValidator();
            }
        }

        public class Handler : CommandHandler<MenuQueryDetail, MenuDto>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<MenuDto> Handle(MenuQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.GetByIdAsync<MenuEntity, MenuDto>(request.Id, cancellationToken);

                return res;
            }
        }
    }
}
