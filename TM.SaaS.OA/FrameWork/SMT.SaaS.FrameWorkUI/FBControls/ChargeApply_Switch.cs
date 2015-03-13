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
        #region "费用获取方法"
        public void QueryTravelSubjectData(bool readOnly)
        {

            deatilGridBar.Visibility = readOnly ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

            selectedDataManager = null;

            if (ExtensionalOrderDetailFBEntityList != null)
            {
                this.ExtensionalOrderDetailFBEntityList.Clear();
            }
            if (this.ExtensionalOrderDetailFBEntityList != null)
            {
                this.DeleteList.Clear();
            }

            if (IsFromFB)
            {
                QueryExpression qe = GetOrderQueryExp();
                //查询预算系统数据
                FBServiceLoacal.QueryFBEntitiesAsync(qe);
                SelectedDataManager.QueryExpression = GetQueryDetail();
            }
            else
            {
                if (string.IsNullOrEmpty(strBussinessTripID))
                {
                    MessageBox.Show("未获取到出差记录，请确认！");
                    return;
                }
                //费用来自工作计划
                WpServiceClient.GetTripSubjectAsync(strBussinessTripID);
            }
        }

        private void OnQueryCompleted(FBEntity queryEntity)
        {
            try { 
            if (queryEntity == null) // 新增
            {
                this.ExtensionalOrder.EXTENSIONALORDERID = Guid.NewGuid().ToString();
                this.ExtensionalOrderFBEntity = ToFBEntity(this.ExtensionalOrder);
                this.ExtensionalOrderFBEntity.FBEntityState = FBEntityState.Added;

                this.ExtensionalOrder.APPLYTYPE = 1;
                this.ExtensionalOrder.PAYTARGET = 1;

            }
            else
            {
                this.ExtensionalOrderFBEntity = queryEntity;
                var curEntity = this.ExtensionalOrderFBEntity.Entity as T_FB_EXTENSIONALORDER;
                if (this.ExtensionalOrderFBEntity.FBEntityState == FBEntityState.Added)
                {
                    curEntity.CREATECOMPANYID = this.ExtensionalOrder.CREATECOMPANYID;
                    curEntity.CREATECOMPANYNAME = this.ExtensionalOrder.CREATECOMPANYNAME;
                    curEntity.CREATEDEPARTMENTID = this.ExtensionalOrder.CREATEDEPARTMENTID;
                    curEntity.CREATEDEPARTMENTNAME = this.ExtensionalOrder.CREATEDEPARTMENTNAME;
                    curEntity.CREATEPOSTID = this.ExtensionalOrder.CREATEPOSTID;
                    curEntity.CREATEPOSTNAME = this.ExtensionalOrder.CREATEPOSTNAME;
                    curEntity.CREATEUSERID = this.ExtensionalOrder.CREATEUSERID;
                    curEntity.CREATEUSERNAME = this.ExtensionalOrder.CREATEUSERNAME;


                    curEntity.OWNERCOMPANYID = this.ExtensionalOrder.OWNERCOMPANYID;
                    curEntity.OWNERCOMPANYNAME = this.ExtensionalOrder.OWNERCOMPANYNAME;
                    curEntity.OWNERDEPARTMENTID = this.ExtensionalOrder.OWNERDEPARTMENTID;
                    curEntity.OWNERDEPARTMENTNAME = this.ExtensionalOrder.OWNERDEPARTMENTNAME;
                    curEntity.OWNERPOSTID = this.ExtensionalOrder.OWNERPOSTID;
                    curEntity.OWNERPOSTNAME = this.ExtensionalOrder.OWNERPOSTNAME;
                    curEntity.OWNERID = this.ExtensionalOrder.OWNERID;
                    curEntity.OWNERNAME = this.ExtensionalOrder.OWNERNAME;

                    curEntity.UPDATEUSERID = this.ExtensionalOrder.UPDATEUSERID;
                    curEntity.UPDATEUSERNAME = this.ExtensionalOrder.UPDATEUSERNAME;


                }
                this.ExtensionalOrder = curEntity;
            }

            this.DeleteList = GetRelationFBEntities(this.ExtensionalOrderFBEntity, "_Delete");
            this.ListExtOrderType = GetRelationFBEntities(this.ExtensionalOrderFBEntity, typeof(T_FB_EXTENSIONALTYPE).Name);
            this.ExtensionalOrderDetailFBEntityList = GetRelationFBEntities(this.ExtensionalOrderFBEntity, typeof(T_FB_EXTENSIONORDERDETAIL).Name);
            var masterList = GetRelationFBEntities(this.ExtensionalOrderFBEntity, typeof(T_FB_CHARGEAPPLYMASTER).Name);
            var masterFBEntity = masterList.FirstOrDefault();
            if (masterFBEntity != null)
            {
                ChargeApplyMaster = masterFBEntity.Entity as T_FB_CHARGEAPPLYMASTER;
                this.ListBorrowDetail = ChargeApplyMaster.T_FB_CHARGEAPPLYREPAYDETAIL;

            }
            else
            {
                ChargeApplyMaster = new T_FB_CHARGEAPPLYMASTER();
                this.ListBorrowDetail = new ObservableCollection<T_FB_CHARGEAPPLYREPAYDETAIL>();
            }

            HandleSpecialSubject();
            this.CountTotalMoney();
            this.ExtensionalOrderDetailFBEntityList.CollectionChanged += new NotifyCollectionChangedEventHandler(FBEntities_CollectionChanged);
            this.ExtensionalOrderDetailFBEntityList.ForEach(detail =>
            {
                this.RegisterFBEntity(detail);
            });

            if (ListExtOrderType.Count() > 0)
            {
                (this.ExtensionalOrderFBEntity.Entity as T_FB_EXTENSIONALORDER).T_FB_EXTENSIONALTYPE = ListExtOrderType.FirstOrDefault().Entity as T_FB_EXTENSIONALTYPE;
            }

            QueryDataCompletedInitControl();

            if (InitDataComplete != null)
            {
                List<string> messages = new List<string>();
                if (this.TravelSubject != null && this.TravelSubject.SpecialListDetail.Count == 0)
                {
                    string msg = "当前申请人没有出差的预算额度";
                    if (!IsFromFB) msg = "工作计划出差单:" + msg;
                    messages.Add(msg);
                }
                InitDataComplete(this, new InitDataCompletedArgs(messages));
            }
            }catch(Exception ex)
            {
                MessageBox.Show("工作计划出差单OnQueryCompleted err:" + ex.ToString());
            }
        }
        #endregion

        #region "费用保存方法"
        private List<string> OnSaveCheck()
        {
            List<string> listReslut = new List<string>();
            if (this.ExtensionalOrderDetailFBEntityList != null)
            {
                this.ExtensionalOrderDetailFBEntityList.ForEach(item =>
                {
                    T_FB_EXTENSIONORDERDETAIL detailEntity = item.Entity as T_FB_EXTENSIONORDERDETAIL;

                    if (detailEntity.APPLIEDMONEY > detailEntity.USABLEMONEY.Value)
                    {
                        listReslut.Add(string.Format("科目:{2} 的" + msgBiggerError, "申请金额", "可用金额", detailEntity.T_FB_SUBJECT.SUBJECTNAME));

                    }
                    if (detailEntity.APPLIEDMONEY <= 0)
                    {
                        string errorMessage = string.Format("科目： {0} 的申请金额必须大于零!", detailEntity.T_FB_SUBJECT.SUBJECTNAME);
                        listReslut.Add(errorMessage);

                    }
                    if (string.IsNullOrWhiteSpace(detailEntity.REMARK))
                    {
                        string errorMessage = string.Format("科目： {0} 的摘要不能为空!", detailEntity.T_FB_SUBJECT.SUBJECTNAME);
                        listReslut.Add(errorMessage);
                    }

                });
            }

         
            if (this.TravelSubject != null)
            {
                if (this.TravelSubject.SpecialListDetail.Count == 0)
                {
                    listReslut.Add("当前申请人没有出差的预算额度");
                }
                else if (this.TravelSubject.ApplyMoney > this.TravelSubject.UsableMoney)
                {
                    listReslut.Add(string.Format("科目:{2} 的" + msgBiggerError, "申请金额", "可用金额", TravelSubject.subject.SUBJECTNAME));
                }
                return listReslut;
            }
            var items = from item in ExtensionalOrderDetailFBEntityList
                        group item by new
                        {

                            (item.Entity as T_FB_EXTENSIONORDERDETAIL).T_FB_SUBJECT.SUBJECTNAME,
                            (item.Entity as T_FB_EXTENSIONORDERDETAIL).CHARGETYPE,
                            (item.Entity as T_FB_EXTENSIONORDERDETAIL).USABLEMONEY
                        } into g
                        where g.Sum(item => (item.Entity as T_FB_EXTENSIONORDERDETAIL).APPLIEDMONEY) > g.Key.USABLEMONEY
                        select new { g.Key, totalCharge = g.Sum(item => (item.Entity as T_FB_EXTENSIONORDERDETAIL).APPLIEDMONEY) };

            foreach (var item in items)
            {
                listReslut.Add(string.Format("科目:{2} 的" + msgBiggerError, "申请金额", "可用金额", item.Key.SUBJECTNAME));
            }

            bool isbError = false;

            if (ListBorrowDetail != null)
            {
                this.ListBorrowDetail.ForEach(item =>
                {
                    if (isbError)
                    {
                        return;
                    }
                    var dItem = this.BorrowGrid.GridItems[0].ReferenceTypes.FirstOrDefault(itemFind => itemFind.DICTIONARYVALUE == item.REPAYTYPE);
                    string typeName = "{0}-的";
                    if (dItem != null)
                    {
                        typeName = string.Format(typeName, dItem.DICTIONARYNAME);
                    }
                    else
                    {
                        typeName = "";
                    }

                    if (item.BORROWMONEY < item.REPAYMONEY)
                    {

                        listReslut.Add(string.Format("冲借款金额不能大于借款余额或报销金额"));
                        isbError = true;
                    }

                    if (item.REPAYMONEY > 0 && string.IsNullOrWhiteSpace(item.REMARK))
                    {
                        listReslut.Add("冲借款明细的摘要不能为空");
                        isbError = true;
                    }
                });


                var bcount = this.ListBorrowDetail.Sum(item =>
                {
                    return item.REPAYMONEY;
                });
                if (bcount > ExtensionalOrder.TOTALMONEY.Value)
                {
                    listReslut.Add("冲借款金额不能大于借款余额或报销金额");
                }
                if (this.rbtnChargeBorrow.IsChecked == true)
                {
                    if (bcount == 0)
                    {
                        listReslut.Add("冲借款时明细不能为空!");
                    }
                }

            }


            return listReslut;
        }

        /// <summary>
        /// 保存数据时用
        /// </summary>
        private void GetValue()
        {
            T_FB_EXTENSIONALORDER eOrder = this.ExtensionalOrderFBEntity.Entity as T_FB_EXTENSIONALORDER;
            //if (this.rbtnApplyTypeBorrow.IsChecked == true)
            //{
            //    eOrder.APPLYTYPE = 2;
            //}
            //else
            //{
            //    eOrder.APPLYTYPE = 1;
            //}
            if (this.rbtnChargePerson.IsChecked == true)
            {
                ChargeApplyMaster.PAYTYPE = 1;
            }
            else
            {
                ChargeApplyMaster.PAYTYPE = 2;
            }

            eOrder.REMARK = this.tbRemark.Text;

            if (this.TravelSubject != null)
            {
                //var sd = this.TravelSubject.SpecialListDetail.FirstOrDefault();
                //(sd.Entity as T_FB_EXTENSIONORDERDETAIL).APPLIEDMONEY = this.TravelSubject.ApplyMoney;

                TravelSubject.SpecialListDetail.ForEach(item =>
                {
                    item.Entity.SetObjValue("CREATEUSERID", eOrder.GetObjValue("CREATEUSERID"));
                    item.Entity.SetObjValue("CREATEDATE", System.DateTime.Now);

                    item.Entity.SetObjValue("UPDATEUSERID", eOrder.GetObjValue("UPDATEUSERID"));
                    item.Entity.SetObjValue("UPDATEDATE", System.DateTime.Now);

                    #region 去掉无关的关联
                    T_FB_EXTENSIONORDERDETAIL detail = item.Entity as T_FB_EXTENSIONORDERDETAIL;
                    if (detail.T_FB_SUBJECT.T_FB_EXTENSIONORDERDETAIL != null)
                    {
                        detail.T_FB_SUBJECT.T_FB_EXTENSIONORDERDETAIL.Clear();
                    }
                    if (detail.T_FB_SUBJECT.T_FB_BUDGETACCOUNT != null)
                    {
                        detail.T_FB_SUBJECT.T_FB_BUDGETACCOUNT.Clear();
                    }
                    #endregion

                });
            }
            ExtensionalOrderDetailFBEntityList.ForEach(item =>
            {
                item.Entity.SetObjValue("CREATEUSERID", eOrder.GetObjValue("CREATEUSERID"));
                item.Entity.SetObjValue("CREATEDATE", System.DateTime.Now);

                item.Entity.SetObjValue("UPDATEUSERID", eOrder.GetObjValue("UPDATEUSERID"));
                item.Entity.SetObjValue("UPDATEDATE", System.DateTime.Now);

                #region 去掉无关的关联
                T_FB_EXTENSIONORDERDETAIL detail = item.Entity as T_FB_EXTENSIONORDERDETAIL;
                if (detail.T_FB_SUBJECT.T_FB_EXTENSIONORDERDETAIL != null)
                {
                    detail.T_FB_SUBJECT.T_FB_EXTENSIONORDERDETAIL.Clear();
                }
                if (detail.T_FB_SUBJECT.T_FB_BUDGETACCOUNT != null)
                {
                    detail.T_FB_SUBJECT.T_FB_BUDGETACCOUNT.Clear();
                }
                #endregion

            });

            if (this.gridForBorrowInfo.Visibility == System.Windows.Visibility.Collapsed)
            {
                var list = GetRelationFBEntities(this.ExtensionalOrderFBEntity, typeof(T_FB_CHARGEAPPLYMASTER).Name);
                list.Clear();
            }
        }
        /// <summary>
        /// 保存预算申请单
        /// </summary>
        /// <param name="statates">当前表单状态</param>
        public void Save(CheckStates statates)
        {
            List<string> listMsg = this.OnSaveCheck();

            if (listMsg.Count > 0)
            {
                OnSaveCompleted(listMsg);
                return;
            }
            this.GetValue();


            if (statates == CheckStates.Delete)
            {
                ExtensionalOrderFBEntity.FBEntityState = FBEntityState.Detached;
            }
            // else if如果是重新提交，如果当前单据是审核不通过的状态才可以重新提交，否则提示异常。
            // 需处理　this.Order.CHECKSTATES 为提交状态，并this.CurrentOrderEntity.FBEntityState 为重新提交
            else
            {
                if (submitFBFormTypes == FormTypes.Audit && statates == CheckStates.Approving)
                {
                    return;
                }


                this.ExtensionalOrder.CHECKSTATES = Convert.ToDecimal((int)statates);

                if (submitFBFormTypes == FormTypes.Resubmit)
                {
                    this.ExtensionalOrderFBEntity.FBEntityState = FBEntityState.ReSubmit;

                }
                else
                {
                    if (this.ExtensionalOrderFBEntity.FBEntityState == FBEntityState.Unchanged)
                    {
                        this.ExtensionalOrderFBEntity.FBEntityState = FBEntityState.Modified;
                    }
                }

            }
            if (this.IsFromFB)
            {
                QueryExpression qeSave = this.GetOrderQueryExp();
                SaveEntity se = new SaveEntity();
                se.QueryExpression = qeSave;
                se.FBEntity = ExtensionalOrderFBEntity;
                FBServiceLoacal.SaveEntityAsync(se);
            }
            else
            {
                ObservableCollection<SMT.Saas.Tools.WpServiceWS.BussinessTripBudget> listTripBudget = new ObservableCollection<Saas.Tools.WpServiceWS.BussinessTripBudget>();
                
                foreach(var itemRM in ExtensionalOrderFBEntity.CollectionEntity)
                {
                    if(itemRM.EntityType==typeof(T_FB_EXTENSIONORDERDETAIL).Name
                        || itemRM.EntityType == "T_FB_EXTENSIONORDERDETAIL_Travel")
                    {
                        foreach(var item in itemRM.FBEntities)
                        {
                        SMT.Saas.Tools.WpServiceWS.BussinessTripBudget saveItem = new Saas.Tools.WpServiceWS.BussinessTripBudget();
                        T_FB_EXTENSIONORDERDETAIL detail = item.Entity as T_FB_EXTENSIONORDERDETAIL;
                        //saveItem.SubjectID = detail.T_FB_SUBJECT.SUBJECTID;
                        saveItem.SubjectName = detail.T_FB_SUBJECT.SUBJECTNAME;
                        saveItem.SubjectCode = detail.T_FB_SUBJECT.SUBJECTCODE;
                        saveItem.PaidMoney = detail.APPLIEDMONEY;
                        saveItem.UseMoney = detail.USABLEMONEY.Value;
                        saveItem.NormName = detail.REMARK;//摘要
                        saveItem.NormID = detail.T_FB_SUBJECT.SUBJECTID;//摘要
                        if (string.IsNullOrEmpty(saveItem.NormID))
                        {
                            MessageBox.Show("工作计划保存科目Id失败，请联系管理员！");
                        }
                        listTripBudget.Add(saveItem);
                        }
                    }
                }
                string msg = string.Empty;
                WpServiceClient.TripSubjectSaveAsync(strBussinessTripID, listTripBudget, msg);
            }
        }

        #endregion
    }
}
