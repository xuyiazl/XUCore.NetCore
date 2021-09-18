
### excel 大文件读取（逐行读取）

源码来自  https://github.com/ChrisHodges/XUCore.Excel

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

有时候在读取excel的时候（非wps或office工具编辑保存后），会读不到内容。

是因为通过程序输出的excel中，没有对xml表头的 `dimension ref="A1:..."` 属性进行更新。导致无法读到大小。

从而我们可以使用下面方式来进行读取。

```csharp

    using (var fileSteam = File.Open(@"C:\Users\Nigel\Downloads\1.xlsx", FileMode.Open))
    {
        var list = new List<object>();

        var rowCount = 0;
        
        //逐行读取（弥补大部分文件没有更新 dimension ref="A1:..." 导致无法获取到行数和列数的问题）
        sheetReader.ReadNextInRow(1, 2, out rowCount, (index, row) =>
        {
            list.Add(row);
        });
        
        //正常情况下的读取
        for (var ndx = 2; ndx <= sheetReader.MaxRow; ndx++)
        {
            var row = sheetReader.Row(ndx).ToArray();
        }
    }
```