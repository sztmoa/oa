using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    public class V_YearlyBudgetAnalysis
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
        /// 年度预算。
        /// </summary>
        public decimal YearBudgetApply { get; set; }
        /// <summary>
        /// 年度预算增补。
        /// </summary>
        public decimal YearBudgetAdd { get; set; }
        /// <summary>
        /// 本年报销费用。
        /// </summary>
        public decimal YearChargeMoney { get; set; }
        /// <summary>
        /// 年度预算偏差。
        /// </summary>
        public decimal YearBudgetDeviation { get; set; }
        /// <summary>
        /// 年度预算偏差率。
        /// </summary>
        public decimal YearBudgetDeviationRate { get; set; }
    }
}
