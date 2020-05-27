using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Security.Principal;

namespace XUCore.NetCore.Jwt.Internal
{
    internal static class DefaultTicketFactory
    {
        /// <summary>
        /// Creates user's <see cref="AuthenticationTicket" /> from user's <see cref="IIdentity" /> and current <see cref="AuthenticationScheme" />
        /// </summary>
        public static AuthenticationTicket CreateTicket(IIdentity identity, AuthenticationScheme scheme) =>
            new AuthenticationTicket(
                new ClaimsPrincipal(identity),
                new AuthenticationProperties(),
                scheme.Name);
    }
}