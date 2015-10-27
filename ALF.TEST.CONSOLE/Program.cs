using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using ALF.EDU.DataModel;
using ALF.MSSQL.DataModel;
using ALF.OFFICE.DataModel;
using ALF.SYSTEM.DataModel;
using Oracle.ManagedDataAccess.Client;

namespace ALF.TEST.CONSOLE
{
    class Program
    {
        static void Main()
        {
            test();

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
            if (ALF.SYSTEM.WindowsTools.IsSoftInstalled(softName))
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


                OFFICE.Tools.InitialSqSetting(d, o);

                Console.WriteLine("start output");
                Console.WriteLine(OFFICE.ExcelTools.ExportSqlToExcel("select top 10 * from eduCodeDB..schoolBusinessRelation",
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

            var oracleConstring = "user id=jgdm;password=jgdm;data source=192.168.0.201:1521/orcl";
            var sqlConstring = @"Data Source=192.168.0.20\SQL2012  ;Initial Catalog=XXJGDM;User ID=sa;Password=abc123,;Pooling=False";



            using (var sqlconn = new SqlConnection(sqlConstring))
            {
                sqlconn.Open();
                var sqlcmd = sqlconn.CreateCommand();

                sqlcmd.CommandText = "select BEIZHU,DM,MC,SJBHLX,DQBS,XTGXRQ,SFCX from JG_JBZ";
                //sqlcmd.CommandText = "insert into JG_JBZ values(@BEIZHU,@DM,@MC,@SJBHLX,@DQBS,@XTGXRQ,@SFCX)";
                var sqlReader = sqlcmd.ExecuteReader();
                while (sqlReader.Read())
                {
                    //SqlParameter parameter = new SqlParameter("@EMPNO", int.Parse(orcreader["EMPNO"].ToString()));
                    //sqlcmd.Parameters.Add(parameter);
                    //parameter = new SqlParameter("@HIREDATE", DateTime.Parse(orcreader["HIREDATE"].ToString()));
                    //sqlcmd.Parameters.Add(parameter);
                    //parameter = new SqlParameter("@COMM", decimal.Parse(orcreader["COMM"].ToString()));
                    //sqlcmd.Parameters.Add(parameter);


                    //if (orcreader.IsDBNull(1))
                    //{
                    //    parameter = new SqlParameter("@ENAME", System.Data.SqlDbType.VarChar);
                    //    parameter.Value = DBNull.Value;
                    //    sqlcmd.Parameters.Add(parameter);
                    //}


                    using (var orcconn = new OracleConnection(oracleConstring))
                    {

                        orcconn.Open();
                        var orccmd = orcconn.CreateCommand();
                        orccmd.CommandType = System.Data.CommandType.Text;
                        orccmd.CommandText = string.Format(@"insert into JG_JBZ(BEIZHU,DM,MC,SJBHLX,DQBS,XTGXRQ,SFCX) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", sqlReader[0], sqlReader[1], sqlReader[2], sqlReader[3], sqlReader[4], sqlReader[5], sqlReader[6]);
                        //var parameter = new OracleParameter("@BEIZHU", sqlReader["BEIZHU"].ToString());
                        //orccmd.Parameters.Add(parameter);
                        //parameter = new OracleParameter("@DM", sqlReader["DM"].ToString());
                        //orccmd.Parameters.Add(parameter);
                        //parameter = new OracleParameter("@MC", sqlReader["MC"].ToString());
                        //orccmd.Parameters.Add(parameter);
                        //parameter = new OracleParameter("@SJBHLX", sqlReader["SJBHLX"].ToString());
                        //orccmd.Parameters.Add(parameter);
                        //parameter = new OracleParameter("@DQBS", sqlReader["DQBS"].ToString());
                        //orccmd.Parameters.Add(parameter);
                        //parameter = new OracleParameter("@XTGXRQ", sqlReader["XTGXRQ"].ToString());
                        //orccmd.Parameters.Add(parameter);
                        //parameter = new OracleParameter("@SFCX", sqlReader["SFCX"].ToString());
                        //orccmd.Parameters.Add(parameter);
                        orccmd.ExecuteNonQuery();
                        orcconn.Close();
                    }
                }
                sqlconn.Close();
            }


        }

        private static void test()
        {
            string tmp = "";
            var t = ALF.SYSTEM.WindowsTools.XmlDeseerializer(typeof(List<ArgInfo>), @"d:\test.xml", out tmp) as List<ArgInfo>;

            foreach (var argInfo in t)
            {
                string sql = string.Format(@"

INSERT INTO eduData2015DB..[dbo].[checkArgInfo]
           ([rowid]
           ,[state]
           ,[description]
           ,[updatetime]
           ,[templateID]
           ,[templateName]
           ,[schoolAttrib]
           ,[businessType]
           ,[argBusinessGroup]
           ,[argID]
           ,[argType]
           ,[argNo]
           ,[argNo1]
           ,[argName]
           ,[argDataSql]
           ,[upLimit]
           ,[downLimit]
           ,[isUsing])
     VALUES
           ('{0}'
           ,'{1}'
           ,'{2}'
           ,'{3}'
           ,'{4}'
           ,'{5}'
           ,'{6}'
           ,'{7}'
           ,'{8}'
           ,'{9}'
           ,'{10}'
           ,'{11}'
           ,'{12}'
           ,'{13}'
           ,'{14}'
           ,'{15}'
           ,'{16}'
           ,'{17}')", argInfo.rowid
                    , argInfo.state
                    , argInfo.description
                    , argInfo.updatetime
                    , argInfo.templateID
                    , argInfo.templateName
                    , argInfo.schoolAttrib
                    , argInfo.businessType
                    , argInfo.argBusinessGroup
                    , argInfo.argID
                    , argInfo.argType
                    , argInfo.argNo
                    , argInfo.argNo1
                    , argInfo.argName
                    , argInfo.argDataSql
                    , argInfo.upLimit
                    , argInfo.downLimit
                    , argInfo.isUsing);
                ALF.MSSQL.Tools.DataBaseType = DataBaseEngineType.SqlExpress;
                MSSQL.Tools.ExecSql(sql);
            }
        }
    }
}
