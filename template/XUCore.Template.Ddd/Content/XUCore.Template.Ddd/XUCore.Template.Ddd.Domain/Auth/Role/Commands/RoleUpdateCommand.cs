using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
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
    /// 角色修改命令
    /// </summary>
    public class RoleUpdateCommand : CommandId<int, string>, IMapFrom<RoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 导航id集合
        /// </summary>
        public string[] MenuIds { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [Required]
        public Status Status { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<RoleUpdateCommand, RoleEntity>()
            ;

        public class Validator : CommandIdValidator<RoleUpdateCommand, int, string>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<RoleUpdateCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [UnitOfWork(DbType = typeof(IDefaultDbContext))]
            public override async Task<int> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.GetByIdAsync<RoleEntity>(request.Id, cancellationToken);

                if (entity == null)
                    return 0;

                entity = mapper.Map(request, entity);

                //先清空导航集合，确保没有冗余信息
                await db.DeleteAsync<RoleMenuEntity>(c => c.RoleId == entity.Id);

                //保存关联导航
                if (request.MenuIds != null && request.MenuIds.Length > 0)
                {
                    entity.RoleMenus = Array.ConvertAll(request.MenuIds, key => new RoleMenuEntity
                    {
                        RoleId = entity.Id,
                        MenuId = key
                    });
                }

                var res = db.Update(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new RoleUpdateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                return res;
            }
        }
    }
}
