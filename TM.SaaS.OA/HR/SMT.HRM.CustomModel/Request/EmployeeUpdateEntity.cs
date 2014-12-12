using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Request
{
    [Serializable]
    [DataContract]
    public class EmployeeVacationUpdateEntity
    {
        /// <summary>
        /// 年份
        /// </summary>
        [DataMember]
        public string YearPeriod { get; set; }
        /// <summary>
        /// 员工ID
        /// </summary>
        [DataMember]
        public string EmployeeID { get; set; }
        /// <summary>
        /// 假期类型
        /// </summary>
        [DataMember]
        public int VacationType { get; set; }
        /// <summary>
        /// 时长，加班为整数，调休为负数
        /// </summary>
        [DataMember]
        public decimal TotalHours { get; set; }
        /// <summary>
        /// 天数，加班为整数，调休为负数
        /// </summary>
        [DataMember]
        public decimal TotalDays { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember]
        public DateTime UpdateDate { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [DataMember]
        public string UpdateUserID { get; set; }
    }
}
