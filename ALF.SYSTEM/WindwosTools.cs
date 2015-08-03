using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Xml.Serialization;
using ALF.SYSTEM.DataModel;

namespace ALF.SYSTEM
{
    public class WindwosTools
    {
        #region WINDOWS TOOLS
        public static bool IsServeiceStart(string serviceName)
        {
            if (serviceName == "")
            {
                return true;
            }

            var ser = new ServiceController { ServiceName = serviceName, MachineName = "." };

            try
            {
                return (ser.Status == ServiceControllerStatus.Running);
            }
            catch
            {
                return false;
            }
        }

        public static string GetBasicName(string fileFullName)
        {
            var index = fileFullName.LastIndexOf("\\", StringComparison.Ordinal);
            return fileFullName.Substring(index + 1);
        }

        public static Action<string> ExecCmdFinished;

        public static string ExecCmd(string fileName, string argName, bool hideWindow = false, bool isShowFinished = false)
        {
            var process = new Process();
            var result = "";
            try
            {
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = argName;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                if (hideWindow)
                {
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                }

                process.StartInfo.RedirectStandardInput = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;

                process.Start();
                if (isShowFinished)
                {
                    result = process.StandardOutput.ReadToEnd().Trim();
                }
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cmd error ;{0}", ex.Message);
                result = ex.Message;
            }
            return result;
        }

        public static void WriteToTxt(string path, string infoString)
        {
            StreamWriter streamWriter = File.Exists(path) ? File.AppendText(path) : new StreamWriter(path);
            streamWriter.Write(infoString);
            streamWriter.Close();
        }

        public static string ReadFromTxt(string path)
        {
            var t = File.ReadAllLines(path, Encoding.Default);
            return t.Aggregate("", (current, s) => current + (s + Environment.NewLine));
        }

        public static bool IsDateString(string dateString)
        {
            return true;
        }

        public static string CreatXml(string filePath, InputType inputType)
        {
            var dir = new DirectoryInfo(string.Format(@"{0}\", filePath));
            if (!dir.Exists)
            {
                return filePath + @"路径不存在";
            }
            var fileList = GetFiles(dir);
            var updateFileList = fileList.Select(item => new UpdateFile
            {
                FileName = item.Name,
                FileSize = (int)item.Length,

            }).ToList();


            var xmlSerializer = new XmlSerializer(updateFileList.GetType());
            var file = new FileStream(string.Format(@"{0}\dataConfig.{1}", filePath, inputType), FileMode.Create);
            xmlSerializer.Serialize(file, updateFileList);
            file.Flush();
            file.Close();
            return "";
        }


        #endregion


        static private IEnumerable<FileInfo> GetFiles(DirectoryInfo dir)
        {
            var result = dir.GetFiles().ToList();
            var tmpDirList = dir.GetDirectories();
            foreach (var item in tmpDirList)
            {
                result.AddRange(GetFiles(item));
            }
            return result;
        }
    }
}
