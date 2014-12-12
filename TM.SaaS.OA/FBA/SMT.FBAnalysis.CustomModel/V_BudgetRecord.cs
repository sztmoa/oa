using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    public class V_BudgetRecord
    {
        /// <summary>
        /// 单据ID。
        /// </summary>
        public string RecordID { get; set; }
        /// <summary>
        /// 单据信息XML。
        /// </summary>
        public string XmlObjectValue { get; set; }
        /// <summary>
        /// 单据编号。
        /// </summary>
        public string RecordCode { get; set; }
        /// <summary>
        /// 创建人ID。
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 创建人姓名。
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 申请人ID。
        /// </summary>
        public string OwnerID { get; set; }
        /// <summary>
        /// 申请人姓名。
        /// </summary>
        public string OwnerName { get; set; }
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
        /// 单据审核状态。
        /// </summary>
        public decimal CheckState { get; set; }
        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 单据金额。
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// 单据类型。
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 科目ID。
        /// </summary>
        public string SubjectID { get; set; }
        /// <summary>
        /// 科目名称。
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
