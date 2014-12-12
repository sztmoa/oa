/*
 *  0. 在初始化数据之前，需要对 Order 赋值
 *  public T_FB_EXTENSIONALORDER Order { get; set; }
 *  
 *  1. 初始化数据
 *  public void InitData(bool readOnly)
 *  1.5 初始化数据后，返回原结果 是一个string 集合。如果为空，就是操作成功，否则就是出错的信息。
 *  public event EventHandler<InitDataCompletedArgs> InitDataComplete;
 *  ...
 *  2. 保存 stataes : 草稿，提交，审核通过，审核不通过
 *  public void Save(CheckStates statates)
 *  2.5 保存后，返回的结果 是一个string 集合。如果为空，就是操作成功，否则就是出错的信息。
 *  public event EventHandler<SaveCompletedArgs> SaveCompleted;
 *  
 */
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

using SMT.Saas.Tools.FBServiceWS;
using System.Windows.Data;
using System.Collections;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Net.Browser;
using System.Collections.Specialized;
using SMT.SaaS.FrameworkUI.Common;
using SMT.SaaS.Globalization;
using SMT.SaaS.FrameworkUI.ChildWidow;

namespace SMT.SaaS.FrameworkUI.FBControls
{
    public partial class ChargeApplyControl : UserControl
    {
        private bool IsFromFB = true;// false 数据源是否来自工作计划

        public bool FromFB
        {
            get { return IsFromFB; }
            set
            {
                IsFromFB = value;
                if (IsFromFB == false)
                {
                    rbtnChargePerson.IsChecked = true;
                    rbtnChargeBorrow.Visibility = Visibility.Collapsed;

                    SetRemarkVisiblity(Visibility.Collapsed);
                    this.gridForBorrowInfo.Visibility = System.Windows.Visibility.Collapsed;                    
                }
            }
        }
        public string strBussinessTripID = string.Empty;
        SelectedForm selectForm = null;
        public SelectedForm SelectForm
        {
            get { return selectForm; }
            set { selectForm = value; }
        }
        public Grid GetPayType
        {
            get
            {
                return this.PayTypeinfo;
            }
            set
            {
                this.PayTypeinfo = value;
            }
        }
        private bool isInit = true;
        private SelectedDataManager selectedDataManager;
        public SelectedDataManager SelectedDataManager
        {
            get
            {
                if (selectedDataManager == null)
                {
                    selectedDataManager = new SelectedDataManager();
                }
                selectedDataManager.IsFromFB = this.IsFromFB;
                selectedDataManager.strBussinessTripID = this.strBussinessTripID;
                return selectedDataManager;
            }
        }
        private FBDataGrid AGrid;
        private FBDataGrid BorrowGrid;
        private FBEntity ExtensionalOrderFBEntity { get; set; }
        private T_FB_CHARGEAPPLYMASTER ChargeApplyMaster { get; set; }
        public ObservableCollection<FBEntity> ExtensionalOrderDetailFBEntityList { get; set; }
        public ObservableCollection<FBEntity> ListExtOrderType { get; set; }
        public ObservableCollection<T_FB_CHARGEAPPLYREPAYDETAIL> ListBorrowDetail { get; set; }
        /// 
        public T_FB_EXTENSIONALORDER ExtensionalOrder { get; set; }

        /// <summary>
        /// 如果是出差申请,可以对该对象赋值.注:需要在调用InitData方法前赋值
        /// e.g
        ///   c.TravelSubject  = new TravelSubject();
        ///   c.InitData()
        ///   ......
        ///   
        ///   c.ApplyMoney = 400
        ///   c.Save()
        /// </summary>
        public TravelSubject TravelSubject { get; set; }

