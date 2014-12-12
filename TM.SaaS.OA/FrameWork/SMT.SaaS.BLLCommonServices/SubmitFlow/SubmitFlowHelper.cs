//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using System.Reflection;
//using System.ServiceModel.Description;
//using System.Collections.ObjectModel;
//using System.Resources;
//using SMT.Foundation.Log;
//using System.ServiceModel;
//using PersonalRecordWS = SMT.SaaS.BLLCommonServices.PersonalRecordWS;
//using OrganizationWS = SMT.SaaS.BLLCommonServices.OrganizationWS;
//using TokenServiceWS = SMT.SaaS.BLLCommonServices.TokenServiceWS;
//using PersonnelWS = SMT.SaaS.BLLCommonServices.PermissionWS;
//using PermissionWS = SMT.SaaS.BLLCommonServices.PermissionWS;
//using BllCommonPermissionWS = SMT.SaaS.BLLCommonServices.BllCommonUserPermissionWS;
//using SMT.SaaS.BLLCommonServices.PersonnelWS;
//using FBServiceWS = SMT.SaaS.BLLCommonServices.FBServiceWS;
//using SMT.SaaS.BLLCommonServices.RMServicesWS;
//using SMT.SaaS.BLLCommonServices.WPServicesWS;
//using SMT.SaaS.BLLCommonServices.TMServicesWS;
//using SMT.SaaS.BLLCommonServices.MVCCacheSV;
//using SMT.SaaS.BLLCommonServices.FlowWFService;

//namespace SMT.SaaS.BLLCommonServices.SubmitFlow
//{
//    class SubmitFlowHelper
//    {
//        private static ServiceClient auditServiceClient;
//        public ServiceClient AuditService
//        {
//            get
//            {
//                if (auditServiceClient == null)
//                {
//                    auditServiceClient = new ServiceClient();
//                    //auditServiceClient.SubimtFlow
//                    //auditServiceClient.GetFlowInfo();
//                }
//                return auditServiceClient;
//            }
            
//        }
//        /// <summary>
//        /// WcfFlowService.FlowResult cc = service.StartFlow(
//        /// "FormID"[FormID:业务表单ID，必填], 
//        /// ""[FlowGUID:待审批流程GUID，不填],
//        /// "SMTTestFlow"[ModelCode:模块代码,必填],
//        /// "SMT"[CompanyID:公司代码,必填], 
//        /// "Manage"[OfficeID:部门代码,必填], 
//        /// "User"[CreateUserID:创建用户ID,必填],
//        /// ""[NextStateCode:自定义流程代码,可选], 
//        /// "User"[AppUserId:下一步骤人ID,可选], 
//        /// ""[Content:审批意见内容,不填],
//        /// ""[Appopt:审批意见(0-不同意,1-同意),不填],
//        /// "Add"[操作标志：Add增加，Update：审批]);  //操作成功返回SUCCESS
//        /// </summary>
//        public Flow_FlowRecord_T AuditEntity { set; get; }
//        public SubmitData AuditSubmitData { set; get; }
//        private Dictionary<Role_UserType, ObservableCollection<UserInfo>> DictCounterUser;


//        protected virtual void SubimtFlow(AuditOperation auditOperation, AuditAction action)
//        {
//            string op = auditOperation.ToString();
//            string tmpnextStateCode = IsEndAudit ? "EndFlow" : NextStateCode; //EndFlow

//            SubmitFlag AuditSubmitFlag = op.ToUpper() == "ADD" ? SubmitFlag.New : SubmitFlag.Approval;
//            #region beyond
//            switch (auditOperation)
//            {
//                case AuditOperation.Add:
//                    AuditSubmitFlag = SubmitFlag.New;
//                    break;
//                case AuditOperation.Update:
//                    AuditSubmitFlag = SubmitFlag.Approval;
//                    break;
//                case AuditOperation.Cancel:
//                    AuditSubmitFlag = SubmitFlag.Cancel;
//                    break;
//                default:
//                    break;

//            }
//            AuditSubmitData.DictCounterUser = this.DictCounterUser;
//            if (AuditSubmitFlag == SubmitFlag.New)
//            {
//                AuditSubmitData.XML = AuditEntity.XmlObject; 
//            }


//            #endregion

//            AuditSubmitData.FormID = AuditEntity.FormID;
//            AuditSubmitData.ModelCode = AuditEntity.ModelCode;
//            AuditSubmitData.ApprovalUser = new UserInfo();
//            AuditSubmitData.ApprovalUser.CompanyID = AuditEntity.CreateCompanyID;

