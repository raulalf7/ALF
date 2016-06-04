namespace ALF.SILVERLIGHT.DataModel
{
    public class Enum
    {
        /// <summary>
        /// 可能的状态
        /// </summary>
        public enum UploadStates
        {
            /// <summary>
            /// 暂停
            /// </summary>
            等待上传 = 0,

            /// <summary>
            /// 上传中
            /// </summary>
            上传中 = 1,

            /// <summary>
            /// 结束
            /// </summary>
            上传完成 = 2,

            /// <summary>
            /// 移除
            /// </summary>
            移除 = 3,

            /// <summary>
            /// 错误
            /// </summary>
            错误 = 4
        }
    }
}
