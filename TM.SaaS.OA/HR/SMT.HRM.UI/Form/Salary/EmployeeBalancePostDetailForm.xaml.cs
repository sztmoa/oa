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
using SMT.Saas.Tools.SalaryWS;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI;
using SMT.Saas.Tools.OrganizationWS;
using SMT.Saas.Tools.PersonnelWS;
using SMT.SaaS.FrameworkUI.AuditControl;
using SMT.SaaS.FrameworkUI.OrganizationControl;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.SaaS.MobileXml;
using System.Text.RegularExpressions;


namespace SMT.HRM.UI.Form.Salary
{
    public partial class EmployeeBalancePostDetailForm : BaseForm, IEntityEditor, IAudit, IClient
    {
        
        public T_HR_EMPLOYEESALARYPOSTASIGN EmployeeAddBalancePost { get; set; }
        public FormTypes FormType { get; set; }
        private SalaryServiceClient client = new SalaryServiceClient();
        private PersonnelServiceClient personnerClient = new PersonnelServiceClient();
        private OrganizationServiceClient orgClient = new OrganizationServiceClient();
        private List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        private ObservableCollection<T_HR_BALANCEPOSTDETAIL> EmployeeBalanceInfoList = new ObservableCollection<T_HR_BALANCEPOSTDETAIL>();
        private ObservableCollection<T_HR_BALANCEPOSTDETAIL> EmployeeBalanceDelList = new ObservableCollection<T_HR_BALANCEPOSTDETAIL>();
        private string addBalanceAsignID;
        private bool isNew;//true 从快捷方式新建单据

        private ObservableCollection<string> StrCompanyIDsList = new ObservableCollection<string>();  //获取公司ID
        private ObservableCollection<string> StrDepartmentIDsList = new ObservableCollection<string>();  //获取部门ID
        private ObservableCollection<string> StrPositionIDsList = new ObservableCollection<string>();  //获取岗位ID
        private ObservableCollection<string> StaffList = new ObservableCollection<string>();  //员工ID
        private List<ExtOrgObj> issuanceExtOrgObj;
        ObservableCollection<T_HR_BALANCEPOSTDETAIL> listDetails = new ObservableCollection<T_HR_BALANCEPOSTDETAIL>();
        
        private bool needsubmit = false;//提交审核
        private bool isSubmit = false;//用于给保存方法判断是点击了保存还是提交
        string strNotes = string.Empty;
        string strRemarkNotes = string.Empty;
        public EmployeeBalancePostDetailForm()
        {
            issuanceExtOrgObj = new List<ExtOrgObj>();
            entall = new List<ExtOrgObj>();
            InitializeComponent();
            FormType = FormTypes.New;
            isNew = true;
            addBalanceAsignID = string.Empty;
            this.Loaded += new RoutedEventHandler(EmployeeBalancePostDetailForm_Loaded);
        }

        public EmployeeBalancePostDetailForm(FormTypes type, string AsignID)
        {
            issuanceExtOrgObj = new List<ExtOrgObj>();
            entall = new List<ExtOrgObj>();
            InitializeComponent();
            FormType = type;
            addBalanceAsignID = AsignID;
            this.Loaded += new RoutedEventHandler(EmployeeBalancePostDetailForm_Loaded);
        }

        void EmployeeBalancePostDetailForm_Loaded(object sender, RoutedEventArgs e)
        {            
            InitParas();            
            client.GetBalancePostsByBalanceIDAsync(addBalanceAsignID, EmployeeAddBalancePost);            
        }
        

        private void InitParas()
        {
            client.GetBalancePostsByBalanceIDCompleted += new EventHandler<GetBalancePostsByBalanceIDCompletedEventArgs>(client_GetBalancePostsByBalanceIDCompleted);
            personnerClient.GetEmployeesPostBriefByEmployeeIDCompleted += new EventHandler<GetEmployeesPostBriefByEmployeeIDCompletedEventArgs>(personnerClient_GetEmployeesPostBriefByEmployeeIDCompleted);            
        }

        
        
