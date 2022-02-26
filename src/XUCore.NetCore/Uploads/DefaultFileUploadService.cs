using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using XUCore.Extensions;
using XUCore.Files;
using XUCore.IO;
using FileInfo = XUCore.Files.FileInfo;

namespace XUCore.NetCore.Uploads
{
    /// <summary>
    /// 默认文件上传服务
    /// </summary>
    internal class DefaultFileUploadService : IFileUploadService
    {
        /// <summary>
        /// 上传文件。单文件
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<FileInfo> UploadAsync(SingleFileUploadParam param, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (param.FormFile == null || param.FormFile.Length < 1)
            {
                if (param.Request.Form.Files != null && param.Request.Form.Files.Any())
                {
                    param.FormFile = param.Request.Form.Files[0];
                }
            }

            if (param.FormFile == null || param.FormFile.Length < 1)
                throw new ArgumentNullException("请选择文件!");

            return await SaveAsync(param.FormFile, param.RelativePath, param.RootPath, cancellationToken);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="formFile">表单文件</param>
        /// <param name="relativePath">相对路径</param>
        /// <param name="rootPath">根路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        private async Task<FileInfo> SaveAsync(IFormFile formFile, string relativePath, string rootPath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var date = DateTime.Now;

            var name = formFile.FileName;
            var size = formFile.Length;
            var path = System.IO.Path.Combine(relativePath, date.ToString("yyyyMMdd"));
            var id = Guid.NewGuid();
            var fileInfo = new FileInfo(path, size, name, id.ToString());
            fileInfo.SaveName = $"{id.ToString().Replace("-", "")}.{fileInfo.Extension}";

            var fullDir = System.IO.Path.Combine(rootPath, fileInfo.Path);
            if (!Directory.Exists(fullDir))
                Directory.CreateDirectory(fullDir);

            var fullPath = Path.Combine(fullDir, fileInfo.SaveName);
            fileInfo.Md5 = await SaveWithMd5Async(formFile, fullPath, cancellationToken);
            return fileInfo;
        }

        /// <summary>
        /// 上传文件。多文件
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<IEnumerable<FileInfo>> UploadAsync(MultipleFileUploadParam param, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (param.FormFiles == null || !param.FormFiles.Any())
            {
                if (param.Request.Form.Files != null && param.Request.Form.Files.Any())
                {
                    param.FormFiles = param.Request.Form.Files.ToList();
                }
            }

            if (param.FormFiles == null || !param.FormFiles.Any())
                throw new ArgumentNullException("请选择文件!");

            var tasks = new List<Task<FileInfo>>();
            foreach (var formFile in param.FormFiles)
            {
                tasks.Add(SaveAsync(formFile, param.RelativePath, param.RootPath, cancellationToken));
            }

            return await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 上传图片。单张图片
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<ImageFileInfo> UploadImageAsync(SingleImageUploadParam param, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (param.FormFile == null || param.FormFile.Length < 1)
            {
                if (param.Request.Form.Files != null && param.Request.Form.Files.Any())
                {
                    param.FormFile = param.Request.Form.Files[0];
                }
            }

            if (param.FormFile == null || param.FormFile.Length < 1)
                throw new ArgumentNullException("请选择文件!");

            string fileExt = FileHelper.GetExtension(param.FormFile.FileName);

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(param.Extensions, fileExt.ToLower()) == -1)
                throw new Exception("上传图片扩展名是不允许的扩展名。只允许" + param.Extensions.Join(",") + "格式。");

            if (param.FormFile.Length > param.Size)
                throw new Exception($"上传文件大小超过限制。图片大小不能超过{new FileSize(param.Size, FileSizeUnit.Byte).ToString()}。");

            var imageInfo = await SaveImageAsync(param.FormFile, param.RelativePath, param.RootPath, cancellationToken);

            if (param.IsZoomOriginal)
            {
                string orgin = Path.Combine(param.RootPath, imageInfo.FullPath);
                string thumPath = $"{orgin}-tmp";

                using var source = Image.Load(orgin, out IImageFormat format);

                source.Mutate(ctx =>
                {
                    var ratio = param.Ratio / 100.0;
                    ctx.Resize((int)(source.Width * ratio), (int)(source.Height * ratio));
                });

                var jpegEncoder = new JpegEncoder { Quality = param.Quality };

                await source.SaveAsync(thumPath, jpegEncoder, cancellationToken);

                File.Move(thumPath, orgin, true);
            }

            if (param.IsCutOriginal)
            {
                string orgin = Path.Combine(param.RootPath, imageInfo.FullPath);
                string thumPath = $"{orgin}-tmp";

                using var source = Image.Load(orgin, out IImageFormat format);

                source.Mutate(ctx =>
                {
                    var towidth = source.Width;
                    var toheight = source.Height;
                    if (source.Width >= source.Height && source.Width > param.AutoCutSize)
                    {
                        towidth = param.AutoCutSize;
                        toheight = source.Height * param.AutoCutSize / source.Width;
                    }
                    if (source.Height >= source.Width && source.Height > param.AutoCutSize)
                    {
                        towidth = source.Width * param.AutoCutSize / source.Height;
                        toheight = param.AutoCutSize;
                    }
                    ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Size = new Size(towidth, toheight)
                    });
                });

