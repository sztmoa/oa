
/*
 * 文件名：ChargeApplyMasterDAL.cs
 * 作  用：T_FB_CHARGEAPPLYMASTER 数据操作类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 9:52:15
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

using SMT_FB_EFModel;
using SMT.FBAnalysis.CustomModel;
using SMT.Foundation.Log;
using System.Data;

namespace SMT.FBAnalysis.DAL
{
    public class ChargeApplyMasterDAL : CommDal<T_FB_CHARGEAPPLYMASTER>
    {
        public static SMT.SaaS.BLLCommonServices.Utility UtilityClass = new SaaS.BLLCommonServices.Utility();

        public ChargeApplyMasterDAL()
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
        /// 获取指定条件的T_FB_CHARGEAPPLYMASTER信息
        /// </summary>        
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_CHARGEAPPLYMASTER信息</returns>
        public T_FB_CHARGEAPPLYMASTER GetChargeApplyMasterRdByMultSearch(string strFilter, params object[] objArgs)
        {
            var q = from v in GetObjects().Include("T_FB_BORROWAPPLYMASTER").Include("T_FB_EXTENSIONALORDER")
                    select v;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }

            T_FB_CHARGEAPPLYMASTER entRes = q.First();
            if (!entRes.T_FB_CHARGEAPPLYDETAIL.IsLoaded)
            {
                entRes.T_FB_CHARGEAPPLYDETAIL.Load();
            }

            return entRes;
        }

        /// <summary>
        /// 获取指定条件的费用报销信息
        /// </summary>
        /// <param name="strDateStart">查询起始时间</param>
        /// <param name="strDateEnd">查询截止时间</param>
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <returns>费用报销信息</returns>
        public IQueryable<T_FB_CHARGEAPPLYMASTER> GetChargeApplyMasterRdListByMultSearch(string strCheckStates, string strDateStart, string strDateEnd,
            string strOrderBy, string strFilter, params object[] objArgs)
        {
            var ents = from c in GetObjects()
                       select c;

            DateTime dtCheck = new DateTime();
            if (!string.IsNullOrWhiteSpace(strDateStart))
            {
                DateTime dtStart = new DateTime();
                DateTime.TryParse(strDateStart, out dtStart);
                if (dtStart > dtCheck)
                {
                    ents = ents.Where(c => c.UPDATEDATE >= dtStart);
                }
            }

            if (!string.IsNullOrWhiteSpace(strDateEnd))
            {
                DateTime dtEnd = new DateTime();
                DateTime.TryParse(strDateEnd, out dtEnd);
                if (dtEnd > dtCheck)
                {
                    ents = ents.Where(c => c.UPDATEDATE <= dtEnd);
                }
            }

            if (!string.IsNullOrWhiteSpace(strCheckStates))
            {
                decimal dCheckStates = 0;
                decimal.TryParse(strCheckStates, out dCheckStates);

                ents = ents.Where(c => c.CHECKSTATES == dCheckStates);
            }


            if (!string.IsNullOrWhiteSpace(strFilter) && objArgs.Count() > 0)
            {
                ents = ents.Where(strFilter, objArgs);
            }

            return ents.OrderBy(strOrderBy);
        }

        /// <summary>
        /// 获取指定条件的T_FB_CHARGEAPPLYMASTER信息
        /// </summary>        
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_CHARGEAPPLYMASTER信息</returns>
        public IQueryable<T_FB_CHARGEAPPLYMASTER> GetChargeApplyMasterRdListByMultSearch(string strOrderBy, string strFilter, string checkState, string userID, List<string> guidStringList, params object[] objArgs)
        {
            int chkState = 5;
            var q = from v in GetObjects()
                    select v;

            List<object> queryParas = new List<object>();
            if (objArgs != null)
            {
                queryParas.AddRange(objArgs);
            }
            //UtilityClass.SetOrganizationFilter(ref strFilter, ref queryParas, userID, "T_FB_CHARGEAPPLYMASTER");
            if (guidStringList == null)
            {
                if (!(strFilter.IndexOf("OWNERID") > -1))
                {
                    UtilityClass.SetOrganizationFilter(ref strFilter, ref queryParas, userID, "T_FB_CHARGEAPPLYMASTER");
                }
            }

            if (checkState == "4")
            {
                if (guidStringList != null)
                {
                    q = from ent in q
                        where guidStringList.Contains(ent.CHARGEAPPLYMASTERID)
                        select ent;
                }
            }
            else
            {
                if (checkState != "5")
                {
                    chkState = int.Parse(checkState);
                    q = q.Where(ent => ent.CHECKSTATES == chkState);
                }
            }

            if (!string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, queryParas.ToArray());
            }
            q = q.OrderBy(strOrderBy);

            return q;
        }

        /// <summary>
        /// 查询申请的费用申请。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回申请的费用申请。</returns>
        public IQueryable<V_Money> GetChargeApply(ExecutionConditions conditions)
        {
            var a = from b in GetObjects<T_FB_CHARGEAPPLYDETAIL>().Include("T_FB_CHARGEAPPLYMASTER")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    where b.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == conditions.CheckStates
                    select b;

            #region 添加查询条件

            // 起止时间
            if (conditions.DateFrom != null && conditions.DateTo != null)
            {
                a = a.Where(b => b.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH >= conditions.DateFrom && b.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH <= conditions.DateTo);
            }
            // 科目
            if (conditions.SubjectID != string.Empty)
            {
                string strTempString = "T_FB_SUBJECT.SUBJECTID==@0 ";
                List<object> objs = new List<object>();
                objs.Add(conditions.SubjectID);
                a = a.Where(strTempString, objs.ToArray());
            }
            // 机构
            if (conditions.OrgnizationType != -1)
            {
                string strTempString = "";
                List<object> objs = new List<object>();

                if (conditions.OrgnizationType == 0)
                {
                    strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERID==@0 ";
                    objs.Add(conditions.OwnerID);
                }

                a = a.Where(strTempString, objs.ToArray());
            }
            #endregion 添加查询条件

            var v = from u in a
                    select new V_Money
                    {
                        Money = u.T_FB_CHARGEAPPLYMASTER.TOTALMONEY
                    };

            return v;
        }

        /// <summary>
        /// 查询费用申请列表。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回费用申请列表。</returns>
        public IQueryable<V_ChargeList> GetChargeApplyList(ExecutionConditions conditions)
        {
            var a = from b in GetObjects<T_FB_CHARGEAPPLYDETAIL>().Include("T_FB_CHARGEAPPLYMASTER")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    where b.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == conditions.CheckStates
                    select b;

            #region 添加查询条件

            // 起止时间
            if (conditions.DateFrom != null && conditions.DateTo != null)
            {
                try
                {
                    a = a.Where(b => b.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH >= conditions.DateFrom && b.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH <= conditions.DateTo);
                }
                catch (Exception e)
                {
                    Tracer.Debug(e.InnerException.Message);
                }
            }

            // 科目
            if (conditions.SubjectID != string.Empty)
            {
                string strTempString = "T_FB_SUBJECT.SUBJECTID==@0 ";
                List<object> objs = new List<object>();
                objs.Add(conditions.SubjectID);
                try
                {
                    a = a.Where(strTempString, objs.ToArray());
                }
                catch (Exception e)
                {
                    Tracer.Debug(e.InnerException.Message);
                }
            }

            // 机构
            if (conditions.OrgnizationType != -1)
            {
                string strTempString = "";
                List<object> objs = new List<object>();

                if (conditions.OrgnizationType == 0)
                {
                    strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "T_FB_CHARGEAPPLYMASTER.OWNERID==@0 ";
                    objs.Add(conditions.CurrentOnlineUser);
                }

                a = a.Where(strTempString, objs.ToArray());
            }

            var t = from u in a
                    select new V_ChargeList
                    {
                        ID = u.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERCODE,
                        SubjectID = u.T_FB_SUBJECT.SUBJECTID,
                        Type = u.T_FB_CHARGEAPPLYMASTER.PAYTYPE,
                        SubjectName = u.T_FB_SUBJECT.SUBJECTNAME,
                        CreateDate = u.T_FB_CHARGEAPPLYMASTER.CREATEDATE,
                        DeptmentID = u.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID,
                        DeptmentName = u.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTNAME,
                        CreateUserID = u.T_FB_CHARGEAPPLYMASTER.CREATEUSERID,
                        CreateUserName = u.T_FB_CHARGEAPPLYMASTER.CREATEUSERNAME,
                        TotalMoney = u.T_FB_CHARGEAPPLYMASTER.TOTALMONEY,
                        BudgetaryMonth = u.T_FB_CHARGEAPPLYMASTER.BUDGETARYMONTH,
                        ChargeType = u.CHARGETYPE.Value,
                        OperateType = "费用申请"
                    };

            return t;

            #endregion 添加查询条件
        }

        public DataTable GetChargeRdList(string strSql)
        {
            if (string.IsNullOrWhiteSpace(strSql))
            {
                return null;
            }

            DataTable dtRes = new DataTable();

            dtRes = this.GetDataTableByCustomerSql(strSql);
            return dtRes;
        }

        public void UpdateBySql(string strSql)
        {
            if (string.IsNullOrWhiteSpace(strSql))
            {
                return;
            }

            this.ExecuteCustomerSql(strSql);
        }
    }
}

