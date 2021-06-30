using AutoMapper;
using AutoMapper.QueryableExtensions;
using XUCore.Net5.Template.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    /// <summary>
    /// 查询一条记录命令
    /// </summary>
    public class AdminUserQueryDetail : CommandId<AdminUserDto, long>
    {
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandIdValidator<AdminUserQueryDetail, AdminUserDto, long>
        {
            public Validator()
            {
                AddIdValidator();
            }
        }

        public class Handler : CommandHandler<AdminUserQueryDetail, AdminUserDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<AdminUserDto> Handle(AdminUserQueryDetail request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminUser
                    .Where(c => c.Id == request.Id)
                    .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                return res;
            }
        }
    }
}
