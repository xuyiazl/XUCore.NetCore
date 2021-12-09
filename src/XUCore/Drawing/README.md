# 验证码模块
## 部署Ubuntu或者Docker
执行一下命令：
```
sudo apt install libc6-dev
sudo apt install libgdiplus
```

``` csharp

//测试图片打水印
//文字水印
string savePath = "Y:\\p-1.jpg";
var filePath = "Y:\\p.jpg";
var sourceImage = ImageHelper.FromFile(filePath, true);

var watermark = new WatermarkProcess("Y:\\logo.jpg");
var img = watermark.MakeImageWatermark(sourceImage, new()
{
    WatermarkTextEnable = true,
    WatermarkText = "优惠活动专用水印",
    TextColor = Color.Red,
    TextRotatedDegree = -45,
    TextSettings = new()
    {
        Size = 60,
        Opacity = 0.4,
        PositionList = new(new[] { WatermarkPosition.Center })
    },
    WatermarkPictureEnable = true,
    PictureSettings = new()
    {
        Opacity = 0.5,
        PositionList = new(new[] { WatermarkPosition.BottomLeftCorner })
    }
});
img.Save(savePath, ImageFormat.Jpeg);
img.Dispose();
```