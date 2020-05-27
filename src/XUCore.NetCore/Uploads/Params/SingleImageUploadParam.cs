using XUCore.Drawing;
using System.Collections.Generic;

namespace XUCore.NetCore.Uploads.Params
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
        public long Size { get; set; } = 1024 * 1024 * 2;

        /// <summary>
        /// 是否裁剪原图
        /// </summary>
        public bool IsCutOriginal { get; set; } = false;

        /// <summary>
        /// 裁剪原图的宽度
        /// </summary>
        public int OriginalWidth { get; set; }

        /// <summary>
        /// 裁剪原图的高度
        /// </summary>
        public int OriginalHeight { get; set; }

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