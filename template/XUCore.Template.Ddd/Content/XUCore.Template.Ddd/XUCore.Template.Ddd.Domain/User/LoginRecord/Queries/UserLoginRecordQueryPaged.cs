using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using XUCore.Template.Ddd.Domain.Core;
using XUCore.Ddd.Domain.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Paging;

namespace XUCore.Template.Ddd.Domain.User.LoginRecord
{
    /// <summary>
    /// 登录记录查询分页
    /// </summary>
    public class UserLoginRecordQueryPaged : CommandPage<PagedModel<UserLoginRecordDto>>
    {
        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Search { get; set; }
        /// <summary>
        /// 排序字段
        /// </summary>
        public string Sort { get; set; }
        /// <summary>
        /// 排序方式 exp：“asc or desc”
        /// </summary>
        public string Order { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public Status Status { get; set; }

        public class Validator : CommandPageValidator<UserLoginRecordQueryPaged, PagedModel<UserLoginRecordDto>>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
        public class Handler : CommandHandler<UserLoginRecordQueryPaged, PagedModel<UserLoginRecordDto>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<PagedModel<UserLoginRecordDto>> Handle(UserLoginRecordQueryPaged request, CancellationToken cancellationToken)
            {
                var res = await View.Create(db.Context)

                    .WhereIf(c => c.Name.Contains(request.Search) || c.Mobile.Contains(request.Search) || c.UserName.Contains(request.Search), !string.IsNullOrEmpty(request.Search))

                    .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                    .ProjectTo<UserLoginRecordDto>(mapper.ConfigurationProvider)
                    .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

                return res.ToModel();
            }
        }
    }
}
