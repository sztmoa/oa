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
    public class StandingBookCondition
    {
        /// <summary>
        /// 一级机构。
        /// </summary>
        public string FirstOrg { get; set; }

        /// <summary>
        /// 二级机构。
        /// </summary>
        public string SencondOrg { get; set; }

        /// <summary>
        /// 三机机构。
        /// </summary>
        public string ThirdOrg { get; set; }

        /// <summary>
        /// 项目。
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// 起止时间－开始。
        /// </summary>
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// 起止时间－结束。
        /// </summary>
        public DateTime DateTo { get; set; }

        /// <summary>
        /// 查询个人。
        /// </summary>
        public bool Personal { get; set; }
    }
}
