//借还款往来
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
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace SMT.FBAnalysis.UI.Views
{
    public partial class ContactDetails : BasePage
    {
        FBAnalysisServiceClient clientFBA = new FBAnalysisServiceClient();
        private SMTLoading loadbar = new SMTLoading();
        private string strOrgType { get; set; }
        private string strOrgId { get; set; }
        Stream stream;

        /// <summary>
        /// 初始化
        /// </summary>
        public ContactDetails()
        {
            CheckConverter();
            InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                RegisterEvents();
                ContactDetails_Loaded(sender, args);
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
            dpStart.Text = DateTime.Now.Year.ToString() + "-1-1";
            dpEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
            ShowToolBarItem();

            clientFBA.GetContactDetailListByPagingCompleted += new EventHandler<GetContactDetailListByPagingCompletedEventArgs>(clientFBA_GetContactDetailListByPagingCompleted);
            clientFBA.GetDeptContactDetailListByPagingCompleted += new EventHandler<GetDeptContactDetailListByPagingCompletedEventArgs>(clientFBA_GetDeptContactDetailListByPagingCompleted);
            clientFBA.OutFileDeptContactDetailListCompleted += new EventHandler<OutFileDeptContactDetailListCompletedEventArgs>(clientFBA_OutFileDeptContactDetailListCompleted);
            clientFBA.OutFileContactDetailListCompleted += new EventHandler<OutFileContactDetailListCompletedEventArgs>(clientFBA_OutFileContactDetailListCompleted);
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
            decimal dBeforeAccount = 0, dAfterAccount = 0;
            decimal dNormalMoney = 0, dSpecialMoney = 0, dReserveMoney = 0;
            string filter = string.Empty;
            ObservableCollection<object> paras = new ObservableCollection<object>();

            if (lkObject.DataContext == null)
            {
                loadbar.Stop();
                return;
            }

            DateTime dtStart = new DateTime(), dtEnd = new DateTime();
            DateTime.TryParse(dpStart.Text, out dtStart);
            DateTime.TryParse(dpEnd.Text, out dtEnd);

            dtEnd = dtEnd.AddDays(1).AddSeconds(-1);

            if (dtStart > dtEnd)
            {
                loadbar.Stop();
                return;
            }

            if (dgDeptQueryResult.Visibility == System.Windows.Visibility.Visible)
            {
                clientFBA.GetDeptContactDetailListByPagingAsync(strOrgType, strOrgId, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, dtStart, dtEnd, filter, paras, "UPDATEDATE", dataPager.PageIndex, dataPager.PageSize, pageCount, dNormalMoney, dSpecialMoney, dReserveMoney);
            }

            if (dgPersQueryResult.Visibility == System.Windows.Visibility.Visible)
            {
                clientFBA.GetContactDetailListByPagingAsync(strOrgType, strOrgId, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, dtStart, dtEnd, filter, paras, "UPDATEDATE", dataPager.PageIndex, dataPager.PageSize, pageCount, dBeforeAccount, dAfterAccount);
            }
        }

        #endregion 私有方法

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ContactDetails_Loaded(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 选择机构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lkObject_FindClick(object sender, EventArgs e)
        {
            string userID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            string perm = (((int)FormTypes.Browse) + 1).ToString();    //zl 3.1
            string entity = "ContactDetailsView";
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
                        lkObject.DataContext = (SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY)objSel.ObjectInstance;
                        lkObject.DisplayMemberPath = "CNAME";
                        strOrgType = "COMPANY";
                        strOrgId = objSel.ObjectID;
                        dpStart.Text = "1900-1-1";
                        iTempOrgType = OrgTreeItemTypes.Company.ToInt32();
                        break;
                    case OrgTreeItemTypes.Department:
                        lkObject.DataContext = (SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT)objSel.ObjectInstance;
                        lkObject.DisplayMemberPath = "T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME";
                        strOrgType = "DEPARTMENT";
                        strOrgId = objSel.ObjectID;
                        dpStart.Text = "1900-1-1";
                        iTempOrgType = OrgTreeItemTypes.Department.ToInt32();
                        break;
                    case OrgTreeItemTypes.Post:
                        lkObject.DataContext = (SMT.Saas.Tools.OrganizationWS.T_HR_POST)objSel.ObjectInstance;
                        lkObject.DisplayMemberPath = "T_HR_POSTDICTIONARY.POSTNAME";
                        strOrgType = "POST";
                        strOrgId = objSel.ObjectID;
                        dpStart.Text = DateTime.Now.Year.ToString() + "-1-1";
                        iTempOrgType = OrgTreeItemTypes.Post.ToInt32();
                        break;
                    case OrgTreeItemTypes.Personnel:
                        lkObject.DataContext = (SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE)objSel.ObjectInstance;
                        lkObject.DisplayMemberPath = "EMPLOYEECNAME";
                        strOrgType = "PERSONAL";
                        strOrgId = objSel.ObjectID;
                        dpStart.Text = DateTime.Now.Year.ToString() + "-1-1";
                        iTempOrgType = OrgTreeItemTypes.Personnel.ToInt32();
                        break;
                }

                if (lkObject.DataContext != null)
                {
                    if (iTempOrgType == 0 || iTempOrgType == 1)
                    {
                        dgDeptQueryResult.Visibility = System.Windows.Visibility.Visible;
                        spDeptTotalBorrowMoney.Visibility = System.Windows.Visibility.Visible;

                        tbStartTitle.Visibility = System.Windows.Visibility.Collapsed;
                        dpStart.Visibility = System.Windows.Visibility.Collapsed;
                        tbEndTitle.Visibility = System.Windows.Visibility.Collapsed;
                        dgPersQueryResult.Visibility = System.Windows.Visibility.Collapsed;
                        spPersBeforeAccount.Visibility = System.Windows.Visibility.Collapsed;
                        spPersAfterAccount.Visibility = System.Windows.Visibility.Collapsed;
                    }
                    else if (iTempOrgType == 2 || iTempOrgType == 3)
                    {
                        dgDeptQueryResult.Visibility = System.Windows.Visibility.Collapsed;
                        spDeptTotalBorrowMoney.Visibility = System.Windows.Visibility.Collapsed;

                        tbStartTitle.Visibility = System.Windows.Visibility.Visible;
                        dpStart.Visibility = System.Windows.Visibility.Visible;
                        tbEndTitle.Visibility = System.Windows.Visibility.Visible;
                        dgPersQueryResult.Visibility = System.Windows.Visibility.Visible;
                        dgPersQueryResult.Visibility = System.Windows.Visibility.Visible;
                        spPersBeforeAccount.Visibility = System.Windows.Visibility.Visible;
                        spPersAfterAccount.Visibility = System.Windows.Visibility.Visible;

                    }
                    else
                    {
                        dgDeptQueryResult.Visibility = System.Windows.Visibility.Visible;
                        spDeptTotalBorrowMoney.Visibility = System.Windows.Visibility.Visible;

                        tbStartTitle.Visibility = System.Windows.Visibility.Collapsed;
                        dpStart.Visibility = System.Windows.Visibility.Collapsed;
                        tbEndTitle.Visibility = System.Windows.Visibility.Collapsed;
                        dgPersQueryResult.Visibility = System.Windows.Visibility.Collapsed;
                        spPersBeforeAccount.Visibility = System.Windows.Visibility.Collapsed;
                        spPersAfterAccount.Visibility = System.Windows.Visibility.Collapsed;
                    }

                    if (dataPager.PageIndex != 1)
                    {
                        dataPager.PageIndex = 1;
                    }
                }
            };
            ogzLookup.Show<string>(DialogMode.ApplicationModal, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, userID);
        }

        /// <summary>
        /// 查询翻页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 浏览单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                string strPageParameter = temp.Elements("PageParameter").SingleOrDefault().Value.Trim();
                string strApplicationOrder = temp.Elements("ApplicationOrder").SingleOrDefault().Value.Trim();

                CommonTools.ShowFBForm(strAssemblyName, strPageParameter, strApplicationOrder);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lkObject.DataContext = null;
            dpStart.Text = DateTime.Now.ToString("yyyy-MM") + "-1";
            dpEnd.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        void btnOutExcel_Click(object sender, RoutedEventArgs e)
        {
            string strFileType = "Excel";
            decimal dBeforeAccount = 0, dAfterAccount = 0;
            decimal dNormalMoney = 0, dSpecialMoney = 0, dReserveMoney = 0;
            string filter = string.Empty;
            ObservableCollection<object> paras = new ObservableCollection<object>();

            if (lkObject.DataContext == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("CAUTION"), "请选择导出的机构或员工！");
                return;
            }

            DateTime dtStart = new DateTime(), dtEnd = new DateTime();
            DateTime.TryParse(dpStart.Text, out dtStart);
            DateTime.TryParse(dpEnd.Text, out dtEnd);

            dtEnd = dtEnd.AddDays(1).AddSeconds(-1);
            if (dtStart > dtEnd)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("CAUTION"), "请注意起止时间填写顺序！");
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
                if (dgDeptQueryResult.Visibility == System.Windows.Visibility.Visible)
                {
                    clientFBA.OutFileDeptContactDetailListAsync(strFileType, strOrgType, strOrgId, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, dtStart, dtEnd, filter, paras, "UPDATEDATE", dNormalMoney, dSpecialMoney, dReserveMoney);
                }

                if (dgPersQueryResult.Visibility == System.Windows.Visibility.Visible)
                {
                    clientFBA.OutFileContactDetailListAsync(strFileType, strOrgType, strOrgId, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, dtStart, dtEnd, filter, paras, "UPDATEDATE", dBeforeAccount, dAfterAccount);
                }                
            }
        }

        void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            BindData();
        }

        /// <summary>
        /// 绑定返回的查询结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_GetContactDetailListByPagingCompleted(object sender, GetContactDetailListByPagingCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                ObservableCollection<V_ContactDetail> entList = new ObservableCollection<V_ContactDetail>();
                if (e.Result != null)
                {
                    entList = e.Result;
                }

                dgPersQueryResult.ItemsSource = entList;
                dataPager.PageCount = e.pageCount;
                tbTimeBeforeAccount.Text = e.dBeforeAccount.ToString();
                tbTimeAfterAccount.Text = e.dAfterAccount.ToString();
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "读取数据失败！" + e.Error.Message);
            }
        }

        /// <summary>
        /// 获取部门下人员借款情况
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_GetDeptContactDetailListByPagingCompleted(object sender, GetDeptContactDetailListByPagingCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                ObservableCollection<V_DeptContactDetail> entList = new ObservableCollection<V_DeptContactDetail>();
                if (e.Result != null)
                {
                    entList = e.Result;
                }

                dgDeptQueryResult.ItemsSource = entList;
                dataPager.PageCount = e.pageCount;
                tbNormalMoney.Text = e.dTotalNormalBorrowMoney.ToString("N");
                tbSpecialMoney.Text = e.dTotalSpecialMoney.ToString("N");
                tbReserveMoney.Text = e.dTotalReserveMoney.ToString("N");
                tbTotalMoney.Text = (e.dTotalNormalBorrowMoney + e.dTotalReserveMoney + e.dTotalSpecialMoney).ToString("N");
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "读取数据失败！" + e.Error.Message);
            }
        }

        /// <summary>
        /// 导出部门借还款往来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_OutFileDeptContactDetailListCompleted(object sender, OutFileDeptContactDetailListCompletedEventArgs e)
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
                //成功加个提示
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESS"), "导出数据成功！");
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "导出数据失败！" + e.Error.Message);
            }
        }

        /// <summary>
        /// 导出个人借还款往来
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientFBA_OutFileContactDetailListCompleted(object sender, OutFileContactDetailListCompletedEventArgs e)
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
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESS"), "导出数据成功！");
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "导出数据失败！" + e.Error.Message);
            }
        }
    }
}
