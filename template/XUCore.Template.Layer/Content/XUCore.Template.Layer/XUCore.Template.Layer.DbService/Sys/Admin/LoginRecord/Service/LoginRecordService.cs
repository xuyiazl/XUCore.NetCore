using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Paging;
using XUCore.Template.Layer.Persistence;
using XUCore.Template.Layer.Persistence.Entities.Sys.Admin;

namespace XUCore.Template.Layer.DbService.Sys.Admin.LoginRecord
{
    public class LoginRecordService : ILoginRecordService
    {
        private readonly IDefaultDbRepository db;
        private readonly IMapper mapper;

        public LoginRecordService(IDefaultDbRepository db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int> CreateAsync(LoginRecordCreateCommand request, CancellationToken cancellationToken)
        {
            var entity = mapper.Map<LoginRecordCreateCommand, LoginRecordEntity>(request);

            var res = await db.AddAsync(entity, cancellationToken: cancellationToken);

            if (res > 0)
            {
                return res;
            }
            else
                return res;
        }

        public async Task<IList<LoginRecordDto>> GetListByAdminIdAsync(int limit, long adminId, CancellationToken cancellationToken)
        {
            var res = await View.Create(db.Context)

                .Where(c => c.AdminId == adminId)

                .OrderByDescending(c => c.LoginTime)
                .Take(limit)

                .ProjectTo<LoginRecordDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return res;
        }

        public async Task<PagedModel<LoginRecordDto>> GetPageListAsync(LoginRecordQueryPagedCommand request, CancellationToken cancellationToken)
        {
            var res = await View.Create(db.Context)

                .WhereIf(c => c.Name.Contains(request.Keyword) || c.Mobile.Contains(request.Keyword) || c.UserName.Contains(request.Keyword), request.Keyword.NotEmpty())

                .OrderByBatch(request.OrderBy, request.OrderBy.NotEmpty())

                .ProjectTo<LoginRecordDto>(mapper.ConfigurationProvider)
                .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
