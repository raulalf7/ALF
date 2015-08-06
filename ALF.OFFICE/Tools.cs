using ALF.MSSQL.DataModel;
using ALF.OFFICE.DataModel;

namespace ALF.OFFICE
{
    /// <summary>
    /// Office工具
    /// </summary>
    public static class Tools
    {

        #region Public Fields

        /// <summary>
        /// 当前使用OFFICE版本
        /// </summary>
        public static OfficeVersion OfficeVersion { get; private set; }

        #endregion


        #region Private Fields

        private const string DynamicSettingStringFormat = @"
EXEC master.dbo.sp_MSset_oledb_prop N'Microsoft.ACE.OLEDB.12.0', N'AllowInProcess', {0}
EXEC master.dbo.sp_MSset_oledb_prop N'Microsoft.ACE.OLEDB.12.0', N'DynamicParameters', {0}";

        #endregion


        #region Public Methods

        /// <summary>
        /// 初始化SQL里面的OLEDB设置(OFFICE 2007及以上)
        /// </summary>
        /// <param name="dataBaseEngineType">所用数据库引擎</param>
        /// <param name="officeVersion">所用OFFICE版本</param>
        /// <returns>初始化结果</returns>
        public static string InitialSqSetting(DataBaseEngineType dataBaseEngineType, OfficeVersion officeVersion)
        {
            if (officeVersion != OfficeVersion.Office2007 && officeVersion != OfficeVersion.Office2010 &&
                officeVersion != OfficeVersion.Office2013)
            {
                return "该方法仅支持OFFICE 2007及以上版本";
            }
            OfficeVersion = officeVersion;
            MSSQL.Tools.DataBaseType = dataBaseEngineType;
            return MSSQL.Tools.ExecSql(string.Format(DynamicSettingStringFormat, "1"));
        }


        /// <summary>
        /// 重置SQL里面的OLEDB设置(OFFICE 2007及以上)
        /// </summary>
        /// <param name="dataBaseEngineType">所用数据库引擎</param>
        /// <param name="officeVersion">所用OFFICE版本</param>
        /// <returns>重置结果</returns>
        public static string ResetSqlSetting(DataBaseEngineType dataBaseEngineType, OfficeVersion officeVersion)
        {
            if (officeVersion != OfficeVersion.Office2007 && officeVersion != OfficeVersion.Office2010 &&
                officeVersion != OfficeVersion.Office2013)
            {
                return "该方法仅支持OFFICE 2007及以上版本";
            }
            OfficeVersion = officeVersion;
            MSSQL.Tools.DataBaseType = dataBaseEngineType;
            return MSSQL.Tools.ExecSql(string.Format(DynamicSettingStringFormat,"0"));
        }

        #endregion
    }
}
