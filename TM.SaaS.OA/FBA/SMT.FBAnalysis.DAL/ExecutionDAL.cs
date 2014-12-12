using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using System.Collections;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;
using SMT_FB_EFModel;
using SMT.FBAnalysis.CustomModel;
using SMT.Foundation.Log;
using System.Data;
using SMT.Foundation.Core;
using SMT.SaaS.BLLCommonServices.OrganizationWS;

namespace SMT.FBAnalysis.DAL
{
    /// <summary>
    /// 预算查询分析数据层函数类
    /// </summary>
    public class ExecutionDAL : CommDal<T_FB_BUDGETACCOUNT>
    {
        #region 预算流水

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

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, subjectname 科目名称, "
                + "case when BUDGETMONEY < 0 then -BUDGETMONEY else BUDGETMONEY end 预算额度, checkstatesname 审核状态, "
                + "createdate 创建时间, updatedate 终审时间, ownercompanyname 所属公司, ownerdepartmentname 所属部门"
                + " from zzz_comp where ORDERTYPENAME like '%年度%' and OWNERDEPARTMENTID='" + strOrgID + "'";

            strSql += " and UPDATEDATE >= to_date('" + iYear.ToString() + "-" + iMonthStart.ToString() + "-1', 'yyyy-MM-dd')";
            if (iMonthEnd == 12)
            {
                strSql += " and UPDATEDATE < to_date('" + (iYear + 1).ToString() + "-" + (iMonthEnd - 11).ToString() + "-1', 'yyyy-MM-dd')";
            }
            else
            {
                strSql += " and UPDATEDATE < to_date('" + iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1', 'yyyy-MM-dd')";
            }
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

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, budgetarymonth 预算月份, subjectname 科目名称, CHARGETYPENAME 预算费用类型, "
                + "case when usablemoney = 99999999 or usablemoney = 9999999999999 then '不受年度预算限制' else to_char(usablemoney) end 可用额度, "
                + "case when BUDGETMONEY <0 then -BUDGETMONEY else BUDGETMONEY end 预算额度, "
                + "checkstatesname 审核状态, createdate 创建时间, updatedate 终审时间, ownerdepartmentname 所属部门, ownercompanyname 所属公司"
                + " from zzz_dept where ORDERTYPENAME like '%月度%' and OWNERDEPARTMENTID='" + strOrgID + "'";

            strSql += " and UPDATEDATE >= to_date('" + iYear.ToString() + "-" + iMonthStart.ToString() + "-1', 'yyyy-MM-dd')";
            if (iMonthEnd == 12)
            {
                strSql += " and UPDATEDATE < to_date('" + (iYear + 1).ToString() + "-" + (iMonthEnd - 11).ToString() + "-1', 'yyyy-MM-dd')";
            }
            else
            {
                strSql += " and UPDATEDATE < to_date('" + iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1', 'yyyy-MM-dd')";
            }
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

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, subjectname 科目名称, CHARGETYPENAME 预算费用类型, "
                + "case when usablemoney = 999999 or usablemoney = 99999999 or usablemoney = 999999999999 THEN '不受月度预算限制' else to_char(usablemoney) end 可用额度, "
                + "remark 摘要, case when BUDGETMONEY <0 then -BUDGETMONEY else BUDGETMONEY end 报销额度, checkstatesname 审核状态, createdate 创建时间, "
                + "updatedate 终审时间, ownername 所属员工, ownerpostname 所属岗位, ownerdepartmentname 所属部门, ownercompanyname 所属公司"
                + " from zz_charge where chargetype = 2 and OWNERDEPARTMENTID = '" + strOrgID + "'";

            strSql += " and UPDATEDATE >= to_date('" + iYear.ToString() + "-" + iMonthStart.ToString() + "-1', 'yyyy-MM-dd')";
            if (iMonthEnd == 12)
            {
                strSql += " and UPDATEDATE < to_date('" + (iYear + 1).ToString() + "-" + (iMonthEnd - 11).ToString() + "-1', 'yyyy-MM-dd')";
            }
            else
            {
                strSql += " and UPDATEDATE < to_date('" + iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1', 'yyyy-MM-dd')";
            }
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
            if (string.IsNullOrWhiteSpace(strOrgType) || string.IsNullOrWhiteSpace(strOrgID))
            {
                return dtRes;
            }

            if (strOrgType.ToUpper() != "DEPARTMENT" && strOrgType.ToUpper() != "PERSONNEL")
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

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, budgetarymonth 预算月份, subjectname 科目名称,CHARGETYPENAME 预算费用类型, "
                + "case when usablemoney = 9999999999999 or usablemoney = 999999999999 THEN '不受年度预算限制' else to_char(usablemoney) end 可用额度, "
                + "case when BUDGETMONEY <0 then -BUDGETMONEY else BUDGETMONEY end 预算额度, "
                + "checkstatesname 审核状态, createdate 创建时间, updatedate 终审时间, ownername 所属员工, "
                + "ownerpostname 所属岗位, ownerdepartmentname 所属部门, ownercompanyname 所属公司"
                + " from zzz_person where ORDERTYPENAME not in ('报销单','借款单','还款单')";
            
            switch (strOrgType.ToUpper())
            {
                case "DEPARTMENT":
                    strSql += "and OWNERDEPARTMENTID = '" + strOrgID + "'";
                    break;
                case "PERSONNEL":
                    strSql += "and OWNERID = '" + strOrgID + "'";
                    break;
            }

            strSql += " and UPDATEDATE >= to_date('" + iYear.ToString() + "-" + iMonthStart.ToString() + "-1', 'yyyy-MM-dd')";
            if (iMonthEnd == 12)
            {
                strSql += " and UPDATEDATE < to_date('" + (iYear + 1).ToString() + "-" + (iMonthEnd - 11).ToString() + "-1', 'yyyy-MM-dd')";
            }
            else
            {
                strSql += " and UPDATEDATE < to_date('" + iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1', 'yyyy-MM-dd')";
            }
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
            if (string.IsNullOrWhiteSpace(strOrgType) || string.IsNullOrWhiteSpace(strOrgID))
            {
                return dtRes;
            }

            if (strOrgType.ToUpper() != "DEPARTMENT" && strOrgType.ToUpper() != "PERSONNEL")
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

            string strSql = "select ordertypename 单据类型, ordercode 单据编号, subjectname 科目名称, CHARGETYPENAME 预算费用类型, "
                + "case when usablemoney = 999999 or usablemoney = 99999999 or usablemoney = 999999999999 THEN '不受月度预算限制' else to_char(usablemoney) end 可用额度, "
                + "remark 摘要, case when BUDGETMONEY <0 then -BUDGETMONEY else BUDGETMONEY end 报销额度, "
                + "checkstatesname 审核状态, createdate 创建时间, updatedate 终审时间, "
                + "ownername 所属员工, ownerpostname 所属岗位, ownerdepartmentname 所属部门, ownercompanyname 所属公司"
                + " from zz_charge where chargetype = 1 ";

            switch (strOrgType.ToUpper())
            {
                case "DEPARTMENT":
                    strSql += "and OWNERDEPARTMENTID = '" + strOrgID + "'";
                    break;
                case "PERSONNEL":
                    strSql += "and OWNERID = '" + strOrgID + "'";
                    break;
            }

            strSql += " and UPDATEDATE >= to_date('" + iYear.ToString() + "-" + iMonthStart.ToString() + "-1', 'yyyy-MM-dd')";
            if (iMonthEnd == 12)
            {
                strSql += " and UPDATEDATE < to_date('" + (iYear + 1).ToString() + "-" + (iMonthEnd - 11).ToString() + "-1', 'yyyy-MM-dd')";
            }
            else
            {
                strSql += " and UPDATEDATE < to_date('" + iYear.ToString() + "-" + (iMonthEnd + 1).ToString() + "-1', 'yyyy-MM-dd')";
            }
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

        #endregion 预算流水

        #region 执行一览
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

                //if (string.IsNullOrWhiteSpace(strOrgType) || string.IsNullOrWhiteSpace(strOrgID))
                //{
                //    return entResList;
                //}

                //if (strOrgType.ToUpper() != "COMPANY" && strOrgType.ToUpper() != "DEPARTMENT")
                //{
                //    return entResList;
                //}

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

                //年度预算
                string strCompSql = "select * from budget_year where CHECKSTATES=2 and CHECKSTATESNAME='审核通过' and BUDGETARYMONTH >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and BUDGETARYMONTH < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and ORDERTYPENAME like '%年度%'";

                //月度预算(含部门和个人)
                //OrderTypeName != '个人调拨(调入)'不算月度部门分配，暂时这样判断吧
                //2012-09-05
                string strDeptSql = "select * from budget_month where BUDGETARYMONTH >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and BUDGETARYMONTH < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') and OrderTypeName != '个人调拨(调入)'";

                //报销
                string strPersSql = "select * from zz_charge where UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";

                var tempSql = "SELECT T.*, SC.SUBJECTNAME FROM T_FB_BUDGETCHECK T,"
                        + " (select '08c1d9c6-2396-43c3-99f9-227e4a7eb417' as SUBJECTID, '部门经费' as SUBJECTNAME from dual"
                        + " union select 'd5134466-c207-44f2-8a36-cf7b96d5851f' as SUBJECTID, '活动经费' as SUBJECTNAME from dual) sc"
                        + " WHERE T.ACCOUNTOBJECTTYPE = 2 AND T.BUDGETMONTH = 12 AND t.SUBJECTID = sc.SUBJECTID"
                        + " and T.CREATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')"
                        + " and T.CREATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";
                tempSql = string.Format("select * from ({0}) where 1=1 ", tempSql);
                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    strFilter = strFilter.Replace("==", "=");
                    for (int i = objArgs.Count() - 1; i >= 0; i--)
                    {
                        strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                    }
                    strFilter += " order by OWNERCOMPANYID,  OWNERDEPARTMENTID, OWNERPOSTID, OWNERID, SUBJECTID";
                    strCompSql += " and " + strFilter;
                    strDeptSql += " and " + strFilter;
                    strPersSql += " and " + strFilter;
                    tempSql += " and " + strFilter;
                }


                DataTable dtComp = new DataTable();
                DataTable dtDept = new DataTable();
                DataTable dtPers = new DataTable();
                DataTable dtTemp = new DataTable();

                dtComp = this.GetDataTableByCustomerSql(strCompSql);
                dtDept = this.GetDataTableByCustomerSql(strDeptSql);
                dtPers = this.GetDataTableByCustomerSql(strPersSql);
                dtTemp = this.GetDataTableByCustomerSql(tempSql);

                #region dtComp
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
                    entRes.BudgetMonth = entRes.BudgetMoneyMonth - entRes.ApprovedApplyMoney;
                    entRes.BudgetYear = entRes.BudgetMoneyYear - entRes.BudgetMoneyMonth;

                    if (entRes.BudgetMoneyYear != 0)
                    {
                        entRes.BudgetSeizureRate = decimal.Round(entRes.BudgetMoneyMonth / entRes.BudgetMoneyYear, 2);
                    }

