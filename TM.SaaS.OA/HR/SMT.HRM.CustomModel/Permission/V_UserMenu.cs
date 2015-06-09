using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TM_SaaS_OA_EFModel;

namespace SMT.HRM.CustomModel.Permission
{
    public class V_UserMenu
    {
        
        public T_SYS_ENTITYMENU EntityMenu { get; set; }
        public T_SYS_ROLEENTITYMENU RoleEntity { get; set; }
        public T_SYS_USER sysuser { get; set; }
        public T_SYS_USERROLE sysuserrole { get; set; }
    }
}
