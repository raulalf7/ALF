using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace ALF.SYSTEM
{
    /// <summary>
    /// Web工具
    /// </summary>
    public static class WebTools
    {

        #region WEB SERVICE
        /// <summary>
        /// 获取GMT时间
        /// </summary>
        /// <returns>时间</returns>
        public static DateTime GetGmtTime()
        {
            var result = default(DateTime);

            foreach (var serverIp in Servers)
            {
                result = GetNistTime(serverIp);
                if (result <= DateTime.MinValue) continue;
                break;
            }

            return result;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <param name="url">文件路径</param>
        /// <returns>下载结果</returns>
        public static string DownloadFile(string fileName, string url)
        {
            try
            {
                Console.WriteLine("Connecting to the URL");
                var request = (HttpWebRequest)WebRequest.Create(url);
                var response = request.GetResponse();
                Console.WriteLine("Connected");
                response.GetResponseStream();

                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    Console.WriteLine("Start download: total {0}", response.ContentLength);
                    SaveBinaryFile(response, fileName);
                    Console.WriteLine("Download finished!");
                }
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
            return "";
        }

        /// <summary>
        /// 判断是否正确URL
        /// </summary>
        /// <param name="url">url路径</param>
        /// <returns>是否正确</returns>
        public static bool IsCorrectUrl(string url)
        {
            var result = false;
            HttpWebResponse webResponse = null;     //HTTP会话
            try
            {
                var webRequst = (HttpWebRequest)WebRequest.Create(url);        //HTTP请求
                webResponse = (HttpWebResponse)webRequst.GetResponse(); //获得http会话
                if (webResponse.StatusCode == HttpStatusCode.OK)
                { //请求成功
                    result = true;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                //关闭会话
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// 保存二进制文件
        /// </summary>
        /// <param name="response">Web响应</param>
        /// <param name="fileName">文件名称</param>
        private static void SaveBinaryFile(WebResponse response, string fileName)
        {
            var buffer = new byte[1024];
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                Stream outStream = File.Create(fileName);
                Stream inStream = response.GetResponseStream();

                if (inStream == null)
                {
                    return;
                }

                int l;
                var size = 0;
                do
                {

                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l <= 0)
                    {
                        continue;
                    }
                    outStream.Write(buffer, 0, l);
                    size++;
                    Console.WriteLine("{0}KB has downloaded", size);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static readonly string[] Servers = { "132.163.4.102", "132.163.4.103" };

        private static DateTime GetNistTime(string host)
        {
            //Returns DateTime.MinValue if host unreachable or does not produce time
            string timeStr;
            try
            {
                var reader = new StreamReader(new TcpClient(host, 13).GetStream());
                timeStr = reader.ReadToEnd();
                reader.Close();
            }
            catch (SocketException)
            {
                //Couldn't connect to server, transmission error
                Debug.WriteLine("Socket Exception [" + host + "]");
                return DateTime.MinValue;
            }
            catch (Exception)
            {
                //Some other error, such as Stream under/overflow
                return DateTime.MinValue;
            }

            //Parse timeStr
            if (timeStr == "" || (timeStr.Substring(38, 9) != "UTC(NIST)"))
            {
                //This signature should be there
                return DateTime.MinValue;
            }
            if ((timeStr.Substring(30, 1) != "0"))
            {
                //Server reports non-optimum status, time off by as much as 5 seconds
                return DateTime.MinValue;
                //Try a different server
            }

            int jd = int.Parse(timeStr.Substring(1, 5));
            int yr = int.Parse(timeStr.Substring(7, 2));
            int mo = int.Parse(timeStr.Substring(10, 2));
            int dy = int.Parse(timeStr.Substring(13, 2));
            int hr = int.Parse(timeStr.Substring(16, 2));
            int mm = int.Parse(timeStr.Substring(19, 2));
            int sc = int.Parse(timeStr.Substring(22, 2));

            if ((jd < 15020))
            {
                //Date is before 1900
                return DateTime.MinValue;
            }
            if ((jd > 51544))
                yr += 2000;
            else
                yr += 1900;

            return new DateTime(yr, mo, dy, hr, mm, sc);

        }
        #endregion
    }
}
