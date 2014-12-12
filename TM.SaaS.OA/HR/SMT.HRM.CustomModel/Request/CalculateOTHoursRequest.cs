using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using SMT.SaaS.Common.Query;

namespace SMT.HRM.CustomModel.Request
{
    [Serializable]
    [DataContract]
    public class CalculateOTHoursRequest
    {
        [DataMember]
        public string EmployeeID { get; set; }

        [DataMember]
        public string StartDate { get; set; }

        [DataMember]
        public string EndDate { get; set; }

        [DataMember]
        public string StartTime { get; set; }

        [DataMember]
        public string EndTime { get; set; }

        [DataMember]
        public string OverTimeRecordID { get; set; }
    }
}
