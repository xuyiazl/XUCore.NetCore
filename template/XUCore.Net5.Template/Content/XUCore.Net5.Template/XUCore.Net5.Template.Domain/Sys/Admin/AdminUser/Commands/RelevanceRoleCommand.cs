using AutoMapper;
using FluentValidation;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;
using System.ComponentModel.DataAnnotations;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    /// <summary>
    /// 关联角色命令
    /// </summary>
    public class AdminUserRelevanceRoleCommand : Command<int>
    {
        /// <summary>
        /// 管理员id
        /// </summary>
        [Required]
        public long AdminId { get; set; }
        /// <summary>
        /// 角色id集合
        /// </summary>
        public long[] RoleIds { get; set; }


        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<AdminUserRelevanceRoleCommand>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
            }
        }

        public class Handler : CommandHandler<AdminUserRelevanceRoleCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserRelevanceRoleCommand request, CancellationToken cancellationToken)
            {
                //先清空用户的角色，确保没有冗余的数据
                await db.DeleteAsync<AdminUserRoleEntity>(c => c.AdminId == request.AdminId);

                var userRoles = Array.ConvertAll(request.RoleIds, roleid => new AdminUserRoleEntity
                {
                    RoleId = roleid,
                    AdminId = request.AdminId
                });

                //添加角色
                if (userRoles.Length > 0)
                    return await db.AddAsync(userRoles);

                return 1;
            }
        }
    }
}
