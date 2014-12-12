using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    public class ExecutionConditions
    {
        private int checkStates = -1;
        private int orgnizationType = -1;

        /// <summary>
        /// 机构类型（0：公司；1：部门；2：岗位；3：个人）。
        /// </summary>
        public int OrgnizationType
        {
            get
            {
                return this.orgnizationType;
            }
            set
            {
                this.orgnizationType = value;
            }
        }

        /// <summary>
        /// 机构（公司）、部门、岗位 ID。
        /// </summary>
        public string OrgnizationID { get; set; }

        /// <summary>
        /// 机构(公司) ID。
        /// </summary>
        public string OwnerCompanyID { get; set; }

        /// <summary>
        /// 机构(公司)名称。
        /// </summary>
        public string OwnerCompanyName { get; set; }

        /// <summary>
        /// 部门编号。
        /// </summary>
        public string OwnerDepartmentID { get; set; }

        /// <summary>
        /// 部门名称。
        /// </summary>
        public string OwnerDepartmentName { get; set; }

        /// <summary>
        /// 职位 ID。
        /// </summary>
        public string OwnerPostID { get; set; }

        /// <summary>
        /// 职位名称。
        /// </summary>
        public string OwnerPostName { get; set; }

        /// <summary>
        /// 项目（科目） ID。
        /// </summary>
        public string SubjectID { get; set; }

        /// <summary>
        /// 项目（科目）名称。
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 开始时间。
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// 开始时间。
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// 查询个人。
        /// </summary>
        public bool IsPersonal { get; set; }

        /// <summary>
        /// 查询个人。
        /// </summary>
        public string OwnerID { get; set; }

        /// <summary>
        /// 审查状态（0 未提交；1 审核中；2 审核通过；3 审核未通过；4 保存。）。
        /// </summary>
        public int CheckStates
        {
            get
            {
                return this.checkStates;
            }
            set
            {
                this.checkStates = value;
            }
        }

        /// <summary>
        /// 在前在线的登录用户。
        /// </summary>
        public string CurrentOnlineUser { get; set; }
    }
}
