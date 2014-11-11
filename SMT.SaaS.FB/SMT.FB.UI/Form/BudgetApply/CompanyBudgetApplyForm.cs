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
using SMT.FB.UI.Common;
using SMT.FB.UI.Common.Controls;
using SMT.FB.UI.FBCommonWS;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI.AuditControl;

namespace SMT.FB.UI.Form.BudgetApply
{
    /// <summary>
    /// 新版年度预算 beyond
    /// </summary>
    public class CompanyBudgetApplyForm : FBPage
    {
        FBEntityService fbService;

        public CompanyBudgetApplyForm(OrderEntity orderEntity)
            : base(orderEntity)
        {
            fbService = new FBEntityService();
            fbService.QueryFBEntitiesCompleted += new EventHandler<QueryFBEntitiesCompletedEventArgs>(fbService_QueryFBEntitiesCompleted);
            this.EditForm.Saving += new EventHandler<SavingEventArgs>(EditForm_Saving);

        }
        

        void EditForm_Saving(object sender, SavingEventArgs e)
        {
            
            ObservableCollection<FBEntity> details = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_COMPANYBUDGETAPPLYDETAIL).Name);

            if (details.Count == 0)
            {
                e.Action = Actions.Cancel;
                CommonFunction.ShowErrorMessage(ErrorMessage.NoDetailInfo);
                return;
            }

            List<string> msgs = new List<string>();

