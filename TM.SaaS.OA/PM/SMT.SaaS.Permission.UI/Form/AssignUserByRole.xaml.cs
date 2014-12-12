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

namespace SMT.SaaS.Permission.UI.Form
{
    public partial class AssignUserByRole :  UserControl
    {
        private PermissionServiceClient client = new PermissionServiceClient();
        public AssignUserByRole()
        {
            InitializeComponent();
        }

        public AssignUserByRole(string roleID)
        {
            InitializeComponent();
            InitEvent(roleID);
            
        }
        private void InitEvent(string roleID)
        {
            client.GetEmployeeInfosByRoleIDCompleted += new EventHandler<GetEmployeeInfosByRoleIDCompletedEventArgs>(client_GetEmployeeInfosByRoleIDCompleted);
            client.GetEmployeeInfosByRoleIDAsync(roleID);
        }

        void client_GetEmployeeInfosByRoleIDCompleted(object sender, GetEmployeeInfosByRoleIDCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error == null)
            {
                List<V_RoleUserInfo> aa = e.Result.ToList();
                BindDataGrid(aa);
            }
        }

        private void BindDataGrid(List<V_RoleUserInfo> obj)
        {
            PagedCollectionView pcv = null;
            if (obj == null || obj.Count < 1)
            {
                //HtmlPage.Window.Alert("对不起！未能找到相关记录。");
                DtGridUsers.ItemsSource = null;
                return;
            }
            var q = from ent in obj
                    select ent;
            //按公司部门排序
            q = q.OrderBy(s=>s.COMPANYNAME).OrderBy(s=>s.DEPARTMENTNAME);
            pcv = new PagedCollectionView(q);
            pcv.PageSize = 500;
            DtGridUsers.ItemsSource = pcv;
            
            //if (e.Result != null)
            //{
            //    List<T_SYS_ROLE> menulist = e.Result.ToList();
            //    var q = from ent in menulist
            //            select ent;

            //    pcv = new PagedCollectionView(q);
            //    pcv.PageSize = 100;
            //}

            //DtGrid.ItemsSource = pcv;
            //DtGridUsers.CacheMode = new BitmapCache();
        }
        private void DtGridUsers_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //SetRowLogo(DtGridUsers, e.Row, "T_SYS_USER");            
            int index = e.Row.GetIndex();
            var cell = DtGridUsers.Columns[0].GetCellContent(e.Row) as TextBlock;
            cell.Text = (index + 1).ToString();
        }

        public void SetRowLogo(DataGrid DtGrid, DataGridRow row, string entityCode)
        {
            if (DtGrid.ItemsSource != null)
            {
                Image logo = DtGrid.Columns[0].GetCellContent(row).FindName("entityLogo") as Image;                
            }
        }
        
        
    }
}

