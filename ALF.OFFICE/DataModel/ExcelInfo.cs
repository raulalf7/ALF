using System;
using System.Data.Linq.Mapping;

namespace ALF.OFFICE.DataModel
{
    /// <summary>
    /// Excel信息
    /// </summary>
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

        /// <summary>
        /// ExcelID
        /// </summary>
        [Column(Storage = "_excelID", DbType = "UniqueIdentifier NOT NULL")]
        public Guid ExcelID
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

        /// <summary>
        /// 文件路径
        /// </summary>
        [Column(Storage = "_filePath", DbType = "NVarChar(MAX) NOT NULL", CanBeNull = false)]
        public string FilePath
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

        /// <summary>
        /// 文件名称
        /// </summary>
        [Column(Storage = "_fileName", DbType = "NVarChar(255) NOT NULL", CanBeNull = false)]
        public string FileName
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

        /// <summary>
        /// Excel名称
        /// </summary>
        [Column(Storage = "_excelName", DbType = "NVarChar(255) NOT NULL", CanBeNull = false)]
        public string ExcelName
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

        /// <summary>
        /// 起始行
        /// </summary>
        [Column(Storage = "_rowStart", DbType = "Int NOT NULL")]
        public int RowStart
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

        /// <summary>
        /// 行数
        /// </summary>
        [Column(Storage = "_rowsCount", DbType = "Int NOT NULL")]
        public int RowsCount
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

        /// <summary>
        /// 起始列
        /// </summary>
        [Column(Storage = "_columnStart", DbType = "Int NOT NULL")]
        public int ColumnStart
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

        /// <summary>
        /// 列数
        /// </summary>
        [Column(Storage = "_columnCount", DbType = "Int NOT NULL")]
        public int ColumnCount
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

        /// <summary>
        /// 是否包含列名
        /// </summary>
        [Column(Storage = "_hasTitle", DbType = "Int NOT NULL")]
        public string HasTitle
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

        /// <summary>
        /// 是否插入
        /// </summary>
        [Column(Storage = "_isInsert", DbType = "Int NOT NULL")]
        public string IsInsert
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

        /// <summary>
        /// 页签名称
        /// </summary>
        [Column(Storage = "_sheetName", DbType = "NVarChar(255) NOT NULL", CanBeNull = false)]
        public string SheetName
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
