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
using SMT.SaaS.OA.FAMPerCellServices;
using SMT.SAAS.Main.CurrentContext;
using SMT.SaaS.OA.Core.Converter;
using SMT.SaaS.OA.Class;
using SMT.SaaS.OA.Core;
using SMT.SaaS.OA.CommForm;
using SMT.SaaS.OA.FAMSingleService;
using SMT.Saas.Tools.PersonnelWS;
using SMT.Saas.Tools.PermissionWS;
namespace SMT.SaaS.OA.UserControls.AssetManagement
{
    public partial class AssetsMaintainControl : UserControl, IEntityEditor
    {
        #region 全局变量

        private FAMPerCellServices.AssetsMaintainClient assetMaintainClient;
        private RefreshedTypes refreshType = RefreshedTypes.CloseAndReloadData;
        private SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult auditResult;   //审批结果
        private DataActionFlag actionFlag = DataActionFlag.Normal;
        private T_FAM_ASSETSMAINTAIN assetMaintain = new T_FAM_ASSETSMAINTAIN();
        private Action actions;
        private SMT.Saas.Tools.FBServiceWS.T_FB_EXTENSIONALORDER order = new SMT.Saas.Tools.FBServiceWS.T_FB_EXTENSIONALORDER();//预算控件
        #endregion

