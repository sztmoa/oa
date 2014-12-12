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
using SMT.FB.UI.FBCommonWS;
using SMT.FB.UI.Common.Controls;
using System.Collections;


namespace SMT.FB.UI.Form
{
    public partial class OrderFormA : ChildWindow
    {
        public OrderForm()
        {
            InitializeComponent();

            this.GridToolBar_Save.MouseLeftButtonDown += new MouseButtonEventHandler(SetFouceBeforeAction);
            this.GridToolBar_Delete.MouseLeftButtonDown += new MouseButtonEventHandler(SetFouceBeforeAction);
            this.GridToolBar_SaveAndClose.MouseLeftButtonDown += new MouseButtonEventHandler(SetFouceBeforeAction);
            this.GridToolBar_Cancel.MouseLeftButtonDown += new MouseButtonEventHandler(SetFouceBeforeAction);

            this.GridToolBar_Save.MouseLeftButtonUp +=new MouseButtonEventHandler(GridToolBar_Save_MouseLeftButtonUp);
            this.GridToolBar_Delete.MouseLeftButtonUp += new MouseButtonEventHandler(GridToolBar_Delete_MouseLeftButtonUp);
            this.GridToolBar_SaveAndClose.MouseLeftButtonUp += new MouseButtonEventHandler(GridToolBar_SaveAndClose_MouseLeftButtonUp);
            this.GridToolBar_Cancel.MouseLeftButtonUp += new MouseButtonEventHandler(GridToolBar_Cancel_MouseLeftButtonUp);
            this.GridToolBar_Submit.MouseLeftButtonUp += new MouseButtonEventHandler(GridToolBar_Submit_MouseLeftButtonUp);
            this.GridToolBar_Cancel11.MouseLeftButtonDown += new MouseButtonEventHandler(GridToolBar_Cancel11_MouseLeftButtonDown);
            this.GridToolBar_Cancel221.MouseLeftButtonDown += new MouseButtonEventHandler(GridToolBar_Cancel221_MouseLeftButtonDown);
            IsNeedToRefresh = false;
            this.KeyUp += new KeyEventHandler(OrderForm_KeyUp);
            
        }

        void GridToolBar_Cancel221_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.EditForm.TempAuditEntity(CheckStates.UnApproved);
        }

        void GridToolBar_Cancel11_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.EditForm.TempAuditEntity(CheckStates.Approved);
        }

        

        void OrderForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (this.OrderEntity.FBEntityState != EntityState.Unchanged)
                {
                    if (CommonFunction.NotifySaveB4Close(string.Empty))
                    {
                        bool save = this.EditForm.Save();
                        if (save)
                        {
                            this.DialogResult = true;
                        }
                    }

                }
                this.DialogResult = false;
            }
        }

        public OrderForm(OrderEntity orderEntity)
            : this()
        {
            this.editForm.OrderEntity = orderEntity;
            this.Title = orderEntity.OrderTypeName;
            this.EditForm.LoadDataComplete += new EventHandler(editForm_LoadDataComplete);
            this.EditForm.LoadControlComplete += new EventHandler(EditForm_LoadControlComplete);
            this.EditForm.SaveCompleted += new EventHandler(EditForm_SaveCompleted);
            
        }

        void EditForm_SaveCompleted(object sender, EventArgs e)
        {
            if (IsClose)
            {
                this.DialogResult = IsClose;
            }
        }

        void editForm_LoadDataComplete(object sender, EventArgs e)
        {
            if (this.OrderEntity.FBEntityState == EntityState.Added)
            {
                string code = "<自动生成>";
                this.OrderEntity.SetValue("Entity." + this.OrderEntity.CodeName, code);
            }
            OnLoadDataComplete();
        }

        public bool IsNeedToRefresh { get; set; }
        private bool IsClose = false;
        protected virtual void OnLoadDataComplete()
        {
           
        }
        protected virtual void OnLoadControlComplete()
        {
            DetailGrid dGrid = this.EditForm.Controls[4] as DetailGrid;
            if (dGrid != null)
            {
                dGrid.ToolBarItemClick += (sa, ea) =>
                {

                    if (ea.Action == ToolBarItemClickEventArgs.Actions.Add)
                    {
                        SelectedForm sForm = new SelectedForm();
                        sForm.SelectedCompleted += (sb, eb) =>
                        {
                            List<OrderDetailEntity> list = eb.SelectedItems;
                            
                            // dGrid.ItemSource = list;
                        };
                        sForm.Show();
                    }
                };
            }
        }

        private void EditForm_LoadControlComplete(object sender, EventArgs e)
        {
            if (this.OrderEntity.FBEntityState != EntityState.Added)
            {
                ComboBox cbb = this.EditForm.FindControl("CreateUserID") as ComboBox;
                if (cbb != null)
                {
                    cbb.IsEnabled = false;
                }

                cbb = this.EditForm.FindControl("OwnerCompanyID") as ComboBox;
                if (cbb != null)
                {
                    cbb.IsEnabled = false;
                }

                //cbb = this.EditForm.FindControl("OwnerDepartmentID") as ComboBox;
                //if (cbb != null)
                //{
                //    cbb.IsEnabled = false;
                //}
            }

            OnDetaiGridDelete(); // 临时处理方法
            OnLoadControlComplete();
        }

        public OrderEntity OrderEntity
        {
            get
            {
                return editForm.OrderEntity;
            }

        }

        public EditForm EditForm
        {
            get
            {
                return editForm;
            }
        }

        public void InitForm()
        {
            this.editForm.InitForm();
        }

        #region ToolBar

        private void GridToolBar_Delete_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CommonFunction.AskDelete(null))
            {
                this.OrderEntity.FBEntityState = EntityState.Deleted;

                IsNeedToRefresh |= this.EditForm.Save();
                this.IsClose = true;
            }
        }

        private void GridToolBar_Save_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
            IsNeedToRefresh |= this.EditForm.Save();
        }

        private void GridToolBar_SaveAndClose_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsNeedToRefresh |= this.EditForm.Save();
            IsClose = true;
        }

        private void GridToolBar_Cancel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }

        private void GridToolBar_Submit_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsNeedToRefresh |= this.EditForm.AuditEntity();
            
            this.IsClose = true;
        }

        // 不要删除
        private void SetFouceBeforeAction(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
        }
        #endregion

        private void OnDetaiGridDelete()
        {
            DetailGrid dGrid = this.EditForm.Controls[4] as DetailGrid;
            if (dGrid != null)
            {
                dGrid.ToolBarItemClick += (sa, ea) =>
                {
                    if (ea.Action == ToolBarItemClickEventArgs.Actions.Delete)
                    {
                        IList<FBEntity> listSource = dGrid.ItemsSource as IList<FBEntity>;
                        IList list = dGrid.ADGrid.SelectedItems as IList;
                        for (int i = 0; i < list.Count; i++)
                        {
                            FBEntity entity = list[i] as FBEntity;
                            listSource.Remove(entity);
                        }
                    }
                };
            }
        }

    }
}

