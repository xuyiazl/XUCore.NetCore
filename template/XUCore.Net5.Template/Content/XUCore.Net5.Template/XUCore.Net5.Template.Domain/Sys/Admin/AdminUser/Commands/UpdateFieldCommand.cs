using AutoMapper;
using FluentValidation;
using XUCore.Net5.Template.Domain.Common;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;
using XUCore.NetCore.AspectCore.Cache;
using System.ComponentModel.DataAnnotations;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    /// <summary>
    /// 更新部分字段
    /// </summary>
    public class AdminUserUpdateFieldCommand : CommandId<int, long>
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

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandIdValidator<AdminUserUpdateFieldCommand, int, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.Field).NotEmpty().WithMessage("字段名");
            }
        }

        public class Handler : CommandHandler<AdminUserUpdateFieldCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            [CacheRemove(Key = CacheKey.AuthTables)]
            public override async Task<int> Handle(AdminUserUpdateFieldCommand request, CancellationToken cancellationToken)
            {
                switch (request.Field.ToLower())
                {
                    case "name":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Name = request.Value, Updated_At = DateTime.Now });
                    case "username":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { UserName = request.Value, Updated_At = DateTime.Now });
                    case "mobile":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Mobile = request.Value, Updated_At = DateTime.Now });
                    case "password":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Password = Encrypt.Md5By32(request.Value), Updated_At = DateTime.Now });
                    case "position":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Position = request.Value, Updated_At = DateTime.Now });
                    case "location":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Location = request.Value, Updated_At = DateTime.Now });
                    case "company":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Company = request.Value, Updated_At = DateTime.Now });
                    case "picture":
                        return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity() { Picture = request.Value, Updated_At = DateTime.Now });
                    default:
                        return 0;
                }
            }
        }
    }
}
