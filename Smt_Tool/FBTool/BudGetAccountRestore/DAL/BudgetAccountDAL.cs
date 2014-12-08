using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT_FB_EFModel;
using System.Data;

namespace DAL
{
    public class BudgetAccountDAL : CommDal<T_FB_BUDGETACCOUNT>
    {
        /// <summary>
        /// 临时存储需要计算的科目，避免重复计算
        /// </summary>
        Dictionary<string, DataRow> department;
        /// <summary>
        /// 上月结算临时记录
        /// </summary>
        public List<FBAcountCheck> DicdepartmentCheck = new List<FBAcountCheck>();
        public List<AcountCheckTemp.BUDGETACCOUNT_TEMPRow> DicdepartmentAcount = new List<AcountCheckTemp.BUDGETACCOUNT_TEMPRow>();
        private DataTable dtcharge = new DataTable();

        public BudgetAccountDAL()
        {

        }

        /// <summary>
        /// 按指定月查出所有月份部门预算总额
        /// </summary>
        /// <param name="dstart"></param>
        /// <param name="dend"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public DataTable DtBudGetAllDataSource(DateTime dstart, DateTime dend, ref StringBuilder strMsg)
        {
            //按指定月查出所有月份部门预算总额
            string strSql = @"select t.* from budget_dept t where  "
               + "t.BUDGETARYMONTH >= to_date('" + dstart.ToString("yyyy-MM-dd")
                + "', 'yyyy-MM-dd') and t.updatedate < to_date('" + dend.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')"
            + "order by t.BUDGETARYMONTH";
            DataTable dtbudget = (DataTable)GetDataTable(strSql);

            return dtbudget;
        }

        ///// <summary>
        ///// 更新预算结算记录
        ///// </summary>
        ///// <param name="dYear">指定年份</param>
        ///// <param name="dMonth">指定月份</param>
        //public void UpdateBudgetCheck(decimal dYear, decimal dMonth, ref string strMsg)
        //{

        //    try
        //    {
        //        //更新部门的预算月结记录
        //        UpdateDeptBudgetCheck(dYear, dMonth, ref strMsg);
        //        //更新员工的预算月结记录
        //        UpdateEmpBudgetCheck(dYear, dMonth, ref strMsg);

        //        strMsg = "Success:" + dYear.ToString() + "-" + dMonth.ToString() + "月结记录于"
        //            + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "更新成功";
        //    }
        //    catch (Exception ex)
        //    {
        //        strMsg = "Error:" + dYear.ToString() + "-" + dMonth.ToString() + "月结记录更新失败，错误原因是：" + ex.ToString();
        //    }
        //}

        #region 更新员工的预算月结记录
        /// <summary>
        /// 更新员工级别的预算月结记录
        /// </summary>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="strMsg"></param>
        private void UpdateEmpBudgetCheck(decimal dYear, decimal dMonth, ref string strMsg)
        {
            //按月查出个人预算总额
            string strSql = @"select t.* from t_fb_budgetcheck t where t.accountobjecttype=3 and t.budgetyear="
                + dYear.ToString() + " and t.budgetmonth=" + dMonth.ToString();
            DataTable dtbudget = (DataTable)GetDataTable(strSql);

            if (dtbudget == null)
            {
                strMsg = "Error:当前查询无个人预算月结记录，请核对数据库记录！";
                throw new Exception(strMsg);
            }

            if (dtbudget.Rows.Count == 0)
            {
                strMsg = "Error:当前查询无个人预算月结记录，请核对数据库记录！";
                throw new Exception(strMsg);
            }

            foreach (DataRow dr in dtbudget.Rows)
            {
                //月度调拨调出额度                
                decimal dTransferFromMoney = 0;
                //月度调拨调入额度
                decimal dTransferToMoney = 0;
                //月度预算额度（申请+增补）
                decimal dBudgetMoney = 0;
                //已报销额度合计（报销单）
                decimal dPaidMoney = 0;
                //结算表剩余的可用额度
                decimal dUsableMoney = 0;

                string strBudgetCheckID = string.Empty, strCompanyID = string.Empty, strOwnerID = string.Empty, strSubjectID = string.Empty;

                if (dr["OWNERCOMPANYID"] == null || dr["OWNERID"] == null || dr["SUBJECTID"] == null)
                {
                    strMsg = "Error:请检查当前预算月结记录所属公司，员工及科目ID，对应的月结记录ID为：" + dr[0] + "，请核对数据库记录！";
                    throw new Exception(strMsg);
                }

                strBudgetCheckID = dr[0].ToString();
                strCompanyID = dr["OWNERCOMPANYID"].ToString();
                strOwnerID = dr["OWNERID"].ToString();
                strSubjectID = dr["SUBJECTID"].ToString();

                //暂时拿在线公司做验证
                if (strCompanyID != "bac05c76-0f5b-40ae-b73b-8be541ed35ed" && strSubjectID != "d5134466-c207-44f2-8a36-cf7b96d5851f")
                {
                    continue;
                }

                GetEmpTransferFromMoney(strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, ref dTransferFromMoney);
                GetEmpTransferToMoney(strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, ref dTransferToMoney);
                GetEmpBudgetMoney(strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, ref dBudgetMoney);
                GetEmpPaidMoney(strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, ref dPaidMoney);



                dUsableMoney = dBudgetMoney + dTransferToMoney - dTransferFromMoney - dPaidMoney;

                UpdateEmpBudgetCheck(strBudgetCheckID, strCompanyID, strOwnerID, strSubjectID, dYear, dMonth,
                    dBudgetMoney, dPaidMoney, dUsableMoney);
            }
        }

