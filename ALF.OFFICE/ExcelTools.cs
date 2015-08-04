using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using ALF.MSSQL.DataModel;
using ALF.OFFICE.DataModel;
using Excel=Microsoft.Office.Interop.Excel;

namespace ALF.OFFICE
{
    /// <summary>
    /// Excel处理工具
    /// </summary>
    public static class ExcelTools
    {
        #region Private Fileds

        private static Excel.Application _objExcel;
        private static Excel.Workbooks _objBooks;
        private static Excel._Workbook _objBook;
        private static Excel._Worksheet _objSheet;
        private static Excel.Range _objRange;
        private static readonly object ObjOpt = System.Reflection.Missing.Value;
        private static int _tmpNumber;

        #endregion
        

        #region Public Methods

        /// <summary>
        /// 将DataTable逐个单元格写入Excel中
        /// </summary>
        /// <param name="dataTables">数据表列表</param>
        /// <param name="excelInfos">Excel信息列表</param>
        /// <param name="isSingleFile">是否导入到单个Excel文件</param>
        /// <returns>操作结果</returns>
        public static string WriteDataToExcel(List<DataTable> dataTables, List<ExcelInfo> excelInfos, bool isSingleFile = false)
        {

            if (dataTables.Count != excelInfos.Count)
            {
                return "The count of data doesn't match count of excel";
            }

            return isSingleFile
                       ? SaveDataToSingleExcel(dataTables, excelInfos)
                       : SaveDataToMultiExcel(dataTables, excelInfos);
        }

        /// <summary>
        /// 将Sql查询结果插入到指定Excel文件中
        /// </summary>
        /// <param name="sql">sql查询语句(注：SQL中需要包含数据库名)</param>
        /// <param name="filePath">excel文件所在路径</param>
        /// <param name="sheetName">所要插入页签名称</param>
        /// <returns>操作结果</returns>
        public static string ExportSqlToExcel(string sql, string filePath, string sheetName)
        {
            if (!File.Exists(filePath))
            {
                return string.Format("【SQL导入发生错误】: 错误语句：【{0}】，错误信息：【没有指定文件】", sql);
            }

            if (!sql.Trim().ToLower().StartsWith("select"))
            {
                return string.Format("【SQL导入发生错误】: 错误语句：【{0}】，错误信息：【SQL语句不是查询语句】", sql);
            }

            var execSql =
                string.Format(
                    @"insert into OPENROWSET('MICROSOFT.ACE.OLEDB.12.0','{3};HDR=YES;DATABASE={0}','SELECT * FROM [{1}$]') {2}",
                    filePath, sheetName, sql, GetExcelVersionString());
            
            try
            {
                return MSSQL.Tools.ExecSql(execSql);
            }
            catch (Exception exception)
            {
                return string.Format("【SQL导入发生错误】: 错误语句：【{0}】，错误信息：【{1}】", sql, exception.Message);
            }
        }

        /// <summary>
        /// 将超过255列Sql查询结果插入到指定Excel文件中
        /// </summary>
        /// <param name="sql">sql查询语句(注：SQL中需要包含数据库名)</param>
        /// <param name="excelInfo">导出EXCEL信息</param>
        /// <param name="dataBaseEngineType">数据库引擎类型</param>
        /// <returns>操作结果</returns>
        public static string ExportLargeSqlToExcel(string sql, ExcelInfo excelInfo, DataBaseEngineType dataBaseEngineType = DataBaseEngineType.MsSqlServer)
        {
            MSSQL.Tools.DataBaseType = dataBaseEngineType;
            var result = MSSQL.Tools.ExportCSV(sql, excelInfo.FilePath.Replace(".xlsx", ".csv"));
            return result != "" ? result : CopyExcelData(new ExcelInfo { FilePath = excelInfo.FilePath.Replace(".xlsx", ".csv") }, excelInfo);
        }

