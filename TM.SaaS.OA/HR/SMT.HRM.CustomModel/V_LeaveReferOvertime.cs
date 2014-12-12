using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT_HRM_EFModel;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel
{
    [Serializable]
    [DataContract]
   public class V_LeaveReferOvertime : T_HR_LEAVEREFEROT
    {
        [DataMember]
        public DateTime LeaveStartDate { get; set; }

        [DataMember]
        public DateTime LeaveEndDate { get; set; }

        [DataMember]
        public DateTime OvertimeStartDate { get; set; }

        [DataMember]
        public DateTime OvertimeEndDate { get; set; }

        [DataMember]
        public decimal OvertimeHours { get; set; }

        [DataMember]
        public string LeaveTypeValue { get; set; }

        [DataMember]
        public string EmployeeName { get; set; }

        [DataMember]
        public string LeaveReason { get; set; }

        [DataMember]
        public decimal LeaveHours { get; set; }

    }
}
