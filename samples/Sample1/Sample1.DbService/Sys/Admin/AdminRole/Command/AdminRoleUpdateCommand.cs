using AutoMapper;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using Sample1.Core;
using Sample1.Core.Enums;
using Sample1.Persistence.Entities.Sys.Admin;

namespace Sample1.DbService.Sys.Admin.AdminRole
{
    /// <summary>
    /// 角色修改命令
    /// </summary>
    public class AdminRoleUpdateCommand : Command<bool>, IMapFrom<AdminRoleEntity>
    {
        /// <summary>
        /// Id
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

        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminRoleUpdateCommand, AdminRoleEntity>()
                .ForMember(c => c.Updated_At, c => c.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandValidator<AdminRoleUpdateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotEmpty().GreaterThan(0).WithName("Id");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("角色名");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
