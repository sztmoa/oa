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
using System.Windows.Navigation;
using System.Windows.Data;
using SMT.Saas.Tools.SalaryWS;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.FrameworkUI.ChildWidow;

using SMT.SaaS.Globalization;
using SMT.Saas.Tools.OrganizationWS;
using OrganizationWS = SMT.Saas.Tools.OrganizationWS;
using SMT.SAAS.Main.CurrentContext;
using SMT.HRM.UI.Form.Salary;

namespace SMT.HRM.UI.Views.Salary
{
    public partial class EmployeeBalancePost :BasePage,IClient
    {
        
        private SalaryServiceClient client = new SalaryServiceClient();        
        private string Checkstate { get; set; }
        SMTLoading loadbar = new SMTLoading();
        public EmployeeBalancePost()
        {
            InitializeComponent();            
            this.Loaded += new RoutedEventHandler(EmployeeBalancePost_Loaded);
        }

        void EmployeeBalancePost_Loaded(object sender, RoutedEventArgs e)
        {
            InitParas();
            GetEntityLogo("T_HR_EMPLOYEESALARYPOSTASIGN");
            //审核状态绑定
            Utility.CbxItemBinder(ToolBar.cbxCheckState, "CHECKSTATE", Convert.ToInt32(CheckStates.All).ToString());
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Utility.DisplayGridToolBarButton(ToolBar, "T_HR_EMPLOYEESALARYPOSTASIGN", true);
            ViewTitles.TextTitle.Text = GetTitleFromURL(e.Uri.ToString());
        }

        private void InitParas()
        {
            PARENT.Children.Add(loadbar);
            loadbar.Stop();            
            client.EmployeeBalancePostDeleteCompleted += new EventHandler<EmployeeBalancePostDeleteCompletedEventArgs>(client_EmployeeBalancePostDeleteCompleted);
            client.GetEmployeeBalancePostWithPagingCompleted += new EventHandler<GetEmployeeBalancePostWithPagingCompletedEventArgs>(client_GetEmployeeBalancePostWithPagingCompleted);
            ToolBar.btnNew.Click += new RoutedEventHandler(btnNew_Click);
            ToolBar.btnEdit.Click += new RoutedEventHandler(btnEdit_Click);
            ToolBar.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            ToolBar.btnAudit.Click += new RoutedEventHandler(btnAudit_Click);
            
            ToolBar.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
            ToolBar.BtnView.Click += new RoutedEventHandler(BtnView_Click);
            
            this.Loaded += new RoutedEventHandler(Left_Loaded);
            ToolBar.cbxCheckState.SelectionChanged += new SelectionChangedEventHandler(cbxCheckState_SelectionChanged);
            ToolBar.btnReSubmit.Click += new RoutedEventHandler(btnReSubmit_Click);            
            treeOrganization.SelectedClick += new EventHandler(treeOrganization_SelectedClick);
            
        }

        void client_GetEmployeeBalancePostWithPagingCompleted(object sender, GetEmployeeBalancePostWithPagingCompletedEventArgs e)
        {
            List<T_HR_EMPLOYEESALARYPOSTASIGN> list = new List<T_HR_EMPLOYEESALARYPOSTASIGN>();
            if (e.Error != null)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                if (e.Result != null)
                {
                    list = e.Result.ToList();                    
                }
                DtGrid.ItemsSource = list;
                dataPager.PageCount = e.pageCount;
            }
            loadbar.Stop();
        }

