using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ALF.SILVERLIGHT.DataModel
{
    public class UploadFileCollection : ObservableCollection<UploadFile>
    {

        /// <summary>
        ///     已上传的累计（多文件）字节数
        /// </summary>
        private double _bytesUploaded;

        /// <summary>
        ///     已上传字符数占全部字节数的百分比
        /// </summary>
        private int _percentage;

        /// <summary>
        ///     构造方法
        /// </summary>
        public UploadFileCollection()
        {
            CollectionChanged += FileCollection_CollectionChanged;
        }

        /// <summary>
        ///     已上传的累计（多文件）字节数,该字段的修改事件通知会发给page.xmal中的TotalKB
        /// </summary>
        public double BytesUploaded
        {
            get { return _bytesUploaded; }
            set
            {
                _bytesUploaded = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BytesUploaded"));
            }
        }

        /// <summary>
        ///     已上传字符数占全部字节数的百分比,该字段的修改事件通知会发给page.xmal中的TotalProgress
        /// </summary>
        public int Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Percentage"));
            }
        }

        /// <summary>
        ///     依次加入所选的上传文件信息
        /// </summary>
        /// <param name="item"></param>
        public new void Add(UploadFile item)
        {
            item.PropertyChanged += item_PropertyChanged;
            base.Add(item);
        }

        /// <summary>
        ///     单个上传文件属性改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //当属性变化为“从上传列表中移除”
            if (e.PropertyName == "IsDeleted")
            {
                var file = (UploadFile)sender;

                if (file.IsDeleted)
                {
                    Remove(file);
                }
            }
            RecountTotal();
        }

        /// <summary>
        ///     重新计算数据
        /// </summary>
        private void RecountTotal()
        {
            //Recount total
            double totalSize = 0;
            double totalSizeDone = 0;

            foreach (var file in this)
            {
                if (file.State == Enum.UploadStates.Pending || file.State == Enum.UploadStates.Uploading)
                {
                    totalSize += file.FileSize;
                    totalSizeDone += file.BytesUploaded;
                }
            }

            double percentage = 0;

            if (totalSize > 0)
                percentage = 100 * totalSizeDone / totalSize;

            BytesUploaded = totalSizeDone;

            Percentage = (int)percentage;
        }

        /// <summary>
        ///     当添加或取消上传文件时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //当集合信息变化时（添加或删除项）时，则重新计算数据 
            RecountTotal();
        }
    }
}
