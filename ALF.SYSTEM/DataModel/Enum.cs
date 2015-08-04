namespace ALF.SYSTEM.DataModel
{
    /// <summary>
    /// 生成文件类型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// CSV
        /// </summary>
        CSV,

        /// <summary>
        /// DATA
        /// </summary>
        Data,
    }

    ///// <summary>
    ///// 数据类型
    ///// </summary>
    //public enum DataType
    //{
    //    /// <summary>
    //    /// 直属高校数据
    //    /// </summary>
    //    GzData,

    //    /// <summary>
    //    /// 代码数据
    //    /// </summary>
    //    CodeData,
    //}

    /// <summary>
    /// 配置文件类型
    /// </summary>
    public enum ConfigType
    {
        /// <summary>
        /// 导入文件
        /// </summary>
        Input,

        /// <summary>
        /// 上报文件
        /// </summary>
        Up,

        /// <summary>
        /// 代码导入文件
        /// </summary>
        CodeInput
    }
}
