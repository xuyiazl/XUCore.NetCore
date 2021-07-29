using AutoMapper;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;
using XUCore.WebApi.Template.Core;
using XUCore.WebApi.Template.Core.Enums;
using XUCore.WebApi.Template.Persistence.Entities.Sys.Admin;

namespace XUCore.WebApi.Template.DbService.Sys.Admin.AdminUser
{
    /// <summary>
    /// 创建管理员命令
    /// </summary>
    public class AdminUserCreateCommand : IMapFrom<AdminUserEntity>
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
        public long[] Roles { get; set; }

        public void Mapping(Profile profile) =>
            profile.CreateMap<AdminUserCreateCommand, AdminUserEntity>()
                .ForMember(c => c.Password, c => c.MapFrom(s => Encrypt.Md5By32(s.Password)))
                .ForMember(c => c.LoginCount, c => c.MapFrom(s => 0))
                .ForMember(c => c.LoginLastIp, c => c.MapFrom(s => ""))
                .ForMember(c => c.Picture, c => c.MapFrom(s => ""))
                .ForMember(c => c.Status, c => c.MapFrom(s => Status.Show))
                .ForMember(c => c.Created_At, c => c.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandValidator<AdminUserCreateCommand>
        {
            public Validator(IAdminUserService adminUserService)
            {
                RuleFor(x => x.UserName).NotEmpty().MaximumLength(20).WithName("账号")
                    .MustAsync(async (account, cancel) =>
                    {
                        var res = await adminUserService.AnyByAccountAsync(AccountMode.UserName, account, 0, cancel);

                        return !res;
                    })
                    .WithMessage(c => $"该账号已存在。");

                RuleFor(x => x.Mobile)
                    .NotEmpty()
                    .Matches("^[1][3-9]\\d{9}$").WithMessage("手机号格式不正确。")
                    .MustAsync(async (account, cancel) =>
                    {
                        var res = await Web.GetService<IAdminUserService>().AnyByAccountAsync(AccountMode.Mobile, account, 0, cancel);

                        return !res;
                    })
                    .WithMessage(c => $"该手机号码已存在。");

                RuleFor(x => x.Password).NotEmpty().MaximumLength(30).WithName("密码");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("名字");
                RuleFor(x => x.Company).NotEmpty().MaximumLength(30).WithName("公司");
                RuleFor(x => x.Location).NotEmpty().MaximumLength(30).WithName("位置");
                RuleFor(x => x.Name).NotEmpty().MaximumLength(20).WithName("名字");
            }
        }
    }
}
