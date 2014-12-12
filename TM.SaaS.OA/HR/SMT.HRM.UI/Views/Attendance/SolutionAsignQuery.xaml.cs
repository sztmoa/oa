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
using System.Collections.ObjectModel;
using SMT.HRM.UI.Form.Attendance;
using SMT.SaaS.FrameworkUI;
using SMT.Saas.Tools.AttendanceWS;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.FrameworkUI.ChildWidow;
using System.IO;


namespace SMT.HRM.UI.Views.Attendance
{
    public partial class SolutionAsignQuery :   BasePage, IClient
    {
        

        #region 全局变量
        byte[] byExport;
        //是否点击了导出excel按钮
        bool clickOutExcel = false;
        AttendanceServiceClient clientAtt;
        private SMTLoading loadbar = new SMTLoading();
        #endregion

        #region 初始化
        public SolutionAsignQuery()
        {
            InitializeComponent();
            clientAtt = new AttendanceServiceClient();
            RegisterEvents();
            GetEntityLogo("T_HR_ATTENDANCESOLUTIONASIGN");

        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewTitles.TextTitle.Text = GetTitleFromURL(e.Uri.ToString());
        }

        
        #endregion

        #region 私有方法
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterEvents()
        {
            PARENT.Children.Add(loadbar);
            nuYear.Value = DateTime.Now.Year;
            //nuMonth.Value = DateTime.Now.Month;
            endYear.Value = DateTime.Now.Year;
            //设置为第1个月
            startMonth.Value = 1;
            //设置为12月
            nuMonth.Value = 12;
            toolbar1.btnNew.Visibility = System.Windows.Visibility.Collapsed;
            toolbar1.btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            toolbar1.btnRefresh.Click += new RoutedEventHandler(btnRefresh_Click);
            toolbar1.BtnView.Click += new RoutedEventHandler(BtnView_Click);                        
            toolbar1.btnDelete.Visibility = System.Windows.Visibility.Collapsed;
            toolbar1.btnAudit.Visibility = System.Windows.Visibility.Collapsed;
            toolbar1.btnReSubmit.Visibility = System.Windows.Visibility.Collapsed;
            toolbar1.cbxCheckState.Visibility = System.Windows.Visibility.Collapsed;
            toolbar1.txtCheckStateName.Visibility = Visibility.Collapsed;
            toolbar1.BtnView.Content = "查看方案";
            toolbar1.btnOutExcel.Visibility = Visibility.Visible;
            toolbar1.btnOutExcel.Click += new RoutedEventHandler(btnOutExcel_Click);
            clientAtt.GetAttendanceSolutionAsignBySearchNewCompleted += new EventHandler<GetAttendanceSolutionAsignBySearchNewCompletedEventArgs>(clientAtt_GetAttendanceSolutionAsignBySearchNewCompleted);            
            clientAtt.RemoveAttendanceSolutionAsignCompleted += new EventHandler<RemoveAttendanceSolutionAsignCompletedEventArgs>(clientAtt_RemoveAttendanceSolutionAsignCompleted);
            clientAtt.OutAttendanceSolutionByMultSearchCompleted += new EventHandler<OutAttendanceSolutionByMultSearchCompletedEventArgs>(clientAtt_OutAttendanceSolutionByMultSearchCompleted);
            treeOrganization.SelectedClick += new EventHandler(treeOrganization_SelectedClick);            
            this.Loaded += new RoutedEventHandler(AttendanceSolutionAsign_Loaded);
        }

