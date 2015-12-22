using System;

namespace ALF.SYSTEM.DataModel
{
    /// <summary>
    /// 连接信息
    /// </summary>
    public class ConnInfo
    {
        private Guid _rowid;

        private string _state;

        private string _description;

        private DateTime _updatetime;

        private Guid _connID;

        private string _connPort;

        private string _connName;

        private string _connIp;

        private string _connPw;

        private bool _isEnabled;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConnInfo()
        {
            Rowid = Guid.NewGuid();
            ConnID = Guid.NewGuid();
            Updatetime = DateTime.Now;
            IsEnabled = true;
        }

        /// <summary>
        /// 行编号
        /// </summary>
        public Guid Rowid
        {
            get
            {
                return _rowid;
            }
            set
            {
                if ((_rowid != value))
                {
                    _rowid = value;
                }
            }
        }

        /// <summary>
        /// 连接ID
        /// </summary>
        public Guid ConnID
        {
            get
            {
                return _connID;
            }
            set
            {
                if ((_connID != value))
                {
                    _connID = value;
                }
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                if ((_state != value))
                {
                    _state = value;
                }
            }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public string ConnPort
        {
            get
            {
                return _connPort;
            }
            set
            {
                if ((_connPort != value))
                {
                    _connPort = value;
                }
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if ((_description != value))
                {
                    _description = value;
                }
            }
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Updatetime
        {
            get
            {
                return _updatetime;
            }
            set
            {
                if ((_updatetime != value))
                {
                    _updatetime = value;
                }
            }
        }

        /// <summary>
        /// 连接名称
        /// </summary>
        public string ConnName
        {
            get
            {
                return _connName;
            }
            set
            {
                if ((_connName != value))
                {
                    _connName = value;
                }
            }
        }

        /// <summary>
        /// 连接密码
        /// </summary>
        public string ConnPw
        {
            get
            {
                return _connPw;
            }
            set
            {
                if ((_connPw != value))
                {
                    _connPw = value;
                }
            }
        }

        /// <summary>
        /// 连接IP
        /// </summary>
        public string ConnIp
        {
            get
            {
                return _connIp;
            }
            set
            {
                if ((_connIp != value))
                {
                    _connIp = value;
                }
            }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                if ((_isEnabled != value))
                {
                    _isEnabled = value;
                }
            }
        }
    }
}
