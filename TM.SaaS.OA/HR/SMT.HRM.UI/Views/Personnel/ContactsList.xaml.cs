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
using SMT.Saas.Tools.PersonnelWS;
using System.Windows.Controls.Primitives;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.Saas.Tools.OrganizationWS;
using OrganizationWS = SMT.Saas.Tools.OrganizationWS;
using SMT.SAAS.Main.CurrentContext;

namespace SMT.HRM.UI.Views.Personnel
{
    public partial class ContactsList : BasePage, IClient
    {
        SMTLoading loadbar = new SMTLoading();
        private SMT.Saas.Tools.PersonnelWS.PersonnelServiceClient client;
        
        private SaveFileDialog dialog = new SaveFileDialog();
        private bool? result;

        public ContactsList()
        {
            InitializeComponent();
            InitPara();
            GetEntityLogo("T_HR_EMPLOYEE");
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Utility.DisplayGridToolBarButton(ToolBar, "T_HR_EMPLOYEE", false);
            ViewTitles.TextTitle.Text = GetTitleFromURL(e.Uri.ToString());
        }

        public void InitPara()
        {
            try
            {
                PARENT.Children.Add(loadbar);
                loadbar.Stop();
               
                client = new SMT.Saas.Tools.PersonnelWS.PersonnelServiceClient();
                client.GetContactsListPagingCompleted += new EventHandler<GetContactsListPagingCompletedEventArgs>(client_GetContactsListPagingCompleted);
                client.ExportContactsListCompleted += new EventHandler<ExportContactsListCompletedEventArgs>(client_ExportContactsListCompleted);
                treeOrganization.SelectedClick += new EventHandler(treeOrganization_SelectedClick);

                
                ToolBar.btnOutExcel.Visibility = Visibility.Visible;
                ToolBar.btnOutExcel.Click += new RoutedEventHandler(btnOutExcel_Click);

                treeOrganization.SelectedClick += new EventHandler(treeOrganization_SelectedClick);
            }
            catch (Exception)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
                 Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
        }

        void client_ExportContactsListCompleted(object sender, ExportContactsListCompletedEventArgs e)
        {
            try
            {
                if (result == true)
                {
                    if (e.Error == null)
                    {
                        if (e.Result != null)
                        {
                            using (System.IO.Stream stream = dialog.OpenFile())
                            {
                                stream.Write(e.Result, 0, e.Result.Length);
                                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("导出成功"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                            }
                        }
                        else
                        {
                            ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), Utility.GetResourceStr("没有数据可导出"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                        }
                    }
                    else
                    {
                        ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), ex.ToString(), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
        }

        void client_GetContactsListPagingCompleted(object sender, GetContactsListPagingCompletedEventArgs e)
        {
            if (e.Error != null && e.Error.Message != "")
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
               Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                DtGrid.ItemsSource = e.Result;
                dataPager.PageCount = e.pageCount;
            }
            loadbar.Stop();
        }

        
        /// <summary>
        /// 导出通讯录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnOutExcel_Click(object sender, RoutedEventArgs e)
        {
            string sType = "", sValue = "";
            sType = treeOrganization.sType;
            sValue = treeOrganization.sValue;
            if (sType != "Company")
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), Utility.GetResourceStr("SELECTCOMPANY"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }

            dialog.Filter = "MS Excel Files|*.xls";
            dialog.FilterIndex = 1;
            result = dialog.ShowDialog();
            
            string filter = "";
            System.Collections.ObjectModel.ObservableCollection<string> paras = new System.Collections.ObjectModel.ObservableCollection<string>();

            TextBox txtEmpName = Utility.FindChildControl<TextBox>(expander, "txtEmpName");
            
            if (txtEmpName != null)
            {
                if (!string.IsNullOrEmpty(txtEmpName.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += " and ";
                    }
                    filter += " @" + paras.Count().ToString() + ".Contains(EMPLOYEECNAME)";
                    paras.Add(txtEmpName.Text.Trim());
                }
            }

            string companyID = sValue;
            //client.ExportEmployeeAsync(companyID, filter, paras, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
            client.ExportContactsListAsync(companyID, filter, paras, sType, sValue, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
        }

        void treeOrganization_SelectedClick(object sender, EventArgs e)
        {
            LoadData();
        }

        void LoadData()
        {
            loadbar.Start();
            int pageCount = 0;
            string filter = "";
            System.Collections.ObjectModel.ObservableCollection<string> paras = new System.Collections.ObjectModel.ObservableCollection<string>();
            TextBox txtEmpName = Utility.FindChildControl<TextBox>(expander, "txtEmpName");
            if (txtEmpName != null)
            {
                if (!string.IsNullOrEmpty(txtEmpName.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter += " and ";
                    }
                    filter += " @" + paras.Count().ToString() + ".Contains(EMPLOYEECNAME)";
                    paras.Add(txtEmpName.Text.Trim());
                }
            }
            string sType = "", sValue = "";
            sType = treeOrganization.sType;
            sValue = treeOrganization.sValue;
            
            client.GetContactsListPagingAsync(dataPager.PageIndex, dataPager.PageSize, "EMPLOYEECNAME",
                    filter, paras, pageCount, sType, sValue, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
        }

        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        
        private void DtGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SetRowLogo(DtGrid, e.Row, "T_HR_EMPLOYEE");
        }

        private void treeOrganization_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            LoadData();
        }

        #region IClient
        public void ClosedWCFClient()
        {
            // throw new NotImplementedException();
            client.DoClose();
            // orgClient.DoClose();
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
