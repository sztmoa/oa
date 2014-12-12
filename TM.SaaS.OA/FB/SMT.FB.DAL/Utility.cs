using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PersonnelWS = SMT.SaaS.BLLCommonServices.PermissionWS;
using PermissionWS = SMT.SaaS.BLLCommonServices.PermissionWS;
using SMT.SaaS.BLLCommonServices.PersonnelWS;
namespace SMT.FB.DAL
{
    public class Utility
    {
        /// <summary>
        /// 分配对象类别
        /// </summary>
        public enum AssignedObjectType
        {
            /// <summary>
            /// 公司
            /// </summary>
            Company,
            /// <summary>
            /// 部门
            /// </summary>
            Department,
            /// <summary>
            /// 岗位
            /// </summary>
            Post
        }

        /// <summary>
        /// 分配对像的类型
        /// </summary>
        public enum AssignObjectType
        {
            Company,
            Department,
            Post,
            Employee,
            Organize
        }
        

        protected PermissionWS.PermissionServiceClient PermClient = new PermissionWS.PermissionServiceClient();
        protected PersonnelServiceClient PersonClient = new PersonnelServiceClient();

        public void SetOrganizationFilter(ref string filterString, ref List<object> queryParas, string employeeID, string entityName)
        {
            //注意，用户总是能看到自己创建的记录

            //获取正常的角色用户权限            

            int maxPerm = -1;

            //获取用户
            PermissionWS.T_SYS_USER user = PermClient.GetUserByEmployeeID(employeeID);
            //获取员工
            T_HR_EMPLOYEE emp = PersonClient.GetEmployeeByID(user.EMPLOYEEID);

            PermissionWS.V_Permission[] perms = PermClient.GetUserMenuPerms(entityName, user.SYSUSERID);

            bool hasPerms = true;

            if (perms == null)
            {
                // hasPerms = false;
                return;
            }
            else
            {
                //获取查询的权限,值越小，权限越大
                var tmpperms = perms.Where(p => p.Permission.PERMISSIONVALUE == "3").ToList();
                if (tmpperms == null || tmpperms.Count <= 0)
                    hasPerms = false;
                else
                    maxPerm = tmpperms.Min(p => Convert.ToInt32(p.RoleMenuPermission.DATARANGE));
            }

            if (hasPerms == false)
            {
                //if (!string.IsNullOrEmpty(filterString))
                //    filterString += " AND ";
                //filterString += " (1==0 OR CREATEUSERID==@" + queryParas.Count().ToString() + ")";
                //queryParas.Add(employeeID);
                filterString += "1 != 1";
                return;
            }


            //取员工岗位
            V_EMPLOYEEPOST emppost = PersonClient.GetEmployeeDetailByID(emp.EMPLOYEEID);

            emp.T_HR_EMPLOYEEPOST = emppost.EMPLOYEEPOSTS;


            //获取自定义权限
            int custPerm = GetCustomPerms(entityName, emp);
            if (custPerm < maxPerm)
                maxPerm = custPerm;

            //看整个集团的
            if (Convert.ToInt32(maxPerm) == Convert.ToInt32(AssignObjectType.Organize))
            {
                return;
            }

            //看整个公司的
            if (Convert.ToInt32(maxPerm) == Convert.ToInt32(AssignObjectType.Company))
            {
                if (!string.IsNullOrEmpty(filterString))
                    filterString += " AND ";

                filterString += " ((";
                int i = 0;
                foreach (T_HR_EMPLOYEEPOST ep in emp.T_HR_EMPLOYEEPOST)
                {

                    if (i > 0)
                        filterString += " OR ";

                    filterString += "OWNERCOMPANYID==@" + queryParas.Count().ToString();

                    queryParas.Add(ep.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.COMPANYID);

                    i++;
                }
                filterString += ") OR CREATEUSERID==@" + queryParas.Count().ToString() + ")";
                queryParas.Add(employeeID);

            }


            //看部门的
            if (Convert.ToInt32(maxPerm) == Convert.ToInt32(AssignObjectType.Department))
            {
                if (!string.IsNullOrEmpty(filterString))
                    filterString += " AND ";

                filterString += " ((";
                int i = 0;
                foreach (T_HR_EMPLOYEEPOST ep in emp.T_HR_EMPLOYEEPOST)
                {

                    if (i > 0)
                        filterString += " OR ";

                    filterString += "OWNERDEPARTMENTID==@" + queryParas.Count().ToString();

                    queryParas.Add(ep.T_HR_POST.T_HR_DEPARTMENT.DEPARTMENTID);

                    i++;
                }

                filterString += ") OR CREATEUSERID==@" + queryParas.Count().ToString() + ")";
                queryParas.Add(employeeID);
            }


            //看岗位的
            if (Convert.ToInt32(maxPerm) == Convert.ToInt32(AssignObjectType.Post))
            {
                if (!string.IsNullOrEmpty(filterString))
                    filterString += " AND ";

                filterString += " ((";
                int i = 0;
                foreach (T_HR_EMPLOYEEPOST ep in emp.T_HR_EMPLOYEEPOST)
                {

                    if (i > 0)
                        filterString += " OR ";

                    filterString += "OWNERPOSTID==@" + queryParas.Count().ToString();

                    queryParas.Add(ep.T_HR_POST.POSTID);

                    i++;
                }
                filterString += ") OR CREATEUSERID==@" + queryParas.Count().ToString() + ")";
                queryParas.Add(employeeID);
            }

            //看员工
            if (Convert.ToInt32(maxPerm) == Convert.ToInt32(AssignObjectType.Employee))
            {
                if (!string.IsNullOrEmpty(filterString))
                    filterString += " AND ";

                filterString += " (( OWNERID==@" + queryParas.Count().ToString();
                queryParas.Add(employeeID);

                filterString += ") OR CREATEUSERID==@" + queryParas.Count().ToString() + ")";
                queryParas.Add(employeeID);
            }

        }

        private int GetCustomPerms(string menuCode, T_HR_EMPLOYEE emp)
        {
            int perm = 99;

            //过滤自定义的权限

            foreach (T_HR_EMPLOYEEPOST ep in emp.T_HR_EMPLOYEEPOST)
            {

                PermissionWS.T_SYS_ENTITYMENUCUSTOMPERM[] custPerms;
                //查看有没有岗位的特别权限
                custPerms = PermClient.GetCustomPostMenuPerms(menuCode, ep.T_HR_POST.POSTID);
                if (custPerms != null && custPerms.Count() > 0)
                    perm = Convert.ToInt32(AssignedObjectType.Post);

                //查看有没有部门的特别权限
                custPerms = PermClient.GetCustomDepartMenuPerms(menuCode, ep.T_HR_POST.T_HR_DEPARTMENT.DEPARTMENTID);
                if (custPerms != null && custPerms.Count() > 0)
                    perm = Convert.ToInt32(AssignedObjectType.Department);

                //查看有没有公司的特别权限
                custPerms = PermClient.GetCustomCompanyMenuPerms(menuCode, ep.T_HR_POST.T_HR_DEPARTMENT.T_HR_COMPANY.COMPANYID);
                if (custPerms != null && custPerms.Count() > 0)
                    perm = Convert.ToInt32(AssignedObjectType.Company);
            }

            return perm;
        }
    }
}