        /// <summary>
        /// 更新预算结算记录
        /// </summary>
        /// <param name="strBudgetCheckID"></param>
        /// <param name="strCompanyID"></param>
        /// <param name="strOwnerID"></param>
        /// <param name="strSubjectID"></param>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="dBudgetMoney"></param>
        /// <param name="dPaidMoney"></param>
        /// <param name="dUsableMoney"></param>
        private void UpdateEmpBudgetCheck(string strBudgetCheckID, string strCompanyID, string strOwnerID, string strSubjectID, decimal dYear, decimal dMonth, decimal dBudgetMoney, decimal dPaidMoney, decimal dUsableMoney)
        {
            if (dMonth > 1)
            {
                //按月查出部门预算总额
                string strSql = @"select t.* from t_fb_budgetcheck t where t.accountobjecttype=3 and t.budgetyear="
                    + dYear.ToString() + " and t.budgetmonth=" + (dMonth - 1).ToString()
                    + " and t.ownercompanyid='" + strCompanyID + "' and t.ownerid='" + strOwnerID
                    + "' and t.subjectid='" + strSubjectID + "'";
                DataTable dtbudget = (DataTable)GetDataTable(strSql);

                decimal dTemp = 0;
                if (dtbudget != null)
                {
                    if (dtbudget.Rows.Count > 0)
                    {
                        if (dtbudget.Rows[0]["USABLEMONEY"] != null)
                        {
                            decimal.TryParse(dtbudget.Rows[0]["USABLEMONEY"].ToString(), out dTemp);

                            dUsableMoney += dTemp;
                        }
                    }
                }
            }

            string strUpdSql = @" Update t_fb_budgetcheck t set t.budgetmoney = " + dBudgetMoney + ", t.actualmoney= " + dPaidMoney + ", t.usablemoney= " + dUsableMoney + " where t.budgetcheckid='" + strBudgetCheckID + "'";
            ExecuteCustomerSql(strUpdSql);
        }

        /// <summary>
        /// 统计当月调出的部门指定科目的月度预算
        /// </summary>
        /// <param name="strCompanyID">公司ID</param>
        /// <param name="strDepartmentID">员工ID</param>
        /// <param name="strSubjectID">科目ID</param>
        /// <param name="dYear">指定年份</param>
        /// <param name="dMonth">指定月份</param>
        /// <param name="dTransferFromMoney">调出预算额度</param>
        private void GetEmpTransferFromMoney(string strCompanyID, string strOwnerID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dTransferFromMoney)
        {
            DateTime dtCurDate = new DateTime();
            DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
            if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
            {
                return;
            }

            DateTime dtStart = dtCurDate;
            DateTime dtEnd = dtCurDate.AddMonths(1);


            string strSql = @"select dd.* from T_FB_DEPTTRANSFERDETAIL dd inner join T_FB_DEPTTRANSFERmaster t "
                + " on dd.DEPTTRANSFERmasterID  = t.DEPTTRANSFERmasterID where t.checkstates=2"
                + " and t.ownercompanyid='" + strCompanyID + "' and t.transferfrom='" + strOwnerID
                + "' and t.updatedate >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                + "', 'yyyy-MM-dd') and t.updatedate < to_date('" + dtEnd.ToString("yyyy-MM-dd")
                + "', 'yyyy-MM-dd') and dd.subjectid='" + strSubjectID + "'";

            DataTable dtTransferFrom = (DataTable)GetDataTable(strSql);
            foreach (DataRow dr in dtTransferFrom.Rows)
            {
                decimal dTemp = 0;
                if (dr["TRANSFERMONEY"] == null)
                {
                    continue;
                }

                decimal.TryParse(dr["TRANSFERMONEY"].ToString(), out dTemp);
                dTransferFromMoney += dTemp;
            }
        }

        /// <summary>
        /// 统计当月指定部门指定科目的月度预算及增补
        /// </summary>
        /// <param name="strCompanyID">公司ID</param>
        /// <param name="strDepartmentID">部门ID</param>
        /// <param name="strSubjectID">科目ID</param>
        /// <param name="dYear">指定年份</param>
        /// <param name="dMonth">指定月份</param>
        /// <param name="dBudgetMoney">月度预算申请及增补的预算总额度</param>
        private void GetEmpTransferToMoney(string strCompanyID, string strOwnerID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dTransferToMoney)
        {
            DateTime dtCurDate = new DateTime();
            DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
            if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
            {
                return;
            }

            DateTime dtStart = dtCurDate;
            DateTime dtEnd = dtCurDate.AddMonths(1);

            string strSql = @"select dd.* from T_FB_DEPTTRANSFERDETAIL dd inner join T_FB_DEPTTRANSFERmaster t "
                + " on dd.DEPTTRANSFERmasterID  = t.DEPTTRANSFERmasterID where t.checkstates=2"
                + " and t.ownercompanyid='" + strCompanyID + "' and t.transferTo='" + strOwnerID
                + "' and t.updatedate >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                + "', 'yyyy-MM-dd') and t.updatedate < to_date('" + dtEnd.ToString("yyyy-MM-dd")
                + "', 'yyyy-MM-dd') and dd.subjectid='" + strSubjectID + "'";

            DataTable dtTransferFrom = (DataTable)GetDataTable(strSql);
            foreach (DataRow dr in dtTransferFrom.Rows)
            {
                decimal dTemp = 0;
                if (dr["TRANSFERMONEY"] == null)
                {
                    continue;
                }

                decimal.TryParse(dr["TRANSFERMONEY"].ToString(), out dTemp);
                dTransferToMoney += dTemp;
            }
        }

        /// <summary>
        /// 统计当月指定员工指定科目的月度预算及增补
        /// </summary>
        /// <param name="strCompanyID">公司ID</param>
        /// <param name="strOwnerID">员工ID</param>
        /// <param name="strSubjectID">科目ID</param>
        /// <param name="dYear">指定年份</param>
        /// <param name="dMonth">指定月份</param>
        /// <param name="dBudgetMoney">月度预算申请及增补的预算总额度</param>
        private void GetEmpBudgetMoney(string strCompanyID, string strOwnerID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dBudgetMoney)
        {
            DateTime dtCurDate = new DateTime();
            DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
            if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
            {
                return;
            }

            DateTime dtStart = dtCurDate;
            DateTime dtEnd = dtCurDate.AddMonths(1);

            string strSql = @"select * from budget_emp t where t.OWNERCOMPANYID='" + strCompanyID + "'"
                + " and t.OWNERID='" + strOwnerID + "' and t.SUBJECTID='" + strSubjectID + "'"
                + " and t.CheckStatesName='审核通过' and t.budgetmoney <> 0 and t.updatedate >= to_date('"
                + dtStart.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd') and t.updatedate < to_date('"
                + dtEnd.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')";

            DataTable dtBudget = (DataTable)GetDataTable(strSql);
            foreach (DataRow dr in dtBudget.Rows)
            {
                decimal dTemp = 0;
                if (dr["BUDGETMONEY"] == null)
                {
                    continue;
                }

                decimal.TryParse(dr["BUDGETMONEY"].ToString(), out dTemp);
                dBudgetMoney += dTemp;
            }
        }