        public ChargeApplyControl()
        {
            try
            {
                InitializeComponent();
                ExtensionalOrder = new T_FB_EXTENSIONALORDER();
                ExtensionalOrder.CHECKSTATES = 0;
                GetPayType.Opacity = 0;
                //InitAllExtOrderType();
                InitApplyType();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region 接口需求

        public void SetRemarkVisiblity(System.Windows.Visibility visibility)
        {
            this.gridRemark.Visibility = visibility;
        }

        public ChargeParameter GetParamenter()
        {
            return new ChargeParameter();
        }


        //public ExtensionalOrderTypes ExtOrderType { get; set; }

        public string strExtOrderModelCode = string.Empty;
        public FormTypes submitFBFormTypes { get; set; }

        public enum ExtensionalOrderTypes
        {
            TravelApplication,
            TravelReimbursement,
            ApprovalForm
        }

        /// <summary>
        /// 对应上预算中的单据类型： 1.费用报销， 2：借款。
        /// 如果是Both，那么类型由用户操作时，决定。
        /// </summary>
        public enum ApplyTypes
        {
            Both,
            ChargeApply,
            BorrowApply

        }

        /// <summary>
        /// 用于出差申请，如果是出差申请，选Travel， 其他的选Charge
        /// 区别在于：可选科目中，Tranvel类型的可以选择差旅费的科目，Charge不可以选择。
        /// </summary>
        public enum OrderTypes
        {
            Travel,
            Charge
        }

        public ApplyTypes _ApplyType = ApplyTypes.Both;
        public ApplyTypes ApplyType
        {
            get
            {
                return _ApplyType;
            }
            set
            {
                _ApplyType = value;
            }


        }

        public OrderTypes _OrderType = OrderTypes.Charge;
        public OrderTypes OrderType
        {
            get
            {
                return _OrderType;
            }
            set
            {
                _OrderType = value;
            }


        }


        private void InitApplyType()
        {
            rbtnChargePerson_Click(null, null);
            //rbtnApplyTypeCharge_Click(null, null);
        }



        string msgBiggerError = Utility.GetResourceStr("DATEGREATERERROR");
        public event EventHandler<SaveCompletedArgs> SaveCompleted;

        public event EventHandler<InitDataCompletedArgs> ItemSelectChange;

        public event EventHandler<InitDataCompletedArgs> InitDataComplete;
        public class SaveCompletedArgs : EventArgs
        {
            private List<string> _Message;
            public List<string> Message
            {
                get
                {
                    return _Message;
                }
            }
            public SaveCompletedArgs(List<string> messages)
            {
                _Message = messages;
            }
        }

        public class InitDataCompletedArgs : SaveCompletedArgs
        {
            public InitDataCompletedArgs(List<string> messages)
                : base(messages)
            {
            }
        }
        #endregion

        #region 初始化数据

        private void QueryDataCompletedInitControl()
        {
            if (isInit)
            {
                this.PayP.Visibility = Visibility.Visible;
                this.ToolBarP.Visibility = Visibility.Visible;
                AGrid = new FBDataGrid();
                this.GridP.Children.Add(AGrid);
                AGrid.GridItems = GetItems();

                AGrid.InitControl(this.GetDelColumn());
                
                AGrid.MinHeight = 80;
                                                
                BorrowGrid = new FBDataGrid();
                this.spForBorrowInfo.Children.Add(BorrowGrid);
                BorrowGrid.GridItems = GetBorrowItems();
                BorrowGrid.InitControl();
                
                BorrowGrid.MinHeight = 60;

                InitToolBar();
                
            }
            AGrid.ItemsSource = this.ExtensionalOrderDetailFBEntityList;
            BorrowGrid.ItemsSource = this.ListBorrowDetail;
            SetValue();
            isInit = false;
            
        }

        private DataGridColumn GetDelColumn()
        {
            DataGridTemplateColumn dgtc = new DataGridTemplateColumn();
            dgtc.CellTemplate = this.Resources["dtDelColumn"] as DataTemplate;
            return dgtc;
        }


        public List<FBDataGrid.FBGridItem> GetItems()
        {
            string xmlDoc = @"<?xml version=""1.0"" encoding=""utf-8"" ?><GridItems>
                <GridItem PropertyDisplayName=""科目名称"" PropertyName=""Entity.T_FB_SUBJECT.SUBJECTNAME"" Width=""150"" IsReadOnly=""true""/>
                <GridItem PropertyDisplayName=""可用结余"" PropertyName=""Entity.USABLEMONEY"" Width=""75"" IsReadOnly=""true""/>
                <GridItem PropertyDisplayName=""摘要"" PropertyName=""Entity.REMARK"" Width=""175"" MaxLength=""200""/>
                <GridItem PropertyDisplayName=""报销金额"" PropertyName=""Entity.APPLIEDMONEY"" Width=""75""/>

            </GridItems>";
            XElement xml = XElement.Parse(xmlDoc);

            XElement xElement = xml;
            List<FBDataGrid.FBGridItem> list = new List<FBDataGrid.FBGridItem>();
            foreach (XElement xeItem in xElement.Elements("GridItem"))
            {
                FBDataGrid.FBGridItem gridItem = new FBDataGrid.FBGridItem();
                Type type = typeof(FBDataGrid.FBGridItem);
                xeItem.Attributes().ForEach(item =>
                {
                    PropertyInfo p = type.GetProperty(item.Name.LocalName);
                    if (p != null)
                    {
                        object v = item.Value.ConvertOrNull(p.PropertyType, null, null, DateTimeStyles.None, null);
                        p.SetValue(gridItem, v, null);
                    }

                });
                list.Add(gridItem);
            }
            return list;

        }


        public List<FBDataGrid.FBGridItem> GetBorrowItems()
        {
            string xmlDoc = @"<?xml version=""1.0"" encoding=""utf-8"" ?><GridItems>
                <GridItem PropertyDisplayName=""借款项目"" PropertyName=""REPAYTYPE"" Width=""100"" IsReadOnly=""true"" ReferenceType=""RepayType""/>
                <GridItem PropertyDisplayName=""借款余额"" PropertyName=""BORROWMONEY"" Width=""75"" IsReadOnly=""true"" />
                <GridItem PropertyDisplayName=""摘要"" PropertyName=""REMARK"" Width=""200"" IsReadOnly=""flase""/>
                <GridItem PropertyDisplayName=""还款金额"" PropertyName=""REPAYMONEY"" Width=""75"" IsReadOnly=""flase""/>
            </GridItems>";
            XElement xml = XElement.Parse(xmlDoc);

            XElement xElement = xml;
            List<FBDataGrid.FBGridItem> list = new List<FBDataGrid.FBGridItem>();
            foreach (XElement xeItem in xElement.Elements("GridItem"))
            {
                FBDataGrid.FBGridItem gridItem = new FBDataGrid.FBGridItem();
                Type type = typeof(FBDataGrid.FBGridItem);
                xeItem.Attributes().ForEach(item =>
                {
                    PropertyInfo p = type.GetProperty(item.Name.LocalName);
                    if (p != null)
                    {
                        object v = item.Value.ConvertOrNull(p.PropertyType, null, null, DateTimeStyles.None, null);
                        p.SetValue(gridItem, v, null);
                    }

                });
                list.Add(gridItem);
            }
            return list;

        }
        private void InitToolBar()
        {
            List<ToolbarItem> listBar = new List<ToolbarItem>();
            ToolbarItem tbItem = ToolBarItems.New;
            tbItem.Title = "选择科目";
            listBar.Add(tbItem);
            // listBar.Add(ToolBarItems.Delete);
            deatilGridBar.InitToolBarItem(listBar);
            deatilGridBar.ShowItem(deatilGridBar.TitleImageName, false);
            //如果要显示图片,这么写。 deatilGridBar.ShowItem("deatilGridBar.TitleImageName", false);
            deatilGridBar.ShowItem(deatilGridBar.HelpButtonName, false);
            deatilGridBar.ItemClicked += new EventHandler<ToolBar.ToolBarItemClickArgs>(deatilGridBar_ItemClicked);
        }

        /// <summary>
        /// 初始化数据绑定
        /// </summary>
        public void InitData()
        {
            QueryTravelSubjectData(false);
        }
        #endregion

        #region 明细 工具栏操作
        void deatilGridBar_ItemClicked(object sender, ToolBar.ToolBarItemClickArgs e)
        {
            switch (e.Key)
            {
                case "New":
                    Add();
                    break;
                case "Delete":
                    IList list = this.AGrid.SelectedItems as IList;
                    Delete(list);
                    break;
            }
        }

        public void Add()
        {
            SelectedForm sf = new SelectedForm();
            sf.SelectedDataManager = this.SelectedDataManager;
            sf.SelectedItems = this.ExtensionalOrderDetailFBEntityList.ToList();
            bool isBigger5 = false;
            sf.SelectedCompleted += (o, e) =>
            {
                
                sf.SelectedItems.ForEach(item =>
                {
                    if (ExtensionalOrderDetailFBEntityList.Count >= 5)
                    {
                        isBigger5 = true;
                        return;
                    }
                    item.FBEntityState = FBEntityState.Added;
                    (item.Entity as T_FB_EXTENSIONORDERDETAIL).T_FB_EXTENSIONALORDER = this.ExtensionalOrderFBEntity.Entity as T_FB_EXTENSIONALORDER;
                    this.ExtensionalOrderDetailFBEntityList.Add(item);
                });
                AGrid.ItemsSource = this.ExtensionalOrderDetailFBEntityList;

                if (isBigger5)
                {
                    MessageWindow.Show<string>("提示", "报销明细项不能超过5条!", MessageIcon.Information,
                    null, "Default", Localization.GetString("OKBUTTON"));
                }

            };

            sf.Show();
            SelectForm = sf;

           
        }

        public void Delete(IList list)
        {
            if (list == null)
            {
                return;
            }

            for (int i = 0; i < list.Count; i++)
            {
                FBEntity entity = list[i] as FBEntity;
                if (entity != null)
                {
                    entity.Actived = false;
                    ExtensionalOrderDetailFBEntityList.Remove(entity);
                }
            }
        }

        //xiedx
        //2012-8-11
        //这个是在出差申请中用到的函数，但是值ListDetail一直是null，所以会出错，现在判断一下
        public void Clear()
        {
            if (this.ExtensionalOrderDetailFBEntityList != null)
            {
                this.ExtensionalOrderDetailFBEntityList.Clear();
            }   
        }
        #endregion

        #region 数据对象，属性值改变处理
        public ObservableCollection<FBEntity> DeleteList = new ObservableCollection<FBEntity>();
        void FBEntities_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // 将删除的实体记录.等待传回service端处理
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                for (int i = 0; i < e.OldItems.Count; i++)
                {
                    FBEntity fbEntity = e.OldItems[i] as FBEntity;
                    if (fbEntity.FBEntityState != FBEntityState.Added)
                    {
                        if (!DeleteList.Contains(fbEntity))
                        {
                            fbEntity.FBEntityState = FBEntityState.Detached;
                            DeleteList.Add(fbEntity);
                        }
                    }
                    UnRegisterFBEntity(fbEntity);
                }
                CountTotalMoney();
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                for (int i = 0; i < e.NewItems.Count; i++)
                {
                    FBEntity fbEntity = e.NewItems[i] as FBEntity;
                    RegisterFBEntity(fbEntity);
                }
            }
            this.OnEntityPropertyChanged(this.ExtensionalOrderFBEntity);

            if (ItemSelectChange != null)
            {
                List<string> messages = new List<string>();
                if (this.TravelSubject != null && this.TravelSubject.SpecialListDetail.Count == 0)
                {
                    messages.Add("科目变动");
                }
                ItemSelectChange(this, new InitDataCompletedArgs(messages));
            }
        }

