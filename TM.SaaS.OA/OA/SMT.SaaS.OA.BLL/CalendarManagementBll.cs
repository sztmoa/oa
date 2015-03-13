using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.SaaS.OA.DAL;
using TM_SaaS_OA_EFModel;
using System.Linq.Dynamic;
using SMT.Foundation.Log;
using SMT.SaaS.BLLCommonServices.PersonnelWS;
namespace SMT.SaaS.OA.BLL
{
    public class CalendarManagementBll : BaseBll<T_OA_CALENDAR>
    {
        /// <summary>
        /// 获取日程安排管理信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="filterString"></param>
        /// <param name="paras"></param>
        /// <param name="pageCount"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IQueryable<T_OA_CALENDAR> GetCalendarListByUserID(int pageIndex, int pageSize, string sort, string filterString, object[] paras, ref int pageCount, string userID)
        {
            try
            {
                var q = from ent in dal.GetTable()

                        select ent;
                List<object> queryParas = new List<object>();
                if (paras != null)
                {
                    queryParas.AddRange(paras);
                }
                UtilityClass.SetOrganizationFilter(ref filterString, ref queryParas, userID, "T_OA_CALENDAR");
                if (!string.IsNullOrEmpty(filterString))
                {
                    q = q.ToList().AsQueryable().Where(filterString, queryParas.ToArray());
                }
                q = q.OrderBy(sort);
                q = Utility.Pager<T_OA_CALENDAR>(q, pageIndex, pageSize, ref pageCount);
                if (q.Count() > 0)
                {
                    return q;
                }
                
            }
            catch (Exception ex)
            {
                Tracer.Debug("日程管理CalendarManagementBll-GetCalendarListByUserID,userID:" + userID + " " + ex.ToString());
                
            }
            return null;
        }
        /// <summary>
        /// 通过权限过滤日程管理
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="sort"></param>
        /// <param name="filterString"></param>
        /// <param name="paras"></param>
        /// <param name="pageCount"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IQueryable<T_OA_CALENDAR> GetCalendarList(int pageIndex, int pageSize, string sort, string filterString, object[] paras, ref int pageCount, string userID)
        {
            try
            {
                var q = from ent in dal.GetObjects<T_OA_CALENDAR>()
                        select ent;
                
                List<object> queryParas = new List<object>();
                if (paras != null)
                {
                    queryParas.AddRange(paras);
                }
                string bb = filterString;
                
                if (!(filterString.IndexOf("OWNERID") > -1))
                {
                    UtilityClass.SetOrganizationFilter(ref filterString, ref queryParas, userID, "T_OA_CALENDAR");
                }

                

                if (!string.IsNullOrEmpty(filterString))
                {
                    q = q.Where(filterString, queryParas.ToArray());
                }
                q = q.OrderBy(sort);

                q = Utility.Pager<T_OA_CALENDAR>(q, pageIndex, pageSize, ref pageCount);

                if (q.Count() > 0)
                {
                    return q;
                }
                
            }
            catch (Exception ex)
            {
                Tracer.Debug("日程管理CalendarManagementBll-GetCalendarList" + System.DateTime.Now.ToString() + " " + ex.ToString());
            }

            return null;
        }
        /// <summary>
        /// 添加日程管理信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddCalendarInfo(T_OA_CALENDAR entity)
        {
            try
            {
                int i = dal.Add(entity);
                if (i == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("日程管理CalendarManagementBll-AddCalendarInfo" + System.DateTime.Now.ToString() + " " + ex.ToString());
                throw (ex);
            }
        }
        /// <summary>
        /// 删除日程管理信息
        /// </summary>
        /// <param name="calendarGuid">日程管理ID</param>
        /// <returns></returns>
        public bool DeleteCalendarInfo(string calendarGuid)
        {
            try
            {
                var entitys = (from ent in dal.GetTable()
                               where ent.CALENDARID == calendarGuid
                               select ent);
                if (entitys.Count() > 0)
                {
                    var entity = entitys.FirstOrDefault();
                    dal.Delete(entity);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Tracer.Debug("日程管理CalendarManagementBll-DeleteCalendarInfo" + System.DateTime.Now.ToString() + " " + ex.ToString());
                return false;
                
            }
        }
        /// <summary>
        /// 修改日程管理信息
        /// </summary>
        /// <param name="entity">日程管理实体</param>
        /// <returns></returns>
        public int UpdateCalendarInfo(T_OA_CALENDAR entity)
        {
            try
            {
                var users = from ent in dal.GetTable()
                            where ent.CALENDARID == entity.CALENDARID
                            select ent;
                if (users.Count() > 0)
                {
                    var user = users.FirstOrDefault();
                    user.TITLE = entity.TITLE;
                    user.CONTENT = entity.CONTENT;
                    user.REMINDERRMODEL = entity.REMINDERRMODEL;
                    user.REPARTREMINDER = entity.REPARTREMINDER;
                    user.UPDATEDATE = entity.UPDATEDATE;
                    user.UPDATEUSERID = entity.UPDATEUSERID;
                    user.PLANTIME = entity.PLANTIME;
                    return dal.Update(user);
                }
                return -1;
            }
            catch (Exception ex)
            {
                Tracer.Debug("日程管理CalendarManagementBll-UpdateCalendarInfo" + System.DateTime.Now.ToString() + " " + ex.ToString());
                return -1;
                
            }
        }

        /// <summary>
        /// 根据日程安排信息获取日程安排信息记录
        /// </summary>
        /// <param name="calendarID">日程安排信息ID</param>
        /// <returns>返回日程安排实体信息</returns>
        public T_OA_CALENDAR GetCalendarInfo(string calendarID)
        {
            Tracer.Debug("获取日程安排的calendarID：" + calendarID);
            var ents = from ent in dal.GetObjects<T_OA_CALENDAR>()
                       where ent.CALENDARID == calendarID
                       select ent;
            if (ents.Count() == 0)
            {
                return null;
            }
            else
            {
                return ents.FirstOrDefault();
            }
        }

        #region 手机版
        /// <summary>
        /// 添加日程安排信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="reminderrmodel">计划模式</param>
        /// <param name="reparteminder">提醒周期</param>
        /// <param name="employeeID">员工ID</param>
        /// <param name="ts">时分</param>        
        /// <returns>返回字符串成功为空</returns>
        public string AddCalendarInfoForMobile(string title, string content, string reminderrmodel, string reparteminder, string employeeID, string strHourMins, ref string calenderID)
        {
            string strReturn = string.Empty;
            try
            {
                Tracer.Debug("开始添加日程计划信息");
                Tracer.Debug("title：" + title);
                Tracer.Debug("content：" + content);
                Tracer.Debug("reminderrmodel：" + reminderrmodel);
                Tracer.Debug("reparteminder：" + reparteminder);
                Tracer.Debug("employeeID： " + employeeID);
                Tracer.Debug("strHourMins： " + strHourMins);
                strReturn = checkCalendarInfo(title,content, reminderrmodel,reparteminder,employeeID,strHourMins);
                if (!string.IsNullOrEmpty(strReturn))
                {
                    return strReturn;
                }
                TimeSpan ts = new TimeSpan();
                try
                {
                    ts = TimeSpan.Parse(strHourMins);
                }
                catch (Exception ex)
                {
                    strReturn = "计划时间中时分错误";
                    return strReturn;
                }
                V_EMPLOYEEVIEW employe = getEmployee(employeeID);
                if (employe == null)
                {
                    strReturn = "没有获取到员工信息";
                    return strReturn;
                }
                if (string.IsNullOrEmpty(employe.EMPLOYEEID))
                {
                    strReturn = "没有获取到员工信息";
                    return strReturn;
                }
                T_OA_CALENDAR calendarInfo = new T_OA_CALENDAR();
                calendarInfo.CALENDARID = Guid.NewGuid().ToString();
                calendarInfo.CREATEUSERID = employeeID;
                calendarInfo.CREATEDATE = DateTime.Now;
                calendarInfo.UPDATEUSERID = employeeID;
                calendarInfo.UPDATEDATE = DateTime.Now;
                
                calendarInfo.REPARTREMINDER = reparteminder;
                calendarInfo.REMINDERRMODEL = reminderrmodel;
                calendarInfo.TITLE = title;
                calendarInfo.CONTENT = content;
                V_EMPLOYEEVIEW employee = getEmployee(employeeID);
                strReturn =UpdatePlanTime(ref calendarInfo, "Add", employe,ts);
                if (string.IsNullOrEmpty(strReturn))
                {
                    bool blResult =AddCalendarInfo(calendarInfo);
                    if (!blResult)
                    {
                        strReturn = "添加日程安排失败";
                        return strReturn;
                    }
                    calenderID = calendarInfo.CALENDARID;
                }
                else
                {
                    return strReturn;
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("title：" + title);
                Tracer.Debug("content：" + content);
                Tracer.Debug("reminderrmodel：" + reminderrmodel);
                Tracer.Debug("reparteminder：" + reparteminder);
                Tracer.Debug("employeeID： " + employeeID);
                Tracer.Debug("strHourMins： " + strHourMins);                
                SMT.Foundation.Log.Tracer.Debug("CalendarManagementBll-AddCalendarInfoForMobile出现错误，员工ID:" + employeeID + ".错误信息：" + ex.ToString());
                strReturn = "添加日程安排时出现异常";
            }
            return strReturn;
        }

        /// <summary>
        /// 修改日程安排信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="reminderrmodel">计划模式</param>
        /// <param name="reparteminder">提醒周期</param>
        /// <param name="employeeID">员工ID</param>
        /// <param name="ts">时分</param>
        /// <param name="calenderID">日程安排ID</param>
        /// <returns>返回字符串成功为空</returns>
        public string UpdateCalendarInfoForMobile(string title, string content, string reminderrmodel, string reparteminder, string employeeID, string strHourMins, string calenderID)
        {
            string strReturn = string.Empty;
            try
            {
                Tracer.Debug("开始修改日程计划信息");
                Tracer.Debug("title：" + title);
                Tracer.Debug("content：" + content);
                Tracer.Debug("reminderrmodel：" + reminderrmodel);
                Tracer.Debug("reparteminder：" + reparteminder);
                Tracer.Debug("employeeID： " + employeeID);
                Tracer.Debug("strHourMins： " + strHourMins);
                Tracer.Debug("calenderID： " + calenderID);
                strReturn = checkCalendarInfo(title, content, reminderrmodel, reparteminder, employeeID, strHourMins);
                if (!string.IsNullOrEmpty(strReturn))
                {
                    return strReturn;
                }
                TimeSpan ts = new TimeSpan();
                try
                {
                    ts = TimeSpan.Parse(strHourMins);
                }
                catch (Exception ex)
                {
                    strReturn = "计划时间中时分错误";
                    return strReturn;
                }
                V_EMPLOYEEVIEW employe = getEmployee(employeeID);
                if (employe == null)
                {
                    strReturn = "没有获取到员工信息";
                    return strReturn;
                }
                if (string.IsNullOrEmpty(employe.EMPLOYEEID))
                {
                    strReturn = "没有获取到员工信息";
                    return strReturn;
                }
                T_OA_CALENDAR calendarInfo = new T_OA_CALENDAR();
                var ents = from ent in dal.GetObjects<T_OA_CALENDAR>()
                           where ent.CALENDARID == calenderID
                           select ent;
                if (ents.Count() == 0)
                {
                    strReturn = "修改的日程安排信息不存在";
                    return strReturn;
                }
                calendarInfo = ents.FirstOrDefault();
                
                calendarInfo.REPARTREMINDER = reparteminder;
                calendarInfo.REMINDERRMODEL = reminderrmodel;
                calendarInfo.TITLE = title;
                calendarInfo.CONTENT = content;
                V_EMPLOYEEVIEW employee = getEmployee(employeeID);
                strReturn = UpdatePlanTime(ref calendarInfo, "Edit", employe,ts);
                if (string.IsNullOrEmpty(strReturn))
                {
                    int intResult = UpdateCalendarInfo(calendarInfo);
                    if (intResult < 1)
                    {
                        strReturn = "修改日程安排失败";
                        return strReturn;
                    }
                }
                else
                {
                    return strReturn;
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("title：" + title);
                Tracer.Debug("content：" + content);
                Tracer.Debug("reminderrmodel：" + reminderrmodel);
                Tracer.Debug("reparteminder：" + reparteminder);
                Tracer.Debug("employeeID： " + employeeID);
                Tracer.Debug("strHourMins： " + strHourMins);
                Tracer.Debug("calenderID： " + calenderID);
                SMT.Foundation.Log.Tracer.Debug("CalendarManagementBll-UpdateCalendarInfoForMobile出现错误，员工ID:" + employeeID + ".错误信息：" + ex.ToString());
                strReturn = "修改日程安排出现异常";
            }
            return strReturn;
        }

        /// <summary>
        /// 重新复制T_OA_CALENDAR实体信息
        /// </summary>
        /// <param name="calendarInfo"></param>
        /// <param name="operationFlag"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        private string UpdatePlanTime(ref T_OA_CALENDAR calendarInfo, string operationFlag, V_EMPLOYEEVIEW employee,TimeSpan ts)
        {
            string strReturn = string.Empty;
            DateTime dtNow = DateTime.Now;
            DateTime planTime;
            string reminderrmodel = calendarInfo.REMINDERRMODEL;
            switch (calendarInfo.REPARTREMINDER)
            {
                case "NOTHING":
                    planTime = Convert.ToDateTime(reminderrmodel);
                    calendarInfo.PLANTIME = planTime;
                    break;
                case "DAY"://每天                        
                    planTime = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, ts.Hours, ts.Minutes, 0);
                    calendarInfo.PLANTIME = planTime;
                    break;
                case "WEEK"://每周
                    switch (reminderrmodel)
                    {
                        case "Monday":
                            calendarInfo.REMINDERRMODEL = DayOfWeek.Monday.ToString();
                            if (dtNow.DayOfWeek > DayOfWeek.Monday)
                            {
                                dtNow = dtNow.AddDays(7.0 - System.Convert.ToDouble(dtNow.DayOfWeek) + 1);
                            }
                            else
                            {
                                dtNow = dtNow.AddDays(DayOfWeek.Monday - dtNow.DayOfWeek);
                            }
                            break;
                        case "Tuesday":
                            if (dtNow.DayOfWeek > DayOfWeek.Thursday)
                            {
                                dtNow = dtNow.AddDays(7.0 - System.Convert.ToDouble(dtNow.DayOfWeek) + 2);
                            }
                            else
                            {
                                dtNow = dtNow.AddDays(DayOfWeek.Thursday - dtNow.DayOfWeek);
                            }

                            break;
                        case "Wednesday":
                            if (dtNow.DayOfWeek > DayOfWeek.Wednesday)
                            {
                                dtNow = dtNow.AddDays(7.0 - System.Convert.ToDouble(dtNow.DayOfWeek) + 3);
                            }
                            else
                            {
                                dtNow = dtNow.AddDays(DayOfWeek.Wednesday - dtNow.DayOfWeek);
                            }
                            break;
                        case "Thursday":
                            if (dtNow.DayOfWeek > DayOfWeek.Thursday)
                            {
                                dtNow = dtNow.AddDays(7.0 - System.Convert.ToDouble(dtNow.DayOfWeek) + 4);
                            }
                            else
                            {
                                dtNow = dtNow.AddDays(DayOfWeek.Thursday - dtNow.DayOfWeek);
                            }
                            break;
                        case "Friday":
                            if (dtNow.DayOfWeek > DayOfWeek.Friday)
                            {
                                dtNow = dtNow.AddDays(7.0 - System.Convert.ToDouble(dtNow.DayOfWeek) + 1);
                            }
                            else
                            {
                                dtNow = dtNow.AddDays(DayOfWeek.Friday - dtNow.DayOfWeek);
                            }
                            break;
                        case "Saturday":
                            if (dtNow.DayOfWeek > DayOfWeek.Saturday)
                            {
                                dtNow = dtNow.AddDays(7.0 - System.Convert.ToDouble(dtNow.DayOfWeek) + 1);
                            }
                            else
                            {
                                dtNow = dtNow.AddDays(DayOfWeek.Saturday - dtNow.DayOfWeek);
                            }
                            break;
                        case "Sunday":
                            if (dtNow.DayOfWeek > DayOfWeek.Sunday)
                            {
                                dtNow = dtNow.AddDays(System.Convert.ToDouble(dtNow.DayOfWeek) + 1);
                            }

                            break;
                    }
                    planTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, ts.Hours, ts.Minutes, 0);
                    calendarInfo.PLANTIME = planTime;
                    break;
                case "MONTH"://每月                                  
                    int intMonth = System.Convert.ToInt16(reminderrmodel);
                    if (intMonth < dtNow.Day)
                    {
                        dtNow = dtNow.AddMonths(1).AddDays(intMonth - dtNow.Day);
                    }
                    else
                    {
                        dtNow = dtNow.AddDays(intMonth - dtNow.Day);
                    }
                    planTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, ts.Hours, ts.Minutes, 0);
                    calendarInfo.PLANTIME = planTime;
                    break;
                case "YEAR"://每年
                    try
                    {
                        DateTime dtyear = System.Convert.ToDateTime(reminderrmodel);
                        if (dtyear.Month >= dtNow.Month)
                        {
                            if (dtyear.Month > dtNow.Month)
                            {
                                dtNow = dtyear.AddMonths(dtyear.Month - dtNow.Month);
                            }
                            else
                            {
                                if (dtyear.Day >= dtNow.Day)
                                {
                                    dtNow = dtyear.AddDays(dtyear.Day - dtNow.Day);
                                }
                                else
                                {
                                    dtNow = dtyear.AddMonths(1).AddDays(dtyear.Day - dtNow.Day);
                                }
                            }
                        }
                        else
                        {
                            dtNow = dtyear.AddYears(1).AddMonths(dtyear.Month - dtNow.Month);
                        }
                        planTime = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, ts.Hours, ts.Minutes, 0);
                        calendarInfo.PLANTIME = planTime;
                    }
                    catch (Exception ex)
                    {
                        SMT.Foundation.Log.Tracer.Debug("CalendarManagementBll-UpdatePlanTime，选择年时出现错误：" + ex.ToString());
                        strReturn = "提醒周期为年时，传递的参数错误";
                        return strReturn;
                    }

                    break;
            }
            if (employee == null)
            {
                strReturn = "员工信息为空";
                return strReturn;
            }
            
            calendarInfo.OWNERCOMPANYID = employee.OWNERCOMPANYID;
            calendarInfo.OWNERDEPARTMENTID = employee.OWNERDEPARTMENTID;
            calendarInfo.OWNERPOSTID = employee.OWNERPOSTID;
            calendarInfo.OWNERID = employee.EMPLOYEEID;
            calendarInfo.OWNERNAME = employee.EMPLOYEECNAME;
            if (operationFlag == "Add")
            {
                calendarInfo.CREATEPOSTID = employee.OWNERPOSTID;
                calendarInfo.CREATEDEPARTMENTID = employee.OWNERDEPARTMENTID;
                calendarInfo.CREATECOMPANYID = employee.OWNERCOMPANYID;
                calendarInfo.CREATEUSERID = employee.EMPLOYEEID;
                calendarInfo.CREATEUSERNAME = employee.EMPLOYEECNAME;
                calendarInfo.CREATEDATE = DateTime.Now;
            }
            else
            {
                calendarInfo.UPDATEUSERID = employee.EMPLOYEEID;
                calendarInfo.UPDATEUSERNAME = employee.EMPLOYEECNAME;
                calendarInfo.UPDATEDATE = DateTime.Now;
            }
            return strReturn;
        }

