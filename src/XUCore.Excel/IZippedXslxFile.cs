using System;
using System.IO;

namespace XUCore.Excel
{
    public interface IZippedXslxFile : IDisposable
    {
        Stream WorkbookXml { get; }
        XslxSharedStringsStream SharedStringsStream { get; }
        XslxIsDateTimeStream IsDateTimeStream { get; }
        Stream GetWorksheetStream(int i);
    }
}