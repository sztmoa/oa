
/*
 * 文件名：BudgetAccountBLL.cs
 * 作  用：T_FB_BUDGETACCOUNT 业务逻辑类
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
using SMT.FBAnalysis.CustomModel;
using System.Reflection;
using SMT.Foundation.Log;

namespace SMT.FBAnalysis.BLL
{
    public class BudgetAccountBLL : BaseBll<T_FB_BUDGETACCOUNT>
    {
        public const string SYSTEM_USER_ID = "001";
        public BudgetAccountBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_BUDGETACCOUNT信息
        /// </summary>
        /// <param name="strBudgetAccountId">主键索引</param>
        /// <returns></returns>
        public T_FB_BUDGETACCOUNT GetBudgetAccountByID(string strBudgetAccountId)
        {
            if (string.IsNullOrEmpty(strBudgetAccountId))
            {
                return null;
            }

            BudgetAccountDAL dalBudgetAccount = new BudgetAccountDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strBudgetAccountId))
            {
                strFilter.Append(" BUDGETACCOUNTID == @0");
                objArgs.Add(strBudgetAccountId);
            }

            T_FB_BUDGETACCOUNT entRd = dalBudgetAccount.GetBudgetAccountRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 查询与个人有关的所有可用预算（个人和部门公共部门)
        /// </summary>
        /// <param name="strOwnerID">员工ID</param>
        /// <param name="strOwnerPostID">员工所在岗位ID</param>
        /// <param name="strOwnerDepID">员工所在部门ID</param>
        /// <param name="strOwnerCompanyID">员工所在公司</param>
        /// <returns></returns>
        public List<T_FB_BUDGETACCOUNT> GetBudgetAccountByPerson(string strOwnerID, string strOwnerPostID, string strOwnerCompanyID)
        {
            BudgetAccountDAL dalBudgetAccount = new BudgetAccountDAL();

            var q = dalBudgetAccount.GetBudgetAccountByPerson(strOwnerID, strOwnerPostID, strOwnerCompanyID);
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_BUDGETACCOUNT信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_BUDGETACCOUNT> GetAllBudgetAccountRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            BudgetAccountDAL dalBudgetAccount = new BudgetAccountDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " BUDGETACCOUNTID ";
            }

            var q = dalBudgetAccount.GetBudgetAccountRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_BUDGETACCOUNT信息  add by zl
        /// </summary>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_BUDGETACCOUNT> GetAllBudgetAccountRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey, string accountType)
        {
            BudgetAccountDAL dalBudgetAccount = new BudgetAccountDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " BUDGETACCOUNTID ";
            }

            var q = dalBudgetAccount.GetBudgetAccountRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_BUDGETACCOUNT信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_BUDGETACCOUNT信息</returns>
        public IQueryable<T_FB_BUDGETACCOUNT> GetBudgetAccountRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllBudgetAccountRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_BUDGETACCOUNT>(q, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 根据条件，获取T_FB_BUDGETACCOUNT信息,并进行分页  add by zl
        /// </summary>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <param name="strSortKey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageCount"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public IQueryable<T_FB_BUDGETACCOUNT> GetBudgetAccountRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount, string accountType)
        {
            var q = GetAllBudgetAccountRdListByMultSearch(strFilter, objArgs, strSortKey, accountType);

            return Utility.Pager<T_FB_BUDGETACCOUNT>(q, pageIndex, pageSize, ref pageCount);
        }

        /// <summary>
        /// 计算年度预算总额。
        /// </summary>
        /// 查询条件对象集。
        /// <returns>返回年度预算总额。</returns>
        public List<V_Money> GetBudgetAccountAndCheckMoney(ExecutionConditions conditions)
        {
            BudgetAccountDAL dalBudgetAccount = new BudgetAccountDAL();
            List<V_Money> list = dalBudgetAccount.GetBudgetAccountAndCheckMoney(conditions);

            return list;
        }
        #endregion

        #region 报销数据检查  zl
        /// <summary>
        /// 个人报销单审核时检查报销金额是否大于对应科目的实际结余  2012.1.12
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <param name="dCheckState"></param>
        /// <returns></returns>
        public string CheckChaMoneyForCharge(T_FB_CHARGEAPPLYMASTER entity, List<T_FB_CHARGEAPPLYDETAIL> detailList, decimal dCheckState)
        {
            string sResult = "";
            string ErrInfo = "";
            T_FB_BUDGETACCOUNT entBudge = new T_FB_BUDGETACCOUNT();
            try
            {
                if (dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approving) || dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approved))
                {
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 报销单ID：" + entity.CHARGEAPPLYMASTERID;
                    foreach (T_FB_CHARGEAPPLYDETAIL da in detailList)
                    {
                        if (da.USABLEMONEY == 999999 || da.USABLEMONEY == 99999999)
                        {
                            continue;
                        }
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
                            if(a==null || a.Count()==0)
                            {
                                ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 报销单ID：" + entity.CHARGEAPPLYMASTERID + ", T_FB_BUDGETACCOUNT表中找不到相关数据。";
                                Tracer.Debug(ErrInfo);
                                sResult = "没有找到相应的预算科目数据！";
                                return sResult;
                            }
                            if(da.CHARGEMONEY > a.FirstOrDefault().ACTUALMONEY)
                            {
                                ErrInfo += " 明细ID：" + da.CHARGEAPPLYDETAILID + ", 个人费用报销金额 " + da.CHARGEMONEY + " 大于科目的实际结余 " + a.FirstOrDefault().ACTUALMONEY +" 审核终止 ||";
                                Tracer.Debug(ErrInfo);
                                sResult = "报销金额不能大于月度预算结余，请先增加月度预算后再操作。";
                                return sResult;
                            }
                        }
                        else if (da.CHARGETYPE == 2)
                        {
                            var a = from i in dal.GetObjects<T_FB_BUDGETACCOUNT>()
                                    where i.OWNERCOMPANYID == da.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID
                                    && i.OWNERDEPARTMENTID == da.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID
                                    && i.T_FB_SUBJECT.SUBJECTID == da.T_FB_SUBJECT.SUBJECTID
                                    && i.ACCOUNTOBJECTTYPE == 2
                                    select i;
                            if (a == null || a.Count() == 0)
                            {
                                ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 报销单ID：" + entity.CHARGEAPPLYMASTERID + ", T_FB_BUDGETACCOUNT表中找不到相关数据。";
                                Tracer.Debug(ErrInfo);
                                sResult = "没有找到相应的预算科目数据！";
                                return sResult;
                            }
                            if (da.CHARGEMONEY > a.FirstOrDefault().ACTUALMONEY)
                            {
                                ErrInfo += " 明细ID：" + da.CHARGEAPPLYDETAILID + ", 个人费用报销金额 " + da.CHARGEMONEY + " 大于科目的实际结余 " + a.FirstOrDefault().ACTUALMONEY + " 审核终止 ||";
                                Tracer.Debug(ErrInfo);
                                sResult = "报销金额不能大于月度预算结余，请先增加月度预算后再操作。";
                                return sResult;
                            }
                        }
                    }
                }
                return sResult;
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：CheckChaMoneyForCharge，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                sResult = "报销金额检查程序异常，审核终止！";
                return sResult;
            }
        }
         
        #endregion

        #region 数据操作

        /// <summary>
        /// 更新预算总账
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dCheckState"></param>
        /// <returns></returns>
        public bool UpdateAccount(EntityObject entity, decimal dCheckState)
        {
            bool tempValue = true;
            Type entityType = entity.GetType();

            tempValue &= UpdateBudgetAccount(entity, dCheckState);
            //tempValue &= Repay(entity, dCheckState);

            //if (tempValue && entityType == typeof(T_FB_BORROWAPPLYMASTER))
            //{
            //    tempValue &= UpdateBorrowApply(entity as T_FB_BORROWAPPLYMASTER, dCheckState);
            //}
            return tempValue;
        }

        private bool UpdateCompanyBudgetApply(T_FB_COMPANYBUDGETAPPLYMASTER t_FB_COMPANYBUDGETAPPLYMASTER, decimal dCheckState)
        {
            throw new NotImplementedException();
        }

        private bool UpdateDeptBudgetApply(T_FB_DEPTBUDGETAPPLYMASTER t_FB_DEPTBUDGETAPPLYMASTER, decimal dCheckState)
        {
            throw new NotImplementedException();
        }

        private bool UpdateDeptBudgetSum(T_FB_DEPTBUDGETSUMMASTER t_FB_DEPTBUDGETSUMMASTER, decimal dCheckState)
        {
            throw new NotImplementedException();
        }

        private bool UpdateCompanyBudgetSum(T_FB_COMPANYBUDGETSUMMASTER t_FB_COMPANYBUDGETSUMMASTER, decimal dCheckState)
        {
            throw new NotImplementedException();
        }

        private bool UpdateBudgetAccount(EntityObject entity, decimal dCheckState)
        {
            //zl 11.10
            T_FB_CHARGEAPPLYMASTER cha = new T_FB_CHARGEAPPLYMASTER();
            T_FB_BORROWAPPLYMASTER bor = new T_FB_BORROWAPPLYMASTER();
            T_FB_REPAYAPPLYMASTER rep = new T_FB_REPAYAPPLYMASTER();
            
            string masid = entity.EntityKey.ToString();
            string tab = "";
            string Log = "";
            try
            {
                switch (entity.GetType().Name)
                {
                    case "T_FB_CHARGEAPPLYMASTER":
                        cha = entity as T_FB_CHARGEAPPLYMASTER;
                        masid = cha.CHARGEAPPLYMASTERID;
                        tab = "费用报销";
                        break;
                    case "T_FB_BORROWAPPLYMASTER":
                        bor = entity as T_FB_BORROWAPPLYMASTER;
                        masid = bor.BORROWAPPLYMASTERID;
                        tab = "个人借款";
                        break;
                    case "T_FB_REPAYAPPLYMASTER":
                        rep = entity as T_FB_REPAYAPPLYMASTER;
                        masid = rep.REPAYAPPLYMASTERID;
                        tab = "个人还款";
                        break;
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("UpdateBudgetAccount出现错误：" + ex.ToString());
            }
            //end
                Tracer.Debug("UpdateBudgetAccount--GetAccountItem：开始" );
            List<AccountItem> listAccountItem = GetAccountItem(entity, AccountType.BudgetAccount);
            Tracer.Debug("UpdateBudgetAccount--listAccountItem：数量" + listAccountItem.Count());
            //zl 11.10
            string Msg = entity.GetType().Name +","+ System.DateTime.Now.ToString() + "," + tab + "更新预算总账表，表单ID:" + masid + ",";
            string ErrInfo = "所更改科目:";
            //end
            listAccountItem.ForEach(accountItem =>
                                        {
                                            T_FB_BUDGETACCOUNT budgetAccountItem = FindBudgetAccout(accountItem);
                                            if(budgetAccountItem.T_FB_SUBJECT == null)
                                            {
                                                Tracer.Debug("UpdateBudgetAccount--budgetAccountItem subject为空");
                                            }
                                            else
                                            {
                                                Tracer.Debug("UpdateBudgetAccount--budgetAccountItem" + budgetAccountItem.T_FB_SUBJECT.SUBJECTID);
                                            }
                                            // 增加预算, 且终审通过
                                            if (accountItem.AccountOpertaion == AccountOpertaion.Add)
                                            {
                                                if (dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approved))
                                                    // 审核通过
                                                {
                                                    budgetAccountItem.USABLEMONEY += accountItem.moneyUsable;
                                                    budgetAccountItem.BUDGETMONEY += accountItem.moenyBudget;
                                                    budgetAccountItem.ACTUALMONEY += accountItem.moneyActual;
                                                }
                                            }
                                            else if (accountItem.AccountOpertaion == AccountOpertaion.Subtract) // 减少预算费用
                                            {
                                                if (dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approving))
                                                    // 提交审核
                                                {
                                                    if (accountItem.moneyUsable == 0) // 有些情况是不需要修改
                                                    {
                                                        return;
                                                    }

                                                    if (accountItem.moneyUsable != null)
                                                    {
                                                        //zl 11.10
                                                        ErrInfo = ErrInfo + budgetAccountItem.T_FB_SUBJECT.SUBJECTID +
                                                                  ",提交审核扣减可用金额：" + accountItem.moneyUsable.Value + " ";
                                                        //end
                                                        budgetAccountItem.USABLEMONEY += 0 -
                                                                                         accountItem.moneyUsable.
                                                                                             Value;
                                                    }
                                                }
                                                else if (dCheckState ==
                                                         Convert.ToDecimal(FBAEnums.CheckStates.Approved)) // 审核通过
                                                {
                                                    if (accountItem.moenyBudget != null)
                                                    {
                                                        budgetAccountItem.BUDGETMONEY += 0 -
                                                                                         accountItem.moenyBudget
                                                                                             .Value;
                                                    }

                                                    if (accountItem.moneyActual != null)
                                                    {
                                                        //zl 11.10
                                                        ErrInfo = ErrInfo +
                                                                  budgetAccountItem.T_FB_SUBJECT.SUBJECTID +
                                                                  ",审核通过扣减实际金额：" + accountItem.moneyActual.Value +
                                                                  " ";
                                                        //end
                                                        budgetAccountItem.ACTUALMONEY += 0 -
                                                                                         accountItem.moneyActual
                                                                                             .Value;
                                                        

                                                    }
                                                    ////add zl 2012.1.14
                                                    //if (accountItem.moneyUsable != null)
                                                    //{
                                                    //    ErrInfo = ErrInfo + budgetAccountItem.T_FB_SUBJECT.SUBJECTID +
                                                    //              ",审核通过扣减可用金额：" + accountItem.moneyUsable.Value + " ";

                                                    //    budgetAccountItem.USABLEMONEY += 0 -
                                                    //                                     accountItem.moneyUsable.
                                                    //                                         Value;
                                                    //}
                                                    ////add end
                                                    if (accountItem.moneyPaid != null)
                                                    {
                                                        budgetAccountItem.PAIEDMONEY +=
                                                            accountItem.moneyPaid.Value;
                                                    }

                                                    // 补差价，提交审核时，扣减了可用金额 moneyUsable, 而实际审核批准的金额可能少于申请时扣减的可用金额
                                                    decimal? subMoney = accountItem.moneyUsable -
                                                                        accountItem.moneyActual;
                                                    
                                                    if (subMoney != null)
                                                    {
                                                        budgetAccountItem.USABLEMONEY += subMoney;
                                                    }

                                                }
                                                else if (dCheckState ==
                                                         Convert.ToDecimal(FBAEnums.CheckStates.UnApproved))
                                                    // 审核不通过
                                                {
                                                    if (accountItem.moneyUsable == 0) // 有些情况是不需要修改
                                                    {
                                                        return;
                                                    }

                                                    if (accountItem.moneyUsable != null)
                                                    {
                                                        //zl 11.10
                                                        ErrInfo = ErrInfo +
                                                                  budgetAccountItem.T_FB_SUBJECT.SUBJECTID +
                                                                  ",审核不通过增加可用金额：" + accountItem.moneyUsable.Value +
                                                                  " ";
                                                        //end
                                                        budgetAccountItem.USABLEMONEY +=
                                                            accountItem.moneyUsable.Value;
                                                    }
                                                }
                                            }

                                            // 检查规则
                                            if (accountItem.CheckRule != null)
                                            {
                                                bool bRulePass = accountItem.CheckRule(budgetAccountItem);
                                                if (!bRulePass)
                                                {
                                                    return;
                                                }
                                            }

                                            int i = SaveBudgetAccount(budgetAccountItem);
                                            if (i > 0)
                                            {
                                                CreateBudgetAccountWaterFlow(budgetAccountItem, accountItem, dCheckState);
                                                ErrInfo += " 成功|";
                                            }
                                            else
                                            {

                                                ErrInfo += " 失败|";
                                            }
                                        });
            //zl 11.10
            Log = Msg + ErrInfo + "， 审核状态：" + dCheckState;
            Tracer.Debug(Log);
            //end
            return true;
            
            //return false;
        }

        /// <summary>
        /// 写BudgetAccount的流水账   2012.1.17
        /// </summary>
        /// <param name="budEnt"></param>
        /// <param name="entItem"></param>
        /// <returns></returns>
        public bool CreateBudgetAccountWaterFlow(T_FB_BUDGETACCOUNT budgetAccount, AccountItem entItem, decimal checkStates)
        {
            T_FB_WFBUDGETACCOUNT wf = new T_FB_WFBUDGETACCOUNT();

            wf.ORDERID = entItem.OrderID;
            wf.ORDERCODE = entItem.OrderCode;
            wf.ORDERTYPE = entItem.EntityType;
            wf.WFBUDGETACCOUNTID = Guid.NewGuid().ToString();
            wf.CREATEUSERID = "系统";
            wf.UPDATEUSERID = "系统";
            wf.CREATEDATE = System.DateTime.Now;
            wf.UPDATEDATE = System.DateTime.Now;

            wf.ORDERDETAILID = entItem.OrderDetailID;
            wf.OPERATIONMONEY = entItem.OPERATIONMONEY;

            wf.BUDGETACCOUNTID = budgetAccount.BUDGETACCOUNTID;
            wf.ACCOUNTOBJECTTYPE = budgetAccount.ACCOUNTOBJECTTYPE;
            wf.BUDGETYEAR = budgetAccount.BUDGETYEAR;
            wf.BUDGETMONTH = budgetAccount.BUDGETMONTH;
            wf.OWNERCOMPANYID = budgetAccount.OWNERCOMPANYID;
            wf.OWNERDEPARTMENTID = budgetAccount.OWNERDEPARTMENTID;
            wf.OWNERID = budgetAccount.OWNERID;
            wf.OWNERPOSTID = budgetAccount.OWNERPOSTID;
            wf.SUBJECTID = entItem.T_FB_SUBJECT.SUBJECTID;
            wf.BUDGETMONEY = budgetAccount.BUDGETMONEY;
            wf.USABLEMONEY = budgetAccount.USABLEMONEY;
            wf.ACTUALMONEY = budgetAccount.ACTUALMONEY;
            wf.PAIEDMONEY = budgetAccount.PAIEDMONEY;
            wf.TRIGGEREVENT = ((FBAEnums.CheckStates)(checkStates)).ToString();
            using (BaseBll<T_FB_WFBUDGETACCOUNT> bll = new BaseBll<T_FB_WFBUDGETACCOUNT>())
            {
                return bll.Add(wf);
            }
        }

        public List<AccountItem> GetAccountItem(EntityObject entity, AccountType accountType)
        {
            List<AccountItem> listResult = new List<AccountItem>();
            try
            {
                Type typeEntity = entity.GetType();
                Tracer.Debug("GetAccountItem-aaaa：" + typeEntity.Name);
                string preMethodName = accountType == AccountType.BudgetAccount ? "Get" : "Fetch";
                Tracer.Debug("GetAccountItem：" + preMethodName);
                Tracer.Debug("GetAccountItem-typeEntity：" + typeEntity.Name);
                MethodInfo method = this.GetType().GetMethod(preMethodName + typeEntity.Name);
                Tracer.Debug("GetAccountItem-typeEntitydddddd：" + typeEntity.Name);
                if (method != null)
                {
                    object result = method.Invoke(this, new object[] { entity });
                    listResult = result as List<AccountItem>;
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetAccountItem出现错误："+ ex.ToString());
            }
            return listResult;
        }

        private T_FB_BUDGETACCOUNT FindBudgetAccout(AccountItem accountItem)
        {

            if (accountItem.T_FB_SUBJECT == null)
            {
                throw new Exception("数据有异常,　科目对象为空, 参数:"
                           + "\r\nBudgetYear: " + accountItem.BudgetYear
                           + "\r\nBudgetMonth: " + accountItem.BudgetMonth
                           + "\r\nOwnerCompanyID: " + accountItem.OwnerCompanyID
                           + "\r\nOwnerDepartmentID: " + accountItem.OwnerDepartmentID
                           + "\r\nAccountType: " + accountItem.AccountType.ToString()
                           + "\r\nOwnerID: " + accountItem.OwnerID
                           + "\r\nOwnerPostID: " + accountItem.OwnerPostID);

            }
            T_FB_BUDGETACCOUNT budgetAccountItem = null;

            BudgetAccountDAL dalAccount = new BudgetAccountDAL();

            var accountData = from item in dalAccount.GetObjects().Include("T_FB_SUBJECT")
                              where item.BUDGETYEAR == accountItem.BudgetYear &&
                                     item.BUDGETMONTH == accountItem.BudgetMonth &&
                                     item.OWNERCOMPANYID == accountItem.OwnerCompanyID &&
                                     item.OWNERDEPARTMENTID == accountItem.OwnerDepartmentID &&
                                     item.T_FB_SUBJECT.SUBJECTID == accountItem.T_FB_SUBJECT.SUBJECTID &&
                                     item.ACCOUNTOBJECTTYPE == (int)accountItem.AccountType
                              select item;

            if (accountItem.AccountType == AccountObjectType.Person || accountItem.AccountType == AccountObjectType.Personnext)
            {
                accountData = from item in accountData
                              where item.OWNERID == accountItem.OwnerID &&
                                item.OWNERPOSTID == accountItem.OwnerPostID
                              select item;
            }
            budgetAccountItem = accountData.FirstOrDefault();
            if (budgetAccountItem == null)
            {

                budgetAccountItem = new T_FB_BUDGETACCOUNT();

                budgetAccountItem.T_FB_SUBJECT = accountItem.T_FB_SUBJECT;
                budgetAccountItem.BUDGETACCOUNTID = Guid.NewGuid().ToString();
                budgetAccountItem.ACCOUNTOBJECTTYPE = Convert.ToInt32(accountItem.AccountType);
                budgetAccountItem.OWNERCOMPANYID = accountItem.OwnerCompanyID;
                budgetAccountItem.OWNERDEPARTMENTID = accountItem.OwnerDepartmentID;
                budgetAccountItem.OWNERID = accountItem.OwnerID;
                budgetAccountItem.OWNERPOSTID = accountItem.OwnerPostID;
                budgetAccountItem.CREATEDATE = DateTime.Now;
                budgetAccountItem.UPDATEDATE = DateTime.Now;
                budgetAccountItem.CREATEUSERID = SYSTEM_USER_ID;
                budgetAccountItem.UPDATEUSERID = SYSTEM_USER_ID;

                budgetAccountItem.BUDGETMONEY = 0;
                budgetAccountItem.ACTUALMONEY = 0;
                budgetAccountItem.USABLEMONEY = 0;
                budgetAccountItem.PAIEDMONEY = 0;

                budgetAccountItem.BUDGETYEAR = Convert.ToInt32(accountItem.BudgetYear);//  DateTime.Now.Year;
                budgetAccountItem.BUDGETMONTH = Convert.ToInt32(accountItem.BudgetMonth); // DateTime.Now.Month;
            }
            return budgetAccountItem;
        }

        private bool Repay(EntityObject entity, decimal dCheckState)
        {
            // 审核通过的单据才生效
            if (dCheckState != (int)FBAEnums.CheckStates.Approved)
            {
                return true;
            }


            bool tempValue = true;

            tempValue &= UpdataBorrowOrder(entity, dCheckState);

            return tempValue;
        }

        private bool UpdateBorrowApply(T_FB_BORROWAPPLYMASTER entity, decimal dCheckState)
        {
            // 审核通过的单据才生效
            if (dCheckState != (int)FBAEnums.CheckStates.Approved)
            {
                return true;
            }

            BorrowApplyDetailBLL bllBorrow = new BorrowApplyDetailBLL();
            if (entity.T_FB_BORROWAPPLYDETAIL != null)
            {
                entity.T_FB_BORROWAPPLYDETAIL.Clear();
            }

            List<T_FB_BORROWAPPLYDETAIL> entList = bllBorrow.GetBorrowApplyDetailByMasterID(entity.BORROWAPPLYMASTERID);
            if (entList != null)
            {
                foreach (T_FB_BORROWAPPLYDETAIL item in entList)
                {
                    entity.T_FB_BORROWAPPLYDETAIL.Add(item);
                }
            }

            if (entity.T_FB_BORROWAPPLYDETAIL == null)
            {
                return false;
            }

            List<T_FB_BORROWAPPLYDETAIL> entDetails = entity.T_FB_BORROWAPPLYDETAIL.ToList();
            entity.ISREPAIED = 0; // 未还款
            foreach (var item in entity.T_FB_BORROWAPPLYDETAIL)
            {
                item.UNREPAYMONEY = item.BORROWMONEY;
                bllBorrow.Update(item);
            }

            BorrowApplyMasterBLL bllBorrowMaster = new BorrowApplyMasterBLL();
            bllBorrowMaster.ModifyBorrowApplyMaster(entity);
            return true;
        }

        /// <summary>
        /// 当有报销冲借款或现金还款审核通过时，需要修改借款单及借款明细的状态
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="statesNew"></param>
        /// <returns></returns>
        public bool UpdataBorrowOrder(EntityObject entity, decimal statesNew)
        {

            List<RepayItem> listRepayItem = GetRepayItem(entity);
            if (listRepayItem == null)
            {
                return true;
            }

            BorrowApplyDetailBLL bllBorrow = new BorrowApplyDetailBLL(); 

            listRepayItem.ForEach(item =>
            {
                // 还款金额大于借款金额
                if (item.RepayMoney > item.T_FB_BORROWAPPLYDETAIL.BORROWMONEY)
                {
                    item.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY = 0;
                }
                else
                {
                    item.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY = item.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY - item.RepayMoney;
                }

                item.T_FB_BORROWAPPLYDETAIL.UPDATEUSERID = SYSTEM_USER_ID;

                bllBorrow.Update(item.T_FB_BORROWAPPLYDETAIL);

                if (!item.T_FB_BORROWAPPLYDETAIL.T_FB_BORROWAPPLYMASTERReference.IsLoaded)
                {
                    item.T_FB_BORROWAPPLYDETAIL.T_FB_BORROWAPPLYMASTERReference.Load();
                }

                var master = item.T_FB_BORROWAPPLYDETAIL.T_FB_BORROWAPPLYMASTER;
                var masterQe = (from b in dal.GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                                where b.T_FB_BORROWAPPLYMASTER.BORROWAPPLYMASTERID == master.BORROWAPPLYMASTERID
                                select b).FirstOrDefault().T_FB_BORROWAPPLYMASTER;

                master = masterQe;
                decimal? totalUnPayMoney = master.T_FB_BORROWAPPLYDETAIL.Sum(detail =>
                {
                    return detail.UNREPAYMONEY;
                });
                if (totalUnPayMoney == 0)
                {
                    master.ISREPAIED = 1;
                }
                master.UPDATEUSERID = SYSTEM_USER_ID;

                BorrowApplyMasterBLL bllBorrowMaster = new BorrowApplyMasterBLL();
                bllBorrowMaster.ModifyBorrowApplyMaster(master);

            });
            return true;
        }

        public List<RepayItem> GetRepayItem(EntityObject entity)
        {
            List<RepayItem> listResult = new List<RepayItem>();

            MethodInfo method = this.GetType().GetMethod("Fetch" + entity.GetType().Name);
            if (method != null)
            {
                object result = method.Invoke(this, new object[] { entity });
                listResult = result as List<RepayItem>;
            }
            return listResult;
        }

        public List<RepayItem> FetchT_FB_CHARGEAPPLYMASTER(T_FB_CHARGEAPPLYMASTER entity)
        {
            ChargeApplyDetailBLL bllCharge = new ChargeApplyDetailBLL();
            if (entity.T_FB_CHARGEAPPLYDETAIL != null)
            {
                entity.T_FB_CHARGEAPPLYDETAIL.Clear();
            }

            List<T_FB_CHARGEAPPLYDETAIL> entList = bllCharge.GetChargeApplyDetailByMasterID(entity.CHARGEAPPLYMASTERID);
            if (entList != null)
            {
                foreach (T_FB_CHARGEAPPLYDETAIL item in entList)
                {
                    entity.T_FB_CHARGEAPPLYDETAIL.Add(item);
                }
            }

            if (entity.T_FB_CHARGEAPPLYDETAIL == null)
            {
                return null;
            }

            List<RepayItem> listResult = new List<RepayItem>();

            foreach (T_FB_CHARGEAPPLYDETAIL item in entity.T_FB_CHARGEAPPLYDETAIL.ToList())
            {
                T_FB_CHARGEAPPLYDETAIL entTemp = new T_FB_CHARGEAPPLYDETAIL();
                entTemp = bllCharge.GetChargeApplyDetailByID(item.CHARGEAPPLYDETAILID);

                if (!entTemp.T_FB_BORROWAPPLYDETAILReference.IsLoaded) { entTemp.T_FB_BORROWAPPLYDETAILReference.Load(); }

                decimal? moneyRepay = entTemp.CHARGEMONEY;

                // 有借款单时， 报销的费用将做为还款
                if (entTemp.T_FB_BORROWAPPLYDETAIL != null)
                {
                    if (entTemp.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY < entTemp.CHARGEMONEY)
                    {
                        moneyRepay = entTemp.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY;
                    }
                    else
                    {
                        moneyRepay = entTemp.CHARGEMONEY;
                    }
                }
                else
                {
                    return null;
                }


                // 更新
                entTemp.REPAYMONEY = moneyRepay;
                bllCharge.Update(entTemp);

                if (!entTemp.T_FB_SUBJECTReference.IsLoaded)
                {
                    entTemp.T_FB_SUBJECTReference.Load();
                }
                RepayItem repayItem = new RepayItem();

                repayItem.RepayMoney = moneyRepay;
                repayItem.T_FB_BORROWAPPLYDETAIL = entTemp.T_FB_BORROWAPPLYDETAIL;
                listResult.Add(repayItem);
            }

            return listResult;
        }

        public List<RepayItem> FetchT_FB_REPAYAPPLYMASTER(T_FB_REPAYAPPLYMASTER entity)
        {
            RepayApplyDetailBLL bllRepay = new RepayApplyDetailBLL();
            if (entity.T_FB_REPAYAPPLYDETAIL != null)
            {
                entity.T_FB_REPAYAPPLYDETAIL.Clear();
            }

            List<T_FB_REPAYAPPLYDETAIL> entList = bllRepay.GetRepayApplyDetailByMasterID(entity.REPAYAPPLYMASTERID);
            if (entList != null)
            {
                foreach (T_FB_REPAYAPPLYDETAIL item in entList)
                {
                    entity.T_FB_REPAYAPPLYDETAIL.Add(item);
                }
            }

            if (entity.T_FB_REPAYAPPLYDETAIL == null)
            {
                return null;
            }

            List<T_FB_REPAYAPPLYDETAIL> entDetails = entity.T_FB_REPAYAPPLYDETAIL.ToList();
            List<RepayItem> listResult = new List<RepayItem>();

            foreach (T_FB_REPAYAPPLYDETAIL item in entDetails)
            {
                T_FB_REPAYAPPLYDETAIL entTemp = new T_FB_REPAYAPPLYDETAIL();
                entTemp = bllRepay.GetRepayApplyDetailByID(item.REPAYAPPLYDETAILID);

                if (!item.T_FB_BORROWAPPLYDETAILReference.IsLoaded)
                {
                    item.T_FB_BORROWAPPLYDETAILReference.Load();
                }

                decimal? moneyRepay = item.REPAYMONEY;

                // 有借款单时， 报销的费用将做为还款
                if (item.T_FB_BORROWAPPLYDETAIL != null)
                {
                    if (item.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY < item.REPAYMONEY)
                    {
                        moneyRepay = item.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY;
                    }
                    else
                    {
                        moneyRepay = item.REPAYMONEY;
                    }
                }
                else
                {
                    return null;
                }

                if (!item.T_FB_SUBJECTReference.IsLoaded)
                {
                    item.T_FB_SUBJECTReference.Load();
                }
                RepayItem repayItem = new RepayItem();

                repayItem.RepayMoney = moneyRepay;
                repayItem.T_FB_BORROWAPPLYDETAIL = item.T_FB_BORROWAPPLYDETAIL;
                listResult.Add(repayItem);
            }
            return listResult;
        }

        #endregion

        #region 报销, 借还款
        /// <summary>
        /// 费用报销
        ///    1. 带还款单
        ///         1.判断报销的金额是否大于还款的金额。
        ///    2. 不带还款单
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<AccountItem> GetT_FB_CHARGEAPPLYMASTER(T_FB_CHARGEAPPLYMASTER entity)
        {
            ChargeApplyDetailBLL bllCharge = new ChargeApplyDetailBLL();
            if (entity.T_FB_CHARGEAPPLYDETAIL != null)
            {
                entity.T_FB_CHARGEAPPLYDETAIL.Clear();
            }

            List<T_FB_CHARGEAPPLYDETAIL> entList = bllCharge.GetChargeApplyDetailByMasterID(entity.CHARGEAPPLYMASTERID);

           
            if (entList != null)
            {
                foreach (T_FB_CHARGEAPPLYDETAIL item in entList)
                {
                    entity.T_FB_CHARGEAPPLYDETAIL.Add(item);
                }
            }

            if (entity.T_FB_CHARGEAPPLYDETAIL == null)
            {
                return null;
            }

            List<T_FB_CHARGEAPPLYDETAIL> entDetails = entity.T_FB_CHARGEAPPLYDETAIL.ToList();
            List<AccountItem> listResult = new List<AccountItem>();

            foreach (T_FB_CHARGEAPPLYDETAIL item in entDetails)
            {
                T_FB_CHARGEAPPLYDETAIL entTemp = new T_FB_CHARGEAPPLYDETAIL();
                entTemp = bllCharge.GetChargeApplyDetailByID(item.CHARGEAPPLYDETAILID);

                if (entTemp == null)
                {
                    throw new Exception("未找到报销明细记录！");
                }

                if (!entTemp.T_FB_BORROWAPPLYDETAILReference.IsLoaded)
                {
                    entTemp.T_FB_BORROWAPPLYDETAILReference.Load();
                }

                decimal? moneyActual = entTemp.CHARGEMONEY;
                decimal? moneyUsable = entTemp.CHARGEMONEY;
                // 有借款单时， 可用金额和实际金额已被预先扣除，些处无需再次处理，但报销费用还需要加入总帐。
                if (item.T_FB_BORROWAPPLYDETAIL != null)
                {
                    moneyUsable = 0;
                    moneyActual = 0;

                    if (entTemp.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY != null)
                    {
                        if (entTemp.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY.Value < entTemp.CHARGEMONEY)
                        {
                            moneyUsable = entTemp.CHARGEMONEY - entTemp.T_FB_BORROWAPPLYDETAIL.UNREPAYMONEY.Value;
                            moneyActual = moneyUsable;
                        }
                    }
                }

                if (!entTemp.T_FB_SUBJECTReference.IsLoaded)
                {
                    entTemp.T_FB_SUBJECTReference.Load();
                }
                // 处理跨年的单据
                if (entity.CHECKSTATES == (decimal)FBAEnums.CheckStates.UnApproved && entity.BUDGETARYMONTH.Year < System.DateTime.Now.Year)
                {
                    // 审核不通过时，如果是过期的单，不扣除失效锁定的金额。此过滤为除部门经费, 活动经费以外的都不扣.
                    if (item.T_FB_SUBJECT.SUBJECTID != "08c1d9c6-2396-43c3-99f9-227e4a7eb417" &&
                            item.T_FB_SUBJECT.SUBJECTID != "d5134466-c207-44f2-8a36-cf7b96d5851f")
                    {
                        continue;
                    }
                }

                AccountItem accountItem = new AccountItem();
                accountItem.OrderCode = entity.CHARGEAPPLYMASTERCODE;
                accountItem.OrderID = entity.CHARGEAPPLYMASTERID;
                accountItem.OrderDetailID = entTemp.CHARGEAPPLYDETAILID;
                accountItem.EntityType = typeof(T_FB_CHARGEAPPLYMASTER).Name;
                accountItem.OPERATIONMONEY = entTemp.CHARGEMONEY;

                accountItem.T_FB_SUBJECT = entTemp.T_FB_SUBJECT;
                accountItem.OwnerDepartmentID = entity.OWNERDEPARTMENTID;
                accountItem.OwnerPostID = entity.OWNERPOSTID;
                accountItem.OwnerCompanyID = entity.OWNERCOMPANYID;
                accountItem.OwnerID = entity.OWNERID;

                accountItem.moneyPaid = entTemp.CHARGEMONEY;
                accountItem.moneyUsable = moneyUsable;
                accountItem.moneyActual = moneyActual;

                accountItem.BudgetYear = System.DateTime.Now.Year;
                accountItem.BudgetMonth = System.DateTime.Now.Month;

                if (item.CHARGETYPE.Value == decimal.Parse("1"))
                {
                    accountItem.AccountType = AccountObjectType.Person;
                    accountItem.CheckRule = CheckRule_PersonBudget;
                }
                else
                {
                    accountItem.AccountType = AccountObjectType.Deaprtment;
                }

                accountItem.AccountOpertaion = AccountOpertaion.Subtract;
                listResult.Add(accountItem);
            }

            return listResult;
        }

        /// <summary>
        /// 借款申请
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<AccountItem> GetT_FB_BORROWAPPLYMASTER(T_FB_BORROWAPPLYMASTER entity)
        {
            BorrowApplyDetailBLL bllBorrow = new BorrowApplyDetailBLL();
            if (entity.T_FB_BORROWAPPLYDETAIL != null)
            {
                entity.T_FB_BORROWAPPLYDETAIL.Clear();
            }

            List<T_FB_BORROWAPPLYDETAIL> entList = bllBorrow.GetBorrowApplyDetailByMasterID(entity.BORROWAPPLYMASTERID);
            if (entList != null)
            {
                foreach (T_FB_BORROWAPPLYDETAIL item in entList)
                {
                    entity.T_FB_BORROWAPPLYDETAIL.Add(item);
                }
            }

            if (entity.T_FB_BORROWAPPLYDETAIL == null)
            {
                return null;
            }

            List<T_FB_BORROWAPPLYDETAIL> entDetails = entity.T_FB_BORROWAPPLYDETAIL.ToList();
            List<AccountItem> listResult = new List<AccountItem>();


            foreach (T_FB_BORROWAPPLYDETAIL item in entDetails)
            {
                T_FB_BORROWAPPLYDETAIL entTemp = new T_FB_BORROWAPPLYDETAIL();
                entTemp = bllBorrow.GetBorrowApplyDetailByID(item.BORROWAPPLYDETAILID);
                
                if (!entTemp.T_FB_SUBJECTReference.IsLoaded)
                {
                    entTemp.T_FB_SUBJECTReference.Load();
                }
                AccountItem accountItem = new AccountItem();
                accountItem.T_FB_SUBJECT = entTemp.T_FB_SUBJECT;
                accountItem.OwnerCompanyID = entity.OWNERCOMPANYID;
                accountItem.OwnerDepartmentID = entity.OWNERDEPARTMENTID;
                accountItem.OwnerPostID = entity.OWNERPOSTID;
                accountItem.OwnerID = entity.OWNERID;

                accountItem.moneyPaid = 0;
                accountItem.moneyUsable = entTemp.BORROWMONEY;
                accountItem.moneyActual = entTemp.BORROWMONEY;

                accountItem.BudgetYear = System.DateTime.Now.Year;
                accountItem.BudgetMonth = System.DateTime.Now.Month;

                //暂时定为部门预算
                accountItem.AccountType = entTemp.CHARGETYPE == 1 ? AccountObjectType.Person : AccountObjectType.Deaprtment;

                accountItem.AccountOpertaion = AccountOpertaion.Subtract;
                listResult.Add(accountItem);
            }
            return listResult;
        }

        /// <summary>
        /// 还款申请
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public List<AccountItem> GetT_FB_REPAYAPPLYMASTER(T_FB_REPAYAPPLYMASTER entity)
        {
            RepayApplyDetailBLL bllRepay = new RepayApplyDetailBLL();
            if (entity.T_FB_REPAYAPPLYDETAIL != null)
            {
                entity.T_FB_REPAYAPPLYDETAIL.Clear();
            }

            List<T_FB_REPAYAPPLYDETAIL> entList = bllRepay.GetRepayApplyDetailByMasterID(entity.REPAYAPPLYMASTERID);
            if (entList != null)
            {
                foreach (T_FB_REPAYAPPLYDETAIL item in entList)
                {
                    entity.T_FB_REPAYAPPLYDETAIL.Add(item);
                }
            }

            if (entity.T_FB_REPAYAPPLYDETAIL == null)
            {
                return null;
            }

            List<T_FB_REPAYAPPLYDETAIL> entDetails = entity.T_FB_REPAYAPPLYDETAIL.ToList();
            List<AccountItem> listResult = new List<AccountItem>();

            foreach (T_FB_REPAYAPPLYDETAIL item in entDetails)
            {
                T_FB_REPAYAPPLYDETAIL entTemp = new T_FB_REPAYAPPLYDETAIL();
                entTemp = bllRepay.GetRepayApplyDetailByID(item.REPAYAPPLYDETAILID);

                if (!item.T_FB_SUBJECTReference.IsLoaded)
                {
                    item.T_FB_SUBJECTReference.Load();
                }
                AccountItem accountItem = new AccountItem();
                accountItem.T_FB_SUBJECT = item.T_FB_SUBJECT;
                accountItem.OwnerCompanyID = entity.OWNERCOMPANYID;
                accountItem.OwnerDepartmentID = entity.OWNERDEPARTMENTID;
                accountItem.OwnerPostID = entity.OWNERPOSTID;
                accountItem.OwnerID = entity.OWNERID;

                accountItem.moneyUsable = item.REPAYMONEY;
                accountItem.moneyActual = item.REPAYMONEY;


                accountItem.BudgetYear = System.DateTime.Now.Year;
                accountItem.BudgetMonth = System.DateTime.Now.Month;

                //暂时定为部门预算
                accountItem.AccountType = item.CHARGETYPE == 1 ? AccountObjectType.Person : AccountObjectType.Deaprtment;

                accountItem.AccountOpertaion = AccountOpertaion.Add;
                listResult.Add(accountItem);
            }
            return listResult;
        }

        #endregion

        /// <summary>
        /// 个人预算是否超出个人预算上限
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool CheckRule_PersonBudget(T_FB_BUDGETACCOUNT account)
        {
            bool bCheck = false;
            if (string.IsNullOrEmpty(account.OWNERPOSTID))
            {
                return bCheck;
            }

            // 1. 查出相应的 岗位科目记录 + 部门科目记录+ 公司科目记录
            SubjectPostBLL bllSubjectPost = new SubjectPostBLL();
            var ents = bllSubjectPost.GetSubjectPostByPostId(account.OWNERPOSTID);

            List<T_FB_SUBJECTPOST> listSubjectPost = new List<T_FB_SUBJECTPOST>();
            if (ents != null)
            {
                listSubjectPost = ents.ToList();
            }

            // 2. 遍历相应岗位科目记录，做校验
            listSubjectPost.ForEach(subjectPost =>
            {
                if (subjectPost.LIMITBUDGEMONEY == null) // 为 0 或 null 时，不做最大预算的限制
                {
                    return;
                }

                if (subjectPost.LIMITBUDGEMONEY.Value == 0) // 为 0 或 null 时，不做最大预算的限制
                {
                    return;
                }

                // 预算上限限制
                if (subjectPost.LIMITBUDGEMONEY.Value < account.BUDGETMONEY)
                {
                    throw new Exception("科目 " + subjectPost.T_FB_SUBJECT.SUBJECTNAME + " 的预算额度超出月预算限制");
                }
            });
            return true;

        }

        private int SaveBudgetAccount(T_FB_BUDGETACCOUNT budgetAccountItem)
        {
            string strMsg = string.Empty;
            int i = 0;
            try
            {
                if (budgetAccountItem == null)
                {
                    return i;
                }


                bool flag = false;
                BudgetAccountDAL dalBudgetAccount = new BudgetAccountDAL();

                var accountData = from item in dalBudgetAccount.GetObjects().Include("T_FB_SUBJECT")
                                  where item.BUDGETYEAR == budgetAccountItem.BUDGETYEAR &&
                                         item.BUDGETMONTH == budgetAccountItem.BUDGETMONTH &&
                                         item.OWNERCOMPANYID == budgetAccountItem.OWNERCOMPANYID &&
                                         item.OWNERDEPARTMENTID == budgetAccountItem.OWNERDEPARTMENTID &&
                                         item.T_FB_SUBJECT.SUBJECTID == budgetAccountItem.T_FB_SUBJECT.SUBJECTID &&
                                         item.ACCOUNTOBJECTTYPE == budgetAccountItem.ACCOUNTOBJECTTYPE
                                  select item;

                if (accountData == null)
                {
                    flag = false;
                }
                else
                {
                    if (accountData.Count() == 0)
                    {
                        flag = false;
                    }
                    else
                    {
                        flag = true;
                    }
                }
                budgetAccountItem.UPDATEDATE = System.DateTime.Now;

                if (!flag)
                {
                    var se = budgetAccountItem.T_FB_SUBJECT.EntityKey;
                    budgetAccountItem.T_FB_SUBJECT = null;
                    budgetAccountItem.T_FB_SUBJECTReference.EntityKey = se;
                    
                    i=dalBudgetAccount.Add(budgetAccountItem);
                }
                else
                {
                    i=dalBudgetAccount.Update(budgetAccountItem);
                }

                strMsg = "{SAVESUCCESSED}";
                
            }
            catch (Exception ex)
            {
                strMsg = ex.Message.ToString();
                Utility.SaveLog(strMsg);
            }
            return i;
        }

        #region  转移数据专用 zl
        /// <summary>
        /// 转移借款表中的相应数据
        /// </summary>
        /// <returns></returns>
        public bool TransBudgetAccountByBor()
        {
            bool re;
            string ErrInfo = "";
            T_FB_BUDGETACCOUNT entBudge = new T_FB_BUDGETACCOUNT();
            DateTime dt = new DateTime(2012, 1, 1);
            try
            {
                dal.BeginTransaction();
                //把借款表中2012.1.1及之后的审核通过的单据占用报销科目的金额还到T_FB_BUDGETACCOUNT表中
                IQueryable<T_FB_BORROWAPPLYDETAIL> bordetObj =
                from a in dal.GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER").Include("T_FB_SUBJECT")
                where a.T_FB_BORROWAPPLYMASTER.CHECKSTATES == 2 &&
                      a.T_FB_BORROWAPPLYMASTER.UPDATEDATE >= dt
                select a;

                foreach (T_FB_BORROWAPPLYDETAIL borDet in bordetObj)
                {
                    if (borDet.CHARGETYPE == 1)
                    {
                        IQueryable<T_FB_BUDGETACCOUNT> qAccount = from i in dal.GetObjects()
                                                                  where i.OWNERCOMPANYID == borDet.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID
                                                                  && i.OWNERDEPARTMENTID == borDet.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID
                                                                  && i.OWNERPOSTID == borDet.T_FB_BORROWAPPLYMASTER.OWNERPOSTID
                                                                  && i.OWNERID == borDet.T_FB_BORROWAPPLYMASTER.OWNERID
                                                                  && i.T_FB_SUBJECT.SUBJECTID == borDet.T_FB_SUBJECT.SUBJECTID
                                                                  && i.ACCOUNTOBJECTTYPE == 3
                                                                  select i;
                        if (qAccount != null)
                        {
                            if(qAccount.Count()>0)
                            {
                                entBudge = qAccount.FirstOrDefault();
                                entBudge.USABLEMONEY += borDet.BORROWMONEY;
                                entBudge.ACTUALMONEY += borDet.BORROWMONEY;
                                entBudge.UPDATEDATE = DateTime.Now;
                                re = Update(entBudge);
                                if(!re)
                                {
                                    dal.RollbackTransaction();
                                    return false;
                                }
                            }
                        }
                    }
                    else if (borDet.CHARGETYPE == 2)
                    {
                        IQueryable<T_FB_BUDGETACCOUNT> qAccount = from i in dal.GetObjects()
                                                                  where i.OWNERCOMPANYID == borDet.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID
                                                                  && i.OWNERDEPARTMENTID == borDet.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID
                                                                  && i.T_FB_SUBJECT.SUBJECTID == borDet.T_FB_SUBJECT.SUBJECTID
                                                                  && i.ACCOUNTOBJECTTYPE == 2
                                                                  select i;
                        if (qAccount != null)
                        {
                            if (qAccount.Count() > 0)
                            {
                                entBudge = qAccount.FirstOrDefault();
                                entBudge.USABLEMONEY += borDet.BORROWMONEY;
                                entBudge.ACTUALMONEY += borDet.BORROWMONEY;
                                entBudge.UPDATEDATE = DateTime.Now;
                                re = Update(entBudge);
                                if (!re)
                                {
                                    dal.RollbackTransaction();
                                    return false;
                                }
                            }
                        }
                    }
                }
                //把借款表中2012.1.1及之后的审核通过的单据占用报销科目的金额还到T_FB_BUDGETACCOUNT表中 end

                //把借款表中审核中的单据打回
                bordetObj = from a in dal.GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER").Include("T_FB_SUBJECT")
                where a.T_FB_BORROWAPPLYMASTER.CHECKSTATES == 1
                select a;

                foreach (T_FB_BORROWAPPLYDETAIL borDet in bordetObj)
                {
                    if (borDet.CHARGETYPE == 1)
                    {
                        IQueryable<T_FB_BUDGETACCOUNT> qAccount = from i in dal.GetObjects()
                                                                  where i.OWNERCOMPANYID == borDet.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID
                                                                  && i.OWNERDEPARTMENTID == borDet.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID
                                                                  && i.OWNERPOSTID == borDet.T_FB_BORROWAPPLYMASTER.OWNERPOSTID
                                                                  && i.OWNERID == borDet.T_FB_BORROWAPPLYMASTER.OWNERID
                                                                  && i.T_FB_SUBJECT.SUBJECTID == borDet.T_FB_SUBJECT.SUBJECTID
                                                                  && i.ACCOUNTOBJECTTYPE == 3
                                                                  select i;
                        if (qAccount != null)
                        {
                            if(qAccount.Count()>0)
                            {
                                entBudge = qAccount.FirstOrDefault();
                                entBudge.USABLEMONEY += borDet.BORROWMONEY;
                                entBudge.UPDATEDATE = DateTime.Now;
                                re = Update(entBudge);
                                if (!re)
                                {
                                    dal.RollbackTransaction();
                                    return false;
                                }
                            }
                        }
                    }
                    else if (borDet.CHARGETYPE == 2)
                    {
                        IQueryable<T_FB_BUDGETACCOUNT> qAccount = from i in dal.GetObjects()
                                                                  where i.OWNERCOMPANYID == borDet.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID
                                                                  && i.OWNERDEPARTMENTID == borDet.T_FB_BORROWAPPLYMASTER.OWNERDEPARTMENTID
                                                                  && i.T_FB_SUBJECT.SUBJECTID == borDet.T_FB_SUBJECT.SUBJECTID
                                                                  && i.ACCOUNTOBJECTTYPE == 2
                                                                  select i;
                        if (qAccount != null)
                        {
                            if(qAccount.Count()>0)
                            {
                                entBudge = qAccount.FirstOrDefault();
                                entBudge.USABLEMONEY += borDet.BORROWMONEY;
                                entBudge.UPDATEDATE = DateTime.Now;
                                re = Update(entBudge);
                                if (!re)
                                {
                                    dal.RollbackTransaction();
                                    return false;
                                }
                            }
                        }
                    }
                }

                IQueryable<T_FB_BORROWAPPLYMASTER> bormasObj = from a in dal.GetObjects<T_FB_BORROWAPPLYMASTER>()
                                                               where a.CHECKSTATES == 1
                                                               select a;
                BorrowApplyMasterBLL borMasBLL = new BorrowApplyMasterBLL();
                foreach (T_FB_BORROWAPPLYMASTER borMas in bormasObj)
                {
                    borMas.CHECKSTATES = 3;
                    borMas.UPDATEDATE = DateTime.Now;
                    re = borMasBLL.UptBorrowApplyMasterChkSta(borMas);
                    if (!re)
                    {
                        dal.RollbackTransaction();
                        return false;
                    }
                }

                //把借款表中审核中的单据打回 end
                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：TransBudgetAccountByBorr，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }

        /// <summary>
        /// 转移还款表中的相应数据
        /// </summary>
        /// <returns></returns>
        public bool TransBudgetAccountByRep()
        {
            bool re;
            string ErrInfo = "";
            T_FB_BUDGETACCOUNT entBudge = new T_FB_BUDGETACCOUNT();
            DateTime dt = new DateTime(2012, 1, 1);
            try
            {
                dal.BeginTransaction();
                //把还款表中审核中的单据打回
                IQueryable<T_FB_REPAYAPPLYMASTER> repmasObj = from a in dal.GetObjects<T_FB_REPAYAPPLYMASTER>()
                                                              where a.CHECKSTATES == 1
                                                              select a;
                RepayApplyMasterBLL repMasBLL = new RepayApplyMasterBLL();
                foreach (T_FB_REPAYAPPLYMASTER repMas in repmasObj)
                {
                    repMas.CHECKSTATES = 3;
                    repMas.UPDATEDATE = DateTime.Now;
                    re = repMasBLL.UptRepayApplyMasterChkSta(repMas);
                    if (!re)
                    {
                        dal.RollbackTransaction();
                        return false;
                    }
                }

                //把还款表中审核中的单据打回 end
                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：TransBudgetAccountByBorr，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }

        /// <summary>
        /// 转移报销表中的相应数据
        /// </summary>
        /// <returns></returns>
        public bool TransBudgetAccountByCha()
        {
            bool re;
            string ErrInfo = "";
            T_FB_BUDGETACCOUNT entBudge = new T_FB_BUDGETACCOUNT();
            DateTime dt = new DateTime(2012, 1, 1);
            try
            {
                dal.BeginTransaction();
                //把报销表中审核中的单据打回
                IQueryable<T_FB_CHARGEAPPLYDETAIL> chaDetObj =
                    from a in dal.GetObjects<T_FB_CHARGEAPPLYDETAIL>().Include("T_FB_CHARGEAPPLYMASTER").Include("T_FB_SUBJECT").Include("T_FB_BORROWAPPLYDETAIL")
                    where a.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == 1
                    select a;
                foreach (T_FB_CHARGEAPPLYDETAIL chaDet in chaDetObj)
                {
                    if (chaDet.CHARGETYPE == 1)
                    {
                        IQueryable<T_FB_BUDGETACCOUNT> qAccount = from i in dal.GetObjects()
                                                                  where i.OWNERCOMPANYID == chaDet.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID
                                                                  && i.OWNERDEPARTMENTID == chaDet.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID
                                                                  && i.OWNERPOSTID == chaDet.T_FB_CHARGEAPPLYMASTER.OWNERPOSTID
                                                                  && i.OWNERID == chaDet.T_FB_CHARGEAPPLYMASTER.OWNERID
                                                                  && i.T_FB_SUBJECT.SUBJECTID == chaDet.T_FB_SUBJECT.SUBJECTID
                                                                  && i.ACCOUNTOBJECTTYPE == 3
                                                                  select i;
                        if (qAccount != null)
                        {
                            if (qAccount.Count() > 0)
                            {
                                entBudge = qAccount.FirstOrDefault();
                                if (chaDet.T_FB_CHARGEAPPLYMASTER.PAYTYPE != 2)
                                {
                                    entBudge.USABLEMONEY += chaDet.CHARGEMONEY;
                                }
                                else if (chaDet.T_FB_CHARGEAPPLYMASTER.PAYTYPE == 2 && chaDet.CHARGEMONEY > chaDet.T_FB_BORROWAPPLYDETAIL.BORROWMONEY)
                                {
                                    entBudge.USABLEMONEY += (chaDet.CHARGEMONEY - chaDet.T_FB_BORROWAPPLYDETAIL.BORROWMONEY);
                                }
                                entBudge.UPDATEDATE = DateTime.Now;
                                re = Update(entBudge);
                                if (!re)
                                {
                                    dal.RollbackTransaction();
                                    return false;
                                }
                            }
                        }
                    }
                    if (chaDet.CHARGETYPE == 2)
                    {
                        IQueryable<T_FB_BUDGETACCOUNT> qAccount = from i in dal.GetObjects()
                                                                  where i.OWNERCOMPANYID == chaDet.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID
                                                                  && i.OWNERDEPARTMENTID == chaDet.T_FB_CHARGEAPPLYMASTER.OWNERDEPARTMENTID
                                                                  && i.T_FB_SUBJECT.SUBJECTID == chaDet.T_FB_SUBJECT.SUBJECTID
                                                                  && i.ACCOUNTOBJECTTYPE == 2
                                                                  select i;
                        if (qAccount != null)
                        {
                            if (qAccount.Count() > 0)
                            {
                                entBudge = qAccount.FirstOrDefault();
                                if (chaDet.T_FB_CHARGEAPPLYMASTER.PAYTYPE != 2)
                                {
                                    entBudge.USABLEMONEY += chaDet.CHARGEMONEY;
                                }
                                else if (chaDet.T_FB_CHARGEAPPLYMASTER.PAYTYPE == 2 && chaDet.CHARGEMONEY > chaDet.T_FB_BORROWAPPLYDETAIL.BORROWMONEY)
                                {
                                    entBudge.USABLEMONEY += (chaDet.CHARGEMONEY - chaDet.T_FB_BORROWAPPLYDETAIL.BORROWMONEY);
                                }
                                entBudge.UPDATEDATE = DateTime.Now;
                                re = Update(entBudge);
                                if (!re)
                                {
                                    dal.RollbackTransaction();
                                    return false;
                                }
                            }
                        }
                    }
                }

                IQueryable<T_FB_CHARGEAPPLYMASTER> chaMasObj = from a in dal.GetObjects<T_FB_CHARGEAPPLYMASTER>()
                                                               where a.CHECKSTATES == 1
                                                               select a;
                ChargeApplyMasterBLL chaMasBLL = new ChargeApplyMasterBLL();
                foreach (T_FB_CHARGEAPPLYMASTER chaMas in chaMasObj)
                {
                    chaMas.CHECKSTATES = 3;
                    chaMas.UPDATEDATE = DateTime.Now;
                    re = chaMasBLL.UptChargeApplyMasterChkSta(chaMas);
                    if (!re)
                    {
                        dal.RollbackTransaction();
                        return false;
                    }
                }

                //把报销表中审核中的单据打回 end
                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：TransBudgetAccountByBorr，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }

        #endregion
    }
}

