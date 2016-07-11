using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using ALF.MSSQL.DataModel;
using ALF.SYSTEM.DataModel;
using Oracle.ManagedDataAccess.Client;

namespace ALF.MSSQL
{
    /// <summary>
    /// MSSQL 处理工具
    /// </summary>
    public static class Tools
    {
        #region Public Properties

        /// <summary>
        /// 当前使用数据库引擎
        /// </summary>
        public static DataBaseEngineType DataBaseType
        {
            get { return _dataBaseEngineType; }
            set
            {
                _dataBaseEngineType = value;
                if (value == DataBaseEngineType.Remote)
                {
                    return;
                }
                _connInfo = null;
            }
        }

        /// <summary>
        /// 当前数据库连接信息
        /// </summary>
        public static ConnInfo ConnInfo
        {
            get { return _connInfo; }
            set
            {
                _connInfo = value;
                DataBaseType = DataBaseEngineType.Remote;
            }
        }

        /// <summary>
        /// 初始数据库名称
        /// </summary>
        public static string DBName
        {
            get { return _dbName; }
            set
            {
                _dbName = value;
            }
        }


        /// <summary>
        /// 数据库引擎服务名称
        /// </summary>
        public static string ServiceName
        {
            get
            {
                switch (DataBaseType)
                {
                    case DataBaseEngineType.SqlExpress:
                        return "MSSQL$SQLEXPRESS";
                    case DataBaseEngineType.MsSqlServer:
                        return "MSSQLSERVER";
                }
                return "";
            }
        }


        /// <summary>
        /// SQL连接字符串
        /// </summary>
        public static string SQLConnString
        {
            get
            {
                if (DataBaseType == DataBaseEngineType.Remote)
                {
                    return string.Format(SqlConnStringFormat, ConnInfo.ConnIp,
                                         string.Format("User ID=sa;Password={0}", ConnInfo.ConnPw), DBName);
                }
                return string.Format(SqlConnStringFormat, ServerName, "Integrated Security=True", DBName);
            }
        }

        #endregion


        #region Private Fields

        private const string SqlConnStringFormat = "Data Source={0};Initial Catalog={2}; {1} ;";
        private static ConnInfo _connInfo;
        private static DataBaseEngineType _dataBaseEngineType = DataBaseEngineType.MsSqlServer;
        private static string _dbName = "master";

        #endregion


        #region Private Properties

        private static string ServerName
        {
            get
            {
                if (DataBaseType == DataBaseEngineType.SqlExpress)
                {
                    return @".\sqlexpress";
                }
                if (DataBaseType == DataBaseEngineType.MsSqlServer)
                {
                    return ".";
                }
                return ConnInfo.ConnIp;
            }
        }

        private static string BCPServerName
        {
            get
            {
                if (DataBaseType == DataBaseEngineType.MsSqlServer)
                {
                    return "-T";
                }
                if (DataBaseType == DataBaseEngineType.SqlExpress)
                {
                    return @"-S .\sqlexpress -T ";
                }
                return string.Format("-S {0} -U sa -P {1}", ConnInfo.ConnIp, ConnInfo.ConnPw);
            }
        }

        #endregion


        #region Public Methods

        /// <summary>
        /// 判断数据库是否开启
        /// </summary>
        /// <returns>是否开启</returns>
        public static bool IsDBOpen()
        {
            string result;
            DataView tmp = GetSqlDataView("select COUNT(0) from master.sys.master_files where name='master'", out result,
                                          10);
            if (tmp == null)
            {
                return false;
            }
            return "1" == tmp.Table.Rows[0][0].ToString();
        }

        /// <summary>
        /// 执行SQL修改语句
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>执行结果</returns>
        public static string ExecSql(string sql, int timeout = 36000)
        {
            using (var conn = new SqlConnection(SQLConnString + "Connect Timeout=" + timeout))
            {
                conn.Open();
                try
                {
                    var sqlCommand = new SqlCommand(sql, conn) { CommandTimeout = timeout };
                    sqlCommand.ExecuteNonQuery();
                    conn.Close();
                    return "";
                }
                catch (Exception ex)
                {
                    conn.Close();
                    return ex.Message;
                }
            }
        }

        /// <summary>
        /// 执行SQL查询语句，返回DataView
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="result">执行结果</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>查询结果DataView</returns>
        public static DataView GetSqlDataView(string sql, out string result, int timeout = 3600)
        {
            using (var conn = new SqlConnection(SQLConnString + "Connect Timeout=" + timeout))
            {
                result = "";
                var dt = new DataTable();
                try
                {
                    var cmd = new SqlCommand(sql, conn) { CommandTimeout = timeout };
                    var da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception e)
                {
                    result = e.Message;
                    Console.WriteLine(e.Message);
                    return null;
                }
                return new DataView(dt);
            }
        }

