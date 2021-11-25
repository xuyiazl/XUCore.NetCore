using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace XUCore.Files
{
    /// <summary>
    /// 文件存储服务
    /// </summary>
    public interface IFileStore
    {
        /// <summary>
        /// 保存文件，返回完整文件路径
        /// </summary>
        /// <param name="file"></param>
        /// <param name="savePath">保存地址</param>
        /// <returns></returns>
        Task<string> SaveAsync(IFormFile file, string savePath);
    }
}