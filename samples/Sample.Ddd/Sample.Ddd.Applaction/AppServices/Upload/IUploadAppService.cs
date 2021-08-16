using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using Sample.Ddd.Applaction.Common.Interfaces;
using XUCore.NetCore;
using XUCore.NetCore.Uploads.Params;

namespace Sample.Ddd.Applaction.AppServices.Upload
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public interface IUploadAppService : IAppService
    {
        Task<Result<XUCore.Files.FileInfo>> UploadFile(IFormFile formFile, CancellationToken cancellationToken);
        Task<Result<ImageFileInfo>> UploadImage(IFormFile formFile, CancellationToken cancellationToken);
        Task<Result<ImageFileInfo>> UploadBase64(Base64Command model, CancellationToken cancellationToken);
    }
}