        void clientAtt_OutAttendanceSolutionByMultSearchCompleted(object sender, OutAttendanceSolutionByMultSearchCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                if (!string.IsNullOrEmpty(e.strMsg))
                {                    
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.strMsg));
                    return;                    
                }                
                byExport = e.Result;               

            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "获取导出数据失败");
            }
        }


        /// <summary>
        /// 导出查询数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOutExcel_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("由于silverlight安全限制，请等待返回数据后再次点击导出Excel");
            //BindGrid("out");
            if (byExport == null)
            {
                MessageBox.Show("数据加载完成后请再次点击导出Excel按钮导出数据");
                clickOutExcel = true;
                BindGrid("out");
            }
            else
            {
                if (byExport.Count() > 0)
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "MS Excel Files|*.xls";
                    dialog.FilterIndex = 1;

                    bool? result = dialog.ShowDialog();
                    if (result == true)
                    {
                        try
                        {
                            using (Stream stream = dialog.OpenFile())
                            {
                                stream.Write(byExport, 0, byExport.Length);
                                System.Threading.Thread.Sleep(3000);//延迟1000毫秒
                                Utility.ShowCustomMessage(MessageTypes.Message, "导出成功", "考勤方案分配记录已导出，请查看");                                
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Message, "导出失败", "数据太多请根据条件进行过滤再导出数据");
                            return;
                        }                        
                    }
                }
                else
                {
                    MessageBox.Show("未获取到任何数据。");
                }
            }
        }

        
        void clientAtt_GetAttendanceSolutionAsignBySearchNewCompleted(object sender, GetAttendanceSolutionAsignBySearchNewCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                IEnumerable<V_EMPLOYEEATTENDANCESOLUTIONASIGN> entlist = e.Result;
                dgAttSolAsignList.ItemsSource = entlist;
                dataPager.PageCount = e.pageCount;
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
            }

            loadbar.Stop();
        }

        

        /// <summary>
        /// 页面初始化
        /// </summary>
        private void InitPage()
        {
            Utility.DisplayGridToolBarButton(toolbar1, "V_EMPLOYEEATTENDANCESOLUTIONASIGN", true);
            
            BindGrid("");

        }

        
        /// <summary>
        /// 根据查询条件，调用WCF服务获取数据，以便加载数据列表
        /// </summary>
        private void BindGrid(string operationFlag)
        {
            string strAttendanceSolutionID = string.Empty, strAssignedObjectType = string.Empty, strSortKey = string.Empty, strOwnerID = string.Empty, strCheckState = string.Empty;
            int pageIndex = 0, pageSize = 0, pageCount = 0;

            strOwnerID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            strSortKey = " CREATEDATE ";
            CheckInputFilter(ref strAttendanceSolutionID, ref strAssignedObjectType, ref strCheckState);
            pageIndex = dataPager.PageIndex;
            pageSize = dataPager.PageSize;
           
            string solutionName = string.Empty;
            solutionName = txtsolutionName.Text;
            string employeeName = string.Empty;
            employeeName = txtEmpName.Text;
            string orgValue = string.Empty;
            strAssignedObjectType = treeOrganization.sType;
            orgValue = treeOrganization.sValue;
            if (string.IsNullOrEmpty(orgValue))
            {
                try
                {
                    orgValue = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                }
                catch(Exception ex)
                {
                    
                }
            }
            if (string.IsNullOrEmpty(strAssignedObjectType))
            {
                strAssignedObjectType = "1"; 
            }
            if (strAssignedObjectType == "Company")
            {
                strAssignedObjectType = "1"; 
            }
            if (strAssignedObjectType == "Department")
            {
                strAssignedObjectType = "2";
            }
            if (strAssignedObjectType == "Post")
            {
                strAssignedObjectType = "3";
            }
            
            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            string startDate = nuYear.Value.ToString() + "-" + startMonth.Value.ToString() + "-01";
            string endDate = string.Empty;
            if (nuMonth.Value < 12)
            {
                endDate = endYear.Value.ToString() + "-" + (nuMonth.Value + 1).ToString() + "-01";                
            }
            else
            {
                endDate = (endYear.Value+1).ToString() + "-01-01";
            }
            if (string.IsNullOrEmpty(nuYear.Value.ToString()) || string.IsNullOrEmpty(startMonth.Value.ToString()))
            {
                Utility.ShowCustomMessage(MessageTypes.Message, "提示", "生效日期年份或月份不能为空");
                return;
            }
            if (string.IsNullOrEmpty(endYear.Value.ToString()) || string.IsNullOrEmpty(nuMonth.Value.ToString()))
            {
                Utility.ShowCustomMessage(MessageTypes.Message, "提示", "失效日期年份或月份不能为空");
                return;
            }
            if (!string.IsNullOrEmpty(startDate))
            {
                dtStart = Convert.ToDateTime(startDate);
            }
            if (!string.IsNullOrEmpty(endDate))
            {                
                dtEnd = Convert.ToDateTime(endDate);
                dtEnd = dtEnd.AddSeconds(-2);
            }
            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate))
            {
                dtStart = Convert.ToDateTime("2001-01-01"); ;
                dtEnd = Convert.ToDateTime("9999-12-31"); ;
            }
            if (dtEnd < dtStart)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, "提示","生效日期不能大于失效日期");
                return;
            }
            if (operationFlag == "out")
            {
                clientAtt.OutAttendanceSolutionByMultSearchAsync(strOwnerID, solutionName, strAssignedObjectType, dtStart, dtEnd, employeeName, orgValue);
                loadbar.Start();
            }
            else
            {
                //查询时将需要导出的数据也查询一次                
                clientAtt.GetAttendanceSolutionAsignBySearchNewAsync(strOwnerID, solutionName, strAssignedObjectType, dtStart, dtEnd, pageIndex, pageSize, pageCount, employeeName, orgValue);
                loadbar.Start();
            }
        }

        /// <summary>
        /// 校验输入的查询条件
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strFineType"></param>
        private void CheckInputFilter(ref string strAttendanceSolutionID, ref string strAssignedObjectType, ref string strCheckState)
        {
           


        }
        #endregion

        #region 事件

        
        /// <summary>
        /// 页面加载时，预绑定FormToolBar的状态ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AttendanceSolutionAsign_Loaded(object sender, RoutedEventArgs e)
        {
            InitPage();
        }

       

        /// <summary>
        /// 删除指定记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientAtt_RemoveAttendanceSolutionAsignCompleted(object sender, RemoveAttendanceSolutionAsignCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("DELETESUCCESSED", Utility.GetResourceStr("CURRENTRECORD", "ATTENDANCESOLUTIONASIGNFORM")));
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
            }

            BindGrid("");
        }

        /// <summary>
        /// 提交子窗口的表单后，回刷父页面
        /// </summary>
        void entBrowser_ReloadDataEvent()
        {
            ReLoadGrid();
        }

        /// <summary>
        /// 请假类型表单提交后，重新加载数据到数据列表
        /// </summary>
        private void ReLoadGrid()
        {
            BindGrid("");
        }

       

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dataPager.PageIndex = 1;
            byExport = null;
            BindGrid("");
        }

        /// <summary>
        /// 首列加载图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgAttSolAsignList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SetRowLogo(dgAttSolAsignList, e.Row, "T_HR_ATTENDANCESOLUTIONASIGN");
        }

        
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            BindGrid("");
        }

       

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            BindGrid("");
        }

        

        /// <summary>
        /// 弹出子窗口，以便浏览指定的考勤方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnView_Click(object sender, RoutedEventArgs e)
        {
            string strAttendanceSolutionAsignID = string.Empty;

            if (dgAttSolAsignList.SelectedItems == null)
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "VIEW"), Utility.GetResourceStr("CONFIRMBUTTON"));
                return;
            }

            if (dgAttSolAsignList.SelectedItems.Count == 0)
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "VIEW"), Utility.GetResourceStr("CONFIRMBUTTON"));
                return;
            }            
            V_EMPLOYEEATTENDANCESOLUTIONASIGN ent = dgAttSolAsignList.SelectedItems[0] as V_EMPLOYEEATTENDANCESOLUTIONASIGN;            
            AttSolRdForm formAttSolRd = new AttSolRdForm(FormTypes.Browse, ent.ATTENDANCESOLUTIONID);

            EntityBrowser entBrowser = new EntityBrowser(formAttSolRd);
            formAttSolRd.MinWidth = 670;
            formAttSolRd.MinHeight = 380;
            entBrowser.FormType = FormTypes.Browse;            
            entBrowser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });            
        }

        
        #endregion

        #region IClient 成员

        public void ClosedWCFClient()
        {
            clientAtt.DoClose();
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

        #region 组织架构控件
        private void treeOrganization_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            byExport = null;
            BindGrid("");
        }

        void treeOrganization_SelectedClick(object sender, EventArgs e)
        {
            dataPager.PageIndex = 1;
            byExport = null;
            BindGrid("");
        }
        #endregion
    }
}
