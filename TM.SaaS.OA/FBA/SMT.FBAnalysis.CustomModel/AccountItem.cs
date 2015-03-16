using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TM_SaaS_OA_EFModel;

namespace SMT.FBAnalysis.CustomModel
{
    public enum AccountOpertaion
    {
        Add, Subtract, JustCheck
    }
    public enum AccountObjectType
    {
        Company = 1,
        Deaprtment,
        Person,
        Simple = 11,
        Backup,
        Special,
        /// <summary>
        /// 非当前记录
        /// </summary>
        Companynext = 20,
        /// <summary>
        /// 非当前记录
        /// </summary>
        Deptnext,
        /// <summary>
        /// 非当前记录
        /// </summary>
        Personnext

    }
    public enum AccountType
    {
        BudgetAccount, PersonAccount
    }
    public class AccountItem
    {
        public AccountItem()
        {
            moenyBudget = 0;
            moneyUsable = 0;
            moneyActual = 0;
            moneyPaid = 0;
        }
        public AccountOpertaion AccountOpertaion { get; set; }
        public AccountObjectType AccountType { get; set; }
        public T_FB_SUBJECT T_FB_SUBJECT { get; set; }
        public string OwnerID { get; set; }
        public string OwnerDepartmentID { get; set; }
        public string OwnerCompanyID { get; set; }
        public string OwnerPostID { get; set; }
        public decimal? moenyBudget { get; set; }
        public decimal? moneyUsable { get; set; }
        public decimal? moneyActual { get; set; }
        public decimal? moneyPaid { get; set; }

        public decimal? BudgetYear { get; set; }
        public decimal? BudgetMonth { get; set; }

        public string OrderCode { get; set; }
        public string EntityType {get;set;}
        public decimal OPERATIONMONEY { get; set; }
        public string OrderDetailID { get; set; }
        public string OrderID { get; set; }
        public Func<T_FB_BUDGETACCOUNT, bool> CheckRule { get; set; }
    }
    public class RepayItem
    {
        public decimal? RepayMoney { get; set; }
        public T_FB_BORROWAPPLYDETAIL T_FB_BORROWAPPLYDETAIL { get; set; }
    }
}
