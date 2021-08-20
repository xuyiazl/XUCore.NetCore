using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.User;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Ddd.Domain.Exceptions;
using XUCore.Helpers;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 登录命令
    /// </summary>
    public class UserLoginCommand : Command<UserDto>
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

        public class Validator : CommandValidator<UserLoginCommand>
        {
            public Validator()
            {
                RuleFor(x => x.Account).NotEmpty().MaximumLength(30).WithName("用户名/手机号码");
                RuleFor(x => x.Password).NotEmpty().MaximumLength(50).WithName("密码");
            }
        }

        public class Handler : CommandHandler<UserLoginCommand, UserDto>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<UserDto> Handle(UserLoginCommand request, CancellationToken cancellationToken)
            {
                var user = default(UserEntity);

                request.Password = Encrypt.Md5By32(request.Password);

                var loginWay = "";

                if (!Valid.IsMobileNumberSimple(request.Account))
                {
                    user = await db.GetFirstAsync<UserEntity>(c => c.UserName.Equals(request.Account), cancellationToken: cancellationToken);
                    if (user == null)
                        Failure.Error("账号不存在");

                    loginWay = "UserName";
                }
                else
                {
                    user = await db.GetFirstAsync<UserEntity>(c => c.Mobile.Equals(request.Account), cancellationToken: cancellationToken);
                    if (user == null)
                        Failure.Error("手机号码不存在");

                    loginWay = "Mobile";
                }

                if (!user.Password.Equals(request.Password))
                    Failure.Error("密码错误");
                if (user.Status != Status.Show)
                    Failure.Error("您的帐号禁止登录,请与用户联系!");


                user.LoginCount += 1;
                user.LoginLastTime = DateTime.Now;
                user.LoginLastIp = Web.IP;

                user.LoginRecords.Add(new UserLoginRecordEntity
                {
                    UserId = user.Id,
                    LoginIp = user.LoginLastIp,
                    LoginTime = user.LoginLastTime,
                    LoginWay = loginWay
                });

                db.Update(user);

                return mapper.Map<UserDto>(user);
            }
        }
    }
}
