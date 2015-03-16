
/*
 * 文件名：ChargeApplyDetailBLL.cs
 * 作  用：T_FB_CHARGEAPPLYDETAIL 业务逻辑类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 11:47:04
 * 修改人：
 * 修改时间：
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.DAL;
using SMT.Foundation.Log;

namespace SMT.FBAnalysis.BLL
{
    public class ChargeApplyDetailBLL : BaseBll<T_FB_CHARGEAPPLYDETAIL>
    {
        public ChargeApplyDetailBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_CHARGEAPPLYDETAIL信息
        /// </summary>
        /// <param name="strChargeApplyDetailId">主键索引</param>
        /// <returns></returns>
        public T_FB_CHARGEAPPLYDETAIL GetChargeApplyDetailByID(string strChargeApplyDetailId)
        {
            if (string.IsNullOrEmpty(strChargeApplyDetailId))
            {
                return null;
            }

            ChargeApplyDetailDAL dalChargeApplyDetail = new ChargeApplyDetailDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strChargeApplyDetailId))
            {
                strFilter.Append(" CHARGEAPPLYDETAILID == @0");
                objArgs.Add(strChargeApplyDetailId);
            }
            T_FB_CHARGEAPPLYDETAIL entRd = new T_FB_CHARGEAPPLYDETAIL();
            try
            {
                entRd = dalChargeApplyDetail.GetChargeApplyDetailRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetChargeApplyDetailByID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取T_FB_CHARGEAPPLYDETAIL信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_CHARGEAPPLYDETAIL> GetAllChargeApplyDetailRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            ChargeApplyDetailDAL dalChargeApplyDetail = new ChargeApplyDetailDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " CHARGEAPPLYDETAILID ";
            }

            try
            {
                var q = dalChargeApplyDetail.GetChargeApplyDetailRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
                return q;
            }
            catch (Exception ex)
            {
                string ErrInfo = new ChargeApplyDetailBLL().GetType().ToString() + "：GetAllChargeApplyDetailRdListByMultSearch，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据条件，获取T_FB_CHARGEAPPLYDETAIL信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_CHARGEAPPLYDETAIL信息</returns>
        public IQueryable<T_FB_CHARGEAPPLYDETAIL> GetChargeApplyDetailRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllChargeApplyDetailRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_CHARGEAPPLYDETAIL>(q, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 获取T_FB_CHARGEAPPLYDETAIL信息   add by zl
        /// </summary>
        /// <param name="objChargeApplyMasterId"></param>
        /// <returns></returns>
        public List<T_FB_CHARGEAPPLYDETAIL> GetChargeApplyDetailByMasterID(string strChargeApplyMasterId)
        {
            if (string.IsNullOrWhiteSpace(strChargeApplyMasterId))
            {
                return null;
            }

            List<T_FB_CHARGEAPPLYDETAIL> entRdlist = new List<T_FB_CHARGEAPPLYDETAIL>();
            var ents = from n in dal.GetObjects().Include("T_FB_CHARGEAPPLYMASTER").Include("T_FB_CHARGEAPPLYMASTER").Include("T_FB_BORROWAPPLYDETAIL").Include("T_FB_SUBJECT")
                       where n.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID == strChargeApplyMasterId
                       select n;

            if (ents.Count() > 0)
            {
                entRdlist = ents.ToList();
                return entRdlist;
            }

            return null;
        }


        /// <summary>
        /// 获取T_FB_CHARGEAPPLYDETAIL信息   add by zl
        /// </summary>
        /// <param name="objChargeApplyMasterId"></param>
        /// <returns></returns>
        public List<T_FB_CHARGEAPPLYDETAIL> GetChargeApplyDetailByMasterID(List<object> objChargeApplyMasterId)
        {
            if (objChargeApplyMasterId.Count <= 0)
            {
                return null;
            }

            List<T_FB_CHARGEAPPLYDETAIL> entRdlist = new List<T_FB_CHARGEAPPLYDETAIL>();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (objChargeApplyMasterId.Count >= 0)
            {
                try
                {
                    foreach (string strChargeApplyMasterId in objChargeApplyMasterId.ToArray())
                    {
                        strFilter.Append(" T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID == @0");
                        objArgs.Add(strChargeApplyMasterId);
                        IQueryable<T_FB_CHARGEAPPLYDETAIL> entRd = GetChargeApplySubjectDetailRdList("UPDATEDATE", strFilter.ToString(), objArgs.ToArray());
                        foreach (T_FB_CHARGEAPPLYDETAIL da in entRd)
                        {
                            //add zl 12.21
                            if(da.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == 0)
                            {
                                if (da.USABLEMONEY != 999999 && da.USABLEMONEY != 999999999999 && da.USABLEMONEY != 99999999)
                                {
                                    if (da.CHARGETYPE == 1)
                                    {
                                        var a = from i in dal.GetObjects<T_FB_BUDGETACCOUNT>()
                                                where i.OWNERCOMPANYID == da.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID
                                                && i.OWNERDEPARTMENTID == da.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID
                                                && i.OWNERPOSTID == da.T_FB_CHARGEAPPLYMASTER.OWNERPOSTID
                                                && i.OWNERID == da.T_FB_CHARGEAPPLYMASTER.OWNERID
                                                && i.T_FB_SUBJECT.SUBJECTID == da.T_FB_SUBJECT.SUBJECTID
                                                && i.ACCOUNTOBJECTTYPE == 3
                                                select i;
                                        da.USABLEMONEY = a.FirstOrDefault().USABLEMONEY;   //据万要求，显示可用结余
                                    }
                                    else if (da.CHARGETYPE == 2)
                                    {
                                        var a = from i in dal.GetObjects<T_FB_BUDGETACCOUNT>()
                                                where i.OWNERCOMPANYID == da.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID
                                                && i.OWNERDEPARTMENTID == da.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID
                                                && i.T_FB_SUBJECT.SUBJECTID == da.T_FB_SUBJECT.SUBJECTID
                                                && i.ACCOUNTOBJECTTYPE == 2
                                                select i;
                                        da.USABLEMONEY = a.FirstOrDefault().USABLEMONEY;    //据万要求，显示可用结余
                                    }
                                }  
                            } 
                            //add end
                            entRdlist.Add(da);
                        }
                        strFilter.Clear();
                        objArgs.Clear();
                    }
                }
                catch (Exception ex)
                {
                    string ErrInfo = this.GetType().ToString() + "：GetChargeApplyDetailByMasterID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                    Tracer.Debug(ErrInfo);
                }
            }
            return entRdlist.OrderBy(t => t.UPDATEDATE).ToList();
        }

        /// <summary>
        /// 获取T_FB_CHARGEAPPLYDETAIL T_FB_SUBJECT T_FB_BORROWAPPLYDETAIL信息   add by zl
        /// </summary>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public IQueryable<T_FB_CHARGEAPPLYDETAIL> GetChargeApplySubjectDetailRdList(string strOrderBy, string strFilter, params object[] objArgs)
        {
            try
            {
                var q = from v in dal.GetObjects<T_FB_CHARGEAPPLYDETAIL>().Include("T_FB_SUBJECT").Include("T_FB_BORROWAPPLYDETAIL").Include("T_FB_CHARGEAPPLYMASTER")
                        select v;

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    q = q.Where(strFilter, objArgs);
                }

                return q.OrderBy(strOrderBy);
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetChargeApplySubjectDetailRdList，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        #endregion

        #region 删除数据
        /// <summary>
        /// 删除费用报销明细表数据   add by zl
        /// </summary>
        /// <param name="chargeMasterID"></param>
        /// <returns></returns>
        public bool DelChargeApplyDetail(string chargeMasterID)
        {
            bool re = true;

            try
            {
                var entitys = from ent in dal.GetObjects().Include("T_FB_CHARGEAPPLYMASTER")
                              where ent.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID == chargeMasterID
                              select ent;

                if (entitys.Count() > 0)
                {
                    foreach (T_FB_CHARGEAPPLYDETAIL obj in entitys)
                    {
                        re = Delete(obj);
                        if (!re)
                        {
                            break;
                        }
                    }
                }

                return re;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：DelChargeApplyDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        #endregion

        #region 更新数据
        public bool UpdateChargeApplyDetail(string strChargeMasterID, List<T_FB_CHARGEAPPLYDETAIL> detailList)
        {
            bool bRes = false;
            try
            {
                if (string.IsNullOrWhiteSpace(strChargeMasterID))
                {
                    return bRes;
                }

                
                ChargeApplyMasterBLL masterBLL = new ChargeApplyMasterBLL();
                T_FB_CHARGEAPPLYMASTER entMaster = masterBLL.GetChargeApplyMasterByID(strChargeMasterID);
                
                bRes = DelChargeApplyDetail(strChargeMasterID);                

                foreach (T_FB_CHARGEAPPLYDETAIL item in detailList)
                {
                    if (item.EntityKey != null)
                    {
                        item.EntityKey = null;
                    }

                    item.CHARGEAPPLYDETAILID = System.Guid.NewGuid().ToString();
                    if (item.T_FB_CHARGEAPPLYMASTER == null)
                    {
                        item.T_FB_CHARGEAPPLYMASTER = entMaster;                        
                    }

                    item.T_FB_CHARGEAPPLYMASTER.EntityKey = new System.Data.EntityKey("TM_SaaS_OA_EFModelContext.T_FB_CHARGEAPPLYMASTER", "CHARGEAPPLYMASTERID", entMaster.CHARGEAPPLYMASTERID);


                    if (item.T_FB_SUBJECT != null)
                    {
                        item.T_FB_SUBJECT.EntityKey = new System.Data.EntityKey("TM_SaaS_OA_EFModelContext.T_FB_SUBJECT", "SUBJECTID", item.T_FB_SUBJECT.SUBJECTID);
                    }

                    if (item.T_FB_BORROWAPPLYDETAIL != null)
                    {
                        item.T_FB_BORROWAPPLYDETAIL.EntityKey = new System.Data.EntityKey("TM_SaaS_OA_EFModelContext.T_FB_BORROWAPPLYDETAIL", "BORROWAPPLYDETAILID", item.T_FB_BORROWAPPLYDETAIL.BORROWAPPLYDETAILID);
                    }

                    Add(item);
                }
                bRes = true;
            }
            catch(Exception ex) 
            {
                Tracer.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +  "调用函数 UpdateChargeApplyDetail 出现异常，异常信息为：" + ex.ToString());
                bRes = false;
            }

            return bRes;
        }
        #endregion

        #region 2014
        /// <summary>
        /// 获取T_FB_CHARGEAPPLYDETAIL信息   add by zl
        /// </summary>
        /// <param name="objChargeApplyMasterId"></param>
        /// <returns></returns>
        //public List<T_FB_CHARGEAPPLYDETAIL> GetChargeApplyDetailByMasterID(string masterID)
        //{

        //    var ids = masterID.Split(':');
        //    var id = ids[0];
            
        //    var q = from v in dal.GetObjects<T_FB_CHARGEAPPLYDETAIL>().Include("T_FB_SUBJECT")
        //            where v.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID == id 
        //            select v;
        //    if (ids.Length > 1)
        //    {

        //    }
        //    List<T_FB_CHARGEAPPLYDETAIL> entRdlist = new List<T_FB_CHARGEAPPLYDETAIL>();
        //    StringBuilder strFilter = new StringBuilder();
        //    List<string> objArgs = new List<string>();

        //    if (objChargeApplyMasterId.Count >= 0)
        //    {
        //        try
        //        {
        //            foreach (string strChargeApplyMasterId in objChargeApplyMasterId.ToArray())
        //            {
        //                strFilter.Append(" T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID == @0");
        //                objArgs.Add(strChargeApplyMasterId);
        //                IQueryable<T_FB_CHARGEAPPLYDETAIL> entRd = GetChargeApplySubjectDetailRdList("UPDATEDATE", strFilter.ToString(), objArgs.ToArray());
        //                foreach (T_FB_CHARGEAPPLYDETAIL da in entRd)
        //                {
        //                    //add zl 12.21
        //                    if (da.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == 0)
        //                    {
        //                        if (da.USABLEMONEY != 999999 && da.USABLEMONEY != 999999999999 && da.USABLEMONEY != 99999999)
        //                        {
        //                            if (da.CHARGETYPE == 1)
        //                            {
        //                                var a = from i in dal.GetObjects<T_FB_BUDGETACCOUNT>()
        //                                        where i.OWNERCOMPANYID == da.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID
        //                                        && i.OWNERDEPARTMENTID == da.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID
        //                                        && i.OWNERPOSTID == da.T_FB_CHARGEAPPLYMASTER.OWNERPOSTID
        //                                        && i.OWNERID == da.T_FB_CHARGEAPPLYMASTER.OWNERID
        //                                        && i.T_FB_SUBJECT.SUBJECTID == da.T_FB_SUBJECT.SUBJECTID
        //                                        && i.ACCOUNTOBJECTTYPE == 3
        //                                        select i;
        //                                da.USABLEMONEY = a.FirstOrDefault().USABLEMONEY;   //据万要求，显示可用结余
        //                            }
        //                            else if (da.CHARGETYPE == 2)
        //                            {
        //                                var a = from i in dal.GetObjects<T_FB_BUDGETACCOUNT>()
        //                                        where i.OWNERCOMPANYID == da.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID
        //                                        && i.OWNERDEPARTMENTID == da.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID
        //                                        && i.T_FB_SUBJECT.SUBJECTID == da.T_FB_SUBJECT.SUBJECTID
        //                                        && i.ACCOUNTOBJECTTYPE == 2
        //                                        select i;
        //                                da.USABLEMONEY = a.FirstOrDefault().USABLEMONEY;    //据万要求，显示可用结余
        //                            }
        //                        }
        //                    }
        //                    //add end
        //                    entRdlist.Add(da);
        //                }
        //                strFilter.Clear();
        //                objArgs.Clear();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            string ErrInfo = this.GetType().ToString() + "：GetChargeApplyDetailByMasterID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
        //            Tracer.Debug(ErrInfo);
        //        }
        //    }
        //    return entRdlist.OrderBy(t => t.UPDATEDATE).ToList();
        //}
        #endregion
    }
}