        void client_GetBalancePostsByBalanceIDCompleted(object sender, GetBalancePostsByBalanceIDCompletedEventArgs e)
        {
            
            if (e.Error != null && !string.IsNullOrEmpty(e.Error.Message))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                EmployeeAddBalancePost = e.asign;
                if (EmployeeAddBalancePost.NOTESCONTENT != null)
                {
                    txtRemark.Text = EmployeeAddBalancePost.NOTESCONTENT;
                }
                if (EmployeeAddBalancePost.REMARK != null)
                {
                    txtNotes.Text = EmployeeAddBalancePost.REMARK;
                }
                lkPost.Text = EmployeeAddBalancePost.BALANCEPOSTNAME;
                EmployeeBalanceInfoList.Clear();
                if (e.Result != null)
                {
                    listDetails = e.Result;
                    EmployeeBalanceInfoList = listDetails;
                }
                if (FormType == FormTypes.Resubmit)
                {
                    EmployeeAddBalancePost.CHECKSTATE = Convert.ToInt32(CheckStates.UnSubmit).ToString();
                }
                //issuanceExtOrgObj.Clear();
                //entall.Clear();
                //foreach (var ent in listDetails)
                //{
                //    ExtOrgObj objPerson = new ExtOrgObj();
                //    objPerson.ObjectID = ent.EMPLOYEEID;
                //    objPerson.ObjectName = ent.EMPLOYEENAME +"-" +ent.EMPLOYEEPOSTNAME + "-"+ent.EMPLOYEEDEPARTMENTNAME +"-" + ent.EMPLOYEECOMPANYNAME;
                //    objPerson.ObjectType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Personnel;
                //    var ExistEnts = from ext in entall
                //                    where ext.ObjectID == objPerson.ObjectID
                //                    && ext.ObjectType == SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Personnel
                //                    select ext;
                //    if (ExistEnts.Count() == 0)
                //    {
                //        entall.Add(objPerson);
                //    }
                //}
                //issuanceExtOrgObj = entall;
                BindData(EmployeeBalanceInfoList.ToList());
                //orgClient.GetPostByIdAsync(EmployeeAddBalancePost.BALANCEPOSTID);
            }
            RefreshUI(RefreshedTypes.HideProgressBar);
            RefreshUI(RefreshedTypes.All);
            if (FormType == FormTypes.Browse || FormType == FormTypes.Audit)
            {
                RefreshUI(RefreshedTypes.AuditInfo); 
            }
        }

