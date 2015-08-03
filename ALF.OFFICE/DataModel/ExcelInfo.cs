using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALF.OFFICE.DataModel
{
    public class ExcelInfo : ICloneable
    {
        private Guid _excelID;

        private string _filePath;

        private string _fileName;

        private string _excelName;

        private int _rowStart = 1;

        private int _rowsCount;

        private int _columnStart = 1;

        private int _columnCount;

        private string _hasTitle;

        private string _isInsert;

        private string _sheetName;

        public object Clone()
        {
            return MemberwiseClone();
        }

        [Column(Storage = "_excelID", DbType = "UniqueIdentifier NOT NULL")]
        public Guid excelID
        {
            get { return _excelID; }
            set
            {
                if ((_excelID != value))
                {
                    _excelID = value;
                }
            }
        }

        [Column(Storage = "_filePath", DbType = "NVarChar(MAX) NOT NULL", CanBeNull = false)]
        public string filePath
        {
            get { return _filePath; }
            set
            {
                if ((_filePath != value))
                {
                    _filePath = value;
                }
            }
        }

        [Column(Storage = "_fileName", DbType = "NVarChar(255) NOT NULL", CanBeNull = false)]
        public string fileName
        {
            get { return _fileName; }
            set
            {
                if ((_fileName != value))
                {
                    _fileName = value;
                }
            }
        }

        [Column(Storage = "_excelName", DbType = "NVarChar(255) NOT NULL", CanBeNull = false)]
        public string excelName
        {
            get { return _excelName; }
            set
            {
                if ((_excelName != value))
                {
                    _excelName = value;
                }
            }
        }

        [Column(Storage = "_rowStart", DbType = "Int NOT NULL")]
        public int rowStart
        {
            get { return _rowStart; }
            set
            {
                if ((_rowStart != value))
                {
                    _rowStart = value;
                }
            }
        }

        [Column(Storage = "_rowsCount", DbType = "Int NOT NULL")]
        public int rowsCount
        {
            get { return _rowsCount; }
            set
            {
                if ((_rowsCount != value))
                {
                    _rowsCount = value;
                }
            }
        }

        [Column(Storage = "_columnStart", DbType = "Int NOT NULL")]
        public int columnStart
        {
            get { return _columnStart; }
            set
            {
                if ((_columnStart != value))
                {
                    _columnStart = value;
                }
            }
        }

        [Column(Storage = "_columnCount", DbType = "Int NOT NULL")]
        public int columnCount
        {
            get { return _columnCount; }
            set
            {
                if ((_columnCount != value))
                {
                    _columnCount = value;
                }
            }
        }

        [Column(Storage = "_hasTitle", DbType = "Int NOT NULL")]
        public string hasTitle
        {
            get { return _hasTitle; }
            set
            {
                if ((_hasTitle != value))
                {
                    _hasTitle = value;
                }
            }
        }

        [Column(Storage = "_isInsert", DbType = "Int NOT NULL")]
        public string isInsert
        {
            get { return _isInsert; }
            set
            {
                if ((_isInsert != value))
                {
                    _isInsert = value;
                }
            }
        }

        [Column(Storage = "_sheetName", DbType = "NVarChar(255) NOT NULL", CanBeNull = false)]
        public string sheetName
        {
            get { return _sheetName; }
            set
            {
                if ((_sheetName != value))
                {
                    _sheetName = value;
                }
            }
        }
    }
}
