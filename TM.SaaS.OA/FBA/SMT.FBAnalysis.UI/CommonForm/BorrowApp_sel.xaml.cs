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

namespace SMT.FBAnalysis.UI.CommonForm
{
    public partial class BorrowApp_sel : BaseForm, IClient, IEntityEditor
    {
        public List<T_FB_BORROWAPPLYMASTER> _lstBorrowApply_Add;
        public DailyManagementServicesClient client = new DailyManagementServicesClient();
        string strChkState;
        private string strOwnerID;
        private string strOwnerPostID;
        private string strOwnerDepartmentID;
        private string strOwnerCompanyID;

        public BorrowApp_sel(string OwnerID, string OwnerPostID, string OwnerDepartmentID, string OwnerCompanyID)
        {
            InitializeComponent();
            strOwnerID = OwnerID;
            strOwnerPostID = OwnerPostID;
            strOwnerDepartmentID = OwnerDepartmentID;
            strOwnerCompanyID = OwnerCompanyID;
            
            client.GetBorrowApplyMasterListForRepayCompleted += new EventHandler<GetBorrowApplyMasterListForRepayCompletedEventArgs>(client_GetBorrowApplyMasterListForRepayCompleted);
            this.Loaded += new RoutedEventHandler(BorrowApp_sel_Loaded);
        }

        void BorrowApp_sel_Loaded(object sender, RoutedEventArgs e)
        {
            strChkState = "2";
            _lstBorrowApply_Add = new List<T_FB_BORROWAPPLYMASTER>();
            GetData(strChkState);
        }

        //加载数据
        private void GetData(string checkState)
        {
            string filter = "";    //查询过滤条件
            ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();   //查询过滤条件   
            RefreshUI(RefreshedTypes.ShowProgressBar);
            if (!string.IsNullOrEmpty(filter))
            {
                filter += " and ";
            }
            filter += "OWNERID == @" + paras.Count().ToString();
            paras.Add(strOwnerID);

            decimal dCheckStates = 0, dIsRepaied = 1;
            decimal.TryParse(checkState, out dCheckStates);
            client.GetBorrowApplyMasterListForRepayAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, dIsRepaied, dCheckStates, filter, paras);
        }

        //获取数据
        void client_GetBorrowApplyMasterListForRepayCompleted(object sender, GetBorrowApplyMasterListForRepayCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            ObservableCollection<T_FB_BORROWAPPLYMASTER> borrowInfoData = e.Result;
            if (borrowInfoData != null)
            {
                dgvBorrowAppList.ItemsSource = borrowInfoData.ToList();
            }
            else
            {
                dgvBorrowAppList.ItemsSource = null;
            }
        }

        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            GetData("2");
        }

        //勾选
        private void myChkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked.Value)
            {
                T_FB_BORROWAPPLYMASTER vd = (T_FB_BORROWAPPLYMASTER)chk.DataContext;
                _lstBorrowApply_Add.Add(vd);
            }

            if (_lstBorrowApply_Add == null)
            {
                return;
            }

            if (_lstBorrowApply_Add.Count() == 0)
            {
                return;
            }
        }

        //取消勾选
        private void myChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (!chk.IsChecked.Value)
            {
                T_FB_BORROWAPPLYMASTER vd = (T_FB_BORROWAPPLYMASTER)chk.DataContext;
                _lstBorrowApply_Add.Remove(vd);
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
            return Utility.GetResourceStr("BorrowApp_sel");
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
             //{ToolbarItemDisplayTypes.Image,"1","SEARCH","/SMT.SaaS.FrameworkUI;Component/Images/(09,24).png"},
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
            if (dgvBorrowAppList.SelectedItems.Count <= 0)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "请选择还款单", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }
            if (_lstBorrowApply_Add.Count() == 0)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "请选择还款单",Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }

            if (_lstBorrowApply_Add.Count() > 1)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "只能选择一张还款单", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }

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
