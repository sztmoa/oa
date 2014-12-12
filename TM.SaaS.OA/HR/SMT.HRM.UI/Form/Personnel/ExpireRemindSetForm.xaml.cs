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
using SMT.Saas.Tools.SalaryWS;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.SaaS.FrameworkUI.OrganizationControl;
using SMT.Saas.Tools.PersonnelWS;

namespace SMT.HRM.UI.Form.Personnel
{
    public partial class ExpireRemindSetForm : BaseForm, IEntityEditor
    {
        private List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        private T_HR_SYSTEMSETTING systemSetting;
        private SalaryServiceClient client;
        private PersonnelServiceClient pClient;

        public T_HR_SYSTEMSETTING SystemSetting
        {
            get { return systemSetting; }
            set
            {
                systemSetting = value;
                this.DataContext = value;
            }
        }

        private void InitParas()
        {
            client = new SalaryServiceClient();
            pClient = new PersonnelServiceClient();
            pClient.GetEmployeeByIDCompleted += new EventHandler<GetEmployeeByIDCompletedEventArgs>(pClient_GetEmployeeByIDCompleted);
            client.GetSystemSettingByCompanyIdCompleted += new EventHandler<GetSystemSettingByCompanyIdCompletedEventArgs>(client_GetSystemSettingByCompanyIdCompleted);
            client.AddSystemParamSetCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_AddSystemParamSetCompleted);
            client.SystemParamSetUpdateCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_SystemParamSetUpdateCompleted);
            this.Loaded += new RoutedEventHandler(ExpireRemindSetForm_Loaded);
            
        }

        void pClient_GetEmployeeByIDCompleted(object sender, GetEmployeeByIDCompletedEventArgs e)
        {
            if (e.Error != null && e.Error.Message != "")
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                RefreshUI(RefreshedTypes.HideProgressBar);
            }
            else
            {
                lkEmployeeName.DataContext = e.Result;
            }
        }

        void client_SystemParamSetUpdateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error != null && e.Error.Message != "")
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), "保存成功",
                 Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
            }
        }

        void client_AddSystemParamSetCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error != null && e.Error.Message != "")
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), "保存成功",
                 Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
            }
        }

        void client_GetSystemSettingByCompanyIdCompleted(object sender, GetSystemSettingByCompanyIdCompletedEventArgs e)
        {
            if (e.Error != null && e.Error.Message != "")
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                RefreshUI(RefreshedTypes.HideProgressBar);
            }
            else
            {
                SystemSetting = e.Result;
                if (SystemSetting != null)
                {
                    pClient.GetEmployeeByIDAsync(systemSetting.OWNERID);
                }
                RefreshUI(RefreshedTypes.HideProgressBar);
            }
        }

        void ExpireRemindSetForm_Loaded(object sender, RoutedEventArgs e)
        {
            client.GetSystemSettingByCompanyIdAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID);
        }

        public ExpireRemindSetForm()
        {
            InitializeComponent();
            InitParas();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void SetToolBar()
        {
            ToolbarItems = Utility.CreateFormSaveButton();

            RefreshUI(RefreshedTypes.All);
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

        private bool Save()
        {
            if (string.IsNullOrEmpty(lkEmployeeName.TxtLookUp.Text))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), "请选择待办接收人,请确认!",
                 Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                RefreshUI(RefreshedTypes.HideProgressBar);
                return false;
            }
            if (systemSetting == null)
            {
                systemSetting = new T_HR_SYSTEMSETTING();
                systemSetting.SYSTEMSETTINGID = Guid.NewGuid().ToString();
                systemSetting.MODELTYPE = "0";
                systemSetting.PARAMETERNAME = ContactDay.Value.ToString();
                systemSetting.PARAMETERVALUE = EntryDay.Value.ToString();
                systemSetting.OWNERCOMPANYID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                var employee = lkEmployeeName.DataContext as T_HR_EMPLOYEE;
                systemSetting.OWNERID = employee.EMPLOYEEID;
                client.AddSystemParamSetAsync(systemSetting);
            }
            else
            {
                systemSetting.PARAMETERNAME = ContactDay.Value.ToString();
                systemSetting.PARAMETERVALUE = EntryDay.Value.ToString();
                var employee = lkEmployeeName.DataContext as T_HR_EMPLOYEE;
                systemSetting.OWNERID = employee.EMPLOYEEID;
                client.SystemParamSetUpdateAsync(systemSetting);
            }
            return true;
        }

        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        private void Cancel()
        {
            bool flag = false;
            flag = Save();
            if (!flag)
            {
                return;
            }

            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            entBrowser.Close();
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
                Title = Utility.GetResourceStr("BASEINFO"),
                Tooltip = Utility.GetResourceStr("BASEINFO")
            };
            items.Add(item);

            return items;
        }

        public string GetStatus()
        {
            return string.Empty;
        }

        public string GetTitle()
        {
            return "到期提醒设置";
        }

        public List<ToolbarItem> GetToolBarItems()
        {
            ToolbarItems = Utility.CreateFormSaveButton();
            return ToolbarItems;
        }

        /// <summary>
        /// 选择异动人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LookUp_FindClick(object sender, EventArgs e)
        {
           
            #region
            OrganizationLookup lookup = new OrganizationLookup();

            lookup.SelectedObjType = OrgTreeItemTypes.Personnel;
            lookup.SelectedClick += (obj, ev) =>
            {
                //  SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE ent = lookup.SelectedObj[0].ObjectInstance as SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE;
                List<ExtOrgObj> ent = lookup.SelectedObj as List<ExtOrgObj>;

                if (ent != null && ent.Count > 0)
                {

                    ExtOrgObj companyInfo = ent.FirstOrDefault();
                    ExtOrgObj post = (ExtOrgObj)companyInfo.ParentObject;
                    string postid = post.ObjectID;
                    //  fromPostLevel=(post as SMT.Saas.Tools.OrganizationWS.T_HR_POST).POSTLEVEL.ToString();

                    ExtOrgObj dept = (ExtOrgObj)post.ParentObject;
                    string deptid = dept.ObjectID;

                    // ExtOrgObj corp = (ExtOrgObj)dept.ParentObject;
                    SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY corp = (dept.ObjectInstance as SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT).T_HR_COMPANY;
                    string corpid = corp.COMPANYID;

                    T_HR_EMPLOYEE temp = new T_HR_EMPLOYEE();
                    temp = ent.FirstOrDefault().ObjectInstance as SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE;

                    lkEmployeeName.DataContext = temp;
                }

            };

            lookup.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
            #endregion

        }
    }
}
