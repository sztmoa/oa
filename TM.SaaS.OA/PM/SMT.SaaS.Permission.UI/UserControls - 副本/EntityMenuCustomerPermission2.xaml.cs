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
using System.Reflection;
using System.Text;

namespace SMT.SaaS.Permission.UI.UserControls
{
    public partial class EntityMenuCustomerPermission2 : UserControl, IEntityEditor
    {
        #region 初始化参数

        private SelectMultiMenu addFrm;
        //基础变量
        public FormTypes FormType { get; set; }
        string RoleID = string.Empty;
        private List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        private string strResMsg = string.Empty;
        private bool closeFormFlag = false;//是否关闭窗体 false 表示不关闭

        private PermissionServiceClient clientPerm = new PermissionServiceClient();
        OrganizationServiceClient clientOrg = new OrganizationServiceClient();
        #endregion

        #region 构造函数

        public EntityMenuCustomerPermission2(FormTypes formtype, string strRoleId)
        {
            InitializeComponent();
            FormType = formtype;
            RoleID = strRoleId;
            InitEvent();
            InitParas();
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void InitEvent()
        {
            clientPerm.GetSysDictionaryByCategoryCompleted += new EventHandler<GetSysDictionaryByCategoryCompletedEventArgs>(clientPerm_GetSysDictionaryByCategoryCompleted);
            clientPerm.GetSysPermissionByEntityIDCompleted += new EventHandler<GetSysPermissionByEntityIDCompletedEventArgs>(clientPerm_GetSysPermissionByEntityIDCompleted);
            clientPerm.GetEntityMenuByMenuIDsCompleted += new EventHandler<GetEntityMenuByMenuIDsCompletedEventArgs>(clientPerm_GetEntityMenuByMenuIDsCompleted);
            

            //初始化控件的状态
            clientPerm.GetCutomterPermissionObjCompleted += new EventHandler<GetCutomterPermissionObjCompletedEventArgs>(clientPerm_GetCutomterPermissionObjCompleted);
            clientPerm.SetCutomterPermissionObjCompleted += new EventHandler<SetCutomterPermissionObjCompletedEventArgs>(clientPerm_SetCutomterPermissionObjCompleted);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitParas()
        {
            //绑定系统类型
            clientPerm.GetSysPermissionByEntityIDAsync("");
            clientPerm.GetCutomterPermissionObjAsync(RoleID);
            clientPerm.GetSysDictionaryByCategoryAsync("SYSTEMTYPE");

            SetToolBar();
        }
        /// <summary>
        /// 显示按钮
        /// </summary>
        private void SetToolBar()
        {
            ToolbarItems = Utility.CreateFormSaveButton();
            RefreshUI(RefreshedTypes.All);
        }
        #endregion

        #region 事件

        /// <summary>
        /// 获取系统列表加载到ComboBox上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientPerm_GetSysDictionaryByCategoryCompleted(object sender, GetSysDictionaryByCategoryCompletedEventArgs e)
        {
            //绑定系统类型
            if (e.Result != null)
            {
                List<T_SYS_DICTIONARY> dicts = e.Result.ToList();

                cbxSystemType.ItemsSource = dicts;
                cbxSystemType.DisplayMemberPath = "DICTIONARYNAME";

                if (cbxSystemType.Items.Count() > 0)
                {
                    cbxSystemType.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// 绑定到权限列表Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientPerm_GetSysPermissionByEntityIDCompleted(object sender, GetSysPermissionByEntityIDCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                if (e.Error != null)
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), e.Error.ToString());
                    return;
                }
                if (e.Result != null)
                {
                    DtGridPermission.ItemsSource = e.Result.ToList();
                }
            }
        }

        /// <summary>
        /// 根据角色ID获取自定义权限数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientPerm_GetCutomterPermissionObjCompleted(object sender, GetCutomterPermissionObjCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<CustomerPermission> ListCustomerpermission = e.Result.ToList();
                if (ListCustomerpermission == null)
                {
                    ListCustomerpermission = new List<CustomerPermission>();
                    return;
                }

                if (ListCustomerpermission.Count() == 0)
                {
                    return;
                }

                this.DataContext = ListCustomerpermission;
                
                   
                    
                ObservableCollection<string> strMenuIDs = new ObservableCollection<string>();
                ListCustomerpermission.ForEach(item =>
                {
                    strMenuIDs.Add(item.EntityMenuId);
                });

