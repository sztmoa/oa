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

namespace SMT.FBAnalysis.UI.Models
{
    public class ExecutionCondition
    {
        /// <summary>
        /// 机构 ID。
        /// </summary>
        public string OrgnizationID { get; set; }

        /// <summary>
        /// 机构名称。
        /// </summary>
        public string OrgnizationName { get; set; }

        /// <summary>
        /// 项目（科目） ID。
        /// </summary>
        public string SubjectID { get; set; }

        /// <summary>
        /// 项目（科目）名称。
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 开始时间。
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// 开始时间。
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// 查询个人。
        /// </summary>
        public bool Personal { get; set; }
    }
}
