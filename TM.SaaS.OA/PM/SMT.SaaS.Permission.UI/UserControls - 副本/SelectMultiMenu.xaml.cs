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
using SMT.Saas.Tools.PermissionWS;
using System.Windows.Data;
using SMT.SaaS.FrameworkUI;
using SMT.SaaS.Permission.UI.Form;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.SAAS.Main.CurrentContext;
using SMT.SaaS.FrameworkUI.Helper;
using SMT.Saas.Tools.OrganizationWS;
using SMT.SaaS.FrameworkUI.OrganizationControl;

namespace SMT.SaaS.Permission.UI.UserControls
{
    public partial class SelectMultiMenu : UserControl, IEntityEditor
    {
        #region 参数定义
        private PermissionServiceClient client = new PermissionServiceClient();
        public List<T_SYS_ENTITYMENU> SelectedMultiMenu ; //选中的菜单集合
        List<T_SYS_ENTITYMENU> allMenu = new List<T_SYS_ENTITYMENU>();
        private T_SYS_ENTITYMENU EntityObj;
        CheckBox SelectBox = new CheckBox();
        #endregion

        

        public SelectMultiMenu(string SysType,List<T_SYS_ENTITYMENU> ListEntity)
        {
            InitializeComponent();
            EntityObj = new T_SYS_ENTITYMENU();
            
            SelectedMultiMenu = new List<T_SYS_ENTITYMENU>();
            if(ListEntity != null)
                SelectedMultiMenu = ListEntity;
            InitEvent();
            InitParas(SysType);
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void InitEvent()
        {            
            client.GetSysMenuByTypeCompleted += new EventHandler<GetSysMenuByTypeCompletedEventArgs>(client_GetSysMenuByTypeCompleted);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitParas(string SysType)
        {
            client.GetSysMenuByTypeAsync(SysType,"");
        }

        /// <summary>
        /// 获取指定系统下的功能项集合，并加载到树控件上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetSysMenuByTypeCompleted(object sender, GetSysMenuByTypeCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    return;
                }

                allMenu = e.Result.ToList();
                DataGridBindingPcv(DaGrMenu, allMenu);

            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), e.Error.Message);
            }
        }

        private void DataGridBindingPcv(DataGrid dt, List<T_SYS_ENTITYMENU> OAMenu)
        {
            PagedCollectionView pcv = new PagedCollectionView(OAMenu);
            Type a = ((PagedCollectionView)pcv).CurrentItem.GetType();
            pcv.PageSize = 400;
            
            dt.ItemsSource = pcv;
        }

        #region DataGrid事件


        private void myChkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = sender as CheckBox;
            if (chkbox.IsChecked.Value)
            {
                EntityObj = chkbox.DataContext as T_SYS_ENTITYMENU;
                if (EntityObj != null)
                {
                    if (SelectedMultiMenu.Count > 0)
                    {
                        var entity = from q in SelectedMultiMenu
                                     where q.ENTITYMENUID == EntityObj.ENTITYMENUID
                                     select q;
                        if (entity.Count() == 0)
                        {
                            SelectedMultiMenu.Add(EntityObj);
                        }
                    }
                    else
                    {
                        SelectedMultiMenu.Add(EntityObj);
                    }
                }
            }
        }


        private void myChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chkbox = sender as CheckBox;
            if (!chkbox.IsChecked.Value)
            {
                EntityObj = (T_SYS_ENTITYMENU)chkbox.DataContext;
                if (SelectedMultiMenu != null)
                {
                    foreach (var h in SelectedMultiMenu)
                    {
                        if (h.ENTITYMENUID == EntityObj.ENTITYMENUID)
                        {
                            SelectedMultiMenu.Remove(h);
                            break;
                        }
                    }
                }
            }
        }


        private void DaGrMenu_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (SelectedMultiMenu.Count() > 0)
            {
                EntityObj = (T_SYS_ENTITYMENU)e.Row.DataContext;
                var entity = from q in SelectedMultiMenu
                             where q.ENTITYMENUID == EntityObj.ENTITYMENUID
                             select q;
                if (entity.Count() > 0)
                {
                    CheckBox chkbox = DaGrMenu.Columns[0].GetCellContent(e.Row).FindName("myChkBox") as CheckBox;
                    chkbox.IsChecked = true;
                }
            }

            T_SYS_ENTITYMENU EntityT = (T_SYS_ENTITYMENU)e.Row.DataContext;
            CheckBox mychkBox = DaGrMenu.Columns[0].GetCellContent(e.Row).FindName("myChkBox") as CheckBox;
            mychkBox.Tag = EntityT;
        }

        #endregion


        #region IEntityEditor
        public string GetTitle()
        {
            //return Utility.GetResourceStr("COMPANY");
            return Utility.GetResourceStr("选择自定义权限菜单");
        }

        public string GetStatus()
        {
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
                    Cancel();
                    break;
            }
        }

        public List<NavigateItem> GetLeftMenuItems()
        {
            List<NavigateItem> items = new List<NavigateItem>();
            NavigateItem item = new NavigateItem
            {
                Title = "详细信息",
                Tooltip = "详细信息"
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
                Title = Utility.GetResourceStr("CONFIRM"),
                ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_save.png"
            };

            items.Add(item);

            item = new ToolbarItem
            {
                DisplayType = ToolbarItemDisplayTypes.Image,
                Key = "1",
                Title = Utility.GetResourceStr("CANCEL"),
                ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/ico_16_delete.png"
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

        #region 确定、取消
        private void Save()
        {
            RefreshUI(RefreshedTypes.CloseAndReloadData);
        }

        private void Cancel()
        {
            SelectedMultiMenu.Clear();
            RefreshUI(RefreshedTypes.CloseAndReloadData);
        }
        #endregion
    }
}