                clientPerm.GetEntityMenuByMenuIDsAsync(strMenuIDs, "");
                
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), e.Error.ToString());
            }
        }

        /// <summary>
        /// 存储当前角色的自定义权限配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientPerm_SetCutomterPermissionObjCompleted(object sender, SetCutomterPermissionObjCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("UPDATESUCCESSED", Utility.GetResourceStr("CURRENTRECORD", "ROLECUSTOMMENUPERMISSION")));
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), e.Error.ToString());
            }
        }

        /// <summary>
        /// 加载当前角色，系统下的菜单，菜单权限及权限范围(组织架构)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clientPerm_GetEntityMenuByMenuIDsCompleted(object sender, GetEntityMenuByMenuIDsCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                List<T_SYS_ENTITYMENU> entMenulist = e.Result.ToList();
                DataGridBindingPcv(DaGrMenu, entMenulist);

                if (DaGrMenu.ItemsSource != null)
                {
                    foreach (object obj in DaGrMenu.ItemsSource)
                    {
                        DaGrMenu.SelectedItem = obj;
                        break;
                    }
                }
                SetCheckBoxIsFalse();
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), e.Error.ToString());
            }
        }

        #region 系统类型事件
        /// <summary>
        /// 系统选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSystemType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            T_SYS_DICTIONARY dict = cbxSystemType.SelectedItem as T_SYS_DICTIONARY;
            if (dict != null && this.DataContext != null)
            {
                List<CustomerPermission> ListCustomerPerms = this.DataContext as List<CustomerPermission>;
                if(ListCustomerPerms.Count() == 0)
                {
                    return;
                }

                ObservableCollection<string> strMenuIDs = new ObservableCollection<string>();
                ListCustomerPerms.ForEach(item =>{
                    strMenuIDs.Add(item.EntityMenuId);
                });

                clientPerm.GetEntityMenuByMenuIDsAsync(strMenuIDs, dict.DICTIONARYVALUE.ToString());
            }
        }

        #endregion

        #endregion

        #region 私有方法

        #region 初始化函数
        /// <summary>
        /// 绑定到菜单Grid
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="OAMenu"></param>
        private void DataGridBindingPcv(DataGrid dt, List<T_SYS_ENTITYMENU> OAMenu)
        {
            PagedCollectionView pcv = new PagedCollectionView(OAMenu);
            //Type a = ((PagedCollectionView)pcv).CurrentItem.GetType();
            pcv.PageSize = 600;
            //dataPager.DataContext = pcv;

            dt.ItemsSource = pcv;
        }

        /// <summary>
        /// 根据当前角色，选择的系统类型，加载其菜单权限及权限范围(组织架构)
        /// </summary>
        private void LoadPageInfo()
        {
            ObservableCollection<CustomerPermission> ListCustomerpermission = this.DataContext as ObservableCollection<CustomerPermission>;
            if (ListCustomerpermission == null)
            {
                return;
            }

            if (ListCustomerpermission.Count() == 0)
            {
                return;
            }

            T_SYS_DICTIONARY dict = cbxSystemType.SelectedItem as T_SYS_DICTIONARY;
            if (dict == null)
            {
                return;
            }

            ObservableCollection<string> listMenuIds = new ObservableCollection<string>();
            ListCustomerpermission.ForEach(item =>
            {
                listMenuIds.Add(item.EntityMenuId);
            });

            clientPerm.GetEntityMenuByMenuIDsAsync(listMenuIds, dict.DICTIONARYVALUE.ToString());
        }

        #region 清空权限的checkbox，权限范围(组织架构)的DataGrid
        private void SetCheckBoxIsFalse()
        {
            ClearPermissionDataGridCheckBox();//清空权限菜单的选择项

            if (DaGrMenu.SelectedItems.Count == 0)
            {
                ClearOrganizationDataGrid();
            }
        }

        /// <summary>
        /// 清空权限的列表的选中值
        /// </summary>
        private void ClearPermissionDataGridCheckBox()
        {            

            if (DtGridPermission.ItemsSource == null)
            {
                return;
            }

            foreach (object obj in DtGridPermission.ItemsSource)
            {
                if (DtGridPermission.Columns[0].GetCellContent(obj) != null)
                {
                    CheckBox cb1 = DtGridPermission.Columns[0].GetCellContent(obj).FindName("DtGridPermissionChkBox") as CheckBox; //cb为
                    cb1.IsChecked = false;
                }
            }

            if (DaGrMenu.ItemsSource == null)
            {
                return;
            }

            List<CustomerPermission> ListCustomerpermission = new List<CustomerPermission>();
            ObservableCollection<PermissionValue> ListPermValue = new ObservableCollection<PermissionValue>();
            if (DaGrMenu.SelectedItems.Count == 0)
            {
                return;
            }

            T_SYS_ENTITYMENU entMenu = DaGrMenu.SelectedItems[0] as T_SYS_ENTITYMENU;
            ListCustomerpermission = this.DataContext as List<CustomerPermission>;

            if (ListCustomerpermission == null)
            {
                return;
            }

            if (ListCustomerpermission.Count() == 0)
            {
                return;
            }

            var q = from c in ListCustomerpermission
                    where c.EntityMenuId == entMenu.ENTITYMENUID
                    select c;

            ListPermValue = q.FirstOrDefault().PermissionValue;


            foreach (object obj in DtGridPermission.ItemsSource)
            {
                T_SYS_PERMISSION entPerm = obj as T_SYS_PERMISSION;

                if (DtGridPermission.Columns[0].GetCellContent(obj) != null)
                {
                    CheckBox cb1 = DtGridPermission.Columns[0].GetCellContent(obj).FindName("DtGridPermissionChkBox") as CheckBox; //cb为

                    if (ListPermValue == null)
                    {
                        continue;
                    }

                    if (ListPermValue.Count() == 0)
                    {
                        continue;
                    }

                    var qc = from p in ListPermValue
                             where p.Permission == entPerm.PERMISSIONID
                             select p;

                    if (qc.Count() > 0)
                    {
                        cb1.IsChecked = true;
                        if (DtGridPermission.SelectedItems.Count == 0)
                        {
                            DtGridPermission.SelectedItem = entPerm;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 重置权限范围的DataGrid值
        /// </summary>
        private void ClearOrganizationDataGrid()
        {
            DtGridCompany.ItemsSource = null;
            DtGridPost.ItemsSource = null;
            DtGridDepartment.ItemsSource = null;

            ObservableCollection<OrgObject> entOrgObjs = GetCurrentOrgObjs();

            ClearOrgDataGrid(DtGridCompany, Convert.ToInt32(OrgTreeItemTypes.Company).ToString(), entOrgObjs);
            ClearOrgDataGrid(DtGridDepartment, Convert.ToInt32(OrgTreeItemTypes.Department).ToString(), entOrgObjs);
            ClearOrgDataGrid(DtGridPost, Convert.ToInt32(OrgTreeItemTypes.Post).ToString(), entOrgObjs);
        }

        /// <summary>
        /// 获取当前选定菜单及权限下的权限范围集合
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<OrgObject> GetCurrentOrgObjs()
        {
            ObservableCollection<OrgObject> ListOrgObjs = new ObservableCollection<OrgObject>();
            if (DaGrMenu.SelectedItems == null)
            {
                return ListOrgObjs;
            }

            if (DaGrMenu.SelectedItems.Count == 0)
            {
                return ListOrgObjs;
            }

            T_SYS_ENTITYMENU entMenu = DaGrMenu.SelectedItems[0] as T_SYS_ENTITYMENU;
            List<CustomerPermission> ListCustomerpermission = this.DataContext as List<CustomerPermission>;
            var q = from c in ListCustomerpermission
                    where c.EntityMenuId == entMenu.ENTITYMENUID
                    select c;

            ObservableCollection<PermissionValue> ListPermValue = q.FirstOrDefault().PermissionValue;

            if (ListPermValue == null)
            {
                return ListOrgObjs;
            }

            if (ListPermValue.Count() == 0)
            {
                return ListOrgObjs;
            }

            if (DtGridPermission.SelectedItems == null)
            {
                return ListOrgObjs;
            }

            if (DtGridPermission.SelectedItems.Count == 0)
            {
                return ListOrgObjs;
            }

            object obj = DtGridPermission.SelectedItems[0];
            if (DtGridPermission.Columns[0].GetCellContent(obj) == null)
            {
                return ListOrgObjs;
            }

            CheckBox cb1 = DtGridPermission.Columns[0].GetCellContent(obj).FindName("DtGridPermissionChkBox") as CheckBox; //cb为
            if (cb1.IsChecked.Value != true)
            {
                return ListOrgObjs;
            }

            T_SYS_PERMISSION entPerm = obj as T_SYS_PERMISSION;

            var n = from t in ListPermValue
                    where t.Permission == entPerm.PERMISSIONID
                    select t;

            if (n.Count() > 0)
            {
                PermissionValue entPV = n.FirstOrDefault();
                ListOrgObjs = entPV.OrgObjects;
            }

            return ListOrgObjs;
        }

        /// <summary>
        /// 重置权限范围的DataGrid绑定
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="strOrgType"></param>
        /// <param name="ListOrgObjs"></param>
        private void ClearOrgDataGrid(DataGrid dg, string strOrgType, ObservableCollection<OrgObject> ListOrgObjs)
        {
            dg.ItemsSource = null;

            if (ListOrgObjs == null)
            {
                return;
            }

            if (ListOrgObjs.Count() == 0)
            {
                return;
            }

            var q = from o in ListOrgObjs
                    where o.OrgType == strOrgType
                    select o;

            if (q.Count() == 0)
            {
                return;
            }

            //Convert.ToInt32(OrgTreeItemTypes.Company).ToString()="1";
            //Convert.ToInt32(OrgTreeItemTypes.Department).ToString()="2";
            //Convert.ToInt32(OrgTreeItemTypes.Post).ToString()="3"
            switch (strOrgType)
            {
                case "0":
                    dg.ItemsSource = GetCompanyList(q.ToList());
                    break;
                case "1":
                    dg.ItemsSource = GetDepartmentList(q.ToList());
                    break;
                case "2":
                    dg.ItemsSource = GetPostList(q.ToList());
                    break;
            }
        }

        /// <summary>
        /// 根据权限范围集合获取对应选取的公司集合
        /// </summary>
        /// <param name="listOrgObjets"></param>
        /// <returns></returns>
        private List<T_HR_COMPANY> GetCompanyList(List<OrgObject> listOrgObjets)
        {
            List<T_HR_COMPANY> entCompanyList = Application.Current.Resources["SYS_CompanyInfo"] as List<T_HR_COMPANY>;
            List<T_HR_COMPANY> entRes = new List<T_HR_COMPANY>();

            if (entCompanyList == null)
            {
                return entRes;
            }

            if (entCompanyList.Count() == 0)
            {
                return entRes;
            }

            if (listOrgObjets == null)
            {
                return entRes;
            }

            if (listOrgObjets.Count() == 0)
            {
                return entRes;
            }

            listOrgObjets.ForEach(item =>
            {
                var q = from c in entCompanyList
                        where c.COMPANYID == item.OrgID
                        select c;

                if (q.Count() > 0)
                {
                    entRes.Add(q.FirstOrDefault());
                }
            });

            return entRes;
        }

        /// <summary>
        /// 根据权限范围集合获取对应选取的公司集合
        /// </summary>
        /// <param name="listOrgObjets"></param>
        /// <returns></returns>
        private List<T_HR_DEPARTMENT> GetDepartmentList(List<OrgObject> listOrgObjets)
        {
            List<T_HR_DEPARTMENT> entDepartmentList = Application.Current.Resources["SYS_DepartmentInfo"] as List<T_HR_DEPARTMENT>;
            List<T_HR_DEPARTMENT> entRes = new List<T_HR_DEPARTMENT>();

            if (entDepartmentList == null)
            {
                return entRes;
            }

            if (entDepartmentList.Count() == 0)
            {
                return entRes;
            }

            if (listOrgObjets == null)
            {
                return entRes;
            }

            if (listOrgObjets.Count() == 0)
            {
                return entRes;
            }

            listOrgObjets.ForEach(item =>
            {
                var q = from c in entDepartmentList
                        where c.DEPARTMENTID == item.OrgID
                        select c;

                if (q.Count() > 0)
                {
                    entRes.Add(q.FirstOrDefault());
                }
            });

            return entRes;
        }

        /// <summary>
        /// 根据权限范围集合获取对应选取的公司集合
        /// </summary>
        /// <param name="listOrgObjets"></param>
        /// <returns></returns>
        private List<T_HR_POST> GetPostList(List<OrgObject> listOrgObjets)
        {
            List<T_HR_POST> entPostList = Application.Current.Resources["SYS_PostInfo"] as List<T_HR_POST>;
            List<T_HR_POST> entRes = new List<T_HR_POST>();

            if (entPostList == null)
            {
                return entRes;
            }

            if (entPostList.Count() == 0)
            {
                return entRes;
            }

            if (listOrgObjets == null)
            {
                return entRes;
            }

            if (listOrgObjets.Count() == 0)
            {
                return entRes;
            }

            listOrgObjets.ForEach(item =>
            {
                var q = from c in entPostList
                        where c.POSTID == item.OrgID
                        select c;

                if (q.Count() > 0)
                {
                    entRes.Add(q.FirstOrDefault());
                }
            });

            return entRes;
        }
        #endregion
        #endregion

        #region 添加,删除权限范围
        /// <summary>
        /// 添加新的权限范围(组织架构)
        /// </summary>
        /// <param name="ListExtOrgObj"></param>
        private void AddCustomPermissionByExtOrgObj(List<ExtOrgObj> ListExtOrgObj)
        {
            if (this.DataContext == null)
            {
                return;
            }

            if (ListExtOrgObj == null)
            {
                return;
            }

            if (ListExtOrgObj.Count() == 0)
            {
                return;
            }

            T_SYS_ENTITYMENU entMenu = DaGrMenu.SelectedItems[0] as T_SYS_ENTITYMENU;
            if (entMenu == null)
            {
                return;
            }

            T_SYS_PERMISSION entPerm = DtGridPermission.SelectedItems[0] as T_SYS_PERMISSION;
            if (entPerm == null)
            {
                return;
            }

            List<CustomerPermission> ListCustomerpermission = this.DataContext as List<CustomerPermission>;
            var q = from c in ListCustomerpermission
                    where c.EntityMenuId == entMenu.ENTITYMENUID
                    select c;
            CustomerPermission entCP = q.FirstOrDefault();
            if (entCP.PermissionValue == null)
            {
                return;
            }

            if (entCP.PermissionValue.Count() == 0)
            {
                return;
            }

            var qp = from p in entCP.PermissionValue
                     where p.Permission == entPerm.PERMISSIONID
                     select p;

            if (qp.Count() == 0)
            {
                return;
            }

            PermissionValue entPV = qp.FirstOrDefault();
            ObservableCollection<OrgObject> ListOrgObjs = new ObservableCollection<OrgObject>();
            if (entPV.OrgObjects == null)
            {
                foreach (ExtOrgObj entItem in ListExtOrgObj)
                {
                    OrgObject entOrg = new OrgObject();
                    entOrg = GetOrgEntity(entItem);
                    ListOrgObjs.Add(entOrg);
                }
            }
            else
            {
                if (entPV.OrgObjects.Count() == 0)
                {
                    foreach (ExtOrgObj entItem in ListExtOrgObj)
                    {
                        OrgObject entOrg = new OrgObject();
                        entOrg = GetOrgEntity(entItem);
                        ListOrgObjs.Add(entOrg);
                    }
                }
                else
                {
                    ListOrgObjs = entPV.OrgObjects;
                    foreach (ExtOrgObj entItem in ListExtOrgObj)
                    {
                        OrgObject entOrg = new OrgObject();
                        entOrg = GetOrgEntity(entItem);

                        var pcc = from o in ListOrgObjs
                                  where o.OrgID == entOrg.OrgID && o.OrgType == entOrg.OrgType
                                  select o;
                        if (pcc.Count() == 0)
                        {
                            ListOrgObjs.Add(entOrg);
                        }
                    }
                }
            }

            entPV.OrgObjects = ListOrgObjs;

            this.DataContext = ListCustomerpermission;
        }

        /// <summary>
        /// 移除权限范围(组织架构)
        /// </summary>
        /// <param name="ListExtOrgObj"></param>
        private void RemoveCustomPermissionByExtOrgObj(ExtOrgObj entExtOrgObj)
        {
            if (this.DataContext == null)
            {
                return;
            }

            if (entExtOrgObj == null)
            {
                return;
            }

            T_SYS_ENTITYMENU entMenu = DaGrMenu.SelectedItems[0] as T_SYS_ENTITYMENU;
            if (entMenu == null)
            {
                return;
            }

            T_SYS_PERMISSION entPerm = DtGridPermission.SelectedItems[0] as T_SYS_PERMISSION;
            if (entPerm == null)
            {
                return;
            }

            List<CustomerPermission> ListCustomerpermission = this.DataContext as List<CustomerPermission>;
            var q = from c in ListCustomerpermission
                    where c.EntityMenuId == entMenu.ENTITYMENUID
                    select c;
            CustomerPermission entCP = q.FirstOrDefault();
            if (entCP.PermissionValue == null)
            {
                return;
            }

            if (entCP.PermissionValue.Count() == 0)
            {
                return;
            }

            var qp = from p in entCP.PermissionValue
                     where p.Permission == entPerm.PERMISSIONID
                     select p;

            if (qp.Count() == 0)
            {
                return;
            }

            PermissionValue entPV = qp.FirstOrDefault();
            ObservableCollection<OrgObject> ListOrgObjs = new ObservableCollection<OrgObject>();
            if (entPV.OrgObjects == null)
            {
                return;
            }

            if (entPV.OrgObjects.Count() == 0)
            {
                return;
            }

            ListOrgObjs = entPV.OrgObjects;
            for (int i = 0; i < ListOrgObjs.Count(); i++)
            {
                OrgObject entOrg = ListOrgObjs[i];
                if (entOrg.OrgID == entExtOrgObj.ObjectID)
                {
                    ListOrgObjs.Remove(entOrg);
                    break;
                }
            }

            entPV.OrgObjects = ListOrgObjs;

            this.DataContext = ListCustomerpermission;
        }

        /// <summary>
        /// 转换为权限范围实体
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private OrgObject GetOrgEntity(ExtOrgObj obj)
        {
            FrameworkUI.OrgTreeItemTypes nodeType = obj.ObjectType;
            OrgObject entTemp = new OrgObject();

            switch (nodeType)
            {
                case FrameworkUI.OrgTreeItemTypes.Company:
                    T_HR_COMPANY tmpOrg = (T_HR_COMPANY)obj.ObjectInstance;
                    if (tmpOrg != null)
                    {
                        entTemp.OrgID = tmpOrg.COMPANYID;
                        entTemp.OrgType = Convert.ToInt32(OrgTreeItemTypes.Company).ToString();
                    }
                    break;
                case FrameworkUI.OrgTreeItemTypes.Department:
                    T_HR_DEPARTMENT tmpDep = obj.ObjectInstance as T_HR_DEPARTMENT;
                    if (tmpDep != null)
                    {
                        entTemp.OrgID = tmpDep.DEPARTMENTID;
                        entTemp.OrgType = Convert.ToInt32(OrgTreeItemTypes.Department).ToString();
                    }
                    break;
                case FrameworkUI.OrgTreeItemTypes.Post:
                    T_HR_POST tmpPost = obj.ObjectInstance as T_HR_POST;
                    if (tmpPost != null)
                    {
                        entTemp.OrgID = tmpPost.POSTID;
                        entTemp.OrgType = Convert.ToInt32(OrgTreeItemTypes.Post).ToString();
                    }
                    break;
                default:
                    entTemp = null;
                    break;
            }

            return entTemp;
        }
        #endregion

        #region 选择，删除公司事件
        /// <summary>
        /// 权限范围(组织架构)之添加公司事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCompany_Click(object sender, RoutedEventArgs e)
        {
            SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup lookup = new SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup();
            lookup.SelectedObjType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Company;
            lookup.SelectedClick += (obj, ev) =>
            {
                List<ExtOrgObj> entExtOrgObjs = lookup.SelectedObj as List<ExtOrgObj>;
                if (entExtOrgObjs == null)
                {
                    return;
                }

                if (entExtOrgObjs.Count() == 0)
                {
                    return;
                }

                List<T_HR_COMPANY> entCompanyList = Application.Current.Resources["SYS_CompanyInfo"] as List<T_HR_COMPANY>;
                //List<T_HR_DEPARTMENT> entDepartmentList = Application.Current.Resources["SYS_DepartmentInfo"] as List<T_HR_DEPARTMENT>;
                //List<T_HR_POST> entPostList = Application.Current.Resources["SYS_PostInfo"] as List<T_HR_POST>;

                List<T_HR_COMPANY> entCompanyChecks = DtGridCompany.ItemsSource as List<T_HR_COMPANY>;
                List<T_HR_COMPANY> entCompanyAdds = new List<T_HR_COMPANY>();
                //List<T_HR_DEPARTMENT> entDepartmentChecks = DtGridDepartment.ItemsSource as List<T_HR_DEPARTMENT>;
                //List<T_HR_POST> entPostChecks = DtGridPost.ItemsSource as List<T_HR_POST>;

                List<ExtOrgObj> ListExtOrgCompanyObj = new List<ExtOrgObj>();

                if (entCompanyList == null)
                {
                    return;
                }

                if (entCompanyList.Count() == 0)
                {
                    return;
                }

                if (entCompanyChecks == null)
                {
                    entCompanyChecks = new List<T_HR_COMPANY>();
                }

                //if (entDepartmentChecks == null)
                //{
                //    entDepartmentChecks = new List<T_HR_DEPARTMENT>();
                //}

                //if (entPostChecks == null)
                //{
                //    entPostChecks = new List<T_HR_POST>();
                //}

                foreach (ExtOrgObj item in entExtOrgObjs)
                {
                    T_HR_COMPANY entCompay = item.ObjectInstance as T_HR_COMPANY;

                    var c = from o in entCompanyChecks
                            where o.COMPANYID == entCompay.COMPANYID
                            select o;

                    if (c.Count() > 0)
                    {
                        return;
                    }

                    var q = from o in entCompanyList
                            where o.COMPANYID == entCompay.COMPANYID
                            select o;

                    if (q.Count() > 0)
                    {
                        entCompanyAdds.Add(q.FirstOrDefault());
                        ListExtOrgCompanyObj.Add(item);
                    }
                }

                entCompanyAdds.AddRange(entCompanyChecks);

                DtGridCompany.ItemsSource = entCompanyAdds;
                AddCustomPermissionByExtOrgObj(ListExtOrgCompanyObj);
            };

            lookup.MultiSelected = true;
            lookup.SelectSameGradeOnly = true;
            lookup.Show();
        }

        private void DtGridCompany_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            ExtOrgObj OrderInfoT = new ExtOrgObj();
            OrderInfoT.ObjectInstance = e.Row.DataContext;
            Button myButton = DtGridCompany.Columns[1].GetCellContent(e.Row).FindName("DeleteCompanyBtn") as Button;
            myButton.Tag = OrderInfoT;
        }

        private void DeleteCompanyBtn_Click(object sender, RoutedEventArgs e)
        {
            //删除公司列表中的值
            Button Btn = sender as Button;
            T_HR_COMPANY entDel = Btn.Tag as T_HR_COMPANY;
            RemoveCompany(entDel);
        }

        /// <summary>
        /// 删除公司项
        /// </summary>
        /// <param name="allMenu"></param>
        private void RemoveCompany(T_HR_COMPANY delCompany)
        {
            if (delCompany == null)
            {
                return;
            }

            if (this.DataContext == null)
            {
                return;
            }

            List<T_HR_COMPANY> entCompanyList = DtGridCompany.ItemsSource as List<T_HR_COMPANY>;
            if (entCompanyList == null)
            {
                return;
            }

            if (entCompanyList.Count() == 0)
            {
                return;
            }

            for (int i = 0; i < entCompanyList.Count(); i++)
            {
                T_HR_COMPANY entCompany = entCompanyList[i] as T_HR_COMPANY;
                if (entCompany.COMPANYID == delCompany.COMPANYID)
                {
                    entCompanyList.Remove(entCompany);
                    break;
                }
            }

            ExtOrgObj extCompany = new ExtOrgObj();
            extCompany.ObjectInstance = delCompany;

            RemoveCustomPermissionByExtOrgObj(extCompany);
        }
        #endregion

        #region 选择，删除部门事件
        /// <summary>
        /// 权限范围(组织架构)之添加部门事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectDepartment_Click(object sender, RoutedEventArgs e)
        {
            SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup lookup = new SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup();
            lookup.SelectedObjType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Department;
            lookup.SelectedClick += (obj, ev) =>
            {
                List<ExtOrgObj> entExtOrgObjs = lookup.SelectedObj as List<ExtOrgObj>;
                if (entExtOrgObjs == null)
                {
                    return;
                }

                if (entExtOrgObjs.Count() == 0)
                {
                    return;
                }

                List<T_HR_DEPARTMENT> entDepartmentList = Application.Current.Resources["SYS_DepartmentInfo"] as List<T_HR_DEPARTMENT>;
                List<T_HR_DEPARTMENT> entChecks = DtGridDepartment.ItemsSource as List<T_HR_DEPARTMENT>;
                List<T_HR_DEPARTMENT> entDepartmentAdds = new List<T_HR_DEPARTMENT>();
                List<ExtOrgObj> ListExtOrgDepartmentObj = new List<ExtOrgObj>();

                if (entDepartmentList == null)
                {
                    return;
                }

                if (entDepartmentList.Count() == 0)
                {
                    return;
                }

                if (entChecks == null)
                {
                    entChecks = new List<T_HR_DEPARTMENT>();
                }

                foreach (ExtOrgObj item in entExtOrgObjs)
                {
                    T_HR_DEPARTMENT entDepartment = item.ObjectInstance as T_HR_DEPARTMENT;

                    var t = from o in entChecks
                            where o.DEPARTMENTID == entDepartment.DEPARTMENTID
                            select o;

                    if (t.Count() > 0)
                    {
                        continue;
                    }

                    var q = from c in entDepartmentList
                            where c.DEPARTMENTID == entDepartment.DEPARTMENTID
                            select c;

                    if (q.Count() > 0)
                    {
                        entDepartmentAdds.Add(q.FirstOrDefault());
                        ListExtOrgDepartmentObj.Add(item);
                    }
                }

                entDepartmentAdds.AddRange(entChecks);

                DtGridDepartment.ItemsSource = entDepartmentAdds;
                AddCustomPermissionByExtOrgObj(ListExtOrgDepartmentObj);
            };
            lookup.MultiSelected = true;
            lookup.SelectSameGradeOnly = true;
            lookup.Show();
        }

        private void DtGridDtGridDepartment_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_HR_DEPARTMENT DepartmentInfoT = (T_HR_DEPARTMENT)e.Row.DataContext;
            Button myButton = DtGridDepartment.Columns[2].GetCellContent(e.Row).FindName("DeleteDepartmentBtn") as Button;
            myButton.Tag = DepartmentInfoT;
        }

        private void DeleteDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            Button Btn = sender as Button;
            T_HR_DEPARTMENT entDepartment = Btn.Tag as T_HR_DEPARTMENT;
            RemoveDepartment(entDepartment);
        }

        /// <summary>
        /// 删除部门项
        /// </summary>
        /// <param name="allMenu"></param>
        private void RemoveDepartment(T_HR_DEPARTMENT delDepartment)
        {
            if (delDepartment == null)
            {
                return;
            }

            if (this.DataContext == null)
            {
                return;
            }

            List<T_HR_DEPARTMENT> entDepartmentList = DtGridDepartment.ItemsSource as List<T_HR_DEPARTMENT>;
            if (entDepartmentList == null)
            {
                return;
            }

            if (entDepartmentList.Count() == 0)
            {
                return;
            }

            for (int i = 0; i < entDepartmentList.Count(); i++)
            {
                T_HR_DEPARTMENT entDepartment = entDepartmentList[i] as T_HR_DEPARTMENT;
                if (entDepartment.DEPARTMENTID == delDepartment.DEPARTMENTID)
                {
                    entDepartmentList.Remove(entDepartment);
                    break;
                }
            }

            ExtOrgObj extDepartment = new ExtOrgObj();
            extDepartment.ObjectInstance = delDepartment;

            RemoveCustomPermissionByExtOrgObj(extDepartment);
        }

        #endregion

        #region 选择，删除岗位事件
        /// <summary>
        /// 权限范围(组织架构)之添加岗位事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPost_Click(object sender, RoutedEventArgs e)
        {
            SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup lookup = new SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup();
            lookup.SelectedObjType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Post;
            lookup.SelectedClick += (obj, ev) =>
            {
                List<ExtOrgObj> entExtOrgObjs = lookup.SelectedObj as List<ExtOrgObj>;
                if (entExtOrgObjs == null)
                {
                    return;
                }

                if (entExtOrgObjs.Count() == 0)
                {
                    return;
                }

                List<T_HR_POST> entPostList = Application.Current.Resources["SYS_PostInfo"] as List<T_HR_POST>;
                List<T_HR_POST> entChecks = DtGridPost.ItemsSource as List<T_HR_POST>;
                List<T_HR_POST> entPostAdds = new List<T_HR_POST>();
                List<ExtOrgObj> ListExtOrgPostObj = new List<ExtOrgObj>();

                if (entPostList == null)
                {
                    return;
                }

                if (entPostList.Count() == 0)
                {
                    return;
                }

                if (entChecks == null)
                {
                    entChecks = new List<T_HR_POST>();
                }

                foreach (ExtOrgObj item in entExtOrgObjs)
                {
                    T_HR_POST entPost = item.ObjectInstance as T_HR_POST;

                    var t = from o in entChecks
                            where o.POSTID == entPost.POSTID
                            select o;

                    if (t.Count() > 0)
                    {
                        continue;
                    }

                    var q = from c in entPostList
                            where c.POSTID == entPost.POSTID
                            select c;

                    if (q.Count() > 0)
                    {
                        T_HR_POST entAdd = SetNewPostEntity(q.FirstOrDefault());
                        entPostAdds.Add(entAdd);
                        ListExtOrgPostObj.Add(item);
                    }
                }

                entPostAdds.AddRange(entChecks);

                DtGridPost.ItemsSource = entPostAdds;
                AddCustomPermissionByExtOrgObj(ListExtOrgPostObj);
            };
            lookup.MultiSelected = true;
            lookup.SelectSameGradeOnly = true;
            lookup.Show();
        }

        private T_HR_POST SetNewPostEntity(T_HR_POST entPost)
        {
            List<T_HR_COMPANY> entCompanyList = Application.Current.Resources["SYS_CompanyInfo"] as List<T_HR_COMPANY>;
            List<T_HR_DEPARTMENT> entDepartmentList = Application.Current.Resources["SYS_DepartmentInfo"] as List<T_HR_DEPARTMENT>;

            var q = from d in entDepartmentList
                    where d.DEPARTMENTID == entPost.T_HR_DEPARTMENT.DEPARTMENTID
                    select d;

            if (q.Count() == 0)
            {
                return entPost;
            }

            entPost.T_HR_DEPARTMENT = q.FirstOrDefault();

            return entPost;
        }

        private void DtGridPost_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_HR_POST DepartmentInfoT = (T_HR_POST)e.Row.DataContext;
            Button myButton = DtGridPost.Columns[3].GetCellContent(e.Row).FindName("DeletePostBtn") as Button;
            myButton.Tag = DepartmentInfoT;
        }

        private void DeletePostBtn_Click(object sender, RoutedEventArgs e)
        {
            Button Btn = sender as Button;
            T_HR_POST entPost = Btn.Tag as T_HR_POST;
            RemovePost(entPost);
        }

        /// <summary>
        /// 删除部门项
        /// </summary>
        /// <param name="allMenu"></param>
        private void RemovePost(T_HR_POST delPost)
        {
            if (delPost == null)
            {
                return;
            }

            if (this.DataContext == null)
            {
                return;
            }

            List<T_HR_POST> entPostList = DtGridPost.ItemsSource as List<T_HR_POST>;
            if (entPostList == null)
            {
                return;
            }

            if (entPostList.Count() == 0)
            {
                return;
            }

            for (int i = 0; i < entPostList.Count(); i++)
            {
                T_HR_POST entPost = entPostList[i] as T_HR_POST;
                if (entPost.POSTID == delPost.POSTID)
                {
                    entPostList.Remove(entPost);
                    break;
                }
            }

            ExtOrgObj extPost = new ExtOrgObj();
            extPost.ObjectInstance = delPost;

            RemoveCustomPermissionByExtOrgObj(extPost);
        }
        #endregion

        #endregion

        #region 权限datagrid事件

        private void DtGridPermission_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_SYS_PERMISSION PermissionInfoT = (T_SYS_PERMISSION)e.Row.DataContext;
            CheckBox myChx = DtGridPermission.Columns[0].GetCellContent(e.Row).FindName("DtGridPermissionChkBox") as CheckBox;
            myChx.Tag = PermissionInfoT;
        }

        private void DtGridPermission_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DtGridPermission.SelectedItems == null)
            {
                return;
            }

            if (DtGridPermission.SelectedItems.Count == 0)
            {
                return;
            }
            
            
            ClearPermissionDataGridCheckBox();
            ClearOrganizationDataGrid();
        }

        private void DtGridPermissionChkBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb1 = sender as CheckBox;
            cb1.IsChecked = true;
            SetCheckedPermission(cb1);
        }

        private void DtGridPermissionChkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb1 = sender as CheckBox;
            cb1.IsChecked = false;
            SetUnCheckedPermission(cb1);
            ClearPermissionDataGridCheckBox();
        }

        private void SetCheckedPermission(CheckBox chBox)
        {
            if (this.DataContext == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("PLEASESELECTENTITYMENU"));
                return;
            }

            if (DaGrMenu.SelectedItems.Count == 0)
            {
                return;
            }

            T_SYS_PERMISSION entPerm = chBox.Tag as T_SYS_PERMISSION;

            PermissionValue PerObj = new PermissionValue();
            PerObj.Permission = entPerm.PERMISSIONID;
            T_SYS_ENTITYMENU entTemp = DaGrMenu.SelectedItems[0] as T_SYS_ENTITYMENU;
            AddCustomPermissionByPerm(entTemp, PerObj);
        }

        private void SetUnCheckedPermission(CheckBox chBox)
        {
            if (DaGrMenu.SelectedItems.Count == 0)
            {
                return;
            }
            T_SYS_PERMISSION entPerm = chBox.Tag as T_SYS_PERMISSION;

            PermissionValue PerObj = new PermissionValue();
            PerObj.Permission = entPerm.PERMISSIONID;
            T_SYS_ENTITYMENU entTemp = DaGrMenu.SelectedItems[0] as T_SYS_ENTITYMENU;
            RemoveCustomPermissionByPerm(entTemp, PerObj);
        }

        /// <summary>
        /// 根据选定的功能项，添加权限项到内存中，以便提交
        /// </summary>
        /// <param name="entTemp"></param>
        /// <param name="ListPermValue"></param>
        private void AddCustomPermissionByPerm(T_SYS_ENTITYMENU entMenu, PermissionValue entPermVal)
        {
            if (this.DataContext == null)
            {
                return;
            }

            List<CustomerPermission> ListCustomerpermission = this.DataContext as List<CustomerPermission>;
            var q = from c in ListCustomerpermission
                    where c.EntityMenuId == entMenu.ENTITYMENUID
                    select c;
            CustomerPermission entTemp = q.FirstOrDefault();

            ObservableCollection<PermissionValue> ListPermValue = new ObservableCollection<PermissionValue>();
            if (entTemp.PermissionValue == null)
            {
                ListPermValue.Add(entPermVal);
            }
            else
            {
                if (entTemp.PermissionValue.Count() == 0)
                {
                    ListPermValue.Add(entPermVal);
                }
                else
                {
                    ListPermValue = entTemp.PermissionValue;
                    var c = from p in ListPermValue
                            where p.Permission == entPermVal.Permission
                            select p;

                    if (q.Count() == 0)
                    {
                        ListPermValue.Add(entPermVal);
                    }
                }
            }

            entTemp.PermissionValue = ListPermValue;
            this.DataContext = ListCustomerpermission;
            ClearOrganizationDataGrid(); //清空组织架构的选择项
        }

        /// <summary>
        /// 根据选定的功能项，从内存中移除已勾销的权限项，以便提交
        /// </summary>
        /// <param name="entTemp"></param>
        /// <param name="PerObj"></param>
        private void RemoveCustomPermissionByPerm(T_SYS_ENTITYMENU entMenu, PermissionValue PerObj)
        {
            if (this.DataContext == null)
            {
                return;
            }

            ClearOrganizationDataGrid();

            List<CustomerPermission> ListCustomerpermission = this.DataContext as List<CustomerPermission>;
            var q = from c in ListCustomerpermission
                    where c.EntityMenuId == entMenu.ENTITYMENUID
                    select c;
            CustomerPermission entTemp = q.FirstOrDefault();
            if (entTemp.PermissionValue == null)
            {
                return;
            }

            if (entTemp.PermissionValue.Count() == 0)
            {
                return;
            }

            entTemp.PermissionValue.Remove(PerObj);
            this.DataContext = ListCustomerpermission;
        }
        #endregion

        #region 菜单datagrid事件
        private void DaGrMenu_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_SYS_ENTITYMENU OrderInfoT = (T_SYS_ENTITYMENU)e.Row.DataContext;
            Button myButton = DaGrMenu.Columns[2].GetCellContent(e.Row).FindName("DeleteBtn") as Button;
            myButton.Tag = OrderInfoT;
        }


        private void DaGrMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DaGrMenu.SelectedItems.Count == 0)
                return;

            //SelectMeeting = DaGr.SelectedItems[0] as V_BumfCompanySendDoc;
            DataGrid grid = sender as DataGrid;
            if (grid.SelectedItems.Count > 0)
            {
                SetCheckBoxIsFalse();                
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Button Btn = sender as Button;
            T_SYS_ENTITYMENU entMenu = Btn.Tag as T_SYS_ENTITYMENU;

            RemoveCustomPermissionByMenu(entMenu);
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ComboBox cbxSystemType = Utility.FindChildControl<ComboBox>(expander, "cbxSystemType");
            T_SYS_DICTIONARY dict = cbxSystemType.SelectedItem as T_SYS_DICTIONARY;

            string systype = dict == null ? "" : dict.DICTIONARYVALUE.GetValueOrDefault().ToString();
            addFrm = new SelectMultiMenu(systype, null);
            EntityBrowser browser = new EntityBrowser(addFrm);
            browser.FormType = FormTypes.Browse;
            browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
            browser.Show<string>(DialogMode.Default, Common.ParentLayoutRoot, "", (result) => { });
        }

        private void browser_ReloadDataEvent()
        {
            if (addFrm.SelectedMultiMenu == null)
            {
                return;
            }
            if (addFrm.SelectedMultiMenu.Count == 0)
            {
                return;
            }

            List<T_SYS_ENTITYMENU> entMenuChecks = DaGrMenu.ItemsSource as List<T_SYS_ENTITYMENU>;
            List<T_SYS_ENTITYMENU> entMenuAdds = new List<T_SYS_ENTITYMENU>();

            if (entMenuChecks == null)
            {
                entMenuChecks = addFrm.SelectedMultiMenu;
            }
            else
            {
                foreach (var h in addFrm.SelectedMultiMenu)
                {
                    if (entMenuChecks != null)
                    {
                        var entity = from q in entMenuChecks
                                     where h.ENTITYMENUID == q.ENTITYMENUID
                                     select q;
                        if (entity.Count() == 0)
                        {
                            entMenuChecks.Add(h);
                        }
                    }
                    else
                    {
                        entMenuChecks.Add(h);
                    }
                }
            }

            entMenuAdds.AddRange(entMenuChecks);

            DataGridBindingPcv(DaGrMenu, entMenuAdds);
            AddCustomPermissionByMenu(entMenuAdds);
        }

        /// <summary>
        /// 把已设置的功能项临时存到内存内，以便提交
        /// </summary>
        /// <param name="allMenu"></param>
        private void AddCustomPermissionByMenu(List<T_SYS_ENTITYMENU> allMenu)
        {
            if (allMenu == null)
            {
                return;
            }

            if (allMenu.Count() == 0)
            {
                return;
            }

            List<CustomerPermission> ListCustomerpermission = new List<CustomerPermission>();

            foreach (T_SYS_ENTITYMENU entMenu in allMenu)
            {
                CustomerPermission entTemp = new CustomerPermission();
                entTemp.EntityMenuId = entMenu.ENTITYMENUID;

                if (this.DataContext == null)
                {
                    ListCustomerpermission.Add(entTemp);
                }
                else
                {
                    ListCustomerpermission = this.DataContext as List<CustomerPermission>;
                    var q = from c in ListCustomerpermission
                            where c.EntityMenuId == entTemp.EntityMenuId
                            select c;
                    if (q.Count() == 0)
                    {
                        ListCustomerpermission.Add(entTemp);
                    }
                }
            }

            this.DataContext = ListCustomerpermission;
        }

        /// <summary>
        /// 把已删除的功能项从内存内移除，以便提交
        /// </summary>
        /// <param name="allMenu"></param>
        private void RemoveCustomPermissionByMenu(T_SYS_ENTITYMENU delMenu)
        {
            if (delMenu == null)
            {
                return;
            }

            if (this.DataContext == null)
            {
                return;
            }
            
            List<T_SYS_ENTITYMENU> entMenuChecks = DaGrMenu.ItemsSource as List<T_SYS_ENTITYMENU>;
            if (entMenuChecks == null)
            {
                return;
            }

            if (entMenuChecks.Count() == 0)
            {
                return;
            }

            for (int i = 0; i < entMenuChecks.Count(); i++)
            {
                T_SYS_ENTITYMENU entTemp = entMenuChecks[i];
                if (entTemp.ENTITYMENUID == delMenu.ENTITYMENUID)
                {
                    entMenuChecks.Remove(entTemp);
                }
            }

            List<CustomerPermission> ListCustomerpermission = this.DataContext as List<CustomerPermission>;
            var q = from c in ListCustomerpermission
                    where c.EntityMenuId == delMenu.ENTITYMENUID
                    select c;
            if (q.Count() == 1)
            {
                ListCustomerpermission.Remove(q.FirstOrDefault());
            }

            this.DataContext = ListCustomerpermission;
            DataGridBindingPcv(DaGrMenu, entMenuChecks);//重新绑定

        }
        #endregion

        #region IEntityEditor 成员

        public string GetTitle()
        {
            return Utility.GetResourceStr("ROLECUSTOMMENUPERMISSION");
        }

        public string GetStatus()
        {
            string strTemp = string.Empty;
            if (this.DataContext != null)
            {
                strTemp = Utility.GetResourceStr("EDIT");
            }

            return strTemp;
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

        private void RefreshUI(RefreshedTypes type)
        {
            if (OnUIRefreshed != null)
            {
                UIRefreshedEventArgs args = new UIRefreshedEventArgs();
                args.RefreshedType = type;
                OnUIRefreshed(this, args);
            }
        }

        public List<NavigateItem> GetLeftMenuItems()
        {
            List<NavigateItem> items = new List<NavigateItem>();
            NavigateItem item = new NavigateItem
            {
                Title = Utility.GetResourceStr("BASEINFO"),
                Tooltip = Utility.GetResourceStr("BASEINFO")
            };
            items.Add(item);

            return items;
        }

        public List<ToolbarItem> GetToolBarItems()
        {
            return ToolbarItems;
        }

        public event UIRefreshedHandler OnUIRefreshed;

        #endregion

        #region 保存

        /// <summary>
        /// 保存
        /// </summary>
        private bool Save()
        {
            bool flag = false;

            try
            {
                List<CustomerPermission> ListCustomerpermission = this.DataContext as List<CustomerPermission>;
                if (ListCustomerpermission == null)
                {
                    return false;
                }

                if (ListCustomerpermission.Count() == 0)
                {
                    return false;
                }

                ObservableCollection<CustomerPermission> entsSubmit = new ObservableCollection<CustomerPermission>();
                ListCustomerpermission.ForEach(item =>
                {
                    entsSubmit.Add(item);
                });

                if (FormType == FormTypes.New)
                {
                    clientPerm.SetCutomterPermissionObjAsync(RoleID, entsSubmit, strResMsg);
                }
                else
                {
                    clientPerm.SetCutomterPermissionObjAsync(RoleID, entsSubmit, strResMsg);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.Message));
            }

            return flag;
        }

        /// <summary>
        /// 保存并关闭
        /// </summary>
        private void Cancel()
        {
            bool flag = false;
            flag = Save();
            if (!flag)
            {
                return;
            }

            closeFormFlag = true;
        }
        #endregion

    }
}
