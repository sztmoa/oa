using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMT.FBAnalysis.BLL;
using System.ServiceModel;
using SMT_FB_EFModel;
using SMT.FBAnalysis.CustomModel;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq.Expressions;
using System.Collections.ObjectModel;


namespace SMT.FBAnalysis.Service
{

    public partial class DailyManagementServices
    {

        #region 查询子表主表
        /// <summary>查询子表、主表数据
        /// 查询子表、主表数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_BORROWAPPLYDETAIL> GetInfo()
        {
            BorrowApplyDetailBLL bll = new BorrowApplyDetailBLL();
            List<T_FB_BORROWAPPLYDETAIL> detailList = null;
            detailList = bll.GetInfo();
            return (detailList != null && detailList.Count() > 0) ? detailList : null;
        }
        #endregion

        #region

        /// <summary>
        /// 获取借款记录用于冲借款或还款
        /// </summary>
        /// <param name="strOwnerID"></param>
        /// <param name="dIsRepaied"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_BORROWAPPLYMASTER> GetBorrowApplyMasterListForRepay(string strOwnerID, decimal dIsRepaied, decimal dCheckStates, string strFilter, List<object> objArgs)
        {
            IQueryable<T_FB_BORROWAPPLYMASTER> entIq;
            IList<T_FB_BORROWAPPLYMASTER> entList = new List<T_FB_BORROWAPPLYMASTER>();
            BorrowApplyMasterBLL bllBorrowApplyMaster = new BorrowApplyMasterBLL();
            entIq = bllBorrowApplyMaster.GetBorrowApplyMasterListForRepay(strOwnerID, dIsRepaied, dCheckStates, strFilter, objArgs);
            if (entIq == null)
            {
                return null;
            }

            entList = entIq.ToList();
            return entList != null ? entList.ToList() : null;
        }

        [OperationContract]
        public IQueryable<T_FB_BORROWAPPLYDETAIL> GetBorrowApps(int pageIndex, int pageSize, string sort, string filterString, object[] paras, ref int pageCount, string userId, List<string> guidStringList, string checkState)
        {
            BorrowApplyDetailBLL bll = new BorrowApplyDetailBLL();
            IQueryable<T_FB_BORROWAPPLYDETAIL> infoMasterList = null;
            infoMasterList = bll.GetBorrowApps(pageIndex, pageSize, sort, filterString, paras, ref pageCount, userId, guidStringList, checkState);
            if (infoMasterList != null && infoMasterList.Count() > 0)
            {
                return infoMasterList;
            }
            else
            {
                return null;
            }


        }
        #endregion

        #region  子表相关操作
        /// <summary>
        /// 根据主表主键查询子表，带出主表数据
        /// </summary>
        /// <param name="masterId">主表借款单号ID</param>
        /// <returns>返回子表与主表的数据</returns>
        [OperationContract]
        public T_FB_BORROWAPPLYDETAIL GetInfoById(string masterId)
        {
            BorrowApplyDetailBLL bll = new BorrowApplyDetailBLL();
            T_FB_BORROWAPPLYDETAIL DetialList = bll.GetInfoById(masterId);
            return DetialList != null ? DetialList : null;
        }
        #endregion

        #region 增删改查

        #region 管理页面查询
        /// <summary> 
        /// 根据用户ID查询主表子表
        /// </summary>
        /// <param name="EmployeeID">当前用户ID</param>
        /// <returns>返回主表与子表的序列</returns>
        [OperationContract]
        public List<T_FB_BORROWAPPLYMASTER> GetMasterData(string employeeID)
        {
            List<T_FB_BORROWAPPLYMASTER> masterList = null;
            if (!string.IsNullOrEmpty(employeeID))
            {
                BorrowApplyMasterBLL bll = new BorrowApplyMasterBLL();
                masterList = bll.GetMasterData(employeeID);
            }
            else
            {
                masterList = null;
            }
            return masterList != null && masterList.Count() > 0 ? masterList : null;
        }
        #endregion

        #region
        /// <summary>
        /// 根据主键ID查询主表子表
        /// </summary>
        /// <param name="borrowKey">主键ID</param>
        /// <returns>返回唯一一条符合条件的记录</returns>
        [OperationContract]
        public T_FB_BORROWAPPLYMASTER GetChildData(string borrowKey)
        {
            T_FB_BORROWAPPLYMASTER borrowEntity;
            if (!string.IsNullOrEmpty(borrowKey))
            {
                BorrowApplyMasterBLL bll = new BorrowApplyMasterBLL();
                borrowEntity = bll.GetChildData(borrowKey);
            }
            else
            {
                borrowEntity = null;
            }
            return borrowEntity != null ? borrowEntity : null;
        }
        #endregion

