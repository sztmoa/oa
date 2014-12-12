using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.SaaS.OA.DAL.Views
{
    public class V_BrowseSendDocEmployee
    {
        /// <summary>
        /// 查看公司发文ID
        /// </summary>
        public string VIEWSENDDOCID { get; set; }
        /// <summary>
        /// 公司发文ID
        /// </summary>
        public string SENDDOCID { get; set; }
        public string EMPLOYEENAME { get; set; }
        public string POSTNAME { get; set; }
        public string DEPARTMENTNAME { get; set; }
        public string COMPANY { get; set; }
        public string OWNERCOMPANYID { get; set; }
        public string OWNERDEPARTMENTID { get; set; }        
        public string OWNERPOSTID { get; set; }
        public string OWNERID { get; set; }
        public string ISVIEW { get; set; }
    }
}
