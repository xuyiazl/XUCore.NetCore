using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Template.Ddd.Applaction.AppServices.Login;
using XUCore.Template.Ddd.Domain.User.User;
using XUCore.Template.Ddd.Web.Models;

namespace XUCore.Template.Ddd.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginAppService loginAppService;

        public HomeController(ILogger<HomeController> logger, ILoginAppService loginAppService)
        {
            _logger = logger;
            this.loginAppService = loginAppService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //测试接入登录

            var command = new UserLoginCommand
            {
                Account = "",
                Password = ""
            };

            //验证命令模型
            if (!command.IsVaild())
                throw new Exception(command.GetErrors(""));

            //登录
            var res = await loginAppService.Login(command, CancellationToken.None);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
