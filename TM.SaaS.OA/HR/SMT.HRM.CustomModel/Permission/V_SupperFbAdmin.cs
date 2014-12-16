using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.SaaS.Permission.CustomerModel
{
    /// <summary>
    /// 预算超级管理员
    /// </summary>
    public class V_SupperFbAdmin
    {
        /// <summary>
        /// 系统用户ID
        /// </summary>
        public string SysUserID { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        public string EmployeeID { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 员工状态 0：禁用 1：正常
        /// </summary>
        public string EmployeeState { get; set; }        
        //可以设置的公司ID，会冗余
        public string CanSetCompanyID { get; set; }
    }
}