//            AuditSubmitData.ApprovalUser.DepartmentID = AuditEntity.CreateDepartmentID;
//            AuditSubmitData.ApprovalUser.PostID = AuditEntity.CreatePostID;
//            AuditSubmitData.ApprovalUser.UserID = AuditEntity.CreateUserID;
//            AuditSubmitData.ApprovalUser.UserName = AuditEntity.CreateUserName;
//            AuditSubmitData.ApprovalContent = AuditRemark;
//            AuditSubmitData.NextStateCode = tmpnextStateCode;
//            AuditSubmitData.NextApprovalUser = new UserInfo();
//            AuditSubmitData.NextApprovalUser.CompanyID = NextCompanyID;
//            AuditSubmitData.NextApprovalUser.DepartmentID = NextDepartmentID;
//            AuditSubmitData.NextApprovalUser.PostID = NextPostID;
//            AuditSubmitData.NextApprovalUser.UserID = NextUserID;
//            AuditSubmitData.NextApprovalUser.UserName = NextUserName;
//            AuditSubmitData.SubmitFlag = AuditSubmitFlag;
//            //AuditSubmitData.XML = XmlObject;

//            AuditSubmitData.FlowSelectType = IsFixedFlow ? FlowSelectType.FixedFlow : FlowSelectType.FreeFlow;

//            if (!IsFixedFlow && ckbIsEndAudit.IsChecked.Value != true && action != AuditAction.Fail)
//            {
//                if (string.IsNullOrEmpty(this.txtAuditId.Text))
//                {
//                    //ComfirmWindow.ConfirmationBox("","请选择下一审核人" , Utility.GetResourceStr("CONFIRMBUTTON"));
//                    DataResult dataResult = new DataResult();
//                    dataResult.FlowResult = FlowResult.FAIL;
//                    dataResult.Err = "请选择下一审核人";
//                    //AuditEventArgs args = new AuditEventArgs(AuditEventArgs.AuditResult.Error, dataResult);
//                    //args.StartDate = this.AuditEntity.StartDate;
//                    //args.EndDate = System.DateTime.Now;
//                    this.DoAuditResult(dataResult);
//                    //this.CloseProcess();
//                    return;
//                }
//                else if (this.txtAuditId.Text == SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID)
//                {
//                    DataResult dataResult = new DataResult();
//                    dataResult.FlowResult = FlowResult.FAIL;
//                    dataResult.Err = "不能提交给自己";
//                    //AuditEventArgs args = new AuditEventArgs(AuditEventArgs.AuditResult.Error, dataResult);
//                    //args.StartDate = this.AuditEntity.StartDate;
//                    //args.EndDate = System.DateTime.Now;
//                    this.DoAuditResult(dataResult);
//                    //this.CloseProcess();
//                    return;
//                }
//            }

//            if (AuditSubmitData.FlowType == null)
//                AuditSubmitData.FlowType = FlowType.Approval;

//            if (AuditSubmitFlag == SubmitFlag.Approval)
//            {
//                AuditSubmitData.ApprovalUser.CompanyID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
//                AuditSubmitData.ApprovalUser.DepartmentID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
//                AuditSubmitData.ApprovalUser.PostID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
//                AuditSubmitData.ApprovalUser.UserID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
//                AuditSubmitData.ApprovalUser.UserName = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
//            }
//            //提交人(只帮别人提单的时候起作用,区分单据所属人)
//            AuditSubmitData.SumbitCompanyID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
//            AuditSubmitData.SumbitDeparmentID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
//            AuditSubmitData.SumbitPostID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
//            AuditSubmitData.SumbitUserID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
//            AuditSubmitData.SumbitUserName = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
//            //end /提交人(只帮别人提单的时候起作用,区分单据所属人)
//            AuditSubmitData.ApprovalResult = (ApprovalResult)((int)action);// SMTWFTest.WcfFlowService.ApprovalResult.Pass;
//           DataResult submitResult= AuditService.SubimtFlow(AuditSubmitData);
//           DoAuditResult(submitResult);
//            //RetSubmit = false;
//            // beyond 记录日志
//            //submitStartTime = DateTime.Now;
//        }
//        /// <summary>
//        /// 检测审核信息
//        /// </summary>
//        private bool AuditCheck()
//        {
//            this.AuditEntity.StartDate = System.DateTime.Now;
//            string strExceptionMsg = "";
//            //if (string.IsNullOrEmpty(this.AuditEntity.CreateCompanyID))
//            //{
//            //    strExceptionMsg = "公司ID不能为空";
//            //}

