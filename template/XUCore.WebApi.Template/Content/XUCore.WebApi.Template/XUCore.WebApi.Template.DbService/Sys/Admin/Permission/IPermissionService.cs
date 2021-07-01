﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace XUCore.WebApi.Template.DbService.Sys.Admin.Permission
{
    public interface IPermissionService : IDbService
    {
        Task<bool> ExistsAsync(long adminId, string onlyCode, CancellationToken cancellationToken);
        Task<IList<PermissionMenuDto>> GetMenuExpressAsync(long adminId, CancellationToken cancellationToken);
        Task<IList<PermissionMenuTreeDto>> GetMenusAsync(long adminId, CancellationToken cancellationToken);
    }
}