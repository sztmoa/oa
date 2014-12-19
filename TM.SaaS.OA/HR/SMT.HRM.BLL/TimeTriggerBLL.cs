//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using SMT.SaaS.Services;
//using SMT.SaaS.Services.Model;

//namespace SMT.HRM.BLL
//{
//    public class TimeTriggerBLL
//    {
//        private EngineService engineService = new EngineService();

//        /// <summary>
//        /// 执行周期是一次的 触发定时器  
//        /// </summary>
//        /// <param name="startDate">开始时间</param>
//        /// <param name="endDate">结束时间</param>
//        /// <param name="id">业务类型的ID</param>
//        /// <param name="code">用于指示执行那个方法</param>
//        /// 此处的startDate=endDate，都为TeminateDate
//        /// <returns></returns>
//        public bool OnceTimingTrigger(DateTime startDate, DateTime endDate, string id, string code)
//        {
//            try
//            {
//                //触发定时器(一次的)
//                var sendTime = startDate.AddDays(-3);
//                //////测试用的
//                //var sendTime = System.DateTime.Now.AddHours(-2);
//                TimmingTrigger trigger = new TimmingTrigger();
//                trigger.FormID = id;
//                //这是调用的方法名
//                trigger.FuncName = "EventTriggerProcess";
//                //参数必须以下这种格式组装
//                string strParam = code + "[" + id + "&E]";
//                //这是传给方法的参数值
//                trigger.FuncParamter = strParam;
//                trigger.ModelCode = "OTP";
//                trigger.SystemCode = "OTP";
//                trigger.Remark = "HR审核业务完成后计划定时任务";
//                //startDate.Year == 9999，表示永不过期，startDate.AddDays(1)，过期时间+1天后执行过期操作;
//                trigger.TriggerTime = startDate.Year == 9999 ? startDate : startDate.AddDays(1);
//                //触发周期0 只触发一次 1 分钟 2小时 3 天 4 月 5年 6周 7未知
//                trigger.TriggerRound = 0;
//                trigger.WcfBindType = "basicHttpBinding";
//                trigger.WCFUrl = "AttendService.svc";
//                trigger.TRIGGERSTART = Convert.ToDateTime(startDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
//                trigger.TRIGGEREND = Convert.ToDateTime(startDate.AddDays(1).ToString("yyyy-MM-dd 00:00:00"));
//                engineService.AddTimingTrigger(trigger);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                //写日志
//                //做异常处理
//                //返回错误信息到前台
//                SMT.SaaS.Common.ErrorLog.WriteErrorMessage(SaaS.Common.SMTAppModule.HR, "OnceTimingTrigger", "ID:" + id + "Function:" + code, "SMT.HRM.BLL.TimeTriggerBLL", ex);
//                return false;
//            }
//        }

//        public bool MailTimingTrigger(DateTime startDate, DateTime endDate, string id, string code)
//        {
//            try
//            {
//                //触发定时器(一次的)
//                var sendTime = startDate.AddDays(-7);
//                //////测试用的
//                //var sendTime = System.DateTime.Now.AddHours(-2);
//                TimmingTrigger trigger = new TimmingTrigger();
//                trigger.FormID = id;
//                //这是调用的方法名
//                trigger.FuncName = "EventTriggerProcess";
//                //参数必须以下这种格式组装
//                string strParam = code + "[" + id + "&E]";
//                //这是传给方法的参数值
//                trigger.FuncParamter = strParam;
//                trigger.ModelCode = "OTP";
//                trigger.SystemCode = "OTP";
//                trigger.Remark = "HR审核业务完成后计划定时任务";
//                //startDate.Year == 9999，表示永不过期，startDate.AddDays(1)，过期时间+1天后执行过期操作;
//                trigger.TriggerTime = startDate.Year == 9999 ? startDate : startDate.AddDays(-7);
//                //触发周期0 只触发一次 1 分钟 2小时 3 天 4 月 5年 6周 7未知
//                trigger.TriggerRound = 0;
//                trigger.WcfBindType = "basicHttpBinding";
//                trigger.WCFUrl = "AttendService.svc";
//                trigger.TRIGGERSTART = Convert.ToDateTime(startDate.AddDays(-7).ToString("yyyy-MM-dd 00:00:00"));
//                trigger.TRIGGEREND = Convert.ToDateTime(startDate.AddDays(-7).ToString("yyyy-MM-dd 00:00:00"));
//                engineService.AddTimingTrigger(trigger);
                
//                return true;
//            }
//            catch (Exception ex)
//            {
//                //写日志
//                //做异常处理
//                //返回错误信息到前台
//                SMT.SaaS.Common.ErrorLog.WriteErrorMessage(SaaS.Common.SMTAppModule.HR, "MailTimingTrigger", "ID:" + id + "Function:" + code, "SMT.HRM.BLL.TimeTriggerBLL", ex);
//                return false;
//            }
//        }

//    }
//}
