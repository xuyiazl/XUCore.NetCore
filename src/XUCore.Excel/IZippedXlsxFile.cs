using System;
using System.IO;

namespace XUCore.Excel
{
    internal interface IZippedXlsxFile : IDisposable
    {
        Stream WorkbookXml { get; }
        XslxSharedStringsStream SharedStringsStream { get; }
        XslxIsDateTimeStream IsDateTimeStream { get; }
        Stream GetWorksheetStream(int i);
    }
}