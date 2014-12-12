using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    /// <summary>
    /// 个人执行一览视图实体
    /// </summary>
    public class V_PerExecutionList
    {
        /// <summary>
        /// 机构 ID。
        /// </summary>
        public string OrganizationID { get; set; }
        /// <summary>
        /// 机构名称。
        /// </summary>
        public string OrganizationName { get; set; }
        /// <summary>
        /// 项目(科目) ID。
        /// </summary>
        public string OwnerID { get; set; }
        /// <summary>
        /// 项目(科目)名称。
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 项目(科目) ID。
        /// </summary>
        public string SubjectID { get; set; }
        /// <summary>
        /// 项目(科目)名称。
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 报销费用－已审核。
        /// </summary>
        public decimal ApprovedApplyMoney { get; set; }
        /// <summary>
        /// 报销费用－审核中。
        /// </summary>
        public decimal ApprovingApplyMoney { get; set; }
        /// <summary>
        /// 报销费用－1月已审核。
        /// </summary>
        public decimal JanApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－2月已审核。
        /// </summary>
        public decimal FebApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－3月已审核。
        /// </summary>
        public decimal MarApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－4月已审核。
        /// </summary>
        public decimal AprApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－5月已审核。
        /// </summary>
        public decimal MayApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－6月已审核。
        /// </summary>
        public decimal JunApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－7月已审核。
        /// </summary>
        public decimal JulApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－8月已审核。
        /// </summary>
        public decimal AugApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－9月已审核。
        /// </summary>
        public decimal SepApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－10月已审核。
        /// </summary>
        public decimal OctApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－11月已审核。
        /// </summary>
        public decimal NovApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－12月已审核。
        /// </summary>
        public decimal DecApprovedMoney { get; set; }
        /// <summary>
        /// 报销费用－小计。
        /// </summary>
        public decimal Subtotal { get; set; }
    }
}
