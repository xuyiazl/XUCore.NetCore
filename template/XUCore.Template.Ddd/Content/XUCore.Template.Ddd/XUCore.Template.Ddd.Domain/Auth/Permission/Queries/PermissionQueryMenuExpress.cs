using AutoMapper;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;
using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;

namespace XUCore.Template.Ddd.Domain.Auth.Permission
{
    public class PermissionQueryMenuExpress : Command<IList<PermissionMenuDto>>
    {
        public string UserId { get; set; }

        public class Validator : CommandValidator<PermissionQueryMenuExpress>
        {
            public Validator()
            {
                RuleFor(x => x.UserId).NotEmpty().WithName("UserId");
            }
        }
        public class Handler : CommandHandler<PermissionQueryMenuExpress, IList<PermissionMenuDto>>
        {
            public Handler(IMapper mapper, IMediatorHandler bus) : base(bus, mapper)
            {

            }

            public override async Task<IList<PermissionMenuDto>> Handle(PermissionQueryMenuExpress request, CancellationToken cancellationToken)
            {
                var menus = await bus.SendCommand(new PermissionQueryUserMenus { UserId = request.UserId }, cancellationToken);

                var list = menus
                    .Where(c => c.IsMenu == true && c.IsExpress == true)
                    .OrderByDescending(c => c.Weight)
                    .ToList();

                var dto = mapper.Map<IList<MenuEntity>, IList<PermissionMenuDto>>(list);

                return dto;
            }
        }
    }
}
