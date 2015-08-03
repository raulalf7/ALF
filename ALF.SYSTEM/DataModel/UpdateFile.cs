using System;

namespace ALF.SYSTEM.DataModel
{
    [Serializable]
    public class UpdateFile
    {

        public int RecordYear  //
        { set; get; }


        public double MustSoftVer  //
        { set; get; }


        public double MustDBVer  // 
        { set; get; }



        public int SequenceID
        { set; get; }


        public double Ver
        { set; get; }


        public FileType FileType
        { set; get; }


        public DataType DataType
        { set; get; }


        public string FileName
        { set; get; }


        public int FileSize
        { set; get; }


        public string Title
        { set; get; }


        public string Describe
        { set; get; }


        public string CurrentPath
        {
            set;
            get;
        }

        public string ErrMessage
        { set; get; }

    }
}