        /// <summary>
        /// 统计当月指定员工指定科目报销费用合计
        /// </summary>
        /// <param name="strCompanyID">公司ID</param>
        /// <param name="strOwnerID">员工ID</param>
        /// <param name="strSubjectID">科目ID</param>
        /// <param name="dYear">指定年份</param>
        /// <param name="dMonth">指定月份</param>
        /// <param name="dPaidMoney">报销费用合计</param>
        private void GetEmpPaidMoney(string strCompanyID, string strOwnerID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dPaidMoney)
        {
            DateTime dtCurDate = new DateTime();
            DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
            if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
            {
                return;
            }

            DateTime dtStart = dtCurDate;
            DateTime dtEnd = dtCurDate.AddMonths(1);

            string strSql = @"select * from detail_charge c where c.OWNERCOMPANYID='" + strCompanyID + "'"
                + " and c.OWNERID='" + strOwnerID + "' and c.SUBJECTID='" + strSubjectID + "'"
                + " and c.CHECKSTATES=2 and c.CHARGETYPE=1 and c.updatedate >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                + "', 'yyyy-MM-dd') and c.updatedate < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')";

            DataTable dtBudget = (DataTable)GetDataTable(strSql);
            foreach (DataRow dr in dtBudget.Rows)
            {
                decimal dTemp = 0;
                if (dr["CHARGEMONEY"] == null)
                {
                    continue;
                }

                decimal.TryParse(dr["CHARGEMONEY"].ToString(), out dTemp);
                dPaidMoney += dTemp;
            }
        }
        #endregion

        #region 更新部门的预算月结记录
        /// <summary>
        /// 更新部门级别的预算月结记录
        /// </summary>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="strMsg"></param>
        private void UpdateDeptBudgetCheck(decimal dYear, decimal dMonth, ref string strMsg)
        {

            //按月查出部门预算总额
            string strSql = @"select t.* from t_fb_budgetcheck t where t.accountobjecttype=2 and t.budgetyear="
                        + dYear.ToString() + " and t.budgetmonth=" + dMonth.ToString();
            DataTable dtbudget = (DataTable)GetDataTable(strSql);
        }

        #region 处理部门数据
        public string ResetDepartMentMoney(decimal dYear, decimal dMonth, ref string strMsg, DataRow[] drbudget)
        {

            if (drbudget == null)
            {
                strMsg = "Error:当前查询无部门预算月结记录，请核对数据库记录！";
                throw new Exception(strMsg);
            }

            if (drbudget.Length == 0)
            {
                strMsg = "Error:当前查询无部门预算月结记录，请核对数据库记录！";
                throw new Exception(strMsg);
            }

            foreach (DataRow dr in drbudget)
            {

                decimal dTransferFromMoney = 0, dTransferToMoney = 0, dBudgetMoney = 0, dPaidMoney = 0, dUsableMoney = 0;

                string strBudgetCheckID = string.Empty, strCompanyID = string.Empty, strDepartmentID = string.Empty, strSubjectID = string.Empty;

                if (dr["OWNERCOMPANYID"] == null || dr["OWNERDEPARTMENTID"] == null || dr["SUBJECTID"] == null)
                {
                    strMsg = "Error:请检查当前预算月结记录所属公司，部门及科目ID，对应的月结记录ID为：" + dr[0] + "，请核对数据库记录！";
                    throw new Exception(strMsg);
                }

                strBudgetCheckID = dr[0].ToString();
                strCompanyID = dr["OWNERCOMPANYID"].ToString();
                strDepartmentID = dr["OWNERDEPARTMENTID"].ToString();
                strSubjectID = dr["SUBJECTID"].ToString();

                //if (strSubjectID == "4680f0d6-4798-4f93-b0d4-75b5deb7ea1c") break;
                //暂时拿在线公司做验证活动经费
                //if (strCompanyID != "7a613fc2-4431-4a46-ae01-232222e9fcb5" || strDepartmentID != "c4ed3aa9-20b3-4651-9be0-0523d766ee16" || strSubjectID != "5c5d0013-dbce-49d6-9402-746a255c66dc")
                //{
                //    continue;
                //}

                //GetTransferFromMoney(strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, ref dTransferFromMoney);
                //GetTransferToMoney(strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, ref dTransferToMoney);
                //GetBudgetMoney(strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, ref dBudgetMoney);

                foreach (DataRow drs in drbudget)
                {
                    if (drs["SUBJECTID"].ToString() == strSubjectID && drs["OWNERCOMPANYID"].ToString()
                        == strCompanyID && drs["OWNERDEPARTMENTID"].ToString() == strDepartmentID)
                    {
                        //调出额度
                        dTransferFromMoney += decimal.Parse(drs["TRANSFERFROMMOENY"].ToString());
                        //调入额度
                        dTransferToMoney += decimal.Parse(drs["TRANSFERTOMONEY"].ToString());
                        dBudgetMoney += decimal.Parse(drs["BUDGETMONEY"].ToString());
                    }
                }

                GetPaidMoney(strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, ref dPaidMoney);
                dUsableMoney = dBudgetMoney + dTransferToMoney - dTransferFromMoney - dPaidMoney;
                //暂时不更新UpdateBudgetCheck(strBudgetCheckID, strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth,
                //dBudgetMoney, dPaidMoney, dUsableMoney);


                //更新临时表空间
                string strguid = dr["SUBJECTID"].ToString() + dr["OWNERDEPARTMENTID"].ToString()
                    + dr["OWNERCOMPANYID"].ToString() + dMonth.ToString();

                var q = from ent in DicdepartmentCheck.AsQueryable()
                        where ent.ID == strguid
                        select ent;
                if (q.Count() > 0)
                {
                    //如果本月存在，则更新本月数据
                    FBAcountCheck fBAcountCheck = q.FirstOrDefault();
                    fBAcountCheck.year = dYear;
                    fBAcountCheck.month = dMonth;
                    fBAcountCheck.Bugedmoney += dBudgetMoney;
                    fBAcountCheck.PaidMoney += dPaidMoney;
                    fBAcountCheck.UsableMoney += dUsableMoney;
                }
                else
                {
                    //如果本月不存在新增一条记录进临时结算表                 

                    FBAcountCheck fBAcountCheck = new FBAcountCheck();
                    fBAcountCheck.ID = strguid;
                    fBAcountCheck.year = dYear;
                    fBAcountCheck.month = dMonth;
                    fBAcountCheck.OWNERCOMPANYID = strCompanyID;
                    fBAcountCheck.OWNERDEPARTMENTID = strDepartmentID;
                    fBAcountCheck.SUBJECTID = strSubjectID;
                    fBAcountCheck.Bugedmoney = dBudgetMoney;
                    fBAcountCheck.PaidMoney = dPaidMoney;
                    fBAcountCheck.UsableMoney = dUsableMoney;
                    fBAcountCheck.AcountType = "2";//部门
                    fBAcountCheck.CompanyName = dr["OWNERCOMPANYNAME"].ToString();
                    fBAcountCheck.DepartmentName = dr["OWNERDEPARTMENTNAME"].ToString();
                    fBAcountCheck.AccountTypeName = "部门预算";
                    fBAcountCheck.subjectName = dr["SUBJECTNAME"].ToString();

                    //判断上月是否存在数据，存在则累加上月额度
                    if (dMonth - 1 > 0)
                    {
                        string strguidLast = dr["SUBJECTID"].ToString() + dr["OWNERDEPARTMENTID"].ToString()
                        + dr["OWNERCOMPANYID"].ToString() + (dMonth - 1).ToString();

                        var lastq = from ent in DicdepartmentCheck.AsQueryable()
                                    where ent.ID == strguidLast
                                    select ent;
                        if (lastq.Count() > 0)
                        {
                            fBAcountCheck.UsableMoney += lastq.FirstOrDefault().UsableMoney;
                        }

                    }
                    DicdepartmentCheck.Add(fBAcountCheck);
                }


            }




            return strMsg;
        }