        void client_EmployeeBalancePostDeleteCompleted(object sender, EmployeeBalancePostDeleteCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), "删除成功", Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);               
                LoadData();
            }
        }
        

        void treeOrganization_SelectedClick(object sender, EventArgs e)
        {
            LoadData();
        }
        
        void btnReSubmit_Click(object sender, RoutedEventArgs e)
        {
            ///TODO: 重新提交审核
            if (DtGrid.SelectedItems.Count > 0)
            {
                
                T_HR_EMPLOYEESALARYPOSTASIGN tmpEnt = DtGrid.SelectedItems[0] as T_HR_EMPLOYEESALARYPOSTASIGN;
                if (tmpEnt.CHECKSTATE != "3")
                {
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "只有审核未通过的才可以重新提交", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                    return;
                }
                else
                {
                    Form.Salary.EmployeeBalancePostAddForm form = new Form.Salary.EmployeeBalancePostAddForm(FormTypes.Resubmit, tmpEnt.EMPLOYEESALARYPOSTASIGNID);
                    EntityBrowser browser = new EntityBrowser(form);
                    form.MinWidth = 600;
                    form.MinHeight = 240;
                    browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                    browser.FormType = FormTypes.Resubmit;
                    browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
                }
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "AUDIT"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "AUDIT"));
                return;
            }
        }

        void Left_Loaded(object sender, RoutedEventArgs e)
        {
            // BindTree();
        }

        void BtnView_Click(object sender, RoutedEventArgs e)
        {
            if (DtGrid.SelectedItems.Count > 0)
            {
                T_HR_EMPLOYEESALARYPOSTASIGN tmpEnt = DtGrid.SelectedItems[0] as T_HR_EMPLOYEESALARYPOSTASIGN;

                Form.Salary.EmployeeBalancePostDetailForm form = new SMT.HRM.UI.Form.Salary.EmployeeBalancePostDetailForm(FormTypes.Browse, tmpEnt.EMPLOYEESALARYPOSTASIGNID);
                form.IsEnabled = false;
                EntityBrowser browser = new EntityBrowser(form);
                browser.FormType = FormTypes.Browse;
                form.MinWidth = 600;
                form.MinHeight = 240;
                browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "EDIT"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "EDIT"));
                return;
            }
        }

        void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        void cbxCheckState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            T_SYS_DICTIONARY dict = ToolBar.cbxCheckState.SelectedItem as T_SYS_DICTIONARY;
            if (dict != null)
            {
                Checkstate = dict.DICTIONARYVALUE.ToString();
                //ToolBar.btnOtherAction("/SMT.SaaS.FrameworkUI;Component/Images/Tool/ico_16_1055.png", Utility.GetResourceStr("MASSAUDIT")).Visibility = Visibility.Visible;
                //if (Checkstate == Convert.ToString(Convert.ToInt32(CheckStates.Approving)))//审核中则隐藏
                //{
                //    ToolBar.btnOtherAction("/SMT.SaaS.FrameworkUI;Component/Images/Tool/ico_16_1055.png", Utility.GetResourceStr("MASSAUDIT")).Visibility = Visibility.Collapsed;
                //}
                Utility.SetToolBarButtonByCheckState(dict.DICTIONARYVALUE.Value.ToInt32(), ToolBar, "T_HR_EMPLOYEESALARYPOSTASIGN");
                LoadData();
            }
        }
        void AuditState_Loaded(object sender, RoutedEventArgs e)
        {
            //审核状态绑定
            Utility.CbxItemBinder(ToolBar.cbxCheckState, "CHECKSTATE", Convert.ToInt32(CheckStates.All).ToString());
        }
        


        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }
        #region 添加,修改,删除,查询,审核


        void btnNew_Click(object sender, RoutedEventArgs e)
        {

            Form.Salary.EmployeeBalancePostAddForm form = new SMT.HRM.UI.Form.Salary.EmployeeBalancePostAddForm(FormTypes.New, "");
            EntityBrowser browser = new EntityBrowser(form);
            form.MinWidth = 600;
            form.MinHeight = 240;
            browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
            browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });

        }

        void browser_ReloadDataEvent()
        {
            LoadData();
        }


        private void LoadData()
        {
            loadbar.Start();

            int pageCount = 0;
            string filter = "";
            int sType = 0;
            string sValue = "";
            string strState = "";
            
            System.Collections.ObjectModel.ObservableCollection<string> paras = new System.Collections.ObjectModel.ObservableCollection<string>();

            TextBox txtName = Utility.FindChildControl<TextBox>(expander, "txtName");
            
            string selectedType = treeOrganization.sType;
            if (!string.IsNullOrEmpty(selectedType))
            {
                switch (selectedType)
                {
                    case "Company":
                        sType = 0;
                        break;
                    case "Department":
                        sType = 1;
                        break;
                    case "Post":
                        sType = 2;
                        break;
                }
                sValue = treeOrganization.sValue;
            }
            else
            {
                //loadbar.Stop();
                //return;
                sType = 0;
                sValue = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            }

            //if (txtName != null && !string.IsNullOrEmpty(txtName.Text.Trim()))
            //{
            //    // filter += " @" + paras.Count().ToString() + ".Contains(SALARYSOLUTIONNAME)";
            //    //filter += " EMPLOYEENAME==@" + paras.Count().ToString();
            //    //filter += " @" + paras.Count().ToString() + ".Contains(EMPLOYEENAME)";
            //    filter += " EMPLOYEENAME.Contains(@" + paras.Count().ToString() + ")";
            //    paras.Add(txtName.Text.Trim());
            //}
            string EmployeeName = string.Empty;
            EmployeeName = txtName.Text.Trim();

            if (Checkstate != Convert.ToInt32(CheckStates.All).ToString())
            {
                strState = Checkstate;
            }
            //client.GetEmployeeAddSumWithPagingAsync(dataPager.PageIndex, dataPager.PageSize, "ADDSUMID", filter, paras, pageCount, Convert.ToDateTime(starttimes), Convert.ToDateTime(endtimes), SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, strState, sType, sValue);
            client.GetEmployeeBalancePostWithPagingAsync(dataPager.PageIndex, dataPager.PageSize, "CREATEDATE", filter, paras, pageCount, EmployeeName,"", SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, strState, sType, sValue);
        }
        void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DtGrid.SelectedItems.Count > 0)
            {
                T_HR_EMPLOYEESALARYPOSTASIGN tmpEnt = DtGrid.SelectedItems[0] as T_HR_EMPLOYEESALARYPOSTASIGN;

                if (tmpEnt.CHECKSTATE == "0" || tmpEnt.CHECKSTATE == "3")
                {

                    //SMT.HRM.UI.Utility.CreateFormFromEngine(tmpEnt.EMPLOYEESALARYPOSTASIGNID, "SMT.HRM.UI.Form.Salary.EmployeeBalancePostAddForm", "Audit");
                    Form.Salary.EmployeeBalancePostAddForm form = new SMT.HRM.UI.Form.Salary.EmployeeBalancePostAddForm(FormTypes.Edit, tmpEnt.EMPLOYEESALARYPOSTASIGNID);

                    EntityBrowser browser = new EntityBrowser(form);
                    form.MinWidth = 600;
                    form.MinHeight = 240;
                    browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                    browser.FormType = FormTypes.Edit;
                    browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
                }
                else
                {
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("NOTEDIT"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                }
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "EDIT"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "EDIT"));
                return;
            }

        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {

            string Result = "";
            ObservableCollection<string> ids = new ObservableCollection<string>();
            if (DtGrid.SelectedItems.Count > 0)
            {
                for (int i = 0; i < DtGrid.SelectedItems.Count; i++)
                {
                    T_HR_EMPLOYEESALARYPOSTASIGN tmpEnt = DtGrid.SelectedItems[i] as T_HR_EMPLOYEESALARYPOSTASIGN;
                    ids.Add(tmpEnt.EMPLOYEESALARYPOSTASIGNID);
                    if (!(tmpEnt.CHECKSTATE == Convert.ToInt32(CheckStates.UnSubmit).ToString() || tmpEnt.CHECKSTATE == Convert.ToInt32(CheckStates.UnApproved).ToString()))
                    {
                        ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("NOTDELETE"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                        
                        return;
                    }
                }
                ComfirmWindow com = new ComfirmWindow();
                
                com.OnSelectionBoxClosed += (obj, result) =>
                {
                    //client.EmployeeAddSumDeleteAsync(ids);
                    client.EmployeeBalancePostDeleteAsync(ids);
                };
                com.SelectionBox(Utility.GetResourceStr("DELETECONFIRM"), Utility.GetResourceStr("DELETEALTER"), ComfirmWindow.titlename, Result);
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "DELETE"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "DELETE"));
                return;
            }
        }


        void btnAudit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ///TODO:ADD 审核   
                if (DtGrid.SelectedItems.Count > 0)
                {
                    T_HR_EMPLOYEESALARYPOSTASIGN tmpEnt = DtGrid.SelectedItems[0] as T_HR_EMPLOYEESALARYPOSTASIGN;
                    Form.Salary.EmployeeBalancePostDetailForm form = new Form.Salary.EmployeeBalancePostDetailForm(FormTypes.Audit, tmpEnt.EMPLOYEESALARYPOSTASIGNID);
                    EntityBrowser browser = new EntityBrowser(form);
                    form.MinWidth = 600;
                    form.MinHeight = 240;
                    browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                    browser.FormType = FormTypes.Audit;
                    browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
                }
                else
                {
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "请选择需要审核的数据", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                    return;
                }
            }
            catch (Exception ex)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.Message.ToString()));
            }

        }

        void btnSumbitAudit_click(object sender, RoutedEventArgs e)
        {
            ///TODO:ADD 提交审核            
        }
        void btnAuitNoTPass_click(object sender, RoutedEventArgs e)
        {
            ///TODO:ADD 审核不通过          
        }
        #endregion

        private void DtGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SetRowLogo(DtGrid, e.Row, "T_HR_EMPLOYEESALARYPOSTASIGN");
        }

        #region IClient
        public void ClosedWCFClient()
        {
            client.DoClose();
            //  orgClient.DoClose();
        }
        public bool CheckDataContenxChange()
        {
            return true;
        }
        public void SetOldEntity(object entity)
        {

        }
        #endregion

    }
}
