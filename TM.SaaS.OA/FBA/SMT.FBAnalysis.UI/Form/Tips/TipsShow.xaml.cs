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
using SMT.FBAnalysis.UI.Common;
using SMT.SaaS.FrameworkUI;
using SMT.FBAnalysis.UI.Form.SubjectManagement;

namespace SMT.FBAnalysis.UI.Form.Tips
{
    public partial class TipsShow : UserControl
    {
        public TipsShow()
        {
            InitializeComponent();
            //this.SubjectData.ItemsSource = new EditDepartmentYearPlan().SubjectData();
            //CreateTooBar();
        }
        //创建工具条
        private void CreateTooBar()
        {

            SMT.SaaS.FrameworkUI.FormToolBar toolbar = new SMT.SaaS.FrameworkUI.FormToolBar();
            //toolbar.Width=
            toolbar.SetValue(Grid.RowProperty, 0);
            toolbar.SetValue(Grid.ColumnProperty, 0);
            SetToolBar(toolbar);
            this.LayoutRoot.Children.Add(toolbar);

        }
        //设置工具条
        private void SetToolBar(SMT.SaaS.FrameworkUI.FormToolBar ToolBar)
        {
            ToolBar.btnRefresh.Visibility = System.Windows.Visibility.Collapsed;
            ToolBar.cbxCheckState.Visibility = System.Windows.Visibility.Collapsed;
            ToolBar.BtnView.Visibility = System.Windows.Visibility.Collapsed;
            ToolBar.btnAudit.Visibility = System.Windows.Visibility.Collapsed;
            ToolBar.txtCheckStateName.Visibility = System.Windows.Visibility.Collapsed;
            ToolBar.btnEdit.Visibility = System.Windows.Visibility.Collapsed;
            ToolBar.btnNew.Visibility = System.Windows.Visibility.Collapsed;
            ToolBar.btnDelete.Visibility = System.Windows.Visibility.Collapsed;


            CreateControl(ToolBar);
        }
        void CreateControl(SMT.SaaS.FrameworkUI.FormToolBar ToolBar)
        {
            ImageButton btnSave = new ImageButton();
            btnSave.AddButtonAction("/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_save.png", Utility.GetResourceStr("保存")).Click += new RoutedEventHandler(btnSave_Click);
            ToolBar.stpOtherAction.Children.Add(btnSave);
        }
        //保存
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
