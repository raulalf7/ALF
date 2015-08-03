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
            rowid = Guid.NewGuid();
            connID = Guid.NewGuid();
            updatetime = DateTime.Now;
            isEnabled = true;
        }

        public Guid rowid
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

        public Guid connID
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

        public string state
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

        public string description
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

        public DateTime updatetime
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

        public string connName
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

        public string connPw
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

        public string connIp
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

        public bool isEnabled
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
