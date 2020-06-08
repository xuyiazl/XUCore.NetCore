using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XUCore.NetCore.Filters;
using XUCore.NetCore.HttpFactory;
using XUCore.NetCore.Razors;
using XUCore.NetCore.Uploads;
using XUCore.NetCore.Uploads.Params;
using XUCore.Drawing;
using XUCore.Helpers;
using XUCore.Webs;
using XUCore.WebTests.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore.Redis;
using XUCore.WebTests.Data.DbService;
using XUCore.Paging;
using XUCore.Json;
using MessagePack;
using System;
using XUCore.NetCore;
using XUCore.WebTests.Data.Entity;
using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Data.SqlClient;

namespace XUCore.WebTests.Controllers
{
    [MessagePackObject]
    public class User
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public DateTime CreateTime { get; set; }
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpService _httpMessage;

        private readonly IRedisService _redisService;
        private readonly IDbAdminUsersServiceProvider _dbAdminUsersServiceProvider;
        /// <summary>
        /// 文件上传服务
        /// </summary>
        private IFileUploadService _fileUploadService;


        public HomeController(ILogger<HomeController> logger, IHttpService httpMessage, IDbAdminUsersServiceProvider dbAdminUsersServiceProvider, IFileUploadService fileUploadService, IRedisService redisService)
        {
            _logger = logger;
            _httpMessage = httpMessage;
            _fileUploadService = fileUploadService;
            _redisService = redisService;
            _dbAdminUsersServiceProvider = dbAdminUsersServiceProvider;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var list = new List<AdminUsers>();
            for (var ndx = 0; ndx < 10; ndx++)
            {
                var user = new AdminUsers
                {
                    Company = "test",
                    CreatedTime = DateTime.Now,
                    Location = "test",
                    LoginCount = 0,
                    LoginLastIp = "127.0.0.1",
                    LoginLastTime = DateTime.Now,
                    Mobile = "17710146178",
                    Name = $"徐毅{ndx}",
                    Password = "123456",
                    Picture = $"徐毅{ndx}",
                    Position = $"徐毅{ ndx }",
                    Status = true,
                    UserName = "xuyi"
                };
                list.Add(user);
            }
            var res4 = _dbAdminUsersServiceProvider.SelectList<AdminUsers>("select * from AdminUsers", CommandType.Text, new SqlParameter("name", "1"));
            var res0 = _dbAdminUsersServiceProvider.BatchInsert(list.ToArray());
            var res3 = _dbAdminUsersServiceProvider.BatchUpdate(c => c.Id > 22, new AdminUsers() { Name = "哈德斯", Location = "吹牛逼总监", Company = "大牛逼公司" });

            //var re2 = await _dbAdminUsersServiceProvider.BatchUpdateAsync(c => c.Id > 22, c => new AdminUsers() { Name = "哈德斯", Location = "吹牛逼总监", Company = "大牛逼公司" });
            var re1 = await _dbAdminUsersServiceProvider.BatchDeleteAsync(c => c.Id > 22);


            var ur2 = UrlArguments.Create("api/messagepack/add");

            var resData = await _httpMessage.CreateClient("msgtest").SetHeaderAccept(HttpMediaType.MessagePack).PostAsync<User>(ur2, null, cancellationToken);

            if (resData.IsSuccessStatusCode)
            {

            }

            var m = await resData.Content.ReadAsAsync<User>(HttpMediaType.MessagePack);

            var url = UrlArguments.Create("msgpack", "api/messagepack/get");

            var res = await url.GetAsync<User>();

            var postUrl = UrlArguments.Create("msgpack", "api/messagepack/add");

            var res1 = await url.PostAsync<User, User>(res);

            var url1 = UrlArguments.Create("test", $"/api/CommentsLive/GetPaged")
                        .Add("aid", 1539)
                        .Add("commentId", 0)
                        .Add("pageSize", 10000);

            var res2 = await url.GetAsync<ReturnModel>();

            return View(res2);
        }

        //[NoCache]
        //[Route("{id}")]
        //[RazorHtmlStatic(Template = "/static/{controller}/{action}-{id}.html")]
        //public async Task<IActionResult> Index(int id, CancellationToken cancellationToken)
        //{
        //var url = UrlArguments.Create("test", $"/api/CommentsLive/GetPaged")
        //     .Add("aid", 1539)
        //     .Add("commentId", 0)
        //     .Add("pageSize", 10000);

        //var res = await _httpService.GetAsync<ReturnModel>(url, cancellationToken);

        //    return View(res);
        //}

        public async Task<IActionResult> IndexView()
        {
            var url = UrlArguments.Create("test", $"/api/CommentsLive/GetPaged")
                 .Add("aid", 1539)
                 .Add("commentId", 0)
                 .Add("pageSize", 10000);

            var res = await url.GetAsync<ReturnModel>();

            return View(res);
        }

        [RazorHtml(Path = "/static/home/privacy.html", ViewName = "Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile, CancellationToken cancellationToken)
        {
            var param = new SingleFileUploadParam()
            {
                Request = Request,
                FormFile = formFile,
                RootPath = Web.WebRootPath,
                Module = "Test",
                Group = "Logo"
            };

            var result = await _fileUploadService.UploadAsync(param, cancellationToken);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UploadMulit(IFormCollection formCollection, CancellationToken cancellationToken)
        {
            FormFileCollection filelist = (FormFileCollection)formCollection.Files;

            var param = new MultipleFileUploadParam()
            {
                Request = Request,
                FormFiles = filelist.ToList(),
                RootPath = Web.WebRootPath,
                Module = "Test",
                Group = "Logo"
            };

            var result = await _fileUploadService.UploadAsync(param, cancellationToken);

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile formFile, CancellationToken cancellationToken)
        {
            var param = new SingleImageUploadParam()
            {
                Request = Request,
                FormFile = formFile,
                RootPath = Web.WebRootPath,
                Module = "Test",
                Group = "Logo",
                ThumbCutMode = ThumbnailMode.Cut,
                Thumbs = new List<string> { "200x300", "400x200" },
            };

            var result = await _fileUploadService.UploadImageAsync(param, cancellationToken);

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}