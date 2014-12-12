using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Request
{
    [DataContract]
    public class CalculateLeaveCancelRequest
    {
        [DataMember]
        public string EmployeeID { get; set; }
        /// <summary>
        /// 请假记录ID
        /// </summary>
        [DataMember]
        public string LeaveRecordID { get; set; }

        /// <summary>
        /// 假期类型ID
        /// </summary>
        [DataMember]
        public string LeaveTypeID { get; set; }

        [DataMember]
        public int LeaveTypeValue { get; set; }

        [DataMember]
        public string StartDate { get; set; }

        [DataMember]
        public string EndDate { get; set; }

        [DataMember]
        public string StartTime { get; set; }

        [DataMember]
        public string EndTime { get; set; }



    }
}
