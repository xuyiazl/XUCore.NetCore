using AutoMapper;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using Sample.Mini.Core;
using Sample.Mini.Core.Enums;
using Sample.Mini.Persistence.Entities.Sys.Admin;

namespace Sample.Mini.Applaction.Admin
{
    /// <summary>
    /// 创建角色命令
    /// </summary>
    public class AdminRoleCreateCommand : Command<bool>, IMapFrom<AdminRoleEntity>
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 导航关联id集合
        /// </summary>
        public long[] MenuIds { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [Required]
        public Status Status { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminRoleCreateCommand, AdminRoleEntity>()
                .ForMember(c => c.Created_At, c => c.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandValidator<AdminRoleCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
