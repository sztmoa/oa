using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Response
{
    [Serializable]
    [DataContract]
    public class CalculateOTHoursResponse
    {
        /// <summary>
        /// 返回结果
        /// 0 失败；1 成功
        /// </summary>
        [DataMember]
        public int Result { get; set; }
        /// <summary>
        /// 返回错误的消息
        /// 默认为空表示成功
        /// 此字段为了兼容老版本
        /// message请参考Constants里面
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// 加班时长/请假时长
        /// </summary>
        [DataMember]
        public double OTHours { get; set; }// = totalHours,

        /// <summary>
        /// 加班天数/请假天数
        /// </summary>
        [DataMember]
        public double OTDays { get; set; }// = Math.Round(totalHours / Convert.ToDouble(OTPeriodAttendSolution.WORKTIMEPERDAY), 2),

        /// <summary>
        /// 考勤月份
        /// </summary>
        [DataMember]
        public string Month { get; set; }// = strMonth,

        /// <summary>
        /// 考勤方案名称
        /// </summary>
        [DataMember]
        public string AttendSolution { get; set; }// = OTPeriodAttendSolution.ATTENDANCESOLUTIONNAME,

        /// <summary>
        /// 考勤方案工作日时长
        /// </summary>
        [DataMember]
        public decimal WorkPerDay { get; set; }// = OTPeriodAttendSolution.WORKTIMEPERDAY       

        /// <summary>
        /// 考勤第一时间段
        /// </summary>
        [DataMember]
        public string FirstStartTime { get; set; }
        /// <summary>
        /// 考勤第二时间段
        /// </summary>
        [DataMember]
        public string SecondStartTime { get; set; }
        /// <summary>
        /// 考勤第三时间段
        /// </summary>
        [DataMember]
        public string ThirdStartTime { get; set; }
        /// <summary>
        /// 考勤第四时间段
        /// </summary>
        [DataMember]
        public string FourthStartTime { get; set; }

        /// <summary>
        /// 考勤第一时间段结束
        /// </summary>
        [DataMember]
        public string FirstEndTime { get; set; }
        /// <summary>
        /// 考勤第二时间段结束
        /// </summary>
        [DataMember]
        public string SecondEndTime { get; set; }
        /// <summary>
        /// 考勤第三时间段结束
        /// </summary>
        [DataMember]
        public string ThirdEndTime { get; set; }
        /// <summary>
        /// 考勤第四时间段结束
        /// </summary>
        [DataMember]
        public string FourthEndTime { get; set; }

        /// <summary>
        /// 加班开始时间
        /// </summary>
        [DataMember]
        public string StartTime { get; set; }
        /// <summary>
        /// 加班结束时间
        /// </summary>
        [DataMember]
        public string EndTime { get; set; }
    }
}
