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

using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI;
using SMT.FBAnalysis.ClientServices.FBAnalysisWS;


namespace SMT.FBAnalysis.UI.Views
{
    public partial class RecordList : UserControl, IEntityEditor
    {
        private FBAnalysisServiceClient clientFBA = new FBAnalysisServiceClient();
        public FormTypes FormType { set; get; }
        public int BudgetRecordType { get; set; }
        public int BudgetYear { get; set; }
        public int BudgetMonthStart { get; set; }
        public int BudgetMonthEnd { get; set; }
        public V_ExecutionList entExecution { set; get; }
        public V_PerExecutionList entPerExecution { set; get; }
        private List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        Stream stream;

        public RecordList(FormTypes formtype, int iBudgetRecordType, int iYear, int iMonthStart, int iMonthEnd, V_ExecutionList entRev)
        {
            CheckConverter();
            InitializeComponent();
            FormType = formtype;
            BudgetRecordType = iBudgetRecordType;
            BudgetYear = iYear;
            BudgetMonthStart = iMonthStart;
            BudgetMonthEnd = iMonthEnd;
            entExecution = entRev;
            InitPage();
        }

        public RecordList(FormTypes formtype, int iBudgetRecordType, int iYear, int iMonthStart, int iMonthEnd, V_PerExecutionList entRev)
        {
            CheckConverter();
            InitializeComponent();
            FormType = formtype;
            BudgetRecordType = iBudgetRecordType;
            BudgetYear = iYear;
            BudgetMonthStart = iMonthStart;
            BudgetMonthEnd = iMonthEnd;
            entPerExecution = entRev;
            InitPage();
        }


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

