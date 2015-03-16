
/*
 * 文件名：ExtensionalOrderBLL.cs
 * 作  用：T_FB_EXTENSIONALORDER 业务逻辑类
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

namespace SMT.FBAnalysis.BLL
{
    public class ExtensionalOrderBLL
    {
        public ExtensionalOrderBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_EXTENSIONALORDER信息
        /// </summary>
        /// <param name="strExtensionalOrderId">主键索引</param>
        /// <returns></returns>
        public T_FB_EXTENSIONALORDER GetExtensionalOrderByID(string strExtensionalOrderId)
        {
            if (string.IsNullOrEmpty(strExtensionalOrderId))
            {
                return null;
            }

            ExtensionalOrderDAL dalExtensionalOrder = new ExtensionalOrderDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strExtensionalOrderId))
            {
                strFilter.Append(" EXTENSIONALORDERID == @0");
                objArgs.Add(strExtensionalOrderId);
            }

            T_FB_EXTENSIONALORDER entRd = dalExtensionalOrder.GetExtensionalOrderRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取T_FB_EXTENSIONALORDER信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_EXTENSIONALORDER> GetAllExtensionalOrderRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            ExtensionalOrderDAL dalExtensionalOrder = new ExtensionalOrderDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " EXTENSIONALORDERID ";
            }

            var q = dalExtensionalOrder.GetExtensionalOrderRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_EXTENSIONALORDER信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_EXTENSIONALORDER信息</returns>
        public IQueryable<T_FB_EXTENSIONALORDER> GetExtensionalOrderRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllExtensionalOrderRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_EXTENSIONALORDER>(q, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 借款控件传递的扩展单据数据保存起来，并根据扩展单据的审核状态，及单据类型产生新的预算关联单据(eg:费用报销单，借款申请单)
        /// 并返回预算关联单据号
        /// </summary>
        /// <param name="entRd">扩展单据实体</param>
        /// <returns></returns>
        public string SaveExtensionalOrderRd(T_FB_EXTENSIONALORDER entTemp, string strFormType)
        {
            string strRes = string.Empty;
            ExtensionalOrderDAL dalExtensionalOrder = new ExtensionalOrderDAL();
            try
            {
                if (entTemp == null)
                {
                    return "NOTFOUND";
                }

                if (entTemp.T_FB_EXTENSIONORDERDETAIL == null)
                {
                    return "NOTFOUND";
                }

                if (entTemp.T_FB_EXTENSIONORDERDETAIL.Count() == 0)
                {
                    return "NOTFOUND";
                }

                if (entTemp.TOTALMONEY == null)
                {
                    return "TOTALMONEYTHANO";
                }

                if (entTemp.TOTALMONEY.Value <= 0)
                {
                    return "TOTALMONEYTHANO";
                }

                dalExtensionalOrder.BeginTransaction();

                string innerOrderID = entTemp.INNERORDERID;

                #region 处理 Extension 单据
                T_FB_EXTENSIONALORDER entAdd = new T_FB_EXTENSIONALORDER();
                Utility.CloneEntity(entTemp, entAdd);
                // 第一次提交 
                if ((entTemp.CHECKSTATES == Convert.ToInt32(FBAEnums.CheckStates.Approving) && string.IsNullOrEmpty(innerOrderID)) || strFormType == Convert.ToInt32(FBAEnums.FormTypes.Resubmit).ToString())
                {
                    entTemp.INNERORDERID = Guid.NewGuid().ToString();
                }

                dalExtensionalOrder.Add(entAdd);

                List<T_FB_EXTENSIONORDERDETAIL> entDetails = entTemp.T_FB_EXTENSIONORDERDETAIL.ToList();
                ExtensionOrderDetailDAL dalDetail = new ExtensionOrderDetailDAL();
                foreach (T_FB_EXTENSIONORDERDETAIL entDetail in entDetails)
                {
                    dalDetail.Add(entDetail);
                }
                #endregion

                int iCurCheckState = Convert.ToInt32(entTemp.CHECKSTATES.ToString());
                switch (iCurCheckState)
                {
                    case 0:                        
                        break;
                    case 1:
                        SaveChargeRdByExtenOrder(strFormType, entAdd);
                        break;
                    case 2:
                        SaveChargeRdByExtenOrder(strFormType, entAdd);
                        break;
                    case 3:
                        SaveChargeRdByExtenOrder(strFormType, entAdd);
                        break;
                }

                dalExtensionalOrder.CommitTransaction();
            }
            catch (Exception ex)
            {
                dalExtensionalOrder.RollbackTransaction();
                Utility.SaveLog(ex.ToString());
                strRes = "保存预算单据失败：系统错误，请联系管理员！";
            }
            return strRes;
        }


        /// <summary>
        /// 根据扩展单据，生成其关联的费用报销单据(仅仅是保存，不冻结/扣除预算额度)
        /// </summary>
        /// <param name="entAdd"></param>
        private void SaveChargeRdByExtenOrder(string strFormType, T_FB_EXTENSIONALORDER entTemp)
        {
            //1.判定当前是否进行了月结
            bool bIsChecked = false;
            IsCheckedAccount(ref bIsChecked);
            if (!bIsChecked)
            {
                throw new Exception("本月尚未结算,无法提交!");
            }

            if (strFormType == Convert.ToInt32(FBAEnums.FormTypes.Resubmit).ToString())
            {
                CancelOldChargeRdByExtenOrder(entTemp.EXTENSIONALORDERID);
            }

            ChargeApplyMasterBLL bllCharge = new ChargeApplyMasterBLL();
            T_FB_CHARGEAPPLYMASTER entCharge = bllCharge.GetChargeApplyMasterByID(entTemp.INNERORDERID);

            if (entCharge == null)
            {
                entCharge = new T_FB_CHARGEAPPLYMASTER();
                entCharge.CHARGEAPPLYMASTERID = entTemp.INNERORDERID;
                entCharge.CHARGEAPPLYMASTERCODE = "自动生成";
                entCharge.BUDGETARYMONTH = System.DateTime.Now.Date;
                entCharge.PAYTYPE = 1;
            }
            
            entCharge.EDITSTATES = 1;
            entCharge.CHECKSTATES = entTemp.CHECKSTATES;

            entCharge.T_FB_EXTENSIONALORDER = entTemp;
            entCharge.BANKACCOUT = entTemp.BANKACCOUT;
            entCharge.BANK = entTemp.BANK;
            entCharge.RECEIVER = entTemp.RECEIVER;
            entCharge.PAYTARGET = entTemp.PAYTARGET;
            entCharge.TOTALMONEY = entTemp.TOTALMONEY.Value;
            entCharge.REMARK = entTemp.REMARK;

            entCharge.CREATECOMPANYID = entTemp.CREATECOMPANYID;
            entCharge.CREATEDEPARTMENTID = entTemp.CREATEDEPARTMENTID;
            entCharge.CREATEPOSTID = entTemp.CREATEPOSTID;
            entCharge.CREATEUSERID = entTemp.CREATEUSERID;
            entCharge.CREATEDATE = entTemp.CREATEDATE;

            entCharge.OWNERCOMPANYID = entTemp.OWNERCOMPANYID;
            entCharge.OWNERDEPARTMENTID = entTemp.OWNERDEPARTMENTID;
            entCharge.OWNERID = entTemp.OWNERID;
            entCharge.OWNERPOSTID = entTemp.OWNERPOSTID;

            entCharge.CREATECOMPANYNAME = entTemp.CREATECOMPANYNAME;
            entCharge.CREATEDEPARTMENTNAME = entTemp.CREATEDEPARTMENTNAME;
            entCharge.CREATEPOSTNAME = entTemp.CREATEPOSTNAME;
            entCharge.CREATEUSERNAME = entTemp.CREATEUSERNAME;

            entCharge.OWNERCOMPANYNAME = entTemp.OWNERCOMPANYNAME;
            entCharge.OWNERDEPARTMENTNAME = entTemp.OWNERDEPARTMENTNAME;
            entCharge.OWNERNAME = entTemp.OWNERNAME;
            entCharge.OWNERPOSTNAME = entTemp.OWNERPOSTNAME;

            entCharge.UPDATEUSERID = entTemp.UPDATEUSERID;
            entCharge.UPDATEDATE = entTemp.UPDATEDATE;
        }
        
        /// <summary>
        /// 检查当前月，是否已进行月结
        /// </summary>
        /// <param name="bIsChecked"></param>
        private void IsCheckedAccount(ref bool bIsChecked)
        {
            SystemSettingsDAL dalSystemSetting = new SystemSettingsDAL();
            T_FB_SYSTEMSETTINGS entRd = dalSystemSetting.GetObjects().FirstOrDefault();
            if (entRd == null)
            {
                return;
            }

            if (entRd.LASTCHECKDATE != null)
            {
                var checkDate = entRd.LASTCHECKDATE.Value;
                var nowDate = System.DateTime.Now.Date;
                if (checkDate.Year == nowDate.Year && checkDate.Month == nowDate.Month)
                {
                    bIsChecked = true;
                }
            }
        }

        /// <summary>
        /// 根据扩展单据ID，查找与其关联的老单据，更新其状态为
        /// </summary>
        /// <param name="strExtenOrderID"></param>
        private void CancelOldChargeRdByExtenOrder(string strExtenOrderID)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strExtenOrderID))
                {
                    return;
                }

                ChargeApplyMasterBLL bllCharge  = new ChargeApplyMasterBLL();

                var ents = bllCharge.GetChargeApplyMasterRdListByExtenOrderID(strExtenOrderID);
                if (ents == null)
                {
                    return;
                }

                decimal dCheckState = 0, dEditState = 0;
                dCheckState = Convert.ToDecimal(FBAEnums.CheckStates.UnApproved);
                dEditState = Convert.ToDecimal(FBAEnums.EditStates.UnActived);
                BudgetAccountBLL bllAccount = new BudgetAccountBLL();
                foreach (T_FB_CHARGEAPPLYMASTER item in ents)
                {
                    item.CHECKSTATES = Convert.ToInt32(dCheckState);
                    item.EDITSTATES = Convert.ToInt32(dEditState);
                    item.UPDATEDATE = DateTime.Now;

                    bllCharge.Update(item);
                    bllAccount.UpdateAccount(item, dCheckState);
                }
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.ToString());
            }
        }

        #endregion

    }
}

