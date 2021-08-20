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
using XUCore.Extensions;
using XUCore.Template.Ddd.Domain.Core.Entities.User;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 账号查询
    /// </summary>
    public class UserQueryByAccount : Command<UserDto>
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

        public class Validator : CommandValidator<UserQueryByAccount>
        {
            public Validator()
            {
                RuleFor(x => x.AccountMode).NotEmpty().WithName("账号类型");
                RuleFor(x => x.Account).NotEmpty().WithName("账号");
            }
        }

        public class Handler : CommandHandler<UserQueryByAccount, UserDto>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<UserDto> Handle(UserQueryByAccount request, CancellationToken cancellationToken)
            {
                switch (request.AccountMode)
                {
                    case AccountMode.UserName:

                        return await db.GetFirstAsync<UserEntity, UserDto>(c => c.UserName.Equals(request.Account), cancellationToken: cancellationToken);

                    case AccountMode.Mobile:

                        return await db.GetFirstAsync<UserEntity, UserDto>(c => c.Mobile.Equals(request.Account), cancellationToken: cancellationToken);
                }

                return null;
            }
        }
    }
}
