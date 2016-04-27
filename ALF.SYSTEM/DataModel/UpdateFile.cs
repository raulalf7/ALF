using System;

namespace ALF.SYSTEM.DataModel
{
    /// <summary>
    /// 上传文件信息
    /// </summary>
    [Serializable]
    public class UpdateFile
    {
        /// <summary>
        /// 备案年份
        /// </summary>
        public int RecordYear  //
        { set; get; }

        /// <summary>
        /// 必要软件版本
        /// </summary>
        public double MustSoftVer  //
        { set; get; }

        /// <summary>
        /// 必要数据库版本
        /// </summary>
        public double MustDBVer  // 
        { set; get; }

        /// <summary>
        /// 序列号
        /// </summary>
        public int SequenceID
        { set; get; }

        /// <summary>
        /// 版本号
        /// </summary>
        public double Ver
        { set; get; }

        /// <summary>
        /// 生成文件类型
        /// </summary>
        public ConfigFileType FileType
        { set; get; }

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        { set; get; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize
        { set; get; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        { set; get; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe
        { set; get; }

        /// <summary>
        /// 当前路径
        /// </summary>
        public string CurrentPath
        {
            set;
            get;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMessage
        { set; get; }

    }
}