                    if (!bIsExists)
                    {
                        entRes.OrganizationID = strDepartmentID;
                        entRes.OrganizationName = strDepartmentID;
                        if (drComp["OWNERCOMPANYNAME"] != DBNull.Value && drComp["OWNERDEPARTMENTNAME"] != DBNull.Value)
                        {
                            entRes.OrganizationName = drComp["OWNERCOMPANYNAME"].ToString() + "-" + drComp["OWNERDEPARTMENTNAME"].ToString();
                        }

                        entRes.SubjectID = strSubjectID;

                        if (drComp["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drComp["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                }
                #endregion dtComp


                foreach (DataRow drTemp in dtTemp.Rows)
                {
                    DataRow dr = dtDept.NewRow();
                    dr["OWNERCOMPANYID"] = drTemp["OWNERCOMPANYID"];
                    dr["OWNERDEPARTMENTID"] = drTemp["OWNERDEPARTMENTID"];
                    dr["ORDERTYPENAME"] = "期初结余";
                    dr["CHECKSTATESNAME"] = "审核通过";
                    dr["UPDATEDATE"] = drTemp["UPDATEDATE"];
                    
                    dr["SUBJECTID"] = drTemp["SUBJECTID"];
                    dr["SUBJECTNAME"] = drTemp["SUBJECTNAME"];
                    dr["BUDGETMONEY"] = drTemp["USABLEMONEY"];
                    dtDept.Rows.Add(dr);
                }
                foreach (DataRow drDept in dtDept.Rows)
                {
                    V_ExecutionList entRes = new V_ExecutionList();
                    bool bIsExists = false;
                    string strCompanyID = string.Empty, strDepartmentID = string.Empty, strSubjectID = string.Empty;
                    string strOrderTypeName = string.Empty, strCheckstatesName = string.Empty,CheckState=string.Empty;
                    decimal dBudgetmoney = 0;

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
                    CheckState = drDept["CHECKSTATES"].ToString();

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

                    if (strCheckstatesName != "审核通过")
                    {
                        continue;
                    }

                    decimal.TryParse(drDept["BUDGETMONEY"].ToString(), out dBudgetmoney);

                    if (dBudgetmoney != 0)
                    {
                        entRes.BudgetMoneyMonth += dBudgetmoney;
                    }

                    entRes.BudgetMonth = entRes.BudgetMoneyMonth - entRes.ApprovedApplyMoney;
                    if (Convert.ToString( drDept["ORDERTYPENAME"]) != "期初结余")
                    {
                        entRes.BudgetYear = entRes.BudgetMoneyYear - entRes.BudgetMoneyMonth;
                        if (entRes.BudgetMoneyYear != 0)
                        {
                            entRes.BudgetSeizureRate = decimal.Round(entRes.BudgetMoneyMonth / entRes.BudgetMoneyYear, 2);
                        }
                    }

                    

                    if (!bIsExists)
                    {
                        entRes.OrganizationID = strDepartmentID;
                        entRes.OrganizationName = strDepartmentID;
                        if (drDept["OWNERCOMPANYNAME"] != DBNull.Value && drDept["OWNERDEPARTMENTNAME"] != DBNull.Value)
                        {
                            entRes.OrganizationName = drDept["OWNERCOMPANYNAME"].ToString() + "-" + drDept["OWNERDEPARTMENTNAME"].ToString();
                        }

                        entRes.SubjectID = strSubjectID;

                        if (drDept["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drDept["SUBJECTNAME"].ToString();
                        }

                        entResList.Add(entRes);
                    }
                }
                OrganizationServiceClient organizationServiceClient = new OrganizationServiceClient();
                entResList.ForEach(item =>
                    {
                        if (item.OrganizationName == item.OrganizationID)
                        {
                            var dept = organizationServiceClient.GetDepartmentById(item.OrganizationID);
                            if (dept != null)
                            {
                                item.OrganizationName = dept.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME + "-" + dept.T_HR_COMPANY.CNAME;
                            }
                        }
                    });

                foreach (DataRow drPers in dtPers.Rows)
                {
                    V_ExecutionList entRes = new V_ExecutionList();
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


                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核通过")
                    {
                        decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dApprovedChargeMoney);

                        if (dApprovedChargeMoney < 0)
                        {
                            dApprovedChargeMoney = 0 - dApprovedChargeMoney;
                        }

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

                        if (dApprovingChargeMoney < 0)
                        {
                            dApprovingChargeMoney = 0 - dApprovingChargeMoney;
                        }
                    }

                    if (dApprovedChargeMoney != 0)
                    {
                        entRes.ApprovedApplyMoney += dApprovedChargeMoney;
                    }

                    if (dApprovingChargeMoney != 0)
                    {
                        entRes.ApprovingApplyMoney += dApprovingChargeMoney;
                    }

                    entRes.Subtotal = entRes.ApprovedApplyMoney + entRes.ApprovingApplyMoney;
                    entRes.BudgetMonth = entRes.BudgetMoneyMonth - entRes.ApprovedApplyMoney;
                    

                    if (entRes.BudgetMoneyYear != 0)
                    {
                        entRes.BudgetSeizureRate = decimal.Round(entRes.BudgetMoneyMonth / entRes.BudgetMoneyYear, 2);
                    }

                    if (!bIsExists)
                    {
                        entRes.OrganizationID = strDepartmentID;
                        entRes.OrganizationName = strDepartmentID;
                        if (drPers["OWNERCOMPANYNAME"] != DBNull.Value && drPers["OWNERDEPARTMENTNAME"] != DBNull.Value)
                        {
                            entRes.OrganizationName = drPers["OWNERCOMPANYNAME"].ToString() + "-" + drPers["OWNERDEPARTMENTNAME"].ToString();
                        }

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

            return entResList.OrderBy(t => t.OrganizationID).ThenBy(t => t.SubjectID).ToList();
        }

        /// <summary>
        /// 执行一览按条件查询(按岗位/员工查询)
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

                //if (string.IsNullOrWhiteSpace(strOrgType) || string.IsNullOrWhiteSpace(strOrgID))
                //{
                //    return entResList;
                //}

                //if (strOrgType.ToUpper() != "POST" && strOrgType.ToUpper() != "PERSONNAL")
                //{
                //    return entResList;
                //}

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

                string strChargeSql = "select * from zz_charge where ORDERTYPENAME = '报销单' and UPDATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                    + "','yyyy-MM-dd') and UPDATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    strFilter = strFilter.Replace("==", "=");
                    for (int i = objArgs.Count() - 1; i >= 0; i--)
                    {
                        strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                    }
                    strFilter += " order by OWNERDEPARTMENTID,  OWNERDEPARTMENTID, OWNERPOSTID, OWNERID, SUBJECTID";
                    strChargeSql += " and " + strFilter;
                }

                DataTable dtPers = new DataTable();

                dtPers = this.GetDataTableByCustomerSql(strChargeSql);

                foreach (DataRow drPers in dtPers.Rows)
                {
                    V_PerExecutionList entRes = new V_PerExecutionList();
                    bool bIsExists = false;
                    string strCompanyID = string.Empty, strDepartmentID = string.Empty, strOwnerID = string.Empty, strSubjectID = string.Empty;
                    string strOrderTypeName = string.Empty, strCheckstatesName = string.Empty;
                    decimal dApprovedChargeMoney = 0, dApprovingChargeMoney = 0;

                    DateTime dtOrderUpdateDate = new DateTime();

                    if (drPers["OWNERCOMPANYID"] == null || drPers["OWNERDEPARTMENTID"] == null || drPers["OWNERID"] == null)
                    {
                        continue;
                    }

                    if (drPers["SUBJECTID"] == null || drPers["BUDGETMONEY"] == null)
                    {
                        continue;
                    }

                    strCompanyID = drPers["OWNERCOMPANYID"].ToString();
                    strDepartmentID = drPers["OWNERDEPARTMENTID"].ToString();
                    strOwnerID = drPers["OWNERID"].ToString();
                    strSubjectID = drPers["SUBJECTID"].ToString();

                    if (entResList.Count() > 0)
                    {
                        var t = from n in entResList
                                where n.OwnerID == strOwnerID && n.SubjectID == strSubjectID
                                select n;

                        if (t.Count() > 0)
                        {
                            entRes = t.FirstOrDefault();
                            bIsExists = true;
                        }
                    }

                    strOrderTypeName = drPers["ORDERTYPENAME"].ToString();
                    strCheckstatesName = drPers["CHECKSTATESNAME"].ToString();

                    DateTime.TryParse(drPers["UPDATEDATE"].ToString(), out dtOrderUpdateDate);

                    if (strOrderTypeName == "报销单" && strCheckstatesName == "审核通过")
                    {
                        decimal.TryParse(drPers["BUDGETMONEY"].ToString(), out dApprovedChargeMoney);

                        if (dApprovedChargeMoney < 0)
                        {
                            dApprovedChargeMoney = 0 - dApprovedChargeMoney;
                        }

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

                        if (dApprovingChargeMoney < 0)
                        {
                            dApprovingChargeMoney = 0 - dApprovingChargeMoney;
                        }
                    }

                    if (dApprovedChargeMoney != 0)
                    {
                        entRes.ApprovedApplyMoney += dApprovedChargeMoney;
                    }

                    if (dApprovingChargeMoney != 0)
                    {
                        entRes.ApprovingApplyMoney += dApprovingChargeMoney;
                    }

                    entRes.Subtotal = entRes.ApprovedApplyMoney + entRes.ApprovingApplyMoney;

                    if (!bIsExists)
                    {
                        entRes.OrganizationID = strDepartmentID;
                        entRes.OrganizationName = strDepartmentID;
                        if (drPers["OWNERCOMPANYNAME"] != null && drPers["OWNERDEPARTMENTNAME"] != null)
                        {
                            entRes.OrganizationName = drPers["OWNERCOMPANYNAME"].ToString() + "-" + drPers["OWNERDEPARTMENTNAME"].ToString();
                        }

                        entRes.OwnerID = strOwnerID;

                        entRes.SubjectID = strSubjectID;

                        if (drPers["SUBJECTNAME"] != null)
                        {
                            entRes.SubjectName = drPers["SUBJECTNAME"].ToString();
                        }

                        if (drPers["OWNERNAME"] != null)
                        {
                            entRes.OwnerName = drPers["OWNERNAME"].ToString();
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

        /// <summary>
        /// 获取执行一览对应条目的台账列表
        /// </summary>
        /// <param name="iBudgetRecordType"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public List<V_BudgetRecord> GetBudgetRecordListByMultSearch(int iBudgetRecordType, int iYear, int iMonthStart, int iMonthEnd,
            string strOrderBy, string strFilter, params object[] objArgs)
        {
            List<V_BudgetRecord> entResList = new List<V_BudgetRecord>();
            try
            {
                decimal dCheckStates = 2;
                switch (iBudgetRecordType)
                {
                    case 1:
                        entResList = GetBudgetRecordListInBudgetYear(iYear, iMonthStart, iMonthEnd, dCheckStates, strOrderBy, strFilter, objArgs);
                        break;
                    case 2:
                        entResList = GetBudgetRecordListInBudgetMonth(iYear, iMonthStart, iMonthEnd, dCheckStates, strOrderBy, strFilter, objArgs);
                        break;
                    case 3:
                        entResList = GetBudgetRecordListInApply(iYear, iMonthStart, iMonthEnd, dCheckStates, strOrderBy, strFilter, objArgs);
                        break;
                    case 4:
                        dCheckStates = 1;
                        entResList = GetBudgetRecordListInApply(iYear, iMonthStart, iMonthEnd, dCheckStates, strOrderBy, strFilter, objArgs);
                        break;
                }

            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
            }
            return entResList;
        }

        #region 年度预算清单
        /// <summary>
        /// 年度预算清单
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        private List<V_BudgetRecord> GetBudgetRecordListInBudgetYear(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates, string strOrderBy,
            string strFilter, params object[] objArgs)
        {
            List<V_BudgetRecord> entResList = new List<V_BudgetRecord>();
            GetBudgetRecordInBudgetApplyYear(iYear, iMonthStart, iMonthEnd, dCheckStates, strFilter, objArgs, ref entResList);
            GetBudgetRecordInBudgetTransferYear(iYear, iMonthStart, iMonthEnd, dCheckStates, strFilter, objArgs, ref entResList);
            GetBudgetRecordInBudgetModYear(iYear, iMonthStart, iMonthEnd, dCheckStates, strFilter, objArgs, ref entResList);
            return entResList;
        }

        /// <summary>
        /// 年度预算申请
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="entResList"></param>
        private void GetBudgetRecordInBudgetApplyYear(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates, string strFilter, object[] objArgs,
            ref List<V_BudgetRecord> entResList)
        {
            if (iYear <= 0)
            {
                return;
            }

            if (iMonthStart <= 0 || iMonthEnd <= 0)
            {
                return;
            }

            if (iMonthStart > iMonthEnd)
            {
                return;
            }

            if (iMonthEnd > 12)
            {
                iMonthEnd = 12;
            }

            int iMaxDays = DateTime.DaysInMonth(iYear, iMonthEnd);
            DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonthEnd.ToString() + "-" + iMaxDays.ToString()).AddDays(1).AddSeconds(-1);

            var cba = from a in GetObjects<T_FB_COMPANYBUDGETAPPLYDETAIL>().Include("T_FB_COMPANYBUDGETAPPLYMASTER").Include("T_FB_SUBJECT")
                      where a.T_FB_COMPANYBUDGETAPPLYMASTER.CHECKSTATES == dCheckStates && a.T_FB_COMPANYBUDGETAPPLYMASTER.BUDGETYEAR == iYear && a.T_FB_COMPANYBUDGETAPPLYMASTER.UPDATEDATE <= dtEnd
                      select a;

            if (iMonthStart > 1)
            {
                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                cba = cba.Where(t => t.T_FB_COMPANYBUDGETAPPLYMASTER.UPDATEDATE == dtStart);
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                if (strFilter.Contains("OWNERDEPARTMENTID"))
                {
                    strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_COMPANYBUDGETAPPLYMASTER.OWNERDEPARTMENTID");
                }
                cba = cba.Where(strFilter, objArgs);
            }

            foreach (T_FB_COMPANYBUDGETAPPLYDETAIL item in cba)
            {
                if (item.BUDGETMONEY == 0)
                {
                    continue;
                }

                string strTableName = string.Empty;
                V_BudgetRecord entRes = new V_BudgetRecord();
                entRes.RecordID = item.T_FB_COMPANYBUDGETAPPLYMASTER.COMPANYBUDGETAPPLYMASTERID;
                entRes.RecordCode = item.T_FB_COMPANYBUDGETAPPLYMASTER.COMPANYBUDGETAPPLYMASTERCODE;
                entRes.XmlObjectValue = GetXmlObject(item.T_FB_COMPANYBUDGETAPPLYMASTER, entRes.RecordID, ref strTableName);

                entRes.TableName = strTableName;
                entRes.CheckState = item.T_FB_COMPANYBUDGETAPPLYMASTER.CHECKSTATES;

                entRes.SubjectID = item.T_FB_SUBJECT.SUBJECTID;
                entRes.SubjectName = item.T_FB_SUBJECT.SUBJECTNAME;
                entRes.TotalMoney = item.BUDGETMONEY;

                entRes.CreateUserID = item.T_FB_COMPANYBUDGETAPPLYMASTER.CREATEUSERID;
                entRes.CreateUserName = item.T_FB_COMPANYBUDGETAPPLYMASTER.CREATEUSERNAME;

                entRes.OwnerID = item.T_FB_COMPANYBUDGETAPPLYMASTER.OWNERID;
                entRes.OwnerName = item.T_FB_COMPANYBUDGETAPPLYMASTER.OWNERNAME;

                entRes.DepartmentID = item.T_FB_COMPANYBUDGETAPPLYMASTER.OWNERDEPARTMENTID;
                entRes.DepartmentName = item.T_FB_COMPANYBUDGETAPPLYMASTER.OWNERDEPARTMENTNAME;

                entRes.CompanyID = item.T_FB_COMPANYBUDGETAPPLYMASTER.OWNERCOMPANYID;
                entRes.CompanyName = item.T_FB_COMPANYBUDGETAPPLYMASTER.OWNERCOMPANYNAME;


                entRes.CreateDate = item.T_FB_COMPANYBUDGETAPPLYMASTER.CREATEDATE;
                entRes.UpdateDate = item.T_FB_COMPANYBUDGETAPPLYMASTER.UPDATEDATE;

                entResList.Add(entRes);
            }
        }

        /// <summary>
        /// 年度调拨申请
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="entResList"></param>
        private void GetBudgetRecordInBudgetTransferYear(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates, string strFilter, object[] objArgs,
            ref List<V_BudgetRecord> entResList)
        {
            if (iYear <= 0)
            {
                return;
            }

            if (iMonthStart <= 0 || iMonthEnd <= 0)
            {
                return;
            }

            if (iMonthStart > iMonthEnd)
            {
                return;
            }

            if (iMonthEnd > 12)
            {
                iMonthEnd = 12;
            }

            int iMaxDays = DateTime.DaysInMonth(iYear, iMonthEnd);
            DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonthEnd.ToString() + "-" + iMaxDays.ToString()).AddDays(1).AddSeconds(-1);
            var cba = from a in GetObjects<T_FB_COMPANYTRANSFERDETAIL>().Include("T_FB_COMPANYTRANSFERMASTER").Include("T_FB_SUBJECT")
                      where a.T_FB_COMPANYTRANSFERMASTER.CHECKSTATES == dCheckStates && a.T_FB_COMPANYTRANSFERMASTER.BUDGETYEAR == iYear && a.T_FB_COMPANYTRANSFERMASTER.UPDATEDATE <= dtEnd
                      select a;

            if (iMonthStart > 1)
            {
                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                cba = cba.Where(t => t.T_FB_COMPANYTRANSFERMASTER.UPDATEDATE >= dtStart);
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                if (strFilter.Contains("OWNERDEPARTMENTID"))
                {
                    strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_COMPANYTRANSFERMASTER.OWNERDEPARTMENTID");
                }
                cba = cba.Where(strFilter, objArgs);
            }

            foreach (T_FB_COMPANYTRANSFERDETAIL item in cba)
            {
                if (item.TRANSFERMONEY == 0)
                {
                    continue;
                }

                string strTableName = string.Empty;
                V_BudgetRecord entRes = new V_BudgetRecord();
                entRes.RecordID = item.T_FB_COMPANYTRANSFERMASTER.COMPANYTRANSFERMASTERID;
                entRes.RecordCode = item.T_FB_COMPANYTRANSFERMASTER.COMPANYTRANSFERMASTERCODE;
                entRes.XmlObjectValue = GetXmlObject(item.T_FB_COMPANYTRANSFERMASTER, entRes.RecordID, ref strTableName);

                entRes.TableName = strTableName;
                entRes.SubjectID = item.T_FB_SUBJECT.SUBJECTID;
                entRes.SubjectName = item.T_FB_SUBJECT.SUBJECTNAME;
                entRes.CheckState = item.T_FB_COMPANYTRANSFERMASTER.CHECKSTATES;

                entRes.TotalMoney = item.TRANSFERMONEY;
                entRes.CreateUserID = item.T_FB_COMPANYTRANSFERMASTER.CREATEUSERID;
                entRes.CreateUserName = item.T_FB_COMPANYTRANSFERMASTER.CREATEUSERNAME;

                entRes.OwnerID = item.T_FB_COMPANYTRANSFERMASTER.OWNERID;
                entRes.OwnerName = item.T_FB_COMPANYTRANSFERMASTER.OWNERNAME;

                entRes.DepartmentID = item.T_FB_COMPANYTRANSFERMASTER.OWNERDEPARTMENTID;
                entRes.DepartmentName = item.T_FB_COMPANYTRANSFERMASTER.OWNERDEPARTMENTNAME;

                entRes.CompanyID = item.T_FB_COMPANYTRANSFERMASTER.OWNERCOMPANYID;
                entRes.CompanyName = item.T_FB_COMPANYTRANSFERMASTER.OWNERCOMPANYNAME;


                entRes.CreateDate = item.T_FB_COMPANYTRANSFERMASTER.CREATEDATE;
                entRes.UpdateDate = item.T_FB_COMPANYTRANSFERMASTER.UPDATEDATE;

                entResList.Add(entRes);
            }
        }

        /// <summary>
        /// 年度预算增补
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="entResList"></param>
        private void GetBudgetRecordInBudgetModYear(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates, string strFilter, object[] objArgs,
            ref List<V_BudgetRecord> entResList)
        {
            if (iYear <= 0)
            {
                return;
            }

            if (iMonthStart <= 0 || iMonthEnd <= 0)
            {
                return;
            }

            if (iMonthStart > iMonthEnd)
            {
                return;
            }

            if (iMonthEnd > 12)
            {
                iMonthEnd = 12;
            }

            int iMaxDays = DateTime.DaysInMonth(iYear, iMonthEnd);
            DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonthEnd.ToString() + "-" + iMaxDays.ToString()).AddDays(1).AddSeconds(-1);

            var cba = from a in GetObjects<T_FB_COMPANYBUDGETMODDETAIL>().Include("T_FB_COMPANYBUDGETMODMASTER").Include("T_FB_SUBJECT")
                      where a.T_FB_COMPANYBUDGETMODMASTER.CHECKSTATES == dCheckStates && a.T_FB_COMPANYBUDGETMODMASTER.BUDGETYEAR == iYear && a.T_FB_COMPANYBUDGETMODMASTER.UPDATEDATE <= dtEnd
                      select a;

            if (iMonthStart > 1)
            {
                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                cba = cba.Where(t => t.T_FB_COMPANYBUDGETMODMASTER.UPDATEDATE == dtStart);
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                if (strFilter.Contains("OWNERDEPARTMENTID"))
                {
                    strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_COMPANYBUDGETMODMASTER.OWNERDEPARTMENTID");
                }
                cba = cba.Where(strFilter, objArgs);
            }

            foreach (T_FB_COMPANYBUDGETMODDETAIL item in cba)
            {
                if (item.BUDGETMONEY == 0)
                {
                    continue;
                }

                string strTableName = string.Empty;
                V_BudgetRecord entRes = new V_BudgetRecord();
                entRes.RecordID = item.T_FB_COMPANYBUDGETMODMASTER.COMPANYBUDGETMODMASTERID;
                entRes.RecordCode = item.T_FB_COMPANYBUDGETMODMASTER.COMPANYBUDGETMODMASTERCODE;
                entRes.XmlObjectValue = GetXmlObject(item.T_FB_COMPANYBUDGETMODMASTER, entRes.RecordID, ref strTableName);

                entRes.TableName = strTableName;
                entRes.SubjectID = item.T_FB_SUBJECT.SUBJECTID;
                entRes.SubjectName = item.T_FB_SUBJECT.SUBJECTNAME;
                entRes.CheckState = item.T_FB_COMPANYBUDGETMODMASTER.CHECKSTATES;

                entRes.TotalMoney = item.BUDGETMONEY;

                entRes.CreateUserID = item.T_FB_COMPANYBUDGETMODMASTER.CREATEUSERID;
                entRes.CreateUserName = item.T_FB_COMPANYBUDGETMODMASTER.CREATEUSERNAME;

                entRes.OwnerID = item.T_FB_COMPANYBUDGETMODMASTER.OWNERID;
                entRes.OwnerName = item.T_FB_COMPANYBUDGETMODMASTER.OWNERNAME;

                entRes.DepartmentID = item.T_FB_COMPANYBUDGETMODMASTER.OWNERDEPARTMENTID;
                entRes.DepartmentName = item.T_FB_COMPANYBUDGETMODMASTER.OWNERDEPARTMENTNAME;

                entRes.CompanyID = item.T_FB_COMPANYBUDGETMODMASTER.OWNERCOMPANYID;
                entRes.CompanyName = item.T_FB_COMPANYBUDGETMODMASTER.OWNERCOMPANYNAME;


                entRes.CreateDate = item.T_FB_COMPANYBUDGETMODMASTER.CREATEDATE;
                entRes.UpdateDate = item.T_FB_COMPANYBUDGETMODMASTER.UPDATEDATE;

                entResList.Add(entRes);
            }
        }

        #endregion 年度预算清单

        #region 月度预算清单

        /// <summary>
        /// 月度预算清单
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        private List<V_BudgetRecord> GetBudgetRecordListInBudgetMonth(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates, string strOrderBy,
            string strFilter, params object[] objArgs)
        {
            List<V_BudgetRecord> entResList = new List<V_BudgetRecord>();
            //期初结余
            GetBudgetRecordInCheckInitMonth(iYear, iMonthStart, iMonthEnd, dCheckStates, strFilter, objArgs, ref entResList);

            GetBudgetRecordInBudgetApplyMonth(iYear, iMonthStart, iMonthEnd, dCheckStates, strFilter, objArgs, ref entResList);
            //GetBudgetRecordInBudgetTransferMonth(iYear, iMonthStart, iMonthEnd, dCheckStates, strFilter, objArgs, ref entResList);
            GetBudgetRecordInBudgetModMonth(iYear, strFilter, iMonthStart, iMonthEnd, dCheckStates, objArgs, ref entResList);
            return entResList;
        }

        /// <summary>
        /// 月度预算申请
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="entResList"></param>
        private void GetBudgetRecordInBudgetApplyMonth(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates,
            string strFilter, object[] objArgs, ref List<V_BudgetRecord> entResList)
        {
            var cba = from a in GetObjects<T_FB_DEPTBUDGETAPPLYDETAIL>().Include("T_FB_DEPTBUDGETAPPLYMASTER").Include("T_FB_SUBJECT")
                      where a.T_FB_DEPTBUDGETAPPLYMASTER.CHECKSTATES == dCheckStates
                      select a;

            if (iYear > 0 && iMonthStart >= 0 && iMonthEnd >= 0 && iMonthStart <= iMonthEnd)
            {
                int iMaxDay = DateTime.DaysInMonth(iYear, iMonthEnd);
                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonthEnd.ToString() + "-" + iMaxDay.ToString());

                cba = cba.Where(t => t.T_FB_DEPTBUDGETAPPLYMASTER.BUDGETARYMONTH >= dtStart && t.T_FB_DEPTBUDGETAPPLYMASTER.BUDGETARYMONTH <= dtEnd);
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                if (strFilter.Contains("OWNERDEPARTMENTID"))
                {
                    strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_DEPTBUDGETAPPLYMASTER.OWNERDEPARTMENTID");
                }
                cba = cba.Where(strFilter, objArgs);
            }

            foreach (T_FB_DEPTBUDGETAPPLYDETAIL item in cba)
            {
                if (item.TOTALBUDGETMONEY == null)
                {
                    continue;
                }

                if (item.TOTALBUDGETMONEY.Value == 0)
                {
                    continue;
                }

                string strTableName = string.Empty;
                V_BudgetRecord entRes = new V_BudgetRecord();
                entRes.RecordID = item.T_FB_DEPTBUDGETAPPLYMASTER.DEPTBUDGETAPPLYMASTERID;
                entRes.RecordCode = item.T_FB_DEPTBUDGETAPPLYMASTER.DEPTBUDGETAPPLYMASTERCODE;
                entRes.XmlObjectValue = GetXmlObject(item.T_FB_DEPTBUDGETAPPLYMASTER, entRes.RecordID, ref strTableName);

                entRes.TableName = strTableName;
                entRes.SubjectID = item.T_FB_SUBJECT.SUBJECTID;
                entRes.SubjectName = item.T_FB_SUBJECT.SUBJECTNAME;
                entRes.CheckState = item.T_FB_DEPTBUDGETAPPLYMASTER.CHECKSTATES;

                entRes.TotalMoney = item.TOTALBUDGETMONEY.Value;

                entRes.CreateUserID = item.T_FB_DEPTBUDGETAPPLYMASTER.CREATEUSERID;
                entRes.CreateUserName = item.T_FB_DEPTBUDGETAPPLYMASTER.CREATEUSERNAME;

                entRes.OwnerID = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERID;
                entRes.OwnerName = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERNAME;

                entRes.DepartmentID = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERDEPARTMENTID;
                entRes.DepartmentName = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERDEPARTMENTNAME;

                entRes.CompanyID = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERCOMPANYID;
                entRes.CompanyName = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERCOMPANYNAME;


                entRes.CreateDate = item.T_FB_DEPTBUDGETAPPLYMASTER.CREATEDATE;
                entRes.UpdateDate = item.T_FB_DEPTBUDGETAPPLYMASTER.UPDATEDATE;

                entResList.Add(entRes);
            }
        }

