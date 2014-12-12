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
using System.Windows.Browser;
namespace SMT.HRM.UI.Form.Salary
{
    public partial class EmployeeBalancePostAddForm : BaseForm, IEntityEditor, IAudit, IClient
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
        private ObservableCollection<string> DelStaffList = new ObservableCollection<string>();  //删除的员工ID
        private List<ExtOrgObj> issuanceExtOrgObj;
        ObservableCollection<T_HR_BALANCEPOSTDETAIL> listDetails = new ObservableCollection<T_HR_BALANCEPOSTDETAIL>();
        
        private bool needsubmit = false;//提交审核
        private bool isSubmit = false;//用于给保存方法判断是点击了保存还是提交
        string strNotes = string.Empty;
        string strRemarkNotes = string.Empty;
        public EmployeeBalancePostAddForm()
        {
            issuanceExtOrgObj = new List<ExtOrgObj>();
            entall = new List<ExtOrgObj>();
            InitializeComponent();
            FormType = FormTypes.New;
            isNew = true;
            addBalanceAsignID = string.Empty;
            this.Loaded += new RoutedEventHandler(EmployeeBalancePostAddForm_Loaded);
        }

        public EmployeeBalancePostAddForm(FormTypes type, string AsignID)
        {           
            issuanceExtOrgObj = new List<ExtOrgObj>();
            entall = new List<ExtOrgObj>();
            InitializeComponent();
            FormType = type;
            addBalanceAsignID = AsignID;
            
            this.Loaded += new RoutedEventHandler(EmployeeBalancePostAddForm_Loaded);
        }

        void EmployeeBalancePostAddForm_Loaded(object sender, RoutedEventArgs e)
        {
            
            InitParas();
            if (string.IsNullOrEmpty(addBalanceAsignID))
            {
                
                EmployeeAddBalancePost = new T_HR_EMPLOYEESALARYPOSTASIGN();
                EmployeeAddBalancePost.EMPLOYEESALARYPOSTASIGNID = Guid.NewGuid().ToString();
                EmployeeAddBalancePost.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                EmployeeAddBalancePost.CREATEUSERNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
                EmployeeAddBalancePost.CREATEDATE = System.DateTime.Now;
                addBalanceAsignID = EmployeeAddBalancePost.EMPLOYEESALARYPOSTASIGNID;
                EmployeeAddBalancePost.UPDATEDATE = System.DateTime.Now;
                EmployeeAddBalancePost.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                
                EmployeeAddBalancePost.CREATECOMPANYID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                EmployeeAddBalancePost.CREATEDEPARTMENTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
                EmployeeAddBalancePost.CREATEPOSTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
                EmployeeAddBalancePost.CHECKSTATE = Convert.ToInt32(CheckStates.UnSubmit).ToString();
                EmployeeAddBalancePost.OWNERCOMPANYID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                EmployeeAddBalancePost.OWNERDEPARTMENTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
                EmployeeAddBalancePost.OWNERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                EmployeeAddBalancePost.OWNERPOSTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
                this.DataContext = EmployeeAddBalancePost;
                SetToolBar();
                
            }
            else
            {
                NotShow(FormType);
                RefreshUI(RefreshedTypes.ShowProgressBar);
                client.GetBalancePostsByBalanceIDAsync(addBalanceAsignID, EmployeeAddBalancePost);
            }
            if (FormType != FormTypes.Browse &&　FormType  != FormTypes.Audit)
            {
                //Load事件之后，加载完后获取到父控件
                EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                entBrowser.BtnSaveSubmit.Click -= new RoutedEventHandler(entBrowser.btnSubmit_Click);
                entBrowser.BtnSaveSubmit.Click += new RoutedEventHandler(BtnSaveSubmit_Click);
                //perClient.GetEmployeeByIDAsync(SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID);
                //Utility.InitFileLoad("EmployeeLeaveRecord", LeaveRecordID, FormType, uploadFile);
            }
        }

        void BtnSaveSubmit_Click(object sender, RoutedEventArgs e)
        {
            isSubmit = true;
            needsubmit = true;
            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            Save();
        }

        private void NotShow(FormTypes type)
        {
            if (type != FormTypes.Edit && type != FormTypes.Resubmit)
            {
                             
            }
        }

