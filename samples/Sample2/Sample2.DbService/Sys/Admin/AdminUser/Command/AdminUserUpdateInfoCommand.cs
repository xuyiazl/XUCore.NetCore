using AutoMapper;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using Sample2.Core;
using Sample2.Persistence.Entities.Sys.Admin;


namespace Sample2.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 用户信息修改命令
    /// </summary>
    public class AdminUserUpdateInfoCommand : Command<bool>, IMapFrom<AdminUserEntity>
    {
        /// <summary>
        /// Id
        /// </summary>
        [Required]
        public long Id { get; set; }
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

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminUserUpdateInfoCommand, AdminUserEntity>()
                .ForMember(c => c.Location, c => c.MapFrom(s => s.Location.SafeString()))
                .ForMember(c => c.Position, c => c.MapFrom(s => s.Position.SafeString()))
                .ForMember(c => c.Company, c => c.MapFrom(s => s.Company.SafeString()))
            ;

        public class Validator : CommandValidator<AdminUserUpdateInfoCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(30).WithName("名字");
                RuleFor(x => x.Company).NotEmpty().MaximumLength(30).WithName("公司");
                RuleFor(x => x.Location).NotEmpty().MaximumLength(30).WithName("位置");
                RuleFor(x => x.Position).NotEmpty().MaximumLength(20).WithName("职位");
            }
        }

    }
}
