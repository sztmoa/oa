
/*
 * 文件名：BudgetCheckBLL.cs
 * 作  用：T_FB_BUDGETCHECK 业务逻辑类
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
using SMT.FBAnalysis.CustomModel;

namespace SMT.FBAnalysis.BLL
{
    public class BudgetCheckBLL
    {
        public BudgetCheckBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_BUDGETCHECK信息
        /// </summary>
        /// <param name="strBudgetCheckId">主键索引</param>
        /// <returns></returns>
        public T_FB_BUDGETCHECK GetBudgetCheckByID(string strBudgetCheckId)
        {
            if (string.IsNullOrEmpty(strBudgetCheckId))
            {
                return null;
            }

            BudgetCheckDAL dalBudgetCheck = new BudgetCheckDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strBudgetCheckId))
            {
                strFilter.Append(" BUDGETCHECKID == @0");
                objArgs.Add(strBudgetCheckId);
            }

            T_FB_BUDGETCHECK entRd = dalBudgetCheck.GetBudgetCheckRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取T_FB_BUDGETCHECK信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_BUDGETCHECK> GetAllBudgetCheckRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            BudgetCheckDAL dalBudgetCheck = new BudgetCheckDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " BUDGETCHECKID ";
            }

            var q = dalBudgetCheck.GetBudgetCheckRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_BUDGETCHECK信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_BUDGETCHECK信息</returns>
        public IQueryable<T_FB_BUDGETCHECK> GetBudgetCheckRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllBudgetCheckRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_BUDGETCHECK>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion

        #region 核算当前预算数据

        #region 预算台帐查询

        /// <summary>
        /// 预算台帐查询分页
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="strSortKey"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<V_StandingBook> GetStandingBookByMultSearch(string strOrgType, string strOwnerId, DateTime dtStart, DateTime dtEnd, string strSortKey,
            string strFilter, List<object> objArgs, int pageIndex, int pageSize, ref int pageCount)
        {
            List<V_StandingBook> entStandingBookList = new List<V_StandingBook>();
            BudgetCheckDAL dalBudgetCheck = new BudgetCheckDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "StandingBook");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            entStandingBookList = dalBudgetCheck.GetStandingBookByMultSearch(strOrgType, dtStart, dtEnd, strOrderBy, strFilter, objArgs.ToArray());
            return Utility.PagerList<V_StandingBook>(entStandingBookList, pageIndex, pageSize, ref pageCount);
        }

        #endregion


        #region 预算年度预算分析

        /// <summary>
        /// 预算年度预算分析分页
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
        public List<V_YearlyBudgetAnalysis> GetYearlyBudgetAnalysisByMultSearch(string strOrgType, string strOwnerId, int iYear, int iMonthStart,
            int iMonthEnd, string strSortKey, string strFilter, List<object> objArgs, int pageIndex, int pageSize, ref int pageCount)
        {
            List<V_YearlyBudgetAnalysis> entYearlyAnalysisList = new List<V_YearlyBudgetAnalysis>();
            BudgetCheckDAL dalBudgetCheck = new BudgetCheckDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "YearlyBudgetAnalysis");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            entYearlyAnalysisList = dalBudgetCheck.GetYearlyBudgetAnalysisByMultSearch(strOrgType, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs.ToArray());
            return Utility.PagerList<V_YearlyBudgetAnalysis>(entYearlyAnalysisList, pageIndex, pageSize, ref pageCount);
        }
        #endregion

        #region 预算月度预算分析

        /// <summary>
        /// 预算月度预算分析分页
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOwnerId"></param>
        /// <param name="strOrgID"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <param name="strSortKey"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public List<V_MonthlyBudgetAnalysis> GetMonthlyBudgetAnalysisByMultSearch(string strOrgType, string strOwnerId, int iYear, int iMonth,
            string strSortKey, string strFilter, List<object> objArgs, int pageIndex, int pageSize, ref int pageCount)
        {
            List<V_MonthlyBudgetAnalysis> entMonthlyAnalysisList = new List<V_MonthlyBudgetAnalysis>();
            BudgetCheckDAL dalBudgetCheck = new BudgetCheckDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "MonthlyBudgetAnalysis");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            entMonthlyAnalysisList = dalBudgetCheck.GetMonthlyBudgetAnalysisByMultSearch(strOrgType, iYear, iMonth, strOrderBy, strFilter, objArgs.ToArray());
            return Utility.PagerList<V_MonthlyBudgetAnalysis>(entMonthlyAnalysisList, pageIndex, pageSize, ref pageCount);
        }

        #endregion

        #endregion

        #region 导出预算台帐列表
        public byte[] OutFileStandingBookList(string strOrgType, string strOwnerId, DateTime dtStart, DateTime dtEnd, string strSortKey, string strFilter, List<object> objArgs)
        {
            List<V_StandingBook> entStandingBookList = new List<V_StandingBook>();
            BudgetCheckDAL dalBudgetCheck = new BudgetCheckDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "StandingBook");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            entStandingBookList = dalBudgetCheck.GetStandingBookByMultSearch(strOrgType, dtStart, dtEnd, strOrderBy, strFilter, objArgs.ToArray());

            DataTable dt = MakeTableToExportStandingBook();
            DataTable dtExport = GetExportDataForStandingBook(dt, entStandingBookList);

            return Utility.OutFileStream("预算台帐列表", dtExport);
        }

        /// <summary>
        /// 构造导出数据所需的DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable MakeTableToExportStandingBook()
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

            DataColumn colTotalMoney = new DataColumn();
            colTotalMoney.ColumnName = "金额";
            colTotalMoney.DataType = typeof(decimal);
            dt.Columns.Add(colTotalMoney);

            DataColumn colOwnerName = new DataColumn();
            colOwnerName.ColumnName = "申请人";
            colOwnerName.DataType = typeof(string);
            dt.Columns.Add(colOwnerName);

            DataColumn colDepartmentName = new DataColumn();
            colDepartmentName.ColumnName = "申请部门";
            colDepartmentName.DataType = typeof(string);
            dt.Columns.Add(colDepartmentName);

            DataColumn colCompanyName = new DataColumn();
            colCompanyName.ColumnName = "申请公司";
            colCompanyName.DataType = typeof(string);
            dt.Columns.Add(colCompanyName);

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

        /// <summary>
        /// 填充导出数据到导出的DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ents"></param>
        /// <returns></returns>
        private DataTable GetExportDataForStandingBook(DataTable dt, List<V_StandingBook> ents)
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
                            row[n] = ents[i].RecordCode;
                            break;
                        case 1:
                            row[n] = ents[i].SubjectName;
                            break;
                        case 2:
                            row[n] = ents[i].TotalMoney;
                            break;
                        case 3:
                            row[n] = ents[i].OwnerName;
                            break;
                        case 4:
                            row[n] = ents[i].DepartmentName;
                            break;
                        case 5:
                            row[n] = ents[i].CompanyName;
                            break;
                        case 6:
                            row[n] = ents[i].UpdateDate;
                            break;
                        case 7:
                            row[n] = ents[i].ChecksatesName;
                            break;
                    }
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion 导出预算台帐列表

        #region 导出月度预算分析列表
        public byte[] OutFileMonthlyBudgetAnalysisList(string strOrgType, string strOwnerId, int iYear, int iMonth,
            string strSortKey, string strFilter, List<object> objArgs)
        {
            List<V_MonthlyBudgetAnalysis> entMonthlyAnalysisList = new List<V_MonthlyBudgetAnalysis>();
            BudgetCheckDAL dalBudgetCheck = new BudgetCheckDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "MonthlyBudgetAnalysis");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            entMonthlyAnalysisList = dalBudgetCheck.GetMonthlyBudgetAnalysisByMultSearch(strOrgType, iYear, iMonth, strOrderBy, strFilter, objArgs.ToArray());

            DataTable dt = MakeTableToExportMonthlyAnalysis();
            DataTable dtExport = GetExportDataForMonthlyAnalysis(dt, entMonthlyAnalysisList);

            return Utility.OutFileStream("月度预算分析列表", dtExport);
        }

        /// <summary>
        /// 构造导出数据所需的DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable MakeTableToExportMonthlyAnalysis()
        {
            DataTable dt = new DataTable();

            DataColumn colSubjectName = new DataColumn();
            colSubjectName.ColumnName = "科目名称";
            colSubjectName.DataType = typeof(string);
            dt.Columns.Add(colSubjectName);

            DataColumn colLastBudgetMonth = new DataColumn();
            colLastBudgetMonth.ColumnName = "上月预算结余";
            colLastBudgetMonth.DataType = typeof(decimal);
            dt.Columns.Add(colLastBudgetMonth);

            DataColumn colMonthBudgetApply = new DataColumn();
            colMonthBudgetApply.ColumnName = "本月预算";
            colMonthBudgetApply.DataType = typeof(decimal);
            dt.Columns.Add(colMonthBudgetApply);

            DataColumn colMonthBudgetAdd = new DataColumn();
            colMonthBudgetAdd.ColumnName = "本月预算增补";
            colMonthBudgetAdd.DataType = typeof(decimal);
            dt.Columns.Add(colMonthBudgetAdd);

            DataColumn colMonthChargeMoney = new DataColumn();
            colMonthChargeMoney.ColumnName = "本月报销费用";
            colMonthChargeMoney.DataType = typeof(decimal);
            dt.Columns.Add(colMonthChargeMoney);

            DataColumn colMonthBudgetDeviation = new DataColumn();
            colMonthBudgetDeviation.ColumnName = "月度预算偏差";
            colMonthBudgetDeviation.DataType = typeof(decimal);
            dt.Columns.Add(colMonthBudgetDeviation);

            DataColumn colMonthBudgetDeviationRate = new DataColumn();
            colMonthBudgetDeviationRate.ColumnName = "月度预算偏差率";
            colMonthBudgetDeviationRate.DataType = typeof(string);
            dt.Columns.Add(colMonthBudgetDeviationRate);

            return dt;
        }

        /// <summary>
        /// 填充导出数据到导出的DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="ents"></param>
        /// <returns></returns>
        private DataTable GetExportDataForMonthlyAnalysis(DataTable dt, List<V_MonthlyBudgetAnalysis> ents)
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
                            row[n] = ents[i].SubjectName;
                            break;
                        case 1:
                            row[n] = ents[i].LastBudgetMonth;
                            break;
                        case 2:
                            row[n] = ents[i].MonthBudgetApply;
                            break;
                        case 3:
                            row[n] = ents[i].MonthBudgetAdd;
                            break;
                        case 4:
                            row[n] = ents[i].MonthChargeMoney;
                            break;
                        case 5:
                            row[n] = ents[i].MonthBudgetDeviation;
                            break;
                        case 6:
                            row[n] = PercentFormat(ents[i].MonthBudgetDeviationRate);
                            break;
                    }
                }

                dt.Rows.Add(row);
            }

            return dt;
        }
        #endregion 导出月度预算分析列表

        #region 导出年度预算分析列表
        public byte[] OutFileYearlyBudgetAnalysisList(string strOrgType, string strOwnerId, int iYear, int iMonthStart,
            int iMonthEnd, string strSortKey, string strFilter, List<object> objArgs)
        {
            List<V_YearlyBudgetAnalysis> entYearlyAnalysisList = new List<V_YearlyBudgetAnalysis>();
            BudgetCheckDAL dalBudgetCheck = new BudgetCheckDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrWhiteSpace(strOwnerId))
            {
                SMT.SaaS.BLLCommonServices.Utility ul = new SaaS.BLLCommonServices.Utility();
                ul.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerId, "YearlyBudgetAnalysis");
            }

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = "SUBJECTID";
            }

            entYearlyAnalysisList = dalBudgetCheck.GetYearlyBudgetAnalysisByMultSearch(strOrgType, iYear, iMonthStart, iMonthEnd, strOrderBy, strFilter, objArgs.ToArray());

            DataTable dt = MakeTableToExportYearlyAnalysis();
            DataTable dtExport = GetExportDataForYearlyAnalysis(dt, entYearlyAnalysisList);

            return Utility.OutFileStream("年度预算分析列表", dtExport);
        }

        /// <summary>
        /// 构造导出数据所需的DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable MakeTableToExportYearlyAnalysis()
        {
            DataTable dt = new DataTable();

            DataColumn colSubjectName = new DataColumn();
            colSubjectName.ColumnName = "科目名称";
            colSubjectName.DataType = typeof(string);
            dt.Columns.Add(colSubjectName);

            DataColumn colYearBudgetApply = new DataColumn();
            colYearBudgetApply.ColumnName = "年度预算";
            colYearBudgetApply.DataType = typeof(decimal);
            dt.Columns.Add(colYearBudgetApply);

            DataColumn colYearBudgetAdd = new DataColumn();
            colYearBudgetAdd.ColumnName = "年度预算增补";
            colYearBudgetAdd.DataType = typeof(decimal);
            dt.Columns.Add(colYearBudgetAdd);

            DataColumn colYearChargeMoney = new DataColumn();
            colYearChargeMoney.ColumnName = "本年报销费用";
            colYearChargeMoney.DataType = typeof(decimal);
            dt.Columns.Add(colYearChargeMoney);

            DataColumn colYearBudgetDeviation = new DataColumn();
            colYearBudgetDeviation.ColumnName = "年度预算偏差";
            colYearBudgetDeviation.DataType = typeof(decimal);
            dt.Columns.Add(colYearBudgetDeviation);

            DataColumn colYearBudgetDeviationRate = new DataColumn();
            colYearBudgetDeviationRate.ColumnName = "年度预算偏差率";
            colYearBudgetDeviationRate.DataType = typeof(string);
            dt.Columns.Add(colYearBudgetDeviationRate);

            return dt;
        }

        /// <summary>
        /// 填充导出数据到导出的DataTable
        /// </summary>
        /// <param name="strOwnerId"></param>
        /// <param name="dt"></param>
        /// <param name="entYearlyAnalysisList"></param>
        /// <returns></returns>
        private DataTable GetExportDataForYearlyAnalysis(DataTable dt, List<V_YearlyBudgetAnalysis> ents)
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
                            row[n] = ents[i].SubjectName;
                            break;
                        case 1:
                            row[n] = ents[i].YearBudgetApply;
                            break;
                        case 2:
                            row[n] = ents[i].YearBudgetAdd;
                            break;
                        case 3:
                            row[n] = ents[i].YearChargeMoney;
                            break;
                        case 4:
                            row[n] = ents[i].YearBudgetDeviation;
                            break;
                        case 5:
                            row[n] = PercentFormat(ents[i].YearBudgetDeviationRate);
                            break;
                    }
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion 导出年度预算分析列表

        /// <summary>
        /// 浮点数格式化为百分比显示
        /// </summary>
        /// <param name="dValue"></param>
        /// <returns></returns>
        private string PercentFormat(decimal dValue)
        {
            dValue = decimal.Round(dValue * 100, 2);
            string[] strlist = dValue.ToString().Split('.');
            string strTemp = string.Empty;
            strTemp = strlist[0];


            if (strlist.Length == 2)
            {
                strTemp += "." + strlist[1].PadRight(2, '0');
            }
            else
            {
                strTemp += ".00";
            }

            return strTemp + "%";
        }
    }
}

