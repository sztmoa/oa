
/*
 * 文件名：RepayApplyDetailBLL.cs
 * 作  用：T_FB_REPAYAPPLYDETAIL 业务逻辑类
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

using SMT_FB_EFModel;
using SMT.FBAnalysis.DAL;
using SMT.Foundation.Log;

namespace SMT.FBAnalysis.BLL
{
    public class RepayApplyDetailBLL : BaseBll<T_FB_REPAYAPPLYDETAIL>
    {
        public RepayApplyDetailBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_REPAYAPPLYDETAIL信息
        /// </summary>
        /// <param name="strRepayApplyDetailId">主键索引</param>
        /// <returns></returns>
        public T_FB_REPAYAPPLYDETAIL GetRepayApplyDetailByID(string strRepayApplyDetailId)
        {
            if (string.IsNullOrEmpty(strRepayApplyDetailId))
            {
                return null;
            }

            RepayApplyDetailDAL dalRepayApplyDetail = new RepayApplyDetailDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            
            if (!string.IsNullOrEmpty(strRepayApplyDetailId))
            {
                strFilter.Append(" REPAYAPPLYDETAILID == @0");
                objArgs.Add(strRepayApplyDetailId);
            }
            try
            {
                T_FB_REPAYAPPLYDETAIL entRd = dalRepayApplyDetail.GetRepayApplyDetailRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
                return entRd;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetRepayApplyDetailByID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据主表ID获取T_FB_REPAYAPPLYDETAIL信息   add by zl
        /// </summary>
        /// <param name="strRepayApplyMasterId"></param>
        /// <returns></returns>
        public List<T_FB_REPAYAPPLYDETAIL> GetRepayApplyDetailByMasterID(string strRepayApplyMasterId)
        {
            if (string.IsNullOrEmpty(strRepayApplyMasterId))
            {
                return null;
            }

            RepayApplyDetailDAL dalRepayApplyDetail = new RepayApplyDetailDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            List<T_FB_REPAYAPPLYDETAIL>repList=new List<T_FB_REPAYAPPLYDETAIL>();

            if (!string.IsNullOrEmpty(strRepayApplyMasterId))
            {
                strFilter.Append(" T_FB_REPAYAPPLYMASTER.REPAYAPPLYMASTERID == @0");
                objArgs.Add(strRepayApplyMasterId);
            }

            try
            {
                IQueryable<T_FB_REPAYAPPLYDETAIL> entRds = GetRepayApplySubjectDetailRdList("UPDATEDATE", strFilter.ToString(), objArgs.ToArray());
                if (entRds == null)
                {
                    return null;
                }
                //add zl 12.29  取实时的借款余额
                foreach (T_FB_REPAYAPPLYDETAIL da in entRds)
                {
                    if(da.T_FB_REPAYAPPLYMASTER.CHECKSTATES != 2)
                    {
                        var a = from i in dal.GetObjects<T_FB_PERSONACCOUNT>()
                                where i.OWNERCOMPANYID == da.T_FB_REPAYAPPLYMASTER.OWNERCOMPANYID
                                && i.OWNERID == da.T_FB_REPAYAPPLYMASTER.OWNERID
                                select i;
                        if(da.REPAYTYPE==1)
                        {
                            da.BORROWMONEY = a.FirstOrDefault().SIMPLEBORROWMONEY.Value;
                        }
                        if(da.REPAYTYPE==2)
                        {
                            da.BORROWMONEY = a.FirstOrDefault().BACKUPBORROWMONEY.Value;
                        }
                        if(da.REPAYTYPE==3)
                        {
                            da.BORROWMONEY = a.FirstOrDefault().SPECIALBORROWMONEY.Value;
                        }
                    }
                    repList.Add(da);
                }
                //add end

                //if (entRds.Count() == 0)
                //{
                //    return null;
                //}
                
                //return entRds.ToList();
                return repList;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetRepayApplyDetailByMasterID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 获取指定条件的T_FB_REPAYAPPLYDETAIL T_FB_SUBJECT信息 add by zl
        /// </summary>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public IQueryable<T_FB_REPAYAPPLYDETAIL> GetRepayApplySubjectDetailRdList(string strOrderBy, string strFilter, params object[] objArgs)
        {
            try
            {
                var q = from v in dal.GetObjects<T_FB_REPAYAPPLYDETAIL>().Include("T_FB_SUBJECT").Include("T_FB_BORROWAPPLYDETAIL").Include("T_FB_REPAYAPPLYMASTER")
                        select v;

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    q = q.Where(strFilter, objArgs);
                }

                return q.OrderBy(strOrderBy);
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetRepayApplySubjectDetailRdList，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据条件，获取T_FB_REPAYAPPLYDETAIL信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_REPAYAPPLYDETAIL> GetAllRepayApplyDetailRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            RepayApplyDetailDAL dalRepayApplyDetail = new RepayApplyDetailDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " REPAYAPPLYDETAILID ";
            }
            try
            {
                var q = dalRepayApplyDetail.GetRepayApplyDetailRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
                return q;
            }
            catch (Exception ex)
            {
                string ErrInfo = new RepayApplyDetailBLL().GetType().ToString() + "：GetAllRepayApplyDetailRdListByMultSearch，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据条件，获取T_FB_REPAYAPPLYDETAIL信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_REPAYAPPLYDETAIL信息</returns>
        public IQueryable<T_FB_REPAYAPPLYDETAIL> GetRepayApplyDetailRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllRepayApplyDetailRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_REPAYAPPLYDETAIL>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion

        #region 删除数据
        /// <summary>
        /// 删除还款明细表数据   add by zl
        /// </summary>
        /// <param name="repayMasterID"></param>
        /// <returns></returns>
        public bool DelRepayApplyDetail(string repayMasterID)
        {
            bool re = false;
            try
            {
                var entitys = from ent in dal.GetObjects().Include("T_FB_REPAYAPPLYMASTER")
                              where ent.T_FB_REPAYAPPLYMASTER.REPAYAPPLYMASTERID == repayMasterID
                              select ent;

                if (entitys.Count() > 0)
                {
                    foreach (T_FB_REPAYAPPLYDETAIL obj in entitys)
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
                string ErrInfo = this.GetType().ToString() + "：DelRepayApplyDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        #endregion

        #region 更新数据
        /// <summary>
        /// 更新还款明细
        /// </summary>
        /// <param name="strRepayMasterID"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public bool UpdateRepayApplyDetail(string strRepayMasterID, List<T_FB_REPAYAPPLYDETAIL> detailList)
        {
            bool bRes = false;
            try
            {
                if (string.IsNullOrWhiteSpace(strRepayMasterID))
                {
                    return bRes;
                }

                RepayApplyMasterBLL masterBLL = new RepayApplyMasterBLL();
                T_FB_REPAYAPPLYMASTER entMaster = masterBLL.GetRepayApplyMasterByID(strRepayMasterID);
                bRes = DelRepayApplyDetail(strRepayMasterID);

                foreach (T_FB_REPAYAPPLYDETAIL item in detailList)
                {
                    if (item.EntityKey != null)
                    {
                        item.EntityKey = null;
                    }

                    item.REPAYAPPLYDETAILID = System.Guid.NewGuid().ToString();
                    if (item.T_FB_REPAYAPPLYMASTER == null)
                    {
                        item.T_FB_REPAYAPPLYMASTER = entMaster;
                    }
                    item.T_FB_REPAYAPPLYMASTER.EntityKey = new System.Data.EntityKey("SMT_FB_EFModelContext.T_FB_REPAYAPPLYMASTER", "REPAYAPPLYMASTERID", item.T_FB_REPAYAPPLYMASTER.REPAYAPPLYMASTERID);
                    
                    if (item.T_FB_SUBJECT != null)
                    {
                        item.T_FB_SUBJECT.EntityKey = new System.Data.EntityKey("SMT_FB_EFModelContext.T_FB_SUBJECT", "SUBJECTID", item.T_FB_SUBJECT.SUBJECTID);
                    }

                    if (item.T_FB_BORROWAPPLYDETAIL != null)
                    {
                        item.T_FB_BORROWAPPLYDETAIL.EntityKey = new System.Data.EntityKey("SMT_FB_EFModelContext.T_FB_BORROWAPPLYDETAIL", "BORROWAPPLYDETAILID", item.T_FB_BORROWAPPLYDETAIL.BORROWAPPLYDETAILID);
                    }

                    Add(item);
                }
                bRes = true;
            }
            catch (Exception ex)
            {
                Tracer.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "调用函数 UpdateRepayApplyDetail 出现异常，异常信息为：" + ex.ToString());
                bRes = false;
            }

            return bRes;
        }
        #endregion
    }
}