        /// <summary>
        /// 执行SQL查询语句，返回字符串列表
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="result">执行结果</param>
        /// <param name="timeout">超时时间</param>
        /// <returns>查询结果字符串列表</returns>
        public static List<string> GetSqlListString(string sql, out string result, int timeout = 3600)
        {
            using (var conn = new SqlConnection((SQLConnString + "Connect Timeout=" + timeout)))
            {
                var cmd = new SqlCommand(sql, conn);
                result = "";
                try
                {
                    conn.Open();
                    var resultList = new List<string>();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        resultList.Add(reader[0].ToString());
                    }
                    conn.Close();
                    return resultList;
                }
                catch (Exception ex)
                {
                    result = ex.Message;
                    conn.Close();
                    return null;
                }
            }
        }

        /// <summary>
        /// 执行SQL查询语句，导出CSV
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="filePath">导出文件路径</param>
        /// <returns>执行结果</returns>
        public static string ExportCSV(string sql, string filePath)
        {
            Console.WriteLine("Exporting CSV [{0}]", filePath);
            sql = sql.Replace("\r", " ");
            sql = sql.Replace("\n", " ");
            var cmd = string.Format("\" {0}\" queryout  {1}  -c -t \",\"  {2}", sql, filePath, BCPServerName);
            return SYSTEM.WindowsTools.ExecCmd("bcp.exe", cmd, true);
        }

