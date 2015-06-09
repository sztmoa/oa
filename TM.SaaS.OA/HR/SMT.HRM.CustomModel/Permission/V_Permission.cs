using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TM_SaaS_OA_EFModel;

namespace SMT.HRM.CustomModel.Permission
{
    public class V_Permission
    {
        public T_SYS_ROLEMENUPERMISSION RoleMenuPermission { get; set; }
        public T_SYS_ENTITYMENU EntityMenu { get; set; }
        public T_SYS_PERMISSION Permission { get; set; }
        public T_SYS_ROLEENTITYMENU EntityRole { get; set; }
        public T_SYS_ROLE Role { get; set; }
        public T_SYS_USER SysUser { get; set; }
        public int Flag { get; set; }

    }


    public enum Permissions
    {
        Add,// 0 
        Edit,// 1
        Delete,//2
        Browse,// 3
        Export,
        Report,
        Audit,
        Import,
    }
}
