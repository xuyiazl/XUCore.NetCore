using AutoMapper;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Common.Mappings;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.Domain.Sys.AdminRole
{
    /// <summary>
    /// 创建角色命令
    /// </summary>
    public class AdminRoleCreateCommand : Command<int>, IMapFrom<AdminRoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 导航关联id集合
        /// </summary>
        public long[] MenuIds { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [Required]
        public Status Status { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminRoleCreateCommand, AdminRoleEntity>()
                .ForMember(c => c.Created_At, c => c.MapFrom(s => DateTime.Now))
            ;
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandValidator<AdminRoleCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<AdminRoleCreateCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminRoleCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = mapper.Map<AdminRoleCreateCommand, AdminRoleEntity>(request);

                //保存关联导航
                if (request.MenuIds != null && request.MenuIds.Length > 0)
                {
                    entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new AdminRoleMenuEntity
                    {
                        RoleId = entity.Id,
                        MenuId = key
                    });
                }

                var res = db.Add(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new CreateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                else
                    return res;
            }
        }
    }
}
