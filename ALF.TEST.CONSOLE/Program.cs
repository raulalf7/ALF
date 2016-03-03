using System;
using System.IO;
using System.Threading;
using ALF.MSSQL.DataModel;
using ALF.OFFICE;
using ALF.OFFICE.DataModel;
using ALF.SYSTEM;
using ALF.SYSTEM.DataModel;

namespace ALF.TEST.CONSOLE
{
    class Program
    {
        static void Main()
        {
            SqlOracleTest();
            //ALF.MSSQL.Tools.DataBaseType = DataBaseEngineType.Remote;
            //ALF.MSSQL.Tools.ConnInfo = new ConnInfo() { ConnIp = @"192.168.0.20\sql2012", ConnPw = "abc123," };
            //ALF.MSSQL.Tools.DBName = "XXJGDM";
            //ALF.MSSQL.Tools.TransferDataToOracle("JG_JBZ", "192.168.0.201", "1521", "orcl", "jgdm", "jgdm");

            //SqlOracleTest();
            //var readString = "";
            //while (readString != "exit")
            //{
            //    CheckSoftware();
            //    Console.WriteLine("Quit print exit");
            //    readString = Console.ReadLine();
            //}
        }

        private static void CheckSoftware()
        {
            Console.WriteLine("Input soft name");
            string softName = Console.ReadLine();
            if (WindowsTools.IsSoftInstalled(softName))
            {
                Console.WriteLine("Installed");
                return;
            }
            Console.WriteLine("Not Installed");
        }

        private static void ExcelVersion()
        {
            var readString = "";
            while (readString != "exit")
            {
                Console.WriteLine("input office version: 2007/2010/2013");
                OfficeVersion o = OfficeVersion.Office2013;
                switch (Console.ReadLine())
                {
                    case "2013":
                        o = OfficeVersion.Office2013;
                        break;
                    case "2010":
                        o = OfficeVersion.Office2010;
                        break;
                    case "2007":
                        o = OfficeVersion.Office2007;
                        break;
                }
                Console.WriteLine("input sql type: local/express");
                DataBaseEngineType d = DataBaseEngineType.MsSqlServer;
                switch (Console.ReadLine())
                {
                    case "local":
                        d = DataBaseEngineType.MsSqlServer;
                        break;
                    case "express":
                        d = DataBaseEngineType.SqlExpress;
                        break;
                }


                Tools.InitialSqSetting(d, o);

                Console.WriteLine("start output");
                Console.WriteLine(ExcelTools.ExportSqlToExcel("select top 10 * from eduCodeDB..schoolBusinessRelation",
                    @"c:\test.xlsx", "sheet1"));

                Console.WriteLine("output finished");



                readString = Console.ReadLine();
            }
        }

        private static void DeleteTest()
        {
            var readString = "";
            while (readString != "exit")
            {
                Console.WriteLine("input directory");
                string dir = Console.ReadLine();
                if (dir != null && Directory.Exists(dir))
                {
                    Directory.Delete(dir,true);
                    Thread.Sleep(500);
                }
                if (dir != null)
                {
                    Directory.CreateDirectory(dir);
                    Thread.Sleep(500);
                }
                File.Copy(@"d:\test1\1.mp4", @"d:\test\1.mp4");
                readString = Console.ReadLine();
            }
        }

        private static void SqlOracleTest()
        {
            MSSQL.Tools.ConnInfo = new ConnInfo() {ConnIp = @"192.168.0.20\sql2012", ConnPw = "abc123,"};
            MSSQL.Tools.DBName = "TS_XXJGDM_DATA";
            Console.WriteLine(ALF.MSSQL.Tools.TransferDataToOracle("XXJGDM_201504",
                new OrclConnInfo()
                {
                    ConnIp = "192.168.0.201",
                    ConnPort = "1521",
                    OrclServiceName = "orcl",
                    OrclUserId = "jgdm",
                    ConnPw = "jgdm"
                }));
            Console.ReadLine();
            //var oracleConstring = "user id=jgdm;password=jgdm;data source=192.168.0.201:1521/orcl";
            //var sqlConstring = @"Data Source=192.168.0.20\SQL2012  ;Initial Catalog=XXJGDM;User ID=sa;Password=abc123,;Pooling=False";



            //using (var sqlconn = new SqlConnection(sqlConstring))
            //{
            //    sqlconn.Open();
            //    var sqlcmd = sqlconn.CreateCommand();

            //    sqlcmd.CommandText = "select BEIZHU,DM,MC,SJBHLX,DQBS,XTGXRQ,SFCX from JG_JBZ";
            //    var sqlReader = sqlcmd.ExecuteReader();
            //    while (sqlReader.Read())
            //    {

            //        using (var orcconn = new OracleConnection(oracleConstring))
            //        {

            //            orcconn.Open();
            //            var orccmd = orcconn.CreateCommand();
            //            orccmd.CommandType = System.Data.CommandType.Text;
            //            orccmd.CommandText = string.Format(@"insert into JG_JBZ(BEIZHU,DM,MC,SJBHLX,DQBS,XTGXRQ,SFCX) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", sqlReader[0], sqlReader[1], sqlReader[2], sqlReader[3], sqlReader[4], sqlReader[5], sqlReader[6]);
            //           orccmd.ExecuteNonQuery();
            //            orcconn.Close();
            //        }
            //    }
            //    sqlconn.Close();
            //}


        }

        private static void GenerateRadomData()
        {
            //MSSQL.Tools.ConnInfo = new ConnInfo() {ConnIp = "192.168.0.20", ConnPw = "abc123,"};
            //MSSQL.Tools.DBName = "eduData2015DB";
            //string tmp;
            //var ran=new Random();
            //var colDict = ALF.EDU.EduTools.GetDataColumn(out tmp);
            //foreach (var colInfo in colDict)
            //{
            //    foreach (var colName in colInfo.Value)
            //    {
            //        var sql =
            //            string.Format("update {0} set {1}={1}/{2}", colInfo.Key, colName, ran.Next(0, 100));
            //        MSSQL.Tools.ExecSql(sql);
            //    }
            //}
        }
    }
}
