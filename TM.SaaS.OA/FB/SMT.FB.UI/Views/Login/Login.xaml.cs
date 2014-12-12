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
using SMT.SAAS.Main.CurrentContext;
using SMT.Saas.Tools.PermissionWS;
using SMT.Saas.Tools.PersonnelWS;
using SMT.Saas.Tools.OrganizationWS;
using SMT.SaaS.LM.UI.AppMain;
using SMT.SaaS.FrameworkUI.Common;
using SMT.SaaS.FrameworkUI;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.FB.UI;
using SMT.FB.UI.Common;
using System.IO.IsolatedStorage;

using CurrentContext = SMT.SAAS.Main.CurrentContext;
using SMT.SAAS.Main.CurrentContextLoadData;

namespace SMT.SaaS.LM.UI.AppMain.Login
{
    public partial class Login : ChildWindow
    {
        public bool isLogin = false;
        public bool isHR = false;
        public bool isDict = false;
        public bool isLogining = false;
        #region 初始化变量

        IsolatedStorageSettings settings = IsolatedStorageSettings.SiteSettings;
        //private PermissionServiceClient client = new PermissionServiceClient();
        //private OrganizationServiceClient organClient = new OrganizationServiceClient();
        //用户登录类
        UserLogin login;
        //用户信息
        T_SYS_USER UserInfo = new T_SYS_USER();

        public Login()
        {

            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Login_Loaded);
            //加载所有公司、部门、岗位信息
            // organClient.GetCompanyActivedAsync("");
            //登录控件禁用  在加载完组织架构后 再激活
            this.OK.IsEnabled = false;
            this.OK.Content = "加载中....";
            settings = IsolatedStorageSettings.ApplicationSettings;

            if (settings.Contains("SYS_PostInfo") && settings["SYS_PostInfo"] != null)
            {
                App.Current.Resources.Add("SYS_PostInfo", settings["SYS_PostInfo"]);
                App.Current.Resources.Add("SYS_CompanyInfo", settings["SYS_CompanyInfo"]);
                App.Current.Resources.Add("SYS_DepartmentInfo", settings["SYS_DepartmentInfo"]);
            }
            isDict = true;
            isHR = true;

            if (settings.Contains("UserName"))
            {
                UserName.Text = Convert.ToString(settings["UserName"]);
                paw.Password = Convert.ToString(settings["UserPWD"]);               
            }            
            RefreshBtn();
            
        }

        #endregion

        public void RefreshBtn()
        {
            this.OK.IsEnabled = isHR && isDict;
            if (isHR && isDict)
            {
                
                if (isLogin)
                {
                    MainPage mainPage = new MainPage();
                    Common.ParentLayoutRoot = mainPage.LayoutRoot;
                    App.Navigation(mainPage);
                }
                if (isLogining)
                {
                    this.OK.Content = "加载中...";
                    this.OK.IsEnabled = false;
                }
                else
                {
                    this.OK.Content = "确认";
                }
            }            
        }
        #region 窗体加载完成事件
        void Login_Loaded(object sender, RoutedEventArgs e)
        {
            UserName.Focus();
            App currentApp = (App)Application.Current;
            currentApp.rootGrid.Style = (Style)Application.Current.Resources["GridStyle4"];
        }
        #endregion

        #region 登录按钮
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            isLogining = true;
            RefreshBtn();
            SaveData();
            string StrUserName = "";//用户名
            string StrPwd = "";//密码
            string StrCode = ""; //验证码
            StrUserName = UserName.Text.ToString().Trim();
            StrPwd = paw.Password.ToString().Trim();
            //暂时不使用
            StrCode = YanZM.Text.ToString().Trim();


