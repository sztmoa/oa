//月度预算分析
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
using System.Collections.ObjectModel;
using System.Text;
using System.IO;

using SMT.SaaS.FrameworkUI;
using SMT.SaaS.FrameworkUI.Common;
using SMT.SaaS.FrameworkUI.OrganizationControl;
using SMT.FBAnalysis.ClientServices.FBAnalysisWS;
using SMT.FBAnalysis.UI.Models;
using SMT.Saas.Tools.OrganizationWS;
using SMT.Saas.Tools.PermissionWS;

namespace SMT.FBAnalysis.UI.Views
{
    public partial class MonthlyBudgetAnalysis : BasePage
    {
        FBAnalysisServiceClient clientFBA = new FBAnalysisServiceClient();
        private SMTLoading loadbar = new SMTLoading();
        private string strOrgType { get; set; }
        private string strOrgId { get; set; }
        private string strSearchSubjects { get; set; }
        Stream stream;

        public MonthlyBudgetAnalysis()
        {
            CheckConverter();
            InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                RegisterEvents();
                MonthlyBudgetAnalysis_Loaded(sender, args);
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
            if (Application.Current.Resources["PercentConverter"] == null)
            {
                Application.Current.Resources.Add("PercentConverter", new SMT.FBAnalysis.UI.PercentConverter());
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
            nudYear.Value = DateTime.Now.Year.ToDouble();
            nudMonth.Value = DateTime.Now.Month;
            ShowToolBarItem();

            clientFBA.GetMonthlyBudgetAnalysisListByPagingCompleted += new EventHandler<GetMonthlyBudgetAnalysisListByPagingCompletedEventArgs>(clientFBA_GetMonthlyBudgetAnalysisListByPagingCompleted);
            clientFBA.OutFileMonthlyBudgetAnalysisListCompleted += new EventHandler<OutFileMonthlyBudgetAnalysisListCompletedEventArgs>(clientFBA_OutFileMonthlyBudgetAnalysisListCompleted);
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
            
            toolBarTop.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
            toolBarTop.btnOutExcel.Visibility = Visibility.Visible;
            toolBarTop.btnOutExcel.Click += new RoutedEventHandler(btnOutExcel_Click);
        }

        /// <summary>
        /// 组织查询条件
        /// </summary>
        private void BindData()
        {
            loadbar.Start();
            int pageCount = 0;
            string filter = string.Empty;
            ObservableCollection<object> paras = new ObservableCollection<object>();

            if (lkOrg.DataContext == null)
            {
                loadbar.Stop();
                return;
            }

            int iYear = 0, iMonth = 0;
            int.TryParse(nudYear.Value.ToString(), out iYear);
            int.TryParse(nudMonth.Value.ToString(), out iMonth);

            if (iYear == 0 || iMonth == 0)
            {
                loadbar.Stop();
                return;
            }

            if (lkOrg.DataContext != null)
            {
                GetSearchConditionByOrgObject(ref filter, ref paras);
            }

            if (!string.IsNullOrWhiteSpace(strSearchSubjects))
            {
                GetSearchConditionBySubjectID(ref filter, ref paras);
            }

            clientFBA.GetMonthlyBudgetAnalysisListByPagingAsync(strOrgType, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, iYear, iMonth, "SUBJECTID", filter, paras, dataPager.PageIndex, dataPager.PageSize, pageCount);

        }

        /// <summary>
        /// 获取机构对象查询条件
        /// </summary>
        /// <param name="strfilter"></param>
        /// <param name="paras"></param>
        private void GetSearchConditionByOrgObject(ref string strfilter, ref ObservableCollection<object> paras)
        {
            switch (strOrgType.ToUpper())
            {
                case "COMPANY":
                    strfilter += " OWNERCOMPANYID == @0";
                    paras.Add(strOrgId);
                    break;
                case "DEPARTMENT":
                    strfilter += " OWNERDEPARTMENTID == @0";
                    paras.Add(strOrgId);
                    break;
                case "POST":
                    strfilter += " OWNERPOSTID == @0";
                    paras.Add(strOrgId);
                    break;
                case "PERSONNAL":
                    SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE entEmp = lkOrg.DataContext as SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE;
                    if (entEmp != null)
                    {
                        strfilter += " OWNERID == @0";
                        paras.Add(entEmp.EMPLOYEEID);
                    }
                    break;
            }
        }

        /// <summary>
        /// 获取预算科目查询条件
        /// </summary>
        /// <param name="strfilter"></param>
        /// <param name="paras"></param>
        private void GetSearchConditionBySubjectID(ref string strfilter, ref ObservableCollection<object> paras)
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

                    strTemp.Append(" (SUBJECTID == @" + paras.Count().ToString() + ") ");
                    paras.Add(strlist[i]);
                }
            }


