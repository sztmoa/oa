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
using SMT.SaaS.FrameworkUI;
using System.Windows.Data;
using System.Windows.Browser;
using System.IO;
using SMT.Saas.Tools.PermissionWS;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI.ChildWidow;

namespace SMT.SaaS.Permission.UI.UserControls
{
    public partial class SysRoleSetMenu : UserControl, IEntityEditor
    {
        private ClientStorage cache = ClientStorage.Instance;//龙康才新增
        private static PermissionServiceClient RoleClient = new PermissionServiceClient();//龙康才新增 static
        private List<T_SYS_PERMISSION> tmpPermission = new List<T_SYS_PERMISSION>();
        private List<T_SYS_DICTIONARY> tmpDictionary = new List<T_SYS_DICTIONARY>();
        private T_SYS_ROLE tmprole = new T_SYS_ROLE();
        private ObservableCollection<T_SYS_ENTITYMENU> menuInfosList = new ObservableCollection<T_SYS_ENTITYMENU>(); //菜单列表
        private ObservableCollection<T_SYS_PERMISSION> PermInfosList = new ObservableCollection<T_SYS_PERMISSION>(); //权限
        private ObservableCollection<V_Permission> EntityPermissionInfosList = new ObservableCollection<V_Permission>(); //权限视图集合
        private ObservableCollection<string> RangeList = new ObservableCollection<string>(); //范围列表
        //private ObservableCollection<string> tmpAllList = new ObservableCollection<string>(); //所有临时字段列表

        private List<T_SYS_ROLEENTITYMENU> tmpEditRoleEntityLIst = new List<T_SYS_ROLEENTITYMENU>();
        private List<T_SYS_ROLEENTITYMENU> tmpOAEditRoleEntityLIst = new List<T_SYS_ROLEENTITYMENU>();
        private List<T_SYS_ROLEMENUPERMISSION> tmpEditRoleEntityPermList = new List<T_SYS_ROLEMENUPERMISSION>();
        private List<T_SYS_ROLEMENUPERMISSION> tmpOAEditRoleEntityPermList = new List<T_SYS_ROLEMENUPERMISSION>();
        private ObservableCollection<string> tmpRoleEntityIDsList = new ObservableCollection<string>();//roleentityid 集合
        private ObservableCollection<string> tmpOARoleEntityIDsList = new ObservableCollection<string>();//roleentityid 集合
        private bool IsAdd = false; //用来控制是否是第1次添加
        string tmpAllList = "";
        private List<T_SYS_ENTITYMENU> HrSource = new List<T_SYS_ENTITYMENU>();
        private int SelectIndex = 0; //TABCONTROL的选择序列

        private ObservableCollection<T_SYS_ROLEENTITYMENU> OldEntityLIst = new ObservableCollection<T_SYS_ROLEENTITYMENU>();
        private ObservableCollection<T_SYS_ROLEMENUPERMISSION> OldOARoleEntityPermList = new ObservableCollection<T_SYS_ROLEMENUPERMISSION>();

        private SMTLoading loadbar = new SMTLoading(); 
        public SysRoleSetMenu(T_SYS_ROLE obj)
        {
            InitializeComponent();
            PARENT.Children.Add(loadbar);
            tmprole = obj;
            this.tblRoleName.Text = obj.ROLENAME;

            InitControlEvent();//初始化控件事件
            GetRoleIDRoleEntityInfos();
            LoadData();
        }

        void SysRoleSetMenu_Loaded(object sender, RoutedEventArgs e)
        {
            FillPermissionDataRangeNewfill(DaGrOA, "myChkBtn", "OArating");
        }

        private void InitControlEvent()
        {//初始化控件事件

            RoleClient.GetSysPermissionAllCompleted += new EventHandler<GetSysPermissionAllCompletedEventArgs>(RoleClient_GetSysPermissionAllCompleted);
            this.DaGrHR.LoadingRowDetails += new EventHandler<DataGridRowDetailsEventArgs>(DaGrHR_LoadingRowDetails);
            RoleClient.GetRoleEntityIDListInfosByRoleIDCompleted += new EventHandler<GetRoleEntityIDListInfosByRoleIDCompletedEventArgs>(RoleClient_GetRoleEntityIDListInfosByRoleIDCompleted);
            RoleClient.GetRolePermsCompleted += new EventHandler<GetRolePermsCompletedEventArgs>(RoleClient_GetRolePermsCompleted);
            RoleClient.BatchAddRoleEntityPermissionInfosCompleted += new EventHandler<BatchAddRoleEntityPermissionInfosCompletedEventArgs>(RoleClient_BatchAddRoleEntityPermissionInfosCompleted);
            RoleClient.GetPermissionByRoleIDCompleted += new EventHandler<GetPermissionByRoleIDCompletedEventArgs>(RoleClient_GetPermissionByRoleIDCompleted);
            RoleClient.GetPermissionByRoleIDAsync(tmprole.ROLEID);
        }

