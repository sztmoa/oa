
/*
 * 文件名：RepayApplyMasterBLL.cs
 * 作  用：T_FB_REPAYAPPLYMASTER 业务逻辑类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 11:47:04
 * 修改人：
 * 修改时间：
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.DAL;
using SMT.SaaS.BLLCommonServices.FlowWFService;
using SMT.Foundation.Log;
using SMT.SaaS.BLLCommonServices.PersonnelWS;

namespace SMT.FBAnalysis.BLL
{
    public class RepayApplyMasterBLL : BaseBll<T_FB_REPAYAPPLYMASTER>
    {
        public RepayApplyMasterBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_REPAYAPPLYMASTER信息
        /// </summary>
        /// <param name="strRepayApplyMasterId">主键索引</param>
        /// <returns></returns>
        public T_FB_REPAYAPPLYMASTER GetRepayApplyMasterByID(string strRepayApplyMasterId)
        {
            if (string.IsNullOrEmpty(strRepayApplyMasterId))
            {
                return null;
            }

            RepayApplyMasterDAL dalRepayApplyMaster = new RepayApplyMasterDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strRepayApplyMasterId))
            {
                strFilter.Append(" REPAYAPPLYMASTERID == @0");
                objArgs.Add(strRepayApplyMasterId);
            }
            try
            {
                T_FB_REPAYAPPLYMASTER entRd = dalRepayApplyMaster.GetRepayApplyMasterRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
                return entRd;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetRepayApplyMasterByID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据查询条件，获取还款信息(主要用于查询分页)
        /// </summary>
        /// <param name="strOwnerID">查询人的员工ID</param>
        /// <param name="strDateStart">查询起始时间</param>
        /// <param name="strDateEnd">查询截止时间</param>
        /// <param name="strCheckState">查询审核状态</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strOrderBy">strSortKey</param>
        /// <returns></returns>
        public static IQueryable<T_FB_REPAYAPPLYMASTER> GetAllRepayApplyMasterRdListByMultSearch(string strOwnerID, string strDateStart,
            string strDateEnd, string strCheckState, string strFilter, List<object> objArgs, string strSortKey)
        {
            try
            {
                RepayApplyMasterDAL dalRepay = new RepayApplyMasterDAL();
                string strOrderBy = string.Empty;

                if (string.IsNullOrWhiteSpace(strOwnerID) || string.IsNullOrWhiteSpace(strCheckState))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(strSortKey))
                {
                    strOrderBy = strSortKey;
                }
                else
                {
                    strOrderBy = " REPAYAPPLYMASTERID ";
                }

                SMT.SaaS.BLLCommonServices.Utility ulFoo = new SaaS.BLLCommonServices.Utility();
                if (strCheckState != Convert.ToInt32(FBAEnums.CheckStates.WaittingApproval).ToString())
                {
                    ulFoo.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerID, "T_FB_REPAYAPPLYMASTER");
                }
                else
                {
                    string StrOld = "";
                    StrOld = strFilter;//将过滤前的字符串付给再比较
                    ulFoo.SetFilterWithflow("REPAYAPPLYMASTERID", "T_FB_REPAYAPPLYMASTER", strOwnerID, ref strCheckState, ref strFilter, ref objArgs);
                    if (StrOld.Equals(strFilter))
                    {
                        return null;
                    }
                    strCheckState = Convert.ToInt32(FBAEnums.CheckStates.Approving).ToString();
                }

                if (strCheckState == Convert.ToInt32(FBAEnums.CheckStates.All).ToString())
                {
                    strCheckState = string.Empty;
                }

                var ents = dalRepay.GetRepayApplyMasterRdListByMultSearch(strCheckState, strDateStart, strDateEnd, strOrderBy, strFilter, objArgs.ToArray());
                return ents;
            }
            catch (Exception ex)
            {
                Utility.SaveLog(DateTime.Now.ToString() + "调用函数GetAllRepayApplyMasterRdListByMultSearch出错， 查询人的员工ID为：" + strOwnerID + "，错误信息为：" + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 根据查询条件，获取还款信息(主要用于查询分页)
        /// </summary>
        /// <param name="userID">查询人的员工ID</param>
        /// <param name="strDateStart">查询起始时间</param>
        /// <param name="strDateEnd">查询截止时间</param>
        /// <param name="checkState">查询审核状态</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示条目数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns></returns>
        public IQueryable<T_FB_REPAYAPPLYMASTER> GetRepayApplyMasterRdListByMultSearch(string userID, string strDateStart,
            string strDateEnd, string checkState, string strFilter, List<object> objArgs, string strSortKey, int pageIndex,
            int pageSize, ref int pageCount)
        {
            var ents = GetAllRepayApplyMasterRdListByMultSearch(userID, strDateStart, strDateEnd, checkState, strFilter,
               objArgs, strSortKey);
            if (ents == null)
            {
                return null;
            }

            if (pageIndex == 0 && pageSize == 0)
            {
                return ents;
            }

            return Utility.Pager<T_FB_REPAYAPPLYMASTER>(ents, pageIndex, pageSize, ref pageCount);
        }

        #endregion


        #region 写入数据
        /// <summary>
        /// 增加还款主从表数据  add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddRepayApplyMasterAndDetail(T_FB_REPAYAPPLYMASTER entity, List<T_FB_REPAYAPPLYDETAIL> detailList)
        {
            try
            {
                var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
                if (company != null)
                {
                    entity.OWNERCOMPANYNAME = company.CNAME;
                }
            }
            catch
            {

            }
            bool re;
            //2012-8-29
            //从服务端获取时间
            entity.PROJECTEDREPAYDATE = DateTime.Now;
            try
            {
                dal.BeginTransaction();
                foreach (T_FB_REPAYAPPLYDETAIL obj in detailList)
                {
                    //添加还款明细
                    Utility.RefreshEntity(obj);
                    entity.T_FB_REPAYAPPLYDETAIL.Add(obj);
                }
                re = Add(entity);
                if (!re)
                {
                    dal.RollbackTransaction();
                    return false;
                }

                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：AddRepayApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }


        #endregion

        #region (重新提交)更新数据
        /// <summary>
        /// 更新还款主从表数据  add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public void UptRepayApplyMasterAndDetail(string strActionType, T_FB_REPAYAPPLYMASTER entity, 
            List<T_FB_REPAYAPPLYDETAIL> detailList, ref string strMsg)
        {
            try
            {
                var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
                if (company != null)
                {
                    entity.OWNERCOMPANYNAME = company.CNAME;
                }
            }
            catch
            {

            }
            bool re = false;
            if (LockOrder(entity.REPAYAPPLYMASTERID))
            {
                strMsg = "单据正在提交或审核中，不可重复操作！";
                return;
            }

            try
            {
                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;
                T_FB_REPAYAPPLYMASTER cha = GetRepayApplyMasterByID(entity.REPAYAPPLYMASTERID);
                if (cha == null)
                {
                    strMsg = "提交的单据不存在或已删除，不可继续操作！";
                    return;
                }

                object checkStatesOld = cha.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());

                object checkStatesNew = entity.CHECKSTATES;
                FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());

                if ((dOldChecksates == FBAEnums.CheckStates.Approved || dOldChecksates == FBAEnums.CheckStates.UnApproved)
                    && strActionType.ToUpper() != "RESUBMIT")
                {
                    strMsg = "单据已审核完毕，不可再次操作";
                    return;
                }

                //不用月结可以进行借还款
                //#region 是否本月有结算

                //bool isChecked = SystemSettingsBLL.IsChecked;
                //// 没月结，只能处理报销。
                //string entityType = entity.GetType().Name;
                //string[] EntityTypes = new string[] { typeof(T_FB_BORROWAPPLYMASTER).Name, 
                //    typeof(T_FB_REPAYAPPLYMASTER).Name, typeof(T_FB_CHARGEAPPLYMASTER).Name};
                //// 月结不可操作：1.日常报销类型的单据的审核或提交，2.所有单据的提交或重新提交
                //if (!isChecked && (EntityTypes.Contains(entityType) || (strActionType.ToUpper() == "SUBMIT")
                //    || (strActionType.ToUpper() == "RESUBMIT")))
                //{
                //    strMsg = "本月尚未结算,无法提交或审核!";
                //    return;
                //}

                //#endregion

                Utility.CloneEntity(entity, cha);
                cha.UPDATEDATE = DateTime.Now;

                bool n = Update(cha);
                if (n == false)
                {
                    dal.RollbackTransaction();
                    strMsg = "单据更新异常！";
                    return;
                }

                RepayApplyDetailBLL bllRepayDetail = new RepayApplyDetailBLL();
                re = bllRepayDetail.UpdateRepayApplyDetail(cha.REPAYAPPLYMASTERID, detailList);//删除报销明细
                if (!re)
                {
                    dal.RollbackTransaction();
                    strMsg = "单据明细更新异常！";
                    return;
                }

                if (dOldChecksates == dNewCheckStates && dOldChecksates == (int)FBAEnums.CheckStates.UnSubmit
                    && strActionType.ToUpper() == "EDIT")
                {
                    strMsg = "单据更新成功！";
                    return;
                }

                //if (dNewCheckStates == FBAEnums.CheckStates.Approving && ((strActionType.ToUpper() == "SUBMIT" && dOldChecksates == FBAEnums.CheckStates.UnSubmit)
                //    || (strActionType.ToUpper() == "RESUBMIT" && dOldChecksates == FBAEnums.CheckStates.UnApproved)))
                //{
                //    BudgetAccountBLL budgetBLL = new BudgetAccountBLL();
                //    budgetBLL.UpdateAccount(cha, (int)dNewCheckStates);
                //}
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptRepayApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            finally
            {
                ReleaseOrder(entity.REPAYAPPLYMASTERID);
            }
        }

        /// <summary>
        /// 更新还款主表CHECKSTATES字段值  add by zl    该方法由引擎调用，用来更改主表checkstates状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string UptRepayApplyCheckState(T_FB_REPAYAPPLYMASTER entity, List<T_FB_REPAYAPPLYDETAIL> detailList)
        {
            bool flag = false;
            string strMsg = string.Empty;
            PersonAccountBLL PerBLL = new PersonAccountBLL();
            if (LockOrder(entity.REPAYAPPLYMASTERID))
            {
                strMsg = "单据正在提交或审核中，不可重复操作！";
                return strMsg;
            }

            try
            {
                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;

                object checkStatesNew = entity.CHECKSTATES;
                FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());


                T_FB_REPAYAPPLYMASTER cha = GetRepayApplyMasterByID(entity.REPAYAPPLYMASTERID);

                if (cha == null)
                {
                    strMsg = entity.REPAYAPPLYMASTERID + "还款单据不存在，不可继续操作！";
                    Tracer.Debug(strMsg);
                    return strMsg;
                }

                object checkStatesOld = cha.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());

                if (dOldChecksates == FBAEnums.CheckStates.Approved && dNewCheckStates == FBAEnums.CheckStates.Approving)
                {
                    strMsg = "已审核通过的费用报销单禁止再次审核!";
                    string ErrInfo = this.GetType().ToString() + "：UptChargeApplyCheckState，" + System.DateTime.Now.ToString() + "，" + strMsg;
                    Tracer.Debug(ErrInfo);
                    return strMsg;
                }

                Utility.CloneEntity(entity, cha);

                //add zl 12.7 提交审核时产生单据号
                dal.BeginTransaction();
                string strCode = "";
                if (string.IsNullOrEmpty(cha.REPAYAPPLYCODE.Trim()))
                {
                    try
                    {
                        strCode = new OrderCodeBLL().GetAutoOrderCode(entity);
                        cha.REPAYAPPLYCODE = strCode;
                        string err = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + cha.REPAYAPPLYMASTERID + "：产生单据号 " + strCode;
                        Tracer.Debug(err);
                    }
                    catch (Exception ex)
                    {
                        string sr = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + cha.REPAYAPPLYMASTERID + "：产生单据号时出现异常 " + ex.Message;
                        Tracer.Debug(sr);
                        dal.RollbackTransaction();
                        strMsg = "产生单据号时出现异常！";
                        return strMsg;
                    }
                }
                string Logmsg = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + "：UptRepayApplyCheckState，表单ID " + cha.REPAYAPPLYMASTERID + ",审核状态：" + cha.CHECKSTATES;
                Tracer.Debug(Logmsg);
                //add end

                //add zl 审核中时检查还款金额是否大于借款余额 2012.1.11
                if (dNewCheckStates == FBAEnums.CheckStates.Approving || dNewCheckStates == FBAEnums.CheckStates.Approved)
                {
                    strMsg = PerBLL.CheckRepMoneyForRepay(cha, detailList, (int)dNewCheckStates);
                    if (!string.IsNullOrEmpty(strMsg))
                    {
                        dal.RollbackTransaction();
                        return strMsg;
                    }
                }
                //add end

                cha.UPDATEDATE = DateTime.Now;
                bool n = Update(cha);
                if (!n)
                {
                    strMsg = "单据明细更新异常！";
                    dal.RollbackTransaction();
                    return strMsg;
                }

                if (dNewCheckStates == FBAEnums.CheckStates.UnSubmit)
                {
                    dal.CommitTransaction();
                    return strMsg;
                }
                if (dNewCheckStates == FBAEnums.CheckStates.Approving && (dOldChecksates != FBAEnums.CheckStates.UnSubmit && dOldChecksates != FBAEnums.CheckStates.UnApproved))
                {
                    dal.CommitTransaction();
                    return strMsg;
                }
                if (dNewCheckStates == FBAEnums.CheckStates.Approved && dOldChecksates != FBAEnums.CheckStates.Approving)
                {
                    dal.CommitTransaction();
                    return strMsg;
                }
                if (dNewCheckStates == FBAEnums.CheckStates.UnApproved && dOldChecksates != FBAEnums.CheckStates.Approving)
                {
                    dal.CommitTransaction();
                    return strMsg;
                }

                //add zl 12.7  终审通过时还款更新PersonAccount表
                if (dNewCheckStates == FBAEnums.CheckStates.Approved)
                { 
                    n = PerBLL.UptPersonAccountByRepa(cha, detailList, (int)dNewCheckStates);
                    if (!n)
                    {
                        strMsg = "更新借还总账表异常！";
                        dal.RollbackTransaction();
                        return strMsg;
                    }
                }
                dal.CommitTransaction();
                //add end
                return strMsg;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptRepayApplyCheckState，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                strMsg = "更新单据状态出现异常！";
                return strMsg;
            }
            finally
            {
                ReleaseOrder(entity.REPAYAPPLYMASTERID);
            }
        }


        #endregion

        #region 删除数据

        /// <summary>
        /// 删除还款主表数据  add by zl
        /// </summary>
        /// <param name="repayMasterID"></param>
        /// <returns></returns>
        public bool DelRepayApplyMaster(string repayMasterID)
        {
            try
            {
                var entitys = (from ent in dal.GetTable() where ent.REPAYAPPLYMASTERID == repayMasterID select ent);
                if (entitys.Count() > 0)
                {
                    var entity = entitys.FirstOrDefault();
                    //只删除未提交状态的单据
                    if (entity.CHECKSTATES == 0)
                    {
                        Delete(entity);
                        return true;
                    }
                    else
                    {
                        string ErrInfo = "删除还款单出现错误，单据审核状态为：" + entity.CHECKSTATES + " .表单ID为：" + entity.REPAYAPPLYMASTERID;
                        Tracer.Debug(ErrInfo);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：DelRepayApplyMaster，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        /// <summary>
        /// 一起删除还款主表和明细表   add by zl
        /// </summary>
        /// <param name="repayMasterID"></param>
        /// <returns></returns>
        public bool DelRepayApplyMasterAndDetail(string repayMasterID)
        {
            try
            {
                RepayApplyDetailBLL repayDetailBll = new RepayApplyDetailBLL();
                dal.BeginTransaction();
                if (!repayDetailBll.DelRepayApplyDetail(repayMasterID))
                {
                    dal.RollbackTransaction();
                    return false;
                }
                if (!DelRepayApplyMaster(repayMasterID))
                {
                    dal.RollbackTransaction();
                    return false;
                }
                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：DelRepayApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }

        #endregion

        #region 手机版修改审核状态  ljx 2011-8-19
        public int GetRepayApplyForMobile(string Repayid, string StrCheckState)
        {
            string sResult = string.Empty;
            T_FB_REPAYAPPLYMASTER master = new T_FB_REPAYAPPLYMASTER();
            master = GetRepayApplyMasterByID(Repayid);
            master.CHECKSTATES = Convert.ToInt32(StrCheckState);

            RepayApplyDetailBLL bllRepayApplyDetail = new RepayApplyDetailBLL();
            List<T_FB_REPAYAPPLYDETAIL> entRds = bllRepayApplyDetail.GetRepayApplyDetailByMasterID(Repayid);
            if (entRds != null)
            {
                //if (UptRepayApplyCheckState(master, entRds.ToList()))
                //{
                //    return 1;
                //}
                sResult = UptRepayApplyCheckState(master, entRds.ToList());
                if(!string.IsNullOrEmpty(sResult))
                {
                    throw new Exception(sResult);
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }
        #endregion

        #region  转移数据专用 zl
        /// <summary>
        /// 改变还款表状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UptRepayApplyMasterChkSta(T_FB_REPAYAPPLYMASTER entity)
        {
            bool re = true;
            re = Update(entity);
            return re;
        }

        #endregion

        #region 手机版使用
        public bool AddRepayApplyMasterAndDetailForMobile(T_FB_REPAYAPPLYMASTER entity, List<T_FB_REPAYAPPLYDETAIL> detailList,ref string strMsg)
        {
            try
            {                
                bool isReturn = CheckRepay(ref entity, ref detailList, ref strMsg,"1");
                if (!isReturn)
                {
                    return isReturn;
                }
            }
            catch
            {

            }

            bool re;
            //2012-8-29
            //从服务端获取时间
            entity.PROJECTEDREPAYDATE = DateTime.Now;
            try
            {
                dal.BeginTransaction();
                foreach (T_FB_REPAYAPPLYDETAIL obj in detailList)
                {
                    //添加还款明细
                    Utility.RefreshEntity(obj);
                    entity.T_FB_REPAYAPPLYDETAIL.Add(obj);
                }
                re = Add(entity);
                if (!re)
                {
                    dal.RollbackTransaction();
                    return false;
                }

                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：AddRepayApplyMasterAndDetailForMobile，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }

        /// <summary>
        /// 判断还款是否符合条件
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <param name="strMsg"></param>
        /// /// <param name="operFlag">1:添加  2：修改</param>
        /// <returns></returns>
        public bool CheckRepay(ref T_FB_REPAYAPPLYMASTER entity, ref List<T_FB_REPAYAPPLYDETAIL> detailList, ref string strMsg,string operFlag)
        {
            bool isReturn = true;
            PersonnelServiceClient personClient = new PersonnelServiceClient();
            var employee = personClient.GetEmployeeDetailByID(entity.OWNERID);
            string postID = entity.OWNERPOSTID;
            string employeeID = entity.OWNERID;
            if (employee == null || employee.EMPLOYEEPOSTS == null)
            {
                strMsg = "申请人已不存在,请重新建单!";
            }
            else
            {
                var find = employee.EMPLOYEEPOSTS.FirstOrDefault(item => item.T_HR_POST.POSTID == postID);
                if (find == null)
                {
                    strMsg = "申请人已异动, 请重新建单!";
                }
            }
            var entPost = from ent in employee.EMPLOYEEPOSTS
                          where ent.T_HR_POST.POSTID == postID
                          select ent;
            if (entPost.Count() == 0)
            {
                strMsg = "获取申请人岗位信息时为空!";
            }            
            SMT.SaaS.BLLCommonServices.PersonnelWS.T_HR_EMPLOYEEPOST employeePost = entPost.FirstOrDefault();
            entity.OWNERNAME = employee.T_HR_EMPLOYEE.EMPLOYEECNAME;            
            if (employeePost.T_HR_POST != null)
            {
                if (employeePost.T_HR_POST.T_HR_POSTDICTIONARY != null)
                {
                    entity.OWNERPOSTNAME = employeePost.T_HR_POST.T_HR_POSTDICTIONARY.POSTNAME;
                    //entity.OWNERPOSTID = employeePost.T_HR_POST.POSTID;                    
                }
                else
                {
                    strMsg = "员工岗位字典信息不存在";
                }
            }
            else
            {
                strMsg = "员工岗位信息不存在";
            }
            if (employeePost.T_HR_POST.T_HR_DEPARTMENT != null)
            {
                if (employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY != null)
                {
                    entity.OWNERDEPARTMENTNAME = employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                    entity.OWNERDEPARTMENTID = employeePost.T_HR_POST.T_HR_DEPARTMENT.DEPARTMENTID;                    
                }
                else
                {
                    strMsg = "员工部门字典信息不存在";
                }
            }
            else
            {
                strMsg = "员工部门信息不存在";
            }
            if (employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY != null)
            {
                if (employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.BRIEFNAME != null)
                {
                    entity.OWNERCOMPANYNAME = employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.BRIEFNAME;                    
                }
                else
                {
                    entity.OWNERCOMPANYNAME = employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.CNAME;
                }
                entity.OWNERCOMPANYID = employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.COMPANYID;
                
            }
            else
            {
                strMsg = "员工对应公司信息不存在";
            }
            if (!string.IsNullOrEmpty(strMsg))
            {
                return false;
            }

            if (operFlag == "1")
            {
                entity.CREATEUSERID = entity.OWNERID;
                entity.CREATEUSERNAME = entity.OWNERNAME;
                entity.CREATEPOSTID = entity.OWNERPOSTID;
                entity.CREATEPOSTNAME = entity.OWNERPOSTNAME;
                entity.CREATEDEPARTMENTID = entity.OWNERDEPARTMENTID;
                entity.CREATEDEPARTMENTNAME = entity.OWNERDEPARTMENTNAME;
                entity.CREATECOMPANYID = entity.OWNERCOMPANYID;
                entity.CREATECOMPANYNAME = entity.OWNERCOMPANYNAME;
                entity.CREATEDATE = DateTime.Now;
                entity.UPDATEDATE = DateTime.Now;
                entity.UPDATEUSERID = entity.OWNERID;
                entity.UPDATEUSERNAME = entity.OWNERNAME;
            }
            else
            {
                entity.UPDATEDATE = DateTime.Now;
                entity.UPDATEUSERID = entity.OWNERID;
                entity.UPDATEUSERNAME = entity.OWNERNAME;
            }
            if (detailList.Count == 0)
            {
                strMsg = "还款明细记录不能为空";
                return false;
            }
            if (entity.TOTALMONEY == null)
            {
                strMsg = "还款金额合计不能为空！";
                return false;
            }
            decimal dei = 0;
            string detotal = entity.TOTALMONEY.ToString();
            decimal.TryParse(detotal, out dei);
            if (entity.TOTALMONEY <= 0 || dei <= 0)
            {
                strMsg = "还款金额合计不能小于或等于0！";
                return false;
            }
            decimal? totalMoney = 0;
            foreach (T_FB_REPAYAPPLYDETAIL obj in detailList)
            {   
                decimal i = 0;
                if (obj.REPAYMONEY == null)
                {
                    strMsg = "还款金额不能为空";
                    return false;
                }
                string reMoney = obj.REPAYMONEY.ToString();
                if (!decimal.TryParse(reMoney, out i))
                {
                    strMsg = "还款金额请输入正确的数值！";
                    return false;
                }
                
                if (obj.REPAYMONEY < 0)
                {
                    strMsg ="备注："+ obj.REMARK + "中,还款金额不能小于0！";
                    return false;
                }
                if (string.IsNullOrEmpty(obj.REMARK) && obj.REPAYMONEY >0)
                {
                    strMsg = "还款摘要不能为空";
                    return false;
                }
                if (obj.REPAYMONEY > obj.BORROWMONEY)
                {
                    strMsg = "备注：" + obj.REMARK + "中,还款金额不能大于借款余额！";
                    return false;
                }
                totalMoney = totalMoney + obj.REPAYMONEY;
                if (operFlag == "1")
                {
                    if (string.IsNullOrEmpty(obj.REPAYAPPLYDETAILID))
                    {
                        obj.REPAYAPPLYDETAILID = Guid.NewGuid().ToString();
                    }
                    if (obj.T_FB_REPAYAPPLYMASTER == null)
                    {
                        obj.T_FB_REPAYAPPLYMASTER = entity;
                    }
                    obj.CREATEUSERID = entity.OWNERID;
                    obj.CREATEDATE = DateTime.Now;
                    obj.UPDATEDATE = DateTime.Now;                    
                    obj.UPDATEUSERID = entity.UPDATEUSERID;
                    
                }
                else
                {
                    obj.UPDATEDATE = DateTime.Now;
                    obj.UPDATEUSERID = entity.UPDATEUSERID;                    
                }
            }
            entity.TOTALMONEY = (decimal)totalMoney;     
            return isReturn;
        }

        /// <summary>
        /// 修改还款申请记录
        /// </summary>
        /// <param name="strActionType"></param>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <param name="strMsg"></param>
        public void UptRepayApplyMasterAndDetailForMobile(string strActionType, T_FB_REPAYAPPLYMASTER entity,
            List<T_FB_REPAYAPPLYDETAIL> detailList, ref string strMsg)
        {
            try
            {
                
                bool isReturn = CheckRepay(ref entity, ref detailList, ref strMsg,"2");
                if (!isReturn)
                {
                    return ;
                }
            }
            catch
            {

            }
            bool re = false;
            if (LockOrder(entity.REPAYAPPLYMASTERID))
            {
                strMsg = "单据正在提交或审核中，不可重复操作！";
                return;
            }

            try
            {
                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;
                T_FB_REPAYAPPLYMASTER cha = GetRepayApplyMasterByID(entity.REPAYAPPLYMASTERID);
                if (cha == null)
                {
                    strMsg = "提交的单据不存在或已删除，不可继续操作！";
                    return;
                }
                object checkStatesOld = cha.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());
                object checkStatesNew = entity.CHECKSTATES;
                FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());
                if ((dOldChecksates == FBAEnums.CheckStates.Approved || dOldChecksates == FBAEnums.CheckStates.UnApproved)
                    && strActionType.ToUpper() != "RESUBMIT")
                {
                    strMsg = "单据已审核完毕，不可再次操作";
                    return;
                }                
                Utility.CloneEntity(entity, cha);
                cha.UPDATEDATE = DateTime.Now;
                bool n = Update(cha);
                if (n == false)
                {
                    dal.RollbackTransaction();
                    strMsg = "单据更新异常！";
                    return;
                }
                RepayApplyDetailBLL bllRepayDetail = new RepayApplyDetailBLL();
                re = bllRepayDetail.UpdateRepayApplyDetail(cha.REPAYAPPLYMASTERID, detailList);//删除报销明细
                if (!re)
                {
                    dal.RollbackTransaction();
                    strMsg = "单据明细更新异常！";
                    return;
                }
                if (dOldChecksates == dNewCheckStates && dOldChecksates == (int)FBAEnums.CheckStates.UnSubmit
                    && strActionType.ToUpper() == "EDIT")
                {
                    strMsg = "单据更新成功！";
                    return;
                }
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptRepayApplyMasterAndDetailForMobile，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            finally
            {
                ReleaseOrder(entity.REPAYAPPLYMASTERID);
            }
        }


        public void UptRepayApplyMasterAndDetailForMobileToSubmit(string strActionType, T_FB_REPAYAPPLYMASTER entity, ref string strMsg)
        {
            
            bool re = false;
            if (LockOrder(entity.REPAYAPPLYMASTERID))
            {
                strMsg = "单据正在提交或审核中，不可重复操作！";
                return;
            }

            try
            {
                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;
                T_FB_REPAYAPPLYMASTER cha = GetRepayApplyMasterByID(entity.REPAYAPPLYMASTERID);
                if (cha == null)
                {
                    strMsg = "提交的单据不存在或已删除，不可继续操作！";
                    return;
                }
                object checkStatesOld = cha.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());
                object checkStatesNew = entity.CHECKSTATES;
                FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());
                if ((dOldChecksates == FBAEnums.CheckStates.Approved || dOldChecksates == FBAEnums.CheckStates.UnApproved)
                    && strActionType.ToUpper() != "RESUBMIT")
                {
                    strMsg = "单据已审核完毕，不可再次操作";
                    return;
                }
                Utility.CloneEntity(entity, cha);
                cha.UPDATEDATE = DateTime.Now;
                bool n = Update(cha);
                if (n == false)
                {
                    dal.RollbackTransaction();
                    strMsg = "单据更新异常！";
                    return;
                }                
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptRepayApplyMasterAndDetailForMobile，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            finally
            {
                ReleaseOrder(entity.REPAYAPPLYMASTERID);
            }
        }
        #region 获取元数据
        /// <summary>
        /// 获取还款申请元数据
        /// </summary>
        /// <param name="Formid"></param>
        /// <returns>返回填充元数据后的字符串</returns>
        public string GetXmlString(string Formid, ref string RepayCode)
        {
            string strReturn = string.Empty;
            try
            {
                Tracer.Debug("RepayApplyMasterBLL-GetXmlString主表IDl：" + Formid);
                T_FB_REPAYAPPLYMASTER Info = dal.GetObjects<T_FB_REPAYAPPLYMASTER>().Where(t => t.REPAYAPPLYMASTERID == Formid).FirstOrDefault();
                PersonnelServiceClient personel = new PersonnelServiceClient();
                V_EMPLOYEEVIEW employee = personel.GetEmployeeInfoByEmployeeID(Info.OWNERID);
                decimal? stateValue = Convert.ToDecimal("1");
                string checkState = string.Empty;
                string checkStateDict
                    = PermClient.GetDictionaryByCategoryArray(new string[] { "CHECKSTATE" }).Where(p => p.DICTIONARYVALUE == stateValue).FirstOrDefault().DICTIONARYNAME;
                checkState = checkStateDict == null ? "" : checkStateDict;
                if (Info == null)
                {
                    Tracer.Debug("RepayApplyMasterBLL获取还款申请主表记录为空,主表ID:" + Formid);
                    //return strReturn;
                    throw new Exception("获取还款申请主表记录为空");
                }
                if (employee == null)
                {
                    Tracer.Debug("RepayApplyMasterBLL获取元数据时员工信息为空：");
                    //return strReturn;
                    throw new Exception("获取元数据时员工信息为空");
                }
                Tracer.Debug("RepayApplyMasterBLL-GetXmlString员工不为空l：");
                if (employee.POSTLEVEL == null)
                {
                    Tracer.Debug("RepayApplyMasterBLL获取元数据时员工信息时岗位级别PostLevel：");
                    throw new Exception("员工岗位级别为空");
                    //return strReturn;
                }
                var ents = from ent in dal.GetObjects<T_FB_REPAYAPPLYDETAIL>().Include("T_FB_REPAYAPPLYMASTER")
                           where ent.T_FB_REPAYAPPLYMASTER.REPAYAPPLYMASTERID == Formid
                           select ent;
                List<T_FB_REPAYAPPLYDETAIL> objR;
                if (ents.Count() > 0)
                {
                    objR = ents.ToList();
                }
                else
                {
                    Tracer.Debug("RepayApplyMasterBLL还款申请获取明细记录为空，主表IDl：" + Formid);
                    throw new Exception("还款申请获取明细记录集合为空");
                }
                Info.REPAYAPPLYCODE = new OrderCodeBLL().GetAutoOrderCode(Info);
                UptRepayApplyMasterAndDetailForMobileToSubmit("Edit", Info,  ref strReturn);
                if (!string.IsNullOrEmpty(strReturn))
                {
                    Tracer.Debug("更新状态：" + strReturn + "主表记录为：" + Formid);
                    throw new Exception(strReturn);
                }
                Tracer.Debug("RepayApplyMasterBLL-GetXmlString主表IDl：修改主表记录成功");
                RepayCode = Info.REPAYAPPLYCODE;
                decimal? postlevelValue = Convert.ToDecimal(employee.POSTLEVEL.ToString());
                string postLevelName = string.Empty;
                string postLevelDict
                     = PermClient.GetDictionaryByCategoryArray(new string[] { "CHECKSTATE" }).Where(p => p.DICTIONARYVALUE == stateValue).FirstOrDefault().DICTIONARYNAME;
                postLevelName = postLevelDict == null ? "" : postLevelDict;


                string strOwnerName = string.Empty;
                string strOwnerCompanyName = string.Empty;
                string strOwnerPostName = string.Empty;
                string strOwnerDepartmentName = string.Empty;
                strOwnerName = employee.EMPLOYEECNAME;
                strOwnerPostName = employee.POSTNAME;
                strOwnerDepartmentName = employee.DEPARTMENTNAME;
                strOwnerCompanyName = employee.COMPANYNAME;

                SMT.SaaS.MobileXml.MobileXml mx = new SaaS.MobileXml.MobileXml();
                SMT.SaaS.MobileXml.AutoDictionary ad = new SaaS.MobileXml.AutoDictionary();
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
                AutoList.Add(basedata("T_FB_REPAYAPPLYMASTER", "POSTLEVEL", postlevelValue.ToString(), null));//POSTLEVEL
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
                Tracer.Debug("RepayApplyMasterBLL-GetXmlString-开始获取主表记录：" );
                string StrSource = GetBusinessObject("T_FB_REPAYAPPLYMASTER");
                Tracer.Debug("获取还款申请的元数据模板为：" + StrSource);
                strReturn = mx.TableToXml(Info, objR, StrSource, AutoList);
                Tracer.Debug("组合还款申请的元数据模板为：" + strReturn);

            }
            catch (Exception ex)
            {
                Tracer.Debug("获取还款申请的元数据模板出现错误：" + ex.ToString());
                throw new Exception("获取还款申请的元数据出错错误");
            }
            return strReturn;
        }

        #endregion
        #endregion
    }
}
