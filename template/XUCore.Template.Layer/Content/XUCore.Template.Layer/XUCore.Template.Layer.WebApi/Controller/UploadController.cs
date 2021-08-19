using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.NetCore.Uploads.Params;
using XUCore.Template.Layer.Applaction;
using XUCore.Template.Layer.Applaction.Upload;

namespace XUCore.Template.Layer.WebApi.Controller
{
    /// <summary>
    /// 文件上传
    /// </summary>
    [ApiExplorerSettings(GroupName = ApiGroup.Admin)]
    public class UploadController : ApiControllerBase
    {
        private readonly IUploadAppService uploadAppService;
        public UploadController(ILogger<UploadController> logger, IUploadAppService uploadAppService) : base(logger)
        {
            this.uploadAppService = uploadAppService;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("File")]
        public async Task<Result<XUCore.Files.FileInfo>> UploadFile([Required] IFormFile formFile, CancellationToken cancellationToken)
        {
            return await uploadAppService.UploadFile(formFile, cancellationToken);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("Image")]
        public async Task<Result<ImageFileInfo>> UploadImage([Required] IFormFile formFile, CancellationToken cancellationToken)
        {
            return await uploadAppService.UploadImage(formFile, cancellationToken);
        }
        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("Base64")]
        public async Task<Result<ImageFileInfo>> UploadBase64([Required][FromBody] Base64Command command, CancellationToken cancellationToken)
        {
            return await uploadAppService.UploadImage(command, cancellationToken);
        }
    }
}
