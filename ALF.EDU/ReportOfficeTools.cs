using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using ALF.EDU.DataModel;
using ALF.MSSQL;
using ALF.SYSTEM;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;

namespace ALF.EDU
{
    public static class ReportOfficeTools
    {
        private const string TagDocStart = "统计文字参数开始";
        private const string TagDocEnd = "统计文字参数结束";
        private const string TagTableStart = "统计表格参数开始";
        private const string TagTableEnd = "统计表格参数结束";
        private const string TagGraphStart = "统计图形参数开始";
        private const string TagGraphEnd = "统计图形参数结束";
        private const string TagFormat = "{0}(?<tag>[A-Za-z0-9_\\u4e00-\\u9fa5]*){1}";
        private static object _mis = Type.Missing;
        private static readonly object RpAll = WdReplace.wdReplaceAll;

        public static List<ArgInfo> GetArgInfoListFromWord(TemplateInfo templateInfo, out string result)
        {
            result = "";
            var resultDocArgList = new List<ArgInfo>();
            var wordApp =
                (Application)
                    Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("000209FF-0000-0000-C000-000000000046")));
            try
            {
                var wordDoc = wordApp.Documents.Open(templateInfo.templatePath, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis);
                var enumerator = wordDoc.Paragraphs.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        var item = (Paragraph) enumerator.Current;
                        var m = Regex.Match(item.Range.Text,
                            string.Format(TagFormat, TagDocStart, TagDocEnd));
                        var argInfo = new ArgInfo
                        {
                            templateID = templateInfo.templateID,
                            templateName = templateInfo.templateName
                        };
                        if (m.Success && resultDocArgList.All(p => p.argName != m.Groups["tag"].Value))
                        {
                            argInfo.argType = ArgType.文字.ToString();
                            argInfo.argName = m.Groups["tag"].Value;
                            resultDocArgList.Add(argInfo);
                        }
                        m = Regex.Match(item.Range.Text,
                            string.Format(TagFormat, TagTableStart, TagTableEnd));
                        if (m.Success && resultDocArgList.All(p => p.argName != m.Groups["tag"].Value))
                        {
                            argInfo.argType = ArgType.表格.ToString();
                            argInfo.argName = m.Groups["tag"].Value;
                            resultDocArgList.Add(argInfo);
                        }
                        m = Regex.Match(item.Range.Text,
                            string.Format(TagFormat, TagGraphStart, TagGraphEnd));
                        if (m.Success && resultDocArgList.All(p => p.argName != m.Groups["tag"].Value))
                        {
                            argInfo.argType = ArgType.图形.ToString();
                            argInfo.argName = m.Groups["tag"].Value;
                            resultDocArgList.Add(argInfo);
                        }
                    }
                }
                finally
                {
                    var disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
                wordApp.Quit(WdSaveOptions.wdDoNotSaveChanges, ref _mis, ref _mis);
            }
            catch (Exception ex)
            {
                result = ex.Message;
                wordApp.Quit(WdSaveOptions.wdDoNotSaveChanges, WdOriginalFormat.wdWordDocument, false);
            }
            return resultDocArgList;
        }

        public static string UpdateWord(List<ArgInfo> argInfoList, string regionString, int appType, string filePath)
        {
            var start = DateTime.Now;
            Console.WriteLine(start.ToString(CultureInfo.InvariantCulture));
            var tableCount = 0;
            var chartCount = 0;
            var wordApp =
                (Application)
                    Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("000209FF-0000-0000-C000-000000000046")));
            object obj = filePath;
            var wordDoc = wordApp.Documents.Open(ref obj, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis);
            wordDoc.PageSetup.TopMargin = wordApp.CentimetersToPoints(float.Parse("0.6"));
            wordDoc.PageSetup.LeftMargin = wordApp.CentimetersToPoints(float.Parse("0.6"));
            wordDoc.PageSetup.RightMargin = wordApp.CentimetersToPoints(float.Parse("0.6"));
            wordDoc.PageSetup.BottomMargin = wordApp.CentimetersToPoints(float.Parse("0.6"));
            wordDoc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;
            wordDoc.SpellingChecked = false;
            wordDoc.ShowSpellingErrors = false;
            var infoString = string.Format("\r\n\r\n 日期：{0}  文件名：{1}  \r\n",
                DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), filePath);
            var maxTime = new TimeSpan(0, 0, 30);
            string result2;
            try
            {
                var i = 1;
                var condition = GetRegionCondition(regionString, appType);
                string result;
                foreach (var argInfo in argInfoList)
                {
                    var sql = AddCondition(argInfo, condition, regionString);
                    var argStartTime = DateTime.Now;
                    Console.WriteLine("***第{1}个参数开始{0}", argInfo.argName, i);
                    var dataViewValue = Tools.GetSqlDataView(sql, out result, 3600);
                    if (result != "")
                    {
                        wordApp.Quit(WdSaveOptions.wdSaveChanges, WdOriginalFormat.wdWordDocument, false);
                        return string.Format("{0}参数{1}，错误信息：{2}", argInfo.argType, argInfo.argName, result);
                    }
                    if (argInfo.argType == ArgType.文字.ToString())
                    {
                        result = EditDocArg(dataViewValue.Table.Rows[0][0].ToString(), argInfo.argName, wordDoc);
                    }
                    if (argInfo.argType == ArgType.表格.ToString())
                    {
                        result = EditTableArg(dataViewValue, tableCount, wordDoc);
                        tableCount++;
                    }
                    if (argInfo.argType == ArgType.图形.ToString())
                    {
                        // result = ReportOfficeTools.editChartArg(dataViewValue, chartCount, wordDoc);
                        chartCount++;
                    }
                    if (result != "")
                    {
                        wordApp.Quit(WdSaveOptions.wdSaveChanges, WdOriginalFormat.wdWordDocument, false);
                        return string.Format("{0}参数{1}，错误信息：{2}", argInfo.argType, argInfo.argName, result);
                    }
                    var usedTime = DateTime.Now - argStartTime;
                    if (usedTime > maxTime)
                    {
                        infoString += string.Format("参数：{0} 用时：{1} 参数名称：{2}\r\n", argInfo.argNo, usedTime,
                            argInfo.argName);
                    }
                    Console.WriteLine("***参数已完成{0}, \n用时{1}\n", argInfo.argName, usedTime);
                    i++;
                }
                result = ClearTag(wordDoc);
                if (result != "")
                {
                    wordApp.Quit(WdSaveOptions.wdDoNotSaveChanges, _mis, _mis);
                    result2 = result;
                }
                else
                {
                    ReleaseObject(wordDoc);
                    wordApp.Quit(WdSaveOptions.wdSaveChanges, WdOriginalFormat.wdWordDocument, false);
                    var timeSpan = DateTime.Now - start;
                    Console.WriteLine( "用时：" + timeSpan);
                    timeSpan = DateTime.Now - start;
                    infoString = infoString + string.Format("总用时：{0}\r\n\r\n", timeSpan);
                    WindowsTools.WriteToTxt(Environment.CurrentDirectory + "\\timeUsed.txt", infoString);
                    result2 = result;
                }
            }
            catch (Exception ex)
            {
                wordApp.Quit(WdSaveOptions.wdSaveChanges, WdOriginalFormat.wdWordDocument, false);
                result2 = ex.Message;
            }
            return result2;
        }

        private static string EditDocArg(string newValue, string oldValue, Document wordDoc)
        {
            var result = "";
            try
            {
                var oldTotalValue = string.Format("{0}{1}{2}", TagDocStart, oldValue, TagDocEnd);
                wordDoc.Content.Find.Text = oldTotalValue;
                wordDoc.Content.Find.ClearFormatting();
                wordDoc.Content.Find.Replacement.Text = newValue;
                wordDoc.Content.Find.Execute(ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis,
                    ref _mis, ref _mis, ref _mis, RpAll, ref _mis, ref _mis, ref _mis, ref _mis);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private static string EditTableArg(DataView newValue, int tableCount, Document wordDoc)
        {
            var result = "";
            try
            {
                Console.WriteLine("调整表格 行：{0}，列{1}", newValue.Table.Rows.Count, newValue.Table.Columns.Count);
                var wordTable = UpdateTableFormat(newValue, wordDoc.Tables[tableCount + 1]);
                wordTable.Range.Font.Size = 10f;
                for (var currentCol = 0; currentCol < newValue.Table.Columns.Count; currentCol++)
                {
                    wordTable.Cell(1, currentCol + 1).Range.Text = newValue.Table.Columns[currentCol].ColumnName;
                }
                wordTable.Cell(1, newValue.Table.Columns.Count + 1).Range.Text = "情况说明";
                for (var currentRow = 0; currentRow < newValue.Table.Rows.Count; currentRow++)
                {
                    for (var currentCol = 0; currentCol < newValue.Table.Columns.Count; currentCol++)
                    {
                        wordTable.Cell(currentRow + 2, currentCol + 1).Range.Text =
                            newValue.Table.Rows[currentRow][currentCol].ToString();
                    }
                    if (currentRow != 0 && currentRow%100 == 0)
                    {
                        Console.WriteLine("填充表格 已完成{0}行，共{1}行", currentRow, newValue.Table.Rows.Count);
                    }
                }
                Console.WriteLine("填充表格 已完成{0}行，共{0}行", newValue.Table.Rows.Count);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private static string GetExcelRange(int rowCount, int colCount)
        {
            var row = (rowCount + 1).ToString(CultureInfo.InvariantCulture);
            var lastColCount = colCount%26;
            var digitCount = colCount/26;
            var c = Convert.ToChar(64 + lastColCount);
            var col = c.ToString(CultureInfo.InvariantCulture);
            if (digitCount > 0)
            {
                c = Convert.ToChar(64 + digitCount);
                col = c.ToString(CultureInfo.InvariantCulture) + col;
            }
            return col + row;
        }

        private static string ClearTag(Document wordDoc)
        {
            var result = "";
            try
            {
                wordDoc.Content.Find.Text = TagDocStart;
                wordDoc.Content.Find.ClearFormatting();
                wordDoc.Content.Find.Replacement.Text = "";
                wordDoc.Content.Find.Execute(ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis,
                    ref _mis, ref _mis, ref _mis, RpAll, ref _mis, ref _mis, ref _mis, ref _mis);
                wordDoc.Content.Find.Text = TagDocEnd;
                wordDoc.Content.Find.Execute(ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis,
                    ref _mis, ref _mis, ref _mis, RpAll, ref _mis, ref _mis, ref _mis, ref _mis);
                wordDoc.Content.Find.Text = TagTableStart;
                wordDoc.Content.Find.Execute(ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis,
                    ref _mis, ref _mis, ref _mis, RpAll, ref _mis, ref _mis, ref _mis, ref _mis);
                wordDoc.Content.Find.Text = TagTableEnd;
                wordDoc.Content.Find.Execute(ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis,
                    ref _mis, ref _mis, ref _mis, RpAll, ref _mis, ref _mis, ref _mis, ref _mis);
                wordDoc.Content.Find.Text = TagGraphStart;
                wordDoc.Content.Find.Execute(ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis,
                    ref _mis, ref _mis, ref _mis, RpAll, ref _mis, ref _mis, ref _mis, ref _mis);
                wordDoc.Content.Find.Text = TagGraphEnd;
                wordDoc.Content.Find.Execute(ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis,
                    ref _mis, ref _mis, ref _mis, RpAll, ref _mis, ref _mis, ref _mis, ref _mis);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        private static Table UpdateTableFormat(DataView dataView, Table wordTable)
        {
            var rowDiff = dataView.Table.Rows.Count - wordTable.Rows.Count;
            var colDiff = dataView.Table.Columns.Count - wordTable.Columns.Count;
            if (rowDiff >= 0)
            {
                for (var i = 0; i < rowDiff + 1; i++)
                {
                    wordTable.Rows.Add(_mis);
                }
            }
            else
            {
                for (var i = 0; i < -(rowDiff + 1); i++)
                {
                    wordTable.Rows.Last.Delete();
                }
            }
            if (colDiff > 0)
            {
                for (var i = 0; i < colDiff + 1; i++)
                {
                    wordTable.Columns.Add(_mis);
                }
                wordTable.Columns[3].Width = 160f;
                wordTable.Columns.Last.Width = 100f;
            }
            else
            {
                for (var i = 0; i < -(colDiff + 1); i++)
                {
                    wordTable.Columns.Last.Delete();
                }
            }
            wordTable.Rows[1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            wordTable.Rows[1].Range.Bold = 1;
            wordTable.Rows[1].Range.Shading.BackgroundPatternColor = WdColor.wdColorGray50;
            return wordTable;
        }

        private static void ReleaseObject(object obj)
        {
            try
            {
                Marshal.ReleaseComObject(obj);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                GC.Collect();
            }
        }


        public static string GetRegionCondition(string regionString, int appType)
        {
            var regionPathSql = "";
            switch (appType)
            {
                case 0:
                    {
                        regionPathSql = "statisticsPath";
                        break;
                    }
                case 1:
                    {
                        regionPathSql = "gatherRegionA+gatherRegionB+gatherRegionC+gatherRegionD";
                        break;
                    }
                case 2:
                    {
                        regionPathSql = "regionA+regionB+regionC+regionD+regionE";
                        break;
                    }
            }
            return string.Format(" left({0},len('{1}'))='{1}'", regionPathSql, regionString);
        }

        public static string AddCondition(ArgInfo argInfo, string condition, string regionPath = "4=4")
        {
            var sql = argInfo.argDataSql;
            string result;
            if (string.IsNullOrEmpty(sql))
            {
                result = "";
            }
            else
            {
                sql = sql.Replace("\"", "");
                sql = sql.Replace("1=1", (condition == "") ? "1=1" : condition);
                sql = sql.Replace("2=2", string.IsNullOrEmpty(argInfo.downLimit) ? "2=2" : argInfo.downLimit);
                sql = sql.Replace("3=3", string.IsNullOrEmpty(argInfo.upLimit) ? "3=3" : argInfo.upLimit);
                sql = sql.Replace("4=4", regionPath);
                sql.Insert(6, " top 10 ");
                result = sql;
            }
            return result;
        }
    }
}
