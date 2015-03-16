
/*
 * 文件名：BudgetCheckDAL.cs
 * 作  用：T_FB_BUDGETCHECK 数据操作类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 9:52:14
 * 修改人：
 * 修改时间：
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data.Objects.DataClasses;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.CustomModel;
using System.Data;
using SMT.Foundation.Log;

namespace SMT.FBAnalysis.DAL
{
    public class BudgetCheckDAL : CommDal<T_FB_BUDGETCHECK>
    {
        public BudgetCheckDAL()
        {
        }

        /// <summary>
        /// 根据参数，检查是否存在指定记录(方便进行数据增删改)
        /// </summary>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>True/False(是/否)</returns>
        public bool IsExistsRd(string strFilter, params object[] objArgs)
        {
            bool flag = false;

            var q = from v in GetObjects()
                    select v;

            if (objArgs.Count() <= 0 || string.IsNullOrEmpty(strFilter))
            {
                return flag;
            }

            q = q.Where(strFilter, objArgs);

            if (q.Count() > 0)
            {
                flag = true;
            }

            return flag;
        }

        /// <summary>
        /// 获取指定条件的T_FB_BUDGETCHECK信息
        /// </summary>        
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_BUDGETCHECK信息</returns>
        public T_FB_BUDGETCHECK GetBudgetCheckRdByMultSearch(string strFilter, params object[] objArgs)
        {
            var q = from v in GetObjects()
                    select v;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }

            return q.First();
        }

        /// <summary>
        /// 获取指定条件的T_FB_BUDGETCHECK信息
        /// </summary>        
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_BUDGETCHECK信息</returns>
        public IQueryable<T_FB_BUDGETCHECK> GetBudgetCheckRdListByMultSearch(string strOrderBy, string strFilter, params object[] objArgs)
        {
            var q = from v in GetObjects()
                    select v;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }


            return q.OrderBy(strOrderBy);
        }

        #region 核算当前预算数据

        #region 执行一览，清查预算及报销

        #region 执行一览(统计)

        #region 执行一览(部门统计)
        /// <summary>
        /// 执行一览按条件查询(按公司/部门查询)
        /// </summary>
        /// <param name="strOrgType">查询机构类型</param>
        /// <param name="strOrgID">查询机构ID</param>
        /// <param name="iYear">查询年份</param>
        /// <param name="iMonthStart">查询起始月份</param>
        /// <param name="iMonthEnd">查询截止月份</param>
        /// <param name="strOrderBy">排序条件</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <returns>返回查询的视图</returns>
        public List<V_ExecutionList> GetExecutionListForDepartment(string strOrgType, string strOrgID, int iYear,
           int iMonthStart, int iMonthEnd, string strOrderBy, string strFilter, params object[] objArgs)
        {
            List<V_ExecutionList> entResList = new List<V_ExecutionList>();
            try
            {
                if (iYear <= 0)
                {
                    return entResList;
                }

                if (iMonthStart <= 0 || iMonthEnd <= 0)
                {
                    return entResList;
                }

                if (iMonthStart > iMonthEnd)
                {
                    return entResList;
                }

                if (string.IsNullOrWhiteSpace(strOrgType) || string.IsNullOrWhiteSpace(strOrgID))
                {
                    return entResList;
                }

                if (strOrgType.ToUpper() != "COMPANY" && strOrgType.ToUpper() != "DEPARTMENT")
                {
                    return entResList;
                }

                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                DateTime dtEnd = new DateTime();
                if (iMonthEnd < 12)
                {
                    dtEnd = DateTime.Parse(iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1");
                }
                else
                {
                    dtEnd = DateTime.Parse((iYear + 1).ToString() + "-1-1");
                }

                string strCompSql = "select * from zzz_comp where CHECKSTATES=2 and CHECKSTATESNAME='审核通过' and UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and ORDERTYPENAME like '%年度%'";
                string strDeptSql = "select * from zzz_dept where BUDGETARYMONTH >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and BUDGETARYMONTH < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";
                string strPersSql = "select * from zzz_person where UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    strFilter = strFilter.Replace("==", "=");
                    for (int i = objArgs.Count() - 1; i >= 0; i--)
                    {
                        strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                    }
                    strCompSql += " and " + strFilter;
                    strDeptSql += " and " + strFilter;
                    strPersSql += " and " + strFilter;
                }


                DataTable dtComp = new DataTable();
                DataTable dtDept = new DataTable();
                DataTable dtPers = new DataTable();

                dtComp = this.GetDataTableByCustomerSql(strCompSql);
                dtDept = this.GetDataTableByCustomerSql(strDeptSql);
                dtPers = this.GetDataTableByCustomerSql(strPersSql);


                foreach (DataRow drComp in dtComp.Rows)
                {
                    V_ExecutionList entRes = new V_ExecutionList();
                    string strCompanyID = string.Empty, strDepartmentID = string.Empty, strSubjectID = string.Empty;
                    decimal dBudgetmoney = 0;
                    bool bIsExists = false;
                    if (drComp["OWNERCOMPANYID"] == null || drComp["OWNERDEPARTMENTID"] == null)
                    {
                        continue;
                    }

                    if (drComp["SUBJECTID"] == null || drComp["BUDGETMONEY"] == null)
                    {
                        continue;
                    }

                    strCompanyID = drComp["OWNERCOMPANYID"].ToString();
                    strDepartmentID = drComp["OWNERDEPARTMENTID"].ToString();
                    strSubjectID = drComp["SUBJECTID"].ToString();
                    decimal.TryParse(drComp["BUDGETMONEY"].ToString(), out dBudgetmoney);

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.OrganizationID == strDepartmentID && n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    entRes.BudgetMoneyYear += dBudgetmoney;

                    if (!bIsExists)
                    {
                        entRes.OrganizationID = strDepartmentID;
                        entRes.OrganizationName = strDepartmentID;

                        entRes.SubjectID = strSubjectID;

                        if (drComp["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drComp["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                }

                foreach (DataRow drDept in dtDept.Rows)
                {
                    V_ExecutionList entRes = new V_ExecutionList();
                    bool bIsExists = false;
                    string strCompanyID = string.Empty, strDepartmentID = string.Empty, strSubjectID = string.Empty;
                    string strOrderTypeName = string.Empty, strCheckstatesName = string.Empty;
                    decimal dBudgetmoney = 0, dApprovedChargeMoney = 0, dApprovingChargeMoney = 0;

                    DateTime dtOrderUpdateDate = new DateTime();

                    if (drDept["OWNERCOMPANYID"] == null || drDept["OWNERDEPARTMENTID"] == null)
                    {
                        continue;
                    }

                    if (drDept["SUBJECTID"] == null || drDept["BUDGETMONEY"] == null)
                    {
                        continue;
                    }

                    strCompanyID = drDept["OWNERCOMPANYID"].ToString();
                    strDepartmentID = drDept["OWNERDEPARTMENTID"].ToString();
                    strSubjectID = drDept["SUBJECTID"].ToString();

                    strOrderTypeName = drDept["ORDERTYPENAME"].ToString();
                    strCheckstatesName = drDept["CHECKSTATESNAME"].ToString();

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.OrganizationID == strDepartmentID && n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    DateTime.TryParse(drDept["UPDATEDATE"].ToString(), out dtOrderUpdateDate);

                    if (strOrderTypeName.Contains("预算") && strCheckstatesName == "审核通过")
                    {
                        decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dBudgetmoney);
                    }

                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核通过")
                    {
                        decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dApprovedChargeMoney);

                        dApprovedChargeMoney = 0 - dApprovedChargeMoney;

                        int iCurMonth = dtOrderUpdateDate.Month;
                        switch (iCurMonth)
                        {
                            case 1:
                                entRes.JanApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 2:
                                entRes.FebApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 3:
                                entRes.MarApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 4:
                                entRes.AprApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 5:
                                entRes.MayApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 6:
                                entRes.JunApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 7:
                                entRes.JulApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 8:
                                entRes.AugApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 9:
                                entRes.SepApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 10:
                                entRes.OctApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 11:
                                entRes.NovApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 12:
                                entRes.DecApprovedMoney += dApprovedChargeMoney;
                                break;
                        }
                    }

                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核中")
                    {
                        decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dApprovingChargeMoney);
                        dApprovedChargeMoney = 0 - dApprovedChargeMoney;
                    }

                    if (dBudgetmoney > 0)
                    {
                        entRes.BudgetMoneyMonth += dBudgetmoney;
                    }

                    if (dApprovedChargeMoney > 0)
                    {
                        entRes.ApprovedApplyMoney += dApprovedChargeMoney;
                    }

                    if (dApprovingChargeMoney > 0)
                    {
                        entRes.ApprovingApplyMoney += dApprovingChargeMoney;
                    }

                    entRes.Subtotal = entRes.ApprovedApplyMoney + entRes.ApprovingApplyMoney;
                    entRes.BudgetMonth = entRes.BudgetMoneyMonth - entRes.Subtotal;
                    entRes.BudgetYear = entRes.BudgetMoneyYear - entRes.BudgetMoneyMonth;
                    entRes.BudgetSeizureRate = decimal.Round(entRes.BudgetMoneyMonth / entRes.BudgetMoneyYear, 2);

                    if (!bIsExists)
                    {
                        entRes.OrganizationID = strDepartmentID;
                        entRes.OrganizationName = strDepartmentID;

                        entRes.SubjectID = strSubjectID;

                        if (drDept["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drDept["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                }

                foreach (DataRow drPers in dtPers.Rows)
                {
                    V_ExecutionList entRes = new V_ExecutionList();
                    bool bIsExists = false;
                    string strCompanyID = string.Empty, strDepartmentID = string.Empty, strSubjectID = string.Empty;
                    string strOrderTypeName = string.Empty, strCheckstatesName = string.Empty;
                    decimal dBudgetmoney = 0, dApprovedChargeMoney = 0, dApprovingChargeMoney = 0;

                    DateTime dtOrderUpdateDate = new DateTime();

                    if (drPers["OWNERCOMPANYID"] == null || drPers["OWNERDEPARTMENTID"] == null)
                    {
                        continue;
                    }

                    if (drPers["SUBJECTID"] == null || drPers["BUDGETMONEY"] == null)
                    {
                        continue;
                    }

                    strCompanyID = drPers["OWNERCOMPANYID"].ToString();
                    strDepartmentID = drPers["OWNERDEPARTMENTID"].ToString();
                    strSubjectID = drPers["SUBJECTID"].ToString();

                    strOrderTypeName = drPers["ORDERTYPENAME"].ToString();
                    strCheckstatesName = drPers["CHECKSTATESNAME"].ToString();

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.OrganizationID == strDepartmentID && n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    DateTime.TryParse(drPers["UPDATEDATE"].ToString(), out dtOrderUpdateDate);

                    if (strOrderTypeName.Contains("预算") && strCheckstatesName == "审核通过")
                    {
                        decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dBudgetmoney);
                    }

                    if (strOrderTypeName == "个人经费下拨" && strCheckstatesName == "审核通过")
                    {
                        decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dBudgetmoney);
                    }

                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核通过")
                    {
                        decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dApprovedChargeMoney);

                        dApprovedChargeMoney = 0 - dApprovedChargeMoney;

                        int iCurMonth = dtOrderUpdateDate.Month;
                        switch (iCurMonth)
                        {
                            case 1:
                                entRes.JanApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 2:
                                entRes.FebApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 3:
                                entRes.MarApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 4:
                                entRes.AprApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 5:
                                entRes.MayApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 6:
                                entRes.JunApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 7:
                                entRes.JulApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 8:
                                entRes.AugApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 9:
                                entRes.SepApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 10:
                                entRes.OctApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 11:
                                entRes.NovApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 12:
                                entRes.DecApprovedMoney += dApprovedChargeMoney;
                                break;
                        }
                    }

                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核中")
                    {
                        decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dApprovingChargeMoney);
                        dApprovedChargeMoney = 0 - dApprovedChargeMoney;
                    }

                    if (dBudgetmoney > 0)
                    {
                        entRes.BudgetMoneyMonth += dBudgetmoney;
                    }

                    if (dApprovedChargeMoney > 0)
                    {
                        entRes.ApprovedApplyMoney += dApprovedChargeMoney;
                    }

                    if (dApprovingChargeMoney > 0)
                    {
                        entRes.ApprovingApplyMoney += dApprovingChargeMoney;
                    }

                    entRes.Subtotal = entRes.ApprovedApplyMoney + entRes.ApprovingApplyMoney;
                    entRes.BudgetMonth = entRes.BudgetMoneyMonth - entRes.Subtotal;
                    entRes.BudgetYear = entRes.BudgetMoneyYear - entRes.BudgetMoneyMonth;
                    entRes.BudgetSeizureRate = decimal.Round(entRes.BudgetMoneyMonth / entRes.BudgetMoneyYear, 2);

                    if (!bIsExists)
                    {
                        entRes.OrganizationID = strDepartmentID;
                        entRes.OrganizationName = strDepartmentID;

                        entRes.SubjectID = strSubjectID;

                        if (drPers["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drPers["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
            }

            return entResList;
        }
        #endregion

        #region 执行一览(个人统计)

        public List<V_PerExecutionList> GetExecutionListForPerson(string strOrgType, string strOrgID, int iYear,
            int iMonthStart, int iMonthEnd, string strOrderBy, string strFilter, params object[] objArgs)
        {
            List<V_PerExecutionList> entResList = new List<V_PerExecutionList>();
            try
            {
                if (iYear <= 0)
                {
                    return entResList;
                }

                if (iMonthStart <= 0 || iMonthEnd <= 0)
                {
                    return entResList;
                }

                if (iMonthStart > iMonthEnd)
                {
                    return entResList;
                }

                if (string.IsNullOrWhiteSpace(strOrgType) || string.IsNullOrWhiteSpace(strOrgID))
                {
                    return entResList;
                }

                if (strOrgType.ToUpper() != "POST" && strOrgType.ToUpper() != "PERSONNAL")
                {
                    return entResList;
                }

                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                DateTime dtEnd = new DateTime();
                if (iMonthEnd < 12)
                {
                    dtEnd = DateTime.Parse(iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1");
                }
                else
                {
                    dtEnd = DateTime.Parse((iYear + 1).ToString() + "-1-1");
                }

                string strDeptSql = "select * from zzz_dept where ORDERTYPENAME = '报销单' and BUDGETARYMONTH >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and BUDGETARYMONTH < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";
                string strPersSql = "select * from zzz_person where ORDERTYPENAME = '报销单' and UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    strFilter = strFilter.Replace("==", "=");
                    for (int i = objArgs.Count() - 1; i >= 0; i--)
                    {
                        strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                    }

                    strDeptSql += " and " + strFilter;
                    strPersSql += " and " + strFilter;
                }

                DataTable dtDept = new DataTable();
                DataTable dtPers = new DataTable();

                dtDept = this.GetDataTableByCustomerSql(strDeptSql);
                dtPers = this.GetDataTableByCustomerSql(strPersSql);


                foreach (DataRow drDept in dtDept.Rows)
                {
                    V_PerExecutionList entRes = new V_PerExecutionList();
                    bool bIsExists = false;
                    string strCompanyID = string.Empty, strDepartmentID = string.Empty, strSubjectID = string.Empty;
                    string strOrderTypeName = string.Empty, strCheckstatesName = string.Empty;
                    decimal dApprovedChargeMoney = 0, dApprovingChargeMoney = 0;

                    DateTime dtOrderUpdateDate = new DateTime();

                    if (drDept["OWNERCOMPANYID"] == null || drDept["OWNERDEPARTMENTID"] == null)
                    {
                        continue;
                    }

                    if (drDept["SUBJECTID"] == null || drDept["BUDGETMONEY"] == null)
                    {
                        continue;
                    }

                    strCompanyID = drDept["OWNERCOMPANYID"].ToString();
                    strDepartmentID = drDept["OWNERDEPARTMENTID"].ToString();
                    strSubjectID = drDept["SUBJECTID"].ToString();

                    strOrderTypeName = drDept["ORDERTYPENAME"].ToString();
                    strCheckstatesName = drDept["CHECKSTATESNAME"].ToString();

                    DateTime.TryParse(drDept["UPDATEDATE"].ToString(), out dtOrderUpdateDate);

                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核通过")
                    {
                        decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dApprovedChargeMoney);

                        dApprovedChargeMoney = 0 - dApprovedChargeMoney;

                        int iCurMonth = dtOrderUpdateDate.Month;
                        switch (iCurMonth)
                        {
                            case 1:
                                entRes.JanApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 2:
                                entRes.FebApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 3:
                                entRes.MarApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 4:
                                entRes.AprApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 5:
                                entRes.MayApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 6:
                                entRes.JunApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 7:
                                entRes.JulApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 8:
                                entRes.AugApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 9:
                                entRes.SepApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 10:
                                entRes.OctApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 11:
                                entRes.NovApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 12:
                                entRes.DecApprovedMoney += dApprovedChargeMoney;
                                break;
                        }
                    }

                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核中")
                    {
                        decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dApprovingChargeMoney);
                        dApprovedChargeMoney = 0 - dApprovedChargeMoney;
                    }

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.OrganizationID == strDepartmentID && n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    if (dApprovedChargeMoney > 0)
                    {
                        entRes.ApprovedApplyMoney += dApprovedChargeMoney;
                    }

                    if (dApprovingChargeMoney > 0)
                    {
                        entRes.ApprovingApplyMoney += dApprovingChargeMoney;
                    }

                    entRes.Subtotal = entRes.ApprovedApplyMoney + entRes.ApprovingApplyMoney;

                    if (!bIsExists)
                    {
                        entRes.OrganizationID = strDepartmentID;
                        entRes.OrganizationName = strDepartmentID;

                        entRes.SubjectID = strSubjectID;

                        if (drDept["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drDept["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                    else
                    {
                        bIsExists = false;
                    }
                }

                foreach (DataRow drPers in dtPers.Rows)
                {
                    V_PerExecutionList entRes = new V_PerExecutionList();
                    bool bIsExists = false;
                    string strCompanyID = string.Empty, strDepartmentID = string.Empty, strSubjectID = string.Empty;
                    string strOrderTypeName = string.Empty, strCheckstatesName = string.Empty;
                    decimal dApprovedChargeMoney = 0, dApprovingChargeMoney = 0;

                    DateTime dtOrderUpdateDate = new DateTime();

                    if (drPers["OWNERCOMPANYID"] == null || drPers["OWNERDEPARTMENTID"] == null)
                    {
                        continue;
                    }

                    if (drPers["SUBJECTID"] == null || drPers["BUDGETMONEY"] == null)
                    {
                        continue;
                    }

                    strCompanyID = drPers["OWNERCOMPANYID"].ToString();
                    strDepartmentID = drPers["OWNERDEPARTMENTID"].ToString();
                    strSubjectID = drPers["SUBJECTID"].ToString();

                    strOrderTypeName = drPers["ORDERTYPENAME"].ToString();
                    strCheckstatesName = drPers["CHECKSTATESNAME"].ToString();

                    DateTime.TryParse(drPers["UPDATEDATE"].ToString(), out dtOrderUpdateDate);

                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核通过")
                    {
                        decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dApprovedChargeMoney);

                        dApprovedChargeMoney = 0 - dApprovedChargeMoney;

                        int iCurMonth = dtOrderUpdateDate.Month;
                        switch (iCurMonth)
                        {
                            case 1:
                                entRes.JanApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 2:
                                entRes.FebApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 3:
                                entRes.MarApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 4:
                                entRes.AprApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 5:
                                entRes.MayApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 6:
                                entRes.JunApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 7:
                                entRes.JulApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 8:
                                entRes.AugApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 9:
                                entRes.SepApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 10:
                                entRes.OctApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 11:
                                entRes.NovApprovedMoney += dApprovedChargeMoney;
                                break;
                            case 12:
                                entRes.DecApprovedMoney += dApprovedChargeMoney;
                                break;
                        }
                    }

                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核中")
                    {
                        decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dApprovingChargeMoney);
                        dApprovedChargeMoney = 0 - dApprovedChargeMoney;
                    }

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.OrganizationID == strDepartmentID && n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    if (dApprovedChargeMoney > 0)
                    {
                        entRes.ApprovedApplyMoney += dApprovedChargeMoney;
                    }

                    if (dApprovingChargeMoney > 0)
                    {
                        entRes.ApprovingApplyMoney += dApprovingChargeMoney;
                    }

                    entRes.Subtotal = entRes.ApprovedApplyMoney + entRes.ApprovingApplyMoney;

                    if (!bIsExists)
                    {
                        entRes.OrganizationID = strDepartmentID;
                        entRes.OrganizationName = strDepartmentID;

                        entRes.SubjectID = strSubjectID;

                        if (drPers["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drPers["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                    else
                    {
                        bIsExists = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
            }

            return entResList;
        }

        #endregion

        #endregion

        #region 执行一览(流水)

        #region 获取部门指定科目的预算流水记录

        /// <summary>
        /// 获取指定科目的年度部门预算流水记录
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public DataTable GetDepartmentYearlyBudgetDaybook(string strOrgType, string strOrgID, int iYear,
            int iMonthStart, int iMonthEnd, string strOrderBy, string strFilter, List<object> objArgs)
        {
            DataTable dtRes = new DataTable();
            dtRes.Clear();
            if (strOrgType.ToUpper() != "DEPARTMENT" || string.IsNullOrWhiteSpace(strOrgID))
            {
                return dtRes;
            }

            if (iMonthStart <= 0 || iMonthEnd <= 0)
            {
                return dtRes;
            }

            if (iMonthStart > iMonthEnd)
            {
                return dtRes;
            }

            DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
            DateTime dtEnd = new DateTime();
            if (iMonthEnd < 12)
            {
                dtEnd = DateTime.Parse(iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1");
            }
            else
            {
                dtEnd = DateTime.Parse((iYear + 1).ToString() + "-1-1");
            }

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, subjectname 科目名称, "
                + "case when BUDGETMONEY < 0 then -BUDGETMONEY else BUDGETMONEY end 预算额度, checkstatesname 审核状态, "
                + "createdate 创建时间, updatedate 终审时间, ownercompanyname 所属公司, ownerdepartmentname 所属部门"
                + " from zzz_comp where ORDERTYPENAME like '%年度%' and OWNERDEPARTMENTID='" + strOrgID + "' and UPDATEDATE >= to_date('"
                + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                strFilter = strFilter.Replace("==", "=");
                for (int i = objArgs.Count() - 1; i >= 0; i--)
                {
                    strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                }
                strSql += " and " + strFilter;
            }

            strSql += strOrderBy;

            dtRes = GetDataTableByCustomerSql(strSql);

            return dtRes;
        }

        /// <summary>
        /// 获取指定科目的月度部门预算流水记录
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public DataTable GetDepartmentMonthlyBudgetDaybook(string strOrgType, string strOrgID, int iYear,
            int iMonthStart, int iMonthEnd, string strOrderBy, string strFilter, List<object> objArgs)
        {
            DataTable dtRes = new DataTable();
            dtRes.Clear();
            if (strOrgType.ToUpper() != "DEPARTMENT" || string.IsNullOrWhiteSpace(strOrgID))
            {
                return dtRes;
            }

            if (iMonthStart <= 0 || iMonthEnd <= 0)
            {
                return dtRes;
            }

            if (iMonthStart > iMonthEnd)
            {
                return dtRes;
            }

            DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
            DateTime dtEnd = new DateTime();
            if (iMonthEnd < 12)
            {
                dtEnd = DateTime.Parse(iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1");
            }
            else
            {
                dtEnd = DateTime.Parse((iYear + 1).ToString() + "-1-1");
            }

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, budgetarymonth 预算月份, subjectname 科目名称, CHARGETYPENAME 预算费用类型, "
                + "case when usablemoney = 99999999 then '不受年度预算控制' else to_char(usablemoney) end 可用额度, "
                + "case when BUDGETMONEY <0 then -BUDGETMONEY else BUDGETMONEY end 预算额度, "
                + "checkstatesname 审核状态, createdate 创建时间, updatedate 终审时间, ownerdepartmentname 所属部门, ownercompanyname 所属公司"
                + " from zzz_dept where ORDERTYPENAME like '%月度%' and OWNERDEPARTMENTID='" + strOrgID + "' and UPDATEDATE >= to_date('"
                + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                strFilter = strFilter.Replace("==", "=");
                for (int i = objArgs.Count() - 1; i >= 0; i--)
                {
                    strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                }
                strSql += " and " + strFilter;
            }

            strSql += strOrderBy;

            dtRes = GetDataTableByCustomerSql(strSql);

            return dtRes;
        }

        /// <summary>
        /// 获取指定科目的部门费用报销，借款及还款流水记录
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public DataTable GetDepartmentChargeDaybook(string strOrgType, string strOrgID, int iYear,
            int iMonthStart, int iMonthEnd, string strOrderBy, string strFilter, List<object> objArgs)
        {
            DataTable dtRes = new DataTable();
            dtRes.Clear();
            if (strOrgType.ToUpper() != "DEPARTMENT" || string.IsNullOrWhiteSpace(strOrgID))
            {
                return dtRes;
            }

            if (iMonthStart <= 0 || iMonthEnd <= 0)
            {
                return dtRes;
            }

            if (iMonthStart > iMonthEnd)
            {
                return dtRes;
            }

            DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
            DateTime dtEnd = new DateTime();
            if (iMonthEnd < 12)
            {
                dtEnd = DateTime.Parse(iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1");
            }
            else
            {
                dtEnd = DateTime.Parse((iYear + 1).ToString() + "-1-1");
            }

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, subjectname 科目名称, CHARGETYPENAME 预算费用类型, "
                + "case when usablemoney = 999999 then '不受月度预算控制' else to_char(usablemoney) end 可用额度, "
                + "case when BUDGETMONEY <0 then -BUDGETMONEY else BUDGETMONEY end 报销额度, checkstatesname 审核状态, createdate 创建时间, "
                + "updatedate 终审时间, ownername 所属员工, ownerpostname 所属岗位, ownerdepartmentname 所属部门, ownercompanyname 所属公司"
                + " from zzz_dept where ORDERTYPENAME in ('报销单','借款单','还款单') and OWNERDEPARTMENTID = '" + strOrgID + "'";

            strSql += " and UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                + "','yyyy-MM-dd') and UPDATEDATE <= to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";
            strSql += " and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')";

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                strFilter = strFilter.Replace("==", "=");
                for (int i = objArgs.Count() - 1; i >= 0; i--)
                {
                    strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                }
                strSql += " and " + strFilter;
            }

            strSql += strOrderBy;

            dtRes = GetDataTableByCustomerSql(strSql);

            return dtRes;
        }
        #endregion

        #region 获取个人指定科目的预算流水记录

        /// <summary>
        /// 获取指定科目的月度个人预算流水记录
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public DataTable GetPersonMonthlyBudgetDaybook(string strOrgType, string strOrgID, int iYear,
            int iMonthStart, int iMonthEnd, string strOrderBy, string strFilter, List<object> objArgs)
        {
            DataTable dtRes = new DataTable();
            dtRes.Clear();
            if (strOrgType.ToUpper() != "PERSONNAL" || string.IsNullOrWhiteSpace(strOrgID))
            {
                return dtRes;
            }

            if (iMonthStart <= 0 || iMonthEnd <= 0)
            {
                return dtRes;
            }

            if (iMonthStart > iMonthEnd)
            {
                return dtRes;
            }

            DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
            DateTime dtEnd = new DateTime();
            if (iMonthEnd < 12)
            {
                dtEnd = DateTime.Parse(iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1");
            }
            else
            {
                dtEnd = DateTime.Parse((iYear + 1).ToString() + "-1-1");
            }

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, budgetarymonth 预算月份, subjectname 科目名称,CHARGETYPENAME 预算费用类型, "
                + "case when usablemoney = 999999 then '不受月度预算控制' else to_char(usablemoney) end 可用额度, "
                + "case when BUDGETMONEY <0 then -BUDGETMONEY else BUDGETMONEY end 预算额度, "
                + "checkstatesname 审核状态, createdate 创建时间, updatedate 终审时间, ownername 所属员工, "
                + "ownerpostname 所属岗位, ownerdepartmentname 所属部门, ownercompanyname 所属公司"
                + " from zzz_person where ORDERTYPENAME not in ('报销单','借款单','还款单') and OWNERID = '" + strOrgID + "'"
                + " and UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and UPDATEDATE < to_date('"
                + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";


            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                strFilter = strFilter.Replace("==", "=");
                for (int i = objArgs.Count() - 1; i >= 0; i--)
                {
                    strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                }
                strSql += " and " + strFilter;
            }

            strSql += strOrderBy;

            dtRes = GetDataTableByCustomerSql(strSql);

            return dtRes;
        }

        /// <summary>
        /// 获取指定科目的个人费用报销，借款及还款流水记录
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public DataTable GetPersonChargeDaybook(string strOrgType, string strOrgID, int iYear,
            int iMonthStart, int iMonthEnd, string strOrderBy, string strFilter, List<object> objArgs)
        {
            DataTable dtRes = new DataTable();
            dtRes.Clear();
            if (strOrgType.ToUpper() != "PERSONNAL" || string.IsNullOrWhiteSpace(strOrgID))
            {
                return dtRes;
            }

            if (iMonthStart <= 0 || iMonthEnd <= 0)
            {
                return dtRes;
            }

            if (iMonthStart > iMonthEnd)
            {
                return dtRes;
            }

            DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
            DateTime dtEnd = new DateTime();
            if (iMonthEnd < 12)
            {
                dtEnd = DateTime.Parse(iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1");
            }
            else
            {
                dtEnd = DateTime.Parse((iYear + 1).ToString() + "-1-1");
            }

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, subjectname 科目名称, CHARGETYPENAME 预算费用类型, "
                + "case when usablemoney = 999999 then '不受月度预算控制' else to_char(usablemoney) end 可用额度, "
                + "case when BUDGETMONEY <0 then -BUDGETMONEY else BUDGETMONEY end 报销额度, "
                + "checkstatesname 审核状态, createdate 创建时间, updatedate 终审时间, "
                + "ownername 所属员工, ownerpostname 所属岗位, ownerdepartmentname 所属部门, ownercompanyname 所属公司"
                + " from zzz_person where ORDERTYPENAME in ('报销单','借款单','还款单') and OWNERID = '" + strOrgID + "'"
                + " and UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and UPDATEDATE < to_date('"
                + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                strFilter = strFilter.Replace("==", "=");
                for (int i = objArgs.Count() - 1; i >= 0; i--)
                {
                    strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                }
                strSql += " and " + strFilter;
            }

            strSql += strOrderBy;

            dtRes = GetDataTableByCustomerSql(strSql);

            return dtRes;
        }
        #endregion

        #endregion

        #endregion

        #region 预算台帐查询

        /// <summary>
        /// 预算台帐查询
        /// </summary>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public List<V_StandingBook> GetStandingBookByMultSearch(string strOrgType, DateTime dtStart, DateTime dtEnd, string strOrderBy, string strFilter, params object[] objArgs)
        {
            Tracer.Debug("根据类型：" + strOrgType+" 查询台账");
            List<V_StandingBook> entResList = new List<V_StandingBook>();
            try
            {
                if (dtStart > dtEnd)
                {
                    return entResList;
                }

                string strCompSql = "select ORDERTYPENAME, ORDERMASTERID, ORDERDETAILID, ORDERCODE, OWNERCOMPANYID, OWNERCOMPANYNAME, OWNERDEPARTMENTID,OWNERDEPARTMENTNAME,"
                    + "OWNERID, OWNERNAME, SUBJECTID, SUBJECTNAME, (case when BUDGETMONEY < 0 then -BUDGETMONEY else BUDGETMONEY end) BUDGETMONEY, CHECKSTATESNAME,"
                    + "CREATEDATE, CREATEUSERID, CREATEUSERNAME, UPDATEDATE from budget_year where UPDATEDATE >= to_date('"
                    + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";
              
                string strDeptSql = "select ORDERTYPENAME, ORDERMASTERID, ORDERDETAILID, ORDERCODE, OWNERCOMPANYID, OWNERCOMPANYNAME, OWNERDEPARTMENTID,OWNERDEPARTMENTNAME,"
                    + "OWNERID, OWNERNAME, SUBJECTID, SUBJECTNAME, (case when BUDGETMONEY < 0 then -BUDGETMONEY else BUDGETMONEY end) BUDGETMONEY, CHECKSTATESNAME,"
                    + "CREATEDATE, CREATEUSERID, CREATEUSERNAME, UPDATEDATE from budget_month where UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and UPDATEDATE <= to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";
                
                string strPersSql = "select ORDERTYPENAME, ORDERMASTERID, ORDERDETAILID, ORDERCODE, OWNERCOMPANYID, OWNERCOMPANYNAME, OWNERDEPARTMENTID,OWNERDEPARTMENTNAME,"
                    + "OWNERID, OWNERNAME, SUBJECTID, SUBJECTNAME, (case when BUDGETMONEY < 0 then -BUDGETMONEY else BUDGETMONEY end) BUDGETMONEY, CHECKSTATESNAME,"
                    + "CREATEDATE, CREATEUSERID, CREATEUSERNAME, UPDATEDATE from zz_charge where UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')"
                    + " and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') ";

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    strFilter = strFilter.Replace("==", "=");
                    for (int i = objArgs.Count() - 1; i >= 0; i--)
                    {
                        if (objArgs[i].ToString() == "月度部门预算")
                        {
                            strFilter = strFilter.Replace("= @" + i.ToString(), "in ('月度部门预算','个人月度部门预算')");
                        }
                        else if (objArgs[i].ToString() == "月度部门增补")
                        {
                            strFilter = strFilter.Replace("= @" + i.ToString(), "in ('月度部门增补','个人月度部门增补')");
                        }
                        else
                        {
                            strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                        }
                    }
                    strCompSql += " and " + strFilter + "order by UPDATEDATE";
                    strDeptSql += " and " + strFilter + "order by UPDATEDATE";
                    strPersSql += " and " + strFilter + "order by UPDATEDATE";
                }

                DataTable dtComp = new DataTable();
                DataTable dtDept = new DataTable();
                DataTable dtPers = new DataTable();
                Tracer.Debug(" 查询的年度预算：" + strCompSql);
                Tracer.Debug(" 查询的部门预算：" + strDeptSql);
                Tracer.Debug(" 查询的费用报销：" + strPersSql);
                dtComp = this.GetDataTableByCustomerSql(strCompSql);
                dtDept = this.GetDataTableByCustomerSql(strDeptSql);
                dtPers = this.GetDataTableByCustomerSql(strPersSql);
                #region "构造年度预算"
                foreach (DataRow drComp in dtComp.Rows)
                {
                    V_StandingBook entResCompany = new V_StandingBook();
                    string strDepartmentID = string.Empty, strSubjectID = string.Empty, strOrderCode = string.Empty;
                    decimal dBudgetmoney = 0;
                    bool bIsExists = false;
                    if (drComp["OWNERDEPARTMENTID"] == null)
                    {
                        continue;
                    }

                    if (drComp["SUBJECTID"] == null || drComp["BUDGETMONEY"] == null || drComp["ORDERCODE"] == null)
                    {
                        continue;
                    }

                    strDepartmentID = drComp["OWNERDEPARTMENTID"].ToString();
                    strSubjectID = drComp["SUBJECTID"].ToString();
                    strOrderCode = drComp["ORDERCODE"].ToString();

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.RecordCode == strOrderCode && n.DepartmentID == strDepartmentID && n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entResCompany = t.FirstOrDefault();
                        }
                    }

                    decimal.TryParse(drComp["BUDGETMONEY"].ToString(), out dBudgetmoney);

                    if (dBudgetmoney == 0)
                    {
                        continue;
                    }

                    entResCompany.TotalMoney += dBudgetmoney;

                    if (!bIsExists)
                    {
                        if (drComp["ORDERTYPENAME"] != null && drComp["ORDERMASTERID"] != null)
                        {
                            entResCompany.XmlObjectValue = GetXmlObject(drComp["ORDERTYPENAME"].ToString(), drComp["ORDERMASTERID"].ToString());
                        }

                        if (drComp["ORDERDETAILID"] != null)
                        {
                            entResCompany.RecordID = drComp["ORDERDETAILID"].ToString();
                        }

                        if (drComp["ORDERCODE"] != null)
                        {
                            entResCompany.RecordCode = drComp["ORDERCODE"].ToString();
                        }

                        entResCompany.DepartmentID = strDepartmentID;

                        if (drComp["OWNERDEPARTMENTNAME"] != null)
                        {
                            entResCompany.DepartmentName = drComp["OWNERDEPARTMENTNAME"].ToString();
                        }

                        if (drComp["OWNERCOMPANYID"] != null)
                        {
                            entResCompany.CompanyID = drComp["OWNERCOMPANYID"].ToString();
                        }

                        if (drComp["OWNERCOMPANYNAME"] != null)
                        {
                            entResCompany.CompanyName = drComp["OWNERCOMPANYNAME"].ToString();
                        }

                        if (drComp["OWNERID"] != null)
                        {
                            entResCompany.OwnerID = drComp["OWNERID"].ToString();
                        }

                        if (drComp["OWNERNAME"] != null)
                        {
                            entResCompany.OwnerName = drComp["OWNERNAME"].ToString();
                        }

                        entResCompany.SubjectID = strSubjectID;

                        if (drComp["SUBJECTNAME"] != null)
                        {
                            entResCompany.SubjectName = drComp["SUBJECTNAME"].ToString();
                        }

                        if (drComp["CHECKSTATESNAME"] != null)
                        {
                            entResCompany.ChecksatesName = drComp["CHECKSTATESNAME"].ToString();
                        }

                        if (drComp["CREATEDATE"] != null)
                        {
                            entResCompany.CreateDate = DateTime.Parse(drComp["CREATEDATE"].ToString());
                        }

                        if (drComp["CREATEUSERID"] != null)
                        {
                            entResCompany.CreateUserID = drComp["CREATEUSERID"].ToString();
                        }

                        if (drComp["CREATEUSERNAME"] != null)
                        {
                            entResCompany.CreateUserName = drComp["CREATEUSERNAME"].ToString();
                        }

                        if (drComp["UPDATEDATE"] != null)
                        {
                            entResCompany.UpdateDate = DateTime.Parse(drComp["UPDATEDATE"].ToString());
                        }

                        entResList.Add(entResCompany);
                    }
                }
                #endregion "结算公司预算"

                #region "构造月度预算数据"
                foreach (DataRow drDept in dtDept.Rows)
                {
                    V_StandingBook entResDepartMent = new V_StandingBook();
                    string strDepartmentID = string.Empty, strSubjectID = string.Empty, strOrderCode = string.Empty;
                    decimal dBudgetmoney = 0;
                    bool bIsExists = false;
                    if (drDept["OWNERDEPARTMENTID"] == null)
                    {
                        continue;
                    }

                    if (drDept["SUBJECTID"] == null || drDept["BUDGETMONEY"] == null || drDept["ORDERCODE"] == null)
                    {
                        continue;
                    }


                    strDepartmentID = drDept["OWNERDEPARTMENTID"].ToString();
                    strSubjectID = drDept["SUBJECTID"].ToString();
                    strOrderCode = drDept["ORDERCODE"].ToString();

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.RecordCode == strOrderCode && n.DepartmentID == strDepartmentID && n.SubjectID == strSubjectID
                                select n;
                        if (strSubjectID == "d5134466-c207-44f2-8a36-cf7b96d5851f")//活动经费特殊处理，绑定个人
                        {   string strowner = drDept["OWNERID"].ToString();
                            t = from n in t
                                where n.OwnerID == strowner
                                select n;
                        }
                        if (t.Count() > 0)
                        {
                            entResDepartMent = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dBudgetmoney);

                    if (dBudgetmoney == 0)
                    {
                        continue;
                    }

                    entResDepartMent.TotalMoney += dBudgetmoney;

                    if (!bIsExists)
                    {
                        if (drDept["ORDERTYPENAME"] != null && drDept["ORDERMASTERID"] != null)
                        {
                            entResDepartMent.XmlObjectValue = GetXmlObject(drDept["ORDERTYPENAME"].ToString(), drDept["ORDERMASTERID"].ToString());
                        }

                        if (drDept["ORDERDETAILID"] != null)
                        {
                            entResDepartMent.RecordID = drDept["ORDERDETAILID"].ToString();
                        }

                        if (drDept["ORDERCODE"] != null)
                        {
                            entResDepartMent.RecordCode = drDept["ORDERCODE"].ToString();
                        }

                        entResDepartMent.DepartmentID = strDepartmentID;

                        if (drDept["OWNERDEPARTMENTNAME"] != null)
                        {
                            entResDepartMent.DepartmentName = drDept["OWNERDEPARTMENTNAME"].ToString();
                        }

                        if (drDept["OWNERCOMPANYID"] != null)
                        {
                            entResDepartMent.CompanyID = drDept["OWNERCOMPANYID"].ToString();
                        }

                        if (drDept["OWNERCOMPANYNAME"] != null)
                        {
                            entResDepartMent.CompanyName = drDept["OWNERCOMPANYNAME"].ToString();
                        }

                        if (drDept["OWNERID"] != null)
                        {
                            entResDepartMent.OwnerID = drDept["OWNERID"].ToString();
                        }

                        if (drDept["OWNERNAME"] != null)
                        {
                            entResDepartMent.OwnerName = drDept["OWNERNAME"].ToString();
                        }

                        entResDepartMent.SubjectID = strSubjectID;

                        if (drDept["SUBJECTNAME"] != null)
                        {
                            entResDepartMent.SubjectName = drDept["SUBJECTNAME"].ToString();
                        }

                        if (drDept["CHECKSTATESNAME"] != null)
                        {
                            entResDepartMent.ChecksatesName = drDept["CHECKSTATESNAME"].ToString();
                        }

                        if (drDept["CREATEDATE"] != null)
                        {
                            entResDepartMent.CreateDate = DateTime.Parse(drDept["CREATEDATE"].ToString());
                        }

                        if (drDept["CREATEUSERID"] != null)
                        {
                            entResDepartMent.CreateUserID = drDept["CREATEUSERID"].ToString();
                        }

                        if (drDept["CREATEUSERNAME"] != null)
                        {
                            entResDepartMent.CreateUserName = drDept["CREATEUSERNAME"].ToString();
                        }

                        if (drDept["UPDATEDATE"] != null)
                        {
                            entResDepartMent.UpdateDate = DateTime.Parse(drDept["UPDATEDATE"].ToString());
                        }

                        entResList.Add(entResDepartMent);
                    }
                }
                #endregion "结束月度预算构造"

                #region "构造个人费用报销数据"
                foreach (DataRow drPers in dtPers.Rows)
                {
                    V_StandingBook entResPerson = new V_StandingBook();
                    string strCompanyID = string.Empty, strDepartmentID = string.Empty, strSubjectID = string.Empty, strOrderID = string.Empty, strOrderCode = string.Empty;
                    decimal dBudgetmoney = 0;
                    bool bIsExists = false;
                    if (drPers["OWNERDEPARTMENTID"] == null)
                    {
                        continue;
                    }

                    if (drPers["SUBJECTID"] == null || drPers["BUDGETMONEY"] == null || drPers["ORDERCODE"] == null)
                    {
                        continue;
                    }

                    strCompanyID = drPers["OWNERCOMPANYID"].ToString();
                    strDepartmentID = drPers["OWNERDEPARTMENTID"].ToString();
                    strSubjectID = drPers["SUBJECTID"].ToString();
                    strOrderID = drPers["ORDERDETAILID"].ToString();
                    strOrderCode = drPers["ORDERCODE"].ToString();

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.RecordID == strOrderID && n.RecordCode == strOrderCode && n.DepartmentID == strDepartmentID && n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entResPerson = t.FirstOrDefault();
                            continue;
                        }
                    }

                    decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dBudgetmoney);

                    if (dBudgetmoney == 0)
                    {
                        continue;
                    }

                    entResPerson.TotalMoney += dBudgetmoney;

                    if (!bIsExists)
                    {
                        if (drPers["ORDERTYPENAME"] != null && drPers["ORDERMASTERID"] != null)
                        {
                            entResPerson.XmlObjectValue = GetXmlObject(drPers["ORDERTYPENAME"].ToString(), drPers["ORDERMASTERID"].ToString());
                        }

                        if (drPers["ORDERDETAILID"] != null)
                        {
                            entResPerson.RecordID = drPers["ORDERDETAILID"].ToString();
                        }

                        if (drPers["ORDERCODE"] != null)
                        {
                            entResPerson.RecordCode = drPers["ORDERCODE"].ToString();
                        }

                        entResPerson.DepartmentID = strDepartmentID;

                        if (drPers["OWNERDEPARTMENTNAME"] != null)
                        {
                            entResPerson.DepartmentName = drPers["OWNERDEPARTMENTNAME"].ToString();
                        }

                        if (drPers["OWNERCOMPANYID"] != null)
                        {
                            entResPerson.CompanyID = drPers["OWNERCOMPANYID"].ToString();
                        }

                        if (drPers["OWNERCOMPANYNAME"] != null)
                        {
                            entResPerson.CompanyName = drPers["OWNERCOMPANYNAME"].ToString();
                        }

                        if (drPers["OWNERID"] != null)
                        {
                            entResPerson.OwnerID = drPers["OWNERID"].ToString();
                        }

                        if (drPers["OWNERNAME"] != null)
                        {
                            entResPerson.OwnerName = drPers["OWNERNAME"].ToString();
                        }

                        entResPerson.SubjectID = strSubjectID;

                        if (drPers["SUBJECTNAME"] != null)
                        {
                            entResPerson.SubjectName = drPers["SUBJECTNAME"].ToString();
                        }

                        if (drPers["CHECKSTATESNAME"] != null)
                        {
                            entResPerson.ChecksatesName = drPers["CHECKSTATESNAME"].ToString();
                        }

                        if (drPers["CREATEDATE"] != null)
                        {
                            entResPerson.CreateDate = DateTime.Parse(drPers["CREATEDATE"].ToString());
                        }

                        if (drPers["CREATEUSERID"] != null)
                        {
                            entResPerson.CreateUserID = drPers["CREATEUSERID"].ToString();
                        }

                        if (drPers["CREATEUSERNAME"] != null)
                        {
                            entResPerson.CreateUserName = drPers["CREATEUSERNAME"].ToString();
                        }

                        if (drPers["UPDATEDATE"] != null)
                        {
                            entResPerson.UpdateDate = DateTime.Parse(drPers["UPDATEDATE"].ToString());
                        }

                        entResList.Add(entResPerson);
                    }
                }
                #endregion "结束个人预算"
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
            }

            return entResList.OrderBy(t => t.UpdateDate).ToList();
        }

        /// <summary>
        /// 构造台帐单据弹出Form所需Xml信息
        /// </summary>
        /// <param name="strOrderTypeName">单据类型</param>
        /// <param name="strOrderID">单据ID</param>
        /// <returns></returns>
        private string GetXmlObject(string strOrderTypeName, string strOrderID)
        {
            if (string.IsNullOrWhiteSpace(strOrderTypeName) || string.IsNullOrWhiteSpace(strOrderID))
            {
                return string.Empty;
            }


            string strSysName = string.Empty, strModelCode = string.Empty, strFormName = string.Empty, strSubmitXmlObj = string.Empty;
            strSysName = "FB";
            switch (strOrderTypeName)
            {
                case "年度部门预算":
                    strModelCode = "T_FB_COMPANYBUDGETAPPLYMASTER";
                    break;
                case "年度部门预算增补":
                    strModelCode = "T_FB_COMPANYBUDGETMODMASTER";
                    break;
                case "月度部门预算":
                case "个人月度部门预算":
                    strModelCode = "T_FB_DEPTBUDGETAPPLYMASTER";
                    break;
                case "月度部门增补":
                case "个人月度部门增补":
                    strModelCode = "T_FB_DEPTBUDGETADDMASTER";
                    break;
                case "报销单":
                    strModelCode = "T_FB_CHARGEAPPLYMASTER";
                    break;
                case "个人调拨(调入)":
                    strModelCode = "T_FB_DEPTTRANSFERMASTER";
                    break;
            }
            strFormName = SMT.SaaS.BLLCommonServices.Utility.GetResourceValue(strModelCode);
            strSubmitXmlObj = SMT.SaaS.BLLCommonServices.Utility.SetSubmitXmlObj(strSysName, strFormName, strOrderID, "AUDIT");
            return strSubmitXmlObj;
        }

                #endregion

        #region 预算年度预算分析

        /// <summary>
        /// 预算年度预算分析
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public List<V_YearlyBudgetAnalysis> GetYearlyBudgetAnalysisByMultSearch(string strOrgType, int iYear, int iMonthStart, int iMonthEnd,
            string strOrderBy, string strFilter, params object[] objArgs)
        {
            List<V_YearlyBudgetAnalysis> entResList = new List<V_YearlyBudgetAnalysis>();
            try
            {
                if (iYear <= 0)
                {
                    return entResList;
                }

                if (iMonthStart <= 0 || iMonthEnd <= 0)
                {
                    return entResList;
                }

                if (iMonthStart > iMonthEnd)
                {
                    return entResList;
                }

                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                DateTime dtEnd = new DateTime();
                if (iMonthEnd < 12)
                {
                    dtEnd = DateTime.Parse(iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1");
                }
                else
                {
                    dtEnd = DateTime.Parse((iYear + 1).ToString() + "-1-1");
                }

                string strCompSql = "select * from budget_year where CHECKSTATES=2 and CHECKSTATESNAME='审核通过' and BUDGETARYMONTH >= to_date('"
                    + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and BUDGETARYMONTH < to_date('" + dtEnd.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and (ORDERTYPENAME = '年度部门预算增补' OR ORDERTYPENAME = '年度部门预算')";
                string strPersSql = "select * from zz_charge where CHECKSTATES=2 and CHECKSTATESNAME='审核通过' and UPDATEDATE >= to_date('"
                    + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and ORDERTYPENAME = '报销单'";

                if (strOrgType.ToUpper() == "POST" || strOrgType.ToUpper() == "PERSONNAL")
                {
                    strPersSql += " and CHARGETYPENAME = '个人预算费用'";
                }

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    strFilter = strFilter.Replace("==", "=");
                    for (int i = objArgs.Count() - 1; i >= 0; i--)
                    {
                        strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                    }
                    strCompSql += " and " + strFilter;
                    strPersSql += " and " + strFilter;
                }


                DataTable dtComp = new DataTable();
                DataTable dtPers = new DataTable();

                if (strOrgType.ToUpper() != "POST" && strOrgType.ToUpper() != "PERSONNAL")
                {
                    dtComp = this.GetDataTableByCustomerSql(strCompSql);
                }

                dtPers = this.GetDataTableByCustomerSql(strPersSql);

                foreach (DataRow drComp in dtComp.Rows)
                {
                    V_YearlyBudgetAnalysis entRes = new V_YearlyBudgetAnalysis();
                    string strOrderTypeName = string.Empty, strSubjectID = string.Empty;
                    decimal dBudgetApplymoney = 0, dBudgetAddmoney = 0;
                    bool bIsExists = false;
                    if (drComp["OWNERDEPARTMENTID"] == null)
                    {
                        continue;
                    }

                    if (drComp["SUBJECTID"] == null || drComp["BUDGETMONEY"] == null)
                    {
                        continue;
                    }

                    strOrderTypeName = drComp["ORDERTYPENAME"].ToString();
                    strSubjectID = drComp["SUBJECTID"].ToString();

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }


                    if (strOrderTypeName == "年度部门预算")
                    {
                        decimal.TryParse(drComp["BUDGETMONEY"].ToString(), out dBudgetApplymoney);
                    }
                    else if (strOrderTypeName == "年度部门预算增补")
                    {
                        decimal.TryParse(drComp["BUDGETMONEY"].ToString(), out dBudgetAddmoney);
                    }


                    entRes.YearBudgetApply += dBudgetApplymoney;
                    entRes.YearBudgetAdd += dBudgetAddmoney;

                    decimal dBugetMoneyTemp = entRes.YearBudgetApply + entRes.YearBudgetAdd;
                    entRes.YearBudgetDeviation = dBugetMoneyTemp - entRes.YearChargeMoney;

                    if (dBugetMoneyTemp > 0)
                    {
                        entRes.YearBudgetDeviationRate = decimal.Round(entRes.YearBudgetDeviation / dBugetMoneyTemp, 4);
                    }

                    if (!bIsExists)
                    {
                        entRes.SubjectID = strSubjectID;

                        if (drComp["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drComp["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                }

                foreach (DataRow drPers in dtPers.Rows)
                {
                    V_YearlyBudgetAnalysis entRes = new V_YearlyBudgetAnalysis();
                    string strOrderTypeName = string.Empty, strSubjectID = string.Empty;
                    decimal dChargeMoney = 0;
                    bool bIsExists = false;

                    if (drPers["SUBJECTID"] == null || drPers["BUDGETMONEY"] == null)
                    {
                        continue;
                    }

                    strOrderTypeName = drPers["ORDERTYPENAME"].ToString();
                    strSubjectID = drPers["SUBJECTID"].ToString();

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dChargeMoney);
                    if (dChargeMoney < 0)
                    {
                        dChargeMoney = 0 - dChargeMoney;
                    }

                    entRes.YearChargeMoney += dChargeMoney;

                    decimal dBugetMoneyTemp = entRes.YearBudgetApply + entRes.YearBudgetAdd;
                    entRes.YearBudgetDeviation = dBugetMoneyTemp - entRes.YearChargeMoney;

                    if (dBugetMoneyTemp > 0)
                    {
                        entRes.YearBudgetDeviationRate = decimal.Round(entRes.YearBudgetDeviation / dBugetMoneyTemp, 4);
                    }

                    if (!bIsExists)
                    {
                        entRes.SubjectID = strSubjectID;

                        if (drPers["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drPers["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
            }

            return entResList;
        }
        #endregion

        #region 预算月度预算分析

        /// <summary>
        /// 预算月度预算分析
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public List<V_MonthlyBudgetAnalysis> GetMonthlyBudgetAnalysisByMultSearch(string strOrgType, int iYear, int iMonth,
            string strOrderBy, string strFilter, params object[] objArgs)
        {
            List<V_MonthlyBudgetAnalysis> entResList = new List<V_MonthlyBudgetAnalysis>();
            try
            {
                if (iYear <= 0 || iMonth <= 0)
                {
                    return entResList;
                }

                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-1-1");
                DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonth.ToString() + "-1").AddMonths(1);

                string strDeptSql = "select * from budget_month where CHECKSTATES=2 and CHECKSTATESNAME='审核通过' and BUDGETARYMONTH >= to_date('"
                    + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and BUDGETARYMONTH < to_date('" + dtEnd.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd')";
                string strPersSql = "select * from zz_charge where CHECKSTATES=2 and CHECKSTATESNAME='审核通过' and UPDATEDATE >= to_date('"
                    + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd')";

                if (strOrgType.ToUpper() == "POST" || strOrgType.ToUpper() == "PERSONNAL")
                {
                    strDeptSql += " and CHARGETYPENAME = '个人预算费用'";
                    strPersSql += " and CHARGETYPENAME = '个人预算费用'";
                }

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    strFilter = strFilter.Replace("==", "=");
                    for (int i = objArgs.Count() - 1; i >= 0; i--)
                    {
                        strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                    }

                    strDeptSql += " and " + strFilter;
                    strPersSql += " and " + strFilter;
                }

                DataTable dtDept = new DataTable();
                DataTable dtPers = new DataTable();

                dtDept = this.GetDataTableByCustomerSql(strDeptSql);
                dtPers = this.GetDataTableByCustomerSql(strPersSql);

                foreach (DataRow drDept in dtDept.Rows)
                {
                    V_MonthlyBudgetAnalysis entRes = new V_MonthlyBudgetAnalysis();
                    string strOrderTypeName = string.Empty, strSubjectID = string.Empty;
                    decimal dBudgetApply = 0, dBudgetAdd = 0, dChargeMoney = 0;
                    bool bIsExists = false;
                    DateTime dtCurMonth = new DateTime();

                    if (drDept["SUBJECTID"] == null || drDept["BUDGETMONEY"] == null || drDept["BUDGETARYMONTH"] == null)
                    {
                        continue;
                    }

                    strOrderTypeName = drDept["ORDERTYPENAME"].ToString();
                    strSubjectID = drDept["SUBJECTID"].ToString();
                    DateTime.TryParse(drDept["BUDGETARYMONTH"].ToString(), out dtCurMonth);

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    if (strOrderTypeName == "月度部门预算")
                    {
                        decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dBudgetApply);
                    }
                    else if (strOrderTypeName == "月度部门增补")
                    {
                        decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dBudgetAdd);
                    }

                    if (dtCurMonth.Month == iMonth)
                    {
                        entRes.MonthBudgetApply += dBudgetApply;
                        entRes.MonthBudgetAdd += dBudgetAdd;
                        entRes.MonthChargeMoney += dChargeMoney;
                    }

                    if (!bIsExists)
                    {
                        entRes.LastBudgetMonth = GetLastBudgetMonth(strOrgType, iYear, iMonth, strSubjectID, strFilter, objArgs);
                    }

                    decimal dBugetMoneyTemp = entRes.LastBudgetMonth + entRes.MonthBudgetApply + entRes.MonthBudgetAdd;
                    entRes.MonthBudgetDeviation = dBugetMoneyTemp - entRes.MonthChargeMoney;
                    if (dBugetMoneyTemp > 0)
                    {
                        entRes.MonthBudgetDeviationRate = decimal.Round(entRes.MonthBudgetDeviation / dBugetMoneyTemp, 4);
                    }

                    if (!bIsExists)
                    {
                        entRes.SubjectID = strSubjectID;

                        if (drDept["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drDept["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                }

                foreach (DataRow drPers in dtPers.Rows)
                {
                    V_MonthlyBudgetAnalysis entRes = new V_MonthlyBudgetAnalysis();
                    string strOrderTypeName = string.Empty, strSubjectID = string.Empty;
                    decimal dBudgetApply = 0, dBudgetAdd = 0, dChargeMoney = 0;
                    bool bIsExists = false;
                    DateTime dtCurMonth = new DateTime();

                    if (drPers["SUBJECTID"] == null || drPers["BUDGETMONEY"] == null || drPers["BUDGETARYMONTH"] == null)
                    {
                        continue;
                    }

                    strOrderTypeName = drPers["ORDERTYPENAME"].ToString();
                    strSubjectID = drPers["SUBJECTID"].ToString();
                    DateTime.TryParse(drPers["BUDGETARYMONTH"].ToString(), out dtCurMonth);

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    if (strOrderTypeName == "报销单")
                    {
                        decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dChargeMoney);
                        if (dChargeMoney < 0)
                        {
                            dChargeMoney = 0 - dChargeMoney;
                        }
                    }

                    if (dtCurMonth.Month == iMonth)
                    {
                        entRes.MonthBudgetApply += dBudgetApply;
                        entRes.MonthBudgetAdd += dBudgetAdd;
                        entRes.MonthChargeMoney += dChargeMoney;
                    }

                    if (!bIsExists)
                    {
                        entRes.LastBudgetMonth = GetLastBudgetMonth(strOrgType, iYear, iMonth, strSubjectID, strFilter, objArgs);
                    }

                    decimal dBugetMoneyTemp = entRes.LastBudgetMonth + entRes.MonthBudgetApply + entRes.MonthBudgetAdd;

                    entRes.MonthBudgetDeviation = dBugetMoneyTemp - entRes.MonthChargeMoney;

                    if (dBugetMoneyTemp > 0)
                    {
                        entRes.MonthBudgetDeviationRate = decimal.Round(entRes.MonthBudgetDeviation / dBugetMoneyTemp, 4);
                    }

                    if (!bIsExists)
                    {
                        entRes.SubjectID = strSubjectID;

                        if (drPers["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drPers["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
            }

            return entResList;
        }

        /// <summary>
        /// 获取上月预算结余
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonth"></param>
        /// <param name="strSubjectID"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        private decimal GetLastBudgetMonth(string strOrgType, int iYear, int iMonth, string strSubjectID, string strFilter, object[] objArgs)
        {
            decimal dLastBudgetMonth = 0;
            if (iYear <= 0)
            {
                return dLastBudgetMonth;
            }

            //1月没有上月预算结余
            if (iMonth <= 1)
            {
                return dLastBudgetMonth;
            }

            //部门经费，活动经费都是特殊科目，可以跨年使用
            string strSpecialSubject = System.Configuration.ConfigurationManager.AppSettings["SpecialSubject"].ToString();

            DateTime dtStart = DateTime.Parse(iYear.ToString() + "-1-1");
            if (strSpecialSubject.Contains(strSubjectID))
            {
                dtStart = new DateTime();
            }

            DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonth.ToString() + "-1");

            //string strDeptSql = "select SUBJECTID, SUBJECTNAME, sum(BUDGETMONEY) BUDGETMONEY"
            //        + " from zzz_dept where CHECKSTATESNAME = '审核通过' and UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd")
            //        + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')"
            //        + " and SUBJECTID = '" + strSubjectID + "'";、

            //加上预算月份的判断，假如8月预算在7月进行审核通过，这是要这个预算不能算在7月，所以加上BUDGETARYMONTH和更新日期判断
            string strDeptSql = "select SUBJECTID, SUBJECTNAME, sum(BUDGETMONEY) BUDGETMONEY"
                    + " from zzz_dept where CHECKSTATESNAME = '审核通过' and UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')"
                    + " and SUBJECTID = '" + strSubjectID + "'" + "and BUDGETARYMONTH < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";

            //string strPersSql = "select SUBJECTID, SUBJECTNAME, sum(BUDGETMONEY) BUDGETMONEY"
            //    + " from zzz_person where UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')"
            //    + " and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and CHECKSTATESNAME = '审核通过'"
            //    + " and SUBJECTID = '" + strSubjectID + "'";

            //加上报销年份为当年的条件，因为会有去年报销今年审核的单据存在
            string strPersSql = "select SUBJECTID, SUBJECTNAME, sum(BUDGETMONEY) BUDGETMONEY"
                + " from zzz_person where UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')"
                + " and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and CHECKSTATESNAME = '审核通过'"
                + " and SUBJECTID = '" + strSubjectID + "'" + "and BUDGETARYMONTH >= to_date('" + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";

            if (strOrgType.ToUpper() == "POST" || strOrgType.ToUpper() == "PERSONNAL")
            {
                strDeptSql += " and CHARGETYPENAME = '个人预算费用'";
                strPersSql += " and CHARGETYPENAME = '个人预算费用'";
            }


            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                strFilter = strFilter.Replace("==", "=");
                for (int i = objArgs.Count() - 1; i >= 0; i--)
                {
                    strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                }

                strDeptSql += " and " + strFilter + "group by SUBJECTID, SUBJECTNAME";
                strPersSql += " and " + strFilter + "group by SUBJECTID, SUBJECTNAME";
            }

            DataTable dtDept = new DataTable();
            DataTable dtPers = new DataTable();

            dtDept = this.GetDataTableByCustomerSql(strDeptSql);
            dtPers = this.GetDataTableByCustomerSql(strPersSql);

            foreach (DataRow drDept in dtDept.Rows)
            {
                if (drDept["SUBJECTID"] == null || drDept["BUDGETMONEY"] == null)
                {
                    continue;
                }

                string strTempSubjectid = string.Empty;
                decimal dTempUsableMoney = 0;

                strTempSubjectid = drDept["SUBJECTID"].ToString();
                decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dTempUsableMoney);

                if (strTempSubjectid == strSubjectID)
                {
                    dLastBudgetMonth += dTempUsableMoney;
                }
            }

            foreach (DataRow drPers in dtPers.Rows)
            {
                if (drPers["SUBJECTID"] == null || drPers["BUDGETMONEY"] == null)
                {
                    continue;
                }

                string strTempSubjectid = string.Empty;
                decimal dTempUsableMoney = 0;

                strTempSubjectid = drPers["SUBJECTID"].ToString();
                decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dTempUsableMoney);

                if (strTempSubjectid == strSubjectID)
                {
                    dLastBudgetMonth += dTempUsableMoney;
                }
            }

            return dLastBudgetMonth;
        }
        #endregion

        #endregion
    }
}

