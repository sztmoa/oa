//台账查询
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

using SMT.SaaS.FrameworkUI;
using SMT.SaaS.FrameworkUI.Common;
using SMT.SaaS.FrameworkUI.OrganizationControl;
using SMT.FBAnalysis.ClientServices.FBAnalysisWS;
using SMT.FBAnalysis.UI.Models;
using SMT.Saas.Tools.OrganizationWS;
using SMT.Saas.Tools.PermissionWS;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace SMT.FBAnalysis.UI.Views
{
    public partial class StandingBook : BasePage
    {
        FBAnalysisServiceClient clientFBA = new FBAnalysisServiceClient();
        private SMTLoading loadbar = new SMTLoading();
        private string strOrgType { get; set; }
        private string strOrgId { get; set; }
        private string strSearchSubjects { get; set; }
        Stream stream;

        /// <summary>
        /// 初始化
        /// </summary>
        public StandingBook()
        {
            CheckConverter();
            InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                RegisterEvents();
                StandingBook_Loaded(sender, args);
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
            dpStart.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            dpEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            ShowToolBarItem();

            clientFBA.GetStandingBookListByPagingCompleted += new EventHandler<GetStandingBookListByPagingCompletedEventArgs>(clientFBA_GetStandingBookListByPagingCompleted);
            clientFBA.OutFileStandingBookListCompleted += new EventHandler<OutFileStandingBookListCompletedEventArgs>(clientFBA_OutFileStandingBookListCompleted);
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

            toolBarTop.btnOutExcel.Visibility = Visibility.Visible;
            toolBarTop.btnOutExcel.Click += new RoutedEventHandler(btnOutExcel_Click);
            toolBarTop.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
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

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            DateTime.TryParse(dpStart.Text, out dtStart);
            DateTime.TryParse(dpEnd.Text, out dtEnd);

            if (dtStart > dtEnd)
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

            if (dicOrderType.SelectedItem != null)
            {
                GetSearchConditionByOrderType(ref filter, ref paras);
            }

            if (dicOrderState.SelectedItem != null)
            {
                GetSearchConditionByOrderState(ref filter, ref paras);
            }

            clientFBA.GetStandingBookListByPagingAsync(strOrgType, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, dtStart, dtEnd.AddDays(1), "SUBJECTID", filter, paras, dataPager.PageIndex, dataPager.PageSize, pageCount);

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

        /// <summary>
        /// 获取单据类型查询条件
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="paras"></param>
        private void GetSearchConditionByOrderType(ref string strfilter, ref ObservableCollection<object> paras)
        {
            if (dicOrderType.SelectedItem == null)
            {
                return;
            }

            T_SYS_DICTIONARY entDic = dicOrderType.SelectedItem as T_SYS_DICTIONARY;
            if (string.IsNullOrEmpty(entDic.DICTIONARYID) || string.IsNullOrEmpty(entDic.DICTIONCATEGORY))
            {
                return;
            }

            if (entDic.DICTIONARYVALUE == null)
            {
                return;
            }

            int dDicValue = entDic.DICTIONARYVALUE.Value.ToInt32();
            switch (dDicValue)
            {
                case 1:
                    if (!string.IsNullOrEmpty(strfilter))
                    {
                        strfilter += " and ";
                    }
                    strfilter += " ORDERTYPENAME == @" + paras.Count().ToString();
                    paras.Add("年度部门预算");
                    break;
                case 2:
                    if (!string.IsNullOrEmpty(strfilter))
                    {
                        strfilter += " and ";
                    }
                    strfilter += " ORDERTYPENAME == @" + paras.Count().ToString();
                    paras.Add("年度部门预算增补");
                    break;
                case 3:
                    if (!string.IsNullOrEmpty(strfilter))
                    {
                        strfilter += " and ";
                    }
                    strfilter += " ORDERTYPENAME == @" + paras.Count().ToString();
                    paras.Add("月度部门预算");
                    break;
                case 4:
                    if (!string.IsNullOrEmpty(strfilter))
                    {
                        strfilter += " and ";
                    }
                    strfilter += " ORDERTYPENAME == @" + paras.Count().ToString();
                    paras.Add("月度部门增补");
                    break;
                case 5:
                    if (!string.IsNullOrEmpty(strfilter))
                    {
                        strfilter += " and ";
                    }
                    strfilter += " ORDERTYPENAME == @" + paras.Count().ToString();
                    paras.Add("报销单");
                    break;
            }
        }

        /// <summary>
        /// 获取单据状态查询条件
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="paras"></param>
        private void GetSearchConditionByOrderState(ref string strfilter, ref ObservableCollection<object> paras)
        {
            if (dicOrderState.SelectedItem == null)
            {
                return;
            }

            T_SYS_DICTIONARY entDic = dicOrderState.SelectedItem as T_SYS_DICTIONARY;
            if (string.IsNullOrEmpty(entDic.DICTIONARYID) || string.IsNullOrEmpty(entDic.DICTIONCATEGORY))
            {
                return;
            }

            if (entDic.DICTIONARYVALUE == null)
            {
                return;
            }

            int dDicValue = entDic.DICTIONARYVALUE.Value.ToInt32();
            switch (dDicValue)
            {
                case 1:
                    if (!string.IsNullOrEmpty(strfilter))
                    {
                        strfilter += " and ";
                    }
                    strfilter += " CHECKSTATES == cast(@" + paras.Count().ToString() + " as number (8,0))";
                    paras.Add("1");
                    break;
                case 2:
                    if (!string.IsNullOrEmpty(strfilter))
                    {
                        strfilter += " and ";
                    }
                    strfilter += " (CHECKSTATES == cast(@" + paras.Count().ToString() + " as number (8,0))";
                    paras.Add("2");
                    strfilter += " and CHECKSTATESNAME == @" + paras.Count().ToString() + ") ";
                    paras.Add("审核通过");
                    break;               
            }
        }

        #endregion 私有方法

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void StandingBook_Loaded(object sender, RoutedEventArgs args)
        {
            BindOrderType();
            BindOrderState();
            BindData();
        }

        private void BindOrderType()
        {
            Utility.CbxItemBinder(dicOrderType, "BUDGETORDERTYPE", "0");            
        }

        private void BindOrderState()
        {
            Utility.CbxItemBinder(dicOrderState, "ORDERSTATE", "0");
        }

        void clientFBA_GetStandingBookListByPagingCompleted(object sender, GetStandingBookListByPagingCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                ObservableCollection<V_StandingBook> entList = new ObservableCollection<V_StandingBook>();
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
        
        void clientFBA_OutFileStandingBookListCompleted(object sender, OutFileStandingBookListCompletedEventArgs e)
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
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESS"), "导出预算台帐列表数据完毕！");
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "导出预算台帐列表数据失败！" + e.Error.Message);
            }
        }

        /// <summary>
        /// 选择查询对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkOrg_FindClick(object sender, EventArgs e)
        {
            string userID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            string perm = (((int)FormTypes.Browse) + 1).ToString();    //zl 3.1
            string entity = "StandingBook";
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

        /// <summary>
        /// 选择查询的科目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                typeof(T_FB_SUBJECT[]), cols, "StandingBook", strfilter.ToString(), objArgs);

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

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 重置查询条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lkOrg.DataContext = null;
            lkSubject.DataContext = null;
            lkSubject.TxtLookUp.Text = string.Empty;
            strSearchSubjects = string.Empty;
            dpStart.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            dpEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dicOrderType.SelectedIndex = 0;
            dicOrderState.SelectedIndex = 0;
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

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();
            DateTime.TryParse(dpStart.Text, out dtStart);
            DateTime.TryParse(dpEnd.Text, out dtEnd);

            if (dtStart > dtEnd)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("CAUTION"), "请注意起止时间填写顺序！");
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

            if (dicOrderType.SelectedItem != null)
            {
                GetSearchConditionByOrderType(ref filter, ref paras);
            }

            if (dicOrderState.SelectedItem != null)
            {
                GetSearchConditionByOrderState(ref filter, ref paras);
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
                clientFBA.OutFileStandingBookListAsync(strOrgType, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, dtStart, dtEnd.AddDays(1), "SUBJECTID", filter, paras);
            }
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
        /// 显示单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hbnShowRecord_Click(object sender, RoutedEventArgs e)
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
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("WARNING"), "此单据禁止查看");
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
    }
}
