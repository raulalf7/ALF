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

        private static readonly string ConnString =
            string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='{0}';Persist Security Info=True", FilePath);

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

            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection conn = new OleDbConnection(ConnString))
            {
                PrepareCommand(cmd, conn, null, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
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
                catch
                {
                    //关闭连接，抛出异常
                    conn.Close();
                    throw;
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
                catch
                {
                    //关闭连接，抛出异常
                    conn.Close();
                    throw;
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
