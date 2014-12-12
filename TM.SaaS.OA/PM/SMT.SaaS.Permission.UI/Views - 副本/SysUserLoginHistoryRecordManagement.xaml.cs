using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using SMT.Saas.Tools.PermissionWS;
using System.Windows.Data;
using SMT.SaaS.FrameworkUI;
using SMT.SAAS.Main.CurrentContext;
using System.Collections.ObjectModel;

namespace SMT.SaaS.Permission.UI.Views
{
    public partial class SysUserLoginHistoryRecordManagement : BasePage
    {
        #region 初始化数据
        private static PermissionServiceClient SysLoginHistoryClient = new PermissionServiceClient();//龙康才新增
        //private PermissionServiceClient SysLoginHistoryClient = new PermissionServiceClient();
        ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();   //参数值
        private bool QueryBtn = false;
        private SMTLoading loadbar = new SMTLoading(); 
        private void InitControlEvent()
        {
            this.Loaded += new RoutedEventHandler(SysUserLoginHistoryRecordManagement_Loaded);            
            SysLoginHistoryClient.GetSysUserLoginHistoryRecordAllInfosPagingCompleted += new EventHandler<GetSysUserLoginHistoryRecordAllInfosPagingCompletedEventArgs>(SysLoginHistoryClient_GetSysUserLoginHistoryRecordAllInfosPagingCompleted);
            GetEntityLogo("USERHISTORYLOGIN");
            SysLoginHistoryClient.GetCustomerPermissionByUserIDAndEntityCodeAsync("DAD32DB2-B07A-49b1-9710-61158D81B863", "OABUMFSENDDOC");
        }

        void SysLoginHistoryClient_GetSysUserLoginHistoryRecordAllInfosPagingCompleted(object sender, GetSysUserLoginHistoryRecordAllInfosPagingCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    try
                    {
                        if (e.Result != null)
                        {

                            BindDataGrid(e.Result.ToList(), e.pageCount);
                        }
                        else
                        {
                            BindDataGrid(null, 0);
                        }
                        loadbar.Stop();
                    }
                    catch (Exception ex)
                    {

                        Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.ToString()));
                    }
                }
                else
                {
                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), e.Error.ToString());
                }


            }
            QueryBtn = false; //查询完成
        }

        //  绑定DataGird
        private void BindDataGrid(List<V_UserLoginRecordHistory> obj, int pageCount)
        {
            GridHelper.HandleDataPageDisplay(dataPager, pageCount);
            if (obj == null || obj.Count < 1)
            {
                //HtmlPage.Window.Alert("对不起！未能找到相关记录。");
                DtGrid.ItemsSource = null;
                return;
            }
            DtGrid.ItemsSource = obj;
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            FormTitleName.TextTitle.Text = GetTitleFromURL(e.Uri.ToString());
        }
        
    
        void SysUserLoginHistoryRecordManagement_Loaded(object sender, RoutedEventArgs e)
        {
            //Utility.DisplayGridToolBarButton(FormToolBar1, "USERHISTORYLOGIN", true);
            FormToolBar1.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
            //隐藏未使用按钮
            //FormToolBar1.btnRead.Visibility = Visibility.Collapsed;
            FormToolBar1.btnNew.Visibility = Visibility.Collapsed;
            FormToolBar1.BtnView.Visibility = Visibility.Collapsed;
            FormToolBar1.btnPrint.Visibility = Visibility.Collapsed;
            FormToolBar1.btnOutPDF.Visibility = Visibility.Collapsed;
            FormToolBar1.btnOutExcel.Visibility = Visibility.Collapsed;
            //FormToolBar1.btnimport.Visibility = Visibility.Collapsed;
            FormToolBar1.stpCheckState.Visibility = Visibility.Collapsed;
            FormToolBar1.stpOtherAction.Visibility = Visibility.Collapsed;

            //FormToolBar1.btnSumbitAudit.Visibility = Visibility.Collapsed;
            FormToolBar1.btnAudit.Visibility = Visibility.Collapsed;

            FormToolBar1.btnEdit.Visibility = Visibility.Collapsed;
            FormToolBar1.btnDelete.Visibility = Visibility.Collapsed;


            FormToolBar1.ShowRect();
        }
        void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string filter = "";    //查询过滤条件
            int pageCount = 0;
            if (QueryBtn)
            {
                string StrState = ""; //状态

                string StrStart = "";//添加文档的起始时间
                string StrEnd = "";//添加文档的结束时间

                ComboBox dpState = Utility.FindChildControl<ComboBox>(expander, "dpState");
                switch (dpState.SelectedIndex)
                {
                    case 0:
                        StrState = "";
                        break;
                    case 1:
                        StrState = "1";
                        break;
                    case 2:
                        StrState = "0";
                        break;
                }

                DatePicker DtpStart = Utility.FindChildControl<DatePicker>(expander, "DtStart");
                DatePicker DtpEnd = Utility.FindChildControl<DatePicker>(expander, "DtEnd");

                StrStart = DtpStart.Text.Trim().ToString();
                StrEnd = DtpEnd.Text.Trim().ToString();

                if (StrState == "" && StrStart == "" && StrEnd == "")
                {
                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("QUERYINFONOTNULL"));
                    return;
                }
                DateTime DtStart = new DateTime();
                DateTime DtEnd = new DateTime();
                if (!string.IsNullOrEmpty(StrStart) && string.IsNullOrEmpty(StrEnd))
                {
                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STARTTIMENOTNULL"));
                    return;
                }
                if (string.IsNullOrEmpty(StrStart) && !string.IsNullOrEmpty(StrEnd))
                {
                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ENDTIMENOTNULL"));
                    return;
                }
                if (!string.IsNullOrEmpty(StrStart) && !string.IsNullOrEmpty(StrEnd))
                {
                    DtStart = System.Convert.ToDateTime(StrStart);
                    DtEnd = System.Convert.ToDateTime(StrEnd);
                    if (DtStart > DtEnd)
                    {

                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("SEARCH"), Utility.GetResourceStr("STARTTIMENOTGREATENDTIME")); Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("SEARCH"), Utility.GetResourceStr("STARTTIMENOTGREATENDTIME"));
                        return;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(filter))
                        {
                            filter += " and ";
                        }
                        filter += "userloginrecord.LOGINDATE >@" + paras.Count().ToString();
                        paras.Add(DtStart);
                        filter += " and ";
                        filter += "userloginrecord.LOGINDATE <@" + paras.Count().ToString();
                        paras.Add(DtEnd);
                    }
                }
            }

            loadbar.Start();
            SMT.Saas.Tools.PermissionWS.LoginUserInfo loginUserInfo = new SMT.Saas.Tools.PermissionWS.LoginUserInfo();
            loginUserInfo.companyID = Common.LoginUserInfo.UserPosts[0].CompanyID;
            loginUserInfo.userID = Common.LoginUserInfo.EmployeeID;
            SysLoginHistoryClient.GetSysUserLoginHistoryRecordAllInfosPagingAsync(dataPager.PageIndex, dataPager.PageSize, "historyrecord.LOGINDATE", filter, paras, pageCount, loginUserInfo);
            
        }
        public SysUserLoginHistoryRecordManagement()
        {
            InitializeComponent();
            PARENT.Children.Add(loadbar);
            InitControlEvent();
            LoadData();
        }
        #endregion   
     
        #region 查询按钮事件

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            QueryBtn = true;
            LoadData();
        }
        #endregion

        private void DtGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SetRowLogo(DtGrid, e.Row, "USERHISTORYLOGIN");
        }

        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}