        private void InitParas()
        {
            client.EmployeeBalancePostDeleteCompleted += new EventHandler<EmployeeBalancePostDeleteCompletedEventArgs>(client_EmployeeBalancePostDeleteCompleted);
            client.EmployeeBalancePostADDCompleted += new EventHandler<EmployeeBalancePostADDCompletedEventArgs>(client_EmployeeBalancePostADDCompleted);
            client.BalancePostUpdateCompleted += new EventHandler<BalancePostUpdateCompletedEventArgs>(client_BalancePostUpdateCompleted);
            personnerClient.GetEmployeeIDsByParasByBalancePostCompleted += new EventHandler<GetEmployeeIDsByParasByBalancePostCompletedEventArgs>(personnerClient_GetEmployeeIDsByParasByBalancePostCompleted);
            client.GetBalancePostsByBalanceIDCompleted += new EventHandler<GetBalancePostsByBalanceIDCompletedEventArgs>(client_GetBalancePostsByBalanceIDCompleted);
            personnerClient.GetEmployeesPostBriefByEmployeeIDCompleted += new EventHandler<GetEmployeesPostBriefByEmployeeIDCompletedEventArgs>(personnerClient_GetEmployeesPostBriefByEmployeeIDCompleted);
            //client.EmployeeAddSumUpdateCompleted += new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(client_EmployeeAddSumUpdateCompleted);
            orgClient.GetPostByIdCompleted += new EventHandler<GetPostByIdCompletedEventArgs>(orgClient_GetPostByIdCompleted);
        }

        void orgClient_GetPostByIdCompleted(object sender, GetPostByIdCompletedEventArgs e)
        {
            if (e.Error == null )
            {
                lkPost.DataContext = e.Result;
                lkPost.TxtLookUp.Text = EmployeeAddBalancePost.BALANCEPOSTNAME;
            }
        }

