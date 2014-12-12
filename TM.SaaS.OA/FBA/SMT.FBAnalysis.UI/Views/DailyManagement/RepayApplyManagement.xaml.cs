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
using SMT.SaaS.FrameworkUI;
using SMT.SaaS.FrameworkUI.Common;
using SMT.SaaS.FrameworkUI.ChildWidow;
using System.Collections.ObjectModel;
using SMT.SAAS.Main.CurrentContext;
using SMT.Saas.Tools.PermissionWS;
using SMT.FBAnalysis.ClientServices.DailyManagementWS; //添加日常管理的服务引用
using SMT.FBAnalysis.UI.Form;
using SMT.SaaS.FrameworkUI.OrganizationControl;
using SMT.SAAS.CommonReportsModel;


namespace SMT.FBAnalysis.UI.Views.DailyManagement
{
    public partial class RepayApplyManagement : BasePage, IClient
    {
        #region 定义变量
        private bool IsQuery = false;
        private SMTLoading loadbar = new SMTLoading();
        private ObservableCollection<string> DelInfosList = new ObservableCollection<string>();//删除单据ID集合
        DailyManagementServicesClient client = new DailyManagementServicesClient();//定义客户端WCF引用对象
        T_FB_REPAYAPPLYMASTER RepayEntity = new T_FB_REPAYAPPLYMASTER();//个人还款申请实体
        private string checkState = ((int)CheckStates.All).ToString();
        private string strOwnerID = ""; //所属人ID


        #endregion

        #region 构造函数
        public RepayApplyManagement()
        {
            CommonTools.InitComonConverter();
            InitializeComponent();
            this.Loaded += (sender, args) => {
                PARENT.Children.Add(loadbar);
                loadbar.Start();
                RepayApplyManagement_Loaded(sender, args);
                InitEvent();//WCF事件声明               
                Utility.DisplayGridToolBarButton(ToolBar, "T_FB_REPAYAPPLYMASTER", true);//权限过滤
                InitToolBarEvent();//TOOLBAR按钮事件                
            };           
        }

        void RepayApplyManagement_Loaded(object sender, RoutedEventArgs e)
        {
            InitQueryFilter();
        }

        #endregion

        #region WCF事件声明
        private void InitEvent()
        {
            client.GetRepayApplyMasterListByMultSearchCompleted += new EventHandler<GetRepayApplyMasterListByMultSearchCompletedEventArgs>(client_GetRepayApplyMasterListByMultSearchCompleted);
            client.DelRepayApplyMasterAndDetailCompleted += new EventHandler<DelRepayApplyMasterAndDetailCompletedEventArgs>(client_DelRepayApplyMasterAndDetailCompleted);
        }
        #endregion

        #region WCF对应的完成事件



        #endregion

        #region TOOBAR按钮事件声明

        private void InitToolBarEvent()
        {
            ToolBar.btnNew.Click += new RoutedEventHandler(btnNew_Click);
            ToolBar.btnEdit.Click += new RoutedEventHandler(btnEdit_Click);
            ToolBar.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            ToolBar.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
           // ToolBar.btnPrint.Click += new RoutedEventHandler(btnPrint_Click);
            ToolBar.btnPrint.Visibility = System.Windows.Visibility.Collapsed;  //转用新系统，停用Silverlight版本打印
            //提交审核
            ToolBar.BtnView.Click += new RoutedEventHandler(BtnDetail_Click);
            ToolBar.btnAudit.Click += new RoutedEventHandler(btnAudit_Click);
            ToolBar.btnReSubmit.Click += new RoutedEventHandler(btnReSubmit_Click);
            ToolBar.cbxCheckState.SelectionChanged += new SelectionChangedEventHandler(cbxCheckState_SelectionChanged);
            ToolBar.stpOtherAction.Visibility = Visibility.Visible;
            //audit.AuditCompleted += new EventHandler<SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs>(audit_AuditCompleted);
            SMT.FBAnalysis.UI.Common.Utility.CbxItemBinder(ToolBar.cbxCheckState, "CHECKSTATE", checkState);

            //InitComboxSource();
            //this.Loaded += new RoutedEventHandler(CompanySendDocManagement_Loaded);
            ToolBar.ShowRect();
            Utility.CbxItemBinder(ToolBar.cbxCheckState, "CHECKSTATE", "5");  
        }
        #endregion

