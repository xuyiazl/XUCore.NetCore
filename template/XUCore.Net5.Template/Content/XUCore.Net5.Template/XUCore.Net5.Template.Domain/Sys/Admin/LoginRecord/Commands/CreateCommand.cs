using AutoMapper;
using FluentValidation;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Common.Mappings;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.NetCore.AspectCore.Cache;
using System.ComponentModel.DataAnnotations;

namespace XUCore.Net5.Template.Domain.Sys.LoginRecord
{
    /// <summary>
    /// 记录登录记录
    /// </summary>
    public class LoginRecordCreateCommand : Command<int>, IMapFrom<LoginRecordEntity>
    {
        /// <summary>
        /// 管理员id
        /// </summary>
        [Required]
        public long AdminId { get; set; }
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
            profile.CreateMap<LoginRecordCreateCommand, LoginRecordEntity>()
                .ForMember(c => c.LoginTime, c => c.MapFrom(s => DateTime.Now))
            ;

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<LoginRecordCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
                RuleFor(x => x.LoginWay).NotEmpty().WithName("登录方式");
                RuleFor(x => x.LoginIp).NotEmpty().MaximumLength(20).WithName("登录IP");
            }
        }

        public class Handler : CommandHandler<LoginRecordCreateCommand, int>
        {
            private readonly INigelDbRepository db;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus, mapper)
            {
                this.db = db;
            }


            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(LoginRecordCreateCommand request, CancellationToken cancellationToken)
            {
                var entity = mapper.Map<LoginRecordCreateCommand, LoginRecordEntity>(request);

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
