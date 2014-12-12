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
using System.Reflection;
using SMT.FB.UI.FBCommonWS;

namespace SMT.FB.UI.Form
{
    public partial class CommonForm : ChildWindow
    {
        public CommonForm()
        {
            InitializeComponent();
        }

        private enum OperationTypes { New, Edit }
        private OrderEntity orderEntity;
        public IOrderSource OrderSource { set; get; }


        public CommonForm(OrderEntity orderEntity)
            : this()
        {
            this.orderEntity = orderEntity;
            OrderSource = CommonFunction.GetOrderSource(orderEntity.Entity.GetType());
            Order order = ControlGenerator.GetOrder(orderEntity.OrderType);
            InitData(order);
            InitControl(order);
        }

        private void InitData(Order order)
        {


            
            if (this.orderEntity.OrderStatus == EntityAdapter.EditStatus.New)
            {
                string typeName = orderEntity.OrderTypeName;
                this.orderEntity = new OrderEntity(orderEntity.OrderType);
                this.orderEntity.OrderTypeName = typeName;


                EmplyeerData itemOwner = DataCore.CurrentUser.CurrentPostInfo.Copy();
                this.orderEntity.ReferencedData.Dictionary = order.ReferenceDataInfoList.Dictionary;
                this.orderEntity.ReferencedData["Entity.OWNERID"] = itemOwner;
                this.orderEntity.ReferencedData["Entity.OWNERDEPARTMENTID"] = itemOwner.DepartMent;
                this.orderEntity.ReferencedData["Entity.OWNERCOMPANYID"] = itemOwner.Company;


                this.orderEntity.SetValue("Entity.CREATEUSERID", DataCore.CurrentUser.UserID);
                this.orderEntity.SetValue("Entity.UPDATEUSERID", DataCore.CurrentUser.UserID);
                this.orderEntity.SetValue("Entity.CREATEDATE", DateTime.Now);
                this.orderEntity.SetValue("Entity.CREATEDEPARTMENTID", DataCore.CurrentUser.CurrentPostInfo.DepartMent.Value);
                this.orderEntity.SetValue("Entity.CREATECOMPANYID", DataCore.CurrentUser.CurrentPostInfo.Company.Value);

                this.orderEntity.SetValue("Entity.CREATEDEPARTMENTNAME", DataCore.CurrentUser.CurrentPostInfo.DepartMent.Text);
                this.orderEntity.SetValue("Entity.CREATECOMPANYNAME", DataCore.CurrentUser.CurrentPostInfo.Company.Text);

                //暂时
                this.orderEntity.SetValue("Entity.OWNERPOSTID", DataCore.CurrentUser.CurrentPostInfo.Post.Value);
                this.orderEntity.SetValue("Entity.CREATEPOSTID", DataCore.CurrentUser.CurrentPostInfo.Post.Value);

                this.orderEntity.SetValue("Entity.CREATEPOSTNAME", DataCore.CurrentUser.CurrentPostInfo.Post.Text);
                this.orderEntity.SetValue("Entity.OWNERPOSTNAME", DataCore.CurrentUser.CurrentPostInfo.Post.Text);

            }
            else if (this.orderEntity.OrderStatus == EntityAdapter.EditStatus.Edit)
            {
                QueryExpression q = new QueryExpression();
                q.PropertyName = orderEntity.Entity.EntityKey.EntityContainerName;
                q.PropertyValue = orderEntity.OrderID;
                q.Operation = QueryExpression.Operations.Equal;
                //orderEntity.Entity.EntityKey.EntityContainerName
                this.orderEntity = OrderSource.GetOrder(q)[0];

                ObjectList<ReferencedDataInfo> list = order.ReferenceDataInfoList;
                for (int i = 0; i < order.ReferenceDataInfoList.Count; i++)
                {
                    object key = this.orderEntity.GetValue(list[i].PropertyName);
                    ITextValueItem item = null;
                    if (key != null)
                    {
                        item = DataCore.FindRefData(list[i].ReferencedType, key.ToString());
                    }
                    orderEntity.ReferencedData.Add(list[i].PropertyName, item);
                }

            }
        }
        private void InitControl(Order order)
        {
            List<IControlAction> listC = new List<IControlAction>();
            OrderForm f = order.Form;

            for (int i = 0; i < f.Panels.Count; i++)
            {
                listC.Add(f.Panels[i].GetUIElement());
            }



            for (int i = 0; i < listC.Count; i++)
            {
                RowDefinition r = new RowDefinition();
                r.Height = new GridLength(30, GridUnitType.Auto);
                this.MainGrid.RowDefinitions.Add(r);

                UIElement ue = listC[i] as UIElement;
                ue.SetValue(Grid.RowProperty, i);

                this.MainGrid.Children.Add(ue);

                listC[i].InitControl();
                listC[i].DataContext = this.orderEntity;
            }
        }
        private bool Save()
        {
            switch (this.orderEntity.OrderStatus)
            {
                case EntityAdapter.EditStatus.New :
                    this.OrderSource.Add(this.orderEntity);
                    break;
                case EntityAdapter.EditStatus.Edit:
                    this.OrderSource.Update(this.orderEntity);
                    break;
                case EntityAdapter.EditStatus.Delete:
                    this.OrderSource.Delete(this.orderEntity);
                    break;
                default :
                    return false;
            }
            return true;
        }


        private void GridToolBar_Delete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CommonFunction.AskDelete(null))
            {
                this.orderEntity.OrderStatus = EntityAdapter.EditStatus.Delete;

                if (Save())
                {
                    this.DialogResult = true;
                }
            }
        }

        private void GridToolBar_Save_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Save();
        }

        private void SaveAndClose_Click(object sender, MouseButtonEventArgs e)
        {
            if (Save())
            {
                this.DialogResult = true;
            }
            
        }
    }
}

