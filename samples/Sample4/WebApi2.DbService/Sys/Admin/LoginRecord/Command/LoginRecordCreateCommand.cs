using AutoMapper;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Extensions;
using WebApi2.Core;
using WebApi2.Persistence.Entities.Sys.Admin;

namespace WebApi2.DbService.Sys.Admin.LoginRecord
{
    public class LoginRecordCreateCommand : Command<bool>, IMapFrom<LoginRecordEntity>
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

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);

            return ValidationResult.ThrowValidation();
        }

        public void Mapping(Profile profile) =>
            profile.CreateMap<LoginRecordCreateCommand, LoginRecordEntity>()
                .ForMember(c => c.LoginTime, c => c.MapFrom(s => DateTime.Now))
            ;

        public class Validator : CommandValidator<LoginRecordCreateCommand>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
                RuleFor(x => x.LoginWay).NotEmpty().WithName("登录方式");
                RuleFor(x => x.LoginIp).NotEmpty().MaximumLength(20).WithName("登录IP");
            }
        }
    }
}
