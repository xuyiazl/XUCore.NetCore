using AutoMapper;
using AutoMapper.QueryableExtensions;
using XUCore.Net5.Template.Domain.Core;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Ddd.Domain.Commands;
using XUCore.Extensions;
using XUCore.Paging;

namespace XUCore.Net5.Template.Domain.Sys.AdminRole
{
    /// <summary>
    /// 角色查询命令
    /// </summary>
    public class AdminRoleQueryPaged : CommandPage<PagedModel<AdminRoleDto>>
    {
        /// <summary>
        /// 搜索字段
        /// </summary>
        public string Field { get; set; }
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

        public override bool IsVaild()
        {
            ValidationResult = new Validator().Validate(this);
            return ValidationResult.IsValid;
        }
        public class Validator : CommandPageValidator<AdminRoleQueryPaged, PagedModel<AdminRoleDto>>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }

        public class Handler : CommandHandler<AdminRoleQueryPaged, PagedModel<AdminRoleDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<PagedModel<AdminRoleDto>> Handle(AdminRoleQueryPaged request, CancellationToken cancellationToken)
            {
                var res = await db.Context.AdminAuthRole

                    .WhereIf(c => c.Status == request.Status, request.Status != Status.Default)
                    .WhereIf(c => c.Name.Contains(request.Search), !request.Search.IsEmpty())

                    .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                    .ProjectTo<AdminRoleDto>(mapper.ConfigurationProvider)
                    .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

                return res.ToModel();
            }
        }
    }
}
