using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMT.HRM.BLL;
using System.Xml.Linq;
using SMT_HRM_EFModel;

namespace SMT.HRM.Services
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LeftOfficeConfirmBLL bllconfirm = new LeftOfficeConfirmBLL();
            bllconfirm.UpdateCheckState("T_HR_LEFTOFFICECONFIRM", "CONFIRMI", "8727d26b-1245-4b51-88cf-e58a8eb0d089", "2");
         

             //OutApplyBLL bll = new OutApplyBLL();
             //bll.UpdateCheckState("T_HR_OUTAPPLYRECORD", "OUTAPPLYID", "b0ea57b8-5592-4cfc-bbc6-c70ba3a079c8", "1");
            // return;

            //using (SMT.SaaS.BLLCommonServices.FBServiceWS.FBServiceClient clientFB = new SaaS.BLLCommonServices.FBServiceWS.FBServiceClient())
            //{
            //    string strAssignOwnerID = System.Configuration.ConfigurationManager.AppSettings["PersonMoneyAssignOwner"];
            //    clientFB.CreatePersonMoneyAssignInfo("703dfb3c-d3dc-4b1d-9bf0-3507ba01b716", strAssignOwnerID, "05de9c77-8656-4bbc-a7b2-8b6f8005ac46");
            //    string strMsg = "生成下拨公司h的活动经费下拨单成功";
            //    SMT.Foundation.Log.Tracer.Debug(strMsg);
            //}
            return;

             EmployeeBLL bll = new EmployeeBLL();
             bll.test();
            // bll.GetEmployeeFunds("bac05c76-0f5b-40ae-b73b-8be541ed35ed", "24a358f9-8539-4faa-aee6-d5cbc8ea450d");
            //return;
            #region 定时触发测试
            //AttendanceSolutionAsignBLL bll = new AttendanceSolutionAsignBLL();
            //T_HR_ATTENDANCESOLUTIONASIGN ent = new T_HR_ATTENDANCESOLUTIONASIGN();

            //string strEntityName="T_HR_SALARYRECORDBATCH";
            //string EntityKeyName="MONTHLYBATCHID";
            //string EntityKeyValue = "f5b9757d-0cb0-4f0c-9135-c13af4946863";
            //Utility.UpdateCheckState(strEntityName, EntityKeyName, EntityKeyValue,"1");

            //EmployeeBLL ebll = new EmployeeBLL();
            //ebll.GetEmployeeFunds("bac05c76-0f5b-40ae-b73b-8be541ed35ed", "24a358f9-8539-4faa-aee6-d5cbc8ea450d");
           // bll.UpdateCheckState("T_HR_ATTENDANCESOLUTIONASIGN", "ATTENDANCESOLUTIONASIGNID", "07D79240-4EA1-4397-A0B1-502A10106530", "2");


            //string str="<Para FuncName=\"ATTENDANCESOLUTIONASIGNRemindTrigger\" Name=\"ATTENDANCESOLUTIONASIGNID\" Value=\"07D79240-4EA1-4397-A0B1-502A10106530\"></Para>";
            //EngineTriggerService sv = new EngineTriggerService();
            //sv.EventTriggerProcess(str);
            #endregion
            //return;
            //using (AttendMonthlyBalanceBLL bll = new AttendMonthlyBalanceBLL())
            //{
            //    bll.CalculateEmployeeAttendanceMonthlyByEmployeeID("2013-12", "c962b4ac-0c93-41b3-bb12-2ec93258ceab");

            //}
            //return;
            //using (AttendMonthlyBalanceBLL bll = new AttendMonthlyBalanceBLL())
            //{
            //    bll.CalculateEmployeeAttendanceMonthlyByEmployeeID("2013-12", "c962b4ac-0c93-41b3-bb12-2ec93258ceab");
              
            //}
             //using (EmployeeEntryBLL bll = new EmployeeEntryBLL())
            //{

            //    EmployeePostBLL ebll = new EmployeePostBLL();
            //    var entity = bll.GetEmployeeEntryByID("ae0b7a2d-ce96-45a8-8203-7f2996f16f68");


            //    var ent = ebll.GetEmployeePostByEmployeeID("095bf1f5-7f8a-4c6d-b9ac-1bb55e9d6331");
            //    bll.EmployeeEntryUpdate(entity, ent);
            //}
            return;
            //using (EmployeeBLL bll = new EmployeeBLL())
            //{
            //    bll.GetEmployeeLeaders("ae4c77df-f734-477a-aee7-09ece0269d7b", 0);
            //}
            return;
            #region 请假消除异常
            //测试请假消除异常
            //OutApplyBLL bll = new OutApplyBLL();
            //bll.updateAllLeve();
            //return;

            // bll.UpdateCheckState("T_HR_EMPLOYEEOUTAPPLIECRECORD", "OVERTIMERECORDID", "826a51bb-21e0-4d49-979f-0fcfbb452c32", "1");
            #endregion
            //EmployeePostChangeBLL bll = new EmployeePostChangeBLL();
            //bll.UpdateCheckState("T_HR_EMPLOYEEPOSTCHANGE", "POSTCHANGEID", "33550c08-aa08-47e5-ad65-9db98faf5375", "1");

            //EngineService eg = new EngineService();
            //string xml = System.IO.File.ReadAllText("c:/HRXML.txt");
            //XElement.Parse(xml);
            //eg.CallWaitAppService(xml);
            #region 重新初始化考勤
            //return;
            //using (AttendanceRecordBLL bll = new AttendanceRecordBLL())
            //{
            //    DateTime dtstart = new DateTime(2013, 09, 01);
            //    DateTime dtend = dtstart.AddMonths(1).AddDays(-1);
            //    string smtmsg
            //        = bll.CompulsoryInitialization("0", "72b3f128-6cf0-498c-8e70-89d0d66403f2", dtstart, dtend, "0");

            //    //return smtmsg;
            //}
            #endregion

            //using (AttendanceSolutionAsignBLL bllAttendanceSolutionAsign = new AttendanceSolutionAsignBLL())
            //{
            //    //bllAttendanceSolutionAsign.AsignAttendanceSolutionByOrgID("1", "703dfb3c-d3dc-4b1d-9bf0-3507ba01b716", "2013-09");     
            //    bllAttendanceSolutionAsign.AsignAttendanceSolutionByOrgID("4", "24a358f9-8539-4faa-aee6-d5cbc8ea450d", "2013-09");
            //}
        }
        //return;

        ////初始化集团打卡记录
        //using (AttendanceSolutionAsignBLL bllAttendanceSolutionAsign = new AttendanceSolutionAsignBLL())
        //{
        //    bllAttendanceSolutionAsign.AsignAttendanceSolutionByOrgID("1", "703dfb3c-d3dc-4b1d-9bf0-3507ba01b716", "2013-09");     
        //    //bllAttendanceSolutionAsign.AsignAttendanceSolutionByOrgID("4", "24a358f9-8539-4faa-aee6-d5cbc8ea450d", "2013-09");
        //}

        //using (ClockInRecordBLL bllClockInRecord = new ClockInRecordBLL())
        //{
        //    EmployeeBLL bll = new EmployeeBLL();
        //    string strMsg = string.Empty;
        //    List<T_HR_EMPLOYEECLOCKINRECORD> entTempList = new List<T_HR_EMPLOYEECLOCKINRECORD>();

        //    T_HR_EMPLOYEE emp = bll.GetEmployeeByName("曹利宁");

        //    DateTime dt= new DateTime(2013, 7, 4);
        //    DateTime dtStar = dt;
        //    DateTime dtEnd = dt.AddDays(1).AddSeconds(-1);

        //    AttendanceRecordBLL attbll = new AttendanceRecordBLL();

        //    string smtmsg = attbll.CompulsoryInitialization("4", emp.EMPLOYEEID, dtStar, dtEnd);


        //    //T_HR_EMPLOYEECLOCKINRECORD entTemp = new T_HR_EMPLOYEECLOCKINRECORD();
        //    //entTemp.CLOCKINRECORDID = System.Guid.NewGuid().ToString().ToUpper();
        //    //entTemp.FINGERPRINTID = idwEnrollNumber.ToString();
        //    //entTemp.CLOCKID = idwTMachineNumber.ToString();
        //    //entTemp.PUNCHDATE = DateTime.Parse(dtCurrent.ToString("yyyy-MM-dd") + " " + idwHour.ToString() + ":" + idwMinute.ToString() + ":00");
        //    //entTemp.PUNCHTIME = idwHour.ToString() + ":" + idwMinute.ToString();
        //    T_HR_EMPLOYEECLOCKINRECORD record = new T_HR_EMPLOYEECLOCKINRECORD();
        //    record.CLOCKINRECORDID = System.Guid.NewGuid().ToString();
        //    record.FINGERPRINTID = emp.FINGERPRINTID;
        //    record.PUNCHDATE = new DateTime(2013,7,4);
        //    record.PUNCHTIME = "08:17";
        //    entTempList.Add(record);

        //    T_HR_EMPLOYEECLOCKINRECORD record2 = new T_HR_EMPLOYEECLOCKINRECORD();
        //    record2.CLOCKINRECORDID = System.Guid.NewGuid().ToString();
        //    record2.FINGERPRINTID = emp.FINGERPRINTID;
        //    record2.PUNCHDATE = new DateTime(2013, 7, 4);
        //    record2.PUNCHTIME = "17:38";
        //    entTempList.Add(record2);

        //    bllClockInRecord.ImportClockInRdListByWindowsService("", entTempList, dtStar
        //        , dtEnd, "", ref strMsg);
        //}
        //}

        protected void Button1_Click(object sender, EventArgs e)
        {
            AttendanceService sv = new AttendanceService();
            sv.CalculateEmployeeAttendanceYearlyByEmployeeID("2013", txtEmployeeid.Text);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AttendanceService sv = new AttendanceService();
            sv.InitYouthLeaveSets();
            this.Button2.Enabled = false;
            this.lblYouth.Text = "初始化完成";
        }

    }
}