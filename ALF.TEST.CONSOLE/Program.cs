using System;
using System.IO;
using System.Threading;
using ALF.MSSQL;
using ALF.MSSQL.DataModel;
using ALF.OFFICE;
using ALF.OFFICE.DataModel;
using ALF.SYSTEM;
using ALF.SYSTEM.DataModel;
using Microsoft.Win32;
using Tools = ALF.OFFICE.Tools;

namespace ALF.TEST.CONSOLE
{
    class Program
    {
        static void Main()
        {
            var s =ALF.SYSTEM.WindowsTools.ReadFromTxt(@"D:\test.txt");


            var r = ALF.SYSTEM.EncryptionTool.Md5Encrypt(s, "raulalf7");

            WindowsTools.WriteToTxt(@"d:\e.txt",r);

            var d = EncryptionTool.Md5Decrypt(r, "raulalf7");

            WindowsTools.WriteToTxt(@"D:\d.txt", d);

            Console.ReadLine();
           // var list = ALF.MSSQL.Tools.DataSetTransferToList(new Int32(), ds);
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
            Console.WriteLine(ALF.MSSQL.Tools.TransferDataToOracle("XXJGDM_201601",
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

        private static void Set()
        {
            string test = @"
--学校模板--

declare	@organizationeType int
declare @recordYear int
declare	@organizationNo nvarchar(50)
declare	@businessTypeNo  nvarchar(50)
declare	@templateOwner  nvarchar(50)


set @organizationeType=5
set @recordYear=2015
set @organizationNo='2134019362'
set @businessTypeNo='119'
set @templateOwner ='34'


	create table #data_getTemplateAndInstanceList_tmp
	(
		organizationNo  nvarchar(50) not null,
		organizationName nvarchar(50) not null, 
		businessID uniqueidentifier not null,
		businessType	nvarchar(50) not null,
		businessTypeNo	nvarchar(50) not null,
		instanceID	uniqueidentifier not null,
		approveState int not null,
		templateBusinessGroup nvarchar(50) not null,
		templateBusinessType nvarchar(50) not null,
		templateCategory nvarchar(50) not null,
		templateGroup nvarchar(50) not null,
		templateType nvarchar(50) not null,
		templateNo	nvarchar(50) not null,
		templateName	nvarchar(50) not null,
		templateNoDisplay	nvarchar(255) not null
	)
	
	--[1]
	if @organizationeType=5
	begin
		insert into #data_getTemplateAndInstanceList_tmp(
			organizationNo,
			organizationName,
			businessID,
			businessType,
			businessTypeNo,
			instanceID,
			approveState,
			templateBusinessGroup,
			templateBusinessType,
			templateCategory,
			templateGroup,
			templateType,
			templateNo,
			templateName,
			templateNoDisplay
		)
		select schoolBusinessRelation.organizationNo
			  ,schoolBusinessRelation.organizationName
			  ,schoolBusinessRelation.businessID
			  ,schoolBusinessRelation.businessType
			  ,schoolBusinessRelation.businessTypeNo
			  ,isnull(instanceID,'00000000-0000-0000-0000-000000000000') as instanceID
			  ,isnull(approveState,-3000) as approveState
			  ,excelTemplateTable.templateBusinessGroup
			  ,excelTemplateTable.templateBusinessType
			  ,excelTemplateTable.templateCategory
			  ,excelTemplateTable.templateGroup
			  ,excelTemplateTable.templateType
			  ,excelTemplateTable.templateNo
			  ,excelTemplateTable.templateName
			  ,excelTemplateTable.templateNoDisplay
			  
		  from (select organizationNo
		              ,businessType
		              ,businessTypeNo 
		              ,organizationName
		              ,businessID
  					  ,businessLevel
					  ,ownerTypeNo
					  ,isNation
					  ,isExistsDoubleLanguage
					  ,isNetSchool
					  ,isLastYearCancel
		          from schoolBusinessRelation 
		         where organizationNo=@organizationNo 
		           and businessTypeNo=@businessTypeNo
		           and recordYear=@recordYear
		           ) as schoolBusinessRelation
		  inner join (select * from excelTemplateBusinessRelation 
		               where businessTypeNo=@businessTypeNo 
		                 and recordYear=@recordYear) as excelTemplateBusinessRelation
		  on excelTemplateBusinessRelation.businessTypeNo=schoolBusinessRelation.businessTypeNo
		  and excelTemplateBusinessRelation.businessLevel=(case when excelTemplateBusinessRelation.businessLevel=-1 then -1 else schoolBusinessRelation.businessLevel end) 
		  and excelTemplateBusinessRelation.ownerTypeNo=(case when excelTemplateBusinessRelation.ownerTypeNo='' then '' else schoolBusinessRelation.ownerTypeNo end)
		  and excelTemplateBusinessRelation.isNation=(case when excelTemplateBusinessRelation.isNation=-1 then -1 else schoolBusinessRelation.isNation end)
		  and excelTemplateBusinessRelation.isLastYearCancel=(case when excelTemplateBusinessRelation.isLastYearCancel=-1 then -1 else schoolBusinessRelation.isLastYearCancel end)
		  and excelTemplateBusinessRelation.isExistsDoubleLanguage=(case when excelTemplateBusinessRelation.isExistsDoubleLanguage=-1 then -1 else schoolBusinessRelation.isExistsDoubleLanguage end)
		  and excelTemplateBusinessRelation.isNetSchool=(case when excelTemplateBusinessRelation.isNetSchool=-1 then -1 else schoolBusinessRelation.isNetSchool end)
	  inner join (select * from excelTemplateTable 
	               where (case when templateOwner='' then '' else templateOwner end)=(case when templateOwner='' then '' else @templateOwner end) 
	                 and recordYear=@recordYear ) as excelTemplateTable
		  on excelTemplateBusinessRelation.templateNo=excelTemplateTable.templateNo
		  left join 
		    (select instanceID
	              ,approveState
	              ,templateNo
	              ,businessTypeNo 
	         from instanceTable 
	        where organizationNo=@organizationNo
	          and recordYear=@recordYear) as instanceTable	
	      on excelTemplateBusinessRelation.templateNo=instanceTable.templateNo
		  and schoolBusinessRelation.businessTypeNo=instanceTable.businessTypeNo

	end
	
	--[2]
	if @organizationeType=0
	begin
		declare @countyCount int 
		set @countyCount=-1
		
		select @countyCount=count(1)
		  from dbo.statisticsEntity 
		 where statisticsRegionA+statisticsRegionB in (
					select distinct regionA+regionB from dbo.codeRegionD where regionC=''
		       )
			and statisticsOrganizationLevel=2
			and statisticsOrganizationNo=@organizationNo
		
		insert into #data_getTemplateAndInstanceList_tmp(
			organizationNo,
			organizationName,
			businessID,
			businessType,
			businessTypeNo,
			instanceID,
			approveState,
			templateBusinessGroup,
			templateBusinessType,
			templateCategory,
			templateGroup,
			templateType,
			templateNo,
			templateName,
			templateNoDisplay
		)
		select statisticsEntity.organizationNo
			  ,statisticsEntity.organizationName
			  ,'00000000-0000-0000-0000-000000000000' as businessID
			  ,'' as businessType
			  ,'' as businessTypeNo
			  ,isnull(instanceID,'00000000-0000-0000-0000-000000000000') as instanceID
			  ,isnull(approveState,-3000) as approveState
			  ,excelTemplateTable.templateBusinessGroup
			  ,excelTemplateTable.templateBusinessType
			  ,excelTemplateTable.templateCategory
			  ,excelTemplateTable.templateGroup
			  ,excelTemplateTable.templateType
			  ,excelTemplateTable.templateNo
			  ,excelTemplateTable.templateName
			  ,excelTemplateTable.templateNoDisplay
		  from (select statisticsOrganizationNo as organizationNo
		              ,statisticsOrganizationName as organizationName
		              ,statisticsOrganizationLevel
		              ,isImmediacy
		          from statisticsEntity 
		         where statisticsOrganizationNo=@organizationNo
		           and recordYear=@recordYear) as statisticsEntity
		  inner join (select * from excelTemplateStatisticsRelation 
		               where recordYear=@recordYear) as excelTemplateStatisticsRelation
		  on excelTemplateStatisticsRelation.organizationLevel=(case when @countyCount>0 then 3 else statisticsEntity.statisticsOrganizationLevel end)
		  and excelTemplateStatisticsRelation.isImmediacy=(case when @countyCount>0 then 0 else statisticsEntity.isImmediacy end)
	  inner join (select * from excelTemplateTable 
	               where (case when templateOwner='' then '' else templateOwner end)=(case when templateOwner='' then '' else @templateOwner end) 
	                 and recordYear=@recordYear ) as excelTemplateTable
		  on excelTemplateStatisticsRelation.templateNo=excelTemplateTable.templateNo
		  left join 
		    (select instanceID
	              ,approveState
	              ,templateNo
	         from instanceTable 
	        where organizationNo=@organizationNo
	          and recordYear=@recordYear) as instanceTable	
	      on excelTemplateStatisticsRelation.templateNo=instanceTable.templateNo				
	end

	select  
	        organizationNo,
		    organizationName, 
		    businessID,
			businessType,
			businessTypeNo,
			instanceID,
			approveState,
			templateBusinessGroup,
			templateBusinessType,
			templateCategory,
			templateGroup,
			templateType,
			templateNo,
			templateName,
			templateNoDisplay
	   from #data_getTemplateAndInstanceList_tmp 
	  order by businessTypeNo,templateNo
";

            string tmp;

            string t2 = "select top 1 * from excelTemplateBusinessRelation";
            MSSQL.AccessTools.FilePath = @".\eduData2015DB.mdb";
            var t = ALF.MSSQL.AccessTools.ExecuteDataSet(t2, out tmp);
            
        }

        private static void  Encrypt(string filePath,string encryptPath)
        {
            var dataString = WindowsTools.ReadFromTxt(filePath);
            var encryptString = EncryptionTool.SymmetriEncrypt(dataString,5,"");
            WindowsTools.WriteToTxt(encryptPath, encryptString);
        }

        private static void Decrypt(string encryptPath,string filePath)
        {
            var encryptString = WindowsTools.ReadFromTxt(encryptPath);
            bool result;
            var dataString = EncryptionTool.SymmetricDecrypt(encryptString,5,"");
            WindowsTools.WriteToTxt(filePath, dataString);
        }
        

        private static void ConvertSqlToXml()
        {

        }
    }
}
