using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XUCore.NetCore.Sign;

namespace XUCore.NetCore.ApiTests
{
    public class SignDemo : SignMiddleware
    {
        public SignDemo(RequestDelegate next, IOptions<SignOptions> options)
            : base(next, options)
        {

        }

        public override string GetAppSecret(string appid)
        {
            return "appsecret";
        }

        public override bool ReplayAttack(string appid, string timestamp, string noncestr)
        {
            return true;
        }
    }
}
