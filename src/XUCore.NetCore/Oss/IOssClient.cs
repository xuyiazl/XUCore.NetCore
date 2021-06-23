using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XUCore.NetCore.Oss
{
    public interface IOssClient
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="filePath"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        (bool res, string url) Upload(string relativePath, string filePath, string md5 = "");
        /// <summary>
        ///  上传文件
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="stream"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        (bool res, string url) Upload(string relativePath, Stream stream, string md5 = "");
        /// <summary>
        /// 大文件上传
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="filePath"></param>
        (bool res, string message) MutiPartUpload(string relativePath, string filePath);
        /// <summary>
        /// 获取Bucket列表 
        /// </summary>
        /// <returns></returns>
        IList<string> GetBucketsList();
        /// <summary>
        /// 获取Bucket中的文件列表
        /// </summary>
        /// <returns></returns>
        IList<string> GetObjectlist();
        /// <summary>
        /// 删除一个Object
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        (bool res, string message) Delete(string relativePath);
        /// <summary>
        /// 删除一个Bucket和其中的Objects 
        /// </summary>
        void Delete();
    }
}
