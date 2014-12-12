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

namespace SMT.FBAnalysis.UI.Form.ReportModels
{
    public class V_TRAVELEXPENSES
    {
        public string TAddress { get; set; }
        public int TDays { get; set; }
        public double TMAirPlane { get; set; }
        public double TMBus { get; set; }
        public double TMLodging { get; set; }
        public double TMEvectionSubsidy { get; set; }
        public double TMSaveSbsidy { get; set; }
        public double TMOther { get; set; }
        public int TSMonth { get; set; }
        public int TSDay { get; set; }
    }
}
