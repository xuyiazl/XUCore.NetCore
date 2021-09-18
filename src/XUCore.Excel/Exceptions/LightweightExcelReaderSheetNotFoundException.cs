using System;

namespace XUCore.Excel.Exceptions
{
    public class ExcelSheetNotFoundException : Exception
    {
        public ExcelSheetNotFoundException(string sheetName) : base(
            $"Sheet with name '{sheetName}' was not found in the workbook")
        {
        }

        public ExcelSheetNotFoundException(int sheetNumber, int numberOfSheets):base($"Sheet with zero-based index {sheetNumber} not found in the workbook. Workbook contains {numberOfSheets} sheets")
        {
        }
    }
}