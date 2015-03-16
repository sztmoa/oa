/*
 * 文件名：WfPersonAccountBLL.cs
 * 作  用：T_FB_WFPERSONACCOUNT 业务逻辑类
 * 创建人：朱磊
 * 创建时间：2012-1-14 16:27
 * 修改人：
 * 修改时间：
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
using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.DAL;
using SMT.FBAnalysis.CustomModel;
using SMT.Foundation.Log;
using SMT.SaaS.BLLCommonServices.FlowWFService;


namespace SMT.FBAnalysis.BLL
{
    class WfPersonAccountBLL:BaseBll<T_FB_WFPERSONACCOUNT>
    {
        public WfPersonAccountBLL()
        { }

        #region 操作数据  zl
        /// <summary>
        /// 添加T_FB_WFPERSONACCOUNT表数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddWfPersonAccount(T_FB_WFPERSONACCOUNT entity)
        {
            bool re;
            try
            {
                dal.BeginTransaction();
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
                string ErrInfo = this.GetType().ToString() + "：AddWfPersonAccount，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }

        #endregion
    }
}
