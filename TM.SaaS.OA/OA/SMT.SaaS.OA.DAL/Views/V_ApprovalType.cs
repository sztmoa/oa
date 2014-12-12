using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.SaaS.OA.DAL.Views
{
    /// <summary>
    /// 事项审批类型
    /// </summary>
    public class V_ApprovalType
    {
        /// <summary>
        /// 所属公司
        /// </summary>
        public string COMPANYID { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string COMPANYNAME { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public string DEPARTMENTID { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DEPARTMENTNAME { get; set; }
        /// <summary>
        /// 岗位ID【选择岗位后事项类型后使用】
        /// </summary>
        public string POSTID { get; set; }
        /// <summary>
        /// 岗位名称【选择岗位后事项类型后使用】
        /// </summary>
        public string POSTNAME { get; set; }
        /// <summary>
        /// 员工姓名【选择岗位后事项类型后使用】
        /// </summary>
        public string EMPLOYEENAME { get; set; }
        /// <summary>
        /// 事项类型值
        /// </summary>
        public decimal APPROVALTYPE { get; set; }
        /// <summary>
        /// 事项类型名称
        /// </summary>
        public string APPROVALTYPENAME { get; set; }
        /// <summary>
        /// 父类型ID
        /// </summary>
        public string FATHERAPPROVALID { get; set; }
        /// <summary>
        /// 父节点对应的字典值
        /// </summary>
        public string FATHERVALUE { get; set; }
        /// <summary>
        /// 事项审批类型ID
        /// </summary>
        public string ApprovalTypeID { get; set; }

    }
}