            if (string.IsNullOrEmpty(StrUserName))
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("USERNAMENOTNULL"));
                UserName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(StrPwd))
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("PASSWORDNOTNULL"));
                paw.Focus();
                return;

            }

            //StrPwd = Utility.AESEncrypt(StrPwd);

            login = new UserLogin(StrUserName, StrPwd);
            login.LoginedClick += (obj, ev) =>
            {
                isLogining = false;
                if (login.LoginResult)
                {
                    UserInfo = login.GetUserInfo();
               
                    if (App.Current.Resources["CurrentUserID"] == null)
                    {
                        App.Current.Resources.Add("CurrentUserID", UserInfo.EMPLOYEEID);
                    }
                    
                    var nameOld = settings.Contains("UserName") ? Convert.ToString(settings["UserName"]) : "";
                    settings["UserName"] = StrUserName;
                    settings["UserPWD"] = StrPwd;
                    settings.Save();
                    isLogin = true;
                    string strEmployeeID = string.Empty;
                    if (SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo != null)
                    {
                        strEmployeeID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                    }
                    if ( (!settings.Contains("SYS_PostInfo")) || (nameOld != StrUserName))
                    {
                        hyperlinkButton1_Click(null, null);                      
                        isLogining = true;
                    }
                    login.OnGetOrgCompleted += (obje, eve) =>
                        {
                        };
                    login.LoadAllOrgInfo();
                }
                else
                {
                    MessageBox.Show("用户或密码错误!");
                }
                
                RefreshBtn();
            };
            
        }
        #endregion

        private void SaveHRData()
        {
            settings["SYS_DepartmentInfo"] = App.Current.Resources["SYS_DepartmentInfo"];
            settings["SYS_PostInfo"] = App.Current.Resources["SYS_PostInfo"];
            settings["SYS_CompanyInfo"] = App.Current.Resources["SYS_CompanyInfo"];
            settings.Save();
        }
        private void SaveData()
        {
            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    // Request 5MB more space in bytes.
                    Int64 spaceToAdd = 15242880;
                    Int64 curAvail = store.AvailableFreeSpace;

                    // If available space is less than
                    // what is requested, try to increase.
                    if (curAvail < spaceToAdd)
                    {

                        // Request more quota space.
                        if (!store.IncreaseQuotaTo(store.Quota + spaceToAdd))
                        {
                            // The user clicked NO to the
                            // host's prompt to approve the quota increase.
                            return;
                        }
                        else
                        {

                        }
                    }
                }
            }
            catch (IsolatedStorageException)
            {
                // TODO: Handle that store could not be accessed.

            }


        }
        private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
        {
            if (settings.Contains("SYS_DepartmentInfo"))
            {
                settings.Remove("SYS_DepartmentInfo");
            }
            if (settings.Contains("SYS_PostInfo"))
            {
                settings.Remove("SYS_PostInfo");
            }
            if (settings.Contains("SYS_CompanyInfo"))
            {
                settings.Remove("SYS_CompanyInfo");
            }

            if (App.Current.Resources.Contains("SYS_DepartmentInfo"))
            {
                App.Current.Resources.Remove("SYS_DepartmentInfo");
            }

            if (App.Current.Resources.Contains("SYS_PostInfo"))
            {
                App.Current.Resources.Remove("SYS_PostInfo");
            }
            if (App.Current.Resources.Contains("SYS_CompanyInfo"))
            {
                App.Current.Resources.Remove("SYS_CompanyInfo");
            }
        }

        protected void LoadDicts()
        {
            try
            {
                PermissionServiceClient cliet = new PermissionServiceClient();

                cliet.GetSysDictionaryByCategoryCompleted += (o, e) =>
                {
                    if (e.Error != null && e.Error.Message != "")
                    {
                        CommonFunction.ShowErrorMessage(Utility.GetResourceStr(e.Error.Message));
                    }
                    else
                    {
                        List<T_SYS_DICTIONARY> dicts = new List<T_SYS_DICTIONARY>();
                        dicts = e.Result == null ? null : e.Result.ToList();
                        if (Application.Current.Resources.Contains("SYS_DICTIONARY"))
                        {
                            Application.Current.Resources.Remove("SYS_DICTIONARY");
                        }
                        Application.Current.Resources.Add("SYS_DICTIONARY", dicts);
                        settings.Add("SYS_DICTIONARY", dicts);
                        isDict = true;
                        RefreshBtn();
                       
                    }
                };
                //TODO: 按需取出字典值
                cliet.GetSysDictionaryByCategoryAsync("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(Utility.GetResourceStr(ex.Message));
            }
        }

        #region ""
        #endregion
    }
}

