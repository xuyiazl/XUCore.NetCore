using XUCore.Drawing;
using System.Collections.Generic;

namespace XUCore.NetCore.Uploads
{
    public class SingleImageBase64UploadParam : SingleFileUploadParamBase
    {
        /// <summary>
        /// 图片base64
        /// </summary>
        public string Base64String { get; set; }

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
        /// 缩略图裁剪方式（原图不动，若设置了裁剪尺寸，则启用该选项）
        /// </summary>
        public ThumbnailMode ThumbCutMode { get; set; }

        /// <summary>
        /// 裁剪缩略图尺寸 item = 300x400
        /// </summary>
        public List<string> Thumbs { get; set; }
    }
}