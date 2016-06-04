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
        /// <param name="commandParameters">查询参数</param>
        /// <returns>影响行数</returns>
        public static int ExecuteNonQuery(string cmdText,out string result, params OleDbParameter[] commandParameters)
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
                    PrepareCommand(cmd, conn, null, cmdText, commandParameters);
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
        /// <param name="commandParameters">查询参数</param>
        /// <returns>查询结果访问器</returns>
        public static OleDbDataReader ExecuteReader(string cmdText, out string result, params OleDbParameter[] commandParameters)
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
                    PrepareCommand(cmd, conn, null, cmdText, commandParameters);
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
        /// <param name="commandParameters">查询参数</param>
        /// <returns>查询结果数据集</returns>
        public static DataSet ExecuteDataSet(string cmdText, out string result, params OleDbParameter[] commandParameters)
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
                PrepareCommand(cmd, conn, null, cmdText, commandParameters);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();
                try
                {
                    da.Fill(ds);
                    cmd.Parameters.Clear();
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

        public static string ExportDataToXml(string cmdText, string tableName, string xmlFilePath, params OleDbParameter[] commandParameters)
        {

            using (OleDbConnection conn = new OleDbConnection(ConnString))
            {
                DataSet dataSet = new DataSet();
                OleDbCommand cmd = new OleDbCommand();
                PrepareCommand(cmd, conn, null, cmdText, commandParameters);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataTable dataTable = dataSet.Tables.Add("Names_Table");
                try
                {
                    da.Fill(dataTable);
                    dataTable.WriteXmlSchema("Names.Schema.xml");
                    dataTable.WriteXml("Names.xml");
                    return "";
                }
                catch (Exception exception)
                {
                    //关闭连接，抛出异常
                    conn.Close();
                    return "[Error in ALF.MSSQL]Access Query Error: " + exception.Message;
                    return null;
                }
            }
        }

        private static void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;
            if (cmdParms != null)
            {
                foreach (OleDbParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        
    }
}
