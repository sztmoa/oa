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
using SMT.FBAnalysis.ClientServices.DailyManagementWS;//添加日常管理的服务引用
using SMT.FBAnalysis.UI.Form;

using SMT.SaaS.FrameworkUI.OrganizationControl;
using SMT.SAAS.CommonReportsModel;
using System.IO;

namespace SMT.FBAnalysis.UI.Views.DailyManagement
{
    public partial class ChargeApplyManagement : BasePage, IClient
    {
        #region 定义变量
        private bool IsQuery = false;
        private SMTLoading loadbar = new SMTLoading();
        private SaveFileDialog dialog = new SaveFileDialog();
        private ObservableCollection<string> DelInfosList = new ObservableCollection<string>();//删除单据ID集合
        DailyManagementServicesClient client = new DailyManagementServicesClient(); //定义客户端WCF引用对象
        T_FB_CHARGEAPPLYMASTER ChargeEntity = new T_FB_CHARGEAPPLYMASTER();//个人费用申请实体
        private string checkState = ((int)CheckStates.All).ToString();
        private string strOwnerID = string.Empty; //所属人ID
        private bool? result;
        DataGrid dgrid = new DataGrid();
        PermissionServiceClient PermClient = new PermissionServiceClient();
        private T_SYS_ENTITYMENUCUSTOMPERM customerPermission;
        #endregion

        #region 构造函数
        public ChargeApplyManagement()
        {
            CommonTools.InitComonConverter();
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                PARENT.Children.Add(loadbar);
                loadbar.Start();
                ChargeApplyManagement_Loaded(sender, args);
                InitEvent();//WCF事件声明
                Utility.DisplayGridToolBarButton(ToolBar, "T_FB_CHARGEAPPLYMASTER", true);//权限过滤
                InitToolBarEvent();//TOOLBAR按钮事件
                InitGrid();
            };
        }
        /// <summary>
        /// 初始化Grid
        /// </summary>
        void InitGrid()
        {
            dgrid.Name = Utility.GetResourceStr("PEOPLECHARGEAPPLY");
            DataGridTextColumn dgtextColumn = new DataGridTextColumn();
            dgtextColumn.Binding = new Binding("CHARGEAPPLYMASTERCODE");
            dgtextColumn.Header = "单据编号";
            dgrid.Columns.Add(dgtextColumn);
            dgtextColumn = new DataGridTextColumn();
            dgtextColumn.Binding = new Binding("BANKCARDNUMBER");
            dgtextColumn.Header = "帐号";
            dgrid.Columns.Add(dgtextColumn);
            dgtextColumn = new DataGridTextColumn();
            dgtextColumn.Binding = new Binding("EMPLOYEECNAME");
            dgtextColumn.Header = "户名";
            dgrid.Columns.Add(dgtextColumn);
            dgtextColumn = new DataGridTextColumn();
            dgtextColumn.Binding = new Binding("TOTALMONEY");
            dgtextColumn.Header = "金额";
            dgrid.Columns.Add(dgtextColumn);
            dgtextColumn = new DataGridTextColumn();
            dgtextColumn.Binding = new Binding("BANKID");
            dgtextColumn.Header = "开户行";
            dgrid.Columns.Add(dgtextColumn);
            dgtextColumn = new DataGridTextColumn();
            dgtextColumn.Binding = new Binding("BANKADDRESS");
            dgtextColumn.Header = "开户地";
            dgrid.Columns.Add(dgtextColumn);
        }
        void ChargeApplyManagement_Loaded(object sender, RoutedEventArgs e)
        {
            InitQueryFilter();
        }
        #endregion

