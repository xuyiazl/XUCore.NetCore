
### excel 大文件读取（逐行读取）

源码来自  https://github.com/ChrisHodges/LightweightExcelReader

原插件有几个缺陷：

1、资源打开未释放的问题。

2、在使用Hash存储的同时，没考虑单个对象超过2G内存占用，以及字典自动扩充空间的时候会成倍分配内存的问题。

3、目前应该是大部分插件都没有考虑到在读取excel的单元格起始位置 `dimension ref="A1:..."`的问题。当没有读取到A1:BK4490起始位置的时候，无法正确计算和读取excel表格的问题。



#### 常规读取

文件过大可能会造成内存溢出。已针对源码进行了内存存储优化。

原源码是用单个字典存储所有单元格。已知的问题是Hash字典是单个对象，所以在存储到一定限度的时候，会自动申请内存（成倍申请），此时会造成内存不足。

```csharp

    using var excelReader = new ExcelReader(@"C:\Users\Nigel\Downloads\1.xlsx");
    using var sheetReader = excelReader[0];

    for (var ndx = 1; ndx <= sheetReader.MaxRow; ndx++)
    {
        var row = sheetReader.Row(ndx).ToArray();
    }

```

#### 在大文件读取中，请使用下面的方法

我们经常在处理excel的时候（文件从其他系统打包下载的数据，而非wps或office工具编辑保存后），会读不到内容。

是因为通过程序输出的excel中，没有对xml表头的 `dimension ref="A1:..."` 属性进行更新。导致无法读到大小。

从而我们可以使用下面方式来进行读取。

```csharp

    using var excelReader = new ExcelReader(@"C:\Users\Nigel\Downloads\1.xlsx");
    using var sheetReader = excelReader[0];

    sheetReader.ReadNextInRow(1, 63, out int rowCount, (index, row) =>
    {

    });

```
` 1 ` 为列的开始位置，一半我们是从1开始读取。
` 63 ` 为列的最后位置，或者想读取的位置。
当然我们可以读取指定的列范围，比如：想读取中间 `5-30` 位置。那么开始位置设置为5，结束位置为30。