        void RoleClient_GetPermissionByRoleIDCompleted(object sender, GetPermissionByRoleIDCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        EntityPermissionInfosList = e.Result;
                    }
                }
                else
                {
                    Utility.ShowCustomMessage(MessageTypes.Error,Utility.GetResourceStr("ERROR"),e.Error.ToString());
                    return;
                }
            }
        }

        void RoleClient_GetRolePermsCompleted(object sender, GetRolePermsCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Result != null)
                {
                    tmpEditRoleEntityPermList = e.Result.ToList();
                    //if(HrSource !=null)
                    //this.DaGrHR.ItemsSource = HrSource;
                    //FillHRDataRange();
                    switch (SelectIndex)
                    { 
                        case 0:
                            break;
                        case 1: //OA
                            FillPermissionDataRange(DaGrOA, "myChkBtn", "OArating");
                            break;
                        case 2://HR
                            FillPermissionDataRange(DaGrHR, "myChkBtnHR", "HRrating");
                            break;
                        case 3:// LM
                            FillPermissionDataRange(DaGrLM, "myChkBtnLM", "LMrating");
                            break;
                        case 4://FB
                            FillPermissionDataRange(DaGrFB, "myChkBtnFB", "FBrating");
                            break;
                    }
                    
                }
            }
            loadbar.Stop();
        }
        
        void RoleClient_GetRoleEntityIDListInfosByRoleIDCompleted(object sender, GetRoleEntityIDListInfosByRoleIDCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Result != null)
                {
                    tmpEditRoleEntityLIst = e.Result.ToList();
                    foreach (T_SYS_ROLEENTITYMENU menu in tmpEditRoleEntityLIst)
                    {
                        tmpRoleEntityIDsList.Add(menu.ROLEENTITYMENUID);
                    }
                    //RoleClient.GetRolePermsAsync(tmpRoleEntityIDsList);
                }
                else
                {
                    IsAdd = true;
                }
                loadbar.Stop();
            }
        }

        void DaGrHR_LoadingRowDetails(object sender,DataGridRowDetailsEventArgs e)
        {
            if (e.Row.DataContext != null)
            {
                //DaGrHR.Columns.Add();
                
            }
        }
        void LoadData()
        {
            
            LoadPermissionInfos();
            loadbar.Start();
            #region 龙康才新增
            //if (cache[this.DaGrOA.Name] != null)
            //{
            //    this.DaGrOA = (DataGrid)cache[this.DaGrOA.Name];
            //}
            //else
            //{
            //    GetOADataInfos(); 
            //}
            //if (cache[this.DaGrHR.Name] != null)
            //{
            //    this.DaGrHR = (DataGrid)cache[this.DaGrHR.Name];
            //}
            //else
            //{
            //    GetHRDataInfos();
            //}
            //if (cache[this.DaGrFB.Name] != null)
            //{
            //    this.DaGrFB = (DataGrid)cache[this.DaGrFB.Name];
            //}
            //else
            //{
            //    GetFBDataInfos();
            //}
            //if (cache[this.DaGrLM.Name] != null)
            //{
            //    this.DaGrLM = (DataGrid)cache[this.DaGrLM.Name];
            //}
            //else
            //{
            //    GetLMDataInfos();
            //}
            #endregion
            GetOADataInfos();
            GetHRDataInfos();
            GetFBDataInfos();
            GetLMDataInfos();
            //GetPMDataInfos();
            //LoadDaGrLMDataRange();
                        
            
        } 
        #region 龙康才新增
        int n = 5;//递归5次就停止
        private List<T_SYS_PERMISSION> GetTmpPermission()
        {
           
            
            if (cache["tmpPermission"] == null)
            {
                if (tmpPermission.Count == 0)
                {
                    if (n < 0)
                    {                        
                        return tmpPermission = null;
                    }
                    n--;
                    tmpPermission = new List<T_SYS_PERMISSION>();
                    GetTmpPermission();
                }
                else
                {
                    cache.Add("tmpPermission", tmpPermission);
                }
            }
            else
            {
                tmpPermission = (List<T_SYS_PERMISSION>)cache["tmpPermission"];
            }
            return tmpPermission;            
        }
        #endregion
        //加载星星
        // HR
        void LoadDaGrHRDataRange()
        {
            #region 龙康才新增
            tmpPermission = GetTmpPermission();
            if (tmpPermission == null)
            {
                MessageBox.Show("没法实例化[HR]的List<T_SYS_PERMISSION>");
            }
            #endregion
            foreach (T_SYS_PERMISSION AA in tmpPermission)
            {
                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                templateColumn.Header = AA.PERMISSIONNAME;
                templateColumn.CellTemplate = (DataTemplate)Resources["myCellTemplate"];  
                //templateColumn.
                DaGrHR.Columns.Add(templateColumn);               
            }
        }
        //OA
        
        void LoadDaGrOADataRange()
        {
            #region 龙康才新增
            tmpPermission = GetTmpPermission();
            if (tmpPermission == null)
            {
                MessageBox.Show("没法实例化[OA]的List<T_SYS_PERMISSION>");
            }
            #endregion
            foreach (T_SYS_PERMISSION AA in tmpPermission)
            {
                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                templateColumn.Header = AA.PERMISSIONNAME;
                templateColumn.CellTemplate = (DataTemplate)Resources["myOACellTemplate"];  
                              
                DaGrOA.Columns.Add(templateColumn);
            }
            //FillOADataRange();
            
        }
        //物流
        void LoadDaGrLMDataRange()
        {
            #region 龙康才新增
            tmpPermission = GetTmpPermission();
            if (tmpPermission == null)
            {
                MessageBox.Show("没法实例化[LM]的List<T_SYS_PERMISSION>");
            }
            #endregion
            foreach (T_SYS_PERMISSION AA in tmpPermission)
            {
                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                templateColumn.Header = AA.PERMISSIONNAME;
                templateColumn.CellTemplate = (DataTemplate)Resources["myLMCellTemplate"];
                DaGrLM.Columns.Add(templateColumn);
            }           

        }
        //加载星星
        // HR
        void LoadDaGrFBDataRange()
        {
            #region 龙康才新增
            tmpPermission = GetTmpPermission();
            if (tmpPermission == null)
            {
                MessageBox.Show("没法实例化[FB]的List<T_SYS_PERMISSION>");
            }
            #endregion
            foreach (T_SYS_PERMISSION AA in tmpPermission)
            {
                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                templateColumn.Header = AA.PERMISSIONNAME;
                templateColumn.CellTemplate = (DataTemplate)Resources["myFBCellTemplate"];
                DaGrFB.Columns.Add(templateColumn);
            }
        }
        //权限
        void LoadDaGrPMDataRange()
        {
            #region 龙康才新增
            tmpPermission = GetTmpPermission();
            if (tmpPermission == null)
            {
                MessageBox.Show("没法实例化[PM]的List<T_SYS_PERMISSION>");
            }
            #endregion
            foreach (T_SYS_PERMISSION AA in tmpPermission)
            {
                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                templateColumn.Header = AA.PERMISSIONNAME;
                templateColumn.CellTemplate = (DataTemplate)Resources["myPMCellTemplate"];
                //templateColumn.
                DaGrPM.Columns.Add(templateColumn);
            }
        }

        #region 获取各系统的权限
        private void GetOADataInfos()
        {
            RoleClient.GetOASysMenuByTypeCompleted += new EventHandler<GetOASysMenuByTypeCompletedEventArgs>(RoleClient_GetOASysMenuByTypeCompleted);
            RoleClient.GetOASysMenuByTypeAsync("1");
        }
        private void GetHRDataInfos()
        {            
            RoleClient.GetHRSysMenuByTypeCompleted += new EventHandler<GetHRSysMenuByTypeCompletedEventArgs>(RoleClient_GetHRSysMenuByTypeCompleted);
            RoleClient.GetHRSysMenuByTypeAsync("0");
        }
        private void GetLMDataInfos()
        {            
            RoleClient.GetLMSysMenuByTypeCompleted += new EventHandler<GetLMSysMenuByTypeCompletedEventArgs>(RoleClient_GetLMSysMenuByTypeCompleted);
            RoleClient.GetLMSysMenuByTypeAsync("2");
        }

        void RoleClient_GetLMSysMenuByTypeCompleted(object sender, GetLMSysMenuByTypeCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Result != null)
                {
                    List<T_SYS_ENTITYMENU> AA = e.Result.ToList();
                    this.DaGrLM.ItemsSource = AA;
                    LoadDaGrLMDataRange();
                    
                }

            }
            
        }
        private void GetFBDataInfos()
        {
            RoleClient.GetFBSysMenuByTypeCompleted += new EventHandler<GetFBSysMenuByTypeCompletedEventArgs>(RoleClient_GetFBSysMenuByTypeCompleted);
            RoleClient.GetFBSysMenuByTypeAsync("3");

        }
        private void GetPMDataInfos()
        {
            RoleClient.GetPMSysMenuByTypeCompleted += new EventHandler<GetPMSysMenuByTypeCompletedEventArgs>(RoleClient_GetPMSysMenuByTypeCompleted);
            RoleClient.GetPMSysMenuByTypeAsync("7");
        }

        void RoleClient_GetPMSysMenuByTypeCompleted(object sender, GetPMSysMenuByTypeCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Result != null)
                {
                    List<T_SYS_ENTITYMENU> AA = e.Result.ToList();
                    this.DaGrPM.ItemsSource = AA;
                    LoadDaGrFBDataRange();

                }

            }
            loadbar.Stop();
        }
        #endregion

        private void GetRoleIDRoleEntityInfos()
        {
            loadbar.Start();
            RoleClient.GetRoleEntityIDListInfosByRoleIDAsync(tmprole.ROLEID);
        }
        private void LoadPermissionInfos()
        { 
            
            RoleClient.GetSysPermissionAllAsync();
        }
        private void GetSysDictionaryInfos()
        { 
            RoleClient.GetSysDictionaryByCategoryCompleted += new EventHandler<GetSysDictionaryByCategoryCompletedEventArgs>(RoleClient_GetSysDictionaryByCategoryCompleted);
            RoleClient.GetSysDictionaryByCategoryAsync("ASSIGNEDOBJECTTYPE");
        }
        /// <summary>
        /// 获取权限数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RoleClient_GetSysPermissionAllCompleted(object sender, GetSysPermissionAllCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Result != null)
                {
                    tmpPermission = e.Result.ToList();
                    //this.Loaded += new RoutedEventHandler(SysRoleSetMenu_Loaded);
                }
            }

        }
        /// <summary>
        /// 获取字典数据范围
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RoleClient_GetSysDictionaryByCategoryCompleted(object sender, GetSysDictionaryByCategoryCompletedEventArgs e)
        {
          
            #region 原来代码
            if (!e.Cancelled)
            {
                if (e.Result != null)
                {
                    tmpDictionary = e.Result.ToList();
                }
            }
            #endregion
           

        }
        /// <summary>
        /// oa 数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RoleClient_GetOASysMenuByTypeCompleted(object sender, GetOASysMenuByTypeCompletedEventArgs e)
        {
            #region 龙康才修改
            if (this.DaGrOA.ItemsSource == null)
            {
                if (!e.Cancelled)
                {
                    if (e.Result != null)
                    {
                        List<T_SYS_ENTITYMENU> AA = e.Result.ToList();
                        this.DaGrOA.ItemsSource = AA;
                        if (this.DaGrOA.Columns.Count <= 2)
                        {
                            this.LoadDaGrOADataRange();
                        }
                    }
                }
            }
            #endregion
            #region 原来代码
            //if (!e.Cancelled)
            //{
            //    if (e.Result != null)
            //    {
            //        List<T_SYS_ENTITYMENU> AA = e.Result.ToList();
            //        this.DaGrOA.ItemsSource = AA;
            //       this.LoadDaGrOADataRange();

            //    }
            //}
#endregion
        }
        /// <summary>
        /// HR菜单数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RoleClient_GetHRSysMenuByTypeCompleted(object sender, GetHRSysMenuByTypeCompletedEventArgs e)
        {
            #region 龙康才修改
            if (this.DaGrHR.ItemsSource == null)
            {
                if (!e.Cancelled)
                {
                    if (e.Result != null)
                    {
                        HrSource = e.Result.ToList();
                        this.DaGrHR.ItemsSource = HrSource;
                        if (this.DaGrHR.Columns.Count <= 2)
                        {
                            this.LoadDaGrHRDataRange();
                        }
                    }
                }
            }
            #endregion
            #region 原来代码
            //if (!e.Cancelled)
            //{
            //    if (e.Result != null)
            //    {
            //        //List<T_SYS_ENTITYMENU> AA = e.Result.ToList();                    
            //        //this.DaGrHR.ItemsSource = AA;                    
            //        HrSource = e.Result.ToList();
            //        if (HrSource != null)
            //            this.DaGrHR.ItemsSource = HrSource;
            //        LoadDaGrHRDataRange();

            //    }

            //}
            #endregion
        }
        /// <summary>
        /// FB 数据源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RoleClient_GetFBSysMenuByTypeCompleted(object sender, GetFBSysMenuByTypeCompletedEventArgs e)
        {
            #region 龙康才修改
            if (this.DaGrFB.ItemsSource == null)
            {
                if (!e.Cancelled)
                {
                    if (e.Result != null)
                    {
                        List<T_SYS_ENTITYMENU> AA = e.Result.ToList();
                        this.DaGrFB.ItemsSource = AA;
                        if (this.DaGrFB.Columns.Count <= 2)
                        {
                            this.LoadDaGrFBDataRange();
                        }
                    }
                }
            }
            #endregion
            #region 原来代码
            //if (!e.Cancelled)
            //{
            //    if (e.Result != null)
            //    {
            //        List<T_SYS_ENTITYMENU> AA = e.Result.ToList();
            //        this.DaGrFB.ItemsSource = AA;
            //        LoadDaGrFBDataRange();

            //    }

            //}
            #endregion
          
            loadbar.Stop();
        }

        
        void RoleClient_GetEntityMenuCustomPermByTypeCompleted(object sender, GetEntityMenuCustomPermByTypeCompletedEventArgs e)
        { 

        }

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
        
        private Items[] comboBoxItem;

        public Items[] ComboBoxItem
        {
            get { return comboBoxItem; }
            set { comboBoxItem = value; }
        }

        #endregion

        #region IEntityEditor
        public string GetTitle()
        {
            return Utility.GetResourceStr("ROLESETPERMISSION");
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

        

        public List<NavigateItem> GetLeftMenuItems()
        {
            List<NavigateItem> items = new List<NavigateItem>();
            NavigateItem item = new NavigateItem
            {
                Title = "角色权限",
                Tooltip = "角色权限信息"
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
                Key = "1",
                Title = Utility.GetResourceStr("SAVEANDCLOSE"),
                ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_saveClose.png"
            };

            items.Add(item);

            item = new ToolbarItem
            {
                DisplayType = ToolbarItemDisplayTypes.Image,
                Key = "0",
                Title = Utility.GetResourceStr("SAVE"),
                ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_SAVE.png"
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

        #region 保存
        private void Save()
        {
            try
            {
                
                
                #region OA系统
                if (DaGrOA.ItemsSource != null)
                {
                    //////////////////////////////////////////////////////
                    //if (DaGrOA.SelectedItems.Count > 0)
                    //{
                    foreach (object obj in DaGrOA.ItemsSource)
                    {
                        T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();

                        if (DaGrOA.Columns[0].GetCellContent(obj) != null)
                        {

                            //tmpAllList += menu.T_SYS_ENTITYMENU2.ENTITYMENUID + ",";
                            CheckBox cb1 = DaGrOA.Columns[0].GetCellContent(obj).FindName("myChkBtn") as CheckBox; //cb为
                            //if (cb1.IsChecked == true)
                            //{

                            menu = cb1.Tag as T_SYS_ENTITYMENU;
                            //tmpAllList += menu.T_SYS_ENTITYMENU2.ENTITYMENUID + ",";
                            string StrMenuID = ""; //菜单ID
                            StrMenuID = menu.ENTITYMENUID;
                            string StrPermissionID = "";//权限ID
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;
                            int IndexCount = 2;
                            //IndexCount = PermCount
                            bool IsCheckRange = false;
                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                IndexCount = IndexCount + i;
                                if (DaGrOA.Columns[i].GetCellContent(obj) != null)
                                {
                                    string StrDataRange = "";
                                    /*
                                     * 
                                     * 首先根据菜单ID和角色ID获取  角色菜单表的记录 存在则获取ROLEENTITYID
                                     * 然后再根据 ROLEENTITYID和权限ID 获取角色菜单权限表中的记录，取得对应的权限和现在的权限值比较
                                     * 如果相同则不处理  不同则处理                                     * 
                                     * 
                                     * 
                                     
                                     
                                     
                                     */
                                    
                                    
                                    StrPermissionID = tmpPermission[i - 2].PERMISSIONID;  //权限ID
                                    var q = from a in EntityPermissionInfosList
                                            where a.EntityRole.T_SYS_ENTITYMENU.ENTITYMENUID == StrMenuID && a.EntityRole.T_SYS_ROLE.ROLEID == tmprole.ROLEID

                                            select a;
                                    string RoleEntityID = "";
                                    if (q.Count() > 0)
                                    {
                                        RoleEntityID = q.ToList().FirstOrDefault().EntityRole.ROLEENTITYMENUID.ToString(); //获取角色菜单ID
                                    }
                                    //var m = from a in EntityPermissionInfosList
                                    var k = from b in EntityPermissionInfosList
                                            where b.RoleMenuPermission.T_SYS_PERMISSION.PERMISSIONID == StrPermissionID && b.RoleMenuPermission.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == RoleEntityID
                                            select b;
                                    string DataRangeValue = ""; //获取数据库中 权限对应的值

                                    if (k.Count() > 0)
                                    {
                                        DataRangeValue = k.ToList().FirstOrDefault().RoleMenuPermission.DATARANGE.ToString();
                                    }
                                    /***********************************************************************************************/
                                    //Rating hrrate = DaGrOA.Columns[i].GetCellContent(obj).FindName("OArating") as Rating;
                                    Button hrrate = DaGrOA.Columns[i].GetCellContent(obj).FindName("OArating") as Button;
                                    //switch ((int)(hrrate.Value * 10))
                                    switch(hrrate.Content.ToString())
                                    {
                                        case "★"://员工 0.2 ×10
                                            StrDataRange = "4";
                                            break;
                                        case "★★"://岗位 0.4×10
                                            StrDataRange = "3";
                                            break;
                                        case "★★★"://部门 0.6*10
                                            StrDataRange = "2";
                                            break;
                                        case "★★★★"://公司 0.8*10
                                            StrDataRange = "1";
                                            break;
                                        case "★★★★★"://集团 1.0*10
                                            StrDataRange = "0";
                                            break;

                                    }
                                    if (DataRangeValue != StrDataRange)
                                    {
                                        tmpAllList += StrDataRange + ",";
                                        tmpAllList += tmpPermission[i - 2].PERMISSIONID + ";";
                                        IsCheckRange = true;
                                    }
                                    else
                                    {
                                        tmpAllList += "" + "," + ";";
                                    }
                                    
                                }

                            }
                            if (IsCheckRange)
                            {
                                
                                menuInfosList.Add(menu);
                                if (menu.T_SYS_ENTITYMENU2 != null)
                                {
                                    tmpAllList += "@" + menu.ENTITYMENUID + "," + menu.T_SYS_ENTITYMENU2.ENTITYMENUID;
                                }
                                else
                                {
                                    tmpAllList += "@" + menu.ENTITYMENUID + "," + "";
                                }
                                tmpAllList += "#";
                            }
                            else
                            {
                                if (tmpAllList.Length != 0)
                                {
                                    int SubStringLength = 2 * PermCount;
                                    tmpAllList = tmpAllList.Substring(0, tmpAllList.Length - SubStringLength);
                                }
                            }



                            //}//if (cb1.IsChecked == true)

                        }
                    }
                    //}// end count
                    

                    

                }
                #endregion
                #region HR系统
                if (DaGrHR.ItemsSource != null)
                {
                    foreach (object obj in DaGrHR.ItemsSource)
                    {
                        T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();

                        if (DaGrHR.Columns[0].GetCellContent(obj) != null)
                        {

                            //tmpAllList += menu.T_SYS_ENTITYMENU2.ENTITYMENUID + ",";
                            CheckBox cb1 = DaGrHR.Columns[0].GetCellContent(obj).FindName("myChkBtnHR") as CheckBox; //cb为
                            //if (cb1.IsChecked == true)
                            //{

                            menu = cb1.Tag as T_SYS_ENTITYMENU;
                            //tmpAllList += menu.T_SYS_ENTITYMENU2.ENTITYMENUID + ",";
                            string StrMenuID = ""; //菜单ID
                            StrMenuID = menu.ENTITYMENUID;
                            string StrPermissionID = "";//权限ID
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;
                            int IndexCount = 2;
                            //IndexCount = PermCount
                            bool IsCheckRange = false;//是否选择了权限范围
                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                IndexCount = IndexCount + i;
                                if (DaGrHR.Columns[i].GetCellContent(obj) != null)
                                {
                                    string StrDataRange = "";
                                    /*
                                     * 
                                     * 首先根据菜单ID和角色ID获取  角色菜单表的记录 存在则获取ROLEENTITYID
                                     * 然后再根据 ROLEENTITYID和权限ID 获取角色菜单权限表中的记录，取得对应的权限和现在的权限值比较
                                     * 如果相同则不处理  不同则处理                                     * 
                                     * 
                                     * 
                                     
                                     
                                     
                                     */
                                    
                                    
                                    StrPermissionID = tmpPermission[i - 2].PERMISSIONID;  //权限ID
                                    var q = from a in EntityPermissionInfosList
                                            where a.EntityRole.T_SYS_ENTITYMENU.ENTITYMENUID == StrMenuID && a.EntityRole.T_SYS_ROLE.ROLEID == tmprole.ROLEID

                                            select a;
                                    string RoleEntityID = "";
                                    if (q.Count() > 0)
                                    {
                                        RoleEntityID = q.ToList().FirstOrDefault().EntityRole.ROLEENTITYMENUID.ToString(); //获取角色菜单ID
                                    }
                                    //var m = from a in EntityPermissionInfosList
                                    var k = from b in EntityPermissionInfosList
                                            where b.RoleMenuPermission.T_SYS_PERMISSION.PERMISSIONID == StrPermissionID && b.RoleMenuPermission.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == RoleEntityID
                                            select b;
                                    string DataRangeValue = ""; //获取数据库中 权限对应的值

                                    if (k.Count() > 0)
                                    {
                                        DataRangeValue = k.ToList().FirstOrDefault().RoleMenuPermission.DATARANGE.ToString();
                                    }
                                    //Rating hrrate = DaGrHR.Columns[i].GetCellContent(obj).FindName("HRrating") as Rating;
                                    Button hrrate = DaGrHR.Columns[i].GetCellContent(obj).FindName("HRrating") as Button;                                    
                                    switch (hrrate.Content.ToString())
                                    {
                                        case "★"://员工 0.2 ×10
                                            StrDataRange = "4";
                                            break;
                                        case "★★"://岗位 0.4×10
                                            StrDataRange = "3";
                                            break;
                                        case "★★★"://部门 0.6*10
                                            StrDataRange = "2";
                                            break;
                                        case "★★★★"://公司 0.8*10
                                            StrDataRange = "1";
                                            break;
                                        case "★★★★★"://集团 1.0*10
                                            StrDataRange = "0";
                                            break;
                                            

                                    }
                                    if (DataRangeValue != StrDataRange)
                                    {
                                        tmpAllList += StrDataRange + ",";
                                        tmpAllList += tmpPermission[i - 2].PERMISSIONID + ";";
                                        IsCheckRange = true;
                                    }
                                    else
                                    {
                                        tmpAllList += "" + "," + ";";
                                    }
                                }

                            }
                            if (IsCheckRange)
                            {
                                
                                menuInfosList.Add(menu);
                                if (menu.T_SYS_ENTITYMENU2 != null)
                                {
                                    tmpAllList += "@" + menu.ENTITYMENUID + "," + menu.T_SYS_ENTITYMENU2.ENTITYMENUID;
                                }
                                else
                                {
                                    tmpAllList += "@" + menu.ENTITYMENUID + "," + "";
                                }
                                tmpAllList += "#";
                            }
                            else
                            {
                                if (tmpAllList.Length != 0)
                                {
                                    int SubStringLength = 2 * PermCount;
                                    tmpAllList = tmpAllList.Substring(0, tmpAllList.Length - SubStringLength);
                                }
                            }



                            //}//if (cb1.IsChecked == true)

                        }
                    }

                    
                    
                    
                }
                #endregion
                #region 物流系统
                if (DaGrHR.ItemsSource != null)
                {
                    foreach (object obj in DaGrLM.ItemsSource)
                    {
                        T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();

                        if (DaGrLM.Columns[0].GetCellContent(obj) != null)
                        {

                            //tmpAllList += menu.T_SYS_ENTITYMENU2.ENTITYMENUID + ",";
                            CheckBox cb1 = DaGrLM.Columns[0].GetCellContent(obj).FindName("myChkBtnLM") as CheckBox; //cb为
                            //if (cb1.IsChecked == true)
                            //{

                            menu = cb1.Tag as T_SYS_ENTITYMENU;
                            //tmpAllList += menu.T_SYS_ENTITYMENU2.ENTITYMENUID + ",";
                            string StrMenuID = ""; //菜单ID
                            StrMenuID = menu.ENTITYMENUID;
                            string StrPermissionID = "";//权限ID
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;
                            int IndexCount = 2;
                            //IndexCount = PermCount
                            bool IsCheckRange = false;//是否选择了权限范围
                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                IndexCount = IndexCount + i;
                                if (DaGrLM.Columns[i].GetCellContent(obj) != null)
                                {
                                    string StrDataRange = "";
                                    /*
                                     * 
                                     * 首先根据菜单ID和角色ID获取  角色菜单表的记录 存在则获取ROLEENTITYID
                                     * 然后再根据 ROLEENTITYID和权限ID 获取角色菜单权限表中的记录，取得对应的权限和现在的权限值比较
                                     * 如果相同则不处理  不同则处理                                     * 
                                     * 
                                     * 
                                     
                                     
                                     
                                     */
                                    //Rating hrrate = DaGrFB.Columns[i].GetCellContent(obj).FindName("FBrating") as Rating;
                                    
                                    StrPermissionID = tmpPermission[i - 2].PERMISSIONID;  //权限ID
                                    var q = from a in EntityPermissionInfosList
                                            where a.EntityRole.T_SYS_ENTITYMENU.ENTITYMENUID == StrMenuID && a.EntityRole.T_SYS_ROLE.ROLEID == tmprole.ROLEID

                                            select a;
                                    string RoleEntityID = "";
                                    if (q.Count() > 0)
                                    {
                                        RoleEntityID = q.ToList().FirstOrDefault().EntityRole.ROLEENTITYMENUID.ToString(); //获取角色菜单ID
                                    }
                                    //var m = from a in EntityPermissionInfosList
                                    var k = from b in EntityPermissionInfosList
                                            where b.RoleMenuPermission.T_SYS_PERMISSION.PERMISSIONID == StrPermissionID && b.RoleMenuPermission.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == RoleEntityID
                                            select b;
                                    string DataRangeValue = ""; //获取数据库中 权限对应的值

                                    if (k.Count() > 0)
                                    {
                                        DataRangeValue = k.ToList().FirstOrDefault().RoleMenuPermission.DATARANGE.ToString();
                                    }
                                    //Rating LMrate = DaGrLM.Columns[i].GetCellContent(obj).FindName("LMrating") as Rating;
                                    Button LMrate = DaGrLM.Columns[i].GetCellContent(obj).FindName("LMrating") as Button;
                                    switch (LMrate.Content.ToString())
                                    {
                                        case "★"://员工 0.2 ×10
                                            StrDataRange = "4";
                                            break;
                                        case "★★"://岗位 0.4×10
                                            StrDataRange = "3";
                                            break;
                                        case "★★★"://部门 0.6*10
                                            StrDataRange = "2";
                                            break;
                                        case "★★★★"://公司 0.8*10
                                            StrDataRange = "1";
                                            break;
                                        case "★★★★★"://集团 1.0*10
                                            StrDataRange = "0";
                                            break;


                                    }
                                    if (DataRangeValue != StrDataRange)
                                    {
                                        tmpAllList += StrDataRange + ",";
                                        tmpAllList += tmpPermission[i - 2].PERMISSIONID + ";";
                                        IsCheckRange = true;
                                    }
                                    else
                                    {
                                        tmpAllList += "" + "," + ";";
                                    }
                                }

                            }
                            if (IsCheckRange)
                            {
                                
                                menuInfosList.Add(menu);
                                if (menu.T_SYS_ENTITYMENU2 != null)
                                {
                                    tmpAllList += "@" + menu.ENTITYMENUID + "," + menu.T_SYS_ENTITYMENU2.ENTITYMENUID;
                                }
                                else
                                {
                                    tmpAllList += "@" + menu.ENTITYMENUID + "," + "";
                                }
                                tmpAllList += "#";
                            }
                            else
                            {
                                if (tmpAllList.Length != 0)
                                {
                                    int SubStringLength = 2 * PermCount;
                                    tmpAllList = tmpAllList.Substring(0, tmpAllList.Length - SubStringLength);
                                }
                            }



                            //}//if (cb1.IsChecked == true)

                        }
                    }




                }
                #endregion
                #region 预算系统
                if (DaGrFB.ItemsSource != null)
                {
                    foreach (object obj in DaGrFB.ItemsSource)
                    {
                        T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();

                        if (DaGrFB.Columns[0].GetCellContent(obj) != null)
                        {
                            CheckBox cb1 = DaGrFB.Columns[0].GetCellContent(obj).FindName("myChkBtnFB") as CheckBox; //cb为
                            //if (cb1.IsChecked == true)
                            //{

                            menu = cb1.Tag as T_SYS_ENTITYMENU;
                            //tmpAllList += menu.T_SYS_ENTITYMENU2.ENTITYMENUID + ",";
                            string StrMenuID = ""; //菜单ID
                            StrMenuID = menu.ENTITYMENUID;
                            string StrPermissionID = "";//权限ID
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;
                            int IndexCount = 2;
                            //IndexCount = PermCount
                            bool IsCheckRange = false;//是否选择了权限范围
                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                IndexCount = IndexCount + i;
                                if (DaGrFB.Columns[i].GetCellContent(obj) != null)
                                {
                                    string StrDataRange = "";
                                    /*
                                     * 
                                     * 首先根据菜单ID和角色ID获取  角色菜单表的记录 存在则获取ROLEENTITYID
                                     * 然后再根据 ROLEENTITYID和权限ID 获取角色菜单权限表中的记录，取得对应的权限和现在的权限值比较
                                     * 如果相同则不处理  不同则处理                                     * 
                                     * 
                                     * 
                                     
                                     
                                     
                                     */
                                    //Rating hrrate = DaGrFB.Columns[i].GetCellContent(obj).FindName("FBrating") as Rating;
                                    Button hrrate = DaGrFB.Columns[i].GetCellContent(obj).FindName("FBrating") as Button;
                                    StrPermissionID = tmpPermission[i - 2].PERMISSIONID;  //权限ID
                                    var q = from a in EntityPermissionInfosList
                                            where a.EntityRole.T_SYS_ENTITYMENU.ENTITYMENUID == StrMenuID && a.EntityRole.T_SYS_ROLE.ROLEID == tmprole.ROLEID

                                            select a;
                                    string RoleEntityID = "";
                                    if (q.Count() > 0)
                                    {
                                        RoleEntityID = q.ToList().FirstOrDefault().EntityRole.ROLEENTITYMENUID.ToString(); //获取角色菜单ID
                                    }
                                    //var m = from a in EntityPermissionInfosList
                                    var k = from b in EntityPermissionInfosList
                                            where b.RoleMenuPermission.T_SYS_PERMISSION.PERMISSIONID == StrPermissionID && b.RoleMenuPermission.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == RoleEntityID
                                            select b;
                                    string DataRangeValue = ""; //获取数据库中 权限对应的值
                                    
                                    if (k.Count() > 0)
                                    { 
                                        DataRangeValue = k.ToList().FirstOrDefault().RoleMenuPermission.DATARANGE.ToString();
                                    }
                                    switch (hrrate.Content.ToString())
                                    {
                                        case "★"://员工 0.2 ×10
                                            StrDataRange = "4";
                                            break;
                                        case "★★"://岗位 0.4×10
                                            StrDataRange = "3";
                                            break;
                                        case "★★★"://部门 0.6*10
                                            StrDataRange = "2";
                                            break;
                                        case "★★★★"://公司 0.8*10
                                            StrDataRange = "1";
                                            break;
                                        case "★★★★★"://集团 1.0*10
                                            StrDataRange = "0";
                                            break;

                                    }
                                    if (DataRangeValue != StrDataRange)
                                    {
                                        tmpAllList += StrDataRange + ",";
                                        tmpAllList += tmpPermission[i - 2].PERMISSIONID + ";";
                                        IsCheckRange = true;
                                    }
                                    else
                                    {
                                        tmpAllList += "" + "," + ";";
                                    }
                                }

                            }
                            if (IsCheckRange)
                            {
                                
                                menuInfosList.Add(menu);
                                if (menu.T_SYS_ENTITYMENU2 != null)
                                {
                                    tmpAllList += "@" + menu.ENTITYMENUID + "," + menu.T_SYS_ENTITYMENU2.ENTITYMENUID;
                                }
                                else
                                {
                                    tmpAllList += "@" + menu.ENTITYMENUID + "," + "";
                                }
                                tmpAllList += "#";
                            }
                            else
                            {                                
                                if (tmpAllList.Length != 0)
                                {
                                    int SubStringLength = 2 * PermCount;
                                    tmpAllList = tmpAllList.Substring(0, tmpAllList.Length - SubStringLength);
                                }
                            }



                            //}//if (cb1.IsChecked == true)

                        }
                    }




                }
                #endregion
                
                if (!string.IsNullOrEmpty(tmpAllList))
                {
                    loadbar.Start();
                    tmpAllList = tmpAllList.Substring(0, tmpAllList.Length - 1);
                }
                if (IsAdd)
                {
                    if (string.IsNullOrEmpty(tmpAllList))
                    {
                        ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), "请选择菜单", Utility.GetResourceStr("CONFIRMBUTTON"));

                        return;
                    }
                    
                }
                
                RoleClient.BatchAddRoleEntityPermissionInfosAsync(tmpAllList, "admin", tmprole.ROLEID);
                
                

            }
            catch (Exception ex)
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), ex.ToString(), Utility.GetResourceStr("CONFIRMBUTTON"));
            }
        }

        void RoleClient_BatchAddRoleEntityPermissionInfosCompleted(object sender, BatchAddRoleEntityPermissionInfosCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                loadbar.Stop();
                if (e.Result)
                {
                    
                    ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), "设置成功", Utility.GetResourceStr("CONFIRMBUTTON"));
           
                    RefreshUI(RefreshedTypes.CloseAndReloadData);
                    
                }
                else
                {
                    ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), "设置失败", Utility.GetResourceStr("CONFIRMBUTTON"));
                    return;
                }
            }
        }

        private void SaveAndClose()
        {
            //saveType = "1";
            Save();
        }

        #endregion

        private void DaGrYS_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void DaGrHR_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_SYS_ENTITYMENU MenuInfoT = (T_SYS_ENTITYMENU)e.Row.DataContext;
            CheckBox mychkBox = DaGrHR.Columns[0].GetCellContent(e.Row).FindName("myChkBtnHR") as CheckBox;
            Button myhrBtn = DaGrHR.Columns[1].GetCellContent(e.Row).FindName("HRBtn") as Button;
            
            myhrBtn.Tag = e;
            mychkBox.Tag = MenuInfoT;
            
        }

        private void DaGrOA_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_SYS_ENTITYMENU MenuInfoT = (T_SYS_ENTITYMENU)e.Row.DataContext;
            CheckBox mychkBox = DaGrOA.Columns[0].GetCellContent(e.Row).FindName("myChkBtn") as CheckBox;
            Button myoaBtn = DaGrOA.Columns[1].GetCellContent(e.Row).FindName("OABtn") as Button;
            
            myoaBtn.Tag = e;
            mychkBox.Tag = MenuInfoT;

        }


        private void SetButtonEnabled()
        { 

        }
        private void myChkBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void myChkBtnHR_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Children_Loaded(object sender, RoutedEventArgs e)
        {
            string ba = "aaaa";
            string cc = "";
        }

        private void DaGrYS_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            
        }

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }
        //自动产生列
        private void Children_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string ColumnName = "";
            string currentHeader = e.Column.Header.ToString();
            foreach (var aa in tmpPermission)
            {
                DataGridTemplateColumn tc = new DataGridTemplateColumn();
                tc.Header = aa.PERMISSIONNAME;
                
                //tc.CellTemplate =(DataTemplate)
                e.Column = tc;
            }
        }

        #region OA权限填充
        //private void FillOADataRange()
        //{
        //    if (tmpEditRoleEntityPermList.Count() == 0) return;
        //    if (DaGrOA.ItemsSource != null)
        //    {
        //        foreach (object obj in DaGrOA.ItemsSource)
        //        {
        //            T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();
        //            if (DaGrOA.Columns[0].GetCellContent(obj) != null)
        //            {
        //                CheckBox cb1 = DaGrOA.Columns[0].GetCellContent(obj).FindName("myChkBtn") as CheckBox; //cb为
        //                //cb1.Tag
        //                menu = cb1.Tag as T_SYS_ENTITYMENU;
        //                var bb = from a in tmpEditRoleEntityLIst
        //                         where a.T_SYS_ENTITYMENU.ENTITYMENUID == menu.ENTITYMENUID
        //                         select a;
        //                if (bb.Count() > 0)
        //                {
        //                    cb1.IsChecked = true;
        //                    T_SYS_ROLEENTITYMENU tmpRoleEntity = new T_SYS_ROLEENTITYMENU();
        //                    tmpRoleEntity = bb.FirstOrDefault();
        //                    int PermCount = 0;
        //                    PermCount = tmpPermission.Count;
        //                    int IndexCount = 2;
        //                    //IndexCount = PermCount
        //                    for (int i = 2; i < PermCount + 2; i++)
        //                    {
        //                        IndexCount = IndexCount + i;
        //                        if (DaGrOA.Columns[i].GetCellContent(obj) != null)
        //                        {
        //                            T_SYS_PERMISSION tmpPerm = new T_SYS_PERMISSION();
        //                            tmpPerm = tmpPermission[i - 2];
        //                            var roles = from cc in tmpEditRoleEntityPermList
        //                                        where cc.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == tmpRoleEntity.ROLEENTITYMENUID && cc.T_SYS_PERMISSION.PERMISSIONID == tmpPerm.PERMISSIONID
        //                                        //where cc.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == tmpRoleEntity.ROLEENTITYMENUID 
        //                                        select cc;
        //                            if (roles.Count() > 0)
        //                            {
        //                                //Rating hrrate = DaGrOA.Columns[i].GetCellContent(obj).FindName("HRrating") as Rating;
        //                                Button hrrate = DaGrOA.Columns[i].GetCellContent(obj).FindName("OArating") as Button;
        //                                switch (roles.FirstOrDefault().DATARANGE)
        //                                {
        //                                    case "0"://集团
        //                                        //hrrate.Value = 1;
        //                                        hrrate.Content = "★★★★★";
        //                                        break;
        //                                    case "1"://公司
        //                                        //hrrate.Value = 0.8;
        //                                        hrrate.Content = "★★★★";
        //                                        break;
        //                                    case "2"://部门
        //                                        //hrrate.Value = 0.6;
        //                                        hrrate.Content = "★★★";
        //                                        break;
        //                                    case "3"://岗位
        //                                        //hrrate.Value = 0.4;
        //                                        hrrate.Content = "★★";
        //                                        break;
        //                                    case "4"://个人
        //                                        //hrrate.Value = 0.2;
        //                                        hrrate.Content = "★";
        //                                        break;


        //                                }
        //                                //hrrate.Value =
        //                            }

        //                        }// if DaGrOA.columns

        //                    }// for int i

        //                }//bb.cout


        //            }
        //        }//foreach(DaGrOA)
        //    }   //foreach (object obj in DaGrOA.ItemsSource)

        //}

        #endregion

        #region 填充 权限


        private void FillPermissionDataRange(DataGrid dg, string chx, string btn)
        {
            #region 龙康才注
            
            if (tmpEditRoleEntityPermList.Count() == 0) return;
            if (dg.Columns.Count < tmpPermission.Count) return;//还没有动态生成列
            if (dg.ItemsSource != null)
            {
                foreach (object obj in dg.ItemsSource)
                {
                    T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();
                    if (dg.Columns[0].GetCellContent(obj) != null)
                    {
                        CheckBox cb1 = dg.Columns[0].GetCellContent(obj).FindName(chx) as CheckBox; //cb为
                        if (cb1==null) continue;
                        //cb1.Tag
                        menu = cb1.Tag as T_SYS_ENTITYMENU;
                        var bb = from a in tmpEditRoleEntityLIst
                                 where a.T_SYS_ENTITYMENU.ENTITYMENUID == menu.ENTITYMENUID
                                 select a;
                        if (bb.Count() > 0)
                        {
                            cb1.IsChecked = true;
                            T_SYS_ROLEENTITYMENU tmpRoleEntity = new T_SYS_ROLEENTITYMENU();
                            tmpRoleEntity = bb.FirstOrDefault();
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;
                            int IndexCount = 2;
                            //IndexCount = PermCount
                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                IndexCount = IndexCount + i;
                                if (dg.Columns[i].GetCellContent(obj) != null)
                                {
                                    T_SYS_PERMISSION tmpPerm = new T_SYS_PERMISSION();
                                    tmpPerm = tmpPermission[i - 2];
                                    var roles = from cc in tmpEditRoleEntityPermList
                                                where cc.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == tmpRoleEntity.ROLEENTITYMENUID && cc.T_SYS_PERMISSION.PERMISSIONID == tmpPerm.PERMISSIONID
                                                //where cc.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == tmpRoleEntity.ROLEENTITYMENUID 
                                                select cc;
                                    if (roles.Count() > 0)
                                    {
                                        //Rating hrrate = dg.Columns[i].GetCellContent(obj).FindName("HRrating") as Rating;
                                        Button hrrate = dg.Columns[i].GetCellContent(obj).FindName(btn) as Button;

                                        switch (roles.FirstOrDefault().DATARANGE)
                                        {
                                            case "0"://集团
                                                //hrrate.Value = 1;
                                                hrrate.Content = "★★★★★";
                                                break;
                                            case "1"://公司
                                                //hrrate.Value = 0.8;
                                                hrrate.Content = "★★★★";
                                                break;
                                            case "2"://部门
                                                //hrrate.Value = 0.6;
                                                hrrate.Content = "★★★";
                                                break;
                                            case "3"://岗位
                                                //hrrate.Value = 0.4;
                                                hrrate.Content = "★★";
                                                break;
                                            case "4"://个人
                                                //hrrate.Value = 0.2;
                                                hrrate.Content = "★";
                                                break;


                                        }
                                        //hrrate.Value =
                                    }

                                }// if dg.columns

                            }// for int i

                        }//bb.cout


                    }
                }//foreach(dg)
            }
            
