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
using System.Windows.Data;
using System.Windows.Browser;
using System.IO;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.Globalization;

namespace SMT.SaaS.Permission.UI.UserControls
{
    public partial class SysUserInfo : UserControl, IEntityEditor
    {
        string tmpUserID = "";
        private T_SYS_USER tmpUser = new T_SYS_USER();
        //public SysUserInfo(T_SYS_USER UserObj)
        public SysUserInfo()
        {
            string aa = Application.Current.Resources["userid"].ToString();
            InitializeComponent();
            //tmpUser = UserObj;
            //this.GetSingleUserDetailInfo(UserObj);
            
        }
        void GetSingleUserDetailInfo(T_SYS_USER obj)
        {
            //this.tblAdddate.Text = obj.CREATEDATE.Value.ToShortDateString();
            this.tblName.Text = obj.EMPLOYEENAME;
            this.tblUserName.Text = obj.USERNAME;
            this.tblremark.Text = obj.REMARK;
            this.tblcode.Text = obj.EMPLOYEECODE;
            
        }


        // 当用户导航到此页面时执行。
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    string aa = Application.Current.Resources["userid"].ToString();
        //    //Resources["userid"]
        //    //tmpUserID = this.NavigationContext.QueryString.Count()==0 ? "" : this.NavigationContext.QueryString["userid"].ToString();
        //    //ServiceClient.GetSysUserSingleInfoByIdCompleted += new EventHandler<GetSysUserSingleInfoByIdCompletedEventArgs>(ServiceClient_GetSysUserSingleInfoByIdCompleted);


        //}

        #region ComboBox定义

        public class Items
        {
            public string recordType { get; set; }
            public Items(string recordTypeInput)
            {
                this.recordType = recordTypeInput;
            }
        }

        private void SetComboBoxSelectIndex(string recordType, ComboBox cbx)
        {
            if (!string.IsNullOrEmpty(recordType))
            {
                cbx.SelectedItem = (from q in comboBoxItem
                                    where q.recordType == recordType
                                    select q).FirstOrDefault();
            }
        }
        #endregion 

        private Items[] comboBoxItem;

        public Items[] ComboBoxItem
        {
            get { return comboBoxItem; }
            set { comboBoxItem = value; }
        }
        #region IEntityEditor
        public string GetTitle()
        {
            return Utility.GetResourceStr("COMPANY");
        }
        public string GetStatus()
        {
            //return EmployeeEntry != null ? EmployeeEntry.CHECKSTATE : "";
            return "";
        }
        public void DoAction(string actionType)
        {
            switch (actionType)
            {
                case "0":
                    Save();
                    break;
                case "1":
                    SaveAndClose();
                    break;
            }
        }

        #region 保存
        private void Save()
        {
            try
            {
                string bb = "";
                
            }
            catch (Exception ex)
            {
                HtmlPage.Window.Alert(ex.ToString());
            }
        }

        private void SaveAndClose()
        {
            //saveType = "1";
            Save();
        }

        #endregion

        public List<NavigateItem> GetLeftMenuItems()
        {
            List<NavigateItem> items = new List<NavigateItem>();
            NavigateItem item = new NavigateItem
            {
                Title = "员工信息",
                Tooltip = "详细信息"
            };
            items.Add(item);
            item = new NavigateItem
            {
                Title = "角色",
                Tooltip = "员工角色",
                Url = "/SysUserRole.xaml?userid="+tmpUser.SYSUSERID
            };
            items.Add(item);
            
            return items;
        }
        public List<ToolbarItem> GetToolBarItems()
        {
            List<ToolbarItem> items = new List<ToolbarItem>();

            ToolbarItem item = new ToolbarItem
            {
                DisplayType = ToolbarItemDisplayTypes.Image,
                Key = "0",
                Title = "保存",
                ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_saveClose.png"
            };

            items.Add(item);

            item = new ToolbarItem
            {
                DisplayType = ToolbarItemDisplayTypes.Image,
                Key = "1",
                Title = "保存并关闭",
                ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_close.png"
            };

            items.Add(item);

            return items;
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
    }
}
