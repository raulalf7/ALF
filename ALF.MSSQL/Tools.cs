using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using ALF.MSSQL.DataModel;
using ALF.SYSTEM.DataModel;

namespace ALF.MSSQL
{
    public static class Tools
    {
        #region Public Properties

        public static DataBaseEngineType DataBaseType
        {
            get { return _dataBaseEngineType; }
            set
            {
                if (value == DataBaseEngineType.Remote)
                {
                    return;
                }
                _dataBaseEngineType = value;
                _connInfo = null;
            }
        }

        public static ConnInfo ConnInfo
        {
            get { return _connInfo; }
            set
            {
                _connInfo = value;
                DataBaseType = DataBaseEngineType.Remote;
            }
        }

        public static string CatalogName
        {
            get { return _catalogName; }
            set
            {
                _catalogName = value;
                UseEduDB = false;
            }
        }

        #endregion


        #region Public Fields

        public static string RecordYear = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
        public static bool UseEduDB;

        #endregion


        #region Private Fields

        private const string SqlConnStringFormat = "Data Source={0};Initial Catalog={2}; {1} ;";
        private static ConnInfo _connInfo;
        private static DataBaseEngineType _dataBaseEngineType = DataBaseEngineType.MsSqlServer;
        private static string _catalogName = "master";

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

        private static string SQLConnString
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

        private static string DBName
        {
            get
            {
                if (!UseEduDB)
                {
                    return CatalogName;
                }

                switch (RecordYear)
                {
                    case "2011":
                        return "eduHistoryDataDB";
                    case "2012":
                        return "eduDataDB";
                    default:
                        return string.Format("eduData{0}DB", RecordYear);
                }
            }
        }

        public static string ServiceName
        {
            get
            {
                if (DataBaseType == DataBaseEngineType.SqlExpress)
                {
                    return "MSSQL$SQLEXPRESS";
                }
                if (DataBaseType == DataBaseEngineType.MsSqlServer)
                {
                    return "MSSQLSERVER";
                }
                return "";
            }
        }

        #endregion


        #region Public Methods

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

        public static string ExportCSV(string sql, string filePath)
        {
            Console.WriteLine("Exporting CSV [{0}]", filePath);
            sql = sql.Replace("\r", " ");
            sql = sql.Replace("\n", " ");
            string cmd = string.Format("\" {0}\" queryout  {1}  -c -t \",\"  {2}", sql, filePath, BCPServerName);
            return SYSTEM.WindowsTools.ExecCmd("bcp.exe", cmd, true);
        }

        #endregion

    }
}
