using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Controllers;
using System;

namespace XUCore.ApiTests.Controllers
{
    public class TestController : ApiControllerBase
    {
        public TestController(ILogger<TestController> logger)
          : base(logger)
        {
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