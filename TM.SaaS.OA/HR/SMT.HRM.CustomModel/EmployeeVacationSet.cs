using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel
{
    [Serializable]
    [DataContract]
    public class EmployeeVacationSet
    {
        [DataMember]
        public decimal ACCOMPANY_CARE_LEAVE { get; set; }

        [DataMember]
        public decimal AFFAIR_LEAVE_HOURS { get; set; }

        [DataMember]
        public decimal ANNUAL_HOURS { get; set; }

        [DataMember]
        public decimal ANNUALDAY { get; set; }

        [DataMember]
        public string ANNUALDAY_EFFECT_DAY { get; set; }

        [DataMember]
        public decimal ANNUALDAY_USED { get; set; }

        [DataMember]
        public decimal ANNUALHOURS_USED { get; set; }

        [DataMember]
        public decimal BREAST_FEEDING_LEAVE { get; set; }

        [DataMember]
        public string CREATE_COMPANYID { get; set; }

        [DataMember]
        public DateTime CREATE_DATE { get; set; }

        [DataMember]
        public string CREATE_DEPARTMENTID { get; set; }

        [DataMember]
        public string CREATE_POSTID { get; set; }

        [DataMember]
        public string CREATE_USERID { get; set; }

        [DataMember]
        public string EMPLOYEECODE { get; set; }

        [DataMember]
        public string EMPLOYEEID { get; set; }

        [DataMember]
        public string EMPLOYEENAME { get; set; }

        [DataMember]
        public decimal FUNERAL_LEAVE { get; set; }


        [DataMember]
        public decimal INJURY_LEAVE { get; set; }

        [DataMember]
        public decimal MARRIAGE_LEAVE { get; set; }

        [DataMember]
        public decimal MATERNITY_LEAVE { get; set; }
        
        [DataMember]
        public decimal OT_LEAVE_HOURS { get; set; }

        [DataMember]
        public decimal OT_TOTAL_HOURS { get; set; }

        [DataMember]
        public string OWNER_COMPANYID { get; set; }

        [DataMember]
        public string OWNER_DEPARTMENTID { get; set; }


        [DataMember]
        public string OWNER_POSTID { get; set; }

        [DataMember]
        public string OWNERID { get; set; }

        [DataMember]
        public decimal PRENATAL_EXAM_LEAVE { get; set; }

        [DataMember]
        public decimal ROADING_LEAVE { get; set; }

        [DataMember]
        public decimal SICK_HOURS { get; set; }

        [DataMember]
        public decimal SICK_LEAVE_HOURS { get; set; }

        [DataMember]
        public DateTime UPDATE_DATE { get; set; }

        [DataMember]
        public string UPDATE_USERID { get; set; }

        [DataMember]
        public decimal WOMEN_DAY { get; set; }

        [DataMember]
        public string YEAR_PERIOD { get; set; }

        [DataMember]
        public decimal YOUTH_DAY { get; set; }

    }
}
