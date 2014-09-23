using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.SaaS.OA.DAL;
using SMT_OA_EFModel;
using System.Linq.Dynamic;
using SMT.Foundation.Log;
using SMT.SaaS.BLLCommonServices.PersonnelWS;

namespace SMT.SaaS.OA.BLL
{
    public class WorkerCordManagementBll : BaseBll<T_OA_WORKRECORD>
    {

        public IQueryable<T_OA_WORKRECORD> GetWorkerCodeListByUserID(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount, string userID)
        {
            var q = from ent in dal.GetTable()
                    select ent;
            List<object> queryParas = new List<object>();
            if (paras != null)
            {
                queryParas.AddRange(paras);
            }
            UtilityClass.SetOrganizationFilter(ref filterString, ref queryParas, userID, "T_OA_WORKRECORD");
            if (!string.IsNullOrEmpty(filterString))
            {
                q = q.Where(filterString, queryParas.ToArray());
            }
            q = q.OrderBy(sort);
            q = Utility.Pager<T_OA_WORKRECORD>(q, pageIndex, pageSize, ref pageCount);
            if (q.Count() > 0)
            {
                return q;
            }
            return null;
        }

        public bool AddWorkCord(T_OA_WORKRECORD entity)
        {
            try
            {
                dal.AddToContext(entity);
                int i = dal.SaveContextChanges();
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
                Tracer.Debug("日程管理WorkerCordManagementBll-AddWorkCord" + System.DateTime.Now.ToString() + " " + ex.ToString());
                throw (ex);
            }
        }

