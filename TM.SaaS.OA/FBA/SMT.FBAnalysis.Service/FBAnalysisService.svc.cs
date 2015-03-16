using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.BLL;
using System.Collections.Generic;
using SMT.FBAnalysis.CustomModel;
using SMT.FBAnalysis.Services;

namespace SMT.FBAnalysis.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class FBAnalysisService
    {
        // 在此处添加更多操作并使用 [OperationContract] 标记它们

        #region 查询分析 服务
        /// <summary>
        /// 获取信息
        /// 注：预算-查询分析-执行一览(针对公司及部门) 正在使用该服务
        /// </summary>
        /// <param name="strOrgType">查询机构类型</param>
        /// <param name="strOrgID">查询机构ID</param>
        /// <param name="strOwnerId">查询人的员工ID</param>
        /// <param name="strBudgetRecordType">查询单据的类型(1:年度预算；2:月度预算；3:报销)</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<V_ExecutionList> GetExecutionListByPaging(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd, string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            IList<V_ExecutionList> entList = new List<V_ExecutionList>();
            ExecutionBLL bllExecution = new ExecutionBLL();
            entList = bllExecution.GetExecutionListByPaging(strOrgType, strOrgID, strOwnerId, iYear, iMonthStart, iMonthEnd, strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount).ToList();
            return entList.Count() > 0 ? entList.ToList() : null;
        }

        /// <summary>
        /// 获取信息
        /// 注：预算-查询分析-执行一览(针对岗位及个人) 正在使用该服务
        /// </summary>
        /// <param name="strOrgType">查询机构类型</param>
        /// <param name="strOrgID">查询机构ID</param>
        /// <param name="strOwnerId">查询人的员工ID</param>
        /// <param name="strBudgetRecordType">查询单据的类型(1:年度预算；2:月度预算；3:报销)</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<V_PerExecutionList> GetPerExecutionListByPaging(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd, string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            IList<V_PerExecutionList> entList = new List<V_PerExecutionList>();
            ExecutionBLL bllExecution = new ExecutionBLL();
            entList = bllExecution.GetPerExecutionListByPaging(strOrgType, strOrgID, strOwnerId, iYear, iMonthStart, iMonthEnd, strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount).ToList();
            return entList.Count() > 0 ? entList.ToList() : null;
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strOwnerId">查询人的员工ID</param>
        /// <param name="strBudgetRecordType">查询单据的类型(1:年度预算；2:月度预算；3:报销)</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<V_BudgetRecord> GetBudgetRecordListByPaging(string strOwnerId, int iBudgetRecordType, int iYear, int iMonthStart,
            int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            IList<V_BudgetRecord> entList = new List<V_BudgetRecord>();
            ExecutionBLL bllExecution = new ExecutionBLL();
            entList = bllExecution.GetBudgetRecordListByPaging(strOwnerId, iBudgetRecordType, iYear, iMonthStart, iMonthEnd, strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount).ToList();
            return entList.Count() > 0 ? entList.ToList() : null;
        }

        /// <summary>
        /// 获取个人借还款往来明细信息
        /// </summary>
        /// <param name="strOwnerId">查询人的员工ID</param>
        /// <param name="strBudgetRecordType">查询单据的类型(1:年度预算；2:月度预算；3:报销)</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<V_ContactDetail> GetContactDetailListByPaging(string strOrgType, string strOrgID, string strOwnerId,
            DateTime dtStart, DateTime dtEnd, string strFilter, List<object> objArgs, string strSortKey, int pageIndex,
            int pageSize, ref int pageCount, ref decimal dBeforeAccount, ref decimal dAfterAccount)
        {
            IList<V_ContactDetail> entList = new List<V_ContactDetail>();
            ExecutionBLL bllExecution = new ExecutionBLL();
            entList = bllExecution.GetContactDetailListByPaging(strOrgType, strOrgID, strOwnerId, dtStart, dtEnd, strFilter, objArgs, strSortKey, pageIndex,
            pageSize, ref pageCount, ref dBeforeAccount, ref dAfterAccount).ToList();
            return entList.Count() > 0 ? entList.ToList() : null;
        }

        /// <summary>
        /// 获取部门借还款往来明细信息
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="dTotalNormalBorrowMoney"></param>
        /// <param name="dTotalSpecialMoney"></param>
        /// <param name="dTotalReserveMoney"></param>
        /// <returns></returns>
        [OperationContract]
        public List<V_DeptContactDetail> GetDeptContactDetailListByPaging(string strOrgType, string strOrgID, string strOwnerId,
            DateTime dtStart, DateTime dtEnd, string strFilter, List<object> objArgs, string strSortKey, int pageIndex,
            int pageSize, ref int pageCount, ref decimal dTotalNormalBorrowMoney, ref decimal dTotalSpecialMoney, ref decimal dTotalReserveMoney)
        {
            IList<V_DeptContactDetail> entList = new List<V_DeptContactDetail>();
            ExecutionBLL bllExecution = new ExecutionBLL();
            entList = bllExecution.GetDeptContactDetailListByPaging(strOrgType, strOrgID, strOwnerId, dtStart, dtEnd, strFilter, objArgs, strSortKey, pageIndex,
            pageSize, ref pageCount, ref dTotalNormalBorrowMoney, ref dTotalSpecialMoney, ref dTotalReserveMoney).ToList();
            return entList.Count() > 0 ? entList.ToList() : null;
        }

        /// <summary>
        /// 预算台帐查询
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="strSortKey"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        [OperationContract]
        public List<V_StandingBook> GetStandingBookListByPaging(string strOrgType, string strOwnerId, DateTime dtStart, DateTime dtEnd, string strSortKey,
            string strFilter, List<object> objArgs, int pageIndex, int pageSize, ref int pageCount)
        {
            IList<V_StandingBook> entList = new List<V_StandingBook>();
            BudgetCheckBLL bllBudgetCheck = new BudgetCheckBLL();
            entList = bllBudgetCheck.GetStandingBookByMultSearch(strOrgType, strOwnerId, dtStart, dtEnd, strSortKey, strFilter, objArgs, pageIndex, pageSize, ref pageCount);
            return entList.Count() > 0 ? entList.ToList() : null;
        }

        /// <summary>
        /// 预算年度预算分析
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strSortKey"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        [OperationContract]
        public List<V_YearlyBudgetAnalysis> GetYearlyBudgetAnalysisListByPaging(string strOrgType, string strOwnerId, int iYear, int iMonthStart,
            int iMonthEnd, string strSortKey, string strFilter, List<object> objArgs, int pageIndex, int pageSize, ref int pageCount)
        {
            IList<V_YearlyBudgetAnalysis> entList = new List<V_YearlyBudgetAnalysis>();
            BudgetCheckBLL bllBudgetCheck = new BudgetCheckBLL();
            entList = bllBudgetCheck.GetYearlyBudgetAnalysisByMultSearch(strOrgType, strOwnerId, iYear, iMonthStart, iMonthEnd, strSortKey, strFilter, objArgs, pageIndex, pageSize, ref pageCount);
            return entList.Count() > 0 ? entList.ToList() : null;
        }

        /// <summary>
        /// 预算月度预算分析
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <param name="strSortKey"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        [OperationContract]
        public List<V_MonthlyBudgetAnalysis> GetMonthlyBudgetAnalysisListByPaging(string strOrgType, string strOwnerId, int iYear, int iMonth,
            string strSortKey, string strFilter, List<object> objArgs, int pageIndex, int pageSize, ref int pageCount)
        {
            IList<V_MonthlyBudgetAnalysis> entList = new List<V_MonthlyBudgetAnalysis>();
            BudgetCheckBLL bllBudgetCheck = new BudgetCheckBLL();
            entList = bllBudgetCheck.GetMonthlyBudgetAnalysisByMultSearch(strOrgType, strOwnerId, iYear, iMonth, strSortKey, strFilter, objArgs, pageIndex, pageSize, ref pageCount);
            return entList.Count() > 0 ? entList.ToList() : null;
        }

        /// <summary>
        /// 导出执行一览的台帐列表
        /// </summary>
        /// <param name="strOwnerId"></param>
        /// <param name="iBudgetRecordType"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFileBudgetRecordList(string strOwnerId, int iBudgetRecordType, int iYear, int iMonthStart,
            int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey)
        {
            ExecutionBLL bllExecution = new ExecutionBLL();
            byte[] byVac = bllExecution.OutFileBudgetRecordList(strOwnerId, iBudgetRecordType, iYear, iMonthStart, iMonthEnd, strFilter, objArgs, strSortKey);
            return byVac;
        }

        /// <summary>
        /// 导出部门执行一览数据(统计)
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFileExecutionList(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart,
            int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey)
        {
            ExecutionBLL bllExecution = new ExecutionBLL();
            byte[] byVac = bllExecution.OutFileExecutionList(strOrgType, strOrgID, strOwnerId, iYear, iMonthStart, iMonthEnd, strFilter, objArgs, strSortKey);
            return byVac;
        }

        /// <summary>
        /// 导出个人执行一览数据(统计)
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFilePerExecutionList(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd,
            string strFilter, List<object> objArgs, string strSortKey)
        {
            ExecutionBLL bllExecution = new ExecutionBLL();
            byte[] byVac = bllExecution.OutFilePerExecutionList(strOrgType, strOrgID, strOwnerId, iYear, iMonthStart, iMonthEnd, strFilter, objArgs, strSortKey);
            return byVac;
        }

        /// <summary>
        /// 导出部门执行一览数据(流水)
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFileDeptDayBookData(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart,
            int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey)
        {
            ExecutionBLL bllExecution = new ExecutionBLL();
            byte[] byVac = bllExecution.OutFileDeptDayBookData(strOrgType, strOrgID, strOwnerId, iYear, iMonthStart, iMonthEnd, strFilter, objArgs, strSortKey);
            return byVac;
        }

        /// <summary>
        /// 导出个人执行一览数据(流水)
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFilePerDayBookData(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd,
            string strFilter, List<object> objArgs, string strSortKey)
        {
            ExecutionBLL bllExecution = new ExecutionBLL();
            byte[] byVac = bllExecution.OutFilePerDayBookData(strOrgType, strOrgID, strOwnerId, iYear, iMonthStart, iMonthEnd, strFilter, objArgs, strSortKey);
            return byVac;
        }


        /// <summary>
        /// 导出个人借还款往来明细
        /// </summary>
        /// <param name="strFileType"></param>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <param name="dBeforeAccount"></param>
        /// <param name="dAfterAccount"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFileContactDetailList(string strFileType, string strOrgType, string strOrgID, string strOwnerId,
            DateTime dtStart, DateTime dtEnd, string strFilter, List<object> objArgs, string strSortKey, ref decimal dBeforeAccount, ref decimal dAfterAccount)
        {
            ExecutionBLL bllExecution = new ExecutionBLL();
            byte[] byVac = bllExecution.OutFileContactDetailListByMultSearch(strFileType, strOrgType, strOrgID, strOwnerId, dtStart,
                dtEnd, strFilter, objArgs, strSortKey, ref dBeforeAccount, ref dAfterAccount);
            return byVac;
        }

        /// <summary>
        /// 导出部门借还款往来明细
        /// </summary>
        /// <param name="strFileType"></param>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <param name="dTotalNormalBorrowMoney"></param>
        /// <param name="dTotalSpecialMoney"></param>
        /// <param name="dTotalReserveMoney"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFileDeptContactDetailList(string strFileType, string strOrgType, string strOrgID, string strOwnerId,
            DateTime dtStart, DateTime dtEnd, string strFilter, List<object> objArgs, string strSortKey, ref decimal dTotalNormalBorrowMoney,
                ref decimal dTotalSpecialMoney, ref decimal dTotalReserveMoney)
        {

            ExecutionBLL bllExecution = new ExecutionBLL();
            byte[] byVac = bllExecution.OutFileDeptContactDetailListByMultSearch(strFileType, strOrgType, strOrgID, strOwnerId, dtStart, dtEnd, strFilter, objArgs, strSortKey, ref dTotalNormalBorrowMoney, ref dTotalSpecialMoney, ref dTotalReserveMoney);
            return byVac;
        }
                
        /// <summary>
        /// 导出台帐查询列表
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="strSortKey"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFileStandingBookList(string strOrgType, string strOwnerId, DateTime dtStart, DateTime dtEnd, string strSortKey, string strFilter, List<object> objArgs)
        {
            BudgetCheckBLL bllExecution = new BudgetCheckBLL();
            byte[] byVac = bllExecution.OutFileStandingBookList(strOrgType, strOwnerId, dtStart, dtEnd, strSortKey, strFilter, objArgs);
            return byVac;
        }

        /// <summary>
        /// 导出月度预算分析列表
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <param name="strSortKey"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFileMonthlyBudgetAnalysisList(string strOrgType, string strOwnerId, int iYear, int iMonth,
            string strSortKey, string strFilter, List<object> objArgs)
        {
            BudgetCheckBLL bllExecution = new BudgetCheckBLL();
            byte[] byVac = bllExecution.OutFileMonthlyBudgetAnalysisList(strOrgType, strOwnerId, iYear, iMonth, strSortKey, strFilter, objArgs);
            return byVac;
        }

        /// <summary>
        /// 导出年度预算分析列表
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strSortKey"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        [OperationContract]
        public byte[] OutFileYearlyBudgetAnalysisList(string strOrgType, string strOwnerId, int iYear, int iMonthStart,
            int iMonthEnd, string strSortKey, string strFilter, List<object> objArgs)
        {
            BudgetCheckBLL bllExecution = new BudgetCheckBLL();
            byte[] byVac = bllExecution.OutFileYearlyBudgetAnalysisList(strOrgType, strOwnerId, iYear, iMonthStart, iMonthEnd, strSortKey, strFilter, objArgs);
            return byVac;
        }

        #endregion

        #region Lookup查询Entity的方法
        /// <summary>
        /// Lookup控件查询Entity的方法
        /// </summary> 
        /// <param name="userName">用户名称</param>
        /// <returns>Entity记录集Xml</returns>
        [OperationContract]
        public string GetLookupOjbects(FBAEnums.BLLPrefixNames prefixName, string modelCode, string userID, int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount)
        {
            string objxml = "";
            object ents = Utility.GetLookupData(prefixName, modelCode, userID, pageIndex, pageSize, sort, filterString, paras, ref pageCount);
            if (ents != null)
            {
                objxml = SerializerHelper.ContractObjectToXml(ents);
            }
            //object other = SerializerHelper.XmlToContractObject(objxml,typeof(T_HR_COMPANY[]));
            return objxml;
        }

        #endregion

        #region T_FB_SUBJECT 服务
        /// <summary>
        /// 根据机构ID及其组织架构类型获取该机构下所有的科目
        /// 注：预算-查询分析-执行一览正在使用该服务
        /// </summary>
        /// <param name="strOwnerID"></param>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgId"></param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_SUBJECT> GetSubjectListByOrgType(string strOwnerID, string strOrgType, string strOrgId)
        {
            SubjectBLL bllSubject = new SubjectBLL();
            List<T_FB_SUBJECT> entList = bllSubject.GetSubjectListByOrgType(strOwnerID, strOrgType, strOrgId);

            return entList.Count() > 0 ? entList : null;
        }
        #endregion

    }
}
