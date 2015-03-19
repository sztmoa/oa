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
using SMT.SaaS.FrameworkUI.Validator;
using SMT.Saas.Tools.PersonnelWS;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.MobileXml;
using SMT.SAAS.ClientServices;
using SMT.SAAS.ClientUtility;
using System.Text.RegularExpressions;
using System.Windows.Browser;
using SMT.SaaS.FrameworkUI.WaterMarkTextBox;
using System.Xml.Linq;
//using SMT.SaaS.FrameworkUI.FileUpload;

namespace SMT.FBAnalysis.UI.Form
{
    public partial class ChargeApplyForm : BaseForm, IClient, IEntityEditor, IAudit
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
        string smtonlineID = "bac05c76-0f5b-40ae-b73b-8be541ed35ed";//科技有限公司ID，实际上不想这样做

        private FormTypes FormTypesAction;//操作定义 增加、修改、审核、查看、重新提交
        private RefreshedTypes saveType = RefreshedTypes.All;//保存的类型默认为ALL
        public SubjectApp_sel frmU;
        public BorrowApp_sel frmB;
        public event refreshGridView ReloadDataEvent;
        private decimal dBorrowTotal = 0;
        private FormTypes types;
        private string chaID = string.Empty;
        private int nChargeType = 1;
        private int nRepType = 1;
        bool isAdd = true;
        private bool isSubmitFlow = false;
        private string txPostLevel = string.Empty;
        public bool needsubmit = false;

        private string PayMySelf = "汇本人账户, ";
        private string PayFromA = "在线支付, ";
        private string PayFromB = "诺亚软杰支付, ";

        PersonnelServiceClient personclient = new PersonnelServiceClient();

        ObservableCollection<T_FB_BORROWAPPLYDETAIL> borrowDetailData = new ObservableCollection<T_FB_BORROWAPPLYDETAIL>();
        T_FB_CHARGEAPPLYMASTER ChargeMasterEntity = new T_FB_CHARGEAPPLYMASTER();
        ObservableCollection<T_FB_CHARGEAPPLYDETAIL> chargeDtlList = new ObservableCollection<T_FB_CHARGEAPPLYDETAIL>();
        ObservableCollection<T_FB_CHARGEAPPLYDETAIL> chaDtlobj = new ObservableCollection<T_FB_CHARGEAPPLYDETAIL>();
        ObservableCollection<T_FB_BUDGETACCOUNT> budgetList = new ObservableCollection<T_FB_BUDGETACCOUNT>();
        ObservableCollection<T_FB_CHARGEAPPLYREPAYDETAIL> chaRepDtlobj = new ObservableCollection<T_FB_CHARGEAPPLYREPAYDETAIL>();
        ObservableCollection<T_FB_CHARGEAPPLYREPAYDETAIL> chaRepayDetailData = new ObservableCollection<T_FB_CHARGEAPPLYREPAYDETAIL>();
        T_FB_EXTENSIONALORDER exten = new T_FB_EXTENSIONALORDER();
        DailyManagementServicesClient client = new DailyManagementServicesClient();

        DictionaryManager DictManager = new DictionaryManager();
        List<string> ListDict = new List<string>(); //字典列表
        //是否为湖南航信版本
        bool isHuNanHangXin = false;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="StrChargeApplyID">个人费用申请ID,添加为空</param>
        /// <param name="ActionType">操作的动作 添加、修改、查看、审核、重新提交</param>
        public ChargeApplyForm(FormTypes ActionType, string StrChargeApplyID)
        {
            //获取版本信息
            client.GetVersionIsHuNanAsync();
            CheckConverter();
            InitializeComponent();
            chaID = StrChargeApplyID;
            this.types = ActionType;
            FormTypesAction = ActionType;
            this.Loaded += (sender, args) =>
            {
                ChargeApplyForm_Loaded(sender, args);

                WcfRegister();//Wcf事件注册

            };

            //ctrFile.SystemName = "FB";
            //ctrFile.ModelName = "ChargeApp";
            //ctrFile.EntityEditor = this;
            //if (FormTypesAction == FormTypes.Audit || FormTypesAction == FormTypes.Browse)
            //{
            //    ctrFile.FileState = SMT.SaaS.FrameworkUI.FileUpload.Constants.FileStates.FileBrowse;
            //    ctrFile.InitBtn(Visibility.Collapsed, Visibility.Collapsed);
            //    ctrFile.Load_fileData(chaID, this);
            //}
            //else
            //{
            //    ctrFile.InitBtn(Visibility.Visible, Visibility.Collapsed);
            //    if (!string.IsNullOrEmpty(chaID))
            //    {
            //        ctrFile.Load_fileData(chaID, this);
            //    }
            //}
        }

        /// <summary>
        ///构造函数 新增时使用（新建单据） 2011-10-11 王继华
        /// </summary>      
        public ChargeApplyForm()
        {
            //获取版本信息
            client.GetVersionIsHuNanAsync();
            CheckConverter();
            InitializeComponent();
            chaID = string.Empty;
            this.types = FormTypes.New; ;
            FormTypesAction = FormTypes.New; ;
            this.Loaded += (sender, args) =>
            {
                ChargeApplyForm_Loaded(sender, args);

                WcfRegister();//Wcf事件注册

            };

            //ctrFile.SystemName = "FB";
            //ctrFile.ModelName = "ChargeApp";
            //ctrFile.EntityEditor = this;
            //ctrFile.InitBtn(Visibility.Visible, Visibility.Collapsed);
            //if (!string.IsNullOrEmpty(chaID))
            //{
            //    ctrFile.Load_fileData(chaID, this);
            //}
        }

        void BtnSaveSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (!Check4Submit())
            {
                return;
            }

