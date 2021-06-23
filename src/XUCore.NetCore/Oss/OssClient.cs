using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace XUCore.NetCore.Oss
{
    public class OssClient : IOssClient
    {
        private Aliyun.OSS.OssClient ossClient;

        private readonly OssOptions options;

        public OssClient(OssOptions options)
        {
            this.options = options;
            ossClient = new Aliyun.OSS.OssClient(options.EndPoint, options.AccessKey, options.AccessKeySecret);

            //if (!ossClient.DoesBucketExist(options.BluckName))
            //    ossClient.CreateBucket(options.BluckName);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="filePath"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        public (bool res, string url) Upload(string relativePath, string filePath, string md5 = "")
        {
            using (FileStream readStream = File.Open(filePath, FileMode.Open))
                return Upload(relativePath, readStream, md5);
        }
        /// <summary>
        ///  上传文件
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="stream"></param>
        /// <param name="md5"></param>
        /// <returns></returns>
        public (bool res, string url) Upload(string relativePath, Stream stream, string md5 = "")
        {
            ObjectMetadata objectMeta = new ObjectMetadata
            {
                ContentLength = stream.Length
            };
            PutObjectResult result = ossClient.PutObject(options.BluckName, relativePath, stream, objectMeta);

            if (result.HttpStatusCode == HttpStatusCode.OK)
            {
                if (!string.IsNullOrEmpty(md5))
                {
                    if (result.ETag != md5.ToUpper())
                        return (false, "MD5值与上传文件不匹配");
                }

                return (true, $"{options.Domain}/{relativePath}");
            }
            return (false, "上传失败");
        }
        /// <summary>
        /// 大文件上传
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="filePath"></param>
        public (bool res, string message) MutiPartUpload(string relativePath, string filePath)
        {
            try
            {
                InitiateMultipartUploadRequest initRequest = new InitiateMultipartUploadRequest(options.BluckName, relativePath);
                InitiateMultipartUploadResult initResult = ossClient.InitiateMultipartUpload(initRequest);

                // 设置每块为 5M   
                int partSize = 1024 * 1024 * 5;

                FileInfo partFile = new FileInfo(filePath);

                // 计算分块数目   
                int partCount = (int)(partFile.Length / partSize);
                if (partFile.Length % partSize != 0)
                {
                    partCount++;
                }

                // 新建一个List保存每个分块上传后的ETag和PartNumber   
                List<PartETag> partETags = new List<PartETag>();

                for (int i = 0; i < partCount; i++)
                {
                    // 获取文件流   
                    using (FileStream fis = new FileStream(partFile.FullName, FileMode.Open))
                    {

                        // 跳到每个分块的开头   
                        long skipBytes = partSize * i;
                        fis.Position = skipBytes;
                        //fis.skip(skipBytes);   

                        // 计算每个分块的大小   
                        long size = partSize < partFile.Length - skipBytes ?
                                partSize : partFile.Length - skipBytes;

                        // 创建UploadPartRequest，上传分块   
                        UploadPartRequest uploadPartRequest = new UploadPartRequest(options.BluckName, relativePath, initResult.UploadId);
                        uploadPartRequest.InputStream = fis;
                        uploadPartRequest.PartSize = size;
                        uploadPartRequest.PartNumber = (i + 1);
                        UploadPartResult uploadPartResult = ossClient.UploadPart(uploadPartRequest);

                        // 将返回的PartETag保存到List中。   
                        partETags.Add(uploadPartResult.PartETag);
                    }
                }

                CompleteMultipartUploadRequest completeReq = new CompleteMultipartUploadRequest(options.BluckName, relativePath, initResult.UploadId);
                foreach (PartETag partETag in partETags)
                {
                    completeReq.PartETags.Add(partETag);
                }

                //完成分块上传   
                CompleteMultipartUploadResult completeResult = ossClient.CompleteMultipartUpload(completeReq);

                //返回最终文件的MD5，用于用户进行校验 
                if (completeResult.HttpStatusCode == HttpStatusCode.OK)
                    return (true, $"{options.Domain}/{relativePath}");

                return (false, "上传失败");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
        /// <summary>
        /// 获取Bucket列表 
        /// </summary>
        /// <returns></returns>
        public IList<string> GetBucketsList()
        {
            IList<string> list = new List<string>();

            foreach (Bucket bt in ossClient.ListBuckets())
                list.Add($"名称：{bt.Name};创建日期：{bt.CreationDate}");

            return list;
        }
        /// <summary>
        /// 获取Bucket中的文件列表
        /// </summary>
        /// <returns></returns>
        public IList<string> GetObjectlist()
        {
            IList<string> res = new List<string>();

            ObjectListing list = ossClient.ListObjects(options.BluckName);

            IEnumerable<OssObjectSummary> objects = list.ObjectSummaries;

            foreach (OssObjectSummary ob in objects)
            {
                if (ob.Key.LastIndexOf('/') != ob.Key.Length - 1)
                    res.Add($"文件名：{ob.Key} 文件大小：{GetSize(ob.Size)}");
            }

            return res;
        }
        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        private string GetSize(long size)
        {
            if (size > 1024 * 1024)
                return size / (1024 * 1024) + "." + (size % (1024 * 1024)) / 1024 + "MB";
            else if (size > 1024)
                return size / 1024 + "." + (size % 1024) / 10 + "KB";
            else
                return size + "字节";
        }
        /// <summary>
        /// 删除一个Object
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        public (bool res, string message) Delete(string relativePath)
        {
            var res = ossClient.DeleteObject(options.BluckName, relativePath);

            if (res.HttpStatusCode == HttpStatusCode.OK)
                return (true, $"删除成功!文件：{relativePath}");

            return (false, "删除失败");
        }
        /// <summary>
        /// 删除一个Bucket和其中的Objects 
        /// </summary>
        public void Delete()
        {
            ObjectListing ol = ossClient.ListObjects(options.BluckName);

            List<OssObjectSummary> listDeletes = new List<OssObjectSummary>(ol.ObjectSummaries);

            foreach (OssObjectSummary s in listDeletes)
            {
                //删除指定bucket下的Bucket下的ObjectName
                ossClient.DeleteObject(options.BluckName, s.Key);
            }
            //删除bucket
            ossClient.DeleteBucket(options.BluckName);
        }
    }
}
