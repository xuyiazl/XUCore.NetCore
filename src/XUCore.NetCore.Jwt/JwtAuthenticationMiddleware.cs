using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace XUCore.NetCore.Jwt
{
    public sealed class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtAuthenticationMiddleware(RequestDelegate next, IAuthenticationSchemeProvider schemes)
        {
            this._next = next ?? throw new ArgumentNullException(nameof(next));
            this.Schemes = schemes ?? throw new ArgumentNullException(nameof(schemes));
        }

        public IAuthenticationSchemeProvider Schemes { get; set; }

        public async Task Invoke(HttpContext context)
        {
            context.Features.Set<IAuthenticationFeature>(new AuthenticationFeature()
            {
                OriginalPath = context.Request.Path,
                OriginalPathBase = context.Request.PathBase
            });
            var handlers = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var authenticationScheme in await this.Schemes.GetRequestHandlerSchemesAsync())
            {
                var handlerAsync = await handlers.GetHandlerAsync(context, authenticationScheme.Name) as IAuthenticationRequestHandler;
                var flag = handlerAsync != null;
                if (flag)
                    flag = await handlerAsync.HandleRequestAsync();
                if (flag)
                    return;
            }
            var authenticateSchemeAsync = await this.Schemes.GetDefaultAuthenticateSchemeAsync();
            if (authenticateSchemeAsync != null)
            {
                //实际的认证业务
                var authenticateResult = await context.AuthenticateAsync(authenticateSchemeAsync.Name);
                if (authenticateResult?.Principal != null)
                    context.User = authenticateResult.Principal;
            }

            await _next.Invoke(context);
        }
    }
}