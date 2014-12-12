
/*
 * 文件名：PersonmoneyAssignDetailBLL.cs
 * 作  用：T_FB_PERSONMONEYASSIGNDETAIL 业务逻辑类
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

namespace SMT.FBAnalysis.BLL
{
    public class PersonmoneyAssignDetailBLL :BaseBll<T_FB_PERSONMONEYASSIGNDETAIL>
    {
        public PersonmoneyAssignDetailBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_PERSONMONEYASSIGNDETAIL信息
        /// </summary>
        /// <param name="strPersonmoneyAssignDetailId">主键索引</param>
        /// <returns></returns>
        public T_FB_PERSONMONEYASSIGNDETAIL GetPersonmoneyAssignDetailByID(string strPersonmoneyAssignDetailId)
        {
            if (string.IsNullOrEmpty(strPersonmoneyAssignDetailId))
            {
                return null;
            }

            PersonmoneyAssignDetailDAL dalPersonmoneyAssignDetail = new PersonmoneyAssignDetailDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            
            if (!string.IsNullOrEmpty(strPersonmoneyAssignDetailId))
            {
                strFilter.Append(" PERSONBUDGETAPPLYDETAILID == @0");
                objArgs.Add(strPersonmoneyAssignDetailId);
            }

            T_FB_PERSONMONEYASSIGNDETAIL entRd = dalPersonmoneyAssignDetail.GetPersonmoneyAssignDetailRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }


        /// <summary>
        /// 获取T_FB_PERSONMONEYASSIGNDETAIL信息
        /// </summary>
        /// <param name="strPersonmoneyAssignDetailId">主表主键ID</param>
        /// <returns></returns>
        public List<T_FB_PERSONMONEYASSIGNDETAIL> GetPersonmoneyAssignDetailByMasterID(string strMasterID)
        {
            if (string.IsNullOrEmpty(strMasterID))
            {
                return null;
            }
            PersonmoneyAssignDetailDAL dalPersonmoneyAssignDetail = new PersonmoneyAssignDetailDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strMasterID))
            {
                strFilter.Append(" T_FB_PERSONMONEYASSIGNMASTER.PERSONMONEYASSIGNMASTERID == @0");
                objArgs.Add(strMasterID);
            }
            string strOrderBy = " PERSONBUDGETAPPLYDETAILID ";

            var entRd = dalPersonmoneyAssignDetail.GetPersonmoneyAssignDetailRdListByMultSearch(strOrderBy, strFilter.ToString(), objArgs.ToArray());
            return entRd.ToList();
        }

        /// <summary>
        /// 根据条件，获取T_FB_PERSONMONEYASSIGNDETAIL信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_PERSONMONEYASSIGNDETAIL> GetAllPersonmoneyAssignDetailRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            PersonmoneyAssignDetailDAL dalPersonmoneyAssignDetail = new PersonmoneyAssignDetailDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " PERSONBUDGETAPPLYDETAILID ";
            }

            var q = dalPersonmoneyAssignDetail.GetPersonmoneyAssignDetailRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_PERSONMONEYASSIGNDETAIL信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_PERSONMONEYASSIGNDETAIL信息</returns>
        public IQueryable<T_FB_PERSONMONEYASSIGNDETAIL> GetPersonmoneyAssignDetailRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllPersonmoneyAssignDetailRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_PERSONMONEYASSIGNDETAIL>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion

    }
}

