using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Controllers;
using System;

namespace XUCore.ApiTests.Controllers
{
    [Area("t")]
    public class TestAreaController : AreaApiControllerBase
    {
        public TestAreaController(ILogger<TestController> logger)
          : base(logger)
        {
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