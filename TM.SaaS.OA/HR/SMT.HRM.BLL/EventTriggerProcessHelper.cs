using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.HRM.BLL
{
    /// <summary>
    /// 该类是用于数据库中定时任务的处理调用 
    /// </summary>
    public class EventTriggerProcessHelper
    {
        public static void processEvent(string param)
        {
            int iPos = param.IndexOf('[');
            int iPos1 = param.IndexOf("<System>");
            //int iLen1 = iPos - iPos1 - 8;
            int iLen = param.Length;
            string strFunctionName = param.Substring(0, iPos);
            string strParas = param.Substring(iPos + 1, (iLen - iPos - 2));
            string[] arrParas = strParas.Split('&');

            strFunctionName = strFunctionName.Replace("\r\n", "");

            if (strFunctionName == "ExpireOvertimeVacation")
            {
                //SendCycleTaskToEmployee(arrParas[0], arrParas[1], arrParas[2], arrParas[3], arrParas[4], arrParas[5]);
               AuditOverTimeRecordPass(arrParas[0]);
            }
            if (strFunctionName == "SendMailForExpireOvertime")
            {
                SendMailForExpiredOvertimeRecord(arrParas[0]);
            }           
        }

        /// <summary>
        /// 加班申请审核通过后添加定时任务，用于加班过期执行
        /// 在T_HR_EmployeeLevelDayCount表中，RecordID等于审核通过后的加班记录的ID
        /// </summary>
        public static void AuditOverTimeRecordPass(string LeaveDayCountRecordID)
        {
            EmployeeLevelDayCountBLL bll = new EmployeeLevelDayCountBLL();
            bll.ExpireOvertimeVacation(LeaveDayCountRecordID);
        }

        public static void SendMailForExpiredOvertimeRecord(string LeaveDayCountRecordID)
        {
            EmployeeLevelDayCountBLL bll = new EmployeeLevelDayCountBLL();
            bll.SendMailForExpiredOvertimeRecord(LeaveDayCountRecordID);
        }

    }
}
