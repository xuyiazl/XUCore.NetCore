using AutoMapper;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.User;
using FluentValidation;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;
using XUCore.NetCore.AspectCore.Cache;

namespace XUCore.Net5.Template.Domain.User.User
{
    /// <summary>
    /// 更新部分字段
    /// </summary>
    public class UserUpdateFieldCommand : CommandId<int, string>
    {
        /// <summary>
        /// 字段名
        /// </summary>
        [Required]
        public string Field { get; set; }
        /// <summary>
        /// 需要修改的值
        /// </summary>
        [Required]
        public string Value { get; set; }

        public class Validator : CommandIdValidator<UserUpdateFieldCommand, int, string>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Field).NotEmpty().WithMessage("字段名");
            }
        }

        public class Handler : CommandHandler<UserUpdateFieldCommand, int>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }


            public override async Task<int> Handle(UserUpdateFieldCommand request, CancellationToken cancellationToken)
            {
                switch (request.Field.ToLower())
                {
                    case "name":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Name = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case "username":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { UserName = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case "mobile":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Mobile = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case "password":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Password = Encrypt.Md5By32(request.Value), UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case "position":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Position = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case "location":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Location = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case "company":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Company = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    case "picture":
                        return await db.UpdateAsync<UserEntity>(c => c.Id == request.Id, c => new UserEntity() { Picture = request.Value, UpdatedAt = DateTime.Now, UpdatedAtUserId = LoginInfo.UserId });
                    default:
                        return 0;
                }
            }
        }
    }
}