        /// <summary>
        /// 统计当月调出的部门指定科目的月度预算
        /// </summary>
        /// <param name="strDepartmentID">部门ID</param>
        /// <param name="strSubjectID">科目ID</param>
        /// <param name="dYear">指定年份</param>
        /// <param name="dMonth">指定月份</param>
        /// <param name="dTransferFromMoney">调出预算额度</param>
        //private void GetTransferFromMoney(string strCompanyID, string strDepartmentID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dTransferFromMoney)
        //{
        //    DateTime dtCurDate = new DateTime();
        //    DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
        //    if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
        //    {
        //        return;
        //    }

        //    DateTime dtStart = dtCurDate;
        //    DateTime dtEnd = dtCurDate.AddMonths(1);


        //    string strSql = @"select dd.* from T_FB_DEPTTRANSFERDETAIL dd inner join T_FB_DEPTTRANSFERmaster t "
        //        + " on dd.DEPTTRANSFERmasterID  = t.DEPTTRANSFERmasterID where t.checkstates=2"
        //        + " and t.ownercompanyid='" + strCompanyID + "' and t.transferfrom='" + strDepartmentID
        //        + "' and t.updatedate >= to_date('" + dtStart.ToString("yyyy-MM-dd")
        //        + "', 'yyyy-MM-dd') and t.updatedate < to_date('" + dtEnd.ToString("yyyy-MM-dd")
        //        + "', 'yyyy-MM-dd') and dd.subjectid='" + strSubjectID + "'";

        //    DataTable dtTransferFrom = (DataTable)GetDataTable(strSql);
        //    foreach (DataRow dr in dtTransferFrom.Rows)
        //    {
        //        decimal dTemp = 0;
        //        if (dr["TRANSFERMONEY"] == null)
        //        {
        //            continue;
        //        }

        //        decimal.TryParse(dr["TRANSFERMONEY"].ToString(), out dTemp);
        //        dTransferFromMoney += dTemp;
        //    }
        //}


        /// <summary>
        /// 统计当月调入的部门指定科目的月度预算
        /// </summary>
        /// <param name="strDepartmentID">部门ID</param>
        /// <param name="strSubjectID">科目ID</param>
        /// <param name="dYear">指定年份</param>
        /// <param name="dMonth">指定月份</param>
        /// <param name="dTransferFromMoney">调入预算额度</param>
        //private void GetTransferToMoney(string strCompanyID, string strDepartmentID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dTransferToMoney)
        //{
        //    DateTime dtCurDate = new DateTime();
        //    DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
        //    if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
        //    {
        //        return;
        //    }

        //    DateTime dtStart = dtCurDate;
        //    DateTime dtEnd = dtCurDate.AddMonths(1);


        //    string strSql = @"select dd.* from T_FB_DEPTTRANSFERDETAIL dd inner join T_FB_DEPTTRANSFERmaster t "
        //        + " on dd.DEPTTRANSFERmasterID  = t.DEPTTRANSFERmasterID where t.checkstates=2"
        //        + " and t.ownercompanyid='" + strCompanyID + "' and t.transferTo='" + strDepartmentID
        //        + "' and t.updatedate >= to_date('" + dtStart.ToString("yyyy-MM-dd")
        //        + "', 'yyyy-MM-dd') and t.updatedate < to_date('" + dtEnd.ToString("yyyy-MM-dd")
        //        + "', 'yyyy-MM-dd') and dd.subjectid='" + strSubjectID + "'";

