
### excel 大文件读取（逐行读取）

源码来自  https://github.com/ChrisHodges/XUCore.Excel

读取过程中会临时存储位置和内容，所以如果文件过大，请勿用该方式。会造成内存溢出


```csharp

    using var excelReader = new ExcelReader(@"C:\Users\Nigel\Downloads\1.xlsx");
    using var sheetReader = excelReader[0];

    for (var ndx = 1; ndx <= sheetReader.MaxRow; ndx++)
    {
        var row = sheetReader.Row(ndx).ToArray();
    }

```

在大文件读取中，请使用下面的方法

有时候在读取excel的时候（非wps或office工具编辑保存后），会读不到内容。

是因为通过程序输出的excel中，没有对xml表头的 `dimension ref="A1:..."` 属性进行更新。导致无法读到大小。

从而我们可以使用下面方式来进行读取。

```csharp

    using var excelReader = new ExcelReader(@"C:\Users\Nigel\Downloads\1.xlsx");
    using var sheetReader = excelReader[0];

    sheetReader.ReadNextInRow(1, 63, out int rowCount, (index, row) =>
    {

    });

```