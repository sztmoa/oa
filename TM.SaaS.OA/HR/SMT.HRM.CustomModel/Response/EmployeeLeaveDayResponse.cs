using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Response
{
    [DataContract]
    public class EmployeeLeaveDayResponse
    {
        [DataMember]
        public decimal LeaveTotalHours { get; set; }
    }
}
