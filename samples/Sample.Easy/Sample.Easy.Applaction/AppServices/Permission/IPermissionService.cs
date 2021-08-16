﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sample.Easy.Applaction;

namespace Sample.Easy.Applaction.Permission
{
    public interface IPermissionService : IAppService
    {
        Task<bool> ExistsAsync(long adminId, string onlyCode, CancellationToken cancellationToken);
        Task<IList<PermissionMenuDto>> GetMenuExpressAsync(long adminId, CancellationToken cancellationToken);
        Task<IList<PermissionMenuTreeDto>> GetMenusAsync(long adminId, CancellationToken cancellationToken);
    }
}