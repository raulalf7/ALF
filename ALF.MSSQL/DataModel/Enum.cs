namespace ALF.MSSQL.DataModel
{
    /// <summary>
    /// 数据库引擎版本枚举列表
    /// </summary>
    public enum DataBaseEngineType
    {
        /// <summary>
        /// SQL SERVER正式版
        /// </summary>
        MsSqlServer,
        
        /// <summary>
        /// SQL EXPRESS版
        /// </summary>
        SqlExpress,

        /// <summary>
        /// 远程服务器
        /// </summary>
        Remote,
    }
}
