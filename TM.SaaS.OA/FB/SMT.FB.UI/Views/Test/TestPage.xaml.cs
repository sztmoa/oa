using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SMT.FB.UI.Common;

namespace SMT.FB.UI.Views.Test
{
    public partial class Test : UserControl
    {
        public Test()
        {
            InitializeComponent();
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
           // OrderHelper.InitOrderInfo();
            string orderID = "c1c6f3ef-d118-4d92-873f-7163d8ab0153";
            string modelCode = "T_FB_DEPTBUDGETADDMASTER";
            string formType = "Audit";
            CommonFunction.ShowAuditForm(orderID, modelCode, formType);
        }

      

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            string orderID = this.tbID.Text;
            string modelCode = this.tbOrderType.Text;
            string formType = this.tbOpType.Text;
            CommonFunction.ShowAuditForm(orderID, modelCode, formType);
        }
    }
}
