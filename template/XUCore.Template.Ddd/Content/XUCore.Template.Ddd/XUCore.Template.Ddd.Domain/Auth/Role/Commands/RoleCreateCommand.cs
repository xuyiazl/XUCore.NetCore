using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.NetCore.Data;

namespace XUCore.Template.Ddd.Domain.Auth.Role
{
    /// <summary>
    /// 创建角色命令
    /// </summary>
    public class RoleCreateCommand : Command<int>, IMapFrom<RoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 导航关联id集合
        /// </summary>
        public string[] MenuIds { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [Required]
        public Status Status { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<RoleCreateCommand, RoleEntity>()
            ;

        public class Validator : CommandValidator<RoleCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<RoleCreateCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [UnitOfWork(DbType = typeof(IDefaultDbContext))]
            public override async Task<int> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = mapper.Map<RoleCreateCommand, RoleEntity>(request);

                //保存关联导航
                if (request.MenuIds != null && request.MenuIds.Length > 0)
                {
                    entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
                    {
                        RoleId = entity.Id,
                        MenuId = key
                    });
                }

                var res = db.Add(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new RoleCreateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                else
                    return res;
            }
        }
    }
}
