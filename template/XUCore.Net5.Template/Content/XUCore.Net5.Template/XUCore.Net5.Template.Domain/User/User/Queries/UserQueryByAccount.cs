using AutoMapper;
using AutoMapper.QueryableExtensions;
using XUCore.Net5.Template.Domain.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;

namespace XUCore.Net5.Template.Domain.User.User
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
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<UserDto> Handle(UserQueryByAccount request, CancellationToken cancellationToken)
            {
                switch (request.AccountMode)
                {
                    case AccountMode.UserName:

                        return await db.Context.User
                            .Where(c => c.UserName.Equals(request.Account))
                            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(cancellationToken);

                    case AccountMode.Mobile:

                        return await db.Context.User
                            .Where(c => c.Mobile.Equals(request.Account))
                            .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                            .FirstOrDefaultAsync(cancellationToken);
                }

                return null;
            }
        }
    }
}
