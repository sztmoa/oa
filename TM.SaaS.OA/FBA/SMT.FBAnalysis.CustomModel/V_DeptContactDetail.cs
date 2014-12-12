using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    /// <summary>
    /// 部门借还款往来视图实体
    /// </summary>
    public class V_DeptContactDetail
    {
        /// <summary>
        /// 申请人ID。
        /// </summary>
        public string OwnerID { get; set; }
        /// <summary>
        /// 申请人姓名。
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 申请人ID。
        /// </summary>
        public string OwnerPostID { get; set; }
        /// <summary>
        /// 申请人岗位姓名。
        /// </summary>
        public string OwnerPostName { get; set; }
        /// <summary>
        /// 申请部门ID。
        /// </summary>
        public string DepartmentID { get; set; }
        /// <summary>
        /// 申请部门名称。
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// 申请公司ID。
        /// </summary>
        public string CompanyID { get; set; }
        /// <summary>
        /// 申请公司名称。
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 普通借款金额。
        /// </summary>
        public decimal NormalMoney { get; set; }
        /// <summary>
        /// 专项借款金额。
        /// </summary>
        public decimal SpecialMoney { get; set; }
        /// <summary>
        /// 备用金借款金额。
        /// </summary>
        public decimal ReserveMoney { get; set; }
        /// <summary>
        /// 借款金额合计。
        /// </summary>
        public decimal TotalMoney { get; set; }        
    }
}
