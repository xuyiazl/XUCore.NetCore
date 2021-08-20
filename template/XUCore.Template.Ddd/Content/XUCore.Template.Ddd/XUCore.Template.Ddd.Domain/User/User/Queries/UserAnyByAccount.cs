using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Template.Ddd.Domain.Core.Entities.User;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 查询账号是否存在
    /// </summary>
    public class UserAnyByAccount : Command<bool>
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
        public string NotId { get; set; }

        public class Validator : CommandValidator<UserAnyByAccount>
        {
            public Validator()
            {
                RuleFor(x => x.AccountMode).NotEmpty().WithName("账号类型");
                RuleFor(x => x.Account).NotEmpty().WithName("账号");
            }
        }

        public class Handler : CommandHandler<UserAnyByAccount, bool>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<bool> Handle(UserAnyByAccount request, CancellationToken cancellationToken)
            {
                if (request.NotId.NotEmpty())
                {
                    switch (request.AccountMode)
                    {
                        case AccountMode.UserName:
                            return await db.AnyAsync<UserEntity>(c => c.Id != request.NotId && c.UserName == request.Account, cancellationToken);
                        case AccountMode.Mobile:
                            return await db.AnyAsync<UserEntity>(c => c.Id != request.NotId && c.Mobile == request.Account, cancellationToken);
                    }
                }
                else
                {
                    switch (request.AccountMode)
                    {
                        case AccountMode.UserName:
                            return await db.AnyAsync<UserEntity>(c => c.UserName == request.Account, cancellationToken);
                        case AccountMode.Mobile:
                            return await db.AnyAsync<UserEntity>(c => c.Mobile == request.Account, cancellationToken);
                    }
                }

                return false;
            }
        }
    }
}
