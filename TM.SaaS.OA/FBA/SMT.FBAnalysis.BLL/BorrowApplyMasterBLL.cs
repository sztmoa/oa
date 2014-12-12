
/*
 * 文件名：BorrowApplyMasterBLL.cs
 * 作  用：T_FB_BORROWAPPLYMASTER 业务逻辑类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 11:47:04
 * 修改人：lezy
 * 修改时间：2011-5-11
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Transactions;
using SMT.Foundation.Core;
using SMT_FB_EFModel;
using SMT.FBAnalysis.DAL;
using SMT.FBAnalysis.CustomModel;
using SMT.Foundation.Log;
using SMT.SaaS.BLLCommonServices.FlowWFService;
using SMT.SaaS.BLLCommonServices.PersonnelWS;


namespace SMT.FBAnalysis.BLL
{
    public class BorrowApplyMasterBLL : BaseBll<T_FB_BORROWAPPLYMASTER>
    {

        public BorrowApplyMasterBLL()
        { }

        #region
        public bool DelMasterData(List<string> masterList)
        {
            if (masterList != null && masterList.Count() > 0)
            {
                try
                {
                    using (SMT_FB_EFModelContext ex = new SMT_FB_EFModel.SMT_FB_EFModelContext())
                    {
                        foreach (var x in masterList)
                        {
                            T_FB_BORROWAPPLYMASTER user =
                                ex.T_FB_BORROWAPPLYMASTER.First<T_FB_BORROWAPPLYMASTER>(u => u.BORROWAPPLYMASTERCODE == x);
                            ex.DeleteObject(user);

                            ex.SaveChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string ErrInfo = this.GetType().ToString() + "：DelMasterData，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                    Tracer.Debug(ErrInfo);
                }
                return true;
            }
            else
            {
                return false;
            }
        }



        #endregion
        
        public IQueryable<T_FB_BORROWAPPLYMASTER> GetBorrowApplyMasterListForRepay(string strOwnerID, decimal dIsRepaied,
            decimal dCheckStates, string strFilter, List<object> objArgs)
        {
            var q = from v in dal.GetObjects()
                    where v.ISREPAIED != dIsRepaied && v.CHECKSTATES == dCheckStates
                    select v;

            if (!string.IsNullOrWhiteSpace(strOwnerID))
            {
                SMT.SaaS.BLLCommonServices.Utility ulFoo = new SaaS.BLLCommonServices.Utility();

                ulFoo.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerID, "T_FB_BORROWAPPLYMASTER");
            }

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs.ToArray());
            }

            return q;
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

            var q = from v in dal.GetObjects()
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
            var q = from v in dal.GetObjects()
                    select v;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }

            return q.First();
        }

        /// <summary>
        /// 获取指定条件的T_FB_BORROWAPPLYMASTER信息
        /// </summary>        
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_BORROWAPPLYMASTER信息</returns>
        //public IQueryable<T_FB_BORROWAPPLYMASTER> GetBorrowApplyMasterRdListByMultSearch(string strFilter, List<object> objArgs,
        //    string strSortKey, int pageIndex, int pageSize, ref int pageCount, string checkState)
        //{
        //    //var q = from v in dal.GetObjects()
        //    //        select v;

        //    //if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
        //    //{
        //    //    q = q.Where(strFilter, objArgs);
        //    //}
        //    //return q.OrderBy(strOrderBy);
        //    var q=
        //}


        /// <summary>
        /// 计算报销费用－借款：审批通过的费用申请（借款）。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回审批通过报销费用－借款（审批通过的费用申请（借款））。</returns>
        public IQueryable<V_Money> GetApplyBorrowMoney(ExecutionConditions conditions)
        {
            try
            {
                var a = from b in dal.GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                        join c in dal.GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
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
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetApplyBorrowMoney，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 查询报销费用－借款列表。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回借款列表。</returns>
        public IQueryable<V_BorrowList> GetApplyBorrowList(ExecutionConditions conditions)
        {
            try
            {
                var a = from b in dal.GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                        join c in dal.GetObjects<T_FB_SUBJECTCOMPANY>().Include("T_FB_SUBJECT") on b.T_FB_SUBJECT.SUBJECTID equals c.T_FB_SUBJECT.SUBJECTID
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
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetApplyBorrowList，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }


        //#region 获取数据

        //#region  获取主表数据
        ///// <summary> 获取主表数据
        ///// </summary>
        ///// <param name="key"，传入的创建人ID></param>
        ///// <returns></returns>
        //public IQueryable<V_BorrowList> GetMasterData(string key)
        //{
        //    IQueryable<V_BorrowList> masterList;
        //   using(BorrowApplyMasterDAL dal = new BorrowApplyMasterDAL())
        //    {
        //        masterList = dal.GetMasterData(key);
        //    }
        //    if ( masterList!=null&&masterList.Count() > 0)
        //    {
        //        return masterList;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        //#endregion
        /// <summary>
        /// 获取T_FB_BORROWAPPLYMASTER信息
        /// </summary>
        /// <param name="strBorrowApplyMasterId">主键索引</param>
        /// <returns></returns>
        public T_FB_BORROWAPPLYMASTER GetBorrowApplyMasterByID(string strBorrowApplyMasterId)
        {
            if (string.IsNullOrEmpty(strBorrowApplyMasterId))
            {
                return null;
            }

            try
            {
                BorrowApplyMasterDAL dalBorrowApplyMaster = new BorrowApplyMasterDAL();
                StringBuilder strFilter = new StringBuilder();
                List<string> objArgs = new List<string>();

                if (!string.IsNullOrEmpty(strBorrowApplyMasterId))
                {
                    strFilter.Append(" BORROWAPPLYMASTERID == @0");
                    objArgs.Add(strBorrowApplyMasterId);
                }

                T_FB_BORROWAPPLYMASTER entRd = dalBorrowApplyMaster.GetBorrowApplyMasterRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
                return entRd;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetBorrowApplyMasterByID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据条件，获取费用报销信息
        /// </summary>
        /// <param name="strOwnerID"></param>
        /// <param name="strDateStart"></param>
        /// <param name="strDateEnd"></param>
        /// <param name="strCheckState"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_BORROWAPPLYMASTER> GetAllBorrowApplyMasterRdListByMultSearch(string strOwnerID, string strDateStart,
            string strDateEnd, string strCheckState, string strFilter, List<object> objArgs, string strSortKey)
        {
            try
            {
                BorrowApplyMasterDAL dalBorrowApplyMaster = new BorrowApplyMasterDAL();
                string strOrderBy = string.Empty;

                if (string.IsNullOrWhiteSpace(strOwnerID) || string.IsNullOrWhiteSpace(strCheckState))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(strSortKey))
                {
                    strOrderBy = strSortKey;
                }
                else
                {
                    strOrderBy = " BORROWAPPLYMASTERID ";
                }

                SMT.SaaS.BLLCommonServices.Utility ulFoo = new SaaS.BLLCommonServices.Utility();
                if (strCheckState != Convert.ToInt32(FBAEnums.CheckStates.WaittingApproval).ToString())
                {
                    ulFoo.SetOrganizationFilter(ref strFilter, ref objArgs, strOwnerID, "T_FB_BORROWAPPLYMASTER");
                }
                else
                {
                    string StrOld = "";
                    StrOld = strFilter;//将过滤前的字符串付给再比较
                    ulFoo.SetFilterWithflow("BORROWAPPLYMASTERID", "T_FB_BORROWAPPLYMASTER", strOwnerID, ref strCheckState, ref strFilter, ref objArgs);
                    if (StrOld.Equals(strFilter))
                    {
                        return null;
                    }
                    strCheckState = Convert.ToInt32(FBAEnums.CheckStates.Approving).ToString();
                }

                if (strCheckState == Convert.ToInt32(FBAEnums.CheckStates.All).ToString())
                {
                    strCheckState = string.Empty;
                }

                var ents = dalBorrowApplyMaster.GetBorrowApplyMasterRdListByMultSearch(strCheckState, strDateStart, strDateEnd, strOrderBy, strFilter, objArgs.ToArray());
                return ents;
            }
            catch (Exception ex)
            {
                Utility.SaveLog(DateTime.Now.ToString() + "调用函数GetAllBorrowApplyMasterRdListByMultSearch出错， 查询人的员工ID为：" + strOwnerID + "，错误信息为：" + ex.ToString());
            }

            return null;
        }

        /// <summary>
        /// 根据条件，获取借款信息,并进行分页
        /// </summary>
        /// <param name="strOwnerID"></param>
        /// <param name="strDateStart"></param>
        /// <param name="strDateEnd"></param>
        /// <param name="strCheckState"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public IQueryable<T_FB_BORROWAPPLYMASTER> GetBorrowApplyMasterRdListByMultSearch(string strOwnerID, string strDateStart,
            string strDateEnd, string strCheckState, string strFilter, List<object> objArgs, string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var ents = GetAllBorrowApplyMasterRdListByMultSearch(strOwnerID, strDateStart, strDateEnd, strCheckState, strFilter,
                objArgs, strSortKey);

            if (ents == null)
            {
                return null;
            }

            if (pageIndex == 0 && pageSize == 0)
            {
                return ents;
            }

            return Utility.Pager<T_FB_BORROWAPPLYMASTER>(ents, pageIndex, pageSize, ref pageCount);
        }

        #region  增删改查

        #region 管理页面查询
        /// <summary>
        /// 根据使用人ID查询35条数据
        /// </summary>
        /// <param name="primaryKey">借款申请表主键ID</param>
        /// <returns>返回主表与相关子表信息</returns>
        public List<T_FB_BORROWAPPLYMASTER> GetMasterData(string employeeID)
        {
            List<T_FB_BORROWAPPLYMASTER> masterList;
            try
            {
                var masterInfo = dal.GetObjects()
                               .Include("T_FB_BORROWAPPLYDETAIL")
                               .Where(x => x.OWNERID == employeeID)
                               .OrderBy(x => x.CREATEDATE)
                               .Select(x => x);
                masterList = masterInfo != null && masterInfo.Count() > 0 ? masterInfo.Take(35).ToList() : null;
            }


            catch (Exception ex)
            {
                Tracer.Debug("个人申请借款BorrowApplyMasterBLL-GetMasterData" +
                              System.DateTime.Now.ToString() + " " + ex.ToString());
                masterList = null;

            }
            return masterList;
        }
        #endregion

        #region 子页面查询
        /// <summary>
        /// 根据主键ID查询主表子表数据
        /// </summary>
        /// <param name="borrowKey">主表主键ID</param>
        /// <returns>与主键相同的一条记录,也是唯一记录</returns>
        public T_FB_BORROWAPPLYMASTER GetChildData(string borrowKey)
        {
            T_FB_BORROWAPPLYMASTER borrowEntity;
            try
            {
                var childInfo = dal.GetObjects()
                              .Include("T_FB_BORROWAPPLYDETAIL")
                              .Where(x => x.BORROWAPPLYMASTERID == borrowKey)
                              .Select(x => x);
                borrowEntity = childInfo.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Tracer.Debug("个人申请借款明细BorrowApplyDetailBLL-UpdBorrowApply" +
                             System.DateTime.Now.ToString() + " " + ex.ToString());
                borrowEntity = null;
            }
            return borrowEntity != null ? borrowEntity : null;
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增个人借款申请
        /// </summary>
        /// <param name="masterKey">主表带上子表信息</param>
        /// <returns>返回bool值表示是否添加成功</returns>
        public bool AddBorrowApply(T_FB_BORROWAPPLYMASTER masterKey)
        {
            bool flag = false;
            try
            {
                int x = dal.Add(masterKey);
                flag = x > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Tracer.Debug("个人申请借款明细BorrowApplyDetailBLL-AddBorrowApply" +
                               System.DateTime.Now.ToString() + " " + ex.ToString());
                flag = false;
            }
            return flag;
        }

        #endregion

        #region  修改
        /// <summary>
        /// 修改个人借款申请
        /// </summary>
        /// <param name="updataKey">要修改的主表数据附带子表信息</param>
        /// <returns>返回bool值表示是否修改成功</returns>
        public bool UpdBorrowApply(T_FB_BORROWAPPLYMASTER updataKey)
        {
            bool flag = false;
            int u = 0;
            try
            {
                var ent = dal.GetObjects()
                        .Where(x => x.BORROWAPPLYMASTERID == updataKey.BORROWAPPLYMASTERID)
                        .Select(x => x).FirstOrDefault();
                if (ent != null)
                {
                    if (updataKey.EntityKey == null)//E  ntityKey EF自动生成的实体键
                    {
                        updataKey.EntityKey = ent.EntityKey;
                    }
                    u = dal.Update(updataKey);
                }
                else
                {
                    flag = false;
                }
                //int u = ent != null && ent.Count() > 0 ? dal.Update(updataKey) : -1;
                flag = u > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Tracer.Debug("个人申请借款明细BorrowApplyDetailBLL-UpdBorrowApply" +
                            System.DateTime.Now.ToString() + " " + ex.ToString());
                flag = false;


            }
            return flag;

        }
        #endregion

        #region 批量删除申请单
        /// <summary>
        ///  根据主键批量删除申请单
        /// </summary>
        /// <param name="masterList">借款申请单ID集合</param>
        /// <returns>返回bool值表示是否删除成功</returns>
        public bool DelMasterDataById(ObservableCollection<string> masterList)
        {
            bool returnType = true;
            try
            {
                if ((masterList != null) && (masterList.Count() > 0))
                {
                    dal.BeginTransaction();
                    foreach (string id in masterList)
                    {
                        var entm = from y in dal.GetObjects()
                                   where y.BORROWAPPLYMASTERID == id
                                   select y;
                        var entd = ((entm != null) && (entm.Count() > 0)) ? entm.FirstOrDefault() : null;
                        if (entd != null)
                        {
                            int x = dal.Delete(entd);
                            returnType = x > 0 ? true : false;
                        }
                        else
                        {

                            returnType = false;
                        }

                    }
                }
                else
                {
                    returnType = false;

                }


                dal.CommitTransaction();
            }

            catch (Exception ex)
            {
                dal.RollbackTransaction();
                Tracer.Debug("个人申请借款BorrowApplyMasterBLL-DelMasterDataById" +
                                System.DateTime.Now.ToString() + " " + ex.ToString());
                returnType = false;

            }
            return returnType;
        }
        #endregion


        #region 写入数据
        /// <summary>
        /// 增加借款主从表数据  add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddBorrowApplyMasterAndDetail(T_FB_BORROWAPPLYMASTER entity, List<T_FB_BORROWAPPLYDETAIL> detailList)
        {
            try
            {
                var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
                if (company != null)
                {
                    entity.OWNERCOMPANYNAME = company.CNAME;
                }
            }
            catch
            {

            }
            bool re;
            try
            { 
                dal.BeginTransaction();
                foreach (T_FB_BORROWAPPLYDETAIL obj in detailList)
                {
                    //添加借款明细
                    Utility.RefreshEntity(obj);
                    entity.T_FB_BORROWAPPLYDETAIL.Add(obj);
                }
                re = Add(entity);
                if (!re)
                {
                    dal.RollbackTransaction();
                    return false;
                }

                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：AddBorrowApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }

        #endregion

        #region 更新数据
        public bool ModifyBorrowApplyMaster(T_FB_BORROWAPPLYMASTER entity)
        {
            try
            {                
                
                T_FB_BORROWAPPLYMASTER entUpt = GetBorrowApplyMasterByID(entity.BORROWAPPLYMASTERID);
                
                Utility.CloneEntity(entity, entUpt);
                if (entity.T_FB_EXTENSIONALORDER != null)
                {
                    entUpt.T_FB_EXTENSIONALORDER.EntityKey =
                        new System.Data.EntityKey("SMT_FB_EFModelContext.T_FB_EXTENSIONALORDER", "RESUMEID", entity.T_FB_EXTENSIONALORDER.EXTENSIONALORDERID);
                }

                if (entUpt.T_FB_BORROWAPPLYDETAIL != null)
                {
                    entUpt.T_FB_BORROWAPPLYDETAIL.Clear();
                }

                //entUpt.UPDATEDATE = DateTime.Now;  //mark zl 11.23
                if(entity.CHECKSTATES!=2)           //add zl 11.23
                {
                    entUpt.UPDATEDATE = DateTime.Now;
                }

                Update(entUpt);

                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptBorrowApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        /// <summary>
        /// 更新借款主从表数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public void UptBorrowApplyMasterAndDetail(string strActionType, T_FB_BORROWAPPLYMASTER entity, 
            List<T_FB_BORROWAPPLYDETAIL> detailList, ref string strMsg)
        {
            try
            {
                var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
                if (company != null)
                {
                    entity.OWNERCOMPANYNAME = company.CNAME;
                }
            }
            catch
            {

            }
            bool re = false;
            if (LockOrder(entity.BORROWAPPLYMASTERID))
            {
                strMsg = "单据正在提交或审核中，不可重复操作！";
                return;
            }           

            try
            {
                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;
                T_FB_BORROWAPPLYMASTER cha = GetBorrowApplyMasterByID(entity.BORROWAPPLYMASTERID);
                if (cha == null)
                {
                    strMsg = "提交的单据不存在或已删除，不可继续操作！";
                    return;
                }

                object checkStatesOld = cha.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());

                object checkStatesNew = entity.CHECKSTATES;
                FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());

                if ((dOldChecksates == FBAEnums.CheckStates.Approved || dOldChecksates == FBAEnums.CheckStates.UnApproved)
                    && strActionType.ToUpper() != "RESUBMIT")
                {
                    strMsg = "单据已审核完毕，不可再次操作";
                    return;
                }

                //不用月结可以进行借还款
                //#region 是否本月有结算

                //bool isChecked = SystemSettingsBLL.IsChecked;
                //// 没月结，只能处理报销。
                //string entityType = entity.GetType().Name;
                //string[] EntityTypes = new string[] { typeof(T_FB_BORROWAPPLYMASTER).Name, 
                //    typeof(T_FB_REPAYAPPLYMASTER).Name, typeof(T_FB_CHARGEAPPLYMASTER).Name};
                //// 月结不可操作：1.日常报销类型的单据的审核或提交，2.所有单据的提交或重新提交
                //if (!isChecked && (EntityTypes.Contains(entityType) || (strActionType.ToUpper() == "SUBMIT")
                //    || (strActionType.ToUpper() == "RESUBMIT")))
                //{
                //    strMsg = "本月尚未结算,无法提交或审核!";
                //    return;
                //}

                //#endregion

                Utility.CloneEntity(entity, cha);
                cha.UPDATEDATE = DateTime.Now;

                bool n = Update(cha);
                if (n == false)
                {
                    strMsg = "单据更新异常！";
                    return;
                }

                BorrowApplyDetailBLL borrowDetailBLL = new BorrowApplyDetailBLL();
                re = borrowDetailBLL.UpdateBorrowApplyDetail(cha.BORROWAPPLYMASTERID, detailList);//删除报销明细
                if (!re)
                {
                    strMsg = "单据明细更新异常！";
                    return;
                }

                if (dOldChecksates == dNewCheckStates && dOldChecksates == (int)FBAEnums.CheckStates.UnSubmit
                    && strActionType.ToUpper() == "EDIT")
                {
                    strMsg = "单据更新成功！";
                    return;
                }
                
                //if (dNewCheckStates == FBAEnums.CheckStates.Approving && ((strActionType.ToUpper() == "SUBMIT" && dOldChecksates == FBAEnums.CheckStates.UnSubmit)
                //    || (strActionType.ToUpper() == "RESUBMIT" && dOldChecksates == FBAEnums.CheckStates.UnApproved)))
                //{
                //    BudgetAccountBLL budgetBLL = new BudgetAccountBLL();
                //    budgetBLL.UpdateAccount(cha, (int)dNewCheckStates);
                //}
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptBorrowApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            finally
            {
                ReleaseOrder(entity.BORROWAPPLYMASTERID);
            }
        }

        /// <summary>
        /// 更新借款主表CHECKSTATES字段值 add by zl    该方法由引擎调用，用来更改主表checkstates状态
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public string UptBorrowApplyCheckState(T_FB_BORROWAPPLYMASTER entity, List<T_FB_BORROWAPPLYDETAIL> detailList)
        {
            bool flag = false;
            string strMsg = string.Empty;
            if (LockOrder(entity.BORROWAPPLYMASTERID))
            {
                strMsg = "单据正在提交或审核中，不可重复操作！";
                return strMsg;
            }
           
            try
            {      
                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;

                object checkStatesNew = entity.CHECKSTATES;
                FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());


                T_FB_BORROWAPPLYMASTER cha = GetBorrowApplyMasterByID(entity.BORROWAPPLYMASTERID);

                if (cha == null)
                {
                    strMsg = entity.BORROWAPPLYMASTERID + "借款单据不存在，不可继续操作！";
                    Tracer.Debug(strMsg);
                    return strMsg;
                }

                object checkStatesOld = cha.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());

                if (dOldChecksates == FBAEnums.CheckStates.Approved && dNewCheckStates == FBAEnums.CheckStates.Approving)
                {
                    strMsg = "已审核通过的费用报销单禁止再次审核!";
                    string ErrInfo = this.GetType().ToString() + "：UptChargeApplyCheckState，" + System.DateTime.Now.ToString() + "，" + strMsg;
                    Tracer.Debug(ErrInfo);
                    return strMsg;
                }

                Utility.CloneEntity(entity, cha);

                //add zl 12.1 提交审核时产生单据号
                dal.BeginTransaction();
                string strCode = "";
                if(string.IsNullOrEmpty(cha.BORROWAPPLYMASTERCODE.Trim()))
                {
                    try
                    {
                        strCode = new OrderCodeBLL().GetAutoOrderCode(entity);
                        cha.BORROWAPPLYMASTERCODE = strCode;
                        string err = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + cha.BORROWAPPLYMASTERID + "：产生单据号 " + strCode;
                        Tracer.Debug(err);
                    }
                    catch (Exception ex)
                    {
                        string sr = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + cha.BORROWAPPLYMASTERID + "：产生单据号时出现异常 " + ex.Message;
                        Tracer.Debug(sr);
                        dal.RollbackTransaction();
                        strMsg = "产生单据号时出现异常！";
                        return strMsg;
                    }
                }
                string Logmsg = this.GetType().ToString() +","+ System.DateTime.Now.ToString() + "：UptBorrowApplyCheckState，表单ID "+cha.BORROWAPPLYMASTERID+",审核状态："+cha.CHECKSTATES;
                Tracer.Debug(Logmsg);
                //add end

                cha.UPDATEDATE = DateTime.Now;
                bool n = Update(cha);
                if (!n)
                {
                    strMsg = "单据明细更新异常！";
                    dal.RollbackTransaction();
                    return strMsg;
                }

                //add zl 12.5
                if(cha.CHECKSTATES == 2)
                {
                    PersonAccountBLL PerBLL = new PersonAccountBLL();
                    n = PerBLL.UptPersonAccountByBorr(cha);
                    if (!n)
                    {
                        strMsg = "更新借还总账表异常！";
                        dal.RollbackTransaction();
                        return strMsg;
                    }
                }
                dal.CommitTransaction();
                //add end

                return strMsg;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptBorrowApplyCheckState，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                strMsg = "更新单据状态出现异常！";
                return strMsg;
            }
            finally
            {
                ReleaseOrder(entity.BORROWAPPLYMASTERID);
            }
        }

        #endregion

        #region 删除数据
        /// <summary>
        /// 删除借款主表数据  add by zl
        /// </summary>
        /// <param name="borrowMasterID"></param>
        /// <returns></returns>
        public bool DelBorrowApplyMaster(string borrowMasterID)
        {
            try
            {
                var entitys = (from ent in dal.GetTable() where ent.BORROWAPPLYMASTERID == borrowMasterID select ent);
                if (entitys.Count() > 0)
                {
                    var entity = entitys.FirstOrDefault();
                    //只删除未提交状态的单据
                    if (entity.CHECKSTATES == 0)
                    {
                        Delete(entity);
                        return true;
                    }
                    else
                    {
                        string ErrInfo = "删除借款单出现错误，单据审核状态为：" + entity.CHECKSTATES + " .表单ID为：" + entity.BORROWAPPLYMASTERID;
                        Tracer.Debug(ErrInfo);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：DelBorrowApplyMaster，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        /// <summary>
        /// 一起删除借款主表和明细表   add by zl
        /// </summary>
        /// <param name="borrowMasterID"></param>
        /// <returns></returns>
        public bool DelBorrowApplyMasterAndDetail(string borrowMasterID)
        {
            try
            {
                BorrowApplyDetailBLL borrowDetailBLL = new BorrowApplyDetailBLL();
                dal.BeginTransaction();
                if (!borrowDetailBLL.DelBorrowApplyDetail(borrowMasterID))
                {
                    dal.RollbackTransaction();
                    return false;
                }
                if (!DelBorrowApplyMaster(borrowMasterID))
                {
                    dal.RollbackTransaction();
                    return false;
                }
                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：DelBorrowApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }


        #endregion


        #region 获取借款申请的信息
        public int GetBorrowApplyForMobile(string BorrowID, string StrCheckState)
        {
            string sResult = string.Empty;
            T_FB_BORROWAPPLYMASTER Master = new T_FB_BORROWAPPLYMASTER();
            Master = GetChildData(BorrowID);
            Master.CHECKSTATES = System.Convert.ToDecimal(StrCheckState);
            List<object> objmaster = new List<object>();
            objmaster.Add(BorrowID);
            List<T_FB_BORROWAPPLYDETAIL> entRdlist = new List<T_FB_BORROWAPPLYDETAIL>();
            BorrowApplyDetailBLL bllBorrowApplyDetail = new BorrowApplyDetailBLL();
            entRdlist = bllBorrowApplyDetail.GetBorrowApplyDetailByMasterID(objmaster);
            if (entRdlist != null)
            {
                sResult = UptBorrowApplyCheckState(Master, entRdlist);
                if (!string.IsNullOrEmpty(sResult))
                {
                    throw new Exception(sResult);
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        }
        #endregion

        #region  转移数据专用 zl
        /// <summary>
        /// 改变借款表状态
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UptBorrowApplyMasterChkSta(T_FB_BORROWAPPLYMASTER entity)
        {
            bool re = true;
            re = Update(entity);
            return re;
        }


        #endregion

        #endregion

        #region 手机版使用
        public bool AddBorrowApplyMasterAndDetailForMobile(T_FB_BORROWAPPLYMASTER entity, List<T_FB_BORROWAPPLYDETAIL> detailList,ref  string strMsg)
        {
            try
            {
                var company = OrgClient.GetCompanyById(entity.OWNERCOMPANYID);
                
                if (!string.IsNullOrEmpty(strMsg))
                {
                    return false;
                }
                if (company != null)
                {
                    entity.OWNERCOMPANYNAME = company.CNAME;
                }
            }
            catch
            {

            }
            Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - 开始" );
            Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - BORROWAPPLYMASTERID" + entity.BORROWAPPLYMASTERID);
            Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - BANK" + entity.BANK);
            Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - BANKACCOUT" + entity.BANKACCOUT);
            if (entity.PAYTARGET != null)
            {
                Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - REPAYTYPE" + entity.REPAYTYPE.ToString());
            }
            if (entity.REPAYTYPE != null)
            {
                Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - REPAYTYPE" + entity.PAYTARGET.ToString());
            }            
            Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile -REMARK " + entity.REMARK);
            bool re;
            re = CheckBorrow(ref entity,ref detailList,ref strMsg,"1");
            if (!re)
            {
                return re;
            }
            try
            {
                dal.BeginTransaction();
                foreach (T_FB_BORROWAPPLYDETAIL obj in detailList)
                {
                    //添加借款明细
                    Utility.RefreshEntity(obj);
                    entity.T_FB_BORROWAPPLYDETAIL.Add(obj);
                }
                re = Add(entity);
                if (!re)
                {
                    dal.RollbackTransaction();
                    Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile -服务回滚 ");
                    return false;
                }

                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：AddBorrowApplyMasterAndDetailForMobile，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }

        /// <summary>
        /// 验证借款申请是否符合要求
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        private bool CheckBorrow(ref T_FB_BORROWAPPLYMASTER entity,ref  List<T_FB_BORROWAPPLYDETAIL> detailList, ref  string strMsg,string operFlag)
        {
            try
            {
                PersonnelServiceClient personClient = new PersonnelServiceClient();
                var employee = personClient.GetEmployeeDetailByID(entity.OWNERID);
                string postID = entity.OWNERPOSTID;
                string employeeID = entity.OWNERID;
                if (employee == null || employee.EMPLOYEEPOSTS == null)
                {
                    strMsg = "申请人已不存在,请重新建单!";
                }
                else
                {
                    var find = employee.EMPLOYEEPOSTS.FirstOrDefault(item => item.T_HR_POST.POSTID == postID);
                    if (find == null)
                    {
                        strMsg = "申请人已异动, 请重新建单!";
                    }
                }
                var entPost = from ent in employee.EMPLOYEEPOSTS
                              where ent.T_HR_POST.POSTID == postID
                              select ent;
                if (entPost.Count() == 0)
                {
                    strMsg = "获取申请人岗位信息时为空!";
                }

                T_HR_EMPLOYEEPOST employeePost = entPost.FirstOrDefault();
                entity.OWNERNAME = employee.T_HR_EMPLOYEE.EMPLOYEECNAME;

                if (employeePost.T_HR_POST != null)
                {
                    if (employeePost.T_HR_POST.T_HR_POSTDICTIONARY != null)
                    {
                        entity.OWNERPOSTNAME = employeePost.T_HR_POST.T_HR_POSTDICTIONARY.POSTNAME;
                        entity.OWNERPOSTID = employeePost.T_HR_POST.POSTID;

                    }
                    else
                    {
                        strMsg = "员工岗位字典信息不存在";
                    }
                }
                else
                {
                    strMsg = "员工岗位信息不存在";
                }
                if (employeePost.T_HR_POST.T_HR_DEPARTMENT != null)
                {
                    if (employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY != null)
                    {
                        entity.OWNERDEPARTMENTNAME = employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                        entity.OWNERDEPARTMENTID = employeePost.T_HR_POST.T_HR_DEPARTMENT.DEPARTMENTID;

                    }
                    else
                    {
                        strMsg = "员工部门字典信息不存在";
                    }
                }
                else
                {
                    strMsg = "员工部门信息不存在";
                }
                if (employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY != null)
                {
                    if (employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.BRIEFNAME != null)
                    {
                        entity.OWNERCOMPANYNAME = employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.BRIEFNAME;

                    }
                    else
                    {
                        entity.OWNERCOMPANYNAME = employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.CNAME;
                    }
                    entity.OWNERCOMPANYID = employeePost.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.COMPANYID;

                }
                else
                {
                    strMsg = "员工对应公司信息不存在";
                }
                Tracer.Debug("验证员工信息-CheckBorrow提示信息：" + strMsg);
                if (!string.IsNullOrEmpty(strMsg))
                {
                    return false;
                }
                if (detailList.Count == 0)
                {
                    strMsg = "借款明细记录不能为空";
                    return false;
                }
                //entity.REPAYTYPE  1：普通借款  2:备用金3：专项借款
                if (entity.REPAYTYPE == 1)
                {
                    if (entity.PAYTARGET == 2 && string.IsNullOrEmpty(entity.PAYMENTINFO))
                    {
                        strMsg = "汇多人账户时请填写支付信息";
                        return false;
                    }
                }
                entity.CHECKSTATES = 0;
                entity.EDITSTATES = 0;
                entity.ISREPAIED = 0;
                if (operFlag == "1")
                {
                    if (string.IsNullOrEmpty(entity.BORROWAPPLYMASTERID))
                    {
                        entity.BORROWAPPLYMASTERID = Guid.NewGuid().ToString();
                    }
                    entity.CREATECOMPANYID = entity.OWNERCOMPANYID;
                    entity.CREATEDEPARTMENTID = entity.OWNERDEPARTMENTID;
                    entity.CREATEPOSTID = entity.OWNERPOSTID;
                    entity.CREATEUSERID = entity.OWNERID;
                    entity.CREATEUSERNAME = entity.OWNERNAME;
                    entity.CREATEDEPARTMENTNAME = entity.OWNERDEPARTMENTNAME;
                    entity.CREATEPOSTNAME = entity.OWNERPOSTNAME;
                    entity.CREATECOMPANYNAME = entity.OWNERCOMPANYNAME;
                    entity.CREATEDATE = DateTime.Now;
                    entity.UPDATEDATE = DateTime.Now;
                    entity.UPDATEUSERID = entity.OWNERID;
                    entity.UPDATEUSERNAME = entity.OWNERNAME;
                }
                else
                {
                    entity.UPDATEDATE = DateTime.Now;
                    entity.UPDATEUSERID = entity.OWNERID;
                    entity.UPDATEUSERNAME = entity.OWNERNAME;
                }
                decimal sumTotal = 0;
                foreach (T_FB_BORROWAPPLYDETAIL detail in detailList)
                {
                    if (string.IsNullOrEmpty(detail.REMARK))
                    {
                        strMsg = "借款明细中摘要不能为空";
                        return false;
                    }
                    decimal i = 0;
                    if (detail.BORROWMONEY == null)
                    {
                        strMsg = "借款明细中金额不能为空";
                        return false;
                    }
                    if (!decimal.TryParse(detail.BORROWMONEY.ToString(), out i))
                    {
                        strMsg = "借款明细中请输入正确的数值！";
                        return false;
                    }

                    if (operFlag == "1")
                    {
                        if (string.IsNullOrEmpty(detail.BORROWAPPLYDETAILID))
                        {
                            detail.BORROWAPPLYDETAILID = Guid.NewGuid().ToString();
                        }
                        if (detail.T_FB_BORROWAPPLYMASTER == null)
                        {
                            detail.T_FB_BORROWAPPLYMASTER = entity;
                        }
                        detail.CREATEUSERID = entity.OWNERID;
                        detail.CREATEDATE = DateTime.Now;
                        detail.UPDATEDATE = DateTime.Now;
                        detail.UPDATEUSERID = entity.UPDATEUSERID;
                    }
                    else
                    {
                        detail.UPDATEDATE = DateTime.Now;
                        detail.UPDATEUSERID = entity.UPDATEUSERID;
                    }
                    sumTotal = sumTotal + i;
                }
                Tracer.Debug("CheckBorrow提示信息：" + strMsg);
                entity.TOTALMONEY = sumTotal;
                if (entity.TOTALMONEY <= 0)
                {
                    strMsg = "借款借款总额不能小于等于0！";
                    return false;
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("CheckBorrow提示信息：" + strMsg+"错误信息："+ ex.ToString());
                strMsg = "验证出错";
            }
            return true;
        }

        /// <summary>
        /// 修改借款管理
        /// </summary>
        /// <param name="strActionType"></param>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <param name="strMsg"></param>
        public void UptBorrowApplyMasterAndDetailForMobile(string strActionType, T_FB_BORROWAPPLYMASTER entity,
            List<T_FB_BORROWAPPLYDETAIL> detailList, ref string strMsg)
        {
            Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - fasdfadadfas开始");
            bool isReturn = CheckBorrow(ref entity, ref detailList, ref strMsg, "2");
            if (!string.IsNullOrEmpty(strMsg))
            {
                return;
            }
            bool re = false;
            if (LockOrder(entity.BORROWAPPLYMASTERID))
            {
                strMsg = "单据正在提交或审核中，不可重复操作！";
                return;
            }

            try
            {
                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;
                T_FB_BORROWAPPLYMASTER cha = GetBorrowApplyMasterByID(entity.BORROWAPPLYMASTERID);
                if (cha == null)
                {
                    strMsg = "提交的单据不存在或已删除，不可继续操作！";
                    return;
                }
                object checkStatesOld = cha.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());
                object checkStatesNew = entity.CHECKSTATES;
                FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());

                if ((dOldChecksates == FBAEnums.CheckStates.Approved || dOldChecksates == FBAEnums.CheckStates.UnApproved)
                    && strActionType.ToUpper() != "RESUBMIT")
                {
                    strMsg = "单据已审核完毕，不可再次操作";
                    return;
                }
                Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - 开始");
                Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - BORROWAPPLYMASTERID" + entity.BORROWAPPLYMASTERID);
                Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - BANK" + entity.BANK);
                Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - BANKACCOUT" + entity.BANKACCOUT);
                if (entity.PAYTARGET != null)
                {
                    Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - REPAYTYPE" + entity.REPAYTYPE.ToString());
                }
                if (entity.REPAYTYPE != null)
                {
                    Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile - REPAYTYPE" + entity.PAYTARGET.ToString());
                }
                Tracer.Debug("AddBorrowApplyMasterAndDetailForMobile -REMARK " + entity.REMARK);
                Utility.CloneEntity(entity, cha);
                Tracer.Debug("开始更新操作了&&&*********");
                cha.UPDATEDATE = DateTime.Now;
                bool n = Update(cha);
                if (n == false)
                {
                    strMsg = "单据更新异常！";
                    return;
                }

                BorrowApplyDetailBLL borrowDetailBLL = new BorrowApplyDetailBLL();
                Tracer.Debug("开始更新操作了");
                re = borrowDetailBLL.UpdateBorrowApplyDetail(cha.BORROWAPPLYMASTERID, detailList);//删除报销明细
                if (!re)
                {
                    strMsg = "单据明细更新异常！";
                    return;
                }

                if (dOldChecksates == dNewCheckStates && dOldChecksates == (int)FBAEnums.CheckStates.UnSubmit
                    && strActionType.ToUpper() == "EDIT")
                {
                    strMsg = "单据更新成功！";
                    return;
                }                
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptBorrowApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            finally
            {
                ReleaseOrder(entity.BORROWAPPLYMASTERID);
            }
        }





        public void UptBorrowApplyMasterAndDetailForMobileToSubmit(string strActionType,T_FB_BORROWAPPLYMASTER entity,ref string strMsg)
        {

           
            bool re = false;
            if (LockOrder(entity.BORROWAPPLYMASTERID))
            {
                strMsg = "单据正在提交或审核中，不可重复操作！";
                return;
            }

            try
            {
                FBAEnums.CheckStates dOldChecksates = FBAEnums.CheckStates.UnSubmit;
                T_FB_BORROWAPPLYMASTER cha = GetBorrowApplyMasterByID(entity.BORROWAPPLYMASTERID);
                if (cha == null)
                {
                    strMsg = "提交的单据不存在或已删除，不可继续操作！";
                    return;
                }
                object checkStatesOld = cha.CHECKSTATES;
                dOldChecksates = (FBAEnums.CheckStates)int.Parse(checkStatesOld.ToString());
                object checkStatesNew = entity.CHECKSTATES;
                FBAEnums.CheckStates dNewCheckStates = (FBAEnums.CheckStates)int.Parse(checkStatesNew.ToString());

                if ((dOldChecksates == FBAEnums.CheckStates.Approved || dOldChecksates == FBAEnums.CheckStates.UnApproved)
                    && strActionType.ToUpper() != "RESUBMIT")
                {
                    strMsg = "单据已审核完毕，不可再次操作";
                    return;
                }
                Tracer.Debug("UptBorrowApplyMasterAndDetailForMobileToSubmit - 开始");
                Tracer.Debug("UptBorrowApplyMasterAndDetailForMobileToSubmit - BORROWAPPLYMASTERID" + entity.BORROWAPPLYMASTERID);
                Tracer.Debug("UptBorrowApplyMasterAndDetailForMobileToSubmit - BANK" + entity.BANK);
                Tracer.Debug("UptBorrowApplyMasterAndDetailForMobileToSubmit - BANKACCOUT" + entity.BANKACCOUT);
                if (entity.PAYTARGET != null)
                {
                    Tracer.Debug("UptBorrowApplyMasterAndDetailForMobileToSubmit - REPAYTYPE" + entity.REPAYTYPE.ToString());
                }
                if (entity.REPAYTYPE != null)
                {
                    Tracer.Debug("UptBorrowApplyMasterAndDetailForMobileToSubmit - REPAYTYPE" + entity.PAYTARGET.ToString());
                }
                Tracer.Debug("UptBorrowApplyMasterAndDetailForMobileToSubmit -REMARK " + entity.REMARK);
                Utility.CloneEntity(entity, cha);
                Tracer.Debug("开始更新操作了&&&*********");
                cha.UPDATEDATE = DateTime.Now;
                bool n = Update(cha);
                if (n == false)
                {
                    strMsg = "单据更新异常！";
                    return;
                }
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：UptBorrowApplyMasterAndDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            finally
            {
                ReleaseOrder(entity.BORROWAPPLYMASTERID);
            }
        }


        #region 获取元数据
        public string GetXmlString(string Formid,ref string BorrowCode)
        {
            string strReturn = string.Empty;
            try
            {
                T_FB_BORROWAPPLYMASTER Info = dal.GetObjects<T_FB_BORROWAPPLYMASTER>().Where(t => t.BORROWAPPLYMASTERID == Formid).FirstOrDefault();
                
                PersonnelServiceClient personel = new PersonnelServiceClient();
                V_EMPLOYEEVIEW employee = personel.GetEmployeeInfoByEmployeeID(Info.OWNERID);
                decimal? stateValue = Convert.ToDecimal("1");
                string checkState = string.Empty;
                string checkStateDict
                    = PermClient.GetDictionaryByCategoryArray(new string[] { "CHECKSTATE" }).Where(p => p.DICTIONARYVALUE == stateValue).FirstOrDefault().DICTIONARYNAME;
                checkState = checkStateDict == null ? "" : checkStateDict;
                if (Info == null)
                {
                    Tracer.Debug("BorrowApplyMasterBLL获取元借款申请主表记录为空,主表ID:"+ Formid);
                    //return strReturn;
                    throw new Exception("获取借款申请主表记录为空");
                }
                if (employee == null)                
                {
                    Tracer.Debug("BorrowApplyMasterBLL获取元数据时员工信息为空：");
                    //return strReturn;
                    throw new Exception("获取元数据时员工信息为空");
                }
                if (employee.POSTLEVEL == null)
                {
                    Tracer.Debug("BorrowApplyMasterBLL获取元数据时员工信息时岗位级别PostLevel：");
                    throw new Exception("员工岗位级别为空");
                    //return strReturn;
                }
                decimal? postlevelValue = Convert.ToDecimal(employee.POSTLEVEL.ToString());
                string postLevelName = string.Empty;
                string postLevelDict
                     = PermClient.GetDictionaryByCategoryArray(new string[] { "CHECKSTATE" }).Where(p => p.DICTIONARYVALUE == stateValue).FirstOrDefault().DICTIONARYNAME;
                postLevelName = postLevelDict == null ? "" : postLevelDict;

                var ents = from ent in dal.GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                           where ent.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID == Formid
                           select ent;

                List<T_FB_BORROWAPPLYDETAIL> objB;
                if (ents.Count() > 0)
                {
                    objB = ents.ToList();
                }
                else
                {
                    Tracer.Debug("BorrowApplyMasterBLL获取元数据时获取借款明细记录为空，主表记录为：" + Formid);
                    throw new Exception("获取借款明细记录为空.");
                }
                
                Info.BORROWAPPLYMASTERCODE = new  OrderCodeBLL().GetAutoOrderCode(Info);
                Tracer.Debug("BorrowApplyMasterBLL表单单据号为：" + Info.BORROWAPPLYMASTERCODE);
                UptBorrowApplyMasterAndDetailForMobileToSubmit("Edit", Info,  ref strReturn);
                if (!string.IsNullOrEmpty(strReturn))
                {
                    Tracer.Debug("更新状态："+strReturn+"主表记录为：" + Formid);
                    throw new Exception(strReturn);
                }
                BorrowCode = Info.BORROWAPPLYMASTERCODE;
                string strOwnerName = string.Empty;
                string strOwnerCompanyName = string.Empty;
                string strOwnerPostName = string.Empty;
                string strOwnerDepartmentName = string.Empty;
                strOwnerName = employee.EMPLOYEECNAME;
                strOwnerPostName = employee.POSTNAME;
                strOwnerDepartmentName = employee.DEPARTMENTNAME;
                strOwnerCompanyName = employee.COMPANYNAME;
                
                SMT.SaaS.MobileXml.MobileXml mx = new SaaS.MobileXml.MobileXml();
                SMT.SaaS.MobileXml.AutoDictionary ad = new SaaS.MobileXml.AutoDictionary();
                List<SMT.SaaS.MobileXml.AutoDictionary> AutoList = new List<SMT.SaaS.MobileXml.AutoDictionary>();
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "PAYTARGET", Info.PAYTARGET.ToString(), "个人"));//付款方式
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "ISREPAIED", Info.ISREPAIED.ToString(), Info.ISREPAIED.ToString() == "0" ? "否" : "是"));//是否还情

                string StrPayType = "";
                string StrEditState = "";
                switch (Info.REPAYTYPE.ToString())
                {
                    case "1":
                        StrPayType = "普通借款";
                        break;
                    case "2":
                        StrPayType = "备用金借款";
                        break;
                    case "3":
                        StrPayType = "专项借款";
                        break;
                }
                switch (Info.EDITSTATES.ToString())
                {
                    case "0":
                        StrEditState = "删除状态";
                        break;
                    case "1":
                        StrEditState = "已生效";
                        break;
                    case "2":
                        StrEditState = "未生效";
                        break;
                    case "3":
                        StrEditState = "撤消中";
                        break;
                    case "4":
                        StrEditState = "已撤消";
                        break;
                }
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "POSTLEVEL", postlevelValue.ToString(), null));//POSTLEVEL
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "CHECKSTATES", "1", "审核中"));
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "REPAYTYPE", Info.REPAYTYPE.ToString(), StrPayType));//相关单据类型
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "EDITSTATES", Info.EDITSTATES.ToString(), StrEditState));//编辑状态
                AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "BORROWAPPLYMASTERCODE", Info.BORROWAPPLYMASTERCODE, Info.BORROWAPPLYMASTERCODE));//单据编号
                if (Info.OWNERID != null && !string.IsNullOrEmpty(strOwnerName))
                {
                    AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "OWNERID", Info.OWNERID, strOwnerName + "-" + strOwnerPostName + "-" + strOwnerDepartmentName + "-" + strOwnerCompanyName));
                }
                if (Info.OWNERCOMPANYID != null && !string.IsNullOrEmpty(strOwnerCompanyName))
                {
                    AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "OWNERCOMPANYID", Info.OWNERCOMPANYID, strOwnerCompanyName));
                }
                if (Info.OWNERDEPARTMENTID != null && !string.IsNullOrEmpty(strOwnerDepartmentName))
                {
                    AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "OWNERDEPARTMENTID", Info.OWNERDEPARTMENTID, strOwnerDepartmentName));
                }
                if (Info.OWNERPOSTID != null && !string.IsNullOrEmpty(strOwnerPostName))
                {
                    AutoList.Add(basedata("T_FB_BORROWAPPLYMASTER", "OWNERPOSTID", Info.OWNERPOSTID, strOwnerPostName));
                }
                
                foreach (T_FB_BORROWAPPLYDETAIL objDetail in objB)
                {
                    if (objDetail.T_FB_SUBJECT != null)
                    {
                        AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "SUBJECTID", objDetail.T_FB_SUBJECT.SUBJECTID, objDetail.T_FB_SUBJECT.SUBJECTID, objDetail.BORROWAPPLYDETAILID));
                        AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "SUBJECTCODE", objDetail.T_FB_SUBJECT.SUBJECTCODE, objDetail.T_FB_SUBJECT.SUBJECTCODE, objDetail.BORROWAPPLYDETAILID));
                        AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "SUBJECTNAME", objDetail.T_FB_SUBJECT.SUBJECTNAME, objDetail.T_FB_SUBJECT.SUBJECTNAME, objDetail.BORROWAPPLYDETAILID));
                    }
                    if (objDetail.T_FB_BORROWAPPLYMASTER != null)
                    {
                        AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "BORROWAPPLYMASTERID", objDetail.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID, objDetail.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID, objDetail.BORROWAPPLYDETAILID));
                    }
                    if (objDetail.CHARGETYPE != null)
                    {
                        AutoList.Add(basedataForChild("T_FB_BORROWAPPLYDETAIL", "CHARGETYPE", objDetail.CHARGETYPE.ToString(), objDetail.CHARGETYPE.ToString() == "1" ? "个人预算费用" : "公共预算费用", objDetail.BORROWAPPLYDETAILID));
                    }

                }
                string StrSource = GetBusinessObject("T_FB_BORROWAPPLYMASTER");
                Tracer.Debug("获取借款申请的元数据模板为：" + StrSource);
                strReturn = mx.TableToXml(Info, objB, StrSource, AutoList);
                Tracer.Debug("组合借款申请的元数据模板为：" + strReturn);
                
            }
            catch (Exception ex)
            {
                Tracer.Debug("获取借款申请的元数据模板出现错误：" + ex.ToString());
                throw new Exception("获取借款申请的元数据出错错误");
            }
            return strReturn;
        }
        #endregion
        #endregion

    }

}

