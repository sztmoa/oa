
/*
 * 文件名：BorrowApplyMasterDAL.cs
 * 作  用：T_FB_BORROWAPPLYMASTER 数据操作类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 9:52:14
 * 修改人：
 * 修改时间：
 */

using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data.Objects.DataClasses;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.CustomModel;
using SMT.Foundation.Log;
using SMT.SaaS.BLLCommonServices;


namespace SMT.FBAnalysis.DAL
{
    public class BorrowApplyMasterDAL : CommDal<T_FB_BORROWAPPLYMASTER>
    {
        public static SMT.SaaS.BLLCommonServices.Utility UtilityClass = new SaaS.BLLCommonServices.Utility();

        public BorrowApplyMasterDAL()
        {
        }

        #region
        public bool DelMasterData(List<string> masterList)
        {
            bool IsReturn = true;
            if (masterList != null && masterList.Count() > 0)
            {
                using (TM_SaaS_OA_EFModelContext ex = new TM_SaaS_OA_EFModel.TM_SaaS_OA_EFModelContext())
                {
                    foreach (var x in masterList)
                    {
                        T_FB_BORROWAPPLYMASTER user =
                            ex.T_FB_BORROWAPPLYMASTER.First<T_FB_BORROWAPPLYMASTER>(u => u.BORROWAPPLYMASTERCODE == x);
                        ex.DeleteObject(user);

                        ex.SaveChanges();
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }



        #endregion


        #region 申请主表查询数据
        /// <summary>根据ID查询数据
        /// </summary>
        /// <param name="userId" 当前员工ID></param>
        /// <returns></returns>
        public IQueryable<T_FB_BORROWAPPLYMASTER> GetMasterData(string userId)
        {
            using (TM_SaaS_OA_EFModelContext ex = new TM_SaaS_OA_EFModel.TM_SaaS_OA_EFModelContext())
            {
                var q = from i in ex.T_FB_BORROWAPPLYMASTER
                        where i.OWNERID == userId
                        select i;
                //{
                //   BORROWAPPLYMASTERCODE=i.BORROWAPPLYMASTERCODE,
                //   CHECKSTATES=i.CHECKSTATES,
                //   OWNERNAME=i.OWNERNAME,
                //   OWNERDEPARTMENTNAME=i.OWNERDEPARTMENTNAME,
                //   OWNERCOMPANYNAME=i.OWNERCOMPANYNAME,
                //   CREATEUSERNAME=i.CREATEUSERNAME,
                //   CREATEDATE=i.CREATEDATE,
                //   UPDATEDATE=i.UPDATEDATE
                //};

                if (q != null && q.Count() > 0)
                {
                    //return q.ToList();
                    return null;

                }
                else
                {
                    return null;
                }
            }
        }
        #endregion
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
        /// 获取指定条件的T_FB_BORROWAPPLYMASTER信息
        /// </summary>        
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_BORROWAPPLYMASTER信息</returns>
        public T_FB_BORROWAPPLYMASTER GetBorrowApplyMasterRdByMultSearch(string strFilter, params object[] objArgs)
        {
            var q = from v in GetObjects().Include("T_FB_EXTENSIONALORDER")                    
                    select v;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }

            T_FB_BORROWAPPLYMASTER entRes = q.First();
            if (!entRes.T_FB_BORROWAPPLYDETAIL.IsLoaded)
            {
                entRes.T_FB_BORROWAPPLYDETAIL.Load();
            }

            return entRes;
        }

        /// <summary>
        /// 获取指定条件的T_FB_BORROWAPPLYMASTER信息
        /// </summary>        
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_BORROWAPPLYMASTER信息</returns>
        public IQueryable<T_FB_BORROWAPPLYMASTER> GetBorrowApplyMasterRdListByMultSearch(string strOrderBy, string strFilter,
            string checkState, string userID, List<string> guidStringList, params object[] objArgs)
        {
            int chkState = 5;
            var q = from v in GetObjects()
                    select v;

            List<object> queryParas = new List<object>();
            if (objArgs != null)
            {
                queryParas.AddRange(objArgs);
            }
            if (guidStringList == null)
            {
                if (!(strFilter.IndexOf("OWNERID") > -1))
                {
                    UtilityClass.SetOrganizationFilter(ref strFilter, ref queryParas, userID, "T_FB_BORROWAPPLYMASTER");
                }

            }

            if (checkState == "4")
            {
                if (guidStringList != null)
                {
                    q = from ent in q
                        where guidStringList.Contains(ent.BORROWAPPLYMASTERID)
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
        /// 获取指定条件的借款信息
        /// </summary>
        /// <param name="strCheckStates">查询审核状态</param>
        /// <param name="strDateStart">查询起始时间</param>
        /// <param name="strDateEnd">查询截止时间</param>
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <returns>借款信息</returns>
        public IQueryable<T_FB_BORROWAPPLYMASTER> GetBorrowApplyMasterRdListByMultSearch(string strCheckStates, string strDateStart,
            string strDateEnd, string strOrderBy, string strFilter, params object[] objArgs)
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
        /// 计算报销费用－借款：审批通过的费用申请（借款）。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回审批通过报销费用－借款（审批通过的费用申请（借款））。</returns>
        public IQueryable<V_Money> GetApplyBorrowMoney(ExecutionConditions conditions)
        {
            var a = from b in GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    where b.T_FB_BORROWAPPLYMASTER.CHECKSTATES == conditions.CheckStates
                    select b;

            #region 添加查询条件

            // 起止时间
            if (conditions.DateFrom != null && conditions.DateTo != null)
            {
                try
                {
                    a = a.Where(b => b.T_FB_BORROWAPPLYMASTER.CREATEDATE >= conditions.DateFrom && b.T_FB_BORROWAPPLYMASTER.CREATEDATE <= conditions.DateTo);
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
                    strTempString = "T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OwnerCompanyID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OwnerDepartmentID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "T_FB_BORROWAPPLYMASTER.OWNERPOSTID==@0 ";
                    objs.Add(conditions.OwnerPostID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "T_FB_BORROWAPPLYMASTER.OWNERID==@0 ";
                    objs.Add(conditions.OwnerID);
                }
                try
                {
                    a = a.Where(strTempString, objs.ToArray());
                }
                catch (Exception e)
                {
                    Tracer.Debug(e.InnerException.Message);
                }
            }
            #endregion 添加查询条件

            var v = from u in a
                    select new V_Money
                    {
                        Money = u.T_FB_BORROWAPPLYMASTER.TOTALMONEY
                    };

            return v;
        }

        /// <summary>
        /// 查询报销费用－借款列表。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回借款列表。</returns>
        public IQueryable<V_BorrowList> GetApplyBorrowList(ExecutionConditions conditions)
        {
            var a = from b in GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    where b.T_FB_BORROWAPPLYMASTER.CHECKSTATES == conditions.CheckStates
                    where b.T_FB_BORROWAPPLYMASTER.CREATEDATE >= conditions.DateFrom && b.T_FB_BORROWAPPLYMASTER.CREATEDATE <= conditions.DateTo
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
                    strTempString = "T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "T_FB_BORROWAPPLYMASTER.OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "T_FB_BORROWAPPLYMASTER.OWNERID==@0 ";
                    objs.Add(conditions.CurrentOnlineUser);
                }

                try
                {
                    a = a.Where(strTempString, objs.ToArray());
                }
                catch (Exception e)
                {
                    Tracer.Debug(e.InnerException.Message);
                }
            }

            var t = from u in a
                    select new V_BorrowList
                    {
                        ID = u.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERCODE,
                        SubjectID = u.T_FB_SUBJECT.SUBJECTID,
                        Type = u.T_FB_BORROWAPPLYMASTER.REPAYTYPE,
                        SubjectName = u.T_FB_SUBJECT.SUBJECTNAME,
                        CreateDate = u.T_FB_BORROWAPPLYMASTER.CREATEDATE,
                        DeptmentID = u.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID,
                        DeptmentName = u.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTNAME,
                        CreateUserID = u.T_FB_BORROWAPPLYMASTER.CREATEUSERID,
                        CreateUserName = u.T_FB_BORROWAPPLYMASTER.CREATEUSERNAME,
                        TotalMoney = u.T_FB_BORROWAPPLYMASTER.TOTALMONEY,
                        BudgetaryMonth = u.T_FB_BORROWAPPLYMASTER.CREATEDATE,
                        ChargeType = u.CHARGETYPE.Value,
                        OperateType = "借款"
                    };

            return t;

            #endregion 添加查询条件
        }

        public DataTable GetUnRepayRdList(string strSql)
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

