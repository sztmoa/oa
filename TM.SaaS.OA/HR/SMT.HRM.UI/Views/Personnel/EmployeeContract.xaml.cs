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


using SMT.SaaS.FrameworkUI;
using SMT.Saas.Tools.PersonnelWS;
using SMT.HRM.UI.Form.Personnel;
using SMT.SaaS.FrameworkUI.ChildWidow;
using System.Collections.ObjectModel;
using SMT.Saas.Tools.PermissionWS;

namespace SMT.HRM.UI.Views.Personnel
{
    public partial class EmployeeContract : BasePage, IClient
    {
        PersonnelServiceClient client;
        public string Checkstate { get; set; }
        SMTLoading loadbar = new SMTLoading();
        public EmployeeContract()
        {
            InitializeComponent();
            InitEvent();
            //GetEntityLogo("T_HR_EMPLOYEECONTRACT");
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Utility.DisplayGridToolBarButton(ToolBar, "T_HR_EMPLOYEECONTRACT", true);
            ViewTitles.TextTitle.Text = GetTitleFromURL(e.Uri.ToString());
        }

        private void InitEvent()
        {
            PARENT.Children.Add(loadbar);
            
            ToolBar.btnNew.Click += new RoutedEventHandler(btnNew_Click);
            ToolBar.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
            ToolBar.btnEdit.Click += new RoutedEventHandler(btnEdit_Click);
            ToolBar.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            ToolBar.cbxCheckState.SelectionChanged += new SelectionChangedEventHandler(cbxCheckState_SelectionChanged);
            ToolBar.btnAudit.Click += new RoutedEventHandler(btnAudit_Click);
            ToolBar.BtnView.Click += new RoutedEventHandler(BtnView_Click);
            ToolBar.btnReSubmit.Visibility = Visibility.Collapsed;

            

            client = new PersonnelServiceClient();
            client.EmployeeContractViewPagingCompleted += new EventHandler<EmployeeContractViewPagingCompletedEventArgs>(client_EmployeeContractViewPagingCompleted);
            client.EmployeeContractPagingCompleted += new EventHandler<EmployeeContractPagingCompletedEventArgs>(client_EmployeeContractPagingCompleted);

            //  client.EmployeeDeleteCompleted += new EventHandler<EmployeeDeleteCompletedEventArgs>(client_EmployeeDeleteCompleted);
            client.EmployeeContractDeleteCompleted += new EventHandler<EmployeeContractDeleteCompletedEventArgs>(client_EmployeeContractDeleteCompleted);
            this.Loaded += new RoutedEventHandler(EmployeeContract_Loaded);

            PermissionServiceClient pClient = new PermissionServiceClient();
            pClient.GetUserMenuPermsByUserPermissionCompleted += new EventHandler<GetUserMenuPermsByUserPermissionCompletedEventArgs>(pClient_GetUserMenuPermsByUserPermissionCompleted);
            pClient.GetUserMenuPermsByUserPermissionAsync("T_HR_EMPLOYEECONTRACT", SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.SysUserID);
        }


        void client_EmployeeContractViewPagingCompleted(object sender, EmployeeContractViewPagingCompletedEventArgs e)
        {
            loadbar.Stop();
            List<V_EMPLOYEECONTACT> list = new List<V_EMPLOYEECONTACT>();
            if (e.Error != null && e.Error.Message != "")
            {
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
               Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);

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
            ToolBar.btnRefresh.IsEnabled = true;
        }

        void pClient_GetUserMenuPermsByUserPermissionCompleted(object sender, GetUserMenuPermsByUserPermissionCompletedEventArgs e)
        {
            List<V_UserPermission> pers = e.Result.ToList();
            if (pers.Any())
            {
                var list = pers.Where(t => t.PermissionDataRange == "1" && t.RoleMenuPermissionValue == "1").ToList();
                if (list.Count() > 0)
                {
                    Button ButtonMainpostChange = new Button();
                    ButtonMainpostChange.VerticalAlignment = VerticalAlignment.Center;
                    ButtonMainpostChange.Content = "提醒日期设置";
                    ButtonMainpostChange.Click += new RoutedEventHandler(ButtonMainpostChange_Click);
                    ToolBar.stpOtherAction.Children.Add(ButtonMainpostChange);
                }
            }
        }

        

