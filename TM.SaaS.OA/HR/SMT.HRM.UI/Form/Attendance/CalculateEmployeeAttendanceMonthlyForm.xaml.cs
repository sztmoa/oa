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

using SMT.Saas.Tools.AttendanceWS;      //考勤接口
using SMT.Saas.Tools.OrganizationWS;    //公司组织接口
using SMT.SaaS.FrameworkUI;
using SMT.Saas.Tools.PermissionWS;
using System.Collections.ObjectModel;

namespace SMT.HRM.UI.Form.Attendance
{
    public partial class CalculateEmployeeAttendanceMonthlyForm : BaseForm, IEntityEditor
    {
        AttendanceServiceClient clientAtt;
        private SMT.Saas.Tools.OrganizationWS.OrganizationServiceClient clientOrg;
        private List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();

        public CalculateEmployeeAttendanceMonthlyForm()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(CalculateEmployeeAttendanceMonthlyForm_Loaded);            
        }

        void CalculateEmployeeAttendanceMonthlyForm_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterEvents();
            InitParas();
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            clientAtt = new AttendanceServiceClient();
            clientOrg = new SMT.Saas.Tools.OrganizationWS.OrganizationServiceClient();
            //clientAtt.CalculateEmployeeAttendanceMonthlyByPostIDCompleted +=clientAtt_CalculateEmployeeAttendanceMonthlyByPostIDCompleted;
            //clientAtt.CalculateEmployeeAttendanceMonthlyByCompanyIDCompleted += clientAtt_CalculateEmployeeAttendanceMonthlyByCompanyIDCompleted;
            //clientAtt.CalculateEmployeeAttendanceMonthlyByDepartmentIDCompleted +=clientAtt_CalculateEmployeeAttendanceMonthlyByDepartmentIDCompleted;
            clientAtt.CalculateAttendanceMonthlyCompleted += clientAtt_CalculateAttendanceMonthlyCompleted;
            //clientOrg.GetCompanyByIdCompleted += new EventHandler<SMT.Saas.Tools.OrganizationWS.GetCompanyByIdCompletedEventArgs>(clientOrg_GetCompanyByIdCompleted);
        }

       
      

        private void InitParas()
        {
            txtBalanceYear.Text = DateTime.Now.Year.ToString();
            txtBalanceMonth.Text = DateTime.Now.AddMonths(-1).Month.ToString();
            
        }        

        #region IEntityEditor 成员

        public string GetTitle()
        {
            return Utility.GetResourceStr("ATTENDMONTHLYBALANCE");
        }

        public string GetStatus()
        {
            return string.Empty;
        }

        public void DoAction(string actionType)
        {
            
        }

        public List<NavigateItem> GetLeftMenuItems()
        {
            List<NavigateItem> items = new List<NavigateItem>();
            return items;       
        }

        public List<ToolbarItem> GetToolBarItems()
        {
            return ToolbarItems;
        }

        public event UIRefreshedHandler OnUIRefreshed;

        public void RefreshUI(RefreshedTypes type)
        {
            if (OnUIRefreshed != null)
            {
                UIRefreshedEventArgs args = new UIRefreshedEventArgs();
                args.RefreshedType = type;
                OnUIRefreshed(this, args);
            }
        }

        #endregion

        /// <summary>
        /// 根据应用对象类型，对应用对象赋值（当前类型为公司）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientAtt_CalculateAttendanceMonthlyCompleted(object sender, CalculateAttendanceMonthlyCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {

                    string msg = e.Result;
                    if (!string.IsNullOrEmpty(msg))
                    {
                        txtMsg.Text = msg;
                    }
                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("MONTHLYBALANCESUCCESSED"));
                }
                else
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
                }

            }
            catch (Exception ex)
            {
                Utility.Log(ex.ToString());
            }
            finally
            {
                RefreshUI(RefreshedTypes.HideProgressBar);
            }
        }
        private void lkAssignObject_FindClick(object sender, EventArgs e)
        {
            if (cbxkAssignedObjectType.SelectedItem == null
                || cbxkAssignedObjectType.SelectedIndex==0)
            {
                MessageBox.Show("请选择结算类型！");
                return;
            }

            var entDic = cbxkAssignedObjectType.SelectedIndex.ToString();
  

            OrganizationLookupForm lookup = new OrganizationLookupForm();
            if (entDic== (Convert.ToInt32(AssignedObjectType.Company) + 1).ToString())
            {
                lookup.SelectedObjType = OrgTreeItemTypes.Company;
            }
            else if (entDic == (Convert.ToInt32(AssignedObjectType.Department) + 1).ToString())
            {
                lookup.SelectedObjType = OrgTreeItemTypes.Department;
            }
            else if (entDic == (Convert.ToInt32(AssignedObjectType.Post) + 1).ToString())
            {
                lookup.SelectedObjType = OrgTreeItemTypes.Post;
            }
            else if (entDic == "4")
            {
                lookup.SelectedObjType = OrgTreeItemTypes.Post;
            }

            lookup.SelectedClick += (obj, ev) =>
            {

                lkAssignObject.DataContext = lookup.SelectedObj;


                if (entDic == "4")
                {
                    T_HR_POST post = lkAssignObject.DataContext as T_HR_POST;
                    bool checkFlag = false;
                    foreach(var item in SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts)
                    {
                        if(item.PostID==post.POSTID)
                        {
                            checkFlag = true;
                        }
                    }
                    if(!checkFlag)
                    {
                        MessageBox.Show("你未在此岗位上任职，无法通过此岗位结算月度考勤！");
                        lkAssignObject.DataContext = null;
                    }
                }


                if (lookup.SelectedObj is T_HR_COMPANY)
                {
                    lkAssignObject.DisplayMemberPath = "CNAME";
                }
                else if (lookup.SelectedObj is T_HR_DEPARTMENT)
                {
                    lkAssignObject.DisplayMemberPath = "T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME";
                }
                else if (lookup.SelectedObj is T_HR_POST)
                {
                    lkAssignObject.DisplayMemberPath = "T_HR_POSTDICTIONARY.POSTNAME";
                }
            };

            lookup.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
        }

        /// <summary>
        /// 结算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBalanceCalculate_Click(object sender, RoutedEventArgs e)
        {

            if (cbxkAssignedObjectType.SelectedItem == null
                || cbxkAssignedObjectType.SelectedIndex == 0)
            {
                MessageBox.Show("请选择结算类型！");
                return;
            }

            if (lkAssignObject.DataContext == null)
            {
                MessageBox.Show("请选择结算对象");
                return;
            }

            bool flag = false;
            int iYear =0, iMonth = 0;
            flag = int.TryParse(txtBalanceYear.Text, out iYear);
            if (iYear <= 0)
            {
                return;
            }

            flag = int.TryParse(txtBalanceMonth.Text, out iMonth);
            if (iMonth <= 0 || iMonth > 12)
            {
                return;
            }

            var entDic = cbxkAssignedObjectType.SelectedIndex.ToString();

            RefreshUI(RefreshedTypes.ProgressBar);
            
           ObservableCollection<string> ClacuEmployeePosts=new ObservableCollection<string>();
            foreach(var item in SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts)
            {
                ClacuEmployeePosts.Add(item.PostID);
            }
            string strMsg = string.Empty;
            string strID = string.Empty;
            string ClacuType=string.Empty;

            if (entDic== (Convert.ToInt32(AssignedObjectType.Company) + 1).ToString())
            {
                T_HR_COMPANY entCompany = lkAssignObject.DataContext as T_HR_COMPANY;
                strID = entCompany.COMPANYID;
                ClacuType = "1";
            }
            else if (entDic== (Convert.ToInt32(AssignedObjectType.Department) + 1).ToString())
            {
                T_HR_DEPARTMENT entDepartment = lkAssignObject.DataContext as T_HR_DEPARTMENT;
                strID = entDepartment.DEPARTMENTID;
                ClacuType = "2";

            }
            else if (entDic== (Convert.ToInt32(AssignedObjectType.Post) + 1).ToString())
            {
                T_HR_POST entPost = lkAssignObject.DataContext as T_HR_POST;
                strID = entPost.POSTID;
                ClacuType = "3";
            }
            else if (entDic== "4")
            {
                T_HR_POST entPost = lkAssignObject.DataContext as T_HR_POST;
                strID = entPost.POSTID;
                ClacuType = "4";
            }
            string balanceEmployeeid = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;

            clientAtt.CalculateAttendanceMonthlyAsync(iYear.ToString() + "-" + iMonth.ToString(), ClacuType, strID, ClacuEmployeePosts, balanceEmployeeid, strMsg);
        }

        private void cbxkAssignedObjectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lkAssignObject.DataContext = null;
            var entDic = cbxkAssignedObjectType.SelectedIndex.ToString();
        }        
    }
}