#endregion           
        }

        /// <summary>
        /// dg DAtagrd 
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="chx">checkbox</param>
        /// <param name="strtxt">textbox</param>
        /// <param name="btn">button</param>
        private void FillPermissionDataRangeNewfill(DataGrid dg, string chx,string btn)
        {
            if (tmpEditRoleEntityPermList.Count() == 0) return;
            if (dg.Columns.Count < tmpPermission.Count) return;//还没有动态生成列
            if (dg.ItemsSource != null)
            {
                foreach (object obj in dg.ItemsSource)
                {
                    T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();
                    if (dg.Columns[0].GetCellContent(obj) != null)
                    {
                        CheckBox cb1 = dg.Columns[0].GetCellContent(obj).FindName(chx) as CheckBox; //cb为
                        if (cb1 == null) continue;
                        //cb1.Tag
                        menu = cb1.Tag as T_SYS_ENTITYMENU;
                        var bb = from a in tmpEditRoleEntityLIst
                                 where a.T_SYS_ENTITYMENU.ENTITYMENUID == menu.ENTITYMENUID
                                 select a;
                        if (bb.Count() > 0)
                        {
                            cb1.IsChecked = true;
                            T_SYS_ROLEENTITYMENU tmpRoleEntity = new T_SYS_ROLEENTITYMENU();
                            tmpRoleEntity = bb.FirstOrDefault();
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;
                            int IndexCount = 2;
                            //IndexCount = PermCount
                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                IndexCount = IndexCount + i;
                                if (dg.Columns[i].GetCellContent(obj) != null)
                                {
                                    T_SYS_PERMISSION tmpPerm = new T_SYS_PERMISSION();
                                    tmpPerm = tmpPermission[i - 2];
                                    var roles = from cc in tmpEditRoleEntityPermList
                                                where cc.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == tmpRoleEntity.ROLEENTITYMENUID && cc.T_SYS_PERMISSION.PERMISSIONID == tmpPerm.PERMISSIONID
                                                //where cc.T_SYS_ROLEENTITYMENU.ROLEENTITYMENUID == tmpRoleEntity.ROLEENTITYMENUID 
                                                select cc;
                                    if (roles.Count() > 0)
                                    {
                                        //Rating hrrate = dg.Columns[i].GetCellContent(obj).FindName("HRrating") as Rating;
                                        Button hrrate = dg.Columns[i].GetCellContent(obj).FindName(btn) as Button;

                                        switch (roles.FirstOrDefault().DATARANGE)
                                        {
                                            case "0"://集团
                                                //hrrate.Value = 1;
                                                hrrate.Content = "★★★★★";
                                                break;
                                            case "1"://公司
                                                //hrrate.Value = 0.8;
                                                hrrate.Content = "★★★★";
                                                break;
                                            case "2"://部门
                                                //hrrate.Value = 0.6;
                                                hrrate.Content = "★★★";
                                                break;
                                            case "3"://岗位
                                                //hrrate.Value = 0.4;
                                                hrrate.Content = "★★";
                                                break;
                                            case "4"://个人
                                                //hrrate.Value = 0.2;
                                                hrrate.Content = "★";
                                                break;


                                        }
                                        //hrrate.Value =
                                    }

                                }// if dg.columns

                            }// for int i

                        }//bb.cout


                    }
                }//foreach(dg)
            }
        }
        #endregion

        private void DaGrOA_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("oa loaded");
            loadbar.Start();
            if (tmpEditRoleEntityPermList.Count == 0)
            {
                RoleClient.GetRolePermsAsync(tmpRoleEntityIDsList);
            }
            else
            {
                FillPermissionDataRange(DaGrOA, "myChkBtn", "OArating");
                loadbar.Stop();
            }
        }

        private void DaGrHR_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("hr loaded");
            loadbar.Start();
            if (tmpEditRoleEntityPermList.Count == 0)
            {
                RoleClient.GetRolePermsAsync(tmpRoleEntityIDsList);
            }
            else
            {
                FillPermissionDataRange(DaGrHR, "myChkBtnHR", "HRrating");
                loadbar.Stop();
            }
        }

        #region 物流授权
        private void DaGrLM_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_SYS_ENTITYMENU MenuInfoT = (T_SYS_ENTITYMENU)e.Row.DataContext;
            CheckBox mychkBox = DaGrLM.Columns[0].GetCellContent(e.Row).FindName("myChkBtnLM") as CheckBox;
            Button mylmBtn = DaGrLM.Columns[1].GetCellContent(e.Row).FindName("LMBtn") as Button;
            mylmBtn.Tag = e;
            mychkBox.Tag = MenuInfoT;
        }

        private void FillLMDataRange()
        {
            
        }

        private void DaGrLM_Loaded(object sender, RoutedEventArgs e)
        {
            loadbar.Start();
            if (tmpEditRoleEntityPermList.Count == 0)
            {
                RoleClient.GetRolePermsAsync(tmpRoleEntityIDsList);
            }
            else
            {
                FillPermissionDataRange(DaGrFB, "myChkBtnLM", "LMrating");
                loadbar.Stop();

            }
        }

        private void myChkBtnLM_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region 预算系统
        private void DaGrFB_Loaded(object sender, RoutedEventArgs e)
        {
            loadbar.Start();
            if (tmpEditRoleEntityPermList.Count == 0)
            {
                RoleClient.GetRolePermsAsync(tmpRoleEntityIDsList);
            }
            else
            {
                FillPermissionDataRange(DaGrFB, "myChkBtnFB", "FBrating");
                loadbar.Stop();

            }
            
        }

        private void DaGrFB_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_SYS_ENTITYMENU MenuInfoT = (T_SYS_ENTITYMENU)e.Row.DataContext;
            CheckBox mychkBox = DaGrFB.Columns[0].GetCellContent(e.Row).FindName("myChkBtnFB") as CheckBox;
            Button myoaBtn = DaGrFB.Columns[1].GetCellContent(e.Row).FindName("FBBtn") as Button;
            myoaBtn.Tag = e;
            mychkBox.Tag = MenuInfoT;
        }
        #endregion

        private void myChkBtnFB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HRrating_MouseEnter(object sender, MouseEventArgs e)
        {

        }



        #region 权限按钮设置
        /// <summary>
        /// HR 授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HRrating_Click(object sender, RoutedEventArgs e)
        {
            Button Hrrating = sender as Button;
            string StrContent = "";
            StrContent = Hrrating.Content.ToString();
            
            switch (StrContent)
            { 
                case "":
                    StrContent = "★";
                    break;
                case "★":
                    StrContent = "★★";
                    break;
                case "★★":
                    StrContent = "★★★";
                    break;
                case "★★★":
                    StrContent = "★★★★";
                    break;
                case "★★★★":
                    StrContent = "★★★★★";
                    break;
                case "★★★★★":
                    StrContent = "";
                    break;
            }
            Hrrating.Content = StrContent;
        }

        
        
        private void OArating_Click(object sender, RoutedEventArgs e)
        {
            Button OArating = sender as Button;
            string StrContent = "";
            StrContent = OArating.Content.ToString();

            switch (StrContent)
            {
                case "":
                    StrContent = "★";
                    break;
                case "★":
                    StrContent = "★★";
                    break;
                case "★★":
                    StrContent = "★★★";
                    break;
                case "★★★":
                    StrContent = "★★★★";
                    break;
                case "★★★★":
                    StrContent = "★★★★★";
                    break;
                case "★★★★★":
                    StrContent = "";
                    break;
            }
            OArating.Content = StrContent;
        }

        private void FBrating_Click(object sender, RoutedEventArgs e)
        {
            Button FBrating = sender as Button;
            
            string StrContent = "";
            StrContent = FBrating.Content.ToString();

            switch (StrContent)
            {
                case "":
                    StrContent = "★";
                    break;
                case "★":
                    StrContent = "★★";
                    break;
                case "★★":
                    StrContent = "★★★";
                    break;
                case "★★★":
                    StrContent = "★★★★";
                    break;
                case "★★★★":
                    StrContent = "★★★★★";
                    break;
                case "★★★★★":
                    StrContent = "";
                    break;
            }
            FBrating.Content = StrContent;
        }
        #endregion

        #region 单击1行选中当前行
        private void OABtn_Click(object sender, RoutedEventArgs e)
        {
            
            Button BtnOA = sender as Button;
            if (BtnOA.Tag == null)
            {
                MessageBox.Show("无法选择此行！");
                return;
            }
            DataGridRowEventArgs OArating = BtnOA.Tag as DataGridRowEventArgs;            
            string StrContent = "";
            int PermCount = 0;
            PermCount = tmpPermission.Count;
            
            for (int i = 2; i < PermCount + 2; i++)
            {
                Button mybtn = DaGrOA.Columns[i].GetCellContent(OArating.Row).FindName("OArating") as Button;
                StrContent = mybtn.Content.ToString();
                
                switch (StrContent)
                {
                    case "":
                        StrContent = "★";
                        break;
                    case "★":
                        StrContent = "★★";
                        break;
                    case "★★":
                        StrContent = "★★★";
                        break;
                    case "★★★":
                        StrContent = "★★★★";
                        break;
                    case "★★★★":
                        StrContent = "★★★★★";
                        break;
                    case "★★★★★":
                        StrContent = "";
                        break;
                }
                mybtn.Content = StrContent;
            }
            
        }

        private void HRBtn_Click(object sender, RoutedEventArgs e)
        {
            Button BtnHR = sender as Button;
            if (BtnHR.Tag == null)
            {
                MessageBox.Show("无法选择此行！");
                return;
            }
            DataGridRowEventArgs HRrating = BtnHR.Tag as DataGridRowEventArgs;
            string StrContent = "";
            int PermCount = 0;
            PermCount = tmpPermission.Count;
            
            for (int i = 2; i < PermCount + 2; i++)
            {
                Button mybtn = DaGrHR.Columns[i].GetCellContent(HRrating.Row).FindName("HRrating") as Button;
                StrContent = mybtn.Content.ToString();

                switch (StrContent)
                {
                    case "":
                        StrContent = "★";
                        break;
                    case "★":
                        StrContent = "★★";
                        break;
                    case "★★":
                        StrContent = "★★★";
                        break;
                    case "★★★":
                        StrContent = "★★★★";
                        break;
                    case "★★★★":
                        StrContent = "★★★★★";
                        break;
                    case "★★★★★":
                        StrContent = "";
                        break;
                }
                mybtn.Content = StrContent;
            }

        }

        private void LMBtn_Click(object sender, RoutedEventArgs e)
        {
            Button BtnLM = sender as Button;
            if (BtnLM.Tag == null)
            {
                MessageBox.Show("无法选择此行！");
                return;
            }
            DataGridRowEventArgs LMrating = BtnLM.Tag as DataGridRowEventArgs;
            string StrContent = "";
            int PermCount = 0;
            PermCount = tmpPermission.Count;

            for (int i = 2; i < PermCount + 2; i++)
            {
                Button mybtn = DaGrLM.Columns[i].GetCellContent(LMrating.Row).FindName("LMrating") as Button;
                StrContent = mybtn.Content.ToString();

                switch (StrContent)
                {
                    case "":
                        StrContent = "★";
                        break;
                    case "★":
                        StrContent = "★★";
                        break;
                    case "★★":
                        StrContent = "★★★";
                        break;
                    case "★★★":
                        StrContent = "★★★★";
                        break;
                    case "★★★★":
                        StrContent = "★★★★★";
                        break;
                    case "★★★★★":
                        StrContent = "";
                        break;
                }
                mybtn.Content = StrContent;
            }
        }


        private void FBBtn_Click(object sender, RoutedEventArgs e)
        {
            Button BtnFB = sender as Button;
            if (BtnFB.Tag == null)
            {
                MessageBox.Show("无法选择此行！");
                return;
            }
            DataGridRowEventArgs FBrating = BtnFB.Tag as DataGridRowEventArgs;
            string StrContent = "";
            int PermCount = 0;
            PermCount = tmpPermission.Count;

            for (int i = 2; i < PermCount + 2; i++)
            {
                Button mybtn = DaGrFB.Columns[i].GetCellContent(FBrating.Row).FindName("FBrating") as Button;
                StrContent = mybtn.Content.ToString();

                switch (StrContent)
                {
                    case "":
                        StrContent = "★";
                        break;
                    case "★":
                        StrContent = "★★";
                        break;
                    case "★★":
                        StrContent = "★★★";
                        break;
                    case "★★★":
                        StrContent = "★★★★";
                        break;
                    case "★★★★":
                        StrContent = "★★★★★";
                        break;
                    case "★★★★★":
                        StrContent = "";
                        break;
                }
                mybtn.Content = StrContent;
            }
        }

        

        #endregion

        #region tablecontrol 

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if(send)
            
            TabControl aa = sender as TabControl;
            
            //if (aa.SelectedIndex == 1)//OA权限
            //{
            //    loadbar.Start();
            //    if (tmpEditRoleEntityPermList.Count == 0)
            //    {
            //        RoleClient.GetRolePermsAsync(tmpRoleEntityIDsList);
            //    }
            //    else
            //    {
            //        FillPermissionDataRange(DaGrOA, "myChkBtn", "OArating");
            //        loadbar.Stop();
            //    }
            //}
            //if (aa.SelectedIndex == 2)//HR权限
            //{
            //    loadbar.Start();
            //    if (tmpEditRoleEntityPermList.Count == 0)
            //    {
            //        RoleClient.GetRolePermsAsync(tmpRoleEntityIDsList);
            //    }
            //    else
            //    {
            //        FillPermissionDataRange(DaGrHR, "myChkBtnHR", "HRrating");
            //        loadbar.Stop();
            //    }

            //}
            //if (aa.SelectedIndex == 4)//FB权限
            //{
            //    loadbar.Start();
            //    if (tmpEditRoleEntityPermList.Count == 0)
            //    {
            //        RoleClient.GetRolePermsAsync(tmpRoleEntityIDsList);
            //    }
            //    else
            //    {
            //        FillPermissionDataRange(DaGrFB, "myChkBtnFB", "FBrating");
            //        loadbar.Stop();

            //    }

                
            //}

            SelectIndex = aa.SelectedIndex;
        }
        #endregion

        private void LMrating_Click(object sender, RoutedEventArgs e)
        {
            Button LMrating = sender as Button;
            string StrContent = "";
            StrContent = LMrating.Content.ToString();

            switch (StrContent)
            {
                case "":
                    StrContent = "★";
                    break;
                case "★":
                    StrContent = "★★";
                    break;
                case "★★":
                    StrContent = "★★★";
                    break;
                case "★★★":
                    StrContent = "★★★★";
                    break;
                case "★★★★":
                    StrContent = "★★★★★";
                    break;
                case "★★★★★":
                    StrContent = "";
                    break;
            }
            LMrating.Content = StrContent;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image CBAll = sender as Image;
            TabControl RoleTabControl = new TabControl();
            RoleTabControl = tabrolemenu;

            if (RoleTabControl.SelectedIndex == 1)//OA权限
            {
                if (DaGrOA.ItemsSource != null)
                {
                    //////////////////////////////////////////////////////
                    
                    foreach (object obj in DaGrOA.ItemsSource)
                    {
                        T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();

                        if (DaGrOA.Columns[0].GetCellContent(obj) != null)
                        {
                            
                            string StrContent = "";
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;

                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                Button mybtn = DaGrOA.Columns[i].GetCellContent(obj).FindName("OArating") as Button;
                                StrContent = mybtn.Content.ToString();

                                switch (StrContent)
                                {
                                    case "":
                                        StrContent = "★";
                                        break;
                                    case "★":
                                        StrContent = "★★";
                                        break;
                                    case "★★":
                                        StrContent = "★★★";
                                        break;
                                    case "★★★":
                                        StrContent = "★★★★";
                                        break;
                                    case "★★★★":
                                        StrContent = "★★★★★";
                                        break;
                                    case "★★★★★":
                                        StrContent = "";
                                        break;
                                }
                                mybtn.Content = StrContent;
                            }
                        }
                    }
                }

                //Button BtnOA = sender as Button;
                //if (BtnOA.Tag == null)
                //{
                //    MessageBox.Show("无法选择此行！");
                //    return;
                //}
                
            }
            if (RoleTabControl.SelectedIndex == 2)//HR权限
            {
                if (DaGrHR.ItemsSource != null)
                {
                    //////////////////////////////////////////////////////

                    foreach (object obj in DaGrHR.ItemsSource)
                    {
                        T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();

                        if (DaGrHR.Columns[0].GetCellContent(obj) != null)
                        {

                            string StrContent = "";
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;

                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                Button mybtn = DaGrHR.Columns[i].GetCellContent(obj).FindName("HRrating") as Button;
                                StrContent = mybtn.Content.ToString();

                                switch (StrContent)
                                {
                                    case "":
                                        StrContent = "★";
                                        break;
                                    case "★":
                                        StrContent = "★★";
                                        break;
                                    case "★★":
                                        StrContent = "★★★";
                                        break;
                                    case "★★★":
                                        StrContent = "★★★★";
                                        break;
                                    case "★★★★":
                                        StrContent = "★★★★★";
                                        break;
                                    case "★★★★★":
                                        StrContent = "";
                                        break;
                                }
                                mybtn.Content = StrContent;
                            }
                        }
                    }
                }

            }
            if (RoleTabControl.SelectedIndex == 3)
            {
                if (DaGrLM.ItemsSource != null)
                {
                    //////////////////////////////////////////////////////

                    foreach (object obj in DaGrLM.ItemsSource)
                    {
                        T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();

                        if (DaGrLM.Columns[0].GetCellContent(obj) != null)
                        {

                            string StrContent = "";
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;

                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                Button mybtn = DaGrLM.Columns[i].GetCellContent(obj).FindName("LMrating") as Button;
                                StrContent = mybtn.Content.ToString();

                                switch (StrContent)
                                {
                                    case "":
                                        StrContent = "★";
                                        break;
                                    case "★":
                                        StrContent = "★★";
                                        break;
                                    case "★★":
                                        StrContent = "★★★";
                                        break;
                                    case "★★★":
                                        StrContent = "★★★★";
                                        break;
                                    case "★★★★":
                                        StrContent = "★★★★★";
                                        break;
                                    case "★★★★★":
                                        StrContent = "";
                                        break;
                                }
                                mybtn.Content = StrContent;
                            }
                        }
                    }
                }
            }
            if (RoleTabControl.SelectedIndex == 4)//FB权限
            {
                if (DaGrFB.ItemsSource != null)
                {
                    //////////////////////////////////////////////////////

                    foreach (object obj in DaGrFB.ItemsSource)
                    {
                        T_SYS_ENTITYMENU menu = new T_SYS_ENTITYMENU();

                        if (DaGrFB.Columns[0].GetCellContent(obj) != null)
                        {

                            string StrContent = "";
                            int PermCount = 0;
                            PermCount = tmpPermission.Count;

                            for (int i = 2; i < PermCount + 2; i++)
                            {
                                Button mybtn = DaGrFB.Columns[i].GetCellContent(obj).FindName("FBrating") as Button;
                                StrContent = mybtn.Content.ToString();

                                switch (StrContent)
                                {
                                    case "":
                                        StrContent = "★";
                                        break;
                                    case "★":
                                        StrContent = "★★";
                                        break;
                                    case "★★":
                                        StrContent = "★★★";
                                        break;
                                    case "★★★":
                                        StrContent = "★★★★";
                                        break;
                                    case "★★★★":
                                        StrContent = "★★★★★";
                                        break;
                                    case "★★★★★":
                                        StrContent = "";
                                        break;
                                }
                                mybtn.Content = StrContent;
                            }
                        }
                    }
                }

            }
            var grid = Utility.FindParentControl<DataGrid>(CBAll);
            if (grid != null)
            {
                //GridHelper.HandleAllCheckBoxClick(grid, "checkbox", CBAll.IsChecked.GetValueOrDefault());
            }
        }

        private void PMrating_Click(object sender, RoutedEventArgs e)
        {
            Button PMrating = sender as Button;

            string StrContent = "";
            StrContent = PMrating.Content.ToString();

            switch (StrContent)
            {
                case "":
                    StrContent = "★";
                    break;
                case "★":
                    StrContent = "★★";
                    break;
                case "★★":
                    StrContent = "★★★";
                    break;
                case "★★★":
                    StrContent = "★★★★";
                    break;
                case "★★★★":
                    StrContent = "★★★★★";
                    break;
                case "★★★★★":
                    StrContent = "";
                    break;
            }
            PMrating.Content = StrContent;
        }

        private void myChkBtnPM_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DaGrPM_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_SYS_ENTITYMENU MenuInfoT = (T_SYS_ENTITYMENU)e.Row.DataContext;
            CheckBox mychkBox = DaGrFB.Columns[0].GetCellContent(e.Row).FindName("myChkBtnPM") as CheckBox;
            Button myoaBtn = DaGrFB.Columns[1].GetCellContent(e.Row).FindName("FBBtn") as Button;
            myoaBtn.Tag = e;
            mychkBox.Tag = MenuInfoT;
        }

        private void DaGrPM_Loaded(object sender, RoutedEventArgs e)
        {
            loadbar.Start();
            if (tmpEditRoleEntityPermList.Count == 0)
            {
                RoleClient.GetRolePermsAsync(tmpRoleEntityIDsList);
            }
            else
            {
                FillPermissionDataRange(DaGrPM, "myChkBtnPM", "PMrating");
                loadbar.Stop();

            }
        }

        private void PMBtn_Click(object sender, RoutedEventArgs e)
        {
            Button BtnPM = sender as Button;
            if (BtnPM.Tag == null)
            {
                MessageBox.Show("无法选择此行！");
                return;
            }
            DataGridRowEventArgs PMrating = BtnPM.Tag as DataGridRowEventArgs;
            string StrContent = "";
            int PermCount = 0;
            PermCount = tmpPermission.Count;

            for (int i = 2; i < PermCount + 2; i++)
            {
                Button mybtn = DaGrPM.Columns[i].GetCellContent(PMrating.Row).FindName("PMrating") as Button;
                StrContent = mybtn.Content.ToString();

                switch (StrContent)
                {
                    case "":
                        StrContent = "★";
                        break;
                    case "★":
                        StrContent = "★★";
                        break;
                    case "★★":
                        StrContent = "★★★";
                        break;
                    case "★★★":
                        StrContent = "★★★★";
                        break;
                    case "★★★★":
                        StrContent = "★★★★★";
                        break;
                    case "★★★★★":
                        StrContent = "";
                        break;
                }
                mybtn.Content = StrContent;
            }
        }




    }
}
