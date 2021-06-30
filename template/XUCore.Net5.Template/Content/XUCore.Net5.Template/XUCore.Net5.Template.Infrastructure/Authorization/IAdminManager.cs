using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUCore.Net5.Template.Domain.Sys.AdminUser;

namespace XUCore.Net5.Template.Infrastructure.Authorization
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
