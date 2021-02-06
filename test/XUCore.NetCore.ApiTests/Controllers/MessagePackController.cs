using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.NetCore;
using XUCore.NetCore.ApiTests;
using XUCore.NetCore.Controllers;
using XUCore.NetCore.MessagePack;
using XUCore.NetCore.Signature;
using XUCore.NetCore.Swagger;

namespace XUCore.ApiTests.Controllers
{
    /// <summary>
    /// XML注释
    /// </summary>
    [MessagePackResponseContentType]
    public class MessagePackController : ApiControllerBase
    {
        public MessagePackController(ILogger<MessagePackController> logger)
            : base(logger)
        {

        }

        //[HttpSignApi]
        [FieldResponse]
        [HttpGet]
        public Result<User> Get()
        {
            return new Result<User>(0, "成功", new User { Id = 1, Name = "test", CreateTime = DateTime.Now });
        }


        [HttpPost]
        public Result<User> Add([FromBody] User user)
        {
            user.Name = "哈哈";
            user.CreateTime = DateTime.Now;

            return new Result<User>(0, "成功", user);
        }
    }
}