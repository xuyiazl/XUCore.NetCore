using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XUCore.Net5.Template.Domain.Core;
using XUCore.Ddd.Domain.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Paging;
using System.ComponentModel.DataAnnotations;

namespace XUCore.Net5.Template.Domain.Sys.LoginRecord
{
    /// <summary>
    /// 查询登录记录
    /// </summary>
    public class LoginRecordQueryList : CommandLimit<IList<LoginRecordDto>>
    {
        /// <summary>
        /// 管理员id
        /// </summary>
        [Required]
        public long AdminId { get; set; }
        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }

        public class Validator : CommandLimitValidator<LoginRecordQueryList, IList<LoginRecordDto>>
        {
            public Validator()
            {
                AddLimitVaildator();

                RuleFor(x => x.AdminId)
                    .NotEmpty().WithMessage("AdminId不可为空")
                    .GreaterThan(0).WithMessage(c => $"AdminId必须大于0");
            }
        }

        public class Handler : CommandHandler<LoginRecordQueryList, IList<LoginRecordDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<LoginRecordDto>> Handle(LoginRecordQueryList request, CancellationToken cancellationToken)
            {
                var res = await View.Create(db.Context)

                    .Where(c => c.AdminId == request.AdminId)

                    .OrderByDescending(c => c.LoginTime)
                    .Take(request.Limit)

                    .ProjectTo<LoginRecordDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
