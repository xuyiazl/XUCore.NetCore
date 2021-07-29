using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sample.Ddd.Domain.Sys.AdminUser;

namespace Sample.Ddd.Infrastructure.Authorization
{
    public interface IAdminManager
    {
        long AdminId { get; }
        string AdminName { get; }
        bool IsAuthenticated { get; }

        bool IsCanAccess(string accessKey);
        Task<(string, string)> LoginAsync(AdminUserLoginCommand command);

        Task LoginOutAsync();
    }
}
