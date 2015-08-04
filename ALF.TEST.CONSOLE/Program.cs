using System;
using ALF.MSSQL.DataModel;
using ALF.OFFICE.DataModel;

namespace ALF.TEST.CONSOLE
{
    class Program
    {
        static void Main(string[] args)
        {
            var readString = "";
            while (readString!="exit")
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



                readString=Console.ReadLine();
            }
        }
    }
}
