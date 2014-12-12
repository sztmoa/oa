/*
 * 文件名：AcountsView.xaml.cs
 * 作  用：预算查询分析-执行一览
 * 创建人：吴鹏
 * 创建时间：2011-01-27 15:33:04
 * 修改人：
 * 修改时间：
 */

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
using SMT.SaaS.FrameworkUI.Common;
using SMT.SaaS.FrameworkUI.OrganizationControl;
using SMT.FBAnalysis.ClientServices.FBAnalysisWS;
using SMT.FBAnalysis.UI.Models;
using SMT.Saas.Tools.OrganizationWS;
using SMT.Saas.Tools.PermissionWS;

using System.Text;
using System.Collections.ObjectModel;
using System.IO;

namespace SMT.FBAnalysis.UI.Views
{
    public partial class OrgSelectEntity
    {
        public OrgSelectEntity(List<ExtOrgObj> selectedValue)
        {
            this.SelectedValue = selectedValue;
            if (selectedValue != null && selectedValue.Count > 0)
            {
                IsMultipleType = selectedValue.GroupBy(item => item.ObjectType).Count() > 1;
            }
        }

        public bool IsMultipleType { get; set; }

        public string Text
        {
            get
            {
                var result = string.Empty;
                if (this.SelectedValue != null)
                {
                    result = string.Join(";", this.SelectedValue.Select(item => item.ObjectName));
                }
                return result;
            }
        }

        public OrgTreeItemTypes SelectedOrgType
        {
            get
            {
                if (this.SelectedValue == null)
                {
                    return OrgTreeItemTypes.All;
                }
                return this.SelectedValue.FirstOrDefault().ObjectType;
            }

        }
        public void GetSqlValue(ref string filter, ref ObservableCollection<object> paras)
        {
            Dictionary<OrgTreeItemTypes, string> dict = new Dictionary<OrgTreeItemTypes, string>();
            dict.Add(OrgTreeItemTypes.Company, "OWNERCOMPANYID");
            dict.Add(OrgTreeItemTypes.Department, "OWNERDEPARTMENTID");
            dict.Add(OrgTreeItemTypes.Post, "OWNERPOSTID");
            dict.Add(OrgTreeItemTypes.Personnel, "OWNERID");
            if (this.SelectedValue == null)
            {
                return;
            }
            List<string> filters = new List<string>();
            foreach (var item in SelectedValue)
            {
                var temp = string.Format("({0}=@{1})", dict[item.ObjectType], paras.Count);
                paras.Add(item.ObjectID);
                filters.Add(temp);
            }
            filter = string.Join(" or ", filters);
            filter = "( " + filter + " )";
        }


        public List<ExtOrgObj> SelectedValue { get; set; }
    }

    public partial class AcountsView : BasePage
    {
        FBAnalysisServiceClient clientFBA = new FBAnalysisServiceClient();
        private SMTLoading loadbar = new SMTLoading();
        private string strSearchSubjects { get; set; }
        Stream stream;

        public AcountsView()
        {
            
            CheckConverter();
            InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                RegisterEvents();
                AcountsView_Loaded(sender, args);
            };
        }

