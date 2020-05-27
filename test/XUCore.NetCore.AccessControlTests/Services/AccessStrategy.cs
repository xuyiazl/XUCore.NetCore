using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XUCore.NetCore.AccessControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.NetCore.AccessControlTests.Services
{

    public class ResourceAccessStrategy : IResourceAccessStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ResourceAccessStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsCanAccess(string accessKey)
        {
            if ("Never".Equals(accessKey, System.StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var httpContext = _httpContextAccessor.HttpContext;

            return httpContext.User.Identity.IsAuthenticated;
        }

        public IActionResult DisallowedCommonResult => new ContentResult
        {
            Content = "You have no access",
            ContentType = "text/html",
            StatusCode = 403
        };

        public IActionResult DisallowedAjaxResult => new JsonResult(new { Data = "You have no access", Code = 403 });
    }

    public class ControlAccessStrategy : IControlAccessStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ControlAccessStrategy(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public bool IsControlCanAccess(string accessKey)
        {
            if ("Never".Equals(accessKey, System.StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }
}
