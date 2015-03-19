
// 内容摘要: 登录界面-加载动画
using System.Windows.Controls;

namespace SMT.SAAS.Platform.Xamls.MainPagePart
{
    /// <summary>
    /// 登录界面-加载动画
    /// </summary>
    public partial class LoadingLogin : UserControl
    {
        /// <summary>
        /// 创建一个LoadingLogin的新实例
        /// </summary>
        public LoadingLogin()
        {
            InitializeComponent();
            SBRolling.Begin();
        }
    }
}
