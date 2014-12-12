using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Response
{
    [DataContract]
    public class CalculateLeaveCancelResponse
    {               
        /// <summary>
        /// 结果代码，数值
        /// 0 成功
        /// 1000 
        /// LeaveCommon.Result.Success.GetHashCode()
        /// </summary>
        [DataMember]
        public int Result { get; set; }

        /// <summary>
        /// 返回错误的消息
        /// 默认为空表示成功
        /// 此字段为了兼容老版本
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        #region "   返回值，状态      "

        /// <summary>
        /// 销假时间超过了请假时间范围
        /// 0 否，1 是
        /// </summary>
        [DataMember]
        public int IsOutOfLeaveDateRange { get; set; }

        /// <summary>
        /// 是否设置了第一时间段打卡
        /// 0 未设置；1 已设置
        /// = hasFirstSetting
        /// </summary>
        [DataMember]
        public int FirstSetting { get; set; }

        /// <summary>
        /// 是否设置了第二时间段打卡
        /// 0 未设置；1 已设置
        /// = hasSecondSetting
        /// </summary>
        [DataMember]
        public int SecondSetting { get; set; }

        /// <summary>
        /// 重复的请假请单
        /// 0，未重复
        /// 1，重复
        /// </summary>
        [DataMember]
        public int HasDuplicateRecord { get; set; }

        /// <summary>
        /// 是否工作日
        /// 0，不是
        /// 1，是
        /// </summary>
        [DataMember]
        public int IsWorkDay { get; set; }

        /// <summary>
        /// 上午是上班，不能算加班
        /// 0 不是；1 是
        /// </summary>
        [DataMember]
        public int IsHalfDayMorningWork { get; set; }

        /// <summary>
        /// 下午是上班，不能加班
        /// 0 不是；1 是
        /// </summary>
        [DataMember]
        public int IsHalfDayNoonWork { get; set; }

        /// <summary>
        /// 五四青年假超龄
        /// 默认值：0
        /// </summary>
        [DataMember]
        public int IsPastYouthDay { get; set; }

        /// <summary>
        /// 事后请假超过一天时间
        /// 默认值：0
        /// </summary>
        [DataMember]
        public int IsPastOneDay { get; set; }

        /// <summary>
        /// 请五四假期根据生日计算
        /// 默认值：1 有
        /// 0 无
        /// </summary>
        [DataMember]
        public int HasBirthday { get; set; }


        /// <summary>
        /// 是否请假超过了5天
        /// 0 否，1 是
        /// </summary>
        [DataMember]
        public decimal IsPastMaxVacationDays { get; set; }

        /// <summary>
        /// 必须一次休完的假期
        /// 默认值：0,1 一次休完
        /// </summary>
        [DataMember]
        public int IsOnceRestVacation { get; set; }

        /// <summary>
        /// 是否有外出申请
        /// 0 无，1 有
        /// </summary>
        [DataMember]
        public int HasOutApply { get; set; }

        /// <summary>
        /// 是否有出差申请
        /// 0 无，1 有
        /// </summary>
        [DataMember]
        public int HasEvection { get; set; }

        #endregion

        #region "   返回值，数据      "

        /// <summary>
        /// 考勤月份
        /// = strMonth
        /// </summary>
        [DataMember]
        public string Month { get; set; }

        /// <summary>
        /// 考勤方案名称
        /// = ATTENDANCESOLUTIONNAME
        /// </summary>
        [DataMember]
        public string AttendSolution { get; set; }

        /// <summary>
        /// 考勤方案工作日时长
        /// = WORKTIMEPERDAY
        /// </summary>
        [DataMember]
        public decimal WorkPerDay { get; set; }

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
        /// 销假天数
        /// </summary>
        [DataMember]
        public double? CancelDays { get; set; }

        /// <summary>
        /// 销假零头，小时数
        /// </summary>
        [DataMember]
        public double? CancelHours { get; set; }

        /// <summary>
        /// 总的销假时长
        /// </summary>
        [DataMember]
        public double? CancelTotalHours { get; set; }

        /// <summary>
        /// 总的销假天数
        /// </summary>
        [DataMember]
        public double? CancelTotalDays { get; set; }

        /// <summary>
        /// 该假期的剩余小时数    
        /// </summary>
        [DataMember]
        public double LeftHours { get; set; }

        /// <summary>
        /// 剩余天数，折合计算而来
        /// </summary>
        [DataMember]
        public double LeftDays { get; set; }


        /// <summary>
        /// 重复开始时间段
        /// </summary>
        [DataMember]
        public string DuplicateStartDate { get; set; }

        /// <summary>
        /// 重复结束时间段
        /// </summary>
        [DataMember]
        public string DuplicateEndDate { get; set; }


        /// <summary>
        /// 扣款性质
        /// 0 不扣，1 扣款，2 调休+扣款，3 调休+带薪假+扣款
        /// </summary>
        [DataMember]
        public string FineType { get; set; }

        /// <summary>
        /// 性别限制
        /// 0 不限；1 男；2 女
        /// </summary>
        [DataMember]
        public string SexRestrict { get; set; }

        /// <summary>
        /// 该类假期最大可请假天数
        /// </summary>
        [DataMember]
        public double MaxVacationDays { get; set; }

        /// <summary>
        /// 年假的天数/小时数
        /// </summary>
        [DataMember]
        public double AnnualVacationHours { get; set; }
        /// <summary>
        /// 年假的天数/小时数
        /// </summary>
        [DataMember]
        public double AnnualVacationDays { get; set; }

        /// <summary>
        /// 可调休假小时数
        /// </summary>
        [DataMember]
        public double AdjustLeaveHours { get; set; }
        /// <summary>
        /// 可调休假
        /// </summary>
        [DataMember]
        public double AdjustLeaveDays { get; set; }

        /// <summary>
        /// 平均每日工作天数
        /// </summary>
        [DataMember]
        public double AvgWorkPerDay { get; set; }

        /// <summary>
        /// 返回可休假列表中加班列表
        /// </summary>
        [DataMember]
        public List<string> OvertTimeList { get; set; }

        #endregion  
    }
}