        #region TOOLBAR按钮事件


        #region 新建按钮

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            //SMT.FBAnalysis.UI.Common.Utility.CreateFormFromEngine("143f3aa5-7c52-48d5-abfb-4a9bef57063c", "SMT.FBAnalysis.UI.Form.RepayApplyForm", "Edit");
            RepayApplyForm AddWin = new RepayApplyForm(FormTypes.New, "");
            EntityBrowser browser = new EntityBrowser(AddWin);
            browser.MinWidth = 800;
            browser.MinHeight = 500;
            browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
            browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true);
            AddWin.Deleted += (ss, ee) => { browser.Close(); GetData(); };

        }
        #endregion

        #region 修改按钮
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (RepayEntity != null)
            {
                if (SMT.SaaS.FrameworkUI.Common.Utility.ToolBarButtonOperationPermission(RepayEntity, "T_FB_REPAYAPPLYMASTER", OperationType.Edit, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID))
                {

                    if (RepayEntity.CHECKSTATES == (int)CheckStates.UnSubmit)
                    {

                        RepayApplyForm AddWin = new RepayApplyForm(FormTypes.Edit, RepayEntity.REPAYAPPLYMASTERID);
                        EntityBrowser browser = new EntityBrowser(AddWin);
                        browser.FormType = FormTypes.Edit;
                        browser.MinWidth = 800;
                        browser.MinHeight = 500;
                        browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                        browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true);
                        AddWin.Deleted += (ss, ee) => { browser.Close(); GetData(); };
                        //RepayEntity = null;
                    }
                    else
                    {
                        ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ONLYEDITUNSUBMITDATA"),
                        Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);//只能修改未提交的数据
                    }
                }
                else
                {
                    //Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), Utility.GetResourceStr("YOUDONOTHAVEPERMISSIONTOOPERATETHEDATA"));
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), Utility.GetResourceStr("YOUDONOTHAVEPERMISSIONTOOPERATETHEDATA"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                }

            }
            else
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "EDIT"), Utility.GetResourceStr("CONFIRMBUTTON"));
                return;
            }

        }
        #endregion

        #region 删除按钮

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DaGr.SelectedItems.Count > 0)
            {
                string Result = "";
                string StrTip = "";
                DelInfosList = new ObservableCollection<string>();
                ComfirmWindow com = new ComfirmWindow();
                com.OnSelectionBoxClosed += (obj, result) =>
                {
                    for (int i = 0; i < DaGr.SelectedItems.Count; i++)
                    {
                        T_FB_REPAYAPPLYMASTER tmpRepay = new T_FB_REPAYAPPLYMASTER();
                        tmpRepay = DaGr.SelectedItems[i] as T_FB_REPAYAPPLYMASTER;
                        string RepayId = "";
                        RepayId = tmpRepay.REPAYAPPLYMASTERID;
                        if (SMT.SaaS.FrameworkUI.Common.Utility.ToolBarButtonOperationPermission(tmpRepay, "T_FB_REPAYAPPLYMASTER", OperationType.Delete, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID))
                        {
                            if (!(DelInfosList.IndexOf(RepayId) > -1))
                            {
                                if (tmpRepay.CHECKSTATES == (int)CheckStates.UnSubmit)
                                {
                                    DelInfosList.Add(RepayId);
                                }
                            }
                        }
                        else
                        {
                            //Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), Utility.GetResourceStr("YOUDONOTHAVEPERMISSIONTOOPERATETHEDATA"));
                            StrTip = "您不能删除您选中的第" + (i + 1).ToString() + "条，单据编号为" + tmpRepay.REPAYAPPLYCODE + "的还款申请";
                            ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), StrTip, Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                            return;
                        }
                    }
                    if (DelInfosList.Count > 0)
                    {
                        client.DelRepayApplyMasterAndDetailAsync(DelInfosList);
                    }
                    else
                    {
                        //Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("CAUTION"), Utility.GetResourceStr("SELECTITEMSNOTDELETE"));
                        ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "只能删除未提交的单据！",
                        Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                    }
                };
                com.SelectionBox(Utility.GetResourceStr("DELETECONFIRM"), Utility.GetResourceStr("DELETEALTER"), ComfirmWindow.titlename, Result);
            }
            else
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "DELETE"), Utility.GetResourceStr("CONFIRMBUTTON"));
            }
        }

        void client_DelRepayApplyMasterAndDetailCompleted(object sender, DelRepayApplyMasterAndDetailCompletedEventArgs e)
        {
            if (e.Result == true)
            {
                GetData();
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("DELETESUCCESSED", ""));
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("FAILED"), Utility.GetResourceStr("DELETEFAILED", ""));
            }
        }

        #endregion

        #region 刷新按钮
        void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            browser_ReloadDataEvent();
        }
        #endregion

        #region 审核按钮
        void btnAudit_Click(object sender, RoutedEventArgs e)
        {

            if (RepayEntity != null)
            {
                if (!string.IsNullOrEmpty(RepayEntity.REPAYAPPLYMASTERID))
                {
                    RepayApplyForm AddWin = new RepayApplyForm(FormTypes.Audit, RepayEntity.REPAYAPPLYMASTERID);

                    EntityBrowser browser = new EntityBrowser(AddWin);
                    browser.FormType = FormTypes.Audit;
                    //browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                    browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
                }
            }
            else
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "AUDIT"), Utility.GetResourceStr("CONFIRMBUTTON"));

            }

        }
        #endregion

        #region 重新提交
        void btnReSubmit_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt = new DateTime(2012, 2, 18);
            var obj = DaGr.SelectedItems;
            if (obj != null && obj.Count >0)
            {
                if (RepayEntity.CHECKSTATES == (int)CheckStates.UnApproved)
                {
                    if (RepayEntity.UPDATEDATE <= dt)   //add zl
                    {
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "2012.2.17号及之前日期的单据不能进行重新提交，请另新建单据！");
                        return;
                    }
                    RepayApplyForm AddWin = new RepayApplyForm(FormTypes.Resubmit, RepayEntity.REPAYAPPLYMASTERID);
                    EntityBrowser browser = new EntityBrowser(AddWin);
                    browser.FormType = FormTypes.Resubmit;
                    browser.MinWidth = 800;
                    browser.MinHeight = 500;
                    browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                    browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true);
                }
                else
                {//todo 
                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("CAUTION"), Utility.GetResourceStr("不能重新提交审核"));
                }
            }
            else
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "SUBMITAUDIT"), Utility.GetResourceStr("CONFIRMBUTTON"));
                return;
            }
        }
        #endregion

        #region 查看按钮

        private void BtnDetail_Click(object sender, RoutedEventArgs e)
        {
            var obj = DaGr.SelectedItems;
            if (obj == null || obj.Count == 0)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), Utility.GetResourceStr("SELECTERROR", "VIEW"), Utility.GetResourceStr("CONFIRMBUTTON"), MessageIcon.Exclamation);
                return;
            }

            if (RepayEntity != null)
            {
                RepayApplyForm DetailWin = new RepayApplyForm(FormTypes.Browse, RepayEntity.REPAYAPPLYMASTERID);
                EntityBrowser browser = new EntityBrowser(DetailWin);
                browser.FormType = FormTypes.Browse;
                browser.MinWidth = 800;
                browser.MinHeight = 500;
                //browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
                //RepayEntity = null;
            }
            else
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "VIEW"), Utility.GetResourceStr("CONFIRMBUTTON"));
                return;
            }
        }
        #endregion

        //#region 打印按钮

        //private void btnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    var obj = DaGr.SelectedItems;
        //    if (obj == null || obj.Count == 0)
        //    {
        //        ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), Utility.GetResourceStr("SELECTERROR", "PRINT"), Utility.GetResourceStr("CONFIRMBUTTON"), MessageIcon.Exclamation);
        //        return;
        //    }

        //    if (RepayEntity != null)
        //    {
        //        RepayApplyReport DetailWin = new RepayApplyReport(RepayEntity.REPAYAPPLYMASTERID);
        //        CommonReportsView browser = new CommonReportsView(null, DetailWin, DetailWin.view) { EntityEditor = DetailWin };                               

        //        browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, RepayEntity.REPAYAPPLYMASTERID);
        //        //RepayEntity = null;
        //    }
        //    else
        //    {
        //        ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "PRINT"), Utility.GetResourceStr("CONFIRMBUTTON"));
        //        return;
        //    }
        //}
        //#endregion

        #region 重新加载数据

        /// <summary>
        /// 设置查询参数初始值
        /// </summary>
        private void InitQueryFilter()
        {
            SMT.FBAnalysis.UI.CommonTools.MultiValuesItem<ExtOrgObj> item = new SMT.FBAnalysis.UI.CommonTools.MultiValuesItem<ExtOrgObj>();
            SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE ep = new Saas.Tools.PersonnelWS.T_HR_EMPLOYEE();
            ep.EMPLOYEECNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
            ep.EMPLOYEEID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;

            List<ExtOrgObj> list = new List<ExtOrgObj>() { new ExtOrgObj { ObjectInstance = ep } };
            item.Values = list;
            item.Text = ep.EMPLOYEECNAME;
            lkOrg.SelectItem = item;
            lkOrg.DataContext = item;
            lkOrg.DisplayMemberPath = "Text";

            // 开始日期
            dpStart.SelectedDate = DateTime.Now.AddDays(-30);

            // 结束日期
            dpEnd.SelectedDate = DateTime.Now.AddDays(1).AddSeconds(-1);
        }

        //刷新
        private void browser_ReloadDataEvent()
        {
            GetData();
        }

        #region 申请人LookUp

        private void lkOrg_FindClick(object sender, EventArgs e)
        {
            SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup lookup = new SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup();
            lookup.SelectedObjType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.All;
            lookup.MultiSelected = false;
            lookup.SelectedClick += (o, ev) =>
            {
                if (lookup.SelectedObj.Count > 0)
                {
                    List<ExtOrgObj> list = new List<ExtOrgObj>();
                    string text = " ";

                    foreach (ExtOrgObj obj in lookup.SelectedObj)
                    {
                        list.Add(obj);

                        text = text.Trim() + ";" + obj.ObjectName;

                    }
                    SMT.FBAnalysis.UI.CommonTools.MultiValuesItem<ExtOrgObj> item = new SMT.FBAnalysis.UI.CommonTools.MultiValuesItem<ExtOrgObj>();
                    item.Values = list;
                    item.Text = text.Substring(1);
                    //item.Text = text.Substring(text.LastIndexOf(";") + 1);
                    lkOrg.SelectItem = item;
                    lkOrg.DataContext = item;
                    lkOrg.DisplayMemberPath = "Text";
                }
            };
            lookup.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
        }

        #endregion

        private void GetData()
        {
            int pageCount = 0;
            string filter = "";    //查询过滤条件
            ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();   //参数值  

            string strChargeCode = string.Empty, strDateStart = string.Empty, strDateEnd = string.Empty;
            strChargeCode = txtCode.Text.ToString();

            if (dpStart.SelectedDate != null)
            {
                strDateStart = dpStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            }

            if (dpEnd.SelectedDate != null)
            {
                strDateEnd = dpEnd.SelectedDate.Value.ToString();
            }

            if (!string.IsNullOrEmpty(strChargeCode))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "@" + paras.Count().ToString() + ".Contains(REPAYAPPLYCODE) "; //费用申请单编号
                paras.Add(strChargeCode);
            }

            if (lkOrg.DataContext != null)
            {
                SMT.FBAnalysis.UI.CommonTools.MultiValuesItem<ExtOrgObj> mutilValues = lkOrg.SelectItem as SMT.FBAnalysis.UI.CommonTools.MultiValuesItem<ExtOrgObj>;
                if (mutilValues != null)
                {
                    Dictionary<OrgTreeItemTypes, string> dictTypes = new Dictionary<OrgTreeItemTypes, string>();
                    dictTypes.Add(OrgTreeItemTypes.Company, "OWNERCOMPANYID");
                    dictTypes.Add(OrgTreeItemTypes.Department, "OWNERDEPARTMENTID");
                    dictTypes.Add(OrgTreeItemTypes.Personnel, "OWNERID");
                    dictTypes.Add(OrgTreeItemTypes.Post, "OWNERPOSTID");

                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += " AND ";
                    }

                    filter += " (";
                    for (int i = 0; i < mutilValues.Values.Count(); i++)
                    {
                        if (i > 0 && i < mutilValues.Values.Count() - 1)
                        {
                            filter += " OR ";

                        }

                        ExtOrgObj item = mutilValues.Values[i];
                        string propertyName = dictTypes[item.ObjectType];
                        string strOrgID = item.ObjectID;

                        filter += propertyName + " ==@" + paras.Count().ToString();
                        paras.Add(strOrgID);
                    };

                    filter += " )";

                }
            }

            strOwnerID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;

            loadbar.Start();
            client.GetRepayApplyMasterListByMultSearchAsync(strOwnerID, strDateStart, strDateEnd, checkState, filter, paras,
                "UPDATEDATE DESC", dataPager.PageIndex, dataPager.PageSize, pageCount);
        }

        void client_GetRepayApplyMasterListByMultSearchCompleted(object sender, GetRepayApplyMasterListByMultSearchCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                ObservableCollection<T_FB_REPAYAPPLYMASTER> entlist = e.Result;
                DaGr.ItemsSource = entlist;
                dataPager.PageCount = e.pageCount;
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
            }            
        }
        #endregion

        #region 审核状态条选择事件

        private void cbxCheckState_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            T_SYS_DICTIONARY dict = ToolBar.cbxCheckState.SelectedItem as T_SYS_DICTIONARY;
            if (dict != null)
            {
                SMT.SaaS.FrameworkUI.Common.Utility.SetToolBarButtonByCheckState(dict.DICTIONARYVALUE.Value.ToInt32(), ToolBar, "T_FB_REPAYAPPLYMASTER");
                checkState = dict.DICTIONARYVALUE.ToString();
                GetData();

                //ToolBar.btnPrint.Visibility = System.Windows.Visibility.Collapsed;
                //if (checkState == Convert.ToInt32(CheckStates.Approved).ToString())
                //{
                //    ToolBar.btnPrint.Visibility = System.Windows.Visibility.Visible;
                //}
            }

        }

        #endregion

        #endregion

        #region 导航函数


        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #endregion

        #region 翻页控件
        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        #endregion

        #region DATAGRID行选择事件
        private void DaGr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DaGr.SelectedItems.Count == 0)
            {
                return;
            }
            //SelectMeeting = DaGr.SelectedItems[0] as V_BumfCompanySendDoc;
            DataGrid grid = sender as DataGrid;
            if (grid.SelectedItems.Count > 0)
            {
                RepayEntity = grid.SelectedItem as T_FB_REPAYAPPLYMASTER;
            }
        }

        #endregion

        #region DaGr_loadingRow
        private void DaGr_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }
        #endregion

        #region 查询按钮
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            IsQuery = true;
            GetData();
        }
        #endregion

        #region 重置按钮
        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            txtCode.Text = string.Empty;
            // 开始日期
            dpStart.SelectedDate = DateTime.Now.AddDays(-30);

            // 结束日期
            dpEnd.SelectedDate = DateTime.Now.AddDays(1).AddSeconds(-1);
            lkOrg.DataContext = null;
        }

        #endregion

        #region 实现接口IClient

        public void ClosedWCFClient()
        {
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

    }
}
