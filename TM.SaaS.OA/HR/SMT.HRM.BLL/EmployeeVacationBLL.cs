using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.HRM.DAL;
using TM_SaaS_OA_EFModel;
using System.Linq.Dynamic;
using SMT.HRM.CustomModel;
using System.Data.Objects.DataClasses;
using SMT.HRM.BLL.Common;
using SMT.Foundation.Log;
using System.Threading;
using System.Linq.Expressions;
using SMT.SaaS.Common.Query;
using SMT.HRM.CustomModel.Request;
using SMT.HRM.CustomModel.Response;
using SMT.HRM.CustomModel.Common;

namespace SMT.HRM.BLL
{
    public class EmployeeVacationBLL : BaseBll<T_HR_EMPLOYEEVACATION>, ILookupEntity, IOperate
    {
        public List<T_HR_EMPLOYEEVACATION> QueryEmployeeVacation(int PageIndex, int PageSize, string Sort, string FilterString,
                                                                List<object> paras, string strOwnerID, out int PageCount)
        {
            try
            {
                IQueryable<T_HR_EMPLOYEEVACATION> vacationList = dal.GetObjects<T_HR_EMPLOYEEVACATION>();
                //过滤当前用户能查看的数据
                this.SetOrganizationFilter(ref FilterString, ref paras, strOwnerID, "T_HR_EMPLOYEEVACATION");
                //提交查询
                vacationList = vacationList.Where(FilterString, paras.ToArray());
                //结果排序
                vacationList = vacationList.OrderBy(Sort);
                int refPageCount = 0;
                //结果分页
                vacationList = Utility.Pager<T_HR_EMPLOYEEVACATION>(vacationList, PageIndex, PageSize, ref refPageCount);
                PageCount = refPageCount;
                return vacationList != null ? vacationList.ToList() : null;
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("获取假期列表数据出错,Function:QueryEmployeeVacation," + ex.ToString());
                PageCount = 0;
                return null;
            }
        }

        public List<T_HR_EMPLOYEEOVERTIMERECORD> QueryEmployeeOTRecord(int PageIndex, int PageSize, string Sort, string FilterString,
                                                                List<object> paras, string strOwnerID, out int PageCount)
        {
            try
            {
                IQueryable<T_HR_EMPLOYEEOVERTIMERECORD> otlist = dal.GetObjects<T_HR_EMPLOYEEOVERTIMERECORD>();
                //过滤当前用户能查看的数据
                this.SetOrganizationFilter(ref FilterString, ref paras, strOwnerID, "T_HR_EMPLOYEEOVERTIMERECORD");
                //提交查询
                otlist = otlist.Where(FilterString, paras.ToArray());
                //结果排序
                otlist = otlist.OrderBy(Sort);
                //结果分页
                int refPageCount = 0;
                //otlist = Utility.Pager<T_HR_EMPLOYEEOVERTIMERECORD>(otlist, PageIndex, PageSize, ref refPageCount);
                PageCount = refPageCount;
                return otlist != null ? otlist.ToList() : null;
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("获取加班列表数据出错,Function:QueryEmployeeOTRecord," + ex.ToString());
                PageCount = 0;
                return null;
            }
        }

        public List<T_HR_EMPLOYEELEAVERECORD> QueryEmployeeLeaveRecord(int PageIndex, int PageSize, string Sort, string FilterString,
                                                                List<object> paras, string strOwnerID, out int PageCount)
        {
            try
            {

                IQueryable<T_HR_EMPLOYEELEAVERECORD> list = dal.GetObjects<T_HR_EMPLOYEELEAVERECORD>().Include("T_HR_LEAVETYPESET");
                //过滤当前用户能查看的数据
                this.SetOrganizationFilter(ref FilterString, ref paras, strOwnerID, "T_HR_EMPLOYEELEAVERECORD");
                //提交查询
                list = list.Where(FilterString, paras.ToArray());
                //结果排序
                list = list.OrderBy(Sort);
                //结果分页
                int refPageCount = 0;



                list = Utility.Pager<T_HR_EMPLOYEELEAVERECORD>(list, PageIndex, PageSize, ref refPageCount);
                PageCount = refPageCount;
                return list.ToList();
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("获取请假申请列表数据出错,Function:QueryEmployeeLeaveRecord," + ex.ToString());
                PageCount = 0;
                return null;
            }
        }

