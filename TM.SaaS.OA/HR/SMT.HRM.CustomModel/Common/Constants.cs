using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.HRM.CustomModel.Common
{
    public class Constants
    {
        //添加字符串常量

        /// <summary>
        /// 保存成功
        /// </summary>
        public const string SaveSuccess = "保存成功！";
        /// <summary>
        /// 操作成功
        /// </summary>
        public const string Success = "操作成功！";
        /// <summary>
        /// 提交成功
        /// </summary>
        public const string SubmitSuccess = "提交成功！";
        /// <summary>
        /// 出错了，请联系管理员
        /// </summary>
        public const string Fail = "出错了，请联系管理员！";
        /// <summary>
        /// 保存数据失败，请联系管理员
        /// </summary>
        public const string SaveFail = "保存数据失败，请联系管理员！";
        /// <summary>
        /// 错误：系统未设置第一时段打卡
        /// </summary>
        public const string NonFirstSetting = "系统未设置第一时段打卡！";
        /// <summary>
        /// 错误：系统未设置第二时段打卡
        /// </summary>
        public const string NonSecondSetting = "系统未设置第二时段打卡！";
        /// <summary>
        /// 错误：系统有重复记录存在
        /// </summary>
        public const string HasDuplicateRecord = "该时间段已经有请假记录存在！";
        /// <summary>
        /// 错误：系统有重复记录存在
        /// </summary>
        public const string HasDuplicateOTRecord = "该时间段已经有加班记录存在！";
        /// <summary>
        /// 销假时间超出了请假时间范围
        /// </summary>
        public const string IsOutOfLeaveDateRange = "销假时间超出了请假时间范围！";
        /// <summary>
        /// 错误：工作日期间无法申请加班
        /// </summary>
        public const string IsWorkDay = "工作日期间无法申请加班！";
        /// <summary>
        /// 错误：上午是工作日，无法申请加班
        /// </summary>
        public const string IsHalfDayMorningWork = "上午是正常上班时间，无法申请加班！";
        /// <summary>
        /// 错误：下午是工作日，无法申请加班
        /// </summary>
        public const string IsHalfDayNoonWork = "下午是正常上班时间，无法申请加班！";
        /// <summary>
        /// 错误：您已经超龄，无法申请该假期
        /// </summary>
        public const string IsPastYouthDay = "您已经超龄，无法申请该假期！";
        /// <summary>
        /// 错误：请假申请必须在事后的一天内进行且不超过最大天数
        /// </summary>
        public const string IsPastOneDayAndPastMaxdays = "事后提单必须在结束日期后1工作日内提单且不超过{0}！";
        /// <summary>
        /// 错误：请假申请必须在事后的一天内进行
        /// </summary>
        public const string IsPastOneDay = "事后提单必须在结束日期后1工作日内提单！";
        /// <summary>
        /// 错误：该员工还未填写出生日期
        /// </summary>
        public const string NonBirthday = "该员工还未填写出生日期！";
        /// <summary>
        /// 错误：请假时长超过了系统允许的最大天数
        /// </summary>
        public const string IsPastMaxVacationDays = "请假时长超过了系统允许的最大限制:{0}！";
        /// <summary>
        /// 错误：该类型的假期只允许一次休完
        /// </summary>
        public const string IsOnceRestVacation = "该类型的假期只允许一次性休完！";
        /// <summary>
        /// 错误：该员工已经有外出申请单
        /// </summary>
        public const string HasOutApply = "该员工已经有外出申请单！";
        /// <summary>
        /// 错误：该员工已经有出差申请单
        /// </summary>
        public const string HasEvection = "该员工已经有出差申请单！";
        /// <summary>
        /// 错误：未找到对应的考勤方案
        /// </summary>
        public const string NonAttendenceSolution = "未找到对应的考勤方案！";
        /// <summary>
        /// 错误：未找到对应的请假记录
        /// </summary>
        public const string NonLeaveRecord = "未找到对应的请假记录！";
        /// <summary>
        /// 错误：该假期只允许男性员工申请
        /// </summary>
        public const string IsOnlyForMale = "该假期只允许男性员工申请！";
        /// <summary>
        /// 错误：该假期只允许女性员工申请
        /// </summary>
        public const string IsOnlyForFemale = "该假期只允许女性员工申请！";
        /// <summary>
        /// 错误：五四假期已经过期
        /// </summary>
        public const string YouthDayHasExpried = "五四假期已经过期！";
        /// <summary>
        /// 错误：三八妇女节假期已经过期
        /// </summary>
        public const string WomenDayHasExpried = "三八妇女节假期已经过期！";
        /// <summary>
        /// 错误：请假时间不能少于1小时
        /// </summary>
        public const string MinLeaveHours = "请假时间不能少于1小时！";
        /// <summary>
        /// 错误：没有足够的年假用于调休，请使用其他假期
        /// </summary>
        public const string NotEnoughAnnualDays = "没有足够的年假，请使用其他假期！";
        /// <summary>
        /// 错误：没有足够的调休假，请使用其他假期
        /// </summary>
        public const string NotEnoughAdjustDays = "没有足够的调休假，请使用其他假期！";

        /// <summary>
        /// 错误：没有足够的调休假，请使用其他假期
        /// </summary>
        public const string NotEnoughVacationDays = "您假期剩余不足,请重新填写或联系相关人员查看假期情况！";

        /// <summary>
        /// 错误：没有足够的调休假，请使用其他假期
        /// </summary>
        public const string AnnualVacationNotAllowCross = "年假不能跨年！";

        /// <summary>
        /// 错误：员工状态：未转正或已离职！
        /// </summary>
        public const string EmployeeNotNormal = "员工状态：未转正或已离职！";
    }
}
