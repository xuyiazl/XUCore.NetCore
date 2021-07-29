using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Sample.Ddd.Application.AppServices.Login;
using Sample.Ddd.Domain.Sys.AdminUser;
using Sample.Ddd.Web.Models;

namespace Sample.Ddd.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdminLoginAppService adminLoginAppService;

        public HomeController(ILogger<HomeController> logger, IAdminLoginAppService adminLoginAppService)
        {
            _logger = logger;
            this.adminLoginAppService = adminLoginAppService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            //测试接入登录

            var command = new AdminUserLoginCommand
            {
                Account = "",
                Password = ""
            };

            //验证命令模型
            if (!command.IsVaild())
                throw new Exception(command.GetErrors(""));

            //登录
            var res = await adminLoginAppService.Login(command, CancellationToken.None);

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