        /// <summary>
        /// 月度预算申请
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="entResList"></param>
        private void GetBudgetRecordInCheckInitMonth(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates,
            string strFilter, object[] objArgs, ref List<V_BudgetRecord> entResList)
        {
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

            var tempSql = "SELECT T.*, SC.SUBJECTNAME FROM T_FB_BUDGETCHECK T,"
                       + " (select '08c1d9c6-2396-43c3-99f9-227e4a7eb417' as SUBJECTID, '部门经费' as SUBJECTNAME from dual"
                       + " union select 'd5134466-c207-44f2-8a36-cf7b96d5851f' as SUBJECTID, '活动经费' as SUBJECTNAME from dual) sc"
                       + " WHERE T.ACCOUNTOBJECTTYPE = 2 AND T.BUDGETMONTH = 12 AND t.SUBJECTID = sc.SUBJECTID"
                       + " and T.CREATEDATE >= to_date('" + dtStart.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')"
                       + " and T.CREATEDATE < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "','yyyy-MM-dd')";
            tempSql = string.Format("select * from ({0}) T_FB_SUBJECT where 1=1 ", tempSql);

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                strFilter = strFilter.Replace("==", "=");
                for (int i = objArgs.Count() - 1; i >= 0; i--)
                {
                    strFilter = strFilter.Replace("@" + i.ToString(), "'" + objArgs[i].ToString() + "'");
                }
                strFilter += " order by OWNERCOMPANYID,  OWNERDEPARTMENTID, OWNERPOSTID, OWNERID, SUBJECTID";
               
                tempSql += " and " + strFilter;
            }