//            if (string.IsNullOrEmpty(this.AuditEntity.CreateUserID))
//            {
//                strExceptionMsg = "创建人不能为空";
//            }
//            //if (string.IsNullOrEmpty(this.AuditEntity.EditUserID))
//            //{
//            //    strExceptionMsg = "修改人不能为空";
//            //}

//            if (string.IsNullOrEmpty(this.AuditEntity.CreatePostID))
//            {
//                strExceptionMsg = "岗位不能为空";
//            }

//            if (string.IsNullOrEmpty(this.AuditEntity.ModelCode))
//            {
//                strExceptionMsg = "模块代码不能为空";
//            }

//            if (string.IsNullOrEmpty(this.AuditEntity.FormID))
//            {
//                strExceptionMsg = "表单ID不能为空";
//            }

//            if (!string.IsNullOrEmpty(strExceptionMsg))
//            {              
//                //if (dt != null) dt.SmtProgressBar.Visibility = Visibility.Collapsed;
//                return false;
//                //throw new Exception(strExceptionMsg);
//            }
//            else
//            {
//                return true;
//            }

//        }

//        private void DoAuditResult(DataResult dataResult)
//        {
//            try
//            {


//                //beyond 加入撤单
//                if (dataResult.FlowResult == FlowResult.SUCCESS && dataResult.SubmitFlag == SubmitFlag.Cancel)
//                {
//                    AuditEventArgs args = new AuditEventArgs(AuditEventArgs.AuditResult.CancelSubmit, dataResult);
//                    args.StartDate = this.AuditEntity.StartDate;
//                    args.EndDate = System.DateTime.Now;
//                    OnAuditCompleted(this, args);
//                    this.BindingData();
//                }
//                else if (dataResult.FlowResult == FlowResult.SUCCESS)
//                {

//                    AuditEventArgs args = new AuditEventArgs(AuditEventArgs.AuditResult.Auditing, dataResult);
//                    if (currAuditOperation == AuditOperation.Add)
//                    {
//                        args.Action = AuditEventArgs.AuditAction.Submit;
//                    }
//                    args.StartDate = this.AuditEntity.StartDate;
//                    args.EndDate = System.DateTime.Now;
//                    OnAuditCompleted(this, args);
//                    this.BindingData();
//                }
//                else if (dataResult.FlowResult == FlowResult.END)
//                {
//                    if (currentAction == AuditAction.Fail)
//                    {
//                        AuditEventArgs args = new AuditEventArgs(AuditEventArgs.AuditResult.Fail, dataResult);
//                        args.StartDate = this.AuditEntity.StartDate;
//                        args.EndDate = System.DateTime.Now;
//                        OnAuditCompleted(this, args);
//                    }
//                    else if (currentAction == AuditAction.Pass)
//                    {
//                        AuditEventArgs args = new AuditEventArgs(AuditEventArgs.AuditResult.Successful, dataResult);
//                        args.StartDate = this.AuditEntity.StartDate;
//                        args.EndDate = System.DateTime.Now;
//                        OnAuditCompleted(this, args);
//                    }
//                    this.BindingData();
//                }
//                else if (dataResult.FlowResult == FlowResult.FAIL)
//                {
//                    AuditEventArgs args = new AuditEventArgs(AuditEventArgs.AuditResult.Error, dataResult);
//                    args.StartDate = this.AuditEntity.StartDate;
//                    args.EndDate = System.DateTime.Now;
//                    OnAuditCompleted(this, args);
//                }
//                else if (dataResult.FlowResult == FlowResult.Countersign)
//                {
//                    this.CounterAction(dataResult);
//                }
//                else
//                {
//                    OtherAction(dataResult);
//                }
//            }

//            // 1s 冉龙军
//            //    catch
//            //{
//            //    throw new Exception();
//            //}
//            catch (Exception ex)
//            {
//                //MessageBox.Show(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.Message), MessageBoxButton.OK);
//                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"),
//                                      Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
//            }
//            // 1e

//            finally
//            {
//                CanAudit(true);
//            }

//        }
//    }


//    /// <summary>
//    /// 审核操作
//    /// </summary>
//    protected enum AuditOperation
//    {
//        /// <summary>
//        /// 新增
//        /// </summary>
//        Add,
//        /// <summary>
//        /// 修改
//        /// </summary>
//        Update,
//        #region beyond
//        Cancel = 5

//        #endregion
//    }
//    /// <summary>
//    /// 审核动作
//    /// </summary>
//    public enum AuditAction
//    {
//        /// <summary>
//        /// 审核不通过
//        /// </summary>
//        Fail = 0,
//        /// <summary>
//        /// 审核通过
//        /// </summary>
//        Pass = 1

//    }
//}
