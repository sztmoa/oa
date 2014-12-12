
/*
 * 文件名：BorrowApplyDetailBLL.cs
 * 作  用：T_FB_BORROWAPPLYDETAIL 业务逻辑类
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
    public class BorrowApplyDetailBLL : BaseBll<T_FB_BORROWAPPLYDETAIL>
    {
        public BorrowApplyDetailBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_BORROWAPPLYDETAIL信息
        /// </summary>
        /// <param name="strBorrowApplyDetailId">主键索引</param>
        /// <returns></returns>
        public T_FB_BORROWAPPLYDETAIL GetBorrowApplyDetailByID(string strBorrowApplyDetailId)
        {
            if (string.IsNullOrEmpty(strBorrowApplyDetailId))
            {
                return null;
            }

            BorrowApplyDetailDAL dalBorrowApplyDetail = new BorrowApplyDetailDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strBorrowApplyDetailId))
            {
                strFilter.Append(" BORROWAPPLYDETAILID == @0");
                objArgs.Add(strBorrowApplyDetailId);
            }
            T_FB_BORROWAPPLYDETAIL entRd = new T_FB_BORROWAPPLYDETAIL();
            try
            {
                entRd = dalBorrowApplyDetail.GetBorrowApplyDetailRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetBorrowApplyDetailByID，" + System.DateTime.Now.ToString() +"，"+ ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return entRd;
        }

        /// <summary>
        /// 获取T_FB_CHARGEAPPLYDETAIL信息   add by zl
        /// </summary>
        /// <param name="objChargeApplyMasterId"></param>
        /// <returns></returns>
        public List<T_FB_BORROWAPPLYDETAIL> GetBorrowApplyDetailByMasterID(string strBorrowApplyMasterId)
        {
            if (string.IsNullOrWhiteSpace(strBorrowApplyMasterId))
            {
                return null;
            }

            List<T_FB_BORROWAPPLYDETAIL> entRdlist = new List<T_FB_BORROWAPPLYDETAIL>();
            var ents = from n in dal.GetObjects().Include("T_FB_BORROWAPPLYMASTER")
                       where n.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID == strBorrowApplyMasterId
                       select n;

            if (ents.Count() > 0)
            {
                entRdlist = ents.ToList();
                return entRdlist;
            }

            return null;
        }

        /// <summary>
        /// 获取T_FB_BORROWAPPLYDETAIL信息   add by zl
        /// </summary>
        /// <param name="strBorrowApplyMasterId">主表主键索引</param>
        /// <returns></returns>
        public List<T_FB_BORROWAPPLYDETAIL> GetBorrowApplyDetailByMasterID(List<object> objBorrowApplyMasterId)
        {
            if (objBorrowApplyMasterId.Count <= 0)
            {
                return null;
            }

            List<T_FB_BORROWAPPLYDETAIL> entRdlist = new List<T_FB_BORROWAPPLYDETAIL>();
            BorrowApplyDetailDAL dalBorrowApplyDetail = new BorrowApplyDetailDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            try
            {
                if (objBorrowApplyMasterId.Count >= 0)
                {
                    foreach (string strBorrowApplyMasterId in objBorrowApplyMasterId.ToArray())
                    {
                        strFilter.Append(" T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID == @0");
                        objArgs.Add(strBorrowApplyMasterId);
                        IQueryable<T_FB_BORROWAPPLYDETAIL> entRd = GetBorrowApplySubjectDetailRdList("UPDATEDATE", strFilter.ToString(), objArgs.ToArray());
                        foreach (T_FB_BORROWAPPLYDETAIL da in entRd)
                        {
                            entRdlist.Add(da);

                        }
                        strFilter.Clear();
                        objArgs.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetBorrowApplyDetailByMasterID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return entRdlist;
        }

        /// <summary>
        /// 获取T_FB_BORROWAPPLYDETAIL T_FB_SUBJECT信息   add by zl
        /// </summary>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public IQueryable<T_FB_BORROWAPPLYDETAIL> GetBorrowApplySubjectDetailRdList(string strOrderBy, string strFilter, params object[] objArgs)
        {
            try
            {
                var q = from v in dal.GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_SUBJECT").Include("T_FB_BORROWAPPLYMASTER")
                        select v;

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    q = q.Where(strFilter, objArgs);
                }

                return q.OrderBy(strOrderBy);
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetBorrowApplySubjectDetailRdList，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据条件，获取T_FB_BORROWAPPLYDETAIL信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_BORROWAPPLYDETAIL> GetAllBorrowApplyDetailRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            try
            {
                BorrowApplyDetailDAL dalBorrowApplyDetail = new BorrowApplyDetailDAL();
                string strOrderBy = string.Empty;

                if (!string.IsNullOrEmpty(strSortKey))
                {
                    strOrderBy = strSortKey;
                }
                else
                {
                    strOrderBy = " BORROWAPPLYDETAILID ";
                }

                var q = dalBorrowApplyDetail.GetBorrowApplyDetailRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
                return q;
            }
            catch (Exception ex)
            {
                string ErrInfo = new BorrowApplyDetailBLL().GetType().ToString() + "：GetAllBorrowApplyDetailRdListByMultSearch，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据条件，获取T_FB_BORROWAPPLYDETAIL信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_BORROWAPPLYDETAIL信息</returns>
        public IQueryable<T_FB_BORROWAPPLYDETAIL> GetBorrowApplyDetailRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllBorrowApplyDetailRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_BORROWAPPLYDETAIL>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion
        
        #region 新增借款单
        /// <summary>
        ///  新增借款单，
        /// </summary>
        /// <param name="detil">主表类型参数</param>
        /// <returns>返回布尔值，表示是否保存成功</returns>
        public bool AddBorrowApply(T_FB_BORROWAPPLYDETAIL master)
        {
            bool flag = false;
            try
            {
                int x = master != null ? dal.Add(master) : -1;
                //int x = dal.Add(detil);
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

        #region CRUD

        public List<T_FB_BORROWAPPLYDETAIL> GetInfo()
        {
            List<T_FB_BORROWAPPLYDETAIL> detailList;
            try
            {
                var q = from i in dal.GetObjects().Include("T_FB_BORROWAPPLYMASTER")
                        select i;
               detailList=(q != null && q.Count() > 0) ? q.ToList() : null;
            }
            catch (Exception ex)
            {
                Tracer.Debug("个人申请借款明细BorrowApplyDetailBLL-GetInfo" +
                             System.DateTime.Now.ToString() + " " + ex.ToString());
                detailList = null;
                 
            }
            return detailList;
        }

        #region  子表查询
        /// <summary>
        /// 根据主表主键查询子表，带出主表数据
        /// </summary>
        /// <param name="masterId">主表借款单号ID</param>
        /// <returns> 返回子表与主表的数据</returns>
        public T_FB_BORROWAPPLYDETAIL GetInfoById(string masterId)
        {
            T_FB_BORROWAPPLYDETAIL DetialList;
            try
            {
                var masterList = dal.GetObjects<T_FB_BORROWAPPLYDETAIL>()
                               .Include("T_FB_BORROWAPPLYMASTER")
                               .Where(p => (p.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID == masterId))
                               .OrderBy(p => (p.UPDATEDATE))
                               .Select(p => p);
                DetialList = (masterList != null && masterList.Count() > 0)
                    ? masterList.FirstOrDefault() : null;
            }
            catch (Exception ex)
            {
                Tracer.Debug("个人申请借款BorrowApplyDetailBLL-GetInfoById" +
                                System.DateTime.Now.ToString() + " " + ex.ToString());
                DetialList = null;

            }
            return DetialList;
        }
        #endregion


        #endregion
        
        #region
        public IQueryable<T_FB_BORROWAPPLYDETAIL> GetBorrowApps(int pageIndex, int pageSize, string sort, string filterString, object[] paras, ref int pageCount, string userId, List<string> guidStringList, string checkState)
        {
            try
            {
                var m = from master in dal.GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                        select master;
                if (checkState == "4")
                {
                    if (guidStringList != null)
                    {
                        m = from ent in m
                            where guidStringList.Contains(ent.BORROWAPPLYDETAILID)
                            select ent;

                    }
                }
                else//创建人
                {
                    m = m.Where(ent => ent.CREATEUSERID == userId);
                    if (checkState != "5")
                    {
                        m = m.Where(ent => ent.T_FB_BORROWAPPLYMASTER.CHECKSTATES.ToString() == checkState);
                    }
                }
                List<object> queryParas = new List<object>();
                if (paras != null)
                {
                    queryParas.AddRange(paras);
                }
                UtilityClass.SetOrganizationFilter(ref filterString, ref queryParas, userId, "T_FB_BORROWAPPLYMASTER");
                if (!string.IsNullOrEmpty(filterString))
                {
                    m = m.ToList().AsQueryable().Where(filterString, queryParas.ToArray());
                }
                m = m.OrderBy(sort);
                m = Utility.Pager<T_FB_BORROWAPPLYDETAIL>(m, pageIndex, pageSize, ref pageCount);
                if (m.Count() > 0)
                {
                    return m;
                }
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetBorrowApps，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }
        #endregion

        #region 删除数据
        /// <summary>
        /// 删除借款明细表数据
        /// </summary>
        /// <param name="borrowMasterID"></param>
        /// <returns></returns>
        public bool DelBorrowApplyDetail(string borrowMasterID)
        {
            bool re = false;
            try
            {
                var entitys = from ent in dal.GetObjects().Include("T_FB_BORROWAPPLYMASTER")
                              where ent.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID == borrowMasterID
                              select ent;

                if (entitys.Count() > 0)
                {
                    foreach (T_FB_BORROWAPPLYDETAIL obj in entitys)
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
                string ErrInfo = this.GetType().ToString() + "：DelBorrowApplyDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        #endregion

        #region 更新数据
        
        /// <summary>
        /// 更新借款明细记录
        /// </summary>
        /// <param name="strBorrowMasterID"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public bool UpdateBorrowApplyDetail(string strBorrowMasterID, List<T_FB_BORROWAPPLYDETAIL> detailList)
        {
            bool bRes = false;
            try
            {
                if (string.IsNullOrWhiteSpace(strBorrowMasterID))
                {
                    return bRes;
                }


                BorrowApplyMasterBLL masterBLL = new BorrowApplyMasterBLL();
                T_FB_BORROWAPPLYMASTER entMaster = masterBLL.GetBorrowApplyMasterByID(strBorrowMasterID);

                bRes = DelBorrowApplyDetail(strBorrowMasterID);

                foreach (T_FB_BORROWAPPLYDETAIL item in detailList)
                {
                    if (item.EntityKey != null)
                    {
                        item.EntityKey = null;
                    }

                    item.BORROWAPPLYDETAILID = System.Guid.NewGuid().ToString();
                    if (item.T_FB_BORROWAPPLYMASTER == null)
                    {
                        item.T_FB_BORROWAPPLYMASTER = entMaster;
                    }
                    item.T_FB_BORROWAPPLYMASTER.EntityKey = new System.Data.EntityKey("SMT_FB_EFModelContext.T_FB_BORROWAPPLYMASTER", "BORROWAPPLYMASTERID", entMaster.BORROWAPPLYMASTERID);
                    Tracer.Debug("开始修改");
                    if (item.T_FB_SUBJECT != null)
                    {
                        item.T_FB_SUBJECT.EntityKey = new System.Data.EntityKey("SMT_FB_EFModelContext.T_FB_SUBJECT", "SUBJECTID", item.T_FB_SUBJECT.SUBJECTID);
                    }
                    Tracer.Debug("开始添加");
                    Add(item);
                }
                bRes = true;
            }
            catch (Exception ex)
            {
                Tracer.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "调用函数 UpdateBorrowApplyDetail 出现异常，异常信息为：" + ex.ToString());
                bRes = false;
            }

            return bRes;
        }

        #endregion
    }
}

