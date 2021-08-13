using AutoMapper;
using AutoMapper.QueryableExtensions;
using XUCore.Net5.Template.Domain.Core;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Paging;

namespace XUCore.Net5.Template.Domain.User.User
{
    /// <summary>
    /// 用户查询分页
    /// </summary>
    public class UserQueryPaged : CommandPage<PagedModel<UserDto>>
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

        public class Validator : CommandPageValidator<UserQueryPaged, PagedModel<UserDto>>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }

        public class Handler : CommandHandler<UserQueryPaged, PagedModel<UserDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<PagedModel<UserDto>> Handle(UserQueryPaged request, CancellationToken cancellationToken)
            {
                var res = await db.Context.User

                    .WhereIf(c => c.Status == request.Status, request.Status != Status.Default)
                    .WhereIf(c =>
                                c.Name.Contains(request.Search) ||
                                c.Mobile.Contains(request.Search) ||
                                c.UserName.Contains(request.Search), !request.Search.IsEmpty())

                    .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                    .ProjectTo<UserDto>(mapper.ConfigurationProvider)
                    .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

                return res.ToModel();
            }
        }
    }
}
