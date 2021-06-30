using AutoMapper;
using XUCore.Net5.Template.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    /// <summary>
    /// 查询账号是否存在
    /// </summary>
    public class AdminUserAnyByAccount : Command<bool>
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
        /// <summary>
        /// 排除id
        /// </summary>
        public long NotId { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<AdminUserAnyByAccount>
        {
            public Validator()
            {
                RuleFor(x => x.AccountMode).NotEmpty().WithName("账号类型");
                RuleFor(x => x.Account).NotEmpty().WithName("账号");
            }
        }

        public class Handler : CommandHandler<AdminUserAnyByAccount, bool>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<bool> Handle(AdminUserAnyByAccount request, CancellationToken cancellationToken)
            {
                if (request.NotId > 0)
                {
                    switch (request.AccountMode)
                    {
                        case AccountMode.UserName:
                            return await db.Context.AdminUser.AnyAsync(c => c.Id != request.NotId && c.UserName == request.Account, cancellationToken);
                        case AccountMode.Mobile:
                            return await db.Context.AdminUser.AnyAsync(c => c.Id != request.NotId && c.Mobile == request.Account, cancellationToken);
                    }
                }
                else
                {
                    switch (request.AccountMode)
                    {
                        case AccountMode.UserName:
                            return await db.Context.AdminUser.AnyAsync(c => c.UserName == request.Account, cancellationToken);
                        case AccountMode.Mobile:
                            return await db.Context.AdminUser.AnyAsync(c => c.Mobile == request.Account, cancellationToken);
                    }
                }

                return false;
            }
        }
    }
}