        /// <summary>
        /// 检测申请加班的相关信息
        /// </summary>
        /// <param name="request">结果数据实体</param>
        /// <returns>返回结果数据实体</returns>
        public CalculateOTHoursResponse CalculateOTHours(CalculateOTHoursRequest request)
        {
            CalculateOTHoursResponse response = new CalculateOTHoursResponse();

            DateTime dtStartDate = Convert.ToDateTime(request.StartDate + " " + request.StartTime);
            DateTime dtEndDate = Convert.ToDateTime(request.EndDate + " " + request.EndTime);

            DateTime dtTempStartDate = dtStartDate;
            DateTime dtTempEndDate = dtEndDate;

            List<string> sWorkArr = new List<string>();//工作时间
            List<string> sNotWorkArr = new List<string>();//休息时间
            List<string> tempWorkArr = new List<string>();

            DateTime FirstCardStartDate = DateTime.MinValue;
            DateTime FirstCardEndDate = DateTime.MinValue;
            DateTime SecondCardStartDate = DateTime.MinValue;
            DateTime SecondCardEndDate = DateTime.MinValue;

            DateTime ThirdCardStartDate = DateTime.MinValue;
            DateTime ThirdCardEndDate = DateTime.MinValue;
            DateTime FourthCardStartDate = DateTime.MinValue;
            DateTime FourthCardEndDate = DateTime.MinValue;

            double totalHours = 0;
            int hasFirstSetting = 1;//0,未设置；1，已设置
            int hasSecondSetting = 1;//0,未设置；1，已设置
            int hasThirdSetting = 1;//0,未设置；1，已设置
            int hasFourthSetting = 1;//0,未设置；1，已设置

            response.Result = Enums.Result.Success.GetHashCode();
            response.Message = string.Empty;
            response.OTHours = 0;
            response.OTDays = 0;
            response.Month = string.Empty;
            response.AttendSolution = string.Empty;
            response.WorkPerDay = 0;
            response.StartTime = Convert.ToDateTime(request.StartTime).ToString("HH:mm");

            //判断加班申请是否重复  
            T_HR_EMPLOYEEOVERTIMERECORD otEntity = dal.GetObjects<T_HR_EMPLOYEEOVERTIMERECORD>().Where(t => t.OVERTIMERECORDID != request.OverTimeRecordID
                                                                    && t.EMPLOYEEID == request.EmployeeID
                                                                    && ((dtStartDate > t.STARTDATE && dtStartDate < t.ENDDATE)//起始时间在已有时间段内
                                                                     || (dtEndDate > t.STARTDATE && dtEndDate < t.ENDDATE)//结束时间在已有时间段内
                                                                     || (dtStartDate == t.STARTDATE && dtEndDate > t.STARTDATE)//当dtStartDate与开始时间重叠时，dtEndDate需大于开始时间
                                                                     || (dtEndDate == t.ENDDATE && dtStartDate < t.ENDDATE)//当dtEndDate与最后时间重叠时dtStartDate需小于束时间
                                                                     || (dtStartDate < t.STARTDATE && dtEndDate > t.ENDDATE)//新增加班完成包含已有加班
                                                                     )
                                                                     && t.CHECKSTATE != "3" && t.CHECKSTATE != "0"
                                                                    ).FirstOrDefault();

            if (otEntity != null)
            {
                response.Result = Enums.Result.HasDuplicateOTRecord.GetHashCode();//结果：失败
                response.Message = Constants.HasDuplicateOTRecord + ";"
                    + otEntity.STARTDATE.Value.ToString("yyyy-MM-dd") + " " + otEntity.STARTDATETIME + " ~ "
                    + otEntity.ENDDATE.Value.ToString("yyyy-MM-dd") + "  " + otEntity.ENDDATETIME;//加班申请重复
                return response;
            }

            #region "   获取考勤方案，排班明细，排班时间段   "

            AttendanceSolutionBLL bllAttendanceSolution = new AttendanceSolutionBLL();
            T_HR_ATTENDANCESOLUTION OTPeriodAttendSolution = bllAttendanceSolution.GetAttendanceSolutionByEmployeeIDAndDate(request.EmployeeID,
                                                                    Convert.ToDateTime(request.StartDate), Convert.ToDateTime(request.EndDate));

            SchedulingTemplateDetailBLL bllTemplateDetail = new SchedulingTemplateDetailBLL();
            IQueryable<T_HR_SCHEDULINGTEMPLATEDETAIL> scheduleSetDetail =
                bllTemplateDetail.GetTemplateDetailRdListByAttendanceSolutionId(OTPeriodAttendSolution.ATTENDANCESOLUTIONID);
            T_HR_SCHEDULINGTEMPLATEMASTER scheduleSetting = scheduleSetDetail.FirstOrDefault().T_HR_SCHEDULINGTEMPLATEMASTER;

            int iCycleDays = 0;
            DateTime dtCycleStartDate = Convert.ToDateTime(Convert.ToDateTime(request.StartDate).ToString("yyyy-MM-01"));//按月为周期的排班表
            DateTime dtCurCycleOTDate = Convert.ToDateTime(DateTime.Parse(request.StartDate).ToString("yyyy-MM-dd"));
            //找出加班时间与循环排班详细中对应的日历号，然后通过T_HR_SCHEDULINGTEMPLATEDETAIL找到对应的 打卡时间段： T_HR_SHIFTDEFINE
            if (scheduleSetting.SCHEDULINGCIRCLETYPE == (Common.SchedulingCircleType.Month.GetHashCode() + 1).ToString())
            {//按月循环的排班打卡方式
                iCycleDays = 31;
            }

            if ((scheduleSetting.SCHEDULINGCIRCLETYPE == (Common.SchedulingCircleType.Week.GetHashCode() + 1).ToString()))
            {//按周排班打卡方式
                iCycleDays = 7;
                //如果是按周统计，则从当前算起
                dtCycleStartDate = Convert.ToDateTime(DateTime.Parse(request.StartDate).ToString("yyyy-MM-dd"));
            }

            T_HR_SHIFTDEFINE dayCardSetting = null;//具体的排班明细，最多包括了4个时段的打卡设置,用于计算加班小时数
            for (int i = 0; i < iCycleDays; i++)//找出加班日期对应的日历中对应明细排班： T_HR_SHIFTDEFINE
            {
                string strSchedulingDate = (i + 1).ToString();
                DateTime dtCurDate = new DateTime();

                dtCurDate = dtCycleStartDate.AddDays(i);

                if (dtCurDate != dtCurCycleOTDate)
                {
                    continue;
                }

                T_HR_SCHEDULINGTEMPLATEDETAIL item = scheduleSetDetail.Where(c => c.SCHEDULINGDATE == strSchedulingDate).FirstOrDefault();
                if (item != null)
                {
                    dayCardSetting = item.T_HR_SHIFTDEFINE;//具体的排班明细
                }
            }

            #endregion

            string strMonth = Convert.ToDateTime(request.StartDate).ToString("yyyy年MM月");
            if (OTPeriodAttendSolution != null)
            {
                if (dayCardSetting != null)
                {
                    #region "   休息时间段的设置，以最大四个班次计算  "
                    DateTime notWorkTimeStart = DateTime.MinValue;
                    DateTime notWorkTimeEnd = DateTime.MinValue;

                    DateTime notWorkTimeStart1 = DateTime.MinValue;
                    DateTime notWorkTimeEnd1 = DateTime.MinValue;

                    DateTime notWorkTimeStart2 = DateTime.MinValue;
                    DateTime notWorkTimeEnd2 = DateTime.MinValue;
                    #endregion

                    #region "   第一时段打卡起始时间  "
                    if (!string.IsNullOrEmpty(dayCardSetting.FIRSTSTARTTIME) && !string.IsNullOrEmpty(dayCardSetting.FIRSTENDTIME))
                    {
                        FirstCardStartDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FIRSTSTARTTIME).ToString("HH:mm:ss"));
                        FirstCardEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FIRSTENDTIME).ToString("HH:mm:ss"));

