///作者 刘建兴
///开始时间2011-5-4
///修改时间 2011-5-5
///完成时间
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
    public partial class RepayApplyForm : BaseForm, IClient, IEntityEditor, IAudit
    {

        #region 变量定义
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
        public BorrowApp_sel frmU;
        private decimal dBorrowTotal = 0;
        private int nRepayType = 1;
        bool isAdd = true;
        private bool isSubmitFlow = false;
        private FormTypes types;
        private string repID = string.Empty;
        private string txPostLevel = string.Empty;
        public bool needsubmit = false;

        PersonnelServiceClient personclient = new PersonnelServiceClient();
        ObservableCollection<T_FB_BORROWAPPLYDETAIL> borrowDetailData = new ObservableCollection<T_FB_BORROWAPPLYDETAIL>();
        //List<T_FB_BORROWAPPLYDETAIL> BorrowDetailList = new List<T_FB_BORROWAPPLYDETAIL>();
        T_FB_REPAYAPPLYMASTER RepayMasterEntity = new T_FB_REPAYAPPLYMASTER();
        ObservableCollection<T_FB_REPAYAPPLYDETAIL> repayDetailData = new ObservableCollection<T_FB_REPAYAPPLYDETAIL>();
        ObservableCollection<T_FB_REPAYAPPLYDETAIL> repDtlobj = new ObservableCollection<T_FB_REPAYAPPLYDETAIL>();
        public DailyManagementServicesClient client = new DailyManagementServicesClient();

        DictionaryManager DictManager = new DictionaryManager();
        List<string> ListDict = new List<string>(); //字典列表

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="StrRepayApplyID">个人费用申请ID,添加为空</param>
        /// <param name="ActionType">操作的动作 添加、修改、查看、审核、重新提交</param>
        public RepayApplyForm(FormTypes ActionType, string StrRepayApplyID)
        {
            InitializeComponent();
            CheckConverter();

            FormTypesAction = ActionType;
            repID = StrRepayApplyID;
            this.types = ActionType;


            this.Loaded += (sender, args) =>
            {
                RepayApplyForm_Loaded(sender, args);
                WcfRegister();
            };

            //ctrFile.SystemName = "FB";
            //ctrFile.ModelName = "RepayApp";
            //ctrFile.EntityEditor = this;
            //if (FormTypesAction == FormTypes.Audit || FormTypesAction == FormTypes.Browse)
            //{
            //    //ctrFile.FileState = SMT.SaaS.FrameworkUI.FileUpload.Constants.FileStates.FileBrowse;
            //    //ctrFile.InitBtn(Visibility.Collapsed, Visibility.Collapsed);
            //    //ctrFile.Load_fileData(repID, this);
            //}
            //else
            //{
            //    ctrFile.InitBtn(Visibility.Visible, Visibility.Collapsed);
            //    if (!string.IsNullOrEmpty(repID))
            //    {
            //        ctrFile.Load_fileData(repID, this);
            //    }
            //}
        }

         /// <summary>
        /// 构造函数 新增时使用（新建单据） 2011-10-11 王继华
        /// </summary>       
        public RepayApplyForm()
        {
            InitializeComponent();
            CheckConverter();

            FormTypesAction = FormTypes.New;
            repID = string.Empty;
            this.types = FormTypes.New;
            this.Loaded += (sender, args) =>
            {
                RepayApplyForm_Loaded(sender, args);
                WcfRegister();
            };

            //ctrFile.SystemName = "FB";
            //ctrFile.ModelName = "RepayApp";
            //ctrFile.EntityEditor = this;
            //ctrFile.InitBtn(Visibility.Visible, Visibility.Collapsed);
            //if (!string.IsNullOrEmpty(repID))
            //{
            //    ctrFile.Load_fileData(repID, this);
            //}
        }

        private void WcfRegister()
        {
            client.AddRepayApplyMasterAndDetailCompleted += new EventHandler<AddRepayApplyMasterAndDetailCompletedEventArgs>(client_AddRepayApplyMasterAndDetailCompleted);
            client.GetRepayApplyMasterByIDCompleted += new EventHandler<GetRepayApplyMasterByIDCompletedEventArgs>(client_GetRepayApplyMasterByIDCompleted);
            client.UptRepayApplyMasterAndDetailCompleted += new EventHandler<UptRepayApplyMasterAndDetailCompletedEventArgs>(client_UptRepayApplyMasterAndDetailCompleted);
            personclient.GetEmployeePostBriefByEmployeeIDCompleted += new EventHandler<GetEmployeePostBriefByEmployeeIDCompletedEventArgs>(personClient_GetEmployeePostBriefByEmployeeIDCompleted);
            //personclient.GetEmployeeDetailByIDCompleted += new EventHandler<GetEmployeeDetailByIDCompletedEventArgs>(personclient_GetEmployeeDetailByIDCompleted);
            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            entBrowser.BtnSaveSubmit.Click -= new RoutedEventHandler(entBrowser.btnSubmit_Click);
            entBrowser.BtnSaveSubmit.Click += new RoutedEventHandler(BtnSaveSubmit_Click);

            client.GetPersonAccountListByMultSearchCompleted += new EventHandler<GetPersonAccountListByMultSearchCompletedEventArgs>(client_GetPersonAccountListByMultSearchCompleted);
            client.GetRepayApplyDetailListByMasterIDCompleted += new EventHandler<GetRepayApplyDetailListByMasterIDCompletedEventArgs>(client_GetRepayApplyDetailListByMasterIDCompleted);
            client.GetRepayOrderCodeCompleted += new EventHandler<GetRepayOrderCodeCompletedEventArgs>(client_GetRepayOrderCodeCompleted);
            client.DelRepayApplyMasterAndDetailCompleted += new EventHandler<DelRepayApplyMasterAndDetailCompletedEventArgs>(client_DelRepayApplyMasterAndDetailCompleted);
        }

       
        void BtnSaveSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!Check())
            {
                return;
            }

            ///注释掉，可以为别人提还款单，因为有些有借款人突然就走了，别人给他离职时如果有借款离不了职，
            ///所以这可以为别人提还款单
            //if (RepayMasterEntity.OWNERID != SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID)
            //{
            //    Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("OPERATINGWITHOUTAUTHORITY"));
            //    return;
            //}

            //EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            //entBrowser.BtnSaveSubmit.IsEnabled = false;
            needsubmit = true;
            SaveRepayApply(RepayMasterEntity);
        }

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
        }

        private List<string> InitDictValue()
        {
            List<string> strList = new List<string>();
            strList.Add("BorrowType");
            strList.Add("REPAYTYPE");
            return strList;
        }

        void DictManager_OnDictionaryLoadCompleted(object sender, OnDictionaryLoadArgs e)
        {
            if (e.Error == null)
            {
                this.Loaded += new RoutedEventHandler(RepayApplyForm_Loaded);
            }
        }

        void RepayApplyForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (FormTypesAction != FormTypes.New)
            {
                btnLookUpOwner.IsEnabled = false;//by xiedx  修改不能选其他人
                if (FormTypesAction == FormTypes.Browse || FormTypesAction == FormTypes.Audit)
                {
                    txtCode.IsReadOnly = false;
                    txtRemark.IsReadOnly = false;
                    txtOwnerID.IsReadOnly = false;
                }
            }
            else
            {
                txtCode.Text = "<自动生成>";
                this.LayoutRoot.DataContext = RepayMasterEntity;
            }

            InitData();
        }


        #endregion

        #region 初始化
        private void InitData()
        {
            if (types == FormTypes.New)
            {
                RepayMasterEntity.REPAYAPPLYMASTERID = System.Guid.NewGuid().ToString();
                RepayMasterEntity.CHECKSTATES = 0;
                txPostLevel = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostLevel.ToString();
                SMT.FBAnalysis.UI.Common.Utility.InitFileLoad("RepayApp", RepayMasterEntity.REPAYAPPLYMASTERID, types, uploadFile);
            }
            else
            {
                client.GetRepayApplyMasterByIDAsync(repID);
                SMT.FBAnalysis.UI.Common.Utility.InitFileLoad("RepayApp", repID, types, uploadFile);
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
                txtCode.IsReadOnly = true;
                txtPayInfo.IsReadOnly = true;
                txttotal.IsReadOnly = true;
                dgvRepayDetailList.IsReadOnly = true;
                dgvRepayDetailList.IsEnabled = false;
            }
            else if (types == FormTypes.Edit)
            {
                //ComPayType.IsEnabled = false;
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
                GetPersonAccountData();
                hideCode();
            }
        }

        void client_GetRepayApplyMasterByIDCompleted(object sender, GetRepayApplyMasterByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        RepayMasterEntity = e.Result;
                        personclient.GetEmployeePostBriefByEmployeeIDAsync(RepayMasterEntity.OWNERID);
                        if(!string.IsNullOrEmpty(RepayMasterEntity.REPAYAPPLYCODE.Trim()))
                        {
                            txtCode.Text = RepayMasterEntity.REPAYAPPLYCODE;
                        }
                        else
                        {
                            hideCode();
                        }
                        txtRemark.Text = RepayMasterEntity.REMARK == null ? "" : RepayMasterEntity.REMARK;
                        txttotal.Text = RepayMasterEntity.TOTALMONEY.ToString();
                        txtPayInfo.Text = RepayMasterEntity.PAYMENTINFO==null?"":RepayMasterEntity.PAYMENTINFO;     //支付信息
                        strOwnerCompanyID = RepayMasterEntity.OWNERCOMPANYID;
                        strOwnerCompanyName = RepayMasterEntity.OWNERCOMPANYNAME;
                        strOwnerDepartmentID = RepayMasterEntity.OWNERDEPARTMENTID;
                        strOwnerDepartmentName = RepayMasterEntity.OWNERDEPARTMENTNAME;
                        strOwnerPostID = RepayMasterEntity.OWNERPOSTID;
                        strOwnerPostName = RepayMasterEntity.OWNERPOSTNAME;
                        strOwnerID = RepayMasterEntity.OWNERID;
                        strOwnerName = RepayMasterEntity.OWNERNAME;

                        client.GetRepayApplyDetailListByMasterIDAsync(RepayMasterEntity.REPAYAPPLYMASTERID);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), ex.ToString());
            }
        }

        void client_GetRepayApplyDetailListByMasterIDCompleted(object sender, GetRepayApplyDetailListByMasterIDCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                List<T_FB_REPAYAPPLYDETAIL> repayDetailList = e.Result.ToList();
                repDtlobj = e.Result;
                dgvRepayDetailList.ItemsSource = repayDetailList;
                dgvRepayDetailList.Loaded += new RoutedEventHandler(dgvRepayDetailList_Loaded);
            }
        }

        void dgvRepayDetailList_Loaded(object sender, RoutedEventArgs e)
        {
            if (types == FormTypes.Browse || types == FormTypes.Audit)
            {
                foreach (object obj in dgvRepayDetailList.ItemsSource)
                {
                    if (dgvRepayDetailList.Columns[2].GetCellContent(obj) != null)
                    {
                        TextBox txtMark = dgvRepayDetailList.Columns[2].GetCellContent(obj).FindName("txtRemark") as TextBox;
                        txtMark.IsReadOnly = true;
                    }
                    if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                    {
                        TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtMon") as TextBox;
                        tbMon.IsReadOnly = true;
                    }
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
                            if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == RepayMasterEntity.OWNERPOSTID) != null)
                            {
                                if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == RepayMasterEntity.OWNERPOSTID).FirstOrDefault() != null)
                                {
                                    if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == RepayMasterEntity.OWNERPOSTID).FirstOrDefault().T_HR_POSTDICTIONARY != null)
                                    {
                                        PostName = (Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == RepayMasterEntity.OWNERPOSTID).FirstOrDefault().T_HR_POSTDICTIONARY.POSTNAME;
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
                            if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == RepayMasterEntity.OWNERDEPARTMENTID) != null)
                            {
                                if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == RepayMasterEntity.OWNERDEPARTMENTID).FirstOrDefault() != null)
                                {
                                    if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == RepayMasterEntity.OWNERDEPARTMENTID).FirstOrDefault().T_HR_DEPARTMENTDICTIONARY != null)
                                    {
                                        DepartmentName = (Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == RepayMasterEntity.OWNERDEPARTMENTID).FirstOrDefault().T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                                    }
                                }
                            }
                        }
                    }

                    if (Application.Current.Resources["SYS_CompanyInfo"] != null)
                    {
                        if ((Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>) != null)
                        {
                            if ((Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(c => c.COMPANYID == RepayMasterEntity.OWNERCOMPANYID) != null)
                            {
                                if ((Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(c => c.COMPANYID == RepayMasterEntity.OWNERCOMPANYID).FirstOrDefault() != null)
                                {
                                    CompanyName = (Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(c => c.COMPANYID == RepayMasterEntity.OWNERCOMPANYID).FirstOrDefault().CNAME;
                                }
                            }
                        }
                    }

                    StrOwnerName = e.Result.EMPLOYEENAME;
                    if (employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == RepayMasterEntity.OWNERPOSTID) != null)
                    {
                        if (employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == RepayMasterEntity.OWNERPOSTID).FirstOrDefault() != null)
                        {
                            if (employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == RepayMasterEntity.OWNERPOSTID).FirstOrDefault().POSTLEVEL != null)
                            {
                                txPostLevel = employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == RepayMasterEntity.OWNERPOSTID).FirstOrDefault().POSTLEVEL.ToString();//获取出差人的岗位级别
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

        #region 保存个人还款申请记录

        private void RefreshFormType(FormTypes formtype, RefreshedTypes refreshedType)
        {
            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            entBrowser.FormType = formtype;
            types = FormTypes.Edit;
            FormTypesAction = FormTypes.Edit;
            RefreshUI(refreshedType);
            RefreshUI(RefreshedTypes.All);
        }

        private void IsEnabledByFormtype()
        {
            if (types == FormTypes.Browse || types == FormTypes.Audit)
            {
                foreach (object obj in dgvRepayDetailList.ItemsSource)
                {
                    if (dgvRepayDetailList.Columns[2].GetCellContent(obj) != null)
                    {
                        TextBox txtMark = dgvRepayDetailList.Columns[2].GetCellContent(obj).FindName("txtRemark") as TextBox;
                        txtMark.IsReadOnly = true;
                    }

                    if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                    {
                        TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtMon") as TextBox;
                        tbMon.IsReadOnly = true;
                    }
                }
            }
        }

        /// <summary>
        /// 检查明细
        /// </summary>
        /// <returns></returns>
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

            if (repDtlobj.Count == 0)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("REQUIRED", "REPAYDETAILDATA"));
                return false;
            }

            if (txttotal.Text=="" || decimal.Parse(txttotal.Text) <= 0 )
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "还款金额合计不能小于或等于0！");
                return false;
            }

            if (dgvRepayDetailList.ItemsSource != null)
            {
                foreach (object obj in dgvRepayDetailList.ItemsSource)
                {
                    T_FB_REPAYAPPLYDETAIL ent = obj as T_FB_REPAYAPPLYDETAIL;
                    if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                    {
                        decimal i = 0;
                        TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtMon") as TextBox;
                        if (!decimal.TryParse(tbMon.Text, out i))
                        {
                            Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "请输入正确的数值！");
                            return false;
                        }
                        if (decimal.Parse(tbMon.Text) < 0)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "还款金额不能小于0！");
                            return false;
                        }
                        if(decimal.Parse(tbMon.Text) > ent.BORROWMONEY)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "还款金额不能大于借款余额！");
                            return false;
                        }

                        if (dgvRepayDetailList.Columns[2].GetCellContent(obj) != null)
                        {
                            TextBox tbMark = dgvRepayDetailList.Columns[2].GetCellContent(obj).FindName("txtRemark") as TextBox;
                            if (string.IsNullOrEmpty(tbMark.Text) && decimal.Parse(tbMon.Text) > 0)
                            {
                                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "摘要不能为空");
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }


        private void SaveRepayApply(T_FB_REPAYAPPLYMASTER repayMaster)
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
                    InnerSaveRepayApply(repayMaster);
                }
            };
            RefreshUI(RefreshedTypes.ShowProgressBar);
            psc.GetEmployeeDetailByIDAsync(strOwnerID);
        }
        /// <summary>
        /// 保存还款单
        /// </summary>
        /// <param name="repayMaster"></param>
        private void InnerSaveRepayApply(T_FB_REPAYAPPLYMASTER repayMaster)
        {
            repayMaster.BRORROWEDMONEY = dBorrowTotal;
            repayMaster.CREATECOMPANYID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            repayMaster.CREATECOMPANYNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyName;
            repayMaster.CREATEDATE = DateTime.Now;
            repayMaster.CREATEDEPARTMENTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
            repayMaster.CREATEDEPARTMENTNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentName;
            repayMaster.CREATEPOSTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
            repayMaster.CREATEPOSTNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostName;
            repayMaster.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            repayMaster.CREATEUSERNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
            repayMaster.EDITSTATES = 0;
            repayMaster.OWNERCOMPANYID = strOwnerCompanyID;
            repayMaster.OWNERCOMPANYNAME = strOwnerCompanyName;
            repayMaster.OWNERDEPARTMENTID = strOwnerDepartmentID;
            repayMaster.OWNERDEPARTMENTNAME = strOwnerDepartmentName;
            repayMaster.OWNERID = strOwnerID;
            repayMaster.OWNERNAME = strOwnerName;
            repayMaster.OWNERPOSTID = strOwnerPostID;
            repayMaster.OWNERPOSTNAME = strOwnerPostName;
            repayMaster.PROJECTEDREPAYDATE = DateTime.Now;
            repayMaster.REMARK = txtRemark.Text;
            if (txtCode.Text.IndexOf('>') > 0)
            {
                repayMaster.REPAYAPPLYCODE = " ";
            }
            else
            {
                repayMaster.REPAYAPPLYCODE = txtCode.Text;
            }
            //repayMaster.REPAYTYPE = nRepayType;
            repayMaster.TOTALMONEY = decimal.Parse(txttotal.Text.ToString());
            repayMaster.UPDATEDATE = DateTime.Now;
            repayMaster.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            repayMaster.UPDATEUSERNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
            repayMaster.PAYMENTINFO = txtPayInfo.Text;

            repayDetailData.Clear();
            string strMsg = string.Empty;
            foreach (object obj in dgvRepayDetailList.ItemsSource)
            {
                T_FB_REPAYAPPLYDETAIL ent = obj as T_FB_REPAYAPPLYDETAIL;
                T_FB_REPAYAPPLYDETAIL repay = new T_FB_REPAYAPPLYDETAIL();
                repay.REPAYAPPLYDETAILID = System.Guid.NewGuid().ToString();
                repay.UPDATEDATE = DateTime.Now;
                repay.CREATEDATE = DateTime.Now;
                repay.T_FB_REPAYAPPLYMASTER = repayMaster;
                repay.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                repay.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                repay.BORROWMONEY = ent.BORROWMONEY;
                repay.CHARGETYPE = ent.CHARGETYPE;
                repay.REPAYTYPE = ent.REPAYTYPE;

                if (dgvRepayDetailList.Columns[2].GetCellContent(obj) != null)
                {
                    TextBox txtMark = dgvRepayDetailList.Columns[2].GetCellContent(obj).FindName("txtRemark") as TextBox;
                    repay.REMARK = txtMark.Text;
                }
                if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtMon") as TextBox;
                    repay.REPAYMONEY = decimal.Parse(string.IsNullOrEmpty(tbMon.Text.ToString()) == true ? "0" : tbMon.Text.ToString());
                }
                repayDetailData.Add(repay);
            }

            if (!string.IsNullOrWhiteSpace(strMsg))
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), strMsg);
                return;
            }

            RefreshUI(RefreshedTypes.ShowProgressBar);
            if (types == FormTypes.New)
            {
                string strCode = string.Empty, strResMsg = string.Empty;
                client.AddRepayApplyMasterAndDetailAsync(repayMaster, repayDetailData);
            }
            else if (types == FormTypes.Edit)
            {
                if (needsubmit == false)
                {
                    client.UptRepayApplyMasterAndDetailAsync("Edit", repayMaster, repayDetailData, strMsg, "Edit");
                }
                else
                {
                    RepayMasterEntity = repayMaster;
                    client.GetRepayOrderCodeAsync(repayMaster);
                }
            }
            else if (types == FormTypes.Resubmit)
            {
                client.UptRepayApplyMasterAndDetailAsync("ReSubmit", repayMaster, repayDetailData, strMsg, "ReSubmit");
            }
        }

        //获得单据号后的完成事件，在此事件中再调用方法把单据号存入  2012.2.21
        void client_GetRepayOrderCodeCompleted(object sender, GetRepayOrderCodeCompletedEventArgs e)
        {
            string strMsg = string.Empty;
            if (e.Error == null)
            {
                if (!string.IsNullOrEmpty(e.Result)) //获取单据号  zl
                {
                    if(string.IsNullOrWhiteSpace(RepayMasterEntity.REPAYAPPLYCODE))
                    {
                        RepayMasterEntity.REPAYAPPLYCODE = e.Result;
                    }
                    txtCode.Text = RepayMasterEntity.REPAYAPPLYCODE;
                    visibCode();
                    client.UptRepayApplyMasterAndDetailAsync("Edit", RepayMasterEntity, repayDetailData, strMsg, "Edit");
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

        //增加还款数据后的完成事件
        void client_AddRepayApplyMasterAndDetailCompleted(object sender, AddRepayApplyMasterAndDetailCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error == null)
            {
                if(e.Result)
                {
                    isAdd = false;
                    Utility.ShowMessageBox("ADD", isSubmitFlow, true);
                    EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                    entBrowser.FormType = FormTypes.Edit;
                    types = FormTypes.Edit;
                    FormTypesAction = FormTypes.Edit;
                    RefreshUI(RefreshedTypes.AuditInfo);
                    RefreshUI(RefreshedTypes.All);
                    if (isSubmitFlow)
                    {
                        saveType = RefreshedTypes.CloseAndReloadData;
                    }
                    //ctrFile.FormID = RepayMasterEntity.REPAYAPPLYMASTERID;
                    //ctrFile.Save();

                    RefreshUI(saveType);

                    if (needsubmit == true)   //如果点击提交按钮则调用引擎提交审核
                    {
                        EntityBrowser entBrowser1 = this.FindParentByType<EntityBrowser>();
                        entBrowser1.ManualSubmit();
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
                Utility.ShowMessageBox("ADD", isSubmitFlow, false);
            }
        }

        //修改还款数据后的完成事件
        void client_UptRepayApplyMasterAndDetailCompleted(object sender, UptRepayApplyMasterAndDetailCompletedEventArgs e)
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
                    //ctrFile.FormID = RepayMasterEntity.REPAYAPPLYMASTERID;
                    //ctrFile.Save();
                    RefreshFormType(FormTypes.Edit, RefreshedTypes.AuditInfo);  
                }
                if (types == FormTypes.Resubmit)    //add zl 2012.2.22
                {
                    if (needsubmit == false)
                    {
                        Utility.ShowMessageBox("UPDATE", isSubmitFlow, true);
                    }
                    //ctrFile.FormID = RepayMasterEntity.REPAYAPPLYMASTERID;
                    //ctrFile.Save();
                    RefreshFormType(FormTypes.Edit, RefreshedTypes.AuditInfo);  
                }
                if (needsubmit == true)   //如果点击提交按钮则调用引擎提交审核
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

        #endregion

        #region 申请人
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
                    strOwnerCompanyName = corpName;
                    strOwnerDepartmentID = deptid;
                    strOwnerDepartmentName = deptName;
                    strOwnerPostID = postid;
                    strOwnerPostName = postName;
                    strOwnerID = userInfo.ObjectID;
                    strOwnerName = userInfo.ObjectName;
                    //tmpSendDocT.OWNERCOMPANYID = corpid;
                    //tmpSendDocT.OWNERDEPARTMENTID = deptid;
                    //tmpSendDocT.OWNERID = userInfo.ObjectID;
                    //tmpSendDocT.OWNERPOSTID = postid;
                    //tmpSendDocT.OWNERNAME = userInfo.ObjectName;
                    txPostLevel = emp.T_HR_EMPLOYEEPOST.Where(t => t.T_HR_POST.POSTID == postid).FirstOrDefault().POSTLEVEL.ToString();
                    ToolTipService.SetToolTip(txtOwnerName, StrEmployee);

                    //SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj companyInfo = ent.FirstOrDefault();
                    //txtAddUser.Text = companyInfo.ObjectName;
                    //StrAddUserID = userInfo.ObjectID;// companyInfo.ObjectID;
                    GetPersonAccountData();
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


        public string GetTitle()
        {
            //string Strreturn = "";
            //switch (FormTypesAction)
            //{
            //    case FormTypes.New:
            //        Strreturn = Utility.GetResourceStr("ADDTITLE", "PEOPLEREPAYAPPLY");
            //        break;
            //    case FormTypes.Edit:
            //        Strreturn = Utility.GetResourceStr("EDITTITLE", "PEOPLEREPAYAPPLY");
            //        break;
            //    case FormTypes.Audit:
            //        Strreturn = Utility.GetResourceStr("AUDITTITLE", "PEOPLEREPAYAPPLY");
            //        break;
            //    case FormTypes.Browse:
            //        Strreturn = Utility.GetResourceStr("VIEWTITLE", "PEOPLEREPAYAPPLY");
            //        break;
            //    case FormTypes.Resubmit:
            //        Strreturn = Utility.GetResourceStr("EDITTITLE", "PEOPLEREPAYAPPLY");
            //        break;
            //}
            return Utility.GetResourceStr("PEOPLEREPAYAPPLY");
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
                        client.DelRepayApplyMasterAndDetailAsync(new ObservableCollection<string>() { RepayMasterEntity.REPAYAPPLYMASTERID });
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
                    //SMT.FBAnalysis.UI.Common.Utility.CreateFormFromEngine("143f3aa5-7c52-48d5-abfb-4a9bef57063c", "SMT.FBAnalysis.UI.Form.RepayApplyForm", "Edit");
                    SaveRepayApply(RepayMasterEntity);
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

            if (FormTypesAction != FormTypes.Browse && FormTypesAction != FormTypes.Audit && FormTypesAction != FormTypes.Resubmit)
            {

                if (FormTypesAction != FormTypes.New && RepayMasterEntity.CHECKSTATES == 0)
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
        public void RefreshUI(RefreshedTypes type)
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

        private string GetXmlString(string StrSorce, T_FB_REPAYAPPLYMASTER Info)
        {
            string goouttomeet = string.Empty;
            string privateaffair = string.Empty;
            string companycar = string.Empty;
            string isagent = string.Empty;

            SMT.SaaS.MobileXml.MobileXml mx = new MobileXml();
            SMT.SaaS.MobileXml.AutoDictionary ad = new AutoDictionary();
            List<SMT.SaaS.MobileXml.AutoDictionary> AutoList = new List<SMT.SaaS.MobileXml.AutoDictionary>();

            string StrPayType = "";
            string StrEditState = "";
            switch (Info.REPAYTYPE.ToString())
            {
                case "1":
                    StrPayType = "现金还普通借款";
                    break;
                case "2":
                    StrPayType = "现金还备用金借款";
                    break;
                case "3":
                    StrPayType = "现金还专项借款";
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
            AutoList.Add(basedata("T_FB_REPAYAPPLYMASTER", "POSTLEVEL", txPostLevel, null));//POSTLEVEL
            AutoList.Add(basedata("T_FB_REPAYAPPLYMASTER", "REPAYTYPE", Info.REPAYTYPE.ToString(), StrPayType));//相关单据类型
            AutoList.Add(basedata("T_FB_REPAYAPPLYMASTER", "EDITSTATES", Info.EDITSTATES.ToString(), StrEditState));//编辑状态
            if (Info.OWNERID != null && !string.IsNullOrEmpty(strOwnerName))
            {
                AutoList.Add(basedata("T_FB_REPAYAPPLYMASTER", "OWNERID", Info.OWNERID, strOwnerName + "-" + strOwnerPostName + "-" + strOwnerDepartmentName + "-" + strOwnerCompanyName));
            }
            if (Info.OWNERCOMPANYID != null && !string.IsNullOrEmpty(strOwnerCompanyName))
            {
                AutoList.Add(basedata("T_FB_REPAYAPPLYMASTER", "OWNERCOMPANYID", Info.OWNERCOMPANYID, strOwnerCompanyName));
            }
            if (Info.OWNERDEPARTMENTID != null && !string.IsNullOrEmpty(strOwnerDepartmentName))
            {
                AutoList.Add(basedata("T_FB_REPAYAPPLYMASTER", "OWNERDEPARTMENTID", Info.OWNERDEPARTMENTID, strOwnerDepartmentName));
            }
            if (Info.OWNERPOSTID != null && !string.IsNullOrEmpty(strOwnerPostName))
            {
                AutoList.Add(basedata("T_FB_REPAYAPPLYMASTER", "OWNERPOSTID", Info.OWNERPOSTID, strOwnerPostName));
            }
            //AutoList.Add(basedata("T_FB_REPAYAPPLYMASTER", "BORROWAPPLYMASTERID", Info.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERCODE, Info.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERCODE));

            ObservableCollection<T_FB_REPAYAPPLYDETAIL> objR;
            if (repayDetailData != null && repayDetailData.Count > 0)
            {
                objR = repayDetailData;
            }
            else
            {
                objR = repayDetailData;
            }
            foreach (T_FB_REPAYAPPLYDETAIL objDetail in objR)
            {
                if (objDetail.T_FB_SUBJECT != null)
                {
                    AutoList.Add(basedataForChild("T_FB_REPAYAPPLYDETAIL", "SUBJECTID", objDetail.T_FB_SUBJECT.SUBJECTID, objDetail.T_FB_SUBJECT.SUBJECTID, objDetail.REPAYAPPLYDETAILID));
                    AutoList.Add(basedataForChild("T_FB_REPAYAPPLYDETAIL", "SUBJECTCODE", objDetail.T_FB_SUBJECT.SUBJECTCODE, objDetail.T_FB_SUBJECT.SUBJECTCODE, objDetail.REPAYAPPLYDETAILID));
                    AutoList.Add(basedataForChild("T_FB_REPAYAPPLYDETAIL", "SUBJECTNAME", objDetail.T_FB_SUBJECT.SUBJECTNAME, objDetail.T_FB_SUBJECT.SUBJECTNAME, objDetail.REPAYAPPLYDETAILID));
                }
                if (objDetail.T_FB_BORROWAPPLYDETAIL != null)
                {
                    AutoList.Add(basedataForChild("T_FB_REPAYAPPLYDETAIL", "BORROWAPPLYDETAILID", objDetail.T_FB_BORROWAPPLYDETAIL.BORROWAPPLYDETAILID, objDetail.T_FB_BORROWAPPLYDETAIL.BORROWAPPLYDETAILID, objDetail.REPAYAPPLYDETAILID));
                    //AutoList.Add(basedataForChild("T_FB_REPAYAPPLYDETAIL", "UNREPAYMONEY", objDetail.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY.ToString(), objDetail.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY.ToString(), objDetail.REPAYAPPLYDETAILID));
                    AutoList.Add(basedataForChild("T_FB_REPAYAPPLYDETAIL", "UNREPAYMONEY", (objDetail.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY - objDetail.REPAYMONEY).ToString(), (objDetail.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY - objDetail.REPAYMONEY).ToString(), objDetail.REPAYAPPLYDETAILID));
                }
                if (objDetail.T_FB_REPAYAPPLYMASTER != null)
                {
                    AutoList.Add(basedataForChild("T_FB_REPAYAPPLYDETAIL", "REPAYAPPLYMASTERID", objDetail.T_FB_REPAYAPPLYMASTER.REPAYAPPLYMASTERID, objDetail.T_FB_REPAYAPPLYMASTER.REPAYAPPLYMASTERID, objDetail.REPAYAPPLYDETAILID));
                }
                if (objDetail.CHARGETYPE != null)
                {
                    AutoList.Add(basedataForChild("T_FB_REPAYAPPLYDETAIL", "CHARGETYPE", objDetail.CHARGETYPE.ToString(), objDetail.CHARGETYPE.ToString() == "1" ? "个人预算费用" : "公共预算费用", objDetail.REPAYAPPLYDETAILID));
                }
                switch (objDetail.REPAYTYPE.ToString())
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
                if (objDetail.REPAYTYPE != null)
                {
                    AutoList.Add(basedataForChild("T_FB_REPAYAPPLYDETAIL", "REPAYTYPE", objDetail.REPAYTYPE.ToString(), StrPayType, objDetail.REPAYAPPLYDETAILID));
                }

            }
            string a = mx.TableToXml(Info, objR, StrSorce, AutoList);
            return a;

        }

        public void SetFlowRecordEntity(SMT.SaaS.FrameworkUI.AuditControl.Flow_FlowRecord_T entity)
        {
            string strXmlObjectSource = string.Empty;
            //XML
            entity.SystemCode = "FB";
            if (!string.IsNullOrEmpty(entity.BusinessObjectDefineXML)) //返回的XML定义不为空时对业务对象进行填充
            {
                strXmlObjectSource = this.GetXmlString(entity.BusinessObjectDefineXML, RepayMasterEntity);
            }
            //END
            //strXmlObjectSource = Utility.ObjListToXml<T_FB_REPAYAPPLYMASTER>(RepayMasterEntity, "FB");
            Dictionary<string, string> paraIDs = new Dictionary<string, string>();
            if (RepayMasterEntity != null)
            {
                paraIDs.Add("CreateUserID", RepayMasterEntity.OWNERID);
                paraIDs.Add("CreatePostID", RepayMasterEntity.OWNERPOSTID);
                paraIDs.Add("CreateDepartmentID", RepayMasterEntity.OWNERDEPARTMENTID);
                paraIDs.Add("CreateCompanyID", RepayMasterEntity.OWNERCOMPANYID);
            }

            if (RepayMasterEntity.CHECKSTATES == ((decimal)CheckStates.UnSubmit) || RepayMasterEntity.CHECKSTATES == ((decimal)CheckStates.UnApproved))
            {
                SMT.SaaS.FrameworkUI.Common.Utility.SetAuditEntity(entity, "T_FB_REPAYAPPLYMASTER", RepayMasterEntity.REPAYAPPLYMASTERID, strXmlObjectSource, paraIDs);
            }
            else
            {
                Utility.SetAuditEntity(entity, "T_FB_REPAYAPPLYMASTER", RepayMasterEntity.REPAYAPPLYMASTERID, strXmlObjectSource);
            }
        }

        public void OnSubmitCompleted(SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult args)
        {
            CheckStates state = CheckStates.UnSubmit;
            string strActionType = "Audit";
            SMT.FBAnalysis.UI.Common.Utility.InitFileLoad(FormTypes.Audit, uploadFile, RepayMasterEntity.REPAYAPPLYMASTERID, false);
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
            if (state == CheckStates.Approving)
            {
                client.GetRepayApplyMasterByIDAsync(RepayMasterEntity.REPAYAPPLYMASTERID);
            }
            //add end

            if (types == FormTypes.Edit || types == FormTypes.Resubmit)
            {
                Utility.ShowMessageBox("SUBMIT", isSubmitFlow, true);//提交审核成功
            }
            else if (types == FormTypes.Audit)
            {
                Utility.ShowMessageBox("AUDIT", isSubmitFlow, true);//审核成功
            }

            RefreshUI(RefreshedTypes.HideProgressBar);

            RepayMasterEntity.CHECKSTATES = Convert.ToInt32(state);
            RefreshUI(RefreshedTypes.AuditInfo);
            RefreshUI(RefreshedTypes.All);
        }

        public string GetAuditState()
        {
            string state = "-1";
            if (RepayMasterEntity != null)
                state = RepayMasterEntity.CHECKSTATES.ToString();

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
            //        SMT.SaaS.FrameworkUI.Common.Utility.HiddenGridRow(this.LayoutRoot, 5);
            //    }
            //}
        }
        #endregion
        
        #region 页面事件
        //获取相应人员的借款金额
        public void GetPersonAccountData()
        {
            string filter = "";    //查询过滤条件
            ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();   //参数值
            if(!string.IsNullOrEmpty(strOwnerCompanyID))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "@" + paras.Count().ToString() + ".Contains(OWNERCOMPANYID) "; 
                paras.Add(strOwnerCompanyID);
            }
            if(!string.IsNullOrEmpty(strOwnerID))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "@" + paras.Count().ToString() + ".Contains(OWNERID) "; 
                paras.Add(strOwnerID);
            }
            dgvRepayDetailList.ItemsSource = null;
            txttotal.Text = "";
            client.GetPersonAccountListByMultSearchAsync(filter, paras, "PERSONACCOUNTID");
        }

        void client_GetPersonAccountListByMultSearchCompleted(object sender, GetPersonAccountListByMultSearchCompletedEventArgs e)
        {
            T_FB_PERSONACCOUNT PerEntity=new T_FB_PERSONACCOUNT();
            repDtlobj.Clear();
            try
            {
                if (e.Error == null)
                {
                    if(e.Result!=null && e.Result.Count>0)
                    {
                        PerEntity = e.Result.FirstOrDefault();
                        if(PerEntity.SIMPLEBORROWMONEY>0)
                        {
                            T_FB_REPAYAPPLYDETAIL repay=new T_FB_REPAYAPPLYDETAIL();
                            repay.BORROWMONEY = PerEntity.SIMPLEBORROWMONEY.Value;
                            repay.CREATEDATE = DateTime.Now;
                            repay.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            repay.REMARK = "";
                            repay.REPAYAPPLYDETAILID = System.Guid.NewGuid().ToString();
                            repay.REPAYMONEY = 0;
                            repay.REPAYTYPE = 1;
                            repay.UPDATEDATE = DateTime.Now;
                            repay.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            repDtlobj.Add(repay);
                        }
                        if(PerEntity.BACKUPBORROWMONEY>0)
                        {
                            T_FB_REPAYAPPLYDETAIL repay = new T_FB_REPAYAPPLYDETAIL();
                            repay.BORROWMONEY = PerEntity.BACKUPBORROWMONEY.Value;
                            repay.CREATEDATE = DateTime.Now;
                            repay.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            repay.REMARK = "";
                            repay.REPAYAPPLYDETAILID = System.Guid.NewGuid().ToString();
                            repay.REPAYMONEY = 0;
                            repay.REPAYTYPE = 2;
                            repay.UPDATEDATE = DateTime.Now;
                            repay.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            repDtlobj.Add(repay);
                        }
                        if(PerEntity.SPECIALBORROWMONEY>0)
                        {
                            T_FB_REPAYAPPLYDETAIL repay = new T_FB_REPAYAPPLYDETAIL();
                            repay.BORROWMONEY = PerEntity.SPECIALBORROWMONEY.Value;
                            repay.CREATEDATE = DateTime.Now;
                            repay.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            repay.REMARK = "";
                            repay.REPAYAPPLYDETAILID = System.Guid.NewGuid().ToString();
                            repay.REPAYMONEY = 0;
                            repay.REPAYTYPE = 3;
                            repay.UPDATEDATE = DateTime.Now;
                            repay.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            repDtlobj.Add(repay);
                        }
                    }
                    dgvRepayDetailList.ItemsSource = repDtlobj;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), ex.ToString());
            }
        }

        private void txtMon_KeyUp(object sender, KeyEventArgs e)
        {
            decimal dMoney = 0;
            foreach (object obj in dgvRepayDetailList.ItemsSource)
            {
                if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtMon") as TextBox;
                    decimal dDetailMon = 0;
                    T_FB_REPAYAPPLYDETAIL ent = obj as T_FB_REPAYAPPLYDETAIL;
                    decimal.TryParse(tbMon.Text, out dDetailMon);
                    ent.REPAYMONEY = dDetailMon;
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
            CalculateRep();
        }

        private void CalculateRep()
        {
            decimal dMoney = 0;
            foreach (object obj in dgvRepayDetailList.ItemsSource)
            {
                if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtMon") as TextBox;
                    decimal dDetailMon = 0;
                    T_FB_REPAYAPPLYDETAIL ent = obj as T_FB_REPAYAPPLYDETAIL;
                    decimal.TryParse(tbMon.Text, out dDetailMon);
                    ent.REPAYMONEY = dDetailMon;
                    dMoney = dMoney + dDetailMon;
                }
            }

            txttotal.Text = dMoney.ToString();
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
        #endregion

        void client_DelRepayApplyMasterAndDetailCompleted(object sender, DelRepayApplyMasterAndDetailCompletedEventArgs e)
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
