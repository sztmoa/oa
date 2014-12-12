
/*
 * 文件名：RepayApplyMasterDAL.cs
 * 作  用：T_FB_REPAYAPPLYMASTER 数据操作类
 * 创建人：吴鹏
 * 创建时间：2010-12-15 9:52:22
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
using System.Data;

namespace SMT.FBAnalysis.DAL
{
    public class RepayApplyMasterDAL : CommDal<T_FB_REPAYAPPLYMASTER>
    {
        public static SMT.SaaS.BLLCommonServices.Utility UtilityClass = new SaaS.BLLCommonServices.Utility();

        public RepayApplyMasterDAL()
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
        /// 获取指定条件的T_FB_REPAYAPPLYMASTER信息
        /// </summary>        
        /// <param name="strFilter">查询语句</param>
        /// <param name="objArgs">查询参数集合</param>
        /// <returns>返回T_FB_REPAYAPPLYMASTER信息</returns>
        public T_FB_REPAYAPPLYMASTER GetRepayApplyMasterRdByMultSearch(string strFilter, params object[] objArgs)
        {
            var q = from v in GetObjects().Include("T_FB_BORROWAPPLYMASTER")
                    select v;

            if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
            {
                q = q.Where(strFilter, objArgs);
            }

            T_FB_REPAYAPPLYMASTER entRes = q.First();
            if (!entRes.T_FB_REPAYAPPLYDETAIL.IsLoaded)
            {
                entRes.T_FB_REPAYAPPLYDETAIL.Load();
            }

            return entRes;
        }

        /// <summary>
        /// 根据查询条件，获取还款信息(主要用于查询分页)
        /// </summary>
        /// <param name="strCheckStates">查询审核状态</param>
        /// <param name="strDateStart">查询起始时间</param>
        /// <param name="strDateEnd">查询截止时间</param>
        /// <param name="strOrderBy">排序字段</param>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <returns>还款信息</returns>
        public IQueryable<T_FB_REPAYAPPLYMASTER> GetRepayApplyMasterRdListByMultSearch(string strCheckStates, string strDateStart, 
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

        public DataTable GetRepayRdList(string strSql)
        {
            if (string.IsNullOrWhiteSpace(strSql))
            {
                return null;
            }

            DataTable dtRes = new DataTable();

            dtRes = this.GetDataTableByCustomerSql(strSql);
            return dtRes;
        }
    }
}

