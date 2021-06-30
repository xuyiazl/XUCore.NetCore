using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Extensions;
using XUCore.Paging;
using Sample.Plain.Persistence;
using Sample.Plain.Persistence.Entities.Sys.Admin;

namespace Sample.Plain.DbService.Sys.Admin.LoginRecord
{
    public class LoginRecordService : ILoginRecordService
    {
        private readonly INigelDbRepository db;
        private readonly IMapper mapper;

        public LoginRecordService(INigelDbRepository db, IMapper mapper)
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

                .WhereIf(c => c.Name.Contains(request.Search) || c.Mobile.Contains(request.Search) || c.UserName.Contains(request.Search), !string.IsNullOrEmpty(request.Search))

                .OrderByBatch($"{request.Sort} {request.Order}", !request.Sort.IsEmpty() && !request.Order.IsEmpty())

                .ProjectTo<LoginRecordDto>(mapper.ConfigurationProvider)
                .ToPagedListAsync(request.CurrentPage, request.PageSize, cancellationToken);

            return res.ToModel();
        }
    }
}
