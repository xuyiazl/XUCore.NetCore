using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.Extensions;
using XUCore.Helpers;
using System;
using XUCore.NetCore.Authorization.JwtBearer;
using System.Collections.Generic;
using XUCore.Serializer;
using Microsoft.AspNetCore.Authorization;
using XUCore.NetCore.Swagger;
using XUCore.NetCore.Authorization;

namespace XUCore.NetCore.MessageApiTest
{
    public class TokenJwtController : ApiControllerBase
    {

        public TokenJwtController(ILogger<TokenJwtController> logger)
            : base(logger)
        {
        }

        [Route("Create")]
        [HttpGet]
        [AllowAnonymous]
        public Result<string> Create()
        {
            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { "UserId","724a09c53dd741dca89f419794da9009" },
                { "Account","19173100088" },
                { "AccountId","eea64108fae64807bc909b6951be174f" }
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            return new Result<string>(1, "", accessToken);
        }

        [Route("Verify")]
        [HttpGet]
        [SecurityDefine("resourceid-verify")]
        public Result<string> Verify()
        {
            var UserId = HttpContext.User.Identity.GetValue<string>("UserId");
            var Account = HttpContext.User.Identity.GetValue<string>("Account");
            var AccountId = HttpContext.User.Identity.GetValue<string>("AccountId");

            return new Result<string>(1, "", new
            {
                UserId,
                Account,
                AccountId
            }.ToJson());
        }
    }
}