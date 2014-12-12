
/*
 * 文件名：BudgetAccountDAL.cs
 * 作  用：T_FB_BUDGETACCOUNT 数据操作类
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

using SMT_FB_EFModel;
using SMT.FBAnalysis.CustomModel;
using SMT.Foundation.Log;
using System.Configuration;

namespace SMT.FBAnalysis.DAL
{
    public class BudgetAccountDAL : CommDal<T_FB_BUDGETACCOUNT>
    {
        public BudgetAccountDAL()
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
        /// 获取指定条件的T_FB_BUDGETACCOUNT信息
        /// </summary>        
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_BUDGETACCOUNT信息</returns>
        public T_FB_BUDGETACCOUNT GetBudgetAccountRdByMultSearch(string strFilter, params object[] objArgs)
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
        /// 获取指定条件的T_FB_BUDGETACCOUNT信息
        /// </summary>        
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_BUDGETACCOUNT信息</returns>
        public IQueryable<T_FB_BUDGETACCOUNT> GetBudgetAccountRdListByMultSearch(string strOrderBy, string strFilter, params object[] objArgs)
        {
            var q = from v in GetObjects().Include("T_FB_SUBJECT")
                    select v;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }

            return q.OrderBy(strOrderBy);
        }

        /// <summary>
        /// 获取指定条件的T_FB_BUDGETACCOUNT信息   add by zl
        /// </summary>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="accountType"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public IQueryable<T_FB_BUDGETACCOUNT> GetBudgetAccountRdListByMultSearch(string strOrderBy, string strFilter, string accountType, params object[] objArgs)
        {
            int accType;
            var q = from v in GetObjects().Include("T_FB_SUBJECT")
                    select v;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }

            if (!string.IsNullOrEmpty(accountType))
            {
                accType = int.Parse(accountType);
                q = q.Where(ent => ent.ACCOUNTOBJECTTYPE == accType);
            }

            return q.OrderBy(strOrderBy);
        }

        /// <summary>
        /// 查询与个人有关的所有可用预算（个人和部门公共部门)
        /// </summary>
        /// <param name="strOwnerID">员工ID</param>
        /// <param name="strOwnerPostID">员工所在岗位ID</param>
        /// <param name="strOwnerDepID">员工所在部门ID</param>
        /// <param name="strOwnerCompanyID">员工所在公司</param>
        /// <returns></returns>
        //public List<T_FB_BUDGETACCOUNT> GetBudgetAccountByPerson(string strOwnerID, string strOwnerPostID, string strOwnerCompanyID)
        //{
        //    IQueryable<T_FB_SUBJECTPOST> qSubjectPost = from t in GetObjects<T_FB_SUBJECTPOST>().Include("T_FB_SUBJECT")
        //                                                where t.OWNERCOMPANYID == strOwnerCompanyID && t.OWNERPOSTID == strOwnerPostID && t.ACTIVED == 1m //&&t.LIMITBUDGEMONEY>0
        //                                                select t;

        //    IQueryable<T_FB_BUDGETACCOUNT> qAccount = from a in GetObjects().Include("T_FB_SUBJECT")
        //                                              where a.ACTUALMONEY>0
        //                                              select a;

        //    // 个人预算
        //    var personResult = (from item in qAccount
        //                        where item.ACCOUNTOBJECTTYPE == (int)AccountObjectType.Person && item.OWNERID == strOwnerID
        //                        && item.OWNERPOSTID == strOwnerPostID
        //                        select item);
        //    // 部门的预算
        //    var departResult = (from item2 in qAccount
        //                        join item3 in qSubjectPost
        //                        on new { item2.T_FB_SUBJECT.SUBJECTID, item2.OWNERDEPARTMENTID } equals new { item3.T_FB_SUBJECT.SUBJECTID, item3.OWNERDEPARTMENTID }
        //                        where item2.ACCOUNTOBJECTTYPE == (int)AccountObjectType.Deaprtment
        //                        select item2);
        //    // 汇总
        //    var resultTemp = personResult.Union(departResult);

        //    // 在设置中可以用的科目，但在总账中没有记录的预算
        //    var newPostSubject = from item in qSubjectPost
        //                         where !(
        //                             from item2 in resultTemp
        //                             select item2.T_FB_SUBJECT.SUBJECTID).Contains(item.T_FB_SUBJECT.SUBJECTID)
        //                         select new { item.T_FB_SUBJECT, item.OWNERCOMPANYID, item.T_FB_SUBJECTDEPTMENT.T_FB_SUBJECTCOMPANY.ISMONTHLIMIT };
        //    List<T_FB_BUDGETACCOUNT> result = new List<T_FB_BUDGETACCOUNT>();

        //    // 如果科目报销不受月度预算控制时，可用额度为999999.
        //    var views = from item in resultTemp
        //                join itemCom in GetObjects<T_FB_SUBJECTCOMPANY>()
        //                on new { item.T_FB_SUBJECT.SUBJECTID, item.OWNERCOMPANYID } equals new { itemCom.T_FB_SUBJECT.SUBJECTID, itemCom.OWNERCOMPANYID }
        //                select new { item, itemCom, item.T_FB_SUBJECT };

        //    foreach (var view in views)
        //    {
        //        if (view.itemCom.ISMONTHLIMIT < 1)
        //        {
        //            view.item.USABLEMONEY = 999999;
        //        }
        //        view.item.T_FB_SUBJECT = view.T_FB_SUBJECT;
        //        result.Add(view.item);
        //    }
        //    newPostSubject.ToList().ForEach(item =>
        //    {
        //        //去年受月度预算控制
        //        if (item.ISMONTHLIMIT == 0)
        //        {
        //            result.Add(
        //                new T_FB_BUDGETACCOUNT
        //                {
        //                    BUDGETACCOUNTID = Guid.NewGuid().ToString(),
        //                    OWNERCOMPANYID = item.OWNERCOMPANYID,
        //                    T_FB_SUBJECT = item.T_FB_SUBJECT,
        //                    ACCOUNTOBJECTTYPE = (int)AccountObjectType.Deaprtment,
        //                    USABLEMONEY = item.ISMONTHLIMIT == 0 ? 999999 : 0,
        //                    ACTUALMONEY = 0,
        //                    BUDGETYEAR = DateTime.Now.Year,
        //                    BUDGETMONTH = DateTime.Now.Month
        //                }
        //            );
        //        }
        //    });

        //    result.ForEach(item =>
        //    {
        //        item.T_FB_SUBJECT.T_FB_SUBJECTPOST.Clear();
        //        item.T_FB_SUBJECT.T_FB_SUBJECTCOMPANY.Clear();
        //        item.T_FB_SUBJECT.T_FB_SUBJECTDEPTMENT.Clear();
        //    });
        //    result = result.OrderBy(item => item.T_FB_SUBJECT.SUBJECTCODE).ToList();
        //    return result;
        //}


        /// <summary>
        /// 查询与个人有关的所有可用预算（个人和部门公共部门)
        /// </summary>
        /// <param name="strOwnerID">员工ID</param>
        /// <param name="strOwnerPostID">员工所在岗位ID</param>
        /// <param name="strOwnerDepID">员工所在部门ID</param>
        /// <param name="strOwnerCompanyID">员工所在公司</param>
        /// <returns></returns>
        public List<T_FB_BUDGETACCOUNT> GetBudgetAccountByPerson_old(string strOwnerID, string strOwnerPostID, string strOwnerCompanyID)
        {
            IQueryable<T_FB_SUBJECTPOST> qSubjectPost = from t in GetObjects<T_FB_SUBJECTPOST>().Include("T_FB_SUBJECT").Include("T_FB_SUBJECTDEPTMENT").Include("T_FB_SUBJECTDEPTMENT.T_FB_SUBJECTCOMPANY")
                                                        where t.OWNERCOMPANYID == strOwnerCompanyID && t.OWNERPOSTID == strOwnerPostID && t.ACTIVED == 1m //&&t.LIMITBUDGEMONEY>0
                                                        select t;

            IQueryable<T_FB_BUDGETACCOUNT> qAccount = from a in GetObjects().Include("T_FB_SUBJECT")
                                                      //where a.ACTUALMONEY > 0
                                                      select a;

            // 个人预算
            var personResult = (from item in qAccount
                                where item.ACCOUNTOBJECTTYPE == (int)AccountObjectType.Person && item.OWNERID == strOwnerID
                                && item.OWNERPOSTID == strOwnerPostID
                                select item);
            // 部门的预算
            var departResult = (from item2 in qAccount
                                join item3 in qSubjectPost
                                on new { item2.T_FB_SUBJECT.SUBJECTID, item2.OWNERDEPARTMENTID } equals new { item3.T_FB_SUBJECT.SUBJECTID, item3.OWNERDEPARTMENTID }
                                where item2.ACCOUNTOBJECTTYPE == (int)AccountObjectType.Deaprtment
                                select item2);
            // 汇总
            var resultTemp = personResult.Union(departResult);

            // 在设置中可以用的科目，但在总账中没有记录的预算
            var newPostSubject = from item in qSubjectPost
                                 where !(
                                     from item2 in resultTemp
                                     select item2.T_FB_SUBJECT.SUBJECTID).Contains(item.T_FB_SUBJECT.SUBJECTID)
                                 select new { item.T_FB_SUBJECT, item.OWNERCOMPANYID, item.T_FB_SUBJECTDEPTMENT.T_FB_SUBJECTCOMPANY.ISMONTHLIMIT };
            List<T_FB_BUDGETACCOUNT> result = new List<T_FB_BUDGETACCOUNT>();

            // 如果科目报销不受月度预算控制时，可用额度为99999999.
            var views = from item in resultTemp
                        join itemCom in GetObjects<T_FB_SUBJECTCOMPANY>()
                        on new { item.T_FB_SUBJECT.SUBJECTID, item.OWNERCOMPANYID } equals new { itemCom.T_FB_SUBJECT.SUBJECTID, itemCom.OWNERCOMPANYID }
                        select new { item, itemCom, item.T_FB_SUBJECT };

            foreach (var view in views)
            {
                if (view.itemCom.ISMONTHLIMIT < 1)
                {
                    view.item.USABLEMONEY = 99999999;
                }
                view.item.T_FB_SUBJECT = view.T_FB_SUBJECT;

                //if(view.item.USABLEMONEY<0)
                //{
                //    if (view.item.ACCOUNTOBJECTTYPE == 2) //部门
                //    {
                //      //查出所有审核中的部门单据
                //    }
                //    if (view.item.ACCOUNTOBJECTTYPE == 3) //个人
                //    {
                //        //查出所有审核中的个人单据
                //    }
                //    decimal i = 0;//审核中的金额
                //    //计算可用额度
                //    view.item.USABLEMONEY = view.item.BUDGETMONEY - view.item.PAIEDMONEY -i;
                //}

                result.Add(view.item);
            }
            newPostSubject.ToList().ForEach(item =>
            {
                //去年受月度预算控制
                if (item.ISMONTHLIMIT == 0)
                {
                    result.Add(
                        new T_FB_BUDGETACCOUNT
                        {
                            BUDGETACCOUNTID = Guid.NewGuid().ToString(),
                            OWNERCOMPANYID = item.OWNERCOMPANYID,
                            T_FB_SUBJECT = item.T_FB_SUBJECT,
                            ACCOUNTOBJECTTYPE = (int)AccountObjectType.Deaprtment,
                            USABLEMONEY = item.ISMONTHLIMIT == 0 ? 99999999 : 0,
                            ACTUALMONEY = 0,
                            BUDGETYEAR = DateTime.Now.Year,
                            BUDGETMONTH = DateTime.Now.Month
                        }
                    );
                }
            });

            result.ForEach(item =>
            {
                item.T_FB_SUBJECT.T_FB_SUBJECTPOST.Clear();
                item.T_FB_SUBJECT.T_FB_SUBJECTCOMPANY.Clear();
                item.T_FB_SUBJECT.T_FB_SUBJECTDEPTMENT.Clear();
            });
            result = result.OrderBy(item => item.T_FB_SUBJECT.SUBJECTCODE).ToList();
            //zl 11.22  科目维护启用个人后，仅可在分配额中核销，不可同时使用部门预算

            List<T_FB_BUDGETACCOUNT> entTemps = new List<T_FB_BUDGETACCOUNT>();

            entTemps.AddRange(result);
            for (int n = 0; n < entTemps.Count(); n++)
            {
                foreach (T_FB_BUDGETACCOUNT obj in personResult)
                {
                    if (entTemps[n].T_FB_SUBJECT.SUBJECTID == obj.T_FB_SUBJECT.SUBJECTID && entTemps[n].ACCOUNTOBJECTTYPE != obj.ACCOUNTOBJECTTYPE)
                    {
                        if (entTemps[n].ACCOUNTOBJECTTYPE == (int)AccountObjectType.Deaprtment)
                        {
                            result.Remove(entTemps[n]);
                        }
                    }
                }
            }

            if (result.Count() > 0)
            {
                //result = result.Where(t => t.ACTUALMONEY > 0).ToList();   //zl 2012.2.18
                result = result.Where(t => t.T_FB_SUBJECT.SUBJECTID != "00161652-e3bf-4e9f-9a57-a9e1ff8cef74").ToList();
            }
           
            //end

            return result;
        }

        /// <summary>
        /// 查询与个人有关的所有可用预算（个人和部门公共部门)
        /// </summary>
        /// <param name="strOwnerID">员工ID</param>
        /// <param name="strOwnerPostID">员工所在岗位ID</param>
        /// <param name="strOwnerDepID">员工所在部门ID</param>
        /// <param name="strOwnerCompanyID">员工所在公司</param>
        /// <returns></returns>
        public List<T_FB_BUDGETACCOUNT> GetBudgetAccountByPerson(string strOwnerID, string strOwnerPostID, string strOwnerCompanyID)
        {
            try
            {
                var sps = GetObjects<T_FB_SUBJECTPOST>();
                var scs = GetObjects<T_FB_SUBJECTCOMPANY>();
                var ss = GetObjects<T_FB_SUBJECT>();
                var qSubjectPost = (
                    from sp in sps
                    from sc in scs
                    from s in ss
                    where (s.SUBJECTID == sc.T_FB_SUBJECT.SUBJECTID && sc.OWNERCOMPANYID == strOwnerCompanyID)
                       && (s.SUBJECTID == sp.T_FB_SUBJECT.SUBJECTID && sp.OWNERPOSTID == strOwnerPostID && sp.ACTIVED == 1)
                    select new { subject = s, sp.ISPERSON, sc.ISMONTHLIMIT, sp.OWNERDEPARTMENTID }).ToList();
     
            // 如果没有可用的岗位
            if (qSubjectPost.Count == 0)
            {
                return new List<T_FB_BUDGETACCOUNT>();
            }
            var ownerDepartmentID = qSubjectPost.FirstOrDefault().OWNERDEPARTMENTID;
            var tempAccount = from item in GetObjects()
                          where 
                          (
                                (item.ACCOUNTOBJECTTYPE == (int)AccountObjectType.Person && item.OWNERID == strOwnerID && item.OWNERPOSTID == strOwnerPostID)
                                || (item.ACCOUNTOBJECTTYPE == (int)AccountObjectType.Deaprtment && item.OWNERDEPARTMENTID == ownerDepartmentID)
                          )
                          select new {account =item, SubjectID=item.T_FB_SUBJECT.SUBJECTID};
            var account = tempAccount.ToList();

            List<T_FB_BUDGETACCOUNT> result = new List<T_FB_BUDGETACCOUNT>();
            qSubjectPost.ForEach(itemSP =>
                {
                    T_FB_BUDGETACCOUNT temp = null;
                    var temps = account.Where(item => item.SubjectID == itemSP.subject.SUBJECTID)
                        .Select(item => item.account).OrderByDescending(item => item.ACCOUNTOBJECTTYPE).ToList();
                    if (itemSP.ISPERSON == 0)//取部门公用的费用
                    {
                        temps.RemoveAll(itemT => itemT.ACCOUNTOBJECTTYPE == (int)AccountObjectType.Person);
                    }
                    else if (itemSP.ISPERSON == 1)//只取分配到岗位上的费用
                    {
                        temps.RemoveAll(itemT => itemT.ACCOUNTOBJECTTYPE == (int)AccountObjectType.Deaprtment);
                    }
                    
                    temp = temps.FirstOrDefault();
                    
                    if (itemSP.ISMONTHLIMIT < 1)
                    {
                        if (temp == null)
                        {
                            temp = new T_FB_BUDGETACCOUNT
                            {
                                BUDGETACCOUNTID = Guid.NewGuid().ToString(),
                                OWNERCOMPANYID = strOwnerCompanyID,
                                ACCOUNTOBJECTTYPE = (int)AccountObjectType.Deaprtment,
                                USABLEMONEY = 0,
                                ACTUALMONEY = 0,
                                BUDGETYEAR = DateTime.Now.Year,
                                BUDGETMONTH = DateTime.Now.Month
                            };
                        }
                        temp.USABLEMONEY = 99999999;
                    }
                    if (temp != null)
                    {
                        temp.T_FB_SUBJECT = itemSP.subject;
                        result.Add(temp);
                    }
                });
          
            result.ForEach(item =>
            {
                item.T_FB_SUBJECT.T_FB_SUBJECTPOST.Clear();
                item.T_FB_SUBJECT.T_FB_SUBJECTCOMPANY.Clear();
                item.T_FB_SUBJECT.T_FB_SUBJECTDEPTMENT.Clear();
            });
            if (ConfigurationManager.AppSettings["isHX"] != "1")
            {
                result.RemoveAll(t => t.T_FB_SUBJECT.SUBJECTID == "00161652-e3bf-4e9f-9a57-a9e1ff8cef74");
            }
            result = result.OrderBy(item => item.T_FB_SUBJECT.SUBJECTCODE).ToList();
            
            return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 计算年度预算总额。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回年度预算总额。</returns>
        public List<V_Money> GetBudgetAccountAndCheckMoney(ExecutionConditions conditions)
        {
            #region 预算总账
            var a = from b in GetObjects().Include("T_FB_SUBJECT")
                    join c in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
                    select b;

            #region 添加查询条件
            // 起止时间
            if (conditions.DateFrom != null && conditions.DateTo != null)
            {
                try
                {
                    a = a.Where(b => b.BUDGETYEAR >= conditions.DateFrom.Year && b.BUDGETYEAR <= conditions.DateTo.Year && b.BUDGETMONTH >= conditions.DateFrom.Month && b.BUDGETMONTH <= conditions.DateTo.Month);
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
                    strTempString = "OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "OWNERID==@0 ";
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
            // 科目
            if (conditions.SubjectID != string.Empty)
            {
                try
                {
                    string strTempString = "T_FB_SUBJECT.SUBJECTID==@0";
                    List<object> objs = new List<object>();
                    objs.Add(conditions.SubjectID);

                    a = a.Where(strTempString, objs.ToArray());
                }
                catch (Exception e)
                {
                    Tracer.Debug(e.InnerException.Message);

                }
            }
            #endregion 添加查询条件

            var t1 = from d in a
                     select new V_Money
                     {
                         Money = d.BUDGETMONEY.Value
                     };

            #endregion 预算总账

            #region 预算结算单
            var x = from u in GetObjects<T_FB_BUDGETCHECK>()
                    join d in GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on u.T_FB_SUBJECT.SUBJECTID equals d.T_FB_SUBJECT.SUBJECTID
                    select u;

            #region 添加查询条件
            // 起止时间
            if (conditions.DateFrom != null && conditions.DateTo != null)
            {
                try
                {
                    x = x.Where(b => b.BUDGETYEAR >= conditions.DateFrom.Year && b.BUDGETYEAR <= conditions.DateTo.Year && b.BUDGETMONTH >= conditions.DateFrom.Month && b.BUDGETMONTH <= conditions.DateTo.Month);
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
                    strTempString = "OWNERCOMPANYID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 1)
                {
                    strTempString = "OWNERDEPARTMENTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 2)
                {
                    strTempString = "OWNERPOSTID==@0 ";
                    objs.Add(conditions.OrgnizationID);
                }
                else if (conditions.OrgnizationType == 3)
                {
                    strTempString = "OWNERID==@0 ";
                    objs.Add(conditions.OwnerID);
                }

                try
                {
                    x = x.Where(strTempString, objs.ToArray());
                }
                catch (Exception e)
                {
                    Tracer.Debug(e.InnerException.Message);

                }
            }
            // 科目
            if (conditions.SubjectID != string.Empty)
            {
                try
                {
                    string strTempString = "T_FB_SUBJECT.SUBJECTID==@0";
                    List<object> objs = new List<object>();
                    objs.Add(conditions.SubjectID);
                    x = x.Where(strTempString, objs.ToArray());
                }
                catch (Exception e)
                {
                    Tracer.Debug(e.InnerException.Message);

                }
            }
            #endregion 添加查询条件

            var t2 = from u in x
                     select new V_Money
                     {
                         Money = u.BUDGETMONEY.Value
                     };

            #endregion 预算结算单

            return t1.Concat(t2).ToList();
        }
    }
}

