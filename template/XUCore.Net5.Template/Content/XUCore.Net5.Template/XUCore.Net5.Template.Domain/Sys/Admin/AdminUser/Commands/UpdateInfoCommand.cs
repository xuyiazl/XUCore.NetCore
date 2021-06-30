using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Common.Mappings;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.NetCore.AspectCore.Cache;
using System.ComponentModel.DataAnnotations;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    /// <summary>
    /// 用户信息修改命令
    /// </summary>
    public class AdminUserUpdateInfoCommand : CommandId<int, long>, IMapFrom<AdminUserEntity>
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
            profile.CreateMap<AdminUserUpdateInfoCommand, AdminUserEntity>()
                .ForMember(c => c.Location, c => c.MapFrom(s => s.Location.SafeString()))
                .ForMember(c => c.Position, c => c.MapFrom(s => s.Position.SafeString()))
                .ForMember(c => c.Company, c => c.MapFrom(s => s.Company.SafeString()))
            ;

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandIdValidator<AdminUserUpdateInfoCommand, int, long>
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

        public class Handler : CommandHandler<AdminUserUpdateInfoCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMediatorHandler bus, IMapper mapper) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserUpdateInfoCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Context.AdminUser.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (entity == null)
                    return 0;

                entity = mapper.Map(request, entity);

                var res = db.Update(entity);

                if (res > 0)
                {
                    await bus.PublishEvent(new UpdateEvent(entity.Id, entity), cancellationToken);

                    return res;
                }
                return res;
            }
        }
    }
}
