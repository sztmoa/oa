using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    public class V_BillOfDocList
    {
        /// <summary>
        /// 单据编号。
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 科目。
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// 类型。
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 登账日期。
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 部门。
        /// </summary>
        public string Dept { get; set; }
        /// <summary>
        /// 登账人。
        /// </summary>
        public string CreateUser { get; set; }
        /// <summary>
        /// 金额。
        /// </summary>
        public string Money { get; set; }
        /// <summary>
        /// 预算月份。
        /// </summary>
        public string BudgetaryMonth { get; set; }
        /// <summary>
        /// 操作类型。
        /// </summary>
        public string OperateType { get; set; }
        /// <summary>
        /// 费用类型。
        /// </summary>
        public string ChargeType { get; set; }
    }
}
