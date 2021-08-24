using AutoMapper;
using AutoMapper.QueryableExtensions;
using XUCore.Template.Ddd.Domain.Core;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Paging;
using XUCore.Template.Ddd.Domain.Core.Entities.User;

namespace XUCore.Template.Ddd.Domain.User.User
{
    /// <summary>
    /// 用户查询分页
    /// </summary>
    public class UserQueryPaged : CommandPage<PagedModel<UserDto>>
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

        public class Validator : CommandPageValidator<UserQueryPaged, PagedModel<UserDto>>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }

        public class Handler : CommandHandler<UserQueryPaged, PagedModel<UserDto>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<PagedModel<UserDto>> Handle(UserQueryPaged request, CancellationToken cancellationToken)
            {
                var selector = db.BuildFilter<UserEntity>()

                    .And(c => c.Status == request.Status, request.Status != Status.Default)
                    .And(c =>
                                c.Name.Contains(request.Keyword) ||
                                c.Mobile.Contains(request.Keyword) ||
                                c.UserName.Contains(request.Keyword), request.Keyword.NotEmpty());

                var res = await db.GetPagedListAsync<UserEntity, UserDto>(selector, request.OrderBy, request.CurrentPage, request.PageSize, cancellationToken);

                return res.ToModel();
            }
        }
    }
}
