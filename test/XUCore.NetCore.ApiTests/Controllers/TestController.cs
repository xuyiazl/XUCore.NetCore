using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using XUCore.Extensions;
using XUCore.Helpers;
using XUCore.NetCore.Authorization.JwtBearer;
using XUCore.NetCore.Controllers;
using XUCore.NetCore.Swagger;

namespace XUCore.ApiTests.Controllers
{
    public class TestController : ApiControllerBase
    {
        public TestController(ILogger<TestController> logger)
          : base(logger)
        {
        }

        [Route("login")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            // 生成 token
            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>
            {
                { "userId" ,1},
                { "userName"  ,"name"},
                { "loginTime"  ,DateTime.Now.ToString()}
            });

            // 生成 刷新token
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken);

            // 设置 Swagger 自动登录
            Web.HttpContext.SigninToSwagger(accessToken);
            // 设置刷新 token
            Web.HttpContext.Response.Headers["x-access-token"] = refreshToken;

            return Success("000", "成功", "哈哈");
        }
        [Route("verifylogin")]
        [HttpPost]
        public IActionResult VerifyLogin()
        {
            return Success("000", "成功", new
            {
                UserId = Web.HttpContext.User.Identity.GetValue<long>("userId"),
                UserName = Web.HttpContext.User.Identity.GetValue<string>("userName"),
                LoginTime = Web.HttpContext.User.Identity.GetValue<string>("loginTime")
            });
        }
        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
            return Success("000", "成功","哈哈");
        }

        [Route("error")]
        [HttpGet]
        public IActionResult Error()
        {
            throw new Exception("这里是API异常");
        }

        [Route("success")]
        [HttpGet]
        public IActionResult Success()
        {
            return Success("000", "");
        }

        [Route("fail")]
        [HttpGet]
        public IActionResult Fail()
        {
            return Fail("000", "测试错误");
        }
    }
}