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


        #region Public Methods

        /// <summary>
        /// 初始化SQL里面的OLEDB设置
        /// </summary>
        /// <param name="dataBaseEngineType">所用数据库引擎</param>
        /// <param name="officeVersion">所用OFFICE版本</param>
        /// <returns>初始化结果</returns>
        public static string InitialSqSetting(DataBaseEngineType dataBaseEngineType, OfficeVersion officeVersion)
        {
            OfficeVersion = officeVersion;
            MSSQL.Tools.DataBaseType = dataBaseEngineType;
            var cmd = @"
USE [master]    
GO    
EXEC master.dbo.sp_MSset_oledb_prop N'Microsoft.ACE.OLEDB.12.0', N'AllowInProcess', 1    
GO    
EXEC master.dbo.sp_MSset_oledb_prop N'Microsoft.ACE.OLEDB.12.0', N'DynamicParameters', 1    
GO   ";
            switch (OfficeVersion)
            {
                case OfficeVersion.Office2007:;
                    break;
            }
            return MSSQL.Tools.ExecSql(cmd);
        }

        #endregion
    }
}
