using AutoMapper;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.Template.Easy.Core;
using XUCore.Template.Easy.Core.Enums;
using XUCore.Template.Easy.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Easy.Applaction.Admin
{
    /// <summary>
    /// 角色修改命令
    /// </summary>
    public class AdminRoleUpdateCommand : Command<bool>, IMapFrom<AdminRoleEntity>
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [Required]
        public long Id { get; set; }
        /// <summary>
        /// 角色名
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 导航id集合
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
            profile.CreateMap<AdminRoleUpdateCommand, AdminRoleEntity>()
                .ForMember(c => c.Updated_At, c => c.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandValidator<AdminRoleUpdateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().WithName("id");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
