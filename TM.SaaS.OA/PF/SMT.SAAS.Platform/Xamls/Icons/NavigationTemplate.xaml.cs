
// 内容摘要: 主面板中导航标签模板
using System.Windows.Controls;
using System;

namespace SMT.SAAS.Platform.Xamls.Icons
{
    /// <summary>
    /// 主面板中导航标签模板
    /// </summary>
    public partial class NavigationTemplate : UserControl
    {
        /// <summary>
        /// 创建一个模版的实例。
        /// </summary>
        public NavigationTemplate()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 标注导航标签
        /// </summary>
        /// <param name="Nname"></param>
        public NavigationTemplate(string Nname)
        {
            InitializeComponent();
            NNavigation.Text = Nname;
        }
    }
}
