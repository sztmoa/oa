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
using SMT.SaaS.FrameworkUI.Common;
using SMT.FBAnalysis.ClientServices.DailyManagementWS;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.FBAnalysis.UI.CommonForm;
using System.Collections.ObjectModel;
using SMT.Saas.Tools.PersonnelWS;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.MobileXml;
using SMT.SAAS.ClientServices;
using SMT.SAAS.ClientUtility;
using System.Text.RegularExpressions;
using System.Windows.Browser;
//using SMT.SaaS.FrameworkUI.FileUpload;

namespace SMT.FBAnalysis.UI.Form
{
    public partial class BorrowAppForm : BaseForm, IClient, IEntityEditor, IAudit
    {
        #region 全局变量
        private string strOwnerCompanyID = ""; //所属人公司ID
        private string strOwnerCompanyName = ""; //所属人公司Name
        private string strOwnerDepartmentID = ""; //所属人部门ID
        private string strOwnerDepartmentName = ""; //所属人部门Name
        private string strOwnerPostID = ""; //所属人岗位ID
        private string strOwnerPostName = ""; //所属人岗位Name
        private string strOwnerID = ""; //所属人ID
        private string strOwnerName = ""; //所属人Name

        private FormTypes FormTypesAction;//操作定义 增加、修改、审核、查看、重新提交
        private RefreshedTypes saveType = RefreshedTypes.All;//保存的类型默认为ALL
        public event refreshGridView ReloadDataEvent;
        public SubjectApp_sel frmU;
        private FormTypes types;
        private string borID = string.Empty;
        private int nBorrowType = 1;
        bool isAdd = true;
        private bool isSubmitFlow = false;
        private string txPostLevel = string.Empty;

        public bool needsubmit = false; //add zl

        PersonnelServiceClient personclient = new PersonnelServiceClient();
        T_FB_BORROWAPPLYMASTER BorrowMasterEntity = new T_FB_BORROWAPPLYMASTER();
        ObservableCollection<T_FB_BORROWAPPLYDETAIL> borrowDtlList = new ObservableCollection<T_FB_BORROWAPPLYDETAIL>();
        ObservableCollection<T_FB_BORROWAPPLYDETAIL> boDtlobj = new ObservableCollection<T_FB_BORROWAPPLYDETAIL>();
        ObservableCollection<T_FB_BUDGETACCOUNT> budgetList = new ObservableCollection<T_FB_BUDGETACCOUNT>();
        public DailyManagementServicesClient client = new DailyManagementServicesClient();

        DictionaryManager DictManager = new DictionaryManager();
        List<string> ListDict = new List<string>(); //字典列表

        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数,定义WCF事件及初始化变量
        /// </summary>
        /// <param name="strBorrowApplyID">个人费用申请ID,添加为空</param>
        /// <param name="FormTypesAction">操作类型：添加、修改、查看、审核、重新提交</param>
        public BorrowAppForm(FormTypes ActionType, string strBorrowApplyID)
        {
            CheckConverter();
            InitializeComponent();

            this.types = ActionType;
            borID = strBorrowApplyID;
            this.FormTypesAction = ActionType;

            this.Loaded += (sender, args) =>
            {
                BorrowAppForm_Loaded(sender, args);
                WcfRegister();//Wcf事件注册             
            };

            //ctrFile.SystemName = "FB";
            //ctrFile.ModelName = "BorrowApp";
            //ctrFile.EntityEditor = this;
            //if (FormTypesAction == FormTypes.Audit || FormTypesAction == FormTypes.Browse)
            //{
            //    ctrFile.FileState = SMT.SaaS.FrameworkUI.FileUpload.Constants.FileStates.FileBrowse;
            //    ctrFile.InitBtn(Visibility.Collapsed, Visibility.Collapsed);
            //    ctrFile.Load_fileData(borID, this);
            //}
            //else
            //{
            //    ctrFile.InitBtn(Visibility.Visible, Visibility.Collapsed);
            //    if (!string.IsNullOrEmpty(borID))
            //    {
            //        ctrFile.Load_fileData(borID, this);
            //    }
            //}
        }

        /// <summary>
        /// 构造函数 新增时使用（新建单据） 2011-10-11 王继华
        /// </summary>        
        public BorrowAppForm()
        {
            CheckConverter();
            InitializeComponent();
            this.types = FormTypes.New;
            borID = string.Empty;
            this.FormTypesAction = FormTypes.New;
            this.Loaded += (sender, args) =>
            {
                BorrowAppForm_Loaded(sender, args);
                WcfRegister();//Wcf事件注册             
            };

            //ctrFile.SystemName = "FB";
            //ctrFile.ModelName = "BorrowApp";
            //ctrFile.EntityEditor = this;
            //ctrFile.InitBtn(Visibility.Visible, Visibility.Collapsed);
            //if (!string.IsNullOrEmpty(borID))
            //{
            //    ctrFile.Load_fileData(borID, this);
            //}
        }

        //add zl
        void BtnSaveSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!Check())
            {
                return;
            }

