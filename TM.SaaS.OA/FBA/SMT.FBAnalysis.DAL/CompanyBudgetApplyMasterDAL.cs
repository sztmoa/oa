
/*
 * 文件名：CompanyBudgetApplyMasterDAL.cs
 * 作  用：T_FB_COMPANYBUDGETAPPLYMASTER 数据操作类
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

using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.CustomModel;

namespace SMT.FBAnalysis.DAL
{
    public class CompanyBudgetApplyMasterDAL : CommDal<T_FB_COMPANYBUDGETAPPLYMASTER>
    {
        public CompanyBudgetApplyMasterDAL()
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
        /// 获取指定条件的T_FB_COMPANYBUDGETAPPLYMASTER信息
        /// </summary>        
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_COMPANYBUDGETAPPLYMASTER信息</returns>
        public T_FB_COMPANYBUDGETAPPLYMASTER GetCompanyBudgetApplyMasterRdByMultSearch(string strFilter, params object[] objArgs)
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
        /// 获取指定条件的T_FB_COMPANYBUDGETAPPLYMASTER信息
        /// </summary>        
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_COMPANYBUDGETAPPLYMASTER信息</returns>
        public IQueryable<T_FB_COMPANYBUDGETAPPLYMASTER> GetCompanyBudgetApplyMasterRdListByMultSearch(string strOrderBy, string strFilter, params object[] objArgs)
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
        /// 查询年度预算申请单列表（查询条件：机构ID[公司、部门、岗位]、起止时间、项目）。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回查询结果列表。</returns>
        public IQueryable<V_BudgetYearList> GetCompanyBudgetApplyList(ExecutionConditions conditions)
        {
            var a = from b in GetObjects<T_FB_COMPANYBUDGETAPPLYDETAIL>().Include("T_FB_COMPANYBUDGETAPPLYMASTER")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    where b.T_FB_COMPANYBUDGETAPPLYMASTER.CHECKSTATES == conditions.CheckStates
                    where b.T_FB_COMPANYBUDGETAPPLYMASTER.BUDGETYEAR >= conditions.DateFrom.Year && b.T_FB_COMPANYBUDGETAPPLYMASTER.BUDGETYEAR <= conditions.DateTo.Year
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
                    strTempString = "T_FB_COMPANYBUDGETAPPLYMASTER.OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "T_FB_COMPANYBUDGETAPPLYMASTER.OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "T_FB_COMPANYBUDGETAPPLYMASTER.OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "T_FB_COMPANYBUDGETAPPLYMASTER.OWNERID==@0 ";
                    objs.Add(conditions.OwnerID);
                }

                a = a.Where(strTempString, objs.ToArray());
            }

            var t = from u in a
                    select new V_BudgetYearList
                    {
                        ID = u.T_FB_COMPANYBUDGETAPPLYMASTER.COMPANYBUDGETAPPLYMASTERCODE,
                        SubjectID = u.T_FB_SUBJECT.SUBJECTID,
                        Type = 0,
                        SubjectName = u.T_FB_SUBJECT.SUBJECTNAME,
                        CreateDate = u.T_FB_COMPANYBUDGETAPPLYMASTER.CREATEDATE,
                        DeptmentID = u.T_FB_COMPANYBUDGETAPPLYMASTER.OWNERDEPARTMENTID,
                        DeptmentName = u.T_FB_COMPANYBUDGETAPPLYMASTER.OWNERDEPARTMENTNAME,
                        CreateUserID = u.T_FB_COMPANYBUDGETAPPLYMASTER.CREATEUSERID,
                        CreateUserName = u.T_FB_COMPANYBUDGETAPPLYMASTER.CREATEUSERNAME,
                        TotalMoney = u.T_FB_COMPANYBUDGETAPPLYMASTER.BUDGETMONEY.Value,
                        BudgetaryMonth = u.T_FB_COMPANYBUDGETAPPLYMASTER.CREATEDATE,
                        ChargeType = 0
                    };

            return t;

            #endregion 添加查询条件
        }
    }
}

