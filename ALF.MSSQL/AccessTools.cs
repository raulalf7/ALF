using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace ALF.MSSQL
{
    /// <summary>
    /// Access数据库处理工具
    /// </summary>
    public static class AccessTools
    {

        /// <summary>
        /// 数据库文件位置
        /// </summary>
        public static string FilePath;

        /// <summary>
        /// 数据库密码
        /// </summary>
        public static string Password;

        /// <summary>
        /// 链接字符串
        /// </summary>
        public static string ConnString
        {
            get {
                return Password=="" ? string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='{0}';Persist Security Info=True", FilePath) : string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='{0}';User ID='admin';Password=;Jet OLEDB:Database Password='{1}'", FilePath,Password);
            }
            //
        }

        /// <summary>
        /// 执行语句
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <param name="result">执行结果</param>
        /// <returns>影响行数</returns>
        public static int ExecuteNonQuery(string cmdText,out string result)
        {
            result = "";
            if (!File.Exists(FilePath))
            {
                result = "No Data Files Found";
                return 0;
            }
            if (cmdText == "")
            {
                return 0;
            }
            try
            {
                var cmd = new OleDbCommand();
                using (var conn = new OleDbConnection(ConnString))
                {
                    PrepareCommand(cmd, conn, null, cmdText);
                    var val = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    return val;
                }
            }
            catch (Exception exception)
            {
                result = "[Error in ALF.MSSQL]Access Query Error: " + exception.Message;
                return 0;
            }
        }

        /// <summary>
        /// 根据查询语句获取查询访问器
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <param name="result">执行结果</param>
        /// <returns>查询结果访问器</returns>
        public static OleDbDataReader ExecuteReader(string cmdText, out string result)
        {
            result = "";
            if (!File.Exists(FilePath))
            {
                result = "No Data Files Found";
                return null;
            }

            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection conn = new OleDbConnection(ConnString))
            {
                try
                {
                    PrepareCommand(cmd, conn, null, cmdText);
                    OleDbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                    return reader;
                }
                catch (Exception exception)
                {
                    //关闭连接，抛出异常
                    conn.Close();
                    result = "[Error in ALF.MSSQL]Access Query Error: " + exception.Message;
                    return null;
                }
            }
        }

        /// <summary>
        /// 根据查询语句获取查询数据集
        /// </summary>
        /// <param name="cmdText">查询语句</param>
        /// <param name="result">执行结果</param>
        /// <param name="tableName">数据表名称</param>
        /// <returns>查询结果数据集</returns>
        public static DataSet ExecuteDataSet(string cmdText, out string result,string tableName="")
        {
            result = "";
            if (!File.Exists(FilePath))
            {
                result = "No Data Files Found";
                return null;
            }

            var cmd = new OleDbCommand();
            using (var conn = new OleDbConnection(ConnString))
            {
                PrepareCommand(cmd, conn, null, cmdText);
                var da = new OleDbDataAdapter(cmd);
                var ds = new DataSet();
                try
                {
                    da.Fill(ds);
                    cmd.Parameters.Clear();
                    if (tableName != "")
                    {
                        ds.Tables[0].TableName = tableName;
                    }
                    return ds;
                }
                catch (Exception exception)
                {
                    //关闭连接，抛出异常
                    conn.Close();
                    result = "[Error in ALF.MSSQL]Access Query Error: " + exception.Message;
                    return null;
                }
            }
        }

        /// <summary>
        /// 从ACCESS库中导出数据为XML
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <param name="path">保存文件完整路径</param>
        /// <param name="dataFormat">数据格式</param>
        /// <param name="tableName">表名</param>
        /// <returns>错误信息</returns>
        public static string ExportDataToXml(string sql, string path, string dataFormat, string tableName)
        {
            string tmp;
            var ds = ExecuteDataSet(sql, out tmp, tableName);
            if (tmp != "")
            {
                Console.WriteLine(tmp);
                return string.Format("导出数据错误：{0}", tmp);
            }
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                ds.WriteXmlSchema(string.Format(@"{0}{1}{2}Schema", path,tableName, dataFormat));
                ds.WriteXml(string.Format(@"{0}{1}{2}", path, tableName, dataFormat));
            
            }
            catch (Exception ex)
            {
                return string.Format("导出数据错误：{0}", ex.Message);
            }
            return "";
        }

        /// <summary>
        /// 从XML导入ACCESS
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="path">文件路径</param>
        /// <returns>错误信息</returns>
        public static string ImportDataFromXml(string tableName,string path)
        {
            var dataSet = new DataSet();
            try
            {
                dataSet.ReadXml(path);
                dataSet.ReadXmlSchema(path + "Schema");
            }
            catch (Exception ex)
            {
                return string.Format("导入数据错误：{0}", ex.Message); 
            }
            var sqlFormat = "Insert into {0} ({1}) values ({2})";

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var rowValues = "";
                var columnTags = "";
                foreach (DataColumn column in dataSet.Tables[0].Columns)
                {
                    columnTags += string.Format("{0},", column.ColumnName);

                    if (column.DataType == typeof (Guid))
                    {
                        rowValues += string.Format("{{{0}}},", row[column]);
                    }
                    else if (column.DataType == typeof(string))
                    {
                        rowValues += string.Format("'{0}',", row[column]);
                    }
                    else
                    {
                        rowValues += string.Format("{0},", row[column]);
                    }
                }
                columnTags = columnTags.Substring(0, columnTags.Length - 1);
                rowValues = rowValues.Substring(0, rowValues.Length - 1);
                var sql = string.Format(sqlFormat, dataSet.Tables[0].TableName, columnTags, rowValues);
                string tmp;
                ExecuteNonQuery(sql, out tmp);
                if (tmp != "")
                {
                    return string.Format("导入数据错误：{0}，导入SQL{1}", tmp,sql);
                }
            }
            return "";
        }

        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
        }
        
    }
}
