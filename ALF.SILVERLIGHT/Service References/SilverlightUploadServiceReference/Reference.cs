﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 5.0.61118.0
// 
namespace ALF.SILVERLIGHT.SilverlightUploadServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="SilverlightUploadServiceReference.SilverlightUploadService")]
    public interface SilverlightUploadService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:SilverlightUploadService/StoreFileAdvanced", ReplyAction="urn:SilverlightUploadService/StoreFileAdvancedResponse")]
        System.IAsyncResult BeginStoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk, System.AsyncCallback callback, object asyncState);
        
        void EndStoreFileAdvanced(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:SilverlightUploadService/CancelUpload", ReplyAction="urn:SilverlightUploadService/CancelUploadResponse")]
        System.IAsyncResult BeginCancelUpload(string fileName, System.AsyncCallback callback, object asyncState);
        
        void EndCancelUpload(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SilverlightUploadServiceChannel : ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SilverlightUploadServiceClient : System.ServiceModel.ClientBase<ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService>, ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService {
        
        private BeginOperationDelegate onBeginStoreFileAdvancedDelegate;
        
        private EndOperationDelegate onEndStoreFileAdvancedDelegate;
        
        private System.Threading.SendOrPostCallback onStoreFileAdvancedCompletedDelegate;
        
        private BeginOperationDelegate onBeginCancelUploadDelegate;
        
        private EndOperationDelegate onEndCancelUploadDelegate;
        
        private System.Threading.SendOrPostCallback onCancelUploadCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public SilverlightUploadServiceClient() {
        }
        
        public SilverlightUploadServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SilverlightUploadServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SilverlightUploadServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SilverlightUploadServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> StoreFileAdvancedCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CancelUploadCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService.BeginStoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginStoreFileAdvanced(fileName, data, dataLength, parameters, firstChunk, lastChunk, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService.EndStoreFileAdvanced(System.IAsyncResult result) {
            base.Channel.EndStoreFileAdvanced(result);
        }
        
        private System.IAsyncResult OnBeginStoreFileAdvanced(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string fileName = ((string)(inValues[0]));
            byte[] data = ((byte[])(inValues[1]));
            int dataLength = ((int)(inValues[2]));
            string parameters = ((string)(inValues[3]));
            bool firstChunk = ((bool)(inValues[4]));
            bool lastChunk = ((bool)(inValues[5]));
            return ((ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService)(this)).BeginStoreFileAdvanced(fileName, data, dataLength, parameters, firstChunk, lastChunk, callback, asyncState);
        }
        
        private object[] OnEndStoreFileAdvanced(System.IAsyncResult result) {
            ((ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService)(this)).EndStoreFileAdvanced(result);
            return null;
        }
        
        private void OnStoreFileAdvancedCompleted(object state) {
            if ((this.StoreFileAdvancedCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.StoreFileAdvancedCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void StoreFileAdvancedAsync(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk) {
            this.StoreFileAdvancedAsync(fileName, data, dataLength, parameters, firstChunk, lastChunk, null);
        }
        
        public void StoreFileAdvancedAsync(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk, object userState) {
            if ((this.onBeginStoreFileAdvancedDelegate == null)) {
                this.onBeginStoreFileAdvancedDelegate = new BeginOperationDelegate(this.OnBeginStoreFileAdvanced);
            }
            if ((this.onEndStoreFileAdvancedDelegate == null)) {
                this.onEndStoreFileAdvancedDelegate = new EndOperationDelegate(this.OnEndStoreFileAdvanced);
            }
            if ((this.onStoreFileAdvancedCompletedDelegate == null)) {
                this.onStoreFileAdvancedCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnStoreFileAdvancedCompleted);
            }
            base.InvokeAsync(this.onBeginStoreFileAdvancedDelegate, new object[] {
                        fileName,
                        data,
                        dataLength,
                        parameters,
                        firstChunk,
                        lastChunk}, this.onEndStoreFileAdvancedDelegate, this.onStoreFileAdvancedCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService.BeginCancelUpload(string fileName, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginCancelUpload(fileName, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService.EndCancelUpload(System.IAsyncResult result) {
            base.Channel.EndCancelUpload(result);
        }
        
        private System.IAsyncResult OnBeginCancelUpload(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string fileName = ((string)(inValues[0]));
            return ((ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService)(this)).BeginCancelUpload(fileName, callback, asyncState);
        }
        
        private object[] OnEndCancelUpload(System.IAsyncResult result) {
            ((ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService)(this)).EndCancelUpload(result);
            return null;
        }
        
        private void OnCancelUploadCompleted(object state) {
            if ((this.CancelUploadCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CancelUploadCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CancelUploadAsync(string fileName) {
            this.CancelUploadAsync(fileName, null);
        }
        
        public void CancelUploadAsync(string fileName, object userState) {
            if ((this.onBeginCancelUploadDelegate == null)) {
                this.onBeginCancelUploadDelegate = new BeginOperationDelegate(this.OnBeginCancelUpload);
            }
            if ((this.onEndCancelUploadDelegate == null)) {
                this.onEndCancelUploadDelegate = new EndOperationDelegate(this.OnEndCancelUpload);
            }
            if ((this.onCancelUploadCompletedDelegate == null)) {
                this.onCancelUploadCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCancelUploadCompleted);
            }
            base.InvokeAsync(this.onBeginCancelUploadDelegate, new object[] {
                        fileName}, this.onEndCancelUploadDelegate, this.onCancelUploadCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService CreateChannel() {
            return new SilverlightUploadServiceClientChannel(this);
        }
        
        private class SilverlightUploadServiceClientChannel : ChannelBase<ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService>, ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService {
            
            public SilverlightUploadServiceClientChannel(System.ServiceModel.ClientBase<ALF.SILVERLIGHT.SilverlightUploadServiceReference.SilverlightUploadService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginStoreFileAdvanced(string fileName, byte[] data, int dataLength, string parameters, bool firstChunk, bool lastChunk, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[6];
                _args[0] = fileName;
                _args[1] = data;
                _args[2] = dataLength;
                _args[3] = parameters;
                _args[4] = firstChunk;
                _args[5] = lastChunk;
                System.IAsyncResult _result = base.BeginInvoke("StoreFileAdvanced", _args, callback, asyncState);
                return _result;
            }
            
            public void EndStoreFileAdvanced(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("StoreFileAdvanced", _args, result);
            }
            
            public System.IAsyncResult BeginCancelUpload(string fileName, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = fileName;
                System.IAsyncResult _result = base.BeginInvoke("CancelUpload", _args, callback, asyncState);
                return _result;
            }
            
            public void EndCancelUpload(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("CancelUpload", _args, result);
            }
        }
    }
}
