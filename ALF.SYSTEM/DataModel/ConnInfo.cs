using System;

namespace ALF.SYSTEM.DataModel
{
    public class ConnInfo
    {
        private Guid _rowid;

        private string _state;

        private string _description;

        private DateTime _updatetime;

        private Guid _connID;

        private string _connName;

        private string _connIp;

        private string _connPw;

        private bool _isEnabled;

        public ConnInfo()
        {
            Rowid = Guid.NewGuid();
            ConnID = Guid.NewGuid();
            Updatetime = DateTime.Now;
            IsEnabled = true;
        }

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
