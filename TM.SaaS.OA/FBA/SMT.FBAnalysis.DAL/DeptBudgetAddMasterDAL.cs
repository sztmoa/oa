
/*
 * 文件名：DeptBudgetAddMasterDAL.cs
 * 作  用：T_FB_DEPTBUDGETADDMASTER 数据操作类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 9:52:18
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

namespace SMT.FBAnalysis.DAL
{
    public class DeptBudgetAddMasterDAL:CommDal< T_FB_DEPTBUDGETADDMASTER >
    {
        public DeptBudgetAddMasterDAL()
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
        /// 获取指定条件的T_FB_DEPTBUDGETADDMASTER信息
        /// </summary>        
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_DEPTBUDGETADDMASTER信息</returns>
        public T_FB_DEPTBUDGETADDMASTER GetDeptBudgetAddMasterRdByMultSearch(string strFilter, params object[] objArgs)
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
        /// 获取指定条件的T_FB_DEPTBUDGETADDMASTER信息
        /// </summary>        
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_DEPTBUDGETADDMASTER信息</returns>
        public IQueryable< T_FB_DEPTBUDGETADDMASTER > GetDeptBudgetAddMasterRdListByMultSearch(string strOrderBy, string strFilter, params object[] objArgs)
        {
            var q = from v in GetObjects()
                    select v;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }


            return q.OrderBy(strOrderBy);
        }

        /// <summary>
        /// 计算批复月度预算(增补)。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回批复月度预算(增补)金额。</returns>
        public IQueryable<V_Money> GetDeptBudgetAdd(ExecutionConditions conditions)
        {
            var a = from b in GetObjects<T_FB_DEPTBUDGETADDDETAIL>().Include("T_FB_DEPTBUDGETADDMASTER")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    where b.T_FB_DEPTBUDGETADDMASTER.CHECKSTATES == conditions.CheckStates
                    select b;

            #region 添加查询条件
            StringBuilder filter = new StringBuilder();
            // 起止时间
            if (conditions.DateFrom != null && conditions.DateTo != null)
            {
                try
                {
                    a = a.Where(b => b.T_FB_DEPTBUDGETADDMASTER.BUDGETARYMONTH >= conditions.DateFrom && b.T_FB_DEPTBUDGETADDMASTER.BUDGETARYMONTH <= conditions.DateTo);
                }
                catch (Exception)
                {
                    // WriteLog
                    throw;
                }                
            }
            // 科目
            if (conditions.SubjectID != string.Empty)
            {
                try
                {
                    string strTempString = "T_FB_SUBJECT.SUBJECTID==@0 ";
                    List<object> objs = new List<object>();
                    objs.Add(conditions.SubjectID);
                    a = a.Where(strTempString, objs.ToArray());
                }
                catch (Exception)
                {
                    // WriteLog
                    throw;
                }                 
            }
            // 机构
            if (conditions.OrgnizationType != -1)
            {
                string strTempString = "";
                List<object> objs = new List<object>();

                if (conditions.OrgnizationType == 0)
                {
                    strTempString = "T_FB_DEPTBUDGETADDMASTER.OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "T_FB_DEPTBUDGETADDMASTER.OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "T_FB_DEPTBUDGETADDMASTER.OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "T_FB_DEPTBUDGETADDMASTER.OWNERID==@0 ";
                    objs.Add(conditions.OwnerID);
                }

                try
                {
                    a = a.Where(strTempString, objs.ToArray());
                }
                catch (Exception)
                {
                    // WriteLog
                    throw;
                }
            }

            #endregion 添加查询条件

            var v = from u in a
                    select new V_Money
                    {
                        Money = u.BUDGETMONEY
                    };

            return v;
        }

        /// <summary>
        /// 查询年度预算申请单列表（查询条件：机构ID[公司、部门、岗位]、起止时间、项目）。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回查询结果列表。</returns>
        public IQueryable<V_BudgetMonthList> GetDeptBudgetAddList(ExecutionConditions conditions)
        {
            var a = from b in GetObjects<T_FB_DEPTBUDGETADDDETAIL>().Include("T_FB_DEPTBUDGETADDMASTER")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    where b.T_FB_DEPTBUDGETADDMASTER.CHECKSTATES == conditions.CheckStates
                    where b.T_FB_DEPTBUDGETADDMASTER.BUDGETARYMONTH >= conditions.DateFrom && b.T_FB_DEPTBUDGETADDMASTER.BUDGETARYMONTH <= conditions.DateTo
                    where b.T_FB_SUBJECT.SUBJECTID == conditions.SubjectID
                    select b;

            #region 添加查询条件

            // 机构
            if (conditions.OrgnizationType != -1)
            {
                string strTempString = "";
                List<object> objs = new List<object>();

                if (conditions.OrgnizationType == 0)
                {
                    strTempString = "T_FB_DEPTBUDGETADDMASTER.OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "T_FB_DEPTBUDGETADDMASTER.OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "T_FB_DEPTBUDGETADDMASTER.OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "T_FB_DEPTBUDGETADDMASTER.OWNERID==@0 ";
                    objs.Add(conditions.CurrentOnlineUser);
                }

                a = a.Where(strTempString, objs.ToArray());
            }

            var t = from u in a
                    select new V_BudgetMonthList
                    {
                        ID = u.T_FB_DEPTBUDGETADDMASTER.DEPTBUDGETADDMASTERCODE,
                        SubjectID = u.T_FB_SUBJECT.SUBJECTID,
                        Type = 0,
                        SubjectName = u.T_FB_SUBJECT.SUBJECTNAME,
                        CreateDate = u.T_FB_DEPTBUDGETADDMASTER.CREATEDATE,
                        DeptmentID = u.T_FB_DEPTBUDGETADDMASTER.OWNERDEPARTMENTID,
                        DeptmentName = u.T_FB_DEPTBUDGETADDMASTER.OWNERDEPARTMENTNAME,
                        CreateUserID = u.T_FB_DEPTBUDGETADDMASTER.CREATEUSERID,
                        CreateUserName = u.T_FB_DEPTBUDGETADDMASTER.CREATEUSERNAME,
                        TotalMoney = u.T_FB_DEPTBUDGETADDMASTER.BUDGETCHARGE.Value,
                        BudgetaryMonth = u.T_FB_DEPTBUDGETADDMASTER.CREATEDATE,
                        ChargeType = 0,
                        OperateType = "预算增补"
                    };

            return t;

            #endregion 添加查询条件
        }
    }
}

