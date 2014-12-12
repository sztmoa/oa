using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel
{
    /// <summary>
    /// 员工个人档案视图
    /// </summary>
    [Serializable]
    public class V_EMPLOYEECONTACT
    {
        public string EMPLOYEECONTACTID { get; set; }

        public string EMPLOYEECODE { get; set; }

        public string EMPLOYEENAME { get; set; }

        public string CONTACTCODE { get; set; }

        public DateTime? FROMDATE { get; set; }

        public DateTime? TODATE { get; set; }

        public DateTime? ENDDATE { get; set; }

        public string CHECKSTATE { get; set; }

        public string CONTACTSTATE { get; set; }
    }
}
