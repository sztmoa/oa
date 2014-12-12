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
using SMT.FBAnalysis.UI.Form.SubjectManagement;

namespace SMT.FBAnalysis.UI.Form.Tips
{
    public partial class TipWindow : UserControl
    {
        EditDepartmentYearPlan eyp = new EditDepartmentYearPlan();

        public TipWindow()
        {
            InitializeComponent();
            this.SubjectData.ItemsSource = eyp.SubjectData();
        }

        public void BindControl(HyperlinkButton uc)
        {
            uc.MouseLeave += new MouseEventHandler(ti_MouseLeave);
            uc.MouseMove += new MouseEventHandler(ti_MouseMove);

        }
        public void CancelBindControl(Rectangle uc)
        {
            uc.MouseLeave -= new MouseEventHandler(ti_MouseLeave);
            uc.MouseMove -= new MouseEventHandler(ti_MouseMove);

        }
        /// <summary>
        /// 鼠标离开自定义控件时，隐藏提示框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ti_MouseLeave(object sender, MouseEventArgs e)
        {
            this.tip.IsOpen = false;

        }
        /// <summary>
        /// 鼠标在自定义控件上移动式时，提示框也跟随移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ti_MouseMove(object sender, MouseEventArgs e)
        {
            this.tip.IsOpen = true;
            this.tip.HorizontalOffset = e.GetPosition(null).X - 158;
            this.tip.VerticalOffset = e.GetPosition(null).Y - 202;

        }
    }
}
