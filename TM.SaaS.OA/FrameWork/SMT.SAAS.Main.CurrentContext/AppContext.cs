
// 内容摘要: XAML中的共享数据

using System;
using System.Net;
namespace SMT.SAAS.Main.CurrentContext
{
    /// <summary>
    /// XAML数据上下文。
    /// </summary>
    public static class AppContext
    {
        //当前UI SHELL
        public static Host AppHost;

        //登录状态
        public static bool LogOff = false;

        public static bool IsMenuOpen = false;

        public static bool IsLoadingCompleted = false;

        

        /// <summary>
        /// 标识权限是否需要升级
        /// </summary>
        public static bool IsPermUpdate = false;

        public static void SystemMessage(string message)
        {
            try
            {
                string strTime = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":"
                    + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();
                if (AppHost == null) return;
                AppHost.txtSystemMessage.Text += System.Environment.NewLine + message + " 记录时间：" + strTime;

                //LogToServer.logToServer(message);
            }
            catch (Exception)
            {

            }finally
            {
            }
        }


        /// <summary>
        /// 显示系统错误信息框
        /// </summary>
        public static void ShowSystemMessageText()
        {
            try
            {
                if (AppHost == null) return;
                AppHost.txtSystemMessage.Height = 20;
                AppHost.systemMessageArea.Height = 25;
                AppHost.systemMessageArea.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception)
            {

            }
        }

        public static void logAndShow(string msg)
        {
            SystemMessage(msg);
            ShowSystemMessageText();
        }

       
    }

    public class LogToServer
    {
        private static EmpInfoWS.EmployeeInfoServiceClient empClient;

        public static EmpInfoWS.EmployeeInfoServiceClient EmpClient
        {
            get
            {
                if (empClient == null)
                {
                    empClient = new EmpInfoWS.EmployeeInfoServiceClient();
                }
                return empClient;
            }
            set { empClient = value; }
        }

        public static void logToServer(string msg)
        {
            //Uri serviceUri = new Uri(SMT.SAAS.Main.CurrentContext.Common.HostAddress + "/HR/EmployeeInfoService.svc");
            //WebClient webClient = new WebClient();
            ////webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            ////webClient.OpenReadAsync(serviceUri);
            //webClient.OpenWriteAsync(serviceUri, "POST", msg);
            string empInfo = string.Empty;
            if(Common.CurrentLoginUserInfo!=null)
            {
                if (Common.CurrentLoginUserInfo.UserPosts.Count>0)
                { 
                empInfo = Common.CurrentLoginUserInfo.UserPosts[0].CompanyName
                   + "-" + Common.CurrentLoginUserInfo.UserPosts[0].DepartmentName
                      + "-" + Common.CurrentLoginUserInfo.UserPosts[0].PostName
                + "-" + Common.CurrentLoginUserInfo.EmployeeName;
                }
            }
            if(string.IsNullOrEmpty(empInfo))
            { 
                EmpClient.RecordUILogAsync(empInfo, msg);
            }
        }
    }
}
