using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Request
{
    [DataContract]
    public  class EmployeeVacationDayCountRequest
    {
        [DataMember]
        public string OwnerID { get; set; }

        [DataMember]
        public string EmployeeID { get;set;}

        [DataMember]
        public string EmployeeName { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public DateTime EndDate { get; set; }
    }
}
