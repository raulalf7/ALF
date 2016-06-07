using System;
using System.Collections.Generic;
using System.IO;
using ALF.MSSQL.DataModel;
using ALF.OFFICE.DataModel;
using Microsoft.Win32;

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<OfficeVersion> GetOfficeVersions()
        {
            var resultList = new List<OfficeVersion>();
            var rk  = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64: RegistryView.Registry32);
            var office97 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\8.0\Excel\InstallRoot\");
            //office 2000
            var office2000 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\9.0\Excel\InstallRoot\");
            //office xp
            var officexp = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\10.0\Excel\InstallRoot\");
            //office 2003
            var office2003 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\11.0\Excel\InstallRoot\");
            //office2007
            var office2007 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\12.0\Excel\InstallRoot\");
            //office2010
            var office2010 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\14.0\Excel\InstallRoot\");
            //office2013
            var office2013 = rk.OpenSubKey(@"SOFTWARE\Microsoft\Office\15.0\Excel\InstallRoot");
            
            if (office97 != null)
            {
                var file97 = office97.GetValue("Path").ToString();
                if (File.Exists(file97 + "Excel.exe"))
                {
                    resultList.Add(OfficeVersion.Office97);
                }
            }
            if (office2000 != null)
            {
                var file = office2000.GetValue("Path").ToString();
                if (File.Exists(file + "Excel.exe"))
                {
                    resultList.Add(OfficeVersion.Office2000);
                }
            }
            if (officexp != null)
            {
                var file = officexp.GetValue("Path").ToString();
                if (File.Exists(file + "Excel.exe"))
                {
                    resultList.Add(OfficeVersion.Office2000);
                }
            }
            if (office2003 != null)
            {
                var file = office2003.GetValue("Path").ToString();
                if (File.Exists(file + "Excel.exe"))
                {
                    resultList.Add(OfficeVersion.Office2000);
                }
            }
            if (office2007 != null)
            {
                var file = office2007.GetValue("Path").ToString();
                if (File.Exists(file + "Excel.exe"))
                {
                    resultList.Add(OfficeVersion.Office2000);
                }
            }
            if (office2010 != null)
            {
                var file = office2010.GetValue("Path").ToString();
                if (File.Exists(file + "Excel.exe"))
                {
                    resultList.Add(OfficeVersion.Office2000);
                }
            }

            if (office2013 != null)
            {
                var file = office2013.GetValue("Path").ToString();
                if (File.Exists(file + "Excel.exe"))
                {
                    resultList.Add(OfficeVersion.Office2013);
                }
            }
            return resultList;

        }

        #endregion
    }
}
