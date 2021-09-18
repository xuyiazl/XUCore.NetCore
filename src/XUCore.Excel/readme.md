
### excel 大文件读取（逐行读取）

源码来自  https://github.com/ChrisHodges/LightweightExcelReader

修改了在读取数据的时候，去掉了原作者的hash存储。避免了hash造成的内存溢出的问题


```csharp

    using (var fileSteam = File.Open(@"C:\Users\Nigel\Downloads\1.xlsx", FileMode.Open))
    {
        var excelReader = new ExcelReader(fileSteam);
        var sheetReader = excelReader["sheet2"];

        for (var ndx = 2; ndx <= sheetReader.MaxRow; ndx++)
        {
            var row = sheetReader.Row(ndx).ToArray();

        }

    }

```