        //    DataTable dtTransferFrom = (DataTable)GetDataTable(strSql);
        //    foreach (DataRow dr in dtTransferFrom.Rows)
        //    {
        //        decimal dTemp = 0;
        //        if (dr["TRANSFERMONEY"] == null)
        //        {
        //            continue;
        //        }

        //        decimal.TryParse(dr["TRANSFERMONEY"].ToString(), out dTemp);
        //        dTransferToMoney += dTemp;
        //    }
        //}

        #endregion
        /// <summary>
        /// 更新预算结算记录
        /// </summary>
        /// <param name="strBudgetCheckID"></param>
        /// <param name="strCompanyID"></param>
        /// <param name="strDepartmentID"></param>
        /// <param name="strSubjectID"></param>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="dBudgetMoney"></param>
        /// <param name="dPaidMoney"></param>
        /// <param name="dUsableMoney"></param>
        //private void UpdateBudgetCheck(string strBudgetCheckID, string strCompanyID, string strDepartmentID,
        //    string strSubjectID, decimal dYear, decimal dMonth, decimal dBudgetMoney, decimal dPaidMoney,
        //    decimal dUsableMoney)
        //{
        //    if (dMonth > 1)
        //    {
        //        //按月查出部门预算总额
        //        string strSql = @"select t.* from t_fb_budgetcheck t where t.accountobjecttype=2 and t.budgetyear="
        //                    + dYear.ToString() + " and t.budgetmonth=" + (dMonth - 1).ToString()
        //                    + " and t.ownercompanyid='" + strCompanyID + "' and t.ownerdepartmentid='" + strDepartmentID
        //                    + "' and t.subjectid='" + strSubjectID + "'";
        //        DataTable dtbudget = (DataTable)GetDataTable(strSql);

        //        decimal dTemp = 0;
        //        if (dtbudget != null)
        //        {
        //            if (dtbudget.Rows.Count > 0)
        //            {
        //                if (dtbudget.Rows[0]["USABLEMONEY"] != null)
        //                {

        //                    decimal.TryParse(dtbudget.Rows[0]["USABLEMONEY"].ToString(), out dTemp);

        //                    dUsableMoney += dTemp;
        //                }
        //            }
        //        }
        //    }

        //    string strUpdSql = @" Update t_fb_budgetcheck t set t.budgetmoney = " + dBudgetMoney + ", t.actualmoney= " + dPaidMoney + ", t.usablemoney= " + dUsableMoney + " where t.budgetcheckid='" + strBudgetCheckID + "'";
        //    ExecuteCustomerSql(strUpdSql);
        //}


        /// <summary>
        /// 统计当月指定部门指定科目的月度预算及增补
        /// </summary>
        /// <param name="strCompanyID">公司ID</param>
        /// <param name="strDepartmentID">部门ID</param>
        /// <param name="strSubjectID">科目ID</param>
        /// <param name="dYear">指定年份</param>
        /// <param name="dMonth">指定月份</param>
        /// <param name="dBudgetMoney">月度预算申请及增补的预算总额度</param>
        //private void GetBudgetMoney(string strCompanyID, string strDepartmentID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dBudgetMoney)
        //{
        //    DateTime dtCurDate = new DateTime();
        //    DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
        //    if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
        //    {
        //        return;
        //    }

        //    DateTime dtStart = dtCurDate;
        //    DateTime dtEnd = dtCurDate.AddMonths(1);

        //    string strSql = @"select * from budget_dept t where t.OWNERCOMPANYID='" + strCompanyID + "'"
        //        + " and t.OWNERDEPARTMENTID='" + strDepartmentID + "' and t.SUBJECTID='" + strSubjectID + "'"
        //        + " and t.CheckStatesName='审核通过' and t.updatedate >= to_date('" + dtStart.ToString("yyyy-MM-dd")
        //        + "', 'yyyy-MM-dd') and t.updatedate < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')";

        //    DataTable dtBudget = (DataTable)GetDataTable(strSql);
        //    foreach (DataRow dr in dtBudget.Rows)
        //    {
        //        decimal dTemp = 0;
        //        if (dr["BUDGETMONEY"] == null)
        //        {
        //            continue;
        //        }

        //        decimal.TryParse(dr["BUDGETMONEY"].ToString(), out dTemp);
        //        dBudgetMoney += dTemp;
        //    }
        //}

        /// <summary>
        /// 统计当月指定部门指定科目报销费用合计
        /// </summary>
        /// <param name="strCompanyID">公司ID</param>
        /// <param name="strDepartmentID">部门ID</param>
        /// <param name="strSubjectID">科目ID</param>
        /// <param name="dYear">指定年份</param>
        /// <param name="dMonth">指定月份</param>
        /// <param name="dPaidMoney">报销费用合计</param>
        private void GetPaidMoney(string strCompanyID, string strDepartmentID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dPaidMoney)
        {
            DateTime dtCurDate = new DateTime();
            DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
            if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
            {
                return;
            }

            DateTime dtStart = dtCurDate;
            DateTime dtEnd = dtCurDate.AddMonths(1);

            if (dtcharge.Rows.Count <= 0)
            {
                string strSql = @"select * from detail_charge ";
                dtcharge = (DataTable)GetDataTable(strSql);
            }
            DataRow[] rows = dtcharge.Select(@"OWNERCOMPANYID='" + strCompanyID + "'"
                + "and CHECKSTATES=2  and OWNERDEPARTMENTID='" + strDepartmentID + "' and SUBJECTID='" + strSubjectID + "'"
                + " and CHARGETYPE=2 and UPDATEDATE >= '" + dtStart.ToString("yyyy-MM-dd") + "'"
                + "and UPDATEDATE < '" + dtEnd.ToString("yyyy-MM-dd") + "'");
            foreach (DataRow dr in rows)
            {
                decimal dTemp = 0;
                if (dr["CHARGEMONEY"] == null)
                {
                    continue;
                }

                decimal.TryParse(dr["CHARGEMONEY"].ToString(), out dTemp);
                dPaidMoney += dTemp;
            }
        }
        #endregion

