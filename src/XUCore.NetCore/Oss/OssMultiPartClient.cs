using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace XUCore.NetCore.Oss
{
    public class OssMultiPartClient : IOssMultiPartClient
    {
        private Aliyun.OSS.OssClient ossClient;

        private readonly OssOptions options;

        private InitiateMultipartUploadRequest initRequest;
        private InitiateMultipartUploadResult initResult;

        // 新建一个List保存每个分块上传后的ETag和PartNumber   
        private List<PartETag> partETags;
        private string relativePath;

        public OssMultiPartClient(OssOptions options)
        {
            this.options = options;

            ossClient = new Aliyun.OSS.OssClient(options.EndPoint, options.AccessKey, options.AccessKeySecret);

        }

        public void Create(string relativePath)
        {
            this.relativePath = relativePath;

            initRequest = new InitiateMultipartUploadRequest(options.BluckName, relativePath);
            initResult = ossClient.InitiateMultipartUpload(initRequest);

            partETags = new List<PartETag>();
        }

        public bool Upload(Stream stream, long size, int number)
        {
            if (!partETags.Any(c => c.PartNumber == number))
            {
                // 创建UploadPartRequest，上传分块   
                UploadPartRequest uploadPartRequest = new UploadPartRequest(options.BluckName, relativePath, initResult.UploadId);
                uploadPartRequest.InputStream = stream;
                uploadPartRequest.PartSize = size;
                uploadPartRequest.PartNumber = number;
                UploadPartResult uploadPartResult = ossClient.UploadPart(uploadPartRequest);
                if (uploadPartResult.HttpStatusCode == HttpStatusCode.OK)
                {
                    // 将返回的PartETag保存到List中。   
                    partETags.Add(uploadPartResult.PartETag);
                    return true;
                }
            }
            return false;
        }

        public (bool res, string url) Done()
        {
            CompleteMultipartUploadRequest completeReq = new CompleteMultipartUploadRequest(options.BluckName, relativePath, initResult.UploadId);
            foreach (PartETag partETag in partETags.OrderBy(c => c.PartNumber).ToList())
            {
                completeReq.PartETags.Add(partETag);
            }
            //完成分块上传   
            CompleteMultipartUploadResult completeResult = ossClient.CompleteMultipartUpload(completeReq);
            //返回最终文件的MD5，用于用户进行校验 
            if (completeResult.HttpStatusCode == HttpStatusCode.OK)
            {
                return (true, $"{options.Domain}/{relativePath}");
            }
            else
            {
                return (false, "保存失败");
            }
        }
    }
}
