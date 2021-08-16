using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;
using XUCore.NetCore;
using XUCore.NetCore.Uploads.Params;

namespace XUCore.Template.Easy.Applaction.Upload
{
    /// <summary>
    /// 文件上传
    /// </summary>
    public interface IUploadAppService : IAppService
    {
        Task<Result<XUCore.Files.FileInfo>> UploadFile(IFormFile formFile, CancellationToken cancellationToken);
        Task<Result<ImageFileInfo>> UploadImage(IFormFile formFile, CancellationToken cancellationToken);
        Task<Result<ImageFileInfo>> UploadImage(Base64Command model, CancellationToken cancellationToken);
    }
}
