/*
 * 文件名：ExecutionBLL.cs
 * 作  用：预算查询分析专用
 * 创建人：吴鹏
 * 创建时间：2011-01-27 15:33:04
 * 修改人：
 * 修改时间：
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.FBAnalysis.CustomModel;
using SMT.FBAnalysis.DAL;
using TM_SaaS_OA_EFModel;
using SMT.Foundation.Log;
using System.Linq.Dynamic;
using System.Data;
using SMT.SaaS.BLLCommonServices;

namespace SMT.FBAnalysis.BLL
{
    public class ExecutionBLL : BaseBll<T_FB_BUDGETCHECK>, ILookupEntity
    {
        #region 查询页面使用函数
        /// <summary>
        /// 执行一览查询分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>V_ExecutionList信息</returns>
        public List<V_ExecutionList> GetExecutionListByPaging(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd, string strFilter,
            List<object> objArgs, string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            List<V_ExecutionList> executionList = new List<V_ExecutionList>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "T_FB_BUDGETCHECK");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            executionList = dalExecution.GetExecutionListForDepartment(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs.ToArray());
            return Utility.PagerList<V_ExecutionList>(executionList, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 个人执行一览
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
        public List<V_PerExecutionList> GetPerExecutionListByPaging(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            List<V_PerExecutionList> executionList = new List<V_PerExecutionList>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "T_FB_BUDGETCHECK");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            executionList = dalExecution.GetExecutionListForPerson(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs.ToArray());
            return Utility.PagerList<V_PerExecutionList>(executionList, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 执行一览明细分页查询
        /// </summary>
        /// <param name="strOwnerId"></param>
        /// <param name="iBudgetRecordType"></param>
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
        public List<V_BudgetRecord> GetBudgetRecordListByPaging(string strOwnerId, int iBudgetRecordType, int iYear, int iMonthStart,
           int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            List<V_BudgetRecord> entBudgetRecordList = new List<V_BudgetRecord>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " UPDATEDATE ";
            }

            entBudgetRecordList = dalExecution.GetBudgetRecordListByMultSearch(iBudgetRecordType, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs.ToArray());
            return Utility.PagerList<V_BudgetRecord>(entBudgetRecordList, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 借支分页查询
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
        /// <param name="dBeforeAccount"></param>
        /// <param name="dAfterAccount"></param>
        /// <returns></returns>
        public List<V_ContactDetail> GetContactDetailListByPaging(string strOrgType, string strOrgID, string strOwnerId,
            DateTime dtStart, DateTime dtEnd, string strFilter, List<object> objArgs, string strSortKey, int pageIndex,
            int pageSize, ref int pageCount, ref decimal dBeforeAccount, ref decimal dAfterAccount)
        {
            List<V_ContactDetail> contactDetailList = new List<V_ContactDetail>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "ContactDetailsView");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "T_FB_SUBJECT.SUBJECTID";
            }

            contactDetailList = dalExecution.GetContactDetailListByMultSearch(strOrgType, strOrgID, dtStart, dtEnd, strOrderBy, ref dBeforeAccount, ref dAfterAccount, strFilter, objArgs.ToArray());
            return Utility.PagerList<V_ContactDetail>(contactDetailList, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 借支分页查询
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
        /// <param name="dBeforeAccount"></param>
        /// <param name="dAfterAccount"></param>
        /// <returns></returns>
        public List<V_DeptContactDetail> GetDeptContactDetailListByPaging(string strOrgType, string strOrgID, string strOwnerId,
            DateTime dtStart, DateTime dtEnd, string strFilter, List<object> objArgs, string strSortKey, int pageIndex,
            int pageSize, ref int pageCount, ref decimal dTotalNormalBorrowMoney, ref decimal dTotalSpecialMoney, ref decimal dTotalReserveMoney)
        {
            List<V_DeptContactDetail> contactDetailList = new List<V_DeptContactDetail>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "ContactDetailsView");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "T_FB_SUBJECT.SUBJECTID";
            }

            contactDetailList = dalExecution.GetDeptContactDetailListByMultSearch(strOrgType, strOrgID, dtStart, dtEnd, strOrderBy, ref dTotalNormalBorrowMoney, ref dTotalSpecialMoney, ref dTotalReserveMoney, strFilter, objArgs.ToArray());
            return Utility.PagerList<V_DeptContactDetail>(contactDetailList, pageIndex, pageSize, ref pageCount);
        }
        #endregion 查询页面使用函数

        #region 文件导出

        #region 导出执行一览(统计)对应的台帐列表
        /// <summary>
        /// 导出执行一览对应的台帐列表
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
        public byte[] OutFileBudgetRecordList(string strOwnerId, int iBudgetRecordType, int iYear, int iMonthStart, int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey)
        {
            List<V_BudgetRecord> entBudgetRecordList = new List<V_BudgetRecord>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " UPDATEDATE ";
            }

            entBudgetRecordList = dalExecution.GetBudgetRecordListByMultSearch(iBudgetRecordType, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs.ToArray());

            DataTable dt = MakeTableToExportBudgetRecord();
            DataTable dtExport = GetExportDataForBudgetRecord(strOwnerId, dt, entBudgetRecordList);

            return Utility.OutFileStream("部门执行一览台帐明细列表", dtExport);
        }

        private DataTable MakeTableToExportBudgetRecord()
        {
            DataTable dt = new DataTable();

            DataColumn colRecordCode = new DataColumn();
            colRecordCode.ColumnName = "单据编号";
            colRecordCode.DataType = typeof(string);
            dt.Columns.Add(colRecordCode);
            
            DataColumn colSubjectName = new DataColumn();
            colSubjectName.ColumnName = "科目名称";
            colSubjectName.DataType = typeof(string);
            dt.Columns.Add(colSubjectName);

            DataColumn colRemark = new DataColumn();
            colRemark.ColumnName = "摘要";
            colRemark.DataType = typeof(string);
            dt.Columns.Add(colRemark);

            DataColumn colTotalMoney = new DataColumn();
            colTotalMoney.ColumnName = "金额";
            colTotalMoney.DataType = typeof(decimal);
            dt.Columns.Add(colTotalMoney);

            DataColumn colOwnerName = new DataColumn();
            colOwnerName.ColumnName = "申请人";
            colOwnerName.DataType = typeof(string);
            dt.Columns.Add(colOwnerName);

            DataColumn colDepartmentName = new DataColumn();
            colDepartmentName.ColumnName = "部门";
            colDepartmentName.DataType = typeof(string);
            dt.Columns.Add(colDepartmentName);

            DataColumn colUpdateDate = new DataColumn();
            colUpdateDate.ColumnName = "更新日期";
            colUpdateDate.DataType = typeof(string);
            dt.Columns.Add(colUpdateDate);

            DataColumn colCheckState = new DataColumn();
            colCheckState.ColumnName = "审核状态";
            colCheckState.DataType = typeof(string);
            dt.Columns.Add(colCheckState);

            return dt;
        }

        private DataTable GetExportDataForBudgetRecord(string strOwnerId, DataTable dt, List<V_BudgetRecord> ents)
        {
            dt.Rows.Clear();

            Dictionary<decimal, string> dictCheckStates = new Dictionary<decimal, string>();
            dictCheckStates.Add(0, "未提交");
            dictCheckStates.Add(1, "审核中");
            dictCheckStates.Add(2, "审核通过");
            dictCheckStates.Add(3, "审核不通过");

            List<SMT.SaaS.BLLCommonServices.OrganizationWS.V_DEPARTMENT> entDeps = GetAllDepartment(strOwnerId);

            for (int i = 0; i < ents.Count(); i++)
            {
                DataRow row = dt.NewRow();
                for (int n = 0; n < dt.Columns.Count; n++)
                {
                    switch (n)
                    {
                        case 0:
                            row[n] = ents[i].RecordCode;
                            break;
                        case 1:
                            row[n] = ents[i].SubjectName;
                            break;
                        case 2:
                            row[n] = ents[i].Remark;
                            break;
                        case 3:
                            row[n] = ents[i].TotalMoney;
                            break;
                        case 4:
                            row[n] = ents[i].OwnerName;
                            break;
                        case 5:
                            row[n] = GetDepName(ents[i].DepartmentID, entDeps);
                            break;
                        case 6:
                            row[n] = ents[i].UpdateDate;
                            break;
                        case 7:
                            row[n] = dictCheckStates[ents[i].CheckState];
                            break;                       
                    }
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion 导出执行一览(统计)对应的台帐列表

        #region 部门的执行一览的导出数据(统计)
        /// <summary>
        /// 获取部门的执行一览的导出数据
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
        public byte[] OutFileExecutionList(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey)
        {
            List<V_ExecutionList> executionList = new List<V_ExecutionList>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "T_FB_BUDGETCHECK");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            executionList = dalExecution.GetExecutionListForDepartment(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs.ToArray());

            DataTable dt = MakeTableToExportExecution(iMonthStart, iMonthEnd);
            DataTable dtExport = GetExportDataForExecution(strOwnerId, dt, executionList);
            string strCustomerBodyHeader = GetCustomerBodyHeaderForExecution(dt);

            return Utility.OutFileStream("部门执行一览", dtExport, strCustomerBodyHeader);
        }

        /// <summary>
        /// 自定义表头
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetCustomerBodyHeaderForExecution(DataTable dt)
        {
            StringBuilder s = new StringBuilder();
            s.Append("<tr>");
            for (int i = 0; i < 4; i++)
            {
                s.Append("<td class='x1281' rowspan='2'>" + dt.Columns[i].Caption.ToString().Replace("*" + i, "") + "</td>");
            }
            s.Append("<td class='x1281' colspan='15' align='center'>报销费用</td>");            
            s.Append("<td class='x1281' colspan='3' align='center'>预算结余</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            for (int i = 4; i < 19; i++)
            {
                s.Append("<td class='x1281'>" + dt.Columns[i].Caption.ToString().Replace("*" + i, "") + "</td>");
            }
            s.Append("<td class='x1281'>年度执行结余</td>");
            s.Append("<td class='x1281'>年度预算</td>");
            s.Append("<td class='x1281'>月度预算</td>");
            s.Append("</tr>");

            return s.ToString();
        }

        private DataTable MakeTableToExportExecution(int iMonthStart, int iMonthEnd)
        {
            DataTable dt = new DataTable();

            DataColumn colOrgObj = new DataColumn();
            colOrgObj.ColumnName = "机构";
            colOrgObj.DataType = typeof(string);
            dt.Columns.Add(colOrgObj);

            DataColumn colSubjectName = new DataColumn();
            colSubjectName.ColumnName = "项目";
            colSubjectName.DataType = typeof(string);
            dt.Columns.Add(colSubjectName);

            DataColumn colBudgetMoneyYear = new DataColumn();
            colBudgetMoneyYear.ColumnName = "年度预算总额";
            colBudgetMoneyYear.DataType = typeof(decimal);
            dt.Columns.Add(colBudgetMoneyYear);

            DataColumn colBudgetMoneyMonth = new DataColumn();
            colBudgetMoneyMonth.ColumnName = "月度预算总额";
            colBudgetMoneyMonth.DataType = typeof(decimal);
            dt.Columns.Add(colBudgetMoneyMonth);

            for (int i = 0; i < 12; i++)
            {
 
            }

            DataColumn colJanApprovedMoney = new DataColumn();
            colJanApprovedMoney.ColumnName = "一月";
            colJanApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colJanApprovedMoney);

            DataColumn colFebApprovedMoney = new DataColumn();
            colFebApprovedMoney.ColumnName = "二月";
            colFebApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colFebApprovedMoney);

            DataColumn colMarApprovedMoney = new DataColumn();
            colMarApprovedMoney.ColumnName = "三月";
            colMarApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colMarApprovedMoney);

            DataColumn colAprApprovedMoney = new DataColumn();
            colAprApprovedMoney.ColumnName = "四月";
            colAprApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colAprApprovedMoney);

            DataColumn colMayApprovedMoney = new DataColumn();
            colMayApprovedMoney.ColumnName = "五月";
            colMayApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colMayApprovedMoney);

            DataColumn colJunApprovedMoney = new DataColumn();
            colJunApprovedMoney.ColumnName = "六月";
            colJunApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colJunApprovedMoney);

            DataColumn colJulApprovedMoney = new DataColumn();
            colJulApprovedMoney.ColumnName = "七月";
            colJulApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colJulApprovedMoney);

            DataColumn colAugApprovedMoney = new DataColumn();
            colAugApprovedMoney.ColumnName = "八月";
            colAugApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colAugApprovedMoney);

            DataColumn colSepApprovedMoney = new DataColumn();
            colSepApprovedMoney.ColumnName = "九月";
            colSepApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colSepApprovedMoney);

            DataColumn colOctApprovedMoney = new DataColumn();
            colOctApprovedMoney.ColumnName = "十月";
            colOctApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colOctApprovedMoney);

            DataColumn colNovApprovedMoney = new DataColumn();
            colNovApprovedMoney.ColumnName = "十一月";
            colNovApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colNovApprovedMoney);

            DataColumn colDecApprovedMoney = new DataColumn();
            colDecApprovedMoney.ColumnName = "十二月";
            colDecApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colDecApprovedMoney);

            DataColumn colApprovedApplyMoney = new DataColumn();
            colApprovedApplyMoney.ColumnName = "已审核";
            colApprovedApplyMoney.DataType = typeof(decimal);
            dt.Columns.Add(colApprovedApplyMoney);

            DataColumn colApprovingApplyMoney = new DataColumn();
            colApprovingApplyMoney.ColumnName = "审核中";
            colApprovingApplyMoney.DataType = typeof(decimal);
            dt.Columns.Add(colApprovingApplyMoney);

            DataColumn colSubtotal = new DataColumn();
            colSubtotal.ColumnName = "报销小计";
            colSubtotal.DataType = typeof(decimal);
            dt.Columns.Add(colSubtotal);

            DataColumn colBudgetYearBalance = new DataColumn();
            colBudgetYearBalance.ColumnName = "年度执行结余";
            colBudgetYearBalance.DataType = typeof(decimal);
            dt.Columns.Add(colBudgetYearBalance);

            DataColumn colBudgetYear = new DataColumn();
            colBudgetYear.ColumnName = "年度预算";
            colBudgetYear.DataType = typeof(decimal);
            dt.Columns.Add(colBudgetYear);

            DataColumn colBudgetMonth = new DataColumn();
            colBudgetMonth.ColumnName = "月度预算";
            colBudgetMonth.DataType = typeof(decimal);
            dt.Columns.Add(colBudgetMonth);

            return dt;
        }

        private DataTable GetExportDataForExecution(string strOwnerId, DataTable dt, List<V_ExecutionList> ents)
        {
            dt.Rows.Clear();

            List<SMT.SaaS.BLLCommonServices.PermissionWS.T_SYS_DICTIONARY> entDicts = GetAllDictionary();
            List<SMT.SaaS.BLLCommonServices.OrganizationWS.V_DEPARTMENT> entDeps = GetAllDepartment(strOwnerId);

            for (int i = 0; i < ents.Count(); i++)
            {
                DataRow row = dt.NewRow();
                for (int n = 0; n < dt.Columns.Count; n++)
                {
                    switch (n)
                    {
                        case 0:
                            row[n] = ents[i].OrganizationName;
                            break;
                        case 1:
                            row[n] = ents[i].SubjectName;
                            break;
                        case 2:
                            row[n] = ents[i].BudgetMoneyYear;
                            break;
                        case 3:
                            row[n] = ents[i].BudgetMoneyMonth;
                            break;
                        case 4:
                            row[n] = ents[i].JanApprovedMoney;
                            break;
                        case 5:
                            row[n] = ents[i].FebApprovedMoney;
                            break;
                        case 6:
                            row[n] = ents[i].MarApprovedMoney;
                            break;
                        case 7:
                            row[n] = ents[i].AprApprovedMoney;
                            break;
                        case 8:
                            row[n] = ents[i].MayApprovedMoney;
                            break;
                        case 9:
                            row[n] = ents[i].JunApprovedMoney;
                            break;
                        case 10:
                            row[n] = ents[i].JulApprovedMoney;
                            break;
                        case 11:
                            row[n] = ents[i].AugApprovedMoney;
                            break;
                        case 12:
                            row[n] = ents[i].SepApprovedMoney;
                            break;
                        case 13:
                            row[n] = ents[i].OctApprovedMoney;
                            break;
                        case 14:
                            row[n] = ents[i].NovApprovedMoney;
                            break;
                        case 15:
                            row[n] = ents[i].DecApprovedMoney;
                            break;
                        case 16:
                            row[n] = ents[i].ApprovedApplyMoney;
                            break;
                        case 17:
                            row[n] = ents[i].ApprovingApplyMoney;
                            break;
                        case 18:
                            row[n] = ents[i].Subtotal;
                            break;
                        case 19:
                            row[n] = ents[i].BudgetYearBalance;
                            break;
                        case 20:
                            row[n] = ents[i].BudgetYear;
                            break;
                        case 21:
                            row[n] = ents[i].BudgetMonth;
                            break;                        
                    }
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// 获取部门名称
        /// </summary>
        /// <param name="strDepId"></param>
        /// <param name="entDeps"></param>
        /// <returns></returns>
        private string GetDepName(string strDepId, List<SMT.SaaS.BLLCommonServices.OrganizationWS.V_DEPARTMENT> entDeps)
        {
            string strRes = string.Empty;
            if (string.IsNullOrWhiteSpace(strDepId))
            {
                return strRes;
            }

            if (entDeps == null)
            {
                return strRes;
            }

            var objs = from a in entDeps
                       where a.DEPARTMENTID == strDepId
                       select a;


            SMT.SaaS.BLLCommonServices.OrganizationWS.V_DEPARTMENT dept = objs.Count() > 0 ? objs.FirstOrDefault() : null;
            return dept == null ? strRes : dept.DEPARTMENTNAME;
        }

        #endregion 获取部门的执行一览的导出数据

        #region 个人的执行一览的导出数据(统计)
        /// <summary>
        /// 获取个人的执行一览的导出数据
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
        public byte[] OutFilePerExecutionList(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey)
        {
            List<V_PerExecutionList> executionList = new List<V_PerExecutionList>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "T_FB_BUDGETCHECK");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            executionList = dalExecution.GetExecutionListForPerson(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs.ToArray());

            DataTable dt = MakeTableToExportPerExecution();
            DataTable dtExport = GetExportDataForPerExecution(dt, executionList);
            string strCustomerBodyHeader = GetCustomerBodyHeaderForPerExecution(dt);

            return Utility.OutFileStream("个人执行一览", dtExport, strCustomerBodyHeader);
        }

        /// <summary>
        /// 自定义表头
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetCustomerBodyHeaderForPerExecution(DataTable dt)
        {
            StringBuilder s = new StringBuilder();
            s.Append("<tr>");
            for (int i = 0; i < 3; i++)
            {
                s.Append("<td class='x1281' rowspan='2'>" + dt.Columns[i].Caption.ToString().Replace("*" + i, "") + "</td>");
            }
            int icols = dt.Columns.Count - 3;
            s.Append("<td class='x1281' colspan=\"" + icols + "\" align='center'>报销费用</td>");
            s.Append("</tr>");
            s.Append("<tr>");
            for (int i = 3; i < dt.Columns.Count; i++)
            {
                s.Append("<td class='x1281'>" + dt.Columns[i].Caption.ToString().Replace("*" + i, "") + "</td>");
            }
            s.Append("</tr>");

            return s.ToString();
        }

        /// <summary>
        /// 构造导出Excel需要的填充数据的DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable MakeTableToExportPerExecution()
        {
            DataTable dt = new DataTable();

            DataColumn colOrgObj = new DataColumn();
            colOrgObj.ColumnName = "机构";
            colOrgObj.DataType = typeof(string);
            dt.Columns.Add(colOrgObj);

            DataColumn colOwnerName = new DataColumn();
            colOwnerName.ColumnName = "员工姓名";
            colOwnerName.DataType = typeof(string);
            dt.Columns.Add(colOwnerName);

            DataColumn colSubjectName = new DataColumn();
            colSubjectName.ColumnName = "项目";
            colSubjectName.DataType = typeof(string);
            dt.Columns.Add(colSubjectName);

            DataColumn colJanApprovedMoney = new DataColumn();
            colJanApprovedMoney.ColumnName = "一月";
            colJanApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colJanApprovedMoney);

            DataColumn colFebApprovedMoney = new DataColumn();
            colFebApprovedMoney.ColumnName = "二月";
            colFebApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colFebApprovedMoney);

            DataColumn colMarApprovedMoney = new DataColumn();
            colMarApprovedMoney.ColumnName = "三月";
            colMarApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colMarApprovedMoney);

            DataColumn colAprApprovedMoney = new DataColumn();
            colAprApprovedMoney.ColumnName = "四月";
            colAprApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colAprApprovedMoney);

            DataColumn colMayApprovedMoney = new DataColumn();
            colMayApprovedMoney.ColumnName = "五月";
            colMayApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colMayApprovedMoney);

            DataColumn colJunApprovedMoney = new DataColumn();
            colJunApprovedMoney.ColumnName = "六月";
            colJunApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colJunApprovedMoney);

            DataColumn colJulApprovedMoney = new DataColumn();
            colJulApprovedMoney.ColumnName = "七月";
            colJulApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colJulApprovedMoney);

            DataColumn colAugApprovedMoney = new DataColumn();
            colAugApprovedMoney.ColumnName = "八月";
            colAugApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colAugApprovedMoney);

            DataColumn colSepApprovedMoney = new DataColumn();
            colSepApprovedMoney.ColumnName = "九月";
            colSepApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colSepApprovedMoney);

            DataColumn colOctApprovedMoney = new DataColumn();
            colOctApprovedMoney.ColumnName = "十月";
            colOctApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colOctApprovedMoney);

            DataColumn colNovApprovedMoney = new DataColumn();
            colNovApprovedMoney.ColumnName = "十一月";
            colNovApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colNovApprovedMoney);

            DataColumn colDecApprovedMoney = new DataColumn();
            colDecApprovedMoney.ColumnName = "十二月";
            colDecApprovedMoney.DataType = typeof(decimal);
            dt.Columns.Add(colDecApprovedMoney);

            DataColumn colApprovedApplyMoney = new DataColumn();
            colApprovedApplyMoney.ColumnName = "已审核";
            colApprovedApplyMoney.DataType = typeof(decimal);
            dt.Columns.Add(colApprovedApplyMoney);

            DataColumn colApprovingApplyMoney = new DataColumn();
            colApprovingApplyMoney.ColumnName = "审核中";
            colApprovingApplyMoney.DataType = typeof(decimal);
            dt.Columns.Add(colApprovingApplyMoney);

            DataColumn colSubtotal = new DataColumn();
            colSubtotal.ColumnName = "小计";
            colSubtotal.DataType = typeof(decimal);
            dt.Columns.Add(colSubtotal);

            return dt;
        }

        /// <summary>
        /// 填充实体集数据到DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="contactDetailList"></param>
        /// <returns></returns>
        private DataTable GetExportDataForPerExecution(DataTable dt, List<V_PerExecutionList> ents)
        {
            dt.Rows.Clear();

            List<SMT.SaaS.BLLCommonServices.PermissionWS.T_SYS_DICTIONARY> entDicts = GetAllDictionary();

            for (int i = 0; i < ents.Count(); i++)
            {
                DataRow row = dt.NewRow();
                for (int n = 0; n < dt.Columns.Count; n++)
                {
                    switch (n)
                    {
                        case 0:
                            row[n] = ents[i].OrganizationName;
                            break;
                        case 1:
                            row[n] = ents[i].OwnerName;
                            break;
                        case 2:
                            row[n] = ents[i].SubjectName;
                            break;
                        case 3:
                            row[n] = ents[i].JanApprovedMoney;
                            break;
                        case 4:
                            row[n] = ents[i].FebApprovedMoney;
                            break;
                        case 5:
                            row[n] = ents[i].MarApprovedMoney;
                            break;
                        case 6:
                            row[n] = ents[i].AprApprovedMoney;
                            break;
                        case 7:
                            row[n] = ents[i].MayApprovedMoney;
                            break;
                        case 8:
                            row[n] = ents[i].JunApprovedMoney;
                            break;
                        case 9:
                            row[n] = ents[i].JulApprovedMoney;
                            break;
                        case 10:
                            row[n] = ents[i].AugApprovedMoney;
                            break;
                        case 11:
                            row[n] = ents[i].SepApprovedMoney;
                            break;
                        case 12:
                            row[n] = ents[i].OctApprovedMoney;
                            break;
                        case 13:
                            row[n] = ents[i].NovApprovedMoney;
                            break;
                        case 14:
                            row[n] = ents[i].DecApprovedMoney;
                            break;
                        case 15:
                            row[n] = ents[i].ApprovedApplyMoney;
                            break;
                        case 16:
                            row[n] = ents[i].ApprovingApplyMoney;
                            break;
                        case 17:
                            row[n] = ents[i].Subtotal;
                            break;
                    }
                }

                dt.Rows.Add(row);
            }

            return dt;
        }
        #endregion 个人的执行一览的导出数据(统计)

        #region 部门的执行一览的导出数据(流水)
        public byte[] OutFileDeptDayBookData(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey)
        {
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "T_FB_BUDGETCHECK");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            DataTable dtYearlyDept = dalExecution.GetDepartmentYearlyBudgetDaybook(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs);
            DataTable dtMonthlyDept = dalExecution.GetDepartmentMonthlyBudgetDaybook(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs);
            DataTable dtDeptCharge = dalExecution.GetDepartmentChargeDaybook(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs);
            DataTable dtMonthlyPer = dalExecution.GetPersonMonthlyBudgetDaybook(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs);
            DataTable dtPerCharge = dalExecution.GetPersonChargeDaybook(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs);

            string strHeaderTitle = "部门预算及报销流水一览";
            string strHeaderTitlePrefix = string.Empty;
            StringBuilder strBody = new StringBuilder();
            StringBuilder strYearlyBody = new StringBuilder();
            StringBuilder strMonthlyBody = new StringBuilder();
            StringBuilder strChargeBody = new StringBuilder();
            int cols = 0;

            if (dtYearlyDept.Rows.Count > 0)
            {
                strHeaderTitlePrefix = dtYearlyDept.Rows[0]["所属公司"] + " " + dtYearlyDept.Rows[0]["所属部门"] + " " + dtYearlyDept.Rows[0]["科目名称"];
                string strCustomerBodyHeader = GetCustomerBodyHeaderForDeptDayBook(dtYearlyDept, strHeaderTitlePrefix + "年度预算流水一览");
                strYearlyBody = Utility.GetBodyWithNoTitle(dtYearlyDept, strCustomerBodyHeader);
                cols = dtYearlyDept.Columns.Count;
            }

            if (dtMonthlyDept.Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(strHeaderTitlePrefix))
                {
                    strHeaderTitlePrefix = dtMonthlyDept.Rows[0]["所属公司"] + " " + dtMonthlyDept.Rows[0]["所属部门"] + " " + dtMonthlyDept.Rows[0]["科目名称"];
                }

                string strCustomerBodyHeader = GetCustomerBodyHeaderForDeptDayBook(dtMonthlyDept, strHeaderTitlePrefix + "月度预算流水一览");
                strMonthlyBody = Utility.GetBodyWithNoTitle(dtMonthlyDept, strCustomerBodyHeader);

                if (cols < dtMonthlyDept.Columns.Count)
                {
                    cols = dtMonthlyDept.Columns.Count;
                }
            }


            if (dtDeptCharge.Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(strHeaderTitlePrefix))
                {
                    strHeaderTitlePrefix = dtDeptCharge.Rows[0]["所属公司"] + " " + dtDeptCharge.Rows[0]["所属部门"] + " " + dtDeptCharge.Rows[0]["科目名称"];
                }

                string strCustomerBodyHeader = GetCustomerBodyHeaderForDeptDayBook(dtDeptCharge, strHeaderTitlePrefix + "报销，借款及还款流水一览");
                strChargeBody = Utility.GetBodyWithNoTitle(dtDeptCharge, strCustomerBodyHeader);

                if (cols < dtMonthlyDept.Columns.Count)
                {
                    cols = dtDeptCharge.Columns.Count;
                }
            }

            if (dtMonthlyPer.Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(strHeaderTitlePrefix))
                {
                    strHeaderTitlePrefix = dtMonthlyPer.Rows[0]["所属员工"] + " " + dtMonthlyPer.Rows[0]["科目名称"];
                }

                string strCustomerBodyHeader = GetCustomerBodyHeaderForDeptDayBook(dtMonthlyPer, strHeaderTitlePrefix + "月度预算流水一览");
                strMonthlyBody.Append(Utility.GetBodyWithNoTitle(dtMonthlyPer, strCustomerBodyHeader)).ToString();

                cols = dtMonthlyDept.Columns.Count;

            }


            if (dtPerCharge.Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(strHeaderTitlePrefix))
                {
                    strHeaderTitlePrefix = dtPerCharge.Rows[0]["所属员工"] + " " + dtPerCharge.Rows[0]["科目名称"];
                }

                string strCustomerBodyHeader = GetCustomerBodyHeaderForDeptDayBook(dtPerCharge, strHeaderTitlePrefix + "报销，借款及还款流水一览");
                strChargeBody.Append(Utility.GetBodyWithNoTitle(dtPerCharge, strCustomerBodyHeader).ToString());

                if (cols < dtMonthlyDept.Columns.Count)
                {
                    cols = dtDeptCharge.Columns.Count;
                }
            }

            strHeaderTitle = strHeaderTitlePrefix + strHeaderTitle;

            strBody.Append("<body>\n\r");
            strBody.Append("<table ID=\"Table0\" BORDER=0 CELLSPACING=1 CELLPADDING=3 width=100% align=center>\n\r");
            strBody.Append("<tr>");
            strBody.Append("<td colspan=\"" + cols + "\" align=center class=\"title\">" + strHeaderTitle + "</td>");
            strBody.Append("</tr>\n\r");
            strBody.Append("</table>\n\r");
            strBody.Append(strYearlyBody.ToString());
            strBody.Append(strMonthlyBody.ToString());
            strBody.Append(strChargeBody.ToString());
            strBody.Append("</body></html>");

            return Utility.OutFileStream(strBody.ToString());
        }

        /// <summary>
        /// 自定义表头
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetCustomerBodyHeaderForDeptDayBook(DataTable dt, string strPartTitle)
        {
            int iColumnCount = dt.Columns.Count;
            StringBuilder s = new StringBuilder();
            s.Append("<tr>");
            s.Append("<td class='x1281' colspan='" + iColumnCount.ToString() + "' align='left'>" + strPartTitle + "</td>");
            s.Append("<tr>");
            for (int i = 0; i < iColumnCount; i++)
            {
                s.Append("<td class='x1281'>" + dt.Columns[i].Caption.ToString() + "</td>");
            }
            s.Append("</tr>");

            return s.ToString();
        }

        #endregion 获取部门的执行一览的导出数据(流水)

        #region 个人的执行一览的导出数据(流水)
        /// <summary>
        /// 获取个人的执行一览的导出数据
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
        public byte[] OutFilePerDayBookData(string strOrgType, string strOrgID, string strOwnerId, int iYear, int iMonthStart, int iMonthEnd, string strFilter, List<object> objArgs, string strSortKey)
        {
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "T_FB_BUDGETCHECK");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            DataTable dtMonthlyDept = dalExecution.GetPersonMonthlyBudgetDaybook(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs);
            DataTable dtDeptCharge = dalExecution.GetPersonChargeDaybook(strOrgType, strOrgID, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs);

            string strHeaderTitle = "个人预算及报销流水一览";
            string strHeaderTitlePrefix = string.Empty;
            StringBuilder strBody = new StringBuilder();
            StringBuilder strMonthlyBody = new StringBuilder();
            StringBuilder strChargeBody = new StringBuilder();
            int cols = 0;


            if (dtMonthlyDept.Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(strHeaderTitlePrefix))
                {
                    strHeaderTitlePrefix = dtMonthlyDept.Rows[0]["所属员工"] + " " + dtMonthlyDept.Rows[0]["科目名称"];
                }

                string strCustomerBodyHeader = GetCustomerBodyHeaderForDeptDayBook(dtMonthlyDept, strHeaderTitlePrefix + "月度预算流水一览");
                strMonthlyBody = Utility.GetBodyWithNoTitle(dtMonthlyDept, strCustomerBodyHeader);

                cols = dtMonthlyDept.Columns.Count;

            }


            if (dtDeptCharge.Rows.Count > 0)
            {
                if (string.IsNullOrWhiteSpace(strHeaderTitlePrefix))
                {
                    strHeaderTitlePrefix = dtDeptCharge.Rows[0]["所属员工"] + " " + dtDeptCharge.Rows[0]["科目名称"];
                }

                string strCustomerBodyHeader = GetCustomerBodyHeaderForDeptDayBook(dtDeptCharge, strHeaderTitlePrefix + "报销，借款及还款流水一览");
                strChargeBody = Utility.GetBodyWithNoTitle(dtDeptCharge, strCustomerBodyHeader);

                if (cols < dtMonthlyDept.Columns.Count)
                {
                    cols = dtDeptCharge.Columns.Count;
                }
            }

            strHeaderTitle = strHeaderTitlePrefix + strHeaderTitle;

            strBody.Append("<body>\n\r");
            strBody.Append("<table ID=\"Table0\" BORDER=0 CELLSPACING=1 CELLPADDING=3 width=100% align=center>\n\r");
            strBody.Append("<tr>");
            strBody.Append("<td colspan=\"" + cols + "\" align=center class=\"title\">" + strHeaderTitle + "</td>");
            strBody.Append("</tr>\n\r");
            strBody.Append("</table>\n\r");
            strBody.Append(strMonthlyBody.ToString());
            strBody.Append(strChargeBody.ToString());
            strBody.Append("</body></html>");

            return Utility.OutFileStream(strBody.ToString());
        }

        /// <summary>
        /// 自定义表头
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private string GetCustomerBodyHeaderForPerDayBook(DataTable dt, string strPartTitle)
        {
            int iColumnCount = dt.Columns.Count;
            StringBuilder s = new StringBuilder();
            s.Append("<tr>");
            s.Append("<td class='x1281' colspan='" + iColumnCount.ToString() + "' align='left'>" + strPartTitle + "</td>");
            s.Append("<tr>");
            for (int i = 0; i < iColumnCount; i++)
            {
                s.Append("<td class='x1281'>" + dt.Columns[i].Caption.ToString() + "</td>");
            }
            s.Append("</tr>");

            return s.ToString();
        }

        #endregion 个人的执行一览的导出数据

        #region 获取个人的借还款往来的导出数据
        /// <summary>
        /// 获取个人的借还款往来的导出数据
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
        public byte[] OutFileContactDetailListByMultSearch(string strFileType, string strOrgType, string strOrgID, string strOwnerId, DateTime dtStart, DateTime dtEnd, string strFilter, List<object> objArgs, string strSortKey, ref decimal dBeforeAccount, ref decimal dAfterAccount)
        {
            List<V_ContactDetail> contactDetailList = new List<V_ContactDetail>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "ContactDetailsView");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "T_FB_SUBJECT.SUBJECTID";
            }

            contactDetailList = dalExecution.GetContactDetailListByMultSearch(strOrgType, strOrgID, dtStart, dtEnd, strOrderBy, ref dBeforeAccount, ref dAfterAccount, strFilter, objArgs.ToArray());

            DataTable dt = MakeTableToExportContactDetail();
            DataTable dtExport = GetExportDataForContactDetail(dt, contactDetailList);

            return Utility.OutFileStream("个人借还款往来", dtExport);
        }

        /// <summary>
        /// 构造导出Excel需要的填充数据的DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable MakeTableToExportContactDetail()
        {
            DataTable dt = new DataTable();

            DataColumn colRecordCode = new DataColumn();
            colRecordCode.ColumnName = "单据编号";
            colRecordCode.DataType = typeof(string);
            dt.Columns.Add(colRecordCode);

            DataColumn colDepartmentName = new DataColumn();
            colDepartmentName.ColumnName = "部门";
            colDepartmentName.DataType = typeof(string);
            dt.Columns.Add(colDepartmentName);

            DataColumn colBorrowType = new DataColumn();
            colBorrowType.ColumnName = "借款类型";
            colBorrowType.DataType = typeof(string);
            dt.Columns.Add(colBorrowType);

            DataColumn colBorrowMoney = new DataColumn();
            colBorrowMoney.ColumnName = "借款金额";
            colBorrowMoney.DataType = typeof(decimal);
            dt.Columns.Add(colBorrowMoney);

            DataColumn colRepayType = new DataColumn();
            colRepayType.ColumnName = "还款类型";
            colRepayType.DataType = typeof(string);
            dt.Columns.Add(colRepayType);

            DataColumn colRepayMoney = new DataColumn();
            colRepayMoney.ColumnName = "还款金额";
            colRepayMoney.DataType = typeof(decimal);
            dt.Columns.Add(colRepayMoney);

            DataColumn colUpdateDate = new DataColumn();
            colUpdateDate.ColumnName = "更新日期";
            colUpdateDate.DataType = typeof(string);
            dt.Columns.Add(colUpdateDate);

            DataColumn colRelRecordCode = new DataColumn();
            colRelRecordCode.ColumnName = "备注";
            colRelRecordCode.DataType = typeof(string);
            dt.Columns.Add(colRelRecordCode);

            return dt;
        }

        /// <summary>
        /// 填充实体集数据到DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="contactDetailList"></param>
        /// <returns></returns>
        private DataTable GetExportDataForContactDetail(DataTable dt, List<V_ContactDetail> ents)
        {
            dt.Rows.Clear();

            List<SMT.SaaS.BLLCommonServices.PermissionWS.T_SYS_DICTIONARY> entDicts = GetAllDictionary();

            for (int i = 0; i < ents.Count(); i++)
            {
                DataRow row = dt.NewRow();
                for (int n = 0; n < dt.Columns.Count; n++)
                {
                    switch (n)
                    {
                        case 0:
                            row[n] = ents[i].RecordCode;
                            break;
                        case 1:
                            row[n] = ents[i].DepartmentName;
                            break;
                        case 2:
                            row[n] = GetTypeName(ents[i].BorrowType, "BorrowType", entDicts);
                            break;
                        case 3:
                            row[n] = ents[i].BorrowMoney;
                            break;
                        case 4:
                            row[n] = GetTypeName(ents[i].RepayType, "FBARepayType", entDicts);
                            break;
                        case 5:
                            row[n] = ents[i].RepayMoney;
                            break;
                        case 6:
                            row[n] = ents[i].UpdateDate;
                            break;
                        case 7:
                            row[n] = ents[i].RelRecordCode;
                            break;
                    }
                }

                dt.Rows.Add(row);
            }

            return dt;
        }
        #endregion 个人的执行一览的导出数据

        #region 获取部门的借还款往来的导出数据
        /// <summary>
        /// 获取部门的执行一览的导出数据
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
        public byte[] OutFileDeptContactDetailListByMultSearch(string strFileType, string strOrgType, string strOrgID, string strOwnerId, DateTime dtStart, DateTime dtEnd, string strFilter, List<object> objArgs, string strSortKey, ref decimal dTotalNormalBorrowMoney, ref decimal dTotalSpecialMoney, ref decimal dTotalReserveMoney)
        {
            List<V_DeptContactDetail> contactDetailList = new List<V_DeptContactDetail>();
            ExecutionDAL dalExecution = new ExecutionDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "ContactDetailsView");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "T_FB_SUBJECT.SUBJECTID";
            }

            contactDetailList = dalExecution.GetDeptContactDetailListByMultSearch(strOrgType, strOrgID, dtStart, dtEnd, strOrderBy, ref dTotalNormalBorrowMoney, ref dTotalSpecialMoney, ref dTotalReserveMoney, strFilter, objArgs.ToArray());

            DataTable dt = MakeTableToExportDeptContactDetail();
            DataTable dtExport = GetExportDataForDeptContactDetail(dt, contactDetailList);

            return Utility.OutFileStream("部门借还款往来", dtExport);
        }

        /// <summary>
        /// 构建导出的DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable MakeTableToExportDeptContactDetail()
        {
            DataTable dt = new DataTable();

            DataColumn colOwnerName = new DataColumn();
            colOwnerName.ColumnName = "员工姓名";
            colOwnerName.DataType = typeof(string);
            dt.Columns.Add(colOwnerName);

            DataColumn colNormalMoney = new DataColumn();
            colNormalMoney.ColumnName = "普通借款";
            colNormalMoney.DataType = typeof(decimal);
            dt.Columns.Add(colNormalMoney);

            DataColumn colReserveMoney = new DataColumn();
            colReserveMoney.ColumnName = "备用金借款";
            colReserveMoney.DataType = typeof(decimal);
            dt.Columns.Add(colReserveMoney);

            DataColumn colSpecialMoney = new DataColumn();
            colSpecialMoney.ColumnName = "专项借款";
            colSpecialMoney.DataType = typeof(decimal);
            dt.Columns.Add(colSpecialMoney);

            DataColumn colTotalMoney = new DataColumn();
            colTotalMoney.ColumnName = "借款合计";
            colTotalMoney.DataType = typeof(decimal);
            dt.Columns.Add(colTotalMoney);

            return dt;
        }

        /// <summary>
        /// 填充实体集数据转存到DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ents"></param>
        /// <returns></returns>
        private DataTable GetExportDataForDeptContactDetail(DataTable dt, List<V_DeptContactDetail> ents)
        {
            dt.Rows.Clear();

            for (int i = 0; i < ents.Count(); i++)
            {
                DataRow row = dt.NewRow();
                for (int n = 0; n < dt.Columns.Count; n++)
                {
                    switch (n)
                    {
                        case 0:
                            row[n] = ents[i].OwnerName;
                            break;
                        case 1:
                            row[n] = ents[i].NormalMoney;
                            break;
                        case 2:
                            row[n] = ents[i].ReserveMoney;
                            break;
                        case 3:
                            row[n] = ents[i].SpecialMoney;
                            break;
                        case 4:
                            row[n] = ents[i].TotalMoney;
                            break;
                    }
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion 获取部门的执行一览的导出数据

        /// <summary>
        /// 根据字典值及其类型，从字典列表获取其字典名称
        /// </summary>
        /// <param name="strTypeValue"></param>
        /// <param name="strTypeCate"></param>
        /// <param name="entDicts"></param>
        /// <returns></returns>
        private string GetTypeName(string strTypeValue, string strTypeCate, List<SaaS.BLLCommonServices.PermissionWS.T_SYS_DICTIONARY> entDicts)
        {
            string strRes = string.Empty;
            if (string.IsNullOrWhiteSpace(strTypeValue))
            {
                return strRes;
            }

            if (entDicts == null)
            {
                return strRes;
            }

            if (strTypeCate == "FBARepayType")
            {
                string v = strTypeValue;
                if (!String.IsNullOrWhiteSpace(v))
                {
                    var strArr = v.Split(',');
                    if (strArr.Length == 2)
                    {
                        strTypeValue = strArr[1];
                        strTypeCate = strArr[0];
                    }
                }
            }

            var objs = from a in entDicts
                       where a.DICTIONARYVALUE.ToString() == strTypeValue && a.DICTIONCATEGORY == strTypeCate
                       select a;


            SaaS.BLLCommonServices.PermissionWS.T_SYS_DICTIONARY dict = objs.Count() > 0 ? objs.FirstOrDefault() : null;
            return dict == null ? strRes : dict.DICTIONARYNAME;
        }

        #endregion 文件导出

        #region ILookupEntity 成员

        public System.Data.Objects.DataClasses.EntityObject[] GetLookupData(string modelCode, string userID, int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount)
        {
            ExecutionDAL dalExecution = new ExecutionDAL();

            List<object> queryParas = new List<object>();
            string strOrderBy = string.Empty;

            if (paras.Count() > 0)
            {
                for (int i = 0; i < paras.Count(); i++)
                {
                    queryParas.Add(paras[i]);
                }
            }

            strOrderBy = " SUBJECTID ";


            IQueryable<T_FB_SUBJECT> res = dalExecution.GetObjects<T_FB_SUBJECT>();
            if (string.IsNullOrWhiteSpace(modelCode))
            {
                modelCode = "T_FB_SUBJECTDEPTMENT";
            }

            string strTag = string.Empty;
            if (filterString.Contains("OWNERID"))
            {
                strTag = "T_FB_SUBJECTPOST";
            }
            if (filterString.Contains("OWNERPOSTID"))
            {
                strTag = "T_FB_SUBJECTPOST";
            }
            else if (filterString.Contains("OWNERDEPARTMENTID"))
            {
                strTag = "T_FB_SUBJECTDEPTMENT";
            }
            else if (filterString.Contains("OWNERCOMPANYID"))
            {
                strTag = "T_FB_SUBJECTCOMPANY";
            }


            SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
            ul.SetOrganizationFilter(ref filterString, ref queryParas, userID, modelCode);


            decimal dActived = 1m;
            if (strTag == "T_FB_SUBJECTPOST")
            {
                var ents = from v in dalExecution.GetObjects<T_FB_SUBJECTPOST>().Include("T_FB_SUBJECT")
                           where v.ACTIVED == dActived
                           select v;

                res = ents.Where(filterString, queryParas.ToArray()).Select(t => t.T_FB_SUBJECT);
            }
            else if (strTag == "T_FB_SUBJECTDEPTMENT")
            {
                var ents = from v in dalExecution.GetObjects<T_FB_SUBJECTDEPTMENT>().Include("T_FB_SUBJECT")
                           where v.ACTIVED == dActived
                           select v;

                res = ents.Where(filterString, queryParas.ToArray()).Select(t => t.T_FB_SUBJECT);
            }
            else if (strTag == "T_FB_SUBJECTCOMPANY")
            {
                var ents = from v in dalExecution.GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT")
                           where v.ACTIVED == dActived
                           select v;

                res = ents.Where(filterString, queryParas.ToArray()).Select(t => t.T_FB_SUBJECT);
            }
            else
            {
                var ents = from v in dalExecution.GetObjects<T_FB_SUBJECTDEPTMENT>().Include("T_FB_SUBJECT")
                           where v.ACTIVED == dActived
                           select v;

                res = ents.Where(filterString, queryParas.ToArray()).Select(t => t.T_FB_SUBJECT);
            }
            res = res.Distinct().OrderBy(item => item.SUBJECTCODE);
            return res.Count() > 0 ? res.ToArray() : null;
        }

        #endregion
    }
}
