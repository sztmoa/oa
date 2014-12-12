using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    /// <summary>
    /// 个人借还款往来视图实体
    /// </summary>
    public class V_ContactDetail
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
        /// 关联单据编号(费用报销，还款，对应借款单据编号)。
        /// </summary>
        public string RelRecordCode { get; set; }        
        /// <summary>
        /// 创建人ID。
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 创建人姓名。
        /// </summary>
        public string CreateUserName { get; set; }
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
        /// 借款类型。
        /// </summary>
        public string BorrowType { get; set; }
        /// <summary>
        /// 还款类型。(逗号分隔)
        /// </summary>
        public string RepayType { get; set; }
        /// <summary>
        /// 借款金额。
        /// </summary>
        public decimal BorrowMoney { get; set; }
        /// <summary>
        /// 还款金额。
        /// </summary>
        public decimal RepayMoney { get; set; }
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

    }


}