        /// <summary>
        /// 更新预算总账记录
        /// </summary>
        /// <param name="dYear">当前年份</param>
        /// <param name="dMonth">当前月份</param>
        public void UpdateBudgetAccount(decimal dYear, decimal dMonth, ref string strMsg)
        {
            try
            {
                //UpdateCompBudgetAccount(dYear, dMonth);   //年度暂时不处理
                //UpdateDeptBudgetAccount(dYear, dMonth, ref strMsg);
                UpdateEmpBudgetAccount(dYear, dMonth, ref strMsg);
                strMsg = "Success:" + dYear.ToString() + "-" + dMonth.ToString() + "总账记录于"
                                   + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "更新成功";
            }
            catch (Exception ex)
            {
                strMsg = "Error:" + dYear.ToString() + "-" + dMonth.ToString() + "总账记录更新失败，错误原因是：" + ex.ToString();
            }
        }

        #region 更新公司预算总账记录
        /// <summary>
        /// 更新公司级预算总账记录
        /// </summary>
        /// <param name="dYear">当前年份</param>
        /// <param name="dMonth">当前月份</param>
        private void UpdateCompBudgetAccount(decimal dYear, decimal dMonth)
        {

        }
        #endregion

        #region 更新部门预算总账记录
        /// <summary>
        /// 更新部门级预算总账记录
        /// </summary>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="strMsg"></param>
        public void ResetDeptBudgetAccount(decimal dYear, decimal dMonth, ref string strMsg, DataRow[] drbudget)
        {
            if (drbudget == null)
            {
                strMsg = "Error:当前查询无部门预算总账记录，请核对数据库记录！";
                throw new Exception(strMsg);
            }

            if (drbudget.Length == 0)
            {
                strMsg = "Error:当前查询无部门预算总账记录，请核对数据库记录！";
                throw new Exception(strMsg);
            }

            foreach (DataRow dr in drbudget)
            {
                decimal dTransferFromMoney = 0, dTransferToMoney = 0, dBudgetMoney = 0, dPaidMoney = 0, dPayingMoney = 0;
                decimal dActualmoney = 0, dUsableMoney = 0;

                string strBudgetCheckID = string.Empty, strCompanyID = string.Empty, strDepartmentID = string.Empty, strSubjectID = string.Empty;

                if (dr["OWNERCOMPANYID"] == null || dr["OWNERDEPARTMENTID"] == null || dr["SUBJECTID"] == null)
                {
                    strMsg = "Error:请检查当前预算总账记录所属公司，部门及科目ID，对应的总账记录ID为：" + dr[0] + "，请核对数据库记录！";
                    throw new Exception(strMsg);
                }

                strBudgetCheckID = dr[0].ToString();
                strCompanyID = dr["OWNERCOMPANYID"].ToString();
                strDepartmentID = dr["OWNERDEPARTMENTID"].ToString();
                strSubjectID = dr["SUBJECTID"].ToString();

                //暂时拿在线公司做验证
                if (strCompanyID != "bac05c76-0f5b-40ae-b73b-8be541ed35ed" && strSubjectID != "d5134466-c207-44f2-8a36-cf7b96d5851f")
                {
                    continue;
                }


                foreach (DataRow drs in drbudget)
                {
                    if (drs["SUBJECTID"].ToString() == strSubjectID && drs["OWNERCOMPANYID"].ToString()
                        == strCompanyID && drs["OWNERDEPARTMENTID"].ToString() == strDepartmentID)
                    {
                        //调出额度
                        dTransferFromMoney += decimal.Parse(drs["TRANSFERFROMMOENY"].ToString());
                        //调入额度
                        dTransferToMoney += decimal.Parse(drs["TRANSFERTOMONEY"].ToString());
                        dBudgetMoney += decimal.Parse(drs["BUDGETMONEY"].ToString());
                    }
                }
                //GetTransferFromMoney(strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, ref dTransferFromMoney);
                //GetTransferToMoney(strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, ref dTransferToMoney);
                //GetBudgetMoney(strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, ref dBudgetMoney);
                GetPaidMoney(strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, ref dPaidMoney);
                GetPayingMoney(strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, ref dPayingMoney);

                dActualmoney = dBudgetMoney + dTransferToMoney - dTransferFromMoney - dPaidMoney;
                dUsableMoney = dActualmoney - dPayingMoney;



                //更新临时表空间
                string strguid = dr["SUBJECTID"].ToString() + dr["OWNERDEPARTMENTID"].ToString()
                    + dr["OWNERCOMPANYID"].ToString() + dMonth.ToString();

                var q = from ent in DicdepartmentAcount.AsQueryable()
                        where ent.BUDGETACCOUNTID == strguid
                        select ent;
                if (q.Count() > 0)
                {
                    //如果本月存在，则更新本月数据
                    AcountCheckTemp.BUDGETACCOUNT_TEMPRow fBAcountCheck = q.FirstOrDefault();
                    fBAcountCheck.BUDGETYEAR = dYear;
                    fBAcountCheck.BUDGETMONTH = dMonth;
                    fBAcountCheck.BUDGETMONEY += dBudgetMoney;
                    fBAcountCheck.PAIEDMONEY += dPaidMoney;
                    fBAcountCheck.USABLEMONEY += dUsableMoney;
                }
                else
                {
                    //如果本月不存在新增一条记录进临时结算表                 
                    AcountCheckTemp.BUDGETACCOUNT_TEMPDataTable dt = new AcountCheckTemp.BUDGETACCOUNT_TEMPDataTable();

                    AcountCheckTemp.BUDGETACCOUNT_TEMPRow fBAcountCheck = dt.NewBUDGETACCOUNT_TEMPRow();
                    fBAcountCheck.BUDGETACCOUNTID = strguid;

                    fBAcountCheck.BUDGETYEAR = dYear;
                    fBAcountCheck.BUDGETMONTH = dMonth;
                    fBAcountCheck.OWNERCOMPANYID = strCompanyID;
                    fBAcountCheck.OWNERDEPARTMENTID = strDepartmentID;
                    fBAcountCheck.SUBJECTID = strSubjectID;
                    fBAcountCheck.BUDGETMONEY = dBudgetMoney;
                    fBAcountCheck.PAIEDMONEY = dPaidMoney;
                    fBAcountCheck.USABLEMONEY = dUsableMoney;
                    fBAcountCheck.ACCOUNTOBJECTTYPE = 2;//部门


                    //判断上月是否存在数据，存在则累加上月额度
                    if (dMonth - 1 > 0)
                    {
                        string strguidLast = dr["SUBJECTID"].ToString() + dr["OWNERDEPARTMENTID"].ToString()
                        + dr["OWNERCOMPANYID"].ToString() + (dMonth - 1).ToString();

                        var lastq = from ent in DicdepartmentCheck.AsQueryable()
                                    where ent.ID == strguidLast
                                    select ent;
                        if (lastq.Count() > 0)
                        {
                            fBAcountCheck.USABLEMONEY += lastq.FirstOrDefault().UsableMoney;
                        }

                    }
                    DicdepartmentAcount.Add(fBAcountCheck);
                }


                //UpdateDeptBudgetAccount(strBudgetCheckID, strCompanyID, strDepartmentID, strSubjectID, dYear, dMonth, dActualmoney, dUsableMoney);
            }
        }

