using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

using System.Configuration;
using System.Web;
using log4net;
using System.Reflection;
using HRTimedTaskService.AttendanceWS;
using HRTimedTaskService.FBServiceWS;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace HRTimedTaskService
{
    public partial class InitAttendanceService : ServiceBase
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private string strFBBudgetElapsedHour = ConfigurationManager.AppSettings["FBBudgetElapsedHour"].ToString();
        private string strFreeDaysElapsedHour = ConfigurationManager.AppSettings["FreeDaysElapsedHour"].ToString();
        private string strAttRdsElapsedHour = ConfigurationManager.AppSettings["AttRdsElapsedHour"].ToString();
        private string strAttRdsCompanyID = ConfigurationManager.AppSettings["AttRdsCompanyID"].ToString();
        private List<string> strAttRdsCompanyIDList = new List<string>();
        private List<string> strAttRdsCompanyIDCheckList = new List<string>();
        AttendanceServiceClient clientAtt = new AttendanceServiceClient();
        FBServiceClient clientFB = new FBServiceClient();


        public InitAttendanceService()
        {
            InitializeComponent();

            if (strAttRdsCompanyIDList.Count() == 0 && !string.IsNullOrWhiteSpace(strAttRdsCompanyID))
            {
                string[] strlist = strAttRdsCompanyID.Split(',');
                strAttRdsCompanyIDList.AddRange(strlist);
            }
        }

        protected override void OnStart(string[] args)
        {
            //服务启动
            this.timerSMTCus.Enabled = true;
            log.Debug(DateTime.Now.ToString() + "，启动协同办公自定义服务成功");
        }

        protected override void OnStop()
        {
            //服务停止
            this.timerSMTCus.Enabled = false;
            log.Debug(DateTime.Now.ToString() + "，关闭协同办公自定义服务成功");
        }

        private void InitAttRd()
        {
            DateTime dtCur = DateTime.Now;
            if (dtCur.Hour != 3 && dtCur.Day != 1)
            {
                log.Debug(DateTime.Now.ToString() + "，初始化考勤记录未在指定时间内");
                return;
            }

            try
            {
                log.Debug(DateTime.Now.ToString() + "，初始化考勤记录开始");
                AttendanceServiceClient clientAtt = new AttendanceServiceClient();
                clientAtt.AsignAttendanceSolutionWithAll();
                log.Debug(DateTime.Now.ToString() + "，初始化考勤记录全部完成");
            }
            catch (Exception ex)
            {
                log.Debug(DateTime.Now.ToString() + "，初始化考勤记录发生异常，异常原因：" + ex.ToString());
            }
        }

        /// <summary>
        /// 考勤初始化
        /// </summary>
        /// <param name="strCompanyID"></param>
        private void AsignAttRds(string strCompanyID)
        {
            string strOrgType = "1";
            string strCurYearMonth = DateTime.Now.ToString("yyyy-MM");
            clientAtt.AsignAttendanceSolutionByOrgIDAndMonth(strOrgType, strCompanyID, strCurYearMonth);
            log.Debug(DateTime.Now.ToString() + "，调用考勤服务，生成（CompanyID：" + strCompanyID + "， 月份为：" + strCurYearMonth + "）考勤初始化记录成功");
        }

        /// <summary>
        /// 预算月结
        /// </summary>
        private void CloseBudget()
        {
            clientFB.CloseBudget();
            log.Debug(DateTime.Now.ToString() + "，调用FB服务，进行预算月结成功");
        }

        /// <summary>
        /// 生成带薪假
        /// </summary>
        /// <param name="strCompanyID"></param>
        private void GenerateFreedays(string strCompanyID)
        {
            string strOrgType = "1";
            clientAtt.CalculateEmployeeLevelDayCountByOrgID(strOrgType, strCompanyID);
            log.Debug(DateTime.Now.ToString() + "，调用考勤服务，生成（CompanyID：" + strCompanyID + "）带薪假成功");
        }

        /// <summary>
        /// 定时任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerSMTCus_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime dtCur = DateTime.Now;
            log.Debug(DateTime.Now.ToString() + "，进入定时任务，执行以下任务：预算月结，生成带薪假，考勤初始化");

            string strCurFBBudgetElapsedHour = "M" + dtCur.Day.ToString() + "-H" + dtCur.Hour.ToString();
            string strCurFreeDaysElapsedHour = "M" + dtCur.Day.ToString() + "-H" + dtCur.Hour.ToString();
            string strCurAttRdsElapsedHour = "BM" + dtCur.Day.ToString() + "-H" + dtCur.Hour.ToString();

            if (strFBBudgetElapsedHour == strCurFBBudgetElapsedHour)
            {
                log.Debug(DateTime.Now.ToString() + "，开始执行以下任务：预算月结，当前时间符合设定任务运行时间。");
                try
                {
                    CloseBudget();
                }
                catch (Exception ex)
                {
                    log.Debug(DateTime.Now.ToString() + "，进行预算月结执行失败，失败原因：" + ex.ToString());
                }
            }

            if (strFreeDaysElapsedHour == strCurFreeDaysElapsedHour)
            {
                log.Debug(DateTime.Now.ToString() + "，开始执行以下任务：生成带薪假，当前时间符合设定任务运行时间。");
                try
                {
                    string[] strList = clientAtt.GetAllCompanyIDByAttendSolAsign(DateTime.Now.ToString("yyyy-MM"));

                    if (strList == null)
                    {
                        log.Debug(DateTime.Now.ToString() + "，执行以下任务：生成带薪假，发现无需要进行生成带薪假的公司。");
                        return;
                    }

                    if (strList.Length == 0)
                    {
                        log.Debug(DateTime.Now.ToString() + "，执行以下任务：生成带薪假，发现无需要进行生成带薪假的公司。");
                        return;
                    }

                    foreach (var strCompanyID in strList)
                    {
                        GenerateFreedays(strCompanyID);
                    }
                }
                catch (Exception ex)
                {
                    log.Debug(DateTime.Now.ToString() + "，生成带薪假执行失败，失败原因：" + ex.ToString());
                }
            }

            if (strAttRdsElapsedHour == strCurAttRdsElapsedHour)
            {
                log.Debug(DateTime.Now.ToString() + "，开始执行以下任务：考勤初始化，当前时间符合设定任务运行时间。");
                if (strAttRdsCompanyIDList.Count() > 0)
                {
                    foreach (var strCompanyID in strAttRdsCompanyIDList)
                    {
                        try
                        {
                            AsignAttRds(strCompanyID);
                        }
                        catch (Exception ex)
                        {
                            log.Debug(DateTime.Now.ToString() + "，考勤初始化执行失败， 对应的CompanyID: " + strCompanyID + "。失败原因：" + ex.ToString());
                        }
                    }
                }
                else
                {
                    log.Debug(DateTime.Now.ToString() + "，执行以下任务：考勤初始化，发现无需要进行考勤初始化的公司。"); 
                }
            }
        }
    }
}
