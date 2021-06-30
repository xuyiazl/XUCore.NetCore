using AutoMapper;
using FluentValidation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;

namespace XUCore.Net5.Template.Domain.Sys.Permission
{
    public class PermissionQueryExists : Command<bool>
    {
        public long AdminId { get; set; }
        public string OnlyCode { get; set; }
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandValidator<PermissionQueryExists>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
                RuleFor(x => x.OnlyCode).NotEmpty().MaximumLength(50).WithName("OnlyCode");
            }
        }
        public class Handler : CommandHandler<PermissionQueryExists, bool>
        {

            public Handler(IMediatorHandler bus) : base(bus)
            {
            }

            public override async Task<bool> Handle(PermissionQueryExists request, CancellationToken cancellationToken)
            {
                var data = await bus.SendCommand(new PermissionQueryData(), cancellationToken);

                return View.Create(data, request.AdminId).Any(c => c.OnlyCode == request.OnlyCode);
            }
        }
    }
}
