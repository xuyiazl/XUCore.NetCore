using System.Collections.Generic;

namespace XUCore.NetCore.Uploads
{
    public class SingleImageUploadParam : SingleFileUploadParam
    {
        /// <summary>
        /// 允许上传的文件扩展名
        /// </summary>
        public string[] Extensions { get; set; } = new string[] { "gif", "jpg", "jpeg", "png", "bmp" };

        /// <summary>
        /// 允许上传的图片文件大小，默认2M
        /// </summary>
        public long Size { get; set; } = 1024 * 1024 * 5;

        /// <summary>
        /// 是否等比缩放原图
        /// </summary>
        public bool IsZoomOriginal { get; set; } = false;

        /// <summary>
        /// 缩放比率（1-100）
        /// </summary>
        public int Ratio { get; set; } = 40;

        /// <summary>
        /// 压缩质量（数字越小压缩率越高）1-100
        /// </summary>
        public int Quality { get; set; } = 100;

        /// <summary>
        /// 是否裁剪原图
        /// </summary>
        public bool IsCutOriginal { get; set; } = false;

        /// <summary>
        /// 自动裁剪原图的最大高度和宽度
        /// </summary>
        public int AutoCutSize { get; set; }

        /// <summary>
        /// 裁剪缩略图尺寸 item = 300x400
        /// </summary>
        public List<string> Thumbs { get; set; }
    }
}