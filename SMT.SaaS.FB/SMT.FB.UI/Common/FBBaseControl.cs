using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SMT.SaaS.FrameworkUI;

namespace SMT.FB.UI.Common
{
    public class FBBaseControl : UserControl
    {
        public FBBaseControl()
        {
            
            this.Loaded += new RoutedEventHandler(FBBaseForm_Loaded);
        }

        void FBBaseForm_Loaded(object sender, RoutedEventArgs e)
        {
            InitProcess();
            if (FBBasePageLoaded != null)
            {
                FBBasePageLoaded(this, null);
            }
        }

        public event EventHandler FBBasePageLoaded;
        bool IsLock = true;
        SMTLoading loadbar = null;
        private void InitProcess()
        {
            Panel parent = this.Content as Panel;
            if (parent != null)
            {
                Grid g = new Grid();

                this.Content = g;
                g.Children.Add(parent);
                loadbar = new SMTLoading(); //全局变量
                g.Children.Add(loadbar);
            }
        }
        public int showProcessFlag=0;//防止开启进度条逻辑里面有关闭进度条的方法导致此进度条也被关闭。
        public void ShowProcess(bool isLock = false)
        {
            showProcessFlag++;
            if (loadbar != null)
            {
                loadbar.Start();//调用服务时写
            }
            this.IsLock = isLock;
        }
        public void CloseProcess(bool keepLock  = true)
        {
            showProcessFlag--;
            if(showProcessFlag>0)return;
            if ((loadbar != null) && !( IsLock && keepLock))
            {
                loadbar.Stop();
            }

        }
    }
      
}
