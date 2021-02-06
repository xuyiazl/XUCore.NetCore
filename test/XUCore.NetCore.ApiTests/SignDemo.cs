using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XUCore.NetCore.Signature;

namespace XUCore.NetCore.ApiTests
{
    public class SignMiddlewareDemo : HttpSignMiddleware
    {
        public SignMiddlewareDemo(RequestDelegate next, IOptions<HttpSignOptions> options)
            : base(next, options)
        {

        }

        public override Task<string> GetAppSecretAsync(string appid)
        {
            appid = "web1ed21e4udroo37fmj";

            return Task.FromResult("CdzL5v9s6cmYOqeYW2ZicfdTaT3LdXhJ");
        }
    }

    public class HttpSignApiAttribute : HttpSignAttribute
    {
        public override Task<string> GetAppSecretAsync(IServiceProvider serviceProvider, string appid)
        {
            appid = "web1ed21e4udroo37fmj";

            return Task.FromResult("CdzL5v9s6cmYOqeYW2ZicfdTaT3LdXhJ");
        }
    }
}