            if (ChargeMasterEntity.OWNERID != SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("OPERATINGWITHOUTAUTHORITY"));
                return;
            }

            //EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            //entBrowser.BtnSaveSubmit.IsEnabled = false;
            needsubmit = true;
            SaveChargeApply(ChargeMasterEntity);
        }

        private void RegisterTooolBar()
        {
            throw new NotImplementedException();
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
            if (Application.Current.Resources["SubjectUsableMoneyFormat"] == null)
            {
                Application.Current.Resources.Add("SubjectUsableMoneyFormat", new SMT.FBAnalysis.UI.SubjectUsableMoneyFormat());
            }
        }

        private void WcfRegister()
        {
            client.AddChargeApplyMasterAndDetailCompleted += new EventHandler<AddChargeApplyMasterAndDetailCompletedEventArgs>(client_AddChargeApplyMasterAndDetailCompleted);
            client.GetChargeApplyMasterByIDCompleted += new EventHandler<GetChargeApplyMasterByIDCompletedEventArgs>(client_GetChargeApplyMasterByIDCompleted);

            client.GetChargeApplyDetailByMasterIDCompleted += new EventHandler<GetChargeApplyDetailByMasterIDCompletedEventArgs>(client_GetChargeApplyDetailByMasterIDCompleted);
            client.UptChargeApplyMasterAndDetailCompleted += new EventHandler<UptChargeApplyMasterAndDetailCompletedEventArgs>(client_UptChargeApplyMasterAndDetailCompleted);
            client.UptChargeApplyCheckStateCompleted += new EventHandler<UptChargeApplyCheckStateCompletedEventArgs>(client_UptChargeApplyCheckStateCompleted);
            client.GetExtensionalOrderByIDCompleted += new EventHandler<GetExtensionalOrderByIDCompletedEventArgs>(client_GetExtensionalOrderByIDCompleted);
            personclient.GetEmployeePostBriefByEmployeeIDCompleted += new EventHandler<GetEmployeePostBriefByEmployeeIDCompletedEventArgs>(personClient_GetEmployeePostBriefByEmployeeIDCompleted);
            //personclient.GetEmployeeByIDCompleted += new EventHandler<GetEmployeeByIDCompletedEventArgs>(personclient_GetEmployeeByIDCompleted);
            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            entBrowser.BtnSaveSubmit.Click -= new RoutedEventHandler(entBrowser.btnSubmit_Click);
            entBrowser.BtnSaveSubmit.Click += new RoutedEventHandler(BtnSaveSubmit_Click);

            client.GetPersonAccountListByMultSearchCompleted += new EventHandler<GetPersonAccountListByMultSearchCompletedEventArgs>(client_GetPersonAccountListByMultSearchCompleted);
            client.GetChargeApplyRepayDetailByMasterIDCompleted += new EventHandler<GetChargeApplyRepayDetailByMasterIDCompletedEventArgs>(client_GetChargeApplyRepayDetailByMasterIDCompleted);   
            client.DelChargeApplyMasterAndDetailCompleted += new EventHandler<DelChargeApplyMasterAndDetailCompletedEventArgs>(client_DelChargeApplyMasterAndDetailCompleted);
            client.GetVersionIsHuNanCompleted += new EventHandler<GetVersionIsHuNanCompletedEventArgs>(client_GetVersionIsHuNanCompleted);
        }

        void client_GetVersionIsHuNanCompleted(object sender, GetVersionIsHuNanCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                isHuNanHangXin = e.Result;
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
                this.Loaded += new RoutedEventHandler(ChargeApplyForm_Loaded);
            }
        }

        void ChargeApplyForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (FormTypesAction != FormTypes.New)
            {
                btnLookUpOwner.IsEnabled = false;//by xiedx  修改不能选其他人
                if (FormTypesAction == FormTypes.Browse || FormTypesAction == FormTypes.Audit)
                {
                    txtCode.IsReadOnly = false;
                    txtRemark.IsReadOnly = false;
                    payInfoB.IsReadOnly = false;
                    txtOwnerID.IsReadOnly = false;
                    spDetailAction.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else
            {
                this.LayoutRoot.DataContext = ChargeMasterEntity;
            }

            InitData();


            // 那坑爹的WaterMarkTextBox有bug,又不能修改,只能这么坑爹的写法了
            //wtOther.Loaded += new RoutedEventHandler(waterRamrTextBoxXXX_Loaded);
            //txtRemark.Loaded += new RoutedEventHandler(waterRamrTextBoxXXX_Loaded);
            //txtPayInfo.Loaded += new RoutedEventHandler(waterRamrTextBoxXXX_Loaded);
            
            tbWaterMark.Loaded += (oo, ee) => { waterRamrTextBoxXXX_Loaded(payInfoB, null);};
          
            tbRemark.Loaded += (oo, ee) => { waterRamrTextBoxXXX_Loaded(txtRemark, null); };

        }
        void waterRamrTextBoxXXX_Loaded(object sender, RoutedEventArgs e)
        {
            WaterMarkTextBox ww = (WaterMarkTextBox)sender;
            var a = ww.Text;
            ww.Text = " ";
            ww.Text = a;
        }

        #endregion

        #region 初始化
        private void InitData()
        {
            if (types == FormTypes.New)
            {
                ChargeMasterEntity = new T_FB_CHARGEAPPLYMASTER();
                ChargeMasterEntity.CHARGEAPPLYMASTERID = System.Guid.NewGuid().ToString();
                ChargeMasterEntity.CHECKSTATES = 0;
                ChargeMasterEntity.PAYTYPE = 1;
                ChargeMasterEntity.PAYTARGET = 0;
                ChargeMasterEntity.PAYMENTINFO = this.PayMySelf;
                txPostLevel = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostLevel.ToString();
                SMT.FBAnalysis.UI.Common.Utility.InitFileLoad("ChargeApp", ChargeMasterEntity.CHARGEAPPLYMASTERID, types, uploadFile);
                
                SetPayInfo();
            }
            else
            {
                var id = chaID;
                string userState = null;
                if (types == FormTypes.Edit || types == FormTypes.Resubmit)
                {
                    id += ":Edit";
                    userState = "Edit";
                }
                client.GetChargeApplyMasterByIDAsync(id, userState);
                SMT.FBAnalysis.UI.Common.Utility.InitFileLoad("ChargeApp", chaID, types, uploadFile);
                RefreshUI(RefreshedTypes.ShowProgressBar);
            }

            SetForms();
        }

        private void SetForms()
        {
            if (types == FormTypes.Audit || types == FormTypes.Browse)
            {
                txtOwnerID.IsReadOnly = true;
                txtRemark.IsReadOnly = true;
                dgvChargeDetailList.IsEnabled = false;
                btnLookUpOwner.IsEnabled = false;
                AddSub.IsEnabled = false;
                txtCode.IsReadOnly = true;             
                txtTolRepayMon.IsReadOnly = true;
                rabPerCharge.IsEnabled = false;
                rabPayBor.IsEnabled = false;
                rabPayAdvance.IsEnabled = false;
                rabPayCus.IsEnabled = false;
                rabPayOther.IsEnabled = false;
                dgvRepayDetailList.IsEnabled = false;
                rbPayMySelft.IsEnabled = false;
                rbPayOther.IsEnabled = false;
                rbPayFromA.IsEnabled = false;
                rbPayFromB.IsEnabled = false;
                payInfoB.IsReadOnly = true;
                //dgvRepayDetailList.Visibility = Visibility.Collapsed;
                spDetailAction.Visibility = System.Windows.Visibility.Collapsed;
                btnLookUpItem.Visibility = System.Windows.Visibility.Collapsed;
                txtItemCode.Visibility = System.Windows.Visibility.Collapsed;
                hypItemCode.Visibility = System.Windows.Visibility.Visible;

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
                dgvRepayDetailList.Visibility = Visibility.Collapsed;

                hideCode();
            }
            setSmtonline();
        }

        void client_GetChargeApplyMasterByIDCompleted(object sender, GetChargeApplyMasterByIDCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (e.Result != null)
                    {
                        ChargeMasterEntity = e.Result;
                        personclient.GetEmployeePostBriefByEmployeeIDAsync(ChargeMasterEntity.OWNERID);
                        if (!string.IsNullOrEmpty(ChargeMasterEntity.CHARGEAPPLYMASTERCODE.Trim()))
                        {
                            txtCode.Text = ChargeMasterEntity.CHARGEAPPLYMASTERCODE;
                        }
                        else
                        {
                            hideCode();
                        }


                        if (!string.IsNullOrEmpty(ChargeMasterEntity.BANK))
                        {
                            txtItemID.Text = ChargeMasterEntity.BANK;
                            txtItemCode.Text = ChargeMasterEntity.BANKACCOUT;
                            hypItemCode.Content = ChargeMasterEntity.BANKACCOUT;
                        }
                        else if (types == FormTypes.Audit || types == FormTypes.Browse)
                        {
                            itemA.Visibility = System.Windows.Visibility.Collapsed;
                            itemB.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        //txtPayInfo.Text = ChargeMasterEntity.PAYMENTINFO == null ? "" : ChargeMasterEntity.PAYMENTINFO;
                        txtRemark.Text = ChargeMasterEntity.REMARK == null ? "" : ChargeMasterEntity.REMARK;
                        txttotal.Text = ChargeMasterEntity.TOTALMONEY.ToString();
                        strOwnerCompanyID = ChargeMasterEntity.OWNERCOMPANYID;
                        strOwnerCompanyName = ChargeMasterEntity.OWNERCOMPANYNAME;
                        strOwnerDepartmentID = ChargeMasterEntity.OWNERDEPARTMENTID;
                        strOwnerDepartmentName = ChargeMasterEntity.OWNERDEPARTMENTNAME;
                        strOwnerPostID = ChargeMasterEntity.OWNERPOSTID;
                        strOwnerPostName = ChargeMasterEntity.OWNERPOSTNAME;
                        strOwnerID = ChargeMasterEntity.OWNERID;
                        strOwnerName = ChargeMasterEntity.OWNERNAME;
                        dgvRepayDetailList.Visibility = Visibility.Collapsed;
                        dgvRepayDetailList.IsEnabled = false;

                        SetPayInfo();
                        if (ChargeMasterEntity.PAYTYPE == 2)
                        {
                            txtTolRepayMon.Text = ChargeMasterEntity.REPAYMENT.ToString();
                            client.GetChargeApplyRepayDetailByMasterIDAsync(ChargeMasterEntity.CHARGEAPPLYMASTERID);
                        }
                        if (ChargeMasterEntity.T_FB_EXTENSIONALORDER != null)
                        {
                            tblExten.Visibility = Visibility.Visible;
                            hypExten.Visibility = Visibility.Visible;
                        }

                        if (Convert.ToString(e.UserState) == "Edit")
                        {
                            var arg = new GetChargeApplyDetailByMasterIDCompletedEventArgs(new object[]{ChargeMasterEntity.T_FB_CHARGEAPPLYDETAIL}, null, false, null);
                            client_GetChargeApplyDetailByMasterIDCompleted(null,arg);
                        }
                        else
                        {
                            ObservableCollection<object> chaMasterLst = new ObservableCollection<object>();
                            chaMasterLst.Add(ChargeMasterEntity.CHARGEAPPLYMASTERID);
                            client.GetChargeApplyDetailByMasterIDAsync(chaMasterLst);
                        }
                        if (ChargeMasterEntity.T_FB_EXTENSIONALORDER != null)
                        {
                            if (types == FormTypes.Resubmit)
                            {
                                EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                                entBrowser.FormType = FormTypes.Browse;
                                RefreshUI(RefreshedTypes.ToolBar);
                                types = FormTypes.Browse;
                                SetForms();
                                MessageBox.Show( "请在出差报销模块中尝试重新提交!", "提示", MessageBoxButton.OK);
                            }
                            client.GetExtensionalOrderByIDAsync(ChargeMasterEntity.T_FB_EXTENSIONALORDER.EXTENSIONALORDERID);

                        }
                        // 删除多余的
                        ChargeMasterEntity.T_FB_CHARGEAPPLYDETAIL = null;
                        //查看时如果是兼职 在线公司 也显示
                        if (types == FormTypes.Browse)
                        {
                            var ents = from ent in SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts
                                       where ent.CompanyID == smtonlineID
                                       select ent;
                            if (ents.Count() > 0)
                            {
                                if (ChargeMasterEntity.OWNERCOMPANYID == smtonlineID)
                                {
                                    this.rbPayFromA.Visibility = Visibility.Visible;
                                    this.rbPayFromB.Visibility = Visibility.Visible;
                                }
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                RefreshUI(RefreshedTypes.HideProgressBar);
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), ex.ToString());
            }
        }

        void client_GetChargeApplyRepayDetailByMasterIDCompleted(object sender, GetChargeApplyRepayDetailByMasterIDCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                List<T_FB_CHARGEAPPLYREPAYDETAIL> chargeRepayDetailList = e.Result.ToList();
                chaRepDtlobj = e.Result;
                dgvRepayDetailList.ItemsSource = chargeRepayDetailList;
                dgvRepayDetailList.Loaded += new RoutedEventHandler(dgvRepayDetailList_Loaded);
            }
        }

        void dgvRepayDetailList_Loaded(object sender, RoutedEventArgs e)
        {
            IsEnabledByFormtype();
        }

        void personClient_GetEmployeePostBriefByEmployeeIDCompleted(object sender, GetEmployeePostBriefByEmployeeIDCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                string StrOwnerName = string.Empty, PostName = string.Empty, DepartmentName = string.Empty, CompanyName = string.Empty;
                string StrName = string.Empty;

                //未离职进入以下程序处理
                if (e.Result != null)
                {
                    V_EMPLOYEEDETAIL employeepost = new V_EMPLOYEEDETAIL();
                    employeepost = e.Result;
                    if (Application.Current.Resources["SYS_PostInfo"] != null)
                    {
                        if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>) != null)
                        {
                            if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == ChargeMasterEntity.OWNERPOSTID) != null)
                            {
                                if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == ChargeMasterEntity.OWNERPOSTID).FirstOrDefault() != null)
                                {
                                    if ((Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == ChargeMasterEntity.OWNERPOSTID).FirstOrDefault().T_HR_POSTDICTIONARY != null)
                                    {
                                        PostName = (Application.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>).Where(c => c.POSTID == ChargeMasterEntity.OWNERPOSTID).FirstOrDefault().T_HR_POSTDICTIONARY.POSTNAME;
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
                            if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == ChargeMasterEntity.OWNERDEPARTMENTID) != null)
                            {
                                if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == ChargeMasterEntity.OWNERDEPARTMENTID).FirstOrDefault() != null)
                                {
                                    if ((Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == ChargeMasterEntity.OWNERDEPARTMENTID).FirstOrDefault().T_HR_DEPARTMENTDICTIONARY != null)
                                    {
                                        DepartmentName = (Application.Current.Resources["SYS_DepartmentInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_DEPARTMENT>).Where(c => c.DEPARTMENTID == ChargeMasterEntity.OWNERDEPARTMENTID).FirstOrDefault().T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                                    }
                                }
                            }
                        }
                    }

                    if (Application.Current.Resources["SYS_CompanyInfo"] != null)
                    {
                        if ((Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>) != null)
                        {
                            if ((Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(c => c.COMPANYID == ChargeMasterEntity.OWNERCOMPANYID) != null)
                            {
                                if ((Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(c => c.COMPANYID == ChargeMasterEntity.OWNERCOMPANYID).FirstOrDefault() != null)
                                {
                                    CompanyName = (Application.Current.Resources["SYS_CompanyInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_COMPANY>).Where(c => c.COMPANYID == ChargeMasterEntity.OWNERCOMPANYID).FirstOrDefault().CNAME;
                                }
                            }
                        }
                    }

                    StrOwnerName = e.Result.EMPLOYEENAME;
                    if (employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == ChargeMasterEntity.OWNERPOSTID) != null)
                    {
                        if (employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == ChargeMasterEntity.OWNERPOSTID).FirstOrDefault() != null)
                        {
                            if (employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == ChargeMasterEntity.OWNERPOSTID).FirstOrDefault().POSTLEVEL != null)
                            {
                                txPostLevel = employeepost.EMPLOYEEPOSTS.Where(s => s.POSTID == ChargeMasterEntity.OWNERPOSTID).FirstOrDefault().POSTLEVEL.ToString();//获取出差人的岗位级别
                            }
                        }
                    }


                    StrName = e.Result.EMPLOYEENAME + "-" + PostName + "-" + DepartmentName + "-" + CompanyName;

                    txtOwnerName.Text = StrName;
                    ToolTipService.SetToolTip(txtOwnerName, StrName);

                  
                }
                else //已离职进入以下程序处理  
                {
                    if (ChargeMasterEntity == null)
                    {
                        return;
                    }

                    if (ChargeMasterEntity.CHECKSTATES != Convert.ToInt32(CheckStates.Approved) && ChargeMasterEntity.CHECKSTATES != Convert.ToInt32(CheckStates.UnApproved))
                    {
                        return;
                    }

                    StrOwnerName = ChargeMasterEntity.OWNERNAME;
                    StrName = StrOwnerName + "-" + ChargeMasterEntity.OWNERPOSTNAME + "-" + ChargeMasterEntity.OWNERDEPARTMENTNAME + "-" + ChargeMasterEntity.OWNERCOMPANYNAME;

                    txtOwnerName.Text = StrName;
                    ToolTipService.SetToolTip(txtOwnerName, StrName);
                }
                if (types != FormTypes.Resubmit)
                {
                    //RefreshUI(RefreshedTypes.All);
                    RefreshUI(RefreshedTypes.ToolBar);
                }
                RefreshUI(RefreshedTypes.AuditInfo);
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

        void client_GetChargeApplyDetailByMasterIDCompleted(object sender, GetChargeApplyDetailByMasterIDCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Result != null)
            {
                List<T_FB_CHARGEAPPLYDETAIL> chargeDetailList = e.Result.ToList();
                chaDtlobj = e.Result;
                //dgvChargeDetailList.ItemsSource = chargeDetailList;
                dgvChargeDetailList.ItemsSource = chaDtlobj;
                dgvChargeDetailList.Loaded += new RoutedEventHandler(dgvChargeDetailList_Loaded);
            }
        }

        void client_GetExtensionalOrderByIDCompleted(object sender, GetExtensionalOrderByIDCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    exten = e.Result;
                    if (exten.T_FB_EXTENSIONALTYPE != null)
                    {
                        tblExten.Visibility = Visibility.Visible;
                        tblExtension.Visibility = Visibility.Visible;
                        tblExtension.Text = exten.T_FB_EXTENSIONALTYPE.EXTENSIONALTYPENAME;
                    }
                }
            }
        }

        void dgvChargeDetailList_Loaded(object sender, RoutedEventArgs e)
        {
            IsEnabledByFormtype();
        }

        private void IsEnabledByFormtype()
        {
            if (types == FormTypes.Browse || types == FormTypes.Audit)
            {
                if (dgvChargeDetailList.ItemsSource != null)
                {
                    foreach (object obj in dgvChargeDetailList.ItemsSource)
                    {
                        if (dgvChargeDetailList.Columns[4].GetCellContent(obj) != null)
                        {
                            TextBox txtMark = dgvChargeDetailList.Columns[4].GetCellContent(obj).FindName("txtRemark") as TextBox;
                            txtMark.IsReadOnly = true;
                        }

                        if (dgvChargeDetailList.Columns[5].GetCellContent(obj) != null)
                        {
                            TextBox tbMon = dgvChargeDetailList.Columns[5].GetCellContent(obj).FindName("txtMon") as TextBox;
                            tbMon.IsReadOnly = true;
                        }

                        if (dgvChargeDetailList.Columns[6].GetCellContent(obj) != null)
                        {
                            Button btDel = dgvChargeDetailList.Columns[6].GetCellContent(obj).FindName("myDelete") as Button;
                            btDel.IsEnabled = false;
                        }
                    }
                }

                if (dgvRepayDetailList.IsEnabled == true && dgvRepayDetailList.ItemsSource != null)
                {
                    foreach (object obj in dgvRepayDetailList.ItemsSource)
                    {
                        if (dgvRepayDetailList.Columns[2].GetCellContent(obj) != null)
                        {
                            TextBox txtMark = dgvRepayDetailList.Columns[2].GetCellContent(obj).FindName("txtRepRemark") as TextBox;
                            txtMark.IsReadOnly = true;
                        }
                        if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                        {
                            TextBox txtMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtRepMon") as TextBox;
                            txtMon.IsReadOnly = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region 类型转换
        string GetChargeTpye(decimal? ChargeTpye)
        {
            return ChargeTpye == 1 ? "个人预算费用" : "部门预算费用";
        }
        #endregion

        #region 保存个人报销申请记录
        /// <summary>
        /// 设置当前页面状态
        /// </summary>
        private void RefreshFormType(FormTypes formtype, RefreshedTypes refreshedType)
        {
            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            entBrowser.FormType = formtype;
            types = formtype;
            FormTypesAction = formtype;
            RefreshUI(refreshedType);
            RefreshUI(RefreshedTypes.All);
        }

        /// <summary>
        /// 校验提交的报销明细
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

           
            if (nChargeType == 1)
            {
                if (this.rbPayOther.IsChecked.Value && string.IsNullOrWhiteSpace(this.payInfoB.Text))
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("汇多人账户时请填写支付信息"));
                    return false;
                }
                //在线的员工才会这样
                if (SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID == smtonlineID)
                {
                    if (!this.rbPayFromA.IsChecked.Value && !this.rbPayFromB.IsChecked.Value)
                    {
                        Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("在线员工请选择支付公司"));
                        return false;
                    }
                }
            }
            if (rabPayBor.IsChecked == true)  //冲借款情况
            {
                if (chaRepDtlobj.Count() == 0)
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "冲借款时明细项不能为空！");
                    return false;
                }

                if (decimal.Parse(txtTolRepayMon.Text) <= 0)
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "冲借款合计不能小于或等于0！");
                    return false;
                }

                if (dgvRepayDetailList.ItemsSource != null)
                {
                    foreach (object obj in dgvRepayDetailList.ItemsSource)
                    {
                        T_FB_CHARGEAPPLYREPAYDETAIL ent = obj as T_FB_CHARGEAPPLYREPAYDETAIL;
                        if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                        {
                            TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtRepMon") as TextBox;
                            decimal i = 0;
                            if (!decimal.TryParse(tbMon.Text, out i))
                            {
                                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "请输入正确的数值！");
                                return false;
                            }
                            if (decimal.Parse(tbMon.Text) < 0)
                            {
                                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "冲借款金额不能小于0！");
                                return false;
                            }
                            if (decimal.Parse(tbMon.Text) > ent.BORROWMONEY)
                            {
                                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "冲借款金额不能大于借款余额！");
                                return false;
                            }
                            if (dgvRepayDetailList.Columns[2].GetCellContent(obj) != null)
                            {
                                TextBox tbMark = dgvRepayDetailList.Columns[2].GetCellContent(obj).FindName("txtRepRemark") as TextBox;
                                if (string.IsNullOrEmpty(tbMark.Text) && decimal.Parse(tbMon.Text) > 0)
                                {
                                    Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "摘要不能为空！");
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            if (dgvChargeDetailList.ItemsSource != null)
            {
                foreach (object obj in dgvChargeDetailList.ItemsSource)
                {
                    if (dgvChargeDetailList.Columns[4].GetCellContent(obj) != null)
                    {
                        TextBox tbMark = dgvChargeDetailList.Columns[4].GetCellContent(obj).FindName("txtRemark") as TextBox;
                        if (string.IsNullOrEmpty(tbMark.Text))
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "摘要不能为空");
                            return false;
                        }
                    }
                    if (dgvChargeDetailList.Columns[5].GetCellContent(obj) != null)
                    {
                        TextBlock tbUsableMon = dgvChargeDetailList.Columns[3].GetCellContent(obj).FindName("txtUsableMoney") as TextBlock;
                        TextBox tbMon = dgvChargeDetailList.Columns[5].GetCellContent(obj).FindName("txtMon") as TextBox;

                        if (string.IsNullOrEmpty(tbMon.Text) || tbMon.Text == "0")
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "报销金额不能为0");
                            return false;
                        }

                        if (tbMon.Text.IndexOf("-") >= 0)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "不能输入负数");
                            return false;
                        }

                        decimal dUsableMon = 0, dChargeMon = 0;
                        GetUsableMon(tbUsableMon, ref dUsableMon);
                        decimal.TryParse(tbMon.Text, out dChargeMon);

                        if (dUsableMon == 999999 || dUsableMon == 99999999)  //add zl 2012.2.18
                        {
                            continue;
                        }

                        if (dUsableMon < dChargeMon && (dUsableMon != 999999 || dUsableMon != 99999999))
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "报销金额不能大于可用结余");
                            return false;
                        }
                    }
                }
            }

            #region 判断同一科目报销总额不能大于可用费用
            if (dgvChargeDetailList.ItemsSource != null)
            {
                var AllDetail = GetChargeDetails().OrderByDescending(p => p.T_FB_SUBJECT.SUBJECTNAME).ToList();
                if (!isHuNanHangXin)
                {
                    if (AllDetail.Count() <= 0 || AllDetail.Count() > 5)
                    {
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "无报销明细，或明细超过5条都不能保存");
                        return false;
                    }
                }
                
                var g = AllDetail.GroupBy(item => new { item.T_FB_SUBJECT.SUBJECTID, item.T_FB_SUBJECT.SUBJECTNAME });
                foreach (var temp in g)
                {
                    var usableMoney = temp.Min(item => item.USABLEMONEY);
                    var chargeMoney = temp.Sum(item => item.CHARGEMONEY);
                    if (chargeMoney > usableMoney)
                    {
                        var msg = string.Format("{0}报销总金额不能大于可用费用！", temp.Key.SUBJECTNAME);
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), msg);
                        return false;
                    }
                }
            }

            #endregion
            return true;
        }


        private bool Check4Submit()
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

            if (rabPayBor.IsChecked == false)
            {
                if (chaDtlobj.Count == 0)
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("REQUIRED", "REIMBURSEMENTDETAILS"));
                    return false;
                }
            }
            if (nChargeType == 1)
            {
                if (this.rbPayOther.IsChecked.Value && string.IsNullOrWhiteSpace(this.payInfoB.Text))
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("汇多人账户时请填写支付信息"));
                    return false;
                }
                //在线的员工才会这样
                if (SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID == smtonlineID)
                {
                    if (!this.rbPayFromA.IsChecked.Value && !this.rbPayFromB.IsChecked.Value)
                    {
                        Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("WARING"), Utility.GetResourceStr("在线员工请选择支付公司"));
                        return false;
                    }
                }
            }
            if (rabPayBor.IsChecked == true)  //冲借款情况
            {
                if (chaRepDtlobj.Count() == 0)
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "冲借款时明细项不能为空！");
                    return false;
                }

                if (decimal.Parse(txtTolRepayMon.Text) <= 0)
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "冲借款合计不能小于或等于0！");
                    return false;
                }

                if (decimal.Parse(txtTolRepayMon.Text) > decimal.Parse(txttotal.Text))
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "冲借款合计不能大于报销合计！");
                    return false;
                }

                if (dgvRepayDetailList.ItemsSource != null)
                {
                    
                    foreach (object obj in dgvRepayDetailList.ItemsSource)
                    {
                        T_FB_CHARGEAPPLYREPAYDETAIL ent = obj as T_FB_CHARGEAPPLYREPAYDETAIL;
                        if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                        {
                            TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtRepMon") as TextBox;
                            decimal i = 0;
                            if (!decimal.TryParse(tbMon.Text, out i))
                            {
                                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "请输入正确的数值！");
                                return false;
                            }
                            var tempMoney = decimal.Parse(tbMon.Text);
                            if (tempMoney < 0)
                            {
                                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "冲借款金额不能小于0！");
                                return false;
                            }
                            
                            if (decimal.Parse(tbMon.Text) > ent.BORROWMONEY)
                            {
                                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "冲借款金额不能大于借款余额！");
                                return false;
                            }
                            if (dgvRepayDetailList.Columns[2].GetCellContent(obj) != null)
                            {
                                TextBox tbMark = dgvRepayDetailList.Columns[2].GetCellContent(obj).FindName("txtRepRemark") as TextBox;
                                if (string.IsNullOrEmpty(tbMark.Text) && decimal.Parse(tbMon.Text) > 0)
                                {
                                    Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "摘要不能为空！");
                                    return false;
                                }
                            }
                        }
                    }
                   
                }
            }

            if (dgvChargeDetailList.ItemsSource != null)
            {
                decimal totalmoney = 0;
                foreach (object obj in dgvChargeDetailList.ItemsSource)
                {
                    if (dgvChargeDetailList.Columns[4].GetCellContent(obj) != null)
                    {
                        TextBox tbMark = dgvChargeDetailList.Columns[4].GetCellContent(obj).FindName("txtRemark") as TextBox;
                        if (string.IsNullOrEmpty(tbMark.Text))
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "摘要不能为空");
                            return false;
                        }
                    }
                    if (dgvChargeDetailList.Columns[5].GetCellContent(obj) != null)
                    {
                        TextBlock tbUsableMon = dgvChargeDetailList.Columns[3].GetCellContent(obj).FindName("txtUsableMoney") as TextBlock;
                        TextBox tbMon = dgvChargeDetailList.Columns[5].GetCellContent(obj).FindName("txtMon") as TextBox;

                        
                        if (string.IsNullOrEmpty(tbMon.Text) || tbMon.Text == "0")
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "报销金额不能为0!");
                            return false;
                        }
                        var tempMoney = decimal.Parse(tbMon.Text);
                        totalmoney += tempMoney;

                        if (tbMon.Text.IndexOf("-") >= 0)
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "不能输入负数");
                            return false;
                        }

                        decimal dUsableMon = 0, dChargeMon = 0;
                        GetUsableMon(tbUsableMon, ref dUsableMon);
                        decimal.TryParse(tbMon.Text, out dChargeMon);

                        if (dUsableMon == 999999 || dUsableMon == 99999999)  //add zl 2012.2.18
                        {
                            continue;
                        }

                        if (dUsableMon < dChargeMon && (dUsableMon != 999999 || dUsableMon != 99999999))
                        {
                            Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "报销金额不能大于可用结余");
                            return false;
                        }
                    }
                }
                if (totalmoney <= 0 && decimal.Parse(txtTolRepayMon.Text) <= 0)
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), Utility.GetResourceStr("CHARGEMONEYISNULL"));
                    return false;
                }
            }

            #region 判断同一科目报销总额不能大于可用费用
            if (dgvChargeDetailList.ItemsSource != null)
            {
                var AllDetail = GetChargeDetails().OrderByDescending(p => p.T_FB_SUBJECT.SUBJECTNAME).ToList();
                if (!isHuNanHangXin)
                {
                    if (AllDetail.Count() <= 0 || AllDetail.Count() > 5)
                    {
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "无报销明细，或明细超过5条都不能保存");
                        return false;
                    }
                }                
                
                var g = AllDetail.GroupBy(item => new { item.T_FB_SUBJECT.SUBJECTID, item.T_FB_SUBJECT.SUBJECTNAME });
                foreach (var temp in g)
                {
                    var usableMoney = temp.Min(item => item.USABLEMONEY);
                    var chargeMoney = temp.Sum(item => item.CHARGEMONEY);
                    if (chargeMoney > usableMoney)
                    {
                        var msg = string.Format("{0}报销总金额不能大于可用费用！", temp.Key.SUBJECTNAME);
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), msg);
                        return false;
                    }
                }
            }

            #endregion
            return true;
        }
      
        /// <summary>
        /// 解决科目不受月度预算限制时可用额度的获取
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="dUsableMon"></param>
        private void GetUsableMon(TextBlock txt, ref decimal dUsableMon)
        {
            decimal dCheck = 0;
            if (txt.Tag != null)
            {
                decimal.TryParse(txt.Tag.ToString(), out dCheck);
            }

            decimal.TryParse(txt.Text, out dUsableMon);

            if (dCheck > dUsableMon)
            {
                dUsableMon = dCheck;
            }
        }

        private ObservableCollection<T_FB_CHARGEAPPLYDETAIL> GetChargeDetails()
        {
            ObservableCollection<T_FB_CHARGEAPPLYDETAIL> entResList = new ObservableCollection<T_FB_CHARGEAPPLYDETAIL>();
            if (dgvChargeDetailList.ItemsSource != null)
            {
                foreach (object obj in dgvChargeDetailList.ItemsSource)
                {
                    T_FB_CHARGEAPPLYDETAIL entRes = obj as T_FB_CHARGEAPPLYDETAIL;
                    entResList.Add(entRes);
                }
            }
            return entResList;
        }

        /// <summary>
        /// 检查申请人的身份
        /// </summary>
        /// <param name="chargeMaster">chargeMaster</param>
        /// <returns></returns>
        private void SaveChargeApply(T_FB_CHARGEAPPLYMASTER chargeMaster)
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
                    if (! string.IsNullOrEmpty(strMsgE))
                    {
                        Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), strMsgE);
                    }
                    else
                    {
                        // 没有错的情况再保存
                        InnerSaveChargeApply(ChargeMasterEntity);
                    }
                };
            RefreshUI(RefreshedTypes.ShowProgressBar);
            psc.GetEmployeeDetailByIDAsync(strOwnerID);
           
        }

        /// <summary>
        /// 保存费用报销
        /// </summary>
        /// <param name="chargeMaster"></param>
        private void InnerSaveChargeApply(T_FB_CHARGEAPPLYMASTER chargeMaster)
        {
            // chargeMaster.BUDGETARYMONTH = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM") + "-1");
            chargeMaster.CREATECOMPANYID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            chargeMaster.CREATECOMPANYNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyName;
            chargeMaster.CREATEDATE = DateTime.Now;
            chargeMaster.CREATEDEPARTMENTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
            chargeMaster.CREATEDEPARTMENTNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentName;
            chargeMaster.CREATEPOSTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
            chargeMaster.CREATEPOSTNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostName;
            chargeMaster.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            chargeMaster.CREATEUSERNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
            chargeMaster.EDITSTATES = 0;
            chargeMaster.OWNERCOMPANYID = strOwnerCompanyID;
            chargeMaster.OWNERCOMPANYNAME = strOwnerCompanyName;
            chargeMaster.OWNERDEPARTMENTID = strOwnerDepartmentID;
            chargeMaster.OWNERDEPARTMENTNAME = strOwnerDepartmentName;
            chargeMaster.OWNERID = strOwnerID;
            chargeMaster.OWNERNAME = strOwnerName;
            chargeMaster.OWNERPOSTID = strOwnerPostID;
            chargeMaster.OWNERPOSTNAME = strOwnerPostName;
            chargeMaster.PAYTYPE = nChargeType;
            if (txtCode.Text.IndexOf('>') > 0)
            {
                chargeMaster.CHARGEAPPLYMASTERCODE = " ";
            }
            else
            {
                chargeMaster.CHARGEAPPLYMASTERCODE = txtCode.Text;
            }
         
            if (nChargeType==1)//1为费用报销
            {
                #region 费用报销情况
                if (this.rbPayMySelft.IsChecked.Value)
                {
                    chargeMaster.PAYTARGET = 0;//2013/12/16号起PAYTARGET字段意思：0付本人，1原始值，目前不用，2付多人
                }
                else
                {
                    chargeMaster.PAYTARGET = 2;
                }
                #endregion
            }
            else
            {
                chargeMaster.PAYTARGET = 1;//不是费用报销则保留原来存值为1
            }
            chargeMaster.PAYMENTINFO = GetPayInfo();
            if (!string.IsNullOrEmpty(txtItemID.Text))
            {
                
                // Bank 字段用于做事项审批关联
                chargeMaster.BANK = txtItemID.Text;
                chargeMaster.BANKACCOUT = txtItemCode.Text;
                chargeMaster.RECEIVER = "T_OA_APPROVALINFO";
            }
            else
            {
                chargeMaster.BANK = null;
                chargeMaster.BANKACCOUT = null;
                chargeMaster.RECEIVER = null;
            }
            chargeMaster.REMARK = txtRemark.Text;
            chargeMaster.TOTALMONEY = decimal.Parse(txttotal.Text.ToString());
            chargeMaster.UPDATEDATE = DateTime.Now;
            chargeMaster.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            chargeMaster.UPDATEUSERNAME = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
            if (rabPayBor.IsChecked == true)    //如果是冲借款就保存冲款金额
            {
                chargeMaster.REPAYMENT = decimal.Parse(txtTolRepayMon.Text);
                //chargeMaster.TOTALMONEY += decimal.Parse(txtTolRepayMon.Text);

                chaRepayDetailData.Clear();
                foreach (object obj in dgvRepayDetailList.ItemsSource)
                {
                    T_FB_CHARGEAPPLYREPAYDETAIL ent = obj as T_FB_CHARGEAPPLYREPAYDETAIL;
                    T_FB_CHARGEAPPLYREPAYDETAIL chaRepay = new T_FB_CHARGEAPPLYREPAYDETAIL();
                    chaRepay.CHARGEAPPLYREPAYDETAILID = System.Guid.NewGuid().ToString();
                    chaRepay.UPDATEDATE = DateTime.Now;
                    chaRepay.CREATEDATE = DateTime.Now;
                    chaRepay.T_FB_CHARGEAPPLYMASTER = chargeMaster;
                    chaRepay.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                    chaRepay.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                    chaRepay.BORROWMONEY = ent.BORROWMONEY;
                    chaRepay.REPAYTYPE = ent.REPAYTYPE;

                    if (dgvRepayDetailList.Columns[2].GetCellContent(obj) != null)
                    {
                        TextBox txtMark = dgvRepayDetailList.Columns[2].GetCellContent(obj).FindName("txtRepRemark") as TextBox;
                        chaRepay.REMARK = txtMark.Text;
                    }
                    if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                    {
                        TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtRepMon") as TextBox;
                        chaRepay.REPAYMONEY = decimal.Parse(string.IsNullOrEmpty(tbMon.Text.ToString()) == true ? "0" : tbMon.Text.ToString());
                    }
                    chaRepayDetailData.Add(chaRepay);
                }
            }
            else
            {
                chargeMaster.REPAYMENT = 0;
                //chargeMaster.REPAYTYPE = null;
            }

          
            chargeDtlList.Clear();
            //chaDtlobj.Clear();
            if (dgvChargeDetailList.ItemsSource != null)
            {
                foreach (object obj in dgvChargeDetailList.ItemsSource)
                {
                    T_FB_CHARGEAPPLYDETAIL ent = obj as T_FB_CHARGEAPPLYDETAIL;
                    T_FB_CHARGEAPPLYDETAIL charge = new T_FB_CHARGEAPPLYDETAIL();
                    charge.CHARGEAPPLYDETAILID = System.Guid.NewGuid().ToString();
                    charge.CHARGETYPE = ent.CHARGETYPE;
                    charge.CREATEDATE = DateTime.Now;
                    charge.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                    charge.T_FB_BORROWAPPLYDETAIL = ent.T_FB_BORROWAPPLYDETAIL;
                    charge.T_FB_CHARGEAPPLYMASTER = chargeMaster;
                    charge.T_FB_SUBJECT = ent.T_FB_SUBJECT;
                    charge.UPDATEDATE = DateTime.Now;
                    charge.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                    charge.USABLEMONEY = ent.USABLEMONEY;

                    if (dgvChargeDetailList.Columns[4].GetCellContent(obj) != null)
                    {
                        TextBox txtMark = dgvChargeDetailList.Columns[4].GetCellContent(obj).FindName("txtRemark") as TextBox;
                        charge.REMARK = txtMark.Text;
                    }
                    if (dgvChargeDetailList.Columns[5].GetCellContent(obj) != null)
                    {
                        TextBox tbMon = dgvChargeDetailList.Columns[5].GetCellContent(obj).FindName("txtMon") as TextBox;
                        charge.CHARGEMONEY = decimal.Parse(string.IsNullOrEmpty(tbMon.Text.ToString()) == true ? "0" : tbMon.Text.ToString());
                    }
                    chargeDtlList.Add(charge);
                }
            }

            RefreshUI(RefreshedTypes.ShowProgressBar);
            string strMsg = string.Empty;
            if (types == FormTypes.New)
            {
                client.AddChargeApplyMasterAndDetailAsync(chargeMaster, chargeDtlList, chaRepayDetailData);
            }
            else if (types == FormTypes.Edit)
            {
                string op = needsubmit == false ? "Edit" : "Submit";
                client.UptChargeApplyMasterAndDetailAsync(op, chargeMaster, chargeDtlList, chaRepayDetailData, strMsg, op);
            }
            else if (types == FormTypes.Resubmit)
            {
                client.UptChargeApplyMasterAndDetailAsync("ReSubmit", chargeMaster, chargeDtlList, chaRepayDetailData, strMsg, "ReSubmit");
            }
        }

        /// <summary>
        /// 处理新增费用报销及其明细后返回的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_AddChargeApplyMasterAndDetailCompleted(object sender, AddChargeApplyMasterAndDetailCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Result)
            {
                if (e.Error != null && e.Error.Message != "")
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
                    return;
                }
                isAdd = false;
                //txtCode.Text = e.Result; //add by zl
                //ChargeMasterEntity.CHARGEAPPLYMASTERCODE = e.Result; //add by zl
                //chaDtlobj = chargeDtlList; // add by zl
                Utility.ShowMessageBox("ADD", isSubmitFlow, true);
                if (isSubmitFlow)
                {
                    saveType = RefreshedTypes.CloseAndReloadData;
                    RefreshUI(saveType);
                    return;
                }
                //ctrFile.FormID = ChargeMasterEntity.CHARGEAPPLYMASTERID;
                //ctrFile.Save();

                RefreshFormType(FormTypes.Edit, RefreshedTypes.AuditInfo);
            }
            else
            {
                Utility.ShowMessageBox("ADD", isSubmitFlow, false);
            }
        }

        /// <summary>
        /// 处理更新费用报销及其明细后返回的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UptChargeApplyMasterAndDetailCompleted(object sender, UptChargeApplyMasterAndDetailCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error == null)
            {
                var msg = e.strMsg ?? "";
                if (!msg.StartsWith("OK:"))
                {
                    msg = msg == "" ? "保存数据失败，服务未返回任何信息!" : msg;
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("WARNING"), msg);
                    return;
                }
                else if (Convert.ToString(e.UserState) == "Edit")
                {
                    Utility.ShowMessageBox("UPDATE", isSubmitFlow, true);//修改成功   
                    RefreshUI(RefreshedTypes.All);
                }
                else
                {   // 提交或重新提交
                    var code = msg.Split(':')[1];
                    if (!string.IsNullOrEmpty(code)) //获取单据号  zl
                    {
                        ChargeMasterEntity.CHARGEAPPLYMASTERCODE = code;
                        txtCode.Text = ChargeMasterEntity.CHARGEAPPLYMASTERCODE;
                        visibCode();

                        EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
                        entBrowser.ManualSubmit();
                        IsEnabledByFormtype();
                        SetForms();
                        needsubmit = false;
                    }
                    else
                    {
                        Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("TIPS"), "产生单据号失败！");
                        return;
                    }

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

        /// <summary>
        /// 根据审核状态，处理费用报销更新后返回的结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UptChargeApplyCheckStateCompleted(object sender, UptChargeApplyCheckStateCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            if (e.Error == null)
            {
                if (e.Result)
                {
                    Utility.ShowMessageBox("AUDIT", isSubmitFlow, true);//审核成功
                    RefreshUI(RefreshedTypes.AuditInfo);
                }
                else
                {
                    Utility.ShowMessageBox("AUDIT", isSubmitFlow, false);//提交失败
                }
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), e.Error.Message);
            }

            RefreshUI(RefreshedTypes.All);
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
                    strOwnerCompanyName = corp.CNAME;
                    strOwnerDepartmentID = deptid;
                    strOwnerDepartmentName = dept.ObjectName;
                    strOwnerPostID = postid;
                    strOwnerPostName = post.ObjectName;
                    strOwnerID = userInfo.ObjectID;
                    strOwnerName = userInfo.ObjectName;
                    txPostLevel = emp.T_HR_EMPLOYEEPOST.Where(t => t.T_HR_POST.POSTID == postid).FirstOrDefault().POSTLEVEL.ToString();
                    ToolTipService.SetToolTip(txtOwnerName, StrEmployee);
                    if (rabPayBor.IsChecked == true)
                    {
                        GetPersonAccountData();
                    }

                }
                dgvChargeDetailList.ItemsSource = null;
                chaDtlobj.Clear();
                txttotal.Text = "";
            };


            lookup.MultiSelected = false;
            lookup.Show();
        }
        #endregion

        #region 事项审批
        private void btnLookUpItem_Click(object sender, RoutedEventArgs e)
        {
            ItemLookUp frmU = new ItemLookUp(strOwnerID, strOwnerPostID, strOwnerDepartmentID, strOwnerCompanyID);
            EntityBrowser browser = new EntityBrowser(frmU);
            browser.MinHeight = 400;
            browser.MinWidth = 500;
            browser.ReloadDataEvent += () =>
                {
                    if (frmU.SelectedItem == null)
                    {
                        frmU.SelectedItem = new SMT.Saas.Tools.SmtOACommonOfficeService.T_OA_APPROVALINFO();
                    }
                    this.txtItemCode.Text = frmU.SelectedItem.APPROVALCODE ?? "";
                    this.txtItemID.Text = frmU.SelectedItem.APPROVALID ?? "";
                };
            browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, strOwnerID);
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
            //   string Strreturn = "";
            //switch (FormTypesAction)
            //{
            //case FormTypes.New:
            //    Strreturn = Utility.GetResourceStr("PEOPLECHARGEAPPLY");
            //    break;
            //case FormTypes.Edit:
            //    Strreturn = Utility.GetResourceStr( "PEOPLECHARGEAPPLY");
            //    break;
            //case FormTypes.Audit:
            //    Strreturn = Utility.GetResourceStr("PEOPLECHARGEAPPLY");
            //    break;
            //case FormTypes.Browse:
            //    Strreturn = Utility.GetResourceStr("PEOPLECHARGEAPPLY");
            //    break;
            //case FormTypes.Resubmit:
            //    Strreturn = Utility.GetResourceStr("EDITTITLE", "PEOPLECHARGEAPPLY");
            //    break;
            // }
            return Utility.GetResourceStr("费用报销");
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
                        client.DelChargeApplyMasterAndDetailAsync(new ObservableCollection<string>() { ChargeMasterEntity.CHARGEAPPLYMASTERID });
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
                    SaveChargeApply(ChargeMasterEntity);
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
                if (FormTypesAction != FormTypes.New && ChargeMasterEntity.CHECKSTATES == 0)
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

        private string GetXmlString(string StrSorce, T_FB_CHARGEAPPLYMASTER Info)
        {
            string goouttomeet = string.Empty;
            string privateaffair = string.Empty;
            string companycar = string.Empty;
            string isagent = string.Empty;
            List<object> ObjectList = new List<object>();

            SMT.SaaS.MobileXml.MobileXml mx = new MobileXml();
            SMT.SaaS.MobileXml.AutoDictionary ad = new AutoDictionary();
            List<SMT.SaaS.MobileXml.AutoDictionary> AutoList = new List<SMT.SaaS.MobileXml.AutoDictionary>();
            AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "PAYTARGET", Info.PAYTARGET.ToString(), "个人"));//付款方式

            string StrPayType = "";
            string StrEditState = "";
            string StrRepType = "";
            switch (Info.PAYTYPE.ToString())
            {
                case "1":
                    StrPayType = "个人费用报销";
                    break;
                case "2":
                    StrPayType = "冲借款";
                    break;
                case "3":
                    StrPayType = "冲预付款";
                    break;
                case "4":
                    StrPayType = "付客户款";
                    break;
                case "5":
                    StrPayType = "其他";
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
            
            
            AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "POSTLEVEL", txPostLevel, null));//POSTLEVEL

            AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "PAYTYPE", Info.PAYTYPE.ToString(), StrPayType));//付款类型
            AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "EDITSTATES", Info.EDITSTATES.ToString(), StrEditState));//编辑状态
            if (Info.T_FB_BORROWAPPLYMASTER != null)
            {
                AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "BORROWAPPLYMASTERID", Info.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERCODE, Info.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERCODE));
            }
            else
            {
                AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "BORROWAPPLYMASTERID", "", ""));
            }
            if (Info.OWNERID != null && !string.IsNullOrEmpty(strOwnerName))
            {
                AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "OWNERID", Info.OWNERID, strOwnerName + "-" + strOwnerPostName + "-" + strOwnerDepartmentName + "-" + strOwnerCompanyName));
            }
            if (Info.OWNERCOMPANYID != null && !string.IsNullOrEmpty(strOwnerCompanyName))
            {
                AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "OWNERCOMPANYID", Info.OWNERCOMPANYID, strOwnerCompanyName));
            }
            if (Info.OWNERDEPARTMENTID != null && !string.IsNullOrEmpty(strOwnerDepartmentName))
            {
                AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "OWNERDEPARTMENTID", Info.OWNERDEPARTMENTID, strOwnerDepartmentName));
            }
            if (Info.OWNERPOSTID != null && !string.IsNullOrEmpty(strOwnerPostName))
            {
                AutoList.Add(basedata("T_FB_CHARGEAPPLYMASTER", "OWNERPOSTID", Info.OWNERPOSTID, strOwnerPostName));
            }

            List<T_FB_CHARGEAPPLYDETAIL> objC;
            if (chargeDtlList != null && chargeDtlList.Count > 0)
            {
                objC = chargeDtlList.ToList();
            }
            else
            {
                objC = chaDtlobj.ToList();
            }
            foreach (T_FB_CHARGEAPPLYDETAIL objDetail in objC)
            {
                if (objDetail.T_FB_SUBJECT != null)
                {
                    AutoList.Add(basedataForChild("T_FB_CHARGEAPPLYDETAIL", "SUBJECTID", objDetail.T_FB_SUBJECT.SUBJECTID, objDetail.T_FB_SUBJECT.SUBJECTID, objDetail.CHARGEAPPLYDETAILID));
                    AutoList.Add(basedataForChild("T_FB_CHARGEAPPLYDETAIL", "SUBJECTCODE", objDetail.T_FB_SUBJECT.SUBJECTCODE, objDetail.T_FB_SUBJECT.SUBJECTCODE, objDetail.CHARGEAPPLYDETAILID));
                    AutoList.Add(basedataForChild("T_FB_CHARGEAPPLYDETAIL", "SUBJECTNAME", objDetail.T_FB_SUBJECT.SUBJECTNAME, objDetail.T_FB_SUBJECT.SUBJECTNAME, objDetail.CHARGEAPPLYDETAILID));
                }
                if (objDetail.T_FB_BORROWAPPLYDETAIL != null)
                {
                    AutoList.Add(basedataForChild("T_FB_CHARGEAPPLYDETAIL", "BORROWAPPLYDETAILID", objDetail.T_FB_BORROWAPPLYDETAIL.BORROWAPPLYDETAILID, objDetail.T_FB_BORROWAPPLYDETAIL.BORROWAPPLYDETAILID, objDetail.CHARGEAPPLYDETAILID));
                    AutoList.Add(basedataForChild("T_FB_CHARGEAPPLYDETAIL", "BORROWMONEY", objDetail.T_FB_BORROWAPPLYDETAIL.BORROWMONEY.ToString(), objDetail.T_FB_BORROWAPPLYDETAIL.BORROWMONEY.ToString(), objDetail.CHARGEAPPLYDETAILID));
                }
                if (objDetail.T_FB_CHARGEAPPLYMASTER != null)
                {
                    AutoList.Add(basedataForChild("T_FB_CHARGEAPPLYDETAIL", "CHARGEAPPLYMASTERID", objDetail.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID, objDetail.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID, objDetail.CHARGEAPPLYDETAILID));
                }
                if (objDetail.CHARGETYPE != null)
                {
                    AutoList.Add(basedataForChild("T_FB_CHARGEAPPLYDETAIL", "CHARGETYPE", objDetail.CHARGETYPE.ToString(), objDetail.CHARGETYPE.ToString() == "1" ? "个人预算费用" : "公共预算费用", objDetail.CHARGEAPPLYDETAILID));
                }
                ObjectList.Add(objDetail);
            }
            //add zl 2012.2.15
            List<T_FB_CHARGEAPPLYREPAYDETAIL> objR;
            if (chaRepayDetailData != null && chaRepayDetailData.Count > 0)
            {
                objR = chaRepayDetailData.ToList();
            }
            else
            {
                objR = chaRepDtlobj.ToList();
            }
            foreach (T_FB_CHARGEAPPLYREPAYDETAIL objReDetail in objR)
            {
                switch (objReDetail.REPAYTYPE.ToString())
                {
                    case "1":
                        StrRepType = "普通借款";
                        break;
                    case "2":
                        StrRepType = "备用金借款";
                        break;
                    case "3":
                        StrRepType = "专项借款";
                        break;
                }
                if (objReDetail.T_FB_CHARGEAPPLYMASTER != null)
                {
                    AutoList.Add(basedataForChild("T_FB_CHARGEAPPLYREPAYDETAIL", "CHARGEAPPLYMASTERID", objReDetail.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID, objReDetail.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID, objReDetail.CHARGEAPPLYREPAYDETAILID));
                }
                if (objReDetail.REPAYTYPE != 0)
                {
                    AutoList.Add(basedataForChild("T_FB_CHARGEAPPLYREPAYDETAIL", "REPAYTYPE", objReDetail.REPAYTYPE.ToString(), StrRepType, objReDetail.CHARGEAPPLYREPAYDETAILID));
                }
                ObjectList.Add(objReDetail);
            }
            //add end
            string a = mx.TableToXml(Info, ObjectList, StrSorce, AutoList);
            XElement xe = XElement.Parse(a);
            if (!string.IsNullOrEmpty(Info.BANK))
            {
                xe.Element("Object").Add(new XElement("Attribute",
                    new XAttribute("Name", "LINK"),
                    new XAttribute("LableResourceID", "LINK"),
                    new XAttribute("Description", "事项审批编号"),
                    new XAttribute("DataType", "string"),
                    new XAttribute("DataValue", Info.RECEIVER + "|" + Info.BANK + "|" + Info.BANKACCOUT),
                    new XAttribute("DataText", "")));   
            }   
            xe.Element("Object").Elements("ObjectList").ToArray()[1].Elements().ForEach(item =>
                {
                    var Attr_REPAYTYPE = item.Elements().Where(itemA => itemA.Attribute("Name").Value == "REPAYTYPE").FirstOrDefault();
                    var dataText = "";
                    switch (Attr_REPAYTYPE.Attribute("DataValue").Value)
                    {
                        case "1":
                            dataText = "普通借款";
                            break;
                        case "2":
                            dataText = "备用金借款";
                            break;
                        case "3":
                            dataText = "专项借款";
                            break;
                    }
                    Attr_REPAYTYPE.Attribute("DataText").Value = dataText;
                });
            return xe.ToString();
        }

        public void SetFlowRecordEntity(SMT.SaaS.FrameworkUI.AuditControl.Flow_FlowRecord_T entity)
        {
            string strXmlObjectSource = string.Empty;
            //XML
            entity.SystemCode = "FB";
            if (!string.IsNullOrEmpty(entity.BusinessObjectDefineXML)) //返回的XML定义不为空时对业务对象进行填充
            {
                strXmlObjectSource = this.GetXmlString(entity.BusinessObjectDefineXML, ChargeMasterEntity);
            }
            Dictionary<string, string> paraIDs = new Dictionary<string, string>();
            if (ChargeMasterEntity != null)
            {
                paraIDs.Add("CreateUserID", ChargeMasterEntity.OWNERID);
                paraIDs.Add("CreatePostID", ChargeMasterEntity.OWNERPOSTID);
                paraIDs.Add("CreateDepartmentID", ChargeMasterEntity.OWNERDEPARTMENTID);
                paraIDs.Add("CreateCompanyID", ChargeMasterEntity.OWNERCOMPANYID);
            }

            if (ChargeMasterEntity.CHECKSTATES == ((decimal)CheckStates.UnSubmit) || ChargeMasterEntity.CHECKSTATES == ((decimal)CheckStates.UnApproved))
            {
                SMT.SaaS.FrameworkUI.Common.Utility.SetAuditEntity(entity, "T_FB_CHARGEAPPLYMASTER", ChargeMasterEntity.CHARGEAPPLYMASTERID, strXmlObjectSource, paraIDs);
            }
            else
            {
                Utility.SetAuditEntity(entity, "T_FB_CHARGEAPPLYMASTER", ChargeMasterEntity.CHARGEAPPLYMASTERID, strXmlObjectSource);
            }
        }

        public void OnSubmitCompleted(SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult args)
        {
            CheckStates state = CheckStates.UnSubmit;
            SMT.FBAnalysis.UI.Common.Utility.InitFileLoad(FormTypes.Audit, uploadFile, ChargeMasterEntity.CHARGEAPPLYMASTERID, false);
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

            //zl add 12.13
            if (state == CheckStates.Approving)
            {
                client.GetChargeApplyMasterByIDAsync(ChargeMasterEntity.CHARGEAPPLYMASTERID);
            }
            //zl end

            if (types == FormTypes.Edit || types == FormTypes.Resubmit)
            {
                Utility.ShowMessageBox("SUBMIT", isSubmitFlow, true);//提交审核成功
            }
            else if (types == FormTypes.Audit)
            {
                Utility.ShowMessageBox("AUDIT", isSubmitFlow, true);//审核成功
            }

            RefreshUI(RefreshedTypes.HideProgressBar);

            ChargeMasterEntity.CHECKSTATES = Convert.ToInt32(state);
            RefreshUI(RefreshedTypes.AuditInfo);
            RefreshUI(RefreshedTypes.All);
        }

        public string GetAuditState()
        {
            string state = "-1";
            if (ChargeMasterEntity != null)
                state = ChargeMasterEntity.CHECKSTATES.ToString();
            //if (types == FormTypes.Resubmit || types == FormTypes.Edit)
            //{
            //    state = "0";
            //}
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
            //        SMT.SaaS.FrameworkUI.Common.Utility.HiddenGridRow(this.LayoutRoot, 9);
            //    }
            //}
        }
        #endregion

        #region 页面事件
        private void rabPayType_Click(object sender, RoutedEventArgs e)
        {
            if (sender == rabPerCharge) { nChargeType = 1; }
            else if (sender == rabPayBor) {
                nChargeType = 2; 
                txtTolRepayMon.Text = "0.00"; 
                GetPersonAccountData(); 
            }
            else if (sender == rabPayAdvance) { nChargeType = 3; }
            else if (sender == rabPayCus) { nChargeType = 4; }
            else if (sender == rabPayOther) {nChargeType = 5; }
            RefreshPayInfo();
        }

        /// <summary>
        /// 在线公司的员工才会出现选择支付公司
        /// </summary>
        private void setSmtonline()
        {
            //如果该员工的主岗位公司ID为在线公司ID则出现改控件，则
            if (SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID == smtonlineID)
            {
                this.rbPayFromA.Visibility = Visibility.Visible;
                this.rbPayFromB.Visibility = Visibility.Visible;
            }                      
            
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
       

        //获取相应人员的借款金额
        public void GetPersonAccountData()
        {
            string filter = "";    //查询过滤条件
            ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();   //参数值
            if (!string.IsNullOrEmpty(strOwnerCompanyID))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "@" + paras.Count().ToString() + ".Contains(OWNERCOMPANYID) ";
                paras.Add(strOwnerCompanyID);
            }
            if (!string.IsNullOrEmpty(strOwnerID))
            {
                if (!string.IsNullOrEmpty(filter))
                {
                    filter += " and ";
                }
                filter += "@" + paras.Count().ToString() + ".Contains(OWNERID) ";
                paras.Add(strOwnerID);
            }
            client.GetPersonAccountListByMultSearchAsync(filter, paras, "PERSONACCOUNTID");
        }

        void client_GetPersonAccountListByMultSearchCompleted(object sender, GetPersonAccountListByMultSearchCompletedEventArgs e)
        {
            T_FB_PERSONACCOUNT PerEntity = new T_FB_PERSONACCOUNT();
            try
            {
                chaRepDtlobj.Clear();
                if (e.Error == null)
                {
                    if (e.Result != null && e.Result.Count > 0)
                    {
                        PerEntity = e.Result.FirstOrDefault();
                        if (PerEntity.SIMPLEBORROWMONEY > 0)
                        {
                            T_FB_CHARGEAPPLYREPAYDETAIL repay = new T_FB_CHARGEAPPLYREPAYDETAIL();
                            repay.BORROWMONEY = PerEntity.SIMPLEBORROWMONEY.Value;
                            repay.CREATEDATE = DateTime.Now;
                            repay.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            repay.REMARK = "";
                            repay.CHARGEAPPLYREPAYDETAILID = System.Guid.NewGuid().ToString();
                            repay.REPAYMONEY = 0;
                            repay.REPAYTYPE = 1;
                            repay.UPDATEDATE = DateTime.Now;
                            repay.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            chaRepDtlobj.Add(repay);
                        }
                        if (PerEntity.BACKUPBORROWMONEY > 0)
                        {
                            T_FB_CHARGEAPPLYREPAYDETAIL repay = new T_FB_CHARGEAPPLYREPAYDETAIL();
                            repay.BORROWMONEY = PerEntity.BACKUPBORROWMONEY.Value;
                            repay.CREATEDATE = DateTime.Now;
                            repay.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            repay.REMARK = "";
                            repay.CHARGEAPPLYREPAYDETAILID = System.Guid.NewGuid().ToString();
                            repay.REPAYMONEY = 0;
                            repay.REPAYTYPE = 2;
                            repay.UPDATEDATE = DateTime.Now;
                            repay.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            chaRepDtlobj.Add(repay);
                        }
                        if (PerEntity.SPECIALBORROWMONEY > 0)
                        {
                            T_FB_CHARGEAPPLYREPAYDETAIL repay = new T_FB_CHARGEAPPLYREPAYDETAIL();
                            repay.BORROWMONEY = PerEntity.SPECIALBORROWMONEY.Value;
                            repay.CREATEDATE = DateTime.Now;
                            repay.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            repay.REMARK = "";
                            repay.CHARGEAPPLYREPAYDETAILID = System.Guid.NewGuid().ToString();
                            repay.REPAYMONEY = 0;
                            repay.REPAYTYPE = 3;
                            repay.UPDATEDATE = DateTime.Now;
                            repay.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                            chaRepDtlobj.Add(repay);
                        }
                    }
                    dgvRepayDetailList.ItemsSource = chaRepDtlobj;
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), ex.ToString());
            }
        }

        private void AddSub_Click(object sender, RoutedEventArgs e)
        {
            List<object> subjectLst = new List<object>();
            frmU = new SMT.FBAnalysis.UI.CommonForm.SubjectApp_sel(strOwnerID, strOwnerPostID, strOwnerDepartmentID, strOwnerCompanyID, subjectLst);
            EntityBrowser browser = new EntityBrowser(frmU);
            browser.MinHeight = 400;
            browser.MinWidth = 500;
            browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
            browser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, strOwnerID);
        }

        void browser_ReloadDataEvent()
        {
            if (frmU._lstSubjectApply_Add == null)
            {
                Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("CAUTION"), "请选择要报销的科目");
                return;
            }

            budgetList = frmU._lstSubjectApply_Add;

            foreach (T_FB_BUDGETACCOUNT obj in budgetList)
            {
                T_FB_CHARGEAPPLYDETAIL chaent = new T_FB_CHARGEAPPLYDETAIL();
                chaent.CHARGEAPPLYDETAILID = System.Guid.NewGuid().ToString();

                if (obj.ACCOUNTOBJECTTYPE == 3)         //个人费用报销
                {
                    chaent.CHARGETYPE = 1;
                }
                else if (obj.ACCOUNTOBJECTTYPE == 2)    //部门费用报销
                {
                    chaent.CHARGETYPE = 2;
                }

                chaent.CREATEDATE = DateTime.Now;
                chaent.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                chaent.T_FB_SUBJECT = obj.T_FB_SUBJECT;
                chaent.UPDATEDATE = DateTime.Now;
                chaent.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;

                chaent.USABLEMONEY = obj.USABLEMONEY;      //据万要求显示可用结余
                //chaent.USABLEMONEY = obj.ACTUALMONEY;    //据万要求显示实际结余
                chaDtlobj.Add(chaent);
            }
            dgvChargeDetailList.ItemsSource = chaDtlobj;

        }

        //表格行加载删除按钮
        private void dg_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            T_FB_CHARGEAPPLYDETAIL tmp = (T_FB_CHARGEAPPLYDETAIL)e.Row.DataContext;
            ImageButton MyButton_Delbaodao = dgvChargeDetailList.Columns[6].GetCellContent(e.Row).FindName("myDelete") as ImageButton;
            MyButton_Delbaodao.Margin = new Thickness(0);
            MyButton_Delbaodao.AddButtonAction("/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/ico_16_delete.png", Utility.GetResourceStr("DELETE"));
            MyButton_Delbaodao.Tag = tmp;
        }

        //删除表格中的某一行
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            T_FB_CHARGEAPPLYDETAIL i = ((Button)sender).DataContext as T_FB_CHARGEAPPLYDETAIL;
            if (chaDtlobj.Contains(i))
            {
                chaDtlobj.Remove(i);
            }
            dgvChargeDetailList.ItemsSource = chaDtlobj;
            txtMon_KeyUp(null, null);
        }


        /// <summary>
        /// 明细内输入指定科目报销金额，进行校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMon_KeyUp(object sender, KeyEventArgs e)
        {
            decimal dMoney = 0;
            foreach (object obj in dgvChargeDetailList.ItemsSource)
            {
                if (dgvChargeDetailList.Columns[5].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvChargeDetailList.Columns[5].GetCellContent(obj).FindName("txtMon") as TextBox;
                    if (!string.IsNullOrWhiteSpace(tbMon.Text) && tbMon.Text.IndexOf('-') != -1)
                    {
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "不能输入负数！");
                        tbMon.Text = "0";
                        break;
                    }

                    decimal dDetailMon = 0;
                    T_FB_CHARGEAPPLYDETAIL ent = obj as T_FB_CHARGEAPPLYDETAIL;
                    decimal.TryParse(tbMon.Text, out dDetailMon);
                    ent.CHARGEMONEY = dDetailMon;
                    if (ChargeMasterEntity.PAYTYPE == 2 && ent.T_FB_BORROWAPPLYDETAIL != null)
                    {
                        if (dDetailMon > ent.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY)
                        {
                            ent.REPAYMONEY = ent.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY;
                        }
                        else
                        {
                            ent.REPAYMONEY = dDetailMon;
                        }
                    }

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
            CalculateCharge();
        }

        /// <summary>
        /// 计算费用报销合计
        /// </summary>
        private void CalculateCharge()
        {
            decimal dMoney = 0;
            foreach (object obj in dgvChargeDetailList.ItemsSource)
            {
                if (dgvChargeDetailList.Columns[5].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvChargeDetailList.Columns[5].GetCellContent(obj).FindName("txtMon") as TextBox;
                    if (!string.IsNullOrWhiteSpace(tbMon.Text) && tbMon.Text.IndexOf('-') != -1)
                    {
                        Utility.ShowCustomMessage(MessageTypes.Caution, Utility.GetResourceStr("TIPS"), "不能输入负数！");
                        tbMon.Text = "0";
                        break;
                    }

                    decimal dDetailMon = 0;
                    T_FB_CHARGEAPPLYDETAIL ent = obj as T_FB_CHARGEAPPLYDETAIL;
                    decimal.TryParse(tbMon.Text, out dDetailMon);
                    ent.CHARGEMONEY = dDetailMon;
                    if (ChargeMasterEntity.PAYTYPE == 2 && ent.T_FB_BORROWAPPLYDETAIL != null)
                    {
                        if (dDetailMon > ent.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY)
                        {
                            ent.REPAYMONEY = ent.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY;
                        }
                        else
                        {
                            ent.REPAYMONEY = dDetailMon;
                        }
                    }

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

        private void txtRepMon_KeyUp(object sender, KeyEventArgs e)
        {
            decimal dMoney = 0;
            foreach (object obj in dgvRepayDetailList.ItemsSource)
            {
                if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtRepMon") as TextBox;
                    decimal dDetailMon = 0;
                    decimal.TryParse(tbMon.Text, out dDetailMon);
                    dMoney = dMoney + dDetailMon;
                }
            }
            txtTolRepayMon.Text = dMoney.ToString();
        }

        private void txtRepMon_LostFocus(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// 算还款合计
        /// </summary>
        private void CalculateRep()
        {
            decimal dMoney = 0;
            foreach (object obj in dgvRepayDetailList.ItemsSource)
            {
                if (dgvRepayDetailList.Columns[3].GetCellContent(obj) != null)
                {
                    TextBox tbMon = dgvRepayDetailList.Columns[3].GetCellContent(obj).FindName("txtRepMon") as TextBox;
                    decimal dDetailMon = 0;
                    decimal.TryParse(tbMon.Text, out dDetailMon);
                    dMoney = dMoney + dDetailMon;
                }
            }
            txtTolRepayMon.Text = dMoney.ToString();
        }

        private void hypExten_Click(object sender, RoutedEventArgs e)
        {
            if (exten != null && exten.T_FB_EXTENSIONALTYPE != null)
            {
                SMT.FBAnalysis.UI.Common.Utility.ShowExtenForm(exten.T_FB_EXTENSIONALTYPE.EXTENSIONALTYPECODE, exten.ORDERID);
            }
        }

        #endregion

        //string TempStr = string.Empty;//存放3行内信息
        //private void txtPayInfo_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    string TempText = string.Empty;
        //    string text = this.txtPayInfo.Text;
        //    string[] arrary = text.Split('\r');
        //    var tempArray = arrary.Reverse().ToList();
        //    int count = tempArray.Count();
        //    if (count <= 3)
        //    {
        //        TempStr = text;
        //    }
        //    else
        //    {
        //        this.txtPayInfo.Text = TempStr;//超过3行数据则赋值为保存的3行内数据
        //    }
        //}

        //private void rbOther_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.rbOther.IsChecked.Value)
        //    {
        //        this.wtOther.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        this.wtOther.Visibility = Visibility.Collapsed;
        //    }
        //}

        //private void cbSelf_Click(object sender, RoutedEventArgs e)
        //{
        //    if (this.cbSelf.IsChecked.Value)
        //    {
        //        this.wtSelf.Visibility = Visibility.Visible;
        //    }
        //    else
        //    {
        //        this.wtSelf.Visibility = Visibility.Collapsed;
        //    }
        //}

        #region 删除
        void client_DelChargeApplyMasterAndDetailCompleted(object sender, DelChargeApplyMasterAndDetailCompletedEventArgs e)
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
        #endregion

        #region ２０１４.及时更新
        //public void RefreshDetails()
        //{
        //    if (e.Result != null)
        //    {
        //        List<T_FB_CHARGEAPPLYDETAIL> chargeDetailList = e.Result.ToList();
        //        chaDtlobj = e.Result;
        //        dgvChargeDetailList.ItemsSource = chargeDetailList;
        //        dgvChargeDetailList.Loaded += new RoutedEventHandler(dgvChargeDetailList_Loaded);
        //    }
        //}

        // 支付信息
        private void RefreshPayInfo()
        {
            switch (nChargeType)
            {
                case 1:
                    payInfoA.Visibility = System.Windows.Visibility.Visible;
                    payInfoC.Visibility = System.Windows.Visibility.Visible;
                    tblRepMon.Visibility = Visibility.Collapsed;
                    txtTolRepayMon.Visibility = Visibility.Collapsed;
                    dgvRepayDetailList.IsEnabled = false;
                    dgvRepayDetailList.Visibility = Visibility.Collapsed;
                    chaRepDtlobj.Clear();
                    tbWaterMark.Text = "汇本人账户或其他账户（户名，开户行，账号），不支持Enter换行";
                    break;
                case 2:
                     payInfoA.Visibility = System.Windows.Visibility.Collapsed;
                    payInfoC.Visibility = System.Windows.Visibility.Collapsed;
                    tbWaterMark.Text = "冲xx单号借款，剩余金额个人报销时填写款汇xx账户，不支持Enter换行";
                    tblRepMon.Visibility = Visibility.Visible;
                    txtTolRepayMon.Visibility = Visibility.Visible;
                    
                    dgvRepayDetailList.IsEnabled = true;
                    dgvRepayDetailList.Visibility = Visibility.Visible;
                    break;
                case 3:
                    payInfoA.Visibility = System.Windows.Visibility.Collapsed;
                    payInfoC.Visibility = System.Windows.Visibility.Collapsed;
                    tblRepMon.Visibility = Visibility.Collapsed;
                    txtTolRepayMon.Visibility = Visibility.Collapsed;
                    dgvRepayDetailList.IsEnabled = false;
                    dgvRepayDetailList.Visibility = Visibility.Collapsed;
                    tbWaterMark.Text = "银行已支付或其他支付信息，不支持Enter换行";
                    chaRepDtlobj.Clear();
                    break;
                case 4:
                    payInfoA.Visibility = System.Windows.Visibility.Collapsed;
                    payInfoC.Visibility = System.Windows.Visibility.Visible;
                    tblRepMon.Visibility = Visibility.Collapsed;
                    txtTolRepayMon.Visibility = Visibility.Collapsed;
                    dgvRepayDetailList.IsEnabled = false;
                    dgvRepayDetailList.Visibility = Visibility.Collapsed;
                    chaRepDtlobj.Clear();
                    this.tbWaterMark.Text = "客户的户名，开户行和账号信息，不支持Enter换行";
                    break;
                case 5:
                     payInfoA.Visibility = System.Windows.Visibility.Collapsed;
                    payInfoC.Visibility = System.Windows.Visibility.Collapsed;
                    tblRepMon.Visibility = Visibility.Collapsed;
                    txtTolRepayMon.Visibility = Visibility.Collapsed;
                    dgvRepayDetailList.IsEnabled = false;
                    dgvRepayDetailList.Visibility = Visibility.Collapsed;
                    this.tbWaterMark.Text = "支付说明，或款付xx人账户或户名，开户行和账号信息，不支持Enter换行";
                    chaRepDtlobj.Clear();
                    break;
            }
            
            
        }

        private void SetPayInfo()
        {
            nChargeType = Convert.ToInt32(ChargeMasterEntity.PAYTYPE);
            (stpPaytype.Children[nChargeType - 1] as RadioButton).IsChecked = true;

            var payInfo = ChargeMasterEntity.PAYMENTINFO ?? "";
            if (payInfo.StartsWith(PayMySelf))
            {
                this.rbPayMySelft.IsChecked = true;
                payInfo = payInfo.Replace(PayMySelf, "");
            }
            else
            {
                this.rbPayOther.IsChecked = true;
            }

            if (payInfo.StartsWith(PayFromA))
            {
                this.rbPayFromA.IsChecked = true;
                payInfo = payInfo.Replace(PayFromA, "");
            }
            else if (payInfo.StartsWith(PayFromB))
            {
                payInfo = payInfo.Replace(PayFromB, "");
                this.rbPayFromB.IsChecked = true;
            }

            this.payInfoB.Text = payInfo;

            RefreshPayInfo();
        }

        private string GetPayInfo()
        {
            
            var result = string.Empty;
            if ( this.payInfoA.Visibility == System.Windows.Visibility.Visible && this.rbPayMySelft.IsChecked.Value)
            {
                result += PayMySelf;
            }
            if ( this.payInfoC.Visibility == System.Windows.Visibility.Visible
                && this.rbPayFromA.Visibility == System.Windows.Visibility.Visible)
            {
                result += this.rbPayFromA.IsChecked.Value ? PayFromA : PayFromB;
            }
            result += this.payInfoB.Text;
            return result;
        }
        #endregion

        private void hypItemCode_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtItemID.Text))
            {
                try
                {
                    HtmlPage.Window.Invoke("SLPopPage", "ApprovalInfo", txtItemID.Text, "查看事项审批单");
                }
                catch(Exception ex)
                {

                }
            }
        }


    }
}
