using Microsoft.AspNetCore.Http;
using System.IO;

namespace XUCore.NetCore.Uploads.Params
{
    /// <summary>
    /// 单文件上传参数
    /// </summary>
    public class SingleFileUploadParam : SingleFileUploadParamBase
    {
        /// <summary>
        /// 当前请求
        /// </summary>
        public HttpRequest Request { get; set; }

        /// <summary>
        /// 上传的文件对象
        /// </summary>
        public IFormFile FormFile { get; set; }
    }
}