        #region WCF事件声明
        private void InitEvent()
        {
            client.ExportChargeApplyMasterReportsCompleted += new EventHandler<ExportChargeApplyMasterReportsCompletedEventArgs>(client_ExportChargeApplyMasterReportsCompleted);
            client.GetChargeApplyMasterListByMultSearchCompleted += new EventHandler<GetChargeApplyMasterListByMultSearchCompletedEventArgs>(client_GetChargeApplyMasterListByMultSearchCompleted);
            client.DelChargeApplyMasterAndDetailCompleted += new EventHandler<DelChargeApplyMasterAndDetailCompletedEventArgs>(client_DelChargeApplyMasterAndDetailCompleted);
            //PermClient.GetCustomerPermissionByUserIDAndEntityCodeCompleted += new EventHandler<GetCustomerPermissionByUserIDAndEntityCodeCompletedEventArgs>(PermClient_GetCustomerPermissionByUserIDAndEntityCodeCompleted);
            //if (customerPermission == null)
            //    PermClient.GetCustomerPermissionByUserIDAndEntityCodeAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.SysUserID, "T_FB_CHARGEAPPLYMASTER");
        }
        /// <summary>
        /// 读取自定义权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PermClient_GetCustomerPermissionByUserIDAndEntityCodeCompleted(object sender, GetCustomerPermissionByUserIDAndEntityCodeCompletedEventArgs e)
        {
            ToolBar.btnImport.Visibility = Visibility.Visible;//首先显示，为了读取失败之后不影响原有操作。
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        customerPermission = new T_SYS_ENTITYMENUCUSTOMPERM();
                        customerPermission = e.Result;
                    }
                    else
                    {
                        ToolBar.btnImport.Visibility = Visibility.Collapsed;//没有自定义权限则隐藏
                    }
                }
        }
        void client_ExportChargeApplyMasterReportsCompleted(object sender, ExportChargeApplyMasterReportsCompletedEventArgs e)
        {
            if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        dgrid.ItemsSource = e.Result;
                        //ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("导出成功"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                    }
                }
        }

        #endregion

        #region TOOBAR按钮事件声明

        private void InitToolBarEvent()
        {
            //ComboBox cbx = this.cbxPayed;
            //if (cbx != null)
            //{
            //    cbx.Loaded += new RoutedEventHandler(cbxPayed_Loaded);
            //}
            ToolBar.btnNew.Click += new RoutedEventHandler(btnNew_Click);
            ToolBar.btnEdit.Click += new RoutedEventHandler(btnEdit_Click);
            ToolBar.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            ToolBar.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
            //提交审核
           // ToolBar.btnPrint.Click += new RoutedEventHandler(btnPrint_Click);
            ToolBar.btnPrint.Visibility = System.Windows.Visibility.Collapsed;  //转用新系统，停用Silverlight版本打印

            ToolBar.BtnView.Click += new RoutedEventHandler(BtnDetail_Click);
            ToolBar.btnAudit.Click += new RoutedEventHandler(btnAudit_Click);
            ToolBar.btnReSubmit.Click += new RoutedEventHandler(btnReSubmit_Click);
            ToolBar.cbxCheckState.SelectionChanged += new SelectionChangedEventHandler(cbxCheckState_SelectionChanged);
            ToolBar.stpOtherAction.Visibility = Visibility.Visible;
            //ToolBar.btnOutExcel.Visibility = Visibility.Visible;
            //ToolBar.btnImport.Visibility = Visibility.Visible;
            //ToolBar.btnOutExcel.Click += new RoutedEventHandler(btnOutExcel_Click);
            //ToolBar.btnImport.Click+=new RoutedEventHandler(btnImport_Click);
            //audit.AuditCompleted += new EventHandler<SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs>(audit_AuditCompleted);
            SMT.FBAnalysis.UI.Common.Utility.CbxItemBinder(ToolBar.cbxCheckState, "CHECKSTATE", checkState);

            //InitComboxSource();
            //this.Loaded += new RoutedEventHandler(CompanySendDocManagement_Loaded);
            ToolBar.ShowRect();
        }
        #endregion

        #region TOOLBAR按钮事件


        #region 新建按钮

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ChargeApplyForm AddWin = new ChargeApplyForm(FormTypes.New, "");
            AddWin.Deleted += (ss, ee) => { GetData(); };
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
            if (ChargeEntity != null)
            {
                if (SMT.SaaS.FrameworkUI.Common.Utility.ToolBarButtonOperationPermission(ChargeEntity, "T_FB_CHARGEAPPLYMASTER", OperationType.Edit, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID))
                {

                    if (ChargeEntity.CHECKSTATES == (int)CheckStates.UnSubmit)
                    {
                        //add zl 2012.2.9
                        if(ChargeEntity.T_FB_EXTENSIONALORDER != null)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "出差报销单不能进行修改！");
                            return;
                        }
                        //add end
                        ChargeApplyForm AddWin = new ChargeApplyForm(FormTypes.Edit, ChargeEntity.CHARGEAPPLYMASTERID);
                        
                        EntityBrowser browser = new EntityBrowser(AddWin);
                        browser.FormType = FormTypes.Edit;
                        browser.MinWidth = 800;
                        browser.MinHeight = 500;
                        browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                        browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, ChargeEntity.CHARGEAPPLYMASTERID);
                        AddWin.Deleted += (ss, ee) => { browser.Close(); GetData(); };
                        //ChargeEntity = null;
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
                        T_FB_CHARGEAPPLYMASTER tmpCharge = new T_FB_CHARGEAPPLYMASTER();
                        tmpCharge = DaGr.SelectedItems[i] as T_FB_CHARGEAPPLYMASTER;
                        string ChargeId = "";
                        ChargeId = tmpCharge.CHARGEAPPLYMASTERID;
                        //add zl 2012.2.9
                        if (tmpCharge.T_FB_EXTENSIONALORDER != null)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "出差报销单不能进行删除！");
                            return;
                        }
                        //add end
                        if (!SMT.SaaS.FrameworkUI.Common.Utility.ToolBarButtonOperationPermission(tmpCharge, "T_FB_CHARGEAPPLYMASTER", OperationType.Delete, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID))
                        {
                            StrTip = "您不能删除您选中的第" + (i + 1).ToString() + "条，单据编号为" + tmpCharge.CHARGEAPPLYMASTERCODE + "的报销申请";
                            ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), StrTip, Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                            break;
                        }

                        if (!(DelInfosList.IndexOf(ChargeId) > -1))
                        {
                            if (tmpCharge.CHECKSTATES == (int)CheckStates.UnSubmit)
                            {
                                DelInfosList.Add(ChargeId);
                            }
                        }
                    }
                    if (DelInfosList.Count > 0)
                    {
                        client.DelChargeApplyMasterAndDetailAsync(DelInfosList);
                    }
                    else
                    {
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

        void client_DelChargeApplyMasterAndDetailCompleted(object sender, DelChargeApplyMasterAndDetailCompletedEventArgs e)
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

            if (ChargeEntity != null)
            {
                if (!string.IsNullOrEmpty(ChargeEntity.CHARGEAPPLYMASTERID))
                {
                    //add zl 2012.2.9
                    if (ChargeEntity.T_FB_EXTENSIONALORDER != null)
                    {
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "出差报销单不能进行审核！");
                        return;
                    }
                    //add end
                    ChargeApplyForm AddWin = new ChargeApplyForm(FormTypes.Audit, ChargeEntity.CHARGEAPPLYMASTERID);
                    

                    EntityBrowser browser = new EntityBrowser(AddWin);
                    browser.FormType = FormTypes.Audit;
                    browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, ChargeEntity.CHARGEAPPLYMASTERID);
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
            if (ChargeEntity != null && !string.IsNullOrEmpty(ChargeEntity.CHARGEAPPLYMASTERID))
            {
                if (ChargeEntity.CHECKSTATES == (int)CheckStates.UnApproved)
                {
                    //add zl 2012.2.9
                    if (ChargeEntity.T_FB_EXTENSIONALORDER != null)
                    {
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "出差报销单不能进行提交！");
                        return;
                    }
                    //add end
                    if (ChargeEntity.UPDATEDATE <= dt)   //add zl
                    {
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "2012.2.17号及之前日期的单据不能进行重新提交，请另新建单据！");
                        return;
                    }
                    ChargeApplyForm AddWin = new ChargeApplyForm(FormTypes.Resubmit, ChargeEntity.CHARGEAPPLYMASTERID);
                    
                    EntityBrowser browser = new EntityBrowser(AddWin);
                    browser.FormType = FormTypes.Resubmit;
                    browser.MinWidth = 800;
                    browser.MinHeight = 500;
                    browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                    browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, ChargeEntity.CHARGEAPPLYMASTERID);
                }
                else
                {
                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("CAUTION"), Utility.GetResourceStr("此费用报销单不能重新提交审核"));
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
            if (ChargeEntity != null)
            {
                ChargeApplyForm DetailWin = new ChargeApplyForm(FormTypes.Browse, ChargeEntity.CHARGEAPPLYMASTERID);
                
                EntityBrowser browser = new EntityBrowser(DetailWin);
                browser.FormType = FormTypes.Browse;
                browser.MinWidth = 800;
                browser.MinHeight = 500;
                //browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
                browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, ChargeEntity.CHARGEAPPLYMASTERID);
                //ChargeEntity = null;
            }
            else
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "VIEW"), Utility.GetResourceStr("CONFIRMBUTTON"));
                return;
            }
        }
        #endregion

        //#region 打印按钮
        //void btnPrint_Click(object sender, RoutedEventArgs e)
        //{
        //    var obj = DaGr.SelectedItems;
        //    if (obj == null || obj.Count == 0)
        //    {
        //        ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), Utility.GetResourceStr("SELECTERROR", "PRINT"), Utility.GetResourceStr("CONFIRMBUTTON"), MessageIcon.Exclamation);
        //        return;
        //    }
        //    if (ChargeEntity == null)
        //    {
        //        ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "PRINT"), Utility.GetResourceStr("CONFIRMBUTTON"));
        //        return;
        //    }

        //    if (ChargeEntity.CHARGEAPPLYMASTERCODE.StartsWith("FYSQ"))
        //    {
        //        CostApplyBillReport DetailWin = new CostApplyBillReport(ChargeEntity.CHARGEAPPLYMASTERID);
        //        CommonReportsView browser = new CommonReportsView(null, DetailWin, DetailWin.view) { EntityEditor = DetailWin };

        //        browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, ChargeEntity.CHARGEAPPLYMASTERID);
        //    }
        //    else if (ChargeEntity.CHARGEAPPLYMASTERCODE.StartsWith("CLBX"))
        //    {
        //        TravelCostApplyReport DetailWin = new TravelCostApplyReport(ChargeEntity.CHARGEAPPLYMASTERID);
        //        CommonReportsView browser = new CommonReportsView(null, DetailWin, DetailWin.view) { EntityEditor = DetailWin };

        //        browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, ChargeEntity.CHARGEAPPLYMASTERID);
        //    }
        //    //ChargeEntity = null;

        //}
        //#endregion

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
        #region 导出、导入事件
        ///// <summary>
        ///// 导出
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void btnOutExcel_Click(object sender, RoutedEventArgs e)
        //{
        //    SMT.FBAnalysis.UI.Common.ExportToCSV.ExportDataGridSaveAs(dgrid);
        //}
        #endregion 导出、导入事件
        #region 重新加载数据


        #region 加载事件
        private void cbxPayed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //DictionaryComboBox txtDepType = Utility.FindChildControl<DictionaryComboBox>(expander, "cbxDepType");
            //T_SYS_DICTIONARY ent = ((System.Windows.Controls.Primitives.Selector)(sender)).SelectedItem as T_SYS_DICTIONARY;
            // txtDepType.SelectedValue = ent.DICTIONARYVALUE.GetValueOrDefault().ToString();
            dataPager.PageIndex = 1;
            GetData();
        }

        //private void cbxPayed_Loaded(object sender, RoutedEventArgs e)
        //{
        //    ComboBox cbxPayed = sender as ComboBox;
        //    List<SMT.Saas.Tools.PermissionWS.T_SYS_DICTIONARY> dicts = Application.Current.Resources["SYS_DICTIONARY"] as List<SMT.Saas.Tools.PermissionWS.T_SYS_DICTIONARY>;
        //    dicts = dicts.Where(s => s.DICTIONCATEGORY == "CHARGEAPPLYISPAYED").OrderBy(s => s.DICTIONARYVALUE).ToList();

        //    cbxPayed.ItemsSource = dicts.ToList();
        //    cbxPayed.DisplayMemberPath = "DICTIONARYNAME";

        //}
        #endregion 加载事件
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
            //  DatePicker dpEnd = this.expander.FindChildControl<DatePicker>("dpEnd");
            dpEnd.SelectedDate = DateTime.Now.AddDays(1).AddSeconds(-1);
        }

        //刷新
        private void browser_ReloadDataEvent()
        {
            GetData();
        }

        private void GetData()
        {
            int pageCount = 0;
            string filter = "";    //查询过滤条件
            ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();   //参数值  

            string strChargeCode = string.Empty, strDateStart = string.Empty, strDateEnd = string.Empty;
            decimal? isPayed;
            strChargeCode = txtCode.Text.ToString();

            if (dpStart.SelectedDate != null)
            {
                strDateStart = dpStart.SelectedDate.Value.ToString("yyyy-MM-dd");
            }

            if (dpEnd.SelectedDate != null)
            {
                //xiedx
                //2012-8-29
                strDateEnd = dpEnd.SelectedDate.Value.AddDays(1).ToString();
            }
            //if (this.cbxPayed.SelectedItem != null)
            //{
            //    T_SYS_DICTIONARY entDic = this.cbxPayed.SelectedItem as T_SYS_DICTIONARY;
            //    isPayed = entDic.DICTIONARYVALUE;
            //    if (!string.IsNullOrEmpty(filter))
            //    {
            //        filter += " and ";
            //    }
            //    filter += " ISPAYED=@" + paras.Count().ToString();
            //    paras.Add(isPayed);
            //}
            if (!string.IsNullOrEmpty(strChargeCode))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "@" + paras.Count().ToString() + ".Contains(CHARGEAPPLYMASTERCODE) "; //费用申请单编号
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
            client.GetChargeApplyMasterListByMultSearchAsync(strOwnerID, strDateStart, strDateEnd, checkState, filter, paras,
                "UPDATEDATE DESC", dataPager.PageIndex, dataPager.PageSize, pageCount);
            client.ExportChargeApplyMasterReportsAsync(strOwnerID, strDateStart, strDateEnd, checkState, filter, paras,
                "UPDATEDATE DESC");
        }

        /// <summary>
        /// 获取费用报销信息，并绑定数据到DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetChargeApplyMasterListByMultSearchCompleted(object sender, GetChargeApplyMasterListByMultSearchCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                ObservableCollection<T_FB_CHARGEAPPLYMASTER> entlist = e.Result;
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
                SMT.SaaS.FrameworkUI.Common.Utility.SetToolBarButtonByCheckState(dict.DICTIONARYVALUE.Value.ToInt32(), ToolBar, "T_FB_CHARGEAPPLYMASTER");
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
                ChargeEntity = grid.SelectedItem as T_FB_CHARGEAPPLYMASTER;
            }
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
        //private void btnImport_Click(object sender, RoutedEventArgs e)
        //{
        //    ImportChargeApply form = new ImportChargeApply();
        //    EntityBrowser entBrowser = new EntityBrowser(form);

        //    form.MinWidth = 450;
        //    form.MinHeight = 200;

        //    entBrowser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
        //    entBrowser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
        //}
        
    }
}