        #region 私有方法
        /// <summary>
        /// 加载Converter
        /// </summary>
        private void CheckConverter()
        {
            if (Application.Current.Resources["CompanyInfoConverter"] == null)
            {
                Application.Current.Resources.Add("CompanyInfoConverter", new SMT.FBAnalysis.UI.CompanyInfoConverter());
            }
            if (Application.Current.Resources["CustomDictionaryConverter"] == null)
            {
                Application.Current.Resources.Add("CustomDictionaryConverter", new SMT.FBAnalysis.UI.CustomDictionaryConverter());
            }
            if (Application.Current.Resources["CustomDateConverter"] == null)
            {
                Application.Current.Resources.Add("CustomDateConverter", new SMT.FBAnalysis.UI.CustomDateConverter());
            }
            if (Application.Current.Resources["FinanceConverter"] == null)
            {
                Application.Current.Resources.Add("FinanceConverter", new SMT.FBAnalysis.UI.FinanceConverter());
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            PARENT.Children.Add(loadbar);
            nudYear.Value = DateTime.Now.Year;
            nudMonthStart.Value = 1;
            nudMonthEnd.Value = DateTime.Now.Month;
            ShowToolBarItem();

            clientFBA.GetExecutionListByPagingCompleted += new EventHandler<GetExecutionListByPagingCompletedEventArgs>(clientFBA_GetExecutionListByPagingCompleted);
            clientFBA.GetPerExecutionListByPagingCompleted += new EventHandler<GetPerExecutionListByPagingCompletedEventArgs>(clientFBA_GetPerExecutionListByPagingCompleted);
            clientFBA.OutFileExecutionListCompleted += new EventHandler<OutFileExecutionListCompletedEventArgs>(clientFBA_OutFileExecutionListCompleted);
            clientFBA.OutFilePerExecutionListCompleted += new EventHandler<OutFilePerExecutionListCompletedEventArgs>(clientFBA_OutFilePerExecutionListCompleted);
            clientFBA.OutFileDeptDayBookDataCompleted += new EventHandler<OutFileDeptDayBookDataCompletedEventArgs>(clientFBA_OutFileDeptDayBookDataCompleted);
            clientFBA.OutFilePerDayBookDataCompleted += new EventHandler<OutFilePerDayBookDataCompletedEventArgs>(clientFBA_OutFilePerDayBookDataCompleted);

        }

        /// <summary>
        /// 显示按钮
        /// </summary>
        private void ShowToolBarItem()
        {
            toolBarTop.btnNew.Visibility = Visibility.Collapsed;
            toolBarTop.retNew.Visibility = System.Windows.Visibility.Collapsed;
            toolBarTop.btnEdit.Visibility = Visibility.Collapsed;
            toolBarTop.retEdit.Visibility = Visibility.Collapsed;
            toolBarTop.btnDelete.Visibility = Visibility.Collapsed;
            toolBarTop.retDelete.Visibility = Visibility.Collapsed;
            toolBarTop.BtnView.Visibility = Visibility.Collapsed;
            toolBarTop.retRead.Visibility = Visibility.Collapsed;
            toolBarTop.btnAudit.Visibility = Visibility.Collapsed;
            toolBarTop.retAudit.Visibility = Visibility.Collapsed;
            toolBarTop.cbxCheckState.Visibility = Visibility.Collapsed;
            toolBarTop.stpCheckState.Visibility = Visibility.Collapsed;

            toolBarTop.btnOtherAction("/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_excel.png", "导出流水").Click += new RoutedEventHandler(btnOutDayBook_Click);

            toolBarTop.btnOutExcel.Visibility = Visibility.Visible;
            toolBarTop.btnOutExcel.Click += new RoutedEventHandler(btnOutExcel_Click);
            toolBarTop.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
        }

        /// <summary>
        /// 绑定DataGrid
        /// </summary>
        private void BindData()
        {
            int pageCount = 0;
            string filter = string.Empty;
            ObservableCollection<object> paras = new ObservableCollection<object>();
            string strOrgType = string.Empty;
            string strOrgID = string.Empty;
            int iYear = 0, iMonthStart = 0, iMonthEnd = 0;
            var isOK = GetFilter(ref filter, ref paras, out iYear, out iMonthStart, out iMonthEnd );
            if (!isOK)
            {
                dgQueryResult.ItemsSource = null;
                dgPerQueryResult.ItemsSource = null;
                return;
            }
            
            loadbar.Start();
            if (dgQueryResult.Visibility == System.Windows.Visibility.Visible)
            {
                if (iMonthStart == 1)
                {
                    IsHideDataGridColumns(dgQueryResult, 20, 2, System.Windows.Visibility.Visible);    //控制显示年度预算结余和月度预算结余 
                }
                else
                {
                    IsHideDataGridColumns(dgQueryResult, 20, 2, System.Windows.Visibility.Collapsed);    //控制隐藏年度预算结余和月度预算结余
                }

                IsHideMonthsDataGridColumns(dgQueryResult, iMonthStart, iMonthEnd, 5);    //控制隐藏月份报销 

                clientFBA.GetExecutionListByPagingAsync(strOrgType, strOrgID, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, iYear, iMonthStart, iMonthEnd, filter, paras, "BUDGETCHECKDATE", dataPager.PageIndex, dataPager.PageSize, pageCount);
            }
            else if (dgPerQueryResult.Visibility == System.Windows.Visibility.Visible)
            {
                IsHideMonthsDataGridColumns(dgPerQueryResult, iMonthStart, iMonthEnd, 4);    //控制隐藏月份报销 
                clientFBA.GetPerExecutionListByPagingAsync(strOrgType, strOrgID, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, iYear, iMonthStart, iMonthEnd, filter, paras, "BUDGETCHECKDATE", dataPager.PageIndex, dataPager.PageSize, pageCount);
            }
        }

        private void GetSearchConditionBySubjectID(ref string filter, ref ObservableCollection<object> paras)
        {
            if (string.IsNullOrWhiteSpace(strSearchSubjects))
            {
                return;
            }

            string[] strlist = strSearchSubjects.Split(';');
            
            StringBuilder strTemp = new StringBuilder();
            for (int i = 0; i < strlist.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(strlist[i]))
                {
                    if (i > 0)
                    {
                        strTemp.Append(" OR ");
                    }

                    strTemp.Append(" (SUBJECTID ==@" + paras.Count().ToString() + ") ");
                    paras.Add(strlist[i]);
                }
            }


            if (!string.IsNullOrEmpty(filter))
            {
                filter += " and ";
            }

            filter += " (" + strTemp.ToString() + ")";
        }
        
        
        /// <summary>
        /// 提交子窗口的表单后，回刷父页面
        /// </summary>
        void browser_ReloadDataEvent()
        {
            return;
        }

