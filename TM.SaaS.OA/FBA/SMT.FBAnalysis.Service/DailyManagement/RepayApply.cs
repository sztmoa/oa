using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMT.FBAnalysis.BLL;
using System.ServiceModel;
using SMT_FB_EFModel;
using SMT.FBAnalysis.CustomModel;

namespace SMT.FBAnalysis.Service
{
    public partial class DailyManagementServices
    {
        #region T_FB_REPAYAPPLYDETAIL 服务

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strRepayApplyDetailId">主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public T_FB_REPAYAPPLYDETAIL GetRepayApplyDetailByID(string strRepayApplyDetailId)
        {
            T_FB_REPAYAPPLYDETAIL entRd = new T_FB_REPAYAPPLYDETAIL();
            using(RepayApplyDetailBLL bllRepayApplyDetail = new RepayApplyDetailBLL())
            {
                entRd = bllRepayApplyDetail.GetRepayApplyDetailByID(strRepayApplyDetailId);
                return entRd;
            }
        }

        /// <summary>
        /// 根据主表ID获取信息   add by zl
        /// </summary>
        /// <param name="strRepayApplyMasterId">还款主表ID</param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_REPAYAPPLYDETAIL> GetRepayApplyDetailListByMasterID(string strRepayApplyMasterId)
        {
            List<T_FB_REPAYAPPLYDETAIL> entRds = new List<T_FB_REPAYAPPLYDETAIL>();
            using(RepayApplyDetailBLL bllRepayApplyDetail = new RepayApplyDetailBLL())
            {
                entRds = bllRepayApplyDetail.GetRepayApplyDetailByMasterID(strRepayApplyMasterId);
                return entRds;
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strRepayApplyDetailName">名称</param>
        /// <param name="strCompanyID">公司序号</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<T_FB_REPAYAPPLYDETAIL> GetRepayApplyDetailListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            IList<T_FB_REPAYAPPLYDETAIL> entList = new List<T_FB_REPAYAPPLYDETAIL>();
            using(RepayApplyDetailBLL bllRepayApplyDetail = new RepayApplyDetailBLL())
            {
                entList = bllRepayApplyDetail.GetRepayApplyDetailRdListByMultSearch(strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount).ToList();
                return entList.Count() > 0 ? entList.ToList() : null;
            }
        }

        #endregion

        #region T_FB_REPAYAPPLYMASTER 服务

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strRepayApplyMasterId">主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public T_FB_REPAYAPPLYMASTER GetRepayApplyMasterByID(string strRepayApplyMasterId)
        {
            T_FB_REPAYAPPLYMASTER entRd = new T_FB_REPAYAPPLYMASTER();
            using(RepayApplyMasterBLL bllRepayApplyMaster = new RepayApplyMasterBLL())
            {
                entRd = bllRepayApplyMaster.GetRepayApplyMasterByID(strRepayApplyMasterId);
                return entRd;
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strRepayApplyMasterName">名称</param>
        /// <param name="strCompanyID">公司序号</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<T_FB_REPAYAPPLYMASTER> GetRepayApplyMasterListByMultSearch(string userID, string strDateStart, string strDateEnd,
            string checkState, string strFilter, List<object> objArgs, string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            IQueryable<T_FB_REPAYAPPLYMASTER> entIq;
            IList<T_FB_REPAYAPPLYMASTER> entList = new List<T_FB_REPAYAPPLYMASTER>();
            using(RepayApplyMasterBLL bllRepayApplyMaster = new RepayApplyMasterBLL())
            {
                entIq = bllRepayApplyMaster.GetRepayApplyMasterRdListByMultSearch(userID, strDateStart, strDateEnd,
                        checkState, strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount);
                if (entIq != null)
                {
                    entList = entIq.ToList();
                    return entList.Count() > 0 ? entList.ToList() : null;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 写入还款主表和明细表数据  add by zl
        /// </summary>
        /// <param name="repayMaster"></param>
        /// <param name="repayDetail"></param>
        /// <returns></returns>
        [OperationContract]
        public bool AddRepayApplyMasterAndDetail(T_FB_REPAYAPPLYMASTER repayMaster, List<T_FB_REPAYAPPLYDETAIL> repayDetail)
        {
            bool re;
            using(RepayApplyMasterBLL repayBLL = new RepayApplyMasterBLL())
            {
                re = repayBLL.AddRepayApplyMasterAndDetail(repayMaster, repayDetail);
                return re;
            }
        }

        /// <summary>
        /// 更新还款主表和明细表数据  add by zl
        /// </summary>
        /// <param name="repayMaster"></param>
        /// <param name="repayDetail"></param>
        /// <returns></returns>
        [OperationContract]
        public void UptRepayApplyMasterAndDetail(string strActionType, T_FB_REPAYAPPLYMASTER repayMaster, 
            List<T_FB_REPAYAPPLYDETAIL> repayDetail, ref string strMsg)
        {
            using(RepayApplyMasterBLL repayBLL = new RepayApplyMasterBLL())
            {
                repayBLL.UptRepayApplyMasterAndDetail(strActionType, repayMaster, repayDetail, ref strMsg);
            }
        }

        /// <summary>
        /// 删除还款主明细表数据   add by zl
        /// </summary>
        /// <param name="repayMasterID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DelRepayApplyMasterAndDetail(List<string> repayMasterID)
        {
            using(RepayApplyMasterBLL repayBLL = new RepayApplyMasterBLL())
            {
                foreach (string obj in repayMasterID)
                {
                    if (!repayBLL.DelRepayApplyMasterAndDetail(obj))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 审核流程时更新还款主表CHECKSTATES字段值 add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        public bool UptRepayApplyCheckState(T_FB_REPAYAPPLYMASTER entity, List<T_FB_REPAYAPPLYDETAIL> repayDetail)
        {
            using(RepayApplyMasterBLL repayBLL = new RepayApplyMasterBLL())
            {
                bool re = true;
                string sMsg = repayBLL.UptRepayApplyCheckState(entity, repayDetail);
                if(!string.IsNullOrEmpty(sMsg))
                {
                    re = false;
                }
                return re;
            }
        }

        /// <summary>
        /// 获取单据编号 add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetRepayOrderCode(T_FB_REPAYAPPLYMASTER entity)
        {
            return new OrderCodeBLL().GetAutoOrderCode(entity);
        }

        #endregion

        #region T_FB_PERSONACCOUNT 服务   zl
        /// <summary>
        /// 根据条件，获取T_FB_PERSONACCOUNT信息
        /// </summary>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_PERSONACCOUNT> GetPersonAccountListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            return PersonAccountBLL.GetAllPersonAccountRdListByMultSearch(strFilter, objArgs, strSortKey).ToList();
        }

        #endregion

        #region 手机版使用接口
        /// <summary>
        /// 添加还款记录
        /// </summary>
        /// <param name="repayMaster">还款主表</param>
        /// <param name="repayDetail">还款明细表集合</param>
        /// <param name="strMsg">返回字符串：无错误返回为空</param>
        /// <returns></returns>
        [OperationContract]
        public bool AddRepayApplyMasterAndDetailForMobile(T_FB_REPAYAPPLYMASTER repayMaster, List<T_FB_REPAYAPPLYDETAIL> repayDetail,ref string strMsg)
        {
            bool re;
            using (RepayApplyMasterBLL repayBLL = new RepayApplyMasterBLL())
            {
                re = repayBLL.AddRepayApplyMasterAndDetailForMobile(repayMaster, repayDetail,ref strMsg);
                return re;
            }
        }
        /// <summary>
        /// 修改借款申请
        /// </summary>
        /// <param name="strActionType">字符串：RESUBMIT 重新提交；EDIT 修改</param>
        /// <param name="repayMaster">还款主表记录</param>
        /// <param name="repayDetail">还款明细表集合</param>
        /// <param name="strMsg">返回提示信息。成功为空</param>
        [OperationContract]
        public void UptRepayApplyMasterAndDetailForMobile(string strActionType, T_FB_REPAYAPPLYMASTER repayMaster,
            List<T_FB_REPAYAPPLYDETAIL> repayDetail, ref string strMsg)
        {
            using (RepayApplyMasterBLL repayBLL = new RepayApplyMasterBLL())
            {
                repayBLL.UptRepayApplyMasterAndDetailForMobile(strActionType, repayMaster, repayDetail, ref strMsg);
            }
        }


        /// <summary>
        /// 获取还款申请元数据
        /// </summary>
        /// <param name="formid">主表ID</param>
        /// <returns>返回填充后的记录</returns>
        [OperationContract]
        public string GetRepayApplyMasterXMLString(string formid, ref string RepayCode)
        {
            using (RepayApplyMasterBLL repayBLL = new RepayApplyMasterBLL())
            {
                return repayBLL.GetXmlString(formid, ref RepayCode);
            }
        }

        /// <summary>
        /// 获取借款信息
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_REPAYAPPLYDETAIL> GetPersonAccountListByMultSearchForMobile(string employeeID, string companyID)
        {
            using (PersonAccountBLL repayBLL = new PersonAccountBLL())
            {
                string strFilter = string.Empty;
                List<object> paras = new List<object>();
                string strSortKey = "PERSONACCOUNTID";
                if (!string.IsNullOrEmpty(companyID))
                {
                    if (!string.IsNullOrEmpty(strFilter))
                    {
                        strFilter += " and ";
                    }
                    strFilter += "@" + paras.Count().ToString() + ".Contains(OWNERCOMPANYID) ";
                    paras.Add(companyID);
                }
                if (!string.IsNullOrEmpty(employeeID))
                {
                    if (!string.IsNullOrEmpty(strFilter))
                    {
                        strFilter += " and ";
                    }
                    strFilter += "@" + paras.Count().ToString() + ".Contains(OWNERID) ";
                    paras.Add(employeeID);
                }
                return PersonAccountBLL.GetAllPersonAccountRdListByMultSearchForMobile(strFilter, paras, strSortKey,employeeID).ToList();
            }
           
        }

        #endregion
    }
}