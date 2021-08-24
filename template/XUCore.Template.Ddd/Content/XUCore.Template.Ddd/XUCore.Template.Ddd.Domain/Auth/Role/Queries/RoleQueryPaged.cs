using AutoMapper;
using AutoMapper.QueryableExtensions;
using XUCore.Template.Ddd.Domain.Core;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Paging;
using XUCore.Template.Ddd.Domain.Core.Entities.Auth;

namespace XUCore.Template.Ddd.Domain.Auth.Role
{
    /// <summary>
    /// 角色查询命令
    /// </summary>
    public class RoleQueryPaged : CommandPage<PagedModel<RoleDto>>
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

        public class Validator : CommandPageValidator<RoleQueryPaged, PagedModel<RoleDto>>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }

        public class Handler : CommandHandler<RoleQueryPaged, PagedModel<RoleDto>>
        {
            private readonly IDefaultDbRepository db;
            private readonly IMapper mapper;

            public Handler(IDefaultDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<PagedModel<RoleDto>> Handle(RoleQueryPaged request, CancellationToken cancellationToken)
            {
                var selector = db.BuildFilter<RoleEntity>()

                    .And(c => c.Status == request.Status, request.Status != Status.Default)
                    .And(c => c.Name.Contains(request.Keyword), request.Keyword.NotEmpty());

                var res = await db.GetPagedListAsync<RoleEntity, RoleDto>(selector, request.OrderBy, request.CurrentPage, request.PageSize, cancellationToken);

                return res.ToModel();
            }
        }
    }
}
