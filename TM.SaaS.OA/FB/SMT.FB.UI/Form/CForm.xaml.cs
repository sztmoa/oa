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
    public partial class CForm : ChildWindow
    {

        private enum OperationTypes { New, Edit }
        private OrderEntity orderEntity;
        private OperationTypes OperationType;
        public IOrderSource OrderSource { set; get; }
        public CForm(OrderEntity source)
        {
            orderEntity = source;

            Type type = typeof(OrderSource<>).MakeGenericType(new Type[] { source.OrderType });
            OrderSource = Activator.CreateInstance(type) as IOrderSource;

            if (string.IsNullOrEmpty(source.OrderID))
            {
                OperationType = OperationTypes.New;
                this.GridToolBar_Delete.Visibility = Visibility.Collapsed;
            }
            else
            {
                OperationType = OperationTypes.Edit;
            }
            InitializeComponent();

            
            List<string[]> FInfo = new List<string[]>();
            FInfo.Add(new string[]{"单据编号", "OrderCode", "OrderCode", "0", "0"});
            FInfo.Add(new string[] { "状态", "OrderStates", "OrderStates", "0", "0" });
            FInfo.Add(new string[] { "创建人", "CreateUser", "CreateUser", "0", "0" });
            FInfo.Add(new string[] { "创建时间", "CreateDate", "CreateDate", "0", "0" });
            this.GForm.LeftFForm.Items = GetFieldItems(FInfo);
            this.GForm.LeftFForm.InitForm();

            FInfo.Clear();
            FInfo.Add(new string[] { "单据类型", "OrderType", "OrderType", "0", "0" });
            FInfo.Add(new string[] { "申请人", "Applicant", "Applicant", "3", "1" });
            FInfo.Add(new string[] { "申请部门", "AppliedDepartMent", "AppliedDepartMentData", "2", "1" });
            FInfo.Add(new string[] { "申请公司", "Company", "CompanyData", "2", "1" });
            this.GForm.RightForm.Items = GetFieldItems(FInfo);
            this.GForm.RightForm.InitForm();

            FInfo.Clear();
            FInfo.Add(new string[] { "备注", "Remark", "Remark", "4", "0" });
            this.GFormRemark.Items = GetFieldItems(FInfo);
            this.GFormRemark.InitForm();

            FInfo.Clear();
            FInfo.Add(new string[] { "编号", "SerialNumber", "75", "1" });
            FInfo.Add(new string[] { "预算项目", "ObjectName", "200", "1" });
            FInfo.Add(new string[] { "可用金额", "UsableMoney", "100", "1" });
            FInfo.Add(new string[] { "已用金额", "UsedMoney", "100", "0" });
            FInfo.Add(new string[] { "费用类型", "ChargeType", "100", "1" });
            this.AGrid.Items= GetDataGridItem(FInfo);
            this.AGrid.InitGrid();

            BindingData();
        }

        private List<DataFieldItem> GetFieldItems(List<string[]> ItemInfo)
        {
            List<DataFieldItem> items = new List<DataFieldItem>();
            for (int i = 0; i < ItemInfo.Count; i++)
            {
                DataFieldItem df = new DataFieldItem();
                df.PropertyDisplayName = ItemInfo[i][0];
                df.PropertyName = ItemInfo[i][1];
                df.ValueMember = ItemInfo[i][2];
                df.CType = (DataFieldItem.ControlType)(int.Parse(ItemInfo[i][3]));
                df.Requited = Convert.ToBoolean(int.Parse(ItemInfo[i][4]));
                items.Add(df);
            }
            return items;
        }

        private List<GridItem> GetDataGridItem(List<string[]> ItemInfo)
        {


            List<GridItem> items = new List<GridItem>();
            for (int i = 0; i < ItemInfo.Count; i++)
            {
                GridItem dgi = new GridItem();
                dgi.PropertyDisplayName = ItemInfo[i][0];
                dgi.PropertyName = ItemInfo[i][1];
                dgi.Width = float.Parse(ItemInfo[i][2]);
                dgi.IsReadOnly = Convert.ToBoolean(int.Parse(ItemInfo[i][3]));
                items.Add(dgi);
            }
            return items;
        }

        private void Save()
        {
            if (this.OperationType == OperationTypes.Edit)
            {
                this.OrderSource.Update(this.orderEntity);
            }
            else
            {
                this.OrderSource.Add(this.orderEntity);
            }
        }

        private void BindingData()
        {
            ComboBox cbbDept = (ComboBox)this.GForm.RightForm.GetFieldControl("AppliedDepartMent");
            ComboBox cbbCom = (ComboBox)this.GForm.RightForm.GetFieldControl("Company");

            CommonFunction.FillComboBox<DepartmentData>(cbbDept);
            CommonFunction.FillComboBox<CompanyData>(cbbCom);


            this.GForm.LeftFForm.DataContext = orderEntity;
            this.GForm.RightForm.DataContext = orderEntity;

            cbbDept.SelectionChanged += new SelectionChangedEventHandler(cbbDept_SelectionChanged);

        }

        void cbbDept_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbbDept = (ComboBox)sender;
            MessageBox.Show(cbbDept.SelectedItem.ToString());
        }
        
        private void GridToolBar_Delete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CommonFunction.AskDelete(null))
            {
                OrderSource.Delete(this.orderEntity);
                this.Close();

            }
        }

        private void GridToolBar_Save_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Save();
        }

        private void SaveAndClose_Click(object sender, MouseButtonEventArgs e)
        {
            Save();
            this.DialogResult = true;
        }

    }
}

