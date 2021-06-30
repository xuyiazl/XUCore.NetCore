using AutoMapper;
using FluentValidation;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;
using System;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Helpers;
using System.ComponentModel.DataAnnotations;

namespace XUCore.Net5.Template.Domain.Sys.AdminUser
{
    /// <summary>
    /// 更新密码
    /// </summary>
    public class AdminUserUpdatePasswordCommand : CommandId<int, long>
    {
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandIdValidator<AdminUserUpdatePasswordCommand, int, long>
        {
            public Validator()
            {
                AddIdValidator();

                RuleFor(x => x.OldPassword).NotEmpty().MaximumLength(50).WithName("旧密码");
                RuleFor(x => x.NewPassword).NotEmpty().MaximumLength(50).When(c => c.OldPassword != c.NewPassword).WithName("新密码");
            }
        }

        public class Handler : CommandHandler<AdminUserUpdatePasswordCommand, int>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper, IMediatorHandler bus) : base(bus)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<int> Handle(AdminUserUpdatePasswordCommand request, CancellationToken cancellationToken)
            {
                var admin = await db.Context.AdminUser.FindAsync(request.Id);

                request.NewPassword = Encrypt.Md5By32(request.NewPassword);
                request.OldPassword = Encrypt.Md5By32(request.OldPassword);

                if (!admin.Password.Equals(request.OldPassword))
                    throw new Exception("旧密码错误");

                return await db.UpdateAsync<AdminUserEntity>(c => c.Id == request.Id, c => new AdminUserEntity { Password = request.NewPassword });
            }
        }
    }
}
