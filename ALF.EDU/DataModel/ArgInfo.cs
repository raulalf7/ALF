using System;
using System.ComponentModel;

namespace ALF.EDU.DataModel
{
    public class ArgInfo : INotifyPropertyChanged
    {
        private Guid _rowid;
        private string _state;
        private string _description;
        private DateTime? _updatetime;
        private string _argBusinessGroup;
        private Guid? _templateID;
        private string _templateName;
        private Guid _argID;
        private string _schoolAttrib;
        private string _businessType;
        private string _upLimit;
        private string _downLimit;
        private int _isUsing;
        private string _argType;
        private string _argDataSql;
        private int _argNo;
        private int _argNo1;
        private string _argName;
        public event PropertyChangedEventHandler PropertyChanged;

        public Guid rowid
        {
            get { return _rowid; }
            set
            {
                if (_rowid != value)
                {
                    _rowid = value;
                }
            }
        }

        public Guid argID
        {
            get { return _argID; }
            set
            {
                if (_argID != value)
                {
                    _argID = value;
                }
            }
        }

        public string state
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                }
            }
        }

        public string description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                }
            }
        }

        public DateTime? updatetime
        {
            get { return _updatetime; }
            set
            {
                var updatetime = _updatetime;
                var dateTime = value;
                if (((updatetime.HasValue != dateTime.HasValue)
                    ? 1
                    : ((!updatetime.HasValue)
                        ? 0
                        : ((updatetime.GetValueOrDefault() != dateTime.GetValueOrDefault()) ? 1 : 0))) != 0)
                {
                    _updatetime = value;
                }
            }
        }

        public Guid? templateID
        {
            get { return _templateID; }
            set
            {
                var templateID = _templateID;
                var guid = value;
                if (((templateID.HasValue != guid.HasValue)
                    ? 1
                    : ((!templateID.HasValue)
                        ? 0
                        : ((templateID.GetValueOrDefault() != guid.GetValueOrDefault()) ? 1 : 0))) != 0)
                {
                    _templateID = value;
                }
            }
        }

        public string schoolAttrib
        {
            get { return _schoolAttrib; }
            set
            {
                if (_schoolAttrib != value)
                {
                    _schoolAttrib = value;
                }
            }
        }

        public string businessType
        {
            get { return _businessType; }
            set
            {
                if (_businessType != value)
                {
                    _businessType = value;
                }
            }
        }

        public string upLimit
        {
            get { return _upLimit; }
            set
            {
                if (_upLimit != value)
                {
                    _upLimit = value;
                }
            }
        }

        public string downLimit
        {
            get { return _downLimit; }
            set
            {
                if (_downLimit != value)
                {
                    _downLimit = value;
                }
            }
        }

        public int isUsing
        {
            get { return _isUsing; }
            set
            {
                if (_isUsing != value)
                {
                    _isUsing = value;
                }
            }
        }

        public string argType
        {
            get { return _argType; }
            set
            {
                if (_argType != value)
                {
                    _argType = value;
                }
            }
        }

        public string argDataSql
        {
            get { return _argDataSql; }
            set
            {
                if (_argDataSql != value)
                {
                    _argDataSql = value;
                    OnPropertyChanged("argDataSql");
                }
            }
        }

        public int argNo
        {
            get { return _argNo; }
            set
            {
                if (_argNo != value)
                {
                    _argNo = value;
                }
            }
        }

        public int argNo1
        {
            get { return _argNo1; }
            set
            {
                if (_argNo1 != value)
                {
                    _argNo1 = value;
                }
            }
        }

        public string argBusinessGroup
        {
            get { return _argBusinessGroup; }
            set
            {
                if (_argBusinessGroup != value)
                {
                    _argBusinessGroup = value;
                }
            }
        }

        public string argName
        {
            get { return _argName; }
            set
            {
                if (_argName != value)
                {
                    _argName = value;
                }
            }
        }

        public string templateName
        {
            get { return _templateName; }
            set
            {
                if (_templateName != value)
                {
                    _templateName = value;
                }
            }
        }

        public ArgInfo()
        {
            argID = Guid.NewGuid();
            rowid = Guid.NewGuid();
            schoolAttrib = "基础教育";
            businessType = "小学";
            isUsing = 1;
        }

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}