using System;

namespace ALF.EDU.DataModel
{
    public class WordInfo
    {
        private Guid _rowid;
        private string _state;
        private string _description;
        private DateTime _updatetime;
        private Guid _templateID;
        private string _templateName;
        private string _regionPath;
        private string _regionType;
        private string _wordPath;
        private string _wordFileName;
        private Guid _wordID;
        public Guid rowid
        {
            get
            {
                return this._rowid;
            }
            set
            {
                if (this._rowid != value)
                {
                    this._rowid = value;
                }
            }
        }
        public string state
        {
            get
            {
                return this._state;
            }
            set
            {
                if (this._state != value)
                {
                    this._state = value;
                }
            }
        }
        public string description
        {
            get
            {
                return this._description;
            }
            set
            {
                if (this._description != value)
                {
                    this._description = value;
                }
            }
        }
        public DateTime updatetime
        {
            get
            {
                return this._updatetime;
            }
            set
            {
                if (this._updatetime != value)
                {
                    this._updatetime = value;
                }
            }
        }
        public Guid templateID
        {
            get
            {
                return this._templateID;
            }
            set
            {
                if (this._templateID != value)
                {
                    this._templateID = value;
                }
            }
        }
        public Guid wordID
        {
            get
            {
                return this._wordID;
            }
            set
            {
                if (this._wordID != value)
                {
                    this._wordID = value;
                }
            }
        } 

        public string templateName
        {
            get
            {
                return this._templateName;
            }
            set
            {
                if (this._templateName != value)
                {
                    this._templateName = value;
                }
            }
        }
        public string wordFileName
        {
            get
            {
                return this._wordFileName;
            }
            set
            {
                if (this._wordFileName != value)
                {
                    this._wordFileName = value;
                }
            }
        }
        public string regionPath
        {
            get
            {
                return this._regionPath;
            }
            set
            {
                if (this._regionPath != value)
                {
                    this._regionPath = value;
                }
            }
        }
        public string regionType
        {
            get
            {
                return this._regionType;
            }
            set
            {
                if (this._regionType != value)
                {
                    this._regionType = value;
                }
            }
        }
        public string wordPath
        {
            get
            {
                return this._wordPath;
            }
            set
            {
                if (this._wordPath != value)
                {
                    this._wordPath = value;
                }
            }
        }
    }
    public enum ArgType
    {
        文字,
        表格,
        图形
    }
}