        private void RegisterFBEntity(FBEntity fbEntity)
        {
            fbEntity.Entity.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Entity_PropertyChanged);
        }
        private void UnRegisterFBEntity(FBEntity fbEntity)
        {
            fbEntity.Entity.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(Entity_PropertyChanged);
        }

        private void OnEntityPropertyChanged(FBEntity fbEntity)
        {
            if (fbEntity.FBEntityState == FBEntityState.Unchanged)
            {
                fbEntity.FBEntityState = FBEntityState.Modified;

            }

        }

        private void Entity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            FBEntity fbEntity = this.ExtensionalOrderDetailFBEntityList.FirstOrDefault(item =>
            {
                return object.Equals(item.Entity, sender);
            });
            OnEntityPropertyChanged(fbEntity);

            if (sender.GetType() == typeof(T_FB_EXTENSIONORDERDETAIL))
            {
                if (e.PropertyName == "APPLIEDMONEY")
                {
                    CountTotalMoney();
                }
            }
            if (ItemSelectChange != null)
            {
                List<string> messages = new List<string>();
                if (this.TravelSubject != null && this.TravelSubject.SpecialListDetail.Count == 0)
                {
                    messages.Add("科目变动");
                }
                ItemSelectChange(this, new InitDataCompletedArgs(messages));
            }
        }

        private void SpecialEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (this.TravelSubject != null)
            {
                FBEntity fbEntity = this.TravelSubject.SpecialListDetail.FirstOrDefault(item =>
                {
                    return object.Equals(item.Entity, sender);
                });
                OnEntityPropertyChanged(fbEntity);

                if (sender.GetType() == typeof(T_FB_EXTENSIONORDERDETAIL))
                {
                    if (e.PropertyName == "APPLIEDMONEY")
                    {
                        CountTotalMoney();
                    }
                }
            }

        }


        #endregion

        /// <summary>
        /// 统计费用总金额
        /// </summary>
        private void CountTotalMoney()
        {
            decimal totalMoney = LittleCount();

            if (this.TravelSubject != null)
            {
                totalMoney += this.TravelSubject.SpecialListDetail.Sum(item =>
                {
                    return (item.Entity as T_FB_EXTENSIONORDERDETAIL).APPLIEDMONEY;
                });
            }

            (this.ExtensionalOrderFBEntity.Entity as T_FB_EXTENSIONALORDER).TOTALMONEY = totalMoney;

            SetLittleCount();
        }

        public decimal totalMoney=0;

        private decimal LittleCount()
        {
            totalMoney = this.ExtensionalOrderDetailFBEntityList.Sum(item =>
            {
                return (item.Entity as T_FB_EXTENSIONORDERDETAIL).APPLIEDMONEY;
            });
            return totalMoney;
        }

        private void SetLittleCount()
        {
            this.tbCount.Text = Convert.ToString(LittleCount());
        }
        private void HandleSpecialSubject()
        {

            if (this.TravelSubject != null)
            {
                this.TravelSubject.SpecialListDetail = GetRelationFBEntities(this.ExtensionalOrderFBEntity, "T_FB_EXTENSIONORDERDETAIL_Travel");

                this.TravelSubject.SpecialListDetail.ForEach(item =>
                {
                    //TravelSubject.subject = (item.Entity as T_FB_EXTENSIONORDERDETAIL).T_FB_SUBJECT;
                    //TravelSubject.UsableMoney = (item.Entity as T_FB_EXTENSIONORDERDETAIL).USABLEMONEY.Value;
                    //TravelSubject.ApplyMoney = TravelSubject.ApplyMoney + (item.Entity as T_FB_EXTENSIONORDERDETAIL).APPLIEDMONEY;

                    item.Entity.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SpecialEntity_PropertyChanged);
                });


            }


        }

        private void rbtnChargePerson_Click(object sender, RoutedEventArgs e)
        {
            if (rbtnChargePerson.IsChecked == true)
            {
                this.gridForBorrowInfo.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.gridForBorrowInfo.Visibility = System.Windows.Visibility.Visible;
            }
        }

        //private void rbtnApplyTypeCharge_Click(object sender, RoutedEventArgs e)
        //{
        //    if (rbtnApplyTypeCharge.IsChecked == true)
        //    {
        //        this.spChargeApplyType.Visibility = System.Windows.Visibility.Visible;
        //    }
        //    else
        //    {
        //        this.spChargeApplyType.Visibility = System.Windows.Visibility.Collapsed;
        //        this.rbtnChargePerson.IsChecked = true;
        //        this.gridForBorrowInfo.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //}

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            
            object obj = sender.GetObjValue("DataContext");
            this.Delete(new List<object>() { obj });
        }
    }

    public class ChargeParameter
    {
        /// <summary>
        /// 当前总费用
        /// </summary>
        public decimal ChargeMoney { get; set; }
        /// <summary>
        /// 当前总借款
        /// </summary>
        public decimal BorrowMoney { get; set; }
        /// <summary>
        /// 已借款
        /// </summary>
        public decimal BorrowedMoney { get; set; }
        /// <summary>
        /// 差旅费
        /// </summary>
        public decimal TravelMoney { get; set; }
        /// <summary>
        /// 招待费
        /// </summary>
        public decimal EntertainmentMoney { get; set; }

    }


    public class TravelSubject
    {

        public T_FB_SUBJECT subject
        {
            get
            {

                if (SpecialListDetail != null)
                {
                    var sd = SpecialListDetail.FirstOrDefault();
                    if (sd != null)
                    {
                        return (sd.Entity as T_FB_EXTENSIONORDERDETAIL).T_FB_SUBJECT;
                    }
                }
                return new T_FB_SUBJECT();
            }
        }

        public decimal ApplyMoney
        {
            get
            {

                if (SpecialListDetail != null)
                {
                    var sd = SpecialListDetail.FirstOrDefault();
                    if (sd != null)
                    {
                        return (sd.Entity as T_FB_EXTENSIONORDERDETAIL).APPLIEDMONEY;
                    }
                }
                return 0;
            }
            set
            {
                if (SpecialListDetail != null)
                {
                    var sd = SpecialListDetail.FirstOrDefault();
                    if (sd != null)
                    {
                        (sd.Entity as T_FB_EXTENSIONORDERDETAIL).APPLIEDMONEY = value;
                    }
                }
            }
        }

        public decimal UsableMoney
        {
            get
            {

                if (SpecialListDetail != null)
                {
                    var sd = SpecialListDetail.FirstOrDefault();
                    if (sd != null)
                    {
                        return (sd.Entity as T_FB_EXTENSIONORDERDETAIL).USABLEMONEY.Value;
                    }
                }
                return 0;
            }
            set
            {
                if (SpecialListDetail != null)
                {
                    var sd = SpecialListDetail.FirstOrDefault();
                    if (sd != null)
                    {
                        (sd.Entity as T_FB_EXTENSIONORDERDETAIL).USABLEMONEY = value;
                    }
                }
            }
        }

        private ObservableCollection<FBEntity> _SpecialListDetail = null;
        public ObservableCollection<FBEntity> SpecialListDetail
        {
            get
            {
                if (_SpecialListDetail == null)
                {
                    _SpecialListDetail = new ObservableCollection<FBEntity>();
                }
                return _SpecialListDetail;
            }
            set
            {
                _SpecialListDetail = value;
            }
        }

    }

}