        /// <summary>
        /// 获取部门级别的审核中报销费用合计
        /// </summary>
        /// <param name="strCompanyID"></param>
        /// <param name="strDepartmentID"></param>
        /// <param name="strSubjectID"></param>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="dPaidMoney"></param>
        private void GetPayingMoney(string strCompanyID, string strDepartmentID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dPaidMoney)
        {
            DateTime dtCurDate = new DateTime();
            DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
            if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
            {
                return;
            }

            DateTime dtStart = dtCurDate;
            DateTime dtEnd = dtCurDate.AddMonths(1);

            if (dtcharge.Rows.Count <= 0)
            {
                string strSql = @"select * from detail_charge ";
                dtcharge = (DataTable)GetDataTable(strSql);
            }

            //string strSql = @"select * from detail_charge c where c.OWNERCOMPANYID='" + strCompanyID + "'"
            //    + " and c.OWNERDEPARTMENTID='" + strDepartmentID + "' and c.SUBJECTID='" + strSubjectID + "'"
            //    + " and c.CHECKSTATES=2 and c.CHARGETYPE=2 and c.updatedate >= to_date('" + dtStart.ToString("yyyy-MM-dd")
            //    + "', 'yyyy-MM-dd') and c.updatedate < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')";
            DataRow[] rows = dtcharge.Select(@"OWNERCOMPANYID='" + strCompanyID + "'"
                + "and CHECKSTATES=1  and OWNERDEPARTMENTID='" + strDepartmentID + "' and SUBJECTID='" + strSubjectID + "'"
                + " and CHARGETYPE=2 and UPDATEDATE >= '" + dtStart.ToString("yyyy-MM-dd") + "'"
                + "and UPDATEDATE < '" + dtEnd.ToString("yyyy-MM-dd") + "'");
            foreach (DataRow dr in rows)
            {
                decimal dTemp = 0;
                if (dr["CHARGEMONEY"] == null)
                {
                    continue;
                }

                decimal.TryParse(dr["CHARGEMONEY"].ToString(), out dTemp);
                dPaidMoney += dTemp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strBudgetCheckID"></param>
        /// <param name="strCompanyID"></param>
        /// <param name="strDepartmentID"></param>
        /// <param name="strSubjectID"></param>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="dUsableMoney"></param>
        private void UpdateDeptBudgetAccount(string strBudgetCheckID, string strCompanyID, string strDepartmentID, string strSubjectID, decimal dYear, decimal dMonth, decimal dActualmoney, decimal dUsableMoney)
        {
            if (dMonth <= 1)
            {
                return;
            }

            //按月查出部门预算总额
            string strSql = @"select t.* from t_fb_budgetcheck t where t.accountobjecttype=2 and t.budgetyear="
                + dYear.ToString() + " and t.budgetmonth=" + (dMonth - 1).ToString() + " and t.subjectid='" + strSubjectID
                + "' and t.ownercompanyid='" + strCompanyID + "' and t.ownerdepartmentid='" + strDepartmentID + "'";
            DataTable dtbudget = (DataTable)GetDataTable(strSql);

            if (dtbudget == null)
            {
                return;
            }

            if (dtbudget.Rows.Count == 0)
            {
                return;
            }

            if (dtbudget.Rows[0]["USABLEMONEY"] == null)
            {
                return;
            }

            decimal dTemp = 0;
            decimal.TryParse(dtbudget.Rows[0]["USABLEMONEY"].ToString(), out dTemp);

            dUsableMoney += dTemp;
            dActualmoney += dTemp;

            string strUpdSql = @" Update t_fb_budgetaccount t set t.actualmoney = " + dActualmoney + ", t.usablemoney= " + dUsableMoney + " where t.budgetaccountid='" + strBudgetCheckID + "'";
            ExecuteCustomerSql(strUpdSql);
        }
        #endregion

        #region 更新员工预算总账记录
        /// <summary>
        /// 更新员工级预算总账记录
        /// </summary>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="strMsg"></param>
        private void UpdateEmpBudgetAccount(decimal dYear, decimal dMonth, ref string strMsg)
        {
            //按月查出个人预算总额
            string strSql = @"select t.* from t_fb_budgetaccount t where t.accountobjecttype=3 and t.budgetyear="
                + dYear.ToString() + " and t.budgetmonth=" + dMonth.ToString();
            DataTable dtbudget = (DataTable)GetDataTable(strSql);

            if (dtbudget == null)
            {
                strMsg = "Error:当前查询无个人预算总账记录，请核对数据库记录！";
                throw new Exception(strMsg);
            }

            if (dtbudget.Rows.Count == 0)
            {
                strMsg = "Error:当前查询无个人预算总账记录，请核对数据库记录！";
                throw new Exception(strMsg);
            }

            foreach (DataRow dr in dtbudget.Rows)
            {
                decimal dTransferFromMoney = 0, dTransferToMoney = 0, dBudgetMoney = 0, dPaidMoney = 0, dPayingMoney = 0;
                decimal dActualmoney = 0, dUsableMoney = 0;

                string strBudgetCheckID = string.Empty, strCompanyID = string.Empty, strOwnerID = string.Empty, strSubjectID = string.Empty;

                if (dr["OWNERCOMPANYID"] == null || dr["OWNERID"] == null || dr["SUBJECTID"] == null)
                {
                    strMsg = "Error:请检查当前预算总账记录所属公司，员工及科目ID，对应的总账记录ID为：" + dr[0] + "，请核对数据库记录！";
                    throw new Exception(strMsg);
                }

                strBudgetCheckID = dr[0].ToString();
                strCompanyID = dr["OWNERCOMPANYID"].ToString();
                strOwnerID = dr["OWNERID"].ToString();
                strSubjectID = dr["SUBJECTID"].ToString();

                //暂时拿在线公司做验证
                if (strCompanyID != "bac05c76-0f5b-40ae-b73b-8be541ed35ed" && strSubjectID != "d5134466-c207-44f2-8a36-cf7b96d5851f")
                {
                    continue;
                }

                GetEmpTransferFromMoney(strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, ref dTransferFromMoney);
                GetEmpTransferToMoney(strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, ref dTransferToMoney);
                GetEmpBudgetMoney(strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, ref dBudgetMoney);
                GetEmpPaidMoney(strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, ref dPaidMoney);
                GetEmpPayingMoney(strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, ref dPayingMoney);

                dActualmoney = dBudgetMoney + dTransferToMoney - dTransferFromMoney - dPaidMoney;
                dUsableMoney = dActualmoney - dPayingMoney;

                UpdateEmpBudgetAccount(strBudgetCheckID, strCompanyID, strOwnerID, strSubjectID, dYear, dMonth, dActualmoney, dUsableMoney);
            }
        }

        /// <summary>
        /// 获取员工级别的审核中报销费用合计
        /// </summary>
        /// <param name="strCompanyID"></param>
        /// <param name="strOwnerID"></param>
        /// <param name="strSubjectID"></param>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="dPayingMoney"></param>
        private void GetEmpPayingMoney(string strCompanyID, string strOwnerID, string strSubjectID, decimal dYear, decimal dMonth, ref decimal dPayingMoney)
        {
            DateTime dtCurDate = new DateTime();
            DateTime.TryParse(dYear.ToString() + "-" + dMonth + "-1", out dtCurDate);
            if (dYear != dtCurDate.Year || dMonth != dtCurDate.Month)
            {
                return;
            }

            DateTime dtStart = dtCurDate;
            DateTime dtEnd = dtCurDate.AddMonths(1);

            string strSql = @"select * from detail_charge c where c.OWNERCOMPANYID='" + strCompanyID + "'"
                + " and c.OWNERID='" + strOwnerID + "' and c.SUBJECTID='" + strSubjectID + "'"
                + " and c.CHECKSTATES=1 and c.CHARGETYPE=1 and c.updatedate >= to_date('" + dtStart.ToString("yyyy-MM-dd")
                + "', 'yyyy-MM-dd') and c.updatedate < to_date('" + dtEnd.ToString("yyyy-MM-dd") + "', 'yyyy-MM-dd')";

            DataTable dtBudget = (DataTable)GetDataTable(strSql);
            foreach (DataRow dr in dtBudget.Rows)
            {
                decimal dTemp = 0;
                if (dr["CHARGEMONEY"] == null)
                {
                    continue;
                }

                decimal.TryParse(dr["CHARGEMONEY"].ToString(), out dTemp);
                dPayingMoney += dTemp;
            }
        }

        /// <summary>
        /// 更新指定员工级预算总账记录
        /// </summary>
        /// <param name="strBudgetCheckID"></param>
        /// <param name="strCompanyID"></param>
        /// <param name="strOwnerID"></param>
        /// <param name="strSubjectID"></param>
        /// <param name="dYear"></param>
        /// <param name="dMonth"></param>
        /// <param name="dUsableMoney"></param>
        private void UpdateEmpBudgetAccount(string strBudgetCheckID, string strCompanyID, string strOwnerID, string strSubjectID, decimal dYear, decimal dMonth, decimal dActualmoney, decimal dUsableMoney)
        {
            if (dMonth <= 1)
            {
                return;
            }

            //按月查出部门预算总额
            string strSql = @"select t.* from t_fb_budgetcheck t where t.accountobjecttype=3 and t.budgetyear="
                + dYear.ToString() + " and t.budgetmonth=" + (dMonth - 1).ToString() + " and t.subjectid='" + strSubjectID
                + "' and t.ownercompanyid='" + strCompanyID + "' and t.ownerid='" + strOwnerID + "'";
            DataTable dtbudget = (DataTable)GetDataTable(strSql);

            if (dtbudget == null)
            {
                return;
            }

            if (dtbudget.Rows.Count == 0)
            {
                return;
            }

            if (dtbudget.Rows[0]["USABLEMONEY"] == null)
            {
                return;
            }

            decimal dTemp = 0;
            decimal.TryParse(dtbudget.Rows[0]["USABLEMONEY"].ToString(), out dTemp);

            dUsableMoney += dTemp;
            dActualmoney += dTemp;

            string strUpdSql = @" Update t_fb_budgetaccount t set t.actualmoney = " + dActualmoney + ", t.usablemoney= " + dUsableMoney + " where t.budgetaccountid='" + strBudgetCheckID + "'";
            ExecuteCustomerSql(strUpdSql);
        }
        #endregion
    }
}
