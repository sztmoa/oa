using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Common
{
    public class Enums
    {
        #region "   Record Status   "

        /// <summary>
        /// 用于表示记录的状态
        /// 0 无效；1 有效
        /// </summary>
        public enum Status
        {
            /// <summary>
            /// 无效，过期
            /// </summary>
            InValid = 0,
            /// <summary>
            /// 有效
            /// </summary>
            Valid = 1
        }

        #endregion

        #region "   VisitAction     "
        /// <summary>
        /// 访问某一个页面时，
        /// 指示该动作是去编辑，是新增
        /// 以便在页面中控制保存，保存并关闭
        /// 和提交三个按钮的显示
        /// </summary>
        public enum VisitAction
        {
            /// <summary>
            /// 新增
            /// </summary>
            Add = 0,
            /// <summary>
            /// 编辑
            /// </summary>
            Edit = 1,
            /// <summary>
            /// 查看
            /// </summary>
            View = 2
        }
        #endregion

        #region "   OperateAction   "
        /// <summary>
        /// 页面中的操作动作
        /// </summary>
        public enum OperateAction
        {
            /// <summary>
            /// 保存
            /// </summary>
            Save = 0,
            /// <summary>
            /// 保存并关闭
            /// </summary>
            SaveAndClose = 1,
            /// <summary>
            /// 提交并关闭
            /// </summary>
            Sumbit = 2
        }
        #endregion

        #region "   ResultValue     "
        /// <summary>
        /// 指示json返回时的状态
        /// </summary>
        public enum Result
        {
            /// <summary>
            /// 成功 0
            /// </summary>
            Success = 0,

            /// <summary>
            /// 失败
            /// 9999
            /// </summary>
            Fail = 9999,
            /// <summary>
            /// 未设置第一时段打卡 1000
            /// </summary>
            NonFirstSetting = 1000,
            /// <summary>
            /// 未设置第二时段打卡 1001
            /// </summary>
            NonSecondSetting = 1001,
            /// <summary>
            /// 有重复记录存在 1002
            /// </summary>
            HasDuplicateRecord = 1002,   
            /// <summary>
            /// 销假时间超过了请假时间范围 1003
            /// </summary>
            IsOutOfLeaveDateRange = 1003,
            /// <summary>
            /// 是工作日，不算加班 1004
            /// </summary>
            IsWorkDay = 1004,
            /// <summary>
            /// 上午是工作日，不算加班 1005
            /// </summary>
            IsHalfDayMorningWork = 1005,
            /// <summary>
            /// 下午是工作日，不算加班 1006
            /// </summary>
            IsHalfDayNoonWork = 1006,
            /// <summary>
            /// 请五四青年节假期超龄 1007
            /// </summary>
            IsPastYouthDay = 1007,
            /// <summary>
            /// 事后请假超过一天 1008
            /// </summary>
            IsPastOneDay = 1008,
            /// <summary>
            /// 未设置生日 1009
            /// </summary>
            NonBirthday = 1009,
            /// <summary>
            /// 请假时长超过了最大允许天数 1010
            /// </summary>
            IsPastMaxVacationDays = 1010,
            /// <summary>
            /// 一次性休完的假期，
            /// 不允许分多次休 1011
            /// </summary>
            IsOnceRestVacation = 1011,
            /// <summary>
            /// 已经存在外出申请，不允许请假 1012
            /// </summary>
            HasOutApply = 1012,
            /// <summary>
            /// 已经存在出差申请，不允许请假 1013
            /// </summary>
            HasEvection = 1013,
            /// <summary>
            /// 未找到对应的考勤方案 1014
            /// </summary>
            NonAttendenceSolution = 1014,
            /// <summary>
            /// 未找到对应的考勤方案 1015
            /// </summary>
            NonLeaveRecord = 1015,
            /// <summary>
            /// 该假期只允许男性 1016
            /// </summary>
            IsOnlyForMale = 1016,
            /// <summary>
            /// 该假期只允许女性 1017
            /// </summary>
            IsOnlyForFemale = 1017,
            /// <summary>
            /// 五四假期已经过期
            /// </summary>
            YouthDayHasExpried = 1018,
            /// <summary>
            /// 三班妇女节假期已经过期
            /// </summary>
            WomenDayHasExpried = 1019,
            /// <summary>
            /// 请假时间必须大于1小时
            /// </summary>
            MinLeaveHours = 1020,
            /// <summary>
            /// 年假天数为0
            /// </summary>
            NotEnoughAnnualDays = 1021,
            /// <summary>
            /// 调休假天数为0
            /// </summary>
            NotEnoughAdjustLeaveDays = 1022,
            /// <summary>
            /// 有重复记录存在 1002
            /// </summary>
            HasDuplicateOTRecord=1023,
            /// <summary>
            /// /调休假天数为0
            /// </summary>
            NotEnoughVacationDays=1024,
            /// <summary>
            /// 时候提单必须一天内进行且不超过最大天数
            /// </summary>
            IsPastOneDayAndPastMaxdays=1025,

            /// <summary>
            /// 年假不能跨年
            /// </summary>
            AnnualVacationNotAllowCross=1026,

            /// <summary>
            /// 员工未转正/离职
            /// </summary>
            EmployeeNotNormal = 1027
            
        }

        #endregion

        #region "   Vacation Type   "

        public enum PublicVacationType
        {
            /// <summary>
            /// 元旦节
            /// </summary>
            SunraiseDay = 1,
            /// <summary>
            /// 春节
            /// </summary>
            SpringFestivalDay = 2,
            /// <summary>
            /// 三八妇女节
            /// </summary>
            WomenDay = 3,
            /// <summary>
            /// 清明节
            /// </summary>
            QingmingDay = 4,
            /// <summary>
            /// 五一劳动节
            /// </summary>
            LaborDay = 5,
            /// <summary>
            /// 五四青年节
            /// </summary>
            YouthDay = 6,
            /// <summary>
            /// 五月初五端午节
            /// </summary>
            DragonDay = 7,
            /// <summary>
            /// 八月十五中秋节
            /// </summary>
            AutumnDay = 8,
            /// <summary>
            /// 十一国庆节
            /// </summary>
            NationalDay = 9,
            /// <summary>
            /// 加班假期
            /// </summary>
            OverTime = 10
        }

        #endregion

        #region "   Leave Vacation Type "

        /// <summary>
        /// 请假假期类型
        /// </summary>
        public enum LeaveVacationType
        {
            /// <summary>
            /// 调休假
            /// </summary>
            AdjustLeaveDay = 1,
            /// <summary>
            /// 事假
            /// </summary>
            AffairLeaveDay = 2,
            /// <summary>
            /// 病假
            /// </summary>
            SickLeaveDay = 3,
            /// <summary>
            /// 年假
            /// </summary>
            AnnualDay = 4,
            /// <summary>
            /// 产假
            /// </summary>
            MaternityLeaveDay = 5,
            /// <summary>
            /// 婚假
            /// </summary>
            MarriageLeaveDay = 6,
            /// <summary>
            /// 陪产假
            /// </summary>
            AccompanyLeaveDay = 7,
            /// <summary>
            /// 路程假
            /// </summary>
            RoadLeaveDay = 8,
            /// <summary>
            /// 工伤假
            /// </summary>
            InjuryLeaveDay = 9,
            /// <summary>
            /// 丧假
            /// </summary>
            FuneralLeaveDay = 10,
            /// <summary>
            /// 产检假
            /// </summary>
            PrenatalExamLeaveDay = 11,
            /// <summary>
            /// 五四青年节
            /// </summary>
            YouthLeaveDay = 12,
            /// <summary>
            /// 三八妇女节
            /// </summary>
            WomenDay = 13,
            /// <summary>
            /// 探亲假
            /// </summary>
            ViewParentDay = 14

        }

        #endregion

        #region "   Leave/Cancel Action "
        /// <summary>
        /// 请假or销假
        /// </summary>
        public enum LeaveAction
        {
            /// <summary>
            /// 请假
            /// </summary>
            Leave = 1,
            /// <summary>
            /// 销假
            /// </summary>
            LeaveCancel = 2
        }
        #endregion


        #region "   工龄对应的年假天数 "

        /// <summary>
        /// ⑵	毕业后参加工作满1年不满10年的,年假5天；
        /// ⑶	毕业后参加工作满10年不满20年的,年假10天；
        /// ⑷	参加工作已满20年的,年假15天；
        /// </summary>
        public enum AnnualYearVacationDays
        {
            /// <summary>
            /// 0-10年，5天
            /// </summary>
            LessTenYears = 5,
            /// <summary>
            /// 5-10年，10天
            /// </summary>
            TenToTwentyYears = 10,
            /// <summary>
            /// 10-20年，15天
            /// </summary>
            TwentyYears = 15
        }

        #endregion

        public enum TaskType
        {
            /// <summary>
            /// 任务下达 0
            /// </summary>
            TaskLower = 0,

            /// <summary>
            /// 任务协同 1
            /// </summary>
            TaskTogether = 1,

            /// <summary>
            /// 任务支持 2
            /// </summary>
            TaskUpper = 2,

            /// <summary>
            /// 共同的上级员工ID
            /// </summary>
            TaskLeader = 3
        }
    }
}