        private void OutExcelFile()
        {
            RefreshUI(RefreshedTypes.ShowProgressBar);
            string filter = string.Empty;
            System.Collections.ObjectModel.ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();

            switch (BudgetRecordType)
            {
                case 1:
                    SetFilterStringForBudgetYear(ref filter, ref paras);
                    break;
                case 2:
                    SetFilterStringForBudgetMonth(ref filter, ref paras);
                    break;
                case 3:
                    SetFilterStringForApply(ref filter, ref paras);
                    break;
                case 4:
                    SetFilterStringForApply(ref filter, ref paras);
                    break;
            }

        

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".xls";
            dialog.Filter = "MS Excel Files|*.xls";
            dialog.FilterIndex = 1;

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                this.stream = dialog.OpenFile();

                clientFBA.OutFileBudgetRecordListAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, BudgetRecordType, BudgetYear, BudgetMonthStart, BudgetMonthEnd, filter, paras, "UPDATEDATE");                
            }
        }

        private void InitPage()
        {
            if (entExecution == null && entPerExecution == null)
            {
                this.IsEnabled = false;
                return;
            }

            clientFBA.GetBudgetRecordListByPagingCompleted += new EventHandler<GetBudgetRecordListByPagingCompletedEventArgs>(clientFBA_GetBudgetRecordListByPagingCompleted);
            clientFBA.OutFileBudgetRecordListCompleted += new EventHandler<OutFileBudgetRecordListCompletedEventArgs>(clientFBA_OutFileBudgetRecordListCompleted);
            BindData();
        }        

        void clientFBA_GetBudgetRecordListByPagingCompleted(object sender, GetBudgetRecordListByPagingCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error == null)
            {
                ObservableCollection<V_BudgetRecord> entList = e.Result;

                if (entList == null)
                {
                    this.IsEnabled = false;
                    return;
                }

                dgQueryResult.ItemsSource = entList;
                dataPager.PageCount = e.pageCount;
            }
            else
            {
                CommonTools.ShowCustomMessage(MessageTypes.Error, CommonTools.GetResourceStr("ERROR"), CommonTools.GetResourceStr(e.Error.Message));
            }
        }

        void clientFBA_OutFileBudgetRecordListCompleted(object sender, OutFileBudgetRecordListCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
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
                CommonTools.ShowCustomMessage(MessageTypes.Message, CommonTools.GetResourceStr("SUCCESS"), "导出预算执行一览台帐明细列表数据完毕！");
            }
            else
            {
                CommonTools.ShowCustomMessage(MessageTypes.Error, CommonTools.GetResourceStr("ERROR"), "导出预算执行一览台帐明细列表数据失败！" + e.Error.Message);
            }
        }

        private void BindData()
        {
            RefreshUI(RefreshedTypes.ShowProgressBar);
            int pageCount = 0;
            string filter = string.Empty;
            System.Collections.ObjectModel.ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();

            switch (BudgetRecordType)
            {
                case 1:
                    SetFilterStringForBudgetYear(ref filter, ref paras);
                    break;
                case 2:
                    SetFilterStringForBudgetMonth(ref filter, ref paras);
                    break;
                case 3:
                    SetFilterStringForApply(ref filter, ref paras);
                    break;
                case 4:
                    SetFilterStringForApply(ref filter, ref paras);
                    break;
            }

            clientFBA.GetBudgetRecordListByPagingAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, BudgetRecordType, BudgetYear, BudgetMonthStart, BudgetMonthEnd, filter, paras, "UPDATEDATE", dataPager.PageIndex, dataPager.PageSize, pageCount);
        }

        private void SetFilterStringForBudgetYear(ref string filter, ref ObservableCollection<object> paras)
        {
            if (entExecution == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(entExecution.OrganizationID))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "OWNERDEPARTMENTID==@" + paras.Count().ToString();
                paras.Add(entExecution.OrganizationID);
            }

            if (!string.IsNullOrWhiteSpace(entExecution.SubjectID))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "T_FB_SUBJECT.SUBJECTID==@" + paras.Count().ToString();
                paras.Add(entExecution.SubjectID);
            }
        }

        private void SetFilterStringForBudgetMonth(ref string filter, ref ObservableCollection<object> paras)
        {
            if (entExecution == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(entExecution.OrganizationID))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "OWNERDEPARTMENTID==@" + paras.Count().ToString();
                paras.Add(entExecution.OrganizationID);
            }

            if (!string.IsNullOrWhiteSpace(entExecution.SubjectID))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "T_FB_SUBJECT.SUBJECTID==@" + paras.Count().ToString();
                paras.Add(entExecution.SubjectID);
            }
        }

        private void SetFilterStringForApply(ref string filter, ref ObservableCollection<object> paras)
        {
            if (entExecution == null && entPerExecution == null)
            {
                return;
            }

            string strOrgId = string.Empty, strSubjectID = string.Empty;
            if (entExecution != null)
            {
                if (!string.IsNullOrWhiteSpace(entExecution.OrganizationID))
                {
                    strOrgId = entExecution.OrganizationID;

                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += " and ";
                    }
                    filter += "OWNERDEPARTMENTID==@" + paras.Count().ToString();
                    paras.Add(strOrgId);
                }

                if (!string.IsNullOrWhiteSpace(entExecution.SubjectID))
                {
                    strSubjectID = entExecution.SubjectID;
                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += " and ";
                    }
                    filter += "T_FB_SUBJECT.SUBJECTID==@" + paras.Count().ToString();
                    paras.Add(strSubjectID);
                }
            }

            if (entPerExecution != null)
            {
                if (!string.IsNullOrWhiteSpace(entPerExecution.OwnerID))
                {
                    strOrgId = entPerExecution.OwnerID;

                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += " and ";
                    }
                    filter += "OWNERID==@" + paras.Count().ToString();
                    paras.Add(strOrgId);
                }

                if (!string.IsNullOrWhiteSpace(entPerExecution.SubjectID))
                {
                    strSubjectID = entPerExecution.SubjectID;
                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += " and ";
                    }
                    filter += "T_FB_SUBJECT.SUBJECTID==@" + paras.Count().ToString();
                    paras.Add(strSubjectID);
                }
            }
        }

        private void hbnViewRd_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton hbnView = sender as HyperlinkButton;
            if (hbnView == null)
            {
                return;
            }

            if (hbnView.Tag == null)
            {
                return;
            }

            string strXmlObj = string.Empty;
            strXmlObj = hbnView.Tag.ToString();
            if (string.IsNullOrEmpty(strXmlObj))
            {
                return;
            }

            using (XmlReader reader = XmlReader.Create(new StringReader(strXmlObj)))
            {
                XElement xmlClient = XElement.Load(reader);
                var temp = from c in xmlClient.DescendantsAndSelf("System")
                           select c;

                string strAssemblyName = temp.Elements("AssemblyName").SingleOrDefault().Value.Trim();
                string PageParameter = temp.Elements("PageParameter").SingleOrDefault().Value.Trim();
                string ApplicationOrder = temp.Elements("ApplicationOrder").SingleOrDefault().Value.Trim();

                CommonTools.ShowFBForm(strAssemblyName, PageParameter, ApplicationOrder);
            }
        }

        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        #region IEntityEditor 成员

        public string GetTitle()
        {
            string strRes = string.Empty;
            switch (BudgetRecordType)
            {
                case 1:
                    strRes = "年度预算总览";
                    break;
                case 2:
                    strRes = "月度预算总览";
                    break;
                case 3:
                    strRes = "费用报销总览";
                    break;
                case 4:
                    strRes = "差旅报销总览";
                    break;
            }
            return strRes;
        }

        public string GetStatus()
        {
            string strTemp = string.Empty;

            return strTemp;
        }

        public void DoAction(string actionType)
        {
            switch (actionType)
            {
                case "0"://导出Excel                    
                    OutExcelFile();
                    break;
            }
        }

        private void RefreshUI(RefreshedTypes type)
        {
            if (OnUIRefreshed != null)
            {
                UIRefreshedEventArgs args = new UIRefreshedEventArgs();
                args.RefreshedType = type;
                OnUIRefreshed(this, args);
            }
        }

        public List<NavigateItem> GetLeftMenuItems()
        {
            List<NavigateItem> items = new List<NavigateItem>();

            return items;
        }

        public List<ToolbarItem> GetToolBarItems()
        {
            List<ToolbarItem> items = new List<ToolbarItem>();

            ToolbarItem item = new ToolbarItem
            {
                DisplayType = ToolbarItemDisplayTypes.Image,
                Key = "0",
                Title = SMT.SaaS.FrameworkUI.Common.Utility.GetResourceStr("EXPORTEXCEL"),
                ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_excel.png"

            };
            items.Add(item);
            return items;
        }

        public event UIRefreshedHandler OnUIRefreshed;

        #endregion
    }
}
