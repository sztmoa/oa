
/*
 * 文件名：DeptBudgetApplyMasterBLL.cs
 * 作  用：T_FB_DEPTBUDGETAPPLYMASTER 业务逻辑类
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
using SMT.FBAnalysis.CustomModel;

namespace SMT.FBAnalysis.BLL
{
    public class DeptBudgetApplyMasterBLL
    {
        public DeptBudgetApplyMasterBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_DEPTBUDGETAPPLYMASTER信息
        /// </summary>
        /// <param name="strDeptBudgetApplyMasterId">主键索引</param>
        /// <returns></returns>
        public T_FB_DEPTBUDGETAPPLYMASTER GetDeptBudgetApplyMasterByID(string strDeptBudgetApplyMasterId)
        {
            if (string.IsNullOrEmpty(strDeptBudgetApplyMasterId))
            {
                return null;
            }

            DeptBudgetApplyMasterDAL dalDeptBudgetApplyMaster = new DeptBudgetApplyMasterDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            
            if (!string.IsNullOrEmpty(strDeptBudgetApplyMasterId))
            {
                strFilter.Append(" DEPTBUDGETAPPLYMASTERID == @0");
                objArgs.Add(strDeptBudgetApplyMasterId);
            }

            T_FB_DEPTBUDGETAPPLYMASTER entRd = dalDeptBudgetApplyMaster.GetDeptBudgetApplyMasterRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取T_FB_DEPTBUDGETAPPLYMASTER信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_DEPTBUDGETAPPLYMASTER> GetAllDeptBudgetApplyMasterRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            DeptBudgetApplyMasterDAL dalDeptBudgetApplyMaster = new DeptBudgetApplyMasterDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " DEPTBUDGETAPPLYMASTERID ";
            }

            var q = dalDeptBudgetApplyMaster.GetDeptBudgetApplyMasterRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_DEPTBUDGETAPPLYMASTER信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_DEPTBUDGETAPPLYMASTER信息</returns>
        public IQueryable<T_FB_DEPTBUDGETAPPLYMASTER> GetDeptBudgetApplyMasterRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllDeptBudgetApplyMasterRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_DEPTBUDGETAPPLYMASTER>(q, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 查询部门预算单。
        /// </summary>
        /// <param name="conditions">查询条件对象集。</param>
        /// <returns>返回部门预算金额。</returns>
        public static IQueryable<V_Money> GetDeptBudgetApply(ExecutionConditions conditions)
        {
            DeptBudgetApplyMasterDAL dalDeptBudgetApplyMaster = new DeptBudgetApplyMasterDAL();

            return dalDeptBudgetApplyMaster.GetDeptBudgetApply(conditions);
        }
        #endregion

    }
}

