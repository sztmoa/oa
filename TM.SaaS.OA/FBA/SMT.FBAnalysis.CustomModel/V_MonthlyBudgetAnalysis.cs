using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    public class V_MonthlyBudgetAnalysis
    {
        /// <summary>
        /// 项目(科目) ID。
        /// </summary>
        public string SubjectID { get; set; }
        /// <summary>
        /// 项目(科目)名称。
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 上月预算结余。
        /// </summary>
        public decimal LastBudgetMonth { get; set; }
        /// <summary>
        /// 本月预算。
        /// </summary>
        public decimal MonthBudgetApply { get; set; }
        /// <summary>
        /// 本月预算增补。
        /// </summary>
        public decimal MonthBudgetAdd { get; set; }
        /// <summary>
        /// 本月报销费用。
        /// </summary>
        public decimal MonthChargeMoney { get; set; }
        /// <summary>
        /// 月度预算偏差。
        /// </summary>
        public decimal MonthBudgetDeviation { get; set; }
        /// <summary>
        /// 月度预算偏差率。
        /// </summary>
        public decimal MonthBudgetDeviationRate { get; set; }
    }
}
