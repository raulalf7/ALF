using System;
using System.ComponentModel;
using System.IO;

namespace ALF.SILVERLIGHT.DataModel
{
    /// <summary>
    ///     上传文件信息类
    /// </summary>
    public class UploadFile : INotifyPropertyChanged
    {
        public Action CancelAction;

        /// <summary>
        ///当前已上传的字节数（这里与FileCollection中的同名属性意义不同，FileCollection中的是已上传的所有文件的字节总数）
        /// </summary>
        private double _bytesUploaded;

        /// <summary>
        ///     上传文件名称
        /// </summary>
        private string _fileName;

        /// <summary>
        ///     当前文件大小
        /// </summary>
        private double _fileSize;

        /// <summary>
        ///     上传文件的流信息
        /// </summary>
        private Stream _fileStream;


        /// <summary>
        ///     是否取消上传该文件
        /// </summary>
        private bool _isDeleted;

        /// <summary>
        ///     已上传文件的百分比
        /// </summary>
        private int _percentage;

        /// <summary>
        ///     当前上传文件状态
        /// </summary>
        private Enum.UploadStates _state = Enum.UploadStates.Pending;

        /// <summary>
        ///     上传文件名称
        /// </summary>
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        /// <summary>
        ///     当前上传文件的状态，注意这时使用了NotifyPropertyChanged来通知FileRowControl控件中的FileRowControl_PropertyChanged事件
        /// </summary>
        public Enum.UploadStates State
        {
            get { return _state; }
            set
            {
                _state = value;

                NotifyPropertyChanged("State");
            }
        }

        /// <summary>
        ///     当前上传文件是否已被移除，注意这时使用了NotifyPropertyChanged来通知FileCollection类中的item_PropertyChanged事件
        /// </summary>
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                _isDeleted = value;

                if (_isDeleted && CancelAction != null)
                    CancelAction();

                NotifyPropertyChanged("IsDeleted");
            }
        }

        /// <summary>
        ///     上传文件的流信息
        /// </summary>
        public Stream FileStream
        {
            get { return _fileStream; }
            set
            {
                _fileStream = value;

                if (_fileStream != null)
                    _fileSize = _fileStream.Length;
            }
        }

        /// <summary>
        ///     当前文件大小
        /// </summary>
        public double FileSize
        {
            get { return _fileSize; }
        }

        /// <summary>
        ///     当前已上传的字节数（这里与FileCollection中的同名属性意义不同，FileCollection中的是已上传的所有文件的字节总数）
        /// </summary>
        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;

                NotifyPropertyChanged("BytesUploaded");

                Percentage = (int)((value * 100) / _fileStream.Length);
            }
        }

        /// <summary>
        ///     已上传文件的百分比（这里与FileCollection中的同名属性意义不同，FileCollection中的是已上传字符数占全部字节数的百分比,该字段的修改事件通知会发给page.xmal中的TotalProgress）
        /// </summary>
        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                NotifyPropertyChanged("Percentage");
            }
        }

        #region INotifyPropertyChanged Members

        private void NotifyPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
