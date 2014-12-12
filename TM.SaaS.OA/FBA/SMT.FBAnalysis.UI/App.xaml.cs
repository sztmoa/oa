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
using System.Globalization;

using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.FrameworkUI.Common;
using SMT.SaaS.FrameworkUI;
//using SMT.FBAnalysis.UI.Form.TaskSchedule;
//using SMT.FBAnalysis.UI.Views.TaskSchedule;
//using SMT.FBAnalysis.UI.Views.SubjectManagement;
//using SMT.FBAnalysis.UI.Form.SubjectManagement;
namespace SMT.FBAnalysis.UI
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        #region 私有函数
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //读取系统初始参数，此参数为服务地址   
            //if (!App.Current.Resources.Contains("PlatformWShost"))
            //{
            //    string hostAddress = "";
            //    hostAddress = hostAddress + e.InitParams["p1"];
            //    hostAddress = hostAddress + e.InitParams["p2"].Replace('.', '/');
            //    App.Current.Resources.Add("PlatformWShost", hostAddress);
            //}

            EntryPointPage page = new EntryPointPage();
            page.Content = rootGrid;
            //TODO: 按需取出字典值
            //LoadDicts();
            InitTheme();
            InitComonConverter();
            rootGrid.Children.Add(new SMT.FBAnalysis.UI.Login());

            this.RootVisual = page;
            //this.RootVisual = new CompanyYearPlan();
            //this.RootVisual = new SMT.FBAnalysis.UI.Form.SubjectManagement.EditDepartmentYearPlan();
            //this.RootVisual = new AnnlualSessionView();
        }

        private void InitTheme()
        {
            ResourceDictionary newStyle3 = new ResourceDictionary();
            Uri uri = new Uri("/SMT.SAAS.Themes;component/ShinyBlue.xaml", UriKind.Relative);
            newStyle3.Source = uri;

            App.Current.Resources.MergedDictionaries.Add(newStyle3);
            ResourceDictionary newStyle_template = new ResourceDictionary();

            Uri uri_template = new Uri("/SMT.SAAS.Themes;component/ControlTemplate.xaml", UriKind.Relative);
            newStyle_template.Source = uri_template;

            App.Current.Resources.MergedDictionaries.Add(newStyle_template);
        }

        private void InitComonConverter()
        {
            //Application.Current.Resources.Add("CustomDictionaryConverter", new SMT.FBAnalysis.UI.CustomDictionaryConverter());
            Application.Current.Resources.Add("CustomDateConverter", new SMT.FBAnalysis.UI.CustomDateConverter());
            Application.Current.Resources.Add("CheckConverter", new SMT.FBAnalysis.UI.CheckConverter());
            //Application.Current.Resources.Add("ResourceConveter", new SMT.SaaS.Globalization.ResourceConveter());
            //Application.Current.Resources.Add("GridHeaderConverter", new SMT.SaaS.Globalization.GridHeaderConverter());
            Application.Current.Resources.Add("CompanyInfoConverter", new SMT.FBAnalysis.UI.CompanyInfoConverter());
        }

        private void LoadDicts()
        {
            try
            {
                PermissionServiceClient clientPerm = new PermissionServiceClient();
                clientPerm.GetSysDictionaryByCategoryCompleted += (o, e) =>
                {
                    if (e.Error != null && e.Error.Message != "")
                    {
                        Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
                    }
                    else
                    {
                        List<T_SYS_DICTIONARY> dicts = new List<T_SYS_DICTIONARY>();
                        dicts = e.Result == null ? null : e.Result.ToList();
                        Application.Current.Resources.Add("SYS_DICTIONARY", dicts);
                    }
                };
                //TODO: 按需取出字典值
                //clientPerm.GetSysDictionaryByCategoryAsync("");
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.Message));
            }
        }       

        private void Application_Exit(object sender, EventArgs e)
        {

        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // 如果应用程序是在调试器外运行的，则使用浏览器的
            // 异常机制报告该异常。在 IE 上，将在状态栏中用一个 
            // 黄色警报图标来显示该异常，而 Firefox 则会显示一个脚本错误。
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // 注意: 这使应用程序可以在已引发异常但尚未处理该异常的情况下
                // 继续运行。 
                // 对于生产应用程序，此错误处理应替换为向网站报告错误
                // 并停止应用程序。
                e.Handled = true;
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }

        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
            try
            {
                string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");");
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region 公开接口

        public Grid rootGrid = new Grid();

        /// <summary>
        /// 页面跳转
        /// </summary>
        /// <param name="newPage"></param>
        public static void Navigation(UserControl newPage)
        {
            //获取当前的Appliaction实例 
            App currentApp = (App)Application.Current;
            // 修改当前显示页面内容. 
            currentApp.rootGrid.Children.Clear();
            currentApp.rootGrid.Children.Add(newPage);
        }

        /// <summary>
        /// 系统主入口
        /// </summary>
        public class EntryPointPage : UserControl
        {
            public UIElement Content
            {
                get { return base.Content; }
                set { base.Content = value; }
            }
        }
        #endregion

        private void SelectCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox CBAll = sender as CheckBox;
            var grid = CommonTools.FindParentControl<DataGrid>(CBAll);
            if (grid != null)
            {
                GridHelper.HandleAllCheckBoxClick(grid, "checkbox", CBAll.IsChecked.GetValueOrDefault());
            }
        }
    }
}
