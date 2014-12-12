using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SMT.FBAnalysis.UI.Models
{
    public class StandingBookInfo
    {
        /// <summary>
        /// 机构。
        /// </summary>
        public string Organization { get; set; }
        /// <summary>
        /// 项目。
        /// </summary>
        public string Project { get; set; }
        /// <summary>
        /// 年度预算总额。
        /// </summary>
        public double BudgetYearCount { get; set; }
        /// <summary>
        /// 批复月度预算。
        /// </summary>
        public double BudgetMonthPass { get; set; }
        /// <summary>
        /// 预算批复率。
        /// </summary>
        public double PassRate { get; set; }
        /// <summary>
        /// 一月。
        /// </summary>
        public double Jan { get; set; }
        /// <summary>
        /// 二月。
        /// </summary>
        public double Feb { get; set; }
        /// <summary>
        /// 三月。
        /// </summary>
        public double Mar { get; set; }
        /// <summary>
        /// 四月。
        /// </summary>
        public double Apr { get; set; }
        /// <summary>
        /// 五月。
        /// </summary>
        public double May { get; set; }
        /// <summary>
        /// 六月。
        /// </summary>
        public double Jun { get; set; }
        /// <summary>
        /// 七月。
        /// </summary>
        public double Jul { get; set; }
        /// <summary>
        /// 八月。
        /// </summary>
        public double Aug { get; set; }
        /// <summary>
        /// 九月。
        /// </summary>
        public double Sept { get; set; }
        /// <summary>
        /// 十月。
        /// </summary>
        public double Oct { get; set; }
        /// <summary>
        /// 十一月。
        /// </summary>
        public double Nov { get; set; }
        /// <summary>
        /// 十二月。
        /// </summary>
        public double Dec { get; set; }
        /// <summary>
        /// 报销费用－已审核。
        /// </summary>
        public double Verification { get; set; }
        /// <summary>
        /// 报销费用－审核中。
        /// </summary>
        public double Reply { get; set; }
        /// <summary>
        /// 小计。
        /// </summary>
        public double Subtotal { get; set; }
        /// <summary>
        /// 预算占用率。
        /// </summary>
        public double BudgetSeizurerate { get; set; }
        /// <summary>
        /// 年度预算。
        /// </summary>
        public double BudgetYear { get; set; }
        /// <summary>
        /// 月度预算。
        /// </summary>
        public double BudgetMonth { get; set; }
    }
}