        /// <summary>
        /// 检查日程安排是否符合要求
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="reminderrmodel"></param>
        /// <param name="reparteminder"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public string checkCalendarInfo(string title, string content, string reminderrmodel, string reparteminder, string employeeID, string strHourMins)
        {
            string strReturn = string.Empty;
            if (string.IsNullOrEmpty(title))
            {
                strReturn = "标题不能为空";
                return strReturn;
            }
            if (title.Length > 50)
            {
                strReturn = "标题长度不能超过50个字符";
                return strReturn;
            }
            if (string.IsNullOrEmpty(content))
            {
                strReturn = "日程详情不能为空";
                return strReturn;
            }
            if (content.Length > 1000)
            {
                strReturn = "日程详情不能超过1000个字符";
                return strReturn;
            }
            if (string.IsNullOrEmpty(reminderrmodel))
            {
                strReturn = "提醒时间不能为空";
                return strReturn;
            }
            if (string.IsNullOrEmpty(reparteminder))
            {
                strReturn = "提醒周期不能为空";
                return strReturn;
            }
            if (string.IsNullOrEmpty(employeeID))
            {
                strReturn = "员工ID不能为空";
                return strReturn;
            }
            if (string.IsNullOrEmpty(strHourMins))
            {
                strReturn = "计划时间中时分不能为空";
                return strReturn;
            }
            if (!(strHourMins.IndexOf(":") > -1))
            {
                strReturn = "计划时间中时分格式不对";
                return strReturn;
            }
            TimeSpan ts = new TimeSpan();
            try
            {
                ts = TimeSpan.Parse(strHourMins);
            }
            catch (Exception ex)
            {
                Tracer.Debug("strHourMins为：" + strHourMins +".错误信息为："+ ex.ToString());
                strReturn = "计划时间中时分错误";
                return strReturn;
            }
            return strReturn;
        }

        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public V_EMPLOYEEVIEW getEmployee(string employeeID)
        {
            V_EMPLOYEEVIEW employee = new V_EMPLOYEEVIEW();
            try
            {
                PersonnelServiceClient personel = new PersonnelServiceClient();
                employee = personel.GetEmployeeInfoByEmployeeID(employeeID);
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("CalendarManagementBll-getEmployee出现错误，员工ID:" + employeeID + ".错误信息：" + ex.ToString());
                return null;
            }
            return employee;
        }
        #endregion
    }
}