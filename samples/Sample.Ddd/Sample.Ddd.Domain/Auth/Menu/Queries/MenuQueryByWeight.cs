using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using Sample.Ddd.Domain.Core;

namespace Sample.Ddd.Domain.Auth.Menu
{
    /// <summary>
    /// 查询导航列表命令
    /// </summary>
    public class MenuQueryByWeight : Command<IList<MenuDto>>
    {
        /// <summary>
        /// 是否是导航
        /// </summary>
        [Required]
        public bool IsMenu { get; set; }

        public class Validator : CommandValidator<MenuQueryByWeight>
        {
            public Validator()
            {

            }
        }

        public class Handler : CommandHandler<MenuQueryByWeight, IList<MenuDto>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<MenuDto>> Handle(MenuQueryByWeight request, CancellationToken cancellationToken)
            {
                var res = await db.Context.Menu
                    .Where(c => c.IsMenu == request.IsMenu)
                    .OrderByDescending(c => c.Weight)
                    .ProjectTo<MenuDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
