using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI;
using SMT.SAAS.Main.CurrentContext;
//using SMT.Saas.Tools.FBServiceWS;
using SMT.FBAnalysis.UI.Common;
using SMT.FBAnalysis.ClientServices.DailyManagementWS;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.Saas.Tools.SmtOACommonOfficeService;


namespace SMT.FBAnalysis.UI.CommonForm
{
    public partial class ItemLookUp : BaseForm, IClient, IEntityEditor
    {
        public T_OA_APPROVALINFO SelectedItem { get; set; }
        public SmtOACommonOfficeClient client = null;
        string strAccountType;
        private string strOwnerID;
        private string strOwnerPostID;
        private string strOwnerDepartmentID;
        private string strOwnerCompanyID;

        private SMTLoading loadbar = new SMTLoading();//滚动圈
        public ItemLookUp(string OwnerID, string OwnerPostID, string OwnerDepartmentID, string OwnerCompanyID)
        {
            InitializeComponent();
            strOwnerID = OwnerID;
            strOwnerPostID = OwnerPostID;
            strOwnerDepartmentID = OwnerDepartmentID;
            strOwnerCompanyID = OwnerCompanyID;
           
            LayoutRoot.Children.Add(loadbar);
           
            this.Loaded += new RoutedEventHandler(SubjectApp_sel_Loaded);
            try
            {
                client = new SmtOACommonOfficeClient();

                client.GetApporvalListforMVCForReimbursementCompleted += new EventHandler<GetApporvalListforMVCForReimbursementCompletedEventArgs>(client_GetApporvalListforMVCForReimbursementCompleted);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        void SubjectApp_sel_Loaded(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        //加载数据
        private void GetData()
        {
            loadbar.Start();

            client.GetApporvalListforMVCForReimbursementAsync(dataPager.PageIndex, dataPager.PageSize, string.Empty, new ObservableCollection<object>(), 0, strOwnerID, string.Empty);
        }

        //获取数据
        void client_GetApporvalListforMVCForReimbursementCompleted(object sender, GetApporvalListforMVCForReimbursementCompletedEventArgs e)
        {
            try
            {
                dataPager.PageCount = e.pageCount;
                RefreshUI(RefreshedTypes.HideProgressBar);
                var  ItemInfoData = e.Result;
                SelectedItem = null;
                if (ItemInfoData != null)
                {
                    dgvItemList.ItemsSource = ItemInfoData;
                }
                else
                {
                    dgvItemList.ItemsSource = null;
                }
                loadbar.Stop();
            }
            catch (Exception ex)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), "获取事项审批信息错误，请联系管理员" + ex.ToString(), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
            }
        }
        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        //勾选
       
        private void myChkBox_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton chk = sender as RadioButton;
            if (chk.IsChecked.Value)
            {
                SelectedItem = (T_OA_APPROVALINFO)chk.DataContext;
            }
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

        public string GetTitle()
        {
            return "事项审批选择";
        }

        public string GetStatus()
        {
            return "";
        }

        public List<NavigateItem> GetLeftMenuItems()
        {
            List<NavigateItem> items = new List<NavigateItem>();
            return items;

        }

        public List<ToolbarItem> GetToolBarItems()
        {
            object[,] arr = new object[,]{
             {ToolbarItemDisplayTypes.Image,"0","CONFIRMBUTTON","/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/18_audit.png"},
            };

            List<ToolbarItem> items = new List<ToolbarItem>();
            ToolbarItem item;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                item = new ToolbarItem
                {
                    DisplayType = (ToolbarItemDisplayTypes)arr[i, 0],
                    Key = (string)arr[i, 1],
                    Title = Utility.GetResourceStr((string)arr[i, 2]),
                    ImageUrl = (string)arr[i, 3]
                };
                items.Add(item);
            }
            return items;
        }

        public void DoAction(string actionType)
        {
            
            //if (dgvItemList.SelectedItems.Count <= 0)
            //{
            //    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), "请选择一条件事件审批", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
            //    return;
            //}
            
            //if (SelectedItem == null)
            //{
            //    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), "请选择一条件事件审批", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
            //    return;

            //}
            SelectedItem = dgvItemList.SelectedItem as T_OA_APPROVALINFO;
            switch (actionType)
            {
                case "0":
                    //CheckData();
                    RefreshUI(RefreshedTypes.CloseAndReloadData);
                    break;
                case "1":
                    //search();

                    break;
            }
            return;
        }

        #region IForm 成员

        public void ClosedWCFClient()
        {
            //vehicleManager.DoClose();
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
