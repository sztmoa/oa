using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAL
{
    public class FBAcountCheck
    {
        public string ID;
        public string SUBJECTID;
        public string OWNERDEPARTMENTID;
        public string OWNERCOMPANYID;
        public decimal UsableMoney;
        public decimal PaidMoney;
        public decimal Bugedmoney;
        /// <summary>
        /// 预算类型 1 : 公司, 2 : 部门, 3 : 个人
        /// </summary>
        public string AcountType;
        public decimal year;
        public decimal month;

        public string CompanyName;
        public string DepartmentName;
        public string AccountTypeName;
        public string subjectName;
        
    }
}
