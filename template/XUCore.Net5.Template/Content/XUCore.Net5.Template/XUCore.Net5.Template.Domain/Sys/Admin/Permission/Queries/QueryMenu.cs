using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using XUCore.Net5.Template.Domain.Sys.AdminMenu;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Bus;
using XUCore.Ddd.Domain.Commands;
using XUCore.Net5.Template.Domain.Core.Entities.Sys.Admin;

namespace XUCore.Net5.Template.Domain.Sys.Permission
{
    public class PermissionQueryMenu : Command<IList<PermissionMenuTreeDto>>
    {
        public long AdminId { get; set; }

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandValidator<PermissionQueryMenu>
        {
            public Validator()
            {
                RuleFor(x => x.AdminId).NotEmpty().GreaterThan(0).WithName("AdminId");
            }
        }

        public class Handler : CommandHandler<PermissionQueryMenu, IList<PermissionMenuTreeDto>>
        {

            public Handler(IMapper mapper, IMediatorHandler bus) : base(bus, mapper)
            {

            }

            public override async Task<IList<PermissionMenuTreeDto>> Handle(PermissionQueryMenu request, CancellationToken cancellationToken)
            {
                var data = await bus.SendCommand(new PermissionQueryData(), cancellationToken);

                var list = View.Create(data, request.AdminId)
                    .Where(c => c.IsMenu == true)
                    .OrderByDescending(c => c.Weight)
                    .ToList();

                return AuthMenuTree(list, 0);
            }

            private IList<PermissionMenuTreeDto> AuthMenuTree(IList<AdminMenuEntity> entities, long parentId)
            {
                IList<PermissionMenuTreeDto> menus = new List<PermissionMenuTreeDto>();

                entities.Where(c => c.FatherId == parentId).ToList().ForEach(entity =>
                {
                    var dto = mapper.Map<AdminMenuEntity, PermissionMenuTreeDto>(entity);

                    dto.Child = AuthMenuTree(entities, dto.Id);

                    menus.Add(dto);
                });

                return menus;
            }
        }
    }
}