        /// <summary>
        /// EXCEL间数据拷贝
        /// </summary>
        /// <param name="sourceExcelInfo">源数据EXCEL文件信息</param>
        /// <param name="destinationExcelInfo">拷贝目标EXCEL文件信息</param>
        /// <param name="copyTime">拷贝次数</param>
        /// <returns>拷贝结果</returns>
        public static string CopyExcelData(ExcelInfo sourceExcelInfo, ExcelInfo destinationExcelInfo, int copyTime = 1)
        {
            try
            {
                if (destinationExcelInfo.RowStart < 1 || destinationExcelInfo.ColumnStart < 1)
                {
                    return @"目标excel初始行列应>0";
                }
                var result = OpenExcel(destinationExcelInfo);

                if (result != "")
                {
                    return result;
                }

                var sourceBook = _objBook;
                var sourceSheet = _objSheet;

                if (sourceExcelInfo.FilePath != destinationExcelInfo.FilePath)
                {
                    sourceBook = _objBooks.Open(sourceExcelInfo.FilePath, ObjOpt, ObjOpt, ObjOpt, ObjOpt,
                                              ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt);
                    sourceSheet = OpenSheet(sourceExcelInfo, sourceBook);
                    if (sourceSheet == null)
                    {
                        return "没有指定页签";
                    }
                }

                var rowCount = CopyDataToClipBoard(sourceExcelInfo, sourceSheet);
                if (destinationExcelInfo.RowsCount == 0)
                {
                    destinationExcelInfo.RowsCount = rowCount;
                }
                for (var n = 1; n <= copyTime; n++)
                {
                    CopyDataFromClipBoard(destinationExcelInfo, n);
                }

                sourceBook.Close(false, ObjOpt, ObjOpt);
                ReleaseObj(sourceSheet);
                ReleaseObj(sourceBook);
                Clipboard.Clear();
                Save();
            }
            catch (Exception err)
            {
                CloseExcel();
                Console.WriteLine(err.Message);
                return err.Message;
            }
            return "";
        }

