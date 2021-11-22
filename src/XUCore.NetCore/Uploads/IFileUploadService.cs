using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FileInfo = XUCore.Files.FileInfo;

namespace XUCore.NetCore.Uploads
{
    /// <summary>
    /// 文件上传服务
    /// </summary>
    public interface IFileUploadService
    {
        /// <summary>
        /// 上传文件。单文件
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<FileInfo> UploadAsync(SingleFileUploadParam param, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 上传图片。单张图片
        /// </summary>
        /// <param name="param"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ImageFileInfo> UploadImageAsync(SingleImageUploadParam param, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// 上传Base64图片。单张图片
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<ImageFileInfo> UploadImageAsync(SingleImageBase64UploadParam param, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 上传文件。多文件
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<IEnumerable<FileInfo>> UploadAsync(MultipleFileUploadParam param, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="formFile">表单文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task SaveAsync(IFormFile formFile, string savePath, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// 保存文件并返回文件MD5值
        /// </summary>
        /// <param name="formFile">表单文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task<string> SaveWithMd5Async(IFormFile formFile, string savePath, CancellationToken cancellationToken = default(CancellationToken));
    }
}