using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.User;
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

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 用户信息修改命令
    /// </summary>
    public class UserUpdateInfoCommand : CommandId<int, string>, IMapFrom<UserEntity>
    {
        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        [Required]
        public string Location { get; set; }
        /// <summary>
        /// 职位
        /// </summary>
        [Required]
        public string Position { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        [Required]
        public string Company { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<UserUpdateInfoCommand, UserEntity>()
                .ForMember(c => c.Location, c => c.MapFrom(s => s.Location.SafeString()))
                .ForMember(c => c.Position, c => c.MapFrom(s => s.Position.SafeString()))
                .ForMember(c => c.Company, c => c.MapFrom(s => s.Company.SafeString()))
            ;

        public class Validator : CommandIdValidator<UserUpdateInfoCommand, int, string>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Name).NotEmpty().MaximumLength(30).WithName("名字");
                RuleFor(x => x.Company).NotEmpty().MaximumLength(30).WithName("公司");
                RuleFor(x => x.Location).NotEmpty().MaximumLength(30).WithName("位置");
                RuleFor(x => x.Position).NotEmpty().MaximumLength(20).WithName("职位");
            }
        }

        public class Handler : CommandHandler<UserUpdateInfoCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [UnitOfWork(DbType = typeof(IDefaultDbContext))]
            public override async Task<int> Handle(UserUpdateInfoCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.GetByIdAsync<UserEntity>(request.Id, cancellationToken);

                if (entity == null)
                    return 0;

                entity = mapper.Map(request, entity);

                var res = db.Update(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new UserUpdateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                return res;
            }
        }
    }
}