        /// <summary>
        /// 显示公司及部门选中链接指向的明细记录
        /// </summary>
        /// <param name="iBudgetRecordType"></param>
        private void ShowRecordDetails(int iBudgetRecordType)
        {
            if (iBudgetRecordType == 0)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, "台帐查询", "当前选中项无单据");
                return;
            }

            string strSignInID = string.Empty;
            if (dgQueryResult.SelectedItems == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "VIEW"));
                return;
            }

            if (dgQueryResult.SelectedItems.Count == 0)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "VIEW"));
                return;
            }

            int iYear = 0, iMonthStart = 0, iMonthEnd = 0;

            int.TryParse(nudYear.Value.ToString(), out iYear);
            int.TryParse(nudMonthStart.Value.ToString(), out iMonthStart);
            int.TryParse(nudMonthEnd.Value.ToString(), out iMonthEnd);

            if (iMonthStart > iMonthEnd)
            {
                return;
            }

            V_ExecutionList ent = dgQueryResult.SelectedItems[0] as V_ExecutionList;

            decimal dMoney = 0;
            string strTitle = string.Empty, strMsg = string.Empty;
            switch (iBudgetRecordType)
            {
                case 1:
                    dMoney = ent.BudgetMoneyYear;
                    strTitle = "年度预算总览";
                    break;
                case 2:
                    dMoney = ent.BudgetMoneyMonth;
                    strTitle = "月度预算总览";
                    break;
                case 3:
                    dMoney = ent.ApprovedApplyMoney;
                    strTitle = "已审核报销费用总览";
                    break;
                case 4:
                    dMoney = ent.ApprovingApplyMoney;
                    strTitle = "审核中报销费用总览";
                    break;
                case -1:
                    iBudgetRecordType = 3;
                    dMoney = ent.JanApprovedMoney;
                    iMonthStart = 1;
                    iMonthEnd = 1;
                    strTitle = "一月已审核报销费用总览";
                    break;
                case -2:
                    iBudgetRecordType = 3;
                    dMoney = ent.FebApprovedMoney;
                    iMonthStart = 2;
                    iMonthEnd = 2;
                    strTitle = "二月已审核报销费用总览";
                    break;
                case -3:
                    iBudgetRecordType = 3;
                    dMoney = ent.MarApprovedMoney;
                    iMonthStart = 3;
                    iMonthEnd = 3;
                    strTitle = "三月已审核报销费用总览";
                    break;
                case -4:
                    iBudgetRecordType = 3;
                    dMoney = ent.AprApprovedMoney;
                    iMonthStart = 4;
                    iMonthEnd = 4;
                    strTitle = "四月已审核报销费用总览";
                    break;
                case -5:
                    iBudgetRecordType = 3;
                    dMoney = ent.MayApprovedMoney;
                    iMonthStart = 5;
                    iMonthEnd = 5;
                    strTitle = "五月已审核报销费用总览";
                    break;
                case -6:
                    iBudgetRecordType = 3;
                    dMoney = ent.JunApprovedMoney;
                    iMonthStart = 6;
                    iMonthEnd = 6;
                    strTitle = "六月已审核报销费用总览";
                    break;
                case -7:
                    iBudgetRecordType = 3;
                    dMoney = ent.JulApprovedMoney;
                    iMonthStart = 7;
                    iMonthEnd = 7;
                    strTitle = "七月已审核报销费用总览";
                    break;
                case -8:
                    iBudgetRecordType = 3;
                    dMoney = ent.AugApprovedMoney;
                    iMonthStart = 8;
                    iMonthEnd = 8;
                    strTitle = "八月已审核报销费用总览";
                    break;
                case -9:
                    iBudgetRecordType = 3;
                    dMoney = ent.SepApprovedMoney;
                    iMonthStart = 9;
                    iMonthEnd = 9;
                    strTitle = "九月已审核报销费用总览";
                    break;
                case -10:
                    iBudgetRecordType = 3;
                    dMoney = ent.OctApprovedMoney;
                    iMonthStart = 10;
                    iMonthEnd = 10;
                    strTitle = "十月已审核报销费用总览";
                    break;
                case -11:
                    iBudgetRecordType = 3;
                    dMoney = ent.NovApprovedMoney;
                    iMonthStart = 11;
                    iMonthEnd = 11;
                    strTitle = "十一月已审核报销费用总览";
                    break;
                case -12:
                    iBudgetRecordType = 3;
                    dMoney = ent.DecApprovedMoney;
                    iMonthStart = 12;
                    iMonthEnd = 12;
                    strTitle = "十二月已审核报销费用总览";
                    break;
            }

            if (dMoney == 0)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, strTitle, "当前选中项无单据");
                return;
            }

            if (ent.SubjectName == "活动经费" && iBudgetRecordType == 2)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, strTitle, "当前选中项禁止查看详单");
                return;
            }

            RecordList viewRd = new RecordList(FormTypes.Browse, iBudgetRecordType, iYear, iMonthStart, iMonthEnd, ent);

            EntityBrowser entBrowser = new EntityBrowser(viewRd);

            entBrowser.FormType = FormTypes.Browse;
            entBrowser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
            entBrowser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
        }

        /// <summary>
        /// 显示岗位及个人选中链接指向的明细记录
        /// </summary>
        /// <param name="iBudgetRecordType"></param>
        private void ShowPerRecordDetails(int iBudgetRecordType)
        {
            if (iBudgetRecordType == 0)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, "台帐查询", "当前选中项无单据");
                return;
            }

            string strSignInID = string.Empty;
            if (dgPerQueryResult.SelectedItems == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "VIEW"));
                return;
            }

            if (dgPerQueryResult.SelectedItems.Count == 0)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("SELECTERROR", "VIEW"));
                return;
            }

            int iYear = 0, iMonthStart = 0, iMonthEnd = 0;

            int.TryParse(nudYear.Value.ToString(), out iYear);
            int.TryParse(nudMonthStart.Value.ToString(), out iMonthStart);
            int.TryParse(nudMonthEnd.Value.ToString(), out iMonthEnd);

            if (iMonthStart > iMonthEnd)
            {
                return;
            }

            V_PerExecutionList ent = dgPerQueryResult.SelectedItems[0] as V_PerExecutionList;

            decimal dMoney = 0;
            string strTitle = string.Empty, strMsg = string.Empty;
            switch (iBudgetRecordType)
            {
                case 3:
                    dMoney = ent.ApprovedApplyMoney;
                    strTitle = "已审核报销费用总览";
                    break;
                case 4:
                    dMoney = ent.ApprovingApplyMoney;
                    strTitle = "审核中报销费用总览";
                    break;
                case -1:
                    iBudgetRecordType = 3;
                    dMoney = ent.JanApprovedMoney;
                    iMonthStart = 1;
                    iMonthEnd = 1;
                    strTitle = "一月已审核报销费用总览";
                    break;
                case -2:
                    iBudgetRecordType = 3;
                    dMoney = ent.FebApprovedMoney;
                    iMonthStart = 2;
                    iMonthEnd = 2;
                    strTitle = "二月已审核报销费用总览";
                    break;
                case -3:
                    iBudgetRecordType = 3;
                    dMoney = ent.MarApprovedMoney;
                    iMonthStart = 3;
                    iMonthEnd = 3;
                    strTitle = "三月已审核报销费用总览";
                    break;
                case -4:
                    iBudgetRecordType = 3;
                    dMoney = ent.AprApprovedMoney;
                    iMonthStart = 4;
                    iMonthEnd = 4;
                    strTitle = "四月已审核报销费用总览";
                    break;
                case -5:
                    iBudgetRecordType = 3;
                    dMoney = ent.MayApprovedMoney;
                    iMonthStart = 5;
                    iMonthEnd = 5;
                    strTitle = "五月已审核报销费用总览";
                    break;
                case -6:
                    iBudgetRecordType = 3;
                    dMoney = ent.JunApprovedMoney;
                    iMonthStart = 6;
                    iMonthEnd = 6;
                    strTitle = "六月已审核报销费用总览";
                    break;
                case -7:
                    iBudgetRecordType = 3;
                    dMoney = ent.JulApprovedMoney;
                    iMonthStart = 7;
                    iMonthEnd = 7;
                    strTitle = "七月已审核报销费用总览";
                    break;
                case -8:
                    iBudgetRecordType = 3;
                    dMoney = ent.AugApprovedMoney;
                    iMonthStart = 8;
                    iMonthEnd = 8;
                    strTitle = "八月已审核报销费用总览";
                    break;
                case -9:
                    iBudgetRecordType = 3;
                    dMoney = ent.SepApprovedMoney;
                    iMonthStart = 9;
                    iMonthEnd = 9;
                    strTitle = "九月已审核报销费用总览";
                    break;
                case -10:
                    iBudgetRecordType = 3;
                    dMoney = ent.OctApprovedMoney;
                    iMonthStart = 10;
                    iMonthEnd = 10;
                    strTitle = "十月已审核报销费用总览";
                    break;
                case -11:
                    iBudgetRecordType = 3;
                    dMoney = ent.NovApprovedMoney;
                    iMonthStart = 11;
                    iMonthEnd = 11;
                    strTitle = "十一月已审核报销费用总览";
                    break;
                case -12:
                    iBudgetRecordType = 3;
                    dMoney = ent.DecApprovedMoney;
                    iMonthStart = 12;
                    iMonthEnd = 12;
                    strTitle = "十二月已审核报销费用总览";
                    break;
            }

            if (dMoney == 0)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, strTitle, "当前选中项无单据");
                return;
            }

            RecordList viewRd = new RecordList(FormTypes.Browse, iBudgetRecordType, iYear, iMonthStart, iMonthEnd, ent);

            EntityBrowser entBrowser = new EntityBrowser(viewRd);

            entBrowser.FormType = FormTypes.Browse;
            entBrowser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
            entBrowser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
        }
        #endregion 私有方法

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        /// <summary>
        /// 当前页加载完毕后执行的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AcountsView_Loaded(object sender, RoutedEventArgs e)
        {
            loadbar.Stop();
           // BindData();
        }

        /// <summary>
        /// 加载公司及部门执行一览查询结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_GetExecutionListByPagingCompleted(object sender, GetExecutionListByPagingCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                //int analyseMonth = 3;
                //int startColumnIndex = 5;
                //for (int i = 0; i < analyseMonth; i++)
                //{
                //    DataGridTemplateColumn dt = new DataGridTemplateColumn();
                //    System.Windows.Style syApply = new System.Windows.Style();
                //    syApply.
                //        //dt.HeaderStyle = 
                //    dt.Header = i.ToString() + "月";
                //    dt.Binding = new System.Windows.Data.Binding() { Path = new PropertyPath("MonthChargeList[0].ChargeMoney") };

                //    dgQueryResult.Columns.Insert(startColumnIndex, dt);
                //    startColumnIndex++;
                //}

                ObservableCollection<V_ExecutionList> entResList = e.Result;

                dgQueryResult.ItemsSource = entResList;
                dataPager.PageCount = e.pageCount;
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
            }
        }

        /// <summary>
        /// 加载岗位及个人执行一览查询结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_GetPerExecutionListByPagingCompleted(object sender, GetPerExecutionListByPagingCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                //int analyseMonth = 3;
                //int startColumnIndex = 5;
                //for (int i = 0; i < analyseMonth; i++)
                //{
                //    DataGridTemplateColumn dt = new DataGridTemplateColumn();
                //    System.Windows.Style syApply = new System.Windows.Style();
                //    syApply.
                //        //dt.HeaderStyle = 
                //    dt.Header = i.ToString() + "月";
                //    dt.Binding = new System.Windows.Data.Binding() { Path = new PropertyPath("MonthChargeList[0].ChargeMoney") };

                //    dgQueryResult.Columns.Insert(startColumnIndex, dt);
                //    startColumnIndex++;
                //}

                ObservableCollection<V_PerExecutionList> entResList = new ObservableCollection<V_PerExecutionList>();
                if (e.Result != null)
                {
                    entResList = e.Result;
                }

                dgPerQueryResult.ItemsSource = entResList;
                dataPager.PageCount = e.pageCount;
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
            }
        }

        /// <summary>
        /// 部门的执行一览(统计)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_OutFileExecutionListCompleted(object sender, OutFileExecutionListCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                byte[] byExport = e.Result;

                if (this.stream != null)
                {
                    this.stream.Write(byExport, 0, byExport.Length);
                    this.stream.Close();
                    this.stream.Dispose();
                    this.stream = null;
                }
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESS"), "导出部门的执行一览(统计)数据完毕！");
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "导出部门的执行一览(统计)数据失败！" + e.Error.Message);
            }
        }

        /// <summary>
        /// 个人的执行一览(统计)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_OutFilePerExecutionListCompleted(object sender, OutFilePerExecutionListCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                byte[] byExport = e.Result;

                if (this.stream != null)
                {
                    this.stream.Write(byExport, 0, byExport.Length);
                    this.stream.Close();
                    this.stream.Dispose();
                    this.stream = null;
                }
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESS"), "导出个人的执行一览(统计)数据完毕！");
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "导出个人的执行一览(统计)数据失败！" + e.Error.Message);
            }
        }

        /// <summary>
        /// 部门的执行一览(流水)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_OutFileDeptDayBookDataCompleted(object sender, OutFileDeptDayBookDataCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                byte[] byExport = e.Result;

                if (this.stream != null)
                {
                    this.stream.Write(byExport, 0, byExport.Length);
                    this.stream.Close();
                    this.stream.Dispose();
                    this.stream = null;
                }
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESS"), "导出部门的执行一览(流水)数据完毕！");
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "导出部门的执行一览(流水)数据失败！" + e.Error.Message);
            }
        }

        /// <summary>
        /// 个人的执行一览(流水)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_OutFilePerDayBookDataCompleted(object sender, OutFilePerDayBookDataCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                byte[] byExport = e.Result;

                if (this.stream != null)
                {
                    this.stream.Write(byExport, 0, byExport.Length);
                    this.stream.Close();
                    this.stream.Dispose();
                    this.stream = null;
                }
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESS"), "导出个人的执行一览(流水)数据完毕！");
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "导出个人的执行一览(流水)数据失败！" + e.Error.Message);
            }
        }

        /// <summary>
        /// 组织架构选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkOrg_FindClick(object sender, EventArgs e)
        {
            OrganizationLookup lookup = new OrganizationLookup();
            lookup.MultiSelected = true;
            lookup.SelectedObjType = OrgTreeItemTypes.All;
            lookup.ShowMessageForSelectOrganization();
            
            lookup.SelectedClick += (obj, ev) =>
            {
                List<ExtOrgObj> entList = lookup.SelectedObj as List<ExtOrgObj>;

                var se = new OrgSelectEntity(entList);
                
                lkOrg.DataContext = se;
                lkOrg.DisplayMemberPath = "Text";
                var iTempOrgType = (int)se.SelectedOrgType;
                if (iTempOrgType == 0 || iTempOrgType == 1)
                {
                    dgQueryResult.Visibility = System.Windows.Visibility.Visible;
                    dgPerQueryResult.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (iTempOrgType == 2 || iTempOrgType == 3)
                {
                    dgQueryResult.Visibility = System.Windows.Visibility.Collapsed;
                    dgPerQueryResult.Visibility = System.Windows.Visibility.Visible;

                }
                else
                {
                    dgQueryResult.Visibility = System.Windows.Visibility.Visible;
                    dgPerQueryResult.Visibility = System.Windows.Visibility.Collapsed;
                }

                lkSubject.DataContext = null;
                if (dataPager.PageIndex != 1)
                {
                    dataPager.PageIndex = 1;
                }
            };
            lookup.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuerySubmit_Click(object sender, RoutedEventArgs e)
        {
            dataPager.PageIndex = 1;
            BindData();
        }

        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 显示当前机构，科目当前查询时间段内对应的年预算明细记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hbnBudgetMoneyYear_Click(object sender, RoutedEventArgs e)
        {
            int iBudgetRecordType = 1;
            ShowRecordDetails(iBudgetRecordType);
        }

        /// <summary>
        /// 显示当前机构，科目当前查询时间段内对应的月预算明细记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hbBudgetMoneyMonth_Click(object sender, RoutedEventArgs e)
        {
            int iBudgetRecordType = 2;
            ShowRecordDetails(iBudgetRecordType);
        }

        /// <summary>
        /// 显示当前机构，科目当前查询时间段内对应的审核通过的报销明细记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hbApprovedApplyMoney_Click(object sender, RoutedEventArgs e)
        {
            int iBudgetRecordType = 3;
            HyperlinkButton hbn = sender as HyperlinkButton;
            if (hbn.Tag != null)
            {
                string strMonthFlag = hbn.Tag.ToString();
                int.TryParse(strMonthFlag, out iBudgetRecordType);
                if (iBudgetRecordType > 0 && iBudgetRecordType != 3)
                {
                    iBudgetRecordType = 0;
                }

                if (iBudgetRecordType < -12)
                {
                    iBudgetRecordType = 0;
                }
            }

            if (dgQueryResult.Visibility == System.Windows.Visibility.Visible)
            {
                ShowRecordDetails(iBudgetRecordType);
            }

            if (dgPerQueryResult.Visibility == System.Windows.Visibility.Visible)
            {
                ShowPerRecordDetails(iBudgetRecordType);
            }
        }

        /// <summary>
        /// 显示当前机构，科目当前查询时间段内对应的审核中的报销明细记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hbApprovingApplyMoney_Click(object sender, RoutedEventArgs e)
        {
            int iBudgetRecordType = 4;

            if (dgQueryResult.Visibility == System.Windows.Visibility.Visible)
            {
                ShowRecordDetails(iBudgetRecordType);
            }

            if (dgPerQueryResult.Visibility == System.Windows.Visibility.Visible)
            {
                ShowPerRecordDetails(iBudgetRecordType);
            }
        }

        /// <summary>
        /// 查询条件重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lkOrg.DataContext = null;
            lkSubject.DataContext = null;
            lkSubject.TxtLookUp.Text = string.Empty;
            strSearchSubjects = string.Empty;
            nudYear.Value = DateTime.Now.Year;
            nudMonthStart.Value = 1;
            nudMonthEnd.Value = DateTime.Now.Month;
        }

        /// <summary>
        /// 选择预算科目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkSubject_FindClick(object sender, EventArgs e)
        {
            string filter = string.Empty;
            ObservableCollection<object> paras = new ObservableCollection<object>();
            string strOrgType = string.Empty;
            string strOrgID = string.Empty;
            OrgSelectEntity ose = lkOrg.DataContext as OrgSelectEntity;
            if (ose == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("WARNING"), "请选择机构后再进行查询");
                return;
            }
            else if (ose.IsMultipleType)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("WARNING"), "请选择相同类型的机构,如全是公司或全是人员");
                return;
            }
         
            ose.GetSqlValue(ref filter, ref paras);
            var intOrgType = (int)ose.SelectedOrgType;
            

            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("SUBJECTCODE", "SUBJECTCODE");
            cols.Add("SUBJECTNAME", "SUBJECTNAME");

            
            if (intOrgType == 3)
            {
                paras.Clear();
                List<string> filters = new List<string>();
                foreach (var item in ose.SelectedValue)
                {
                    var entEmp = item.ObjectInstance as SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE;
                    var temp =  string.Format("(OWNERPOSTID=@{0})", paras.Count );
                    paras.Add(entEmp.OWNERPOSTID);
                    filters.Add(temp);
                }
                filter = "( " + string.Join(" or ", filters)+ " )";
            }
            
            LookupForm lookup = new LookupForm(FBAEnumsBLLPrefixNames.Execution,
                typeof(T_FB_SUBJECT[]), cols, "T_FB_BUDGETCHECK", filter, paras);

            lookup.SelectedClick += (o, ev) =>
            {
                if (lookup.SelectedObj == null)
                {
                    return;
                }

                ObservableCollection<object> entList = lookup.SelectedObj as ObservableCollection<object>;

                StringBuilder strIDlist = new StringBuilder();
                StringBuilder strNamelist = new StringBuilder();

                foreach (var obj in entList)
                {
                    T_FB_SUBJECT ent = obj as T_FB_SUBJECT;

                    strIDlist.Append(ent.SUBJECTID + ";");
                    strNamelist.Append(ent.SUBJECTNAME + ";");
                }

                strSearchSubjects = strIDlist.ToString();
                lkSubject.TxtLookUp.Text = strNamelist.ToString();
            };

            lookup.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 导出流水
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOutDayBook_Click(object sender, RoutedEventArgs e)
        {
            string filter = string.Empty;
            ObservableCollection<object> paras = new ObservableCollection<object>();
            string strOrgType = string.Empty;
            string strOrgID = string.Empty;
            int iYear = 0, iMonthStart = 0, iMonthEnd = 0;
            var isOK = GetFilter(ref filter, ref paras, out iYear, out iMonthStart, out iMonthEnd);
            if (!isOK)
            {
                return;
            }
            OrgSelectEntity ose = lkOrg.DataContext as OrgSelectEntity;

            if (ose.SelectedOrgType == OrgTreeItemTypes.Company || ose.SelectedValue.Count > 1)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("WARNING"), "导出流水,只支持选择一个部门或一个员工");
                return;
            }
   
            strOrgType = ose.SelectedOrgType.ToString();
            strOrgID = ose.SelectedValue.FirstOrDefault().ObjectID;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xls";
            dialog.Filter = "MS Excel Files|*.xls";
            dialog.FilterIndex = 1;

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                this.stream = dialog.OpenFile();
                loadbar.Start();

                if (dgQueryResult.Visibility == System.Windows.Visibility.Visible)
                {
                    clientFBA.OutFileDeptDayBookDataAsync(strOrgType, strOrgID, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, iYear, iMonthStart, iMonthEnd, filter, paras, "order by UPDATEDATE");
                }

                if (dgPerQueryResult.Visibility == System.Windows.Visibility.Visible)
                {
                    clientFBA.OutFilePerDayBookDataAsync(strOrgType, strOrgID, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, iYear, iMonthStart, iMonthEnd, filter, paras, "order by UPDATEDATE");
                }
            }
        }

        /// <summary>
        /// 导出统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOutExcel_Click(object sender, RoutedEventArgs e)
        {
            string filter = string.Empty;
            ObservableCollection<object> paras = new ObservableCollection<object>();
            string strOrgType = string.Empty;
            string strOrgID = string.Empty;
            int iYear = 0, iMonthStart = 0, iMonthEnd = 0;
            var isOK = GetFilter(ref filter, ref paras, out iYear, out iMonthStart, out iMonthEnd);
            if (!isOK)
            {
                return;
            }
            
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xls";
            dialog.Filter = "MS Excel Files|*.xls";
            dialog.FilterIndex = 1;

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                this.stream = dialog.OpenFile();
                loadbar.Start();

                if (dgQueryResult.Visibility == System.Windows.Visibility.Visible)
                {
                    clientFBA.OutFileExecutionListAsync(strOrgType, strOrgID, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, iYear, iMonthStart, iMonthEnd, filter, paras, "BUDGETCHECKDATE");
                }

                if (dgPerQueryResult.Visibility == System.Windows.Visibility.Visible)
                {
                    clientFBA.OutFilePerExecutionListAsync(strOrgType, strOrgID, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, iYear, iMonthStart, iMonthEnd, filter, paras, "BUDGETCHECKDATE");
                }
            }
        }

        /// <summary>
        /// 显示/隐藏月份报销
        /// </summary>
        /// <param name="dgShow"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="iAddStart"></param>
        private void IsHideMonthsDataGridColumns(DataGrid dgShow, int iMonthStart, int iMonthEnd, int iAddStart)
        {
            for (int i = 0; i < 12; i++)
            {
                int n = i + 1;
                int j = i + iAddStart;
                if (n >= iMonthStart && n <= iMonthEnd)
                {
                    dgShow.Columns[j].Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    dgShow.Columns[j].Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 显示/隐藏年度预算结余和月度预算结余
        /// </summary>
        /// <param name="dgShow"></param>
        /// <param name="ColumnStartIndex"></param>
        /// <param name="length"></param>
        /// <param name="visibility"></param>
        private void IsHideDataGridColumns(DataGrid dgShow, int ColumnStartIndex, int length, System.Windows.Visibility visibility)
        {
            for (int i = 0; i < length; i++)
            {
                int j = ColumnStartIndex + i;
                dgShow.Columns[j].Visibility = visibility;
            }
        }

        private bool GetFilter(ref string filter, ref ObservableCollection<object> paras, out int iYear, out int iMonthStart, out int iMonthEnd )
        {
            int.TryParse(nudYear.Value.ToString(), out iYear);
            int.TryParse(nudMonthStart.Value.ToString(), out iMonthStart);
            int.TryParse(nudMonthEnd.Value.ToString(), out iMonthEnd);

            OrgSelectEntity ose = lkOrg.DataContext as OrgSelectEntity;
            if (ose == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("WARNING"), "请选择机构后再进行查询");
                return false;
            }
            else if (ose.IsMultipleType)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("WARNING"), "请选择相同类型的机构,如全是公司或全是人员");
                return false;
            }
            else if (iMonthStart > iMonthEnd)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("WARNING"), "请输入正确的起止月份！");
                return false;
            }
            
            
            ose.GetSqlValue(ref filter, ref paras);
            if (!string.IsNullOrWhiteSpace(strSearchSubjects))
            {
                GetSearchConditionBySubjectID(ref filter, ref paras);
            }
            return true;
        }
    }

    
}
