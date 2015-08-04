using System;
using System.Globalization;

namespace ALF.MSSQL
{
    /// <summary>
    /// 教育事业相关数据库工具
    /// </summary>
    public static class EduTools
    {


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
    }
}
