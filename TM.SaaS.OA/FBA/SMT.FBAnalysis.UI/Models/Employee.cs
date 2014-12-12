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
    public class Employee
    {
        /// <summary>
        /// 雇员编号。
        /// </summary>
        public string EmpID
        {
            get;
            set;
        }

        /// <summary>
        /// 雇员姓名。
        /// </summary>
        public string EmpName
        {
            get;
            set;
        }
    }
}
