using AutoMapper;
using AutoMapper.QueryableExtensions;
using XUCore.Template.Ddd.Domain.Core;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;

namespace XUCore.Template.Ddd.Domain.User.LoginRecord
{
    /// <summary>
    /// 查询登录记录
    /// </summary>
    public class UserLoginRecordQueryList : CommandLimit<IList<UserLoginRecordDto>>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Required]
        public string UserId { get; set; }
       
        public class Validator : CommandLimitValidator<UserLoginRecordQueryList, IList<UserLoginRecordDto>>
        {
            public Validator()
            {
                AddLimitVaildator();

                RuleFor(x => x.UserId)
                    .NotEmpty().WithMessage("UserId不可为空");
            }
        }

        public class Handler : CommandHandler<UserLoginRecordQueryList, IList<UserLoginRecordDto>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<IList<UserLoginRecordDto>> Handle(UserLoginRecordQueryList request, CancellationToken cancellationToken)
            {
                var res = await View.Create(db.Context)

                    .Where(c => c.UserId == request.UserId)

                    .OrderByDescending(c => c.LoginTime)
                    .Take(request.Limit)

                    .ProjectTo<UserLoginRecordDto>(mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);

                return res;
            }
        }
    }
}
