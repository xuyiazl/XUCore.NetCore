using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using Sample.Ddd.Domain.Core;

namespace Sample.Ddd.Domain.Sys.AdminUser
{
    /// <summary>
    /// 账号查询
    /// </summary>
    public class AdminUserQueryByAccount : Command<AdminUserDto>
    {
        /// <summary>
        /// 账号类型
        /// </summary>
        [Required]
        public AccountMode AccountMode { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        public string Account { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<AdminUserQueryByAccount>
        {
            public Validator()
            {
                RuleFor(x => x.AccountMode).NotEmpty().WithName("账号类型");
                RuleFor(x => x.Account).NotEmpty().WithName("账号");
            }
        }

        public class Handler : CommandHandler<AdminUserQueryByAccount, AdminUserDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<AdminUserDto> Handle(AdminUserQueryByAccount request, CancellationToken cancellationToken)
            {
                switch (request.AccountMode)
                {
                    case AccountMode.UserName:

                        return await db.Context.AdminUser
                            .Where(c => c.UserName.Equals(request.Account))
                            .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(cancellationToken);

                    case AccountMode.Mobile:

                        return await db.Context.AdminUser
                            .Where(c => c.Mobile.Equals(request.Account))
                            .ProjectTo<AdminUserDto>(mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(cancellationToken);
                }

                return null;
            }
        }
    }
}
