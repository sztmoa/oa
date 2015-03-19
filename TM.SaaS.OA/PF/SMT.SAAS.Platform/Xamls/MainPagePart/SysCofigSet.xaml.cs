using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

// 内容摘要: 系统设置页面
      
namespace SMT.SAAS.Platform.Xamls.MainPagePart
{
    /// <summary>
    /// 系统设置页面
    /// </summary>
    public partial class SysCofigSet : UserControl,ICleanup
    {
        
        public SysCofigSet()
        {
            InitializeComponent();
        }

        public void Cleanup()
        {
            //MessageBox.Show("Cleanup : SysCofigSet");
        }
    }
}