        public bool DeleteWorkCord(string calendarGuid)
        {
            try
            {
                var entitys = (from ent in dal.GetTable()
                               //where ent.GUID == calendarGuid
                               where ent.WORKRECORDID == calendarGuid
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
                Tracer.Debug("日程管理WorkerCordManagementBll-DeleteWorkCord" + System.DateTime.Now.ToString() + " " + ex.ToString());
                return false;
                throw (ex);
            }
        }

        public void UpdateWorkCord(T_OA_WORKRECORD entity)
        {
            try
            {
                var workerCordList = from ent in dal.GetTable()
                                     //where ent.GUID == entity.GUID
                                     where ent.WORKRECORDID == entity.WORKRECORDID
                                     select ent;
                if (workerCordList.Count() > 0)
                {
                    var workerCordInfo = workerCordList.FirstOrDefault();
                    workerCordInfo.TITLE = entity.TITLE;
                    workerCordInfo.CONTENT = entity.CONTENT;
                    workerCordInfo.CREATEDATE = entity.CREATEDATE;
                    workerCordInfo.PLANTIME = entity.PLANTIME;
                    workerCordInfo.UPDATEUSERID = entity.UPDATEUSERID;
                    workerCordInfo.UPDATEUSERNAME = entity.UPDATEUSERNAME;
                    workerCordInfo.UPDATEDATE = entity.UPDATEDATE;

                    dal.Update(workerCordInfo);
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("日程管理WorkerCordManagementBll-UpdateWorkCord" + System.DateTime.Now.ToString() + " " + ex.ToString());
                throw (ex);
            }
        }


        public int UpdateWorkCordNew(T_OA_WORKRECORD entity)
        {
            int intResult = 0;
            try
            {
                var workerCordList = from ent in dal.GetTable()
                                     //where ent.GUID == entity.GUID
                                     where ent.WORKRECORDID == entity.WORKRECORDID
                                     select ent;
                if (workerCordList.Count() > 0)
                {
                    var workerCordInfo = workerCordList.FirstOrDefault();
                    workerCordInfo.TITLE = entity.TITLE;
                    workerCordInfo.CONTENT = entity.CONTENT;
                    workerCordInfo.CREATEDATE = entity.CREATEDATE;
                    workerCordInfo.PLANTIME = entity.PLANTIME;
                    workerCordInfo.UPDATEUSERID = entity.UPDATEUSERID;
                    workerCordInfo.UPDATEUSERNAME = entity.UPDATEUSERNAME;
                    workerCordInfo.UPDATEDATE = entity.UPDATEDATE;

                    intResult = dal.Update(workerCordInfo);
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("日程管理WorkerCordManagementBll-UpdateWorkCordNew" + System.DateTime.Now.ToString() + " " + ex.ToString());
                throw (ex);
            }
            return intResult;
        }

        /// <summary>
        /// 获取工作日志实体
        /// </summary>
        /// <param name="workRecordID">工作日志ID</param>
        /// <returns>返回工作日志实体对象</returns>
        public T_OA_WORKRECORD GetWorkCordByID(string workRecordID)
        {
            T_OA_WORKRECORD record = new T_OA_WORKRECORD();
            try
            {
                Tracer.Debug("workRecordID值为： " + workRecordID);
                var ents = from ent in dal.GetObjects<T_OA_WORKRECORD>()                                     
                                     where ent.WORKRECORDID == workRecordID
                                     select ent;
                if (ents.Count() > 0)
                {                    
                    record = ents.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("日程管理WorkerCordManagementBll-GetWorkCordByID" + System.DateTime.Now.ToString() + " " + ex.ToString());
                throw (ex);
            }
            return record;
        }

        #region 手机版调用服务

        /// <summary>
        /// 添加工作日志
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="employeeID">员工ID</param>
        /// <param name="strTime">工作日志时间</param>
        /// <returns>成功返回为空</returns>
        public string AddWorkCordForMobile(string title, string content, string employeeID, string strTime, ref string workRecordID)
        {
            string strReturn = string.Empty;
            try
            {
                Tracer.Debug("开始添加工作日志信息");
                Tracer.Debug("title：" + title);
                Tracer.Debug("content： " + content);
                Tracer.Debug("employeeID：" + employeeID);
                Tracer.Debug("strTime： " + strTime);
                Tracer.Debug("workRecordID： " + workRecordID);  
                strReturn = checkWorkCordInfo(title, content, employeeID, strTime);
                if (!string.IsNullOrEmpty(strReturn))
                {
                    return strReturn;
                }
                CalendarManagementBll calenDarBll = new CalendarManagementBll();
                V_EMPLOYEEVIEW employee = calenDarBll.getEmployee(employeeID);
                if (employee == null)
                {
                    strReturn = "没获取到员工信息，保存失败";
                    return strReturn;
                }
                T_OA_WORKRECORD workRecord = new T_OA_WORKRECORD();
                workRecord.WORKRECORDID = Guid.NewGuid().ToString();
                workRecord.TITLE = title;
                workRecord.CONTENT = content;
                DateTime planTime = System.Convert.ToDateTime(strTime);
                workRecord.PLANTIME = planTime;
                strReturn = UpdateWorkCordOwnerInfo(ref workRecord, "Add", employee);
                if (string.IsNullOrEmpty(strReturn))
                {
                    bool blResult = AddWorkCord(workRecord);
                    if (!blResult)
                    {
                        strReturn = "添加工作日志失败";
                    }
                    workRecordID = workRecord.WORKRECORDID;
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("title：" + title);
                Tracer.Debug("content： " + content);
                Tracer.Debug("employeeID：" + employeeID);
                Tracer.Debug("strTime： " + strTime);
                Tracer.Debug("日程管理WorkerCordManagementBll-AddWorkCordForMobile："  + ex.ToString());
                strReturn = "添加工作日志时出现异常";
            }
            return strReturn;
        }

        /// <summary>
        /// 修改工作日志信息
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="employeeID">员工ID</param>
        /// <param name="strTime">工作日志时间</param>
        /// <param name="workRecordID">工作日志ID</param>
        /// <returns>成功为空</returns>
        public string UpdateWorkCordForMobile(string title, string content, string employeeID, string strTime,string workRecordID)
        {
            string strReturn = string.Empty;
            try
            {
                Tracer.Debug("开始修改工作日志信息");
                Tracer.Debug("title：" + title);
                Tracer.Debug("content： " + content);
                Tracer.Debug("employeeID：" + employeeID);
                Tracer.Debug("strTime： " + strTime);
                Tracer.Debug("workRecordID： " + workRecordID);
                strReturn = checkWorkCordInfo(title, content, employeeID, strTime);
                if (!string.IsNullOrEmpty(strReturn))
                {
                    return strReturn;
                }
                CalendarManagementBll calenDarBll = new CalendarManagementBll();
                V_EMPLOYEEVIEW employee = calenDarBll.getEmployee(employeeID);
                if (employee == null)
                {
                    strReturn = "没获取到员工信息，保存失败";
                    return strReturn;
                }
                T_OA_WORKRECORD workRecord = new T_OA_WORKRECORD();
                var ents = from ent in dal.GetObjects<T_OA_WORKRECORD>()
                           where ent.WORKRECORDID == workRecordID
                           select ent;
                if (ents.Count() == 0)
                {
                    strReturn = "修改的工作日志不存在";
                    return strReturn;
                }
                workRecord = ents.FirstOrDefault();
                workRecord.TITLE = title;
                workRecord.CONTENT = content;
                DateTime planTime = System.Convert.ToDateTime(strTime);
                workRecord.PLANTIME = planTime;
                strReturn = UpdateWorkCordOwnerInfo(ref workRecord, "Edit", employee);
                if (string.IsNullOrEmpty(strReturn))
                {
                    int intResult = UpdateWorkCordNew(workRecord);
                    if (intResult < 1)
                    {
                        strReturn = "修改工作日志失败";
                    }
                }

            }
            catch (Exception ex)
            {
                Tracer.Debug("title：" + title);
                Tracer.Debug("content： " + content);
                Tracer.Debug("employeeID：" + employeeID);
                Tracer.Debug("strTime： " + strTime);
                Tracer.Debug("workRecordID： " + workRecordID);
                Tracer.Debug("日程管理WorkerCordManagementBll-UpdateWorkCordForMobile：" + ex.ToString());
                strReturn = "修改工作日志时出现异常";
            }
            return strReturn;
        }

        /// <summary>
        /// 检查工作日志是否符合要求
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="employeeID"></param>
        /// <param name="strTime"></param>
        /// <returns>符合返回为空</returns>
        public string checkWorkCordInfo(string title, string content, string employeeID, string strTime)
        {
            string strReturn = string.Empty;
            if (string.IsNullOrEmpty(employeeID))
            {
                strReturn = "员工ID不能为空";
                return strReturn;
            }
            if (string.IsNullOrEmpty(title))
            {
                strReturn = "日志标题不能为空";
                return strReturn;
            }
            if (title.Length > 50)
            {
                strReturn = "日志标题长度不能超过50个字符";
                return strReturn;
            }
            if (string.IsNullOrEmpty(content))
            {
                strReturn = "日志内容不能为空";
                return strReturn;
            }
            if (content.Length > 1000)
            {
                strReturn = "日志内容不能超过1000个字符";
                return strReturn;
            }
            if (string.IsNullOrEmpty(strTime))
            {
                strReturn = "日志时间不能为空";
                return strReturn;
            }
            
            
            DateTime dt = new DateTime();
            try
            {
                dt = System.Convert.ToDateTime(strTime);
            }
            catch (Exception ex)
            {
                Tracer.Debug("strTime为：" + strTime + ".错误信息为：" + ex.ToString());
                strReturn = "日志时间格式错误";
                return strReturn;
            }
            return strReturn;
        }

        /// <summary>
        /// 修改工作日志的实体
        /// </summary>
        /// <param name="workRecordInfo"></param>
        /// <param name="operationFlag"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        private string UpdateWorkCordOwnerInfo(ref T_OA_WORKRECORD workRecordInfo, string operationFlag, V_EMPLOYEEVIEW employee)
        {
            string strReturn = string.Empty;
            workRecordInfo.OWNERCOMPANYID = employee.OWNERCOMPANYID;
            workRecordInfo.OWNERDEPARTMENTID = employee.OWNERDEPARTMENTID;
            workRecordInfo.OWNERPOSTID = employee.OWNERPOSTID;
            workRecordInfo.OWNERID = employee.EMPLOYEEID;
            workRecordInfo.OWNERNAME = employee.EMPLOYEECNAME;
            if (operationFlag == "Add")
            {
                workRecordInfo.CREATEPOSTID = employee.OWNERPOSTID;
                workRecordInfo.CREATEDEPARTMENTID = employee.OWNERDEPARTMENTID;
                workRecordInfo.CREATECOMPANYID = employee.OWNERCOMPANYID;
                workRecordInfo.CREATEUSERID = employee.EMPLOYEEID;
                workRecordInfo.CREATEUSERNAME = employee.EMPLOYEECNAME;
                workRecordInfo.CREATEDATE = DateTime.Now;
            }
            else
            {
                workRecordInfo.UPDATEUSERID = employee.EMPLOYEEID;
                workRecordInfo.UPDATEUSERNAME = employee.EMPLOYEECNAME;
                workRecordInfo.UPDATEDATE = DateTime.Now;
            }
            return strReturn;
        }

        /// <summary>
        /// 获取员工在1个月中的日期集合
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <param name="strDate">日期</param>
        /// <returns>返回日期集合</returns>
        public List<string> GetDateByEmployeeIDAndDate(string employeeID, string strDate)
        {
            List<string> listReturns = new List<string>();
            try
            {
                Tracer.Debug("GetDateByEmployeeIDAndDate开始");
                Tracer.Debug("GetDateByEmployeeIDAndDate-employeeID：" + employeeID);
                Tracer.Debug("GetDateByEmployeeIDAndDate-strDate：" + strDate);
                Tracer.Debug("GetDateByEmployeeIDAndDate结束");
                if (string.IsNullOrEmpty(strDate))
                {
                    return listReturns;
                }
                DateTime dt = new DateTime();
                try
                {
                    dt = System.Convert.ToDateTime(strDate);
                }
                catch (Exception ex)
                {
                    Tracer.Debug("strDate：" + strDate);
                    Tracer.Debug("日期转换出错：" + ex.ToString());
                }
                int year = dt.Year;
                int month = dt.Month;
                DateTime d1 = new DateTime(year, month, 1); 
                DateTime d2 = d1.AddMonths(1).AddDays(-1);
                //int lastDays = dt.
                var ents = from ent in dal.GetObjects<T_OA_WORKRECORD>()
                           where ent.OWNERID == employeeID
                           && ent.PLANTIME >= d1 && ent.PLANTIME <= d2
                           select ent;
                if (ents.Count() > 0)
                {
                    ents = ents.OrderByDescending(s=>s.CREATEDATE);
                    ents.ToList().ForEach(s => {
                        var str = s.PLANTIME.ToString("yyyy-MM-dd");
                        var entExist = from en in listReturns
                                       where en == str
                                       select en;
                        if (entExist.Count() == 0)
                        {
                            listReturns.Add(str);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("strDate：" + strDate);
                Tracer.Debug("GetDateByEmployeeIDAndDate日期转换出错：" + ex.ToString());
            }
            return listReturns;
        }

        /// <summary>
        /// 获取员工在1个月中的工作日志集合
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <param name="strDate">日期</param>
        /// <returns>返回1个月的工作日志集合</returns>
        public List<T_OA_WORKRECORD> GetWorkRecordsByEmployeeIDAndDate(string employeeID, string strDate)
        {
            List<T_OA_WORKRECORD> listReturns = new List<T_OA_WORKRECORD>();
            try
            {
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndDate开始");
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndDate-employeeID：" + employeeID);
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndDate-strDate：" + strDate);
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndDate结束");
                if (string.IsNullOrEmpty(strDate))
                {
                    return listReturns;
                }
                DateTime dt = new DateTime();
                try
                {
                    dt = System.Convert.ToDateTime(strDate);
                }
                catch (Exception ex)
                {
                    Tracer.Debug("strDate：" + strDate);
                    Tracer.Debug("日期转换出错：" + ex.ToString());
                }
                int year = dt.Year;
                int month = dt.Month;
                DateTime d1 = new DateTime(year, month, 1);
                DateTime d2 = d1.AddMonths(1).AddDays(-1);
                //int lastDays = dt.
                var ents = from ent in dal.GetObjects<T_OA_WORKRECORD>()
                           where ent.OWNERID == employeeID
                           && ent.PLANTIME >= d1 && ent.PLANTIME <= d2
                           select ent;
                listReturns = ents.ToList();
            }
            catch (Exception ex)
            {
                Tracer.Debug("employeeID：" + employeeID);
                Tracer.Debug("strDate：" + strDate);
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndDate日期转换出错：" + ex.ToString());
            }
            return listReturns;
        }


        /// <summary>
        /// 获取员工当天的日志记录
        /// </summary>
        /// <param name="employeeID">员工ID</param>
        /// <param name="strDate">日期</param>
        /// <returns>工作日志实体</returns>
        public T_OA_WORKRECORD GetWorkRecordsByEmployeeIDAndCurrentDay(string employeeID, string strDate)
        {
            T_OA_WORKRECORD record = new T_OA_WORKRECORD();
            try
            {
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndCurrentDay开始");
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndCurrentDay-employeeID：" + employeeID);
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndCurrentDay-strDate：" + strDate);
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndCurrentDay结束");
                if (string.IsNullOrEmpty(strDate))
                {
                    return record;
                }
                DateTime dt = new DateTime();
                try
                {
                    dt = System.Convert.ToDateTime(strDate);
                }
                catch (Exception ex)
                {
                    Tracer.Debug("strDate：" + strDate);
                    Tracer.Debug("日期转换出错：" + ex.ToString());
                }
                int year = dt.Year;
                int month = dt.Month;
                DateTime d1 = new DateTime(year, month,dt.Day );
                DateTime d2 = d1.AddDays(1).AddSeconds(-1);
                //int lastDays = dt.
                var ents = from ent in dal.GetObjects<T_OA_WORKRECORD>()
                           where ent.OWNERID == employeeID
                           && ent.PLANTIME >= d1 && ent.PLANTIME <= d2
                           orderby ent.CREATEDATE descending
                           select ent;
                record = ents.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Tracer.Debug("错误GetWorkRecordsByEmployeeIDAndCurrentDay-employeeID：" + employeeID);
                Tracer.Debug("错误GetWorkRecordsByEmployeeIDAndCurrentDay-strDate：" + strDate);
                Tracer.Debug("GetWorkRecordsByEmployeeIDAndCurrentDay日期转换出错：" + ex.ToString());
            }
            return record;
        }
        #endregion
    }
}