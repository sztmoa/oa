using System.Windows.Controls;

// 内容摘要: 系统日志展示页面
      
namespace SMT.SAAS.Platform.Xamls
{
    /// <summary>
    /// 系统日志展示页面
    /// </summary>
    public partial class SystemLogger : UserControl
    {
        /// <summary>
        /// 创建一个日志展示的新实例。
        /// </summary>
        public SystemLogger()
        {
            InitializeComponent();

            dgLogList.ItemsSource = null;
            dgLogList.ItemsSource = Logging.Logger.Current.Logs;
        }
    }
}
