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

namespace XUCore.Template.Ddd.Domain.Auth.Menu
{
    /// <summary>
    /// 创建导航命令
    /// </summary>
    public class MenuCreateCommand : Command<int>, IMapFrom<MenuEntity>
    {
        /// <summary>
        /// 导航父级id
        /// </summary>
        public string FatherId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [Required]
        public string Url { get; set; }
        /// <summary>
        /// 唯一代码（权限使用）
        /// </summary>
        [Required]
        public string OnlyCode { get; set; }
        /// <summary>
        /// 是否是导航
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 是否是快捷导航
        /// </summary>
        public bool IsExpress { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [Required]
        public Status Status { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<MenuCreateCommand, MenuEntity>()
                .ForMember(c => c.Url, c => c.MapFrom(s => s.Url.IsEmpty() ? "#" : s.Url))
            ;

        public class Validator : CommandValidator<MenuCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("菜单名");
                RuleFor(x => x.Url).NotEmpty().MaximumLength(50).WithName("Url");
                RuleFor(x => x.OnlyCode).NotEmpty().MaximumLength(50).WithName("唯一代码");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }

        public class Handler : CommandHandler<MenuCreateCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [UnitOfWork(DbType = typeof(IDefaultDbContext))]
            public override async Task<int> Handle(MenuCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = mapper.Map<MenuCreateCommand, MenuEntity>(request);

                var res = db.Add(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new MenuCreateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                else
                    return res;
            }
        }
    }
}
