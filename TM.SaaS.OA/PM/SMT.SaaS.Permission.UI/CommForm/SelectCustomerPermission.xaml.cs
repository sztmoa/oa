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
using SMT.Saas.Tools.OrganizationWS;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMTWindow = System.Windows.Controls;

namespace SMT.SaaS.Permission.UI
{
    public partial class SelectCustomerPermission : SMTWindow.Window
    {
        public SelectCustomerPermission()
        {
            InitializeComponent();
        }
        private List<T_SYS_ENTITYMENU> allMenu;

        public object SelectedObj
        {
            get;
            set;
        }
        public OrgTreeItemTypes SelectedObjType { get; set; }

        public event EventHandler SelectedClick;

        public string SystemType = "";

        
        PermissionServiceClient client = new PermissionServiceClient();
        private void InitParas()
        {
            //初始化控件的状态            

            client.GetSysMenuByTypeCompleted += new EventHandler<GetSysMenuByTypeCompletedEventArgs>(client_GetSysMenuByTypeCompleted);
            client.GetSysMenuByTypeAsync(SystemType, "");
        }

        void client_GetSysMenuByTypeCompleted(object sender, GetSysMenuByTypeCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                allMenu = e.Result.ToList();
            }
            BindTree();
        }

        private void BindTree()
        {
            if (allMenu != null)
            {

                foreach (T_SYS_ENTITYMENU menu in allMenu)
                {
                    TreeViewItem childItem = new TreeViewItem();
                    childItem.Header = menu.MENUNAME;
                    childItem.DataContext = menu;

                    AddChildItems(childItem, menu);

                    treeMenu.Items.Add(childItem);

                }
            }
        }
        private void AddChildItems(TreeViewItem treeitem, T_SYS_ENTITYMENU menu)
        {
            var menulist = from m in allMenu
                           where m.T_SYS_ENTITYMENU2Reference.EntityKey != null
                           && m.T_SYS_ENTITYMENU2Reference.EntityKey.EntityKeyValues[0].Value.ToString() == menu.ENTITYMENUID
                           select m;

            foreach (var tmpMenu in menulist)
            {
                TreeViewItem childItem = new TreeViewItem();
                childItem.Header = tmpMenu.MENUNAME;
                childItem.DataContext = tmpMenu;

                AddChildItems(childItem, tmpMenu);

                treeitem.Items.Add(childItem);
            }
        }


        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedObj == null)
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), "所选不能为空！", Utility.GetResourceStr("CONFIRMBUTTON"));
                return;
            }
            //this.DialogResult = true;
            this.Close();

            if (SelectedClick != null)
                SelectedClick(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            //this.DialogResult = false;
            this.Close();
        }

        private void lookUpTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (treeMenu.SelectedItem.GetType() != typeof(TreeViewItem))
                return;

            TreeViewItem item = (TreeViewItem)treeMenu.SelectedItem;

            if (item == null || item.DataContext == null)
                return;

            SelectedObj = item.DataContext;
        }
    }
}
