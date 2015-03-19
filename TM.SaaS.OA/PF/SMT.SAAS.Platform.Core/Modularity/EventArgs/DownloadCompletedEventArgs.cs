using System;
using System.ComponentModel;
using System.IO;

 
namespace SMT.SAAS.Platform.Core.Modularity
{
    /// <summary>
    /// 为<see cref="IFileDownloader.DownloadCompleted"/> 事件提供事件参数。
    /// </summary>
    public class DownloadCompletedEventArgs : AsyncCompletedEventArgs
    {
        private readonly Stream result;

        /// <summary>
        /// 初始化<see cref="DownloadCompletedEventArgs"/>的实例。
        /// </summary>
        /// <param name="result">
        /// 下载的<see cref="Stream"/>
        /// .</param>
        /// <param name="error">
        /// 异步操作过程中有可能参数的错误。
        /// </param>
        /// <param name="canceled">
        /// 表明是否取消异步操作。
        /// <param name="userState">
        /// 可选的用户提供状态对象，用来标识引发 MethodNameCompleted 事件的任务。
        /// </param>
        public DownloadCompletedEventArgs(Stream result, Exception error, bool canceled, object userState)
            : base(error, canceled, userState)
        {
            this.result = result;
        }

        /// <summary>
        /// 获取下载的<see cref="Stream"/>。
        /// </summary>
        public Stream Result
        {
            get { return this.result; }
        }
    }
}