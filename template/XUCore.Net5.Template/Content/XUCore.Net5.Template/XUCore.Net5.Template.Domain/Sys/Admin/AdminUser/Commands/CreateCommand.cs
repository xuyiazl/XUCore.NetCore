using AutoMapper;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Common.Mappings;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    /// <summary>
    /// 创建管理员命令
    /// </summary>
    public class AdminUserCreateCommand : Command<int>, IMapFrom<AdminUserEntity>
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required]
        public string UserName { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required]
        public string Mobile { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 角色id集合
        /// </summary>
        public long[] Roles { get; set; }


        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminUserCreateCommand, AdminUserEntity>()
                .ForMember(c => c.Password, c => c.MapFrom(s => Encrypt.Md5By32(s.Password)))
                .ForMember(c => c.LoginCount, c => c.MapFrom(s => 0))
                .ForMember(c => c.LoginLastIp, c => c.MapFrom(s => ""))
                .ForMember(c => c.Picture, c => c.MapFrom(s => ""))
                .ForMember(c => c.Status, c => c.MapFrom(s => Status.Show))
                .ForMember(c => c.Created_At, c => c.MapFrom(s => DateTime.Now))
            ;

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<AdminUserCreateCommand>
        {
            public Validator()
            {
                var bus = Web.GetService<IMediatorHandler>();

                RuleFor(x => x.UserName).NotEmpty().MaximumLength(20).WithName("账号")
                    .MustAsync(async (account, cancel) =>
                    {
                        var res = await bus.SendCommand(new AdminUserAnyByAccount() { AccountMode = AccountMode.UserName, Account = account, NotId = 0 }, cancel);

                        return !res;
                    })
                    .WithMessage(c => $"该账号已存在");

                RuleFor(x => x.Mobile).NotEmpty().MaximumLength(11).WithName("手机号码")
                    .MustAsync(async (account, cancel) =>
                    {
                        var res = await bus.SendCommand(new AdminUserAnyByAccount() { AccountMode = AccountMode.Mobile, Account = account, NotId = 0 }, cancel);

                        return !res;
                    })
                    .WithMessage(c => $"该手机号码已存在");

                RuleFor(x => x.Password).NotEmpty().MaximumLength(50).WithName("密码");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("名字");
                RuleFor(x => x.Company).NotEmpty().MaximumLength(30).WithName("公司");
                RuleFor(x => x.Location).NotEmpty().MaximumLength(30).WithName("位置");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("名字");
            }
        }

        public class Handler : CommandHandler<AdminUserCreateCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserCreateCommand request, CancellationToken cancellationToken)
            {
                //await bus.PublishEvent(new DomainNotification("", "开始注册...."), cancellationToken);

                var entity = mapper.Map<AdminUserCreateCommand, AdminUserEntity>(request);

                //角色操作
                if (request.Roles != null && request.Roles.Length > 0)
                {
                    //转换角色对象 并写入
                    entity.UserRoles = Array.ConvertAll(request.Roles, roleid => new AdminUserRoleEntity
                    {
                        RoleId = roleid,
                        AdminId = entity.Id
                    });
                }

                var res = db.Add(entity);

                //await bus.PublishEvent(new DomainNotification("", "结束注册...."), cancellationToken);

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
