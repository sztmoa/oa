using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.HRM.CustomModel.Permission
{
    /// <summary>
    /// 预算菜单类
    /// </summary>
    public class FbMenuPermission
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public string menuID { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string menuName { get; set; }
        /// <summary>
        /// 权限值
        /// </summary>
        public string permissionVlaue { get; set; }
        /// <summary>
        /// 权限范围
        /// 1 公司
        /// 2 部门
        /// </summary>
        public string permissionText { get; set; }
    }
}