                        notWorkTimeStart = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FIRSTENDTIME).ToString("HH:mm:ss"));

                        sWorkArr.Add(FirstCardStartDate.ToString() + "|" + FirstCardEndDate.ToString());

                        response.FirstStartTime = Convert.ToDateTime(dayCardSetting.FIRSTSTARTTIME).ToString("HH:mm:ss");
                        response.FirstEndTime = Convert.ToDateTime(dayCardSetting.FIRSTENDTIME).ToString("HH:mm:ss");
                    }
                    else
                    {
                        hasFirstSetting = 0;
                    }
                    #endregion
                    #region "   第二时段打卡起始时间  "
                    if (!string.IsNullOrEmpty(dayCardSetting.SECONDSTARTTIME) && !string.IsNullOrEmpty(dayCardSetting.SECONDENDTIME))
                    {
                        SecondCardStartDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.SECONDSTARTTIME).ToString("HH:mm:ss"));
                        SecondCardEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.SECONDENDTIME).ToString("HH:mm:ss"));

                        notWorkTimeEnd = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.SECONDSTARTTIME).ToString("HH:mm:ss"));
                        notWorkTimeStart1 = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.SECONDENDTIME).ToString("HH:mm:ss"));

                        sWorkArr.Add(SecondCardStartDate.ToString() + "|" + SecondCardEndDate.ToString());
                        sNotWorkArr.Add(notWorkTimeStart.ToString() + "|" + notWorkTimeEnd.ToString());

                        response.SecondStartTime = Convert.ToDateTime(dayCardSetting.SECONDSTARTTIME).ToString("HH:mm:ss");
                        response.SecondEndTime = Convert.ToDateTime(dayCardSetting.SECONDENDTIME).ToString("HH:mm:ss");
                    }
                    else
                    {
                        hasSecondSetting = 0;
                    }
                    #endregion
                    #region "   第三时段打卡起始时间  "
                    if (!string.IsNullOrEmpty(dayCardSetting.THIRDSTARTTIME) && !string.IsNullOrEmpty(dayCardSetting.THIRDENDTIME))
                    {
                        ThirdCardStartDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.THIRDSTARTTIME).ToString("HH:mm:ss"));
                        ThirdCardEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.THIRDENDTIME).ToString("HH:mm:ss"));

                        notWorkTimeEnd1 = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.THIRDSTARTTIME).ToString("HH:mm:ss"));
                        notWorkTimeStart2 = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.THIRDENDTIME).ToString("HH:mm:ss"));

                        sWorkArr.Add(ThirdCardStartDate.ToString() + "|" + ThirdCardEndDate.ToString());
                        sNotWorkArr.Add(notWorkTimeStart1.ToString() + "|" + notWorkTimeEnd1.ToString());

                        response.ThirdStartTime = string.IsNullOrEmpty(dayCardSetting.THIRDSTARTTIME) ? string.Empty : Convert.ToDateTime(dayCardSetting.THIRDSTARTTIME).ToString("HH:mm:ss");
                        response.ThirdEndTime = string.IsNullOrEmpty(dayCardSetting.THIRDENDTIME) ? string.Empty : Convert.ToDateTime(dayCardSetting.THIRDENDTIME).ToString("HH:mm:ss");
                    }
                    else
                    {
                        hasThirdSetting = 0;
                    }
                    #endregion
                    #region "   第四时段打卡起始时间  "
                    if (!string.IsNullOrEmpty(dayCardSetting.FOURTHSTARTTIME) && !string.IsNullOrEmpty(dayCardSetting.FOURTHENDTIME))
                    {
                        FourthCardStartDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FOURTHSTARTTIME).ToString("HH:mm:ss"));
                        FourthCardEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FOURTHENDTIME).ToString("HH:mm:ss"));

                        notWorkTimeEnd2 = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FOURTHSTARTTIME).ToString("HH:mm:ss"));

                        sWorkArr.Add(FourthCardStartDate.ToString() + "|" + FourthCardEndDate.ToString());
                        sNotWorkArr.Add(notWorkTimeStart2.ToString() + "|" + notWorkTimeEnd2.ToString());

                        response.FourthStartTime = string.IsNullOrEmpty(dayCardSetting.FOURTHSTARTTIME) ? string.Empty : Convert.ToDateTime(dayCardSetting.FOURTHSTARTTIME).ToString("HH:mm:ss");
                        response.FourthEndTime = string.IsNullOrEmpty(dayCardSetting.FOURTHENDTIME) ? string.Empty : Convert.ToDateTime(dayCardSetting.FOURTHENDTIME).ToString("HH:mm:ss");
                    }
                    else
                    {
                        hasFourthSetting = 0;
                    }
                    #endregion

                    #region "   判断是否设置了至少两个的打卡时间段   "
                    //为设置打卡时间段,至少设置两个打卡时间段
                    if (hasFirstSetting == 0)
                    {
                        response.Result = Enums.Result.NonFirstSetting.GetHashCode();
                        response.Month = strMonth;
                        response.AttendSolution = OTPeriodAttendSolution.ATTENDANCESOLUTIONNAME;
                        response.WorkPerDay = OTPeriodAttendSolution.WORKTIMEPERDAY.HasValue ? OTPeriodAttendSolution.WORKTIMEPERDAY.Value : 0;
                        response.Message = Constants.NonFirstSetting;
                        return response;
                    }

                    if (hasSecondSetting == 0)
                    {
                        response.Result = Enums.Result.NonSecondSetting.GetHashCode();
                        response.Month = strMonth;
                        response.AttendSolution = OTPeriodAttendSolution.ATTENDANCESOLUTIONNAME;
                        response.WorkPerDay = OTPeriodAttendSolution.WORKTIMEPERDAY.HasValue ? OTPeriodAttendSolution.WORKTIMEPERDAY.Value : 0;
                        response.Message = Constants.NonSecondSetting;
                        return response;
                    }
                    #endregion

                    #region "   检查加班时间是否在公共假期，或是工作日，或是三八，或是五四     "

                    decimal dWorkMode = OTPeriodAttendSolution.WORKMODE.Value;
                    int iWorkMode = 0;
                    int.TryParse(dWorkMode.ToString(), out iWorkMode);//获取工作制(工作天数/周)

                    List<int> iWorkDays = new List<int>();
                    SMT.HRM.BLL.Utility.GetWorkDays(iWorkMode, ref iWorkDays);

                    OutPlanDaysBLL bllOutPlanDays = new OutPlanDaysBLL();
                    IQueryable<T_HR_OUTPLANDAYS> entOutPlanDays = bllOutPlanDays.GetOutPlanDaysRdListByEmployeeID(request.EmployeeID);
                    string strVacDayType = (Convert.ToInt32(SMT.HRM.BLL.Common.OutPlanDaysType.Vacation) + 1).ToString();
                    string strWorkDayType = (Convert.ToInt32(SMT.HRM.BLL.Common.OutPlanDaysType.WorkDay) + 1).ToString();
                    //获取公共假期设置  
                    DateTime vacTempStartDate = Convert.ToDateTime(request.StartDate);
                    DateTime vacTempEndDate = Convert.ToDateTime(request.EndDate);
                    //加班只可能在一天内
                    IQueryable<T_HR_OUTPLANDAYS> entVacDays = entOutPlanDays.Where(s => s.DAYTYPE == strVacDayType
                            && vacTempStartDate >= s.STARTDATE && vacTempEndDate <= s.ENDDATE);

                    //获取工作日设置
                    DateTime workTempStartDate = Convert.ToDateTime(request.StartDate);
                    DateTime workTempEndDate = Convert.ToDateTime(request.EndDate);
                    //加班只可能在一天内
                    IQueryable<T_HR_OUTPLANDAYS> entWorkDays = entOutPlanDays.Where(s => s.DAYTYPE == strWorkDayType
                            && workTempStartDate >= s.STARTDATE && workTempEndDate <= s.ENDDATE);
                    //当前星期几,是否要工作
                    //Sunday = 0, Monday = 1, Tuesday = 2, Wednesday = 3, Thursday = 4, Friday = 5, Saturday = 6.
                    int iDayOfWeek = Convert.ToDateTime(request.StartDate).DayOfWeek.GetHashCode();
                    bool iDayCount = iWorkDays.Contains(iDayOfWeek);

                    DateTime WorkDayEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.SECONDENDTIME).ToString("HH:mm:ss"));
                    //工作日加班，要在第二时间段以后
                    if (dayCardSetting != null)
                    {
                        if (hasThirdSetting == 1)
                        {
                            WorkDayEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.THIRDENDTIME).ToString("HH:mm:ss"));
                        }
                        if (hasFourthSetting == 1)
                        {
                            WorkDayEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FOURTHENDTIME).ToString("HH:mm:ss"));
                        }
                    }

                    #region "   不是假期    "
                    if (entVacDays.Count() == 0)
                    {   //也不是设置的工作日，
                        if (entWorkDays.Count() == 0)
                        {   //并且在上班时间列表中
                            if (iDayCount)
                            {
                                //工作日加班，要在第二时间段以后
                                if (dayCardSetting != null)
                                {
                                    #region 梁杰文 平时加班，加班起始时间要在工作日结束之后
                                    //改判断逻辑dtEndDate <= WorkDayEndDate为WorkDayEndDate>dtStartDate
                                    #endregion
                                    if (WorkDayEndDate > dtStartDate)
                                    {
                                        #region "   在上班时间列表中,这天要上班，不算加班  "
                                        //这天要上班，不算加班
                                        response.Result = Enums.Result.IsWorkDay.GetHashCode();
                                        response.Message = Constants.IsWorkDay;//是工作日，不算加班
                                        return response;
                                        #endregion
                                    }
                                }
                            }
                        }
                        else
                        {
                            //工作日加班，要在第二时间段以后
                            if (dayCardSetting != null)
                            {
                                #region 梁杰文 平时加班，加班起始时间要在工作日结束之后
                                //改判断逻辑dtEndDate <= WorkDayEndDate为WorkDayEndDate>dtStartDate
                                #endregion
                                if (WorkDayEndDate > dtStartDate)
                                {
                                    #region "   设置为工作日，这天要上班，不算加班   "
                                    //这天要上班，不算加班
                                    response.Result = Enums.Result.IsWorkDay.GetHashCode();
                                    response.Message = Constants.IsWorkDay;//是工作日，不算加班
                                    return response;
                                    #endregion
                                }
                            }
                        }
                    }
                    else
                    {
                        //加班时间在假期设置中，但只是半天的设置
                        foreach (var vac in entVacDays)
                        {
                            #region "   半天公共假期，三八妇女节，青年节    "
                            if (vac.ISHALFDAY == "1")
                            {
                                if (vac.PEROID == "1")//下午
                                {
                                    if (dayCardSetting != null)
                                    {
                                        DateTime HalfMorningStartDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FIRSTSTARTTIME).ToString("HH:mm:ss"));
                                        DateTime HalfMorningEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FIRSTENDTIME).ToString("HH:mm:ss"));
                                        //四个时间中，上午时间的开始与结束
                                        if (hasThirdSetting == 1 && hasFourthSetting == 1)
                                        {
                                            HalfMorningEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.SECONDENDTIME).ToString("HH:mm:ss"));
                                        }
                                        //如果填写的加班时间段在上午，则不算加班
                                        if (dtEndDate <= HalfMorningEndDate)
                                        {
                                            response.Result = Enums.Result.IsHalfDayMorningWork.GetHashCode();
                                            response.Month = strMonth;
                                            response.AttendSolution = OTPeriodAttendSolution.ATTENDANCESOLUTIONNAME;
                                            response.WorkPerDay = OTPeriodAttendSolution.WORKTIMEPERDAY.HasValue ? OTPeriodAttendSolution.WORKTIMEPERDAY.Value : 0;
                                            response.Message = Constants.IsHalfDayMorningWork;
                                            return response;
                                        }
                                    }
                                }
                                else//上午
                                {
                                    if (dayCardSetting != null)
                                    {
                                        DateTime HalfNoonStartDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.SECONDSTARTTIME).ToString("HH:mm:ss"));
                                        DateTime HalfNoonEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.SECONDENDTIME).ToString("HH:mm:ss"));
                                        //四个时间中，下午时间的开始与结束
                                        if (hasThirdSetting == 1 && hasFourthSetting == 1)
                                        {
                                            HalfNoonStartDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.THIRDSTARTTIME).ToString("HH:mm:ss"));
                                            HalfNoonEndDate = Convert.ToDateTime(request.StartDate + " " + Convert.ToDateTime(dayCardSetting.FOURTHENDTIME).ToString("HH:mm:ss"));
                                        }
                                        //如果填写的加班时间段在下午，则不算加班
                                        if (dtStartDate >= HalfNoonStartDate && dtStartDate <= HalfNoonEndDate)
                                        {
                                            response.Result = Enums.Result.IsHalfDayNoonWork.GetHashCode();
                                            response.Month = strMonth;
                                            response.AttendSolution = OTPeriodAttendSolution.ATTENDANCESOLUTIONNAME;
                                            response.WorkPerDay = OTPeriodAttendSolution.WORKTIMEPERDAY.HasValue ? OTPeriodAttendSolution.WORKTIMEPERDAY.Value : 0;
                                            response.Message = Constants.IsHalfDayNoonWork;
                                            return response;
                                        }
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                    #endregion

                    //填写的加班时段都在4个设置的上班时间段里面，则直接用结束时间减去开始时间
                    if (
                          (dtStartDate >= FirstCardStartDate && dtStartDate <= FirstCardEndDate
                          && dtEndDate >= FirstCardStartDate && dtEndDate <= FirstCardEndDate)
                          ||
                          (dtStartDate >= SecondCardStartDate && dtStartDate <= SecondCardEndDate
                          && dtEndDate >= SecondCardStartDate && dtEndDate <= SecondCardEndDate)
                          ||
                          (dtStartDate >= ThirdCardStartDate && dtStartDate <= ThirdCardEndDate
                          && dtEndDate >= ThirdCardStartDate && dtEndDate <= ThirdCardEndDate)
                          ||
                          (dtStartDate >= FourthCardStartDate && dtStartDate <= FourthCardEndDate
                          && dtEndDate >= FourthCardStartDate && dtEndDate <= FourthCardEndDate)
                        )
                    {
                        totalHours = dtEndDate.Subtract(dtStartDate).TotalHours;
                    }
                    else
                    {
                        #region "   计算加班时间     "

                        //早7：50打开，将计算加班的有效开始时间设置为8：30，也就是dtCardStartDate
                        //23:00下班，假定四个工作时间段
                        if (dtStartDate < FirstCardStartDate)
                        {
                            dtTempStartDate = FirstCardStartDate;
                            response.StartTime = dtTempStartDate.ToString("HH:mm:ss");
                        }

                        #region "   找出开始计算加班的时间点    "
                        DateTime tempOTDate = new DateTime();
                        foreach (string str in sWorkArr)
                        {
                            string[] s = str.Split('|');
                            DateTime WorkStartDate = Convert.ToDateTime(s[0]);
                            DateTime WorkEndDate = Convert.ToDateTime(s[1]);
                            //如果开始时间在工作时间范围内，那就从开始时间算加班
                            if (dtTempStartDate >= WorkStartDate && dtTempStartDate <= WorkEndDate)
                            {
                                tempOTDate = dtTempStartDate;
                            }
                            //如果开始时间大于工作时间段的结束时间，则属于休息时间段的时间点
                            //找出里他最近的上班时间点作为加班开始时间
                            if (dtTempStartDate > WorkEndDate)
                            {
                                foreach (string str1 in sNotWorkArr)
                                {
                                    string[] sn = str1.Split('|');
                                    DateTime notWorkStartDate = Convert.ToDateTime(sn[0]);
                                    DateTime notWorkEndDate = Convert.ToDateTime(sn[1]);
                                    //加班开始时间在休息时间点内，则加班的开始时间从休息时间的结束点开始
                                    if (dtTempStartDate >= notWorkStartDate && dtTempStartDate <= notWorkEndDate)
                                    {
                                        tempOTDate = notWorkEndDate;
                                    }
                                    else
                                    {
                                        //不在所有的休息时间段内，则说明加班是在
                                        //一天正常的上班时间段以外进行
                                        tempOTDate = dtTempStartDate;
                                    }
                                }
                            }
                        }
                        #endregion

                        #region "   设置具体的加班是时间段     "
                        for (int i = 0; i < sWorkArr.Count(); i++)
                        {
                            string[] ss = sWorkArr[i].Split('|');
                            DateTime WorkStartDate = Convert.ToDateTime(ss[0]);
                            DateTime WorkEndDate = Convert.ToDateTime(ss[1]);
                            //工作时间段的开始时间大于加班开始时间
                            if (WorkStartDate >= tempOTDate)
                            {
                                //加班结束时间大于工作时间段的结束时间
                                //则说明加班时间包含这段工作时间段，计算完整的加班时间段
                                if (dtTempEndDate >= WorkEndDate)
                                {
                                    if (sNotWorkArr.Count() == 0)
                                    {
                                        tempWorkArr.Add(WorkStartDate.ToString() + "|" + dtTempEndDate.ToString());
                                    }

                                    foreach (string str3 in sNotWorkArr)
                                    {
                                        string[] sn = str3.Split('|');
                                        DateTime notWorkStartDate = Convert.ToDateTime(sn[0]);
                                        DateTime notWorkEndDate = Convert.ToDateTime(sn[1]);
                                        if (dtTempEndDate >= notWorkStartDate && dtTempEndDate <= notWorkEndDate)
                                        {
                                            tempWorkArr.Add(WorkStartDate.ToString() + "|" + WorkEndDate.ToString());
                                        }
                                        else if (dtTempEndDate >= WorkEndDate)
                                        {
                                            if (i == sWorkArr.Count() - 1)
                                            {
                                                WorkEndDate = dtTempEndDate;
                                            }
                                            tempWorkArr.Add(WorkStartDate.ToString() + "|" + WorkEndDate.ToString());
                                            sNotWorkArr.Remove(str3);
                                            break;
                                        }
                                        else
                                        {
                                            tempWorkArr.Add(WorkStartDate.ToString() + "|" + notWorkStartDate.ToString());
                                            sNotWorkArr.Remove(str3);
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (sNotWorkArr.Count() == 0)
                                    {
                                        tempWorkArr.Add(WorkStartDate.ToString() + "|" + dtTempEndDate.ToString());
                                    }

                                    foreach (string str3 in sNotWorkArr)
                                    {
                                        string[] sn = str3.Split('|');
                                        DateTime notWorkStartDate = Convert.ToDateTime(sn[0]);
                                        DateTime notWorkEndDate = Convert.ToDateTime(sn[1]);
                                        if (dtTempEndDate >= notWorkStartDate && dtTempEndDate <= notWorkEndDate)
                                        {

                                        }
                                        else if (dtTempEndDate >= WorkEndDate)
                                        {
                                            tempWorkArr.Add(WorkStartDate.ToString() + "|" + WorkEndDate.ToString());
                                            sNotWorkArr.Remove(str3);
                                            break;
                                        }
                                        else
                                        {
                                            tempWorkArr.Add(WorkStartDate.ToString() + "|" + dtTempEndDate.ToString());
                                            sNotWorkArr.Remove(str3);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (i == sWorkArr.Count() - 1)
                                {
                                    WorkEndDate = dtTempEndDate;
                                }
                                tempWorkArr.Add(tempOTDate.ToString() + "|" + WorkEndDate.ToString());
                            }
                        }
                        #endregion

                        #region "   计算加班时长  "
                        tempWorkArr = tempWorkArr.Distinct().ToList();
                        foreach (string str4 in tempWorkArr)
                        {
                            string[] sss = str4.Split('|');
                            DateTime WorkStartDate = Convert.ToDateTime(sss[0]);
                            DateTime WorkEndDate = Convert.ToDateTime(sss[1]);
                            if (WorkStartDate <= WorkEndDate)
                            {
                                totalHours += Math.Round(WorkEndDate.Subtract(WorkStartDate).TotalHours, 1);
                            }
                        }
                        #endregion

                        #endregion
                    }
                }


                response.Result = Enums.Result.Success.GetHashCode();
                response.OTHours = totalHours;
                response.OTDays = Math.Round(totalHours / Convert.ToDouble(OTPeriodAttendSolution.WORKTIMEPERDAY), 2);
                response.Month = strMonth;
                response.AttendSolution = OTPeriodAttendSolution.ATTENDANCESOLUTIONNAME;
                response.WorkPerDay = OTPeriodAttendSolution.WORKTIMEPERDAY.HasValue ? OTPeriodAttendSolution.WORKTIMEPERDAY.Value : 0;
                response.Message = "";
                return response;
            }


            response.Result = Enums.Result.NonAttendenceSolution.GetHashCode();
            response.Message = Constants.NonAttendenceSolution;
            response.OTHours = 0;
            response.OTDays = 0;
            response.Month = strMonth;
            response.AttendSolution = string.Format("未找到{0}对应的考勤方案", strMonth);
            response.WorkPerDay = 0;
            return response;
        }

        /// <summary>
        /// 更新假期天数及小时数
        /// 如果是加班就带整数；
        /// 如果是请调休假就带负数；
        /// </summary>
        /// <param name="YearPeriod"></param>
        /// <param name="EmployeeID"></param>
        /// <param name="VacationType"></param>
        /// <param name="TotalHours"></param>
        /// <param name="TotalDays"></param>
        /// <returns></returns>
        public int UpdateEmployeeVacation(EmployeeVacationUpdateEntity data)
        {
            T_HR_EMPLOYEEVACATION entity = new T_HR_EMPLOYEEVACATION();
            entity = dal.GetObjects<T_HR_EMPLOYEEVACATION>().Where(t => t.YEAR_PERIOD == data.YearPeriod && t.EMPLOYEEID == data.EmployeeID).FirstOrDefault();
            int effect = 0;
            if (entity != null)
            {
                switch (data.VacationType)
                {
                    case (int)Enums.PublicVacationType.OverTime:
                        entity.OT_TOTAL_HOURS += data.TotalHours;
                        entity.UPDATE_DATE = DateTime.Now;
                        entity.UPDATE_USERID = data.UpdateUserID;
                        break;
                }
                effect = dal.Update(entity);
            }

            return effect;
        }

        public int UpdateCheckState(string strEntityName, string EntityKeyName, string EntityKeyValue, string CheckState)
        {
            throw new NotImplementedException();
        }

        public EntityObject[] GetLookupData(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount, string userID)
        {
            throw new NotImplementedException();
        }
    }
}
