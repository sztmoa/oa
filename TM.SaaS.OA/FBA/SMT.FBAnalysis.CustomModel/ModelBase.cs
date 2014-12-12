using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    public class ModelBase
    {
        /// <summary>
        /// 单据编号。
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 科目 ID。
        /// </summary>
        public string SubjectID { get; set; }
        /// <summary>
        /// 科目名称。
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 类型。
        /// </summary>
        public decimal Type { get; set; }
        /// <summary>
        /// 登账日期。
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 部门 ID。
        /// </summary>
        public string DeptmentID { get; set; }
        /// <summary>
        /// 部门 ID。
        /// </summary>
        public string DeptmentName { get; set; }
        /// <summary>
        /// 登账人。
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 登账人。
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 金额。
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// 预算月份。
        /// </summary>
        public DateTime BudgetaryMonth { get; set; }
        /// <summary>
        /// 操作类型。
        /// </summary>
        public string OperateType { get; set; }
        /// <summary>
        /// 费用类型。
        /// </summary>
        public decimal ChargeType { get; set; }
    }
}
