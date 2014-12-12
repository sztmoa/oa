using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SMT.FBAnalysis.UI.Form.SubjectManagement;

namespace SMT.FBAnalysis.UI.Models
{
    /// <summary>
    /// 公司经营指标
    /// </summary>
    public class CompanySubject
    {
        /// <summary>
        /// 公司ID
        /// </summary>
        public int CompanyID { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 经营指标集合
        /// </summary>
        public List<TradeGoal> TradGoalData { get; set;}
    }
    /// <summary>
    /// 各公司经营指标
    /// </summary>
    public class CompanyData
    {
        public static List<CompanySubject> CompanySubjectData
        {
            get
            {
                List<CompanySubject> data = new List<CompanySubject>();
                data.Add(new CompanySubject { CompanyID = 1, CompanyName = "爱施德", TradGoalData = Data1() });
                data.Add(new CompanySubject { CompanyID = 2, CompanyName = "物流", TradGoalData = Data2() });
                data.Add(new CompanySubject { CompanyID = 3, CompanyName = "彩梦", TradGoalData = Data3() });
                data.Add(new CompanySubject { CompanyID = 4, CompanyName = "地产", TradGoalData = Data4() });
                data.Add(new CompanySubject { CompanyID = 5, CompanyName = "酷人", TradGoalData = Data5() });
                data.Add(new CompanySubject { CompanyID = 6, CompanyName = "酷动", TradGoalData = Data6() });
                data.Add(new CompanySubject { CompanyID = 7, CompanyName = "酷奇", TradGoalData = Data7() });
                data.Add(new CompanySubject { CompanyID = 8, CompanyName = "在线", TradGoalData = Data8() });
                data.Add(new CompanySubject { CompanyID = 9, CompanyName = "集团", TradGoalData = Data9() });
                data.Add(new CompanySubject { CompanyID = 10, CompanyName = "星云风", TradGoalData = Data10() });
                return data;
            }
        }
        private static List<TradeGoal> Data1()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "期间费用" });
            data.Add(new TradeGoal { ID = 2, Goal = "单台期间费用" });
            data.Add(new TradeGoal { ID = 3, Goal = "销售收入" });
            data.Add(new TradeGoal { ID = 4, Goal = "销售数量" });
            data.Add(new TradeGoal { ID = 5, Goal = "人均销售收入" });
            data.Add(new TradeGoal { ID = 6, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 7, Goal = "净利润" });
            data.Add(new TradeGoal { ID = 8, Goal = "人均销售利润" });
            data.Add(new TradeGoal { ID = 9, Goal = "存货周转率" });
            data.Add(new TradeGoal { ID = 10, Goal = "应收帐款周转率" });
            return data;
        }
        private static List<TradeGoal> Data2()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "物流收入" });
            data.Add(new TradeGoal { ID = 2, Goal = "成本率" });
            data.Add(new TradeGoal { ID = 3, Goal = "净利润" });
            data.Add(new TradeGoal { ID = 4, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 5, Goal = "费用率" });
            data.Add(new TradeGoal { ID = 6, Goal = "人均物流收入" });
            data.Add(new TradeGoal { ID = 7, Goal = "应收帐款周转率" });
            return data;
        }
        private static List<TradeGoal> Data3()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "营业收入" });
            data.Add(new TradeGoal { ID = 2, Goal = "净利润" });
            data.Add(new TradeGoal { ID = 3, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 4, Goal = "人均营业收入" });
            data.Add(new TradeGoal { ID = 5, Goal = "期间费用" });
            data.Add(new TradeGoal { ID = 6, Goal = "人均费用" });
            data.Add(new TradeGoal { ID = 7, Goal = "应收帐款周转率" });
            return data;
        }
        private static List<TradeGoal> Data4()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "期间费用" });
            data.Add(new TradeGoal { ID = 2, Goal = "人均净利润" });
            data.Add(new TradeGoal { ID = 3, Goal = "营业收入" });
            data.Add(new TradeGoal { ID = 4, Goal = "净利润" });
            data.Add(new TradeGoal { ID = 5, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 6, Goal = "综合资本回报率" });
            data.Add(new TradeGoal { ID = 7, Goal = "新增可销售面积" });
            data.Add(new TradeGoal { ID = 8, Goal = "新增开发面积" });
            data.Add(new TradeGoal { ID = 9, Goal = "销售面积" });
            return data;
        }
        private static List<TradeGoal> Data5()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "期间费用" });
            data.Add(new TradeGoal { ID = 2, Goal = "单台期间费用" });
            data.Add(new TradeGoal { ID = 3, Goal = "销售收入" });
            data.Add(new TradeGoal { ID = 4, Goal = "销售数量" });
            data.Add(new TradeGoal { ID = 5, Goal = "人均销售收入" });
            data.Add(new TradeGoal { ID = 6, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 7, Goal = "净利润" });
            data.Add(new TradeGoal { ID = 8, Goal = "人均销售利润" });
            data.Add(new TradeGoal { ID = 9, Goal = "存货周转率" });
            data.Add(new TradeGoal { ID = 10, Goal = "应收帐款周转率" });
            return data;
        }
        private static List<TradeGoal> Data6()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "销售收入" });
            data.Add(new TradeGoal { ID = 2, Goal = "净利润" });
            data.Add(new TradeGoal { ID = 3, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 4, Goal = "期间费用" });
            data.Add(new TradeGoal { ID = 5, Goal = "人均净利润" });
            data.Add(new TradeGoal { ID = 6, Goal = "人均销售收入" });
            data.Add(new TradeGoal { ID = 7, Goal = "平效" });
            data.Add(new TradeGoal { ID = 8, Goal = "新增苹果专营店" });
            data.Add(new TradeGoal { ID = 9, Goal = "新增3C4U店" });
            data.Add(new TradeGoal { ID = 10, Goal = "应收帐款周转率" });
            data.Add(new TradeGoal { ID = 11, Goal = "存货周转率" });
            return data;
        }
        private static List<TradeGoal> Data7()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "期间费用" });
            data.Add(new TradeGoal { ID = 2, Goal = "销售收入" });
            data.Add(new TradeGoal { ID = 3, Goal = "销售数量" });
            data.Add(new TradeGoal { ID = 4, Goal = "人均销售收入" });
            data.Add(new TradeGoal { ID = 5, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 6, Goal = "净利润" });
            data.Add(new TradeGoal { ID = 7, Goal = "人均销售利润" });
            data.Add(new TradeGoal { ID = 8, Goal = "存货周转率" });
            data.Add(new TradeGoal { ID = 9, Goal = "应收帐款周转率" });
            return data;
        }
        private static List<TradeGoal> Data8()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 2, Goal = "期间费用" });
            data.Add(new TradeGoal { ID = 3, Goal = "人均期间费用" });
            return data;
        }
        private static List<TradeGoal> Data9()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 2, Goal = "期间费用" });
            data.Add(new TradeGoal { ID = 3, Goal = "人均期间费用" });
            data.Add(new TradeGoal { ID = 4, Goal = "补贴收入" });
            return data;
        }
        private static List<TradeGoal> Data10()
        {
            List<TradeGoal> data = new List<TradeGoal>();
            data.Add(new TradeGoal { ID = 1, Goal = "实际发货销售额" });
            data.Add(new TradeGoal { ID = 2, Goal = "毛利率" });
            data.Add(new TradeGoal { ID = 3, Goal = "库存周转率" });
            data.Add(new TradeGoal { ID = 4, Goal = "总人数" });
            data.Add(new TradeGoal { ID = 5, Goal = "期间费用" });
            data.Add(new TradeGoal { ID = 6, Goal = "人均期间费用" });
            data.Add(new TradeGoal { ID = 7, Goal = "净利润" });
            data.Add(new TradeGoal { ID = 8, Goal = "线下合作店面拓展数量" });
            return data;
        }
    }
    /// <summary>
    /// 经营指标和工作计划
    /// </summary>
    public class SubjectObject
    {
        public SMT.FBAnalysis.UI.Models.CompanySubject companySubject { get; set; }
        public List<WorkPlan> WorkPlanData { get; set; }
    }
}
