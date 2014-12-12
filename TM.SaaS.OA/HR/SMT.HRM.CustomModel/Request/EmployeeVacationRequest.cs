using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SMT.SaaS.Common.Query;

namespace SMT.HRM.CustomModel
{
    [Serializable]
    [DataContract]
    public class EmployeeVacationRequest
    {
        [DataMember]
        public string EmployeeID { get; set; }

        [DataMember]
        public int YearPeriod { get; set; }

        [DataMember]
        public string SortField { get; set; }

        [DataMember]
        public string SortDirection { get; set; }       
    }
}