        void ButtonMainpostChange_Click(object sender, RoutedEventArgs e)
        {
            ExpireRemindSetForm form = new ExpireRemindSetForm();
            EntityBrowser browser = new EntityBrowser(form);
            //  form.MinHeight = 390;
            browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
            browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
        }




        void cbxCheckState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            T_SYS_DICTIONARY dict = ToolBar.cbxCheckState.SelectedItem as T_SYS_DICTIONARY;
            if (dict != null)
            {
                Checkstate = dict.DICTIONARYVALUE.ToString();
                Utility.SetToolBarButtonByCheckState(dict.DICTIONARYVALUE.Value.ToInt32(), ToolBar, "T_HR_EMPLOYEECONTRACT");
                ToolBar.btnReSubmit.Visibility = Visibility.Collapsed;
                LoadData();
            }
        }

        void client_EmployeeContractPagingCompleted(object sender, EmployeeContractPagingCompletedEventArgs e)
        {
            loadbar.Stop();
            List<T_HR_EMPLOYEECONTRACT> list = new List<T_HR_EMPLOYEECONTRACT>();
            if (e.Error != null && e.Error.Message != "")
            {
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
               Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);

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
            ToolBar.btnRefresh.IsEnabled = true;
        }

        void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            ToolBar.btnRefresh.IsEnabled = false;
            LoadData();
        }

        void EmployeeContract_Loaded(object sender, RoutedEventArgs e)
        {
            GetEntityLogo("T_HR_EMPLOYEECONTRACT");

            Utility.CbxItemBinder(ToolBar.cbxCheckState, "CHECKSTATE", Convert.ToInt32(CheckStates.All).ToString());
        }

        private void LoadData()
        {
            loadbar.Start();
            int pageCount = 0;
            string filter = "";
            string strState = "";
            if (Checkstate != Convert.ToInt32(CheckStates.All).ToString())
            {
                strState = Checkstate;
            }
            System.Collections.ObjectModel.ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();

            //if (Checkstate != Convert.ToInt32(CheckStates.All).ToString())
            //{
            //    filter += "CHECKSTATE==@" + paras.Count().ToString();
            //    paras.Add(Convert.ToInt32(Checkstate).ToString());
            //}

            TextBox txtEmpName = Utility.FindChildControl<TextBox>(expander, "txtEmpName");
            if (txtEmpName != null)
            {
                if (!string.IsNullOrEmpty(txtEmpName.Text))
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += " and ";
                    }
                    //filter += "T_HR_EMPLOYEE.EMPLOYEENAME==@" + paras.Count().ToString();
                    filter += " @" + paras.Count().ToString() + ".Contains(T_HR_EMPLOYEE.EMPLOYEECNAME)";
                    paras.Add(txtEmpName.Text.Trim());

                }
            }
            

            //if (Checkstate != Convert.ToInt32(CheckStates.All).ToString())
            //    {
            //        if (!string.IsNullOrEmpty(filter))
            //        {
            //            filter += " and ";
            //        }
            //        filter += "CHECKSTATE==@" + paras.Count().ToString();
            //        paras.Add(Checkstate);
            //    }
            string employeeState = "0";
            ComboBox cbxstate = Utility.FindChildControl<ComboBox>(expander, "cbxEmployeeState");
            Employeestate statetmp = cbxstate.SelectedItem as Employeestate;
            if (statetmp != null)
            {
                employeeState = statetmp.Value;
            }
            DatePicker dpEndToDate = Utility.FindChildControl<DatePicker>(expander, "dpEndToDate");
            DatePicker dpStartToDate = Utility.FindChildControl<DatePicker>(expander, "dpStartToDate");

            if (!string.IsNullOrEmpty(dpEndToDate.Text) && string.IsNullOrEmpty(dpStartToDate.Text))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), "请选择合同到期开始时间",
                Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                loadbar.Stop();
                return;
            }
            if (string.IsNullOrEmpty(dpEndToDate.Text) && !string.IsNullOrEmpty(dpStartToDate.Text))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), "请选择合同到期结束时间",
                Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                loadbar.Stop();
                return;
            }
            if (!string.IsNullOrEmpty(dpEndToDate.Text) && !string.IsNullOrEmpty(dpStartToDate.Text))
            {
                if (DateTime.Parse(dpStartToDate.Text) > DateTime.Parse(dpEndToDate.Text))
                {
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), "合同到期开始时间不能大于结束时间",
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                    loadbar.Stop();
                    return;
                }
            }

            client.EmployeeContractViewPagingAsync(dataPager.PageIndex, dataPager.PageSize, "T_HR_EMPLOYEE.EMPLOYEECODE", filter, paras,
                pageCount, strState, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID,dpStartToDate.Text, dpEndToDate.Text, employeeState);
            //client.EmployeeContractPagingAsync(dataPager.PageIndex, dataPager.PageSize, "T_HR_EMPLOYEE.EMPLOYEECODE", filter, paras,
            //    pageCount, strState, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
        }

        #region

        void btnNew_Click(object sender, RoutedEventArgs e)
        {
            //Utility.OpenNewTask("SMT.HRM.UI.Form.Personnel.EmployeeContractForm,SMT.HRM.UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",null);

            EmployeeContractForm form = new EmployeeContractForm(FormTypes.New, "");
            EntityBrowser browser = new EntityBrowser(form);
            form.MinHeight = 450;
            browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
            browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
        }

        void browser_ReloadDataEvent()
        {
            LoadData();
        }
        void BtnView_Click(object sender, RoutedEventArgs e)
        {
            if (DtGrid.SelectedItems.Count > 0)
            {
                //T_HR_EMPLOYEECONTRACT tempEnt = DtGrid.SelectedItems[0] as T_HR_EMPLOYEECONTRACT;
                V_EMPLOYEECONTACT tempEnt = DtGrid.SelectedItems[0] as V_EMPLOYEECONTACT;
                EmployeeContractForm form = new EmployeeContractForm(FormTypes.Browse, tempEnt.EMPLOYEECONTACTIDk__BackingField);
                EntityBrowser browser = new EntityBrowser(form);
                browser.FormType = FormTypes.Browse;
                form.MinHeight = 450;
                browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
            }
            else
            {
                //ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTDATAALERT"), Utility.GetResourceStr("CONFIRMBUTTON"));
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "VIEW"),
            Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
            }
        }
        void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DtGrid.SelectedItems.Count > 0)
            {
                V_EMPLOYEECONTACT tempEnt = DtGrid.SelectedItems[0] as V_EMPLOYEECONTACT;
                //T_HR_EMPLOYEECONTRACT tempEnt = DtGrid.SelectedItems[0] as T_HR_EMPLOYEECONTRACT;
                //if (!SMT.SaaS.FrameworkUI.Common.Utility.ToolBarButtonOperationPermission(tempEnt, "T_HR_EMPLOYEECONTRACT", SMT.SaaS.FrameworkUI.OperationType.Edit, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID))
                //{
                //    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), Utility.GetResourceStr("NOEDITPERMISSION"),
                //  Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                //    return;
                //}
                EmployeeContractForm form = new EmployeeContractForm(FormTypes.Edit, tempEnt.EMPLOYEECONTACTIDk__BackingField);
                EntityBrowser browser = new EntityBrowser(form);
                browser.FormType = FormTypes.Edit;
                form.MinHeight = 450;
                browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
            }
            else
            {
                //ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTDATAALERT"), Utility.GetResourceStr("CONFIRMBUTTON"));
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "EDIT"),
             Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
            }
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string Result = "";
            if (DtGrid.SelectedItems.Count > 0)
            {
                ComfirmWindow com = new ComfirmWindow();
                com.OnSelectionBoxClosed += (obj, result) =>
                {
                    ObservableCollection<string> ids = new ObservableCollection<string>();

                    foreach (V_EMPLOYEECONTACT tmp in DtGrid.SelectedItems)
                    {
                        if (tmp.CHECKSTATEk__BackingField != Convert.ToInt32(CheckStates.UnSubmit).ToString())
                        {
                            ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), Utility.GetResourceStr("Msg_NoDeleteOrder"),
                 Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                            break;
                        }
                        //if (!SMT.SaaS.FrameworkUI.Common.Utility.ToolBarButtonOperationPermission(tmp, "T_HR_EMPLOYEECONTRACT", SMT.SaaS.FrameworkUI.OperationType.Delete, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID))
                        //{
                        //    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), Utility.GetResourceStr("NOPERMISSIONDELETE", tmp.T_HR_EMPLOYEE.EMPLOYEECNAME + ",CONTRACTINFORMATION"),
                        //  Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                        //    return;
                        //}
                        ids.Add(tmp.EMPLOYEECONTACTIDk__BackingField);
                    }
                    client.EmployeeContractDeleteAsync(ids);
                };
                com.SelectionBox(Utility.GetResourceStr("DELETECONFIRM"), Utility.GetResourceStr("DELETEALTER"), ComfirmWindow.titlename, Result);
            }
            else
            {
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "DELETE"));
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "DELETE"),
            Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);

            }
        }

        //void client_EmployeeDeleteCompleted(object sender, EmployeeDeleteCompletedEventArgs e)
        //{

        //}
        void client_EmployeeContractDeleteCompleted(object sender, EmployeeContractDeleteCompletedEventArgs e)
        {
            if (e.Error != null && e.Error.Message != "")
            {
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
               Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                //Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("DELETESUCCESSED", "EMPLOYEECONTRACT"));
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("DELETESUCCESS"),
                 Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                LoadData();
            }
        }

        void btnAudit_Click(object sender, RoutedEventArgs e)
        {
            if (DtGrid.SelectedItems.Count > 0)
            {
                //T_HR_EMPLOYEECONTRACT temp = DtGrid.SelectedItems[0] as T_HR_EMPLOYEECONTRACT;
                V_EMPLOYEECONTACT temp = DtGrid.SelectedItems[0] as V_EMPLOYEECONTACT;
                EmployeeContractForm form = new EmployeeContractForm(FormTypes.Audit, temp.EMPLOYEECONTACTIDk__BackingField);
                EntityBrowser browser = new EntityBrowser(form);
                browser.FormType = FormTypes.Audit;
                form.MinHeight = 450;
                browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
            }
            else
            {
                //ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTDATAALERT"), Utility.GetResourceStr("CONFIRMBUTTON"));
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "AUDIT"),
        Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
            }
        }
        #endregion

        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void DtGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SetRowLogo(DtGrid, e.Row, "T_HR_EMPLOYEECONTRACT");
        }
        #region IClient
        public void ClosedWCFClient()
        {
            // throw new NotImplementedException();
            client.DoClose();
        }

        public bool CheckDataContenxChange()
        {
            throw new NotImplementedException();
        }

        public void SetOldEntity(object entity)
        {
            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// 查找按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            dataPager.PageIndex = 1;//查找后默认跳到第一页
            
            LoadData();
        }

        private void cbxEmployeeState_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox cbx = sender as ComboBox;
            cbx.ItemsSource = new List<Employeestate>()
            {       
                new Employeestate(){ Name="在职", Value="0"},        
                new Employeestate(){ Name="离职", Value="1"}         
                     };
            cbx.DisplayMemberPath = "Name";
            cbx.SelectedIndex = 0;
        }
        public class Employeestate
        {
            private string name;
            private string value;
            public string Name
            {
                get { return name; }
                set { name = value; }
            }
            public string Value
            {
                get { return this.value; }
                set { this.value = value; }
            }
        }
    }
}
