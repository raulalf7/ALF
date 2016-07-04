namespace ALF.SYSTEM.DataModel
{

    /// <summary>
    /// 数据文件类型（标识文件的用途来源）
    /// </summary>
    public enum ConfigFileType
    {
        /// <summary>
        /// CSV
        /// </summary>
        CSV,
        /// <summary>
        /// 
        /// </summary>
        Code,
        /// <summary>
        /// 
        /// </summary>
        Data,
        /// <summary>
        /// 
        /// </summary>
        DBF,
        /// <summary>
        /// 
        /// </summary>
        MiniData,
        /// <summary>
        /// 
        /// </summary>
        GzData
    }

    /// <summary>
    /// 数据文件格式
    /// </summary>
    public enum ConfigFileFormat
    {
        /// <summary>
        /// 
        /// </summary>
        Input,
        /// <summary>
        /// 
        /// </summary>
        InputMini,
        /// <summary>
        /// 
        /// </summary>
        Up,
        /// <summary>
        /// 
        /// </summary>
        UpMini
    }
}
