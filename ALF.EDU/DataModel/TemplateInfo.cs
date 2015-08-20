using System;

namespace ALF.EDU.DataModel
{
    public class TemplateInfo
    {
        private Guid _rowid;
        private string _state;
        private string _description;
        private DateTime _updatetime;
        private Guid _templateID;
        private int _templateVersion;
        private string _templateName;
        private string _templateSize;
        private string _templatePath;
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
        public string templateSize
        {
            get
            {
                return this._templateSize;
            }
            set
            {
                if (this._templateSize != value)
                {
                    this._templateSize = value;
                }
            }
        }
        public int templateVersion
        {
            get
            {
                return this._templateVersion;
            }
            set
            {
                if (this._templateVersion != value)
                {
                    this._templateVersion = value;
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
        public string templatePath
        {
            get
            {
                return this._templatePath;
            }
            set
            {
                if (this._templatePath != value)
                {
                    this._templatePath = value;
                }
            }
        }
    }
}