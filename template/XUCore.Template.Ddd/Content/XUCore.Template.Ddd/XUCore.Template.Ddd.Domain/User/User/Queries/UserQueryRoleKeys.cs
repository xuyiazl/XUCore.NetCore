using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 查询关联角色id集合
    /// </summary>
    public class UserQueryRoleKeys : Command<IList<string>>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public string UserId { get; set; }

        public class Validator : CommandValidator<UserQueryRoleKeys>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("UserId");
            }
        }

        public class Handler : CommandHandler<UserQueryRoleKeys, IList<string>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<string>> Handle(UserQueryRoleKeys request, CancellationToken cancellationToken)
            {
                return await db.Context.UserRole
                    .Where(c => c.UserId == request.UserId)
                    .Select(c => c.RoleId)
                    .ToListAsync();
            }
        }
    }
}