        void client_BalancePostUpdateCompleted(object sender, BalancePostUpdateCompletedEventArgs e)
        {
            if (e.Error != null && !string.IsNullOrEmpty(e.Error.Message))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                if (e.Result != "error")
                {
                    strNotes = e.Result + "，没有设置结算岗位";
                }
                strRemarkNotes = e.strResult;
                if (!string.IsNullOrEmpty(e.Result))
                {
                    
                    ComfirmWindow com = new ComfirmWindow();
                    com.OnSelectionBoxClosed += (obj, result) =>
                    {
                        RefreshUI(RefreshedTypes.ShowProgressBar);
                        client.BalancePostUpdateAsync(EmployeeAddBalancePost, EmployeeBalanceInfoList, "2", strRemarkNotes);
                    };
                    com.SelectionBox("保存确认", e.Result, ComfirmWindow.titlename, strNotes);
                }
                else
                {
                    if (EmployeeAddBalancePost.CHECKSTATE == Utility.GetCheckState(CheckStates.UnSubmit))
                    {
                        if (needsubmit)
                        {
                            try
                            {
                                EntityBrowser entBrowser1 = this.FindParentByType<EntityBrowser>();
                                entBrowser1.ManualSubmit();
                                BackToSubmit();
                            }
                            catch (Exception ex)
                            {
                                RefreshUI(RefreshedTypes.HideProgressBar);
                                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("提交失败"));
                            }
                        }
                        if (!isSubmit)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("UPDATESUCCESSED", Utility.GetResourceStr("CURRENTRECORD", "")));
                            //ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), "修改成功", Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                        }                        
                    }
                    else
                    {
                        if (!isSubmit)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("AUDITSUCCESSED", Utility.GetResourceStr("CURRENTRECORD", "")));
                        }
                    }
                    //ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), "保存成功", Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                    if (needsubmit)
                    {
                        try
                        {
                            EntityBrowser entBrowser1 = this.FindParentByType<EntityBrowser>();
                            entBrowser1.ManualSubmit();
                            FirstToSubmit();
                        }
                        catch (Exception ex)
                        {
                            RefreshUI(RefreshedTypes.HideProgressBar);
                            Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("提交失败"));
                        }
                    }
                    RefreshUI(RefreshedTypes.AuditInfo);
                    RefreshUI(RefreshedTypes.All);
                }
            }
            
            RefreshUI(RefreshedTypes.HideProgressBar);
            RefreshUI(RefreshedTypes.All);
        }
        private void BackToSubmit()
        {
            RefreshUI(RefreshedTypes.AuditInfo);
            needsubmit = false;
        }

        private void FirstToSubmit()
        {
            RefreshUI(RefreshedTypes.AuditInfo);
            
        }
        void client_EmployeeBalancePostADDCompleted(object sender, EmployeeBalancePostADDCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error != null && !string.IsNullOrEmpty(e.Error.Message))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                if (e.Result != "error")
                {
                    strNotes = e.Result ;
                }
                strRemarkNotes = e.strResult;
                if (!string.IsNullOrEmpty(e.Result))
                {
                    ComfirmWindow com = new ComfirmWindow();
                    com.OnSelectionBoxClosed += (obj, result) =>
                    {
                        RefreshUI(RefreshedTypes.ShowProgressBar);
                        client.EmployeeBalancePostADDAsync(EmployeeAddBalancePost, EmployeeBalanceInfoList, "2", strRemarkNotes);
                    };
                    com.SelectionBox("修改确认", e.Result, ComfirmWindow.titlename, e.Result);
                }
                else
                {
                    FormType = FormTypes.Edit;
                    EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                    entBrowser.FormType = FormTypes.Edit;
                    RefreshUI(RefreshedTypes.AuditInfo);
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("ADDDATASUCCESSED"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                }                
                
            }            
            RefreshUI(RefreshedTypes.HideProgressBar);
            RefreshUI(RefreshedTypes.All);
        }

        void client_GetBalancePostsByBalanceIDCompleted(object sender, GetBalancePostsByBalanceIDCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);            
            if (e.Error != null && !string.IsNullOrEmpty(e.Error.Message))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                EmployeeAddBalancePost = e.asign;
                this.DataContext = EmployeeAddBalancePost;
                if (EmployeeAddBalancePost.REMARK != null)
                {
                    txtRemark.Text = EmployeeAddBalancePost.REMARK;
                }
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
                //        var entStaffs = from em in StaffList
                //                        where em == ent.EMPLOYEEID
                //                        select em;
                //        if (entStaffs.Count() == 0)
                //        {
                //            StaffList.Add(ent.EMPLOYEEID);
                //        }
                //    }
                //}
                //issuanceExtOrgObj = entall;
                BindData(EmployeeBalanceInfoList.ToList());
                orgClient.GetPostByIdAsync(EmployeeAddBalancePost.BALANCEPOSTID);
            }
            RefreshUI(RefreshedTypes.AuditInfo); 
            RefreshUI(RefreshedTypes.HideProgressBar);
            RefreshUI(RefreshedTypes.All);
            //if (FormType == FormTypes.Browse || FormType == FormTypes.Audit)
            //{
            //    RefreshUI(RefreshedTypes.AuditInfo); 
            //}
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
                BindData(EmployeeBalanceInfoList.ToList());
 
            }
            
        }

        void client_EmployeeBalancePostDeleteCompleted(object sender, EmployeeBalancePostDeleteCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error != null)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), "删除成功", Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                if (EmployeeAddBalancePost != null)
                {
                    EmployeeAddBalancePost.CHECKSTATE = "1";
                }
                FormType = FormTypes.Browse;
                RefreshUI(RefreshedTypes.All);
                try
                {
                    EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                    entBrowser.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }

        void personnerClient_GetEmployeeIDsByParasByBalancePostCompleted(object sender, GetEmployeeIDsByParasByBalancePostCompletedEventArgs e)
        {

            if (e.Error != null && !string.IsNullOrEmpty(e.Error.Message))
            {
                RefreshUI(RefreshedTypes.HideProgressBar);
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            else
            {
                ObservableCollection<string> employees = e.Result;
                if (employees.Count() > 0)
                {
                    foreach (var ent in employees)
                    {
                        if (!StaffList.Contains(ent))
                        {
                            StaffList.Add(ent); 
                        }
                    }
                }
                //StaffList = e.Result;
                personnerClient.GetEmployeesPostBriefByEmployeeIDAsync(StaffList);
            }
        }

               
        private void SetToolBar()
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
                ToolbarItems = Utility.CreateFormEditButton("T_HR_EMPLOYEESALARYPOSTASIGN", EmployeeAddBalancePost.OWNERID,
                    EmployeeAddBalancePost.OWNERPOSTID, EmployeeAddBalancePost.OWNERDEPARTMENTID, EmployeeAddBalancePost.OWNERCOMPANYID);
            }
            RefreshUI(RefreshedTypes.All);

        }

        #region IEntityEditor
        public string GetTitle()
        {
            return "薪资结算岗位变更";
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
                case "Delete":
                    Delete(EmployeeAddBalancePost.EMPLOYEESALARYPOSTASIGNID);
                    break;
            }

        }
        private void Delete(string id)
        {
            string Result = "";
            string strMsg = string.Empty;
            //提示是否删除
            ObservableCollection<string> ids = new ObservableCollection<string>();
            ids.Add(id);
            ComfirmWindow com = new ComfirmWindow();
            com.OnSelectionBoxClosed += (obj, result) =>
            {
                RefreshUI(RefreshedTypes.ShowProgressBar);
                client.EmployeeBalancePostDeleteAsync(ids);
            };
            com.SelectionBox(Utility.GetResourceStr("DELETECONFIRM"), Utility.GetResourceStr("确定要删除吗？"), ComfirmWindow.titlename, Result);
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
        private AutoDictionary basedata(string TableName, string Name, string Value, string Text, string keyValue)
        {
            string[] strlist = new string[5];
            strlist[0] = TableName;
            strlist[1] = Name;
            strlist[2] = Value;
            strlist[3] = Text;
            strlist[4] = keyValue;
            AutoDictionary ad = new AutoDictionary();
            ad.AutoDictionaryChiledList(strlist);
            return ad;
        }
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

            foreach (T_HR_BALANCEPOSTDETAIL objDetail in EmployeeBalanceInfoList)//填充子表
            {
                string strSalary = "";
                string strAttendance = "";
                if (objDetail.ATTENDANCESET == "0")
                {
                    strAttendance = "否";                    
                }
                else
                {
                    strAttendance = "是";
                }
                if (objDetail.SALARYSET == "0")
                {
                    strSalary = "否";                    
                }
                else
                {
                    strSalary = "是";
                }
                AutoList.Add(basedata("T_HR_BALANCEPOSTDETAIL", "ATTENDANCESET", objDetail.ATTENDANCESET, strAttendance, objDetail.BALANCEPOSTDETAIL));
                AutoList.Add(basedata("T_HR_BALANCEPOSTDETAIL", "SALARYSET", objDetail.SALARYSET, strSalary, objDetail.BALANCEPOSTDETAIL));                
            }
            string a = mx.TableToXml(Info, EmployeeBalanceInfoList, StrSource, AutoList);
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
            Form.Salary.EmployeeBalancePostDetailForm form = new Form.Salary.EmployeeBalancePostDetailForm(FormTypes.Audit, EmployeeAddBalancePost.EMPLOYEESALARYPOSTASIGNID);
            EntityBrowser browser = new EntityBrowser(form);
            form.MinWidth = 600;
            form.MinHeight = 240;            
            browser.FormType = FormTypes.Audit;
            browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
            try
            {
                EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                entBrowser.Close();
            }
            catch (Exception ex)
            { 

            }
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
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns>保存结果</returns>
        public bool Save()
        {
            RefreshUI(RefreshedTypes.ShowProgressBar);
            if (string.IsNullOrEmpty(EmployeeAddBalancePost.BALANCEPOSTID))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "结算岗位不能为空", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                RefreshUI(RefreshedTypes.HideProgressBar);
                return false;
            }
            if (EmployeeBalanceInfoList.Count == 0)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "员工不能为空", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                RefreshUI(RefreshedTypes.HideProgressBar);
                return false;
            }
            string remark = string.Empty;
            remark = txtRemark.Text.Trim();
            EmployeeAddBalancePost.REMARK = remark;
            foreach (var ent in EmployeeBalanceInfoList)
            {
                if (ent.ATTENDANCESET == "0" && ent.SALARYSET == "0")
                {
                    string strMsg = "岗位："+ ent.EMPLOYEEPOSTNAME +"，必须在薪资结算或考勤结算中勾选一项";
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), strMsg, Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                    RefreshUI(RefreshedTypes.HideProgressBar);
                    return false;
                }
            }
            if (FormType == FormTypes.New)
            {
                client.EmployeeBalancePostADDAsync(EmployeeAddBalancePost, EmployeeBalanceInfoList, "1", strRemarkNotes);
            }
            else
            {
                client.BalancePostUpdateAsync(EmployeeAddBalancePost, EmployeeBalanceInfoList, "1", strRemarkNotes);                
            }
            return true;
        }
        /// <summary>
        /// 保存并关闭当前窗口
        /// </summary>
        public void Cancel()
        {
            bool flag = false;
            flag = Save();

            if (flag)
            {
                EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                entBrowser.Close();
            }
        }



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
            
            AddIssuanObj();
        }

        private void AddIssuanObj()
        {
            StrCompanyIDsList.Clear();
            StrDepartmentIDsList.Clear();
            StrPositionIDsList.Clear();
            entall = new List<ExtOrgObj>();
            SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup lookup = new SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup();
            lookup.SelectedObjType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Post;
            lookup.SelectedClick += (obj, ev) =>
            {
                List<ExtOrgObj> ent = lookup.SelectedObj as List<ExtOrgObj>;
                if (ent != null && ent.Count > 0)
                {
                    //issuanceExtOrgObj = ent;
                    foreach (var h in ent)
                    {   
                        if (h.ObjectType == SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Post)//岗位
                        {
                            var entPosts = from entc in StrPositionIDsList
                                           where entc == h.ObjectID
                                           select entc;
                            if (entPosts.Count() == 0)
                            {
                                StrPositionIDsList.Add(h.ObjectID);
                            }
                        }
                        SMT.Saas.Tools.OrganizationWS.T_HR_POST ent1 = h.ObjectInstance as SMT.Saas.Tools.OrganizationWS.T_HR_POST;
                        
                        if (ent1 == null)
                        {
                            //Utility.ShowCustomMessage(MessageTypes.Message, "员工岗位", "当前选取的岗位为空");
                            ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), "当前选取的岗位为空",
                        Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                            return;
                        }
                        string postID = string.Empty;
                        string postName = string.Empty;
                        string departID = string.Empty;
                        string departName = string.Empty;
                        string companyID = string.Empty;
                        string companyName = string.Empty;
                        postID = ent1.POSTID;
                        postName = ent1.T_HR_POSTDICTIONARY.POSTNAME;
                        if (ent1.T_HR_DEPARTMENT != null)
                        {
                            departID = ent1.T_HR_DEPARTMENT.DEPARTMENTID; 
                            if (ent1.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY != null)
                            {
                                 departName =  ent1.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                            }
                            if (ent1.T_HR_DEPARTMENT.T_HR_COMPANY != null)
                            {
                                companyID = ent1.T_HR_DEPARTMENT.T_HR_COMPANY.COMPANYID;
                                
                                if (!string.IsNullOrEmpty(ent1.T_HR_DEPARTMENT.T_HR_COMPANY.BRIEFNAME))
                                {
                                    companyName = ent1.T_HR_DEPARTMENT.T_HR_COMPANY.BRIEFNAME;
                                }
                                else
                                {
                                    companyName = ent1.T_HR_DEPARTMENT.T_HR_COMPANY.CNAME;
                                }
                            }
                        }
                        //ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), "开始添加明细记录",
                        //Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                        T_HR_BALANCEPOSTDETAIL detail = new T_HR_BALANCEPOSTDETAIL();
                        detail.BALANCEPOSTDETAIL = Guid.NewGuid().ToString();
                        detail.BALANCEPOSTASIGNID = EmployeeAddBalancePost.EMPLOYEESALARYPOSTASIGNID;
                        detail.EMPLOYEEID = "";
                        detail.EMPLOYEENAME = "";
                        detail.EMPLOYEEPOSTID = postID;
                        detail.EMPLOYEEPOSTNAME = postName;
                        detail.EMPLOYEEDEPARTMENTID = departID;
                        detail.EMPLOYEEDEPARTMENTNAME = departName;
                        detail.EMPLOYEECOMPANYID = companyID;
                        detail.EMPLOYEECOMPANYNAME = companyName;
                        detail.SALARYSET = "0";
                        detail.ATTENDANCESET = "0";
                        //detail.EDITSTATE = "0";
                        detail.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                        detail.UPDATEDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                        detail.CREATEDATE = DateTime.Now;
                        detail.UPDATEDATE = DateTime.Now;
                        var exist = from a in EmployeeBalanceInfoList
                                    where a.EMPLOYEEPOSTID == postID
                                    select a;
                        if (exist.Count() == 0)
                        {
                            EmployeeBalanceInfoList.Add(detail);
                        }
                        
                    }
                    //RefreshUI(RefreshedTypes.ShowProgressBar);
                    //personnerClient.GetEmployeeIDsByParasByBalancePostAsync(StrCompanyIDsList, StrDepartmentIDsList, StrPositionIDsList);
                    
                }
                BindData(EmployeeBalanceInfoList.ToList());
                
            };
            lookup.MultiSelected = true;
            lookup.SelectSameGradeOnly = true;
            lookup.Show();
        }


        private void BindData(List<T_HR_BALANCEPOSTDETAIL> listDetails)
        {
            dgIssunanceObj.ItemsSource = null;
            if (listDetails == null || listDetails.Count < 1)
            {
                return;
            }
            else
            {                
                dgIssunanceObj.ItemsSource = listDetails;
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
            T_HR_BALANCEPOSTDETAIL ext = delBtn.Tag as T_HR_BALANCEPOSTDETAIL;
            
            List<Saas.Tools.PersonnelWS.V_EMPLOYEEPOST> listpost = new List<Saas.Tools.PersonnelWS.V_EMPLOYEEPOST>();

            
            if (listpost != null)
            {
                foreach (var ent in listpost)
                {
                    StaffList.Remove(ent.T_HR_EMPLOYEE.EMPLOYEEID);
                }
            }
            
            var ents = from ent in EmployeeBalanceInfoList
                       where ent.BALANCEPOSTDETAIL == ext.BALANCEPOSTDETAIL
                       select ent;
            if (ents.Count() > 0)
            {
                EmployeeBalanceInfoList.Remove(ents.FirstOrDefault());                
            }

            BindData(EmployeeBalanceInfoList.ToList());
        }

        private void dgIssunanceObj_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //SMT.Saas.Tools.PersonnelWS.V_EMPLOYEEPOST StaffV = (SMT.Saas.Tools.PersonnelWS.V_EMPLOYEEPOST)e.Row.DataContext;
            T_HR_BALANCEPOSTDETAIL detail = (T_HR_BALANCEPOSTDETAIL)e.Row.DataContext;
            
            
            CheckBox attendanceCheckbox = dgIssunanceObj.Columns[4].GetCellContent(e.Row).FindName("attendanceCheckbox") as CheckBox;
            CheckBox salaryCheckbox = dgIssunanceObj.Columns[5].GetCellContent(e.Row).FindName("salaryCheckbox") as CheckBox;
            Button DelBtn = dgIssunanceObj.Columns[6].GetCellContent(e.Row).FindName("BtnDel") as Button;

            DelBtn.Tag = detail;
            int index = e.Row.GetIndex();
            var cell = dgIssunanceObj.Columns[0].GetCellContent(e.Row) as TextBlock;
            cell.Text = (index + 1).ToString();
            if (EmployeeAddBalancePost.CHECKSTATE != "0")
            {
                DelBtn.IsEnabled = false; 
            }
            if (detail.ATTENDANCESET == "1")
            {
                attendanceCheckbox.IsChecked = true;
            }            
            if (detail.SALARYSET == "1")
            {
                salaryCheckbox.IsChecked = true;
            }
            
            //ObservableCollection<T_HR_BALANCEPOSTDETAIL> objs = dgIssunanceObj.ItemsSource as ObservableCollection<T_HR_BALANCEPOSTDETAIL>;
            //if (FormType != FormTypes.New)
            //{
            //    if (dgIssunanceObj.ItemsSource != null && EmployeeBalanceInfoList != null)
            //    {
            //        foreach (var obje in objs)
            //        {
            //            if (obje.BALANCEPOSTDETAIL == detail.BALANCEPOSTDETAIL)
            //            { 
            //                if()
            //            }
            //        }
            //    }
            //}
        }
        #endregion

        private void lkPost_FindClick(object sender, EventArgs e)
        {
            
            OrganizationLookup lookup = new OrganizationLookup();
            lookup.SelectedObjType = OrgTreeItemTypes.Post;
            lookup.SelectedClick += (obj, ev) =>
            {
                SMT.Saas.Tools.OrganizationWS.T_HR_POST ent = lookup.SelectedObj[0].ObjectInstance as SMT.Saas.Tools.OrganizationWS.T_HR_POST;
                //T_HR_POST entity = Utility.CloneObject<T_HR_POST>(ent);
                if (ent == null)
                {
                    //Utility.ShowCustomMessage(MessageTypes.Message, "员工岗位", "当前选取的岗位为空");
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), "当前选取的岗位为空",
                Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                    return;
                }
                HandlePostChanged(ent);
                //orclient.GetPostNumberAsync(ent.POSTID);

            };

            lookup.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
        }


        private void HandlePostChanged(SMT.Saas.Tools.OrganizationWS.T_HR_POST ent)
        {
            lkPost.DataContext = ent;
            EmployeeAddBalancePost.BALANCEPOSTID = ent.POSTID;                        
            string orgName = ent.T_HR_POSTDICTIONARY.POSTNAME ;//+ "-"GetFullOrgName(ent.T_HR_DEPARTMENT.DEPARTMENTID);
            if (ent.T_HR_DEPARTMENT != null)
            {
                if (ent.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY != null)
                {
                    orgName += "-" + ent.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                }
                if (ent.T_HR_DEPARTMENT.T_HR_COMPANY != null)
                {
                    orgName += "-" + ent.T_HR_DEPARTMENT.T_HR_COMPANY.BRIEFNAME;
                }
            }
            lkPost.TxtLookUp.Text = orgName;
            EmployeeAddBalancePost.BALANCEPOSTNAME = orgName;
        }

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

        #region 考勤设置
        
        private void attendanceCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked.Value)
            {
                T_HR_BALANCEPOSTDETAIL btlist = (T_HR_BALANCEPOSTDETAIL)chk.DataContext;
                if (btlist != null)
                {
                    var ents = from ent in EmployeeBalanceInfoList
                               where ent.BALANCEPOSTDETAIL == btlist.BALANCEPOSTDETAIL
                               select ent;
                    if (ents.Count() > 0)
                    {
                        int k = EmployeeBalanceInfoList.IndexOf(ents.FirstOrDefault());
                        EmployeeBalanceInfoList[k].ATTENDANCESET = "1";                        
                    }
                }
            }
        }

        private void attendanceCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked.Value)
            {
                T_HR_BALANCEPOSTDETAIL btlist = (T_HR_BALANCEPOSTDETAIL)chk.DataContext;
                if (btlist != null)
                {
                    var ents = from ent in EmployeeBalanceInfoList
                               where ent.BALANCEPOSTDETAIL == btlist.BALANCEPOSTDETAIL
                               select ent;
                    if (ents.Count() > 0)
                    {
                        int k = EmployeeBalanceInfoList.IndexOf(ents.FirstOrDefault());
                        EmployeeBalanceInfoList[k].ATTENDANCESET = "0";
                    }
                }
            }
        }
        #endregion

        #region 薪资设置

        private void salaryCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked.Value)
            {
                T_HR_BALANCEPOSTDETAIL btlist = (T_HR_BALANCEPOSTDETAIL)chk.DataContext;
                if (btlist != null)
                {
                    var ents = from ent in EmployeeBalanceInfoList
                               where ent.BALANCEPOSTDETAIL == btlist.BALANCEPOSTDETAIL
                               select ent;
                    if (ents.Count() > 0)
                    {
                        int k = EmployeeBalanceInfoList.IndexOf(ents.FirstOrDefault());
                        EmployeeBalanceInfoList[k].SALARYSET = "1";
                    }
                }
            }
        }

        private void salaryCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk.IsChecked.Value)
            {
                T_HR_BALANCEPOSTDETAIL btlist = (T_HR_BALANCEPOSTDETAIL)chk.DataContext;
                if (btlist != null)
                {
                    var ents = from ent in EmployeeBalanceInfoList
                               where ent.BALANCEPOSTDETAIL == btlist.BALANCEPOSTDETAIL
                               select ent;
                    if (ents.Count() > 0)
                    {
                        int k = EmployeeBalanceInfoList.IndexOf(ents.FirstOrDefault());
                        EmployeeBalanceInfoList[k].SALARYSET = "0";
                    }
                }
            }
        }
        #endregion

    }
}

