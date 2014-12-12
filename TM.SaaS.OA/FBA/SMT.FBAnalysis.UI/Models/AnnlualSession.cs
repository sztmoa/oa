using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace SMT.FBAnalysis.UI.Models
{
    public class 年度战略会议
    {
        public string 编号 { get; set; }
        public string 所属公司 { get; set; }
        public string 年度 { get; set; }
        public string 审核状态 { get; set; }
        public string 备注 { get; set; }
    }

    public class 年度战略会议项目
    {
        public List<年度战略会议项目明细> 年度战略会议项目明细 { get; set; }
        public List<年度战略计划费用情况> 年度战略计划费用情况 { get; set; }
        public List<List<战略会议明细图>> 战略会议明细图 { get; set; }
        public List<List<本年工作计划>> 本年工作计划 { get; set; }

        public string 编号 { get; set; }
        public string 指标名称 { get; set; }
        public double 上年计划 { get; set; }
        public double 上年完成 { get; set; }
        public string 完成率 { get; set; }
        public double 本年计划 { get; set; }
        public string 增长率 { get; set; }
    }

    public class 年度战略会议项目明细
    {
        public string 编号 { get; set; }
        public string 会议项目编号 { get; set; }
        public string 部门 { get; set; }
        public double 上年计划 { get; set; }
        public double 上年完成 { get; set; }
        public string 完成率 { get; set; }
        public double 本年计划 { get; set; }
        public string 增长率 { get; set; }
    }

    public class 年度战略计划费用情况
    {
        public string 费用名称 { get; set; }
        public string 上年计划 { get; set; }
        public string 上年开销 { get; set; }
        public string 完成率 { get; set; }
        public string 本年计划 { get; set; }
        public string 增长率 { get; set; }
    }

    public class 本年工作计划
    {
        public List<部门计划> 部门计划 { get; set; }
        public string 事项编号 { get; set; }
        public string 事项名称 { get; set; }
        public string 事项类型 { get; set; }
        public string 事项说明 { get; set; }
        public string 预期开始时间 { get; set; }
        public string 预期结束时间 { get; set; }
        public string 预期目标 { get; set; }
        public string 描述 { get; set; }
    }

    public class 部门计划
    {
        public string 部门 { get; set; }
        public string 事项说明 { get; set; }
        public string 预期开始时间 { get; set; }
        public string 预期结束时间 { get; set; }
        public string 预期目标 { get; set; }
        public string 描述 { get; set; }
    }

    public class 战略会议明细图
    {
    }

    public class 组织架构调整
    {
        public string 编号 { get; set; }
        public string 所属公司 { get; set; }
        public string 年度 { get; set; }
        public string 审核状态 { get; set; }
        public string 备注 { get; set; }
    }

    public enum 经营指标
    {
        销售收入,
        毛利润,
        净利润,
        人均利润,
        人力成本,
        管理费
    }

    public enum 事项
    {
        人力开销,
        电话费,
        招待费,
        固定资产折旧,
        办公费
    }

    public static class EnumExpand
    {
        //public IEnumerable<string> GetValues(this Type enumType)
        //{
        //    //yield 
        //}
    }
}
