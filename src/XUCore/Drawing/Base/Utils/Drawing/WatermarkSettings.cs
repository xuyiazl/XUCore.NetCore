using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace XUCore.Drawing
{
    /// <summary>
    /// 水印设置
    /// </summary>
    public class WatermarkSettings
    {
        /// <summary>
        /// 是否启用文本水印
        /// </summary>
        public bool WatermarkTextEnable { get; set; } = false;

        /// <summary>
        /// 水印文本
        /// </summary>
        public string WatermarkText { get; set; } = "";

        /// <summary>
        /// 文字水印字体
        /// </summary>
        public string WatermarkFont { get; set; } = "Arial";

        /// <summary>
        /// 文本水印颜色
        /// </summary>
        public Color TextColor { get; set; } = Color.White;

        /// <summary>
        /// 水印文本旋转角度
        /// </summary>
        public int TextRotatedDegree { get; set; } = 0;

        /// <summary>
        /// 文本水印的公共设置
        /// </summary>
        public CommonSettings TextSettings { get; set; } = new CommonSettings();

        /// <summary>
        /// 图片水印是否启用
        /// </summary>
        public bool WatermarkPictureEnable { get; set; } = false;

        /// <summary>
        /// 图片水印的公共设置
        /// </summary>
        public CommonSettings PictureSettings { get; set; } = new CommonSettings();

        /// <summary>
        /// 加水印最小图片宽度
        /// </summary>
        public int MinimumImageWidthForWatermark { get; set; } = 150;
        /// <summary>
        /// 加水印最小图片高度
        /// </summary>
        public int MinimumImageHeightForWatermark { get; set; } = 150;
    }

    /// <summary>
    /// 水印位置
    /// </summary>
    public enum WatermarkPosition
    {
        /// <summary>
        /// 左上角
        /// </summary>
        TopLeftCorner,
        /// <summary>
        /// 中上
        /// </summary>
        TopCenter,
        /// <summary>
        /// 右上角
        /// </summary>
        TopRightCorner,
        /// <summary>
        /// 左中
        /// </summary>
        CenterLeft,
        /// <summary>
        /// 居中
        /// </summary>
        Center,
        /// <summary>
        /// 右中
        /// </summary>
        CenterRight,
        /// <summary>
        /// 左下角
        /// </summary>
        BottomLeftCorner,
        /// <summary>
        /// 中下
        /// </summary>
        BottomCenter,
        /// <summary>
        /// 右下角
        /// </summary>
        BottomRightCorner,
    }

    /// <summary>
    /// 公共设置
    /// </summary>
    public class CommonSettings
    {
        /// <summary>
        /// 相对于原始图像的水印大小仅水印文字有效(%)
        /// </summary>
        public int Size { get; set; } = 20;
        /// <summary>
        /// 水印位置
        /// </summary>
        public List<WatermarkPosition> PositionList { get; set; } = new List<WatermarkPosition>(new WatermarkPosition[] { WatermarkPosition.BottomRightCorner });
        /// <summary>
        /// 透明度,0透明、1不透明(0-1)
        /// </summary>
        public double Opacity { get; set; } = 1;
    }

}