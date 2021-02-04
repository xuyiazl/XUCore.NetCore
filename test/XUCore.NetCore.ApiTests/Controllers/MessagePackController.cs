using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XUCore.NetCore;
using XUCore.NetCore.ApiTests;
using XUCore.NetCore.MessagePack;
using XUCore.NetCore.Signature;

namespace XUCore.ApiTests.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [MessagePackResponseContentType]
    public class MessagePackController : ControllerBase
    {
        [SignAction]
        public User Get()
        {
            return new User { Id = 1, Name = "test", CreateTime = DateTime.Now };
        }


        [HttpPost]
        public User Add([FromBody] User user)
        {
            user.Name = "哈哈";
            user.CreateTime = DateTime.Now;

            return user;
        }
    }
}