        /// <summary>
        /// 设置单元格边框
        /// </summary>
        /// <param name="excelInfo">需要设置单元格边框的excel信息</param>
        /// <returns>操作结果</returns>
        public static string SetCellBorder(ExcelInfo excelInfo)
        {
            try
            {
                OpenExcel(excelInfo);
                if (excelInfo.RowsCount == 0 || excelInfo.ColumnCount == 0)
                {
                    _objSheet.UsedRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                else
                {
                    var startCell = SYSTEM.WindowsTools.ConvertIntToChar(excelInfo.ColumnStart) + excelInfo.RowStart;
                    var endCell = SYSTEM.WindowsTools.ConvertIntToChar(excelInfo.ColumnStart + excelInfo.ColumnCount - 1)
                        + (excelInfo.RowStart + excelInfo.RowsCount - 1);
                    var range = _objSheet.Range[startCell, endCell];
                    range.Cells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                }
                Save();

            }
            catch (Exception ex)
            {
                CloseExcel();
                return ex.Message;
            }
            return "";
        }

        /// <summary>
        /// 隐藏页签
        /// </summary>
        /// <param name="excelInfo">EXCEL信息</param>
        /// <param name="sheetNames">待隐藏页签列表</param>
        /// <returns>操作结果</returns>
        public static string HideSheets(ExcelInfo excelInfo, List<string> sheetNames)
        {

            try
            {
                OpenExcel(excelInfo);
                foreach (Excel.Worksheet worksheet in _objBook.Worksheets)
                {
                    foreach (var worksheetName in sheetNames)
                    {
                        if (worksheet.Name != worksheetName) continue;
                        worksheet.Name = worksheet.Name + "(无)";
                        worksheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                    }
                }
                Save();
            }
            catch (Exception ex)
            {
                CloseExcel();
                return ex.Message;
            }
            return "";
        }
        
        /// <summary>
        /// 获取Excel中页签名称列表
        /// </summary>
        /// <param name="excelInfo">Excel信息</param>
        /// <returns>页签名称列表</returns>
        public static List<string> GetSheetName(ExcelInfo excelInfo)
        {
            var sheetNameList = new List<string>();

            using (var conn = new OleDbConnection(
                string.Format(
                    "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"{0}\";Extended Properties='{1};HDR=YES;IMEX=0'",
                    excelInfo.FilePath, GetExcelVersionString())))
            {
                conn.Open();
                var dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                if (dt == null)
                {
                    return sheetNameList;
                }
                foreach (DataRow row in dt.Rows)
                {
                    var tmp = row[2].ToString();
                    if (tmp.ToLower().Contains("print_area"))
                    {
                        continue;
                    }
                    sheetNameList.Add(tmp.Replace("$", "").Replace("'", ""));
                }
            }
            return sheetNameList;
        }

        #endregion


        #region Private Methods

        #region 基本操作

        private static string OpenExcel(ExcelInfo excelInfo, bool isOpenSheet = true)
        {
            Console.WriteLine("Open File:【{0}】", excelInfo.FilePath);
            if (!File.Exists(excelInfo.FilePath))
            {
                return string.Format("文件【{0}】不存在", excelInfo.FilePath);
            }

            _objExcel = new Excel.Application { Visible = false, DisplayAlerts = false, AlertBeforeOverwriting = false };

            _objBooks = _objExcel.Workbooks;
            if (excelInfo.FilePath.Equals(String.Empty) || !File.Exists(excelInfo.FilePath))
            {
                _objBook = _objBooks.Add(ObjOpt);
            }
            else
            {
                _objBook = _objBooks.Open(excelInfo.FilePath, ObjOpt, ObjOpt, ObjOpt, ObjOpt,
                                          ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt);
            }
            if (isOpenSheet)
            {
                _objSheet = OpenSheet(excelInfo);
                if (_objSheet == null)
                {
                    return "没有指定页签";
                }
            }
            return "";
        }

        private static Excel._Worksheet OpenSheet(ExcelInfo excelInfo, Excel._Workbook workbook = null)
        {
            var objSheets = _objBook.Worksheets;
            if (workbook != null)
            {
                objSheets = workbook.Worksheets;
            }
            if (string.IsNullOrEmpty(excelInfo.SheetName))
            {
                return (Excel._Worksheet)(objSheets.Item[1]);
            }
            try
            {
                Console.WriteLine("Open Sheet:【{0}】", excelInfo.SheetName);
                return (Excel._Worksheet)(objSheets.Item[excelInfo.SheetName]);
            }
            catch (Exception)
            {
                CloseExcel();
                return null;
            }
        }

        private static void SaveAs(string excelFilePath)
        {
            Console.WriteLine("Save File:【{0}】\n", excelFilePath);
            _objBook.SaveAs(excelFilePath, ObjOpt, ObjOpt,
                            ObjOpt, ObjOpt, ObjOpt, Excel.XlSaveAsAccessMode.xlNoChange,
                            ObjOpt, ObjOpt, ObjOpt, ObjOpt, ObjOpt);
            CloseExcel();
        }

        private static void Save()
        {
            Console.WriteLine("Save File:\n");
            _objBook.Save();
            CloseExcel();
        }

        private static void CloseExcel()
        {
            if (_objBook != null)
            {
                _objBook.Close(false, ObjOpt, ObjOpt);
            }
            if (_objExcel != null)
            {
                _objExcel.Quit();
            }
            Dispose();
        }

        private static void Dispose()
        {
            Console.WriteLine("Release Cache\n");
            ReleaseObj(_objSheet);
            ReleaseObj(_objBook);
            ReleaseObj(_objBooks);
            ReleaseObj(_objExcel);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static void ReleaseObj(object o)
        {
            if (o == null)
            {
                return;
            }

            System.Runtime.InteropServices.Marshal.ReleaseComObject(o);
            // ReSharper disable RedundantAssignment
            o = null;
            // ReSharper restore RedundantAssignment
        }

        #endregion

        private static string SaveDataToSingleExcel(List<DataTable> dataTables, List<ExcelInfo> excelInfos)
        {
            try
            {
                OpenExcel(excelInfos[0], false);
                for (int n = 0; n < dataTables.Count; n++)
                {
                    _objSheet = OpenSheet(excelInfos[n]);
                    if (_objSheet == null)
                    {
                        return "没有指定页签";
                    }
                    SaveDataToExcelImpl(dataTables[n], excelInfos[n]);
                }
                SaveAs(excelInfos[0].FilePath);
            }
            catch (Exception err)
            {
                CloseExcel();
                Console.WriteLine(err.Message);
                return err.Message;
            }
            return "";

        }

        private static string SaveDataToMultiExcel(List<DataTable> dataTables, List<ExcelInfo> excelInfos)
        {
            for (int n = 0; n < dataTables.Count; n++)
            {
                var tmp = WriteDataToExcel(new List<DataTable>{dataTables[n]},new List<ExcelInfo>{excelInfos[n]});
                if (tmp != "")
                {
                    return tmp;
                }
            }
            return "";
        }

        private static void SaveDataToExcelImpl(DataTable dataTable, ExcelInfo excelInfo)
        {
            var rowCountUsing = dataTable.Rows.Count;
            if (dataTable.Rows.Count > excelInfo.RowsCount && excelInfo.RowsCount != 0)
            {
                rowCountUsing = excelInfo.RowsCount;
            }

            var colCountUsing = dataTable.Columns.Count;
            if (dataTable.Columns.Count > excelInfo.ColumnCount && excelInfo.ColumnCount != 0)
            {
                colCountUsing = excelInfo.ColumnCount;
            }


            if (excelInfo.IsInsert == "1")
            {
                InsertRow(excelInfo.RowStart, rowCountUsing);
            }

            if (excelInfo.HasTitle == "1")
            {
                SetTitle(excelInfo.RowStart, excelInfo.ColumnStart, colCountUsing, dataTable);
                excelInfo.RowStart++;
            }

            if (excelInfo.SheetName != "" && _objSheet.Name == "sheet1")
            {
                _objSheet.Name = excelInfo.SheetName;
            }

            Console.WriteLine("Insert data into excel file");
            if (dataTable.Rows.Count > 0)
            {
                for (var i = 0; i < rowCountUsing; i++)
                {
                    _tmpNumber = 0;
                    for (var j = 0; j < colCountUsing; j++)
                    {
                        SetCellValue(i + excelInfo.RowStart, j + excelInfo.ColumnStart, dataTable.Rows[i][j].ToString(),
                                     i);

                        Console.WriteLine("row:{0},col:{1}", i + 1, j + 1);
                    }
                    if (rowCountUsing % 20 == 0)
                    {
                        Console.WriteLine("【{0}】/【{1}】 rows of data inserted", i, rowCountUsing);
                    }
                }
            }
            Console.WriteLine("Insert finished\n");

        }

        private static void SetCellValue(int origRow, int origCol, string value, int addedRow)
        {
            _objSheet.Cells[origRow, origCol + _tmpNumber] = value;
            int mergedCountPlus;
            if (!IsMerged(origRow - addedRow, origCol + _tmpNumber, out mergedCountPlus))
            {
                return;
            }
            _objSheet.Range[
                _objSheet.Cells[origRow, origCol + _tmpNumber],
                _objSheet.Cells[origRow, origCol + _tmpNumber + mergedCountPlus]].MergeCells = true;
            _tmpNumber += mergedCountPlus;
        }

        private static void InsertRow(int startRow, int rowCount)
        {
            if (startRow == 0)
            {
                startRow++;
            }
            Console.WriteLine("Insert empty rows into excel file");
            var range = (Excel.Range)_objSheet.Rows[startRow, ObjOpt];
            int count = 0;
            while (count < rowCount)
            {
                range.EntireRow.Insert(Excel.XlDirection.xlDown, Excel.XlInsertFormatOrigin.xlFormatFromLeftOrAbove);
                count++;
            }
        }

        private static void SetTitle(int rowStart, int colStart, int colCountUsing, DataTable excelTable)
        {
            for (var i = 0; i < colCountUsing; i++)
            {

                _objSheet.Cells[rowStart, i + colStart] =
                    excelTable.Columns[i].ColumnName;
            }
        }

        private static bool IsMerged(int row, int col, out int mergedColCount)
        {
            mergedColCount = 0;
            var tmp = _objSheet.Cells[row, col];
            _objRange = tmp as Excel.Range;
            if (_objRange == null)
            {
                return false;
            }
            mergedColCount = _objRange.MergeArea.Columns.Count - 1;
            return ((bool)_objRange.MergeCells);
        }

        private static string GetExcelVersionString()
        {
            switch (Tools.OfficeVersion)
            {
                case OfficeVersion.Office95:
                    return "Excel 7.0";
                case OfficeVersion.Office97:
                    return "Excel 8.0";
                case OfficeVersion.Office2000:
                    return "Excel 9.0";
                case OfficeVersion.Office2003:
                    return "Excel 11.0";
                case OfficeVersion.Office2007:
                    return "Excel 12.0";
                case OfficeVersion.Office2010:
                    return "Excel 12.0";
                    //return "Excel 14.0";
                case OfficeVersion.Office2013:
                    return "Excel 15.0";
                default:
                    return "Excel 4.0";
            }
        }

        private static void CopyDataFromClipBoard(ExcelInfo destinationExcelInfo, int index = 1)
        {

            var startRow = (index - 1) * destinationExcelInfo.RowsCount + destinationExcelInfo.RowStart;
            var startCell = SYSTEM.WindowsTools.ConvertIntToChar(destinationExcelInfo.ColumnStart) +
                            startRow.ToString(CultureInfo.InvariantCulture);
            var destinationRange = _objSheet.Range[startCell];
            _objSheet.Paste(destinationRange, false);
            Console.WriteLine("Paste [{0}]", startCell);
        }

        private static int CopyDataToClipBoard(ExcelInfo sourceExcelInfo, Excel._Worksheet firstWorksheet)
        {
            if (sourceExcelInfo.ColumnCount == 0 || sourceExcelInfo.RowsCount == 0)
            {
                firstWorksheet.UsedRange.Copy(ObjOpt);
                return firstWorksheet.UsedRange.Rows.Count;
            }
            var startCell = SYSTEM.WindowsTools.ConvertIntToChar(sourceExcelInfo.ColumnStart) +
                            sourceExcelInfo.RowStart.ToString(CultureInfo.InvariantCulture);
            var endCell = SYSTEM.WindowsTools.ConvertIntToChar(sourceExcelInfo.ColumnStart + sourceExcelInfo.ColumnCount - 1) +
                          (sourceExcelInfo.RowStart + sourceExcelInfo.RowsCount - 1).ToString(CultureInfo.InvariantCulture);
            var sourceRange = firstWorksheet.Range[startCell, endCell];
            sourceRange.Copy(ObjOpt);
            Console.WriteLine("Copy [{0}][{1}]", startCell, endCell);
            return 0;
        }

        #endregion
    }
}
