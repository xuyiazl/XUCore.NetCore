using XUCore.Files.Paths;
using XUCore.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace XUCore.Files
{
    /// <summary>
    /// 本地文件存储服务
    /// </summary>
    public class DefaultFileStore : IFileStore
    {
        /// <summary>
        /// 路径生成器
        /// </summary>
        private readonly IPathGenerator _pathGenerator;

        /// <summary>
        /// 初始化一个<see cref="DefaultFileStore"/>类型的实例
        /// </summary>
        /// <param name="pathGenerator">路径生成器</param>
        public DefaultFileStore(IPathGenerator pathGenerator)
        {
            _pathGenerator = pathGenerator;
        }

        /// <summary>
        /// 保存文件，返回完整文件路径
        /// </summary>
        /// <param name="file"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public async Task<string> SaveAsync(IFormFile file, string savePath)
        {
            var path = _pathGenerator.Generate(file.FileName);
            var directory = Path.GetDirectoryName(savePath);
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("上传失败");
            }

            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return path;
        }
    }
}