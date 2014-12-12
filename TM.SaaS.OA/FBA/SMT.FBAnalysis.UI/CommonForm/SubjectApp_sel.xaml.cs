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
    public partial class SubjectApp_sel : BaseForm, IClient, IEntityEditor
    {
        public ObservableCollection<T_FB_BUDGETACCOUNT> _lstSubjectApply_Add;
        //public ObservableCollection<T_FB_BORROWAPPLYDETAIL> _lstSubjectApply_Add;
        public DailyManagementServicesClient client = new DailyManagementServicesClient();
        List<object> subList;
        string strAccountType;
        private string strOwnerID;
        private string strOwnerPostID;
        private string strOwnerDepartmentID;
        private string strOwnerCompanyID;
        private int nYear;
        private int nMonth;
        private SMTLoading loadbar = new SMTLoading();//滚动圈
        public SubjectApp_sel(string OwnerID, string OwnerPostID, string OwnerDepartmentID, string OwnerCompanyID, List<object> subcodeLst)
        {
            InitializeComponent();
            strOwnerID = OwnerID;
            strOwnerPostID = OwnerPostID;
            strOwnerDepartmentID = OwnerDepartmentID;
            strOwnerCompanyID = OwnerCompanyID;
            subList = subcodeLst;
            LayoutRoot.Children.Add(loadbar);
            client.GetBudgetAccountByPersonCompleted += new EventHandler<GetBudgetAccountByPersonCompletedEventArgs>(client_GetBudgetAccountByPersonCompleted);
            this.Loaded += new RoutedEventHandler(SubjectApp_sel_Loaded);
        }

        void SubjectApp_sel_Loaded(object sender, RoutedEventArgs e)
        {
            _lstSubjectApply_Add = new ObservableCollection<T_FB_BUDGETACCOUNT>();
            strAccountType = "3";
            nYear = DateTime.Now.Year;
            nMonth = DateTime.Now.Month;
            GetData(strAccountType, nYear, nMonth);
        }

        //加载数据
        private void GetData(string accountType, int Yeat, int Month)
        {
            loadbar.Start(); 
            client.GetBudgetAccountByPersonAsync(strOwnerID, strOwnerPostID, strOwnerCompanyID);
        }

        //获取数据
        void client_GetBudgetAccountByPersonCompleted(object sender, GetBudgetAccountByPersonCompletedEventArgs e)
        {
            try
            {
                RefreshUI(RefreshedTypes.HideProgressBar);
                ObservableCollection<T_FB_BUDGETACCOUNT> subjectInfoData = e.Result;

                if (subjectInfoData != null)
                {
                    dgvSubjectList.ItemsSource = subjectInfoData;
                }
                else
                {
                    dgvSubjectList.ItemsSource = null;
                }
                loadbar.Stop();
            }
            catch (Exception ex)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), "获取科目信息错误，请联系管理员"+ex.ToString(), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);              
            }
        }

        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            GetData("3", nYear, nMonth);
        }

        //勾选
        private void myChkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked.Value)
            {
                T_FB_BUDGETACCOUNT vd = (T_FB_BUDGETACCOUNT)chk.DataContext;
                //T_FB_BORROWAPPLYDETAIL vd = (T_FB_BORROWAPPLYDETAIL)chk.DataContext;
                _lstSubjectApply_Add.Add(vd);
            }
        }

        //取消勾选
        private void myChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (!chk.IsChecked.Value)
            {
                T_FB_BUDGETACCOUNT vd = (T_FB_BUDGETACCOUNT)chk.DataContext;
                //T_FB_BORROWAPPLYDETAIL vd = (T_FB_BORROWAPPLYDETAIL)chk.DataContext;
                _lstSubjectApply_Add.Remove(vd);
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
            return Utility.GetResourceStr("SubjectApp_sel");
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
            if (dgvSubjectList.SelectedItems.Count <= 0)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), "请选择相应的项目信息", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);                
                return;
            }
            if (_lstSubjectApply_Add == null)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), "请选择相应的项目信息", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }
            else
            {
                if (_lstSubjectApply_Add.Count() == 0)
                {
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "请选择相应的项目信息", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                    return;
                }
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
