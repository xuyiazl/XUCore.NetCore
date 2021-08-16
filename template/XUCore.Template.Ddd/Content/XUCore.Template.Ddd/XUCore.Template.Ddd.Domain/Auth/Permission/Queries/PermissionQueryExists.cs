using AutoMapper;
using FluentValidation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;

namespace XUCore.Template.Ddd.Domain.Auth.Permission
{
    public class PermissionQueryExists : Command<bool>
    {
        public string UserId { get; set; }
        public string OnlyCode { get; set; }
      
        public class Validator : CommandValidator<PermissionQueryExists>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("UserId");
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
                var menus = await bus.SendCommand(new PermissionQueryUserMenus { UserId = request.UserId }, cancellationToken);

                return menus.Any(c => c.OnlyCode == request.OnlyCode);
            }
        }
    }
}
