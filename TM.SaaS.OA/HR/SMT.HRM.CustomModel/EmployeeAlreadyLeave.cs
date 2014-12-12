/*
 *作用：用于显示员工可用加班与已用调休
 *作者：梁杰文
 *时间：2014/7/30 16:30
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.HRM.CustomModel
{
    // s.RECORDID,
    //s.EMPLOYEENAME,
    //s.EMPLOYEECODE,
    //s.HOURS,
    //s.EFFICDATE.HasValue?s.EFFICDATE.Value.ToString("yyyy-MM-dd"):"",
    //s.TERMINATEDATE.HasValue?s.TERMINATEDATE.Value.ToString("yyyy-MM-dd"):"",
    //s.LEFTHOURS
    public class EmployeeAlreadyLeave
    {
        public string RECORDID { get;set;}//调休记录ID
        public string OVERTIMERECORDID { get; set; }//加班id
        public string EMPLOYEEID { get; set; }//调休人员ID
        public string EMPLOYEENAME { get; set; }//调休人员姓名
        public string EMPLOYEECODE { get; set; }
        public decimal? HOURS { get; set; }//加班总小时数
        public DateTime? EFFICDATE { get; set; }//加班有效起始时间
        public DateTime? TERMINATEDATE { get; set; }//加班有效截止时间
        public decimal? LEFTHOURS { get; set; }//加班为：可用小时数，调休为：已用休数

    }
}
