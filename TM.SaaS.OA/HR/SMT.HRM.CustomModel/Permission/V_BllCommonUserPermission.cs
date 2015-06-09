using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TM_SaaS_OA_EFModel;

namespace SMT.HRM.CustomModel.Permission
{
    public class V_BllCommonUserPermission
    {   
        public string PermissionDataRange { get; set; }
        public string RoleMenuPermissionValue { get; set; }
        public string OwnerCompanyID { get; set; }
        public string OwnerDepartmentID { get; set; }
        public string OwnerPostID { get; set; }
        
    }
}