        /// <summary>
        /// 将数据整表从SQLSERVER中导入到ORACLE，要求ORACLE已有相同的表及表结构
        /// </summary>
        /// <param name="tableName">需要导出的报表名称</param>
        /// <param name="orclConnInfo">Orcale链接信息</param>
        /// <param name="infoCount">输出行数</param>
        /// <returns></returns>
        public static string TransferDataToOracle(string tableName, OrclConnInfo orclConnInfo, int infoCount=100)
        {
            string result;
            var sqlCmdString =
                string.Format("select b.name from sysobjects a,syscolumns b where a.name = '{0}' and a.id=b.id order by colid",
                    tableName);

            var colList = GetSqlListString(sqlCmdString, out result);
            if (result != "")
            {
                return result;
            }
            var colString = "(";
            int n = 0;
            foreach (var colName in colList)
            {
                colString += colName + ",";
                n++;
            }
            colString = colString.Substring(0, colString.Length - 1) + ")";
            
            sqlCmdString=string.Format("select * from [{0}]",tableName);
            var sqlConn =new SqlConnection(SQLConnString);
            var sqlCmd =new SqlCommand(sqlCmdString,sqlConn);

            try
            {
                sqlConn.Open();
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            var sqlReader = sqlCmd.ExecuteReader();

            int count = 0;
            var orclCmdString = string.Format(" insert into {0} {1} ", tableName, colString);
            while (sqlReader.Read())
            {
                try
                {
                    orclCmdString += "select ";
                    for (int index = 0; index < n; index++)
                    {
                        orclCmdString += "'" + sqlReader[index].ToString().Trim() + "',";
                    }
                    orclCmdString = orclCmdString.Substring(0, orclCmdString.Length - 1) + " from dual union all\n";
                }
                catch (Exception exception)
                {
                    sqlConn.Close();
                    return exception.Message;
                }

                count++;
                if (count % infoCount == 0)
                {
                    orclCmdString = orclCmdString.Substring(0, orclCmdString.Length - 10);

                    //orclCmdString += "commit;\n exception\n when others then \n rollback; end;";
                    result = ExecOracleSql(orclCmdString, orclConnInfo);
                    Console.WriteLine(@"Already Inserted {0} rows of data", count);
                    orclCmdString = string.Format(" insert into {0} {1} ", tableName, colString);
                }
                if (result != "")
                {
                    sqlConn.Close();
                    return result;
                }
            }
            if (count % infoCount != 0)
            {
                orclCmdString = orclCmdString.Substring(0, orclCmdString.Length - 10);
               // orclCmdString += "commit;\n exception\n when others then \n rollback; end;";
                result = ExecOracleSql(orclCmdString, orclConnInfo);
                Console.WriteLine(@"Already Inserted {0} rows of data", count);
            }
            sqlConn.Close();
            Console.WriteLine(@"Already Inserted {0} rows of data. Finished.", count);
            return result;
        }
        
        /// <summary>
        /// 将DataSet转化为泛型的List
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ds">需要转换的数据集</param>
        /// <typeparam name="T">需要转换成为的类型</typeparam>
        /// <returns></returns>
        public static List<T> DataSetTransferToList<T>(T entity, DataSet ds) where T : new()
        {
            var lists = new List<T>();
            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            if (ds.Tables[0].Rows.Count <= 0) return lists;
            lists.AddRange(from DataRow row in ds.Tables[0].Rows select DataRowTransfer(new T(), row));
            return lists;
        }


        /// <summary>
        /// 从XML导入SQLSERVER
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="filePath">文件路径</param>
        /// <returns>错误信息</returns>
        public static string ImportDataFromXml(string tableName, string filePath)
        {

            using (var conn = new SqlConnection(SQLConnString))
            {
                conn.Open();
                string result="";
                var bcp = new SqlBulkCopy(conn) { DestinationTableName = tableName };
                var ds = new DataSet();
                if (!File.Exists(filePath) || !File.Exists(filePath + "Schema"))
                {
                    result = string.Format("导入数据失败：{0}【数据文件或者配置文件不存在】", filePath);
                    Console.WriteLine(result);
                    return result;
                }
                try
                {
                    ds.ReadXmlSchema(filePath + "Schema");
                    ds.ReadXml(filePath);
                    bcp.WriteToServer(ds.Tables[0]);
                }
                catch (Exception ex)
                {
                    conn.Close();
                    result = string.Format("导入数据失败：{0}【{1}】", filePath, ex.Message);
                    Console.WriteLine(result);
                    return result;
                }
                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName"></param>
        public static void CloseDB(string dbName)
        {
            try
            {
                var tmp = DBName;
                DBName = "master";
                ExecSql(string.Format("ALTER DATABASE [{0}] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE", dbName));
                ExecSql(string.Format("EXEC master.dbo.sp_detach_db @dbname = N'{0}'", dbName));
                Console.WriteLine(@"数据库{0}已Close", dbName);
                DBName = tmp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="fileName"></param>
        public static void OpenDB(string dbName, string fileName)
        {
            var tmp = DBName;
            DBName = "master";
            ExecSql(string.Format(@"CREATE DATABASE [{0}] ON ( FILENAME = N'{1}.mdf' ),( FILENAME = N'{1}_log.ldf' ) FOR ATTACH ", dbName, fileName));
            ExecSql(@"EXEC sys.sp_configure N'remote query timeout (s)', N'0'");
            ExecSql(@"RECONFIGURE WITH OVERRIDE");
            Console.WriteLine(@"数据库{0}已Open", dbName);
            DBName = tmp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbName"></param>
        /// <returns></returns>
        public static string GetDBFilePath(string dbName)
        {
            string tmp;
            var result = GetSqlListString(string.Format(@"select physical_name from master.sys.master_files where file_id=1 and database_id=DB_ID('{0}')  ",dbName),out tmp);
            if (tmp != "")
            {
                Console.WriteLine(@"发生错误{0}", result);
            }

            return result[0];
        }



        #endregion


        #region Private Methods

        private static string ExecOracleSql(string sql, OrclConnInfo orclConnInfo)
        {
            var orclConnString = string.Format("user id={0};password={1};data source={2}:{3}/{4}",
                orclConnInfo.OrclUserId, orclConnInfo.ConnPw,
                orclConnInfo.ConnIp, orclConnInfo.ConnPort, orclConnInfo.OrclServiceName);

            var orclConn = new OracleConnection(orclConnString);
            orclConn.Open();
            var orclCmd = orclConn.CreateCommand();
            orclCmd.CommandType = CommandType.Text;
            orclCmd.CommandText = sql;
            try
            {
                orclCmd.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                orclConn.Close();
                return exception.Message;
            }
            orclConn.Close();

            return "";
        }

        private static T DataRowTransfer<T>(T entity, DataRow row) where T : new()
        {
            //初始化 如果为null
            if (entity == null)
            {
                entity = new T();
            }
            //得到类型
            var type = typeof(T);
            //取得属性集合
            var pi = type.GetProperties();
            foreach (var item in pi.Where(item => row[item.Name] != null && row[item.Name] != DBNull.Value))
            {
                try
                {
                    item.SetValue(entity,
                        item.PropertyType == typeof (DateTime?)
                            ? Convert.ToDateTime(row[item.Name].ToString())
                            : Convert.ChangeType(row[item.Name], item.PropertyType), null);
                }
                catch (Exception exception)
                {
                    Console.WriteLine("【类型转换发生错误】属性：[{0}],类型：[{1}]，错误信息：{2}", item.Name, item.PropertyType.FullName, exception.Message);
                }
            }
            return entity;
        }
        
        #endregion
    }
}