                var jpegEncoder = new JpegEncoder { Quality = 100 };

                await source.SaveAsync(thumPath, jpegEncoder, cancellationToken);

                File.Move(thumPath, orgin, true);
            }

            if (param.Thumbs == null || param.Thumbs.Count == 0)
                return imageInfo;

            foreach (var thumbSize in param.Thumbs)
            {
                var _size = thumbSize.Split('x', StringSplitOptions.RemoveEmptyEntries);
                if (_size.Length < 2) continue;

                var thumbPath = Path.Combine(imageInfo.Path, $"{imageInfo.Id.ToString().Replace("-", "")}-{thumbSize}.{imageInfo.Extension}");
                var thumb = Path.Combine(param.RootPath, thumbPath);
                var orgin = Path.Combine(param.RootPath, imageInfo.FullPath);

                using var source = Image.Load(orgin, out IImageFormat format);

                source.Mutate(ctx =>
                {
                    var towidth = _size[0].ToInt();
                    var toheight = _size[1].ToInt();

                    ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Position = AnchorPositionMode.Center,
                        Size = new Size(towidth, toheight)
                    });
                });

                var jpegEncoder = new JpegEncoder { Quality = 100 };

                await source.SaveAsync(thumb, jpegEncoder,cancellationToken);

                imageInfo.Thumbs.TryAdd(thumbSize, thumbPath);
            }

            return imageInfo;
        }


        /// <summary>
        /// 上传Base64图片。单张图片
        /// </summary>
        /// <param name="param">参数</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<ImageFileInfo> UploadImageAsync(SingleImageBase64UploadParam param, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (param.Base64String.IsEmpty())
                throw new ArgumentNullException("请传递图片Base64字符串!");

            var imageInfo = await SaveImageAsync(param.Base64String, param.RelativePath, param.RootPath, cancellationToken);


            if (param.IsZoomOriginal)
            {
                string orgin = Path.Combine(param.RootPath, imageInfo.FullPath);
                string thumPath = $"{orgin}-tmp";

                using var source = Image.Load(orgin, out IImageFormat format);

                source.Mutate(ctx =>
                {
                    var ratio = param.Ratio / 100.0;
                    ctx.Resize((int)(source.Width * ratio), (int)(source.Height * ratio));
                });

                var jpegEncoder = new JpegEncoder { Quality = param.Quality };

                await source.SaveAsync(thumPath, jpegEncoder, cancellationToken);

                File.Move(thumPath, orgin, true);
            }

            if (param.IsCutOriginal)
            {
                string orgin = Path.Combine(param.RootPath, imageInfo.FullPath);
                string thumPath = $"{orgin}-tmp";

                using var source = Image.Load(orgin, out IImageFormat format);

                source.Mutate(ctx =>
                {
                    var towidth = source.Width;
                    var toheight = source.Height;
                    if (source.Width >= source.Height && source.Width > param.AutoCutSize)
                    {
                        towidth = param.AutoCutSize;
                        toheight = source.Height * param.AutoCutSize / source.Width;
                    }
                    if (source.Height >= source.Width && source.Height > param.AutoCutSize)
                    {
                        towidth = source.Width * param.AutoCutSize / source.Height;
                        toheight = param.AutoCutSize;
                    }
                    ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Size = new Size(towidth, toheight)
                    });
                });

                var jpegEncoder = new JpegEncoder { Quality = 100 };

                await source.SaveAsync(thumPath, jpegEncoder, cancellationToken);

                File.Move(thumPath, orgin, true);
            }

            if (param.Thumbs == null || param.Thumbs.Count == 0)
                return imageInfo;

            foreach (var thumbSize in param.Thumbs)
            {
                var _size = thumbSize.Split('x', StringSplitOptions.RemoveEmptyEntries);
                if (_size.Length < 2) continue;

                var thumbPath = Path.Combine(imageInfo.Path, $"{imageInfo.Id.ToString().Replace("-", "")}-{thumbSize}.{imageInfo.Extension}");
                var thumb = Path.Combine(param.RootPath, thumbPath);
                var orgin = Path.Combine(param.RootPath, imageInfo.FullPath);

                using var source = Image.Load(orgin, out IImageFormat format);

                source.Mutate(ctx =>
                {
                    var towidth = _size[0].ToInt();
                    var toheight = _size[1].ToInt();

                    ctx.Resize(new ResizeOptions
                    {
                        Mode = ResizeMode.Crop,
                        Position = AnchorPositionMode.Center,
                        Size = new Size(towidth, toheight)
                    });
                });

                var jpegEncoder = new JpegEncoder { Quality = 100 };

                await source.SaveAsync(thumb, jpegEncoder, cancellationToken);

                imageInfo.Thumbs.TryAdd(thumbSize, thumbPath);
            }

            return imageInfo;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="base64String">base64字符串</param>
        /// <param name="relativePath">相对路径</param>
        /// <param name="rootPath">根路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        private async Task<ImageFileInfo> SaveImageAsync(string base64String, string relativePath, string rootPath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var date = DateTime.Now;

            var id = Guid.NewGuid().ToString();
            var name = $"{id.Replace("-", "")}.jpg";
            var size = 0L;
            var path = Path.Combine(relativePath, date.ToString("yyyyMMdd"));
            var saveName = name;

            var fullDir = Path.Combine(rootPath, path);

            if (!Directory.Exists(fullDir))
                Directory.CreateDirectory(fullDir);

            var fullPath = Path.Combine(fullDir, saveName);

            var md5 = "";

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                byte[] bytes = XUCore.Drawing.ImageHelper.GetBytesFromBase64String(base64String);

                await stream.WriteAsync(bytes, cancellationToken);

                size = bytes.Length;
                md5 = Md5(stream);
            }

            var imageInfo = new ImageFileInfo(path, size, name, id);
            imageInfo.SaveName = saveName;
            imageInfo.Md5 = md5;

            return imageInfo;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="formFile">表单文件</param>
        /// <param name="relativePath">相对路径</param>
        /// <param name="rootPath">根路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        private async Task<ImageFileInfo> SaveImageAsync(IFormFile formFile, string relativePath, string rootPath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var date = DateTime.Now;

            var name = formFile.FileName;
            var size = formFile.Length;
            var path = System.IO.Path.Combine(relativePath, date.ToString("yyyyMMdd"));
            var id = Guid.NewGuid();
            var fileInfo = new ImageFileInfo(path, size, name, id.ToString());
            fileInfo.SaveName = $"{id.ToString().Replace("-", "")}.{fileInfo.Extension}";

            var fullDir = System.IO.Path.Combine(rootPath, fileInfo.Path);
            if (!Directory.Exists(fullDir))
            {
                Directory.CreateDirectory(fullDir);
            }

            var fullPath = Path.Combine(fullDir, fileInfo.SaveName);
            fileInfo.Md5 = await SaveWithMd5Async(formFile, fullPath, cancellationToken);
            return fileInfo;
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="formFile">表单文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task SaveAsync(IFormFile formFile, string savePath, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream, cancellationToken);
            }
        }

        /// <summary>
        /// 保存文件并返回文件MD5值
        /// </summary>
        /// <param name="formFile">表单文件</param>
        /// <param name="savePath">保存路径</param>
        /// <param name="cancellationToken">取消令牌</param>
        public async Task<string> SaveWithMd5Async(IFormFile formFile, string savePath,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            string md5;
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                md5 = Md5(stream);
                await formFile.CopyToAsync(stream, cancellationToken);
            }

            return md5;
        }

        /// <summary>
        /// MD5哈希
        /// </summary>
        /// <param name="stream">流</param>
        private static string Md5(Stream stream)
        {
            if (stream == null)
            {
                return string.Empty;
            }

            using (var md5Hash = MD5.Create())
            {
                return BitConverter.ToString(md5Hash.ComputeHash(stream)).Replace("-", "");
            }
        }
    }
}