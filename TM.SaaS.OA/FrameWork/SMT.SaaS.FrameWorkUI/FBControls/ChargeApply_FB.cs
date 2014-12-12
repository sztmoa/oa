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
    public partial class ChargeApplyControl
    {
        FBServiceClient fbService = null;
        FBServiceClient FBServiceLoacal
        {
            get
            {
                if (fbService == null)
                {
                    bool registerResult = WebRequest.RegisterPrefix("http://", WebRequestCreator.ClientHttp);
                    bool httpsResult = WebRequest.RegisterPrefix("https://", WebRequestCreator.ClientHttp);

                    fbService = new FBServiceClient();
                    fbService.SaveEntityCompleted += new EventHandler<SaveEntityCompletedEventArgs>(fbService_SaveEntityCompleted);
                    fbService.QueryFBEntitiesCompleted += new EventHandler<QueryFBEntitiesCompletedEventArgs>(fbService_QueryFBEntitiesCompleted);
                }
                return fbService;
            }
        }

        #region Extensional Method

        /// <summary>
        /// 根据 entityType , 返回对应的有关系的 FBEntity的集合, 
        /// </summary>
        /// <param name="fbEntity"></param>
        /// <param name="entityType">有关系的对象集合类型</param>
        /// <returns>如果不存在,将会创建相应的集合</returns>
        private ObservableCollection<FBEntity> GetRelationFBEntities(FBEntity fbEntity, string entityType)
        {
            ObservableCollection<FBEntity> listFBEntities = null;
            try
            {
                RelationManyEntity rmEntity = fbEntity.CollectionEntity.FirstOrDefault(item =>
                {
                    return item.EntityType == entityType;
                });

                if (rmEntity == null)
                {
                    rmEntity = new RelationManyEntity();
                    rmEntity.EntityType = entityType;
                    rmEntity.FBEntities = new ObservableCollection<FBEntity>();
                    fbEntity.CollectionEntity.Add(rmEntity);
                }
                listFBEntities = rmEntity.FBEntities;
            }
            catch (Exception ex)
            {

            }
            return listFBEntities;
        }

        private FBEntity ToFBEntity(EntityObject t)
        {
            FBEntity fbEntity = new FBEntity();
            fbEntity.Entity = t;
            fbEntity.CollectionEntity = new ObservableCollection<RelationManyEntity>();
            fbEntity.ReferencedEntity = new ObservableCollection<RelationOneEntity>();
            return fbEntity;
        }


        private FBEntity GetModifiedFBEntity(FBEntity fbEntity)
        {
            FBEntity newEntity = ToFBEntity(fbEntity.Entity);
            newEntity.FBEntityState = fbEntity.FBEntityState;

            foreach (RelationManyEntity rme in fbEntity.CollectionEntity)
            {
                RelationManyEntity rmeNew = new RelationManyEntity();
                rmeNew.EntityType = rme.EntityType;
                rmeNew.FBEntities = new ObservableCollection<FBEntity>();
                rme.FBEntities.ToList().ForEach(item =>
                {
                    if (item.FBEntityState != FBEntityState.Unchanged)
                    {
                        if (item.FBEntityState == FBEntityState.Added)
                        {
                            this.ExtensionalOrderFBEntity.CopyTo(item, "Entity.CREATEUSERID");
                        }
                        this.ExtensionalOrderFBEntity.CopyTo(item, "Entity.UPDATEUSERID");

                        rmeNew.FBEntities.Add(item);
                    }
                });

                newEntity.CollectionEntity.Add(rmeNew);
            }

            if (DeleteList.Count > 0)
            {
                RelationManyEntity rmeDel = new RelationManyEntity();
                rmeDel.EntityType = "DELETE_ENTITYTYPE";
                rmeDel.FBEntities = DeleteList;
                newEntity.CollectionEntity.Add(rmeDel);
            }
            return newEntity;
        }


        #endregion

        #region FB组装查询数据表达式
        /// <summary>
        /// 刷新当前可用预算科目
        /// </summary>
        public void RefreshData()
        {
            SelectedDataManager.QueryExpression = GetQueryDetail();
            this.Clear();
        }
        private QueryExpression GetQueryDetail()
        {
            var qeOwnerID = GetQueryE();
            // 查当前用户可用预算科目的条件
            qeOwnerID.QueryType = typeof(T_FB_EXTENSIONORDERDETAIL).Name;
            qeOwnerID.Include = new ObservableCollection<string>() { typeof(T_FB_SUBJECT).Name };
            return qeOwnerID;
        }
        private QueryExpression GetQueryE()
        {
            string ownerID = ExtensionalOrder.OWNERID;
            string postID = ExtensionalOrder.OWNERPOSTID;
            string deptID = ExtensionalOrder.OWNERDEPARTMENTID;
            string companyID = ExtensionalOrder.OWNERCOMPANYID;


            QueryExpression qePost = GetQE("OWNERPOSTID", postID);
            QueryExpression qeDept = GetQE("OWNERDEPARTMENTID", deptID);
            QueryExpression qeOwnerID = GetQE("OWNERID", ownerID);
            QueryExpression qeCom = GetQE("OWNERCOMPANYID", companyID);

            qeOwnerID.RelatedExpression = qePost;
            qePost.RelatedExpression = qeDept;
            qeDept.RelatedExpression = qeCom;

            if (this.OrderType == OrderTypes.Travel)
            {
                var tempQe = qeOwnerID;
                qeOwnerID = GetQE("OrderTypes", "Travel");
                qeOwnerID.RelatedExpression = tempQe;
            }
            return qeOwnerID;
        }
        private QueryExpression GetQE(string propertyName, string propertyValue)
        {
            QueryExpression qe = new QueryExpression();
            qe.Operation = QueryExpression.Operations.Equal;
            qe.PropertyName = propertyName;
            qe.PropertyValue = propertyValue;
            return qe;
        }
        
        private QueryExpression GetOrderQueryExp()
        {
            string orderID = ExtensionalOrder.ORDERID;
            QueryExpression qe = GetQE("ORDERID", orderID);


            qe.RelatedExpression = GetQueryE();

            if (this.TravelSubject != null)
            {
                var tempQe = qe;
                qe = GetQE("TravelSubject", "1");
                qe.RelatedExpression = tempQe;
            }

            QueryExpression qeExtType = GetExtOrderType();
            qeExtType.RelatedExpression = qe;
            qe = qeExtType;

            qe.QueryType = typeof(T_FB_EXTENSIONALORDER).Name;

            return qe;
        }

        private QueryExpression GetExtOrderType()
        {
            QueryExpression qeRes = null;
            if (string.IsNullOrWhiteSpace(strExtOrderModelCode))
            {
                return qeRes;
            }

            qeRes = new QueryExpression();
            qeRes.Operation = QueryExpression.Operations.Equal;
            qeRes.PropertyName = "EXTENSIONALTYPECODE";
            qeRes.PropertyValue = strExtOrderModelCode;
            qeRes.QueryType = typeof(T_FB_EXTENSIONALTYPE).Name;

            return qeRes;
        }
        #endregion

        #region 查询数据完成事件

        void fbService_QueryFBEntitiesCompleted(object sender, QueryFBEntitiesCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    return;
                }

                if (e.Result.Count() == 0)
                {
                    return;
                }

                FBEntity queryEntity = e.Result.FirstOrDefault();
                OnQueryCompleted(queryEntity);
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), e.Error.Message.ToString(), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }

        }
        #endregion

        #region 保存数据完成事件

        void fbService_SaveEntityCompleted(object sender, SaveEntityCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result.FBEntity != null)
                {
                    OnQueryCompleted(e.Result.FBEntity);
                }
                List<string> listResult = new List<string>();
                if (!string.IsNullOrEmpty(e.Result.Exception))
                {
                    listResult.Add(e.Result.Exception);
                }
                OnSaveCompleted(listResult);
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "系统错误，请联系管理员!");
            }
        }


        private void OnSaveCompleted(List<string> list)
        {
            if (SaveCompleted != null)
            {

                SaveCompletedArgs args = new SaveCompletedArgs(null);
                if (list.Count > 0)
                {

                    args = new SaveCompletedArgs(list);
                }

                SaveCompleted(this, args);
            }
        }
        /// <summary>
        /// 查询数据后对控件赋值
        /// </summary>
        private void SetValue()
        {
            T_FB_EXTENSIONALORDER eOrder = this.ExtensionalOrderFBEntity.Entity as T_FB_EXTENSIONALORDER;



            if (ApplyType == ApplyTypes.BorrowApply)
            {
                eOrder.APPLYTYPE = 2;
            }
            else if (ApplyType == ApplyTypes.ChargeApply)
            {
                eOrder.APPLYTYPE = 1;
            }

            if (ChargeApplyMaster != null)
            {
              this.rbtnChargeBorrow.IsChecked = (ChargeApplyMaster != null && ChargeApplyMaster.PAYTYPE == decimal.Parse("2"));
              if(IsFromFB==true)
              { 
                this.rbtnChargePerson.IsChecked = (ChargeApplyMaster != null && ChargeApplyMaster.PAYTYPE == decimal.Parse("1"));
              }
            }
            //this.rbtnApplyTypeBorrow.IsChecked = (eOrder.APPLYTYPE != null && eOrder.APPLYTYPE.Value == decimal.Parse("2"));
            //this.rbtnApplyTypeCharge.IsChecked = (eOrder.APPLYTYPE != null && eOrder.APPLYTYPE.Value == decimal.Parse("1"));

            this.tbRemark.Text = string.IsNullOrEmpty(eOrder.REMARK) ? "" : eOrder.REMARK;

            rbtnChargePerson_Click(null, null);
            //rbtnApplyTypeCharge_Click(null, null);

            // this.tbCount.SetBinding(TextBox.TextProperty, new Binding() { Path = new PropertyPath("TOTALMONEY"), Source = this.Order });
            SetLittleCount();

        }

        #endregion
    }
}
