using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Request
{
    [DataContract]
    public class EmployeeLeaveDayRequest
    {
        [DataMember]
        public string EmployeeID { get; set; }

        [DataMember]
        public string LeaveTypeID { get; set; }

        [DataMember]
        public string LeaveTypeValue { get; set; }

        [DataMember]
        public DateTime LeaveStartDate { get; set; }

        [DataMember]
        public DateTime LeaveEndDate { get; set; }
    }
}
