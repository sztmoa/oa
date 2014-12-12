using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    /// <summary>
    /// 台帐视图实体
    /// </summary>
    public class V_StandingBook
    {
        /// <summary>
        /// 单据ID。
        /// </summary>
        public string RecordID { get; set; }
        /// <summary>
        /// 单据编号。
        /// </summary>
        public string RecordCode { get; set; }
        /// <summary>
        /// 单据信息XML。
        /// </summary>
        public string XmlObjectValue { get; set; }                
        /// <summary>
        /// 项目(科目) ID。
        /// </summary>
        public string SubjectID { get; set; }
        /// <summary>
        /// 项目(科目)名称。
        /// </summary>
        public string SubjectName { get; set; }
        /// <summary>
        /// 预算额度。
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// 单据状态
        /// </summary>
        public string ChecksatesName { get; set; }
        /// <summary>
        /// 创建时间。
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTime UpdateDate { get; set; }        
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
    }
}