            if (!string.IsNullOrEmpty(strfilter))
            {
                strfilter += " and ";
            }

            strfilter += " (" + strTemp.ToString() + ")";
        }

        #endregion 私有方法

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void MonthlyBudgetAnalysis_Loaded(object sender, RoutedEventArgs args)
        {
            BindData();
        }

        void clientFBA_GetMonthlyBudgetAnalysisListByPagingCompleted(object sender, GetMonthlyBudgetAnalysisListByPagingCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                ObservableCollection<V_MonthlyBudgetAnalysis> entList = new ObservableCollection<V_MonthlyBudgetAnalysis>();
                if (e.Result != null)
                {
                    entList = e.Result;
                }

                dgQueryResult.ItemsSource = entList;
                dataPager.PageCount = e.pageCount;
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "读取数据失败！" + e.Error.Message);
            }
        }
        
        void clientFBA_OutFileMonthlyBudgetAnalysisListCompleted(object sender, OutFileMonthlyBudgetAnalysisListCompletedEventArgs e)
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
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESS"), "导出月度预算分析数据完毕！");
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "导出月度预算分析数据失败！" + e.Error.Message);
            }
        }

        private void lkOrg_FindClick(object sender, EventArgs e)
        {
            string userID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            string perm = (((int)FormTypes.Browse) + 1).ToString();   //zl 3.1
            string entity = "MonthlyBudgetAnalysis";
            OrganizationLookup ogzLookup = new OrganizationLookup(userID, perm, entity);
            ogzLookup.MultiSelected = false;
            ogzLookup.SelectedObjType = OrgTreeItemTypes.All;
            ogzLookup.ShowMessageForSelectOrganization();

            ogzLookup.SelectedClick += (o, ev) =>
            {
                ExtOrgObj objSel = ogzLookup.SelectedObj.FirstOrDefault();
                if (objSel.ObjectInstance == null)
                {
                    return;
                }

                int iTempOrgType = -1;
                switch (objSel.ObjectType)
                {
                    case OrgTreeItemTypes.Company:
                        lkOrg.DataContext = (SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY)objSel.ObjectInstance;
                        lkOrg.DisplayMemberPath = "CNAME";
                        strOrgType = "COMPANY";
                        strOrgId = objSel.ObjectID;
                        iTempOrgType = OrgTreeItemTypes.Company.ToInt32();
                        break;
                    case OrgTreeItemTypes.Department:
                        lkOrg.DataContext = (SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT)objSel.ObjectInstance;
                        lkOrg.DisplayMemberPath = "T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME";
                        strOrgType = "DEPARTMENT";
                        strOrgId = objSel.ObjectID;
                        iTempOrgType = OrgTreeItemTypes.Department.ToInt32();
                        break;
                    case OrgTreeItemTypes.Post:
                        lkOrg.DataContext = (SMT.Saas.Tools.OrganizationWS.T_HR_POST)objSel.ObjectInstance;
                        lkOrg.DisplayMemberPath = "T_HR_POSTDICTIONARY.POSTNAME";
                        strOrgType = "POST";
                        strOrgId = objSel.ObjectID;
                        iTempOrgType = OrgTreeItemTypes.Post.ToInt32();
                        break;
                    case OrgTreeItemTypes.Personnel:
                        lkOrg.DataContext = (SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE)objSel.ObjectInstance;
                        lkOrg.DisplayMemberPath = "EMPLOYEECNAME";
                        strOrgType = "PERSONNAL";
                        strOrgId = objSel.ObjectID;
                        iTempOrgType = OrgTreeItemTypes.Personnel.ToInt32();
                        break;
                }

                lkSubject.DataContext = null;
            };
            ogzLookup.Show<string>(DialogMode.ApplicationModal, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, userID);
        }

        private void lkSubject_FindClick(object sender, EventArgs e)
        {
            if (lkOrg.DataContext == null)
            {
                return;
            }

            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("SUBJECTCODE", "SUBJECTCODE");
            cols.Add("SUBJECTNAME", "SUBJECTNAME");

            StringBuilder strfilter = new StringBuilder();
            ObservableCollection<object> objArgs = new ObservableCollection<object>();

            switch (strOrgType.ToUpper())
            {
                case "COMPANY":
                    strfilter.Append(" OWNERCOMPANYID == @0");
                    break;
                case "DEPARTMENT":
                    strfilter.Append(" OWNERDEPARTMENTID == @0");
                    break;
                case "POST":
                    strfilter.Append(" OWNERPOSTID == @0");
                    break;
                case "PERSONNAL":
                    strfilter.Append(" OWNERPOSTID == @0");
                    break;
            }

            if (strOrgType.ToUpper() == "PERSONNAL" && lkOrg.DataContext != null)
            {
                SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE entEmp = lkOrg.DataContext as SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE;
                if (entEmp != null)
                {
                    strOrgId = entEmp.OWNERPOSTID;
                }
            }

            objArgs.Add(strOrgId);

            LookupForm lookup = new LookupForm(FBAEnumsBLLPrefixNames.Execution,
                typeof(T_FB_SUBJECT[]), cols, "MonthlyBudgetAnalysis", strfilter.ToString(), objArgs);

            lookup.SelectedClick += (o, ev) =>
            {
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

        private void btnQuerySubmit_Click(object sender, RoutedEventArgs e)
        {
            if (lkOrg.DataContext == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("WARNING"), "请选择机构后再进行查询");
                dgQueryResult.ItemsSource = null;
                return;
            }

            if (dataPager.PageIndex != 1)
            {
                dataPager.PageIndex = 1;
            }
            else
            {
                BindData();
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lkOrg.DataContext = null;
            lkSubject.DataContext = null;
            lkSubject.TxtLookUp.Text = string.Empty;
            strSearchSubjects = string.Empty;
            nudYear.Value = DateTime.Now.Year;
            nudMonth.Value = DateTime.Now.Month;
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
        /// 导出Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOutExcel_Click(object sender, RoutedEventArgs e)
        {
            string filter = string.Empty;
            ObservableCollection<object> paras = new ObservableCollection<object>();

            if (lkOrg.DataContext == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("CAUTION"), "请选择导出的机构或员工！");
                return;
            }

            int iYear = 0, iMonth = 0;
            int.TryParse(nudYear.Value.ToString(), out iYear);
            int.TryParse(nudMonth.Value.ToString(), out iMonth);

            if (iYear == 0 || iMonth == 0)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("CAUTION"), "请输入正确年份和月份！");
                return;
            }

            if (lkOrg.DataContext != null)
            {
                GetSearchConditionByOrgObject(ref filter, ref paras);
            }

            if (!string.IsNullOrWhiteSpace(strSearchSubjects))
            {
                GetSearchConditionBySubjectID(ref filter, ref paras);
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
                clientFBA.OutFileMonthlyBudgetAnalysisListAsync(strOrgType, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, iYear, iMonth, "SUBJECTID", filter, paras);
            }
        }

        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            BindData();
        }

    }
}
