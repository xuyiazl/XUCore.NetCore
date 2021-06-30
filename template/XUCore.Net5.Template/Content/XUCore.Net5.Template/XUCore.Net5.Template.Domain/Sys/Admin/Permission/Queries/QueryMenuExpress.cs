using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Sys.AdminMenu;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.Net5.Template.Domain.Sys.Permission
{
    public class PermissionQueryMenuExpress : Command<IList<PermissionMenuDto>>
    {
        public long AdminId { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<PermissionQueryMenuExpress>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
            }
        }
        public class Handler : CommandHandler<PermissionQueryMenuExpress, IList<PermissionMenuDto>>
        {
            public Handler(IMapper mapper, IMediatorHandler bus) : base(bus, mapper)
            {

            }

            public override async Task<IList<PermissionMenuDto>> Handle(PermissionQueryMenuExpress request, CancellationToken cancellationToken)
            {
                var data = await bus.SendCommand(new PermissionQueryData(), cancellationToken);

                var list = View.Create(data, request.AdminId)
                    .Where(c => c.IsMenu == true && c.IsExpress == true)
                    .OrderByDescending(c => c.Weight)
                    .ProjectTo<PermissionMenuDto>(mapper.ConfigurationProvider)
                    .ToList();

                return list;
            }
        }
    }
}
