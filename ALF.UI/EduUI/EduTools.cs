using System;

namespace ALF.UI.EduUI
{
    internal class EduTools
    {
        private static int _linkN;
        private static EduDataLINQDataContext _dataDB;

        public static EduDataLINQDataContext DataDB
        {
            get
            {
                GC.Collect();
                if (_dataDB == null)
                {
                    _linkN = 0;
                    _dataDB = new EduDataLINQDataContext(MSSQL.Tools.SQLConnString) { CommandTimeout = 1500 };
                }
                Console.Write("[{0}]", _linkN++);
                return _dataDB;
            }
            set { _dataDB = value; }
        }
    }
}
