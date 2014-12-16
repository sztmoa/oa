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
using SMT.HRM.CustomModel.Common;
using SMT.HRM.CustomModel.Response;
using SMT.HRM.CustomModel.Request;

namespace SMT.HRM.BLL
{
    public class LeaveReferOTBLL : BaseBll<T_HR_LEAVEREFEROT>, ILookupEntity, IOperate
    {
        public int UpdateCheckState(string strEntityName, string EntityKeyName, string EntityKeyValue, string CheckState)
        {
            throw new NotImplementedException();
        }

        public EntityObject[] GetLookupData(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount, string userID)
        {
            throw new NotImplementedException();
        }

        public List<T_HR_LEAVEREFEROT> GetLeaveReferOTList(string LeaveRecordID)
        {
            return dal.GetObjects().Where(t => t.LEAVE_RECORDID == LeaveRecordID).ToList();
        }

        public List<V_LeaveReferOvertime> QueryLeaveRecordForVacationList(VacationForAdjustRequest request)
        {
            List<V_LeaveReferOvertime> list = new List<V_LeaveReferOvertime>();

            var refot = dal.GetObjects().Where(t => t.OVERTIME_RECORDID == request.OTID);
            var ots = dal.GetObjects<T_HR_EMPLOYEEOVERTIMERECORD>().Where(t => t.EMPLOYEEID == request.EmployeeID);

            var entrs = from e in refot
                        join t in dal.GetObjects<T_HR_EMPLOYEEOVERTIMERECORD>() on e.OVERTIME_RECORDID equals t.OVERTIMERECORDID
                        join s in dal.GetObjects<T_HR_EMPLOYEELEAVERECORD>() on e.LEAVE_RECORDID equals s.LEAVERECORDID
                        join k in dal.GetObjects<T_HR_LEAVETYPESET>() on e.LEAVE_TYPE_SETID equals k.LEAVETYPESETID
                        select new V_LeaveReferOvertime
                        {
                            ACTION = e.ACTION,
                            EFFECTDATE = e.EFFECTDATE,
                            EMPLOYEEID = e.EMPLOYEEID,
                            EmployeeName = t.EMPLOYEENAME,
                            EXPIREDATE = e.EXPIREDATE,
                            LEAVE_APPLY_DATE = e.LEAVE_APPLY_DATE,
                            LEAVE_CANCEL_RECORDID = e.LEAVE_CANCEL_RECORDID,
                            LEAVE_RECORDID = e.LEAVE_RECORDID,
                            LEAVE_TOTAL_DAYS = e.LEAVE_TOTAL_DAYS,
                            LEAVE_TYPE_SETID = e.LEAVE_TYPE_SETID,
                            LEAVE_TOTAL_HOURS = e.LEAVE_TOTAL_HOURS,
                            LeaveEndDate = s.ENDDATETIME.Value,
                            LeaveStartDate = s.STARTDATETIME.Value,
                            LeaveTypeValue = k.LEAVETYPEVALUE,
                            RECORDID = e.RECORDID,
                            OVERTIME_RECORDID = e.OVERTIME_RECORDID,
                            OvertimeStartDate = t.STARTDATE.Value,
                             OvertimeEndDate = t.ENDDATE.Value,
                              OvertimeHours = t.OVERTIMEHOURS.Value,
                            STATUS = e.STATUS,
                            LeaveReason = s.REASON,
                            LeaveHours = s.TOTALHOURS.Value                            

                        };

            return entrs.ToList();
        }


        /// <summary>
        /// 新增加班和请假的对应关系
        /// </summary>
        /// <param name="lroList"></param>
        /// <returns></returns>
        public int AddLeaveReferOvertime(List<T_HR_LEAVEREFEROT> lroList)
        {

            int effect = 0;
            if (lroList != null && lroList.Count > 0)
            {
                foreach (T_HR_LEAVEREFEROT entity in lroList)
                {
                    string strReturn = string.Empty;
                    try
                    {
                        //添加请假记录
                        effect = dal.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        Utility.SaveLog(ex.ToString());
                        throw ex;
                    }
                }
            }
            return effect;
        }

        public int UpdateLeaveReferOvertime(List<T_HR_LEAVEREFEROT> lroList, string LeaveRecordID)
        {
            int effect = 0;
            //删除所有记录 
            var referList = dal.GetObjects().Where(t => t.LEAVE_RECORDID == LeaveRecordID && t.ACTION == 1);
            if (referList != null && referList.ToList().Count > 0)
            {
                foreach (T_HR_LEAVEREFEROT entity in referList)
                {
                    try
                    {
                        dal.Delete(entity);
                    }
                    catch (Exception ex)
                    {
                        Utility.SaveLog(ex.ToString());
                        throw ex;
                    }
                }
            }

            if (lroList != null && lroList.Count > 0)
            {
                foreach (T_HR_LEAVEREFEROT entity in lroList)
                {
                    string strReturn = string.Empty;
                    try
                    {
                        //添加请假记录
                        effect = dal.Add(entity);
                    }
                    catch (Exception ex)
                    {
                        Utility.SaveLog(ex.ToString());
                        throw ex;
                    }
                }
            }
            return effect;
        }

    }
}
