using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.HRM.CustomModel
{
    /// <summary>
    /// 员工考勤方案分配查询
    /// </summary>
    public class V_EMPLOYEEATTENDANCESOLUTIONASIGN
    {
        public string ATTENDANCESOLUTIONASIGNID { get; set; }			//考勤方案ID       
        public string ATTENDANCESOLUTIONID { get; set; }//考勤方案ID
        public string EMPLOYEENAME { get; set; }			//员工姓名 
        public string ATTENDANCETYPE { get; set; }  //考勤方式  1:打卡  2：:不考勤 3：登陆系统 4：打卡+登录
        public string CompanyName { get; set; }
        public string DepartmentName { get; set; }
        public string PostName { get; set; }
        public string ATTENDANCESOLUTIONNAME { get; set; }			//考勤方案名称
        public string CHECKSTATE { get; set; }			//审核状态
        public string ASSIGNEDOBJECTTYPE { get; set; }			//分配对象类型
        public string ASSIGNEDOBJECTID { get; set; }			//分配对象ID
        public DateTime? STARTDATE { get; set; }			//有效开始时间
        public DateTime? ENDDATE { get; set; }			//有效结束时间        
        public string REMARK { get; set; }			   //备注
        public string OWNERID { get; set; }			  //所属员工ID
        public string OWNERPOSTID { get; set; }			//所属岗位ID
        public string OWNERDEPARTMENTID { get; set; }			//所属部门ID
        public string OWNERCOMPANYID { get; set; }			//所属公司ID
        public string CREATEPOSTID { get; set; }			//创建人岗位ID
        public string CREATEDEPARTMENTID { get; set; }			//创建人部门ID
        public string CREATECOMPANYID { get; set; }			//创建人公司ID
        public string CREATEUSERID { get; set; }			//创建人
        public DateTime? CREATEDATE { get; set; }			//创建时间
        public string UPDATEUSERID { get; set; }			//修改人
        public DateTime? UPDATEDATE { get; set; }			//修改时间
        public string EMPLOYEEID { get; set; }			//员工ID
    }
}
