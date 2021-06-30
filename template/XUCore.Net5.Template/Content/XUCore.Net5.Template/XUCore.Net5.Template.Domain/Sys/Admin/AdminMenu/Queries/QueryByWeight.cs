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
using XUCore.Net5.Template.Domain.Core;

namespace XUCore.Net5.Template.Domain.Sys.AdminMenu
{
    /// <summary>
    /// 查询导航列表命令
    /// </summary>
    public class AdminMenuQueryByWeight : Command<IList<AdminMenuDto>>
    {
        /// <summary>
        /// 是否是导航
        /// </summary>
        [Required]
        public bool IsMenu { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<AdminMenuQueryByWeight>
        {
            public Validator()
            {

            }
        }

        public class Handler : CommandHandler<AdminMenuQueryByWeight, IList<AdminMenuDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<AdminMenuDto>> Handle(AdminMenuQueryByWeight request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthMenus
                    .Where(c => c.IsMenu == request.IsMenu)
                    .OrderByDescending(c => c.Weight)
                    .ProjectTo<AdminMenuDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