        void personnerClient_GetEmployeesPostBriefByEmployeeIDCompleted(object sender, GetEmployeesPostBriefByEmployeeIDCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error != null && !string.IsNullOrEmpty(e.Error.Message))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                List<V_EMPLOYEEDETAIL> employees = new List<V_EMPLOYEEDETAIL>();
                if (e.Result != null)
                {
                    employees = e.Result.ToList();
                }
                foreach (var ent in employees)
                {
                    ExtOrgObj objPerson = new ExtOrgObj();
                    objPerson.ObjectID = ent.EMPLOYEEID;
                    objPerson.ObjectName = ent.EMPLOYEENAME;
                    objPerson.ObjectType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Personnel;
                    var ExistEnts = from ext in entall
                                    where ext.ObjectID == objPerson.ObjectID
                                    && ext.ObjectType == SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Personnel
                                    select ext;
                    if (ExistEnts.Count() == 0)
                    {
                        
                        T_HR_BALANCEPOSTDETAIL detail = new T_HR_BALANCEPOSTDETAIL();
                        detail.BALANCEPOSTDETAIL = Guid.NewGuid().ToString();
                        detail.BALANCEPOSTASIGNID = EmployeeAddBalancePost.EMPLOYEESALARYPOSTASIGNID;
                        detail.EMPLOYEEID = ent.EMPLOYEEID;
                        detail.EMPLOYEENAME = ent.EMPLOYEENAME;
                        
                        var ents = from ent1 in ent.EMPLOYEEPOSTS
                                   where ent1.ISAGENCY =="0"
                                   select ent1;
                        if (ents.Count() > 0)
                        {
                            detail.EMPLOYEEPOSTID = ents.FirstOrDefault().POSTID;
                            detail.EMPLOYEEPOSTNAME = ents.FirstOrDefault().PostName;
                            detail.EMPLOYEEDEPARTMENTID = ents.FirstOrDefault().DepartmentID;
                            detail.EMPLOYEEDEPARTMENTNAME = ents.FirstOrDefault().DepartmentName;
                            detail.EMPLOYEECOMPANYID = ents.FirstOrDefault().CompanyID;
                            detail.EMPLOYEECOMPANYNAME = ents.FirstOrDefault().CompanyName;
                        }
                        //detail.EDITSTATE = "0";
                        detail.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                        detail.UPDATEDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                        detail.CREATEDATE = DateTime.Now;
                        detail.UPDATEDATE = DateTime.Now;
                        EmployeeBalanceInfoList.Add(detail);
                        objPerson.ObjectName = ent.EMPLOYEENAME + "-"+ detail.EMPLOYEEPOSTNAME+"-"+ detail.EMPLOYEEDEPARTMENTNAME+"-"+ detail.EMPLOYEECOMPANYNAME;
                        entall.Add(objPerson);
                    }
                }
                issuanceExtOrgObj = entall;
                //BindData();
 
            }
            
        }

        #region IAudit
        public void SetFlowRecordEntity(SMT.SaaS.FrameworkUI.AuditControl.Flow_FlowRecord_T entity)
        {

            Dictionary<string, string> para = new Dictionary<string, string>();
            para.Add("BALANCEPOSTNAME", EmployeeAddBalancePost.BALANCEPOSTNAME);
            para.Add("BALANCEPOSTID", EmployeeAddBalancePost.BALANCEPOSTID);

            Dictionary<string, string> para2 = new Dictionary<string, string>();


            entity.SystemCode = "HR";
            string strXmlObjectSource = string.Empty;

            //  strXmlObjectSource = Utility.ObjListToXml<T_HR_EMPLOYEESALARYPOSTASIGN>(EmployeeAddBalancePost, para, "HR", para2, null);
            if (!string.IsNullOrEmpty(entity.BusinessObjectDefineXML))
                strXmlObjectSource = this.GetXmlString(entity.BusinessObjectDefineXML, EmployeeAddBalancePost);

            Dictionary<string, string> paraIDs = new Dictionary<string, string>();
            paraIDs.Add("CreateUserID", EmployeeAddBalancePost.CREATEUSERID);
            paraIDs.Add("CreatePostID", EmployeeAddBalancePost.CREATEPOSTID);
            paraIDs.Add("CreateDepartmentID", EmployeeAddBalancePost.CREATEDEPARTMENTID);
            paraIDs.Add("CreateCompanyID", EmployeeAddBalancePost.CREATECOMPANYID);

            if (EmployeeAddBalancePost.CHECKSTATE == Convert.ToInt32(CheckStates.UnSubmit).ToString())
            {
                Utility.SetAuditEntity(entity, "T_HR_EMPLOYEESALARYPOSTASIGN", EmployeeAddBalancePost.EMPLOYEESALARYPOSTASIGNID, strXmlObjectSource, paraIDs);
            }
            else
            {
                Utility.SetAuditEntity(entity, "T_HR_EMPLOYEESALARYPOSTASIGN", EmployeeAddBalancePost.EMPLOYEESALARYPOSTASIGNID, strXmlObjectSource);
            }

        }
        public void OnSubmitCompleted(SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult args)
        {
            //RefreshUI(RefreshedTypes.ShowProgressBar);
            string state = "";
            string UserState = "Audit";
            switch (args)
            {
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Auditing:
                    state = Utility.GetCheckState(CheckStates.Approving);
                    break;
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Successful:
                    state = Utility.GetCheckState(CheckStates.Approved);
                    break;
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Fail:
                    state = Utility.GetCheckState(CheckStates.UnApproved);
                    break;

            }
            if (EmployeeAddBalancePost.CHECKSTATE == Convert.ToInt32(CheckStates.UnSubmit).ToString())
            {
                UserState = "Submit";
            }
            EmployeeAddBalancePost.CHECKSTATE = state;
            if (UserState.ToString() == "Audit")
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("SUCCESSAUDIT"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
            }
            else if (UserState.ToString() == "Submit")
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("SUCCESSSUBMITAUDIT"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
            }
            RefreshUI(RefreshedTypes.HideProgressBar);
            RefreshUI(RefreshedTypes.AuditInfo);//刷新审核控件
            RefreshUI(RefreshedTypes.All);//刷新管理列表界面
            // client.EmployeeAddSumUpdateAsync(EmployeeAddBalancePost);
            //Utility.UpdateCheckState("T_HR_EMPLOYEESALARYPOSTASIGN", "EMPLOYEESALARYPOSTASIGNID", EmployeeAddBalancePost.EMPLOYEESALARYPOSTASIGNID, args);

        }

        public string GetAuditState()
        {
            string state = "-1";
            if (EmployeeAddBalancePost != null)
                state = EmployeeAddBalancePost.CHECKSTATE;
            return state;
        }
        #endregion
        #region mobilexml
        private string GetXmlString(string StrSource, T_HR_EMPLOYEESALARYPOSTASIGN Info)
        {
            //SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY ownerCompany = (Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(s => s.COMPANYID == Info.OWNERCOMPANYID).FirstOrDefault();
            //SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT ownerDepartment = (Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(s => s.DEPARTMENTID == Info.OWNERDEPARTMENTID).FirstOrDefault();
            //SMT.Saas.Tools.OrganizationWS.T_HR_POST ownerPost = (Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(s => s.POSTID == Info.OWNERPOSTID).FirstOrDefault();
            //string ownerCompanyName = string.Empty;
            //string ownerDepartmentName = string.Empty;
            //string ownerPostName = string.Empty;
            //if (ownerCompany != null)
            //{
            //    ownerCompanyName = ownerCompany.CNAME;
            //}
            //if (ownerDepartment != null)
            //{
            //    ownerDepartmentName = ownerDepartment.T_HR_DEPARTMENTDICTIONARY == null ? "" : ownerDepartment.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
            //}
            //if (ownerPost != null)
            //{
            //    ownerPostName = ownerPost.T_HR_POSTDICTIONARY == null ? "" : ownerPost.T_HR_POSTDICTIONARY.POSTNAME;
            //}
            decimal? stateValue = Convert.ToDecimal("1");
            string checkState = string.Empty;
            SMT.Saas.Tools.PermissionWS.T_SYS_DICTIONARY checkStateDict = (Application.Current.Resources["SYS_DICTIONARY"] as List<SMT.Saas.Tools.PermissionWS.T_SYS_DICTIONARY>).Where(s => s.DICTIONCATEGORY == "CHECKSTATE" && s.DICTIONARYVALUE == stateValue).FirstOrDefault();
            checkState = checkStateDict == null ? "" : checkStateDict.DICTIONARYNAME;

            SMT.SaaS.MobileXml.MobileXml mx = new SMT.SaaS.MobileXml.MobileXml();
            List<SMT.SaaS.MobileXml.AutoDictionary> AutoList = new List<SMT.SaaS.MobileXml.AutoDictionary>();
            AutoList.Add(basedata("T_HR_EMPLOYEESALARYPOSTASIGN", "CHECKSTATE", "1", checkState));

            AutoList.Add(basedata("T_HR_EMPLOYEESALARYPOSTASIGN", "BALANCEPOSTID", Info.BALANCEPOSTID, Info.BALANCEPOSTID));
            AutoList.Add(basedata("T_HR_EMPLOYEESALARYPOSTASIGN", "BALANCEPOSTNAME", Info.BALANCEPOSTNAME, Info.BALANCEPOSTNAME));
            AutoList.Add(basedata("T_HR_EMPLOYEESALARYPOSTASIGN", "NOTESCONTENT", strRemarkNotes, strRemarkNotes));
            //AutoList.Add(basedata("T_HR_EMPLOYEESALARYPOSTASIGN", "OWNERCOMPANYID", Info.OWNERCOMPANYID, ownerCompanyName));
            //AutoList.Add(basedata("T_HR_EMPLOYEESALARYPOSTASIGN", "OWNERDEPARTMENTID", Info.OWNERDEPARTMENTID, ownerDepartmentName));
            //AutoList.Add(basedata("T_HR_EMPLOYEESALARYPOSTASIGN", "OWNERPOSTID", Info.OWNERPOSTID, ownerPostName));

            ObservableCollection<Object> TrListObj = new ObservableCollection<Object>();
            foreach (var item in listDetails)
            {
                item.EMPLOYEENAME = item.EMPLOYEENAME + "-" + item.EMPLOYEEPOSTNAME + "-" + item.EMPLOYEEDEPARTMENTNAME + "-" + item.EMPLOYEECOMPANYNAME;
                TrListObj.Add(item);
            }

            string a = mx.TableToXml(Info, TrListObj, StrSource, AutoList);

            return a;
        }

        private AutoDictionary basedata(string TableName, string Name, string Value, string Text)
        {
            string[] strlist = new string[4];
            strlist[0] = TableName;
            strlist[1] = Name;
            strlist[2] = Value;
            strlist[3] = Text;
            AutoDictionary ad = new AutoDictionary();
            ad.AutoDictionaryList(strlist);
            return ad;
        }

        #endregion
        private void SetToolBar()
        {
            if (FormType == FormTypes.New)
            {
                //ToolbarItems = Utility.CreateFormSaveButton();
            }
            else if (FormType == FormTypes.Edit)
            {
                //ToolbarItems = Utility.CreateFormEditButton();
                //if (EmployeeAddBalancePost != null)
                //{
                //    if (EmployeeAddBalancePost.CHECKSTATE == "0")
                //    {
                //        ToolbarItems.Add(ToolBarItems.Delete);
                //    }
                //}
            }
            else if (FormType == FormTypes.Browse)
            {
                ToolbarItems = new List<ToolbarItem>();
            }
            else
            {
                //ToolbarItems = Utility.CreateFormEditButton("T_HR_EMPLOYEESALARYPOSTASIGN", EmployeeAddBalancePost.OWNERID,
                //    EmployeeAddBalancePost.OWNERPOSTID, EmployeeAddBalancePost.OWNERDEPARTMENTID, EmployeeAddBalancePost.OWNERCOMPANYID);
            }
            RefreshUI(RefreshedTypes.All);

        }

        #region IEntityEditor
        public string GetTitle()
        {
            return "查看薪资结算岗位变更";
        }
        public string GetStatus()
        {
            return "";
        }
        public void DoAction(string actionType)
        {
            
        }
        public void RefreshUI(RefreshedTypes type)
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
            if (FormType == FormTypes.New)
            {
                ToolbarItems = Utility.CreateFormSaveButton();
            }
            else if (FormType == FormTypes.Edit)
            {
                ToolbarItems = Utility.CreateFormEditButton();
                if (EmployeeAddBalancePost != null)
                {
                    if (EmployeeAddBalancePost.CHECKSTATE == "0")
                    {
                        ToolbarItems.Add(ToolBarItems.Delete);
                    }
                }
            }
            else if (FormType == FormTypes.Browse)
            {
                ToolbarItems = new List<ToolbarItem>();
            }
            else
            {
                if (EmployeeAddBalancePost != null)
                {
                    ToolbarItems = Utility.CreateFormEditButton("T_HR_EMPLOYEESALARYPOSTASIGN", EmployeeAddBalancePost.OWNERID,
                        EmployeeAddBalancePost.OWNERPOSTID, EmployeeAddBalancePost.OWNERDEPARTMENTID, EmployeeAddBalancePost.OWNERCOMPANYID);
                }
                else
                {
                    ToolbarItems = new List<ToolbarItem>();
                }
            }
            return ToolbarItems;
        }

        public event UIRefreshedHandler OnUIRefreshed;
        
        #endregion

        #region mobilexml
        

        

        #endregion
        
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存结果</returns>
        
        /// <summary>
        /// 保存并关闭当前窗口
        /// </summary>
        



        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }

        

        #region IClient
        public void ClosedWCFClient()
        {
            client.DoClose();
        }
        public bool CheckDataContenxChange()
        {
            return true;
        }
        public void SetOldEntity(object entity)
        {

        }
        #endregion

        
        


        List<ExtOrgObj> entall = null;//初始化组织架构集合
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshUI(RefreshedTypes.ShowProgressBar);
            AddIssuanObj();
        }

        private void AddIssuanObj()
        {
            entall = new List<ExtOrgObj>();
            SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup lookup = new SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup();
            lookup.SelectedObjType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.All;
            lookup.SelectedClick += (obj, ev) =>
            {
                List<ExtOrgObj> ent = lookup.SelectedObj as List<ExtOrgObj>;
                if (ent != null && ent.Count > 0)
                {
                    //issuanceExtOrgObj = ent;
                    foreach (var h in ent)
                    {
                        if (h.ObjectType == SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Company)//公司
                        {
                            StrCompanyIDsList.Add(h.ObjectID);                            
                        }
                        if (h.ObjectType == SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Department)//部门
                        {
                            StrDepartmentIDsList.Add(h.ObjectID);                            
                        }
                        if (h.ObjectType == SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Post)//岗位
                        {
                            StrPositionIDsList.Add(h.ObjectID);
                        }
                        if (h.ObjectType == SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Personnel)
                        {
                            StaffList.Add(h.ObjectID);                            
                        }
                    }
                    
                    
                    personnerClient.GetEmployeeIDsByParasAsync(StrCompanyIDsList,StrDepartmentIDsList,StrPositionIDsList);
                }
            };
            lookup.MultiSelected = true;
            lookup.SelectSameGradeOnly = true;
            lookup.Show();
        }


        private void BindData(List<T_HR_BALANCEPOSTDETAIL> details)
        {
            dgIssunanceObj.ItemsSource = null;
            if (details == null || details.Count < 1)
            {
                return;
            }
            else
            {
                dgIssunanceObj.ItemsSource = details;
            }

        }
        private void GetCompanyExtOrgObj(List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY> LstOldCompanys, List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY> LstCompanys)
        {
            if (LstCompanys.Count() > 0)
            {
                LstCompanys.ToList().ForEach(child =>
                {
                    StrCompanyIDsList.Add(child.COMPANYID);
                    //issuanceExtOrgObj.Add(item);
                    ExtOrgObj objSecond = new ExtOrgObj();
                    objSecond.ObjectID = child.COMPANYID;
                    objSecond.ObjectName = child.CNAME;
                    objSecond.ObjectType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Company;
                    var ExistEnts = from ext in entall
                                    where ext.ObjectID == child.COMPANYID
                                    && ext.ObjectType == SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Company
                                    select ext;
                    if (ExistEnts.Count() == 0)
                    {
                        entall.Add(objSecond);
                    }
                    var ents = from childcompany in LstOldCompanys
                               where childcompany.T_HR_COMPANY2 != null && childcompany.T_HR_COMPANY2.COMPANYID == child.COMPANYID
                               select childcompany;
                    if (ents.Count() > 0)
                    {
                        GetCompanyExtOrgObj(LstOldCompanys, ents.ToList());
                    }

                });
            }
        }

        #region 删除按钮
        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            Button delBtn = sender as Button;
            //SMT.Saas.Tools.PersonnelWS.V_EMPLOYEEPOST MeetingV = delBtn.Tag as SMT.Saas.Tools.PersonnelWS.V_EMPLOYEEPOST;
            ExtOrgObj ext = delBtn.Tag as ExtOrgObj;
            
            List<Saas.Tools.PersonnelWS.V_EMPLOYEEPOST> listpost = new List<Saas.Tools.PersonnelWS.V_EMPLOYEEPOST>();

            switch (ext.ObjectType)
            {
                
                case SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Personnel:
                    var persons = from ent in issuanceExtOrgObj
                                  where ent.ObjectID == ext.ObjectID
                                  select ent;
                    //listpost = persons.Count() > 0 ? persons.ToList() : null;
                    break;
            }
            if (listpost != null)
            {
                foreach (var ent in listpost)
                {
                    StaffList.Remove(ent.T_HR_EMPLOYEE.EMPLOYEEID);
                }
            }
            
            issuanceExtOrgObj.Remove(ext);
            var ents = from ent in EmployeeBalanceInfoList
                       where ent.EMPLOYEEID == ext.ObjectID
                       select ent;
            if (ents.Count() > 0)
            {
                EmployeeBalanceInfoList.Remove(ents.FirstOrDefault());
            }
            
            //BindData();
        }

        private void dgIssunanceObj_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_HR_BALANCEPOSTDETAIL detail = (T_HR_BALANCEPOSTDETAIL)e.Row.DataContext;


            TextBlock attendancetbl = dgIssunanceObj.Columns[4].GetCellContent(e.Row).FindName("attendancetbl") as TextBlock;
            TextBlock salarytbl = dgIssunanceObj.Columns[5].GetCellContent(e.Row).FindName("salarytbl") as TextBlock;
            
            int index = e.Row.GetIndex();
            var cell = dgIssunanceObj.Columns[0].GetCellContent(e.Row) as TextBlock;
            cell.Text = (index + 1).ToString();

            if (detail.ATTENDANCESET == "1")
            {
                attendancetbl.Text = "是";
            }
            else
            {
                attendancetbl.Text = "否";
            }
            if (detail.SALARYSET == "1")
            {
                salarytbl.Text = "是";
            }
            else
            {
                salarytbl.Text = "否";
            }

    
        }
        #endregion

        


        

        public string GetFullOrgName(string depID)
        {
            List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY> allCompanys = Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>;
            List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT> allDepartments = Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>;
            SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT department = new Saas.Tools.OrganizationWS.T_HR_DEPARTMENT();
            SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY company = new SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY();
            string orgName = string.Empty;
            string fatherType = "0";
            string fatherID = "";
            bool hasFather = false;

            department = (from c in allDepartments
                          where c.DEPARTMENTID == depID
                          select c).FirstOrDefault();
            if (department != null)
            {
                orgName += " - " + department.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                if (!string.IsNullOrEmpty(department.FATHERTYPE) && !string.IsNullOrEmpty(department.FATHERID))
                {
                    fatherType = department.FATHERTYPE;
                    fatherID = department.FATHERID;
                    hasFather = true;
                }
                else
                {
                    hasFather = false;
                }
            }

            while (hasFather)
            {
                if (fatherType == "1" && !string.IsNullOrEmpty(fatherID))
                {
                    department = (from de in allDepartments
                                  where de.DEPARTMENTID == fatherID
                                  select de).FirstOrDefault();
                    if (department != null)
                    {
                        orgName += " - " + department.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                        if (!string.IsNullOrEmpty(department.FATHERTYPE) && !string.IsNullOrEmpty(department.FATHERID))
                        {
                            fatherID = department.FATHERID;
                            fatherType = department.FATHERTYPE;
                        }
                        else
                        {
                            hasFather = false;
                        }
                    }
                    else
                    {
                        hasFather = false;
                    }
                }
                else if (fatherType == "0" && !string.IsNullOrEmpty(fatherID))
                {
                    company = (from com in allCompanys
                               where com.COMPANYID == fatherID
                               select com).FirstOrDefault();

                    if (company != null)
                    {
                        orgName += " - " + company.CNAME;
                        if (!string.IsNullOrEmpty(company.FATHERTYPE) && !string.IsNullOrEmpty(company.FATHERID))
                        {
                            fatherID = company.FATHERID;
                            fatherType = company.FATHERTYPE;
                        }
                        else
                        {
                            hasFather = false;
                        }
                    }
                    else
                    {
                        hasFather = false;
                    }

                }
                else
                {
                    hasFather = false;
                }

            }
            return orgName;
        }
    }
}

