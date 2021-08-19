﻿using AutoMapper;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Template.Ddd.Domain.Core.Entities.User;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 更新密码
    /// </summary>
    public class UserUpdatePasswordCommand : CommandId<int, string>
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

        public class Validator : CommandIdValidator<UserUpdatePasswordCommand, int, string>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(30).WithName("旧密码");
                RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(30).WithName("新密码").NotEqual(c => c.OldPassword).WithName("新密码不能和旧密码相同");
            }
        }

        public class Handler : CommandHandler<UserUpdatePasswordCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly ILoginInfoService loginInfo;

            public Handler(IDefaultDbRepository db, IMediatorHandler bus, ILoginInfoService loginInfo) : base(bus)
            {
                this.db = db;
                this.loginInfo = loginInfo;
            }

            public override async Task<int> Handle(UserUpdatePasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await db.Context.User.FindAsync(request.Id);

                request.NewPassword = Encrypt.Md5By32(request.NewPassword);
                request.OldPassword = Encrypt.Md5By32(request.OldPassword);

                if (!user.Password.Equals(request.OldPassword))
                    throw new Exception("旧密码错误");

                return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity { Password = request.NewPassword, UpdatedAt = DateTime.Now, UpdatedAtUserId = loginInfo.UserId });
            }
        }
    }
}