        #region 初始化
        public AssetsMaintainControl(Action action, T_FAM_ASSETSMAINTAIN AssetObj)
        {
            InitializeComponent();
            actions = action;
            InitEvent();
            audit.Visibility = Visibility.Visible;
            if (action == Action.Add)//新增时隐藏审核控件
            {

                txtMaintainedID.Text = Utility.GetResourceStr("AUTOGENERATION");
                this.audit.Visibility = Visibility.Collapsed;
            }
            if (action == Action.Edit || action == Action.AUDIT
               || action == Action.Read)
            {
                assetMaintain = AssetObj;
                GetMaintainData(AssetObj);
                audit.XmlObject = DataObjectToXml<T_FAM_ASSETSMAINTAIN>.ObjListToXml(assetMaintain, "OA");
            }
            if (action == Action.Read || action == Action.AUDIT)
            {

                txtMaintainedID.IsEnabled = false;
                txtMaintainedName.IsEnabled = false;
                txtAssetsID.IsEnabled = false;
                txtSpecification.IsEnabled = false;
                txtUnitID.IsEnabled = false;
                txtUsedDepartment.IsEnabled = false;
                txtMaintainedReason.IsEnabled = false;
                txtMaintainedDate.IsEnabled = false;
                txtMaintainDayCount.IsEnabled = false;
                txtConstructionUnit.IsEnabled = false;
                txtMaintainMaterial.IsEnabled = false;
                txtMaintainBudget.IsEnabled = false;
                txtRemark.IsEnabled = false;
                cmbMaintainedLevel.IsEnabled = false;
                btnLookFixedAsset.IsEnabled = false;
            }
            InitFBControl();
        }
        public AssetsMaintainControl(Action action, string strMaintainID)
        {
            try
            {
                InitializeComponent();
                actions = action;
                InitEvent();
                if (cmbMaintainedLevel.Items.Count > 0)
                {
                    cmbMaintainedLevel.SelectedIndex = 0;
                }

                audit.Visibility = Visibility.Visible;
                if (action == Action.Add)//新增时隐藏审核控件
                {
                    txtMaintainedID.Text = Utility.GetResourceStr("AUTOGENERATION");
                    this.audit.Visibility = Visibility.Collapsed;
                }
                if (action == Action.Edit || action == Action.AUDIT
                   || action == Action.Read)
                {
                    GetMaintain(strMaintainID);
                }
                if (action == Action.Read || action == Action.AUDIT)
                {

                    txtMaintainedID.IsEnabled = false;
                    txtMaintainedName.IsEnabled = false;
                    txtAssetsID.IsEnabled = false;
                    txtSpecification.IsEnabled = false;
                    txtUnitID.IsEnabled = false;
                    txtUsedDepartment.IsEnabled = false;
                    txtMaintainedReason.IsEnabled = false;
                    txtMaintainedDate.IsEnabled = false;
                    txtMaintainDayCount.IsEnabled = false;
                    txtConstructionUnit.IsEnabled = false;
                    txtMaintainMaterial.IsEnabled = false;
                    txtMaintainBudget.IsEnabled = false;
                    txtRemark.IsEnabled = false;
                    cmbMaintainedLevel.IsEnabled = false;
                    btnLookFixedAsset.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
        private void InitEvent()
        {
            this.txtMaintainedDate.Text = DateTime.Now.ToShortDateString(); //设置维修日期，默认为当前日期。
            audit.AuditCompleted += new EventHandler<SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs>(audit_AuditCompleted);
            assetMaintainClient = new AssetsMaintainClient();
            assetMaintainClient.AddAssetsMaintainCompleted += new EventHandler<AddAssetsMaintainCompletedEventArgs>(assetMaintainClient_AddAssetsMaintainCompleted);
            assetMaintainClient.UpdateAssetsMaintainCompleted += new EventHandler<UpdateAssetsMaintainCompletedEventArgs>(assetMaintainClient_UpdateAssetsMaintainCompleted);
            assetMaintainClient.GetAssetsMaintainCompleted += new EventHandler<GetAssetsMaintainCompletedEventArgs>(assetMaintainClient_GetAssetsMaintainCompleted);
            cmbMaintainedLevel.SelectedIndex = 0;
        }
        private void GetMaintain(string strMaintainID)
        {
            //loadbar.Start();//打开转动动画
            //RefreshUI(RefreshedTypes.ProgressBar);//点击保存后显示进度条
            assetMaintainClient.GetAssetsMaintainAsync(strMaintainID);
        }

        #region WCF完成事件
        void assetMaintainClient_AddAssetsMaintainCompleted(object sender, AddAssetsMaintainCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (!e.Result)
                    {
                        //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Result));
                        RefreshUI(RefreshedTypes.ProgressBar);//数据完成后隐藏进度条
                    }
                    else
                    {
                        assetMaintain.MAINTAINEDID = e.ID;

                        if (actionFlag == DataActionFlag.SubmitFlow)
                        {
                            actionFlag = DataActionFlag.SubmitComplete;
                            SumbitFlow();
                        }
                        else
                        {
                            if (actionFlag == DataActionFlag.SubmitComplete)
                            {
                                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("AUDITSUCCESSED", "ASSETSMAINTAIN"));
                            }
                            else
                            {
                                fbCtr.Order.ORDERID = assetMaintain.MAINTAINEDID;
                                fbCtr.Save(SMT.SaaS.FrameworkUI.CheckStates.UnSubmit);//提交费用
                                if (GlobalFunction.IsSaveAndClose(refreshType))
                                {
                                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("ADDSUCCESSED", "ASSETSMAINTAIN"));
                                }
                                else
                                {
                                    this.actions = Action.Edit;
                                }

                                if (refreshType == RefreshedTypes.All)
                                {
                                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("ADDSUCCESSED", "ASSETSMAINTAIN"));
                                }
                                else
                                {
                                    this.actions = Action.Edit;
                                }
                            }
                            RefreshUI(refreshType);
                          
                        }
                    }
                }
                else
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
                }
            }
            catch (Exception ex)
            {   
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.ToString()));
            }

            RefreshUI(RefreshedTypes.ProgressBar);

        }
        void assetMaintainClient_UpdateAssetsMaintainCompleted(object sender, UpdateAssetsMaintainCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (!e.Result)
                    {
                        RefreshUI(RefreshedTypes.ProgressBar);
                        //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Result));
                    }
                    else
                    {
                        if (actionFlag == DataActionFlag.SubmitFlow)
                        {
                            actionFlag = DataActionFlag.SubmitComplete;
                            SumbitFlow();
                        }
                        else
                        {
                            fbCtr.Order.ORDERID = assetMaintain.MAINTAINEDID;
                            fbCtr.Save(SMT.SaaS.FrameworkUI.CheckStates.UnSubmit);//提交费用
                            if (actionFlag == DataActionFlag.SubmitComplete)
                            {
                                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("AUDITSUCCESSED", "ASSETSMAINTAIN"));
                                RefreshUI(RefreshedTypes.ProgressBar);//数据完成后隐藏进度条
                            }
                            else
                            {
                                if (GlobalFunction.IsSaveAndClose(refreshType))
                                {
                                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("UPDATESUCCESSED", "ASSETSMAINTAIN"));
                                }

                                if (refreshType == RefreshedTypes.All)
                                {
                                    Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("UPDATESUCCESSED", "ASSETSMAINTAIN"));
                                }
                            }
                            RefreshUI(refreshType);
                        }
                    }
                }
                else
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message));
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.ToString()));
            }

            RefreshUI(RefreshedTypes.ProgressBar);//数据完成后隐藏进度条

        }
        void assetMaintainClient_GetAssetsMaintainCompleted(object sender, GetAssetsMaintainCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result != null)
                {
                    assetMaintain = e.Result;
                    GetMaintainData(e.Result);
                    audit.XmlObject = DataObjectToXml<T_FAM_ASSETSMAINTAIN>.ObjListToXml(assetMaintain, "OA");
                }
            }
            // RefreshUI(RefreshedTypes.ProgressBar);
        }
        #endregion
        #region GetAssetPurchaseData
        private void GetMaintainData(T_FAM_ASSETSMAINTAIN assetMaintainData)
        {
            if (assetMaintainData != null)
            {

                txtMaintainedID.Text = assetMaintainData.MAINTAINEDID.CvtString();
                txtMaintainedName.Text = assetMaintainData.MAINTAINEDNAME.CvtString();
                txtAssetsID.Text = assetMaintainData.ASSETSID.CvtString();
                txtAssetsName.Text = assetMaintainData.ASSETSNAME.CvtString();
                txtSpecification.Text = assetMaintainData.SPECIFICATION.CvtString();
                txtUnitID.Text = assetMaintainData.UNITID.CvtString();
                txtUsedDepartment.Text = Utility.GetDepartmentName(assetMaintainData.USEDDEPARTMENT);
                txtMaintainedReason.Text = assetMaintainData.MAINTAINEDREASON.CvtString();
                txtMaintainedDate.Text = assetMaintainData.MAINTAINEDDATE.CvtString();
                txtMaintainDayCount.Text = assetMaintainData.MAINTAINDAYCOUNT.CvtString();
                txtConstructionUnit.Text = assetMaintainData.CONSTRUCTIONUNIT.CvtString();
                txtMaintainMaterial.Text = assetMaintainData.MAINTAINMATERIAL.CvtString();
                txtMaintainBudget.Text = assetMaintainData.MAINTAINBUDGET.CvtString();
                txtRemark.Text = assetMaintainData.REMARK.CvtString();

                if (!string.IsNullOrEmpty(assetMaintainData.MAINTAINEDLEVEL.CvtString()))
                {
                    T_SYS_DICTIONARY StrMaintainedLevel = cmbMaintainedLevel.SelectedItem as T_SYS_DICTIONARY;
                    foreach (T_SYS_DICTIONARY Level in cmbMaintainedLevel.Items)
                    {
                        if (Level.DICTIONARYVALUE == assetMaintain.MAINTAINEDLEVEL.CvtDecimal())
                        {
                            cmbMaintainedLevel.SelectedItem = Level;
                            break;
                        }
                    }
                }
                // assetMaintainData.MAINTAINEDLEVEL = (cmbMaintainedLevel.SelectedItem as Level).LevelID=
            }
            InitAudit(assetMaintainData);
        }
        #endregion



        #region 部门名转换
        public string DepartmentName(string departmentid)
        {
            List<T_HR_DEPARTMENT> dictd = Application.Current.Resources["SYS_DepartmentInfo"] as List<T_HR_DEPARTMENT>;
            if (dictd == null)
                return departmentid;
            var objd = from a in dictd
                       where a.DEPARTMENTID == departmentid
                       select a.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
            return objd.Count() > 0 ? objd.FirstOrDefault() : departmentid;
        }
        #endregion

        #region 保存函数
        private void Save()
        {
            try
            {
                if (Check())
                {

                    T_SYS_DICTIONARY StrMaintainedLevel = cmbMaintainedLevel.SelectedItem as T_SYS_DICTIONARY;//维修级别
                    RefreshUI(RefreshedTypes.ProgressBar);//点击保存后显示进度条
                    // T_FAM_ASSETSMAINTAIN assetMaintainData = new T_FAM_ASSETSMAINTAIN();
                    assetMaintain.MAINTAINEDNAME = txtMaintainedName.Text;
                    assetMaintain.ASSETSID = txtAssetsID.Text;
                    assetMaintain.ASSETSNAME = txtAssetsName.Text;
                    assetMaintain.SPECIFICATION = txtSpecification.Text;
                    assetMaintain.UNITID = txtUnitID.Text;
                    assetMaintain.USEDDEPARTMENT = txtUsedDepartment.Text;
                    assetMaintain.MAINTAINEDREASON = txtMaintainedReason.Text;
                    assetMaintain.MAINTAINEDLEVEL = StrMaintainedLevel.DICTIONARYVALUE.CvtString();//维修级别
                    assetMaintain.MAINTAINEDDATE = txtMaintainedDate.Text;
                    assetMaintain.MAINTAINDAYCOUNT = txtMaintainDayCount.Text;
                    assetMaintain.CONSTRUCTIONUNIT = txtConstructionUnit.Text;
                    assetMaintain.MAINTAINMATERIAL = txtMaintainMaterial.Text;
                    assetMaintain.MAINTAINBUDGET = txtMaintainBudget.Text;
                    assetMaintain.REMARK = txtRemark.Text;

                    assetMaintain.GUID = Guid.NewGuid().ToString();
                    assetMaintain.CHECKSTATE = "0";
                    assetMaintain.CREATEUSERID = Common.loginUserInfo.userID;
                    assetMaintain.CREATEDATE = "";
                    assetMaintain.CREATETIME = "";
                    assetMaintain.UPDATEDATE = "";
                    assetMaintain.UPDATETIME = "";
                    assetMaintain.OWNERID = Common.loginUserInfo.userID;
                    assetMaintain.OWNERNAME = Common.loginUserInfo.userName;
                    assetMaintain.OWNERCOMPANYID = Common.loginUserInfo.companyID;
                    assetMaintain.OWNERDEPARTMENTID = Common.loginUserInfo.departmentID;
                    assetMaintain.OWNERPOSTID = Common.loginUserInfo.postID;
                    assetMaintain.CREATEUSERNAME = Common.loginUserInfo.userName;
                    assetMaintain.CREATECOMPANYID = Common.loginUserInfo.companyID;
                    assetMaintain.CREATEDEPARTMENTID = Common.loginUserInfo.departmentID;
                    assetMaintain.CREATEPOSTID = Common.loginUserInfo.postID;
                    assetMaintain.MAINTAINBUDGET = fbCtr.Order.TOTALMONEY.CvtString();//费用;

                    CustomPerfix perfix = new CustomPerfix();
                    perfix.PrefixTypeId = "FixedAsset";
                    perfix.PrefixId = "MN";
                    if (actions == Action.Add)
                    {
                        assetMaintain.MAINTAINEDID = txtMaintainedID.Text;
                        string MaintainID = string.Empty;
                        assetMaintain.CHECKSTATE = "0";
                        assetMaintainClient.AddAssetsMaintainAsync(assetMaintain, perfix, MaintainID);
                    }
                    else
                    {
                        assetMaintainClient.UpdateAssetsMaintainAsync(assetMaintain);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.Message));
            }
        }
        #endregion

        #region 验证
        private bool Check()
        {

            System.Text.RegularExpressions.Regex reg
               = new System.Text.RegularExpressions.Regex(@"^[1-9][0-9]*$");
            //if (!reg.IsMatch(txt))
            //{
            //    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("PurchaseQuantityIsEmptyOrLessThan0", ""));
            //    //RefreshUI(RefreshedTypes.ProgressBar);//关闭进度条动画
            //    return false;
            //}
            if (string.IsNullOrEmpty(txtMaintainedName.Text))//维修单名称
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "MAINTAINEDNAME"));
                return false;
            }

            if (string.IsNullOrEmpty(txtAssetsID.Text))//资产编号
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "ASSETID"));
                return false;
            }

            if (string.IsNullOrEmpty(txtUnitID.Text))//单位
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "UNITID"));
                return false;
            }

            if (string.IsNullOrEmpty(txtUsedDepartment.Text))//使用部门
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "USEDEPT"));
                return false;
            }

            if (string.IsNullOrEmpty(txtMaintainedReason.Text))//维修原因
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "MAINTAINEDREASON"));
                return false;
            }

            if (cmbMaintainedLevel.SelectedIndex<=0)//维修级别
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "MAINTAINEDLEVEL"));
                return false;
            }

            if (string.IsNullOrEmpty(txtMaintainedDate.Text))//维修时间
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "MAINTAINEDDATE"));
                return false;
            }
            else
            {
                if (!txtMaintainedDate.Text.IsDateTime())
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("DATETIMEERROR", "MAINTAINEDDATE"));
                    return false;
                }
            }

            if (string.IsNullOrEmpty(txtMaintainDayCount.Text))//维修天数
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "MAINTAINDAYCOUNT"));
                return false;
            }
            else
            {
                if (!txtMaintainDayCount.Text.IsNumber())
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("NUMBERERROR", "MAINTAINDAYCOUNT"));
                    return false;
                }

                if (txtMaintainDayCount.Text.CvtDouble() <= 0) 
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("NOTLESSTHANZERO", "MAINTAINDAYCOUNT"));
                    return false;
                }
            }

            if (string.IsNullOrEmpty(txtConstructionUnit.Text))//施工单位
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "CONSTRUCTIONUNIT"));
                return false;
            }

            if (string.IsNullOrEmpty(txtMaintainMaterial.Text))//维修材料
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "MAINTAINMATERIAL"));
                return false;
            }

            if (string.IsNullOrEmpty(txtMaintainBudget.Text))//维修预算
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("STRINGNOTNULL", "MAINTAINBUDGET"));
                return false;
            }
            List<SMT.SaaS.FrameworkUI.Validator.ValidatorBase> validators = Group1.ValidateAll();
            if (validators.Count > 0)
            {
                foreach (var h in validators)
                {
                    Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(h.ErrorMessage));
                    return false;
                }
                //RefreshUI(RefreshedTypes.ProgressBar);//关闭进度条动画
            }
            return true;
        }
        #endregion

        #region 提交审批
        private void SubmitAuditToClose()
        {
            actionFlag = DataActionFlag.SubmitFlow;
            Save();
        }
        #endregion

        #region IEntityEditor 成员

        public string GetTitle()
        {
            if (actions == Action.Add)
            {
                return Utility.GetResourceStr("ADDTITLE", "ASSETSMAINTAIN");//新增
            }
            else if (actions == Action.Edit)
            {
                return Utility.GetResourceStr("EDITTITLE", "ASSETSMAINTAIN");//修改
            }
            else if (actions == Action.Read)
            {
                return Utility.GetResourceStr("VIEW", "ASSETSMAINTAIN");//查看
            }
            else
            {
                return Utility.GetResourceStr("AUDIT", "ASSETSMAINTAIN");//审核
            }
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
                    refreshType = RefreshedTypes.All;
                    Save();
                    break;
                case "1":
                    refreshType = RefreshedTypes.CloseAndReloadData;
                    Save();
                    break;
                case "2":
                    refreshType = RefreshedTypes.CloseAndReloadData;
                    SubmitAuditToClose();
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

            if (actions != Action.Read)
            {
                if (actions != Action.AUDIT)
                {
                
                ToolbarItem item = new ToolbarItem
                {
                    DisplayType = ToolbarItemDisplayTypes.Image,
                    Key = "2",
                    Title = Utility.GetResourceStr("SUBMITAUDIT"),
                    ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/18_audit.png"
                };
                items.Add(item);
                
                    item = new ToolbarItem
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
                        ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_save.png"
                    };
                    items.Add(item);
                }
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
        #endregion

        #region 申购人
        private void btnLookUpUserName_Click(object sender, RoutedEventArgs e)
        {
            SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup lookup = new SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup();
            lookup.SelectedObjType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Personnel;
            lookup.SelectedClick += (obj, ev) =>
            {
                List<SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj> ent = lookup.SelectedObj as List<SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj>;
                if (ent != null && ent.Count > 0)
                {
                    SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj companyInfo = ent.FirstOrDefault();
                    // assetMaintain.APPLYUSER = companyInfo.ObjectID;//申购人Id
                    //txtUserName.Text = companyInfo.ObjectName;//申购人姓名
                }
            };
            lookup.MultiSelected = true;
            lookup.Show();
        }
        #endregion

        #region 申请部门
        private void btnLookDepartment_Click(object sender, RoutedEventArgs e)
        {
            SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup lookup = new SMT.SaaS.FrameworkUI.OrganizationControl.OrganizationLookup();
            lookup.SelectedObjType = SMT.SaaS.FrameworkUI.OrgTreeItemTypes.Department;
            lookup.SelectedClick += (obj, ev) =>
            {
                List<SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj> ent = lookup.SelectedObj as List<SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj>;
                if (ent != null && ent.Count > 0)
                {
                    SMT.SaaS.FrameworkUI.OrganizationControl.ExtOrgObj companyInfo = ent.FirstOrDefault();
                    assetMaintain.USEDDEPARTMENT = companyInfo.ObjectID;//申购部门ID
                    txtUsedDepartment.Text = companyInfo.ObjectName;//申购部门名
                }
            };
            lookup.MultiSelected = true;
            lookup.Show();
        }
        #endregion

        #region 审批流程
        /// <summary>
        /// 提交审核完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void audit_AuditCompleted(object sender, SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs e)
        {
            auditResult = e.Result;
            switch (auditResult)
            {
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Auditing:
                    //todo 审核中
                    SumbitCompleted();
                    break;
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Cancel:
                    //todo 取消
                    Cancel();
                    break;
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Successful:
                    //todo 终审通过
                    SumbitCompleted();
                    break;
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Fail:
                    //todo 审核不通过
                    SumbitCompleted();
                    break;
                case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Error:
                    //todo 审核异常
                    HandError();
                    break;
            }
        }
        private void Cancel()
        {
            RefreshUI(RefreshedTypes.Close);
        }
        private void HandError()
        {
            Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("AUDITFAILURE"));
            RefreshUI(RefreshedTypes.CloseAndReloadData);
        }
        private void SumbitCompleted()
        {
            try
            {
                if (assetMaintain != null)
                {
                    assetMaintain.UPDATEDATE = DateTime.Now.ToShortDateString();
                    assetMaintain.UPDATEUSERID = Common.loginUserInfo.userID;
                    switch (auditResult)
                    {
                        case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Auditing://审核中
                            assetMaintain.CHECKSTATE = Utility.GetCheckState(CheckStates.Approving);
                            fbCtr.Order.ORDERID = assetMaintain.MAINTAINEDID;
                            fbCtr.Save(SMT.SaaS.FrameworkUI.CheckStates.Approving); //审核中
                            break;
                        case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Successful://审核通过
                            assetMaintain.CHECKSTATE = Utility.GetCheckState(CheckStates.Approved);
                            fbCtr.Order.ORDERID = assetMaintain.MAINTAINEDID;
                            fbCtr.Save(SMT.SaaS.FrameworkUI.CheckStates.Approved); //审核通过
                            break;
                        case SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs.AuditResult.Fail://审核未通过
                            assetMaintain.CHECKSTATE = Utility.GetCheckState(CheckStates.UnApproved);
                            fbCtr.Order.ORDERID = assetMaintain.MAINTAINEDID;
                            fbCtr.Save(SMT.SaaS.FrameworkUI.CheckStates.UnApproved); //审核不通过 
                            break;
                    }
                    assetMaintainClient.UpdateAssetsMaintainAsync(assetMaintain);
                }
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.ToString()));
            }
        }
        #region 提交流程
        /// <summary>
        /// 提交流程
        /// </summary>
        private void SumbitFlow()
        {
            if (assetMaintain != null)
            {
                SMT.SaaS.FrameworkUI.AuditControl.Flow_FlowRecord_T entity = this.audit.AuditEntity;
                entity.ModelCode = "T_FAM_ASSETSMAINTAIN";
                entity.FormID = assetMaintain.MAINTAINEDID;
                entity.CreateCompanyID = Common.loginUserInfo.companyID;
                entity.CreateDepartmentID = Common.loginUserInfo.departmentID;
                entity.CreatePostID = Common.loginUserInfo.postID;
                entity.CreateUserID = Common.loginUserInfo.userID;
                entity.CreateUserName = Common.loginUserInfo.userName;
                entity.EditUserID = Common.loginUserInfo.userID;
                entity.EditUserName = Common.loginUserInfo.userName;
                audit.XmlObject = DataObjectToXml<T_FAM_ASSETSMAINTAIN>.ObjListToXml(assetMaintain, "OA");
                audit.Submit();
            }
        }
        #endregion
        private void InitAudit(T_FAM_ASSETSMAINTAIN assetMaintainAudit)
        {
            SMT.SaaS.FrameworkUI.AuditControl.Flow_FlowRecord_T entity = this.audit.AuditEntity;
            entity.ModelCode = "T_FAM_ASSETSMAINTAIN";
            entity.FormID = assetMaintainAudit.MAINTAINEDID;
            entity.CreateCompanyID = Common.loginUserInfo.companyID;
            entity.CreateDepartmentID = Common.loginUserInfo.departmentID;
            entity.CreatePostID = Common.loginUserInfo.postID;
            entity.CreateUserID = Common.loginUserInfo.userID;
            entity.CreateUserName = Common.loginUserInfo.userName;
            entity.EditUserID = Common.loginUserInfo.userID;
            entity.EditUserName = Common.loginUserInfo.userName;
            audit.BindingData();
        }
        #endregion

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void FeeChkBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void FeeChkBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void btnLookPrefix_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnLookFixedAsset_Click(object sender, RoutedEventArgs e)
        {
            LookupFixedAsset lookup = new LookupFixedAsset();
            lookup.Width = 740;
            lookup.Height = 450;
            lookup.Selected += new LookupFixedAsset.DelegateSelect(lookup_Selected);
            lookup.Show();
        }

        void lookup_Selected(T_FAM_FIXEDASSET asset)
        {
            if (asset != null)
            {
                txtAssetsID.Text = asset.ASSETSID.CvtString();
                txtAssetsName.Text = asset.ASSETSNAME.CvtString();
                txtSpecification.Text = asset.SPECIFICATION.CvtString();
                txtUnitID.Text = asset.UNITID.CvtString();
                assetMaintain.USEDDEPARTMENT = Utility.GetDepartmentName(asset.USEDEPARTMENT);
                txtUsedDepartment.Text = Utility.GetDepartmentName(asset.USEDEPARTMENT.CvtString());//使用部门
            }
        }


        private void InitFBControl()
        {
            if (actions == Action.Add)
            {
                fbCtr.Order.ORDERID = "";
            }
            else
            {
                fbCtr.Order.ORDERID = assetMaintain.MAINTAINEDID;//费用对象
            }
            fbCtr.Order.CREATECOMPANYID = Common.loginUserInfo.companyID;
            fbCtr.Order.CREATECOMPANYNAME = Common.loginUserInfo.companyName;
            fbCtr.Order.CREATEDEPARTMENTID = Common.loginUserInfo.departmentID;
            fbCtr.Order.CREATEDEPARTMENTNAME = Common.loginUserInfo.departmentName;
            fbCtr.Order.CREATEPOSTID = Common.loginUserInfo.postID;
            fbCtr.Order.CREATEPOSTNAME = Common.loginUserInfo.postName;
            fbCtr.Order.CREATEUSERID = Common.loginUserInfo.userID;
            fbCtr.Order.CREATEUSERNAME = Common.loginUserInfo.userName;

            fbCtr.Order.OWNERCOMPANYID = Common.loginUserInfo.companyID;
            fbCtr.Order.OWNERCOMPANYNAME = Common.loginUserInfo.companyName;
            fbCtr.Order.OWNERDEPARTMENTID = Common.loginUserInfo.departmentID;
            fbCtr.Order.OWNERDEPARTMENTNAME = Common.loginUserInfo.departmentName;
            fbCtr.Order.OWNERPOSTID = Common.loginUserInfo.postID;
            fbCtr.Order.OWNERPOSTNAME = Common.loginUserInfo.postName;
            fbCtr.Order.OWNERID = Common.loginUserInfo.userID;
            fbCtr.Order.OWNERNAME = Common.loginUserInfo.userName;


            fbCtr.Order.UPDATEUSERID = Common.loginUserInfo.userID;
            fbCtr.Order.UPDATEUSERNAME = Common.loginUserInfo.userName;

            fbCtr.InitDataComplete += (o, e) =>
            {
                System.Windows.Data.Binding bding = new System.Windows.Data.Binding();
                bding.Path = new PropertyPath("TOTALMONEY");
                this.txtMaintainBudget.SetBinding(TextBox.TextProperty, bding);
                this.txtMaintainBudget.DataContext = fbCtr.Order;
            };
            fbCtr.InitData();
        }
    }
}
