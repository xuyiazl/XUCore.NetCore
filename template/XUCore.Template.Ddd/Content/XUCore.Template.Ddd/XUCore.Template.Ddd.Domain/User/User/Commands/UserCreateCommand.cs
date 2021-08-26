using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using XUCore.Template.Ddd.Domain.Core.Entities.User;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;
using XUCore.NetCore.Data;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 创建用户命令
    /// </summary>
    public class UserCreateCommand : Command<int>, IMapFrom<UserEntity>
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
        public string[] Roles { get; set; }


        public void Mapping(Profile profile) =>
            profile.CreateMap<UserCreateCommand, UserEntity>()
                .ForMember(c => c.Password, c => c.MapFrom(s => Encrypt.Md5By32(s.Password)))
                .ForMember(c => c.LoginCount, c => c.MapFrom(s => 0))
                .ForMember(c => c.LoginLastIp, c => c.MapFrom(s => ""))
                .ForMember(c => c.Picture, c => c.MapFrom(s => ""))
                .ForMember(c => c.Status, c => c.MapFrom(s => Status.Show))
            ;

        public class Validator : AbstractValidator<UserCreateCommand>
        {
            public Validator()
            {
                var bus = Web.GetService<IMediatorHandler>();

                RuleFor(x => x.UserName).NotEmpty().MaximumLength(20).WithName("账号")
                    .MustAsync(async (account, cancel) =>
                    {
                        var res = await bus.SendCommand(new UserAnyByAccount() { AccountMode = AccountMode.UserName, Account = account }, cancel);

                        return !res;
                    })
                    .WithMessage(c => $"该账号已存在。");

                RuleFor(x => x.Mobile)
                    .NotEmpty()
                    .Matches("^[1][3-9]\\d{9}$").WithMessage("手机号格式不正确。")
                    .MustAsync(async (account, cancel) =>
                    {
                        var res = await bus.SendCommand(new UserAnyByAccount() { AccountMode = AccountMode.Mobile, Account = account }, cancel);

                        return !res;
                    })
                    .WithMessage(c => $"该手机号码已存在。");

                RuleFor(x => x.Password).NotEmpty().MaximumLength(30).WithName("密码");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("名字");
            }
        }

        public class Handler : CommandHandler<UserCreateCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [UnitOfWork(DbType = typeof(IDefaultDbContext))]
            public override async Task<int> Handle(UserCreateCommand request, CancellationToken cancellationToken)
            {
                //await bus.PublishEvent(new DomainNotification("", "开始注册...."), cancellationToken);

                var entity = mapper.Map<UserCreateCommand, UserEntity>(request);

                //角色操作
                if (request.Roles != null && request.Roles.Length > 0)
                {
                    //转换角色对象 并写入
                    entity.UserRoles = Array.ConvertAll(request.Roles, roleid => new UserRoleEntity
                    {
                        RoleId = roleid,
                        UserId = entity.Id
                    });
                }

                var res = db.Add(entity);

                //await bus.PublishEvent(new DomainNotification("", "结束注册...."), cancellationToken);

                if (res > 0)
                {
                    await bus.PublishEvent(new UserCreateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                else
                    return res;
            }
        }
    }
}
