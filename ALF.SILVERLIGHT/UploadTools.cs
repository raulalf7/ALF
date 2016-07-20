using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows;
using ALF.SILVERLIGHT.DataModel;
using ALF.SILVERLIGHT.SilverlightUploadServiceReference;
using Enum = ALF.SILVERLIGHT.DataModel.Enum;

namespace ALF.SILVERLIGHT
{
    /// <summary>
    ///     文件上传类
    /// </summary>
    public class UploadTools
    {
        private readonly SilverlightUploadServiceClient _client;
        
        private readonly UploadFile _file;
        private bool _firstChunk = true;
        private bool _lastChunk;

        public UploadTools(UploadFile file, string serviceUrl = @"../SilverlightUploadService.svc")
        {
            _file = file;
            if (_file.CancelAction == null)
            {
                file.CancelAction += CancelUpload;
            }
            _file.BytesUploaded = 0;

            _client = new SilverlightUploadServiceClient();
            _client.Endpoint.Address = new EndpointAddress(serviceUrl);
            _client.StoreFileAdvancedCompleted += _client_StoreFileAdvancedCompleted;
            _client.CancelUploadCompleted += client_CancelUploadCompleted;
            _client.ChannelFactory.Closed += ChannelFactory_Closed;
        }

        /// <summary>
        ///     上传文件
        /// </summary>
        public void UploadAdvanced()
        {
            var buffer = new byte[4 * 4096];
            var bytesRead = _file.FileStream.Read(buffer, 0, buffer.Length);

            _file.State = Enum.UploadStates.上传中;
            //文件是否上传完毕?
            if (bytesRead != 0)
            {

                _file.BytesUploaded += bytesRead;

                if ((long)_file.BytesUploaded == _file.FileStream.Length)
                    _lastChunk = true; //是否是最后一块数据，这样WCF会在服务端根据该信息来决定是否对临时文件重命名

                //上传当前数据块
                _client.StoreFileAdvancedAsync(_file.FilePhysicalName, buffer, bytesRead, null, _firstChunk, _lastChunk);

                //在第一条消息之后一直为false
                _firstChunk = false;
            }
            else
            {
                //当上传完毕后
                _file.FileStream.Dispose();
                _file.FileStream.Close();
                ChannelIsClosed();
            }
        }

        private void _client_StoreFileAdvancedCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //检查WEB服务是否存在错误
            if (e.Error != null)
            {
                //当错误时放弃上传
                _file.State = Enum.UploadStates.错误;
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                //如果文件未取消上传的话，则继续上传
                if (!_file.IsDeleted)
                    UploadAdvanced();
            }
        }

        #region

        /// <summary>
        ///     关闭ChannelFactory事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelFactory_Closed(object sender, EventArgs e)
        {
            ChannelIsClosed();
        }

        private void client_CancelUploadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //当取消上传完成后关闭Channel
            //   _client.ChannelFactory.Close();
        }

        /// <summary>
        ///     Channel被关闭
        /// </summary>
        private void ChannelIsClosed()
        {
            if (!_file.IsDeleted)
            {
                _file.State = Enum.UploadStates.上传完成;
            }
        }

        /// <summary>
        ///     取消上传
        /// </summary>
        public void CancelUpload()
        {
            _client.CancelUploadAsync(_file.FilePhysicalName);
        }

        #endregion
    }
}
