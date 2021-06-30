using AutoMapper;
using Sample.Ddd.Domain.Core;
using Sample.Ddd.Domain.Core.Entities.Sys.Admin;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;
using XUCore.Ddd.Domain.Exceptions;

namespace Sample.Ddd.Domain.Sys.AdminUser
{
    /// <summary>
    /// 登录命令
    /// </summary>
    public class AdminUserLoginCommand : Command<AdminUserDto>
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [Required]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        public string Password { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandValidator<AdminUserLoginCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Account).NotEmpty().MaximumLength(30).WithName("用户名/手机号码");
                RuleFor(x => x.Password).NotEmpty().MaximumLength(50).WithName("密码");
            }
        }

        public class Handler : CommandHandler<AdminUserLoginCommand, AdminUserDto>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<AdminUserDto> Handle(AdminUserLoginCommand request, CancellationToken cancellationToken)
            {
                var user = default(AdminUserEntity);

                request.Password = Encrypt.Md5By32(request.Password);

                var loginWay = "";

                if (!Valid.IsMobileNumberSimple(request.Account))
                {
                    user = await db.Context.AdminUser.Where(c => c.UserName.Equals(request.Account)).FirstOrDefaultAsync();
                    if (user == null)
                        Failure.Error("账号不存在");

                    loginWay = "Mobile";
                }
                else
                {
                    user = await db.Context.AdminUser.Where(c => c.Mobile.Equals(request.Account)).FirstOrDefaultAsync();
                    if (user == null)
                        Failure.Error("手机号码不存在");

                    loginWay = "UserName";
                }

                if (!user.Password.Equals(request.Password))
                    Failure.Error("密码错误");
                if (user.Status != Status.Show)
                    Failure.Error("您的帐号禁止登录,请与管理员联系!");


                user.LoginCount += 1;
                user.LoginLastTime = DateTime.Now;
                user.LoginLastIp = Web.IP;

                user.LoginRecords.Add(new LoginRecordEntity
                {
                    AdminId = user.Id,
                    LoginIp = user.LoginLastIp,
                    LoginTime = user.LoginLastTime,
                    LoginWay = loginWay
                });

                db.Update(user);

                return mapper.Map<AdminUserDto>(user);
            }
        }
    }
}
