using System;

namespace XUCore.NetCore.Jwt
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class JwtAllowAnonymousAttribute : Attribute
    {
    }
}