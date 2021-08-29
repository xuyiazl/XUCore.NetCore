using AutoMapper;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using XUCore.NetCore.Data;
using XUCore.Template.EasyLayer.Core;
using XUCore.Template.EasyLayer.Core.Enums;
using XUCore.Template.EasyLayer.Persistence.Entities.Admin;

namespace XUCore.Template.EasyLayer.DbService.Admin.AdminMenu
{
    /// <summary>
    /// 导航更新命令
    /// </summary>
    public class AdminMenuUpdateCommand : UpdateCommand<long>, IMapFrom<AdminMenuEntity>
    {
        /// <summary>
        /// 导航父级id
        /// </summary>
        public long FatherId { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 链接地址
        /// </summary>
        [Required]
        public string Url { get; set; }
        /// <summary>
        /// 唯一代码（权限使用）
        /// </summary>
        [Required]
        public string OnlyCode { get; set; }
        /// <summary>
        /// 是否是导航
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 排序权重
        /// </summary>
        public int Weight { get; set; }
        /// <summary>
        /// 是否是快捷导航
        /// </summary>
        public bool IsExpress { get; set; }
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
            profile.CreateMap<AdminMenuUpdateCommand, AdminMenuEntity>()
                .ForMember(c => c.Url, c => c.MapFrom(s => s.Url.IsEmpty() ? "#" : s.Url))
            ;

        public class Validator : CommandIdValidator<AdminMenuUpdateCommand, bool, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("菜单名");
                RuleFor(x => x.Url).NotEmpty().MaximumLength(50).WithName("Url");
                RuleFor(x => x.OnlyCode).NotEmpty().MaximumLength(50).WithName("唯一代码");
                RuleFor(x => x.Status).IsInEnum().NotEqual(Status.Default).WithName("数据状态");
            }
        }
    }
}
