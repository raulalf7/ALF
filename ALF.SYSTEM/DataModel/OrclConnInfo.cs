namespace ALF.SYSTEM.DataModel
{
    /// <summary>
    /// 
    /// </summary>
    public class OrclConnInfo:ConnInfo
    {
        private string _orclServiceName;
        private string _orclOrclUserId;

        /// <summary>
        /// 连接服务名称
        /// </summary>
        public string OrclServiceName
        {
            get
            {
                return _orclServiceName;
            }
            set
            {
                if ((_orclServiceName != value))
                {
                    _orclServiceName = value;
                }
            }
        }

        /// <summary>
        /// 连接用户名
        /// </summary>
        public string OrclUserId
        {
            get
            {
                return _orclOrclUserId;
            }
            set
            {
                if ((_orclOrclUserId != value))
                {
                    _orclOrclUserId = value;
                }
            }
        }
    }
}