            DataTable dtTemp = this.GetDataTableByCustomerSql(tempSql);

          
            foreach (DataRow dr in dtTemp.Rows)
            {
                string strTableName = string.Empty;
                V_BudgetRecord entRes = new V_BudgetRecord();

                entRes.RecordCode = "期初结余";
                entRes.TotalMoney = Convert.ToDecimal(dr["USABLEMONEY"]);
                entRes.CreateDate = Convert.ToDateTime(dr["CREATEDATE"]);
                entRes.UpdateDate = Convert.ToDateTime(dr["UPDATEDATE"]);
                entRes.SubjectID = dr["SUBJECTID"].ToString();
                entRes.SubjectName = dr["SUBJECTNAME"].ToString();
                //entRes.CheckState = item.T_FB_DEPTBUDGETAPPLYMASTER.CHECKSTATES;

                //entRes.RecordID = item.T_FB_DEPTBUDGETAPPLYMASTER.DEPTBUDGETAPPLYMASTERID;
                //entRes.RecordCode = item.T_FB_DEPTBUDGETAPPLYMASTER.DEPTBUDGETAPPLYMASTERCODE;
                //entRes.XmlObjectValue = GetXmlObject(item.T_FB_DEPTBUDGETAPPLYMASTER, entRes.RecordID, ref strTableName);

                //entRes.TableName = strTableName;
                //entRes.SubjectID = item.T_FB_SUBJECT.SUBJECTID;
                //entRes.SubjectName = item.T_FB_SUBJECT.SUBJECTNAME;
                //entRes.CheckState = item.T_FB_DEPTBUDGETAPPLYMASTER.CHECKSTATES;

                //entRes.TotalMoney = item.TOTALBUDGETMONEY.Value;

                //entRes.CreateUserID = item.T_FB_DEPTBUDGETAPPLYMASTER.CREATEUSERID;
                //entRes.CreateUserName = item.T_FB_DEPTBUDGETAPPLYMASTER.CREATEUSERNAME;

                //entRes.OwnerID = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERID;
                //entRes.OwnerName = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERNAME;

                //entRes.DepartmentID = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERDEPARTMENTID;
                //entRes.DepartmentName = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERDEPARTMENTNAME;

                //entRes.CompanyID = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERCOMPANYID;
                //entRes.CompanyName = item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERCOMPANYNAME;


                //entRes.CreateDate = item.T_FB_DEPTBUDGETAPPLYMASTER.CREATEDATE;
                //entRes.UpdateDate = item.T_FB_DEPTBUDGETAPPLYMASTER.UPDATEDATE;

                entResList.Add(entRes);
            }
        }

        /// <summary>
        /// 月度预算调拨
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="entResList"></param>
        private void GetBudgetRecordInBudgetTransferMonth(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates,
            string strFilter, object[] objArgs, ref List<V_BudgetRecord> entResList)
        {
            var cba = from a in GetObjects<T_FB_DEPTTRANSFERDETAIL>().Include("T_FB_DEPTTRANSFERMASTER").Include("T_FB_SUBJECT")
                      where a.T_FB_DEPTTRANSFERMASTER.CHECKSTATES == dCheckStates
                      select a;

            if (iYear > 0 && iMonthStart >= 0 && iMonthEnd >= 0 && iMonthStart <= iMonthEnd)
            {
                int iMaxDay = DateTime.DaysInMonth(iYear, iMonthEnd);
                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonthEnd.ToString() + "-" + iMaxDay.ToString());

                cba = cba.Where(t => t.T_FB_DEPTTRANSFERMASTER.BUDGETARYMONTH >= dtStart && t.T_FB_DEPTTRANSFERMASTER.BUDGETARYMONTH <= dtEnd);
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                if (strFilter.Contains("OWNERDEPARTMENTID"))
                {
                    strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_DEPTTRANSFERMASTER.OWNERDEPARTMENTID");
                }
                cba = cba.Where(strFilter, objArgs);
            }

            foreach (T_FB_DEPTTRANSFERDETAIL item in cba)
            {
                if (item.TRANSFERMONEY == 0)
                {
                    continue;
                }

                string strTableName = string.Empty;
                V_BudgetRecord entRes = new V_BudgetRecord();
                entRes.RecordID = item.T_FB_DEPTTRANSFERMASTER.DEPTTRANSFERMASTERID;
                entRes.RecordCode = item.T_FB_DEPTTRANSFERMASTER.DEPTTRANSFERMASTERCODE;
                entRes.XmlObjectValue = GetXmlObject(item.T_FB_DEPTTRANSFERMASTER, entRes.RecordID, ref strTableName);

                entRes.TableName = strTableName;
                entRes.SubjectID = item.T_FB_SUBJECT.SUBJECTID;
                entRes.SubjectName = item.T_FB_SUBJECT.SUBJECTNAME;
                entRes.CheckState = item.T_FB_DEPTTRANSFERMASTER.CHECKSTATES;

                entRes.TotalMoney = item.TRANSFERMONEY;

                entRes.CreateUserID = item.T_FB_DEPTTRANSFERMASTER.CREATEUSERID;
                entRes.CreateUserName = item.T_FB_DEPTTRANSFERMASTER.CREATEUSERNAME;

                entRes.OwnerID = item.T_FB_DEPTTRANSFERMASTER.OWNERID;
                entRes.OwnerName = item.T_FB_DEPTTRANSFERMASTER.OWNERNAME;

                entRes.DepartmentID = item.T_FB_DEPTTRANSFERMASTER.OWNERDEPARTMENTID;
                entRes.DepartmentName = item.T_FB_DEPTTRANSFERMASTER.OWNERDEPARTMENTNAME;

                entRes.CompanyID = item.T_FB_DEPTTRANSFERMASTER.OWNERCOMPANYID;
                entRes.CompanyName = item.T_FB_DEPTTRANSFERMASTER.OWNERCOMPANYNAME;


                entRes.CreateDate = item.T_FB_DEPTTRANSFERMASTER.CREATEDATE;
                entRes.UpdateDate = item.T_FB_DEPTTRANSFERMASTER.UPDATEDATE;

                entResList.Add(entRes);
            }
        }

        /// <summary>
        /// 月度预算增补
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="strFilter"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="objArgs"></param>
        /// <param name="entResList"></param>
        private void GetBudgetRecordInBudgetModMonth(int iYear, string strFilter, int iMonthStart, int iMonthEnd, decimal dCheckStates,
            object[] objArgs, ref List<V_BudgetRecord> entResList)
        {
            var cba = from a in GetObjects<T_FB_DEPTBUDGETADDDETAIL>().Include("T_FB_DEPTBUDGETADDMASTER").Include("T_FB_SUBJECT")
                      where a.T_FB_DEPTBUDGETADDMASTER.CHECKSTATES == dCheckStates
                      select a;

            if (iYear > 0 && iMonthStart >= 0 && iMonthEnd >= 0 && iMonthStart <= iMonthEnd)
            {
                int iMaxDay = DateTime.DaysInMonth(iYear, iMonthEnd);
                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonthEnd.ToString() + "-" + iMaxDay.ToString());

                cba = cba.Where(t => t.T_FB_DEPTBUDGETADDMASTER.BUDGETARYMONTH >= dtStart && t.T_FB_DEPTBUDGETADDMASTER.BUDGETARYMONTH <= dtEnd);
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                if (strFilter.Contains("OWNERDEPARTMENTID"))
                {
                    strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_DEPTBUDGETADDMASTER.OWNERDEPARTMENTID");
                }
                cba = cba.Where(strFilter, objArgs);
            }

            foreach (T_FB_DEPTBUDGETADDDETAIL item in cba)
            {
                if (item.TOTALBUDGETMONEY == null)
                {
                    continue;
                }

                if (item.TOTALBUDGETMONEY.Value == 0)
                {
                    continue;
                }

                string strTableName = string.Empty;
                V_BudgetRecord entRes = new V_BudgetRecord();
                entRes.RecordID = item.T_FB_DEPTBUDGETADDMASTER.DEPTBUDGETADDMASTERID;
                entRes.RecordCode = item.T_FB_DEPTBUDGETADDMASTER.DEPTBUDGETADDMASTERCODE;
                entRes.XmlObjectValue = GetXmlObject(item.T_FB_DEPTBUDGETADDMASTER, entRes.RecordID, ref strTableName);

                entRes.TableName = strTableName;
                entRes.SubjectID = item.T_FB_SUBJECT.SUBJECTID;
                entRes.SubjectName = item.T_FB_SUBJECT.SUBJECTNAME;
                entRes.CheckState = item.T_FB_DEPTBUDGETADDMASTER.CHECKSTATES;

                entRes.TotalMoney = item.TOTALBUDGETMONEY.Value;
                entRes.CreateUserID = item.T_FB_DEPTBUDGETADDMASTER.CREATEUSERID;
                entRes.CreateUserName = item.T_FB_DEPTBUDGETADDMASTER.CREATEUSERNAME;

                entRes.OwnerID = item.T_FB_DEPTBUDGETADDMASTER.OWNERID;
                entRes.OwnerName = item.T_FB_DEPTBUDGETADDMASTER.OWNERNAME;

                entRes.DepartmentID = item.T_FB_DEPTBUDGETADDMASTER.OWNERDEPARTMENTID;
                entRes.DepartmentName = item.T_FB_DEPTBUDGETADDMASTER.OWNERDEPARTMENTNAME;

                entRes.CompanyID = item.T_FB_DEPTBUDGETADDMASTER.OWNERCOMPANYID;
                entRes.CompanyName = item.T_FB_DEPTBUDGETADDMASTER.OWNERCOMPANYNAME;


                entRes.CreateDate = item.T_FB_DEPTBUDGETADDMASTER.CREATEDATE;
                entRes.UpdateDate = item.T_FB_DEPTBUDGETADDMASTER.UPDATEDATE;

                entResList.Add(entRes);
            }
        }

        #endregion 月度预算清单

        /// <summary>
        /// 报销及老差旅清单(执行一览台帐列表使用)
        /// </summary>
        /// <param name="iYear">查询年份</param>
        /// <param name="iMonthStart">查询起始月份</param>
        /// <param name="iMonthEnd">查询截止月份</param>
        /// <param name="strOrderBy">排序条件</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <returns></returns>
        private List<V_BudgetRecord> GetBudgetRecordListInApply(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates,
            string strOrderBy, string strFilter, params object[] objArgs)
        {
            List<V_BudgetRecord> entResList = new List<V_BudgetRecord>();
            GetBudgetRecordInChargeApply(iYear, iMonthStart, iMonthEnd, dCheckStates, strFilter, objArgs, ref entResList);
            GetBudgetRecordInTravelExpApply(iYear, iMonthStart, iMonthEnd, dCheckStates, strFilter, objArgs, ref entResList);
            return entResList;
        }

        /// <summary>
        /// 获取费用报销清单
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="dCheckStates"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="entResList"></param>
        private void GetBudgetRecordInChargeApply(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates, string strFilter,
            object[] objArgs, ref List<V_BudgetRecord> entResList)
        {
            var cba = from a in GetObjects<T_FB_CHARGEAPPLYDETAIL>().Include("T_FB_CHARGEAPPLYMASTER").Include("T_FB_SUBJECT")
                      where a.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == dCheckStates
                      select a;

            if (iYear > 0 && iMonthStart >= 0 && iMonthEnd >= 0 && iMonthStart <= iMonthEnd)
            {
                int iMaxDay = DateTime.DaysInMonth(iYear, iMonthEnd);
                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonthEnd.ToString() + "-" + iMaxDay.ToString()).AddDays(1).AddSeconds(-1);

                cba = cba.Where(t => t.T_FB_CHARGEAPPLYMASTER.UPDATEDATE >= dtStart && t.T_FB_CHARGEAPPLYMASTER.UPDATEDATE <= dtEnd);
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                if (strFilter.Contains("OWNERDEPARTMENTID"))
                {
                    strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID");
                }

                if (strFilter.Contains("OWNERID"))
                {
                    strFilter = strFilter.Replace("OWNERID", "T_FB_CHARGEAPPLYMASTER.OWNERID");
                }
                cba = cba.Where(strFilter, objArgs);
            }

            foreach (T_FB_CHARGEAPPLYDETAIL item in cba)
            {
                string strTableName = string.Empty;
                V_BudgetRecord entRes = new V_BudgetRecord();
                entRes.RecordID = item.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID;
                entRes.RecordCode = item.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERCODE;
                entRes.XmlObjectValue = GetXmlObject(item.T_FB_CHARGEAPPLYMASTER, entRes.RecordID, ref strTableName);

                entRes.TableName = strTableName;
                entRes.SubjectID = item.T_FB_SUBJECT.SUBJECTID;
                entRes.SubjectName = item.T_FB_SUBJECT.SUBJECTNAME;
                entRes.CheckState = item.T_FB_CHARGEAPPLYMASTER.CHECKSTATES;

                entRes.TotalMoney = item.CHARGEMONEY;

                entRes.CreateUserID = item.T_FB_CHARGEAPPLYMASTER.CREATEUSERID;
                entRes.CreateUserName = item.T_FB_CHARGEAPPLYMASTER.CREATEUSERNAME;

                entRes.OwnerID = item.T_FB_CHARGEAPPLYMASTER.OWNERID;
                entRes.OwnerName = item.T_FB_CHARGEAPPLYMASTER.OWNERNAME;

                entRes.DepartmentID = item.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID;
                entRes.DepartmentName = item.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTNAME;

                entRes.CompanyID = item.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID;
                entRes.CompanyName = item.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYNAME;
                entRes.Remark = item.REMARK;

                entRes.CreateDate = item.T_FB_CHARGEAPPLYMASTER.CREATEDATE;
                entRes.UpdateDate = item.T_FB_CHARGEAPPLYMASTER.UPDATEDATE;

                entResList.Add(entRes);
            }
        }

        /// <summary>
        /// 获取老差旅报销清单(此清单为早期设计，2011年3月以后基本未使用)
        /// </summary>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="dCheckStates"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="entResList"></param>
        private void GetBudgetRecordInTravelExpApply(int iYear, int iMonthStart, int iMonthEnd, decimal dCheckStates, string strFilter,
            object[] objArgs, ref List<V_BudgetRecord> entResList)
        {
            var cba = from a in GetObjects<T_FB_TRAVELEXPAPPLYDETAIL>().Include("T_FB_TRAVELEXPAPPLYMASTER").Include("T_FB_SUBJECT")
                      where a.T_FB_TRAVELEXPAPPLYMASTER.CHECKSTATES == dCheckStates
                      select a;

            if (iYear > 0 && iMonthStart >= 0 && iMonthEnd >= 0 && iMonthStart <= iMonthEnd)
            {
                int iMaxDay = DateTime.DaysInMonth(iYear, iMonthEnd);
                DateTime dtStart = DateTime.Parse(iYear.ToString() + "-" + iMonthStart.ToString() + "-1");
                DateTime dtEnd = DateTime.Parse(iYear.ToString() + "-" + iMonthEnd.ToString() + "-" + iMaxDay.ToString()).AddDays(1).AddSeconds(-1);

                cba = cba.Where(t => t.T_FB_TRAVELEXPAPPLYMASTER.UPDATEDATE >= dtStart && t.T_FB_TRAVELEXPAPPLYMASTER.UPDATEDATE <= dtEnd);
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                if (strFilter.Contains("OWNERDEPARTMENTID"))
                {
                    strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_TRAVELEXPAPPLYMASTER.OWNERDEPARTMENTID");
                }

                if (strFilter.Contains("OWNERID"))
                {
                    strFilter = strFilter.Replace("OWNERID", "T_FB_TRAVELEXPAPPLYMASTER.OWNERID");
                }
                cba = cba.Where(strFilter, objArgs);
            }

            foreach (T_FB_TRAVELEXPAPPLYDETAIL item in cba)
            {
                string strTableName = string.Empty;
                V_BudgetRecord entRes = new V_BudgetRecord();
                entRes.RecordID = item.T_FB_TRAVELEXPAPPLYMASTER.TRAVELEXPAPPLYMASTERID;
                entRes.RecordCode = item.T_FB_TRAVELEXPAPPLYMASTER.TRAVELEXPAPPLYMASTERCODE;
                entRes.XmlObjectValue = GetXmlObject(item.T_FB_TRAVELEXPAPPLYMASTER, entRes.RecordID, ref strTableName);

                entRes.TableName = strTableName;
                entRes.SubjectID = item.T_FB_SUBJECT.SUBJECTID;
                entRes.SubjectName = item.T_FB_SUBJECT.SUBJECTNAME;
                entRes.CheckState = item.T_FB_TRAVELEXPAPPLYMASTER.CHECKSTATES;

                entRes.TotalMoney = item.TOTALCHARGE;

                entRes.CreateUserID = item.T_FB_TRAVELEXPAPPLYMASTER.CREATEUSERID;
                entRes.CreateUserName = item.T_FB_TRAVELEXPAPPLYMASTER.CREATEUSERNAME;

                entRes.OwnerID = item.T_FB_TRAVELEXPAPPLYMASTER.OWNERID;
                entRes.OwnerName = item.T_FB_TRAVELEXPAPPLYMASTER.OWNERNAME;

                entRes.DepartmentID = item.T_FB_TRAVELEXPAPPLYMASTER.OWNERDEPARTMENTID;
                entRes.DepartmentName = item.T_FB_TRAVELEXPAPPLYMASTER.OWNERDEPARTMENTNAME;

                entRes.CompanyID = item.T_FB_TRAVELEXPAPPLYMASTER.OWNERCOMPANYID;
                entRes.CompanyName = item.T_FB_TRAVELEXPAPPLYMASTER.OWNERCOMPANYNAME;

                entRes.CreateDate = item.T_FB_TRAVELEXPAPPLYMASTER.CREATEDATE;
                entRes.UpdateDate = item.T_FB_TRAVELEXPAPPLYMASTER.UPDATEDATE;

                entResList.Add(entRes);
            }
        }

        #endregion 执行一览

        #region 借还款往来查询

        /// <summary>
        /// 个人借还款往来明细
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<V_ContactDetail> GetContactDetailListByMultSearch(string strOrgType, string strOrgID, DateTime dtStart,
            DateTime dtEnd, string strOrderBy, ref decimal dBeforeAccount, ref decimal dAfterAccount, string strFilter, params object[] objArgs)
        {
            List<V_ContactDetail> entResList = new List<V_ContactDetail>();
            try
            {
                //还款(旧)
                var rs = from v in GetObjects<T_FB_REPAYAPPLYMASTER>().Include("T_FB_BORROWAPPLYMASTER")
                         where v.CHECKSTATES == 2 && v.T_FB_BORROWAPPLYMASTER != null
                         select v;

                //还款(新)
                var rns = from v in GetObjects<T_FB_REPAYAPPLYMASTER>().Include("T_FB_BORROWAPPLYMASTER")
                          where v.CHECKSTATES == 2 && v.T_FB_BORROWAPPLYMASTER == null
                          select v;

                //费用报销--冲借款(旧)
                var cs = from v in GetObjects<T_FB_CHARGEAPPLYMASTER>().Include("T_FB_BORROWAPPLYMASTER")
                         where v.CHECKSTATES == 2 && v.PAYTYPE == 2 && v.T_FB_BORROWAPPLYMASTER != null
                         select v;

                //费用报销--冲借款(新)
                var crs = from v in GetObjects<T_FB_CHARGEAPPLYMASTER>().Include("T_FB_BORROWAPPLYMASTER")
                          where v.CHECKSTATES == 2 && v.PAYTYPE == 2 && v.T_FB_BORROWAPPLYMASTER == null
                          select v;

                //借款
                var ds = from v in GetObjects<T_FB_BORROWAPPLYMASTER>()
                         where v.CHECKSTATES == 2
                         select v;

                //组织架构过滤
                switch (strOrgType.ToUpper())
                {
                    case "POST":
                        ds = ds.Where(t => t.OWNERPOSTID == strOrgID);
                        rs = rs.Where(t => t.OWNERPOSTID == strOrgID);
                        rns = rns.Where(t => t.OWNERPOSTID == strOrgID);
                        cs = cs.Where(t => t.OWNERPOSTID == strOrgID);
                        crs = crs.Where(t => t.OWNERPOSTID == strOrgID);
                        break;
                    case "PERSONAL":
                        ds = ds.Where(t => t.OWNERID == strOrgID);
                        rs = rs.Where(t => t.OWNERID == strOrgID);
                        rns = rns.Where(t => t.OWNERID == strOrgID);
                        cs = cs.Where(t => t.OWNERID == strOrgID);
                        crs = crs.Where(t => t.OWNERID == strOrgID);
                        break;
                }

                DateTime dtCheck = new DateTime();

                if (dtEnd > dtCheck)
                {
                    ds = ds.Where(t => t.UPDATEDATE >= dtStart && t.UPDATEDATE <= dtEnd);
                    rs = rs.Where(t => t.UPDATEDATE >= dtStart && t.UPDATEDATE <= dtEnd);
                    rns = rns.Where(t => t.UPDATEDATE >= dtStart && t.UPDATEDATE <= dtEnd);
                    cs = cs.Where(t => t.UPDATEDATE >= dtStart && t.UPDATEDATE <= dtEnd);
                    crs = crs.Where(t => t.UPDATEDATE >= dtStart && t.UPDATEDATE <= dtEnd);
                }

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    ds = ds.Where(strFilter, objArgs);
                    rs = rs.Where(strFilter, objArgs);
                    rns = rns.Where(strFilter, objArgs);
                    cs = cs.Where(strFilter, objArgs);
                    crs = crs.Where(strFilter, objArgs);
                }

                ds = ds.OrderBy(strOrderBy);

                decimal? dBefore = 0;
                decimal? dAfter = 0;
                GetTotalBorrowMoney(ref dBefore, strOrgType, strOrgID, dtStart, strOrderBy, strFilter, objArgs);
                //注释掉，不用此方法来得到期末借款额
                //GetTotalBorrowMoney(ref dAfter, strOrgType, strOrgID, dtEnd, strOrderBy, strFilter, objArgs);


                if (dBefore != null)
                {
                    dBeforeAccount = dBefore.Value;
                }

                if (dAfter != null)
                {
                    //dAfterAccount = dAfter.Value;
                }

                //借款统计
                if (ds != null)
                {
                    foreach (T_FB_BORROWAPPLYMASTER item in ds)
                    {
                        if (!item.T_FB_BORROWAPPLYDETAIL.IsLoaded)
                        {
                            item.T_FB_BORROWAPPLYDETAIL.Load();
                        }

                        if (item.TOTALMONEY == 0 || item.T_FB_BORROWAPPLYDETAIL == null)
                        {
                            continue;
                        }

                        string strTableName = string.Empty;
                        V_ContactDetail entRes = new V_ContactDetail();
                        entRes.RecordID = item.BORROWAPPLYMASTERID;
                        entRes.RecordCode = item.BORROWAPPLYMASTERCODE;
                        entRes.RelRecordCode = string.Empty;

                        entRes.XmlObjectValue = GetXmlObject(item, entRes.RecordID, ref strTableName);

                        entRes.TableName = strTableName;
                        entRes.CheckState = item.CHECKSTATES;


                        entRes.BorrowType = item.REPAYTYPE.ToString();
                        foreach (T_FB_BORROWAPPLYDETAIL BdItem in item.T_FB_BORROWAPPLYDETAIL)
                        {
                            entRes.BorrowMoney += BdItem.BORROWMONEY;
                        }

                        entRes.RepayMoney = 0;

                        entRes.CreateUserID = item.OWNERID;

                        entRes.DepartmentID = item.OWNERDEPARTMENTID;
                        entRes.DepartmentName = item.OWNERDEPARTMENTNAME;

                        entRes.CompanyID = item.OWNERCOMPANYID;
                        entRes.CompanyName = item.OWNERCOMPANYNAME;

                        entRes.CreateUserName = item.OWNERCOMPANYNAME + "-" + item.OWNERDEPARTMENTNAME + "-" + item.OWNERPOSTNAME + "-" + item.OWNERNAME;

                        entRes.CreateDate = item.CREATEDATE;
                        entRes.UpdateDate = item.UPDATEDATE;

                        entResList.Add(entRes);
                    }
                }

                //UNREPAYMONEY为未还款，不用赋值 RepayMoney = TotalMoney,BorrowMoney=0,BorrowType不用赋值，赋值0
                //1.先遍历还款的，循环插入到视图,
                if (rs != null)
                {
                    foreach (T_FB_REPAYAPPLYMASTER RsItem in rs)
                    {
                        if (!RsItem.T_FB_REPAYAPPLYDETAIL.IsLoaded)
                        {
                            RsItem.T_FB_REPAYAPPLYDETAIL.Load();
                        }

                        if (RsItem.T_FB_REPAYAPPLYDETAIL == null)
                        {
                            continue;
                        }

                        V_ContactDetail entRs = new V_ContactDetail();
                        string strtableName = string.Empty;
                        entRs.RecordID = RsItem.REPAYAPPLYMASTERID;
                        entRs.RecordCode = RsItem.REPAYAPPLYCODE;
                        entRs.RelRecordCode = RsItem.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERCODE;

                        entRs.XmlObjectValue = GetXmlObject(RsItem, entRs.RecordID, ref strtableName);

                        entRs.TableName = strtableName;
                        entRs.CheckState = RsItem.CHECKSTATES;


                        entRs.RepayType = "RepayType," + RsItem.REPAYTYPE.ToString();
                        entRs.BorrowType = string.Empty;//RsItem.T_FB_REPAYAPPLYMASTER.REPAYTYPE;
                        entRs.BorrowMoney = 0;

                        foreach (T_FB_REPAYAPPLYDETAIL RdItem in RsItem.T_FB_REPAYAPPLYDETAIL)
                        {
                            if (RdItem.REPAYMONEY != null)
                            {
                                entRs.RepayMoney += RdItem.REPAYMONEY.Value;
                            }
                        }

                        entRs.RepayMoney = RsItem.TOTALMONEY;

                        entRs.CreateUserID = RsItem.OWNERID;
                        entRs.CreateUserName = RsItem.OWNERNAME;

                        entRs.DepartmentID = RsItem.OWNERDEPARTMENTID;
                        entRs.DepartmentName = RsItem.OWNERDEPARTMENTNAME;

                        entRs.CompanyID = RsItem.OWNERCOMPANYID;
                        entRs.CompanyName = RsItem.OWNERCOMPANYNAME;

                        entRs.CreateDate = RsItem.CREATEDATE;
                        entRs.UpdateDate = RsItem.UPDATEDATE;

                        entResList.Add(entRs);
                    }
                }

                if (rns != null)
                {
                    foreach (T_FB_REPAYAPPLYMASTER RsItem in rns)
                    {
                        if (!RsItem.T_FB_REPAYAPPLYDETAIL.IsLoaded)
                        {
                            RsItem.T_FB_REPAYAPPLYDETAIL.Load();
                        }

                        if (RsItem.T_FB_REPAYAPPLYDETAIL == null)
                        {
                            continue;
                        }

                        foreach (T_FB_REPAYAPPLYDETAIL RdItem in RsItem.T_FB_REPAYAPPLYDETAIL)
                        {
                            if (RdItem.REPAYMONEY == null)
                            {
                                continue;
                            }

                            if (RdItem.REPAYMONEY.Value == 0)
                            {
                                continue;
                            }

                            V_ContactDetail entRs = new V_ContactDetail();
                            bool bIsExists = false;
                            if (entResList.Count() > 0)
                            {
                                string strpayType = RdItem.REPAYTYPE.ToString();
                                var t = from n in entResList
                                        where n.RecordID == RsItem.REPAYAPPLYMASTERID && n.RepayType == strpayType
                                        select n;

                                if (t.Count() > 0)
                                {
                                    entRs = t.FirstOrDefault();
                                    bIsExists = true;
                                }
                            }

                            entRs.RepayMoney += RdItem.REPAYMONEY.Value;

                            if (!bIsExists)
                            {
                                string strtableName = string.Empty;
                                entRs.RecordID = RsItem.REPAYAPPLYMASTERID;
                                entRs.RecordCode = RsItem.REPAYAPPLYCODE;
                                entRs.RelRecordCode = string.Empty;

                                entRs.XmlObjectValue = GetXmlObject(RsItem, entRs.RecordID, ref strtableName);

                                entRs.TableName = strtableName;
                                entRs.CheckState = RsItem.CHECKSTATES;


                                entRs.RepayType = "RepayType," + RdItem.REPAYTYPE.ToString();
                                entRs.BorrowType = string.Empty;//RsItem.T_FB_REPAYAPPLYMASTER.REPAYTYPE;
                                entRs.BorrowMoney = 0;

                                entRs.CreateUserID = RsItem.OWNERID;
                                entRs.CreateUserName = RsItem.OWNERNAME;

                                entRs.DepartmentID = RsItem.OWNERDEPARTMENTID;
                                entRs.DepartmentName = RsItem.OWNERDEPARTMENTNAME;

                                entRs.CompanyID = RsItem.OWNERCOMPANYID;
                                entRs.CompanyName = RsItem.OWNERCOMPANYNAME;

                                entRs.CreateDate = RsItem.CREATEDATE;
                                entRs.UpdateDate = RsItem.UPDATEDATE;

                                entResList.Add(entRs);
                            }
                        }
                    }
                }

                //2.接着遍历费用报销的，循环插入到视图
                if (cs != null)
                {
                    foreach (T_FB_CHARGEAPPLYMASTER CsItem in cs)
                    {
                        if (!CsItem.T_FB_CHARGEAPPLYDETAIL.IsLoaded)
                        {
                            CsItem.T_FB_CHARGEAPPLYDETAIL.Load();
                        }

                        if (CsItem.T_FB_CHARGEAPPLYDETAIL == null)
                        {
                            continue;
                        }

                        V_ContactDetail entCs = new V_ContactDetail();
                        string strtableName = string.Empty;
                        entCs.RecordID = CsItem.CHARGEAPPLYMASTERID;
                        entCs.RecordCode = CsItem.CHARGEAPPLYMASTERCODE;
                        entCs.RelRecordCode = CsItem.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERCODE;

                        entCs.XmlObjectValue = GetXmlObject(CsItem, entCs.RecordID, ref strtableName);

                        entCs.TableName = strtableName;
                        entCs.CheckState = CsItem.CHECKSTATES;


                        entCs.BorrowType = string.Empty;//CsItem.T_FB_CHARGEAPPLYMASTER.REPAYTYPE;
                        entCs.BorrowMoney = 0;
                        entCs.RepayType = "PayType," + CsItem.PAYTYPE.ToString();

                        foreach (T_FB_CHARGEAPPLYDETAIL CdItem in CsItem.T_FB_CHARGEAPPLYDETAIL)
                        {
                            if (CdItem.REPAYMONEY != null)
                            {
                                entCs.RepayMoney += CdItem.REPAYMONEY.Value;
                            }
                        }

                        entCs.CreateUserID = CsItem.OWNERID;
                        entCs.CreateUserName = CsItem.OWNERNAME;

                        entCs.DepartmentID = CsItem.OWNERDEPARTMENTID;
                        entCs.DepartmentName = CsItem.OWNERDEPARTMENTNAME;

                        entCs.CompanyID = CsItem.OWNERCOMPANYID;
                        entCs.CompanyName = CsItem.OWNERCOMPANYNAME;

                        entCs.CreateDate = CsItem.CREATEDATE;
                        entCs.UpdateDate = CsItem.UPDATEDATE;

                        entResList.Add(entCs);
                    }
                }

                //2.接着遍历费用报销的，循环插入到视图
                if (crs != null)
                {
                    foreach (T_FB_CHARGEAPPLYMASTER CsItem in crs)
                    {
                        if (!CsItem.T_FB_CHARGEAPPLYREPAYDETAIL.IsLoaded)
                        {
                            CsItem.T_FB_CHARGEAPPLYREPAYDETAIL.Load();
                        }

                        if (CsItem.T_FB_CHARGEAPPLYREPAYDETAIL == null)
                        {
                            continue;
                        }

                        foreach (T_FB_CHARGEAPPLYREPAYDETAIL CdItem in CsItem.T_FB_CHARGEAPPLYREPAYDETAIL)
                        {
                            V_ContactDetail entCs = new V_ContactDetail();
                            bool bIsExists = false;
                            if (entResList.Count() > 0)
                            {
                                string strpayType = CdItem.REPAYTYPE.ToString();
                                var t = from n in entResList
                                        where n.RecordID == CsItem.CHARGEAPPLYMASTERID && n.RepayType == strpayType
                                        select n;

                                if (t.Count() > 0)
                                {
                                    entCs = t.FirstOrDefault();
                                    bIsExists = true;
                                }
                            }

                            if (CdItem.REPAYMONEY <= 0)
                            {
                                continue;
                            }

                            entCs.RepayMoney += CdItem.REPAYMONEY;

                            if (!bIsExists)
                            {
                                string strtableName = string.Empty;
                                entCs.RecordID = CsItem.CHARGEAPPLYMASTERID;
                                entCs.RecordCode = CsItem.CHARGEAPPLYMASTERCODE;
                                entCs.RelRecordCode = string.Empty;

                                entCs.XmlObjectValue = GetXmlObject(CsItem, entCs.RecordID, ref strtableName);

                                entCs.TableName = strtableName;
                                entCs.CheckState = CsItem.CHECKSTATES;

                                entCs.BorrowType = string.Empty;//CsItem.T_FB_CHARGEAPPLYMASTER.REPAYTYPE;
                                entCs.BorrowMoney = 0;
                                entCs.RepayType = "PayType,2";

                                entCs.CreateUserID = CsItem.OWNERID;
                                entCs.CreateUserName = CsItem.OWNERNAME;

                                entCs.DepartmentID = CsItem.OWNERDEPARTMENTID;
                                entCs.DepartmentName = CsItem.OWNERDEPARTMENTNAME;

                                entCs.CompanyID = CsItem.OWNERCOMPANYID;
                                entCs.CompanyName = CsItem.OWNERCOMPANYNAME;

                                entCs.CreateDate = CsItem.CREATEDATE;
                                entCs.UpdateDate = CsItem.UPDATEDATE;

                                entResList.Add(entCs);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
            }


            List<V_ContactDetail> ResList = new List<V_ContactDetail>();
            ResList= entResList.OrderBy(t => t.UpdateDate).ToList();
            //借款金额
            decimal borrowMoney = ResList.Sum(t => t.BorrowMoney);
            //还款金额
            decimal repayMoney = ResList.Sum(t => t.RepayMoney);
            dAfterAccount = borrowMoney - repayMoney;

            return ResList;
        }

        /// <summary>
        /// 部门借还款往来明细
        /// </summary>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="iYear"></param>
        /// <param name="iMonthStart"></param>
        /// <param name="iMonthEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public List<V_DeptContactDetail> GetDeptContactDetailListByMultSearch(string strOrgType, string strOrgID, DateTime dtStart,
            DateTime dtEnd, string strOrderBy, ref decimal dTotalNormalBorrowMoney, ref decimal dTotalSpecialMoney, ref decimal dTotalReserveMoney, string strFilter, params object[] objArgs)
        {
            List<V_DeptContactDetail> entResList = new List<V_DeptContactDetail>();
            try
            {
                //借款
                var ds = from v in GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                         where v.T_FB_BORROWAPPLYMASTER.CHECKSTATES == 2
                         select v;

                //还款(旧)
                var rs = from v in GetObjects<T_FB_REPAYAPPLYMASTER>().Include("T_FB_BORROWAPPLYMASTER")
                         where v.CHECKSTATES == 2 && v.T_FB_BORROWAPPLYMASTER != null
                         select v;

                //还款(新)
                var rns = from v in GetObjects<T_FB_REPAYAPPLYMASTER>().Include("T_FB_BORROWAPPLYMASTER")
                          where v.CHECKSTATES == 2 && v.T_FB_BORROWAPPLYMASTER == null
                          select v;

                //费用报销--冲借款(旧)
                var cs = from v in GetObjects<T_FB_CHARGEAPPLYMASTER>().Include("T_FB_BORROWAPPLYMASTER")
                         where v.CHECKSTATES == 2 && v.PAYTYPE == 2 && v.T_FB_BORROWAPPLYMASTER != null
                         select v;

                //费用报销--冲借款(新)
                var crs = from v in GetObjects<T_FB_CHARGEAPPLYMASTER>().Include("T_FB_BORROWAPPLYMASTER")
                          where v.CHECKSTATES == 2 && v.PAYTYPE == 2 && v.T_FB_BORROWAPPLYMASTER == null
                          select v;

                //组织架构过滤
                switch (strOrgType.ToUpper())
                {
                    case "COMPANY":
                        ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                        rs = rs.Where(t => t.OWNERCOMPANYID == strOrgID);
                        rns = rns.Where(t => t.OWNERCOMPANYID == strOrgID);
                        cs = cs.Where(t => t.OWNERCOMPANYID == strOrgID);
                        crs = crs.Where(t => t.OWNERCOMPANYID == strOrgID);
                        break;
                    case "DEPARTMENT":
                        ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID == strOrgID);
                        rs = rs.Where(t => t.OWNERDEPARTMENTID == strOrgID);
                        rns = rns.Where(t => t.OWNERDEPARTMENTID == strOrgID);
                        cs = cs.Where(t => t.OWNERDEPARTMENTID == strOrgID);
                        crs = crs.Where(t => t.OWNERDEPARTMENTID == strOrgID);
                        break;
                }

                DateTime dtCheck = new DateTime();

                if (dtEnd > dtCheck)
                {
                    ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.UPDATEDATE >= dtStart && t.T_FB_BORROWAPPLYMASTER.UPDATEDATE <= dtEnd);
                    rs = rs.Where(t => t.UPDATEDATE >= dtStart && t.UPDATEDATE <= dtEnd);
                    rns = rns.Where(t => t.UPDATEDATE >= dtStart && t.UPDATEDATE <= dtEnd);
                    cs = cs.Where(t => t.UPDATEDATE >= dtStart && t.UPDATEDATE <= dtEnd);
                    crs = crs.Where(t => t.UPDATEDATE >= dtStart && t.UPDATEDATE <= dtEnd);
                }

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    string strRsFilter = string.Copy(strFilter);
                    if (strFilter.Contains("OWNERCOMPANYID"))
                    {
                        strFilter = strFilter.Replace("OWNERCOMPANYID", "T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID");
                    }

                    if (strFilter.Contains("OWNERDEPARTMENTID"))
                    {
                        strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID");
                    }

                    if (strFilter.Contains("OWNERPOSTID"))
                    {
                        strFilter = strFilter.Replace("OWNERPOSTID", "T_FB_BORROWAPPLYMASTER.OWNERPOSTID");
                    }

                    if (strFilter.Contains("OWNERID"))
                    {
                        strFilter = strFilter.Replace("OWNERID", "T_FB_BORROWAPPLYMASTER.OWNERID");
                    }

                    if (strFilter.Contains("CREATEUSERID"))
                    {
                        strFilter = strFilter.Replace("CREATEUSERID", "T_FB_BORROWAPPLYMASTER.CREATEUSERID");
                    }

                    ds = ds.Where(strFilter, objArgs);
                    rs = rs.Where(strRsFilter, objArgs);
                    rns = rns.Where(strRsFilter, objArgs);
                    cs = cs.Where(strRsFilter, objArgs);
                    crs = crs.Where(strRsFilter, objArgs);
                }

                ds = ds.OrderBy(strOrderBy);

                decimal? dTotalNormal = 0;
                decimal? dTotalSpecial = 0;
                decimal? dTotalReserve = 0;
                GetTotalBorrowMoneyByBorrowType(ref dTotalNormal, ref dTotalSpecial, ref dTotalReserve, strOrgType, strOrgID, dtStart,
                dtEnd, strOrderBy, strFilter, objArgs);


                if (dTotalNormal != null)
                {
                    dTotalNormalBorrowMoney = dTotalNormal.Value;
                }

                if (dTotalSpecial != null)
                {
                    dTotalSpecialMoney = dTotalSpecial.Value;
                }

                if (dTotalReserve != null)
                {
                    dTotalReserveMoney = dTotalReserve.Value;
                }

                if (ds == null)
                {
                    return entResList;
                }

                if (ds.Count() == 0)
                {
                    return entResList;
                }

                bool bIsExists = false;
                foreach (T_FB_BORROWAPPLYDETAIL item in ds)
                {
                    if (item.BORROWMONEY == 0)
                    {
                        continue;
                    }

                    V_DeptContactDetail entRes = new V_DeptContactDetail();

                    var ec = from b in entResList
                             where b.OwnerID == item.T_FB_BORROWAPPLYMASTER.OWNERID
                             select b;

                    if (ec.Count() > 0)
                    {
                        entRes = ec.FirstOrDefault();
                        bIsExists = true;
                    }

                    string strBorrowType = item.T_FB_BORROWAPPLYMASTER.REPAYTYPE.ToString();
                    if (strBorrowType == "1")//普通借款
                    {
                        entRes.NormalMoney += item.BORROWMONEY;
                    }
                    else if (strBorrowType == "2")//备用金借款
                    {
                        entRes.ReserveMoney += item.BORROWMONEY;
                    }
                    else if (strBorrowType == "3")//专项借款
                    {
                        entRes.SpecialMoney += item.BORROWMONEY;
                    }


                    entRes.TotalMoney = entRes.NormalMoney + entRes.SpecialMoney + entRes.ReserveMoney;

                    if (bIsExists)
                    {
                        bIsExists = false;
                        continue;
                    }

                    entRes.OwnerID = item.T_FB_BORROWAPPLYMASTER.OWNERID;
                    entRes.OwnerName = item.T_FB_BORROWAPPLYMASTER.OWNERNAME;

                    entRes.OwnerPostID = item.T_FB_BORROWAPPLYMASTER.OWNERPOSTID;
                    entRes.OwnerPostName = item.T_FB_BORROWAPPLYMASTER.OWNERPOSTNAME;

                    entRes.DepartmentID = item.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID;
                    entRes.DepartmentName = item.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTNAME;

                    entRes.CompanyID = item.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID;
                    entRes.CompanyName = item.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYNAME;


                    entResList.Add(entRes);
                }

                decimal dNormalRepay = 0, dReserveRepay = 0, dSpecialRepay = 0;
                if (rs != null && entResList.Count() > 0)
                {
                    foreach (T_FB_REPAYAPPLYMASTER RsItem in rs)
                    {
                        if (!RsItem.T_FB_REPAYAPPLYDETAIL.IsLoaded)
                        {
                            RsItem.T_FB_REPAYAPPLYDETAIL.Load();
                        }

                        if (RsItem.T_FB_REPAYAPPLYDETAIL == null)
                        {
                            continue;
                        }

                        if (RsItem.REPAYTYPE == null)
                        {
                            continue;
                        }

                        V_DeptContactDetail entRes = new V_DeptContactDetail();
                        int iRepayType = 0;

                        var ec = from b in entResList
                                 where b.OwnerID == RsItem.OWNERID
                                 select b;

                        if (ec == null)
                        {
                            continue;
                        }

                        entRes = ec.FirstOrDefault();

                        if (entRes == null)
                        {
                            continue;
                        }

                        int.TryParse(RsItem.REPAYTYPE.Value.ToString(), out iRepayType);

                        foreach (T_FB_REPAYAPPLYDETAIL RdItem in RsItem.T_FB_REPAYAPPLYDETAIL)
                        {
                            if (RdItem.REPAYMONEY == null)
                            {
                                continue;
                            }

                            switch (iRepayType)
                            {
                                case 1:
                                    entRes.NormalMoney -= RdItem.REPAYMONEY.Value;
                                    dNormalRepay += RdItem.REPAYMONEY.Value;
                                    break;
                                case 2:
                                    entRes.ReserveMoney -= RdItem.REPAYMONEY.Value;
                                    dReserveRepay += RdItem.REPAYMONEY.Value;
                                    break;
                                case 3:
                                    entRes.SpecialMoney -= RdItem.REPAYMONEY.Value;
                                    dSpecialRepay += RdItem.REPAYMONEY.Value;
                                    break;
                            }
                        }

                        entRes.TotalMoney = entRes.NormalMoney + entRes.SpecialMoney + entRes.ReserveMoney;
                    }
                }

                if (rns != null && entResList.Count() > 0)
                {
                    foreach (T_FB_REPAYAPPLYMASTER RsItem in rns)
                    {
                        if (!RsItem.T_FB_REPAYAPPLYDETAIL.IsLoaded)
                        {
                            RsItem.T_FB_REPAYAPPLYDETAIL.Load();
                        }

                        if (RsItem.T_FB_REPAYAPPLYDETAIL == null)
                        {
                            continue;
                        }

                        V_DeptContactDetail entRes = new V_DeptContactDetail();

                        var ec = from b in entResList
                                 where b.OwnerID == RsItem.OWNERID
                                 select b;

                        if (ec == null)
                        {
                            continue;
                        }

                        entRes = ec.FirstOrDefault();

                        if (entRes == null)
                        {
                            continue;
                        }

                        foreach (T_FB_REPAYAPPLYDETAIL RdItem in RsItem.T_FB_REPAYAPPLYDETAIL)
                        {
                            if (RdItem.REPAYMONEY == null || RdItem.REPAYTYPE == null)
                            {
                                continue;
                            }

                            int iRepayType = 0;
                            int.TryParse(RdItem.REPAYTYPE.Value.ToString(), out iRepayType);
                            switch (iRepayType)
                            {
                                case 1:
                                    entRes.NormalMoney -= RdItem.REPAYMONEY.Value;
                                    dNormalRepay += RdItem.REPAYMONEY.Value;
                                    break;
                                case 2:
                                    entRes.ReserveMoney -= RdItem.REPAYMONEY.Value;
                                    dReserveRepay += RdItem.REPAYMONEY.Value;
                                    break;
                                case 3:
                                    entRes.SpecialMoney -= RdItem.REPAYMONEY.Value;
                                    dSpecialRepay += RdItem.REPAYMONEY.Value;
                                    break;
                            }
                        }

                        entRes.TotalMoney = entRes.NormalMoney + entRes.SpecialMoney + entRes.ReserveMoney;

                    }
                }

                if (cs != null && entResList.Count() > 0)
                {
                    foreach (T_FB_CHARGEAPPLYMASTER CsItem in cs)
                    {
                        if (!CsItem.T_FB_CHARGEAPPLYDETAIL.IsLoaded)
                        {
                            CsItem.T_FB_CHARGEAPPLYDETAIL.Load();
                        }

                        if (CsItem.T_FB_CHARGEAPPLYDETAIL == null)
                        {
                            continue;
                        }

                        V_DeptContactDetail entRes = new V_DeptContactDetail();
                        int iRepayType = 0;

                        var ec = from b in entResList
                                 where b.OwnerID == CsItem.OWNERID
                                 select b;

                        if (ec == null)
                        {
                            continue;
                        }

                        entRes = ec.FirstOrDefault();

                        if (entRes == null)
                        {
                            continue;
                        }

                        int.TryParse(CsItem.T_FB_BORROWAPPLYMASTER.REPAYTYPE.ToString(), out iRepayType);

                        foreach (T_FB_CHARGEAPPLYDETAIL CdItem in CsItem.T_FB_CHARGEAPPLYDETAIL)
                        {
                            if (CdItem.REPAYMONEY == null)
                            {
                                continue;
                            }

                            switch (iRepayType)
                            {
                                case 1:
                                    entRes.NormalMoney -= CdItem.REPAYMONEY.Value;
                                    dNormalRepay += CdItem.REPAYMONEY.Value;
                                    break;
                                case 2:
                                    entRes.ReserveMoney -= CdItem.REPAYMONEY.Value;
                                    dReserveRepay += CdItem.REPAYMONEY.Value;
                                    break;
                                case 3:
                                    entRes.SpecialMoney -= CdItem.REPAYMONEY.Value;
                                    dSpecialRepay += CdItem.REPAYMONEY.Value;
                                    break;
                            }
                        }

                        entRes.TotalMoney = entRes.NormalMoney + entRes.SpecialMoney + entRes.ReserveMoney;
                    }
                }

                if (crs != null && entResList.Count() > 0)
                {
                    foreach (T_FB_CHARGEAPPLYMASTER CsItem in crs)
                    {
                        if (!CsItem.T_FB_CHARGEAPPLYREPAYDETAIL.IsLoaded)
                        {
                            CsItem.T_FB_CHARGEAPPLYREPAYDETAIL.Load();
                        }

                        if (CsItem.T_FB_CHARGEAPPLYREPAYDETAIL == null)
                        {
                            continue;
                        }

                        V_DeptContactDetail entRes = new V_DeptContactDetail();

                        var ec = from b in entResList
                                 where b.OwnerID == CsItem.OWNERID
                                 select b;

                        if (ec == null)
                        {
                            continue;
                        }

                        entRes = ec.FirstOrDefault();

                        if (entRes == null)
                        {
                            continue;
                        }

                        foreach (T_FB_CHARGEAPPLYREPAYDETAIL CdItem in CsItem.T_FB_CHARGEAPPLYREPAYDETAIL)
                        {
                            if (CdItem.REPAYMONEY == 0)
                            {
                                continue;
                            }

                            int iRepayType = 0;
                            int.TryParse(CdItem.REPAYTYPE.ToString(), out iRepayType);
                            switch (iRepayType)
                            {
                                case 1:
                                    entRes.NormalMoney -= CdItem.REPAYMONEY;
                                    dNormalRepay += CdItem.REPAYMONEY;
                                    break;
                                case 2:
                                    entRes.ReserveMoney -= CdItem.REPAYMONEY;
                                    dReserveRepay += CdItem.REPAYMONEY;
                                    break;
                                case 3:
                                    entRes.SpecialMoney -= CdItem.REPAYMONEY;
                                    dSpecialRepay += CdItem.REPAYMONEY;
                                    break;
                            }
                        }

                        entRes.TotalMoney = entRes.NormalMoney + entRes.SpecialMoney + entRes.ReserveMoney;

                    }
                }

                dTotalNormalBorrowMoney -= dNormalRepay;
                dTotalReserveMoney -= dReserveRepay;
                dTotalSpecialMoney -= dSpecialRepay;
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
            }
            return entResList.Where(c => c.TotalMoney != 0).OrderBy(t => t.OwnerName).ToList();
        }

        /// <summary>
        /// 获取指定截至时间的未还款总金额
        /// </summary>
        /// <param name="dUnRepayMoney"></param>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="dtCheckDate"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        private void GetTotalBorrowMoney(ref decimal? dUnRepayMoney, string strOrgType, string strOrgID, DateTime dtCheckDate,
           string strOrderBy, string strFilter, object[] objArgs)
        {
            //借款流水
            var ds = from v in GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                     where v.T_FB_BORROWAPPLYMASTER.CHECKSTATES == 2
                     select v;

            //还款(旧)流水
            var rs = from v in GetObjects<T_FB_REPAYAPPLYDETAIL>().Include("T_FB_REPAYAPPLYMASTER").Include("T_FB_REPAYAPPLYMASTER.T_FB_BORROWAPPLYMASTER")
                     where v.T_FB_REPAYAPPLYMASTER.CHECKSTATES == 2 && v.T_FB_REPAYAPPLYMASTER.T_FB_BORROWAPPLYMASTER != null
                     select v;

            //还款(新)流水
            var rns = from v in GetObjects<T_FB_REPAYAPPLYDETAIL>().Include("T_FB_REPAYAPPLYMASTER").Include("T_FB_REPAYAPPLYMASTER.T_FB_BORROWAPPLYMASTER")
                      where v.T_FB_REPAYAPPLYMASTER.CHECKSTATES == 2 && v.T_FB_REPAYAPPLYMASTER.T_FB_BORROWAPPLYMASTER == null
                      select v;

            //费用报销--冲借款(旧)流水
            var cs = from v in GetObjects<T_FB_CHARGEAPPLYDETAIL>().Include("T_FB_CHARGEAPPLYMASTER").Include("T_FB_CHARGEAPPLYMASTER.T_FB_BORROWAPPLYMASTER")
                     where v.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == 2 && v.T_FB_CHARGEAPPLYMASTER.T_FB_BORROWAPPLYMASTER != null
                     select v;

            //费用报销--冲借款(新)流水
            var cns = from v in GetObjects<T_FB_CHARGEAPPLYREPAYDETAIL>().Include("T_FB_CHARGEAPPLYMASTER").Include("T_FB_CHARGEAPPLYMASTER.T_FB_BORROWAPPLYMASTER")
                      where v.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == 2 && v.T_FB_CHARGEAPPLYMASTER.T_FB_BORROWAPPLYMASTER == null
                      select v;


            switch (strOrgType.ToUpper())
            {
                case "COMPANY":
                    ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    rs = rs.Where(t => t.T_FB_REPAYAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    rns = rns.Where(t => t.T_FB_REPAYAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    cs = cs.Where(t => t.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    cns = cns.Where(t => t.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    break;
                case "DEPARTMENT":
                    ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID == strOrgID);
                    rs = rs.Where(t => t.T_FB_REPAYAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    rns = rns.Where(t => t.T_FB_REPAYAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    cs = cs.Where(t => t.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID == strOrgID);
                    cns = cns.Where(t => t.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID == strOrgID);
                    break;
                case "POST":
                    ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERPOSTID == strOrgID);
                    rs = rs.Where(t => t.T_FB_REPAYAPPLYMASTER.OWNERPOSTID == strOrgID);
                    rns = rns.Where(t => t.T_FB_REPAYAPPLYMASTER.OWNERPOSTID == strOrgID);
                    cs = cs.Where(t => t.T_FB_CHARGEAPPLYMASTER.OWNERPOSTID == strOrgID);
                    cns = cns.Where(t => t.T_FB_CHARGEAPPLYMASTER.OWNERPOSTID == strOrgID);
                    break;
                case "PERSONAL":
                    ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERID == strOrgID);
                    rs = rs.Where(t => t.T_FB_REPAYAPPLYMASTER.OWNERID == strOrgID);
                    rns = rns.Where(t => t.T_FB_REPAYAPPLYMASTER.OWNERID == strOrgID);
                    cs = cs.Where(t => t.T_FB_CHARGEAPPLYMASTER.OWNERID == strOrgID);
                    cns = cns.Where(t => t.T_FB_CHARGEAPPLYMASTER.OWNERID == strOrgID);
                    break;
            }


            ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.UPDATEDATE <= dtCheckDate);
            rs = rs.Where(t => t.T_FB_REPAYAPPLYMASTER.UPDATEDATE <= dtCheckDate);
            rns = rns.Where(t => t.T_FB_REPAYAPPLYMASTER.UPDATEDATE <= dtCheckDate);
            cs = cs.Where(t => t.T_FB_CHARGEAPPLYMASTER.UPDATEDATE <= dtCheckDate);
            cns = cns.Where(t => t.T_FB_CHARGEAPPLYMASTER.UPDATEDATE <= dtCheckDate);


            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                string strRsFilter = string.Copy(strFilter);
                string strCsFilter = string.Copy(strFilter);

                if (strFilter.Contains("OWNERCOMPANYID"))
                {
                    strFilter = strFilter.Replace("OWNERCOMPANYID", "T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID");
                    strRsFilter = strRsFilter.Replace("OWNERCOMPANYID", "T_FB_REPAYAPPLYMASTER.OWNERCOMPANYID");
                    strCsFilter = strCsFilter.Replace("OWNERCOMPANYID", "T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID");
                }

                if (strFilter.Contains("OWNERDEPARTMENTID"))
                {
                    strFilter = strFilter.Replace("OWNERDEPARTMENTID", "T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID");
                    strRsFilter = strRsFilter.Replace("OWNERDEPARTMENTID", "T_FB_REPAYAPPLYMASTER.OWNERDEPARTMENTID");
                    strCsFilter = strCsFilter.Replace("OWNERDEPARTMENTID", "T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID");
                }

                if (strFilter.Contains("OWNERPOSTID"))
                {
                    strFilter = strFilter.Replace("OWNERPOSTID", "T_FB_BORROWAPPLYMASTER.OWNERPOSTID");
                    strRsFilter = strRsFilter.Replace("OWNERPOSTID", "T_FB_REPAYAPPLYMASTER.OWNERPOSTID");
                    strCsFilter = strCsFilter.Replace("OWNERPOSTID", "T_FB_CHARGEAPPLYMASTER.OWNERPOSTID");
                }

                if (strFilter.Contains("OWNERID"))
                {
                    strFilter = strFilter.Replace("OWNERID", "T_FB_BORROWAPPLYMASTER.OWNERID");
                    strRsFilter = strRsFilter.Replace("OWNERID", "T_FB_REPAYAPPLYMASTER.OWNERID");
                    strCsFilter = strCsFilter.Replace("OWNERID", "T_FB_CHARGEAPPLYMASTER.OWNERID");
                }

                if (strFilter.Contains("CREATEUSERID"))
                {
                    strFilter = strFilter.Replace("CREATEUSERID", "T_FB_BORROWAPPLYMASTER.CREATEUSERID");
                    strRsFilter = strRsFilter.Replace("CREATEUSERID", "T_FB_REPAYAPPLYMASTER.CREATEUSERID");
                    strCsFilter = strCsFilter.Replace("CREATEUSERID", "T_FB_CHARGEAPPLYMASTER.CREATEUSERID");
                }

                ds = ds.Where(strFilter, objArgs);
                rs = rs.Where(strRsFilter, objArgs);
                rns = rns.Where(strRsFilter, objArgs);
                cs = cs.Where(strCsFilter, objArgs);
                cns = cns.Where(strCsFilter, objArgs);
            }

            ds = ds.OrderBy(strOrderBy);
            rs = rs.OrderBy(strOrderBy);
            rns = rns.OrderBy(strOrderBy);
            cs = cs.OrderBy(strOrderBy);
            cns = cns.OrderBy(strOrderBy);

            dUnRepayMoney = GeTimeBeforUnRepayMoney(ds, rs, rns, cs, cns);
        }

        /// <summary>
        /// 计算期初未还款总额
        /// </summary>
        /// <param name="ts"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        private decimal? GeTimeBeforUnRepayMoney(IQueryable<T_FB_BORROWAPPLYDETAIL> ts, IQueryable<T_FB_REPAYAPPLYDETAIL> rs, IQueryable<T_FB_REPAYAPPLYDETAIL> rns, IQueryable<T_FB_CHARGEAPPLYDETAIL> cs, IQueryable<T_FB_CHARGEAPPLYREPAYDETAIL> cns)
        {
            decimal? dRes = 0, dRepayMoney = 0, dBorrowMoney = 0;

            if (rs.Count() > 0)
            {
                dRepayMoney = rs.Sum(t => t.REPAYMONEY);
            }

            if (cs.Count() > 0)
            {
                dRepayMoney += cs.Sum(t => t.REPAYMONEY);
            }

            if (ts.Count() > 0)
            {
                dBorrowMoney = ts.Sum(t => t.BORROWMONEY);
            }

            if (rns.Count() > 0)
            {
                dRepayMoney += rns.Sum(t => t.REPAYMONEY);
            }

            if (cns.Count() > 0)
            {
                dRepayMoney += cns.Sum(t => t.REPAYMONEY);
            }

            dRes = dBorrowMoney - dRepayMoney;
            return dRes;
        }


        /// <summary>
        /// 统计部门内各借款类型下的未还款总额
        /// </summary>
        /// <param name="dTotalBorrowMoney"></param>
        /// <param name="dTotalSpecialMoney"></param>
        /// <param name="dTotalReserveMoney"></param>
        /// <param name="strOrgType"></param>
        /// <param name="strOrgID"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        private void GetTotalBorrowMoneyByBorrowType(ref decimal? dTotalBorrowMoney, ref decimal? dTotalSpecialMoney, ref decimal? dTotalReserveMoney,
            string strOrgType, string strOrgID, DateTime dtStart, DateTime dtEnd, string strOrderBy, string strFilter, object[] objArgs)
        {
            //借款
            var ds = from v in GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                     where v.T_FB_BORROWAPPLYMASTER.CHECKSTATES == 2 && v.T_FB_BORROWAPPLYMASTER.REPAYTYPE == 1m
                     select v;

            var ss = from v in GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                     where v.T_FB_BORROWAPPLYMASTER.CHECKSTATES == 2 && v.T_FB_BORROWAPPLYMASTER.REPAYTYPE == 3m
                     select v;

            var rs = from v in GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                     where v.T_FB_BORROWAPPLYMASTER.CHECKSTATES == 2 && v.T_FB_BORROWAPPLYMASTER.REPAYTYPE == 2m
                     select v;

            //组织架构过滤
            switch (strOrgType.ToUpper())
            {
                case "COMPANY":
                    ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    ss = ss.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    rs = rs.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID == strOrgID);
                    break;
                case "DEPARTMENT":
                    ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID == strOrgID);
                    ss = ss.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID == strOrgID);
                    rs = rs.Where(t => t.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID == strOrgID);
                    break;
            }

            DateTime dtCheck = new DateTime();

            if (dtEnd > dtCheck)
            {
                ds = ds.Where(t => t.T_FB_BORROWAPPLYMASTER.UPDATEDATE >= dtStart && t.T_FB_BORROWAPPLYMASTER.UPDATEDATE <= dtEnd);
                ss = ss.Where(t => t.T_FB_BORROWAPPLYMASTER.UPDATEDATE >= dtStart && t.T_FB_BORROWAPPLYMASTER.UPDATEDATE <= dtEnd);
                rs = rs.Where(t => t.T_FB_BORROWAPPLYMASTER.UPDATEDATE >= dtStart && t.T_FB_BORROWAPPLYMASTER.UPDATEDATE <= dtEnd);
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                ds = ds.Where(strFilter, objArgs);
                ss = ss.Where(strFilter, objArgs);
                rs = rs.Where(strFilter, objArgs);
            }

            strOrderBy = strOrderBy.Replace("UPDATEDATE", "T_FB_BORROWAPPLYMASTER.UPDATEDATE");

            ds = ds.OrderBy(strOrderBy);
            ss = ss.OrderBy(strOrderBy);
            rs = rs.OrderBy(strOrderBy);

            if (ds.Count() > 0)
            {
                dTotalBorrowMoney = ds.Sum(t => t.BORROWMONEY);
            }

            if (ss.Count() > 0)
            {
                dTotalSpecialMoney = ss.Sum(t => t.BORROWMONEY);
            }

            if (rs.Count() > 0)
            {
                dTotalReserveMoney = rs.Sum(t => t.BORROWMONEY);
            }
        }

        #endregion 借还款往来查询

        #region 公共函数
        /// <summary>
        /// 获取弹出单据所需要的单据信息XML
        /// </summary>
        /// <param name="objEntity">单据实体</param>
        /// <param name="strFormId">单据ID</param>
        /// <param name="strFormId">系统菜单名</param>
        /// <returns></returns>
        private string GetXmlObject(object objEntity, string strFormId, ref string strTableName)
        {
            if (objEntity == null)
            {
                return string.Empty;
            }

            string strSystype = string.Empty, strSysName = string.Empty, strModelCode = string.Empty, strFormName = string.Empty, strSubmitXmlObj = string.Empty;

            Type a = objEntity.GetType();
            strSysName = GetSysName(a.FullName);
            strSystype = GetSystypeByName(strSysName);
            strModelCode = a.Name;
            strFormName = SMT.SaaS.BLLCommonServices.Utility.GetResourceValue(strModelCode);
            strSubmitXmlObj = SMT.SaaS.BLLCommonServices.Utility.SetSubmitXmlObj(strSysName, strFormName, strFormId, "AUDIT");
            strTableName = GetTableNameByPermissionWS(strModelCode);
            return strSubmitXmlObj;
        }

        /// <summary>
        /// 通过权限服务获取当前表对应的系统菜单名
        /// </summary>
        /// <param name="strModelCode">当前表名</param>
        /// <returns>系统菜单名</returns>
        private string GetTableNameByPermissionWS(string strModelCode)
        {
            string strRes = string.Empty;
            if (string.IsNullOrWhiteSpace(strModelCode))
            {
                return strRes;
            }

            SMT.SaaS.BLLCommonServices.PermissionWS.PermissionServiceClient clientPerm = new SMT.SaaS.BLLCommonServices.PermissionWS.PermissionServiceClient();
            SMT.SaaS.BLLCommonServices.PermissionWS.T_SYS_ENTITYMENU entMenu = clientPerm.GetSysMenuByEntityCode(strModelCode);

            if (entMenu != null)
            {
                strRes = entMenu.MENUNAME;
            }

            return strRes;
        }

        /// <summary>
        /// 获取当前分系统的类型名
        /// </summary>
        /// <param name="strFullClassName"></param>
        /// <returns></returns>
        private static string GetSysName(string strFullClassName)
        {
            string strRes = string.Empty;

            string[] strlist = strFullClassName.Split('_');
            if (strlist.Length > 2)
            {
                strRes = strlist[1].ToString().ToUpper();
            }

            return strRes;
        }

        /// <summary>
        /// 获取当前分系统的类型值
        /// </summary>
        /// <param name="strSysName"></param>
        /// <returns></returns>
        private static string GetSystypeByName(string strSysName)
        {
            string strRes = string.Empty;
            switch (strSysName)
            {
                case "HRM":
                    strRes = "HR";
                    break;
                case "OA":
                    strRes = "OA";
                    break;
                case "FB":
                    strRes = "FB";
                    break;
                default:
                    strRes = string.Empty;
                    break;
            }

            return strRes;
        }

        #endregion 公共函数
    }
}

