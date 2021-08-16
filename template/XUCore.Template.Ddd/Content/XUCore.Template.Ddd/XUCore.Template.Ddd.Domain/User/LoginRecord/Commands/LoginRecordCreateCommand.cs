using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.User;
using XUCore.Template.Ddd.Domain.Core.Mappings;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;

namespace XUCore.Template.Ddd.Domain.User.LoginRecord
{
    /// <summary>
    /// 记录登录记录
    /// </summary>
    public class LoginRecordCreateCommand : Command<int>, IMapFrom<UserLoginRecordEntity>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public string UserId { get; set; }
        /// <summary>
        /// 登录方式
        /// </summary>
        [Required]
        public string LoginWay { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 登录ip
        /// </summary>
        [Required]
        public string LoginIp { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<LoginRecordCreateCommand, UserLoginRecordEntity>()
                .ForMember(c => c.LoginTime, c => c.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandValidator<LoginRecordCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("UserId");
                RuleFor(x => x.LoginWay).NotEmpty().WithName("登录方式");
                RuleFor(x => x.LoginIp).NotEmpty().MaximumLength(20).WithName("登录IP");
            }
        }

        public class Handler : CommandHandler<LoginRecordCreateCommand, int>
        {
            private readonly IDefaultDbRepository db;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus, mapper)
            {
                this.db = db;
            }

            public override async Task<int> Handle(LoginRecordCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = mapper.Map<LoginRecordCreateCommand, UserLoginRecordEntity>(request);

                var res = db.Add(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new LoginRecordCreateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                else
                    return res;
            }
        }
    }
}
