using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Request
{
    [DataContract]
   public class CaculateLeaveHoursRequest
    {
        [DataMember]
        public string EmployeeID { get; set; }

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

        [DataMember]
        public string LeaveRecordID { get; set; }
        /// <summary>
        /// 是保存，更新，还是获取
        /// </summary>
        [DataMember]
        public bool IsSave { get; set; }
    }
}