            if (BorrowMasterEntity.OWNERID != SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("OPERATINGWITHOUTAUTHORITY"));
                return;
            }

            //EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            //entBrowser.BtnSaveSubmit.IsEnabled = false;
            needsubmit = true;
            SaveBorrowApply(BorrowMasterEntity);
        }
        //add end

        private void CheckConverter()
        {
            if (Application.Current.Resources["CompanyInfoConverter"] == null)
            {
                Application.Current.Resources.Add("CompanyInfoConverter", new SMT.FBAnalysis.UI.CompanyInfoConverter());
            }
            if (Application.Current.Resources["DictionaryConverter"] == null)
            {
                Application.Current.Resources.Add("DictionaryConverter", new SMT.FBAnalysis.UI.DictionaryConverter());
            }
            if (Application.Current.Resources["CustomDateConverter"] == null)
            {
                Application.Current.Resources.Add("CustomDateConverter", new SMT.FBAnalysis.UI.CustomDateConverter());
            }
            if (Application.Current.Resources["SubjectUsableMoneyFormat"] == null)
            {
                Application.Current.Resources.Add("SubjectUsableMoneyFormat", new SMT.FBAnalysis.UI.SubjectUsableMoneyFormat());
            }
        }

        private void WcfRegister()
        {
            client.GetBorrowApplyMasterByIDCompleted += new EventHandler<GetBorrowApplyMasterByIDCompletedEventArgs>(client_GetBorrowApplyMasterByIDCompleted);
            client.AddBorrowApplyMasterAndDetailCompleted += new EventHandler<AddBorrowApplyMasterAndDetailCompletedEventArgs>(client_AddBorrowApplyMasterAndDetailCompleted);
            client.UptBorrowApplyMasterAndDetailCompleted += new EventHandler<UptBorrowApplyMasterAndDetailCompletedEventArgs>(client_UptBorrowApplyMasterAndDetailCompleted);
            personclient.GetEmployeePostBriefByEmployeeIDCompleted += new EventHandler<GetEmployeePostBriefByEmployeeIDCompletedEventArgs>(personClient_GetEmployeePostBriefByEmployeeIDCompleted);
            client.GetBorrowApplyDetailByMasterIDCompleted += new EventHandler<GetBorrowApplyDetailByMasterIDCompletedEventArgs>(client_GetBorrowApplyDetailByMasterIDCompleted);
            //add zl
            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            entBrowser.BtnSaveSubmit.Click -= new RoutedEventHandler(entBrowser.btnSubmit_Click);
            entBrowser.BtnSaveSubmit.Click += new RoutedEventHandler(BtnSaveSubmit_Click);
            client.GetBorrowOrderCodeCompleted += new EventHandler<GetBorrowOrderCodeCompletedEventArgs>(client_GetBorrowOrderCodeCompleted);
            client.DelBorrowApplyMasterAndDetailCompleted += new EventHandler<DelBorrowApplyMasterAndDetailCompletedEventArgs>(client_DelBorrowApplyMasterAndDetailCompleted);
            //add end
        }

        

        /// <summary>
        /// 根据员工ID获取员工信息，速度慢可以加一个新的接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void personclient_GetEmployeeByIDCompleted(object sender, GetEmployeeByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    var emp = e.Result;
                    if (emp != null)
                    {
                        this.txtPayInfo.Text = emp.BANKID + ":" + emp.BANKCARDNUMBER;
                    }
                }

            }
            catch
            {
                ///暂无
            }
        }

        private List<string> InitDictValue()
        {
            List<string> strList = new List<string>();
            strList.Add("BorrowType");
            return strList;
        }

        void DictManager_OnDictionaryLoadCompleted(object sender, OnDictionaryLoadArgs e)
        {
            if (e.Error == null)
            {
                this.Loaded += new RoutedEventHandler(BorrowAppForm_Loaded);
            }
        }

        void BorrowAppForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (FormTypesAction != FormTypes.New)
            {
                btnLookUpOwner.IsEnabled = false;//by xiedx  修改不能选其他人
                if (FormTypesAction == FormTypes.Browse || FormTypesAction == FormTypes.Audit)
                {
                    txtCode.IsReadOnly = false;
                    txtRemark.IsReadOnly = false;
                    txtOwnerID.IsReadOnly = false;
                    RepayDate.IsEnabled = false;
                    txtPayInfo.IsReadOnly = false;

                }
            }
            else
            {
                this.LayoutRoot.DataContext = BorrowMasterEntity;
               
               
            }

            InitData();
        }

        #endregion

        #region 初始化
        private void InitData()
        {
            if (types == FormTypes.New)
            {
                BorrowMasterEntity = new T_FB_BORROWAPPLYMASTER();
                BorrowMasterEntity.BORROWAPPLYMASTERID = System.Guid.NewGuid().ToString();
                BorrowMasterEntity.CHECKSTATES = 0;
                txPostLevel = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostLevel.ToString();
                SMT.FBAnalysis.UI.Common.Utility.InitFileLoad("BorrowApp", BorrowMasterEntity.BORROWAPPLYMASTERID, types, uploadFile);
            }
            else
            {
                client.GetBorrowApplyMasterByIDAsync(borID);
                SMT.FBAnalysis.UI.Common.Utility.InitFileLoad("BorrowApp", borID, types, uploadFile);
            }
            SetForms();
        }

        private void SetForms()
        {
            if (types == FormTypes.Audit || types == FormTypes.Browse)
            {
                txtOwnerID.IsReadOnly = true;
                txtRemark.IsReadOnly = true;
                btnLookUpOwner.IsEnabled = false;
                RepayDate.IsEnabled = false;
                txtCode.IsReadOnly = true;
                txtRemark.IsReadOnly = true;
                txtPayInfo.IsReadOnly = true;
                txttotal.IsReadOnly = true;
                rabGeneral.IsEnabled = false;
                rabBackup.IsEnabled = false;
                rabSpecial.IsEnabled = false;
                //AddSub.IsEnabled = false;
                dgvBorrowDetailList.IsReadOnly = true;
                dgvBorrowDetailList.IsEnabled = false;
                this.rbSelf.IsEnabled = false;
                this.rbOther.IsEnabled = false;
                this.wtOther.IsReadOnly = true;
            }
            else if (types == FormTypes.New)
            {
                strOwnerCompanyID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                strOwnerCompanyName = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyName;
                strOwnerDepartmentID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
                strOwnerDepartmentName = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentName;
                strOwnerPostID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
                strOwnerPostName = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostName;
                strOwnerID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                strOwnerName = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
                txtOwnerName.Text = strOwnerName + "-" + strOwnerPostName + "-" + strOwnerDepartmentName + "-" + strOwnerCompanyName;
                //新建借款单是默认添加一天明细信息
                T_FB_BORROWAPPLYDETAIL borent = new T_FB_BORROWAPPLYDETAIL();
                borent.BORROWAPPLYDETAILID = System.Guid.NewGuid().ToString();
                borent.CHARGETYPE = 1;
                borent.REMARK = "";
                borent.BORROWMONEY = 0;
                borent.CREATEDATE = DateTime.Now;
                borent.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                borent.UPDATEDATE = DateTime.Now;
                borent.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                boDtlobj.Add(borent);
                dgvBorrowDetailList.ItemsSource = boDtlobj;
                if(dgvBorrowDetailList.ItemsSource != null)
                {
                    foreach (object obj in dgvBorrowDetailList.ItemsSource)
                    {
                        if (dgvBorrowDetailList.Columns[0].GetCellContent(obj) != null)
                        {
                            TextBox tbMark = dgvBorrowDetailList.Columns[0].GetCellContent(obj).FindName("txtRemark") as TextBox;
                            tbMark.Focus();
                        }
                    }
                }
                this.wtOther.Visibility = Visibility.Collapsed;
                this.txtPayInfo.Visibility = Visibility.Collapsed;
                hideCode();
                //end
            }
        }

        //根据ID获得借款主表数据方法的完成事件
        void client_GetBorrowApplyMasterByIDCompleted(object sender, GetBorrowApplyMasterByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        BorrowMasterEntity = e.Result;

                        personclient.GetEmployeePostBriefByEmployeeIDAsync(BorrowMasterEntity.OWNERID);
                        if (!string.IsNullOrEmpty(BorrowMasterEntity.BORROWAPPLYMASTERCODE.Trim()))
                        {
                            txtCode.Text = BorrowMasterEntity.BORROWAPPLYMASTERCODE;
                        }
                        else
                        {
                            hideCode();
                        }
                        txtRemark.Text = BorrowMasterEntity.REMARK == null ? "" : BorrowMasterEntity.REMARK;
                        txttotal.Text = BorrowMasterEntity.TOTALMONEY.ToString();
                       // txtPayInfo.Text = BorrowMasterEntity.PAYMENTINFO==null?"":BorrowMasterEntity.PAYMENTINFO;     //支付信息
                        if(BorrowMasterEntity.REPAYTYPE==1)
                        {
                            rabGeneral.IsChecked = true;
                            nBorrowType = 1;
                            txtPayInfo.Visibility = Visibility.Collapsed;
                            if (BorrowMasterEntity.PAYTARGET == 1) //2013/12/16号起PAYTARGET字段意思：0原始值，目前不用，1付本人，2付多人
                            {
                                this.rbSelf.IsChecked = true;
                                this.wtOther.Visibility = Visibility.Collapsed;
                            }
                            else if (BorrowMasterEntity.PAYTARGET == 2)
                            {
                                this.rbOther.IsChecked = true;
                                this.wtOther.Text = BorrowMasterEntity.PAYMENTINFO == null ? "" : BorrowMasterEntity.PAYMENTINFO;
                            }
                            else
                            {
                                this.wtOther.Text = BorrowMasterEntity.PAYMENTINFO == null ? "" : BorrowMasterEntity.PAYMENTINFO;
                                this.rbSelf.IsChecked = false;
                                this.rbOther.IsChecked = false;
                            }
                        }
                        else if(BorrowMasterEntity.REPAYTYPE==2)
                        {
                            rabBackup.IsChecked = true;
                            nBorrowType = 2;
                            this.pzGrid.Visibility = Visibility.Collapsed;
                            this.txtPayInfo.Visibility = Visibility.Visible;
                            this.txtPayInfo.Text = BorrowMasterEntity.PAYMENTINFO == null ? "" : BorrowMasterEntity.PAYMENTINFO;
                       
                        }
                        else if(BorrowMasterEntity.REPAYTYPE==3)
                        {
                            rabSpecial.IsChecked = true;
                            nBorrowType = 3;
                            this.pzGrid.Visibility = Visibility.Collapsed;
                            this.txtPayInfo.Visibility = Visibility.Visible;
                            this.txtPayInfo.Text = BorrowMasterEntity.PAYMENTINFO == null ? "" : BorrowMasterEntity.PAYMENTINFO;
                       
                        }

                        if (BorrowMasterEntity.PLANREPAYDATE != null)
                        {
                            RepayDate.Text = BorrowMasterEntity.PLANREPAYDATE.ToString();
                        }
                        strOwnerCompanyID = BorrowMasterEntity.OWNERCOMPANYID;
                        strOwnerCompanyName = BorrowMasterEntity.OWNERCOMPANYNAME;
                        strOwnerDepartmentID = BorrowMasterEntity.OWNERDEPARTMENTID;
                        strOwnerDepartmentName = BorrowMasterEntity.OWNERDEPARTMENTNAME;
                        strOwnerPostID = BorrowMasterEntity.OWNERPOSTID;
                        strOwnerPostName = BorrowMasterEntity.OWNERPOSTNAME;
                        strOwnerID = BorrowMasterEntity.OWNERID;
                        strOwnerName = BorrowMasterEntity.OWNERNAME;

                        ObservableCollection<object> borMasterLst = new ObservableCollection<object>();
                        borMasterLst.Add(BorrowMasterEntity.BORROWAPPLYMASTERID);
                        client.GetBorrowApplyDetailByMasterIDAsync(borMasterLst);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), ex.ToString());
            }
        }

        //根据ID获得借款明细表数据方法的完成事件
        void client_GetBorrowApplyDetailByMasterIDCompleted(object sender, GetBorrowApplyDetailByMasterIDCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                List<T_FB_BORROWAPPLYDETAIL> borrowDetailList = e.Result.ToList();
                boDtlobj = e.Result;
                //borrowDtlList = e.Result;// zl
                dgvBorrowDetailList.ItemsSource = borrowDetailList;
                dgvBorrowDetailList.Loaded += new RoutedEventHandler(dgvBorrowDetailList_Loaded);
            }
        }

        void dgvBorrowDetailList_Loaded(object sender, RoutedEventArgs e)
        {
            if (types == FormTypes.Browse || types == FormTypes.Audit)
            {
                foreach (object obj in dgvBorrowDetailList.ItemsSource)
                {
                    if (dgvBorrowDetailList.Columns[0].GetCellContent(obj) != null)
                    {
                        TextBox txtMark = dgvBorrowDetailList.Columns[0].GetCellContent(obj).FindName("txtRemark") as TextBox;
                        txtMark.IsReadOnly = true;
                    }

                    if (dgvBorrowDetailList.Columns[1].GetCellContent(obj) != null)
                    {
                        TextBox tbMon = dgvBorrowDetailList.Columns[1].GetCellContent(obj).FindName("txtMon") as TextBox;
                        tbMon.IsReadOnly = true;
                    }

                    //if (dgvBorrowDetailList.Columns[2].GetCellContent(obj) != null)
                    //{
                    //    Button btDel = dgvBorrowDetailList.Columns[2].GetCellContent(obj).FindName("myDelete") as Button;
                    //    btDel.IsEnabled = false;
                    //}
                }
            }
        }

        void personclient_GetEmployeeDetailByIDCompleted(object sender, GetEmployeeDetailByIDCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    string PostName = "";
                    string DepartmentName = "";
                    string CompanyName = "";
                    string StrName = "";
                    PostName = e.Result.EMPLOYEEPOSTS[0].T_HR_POST.T_HR_POSTDICTIONARY.POSTNAME;
                    DepartmentName = e.Result.EMPLOYEEPOSTS[0].T_HR_POST.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                    CompanyName = e.Result.EMPLOYEEPOSTS[0].T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.CNAME;
                    StrName = e.Result.T_HR_EMPLOYEE.EMPLOYEECNAME + "-" + PostName + "-" + DepartmentName + "-" + CompanyName;
                    txtOwnerName.Text = StrName;
                    txPostLevel = e.Result.EMPLOYEEPOSTS[0].POSTLEVEL.ToString();
                    ToolTipService.SetToolTip(txtOwnerName, StrName);
                    if (types != FormTypes.Resubmit)
                    {
                        RefreshUI(RefreshedTypes.All);
                    }
                    RefreshUI(RefreshedTypes.AuditInfo);
                }
            }
        }

        void personClient_GetEmployeePostBriefByEmployeeIDCompleted(object sender, GetEmployeePostBriefByEmployeeIDCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    string PostName = "";
                    string DepartmentName = "";
                    string CompanyName = "";
                    string StrName = "";
                    string StrOwnerName = "";


                    V_EMPLOYEEDETAIL employeepost = new V_EMPLOYEEDETAIL();
                    employeepost = e.Result;
                    if (Application.Current.Resources["SYS_PostInfo"] != null)
                    {
                        if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>) != null)
                        {
                            if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == BorrowMasterEntity.OWNERPOSTID) != null)
                            {
                                if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == BorrowMasterEntity.OWNERPOSTID).FirstOrDefault() != null)
                                {
                                    if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == BorrowMasterEntity.OWNERPOSTID).FirstOrDefault().T_HR_POSTDICTIONARY != null)
                                    {
                                        PostName = (Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == BorrowMasterEntity.OWNERPOSTID).FirstOrDefault().T_HR_POSTDICTIONARY.POSTNAME;
                                    }
                                }
                            }
                        }
                    }

                    //ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), StrPostName,Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                    if (Application.Current.Resources["SYS_DepartmentInfo"] != null)
                    {
                        if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>) != null)
                        {
                            if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == BorrowMasterEntity.OWNERDEPARTMENTID) != null)
                            {
                                if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == BorrowMasterEntity.OWNERDEPARTMENTID).FirstOrDefault() != null)
                                {
                                    if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == BorrowMasterEntity.OWNERDEPARTMENTID).FirstOrDefault().T_HR_DEPARTMENTDICTIONARY != null)
                                    {
                                        DepartmentName = (Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == BorrowMasterEntity.OWNERDEPARTMENTID).FirstOrDefault().T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                                    }
                                }
                            }
                        }
                    }

                    if (Application.Current.Resources["SYS_CompanyInfo"] != null)
                    {
                        if ((Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>) != null)
                        {
                            if ((Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(c => c.COMPANYID == BorrowMasterEntity.OWNERCOMPANYID) != null)
                            {
                                if ((Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(c => c.COMPANYID == BorrowMasterEntity.OWNERCOMPANYID).FirstOrDefault() != null)
                                {
                                    CompanyName = (Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(c => c.COMPANYID == BorrowMasterEntity.OWNERCOMPANYID).FirstOrDefault().CNAME;
                                }
                            }
                        }
                    }

                    StrOwnerName = e.Result.EMPLOYEENAME;
                    if (employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == BorrowMasterEntity.OWNERPOSTID) != null)
                    {
                        if (employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == BorrowMasterEntity.OWNERPOSTID).FirstOrDefault() != null)
                        {
                            if (employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == BorrowMasterEntity.OWNERPOSTID).FirstOrDefault().POSTLEVEL != null)
                            {
                                txPostLevel = employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == BorrowMasterEntity.OWNERPOSTID).FirstOrDefault().POSTLEVEL.ToString();//获取出差人的岗位级别
                            }
                        }
                    }


                    StrName = e.Result.EMPLOYEENAME + "-" + PostName + "-" + DepartmentName + "-" + CompanyName;

                    txtOwnerName.Text = StrName;
                    ToolTipService.SetToolTip(txtOwnerName, StrName);

                    if (types != FormTypes.Resubmit)
                    {
                        RefreshUI(RefreshedTypes.All);
                    }
                    RefreshUI(RefreshedTypes.AuditInfo);
                }
            }
        }

        #endregion

        #region 保存个人借款申请记录
        private void RefreshFormType(FormTypes formtype, RefreshedTypes refreshedType)
        {
            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            entBrowser.FormType = formtype;
            types = formtype;
            FormTypesAction = formtype;
            RefreshUI(refreshedType);
            RefreshUI(RefreshedTypes.All);
        }

        private bool Check()
        {
            List<SMT.SaaS.FrameworkUI.Validator.ValidatorBase> validators = Group1.ValidateAll();
            if (validators.Count > 0)
            {
                foreach (var h in validators)
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), Utility.GetResourceStr(h.ErrorMessage));
                    return false;
                }
            }

            if (boDtlobj.Count == 0)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("REQUIRED", "BORROWDETALDATA"));
                return false;
            }
            if (nBorrowType==1)
            {
                   if (this.rbOther.IsChecked.Value && string.IsNullOrWhiteSpace(wtOther.Text))
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("汇多人账户时请填写支付信息"));
                    return false;
                }
            }
            if (dgvBorrowDetailList.ItemsSource != null)
            {
                foreach (object obj in dgvBorrowDetailList.ItemsSource)
                {
                    if (dgvBorrowDetailList.Columns[0].GetCellContent(obj) != null)
                    {
                        TextBox tbMark = dgvBorrowDetailList.Columns[0].GetCellContent(obj).FindName("txtRemark") as TextBox;
                        if (string.IsNullOrEmpty(tbMark.Text))
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "摘要不能为空");
                            return false;
                        }
                    }
                    if (dgvBorrowDetailList.Columns[1].GetCellContent(obj) != null)
                    {
                        decimal i = 0;
                        TextBox tbMon = dgvBorrowDetailList.Columns[1].GetCellContent(obj).FindName("txtMon") as TextBox;
                        if (!decimal.TryParse(tbMon.Text, out i))
                        {
                            Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "请输入正确的数值！");
                            return false;
                        }
                        if (decimal.Parse(tbMon.Text) <= 0)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "借款金额不能小于或等于0！");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void SaveBorrowApply(T_FB_BORROWAPPLYMASTER borrowMaster)
        {
            PersonnelServiceClient psc = new PersonnelServiceClient();
            psc.GetEmployeeDetailByIDCompleted += (sender, e) =>
            {
                RefreshUI(RefreshedTypes.HideProgressBar);
                var employee = e.Result;
                var strMsgE = string.Empty;
                if (employee == null || employee.EMPLOYEEPOSTS == null)
                {
                    strMsgE = "申请人已不存在,请重新建单!";
                }
                else
                {
                    var find = employee.EMPLOYEEPOSTS.FirstOrDefault(item => item.T_HR_POST.POSTID == strOwnerPostID);
                    if (find == null)
                    {
                        strMsgE = "申请人已异动, 请重新建单!";
                    }
                }
                if (!string.IsNullOrEmpty(strMsgE))
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), strMsgE);
                }
                else
                {
                    // 没有错的情况再保存
                    InnerSaveBorrowApply(borrowMaster);
                }
            };
            RefreshUI(RefreshedTypes.ShowProgressBar);
            psc.GetEmployeeDetailByIDAsync(strOwnerID);
        }

        private void InnerSaveBorrowApply(T_FB_BORROWAPPLYMASTER borrowMaster)
        {
            borrowMaster.CREATECOMPANYID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            borrowMaster.CREATECOMPANYNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyName;
            borrowMaster.CREATEDATE = DateTime.Now;
            borrowMaster.CREATEDEPARTMENTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
            borrowMaster.CREATEDEPARTMENTNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentName;
            borrowMaster.CREATEPOSTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
            borrowMaster.CREATEPOSTNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostName;
            borrowMaster.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            borrowMaster.CREATEUSERNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
            borrowMaster.EDITSTATES = 0;
            borrowMaster.OWNERCOMPANYID = strOwnerCompanyID;
            borrowMaster.OWNERCOMPANYNAME = strOwnerCompanyName;
            borrowMaster.OWNERDEPARTMENTID = strOwnerDepartmentID;
            borrowMaster.OWNERDEPARTMENTNAME = strOwnerDepartmentName;
            borrowMaster.OWNERID = strOwnerID;
            borrowMaster.OWNERNAME = strOwnerName;
            borrowMaster.OWNERPOSTID = strOwnerPostID;
            borrowMaster.OWNERPOSTNAME = strOwnerPostName;
            borrowMaster.ISREPAIED = 0;
           
            if (string.IsNullOrEmpty(RepayDate.Text))
            {
                borrowMaster.PLANREPAYDATE = null;
            }
            else
            {
                borrowMaster.PLANREPAYDATE = DateTime.Parse(RepayDate.Text);
            }
            if (txtCode.Text.IndexOf('>') > 0)
            {
                borrowMaster.BORROWAPPLYMASTERCODE = " ";
            }
            else
            {
                borrowMaster.BORROWAPPLYMASTERCODE = txtCode.Text;
            }
            if (nBorrowType==1)//普通借款
            {
                #region 费用报销情况
                if (this.rbSelf.IsChecked.Value)
                {
                    borrowMaster.PAYTARGET = 1;//2013/12/16号起PAYTARGET字段意思：0原始值，目前不用，1付本人，2付多人
                }
                else
                {
                    borrowMaster.PAYTARGET = 2;
                }
                string strPayInfo = string.Empty;
                if (this.rbSelf.IsChecked.Value)
                {
                    strPayInfo += "汇本人账户";
                }
                else
                {
                    strPayInfo += wtOther.Text;
                }
                borrowMaster.PAYMENTINFO = strPayInfo;
                #endregion
            }
            else
            {
                borrowMaster.PAYTARGET = 0;     //待改,保留原来值
                borrowMaster.PAYMENTINFO = txtPayInfo.Text;
            }
            borrowMaster.REMARK = txtRemark.Text;
            borrowMaster.REPAYTYPE = nBorrowType;
            borrowMaster.TOTALMONEY = decimal.Parse(txttotal.Text.ToString());
            borrowMaster.UPDATEDATE = DateTime.Now;
            borrowMaster.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            borrowMaster.UPDATEUSERNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
         

            borrowDtlList.Clear();
            foreach (object obj in dgvBorrowDetailList.ItemsSource)
            {
                T_FB_BORROWAPPLYDETAIL ent = obj as T_FB_BORROWAPPLYDETAIL;
                T_FB_BORROWAPPLYDETAIL borrow = new T_FB_BORROWAPPLYDETAIL();
                borrow.BORROWAPPLYDETAILID = System.Guid.NewGuid().ToString();
                borrow.CHARGETYPE = ent.CHARGETYPE;
                borrow.CREATEDATE = DateTime.Now;
                borrow.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                borrow.T_FB_BORROWAPPLYMASTER = borrowMaster;
                borrow.UPDATEDATE = DateTime.Now;
                borrow.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;

                if (dgvBorrowDetailList.Columns[0].GetCellContent(obj) != null)
                {
                    TextBox txtMark = dgvBorrowDetailList.Columns[0].GetCellContent(obj).FindName("txtRemark") as TextBox;
                    borrow.REMARK = txtMark.Text;
                }
                if (dgvBorrowDetailList.Columns[1].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvBorrowDetailList.Columns[1].GetCellContent(obj).FindName("txtMon") as TextBox;
                    borrow.BORROWMONEY = decimal.Parse(string.IsNullOrEmpty(tbMon.Text.ToString()) == true ? "0" : tbMon.Text.ToString());
                    borrow.UNREPAYMONEY = borrow.BORROWMONEY;
                }
                borrowDtlList.Add(borrow);
            }

            RefreshUI(RefreshedTypes.ShowProgressBar);
            try
            {
                string strMsg = string.Empty;
                if (types == FormTypes.New)
                {
                    client.AddBorrowApplyMasterAndDetailAsync(borrowMaster, borrowDtlList);
                }
                else if (types == FormTypes.Edit)
                {
                    if (needsubmit == false)
                    {
                        client.UptBorrowApplyMasterAndDetailAsync("Edit", borrowMaster, borrowDtlList, strMsg, "Edit");
                    }
                    else
                    {
                        BorrowMasterEntity = borrowMaster;
                        client.GetBorrowOrderCodeAsync(borrowMaster);
                    }
                }
                else if (types == FormTypes.Resubmit)
                {
                    client.UptBorrowApplyMasterAndDetailAsync("ReSubmit", borrowMaster, borrowDtlList, strMsg, "ReSubmit");
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), ex.ToString());
                return;
            }
        }

        //获得单据号后的完成事件，在此事件中再调用方法把单据号存入  2012.2.21
        void client_GetBorrowOrderCodeCompleted(object sender, GetBorrowOrderCodeCompletedEventArgs e)
        {
            string strMsg = string.Empty;
            if (e.Error == null)
            {
                if (!string.IsNullOrEmpty(e.Result)) //获取单据号  zl
                {
                    if(string.IsNullOrWhiteSpace(BorrowMasterEntity.BORROWAPPLYMASTERCODE))
                    {
                        BorrowMasterEntity.BORROWAPPLYMASTERCODE = e.Result;
                    }
                    txtCode.Text = BorrowMasterEntity.BORROWAPPLYMASTERCODE;
                    visibCode();
                    client.UptBorrowApplyMasterAndDetailAsync("Edit", BorrowMasterEntity, borrowDtlList, strMsg, "Edit");
                }
                else
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "产生单据号失败！");
                    return;
                }
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), e.Error.ToString());
                return;
            }
        }

        //增加借款数据后的完成事件
        void client_AddBorrowApplyMasterAndDetailCompleted(object sender, AddBorrowApplyMasterAndDetailCompletedEventArgs e)
        {
            try
            {
                RefreshUI(RefreshedTypes.HideProgressBar);
                if (e.Error == null)
                {
                    if (e.Result)
                    {
                        isAdd = false;
                        Utility.ShowMessageBox("ADD", isSubmitFlow, true);
                        Utility.ShowMessageBox("ADD", isSubmitFlow, true);
                        if (isSubmitFlow)
                        {
                            saveType = RefreshedTypes.CloseAndReloadData;
                            RefreshUI(saveType);
                            return;
                        }
                        //ctrFile.FormID = BorrowMasterEntity.BORROWAPPLYMASTERID;
                        //ctrFile.Save();

                        RefreshFormType(FormTypes.Edit, RefreshedTypes.AuditInfo);

                        if (needsubmit == true)  //add zl //如果点击提交按钮则调用引擎提交审核
                        {
                            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                            entBrowser.ManualSubmit();
                            IsEnabledByFormtype();
                            SetForms();
                            needsubmit = false;
                        }
                    }
                    else
                    {
                        Utility.ShowMessageBox("ADD", isSubmitFlow, false);
                    }
                }
                else
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
                    return;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), ex.ToString());
            }
        }

        //修改借款数据后的完成事件
        void client_UptBorrowApplyMasterAndDetailCompleted(object sender, UptBorrowApplyMasterAndDetailCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error == null)
            {
                if (types == FormTypes.Edit)
                {
                    if(needsubmit == false)
                    {
                        Utility.ShowMessageBox("UPDATE", isSubmitFlow, true);//修改成功     
                    }
                    //ctrFile.FormID = BorrowMasterEntity.BORROWAPPLYMASTERID;
                    //ctrFile.Save();
                    RefreshFormType(FormTypes.Edit, RefreshedTypes.AuditInfo);
                }
                if (types == FormTypes.Resubmit)    //add zl 2012.2.22
                {
                    if (needsubmit == false)
                    {
                        Utility.ShowMessageBox("UPDATE", isSubmitFlow, true);   
                    }
                    //ctrFile.FormID = BorrowMasterEntity.BORROWAPPLYMASTERID;
                    //ctrFile.Save();
                    RefreshFormType(FormTypes.Edit, RefreshedTypes.AuditInfo);
                }
                if (needsubmit == true)  //add zl //如果点击提交按钮则调用引擎提交审核
                {
                    EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                    entBrowser.ManualSubmit();
                    IsEnabledByFormtype();
                    SetForms();
                    needsubmit = false;
                }
            }
            else
            {
                if (types == FormTypes.Edit)
                {
                    Utility.ShowMessageBox("UPDATE", isSubmitFlow, false);//修改失败
                }
                else
                {
                    Utility.ShowMessageBox("AUDITFAILURE", isSubmitFlow, false);//提交失败
                }
            }
        }

        private void IsEnabledByFormtype()
        {
            if (types == FormTypes.Browse || types == FormTypes.Audit)
            {
                foreach (object obj in dgvBorrowDetailList.ItemsSource)
                {
                    if (dgvBorrowDetailList.Columns[0].GetCellContent(obj) != null)
                    {
                        TextBox txtMark = dgvBorrowDetailList.Columns[0].GetCellContent(obj).FindName("txtRemark") as TextBox;
                        txtMark.IsReadOnly = true;
                    }

                    if (dgvBorrowDetailList.Columns[1].GetCellContent(obj) != null)
                    {
                        TextBox tbMon = dgvBorrowDetailList.Columns[1].GetCellContent(obj).FindName("txtMon") as TextBox;
                        tbMon.IsReadOnly = true;
                    }

                    if (dgvBorrowDetailList.Columns[2].GetCellContent(obj) != null)
                    {
                        Button btDel = dgvBorrowDetailList.Columns[2].GetCellContent(obj).FindName("myDelete") as Button;
                        btDel.IsEnabled = false;
                    }
                }
            }
        }

        #endregion

        #region 申请人LookUp

        private void btnLookUpOwner_Click(object sender, RoutedEventArgs e)
        {

            SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup lookup = new SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup();
            lookup.SelectedObjType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Personnel;
            lookup.SelectedClick += (obj, ev) =>
            {
                List<SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj> ent = lookup.SelectedObj as List<SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj>;
                if (ent != null && ent.Count > 0)
                {
                    string Mobile = "";
                    string Tel = "";
                    SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj userInfo = ent.FirstOrDefault();

                    SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj post = (SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj)userInfo.ParentObject;
                    string postid = post.ObjectID;
                    string postName = post.ObjectName;//岗位

                    SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj dept = (SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj)post.ParentObject;
                    string deptid = dept.ObjectID;
                    string deptName = dept.ObjectName;//部门


                    SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY corp = (dept.ObjectInstance as SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT).T_HR_COMPANY;
                    string corpid = corp.COMPANYID;
                    string corpName = corp.CNAME;//公司

                    SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE emp = userInfo.ObjectInstance as SMT.Saas.Tools.PersonnelWS.T_HR_EMPLOYEE;

                    if (emp.MOBILE != null)
                        Mobile = emp.MOBILE.ToString();
                    if (emp.OFFICEPHONE != null)
                        Tel = emp.OFFICEPHONE.ToString();
                    string StrEmployee = userInfo.ObjectName + "-" + post.ObjectName + "-" + dept.ObjectName + "-" + corp.CNAME;
                    txtOwnerName.Text = StrEmployee;
                    //txtTel.Text = userInfo.te
                    strOwnerCompanyID = corpid;
                    strOwnerCompanyName = corp.CNAME;
                    strOwnerDepartmentID = deptid;
                    strOwnerDepartmentName = dept.ObjectName;
                    strOwnerPostID = postid;
                    strOwnerPostName = post.ObjectName;
                    strOwnerID = userInfo.ObjectID;
                    strOwnerName = userInfo.ObjectName;
                    txPostLevel = emp.T_HR_EMPLOYEEPOST.Where(t => t.T_HR_POST.POSTID == postid).FirstOrDefault().POSTLEVEL.ToString();
                    ToolTipService.SetToolTip(txtOwnerName, StrEmployee);


                    //SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj companyInfo = ent.FirstOrDefault();
                    //txtAddUser.Text = companyInfo.ObjectName;
                    //StrAddUserID = userInfo.ObjectID;// companyInfo.ObjectID;
                }
            };
            lookup.MultiSelected = false;
            lookup.Show();
        }
        #endregion

        #region IClient接口
        public void ClosedWCFClient()
        {
            client.DoClose();
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

        #region IEntityEditor接口
        public string GetTitle()//根据状态显示页面标题
        {
            //string Strreturn = "";
            //switch (FormTypesAction)
            //{
            //    case FormTypes.New:
            //        Strreturn = Utility.GetResourceStr("ADDTITLE", "PERSONBORROWAPPLY");
            //        break;
            //    case FormTypes.Edit:
            //        Strreturn = Utility.GetResourceStr("EDITTITLE", "PERSONBORROWAPPLY");
            //        break;
            //    case FormTypes.Audit:
            //        Strreturn = Utility.GetResourceStr("AUDITTITLE", "PERSONBORROWAPPLY");
            //        break;
            //    case FormTypes.Browse:
            //        Strreturn = Utility.GetResourceStr("VIEWTITLE", "PERSONBORROWAPPLY");
            //        break;
            //    case FormTypes.Resubmit:
            //        Strreturn = Utility.GetResourceStr("EDITTITLE", "PERSONBORROWAPPLY");
            //        break;
            //}
            return Utility.GetResourceStr("PERSONBORROWAPPLY"); ;
        }

        public string GetStatus()
        {
            return "";
        }

        public void DoAction(string actionenum)
        {
            if (actionenum == "Delete")
            {
                ComfirmWindow cw = new ComfirmWindow();
                cw.OnSelectionBoxClosed += (o, e) =>
                {
                    if (e.Result == ComfirmWindow.titlename[0])
                    {
                        RefreshUI(RefreshedTypes.ShowProgressBar);
                        client.DelBorrowApplyMasterAndDetailAsync(new ObservableCollection<string>() { BorrowMasterEntity.BORROWAPPLYMASTERID });
                    }
                };
                cw.SelectionBox("确认删除", "你确定要删除吗?", ComfirmWindow.titlename, string.Empty);
                return;


            }
            if (!Check())
            {
                return;
            }

            switch (actionenum)
            {
                case "0"://保存                    
                    SaveBorrowApply(BorrowMasterEntity);
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

        public List<ToolbarItem> GetToolBarItems()//代码隐藏实现按钮加载
        {
            List<ToolbarItem> items = new List<ToolbarItem>();

            if (FormTypesAction != FormTypes.Browse && FormTypesAction != FormTypes.Audit && FormTypesAction != FormTypes.Resubmit)
            {
                if (FormTypesAction != FormTypes.New && BorrowMasterEntity.CHECKSTATES == 0)
                {
                    items.Add(ToolBarItems.Delete);
                }
                ToolbarItem item = new ToolbarItem
                {
                    DisplayType = ToolbarItemDisplayTypes.Image,
                    Key = "0",
                    Title = Utility.GetResourceStr("SAVE"),
                    ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_save.png"

                };
                items.Add(item);
            }
          


            return items;
        }

        public event UIRefreshedHandler OnUIRefreshed;
        public void RefreshUI(RefreshedTypes type)//载入动画
        {
            if (OnUIRefreshed != null)
            {
                UIRefreshedEventArgs args = new UIRefreshedEventArgs();
                args.RefreshedType = type;
                OnUIRefreshed(this, args);
            }
        }

        public void ReloadData()
        {
            if (ReloadDataEvent != null)
            {
                ReloadDataEvent();
            }
        }

        #endregion

        #region IAudit接口
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

        private AutoDictionary basedataForChild(string TableName, string Name, string Value, string Text, string keyValue)
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

        private string GetXmlString(string StrSorce, T_FB_BORROWAPPLYMASTER Info)
        {
            string goouttomeet = string.Empty;
            string privateaffair = string.Empty;
            string companycar = string.Empty;
            string isagent = string.Empty;

            SMT.SaaS.MobileXml.MobileXml mx = new MobileXml();
            SMT.SaaS.MobileXml.AutoDictionary ad = new AutoDictionary();
            List<SMT.SaaS.MobileXml.AutoDictionary> AutoList = new List<SMT.SaaS.MobileXml.AutoDictionary>();
            AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "PAYTARGET", Info.PAYTARGET.ToString(), "个人"));//付款方式
            AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "ISREPAIED", Info.ISREPAIED.ToString(), Info.ISREPAIED.ToString() == "0" ? "否" : "是"));//是否还情

            string StrPayType = "";
            string StrEditState = "";
            switch (Info.REPAYTYPE.ToString())
            {
                case "1":
                    StrPayType = "普通借款";
                    break;
                case "2":
                    StrPayType = "备用金借款";
                    break;
                case "3":
                    StrPayType = "专项借款";
                    break;
            }
            switch (Info.EDITSTATES.ToString())
            {
                case "0":
                    StrEditState = "删除状态";
                    break;
                case "1":
                    StrEditState = "已生效";
                    break;
                case "2":
                    StrEditState = "未生效";
                    break;
                case "3":
                    StrEditState = "撤消中";
                    break;
                case "4":
                    StrEditState = "已撤消";
                    break;
            }
            AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "POSTLEVEL", txPostLevel, null));//POSTLEVEL
            AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "CHECKSTATES", "1", "审核中"));
            AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "REPAYTYPE", Info.REPAYTYPE.ToString(), StrPayType));//相关单据类型
            AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "EDITSTATES", Info.EDITSTATES.ToString(), StrEditState));//编辑状态
            AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "BORROWAPPLYMASTERCODE", Info.BORROWAPPLYMASTERCODE, Info.BORROWAPPLYMASTERCODE));//单据编号
            if (Info.OWNERID != null && !string.IsNullOrEmpty(strOwnerName))
            {
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "OWNERID", Info.OWNERID, strOwnerName + "-" + strOwnerPostName + "-" + strOwnerDepartmentName + "-" + strOwnerCompanyName));
            }
            if (Info.OWNERCOMPANYID != null && !string.IsNullOrEmpty(strOwnerCompanyName))
            {
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "OWNERCOMPANYID", Info.OWNERCOMPANYID, strOwnerCompanyName));
            }
            if (Info.OWNERDEPARTMENTID != null && !string.IsNullOrEmpty(strOwnerDepartmentName))
            {
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "OWNERDEPARTMENTID", Info.OWNERDEPARTMENTID, strOwnerDepartmentName));
            }
            if (Info.OWNERPOSTID != null && !string.IsNullOrEmpty(strOwnerPostName))
            {
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "OWNERPOSTID", Info.OWNERPOSTID, strOwnerPostName));
            }

            ObservableCollection<T_FB_BORROWAPPLYDETAIL> objB;
            if (borrowDtlList != null && borrowDtlList.Count > 0)
            {
                objB = borrowDtlList;
            }
            else
            {
                objB = boDtlobj;
            }
            foreach (T_FB_BORROWAPPLYDETAIL objDetail in objB)
            {
                if (objDetail.T_FB_SUBJECT != null)
                {
                    AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "SUBJECTID", objDetail.T_FB_SUBJECT.SUBJECTID, objDetail.T_FB_SUBJECT.SUBJECTID, objDetail.BORROWAPPLYDETAILID));
                    AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "SUBJECTCODE", objDetail.T_FB_SUBJECT.SUBJECTCODE, objDetail.T_FB_SUBJECT.SUBJECTCODE, objDetail.BORROWAPPLYDETAILID));
                    AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "SUBJECTNAME", objDetail.T_FB_SUBJECT.SUBJECTNAME, objDetail.T_FB_SUBJECT.SUBJECTNAME, objDetail.BORROWAPPLYDETAILID));
                }
                if (objDetail.T_FB_BORROWAPPLYMASTER != null)
                {
                    AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "BORROWAPPLYMASTERID", objDetail.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID, objDetail.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID, objDetail.BORROWAPPLYDETAILID));
                }
                if (objDetail.CHARGETYPE != null)
                {
                    AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "CHARGETYPE", objDetail.CHARGETYPE.ToString(), objDetail.CHARGETYPE.ToString() == "1" ? "个人预算费用" : "公共预算费用", objDetail.BORROWAPPLYDETAILID));
                }

            }
            string a = mx.TableToXml(Info, objB, StrSorce, AutoList);
            return a;

        }

        public void SetFlowRecordEntity(SMT.SaaS.FrameworkUI.AuditControl.Flow_FlowRecord_T entity)
        {
            string strXmlObjectSource = string.Empty;
            //XML
            entity.SystemCode = "FB";
            if (!string.IsNullOrEmpty(entity.BusinessObjectDefineXML)) //返回的XML定义不为空时对业务对象进行填充
            {
                strXmlObjectSource = this.GetXmlString(entity.BusinessObjectDefineXML, BorrowMasterEntity);
            }
            //END
            //strXmlObjectSource = Utility.ObjListToXml<T_FB_BORROWAPPLYMASTER>(BorrowMasterEntity, "FB");
            Dictionary<string, string> paraIDs = new Dictionary<string, string>();
            if (BorrowMasterEntity != null)
            {
                paraIDs.Add("CreateUserID", BorrowMasterEntity.OWNERID);
                paraIDs.Add("CreatePostID", BorrowMasterEntity.OWNERPOSTID);
                paraIDs.Add("CreateDepartmentID", BorrowMasterEntity.OWNERDEPARTMENTID);
                paraIDs.Add("CreateCompanyID", BorrowMasterEntity.OWNERCOMPANYID);
            }

            if (BorrowMasterEntity.CHECKSTATES == ((decimal)CheckStates.UnSubmit) || BorrowMasterEntity.CHECKSTATES == ((decimal)CheckStates.UnApproved))
            {
                SMT.SaaS.FrameworkUI.Common.Utility.SetAuditEntity(entity, "T_FB_BORROWAPPLYMASTER", BorrowMasterEntity.BORROWAPPLYMASTERID, strXmlObjectSource, paraIDs);
            }
            else
            {
                Utility.SetAuditEntity(entity, "T_FB_BORROWAPPLYMASTER", BorrowMasterEntity.BORROWAPPLYMASTERID, strXmlObjectSource);
            }
        }

        public void OnSubmitCompleted(SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult args)
        {
            CheckStates state = CheckStates.UnSubmit;
            string strActionType = "Audit";
            SMT.FBAnalysis.UI.Common.Utility.InitFileLoad(FormTypes.Audit, uploadFile, BorrowMasterEntity.BORROWAPPLYMASTERID, false);
            switch (args)
            {
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Auditing:
                    state = CheckStates.Approving;
                    break;
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Successful:
                    state = CheckStates.Approved;
                    break;
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Fail:
                    state = CheckStates.UnApproved;
                    break;
            }
            //zl add 12.2
            //if (state == CheckStates.Approving)
            //{
            //    client.GetBorrowApplyMasterByIDAsync(BorrowMasterEntity.BorrowMasterEntity);
            //}
            //zl end

            //add zl
            if (types == FormTypes.Edit || types == FormTypes.Resubmit)
            {
                Utility.ShowMessageBox("SUBMIT", isSubmitFlow, true);//提交审核成功
            }
            else if (types == FormTypes.Audit)
            {
                Utility.ShowMessageBox("AUDIT", isSubmitFlow, true);//审核成功
            }

            RefreshUI(RefreshedTypes.HideProgressBar);
            BorrowMasterEntity.CHECKSTATES = Convert.ToInt32(state);
            RefreshUI(RefreshedTypes.AuditInfo);
            RefreshUI(RefreshedTypes.All);
            //add end
        }

        public string GetAuditState()
        {

            string state = "-1";
            if (BorrowMasterEntity != null)
                state = BorrowMasterEntity.CHECKSTATES.ToString();

            return state;
        }

        #endregion

        #region IFileLoadedCompleted接口
        public void FileLoadedCompleted()
        {
            //if (!ctrFile._files.HasAccessory)
            //{
            //    if (FormTypesAction == FormTypes.Browse || FormTypesAction == FormTypes.Audit)
            //    {
            //        SMT.SaaS.FrameworkUI.Common.Utility.HiddenGridRow(this.LayoutRoot, 6);
            //    }
            //}
        }
        #endregion

        #region 页面事件
        private void AddSub_Click(object sender, RoutedEventArgs e)
        {
            if(boDtlobj.Count()==5)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "明细不能超过5条！");
                return;
            }
            T_FB_BORROWAPPLYDETAIL borent = new T_FB_BORROWAPPLYDETAIL();
            borent.BORROWAPPLYDETAILID = System.Guid.NewGuid().ToString();
            borent.CHARGETYPE = 1;
            borent.REMARK = "";
            borent.BORROWMONEY = 0;
            borent.CREATEDATE = DateTime.Now;
            borent.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            borent.UPDATEDATE = DateTime.Now;
            borent.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            boDtlobj.Add(borent);
            dgvBorrowDetailList.ItemsSource = boDtlobj;
        }

        //表格行加载删除按钮
        //private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        //{
        //    T_FB_BORROWAPPLYDETAIL tmp = (T_FB_BORROWAPPLYDETAIL)e.Row.DataContext;
        //    ImageButton MyButton_Delbaodao = dgvBorrowDetailList.Columns[2].GetCellContent(e.Row).FindName("myDelete") as ImageButton;
        //    MyButton_Delbaodao.Margin = new Thickness(0);
        //    MyButton_Delbaodao.AddButtonAction("/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/ico_16_delete.png", Utility.GetResourceStr("DELETE"));
        //    MyButton_Delbaodao.Tag = tmp;
        //}

        //删除表格中的某一行
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            T_FB_BORROWAPPLYDETAIL i = ((Button)sender).DataContext as T_FB_BORROWAPPLYDETAIL;
            if (boDtlobj.Contains(i))
            {
                boDtlobj.Remove(i);
            }
            dgvBorrowDetailList.ItemsSource = boDtlobj;
            txtMon_KeyUp(null, null);
        }

        private void txtMon_KeyUp(object sender, KeyEventArgs e)
        {
            decimal dMoney = 0;
            foreach (object obj in dgvBorrowDetailList.ItemsSource)
            {
                if (dgvBorrowDetailList.Columns[1].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvBorrowDetailList.Columns[1].GetCellContent(obj).FindName("txtMon") as TextBox;
                    decimal dDetailMon = 0;
                    T_FB_BORROWAPPLYDETAIL ent = obj as T_FB_BORROWAPPLYDETAIL;
                    decimal.TryParse(tbMon.Text, out dDetailMon);
                    ent.BORROWMONEY = dDetailMon;
                    dMoney = dMoney + dDetailMon;
                }
            }

            txttotal.Text = dMoney.ToString();
        }

        private void txtMon_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tbMon = sender as TextBox;
            if (string.IsNullOrEmpty(tbMon.Text))
            {
                tbMon.Text = "0";
            }
            else if (!isNumber(tbMon.Text))
            {
                tbMon.Text = "0";
            }
            CalculateBorrow();
        }

        /// <summary>
        /// 算合计
        /// </summary>
        private void CalculateBorrow()
        {
            decimal dMoney = 0;
            foreach (object obj in dgvBorrowDetailList.ItemsSource)
            {
                if (dgvBorrowDetailList.Columns[1].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvBorrowDetailList.Columns[1].GetCellContent(obj).FindName("txtMon") as TextBox;
                    decimal dDetailMon = 0;
                    T_FB_BORROWAPPLYDETAIL ent = obj as T_FB_BORROWAPPLYDETAIL;
                    decimal.TryParse(tbMon.Text, out dDetailMon);
                    ent.BORROWMONEY = dDetailMon;
                    dMoney = dMoney + dDetailMon;
                }
            }

            txttotal.Text = dMoney.ToString();
        }
        /// <summary>
        /// 比较传入的值是不是数字
        /// </summary>
        /// <param name="str">输入框的值</param>
        /// <returns>是数字返回true，否则返回false</returns>
        private bool isNumber(string str)
        {
            bool flag = true;
            string pattern = @"^(-)?(([1-9]+[0-9]*.{1}[0-9]+)|([0].{1}[1-9]+[0-9]*)|([1-9][0-9]*)|([0][.][0-9]+[1-9]*))$";
            Match mText = Regex.Match(str, pattern);   // 匹配正则表达式
            if (!mText.Success)
            {
                flag = false;
            }
            return flag;
        }

        private void rabGeneral_Click(object sender, RoutedEventArgs e)
        {
            nBorrowType = 1;
            this.pzGrid.Visibility = Visibility.Visible;
            this.txtPayInfo.Visibility = Visibility.Collapsed;
        }

        private void rabBackup_Click(object sender, RoutedEventArgs e)
        {
            nBorrowType = 2;
            this.pzGrid.Visibility = Visibility.Collapsed;
            this.txtPayInfo.Visibility = Visibility.Visible;
        }

        private void rabSpecial_Click(object sender, RoutedEventArgs e)
        {
            nBorrowType = 3;
            this.pzGrid.Visibility = Visibility.Collapsed;
            this.txtPayInfo.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 显示单据编号
        /// </summary>
        private void visibCode()
        {
            this.tbCode.Visibility = Visibility.Visible;
            this.txtCode.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 隐藏单据编号
        /// </summary>
        private void hideCode()
        {
            this.tbCode.Visibility = Visibility.Collapsed;
            this.txtCode.Visibility = Visibility.Collapsed;
        }
        private void txttotal_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txttotal.Text))
            {
                txttotal.Text = "0.00";
            }
        }

        #endregion

        private void rbOther_Click(object sender, RoutedEventArgs e)
        {
            if (this.rbOther.IsChecked.Value)
            {
                this.wtOther.Visibility = Visibility.Visible;
            }
            else
            {
                this.wtOther.Visibility = Visibility.Collapsed;
            }
        }

        void client_DelBorrowApplyMasterAndDetailCompleted(object sender, DelBorrowApplyMasterAndDetailCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Result == true)
            {
                //GetData();
                //Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("DELETESUCCESSED", ""));
                // 删除成功
                if (Deleted != null)
                {
                    Deleted(this, null);
                }
                else
                {
                    try
                    {
                        HtmlPage.Window.Invoke("SLCloseCurrentPage");
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("FAILED"), Utility.GetResourceStr("DELETEFAILED", ""));
            }
        }
        public event EventHandler Deleted;
    }
}
