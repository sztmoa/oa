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
    public class VacationForAdjustRequest
    {
        [DataMember]
        public string EmployeeID { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string OTID { get; set; }
    }
}
