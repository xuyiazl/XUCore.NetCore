using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Controllers;
using XUCore.NetCore.Jwt;
using XUCore.NetCore.Jwt.Algorithms;
using XUCore.NetCore.Jwt.Builder;
using XUCore.Extensions;
using XUCore.Helpers;
using System;

namespace XUCore.ApiTests.Controllers
{
    [JwtAuthorize]
    public class TokenController : ApiControllerBase
    {
        private readonly JwtOptions _jwtOptions;

        public TokenController(ILogger<TokenController> logger, JwtOptions jwtOptions)
            : base(logger)
        {
            _jwtOptions = jwtOptions;
        }

        [Route("Create")]
        [HttpGet]
        [JwtAllowAnonymous]
        public IActionResult Create()
        {
            var token = new JwtBuilder()
               .WithAlgorithm(new HMACSHA256Algorithm())
               .WithSecret(_jwtOptions.Secret)
               .JwtId(Id.GuidGenerator.Create())
               .Id(Id.LongGenerator.Create())
               .Account("XUCore")
               .NickName("哈哈")
               .VerifiedPhoneNumber("19173100454")
               .ExpirationTime(DateTime.UtcNow.AddMinutes(1))
               .Build();

            return Success("000001", token);
        }

        [Route("Verify")]
        [HttpGet]
        public IActionResult Verify()
        {
            var jwtid = HttpContext.User.Identity.GetValue<Guid>(ClaimName.JwtId);
            var id = HttpContext.User.Identity.GetValue<long>(ClaimName.Id);
            var account = HttpContext.User.Identity.GetValue<string>(ClaimName.Account);
            var nickname = HttpContext.User.Identity.GetValue<string>(ClaimName.NickName);
            var phone = HttpContext.User.Identity.GetValue<string>(ClaimName.VerifiedPhoneNumber);
            var expirationtime = HttpContext.User.Identity.GetValue<long>(ClaimName.ExpirationTime).ToDateTime();

            return Success("000002",
                data: new
                {
                    jwtid,
                    id,
                    account,
                    nickname,
                    phone,
                    expirationtime
                },
                message: "验证成功");
        }
    }
}