        #region 新增
        /// <summary>
        /// 新增借款单
        /// </summary>
        /// <param name="masterKey">主表带上子表信息</param>
        /// <returns>返回bool值表示是否添加成功</returns>
        [OperationContract]
        public bool AddBorrowApply(T_FB_BORROWAPPLYMASTER masterKey)
        {
            bool flag = false;
            if (masterKey != null)
            {
                BorrowApplyMasterBLL bll = new BorrowApplyMasterBLL();
                flag = bll.AddBorrowApply(masterKey);

            }
            else
            {
                flag = false;

            }
            return flag;
        }
        #endregion

        #region 修改
        /// <summary>
        /// 修改借款单
        /// </summary>
        /// <param name="updataKey">要修改的数据</param>
        /// <returns>返回bool值表示是否修改成功</returns>
        [OperationContract]
        public bool UpdBorrowApply(T_FB_BORROWAPPLYMASTER updataKey)
        {
            bool flag = false;
            if (updataKey != null)
            {
                BorrowApplyMasterBLL bll = new BorrowApplyMasterBLL();
                flag = bll.UpdBorrowApply(updataKey);
            }
            else
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        #region 主表批量删除
        /// <summary>删除服务
        /// 管理页面DataGrid删除服务
        /// </summary>
        /// <param name="masterList">主表主键ID集合</param>
        /// <returns>返回bool表示是否删除成功</returns>
        [OperationContract]
        public bool DelMasterDataById(ObservableCollection<string> masterList)
        {
            bool flag = false;
            if ((masterList != null) && (masterList.Count() > 0))
            {
                BorrowApplyMasterBLL bll = new BorrowApplyMasterBLL();
                flag = bll.DelMasterDataById(masterList);
            }
            else
            {
                flag = false;
            }
            return flag;
        }
        #endregion

        #endregion

        #region 移动的服务
        #region T_FB_BORROWAPPLYDETAIL 服务

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strBorrowApplyDetailId">主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public T_FB_BORROWAPPLYDETAIL GetBorrowApplyDetailByID(string strBorrowApplyDetailId)
        {
            T_FB_BORROWAPPLYDETAIL entRd = new T_FB_BORROWAPPLYDETAIL();
            using(BorrowApplyDetailBLL bllBorrowApplyDetail = new BorrowApplyDetailBLL())
            {
                entRd = bllBorrowApplyDetail.GetBorrowApplyDetailByID(strBorrowApplyDetailId);
                return entRd;
            }
        }

        /// <summary>
        /// 获取信息   add by zl
        /// </summary>
        /// <param name="strBorrowApplyMasterId">主表主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_BORROWAPPLYDETAIL> GetBorrowApplyDetailByMasterID(List<object> objBorrowApplyMasterId)
        {
            List<T_FB_BORROWAPPLYDETAIL> entRdlist = new List<T_FB_BORROWAPPLYDETAIL>();
            using(BorrowApplyDetailBLL bllBorrowApplyDetail = new BorrowApplyDetailBLL())
            {
                entRdlist = bllBorrowApplyDetail.GetBorrowApplyDetailByMasterID(objBorrowApplyMasterId);
                return entRdlist;
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strBorrowApplyDetailName">名称</param>
        /// <param name="strCompanyID">公司序号</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<T_FB_BORROWAPPLYDETAIL> GetBorrowApplyDetailListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            IList<T_FB_BORROWAPPLYDETAIL> entList = new List<T_FB_BORROWAPPLYDETAIL>();
            using(BorrowApplyDetailBLL bllBorrowApplyDetail = new BorrowApplyDetailBLL())
            {
                entList = bllBorrowApplyDetail.GetBorrowApplyDetailRdListByMultSearch(strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount).ToList();
                return entList.Count() > 0 ? entList.ToList() : null;
            }
        }

        #endregion

        #region T_FB_BORROWAPPLYMASTER 服务

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strBorrowApplyMasterId">主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public T_FB_BORROWAPPLYMASTER GetBorrowApplyMasterByID(string strBorrowApplyMasterId)
        {
            T_FB_BORROWAPPLYMASTER entRd = new T_FB_BORROWAPPLYMASTER();
            using(BorrowApplyMasterBLL bllBorrowApplyMaster = new BorrowApplyMasterBLL())
            {
                entRd = bllBorrowApplyMaster.GetBorrowApplyMasterByID(strBorrowApplyMasterId);
                return entRd;
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strBorrowApplyMasterName">名称</param>
        /// <param name="strCompanyID">公司序号</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<T_FB_BORROWAPPLYMASTER> GetBorrowApplyMasterListByMultSearch(string userID, string strDateStart, string strDateEnd,
            string checkState, string strFilter, List<object> objArgs, string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            IQueryable<T_FB_BORROWAPPLYMASTER> entIq;
            IList<T_FB_BORROWAPPLYMASTER> entList = new List<T_FB_BORROWAPPLYMASTER>();
            using(BorrowApplyMasterBLL bllBorrowApplyMaster = new BorrowApplyMasterBLL())
            {
                entIq = bllBorrowApplyMaster.GetBorrowApplyMasterRdListByMultSearch(userID, strDateStart, strDateEnd,
                            checkState, strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount);
                if (entIq == null)
                {
                    return null;
                }

                entList = entIq.ToList();
                return entList != null ? entList.ToList() : null;
            }
        }

        #endregion
        #endregion

        #region T_FB_BORROWAPPLYMASTER 服务

        /// <summary>
        /// 增加借款主表和明细表数据  add by zl
        /// </summary>
        /// <param name="borrowMaster"></param>
        /// <param name="borrowDetail"></param>
        /// <returns></returns>
        [OperationContract]
        public bool AddBorrowApplyMasterAndDetail(T_FB_BORROWAPPLYMASTER borrowMaster, List<T_FB_BORROWAPPLYDETAIL> borrowDetail)
        {
            using(BorrowApplyMasterBLL borrowBLL = new BorrowApplyMasterBLL())
            {
                bool re;
                re = borrowBLL.AddBorrowApplyMasterAndDetail(borrowMaster, borrowDetail);
                return re;
            }
        }

        /// <summary>
        /// 更新借款主表和明细表数据  add by zl
        /// </summary>
        /// <param name="repayMaster"></param>
        /// <param name="repayDetail"></param>
        /// <returns></returns>
        [OperationContract]
        public void UptBorrowApplyMasterAndDetail(string strActionType, T_FB_BORROWAPPLYMASTER borrowMaster, 
            List<T_FB_BORROWAPPLYDETAIL> borrowDetails, ref string strMsg)
        {
            using(BorrowApplyMasterBLL borrowBLL = new BorrowApplyMasterBLL())
            {
                borrowBLL.UptBorrowApplyMasterAndDetail(strActionType, borrowMaster, borrowDetails, ref strMsg);
            }
        }

        /// <summary>
        /// 删除借款主明细表数据   add by zl
        /// </summary>
        /// <param name="borrowMasterID"></param>
        /// <returns></returns>
        [OperationContract]
        public bool DelBorrowApplyMasterAndDetail(List<string> borrowMasterID)
        {
           using(BorrowApplyMasterBLL borrowBLL = new BorrowApplyMasterBLL())
           {
               foreach (string obj in borrowMasterID)
               {
                   if (!borrowBLL.DelBorrowApplyMasterAndDetail(obj))
                   {
                       return false;
                   }
               }
               return true;
           }
        }

        /// <summary>
        /// 审核流程时更新借款主表CHECKSTATES字段值 add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="borrowDetail"></param>
        /// <returns></returns>
        [OperationContract]
        public bool UptBorrowApplyCheckState(T_FB_BORROWAPPLYMASTER entity, List<T_FB_BORROWAPPLYDETAIL> borrowDetail)
        {
            using(BorrowApplyMasterBLL borrowBLL = new BorrowApplyMasterBLL())
            {
                bool re = true;
                string sMsg = borrowBLL.UptBorrowApplyCheckState(entity, borrowDetail);
                if (!string.IsNullOrEmpty(sMsg))
                {
                    re = false;
                }
                return re;
            }
        }

        /// <summary>
        /// 获取单据编号 add by zl
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [OperationContract]
        public string GetBorrowOrderCode(T_FB_BORROWAPPLYMASTER entity)
        {
            return new OrderCodeBLL().GetAutoOrderCode(entity);
        }

        #endregion

        #region T_FB_BUDGETACCOUNT 服务

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strBudgetAccountId">主键索引</param>
        /// <returns></returns>
        [OperationContract]
        public T_FB_BUDGETACCOUNT GetBudgetAccountByID(string strBudgetAccountId)
        {
            T_FB_BUDGETACCOUNT entRd = new T_FB_BUDGETACCOUNT();
            using(BudgetAccountBLL bllBudgetAccount = new BudgetAccountBLL())
            {
                entRd = bllBudgetAccount.GetBudgetAccountByID(strBudgetAccountId);
                return entRd;
            }
        }

        /// <summary>
        /// 查询与个人有关的所有可用预算（个人和部门公共部门)
        /// </summary>
        /// <param name="strOwnerID">员工ID</param>
        /// <param name="strOwnerPostID">员工所在岗位ID</param>
        /// <param name="strOwnerDepID">员工所在部门ID</param>
        /// <param name="strOwnerCompanyID">员工所在公司</param>
        /// <returns></returns>
        [OperationContract]
        public List<T_FB_BUDGETACCOUNT> GetBudgetAccountByPerson(string strOwnerID, string strOwnerPostID, string strOwnerCompanyID)
        {
            using(BudgetAccountBLL bllBudgetAccount = new BudgetAccountBLL())
            {
                List<T_FB_BUDGETACCOUNT> entList = bllBudgetAccount.GetBudgetAccountByPerson(strOwnerID, strOwnerPostID, strOwnerCompanyID);

                return entList.Count() > 0 ? entList : null;
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="strBudgetAccountName">名称</param>
        /// <param name="strCompanyID">公司序号</param>        
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示记录数</param>
        /// <param name="pageCount">总页码</param>
        /// <returns>返回信息</returns>
        [OperationContract]
        public List<T_FB_BUDGETACCOUNT> GetBudgetAccountListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount, string accountType)
        {
            IList<T_FB_BUDGETACCOUNT> entList = new List<T_FB_BUDGETACCOUNT>();
            using(BudgetAccountBLL bllBudgetAccount = new BudgetAccountBLL())
            {
                entList = bllBudgetAccount.GetBudgetAccountRdListByMultSearch(strFilter, objArgs, strSortKey, pageIndex, pageSize, ref pageCount, accountType).ToList();
                return entList.Count() > 0 ? entList.ToList() : null;
            }   
        }
        #endregion

        #region 手机版使用
        /// <summary>
        /// 添加借款申请记录
        /// </summary>
        /// <param name="borrowMaster">借款申请记录</param>
        /// <param name="borrowDetail">借款记录子表集合</param>
        /// <param name="strMsg">返回提示信息：成功为空</param>
        /// <returns>成功返回true,失败false</returns>
        [OperationContract]
        public bool AddBorrowApplyMasterAndDetailForMobile(T_FB_BORROWAPPLYMASTER borrowMaster, List<T_FB_BORROWAPPLYDETAIL> borrowDetail,ref string strMsg)
        {
            using (BorrowApplyMasterBLL borrowBLL = new BorrowApplyMasterBLL())
            {
                SMT.Foundation.Log.Tracer.Debug("调用了AddBorrowApplyMasterAndDetailForMobile");
                bool re;
                re = borrowBLL.AddBorrowApplyMasterAndDetailForMobile(borrowMaster, borrowDetail, ref strMsg);
                return re;
            }
        }

        /// <summary>
        /// 修改借款申请记录
        /// </summary>
        /// <param name="strActionType">字符串：RESUBMIT 重新提交；EDIT 修改</param>
        /// <param name="borrowMaster">借款主表记录</param>
        /// <param name="borrowDetails">借款子表记录集合</param>
        /// <param name="strMsg">返回提示信息：成功为空</param>
        [OperationContract]
        public void UptBorrowApplyMasterAndDetailForMobile(string strActionType, T_FB_BORROWAPPLYMASTER borrowMaster,
            List<T_FB_BORROWAPPLYDETAIL> borrowDetails, ref string strMsg)
        {
            using (BorrowApplyMasterBLL borrowBLL = new BorrowApplyMasterBLL())
            {
                
                borrowBLL.UptBorrowApplyMasterAndDetailForMobile(strActionType, borrowMaster, borrowDetails, ref strMsg);
            }
        }

        /// <summary>
        /// 获取借款申请元数据
        /// </summary>
        /// <param name="formid">主表ID</param>
        /// <returns>返回填充后的记录</returns>
        [OperationContract]
        public string GetBorrowApplyMasterXMLString(string formid, ref string BorrowCode)
        {
            using (BorrowApplyMasterBLL borrowBLL = new BorrowApplyMasterBLL())
            {
                return borrowBLL.GetXmlString(formid,ref BorrowCode);
            }
        }


        #endregion
    }
}
