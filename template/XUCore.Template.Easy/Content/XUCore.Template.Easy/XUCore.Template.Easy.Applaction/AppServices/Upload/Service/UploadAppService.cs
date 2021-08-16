using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.Helpers;
using XUCore.NetCore;
using XUCore.NetCore.Oss;
using XUCore.NetCore.Uploads;
using XUCore.NetCore.Uploads.Params;
using XUCore.Template.Easy.Core;

namespace XUCore.Template.Easy.Applaction.Upload
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class UploadAppService : AppService, IUploadAppService
    {
        private readonly IOssFactory _ossFactory;
        /// <summary>
        /// 文件上传服务
        /// </summary>
        private IFileUploadService _fileUploadService;

        public UploadAppService(IFileUploadService fileUploadService, IOssFactory ossFactory)
        {
            _fileUploadService = fileUploadService;
            _ossFactory = ossFactory;
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[Controller]/File")]
        public async Task<Result<XUCore.Files.FileInfo>> UploadFile([Required] IFormFile formFile, CancellationToken cancellationToken)
        {
            var param = new SingleFileUploadParam()
            {
                Request = Web.Request,
                FormFile = formFile,
                RootPath = Web.WebRootPath,
                Module = "upload",
                Group = "file"
            };

            var result = await _fileUploadService.UploadAsync(param, cancellationToken);

            result.Url = $"https://www.xx.com/{result.FullPath.Replace("\\", "/")}";

            return RestFull.Success(data: result);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[Controller]/Image")]
        public async Task<Result<ImageFileInfo>> UploadImage([Required] IFormFile formFile, CancellationToken cancellationToken)
        {
            var param = new SingleImageUploadParam()
            {
                Request = Web.Request,
                FormFile = formFile,
                RootPath = Web.WebRootPath,
                Module = "upload",
                Group = "image",

                //*******裁剪和等比缩放 二者取其一*******
                //是否开自动裁剪原图（根据自定大小，自动对宽高裁剪）
                //IsCutOriginal = true,
                //AutoCutSize = 800,

                //是否开启等比缩放原图（根据等比大小和压缩质量裁剪）
                IsZoomOriginal = true,
                Ratio = 50,
                Quality = 100,

                //ThumbCutMode = ThumbnailMode.Cut,
                //Thumbs = new List<string> { "200x300", "400x200" },
            };

            var result = await _fileUploadService.UploadImageAsync(param, cancellationToken);

            result.Url = $"https://www.xx.com/{result.FullPath.Replace("\\", "/")}";

            // oss 单文件上传

            //var client = _ossFactory.GetClient("images");

            //(var res1, string message) = client.Delete("upload/images/master/2019/11/28/test111111.png");

            //(var res, string url) = client.Upload("upload/images/master/2019/11/28/test111111.png", @"C:\Users\Nigel\Downloads\QQ图片20200611104303.png");

            return RestFull.Success(data: result);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("/api/[Controller]/Base64")]
        public async Task<Result<ImageFileInfo>> UploadImage([Required][FromBody] Base64Command request, CancellationToken cancellationToken)
        {
            var param = new SingleImageBase64UploadParam()
            {
                Base64String = request.Base64,
                RootPath = Web.WebRootPath,
                Module = "upload",
                Group = "image",

                //*******裁剪和等比缩放 二者取其一*******
                //是否开自动裁剪原图（根据自定大小，自动对宽高裁剪）
                //IsCutOriginal = true, 
                //AutoCutSize = 800,

                //是否开启等比缩放原图（根据等比大小和压缩质量裁剪）
                IsZoomOriginal = true,
                Ratio = 50,
                Quality = 100,

                //ThumbCutMode = ThumbnailMode.Cut,
                //Thumbs = new List<string> { "200x200", "400x400" },
            };

            var result = await _fileUploadService.UploadImageAsync(param, cancellationToken);

            result.Url = $"https://www.xx.com/{result.FullPath.Replace("\\", "/")}";

            // oss 单文件上传

            //var client = _ossFactory.GetClient("images");

            //(var res1, string message) = client.Delete("upload/images/master/2019/11/28/test111111.png");

            //(var res, string url) = client.Upload("upload/images/master/2019/11/28/test111111.png", @"C:\Users\Nigel\Downloads\QQ图片20200611104303.png");

            return RestFull.Success(data: result);
        }
    }
}
