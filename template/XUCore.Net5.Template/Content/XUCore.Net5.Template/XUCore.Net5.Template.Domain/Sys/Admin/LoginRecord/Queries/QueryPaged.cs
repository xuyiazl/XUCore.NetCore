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
using XUCore.Extensions;
using XUCore.Paging;

namespace XUCore.Net5.Template.Domain.Sys.LoginRecord
{
    /// <summary>
    /// 登录记录查询分页
    /// </summary>
    public class LoginRecordQueryPaged : CommandPage<PagedModel<LoginRecordDto>>
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
        public class Validator : CommandPageValidator<LoginRecordQueryPaged, PagedModel<LoginRecordDto>>
        {
            public Validator()
            {
                AddPageVaildator();
            }
        }
        public class Handler : CommandHandler<LoginRecordQueryPaged, PagedModel<LoginRecordDto>>
        {
            private readonly INigelDbRepository db;
            private readonly IMapper mapper;

            public Handler(INigelDbRepository db, IMapper mapper)
            {
                this.db = db;
                this.mapper = mapper;
            }

            public override async Task<PagedModel<LoginRecordDto>> Handle(LoginRecordQueryPaged request, CancellationToken cancellationToken)
            {
                var res = await View.Create(db.Context)

                    .WhereIf(c => c.Name.Contains(request.Search) || c.Mobile.Contains(request.Search) || c.UserName.Contains(request.Search), !string.IsNullOrEmpty(request.Search))

                    .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                    .ProjectTo<LoginRecordDto>(mapper.ConfigurationProvider)
                    .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

                return res.ToModel();
            }
        }
    }
}
