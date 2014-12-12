
/*
 * 文件名：CompanyTransferMasterBLL.cs
 * 作  用：T_FB_COMPANYTRANSFERMASTER 业务逻辑类
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
    public class CompanyTransferMasterBLL
    {
        public CompanyTransferMasterBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_COMPANYTRANSFERMASTER信息
        /// </summary>
        /// <param name="strCompanyTransferMasterId">主键索引</param>
        /// <returns></returns>
        public T_FB_COMPANYTRANSFERMASTER GetCompanyTransferMasterByID(string strCompanyTransferMasterId)
        {
            if (string.IsNullOrEmpty(strCompanyTransferMasterId))
            {
                return null;
            }

            CompanyTransferMasterDAL dalCompanyTransferMaster = new CompanyTransferMasterDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            
            if (!string.IsNullOrEmpty(strCompanyTransferMasterId))
            {
                strFilter.Append(" COMPANYTRANSFERMASTERID == @0");
                objArgs.Add(strCompanyTransferMasterId);
            }

            T_FB_COMPANYTRANSFERMASTER entRd = dalCompanyTransferMaster.GetCompanyTransferMasterRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取T_FB_COMPANYTRANSFERMASTER信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_COMPANYTRANSFERMASTER> GetAllCompanyTransferMasterRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            CompanyTransferMasterDAL dalCompanyTransferMaster = new CompanyTransferMasterDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " COMPANYTRANSFERMASTERID ";
            }

            var q = dalCompanyTransferMaster.GetCompanyTransferMasterRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_COMPANYTRANSFERMASTER信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_COMPANYTRANSFERMASTER信息</returns>
        public IQueryable<T_FB_COMPANYTRANSFERMASTER> GetCompanyTransferMasterRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllCompanyTransferMasterRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_COMPANYTRANSFERMASTER>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion

    }
}

