using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Paging;
using XUCore.Template.Ddd.Domain.Core;

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
        public string Keyword { get; set; }
        /// <summary>
        /// 排序方式 exp：“Id asc or Id desc”
        /// </summary>
        public string OrderBy { get; set; }
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

                    .WhereIf(c => c.Name.Contains(request.Keyword) || c.Mobile.Contains(request.Keyword) || c.UserName.Contains(request.Keyword), request.Keyword.NotEmpty())

                    .OrderByBatch(request.OrderBy, request.OrderBy.NotEmpty())

                    .ProjectTo<UserLoginRecordDto>(mapper.ConfigurationProvider)
                    .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

                return res.ToModel();
            }
        }
    }
}
