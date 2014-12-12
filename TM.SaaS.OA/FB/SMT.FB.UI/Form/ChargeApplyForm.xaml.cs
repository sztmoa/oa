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

namespace SMT.FB.UI.Form
{
    public partial class ChargeApplyForm : ChildWindow
    {
        public ChargeApplyForm()
        {
            InitializeComponent();
        }

        public ChargeApplyForm(OrderEntity order ) : this()
        {

            GridItem dgi1 = new GridItem();
            GridItem dgi2 = new GridItem();
            GridItem dgi3 = new GridItem();
            GridItem dgi4 = new GridItem();
            GridItem dgi5 = new GridItem();

            AGrid.Items.Add(dgi1);
            AGrid.Items.Add(dgi2);
            AGrid.Items.Add(dgi3);
            AGrid.Items.Add(dgi4);
            AGrid.Items.Add(dgi5);

            dgi1.PropertyDisplayName = "单据编号";
            dgi1.PropertyName = "OrderCode";
            dgi1.Width = 75;

            dgi2.PropertyDisplayName = "单据类型";
            dgi2.PropertyName = "OrderType";
            dgi2.Width = 75;

            dgi3.PropertyDisplayName = "状态";
            dgi3.PropertyName = "OrderStates";
            dgi3.Width = 75;

            dgi4.PropertyDisplayName = "申请人";
            dgi4.PropertyName = "Applicant";
            dgi4.Width = 75;

            dgi5.PropertyDisplayName = "申请部门";
            dgi5.PropertyName = "AppliedDepartMent";
            dgi5.Width = 75;
            AGrid.InitGrid();

        }
        //public ChargeApplyForm(TestData.OrderType orderType)
        //    : this()
        //{
            
        //    if (orderType == TestData.OrderType.ChargeApply)
        //    {
        //        this.Title = "费用申请单";
        //    }
        //    else
        //    {
        //        this.Title = "差旅报销单";
        //    }
            
            
        //}

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

