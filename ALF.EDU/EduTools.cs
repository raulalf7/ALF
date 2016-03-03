using System;
using System.Collections.Generic;
using System.Globalization;
using ALF.EDU.DataModel;
using ALF.MSSQL;

namespace ALF.EDU
{
    /// <summary>
    /// 教育事业相关数据库工具
    /// </summary>
    public static class EduTools
    {


        private static int _linkN;
        private static EduDataClassDataContext _dataDB;

        #region Public Fields

        /// <summary>
        /// 备案年份
        /// </summary>
        public static string RecordYear = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// 
        /// </summary>
        public static bool UseEduDB;

        #endregion


        /// <summary>
        /// 教育事业统计数据库名称
        /// </summary>
        public static string EduDBName
        {
            get
            {
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


        /// <summary>
        /// 
        /// </summary>
        public static EduDataClassDataContext DataDB
        {
            get
            {
                GC.Collect();
                if (_dataDB == null)
                {
                    _linkN = 0;
                    _dataDB = new EduDataClassDataContext(Tools.SQLConnString) { CommandTimeout = 1500 };
                }
                Console.Write("[{0}]", _linkN++);
                return _dataDB;
            }
            set { _dataDB = value; }
        }


        public static Dictionary<string,List<string>> GetDataColumn( out string result)
        {
            var resultDict = new Dictionary<string, List<string>>();
            var tableList =
                Tools.GetSqlListString(
                    "select   templateNo  from  eduData2015DB..excelTemplateTable where templateGroup='基表' and templateOwner='' order by templateNo",
                    out result);
            if (result != "")
            {
                return null;
            }
            foreach (var table in tableList)
            {
                var list = Tools.GetSqlListString(string.Format("select distinct columnTag from excelTemplateCell where templateNo='{0}' and showBackgroundColor in('FFFFFF', '8DB4E3')",table), out result);
                if (result != "")
                {
                    return null;
                }
                resultDict.Add(table, list);
            }
            return resultDict;
        }
    }
}
