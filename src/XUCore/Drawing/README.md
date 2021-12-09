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
                string savePath = "";
                var filePath = "";
                var sourceImage = ImageHelper.FromFile(filePath);

                var watermark = new WatermarkProcess();
                var img = watermark.MakeImageWatermark(sourceImage, new WatermarkSettings()
                {
                    WatermarkTextEnable = true,
                    WatermarkText = "优惠活动专用水印",
                    TextColor = Color.Red,
                    TextRotatedDegree = -45,
                    WatermarkPictureEnable = true,
                    TextSettings = new CommonSettings()
                    {
                        Size = 60,
                        Opacity = 0.4,
                        PositionList = new List<WatermarkPosition>(new WatermarkPosition[] { WatermarkPosition.Center })
                    },
                    PictureSettings = new CommonSettings()
                    {
                        Opacity = 0.5,
                        PositionList = new List<WatermarkPosition>(new WatermarkPosition[] { WatermarkPosition.BottomLeftCorner })
                    }
                });
                img.Save(savePath, ImageFormat.Jpeg);
                img.Dispose();
                
```