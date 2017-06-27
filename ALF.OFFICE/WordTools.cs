using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace ALF.OFFICE
{
    /// <summary>
    /// 
    /// </summary>
    public class WordTools
    {
        #region Private Fields

        private static object _mis = Type.Missing;
        private static readonly object RpAll = WdReplace.wdReplaceAll;
        private static Application wordApp;

        #endregion


        #region Public Methods

        /// <summary>
        /// 将word文档转换为png图片
        /// </summary>
        /// <param name="docPath">word文档路径</param>
        /// <returns>图片路径列表</returns>
        public static List<string> ConvertWordToImage(string docPath)
        {
            Console.WriteLine("Creating Images");
            var list = new List<string>();
            Document wordDoc = new Document();
            wordDoc = OpenWord(docPath);
            if (wordDoc == null)
            {
                return null;
            }

            //Opens the word document and fetch each page and converts to image
            foreach (Window ww in wordDoc.Windows)
            {
                foreach (Pane pane in ww.Panes)
                {
                    for (var i = 1; i <= pane.Pages.Count; i++)
                    {
                        var page = pane.Pages[i];
                        var bits = page.EnhMetaFileBits;

                        var name = string.Format("{1}-{0}.png", i, SYSTEM.WindowsTools.GetBasicName(docPath).Split('.')[0]);
                        var target = string.Format(@"{0}\tmpPng\{1}", Environment.CurrentDirectory, name);
                        list.Add(target);

                        try
                        {
                            using (var ms = new MemoryStream((byte[])(bits)))
                            {
                                var image = System.Drawing.Image.FromStream(ms);
                                var pngTarget = Path.ChangeExtension(target, "png");
                                image.Save(pngTarget, ImageFormat.Png);
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Console.WriteLine("Convert Failure:[{0}-{1}]:{2}", docPath, i, ex.Message);
                        }
                    }
                }
            }
            CloseWord(wordDoc);
            return list;
        }

        /// <summary>
        /// 替换Word文档文本内容
        /// </summary>
        /// <param name="newValue">新值</param>
        /// <param name="oldValue">原值</param>
        /// <param name="docPath">Word文档路径</param>
        /// <returns>转换结果信息</returns>
        public static string ReplaceContent(string newValue, string oldValue, string docPath)
        {
            var result = "";
            try
            {
                var wordDoc = OpenWord(docPath);
                if (wordDoc == null)
                {
                    return "Open Word File Failed";
                }
                var wfnd = wordDoc.Range().Find;
                wfnd.Text = oldValue;
                wfnd.ClearFormatting();
                if (wfnd.Execute(ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, newValue, RpAll, ref _mis, ref _mis, ref _mis, ref _mis))
                {
                    return "";
                }

                StoryRanges sr = wordDoc.StoryRanges;
                foreach (Range r in sr)
                {
                    Range r1 = r;
                    if (WdStoryType.wdTextFrameStory == r.StoryType) //这句话用来判断什么的？
                    {
                        do
                        {
                            r1.Find.ClearFormatting();
                            r1.Find.Text = oldValue;

                            r1.Find.Replacement.ClearFormatting();
                            r1.Find.Replacement.Text = newValue;

                            if (r1.Find.Execute(
                                            ref _mis, ref _mis, ref _mis, ref _mis, ref _mis,
                                            ref _mis, ref _mis, ref _mis, ref _mis, ref _mis,
                                            RpAll, ref _mis, ref _mis, ref _mis, ref _mis))
                            {
                                break;//找到并替换后跳出循环，省点时间
                            }
                            else
                            {
                                r1 = r1.NextStoryRange;
                            }
                        } while (r1 != null);
                    }
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
                Console.WriteLine("Error:" + ex.Message);
            }

            return result;
        }
        
        /// <summary>
        /// 复制整篇文档内容
        /// </summary>
        /// <param name="sorceDocPath">源文件路径</param>
        /// <param name="destDocPath">目标文件路径</param>
        /// <param name="isPasteAtLast">是否粘贴在文章最后</param>
        /// <param name="replaceKeyWord">替换书签名称（不粘贴在最后的情况下）</param>
        public static void CopyWordContent(string sorceDocPath, string destDocPath, bool isPasteAtLast, string replaceKeyWord)
        {

            object readOnly = false;
            object isVisible = false;

            Document destWordDoc = OpenWord(destDocPath);
            if (destWordDoc == null)
            {
                return;
            }
            Document openWord = OpenWord(sorceDocPath);
            if (openWord == null)
            {
                return;
            }

            openWord.Select();
            openWord.Sections[1].Range.Copy();

            if (isPasteAtLast)
            {
                destWordDoc.Select();
                object objUnit = WdUnits.wdStory;
                wordApp.Selection.EndKey(ref objUnit);
                wordApp.ActiveWindow.Selection.PasteAndFormat(WdRecoveryType.wdPasteDefault);
            }
            else
            {
                foreach (Bookmark bm in destWordDoc.Bookmarks)
                {
                    if (bm.Name == replaceKeyWord)
                    {
                        bm.Select();
                        bm.Range.PasteAndFormat(WdRecoveryType.wdPasteDefault);
                    }
                }
            }

            SaveAndClose(destWordDoc);
            SaveAndClose(openWord);
        }

        /// <summary>
        /// 打印Word文档
        /// </summary>
        /// <param name="filePath">Word文档路径</param>
        public static void PrintWord(string filePath)
        {
            Console.WriteLine("Print Word");
            Document wordDoc = new Document();
            wordDoc = OpenWord(filePath);
            wordDoc.PrintOut(ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis);
            CloseWord(wordDoc);
        }

        #endregion


        #region Private Methods

        private static void StartWordApp()
        {
            try
            {
                Console.WriteLine("打开Word应用");
                wordApp =
                    (Application)
                        Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("000209FF-0000-0000-C000-000000000046")));
            }
            catch (Exception ex)
            {
                Console.WriteLine("打开Word应用失败 [0]", ex.Message);
            }
        }

        private static void CloseWordApp()
        {
            try
            {
                wordApp.Quit(WdSaveOptions.wdSaveChanges, WdOriginalFormat.wdWordDocument, false);
            }
            catch
            {
            }
        }

        private static Document OpenWord(string filePath)
        {
            Console.WriteLine("Open File:【{0}】", filePath);
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File Not Exists:【{0}】", filePath);
                return null;
            }

            Document doc;
            try
            {
                doc = wordApp.Documents.Open(filePath, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis);
                return doc;
            }
            catch
            {
                StartWordApp();
                try
                {
                    doc = wordApp.Documents.Open(filePath, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis);
                    return doc;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Open File Failed:【{0}】", ex.Message);
                    return null;
                }
            }
        }

        private static void SaveAsAndClose(Document doc, string filePath, bool isClose = true)
        {
            Console.WriteLine("Save File:【{0}】", filePath);
            doc.SaveAs2(filePath, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis, ref _mis);
            if (isClose)
            {
                CloseWord(doc);
            }
        }

        private static void SaveAndClose(Document doc, bool isClose = true)
        {
            Console.WriteLine("Save File:");
            doc.Save();
            if (isClose)
            {
                CloseWord(doc);
            }
        }

        private static void CloseWord(Document doc)
        {
            doc.Close(ref _mis, ref _mis, ref _mis);
            wordApp?.Quit();
        }

        #endregion
    }
}