            details.ToList().ForEach(item =>
                {
                    T_FB_COMPANYBUDGETAPPLYDETAIL detail = item.Entity as T_FB_COMPANYBUDGETAPPLYDETAIL;
                    if (detail.BUDGETMONEY < 0)
                    {
                        string errorMessage = string.Format(ErrorMessage.BudgetMoneyZero, detail.T_FB_SUBJECT.SUBJECTNAME);
                        msgs.Add(errorMessage);
                       
                    }
                    //if (detail.USABLEMONEY.LessThan(detail.BUDGETMONEY))
                    //{
                    //    msgs.Add(string.Format(ErrorMessage.BudgetMoneyBigger, detail.T_FB_SUBJECT.SUBJECTNAME));
                    //}
                    
                });
            if (msgs.Count > 0)
            {
                e.Action = Actions.Cancel;
                CommonFunction.ShowErrorMessage(msgs);
            }
            
        }

      
        void fbService_QueryFBEntitiesCompleted(object sender, QueryFBEntitiesCompletedEventArgs e)
        {

            if (this.EditForm.OperationType == OperationTypes.Add)
            {
                DoForAdd(e);
            }
            else
            {
                DoForEdit(e);
            }

            SetPropertyChanged();
        }

        /// <summary>
        /// 添加时操作
        /// </summary>
        /// <param name="e"></param>
        private void DoForAdd(QueryFBEntitiesCompletedEventArgs e)
        {
            // 清除预算明细
            this.OrderEntity.GetRelationFBEntities(typeof(T_FB_COMPANYBUDGETAPPLYDETAIL).Name).Clear();

            // 添加预算明细
            e.Result.ToList().ForEach(item =>
            {
                T_FB_COMPANYBUDGETAPPLYDETAIL ComDetail = (item.Entity as T_FB_COMPANYBUDGETAPPLYDETAIL);
                ComDetail.T_FB_COMPANYBUDGETAPPLYMASTER = this.OrderEntity.Entity as T_FB_COMPANYBUDGETAPPLYMASTER;
                ComDetail.CREATEDATE = DateTime.Now;
                ComDetail.CREATEUSERID = this.OrderEntity.LoginUser.Value.ToString();
                ComDetail.UPDATEDATE = DateTime.Now;
                ComDetail.UPDATEUSERID = this.OrderEntity.LoginUser.Value.ToString(); ;
                item.FBEntityState = FBEntityState.Added;
            });
            this.OrderEntity.FBEntity.AddFBEntities<T_FB_COMPANYBUDGETAPPLYDETAIL>(e.Result);

            this.OrderEntity.FBEntity.Entity.SetObjValue("BUDGETMONEY", 0);
        }

        /// <summary>
        /// 编辑时操作
        /// </summary>
        /// <param name="e"></param>
        private void DoForEdit(QueryFBEntitiesCompletedEventArgs e)
        {

            // 清除预算明细
            var details = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_COMPANYBUDGETAPPLYDETAIL).Name);
        
            var detailsTemp = details.ToList();

            var resultsTemp = e.Result.ToList();
        
            details.CompareFBEntity(e.Result,
                (item, itemR) => item.CompareFBEntity<T_FB_COMPANYBUDGETAPPLYDETAIL>(itemR, entity => entity.T_FB_SUBJECT.SUBJECTID),
                (item, find) =>  /*找到的处理*/
                {
                    var itemEntity = item.Entity as T_FB_COMPANYBUDGETAPPLYDETAIL;
                    var findEntity = find.Entity as T_FB_COMPANYBUDGETAPPLYDETAIL;
                    itemEntity.LASTBUDGETMONEY = findEntity.LASTBUDGETMONEY;                 
                },
                (item) => /*需要删除的item*/
                {
                    details.Remove(item);
                },

                (itemR) => /*需要新的item*/
                {
                    var ComDetail = itemR.Entity as T_FB_COMPANYBUDGETAPPLYDETAIL;
                    ComDetail.T_FB_COMPANYBUDGETAPPLYMASTER = this.OrderEntity.Entity as T_FB_COMPANYBUDGETAPPLYMASTER;
                    ComDetail.CREATEDATE = DateTime.Now;
                    ComDetail.CREATEUSERID = this.OrderEntity.LoginUser.Value.ToString();
                    ComDetail.UPDATEDATE = DateTime.Now;
                    ComDetail.UPDATEUSERID = this.OrderEntity.LoginUser.Value.ToString(); ;
                    itemR.FBEntityState = FBEntityState.Added;

                    details.Add(itemR);
                }
            );
            RefreshBudgetData();
        }



        private void InitData()
        {
            //string companyID = Convert.ToString(OrderEntity.GetValue("Entity.OWNERCOMPANYID"));
            //curCompany = DataCore.CompanyList.FirstOrDefault(company => { return company.ID == companyID; });
            if (this.OrderEntity.FBEntityState == FBEntityState.Added)
            {

                EmployeerData create = this.OrderEntity.GetCreateInfo();
                this.OrderEntity.SetObjValue(EntityFieldName.OwnerID, create.Value);
                this.OrderEntity.SetObjValue(EntityFieldName.OwnerName, create.Text);
                this.OrderEntity.SetObjValue(EntityFieldName.OwnerPostID, create.Post.Value);

                OrderEntity.SetObjValue("Entity.ISVALID", "0");

                // OrderEntity.SetObjValue("Entity.BUDGETYEAR", DataCore.SystemDateTime.Year);
                OrderEntity.ReferencedData["Entity.BUDGETYEAR"] = DataCore.FindReferencedData<YearItem>(DataCore.SystemDateTime.Year);
                
            }
            else if (this.EditForm.OperationType == OperationTypes.Browse)
            {
                
                SetPropertyChanged();
                return;
            }
            GetOrderDetail();


         
        }

        
        protected override void OnLoadDataComplete()
        {
            InitData();
        }

        protected override void OnLoadControlComplete()
        {
            this.OrderEntity.OrderPropertyChanged += new EventHandler<OrderPropertyChangedArgs>(OrderEntity_OrderPropertyChanged);
            if (this.EditForm.OperationType != OperationTypes.Browse)
            {
                this.ShowProcess(true);
                this.EditForm.BindingData(new OrderEntity(new T_FB_DEPTBUDGETAPPLYMASTER()));
            }
        }

        void OrderEntity_OrderPropertyChanged(object sender, OrderPropertyChangedArgs e)
        {
            if ( e.Result.Contains(EntityFieldName.OwnerDepartmentID))
            {
                 GetOrderDetail();
                 ChangeCreator();
                 ChangeOwnerID();
            }
        }

        void DeatilEntity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BUDGETMONEY")
            {
                RefreshBudgetData();
            }
        }
        
        private void GetOrderDetail()
        {
            this.ShowProcess(true);
            T_FB_COMPANYBUDGETAPPLYMASTER master = this.OrderEntity.Entity as T_FB_COMPANYBUDGETAPPLYMASTER;

            QueryExpression qeYear = QueryExpressionHelper.Equal("BUDGETYEAR", master.BUDGETYEAR.Add(-1).ToString());
            qeYear.QueryType = typeof(T_FB_COMPANYBUDGETAPPLYDETAIL).Name;           

            // 预算部门
            QueryExpression qeDept = QueryExpressionHelper.Equal("OWNERDEPARTMENTID", this.OrderEntity.GetObjValue("Entity.OWNERDEPARTMENTID").ToString());
            qeDept.QueryType = typeof(T_FB_COMPANYBUDGETAPPLYDETAIL).Name;
            qeDept.RelatedExpression = qeYear;

            fbService.QueryFBEntities(qeDept);
        }

        protected override void OnAuditing(object sender, SMT.SaaS.FrameworkUI.AuditControl.AuditEventArgs e)
        {
            if (e.Result == AuditEventArgs.AuditResult.Auditing)
            {
                var DeptDetails = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETADDDETAIL).Name);
                DeptDetails.ToList().ForEach(deptDetail =>
                {
                    var pDetails = deptDetail.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETAPPLYDETAIL).Name);
                    pDetails.ToList().ForEach(item =>
                    {
                        item.SetObjValue(typeof(T_FB_COMPANYBUDGETAPPLYMASTER).Name.ToEntityString(), this.OrderEntity.Entity);
                        item.FBEntityState = FBEntityState.Modified;
                    });
                });
            }
        }

        private void SetPropertyChanged()
        {
            ObservableCollection<FBEntity> detailList = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_COMPANYBUDGETAPPLYDETAIL).Name);
            detailList.ToList().ForEach(item =>
            {
                item.Entity.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(DeatilEntity_PropertyChanged);
            });

            this.EditForm.BindingData();
            this.CloseProcess(false);
        }

        /// <summary>
        /// 统计总预算
        /// </summary>
        private void RefreshBudgetData()
        {
            var details = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_COMPANYBUDGETAPPLYDETAIL).Name);

            foreach (var item in details)
            {
                var q = (T_FB_COMPANYBUDGETMODDETAIL)item.Entity;
                if (string.IsNullOrEmpty(q.CREATEUSERID))
                {
                    MessageBox.Show("T_FB_COMPANYBUDGETAPPLYDETAIL CREATEUSERID == null");
                }
            }
            
            var total = details.Sum(itemFB => (itemFB.Entity as T_FB_COMPANYBUDGETAPPLYDETAIL).BUDGETMONEY);

            (this.OrderEntity.Entity as T_FB_COMPANYBUDGETAPPLYMASTER).BUDGETMONEY = total;
        }

    }
}
