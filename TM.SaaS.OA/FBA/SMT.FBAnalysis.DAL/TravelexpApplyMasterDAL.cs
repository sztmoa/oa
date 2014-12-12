
/*
 * 文件名：TravelexpApplyMasterDAL.cs
 * 作  用：T_FB_TRAVELEXPAPPLYMASTER 数据操作类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 9:52:25
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

namespace SMT.FBAnalysis.DAL
{
    public class TravelexpApplyMasterDAL:CommDal<T_FB_TRAVELEXPAPPLYMASTER>
    {
        public TravelexpApplyMasterDAL()
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
        /// 获取指定条件的T_FB_TRAVELEXPAPPLYMASTER信息
        /// </summary>        
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_TRAVELEXPAPPLYMASTER信息</returns>
        public T_FB_TRAVELEXPAPPLYMASTER GetTravelexpApplyMasterRdByMultSearch(string strFilter, params object[] objArgs)
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
        /// 获取指定条件的T_FB_TRAVELEXPAPPLYMASTER信息
        /// </summary>        
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_TRAVELEXPAPPLYMASTER信息</returns>
        public IQueryable< T_FB_TRAVELEXPAPPLYMASTER > GetTravelexpApplyMasterRdListByMultSearch(string strOrderBy, string strFilter, params object[] objArgs)
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
        /// 查询申请的差旅报销。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回申请的差旅报销费。</returns>
        public IQueryable<V_Money> GetTravelexpApply(ExecutionConditions conditions)
        {
            var a = from b in GetObjects<T_FB_TRAVELEXPAPPLYDETAIL>().Include("T_FB_TRAVELEXPAPPLYMASTER")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    where b.T_FB_TRAVELEXPAPPLYMASTER.CHECKSTATES == conditions.CheckStates
                    select b;

            #region 添加查询条件
            
            // 起止时间
            if (conditions.DateFrom != null && conditions.DateTo != null)
            {
                a = a.Where(c => c.T_FB_TRAVELEXPAPPLYMASTER.BUDGETARYMONTH >= conditions.DateFrom && c.CREATEDATE <= conditions.DateTo);
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
                    strTempString = "T_FB_TRAVELEXPAPPLYMASTER.OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "T_FB_TRAVELEXPAPPLYMASTER.OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "T_FB_TRAVELEXPAPPLYMASTER.OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "T_FB_TRAVELEXPAPPLYMASTER.OWNERID==@0 ";
                    objs.Add(conditions.OwnerID);
                }

                //a = a.Where(orgFilter.ToString());
                a = a.Where(strTempString, objs.ToArray());
            }

            #endregion 添加查询条件

            var v = from u in a
                    select new V_Money
                    {
                        Money = u.TOTALCHARGE
                    };
            
            return v;
        }

        /// <summary>
        /// 查询申请的差旅报销列表。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回申请的差旅报销清单列表。</returns>
        public IQueryable<V_ChargeList> GetTravelexpApplyList(ExecutionConditions conditions)
        {
            var a = from b in GetObjects<T_FB_TRAVELEXPAPPLYDETAIL>().Include("T_FB_TRAVELEXPAPPLYMASTER")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    where b.T_FB_TRAVELEXPAPPLYMASTER.CHECKSTATES == conditions.CheckStates
                    where b.T_FB_TRAVELEXPAPPLYMASTER.BUDGETARYMONTH >= conditions.DateFrom && b.T_FB_TRAVELEXPAPPLYMASTER.BUDGETARYMONTH <= conditions.DateTo
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
                    strTempString = "T_FB_TRAVELEXPAPPLYMASTER.OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "T_FB_TRAVELEXPAPPLYMASTER.OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "T_FB_TRAVELEXPAPPLYMASTER.OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "T_FB_TRAVELEXPAPPLYMASTER.OWNERID==@0 ";
                    objs.Add(conditions.CurrentOnlineUser);
                }

                a = a.Where(strTempString, objs.ToArray());
            }

            var t = from u in a
                    select new V_ChargeList
                    {
                        ID = u.T_FB_TRAVELEXPAPPLYMASTER.TRAVELEXPAPPLYMASTERCODE,
                        SubjectID = u.T_FB_SUBJECT.SUBJECTID,
                        Type = u.T_FB_TRAVELEXPAPPLYMASTER.PAYTYPE,
                        SubjectName = u.T_FB_SUBJECT.SUBJECTNAME,
                        CreateDate = u.T_FB_TRAVELEXPAPPLYMASTER.CREATEDATE,
                        DeptmentID = u.T_FB_TRAVELEXPAPPLYMASTER.OWNERDEPARTMENTID,
                        DeptmentName = u.T_FB_TRAVELEXPAPPLYMASTER.OWNERDEPARTMENTNAME,
                        CreateUserID = u.T_FB_TRAVELEXPAPPLYMASTER.CREATEUSERID,
                        CreateUserName = u.T_FB_TRAVELEXPAPPLYMASTER.CREATEUSERNAME,
                        TotalMoney = u.T_FB_TRAVELEXPAPPLYMASTER.TOTALMONEY,
                        BudgetaryMonth = u.T_FB_TRAVELEXPAPPLYMASTER.BUDGETARYMONTH,
                        ChargeType = u.CHARGETYPE.Value,
                        OperateType = "差旅报销"
                    };

            return t;

            #endregion 添加查询条件
        }
    }
}

