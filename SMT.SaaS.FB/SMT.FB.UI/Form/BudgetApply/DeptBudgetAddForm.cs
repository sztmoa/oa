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
    public class DeptBudgetAddForm: FBPage
    {

      
        FBEntityService fbService;
        public DeptBudgetAddForm(OrderEntity orderEntity)
            : base(orderEntity)
        {
            fbService = new FBEntityService();
            fbService.QueryFBEntitiesCompleted += new EventHandler<QueryFBEntitiesCompletedEventArgs>(fbService_QueryFBEntitiesCompleted);
            this.EditForm.Saving += new EventHandler<SavingEventArgs>(EditForm_Saving);
        }

        Dictionary<string, FBEntity> dictPersonDetail = new Dictionary<string, FBEntity>();
        Dictionary<string, decimal?> dictPersonBUDGETMONEY = new Dictionary<string, decimal?>();

        

        void persondetail_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "BUDGETMONEY")
            {

                T_FB_PERSONBUDGETADDDETAIL persondetail = sender as T_FB_PERSONBUDGETADDDETAIL;
                decimal? budgetMoney = persondetail.BUDGETMONEY;
                string Key = persondetail.T_FB_SUBJECT.SUBJECTID + "-" + persondetail.OWNERID;
                FBEntity fbDeptDetail = dictPersonDetail[Key];
                decimal? PERSONBUDGETMONEY = (decimal?)(fbDeptDetail.Entity.GetObjValue("PERSONBUDGETMONEY"));
                decimal? oldbudgetMoney = dictPersonBUDGETMONEY[Key];
                dictPersonBUDGETMONEY[Key] = budgetMoney;
                fbDeptDetail.Entity.SetObjValue("PERSONBUDGETMONEY", budgetMoney - oldbudgetMoney + PERSONBUDGETMONEY);

                //object personMoney = sender.GetObjValue("PERSONBUDGETMONEY");

                //Nullable<decimal> a = (decimal?)budgetMoney;
                //Nullable<decimal> b = (decimal?)personMoney;

                //sender.SetObjValue("TOTALBUDGETMONEY", a.Add(b));
            }
        }

        void EditForm_Saving(object sender, SavingEventArgs e)
        {
            List<string> msgs = null;
            if (DataCore.GetSetting("CanAddLessThanZero") == "1")
            {
                msgs = CheckSaveB();
            }
            else
            {
                msgs = CheckSaveA();
            }
            if (msgs.Count > 0)
            {
                e.Action = Actions.Cancel;
                CommonFunction.ShowErrorMessage(msgs);
            }

        }
        /// <summary>
        /// 不可以为负数的保存校验
        /// </summary>
        /// <returns>returns</returns>
        private List<string> CheckSaveA()
        {
            List<string> msgs = new List<string>();

            ObservableCollection<FBEntity> details = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETADDDETAIL).Name);
            if (details.Count == 0)
            {
                msgs.Add(ErrorMessage.NoDetailInfo);
                return msgs;
            }


            details.ToList().ForEach(item =>
            {
                T_FB_DEPTBUDGETADDDETAIL detail = item.Entity as T_FB_DEPTBUDGETADDDETAIL;

                // 判断个人预算是否出现负数的情况
                var personDetailList = item.GetEntityList<T_FB_PERSONBUDGETADDDETAIL>().FindAll(itemT => itemT != null);
              
                // 表示没做过任何操作
                if (detail.TOTALBUDGETMONEY.Equal(0) && detail.BUDGETMONEY.Equal(0) )
                {
                    return;
                }

                if (detail.TOTALBUDGETMONEY < 0)
                {
                    string errorMessage = string.Format(ErrorMessage.BudgetMoneyZero, detail.T_FB_SUBJECT.SUBJECTNAME);
                    msgs.Add(errorMessage);

                }
                if (detail.BUDGETMONEY < 0)
                {
                    msgs.Add(string.Format(ErrorMessage.PersonBudgetMoneyBigger, detail.T_FB_SUBJECT.SUBJECTNAME));
                }
                //不能大于年度结余
                if (detail.USABLEMONEY.LessThan(detail.TOTALBUDGETMONEY))
                {
                    msgs.Add(string.Format(ErrorMessage.BudgetYearMoneyBigger, detail.T_FB_SUBJECT.SUBJECTNAME));
                }
                personDetailList.ForEach(itemP =>
                {
                    
                    if (itemP.BUDGETMONEY < 0)
                    {
                        msgs.Add(string.Format(ErrorMessage.MoneyZero, detail.T_FB_SUBJECT.SUBJECTNAME + "(" + itemP.OWNERNAME + ")", "个人预算金额"));
                    }
                });
                

            });

            T_FB_DEPTBUDGETADDMASTER entMaster = this.OrderEntity.FBEntity.Entity as T_FB_DEPTBUDGETADDMASTER;
            if (entMaster.BUDGETCHARGE <= 0)
            {
                msgs.Add("费用总预算必须大于0!");
            }
            return msgs;
        }

        /// <summary>
        /// 可为负数的保存校验
        /// </summary>
        /// <returns>returns</returns>
        private List<string> CheckSaveB()
        {
            List<string> msgs = new List<string>();
            ObservableCollection<FBEntity> details = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETADDDETAIL).Name);
            if (details.Count == 0)
            {
                msgs.Add(ErrorMessage.NoDetailInfo);
                return msgs;
            }

            var detailList = details.Select(item => item.Entity as T_FB_DEPTBUDGETADDDETAIL).ToList();
            var existBiggerThanZero = detailList.Count(item => item.TOTALBUDGETMONEY > 0) > 0;
            var existLessThanZero = detailList.Count(item => item.TOTALBUDGETMONEY < 0) > 0;
            if (existBiggerThanZero && existLessThanZero)
            {
                msgs.Add("预算明细只能全为负数，或全为正数，请确认!");
                return msgs;
            }
            // 都大于０的
            if (!existLessThanZero)
            {
                return CheckSaveA();
            }
            details.ToList().ForEach(item =>
            {
                T_FB_DEPTBUDGETADDDETAIL detail = item.Entity as T_FB_DEPTBUDGETADDDETAIL;

                // 判断个人预算是否出现负数的情况
                var personDetailList = item.GetEntityList<T_FB_PERSONBUDGETADDDETAIL>(itemT =>
                    {
                        var result = itemT as T_FB_PERSONBUDGETADDDETAIL;
                        if (result == null)
                        {
                            V_DepartmentSum vd = itemT as V_DepartmentSum;
                            result = new T_FB_PERSONBUDGETADDDETAIL()
                            {
                                LIMITBUDGETMONEY = vd.LIMITBUDGETMONEY,
                                BUDGETMONEY = vd.BUDGETMONEY,
                                OWNERNAME = vd.OWNERNAME,
                                OWNERPOSTNAME = vd.OWNERPOSTNAME
                            };
                        }
                        return result;
                    });
               
                // 表示没做过任何操作
                if (detail.TOTALBUDGETMONEY.Equal(0) && detail.BUDGETMONEY.Equal(0))
                {
                    return;
                }

                if (detail.TOTALBUDGETMONEY > 0)
                {
                    string errorMessage = string.Format(ErrorMessage.MoneyBiggerThanZero, detail.T_FB_SUBJECT.SUBJECTNAME, FieldDisplayName.BudgetMoney);
                    msgs.Add(errorMessage);

                }

                //不能大于当前可用结余AUDITBUDGETMONEY
                if (detail.AUDITBUDGETMONEY.Add(detail.TOTALBUDGETMONEY) < 0)
                {
                    msgs.Add(string.Format(ErrorMessage.MoneyBigger,"扣减的预算金额", "可用结余", detail.T_FB_SUBJECT.SUBJECTNAME));
                }
                personDetailList.ForEach(itemP =>
                    {
                        if (itemP.LIMITBUDGETMONEY.Add(itemP.BUDGETMONEY) < 0)
                        {
                            msgs.Add(string.Format("科目：{0} 扣减的预算金额不能大于可用结余!",detail.T_FB_SUBJECT.SUBJECTNAME + "(" + itemP.OWNERNAME + ")"));
                        }
                        else if (itemP.BUDGETMONEY > 0)
                        {
                            msgs.Add(string.Format("科目：{0} 扣减的预算金额不能大于零!", detail.T_FB_SUBJECT.SUBJECTNAME + "(" + itemP.OWNERNAME + ")"));
                            
                        }
                    });

            });

            T_FB_DEPTBUDGETADDMASTER entMaster = this.OrderEntity.FBEntity.Entity as T_FB_DEPTBUDGETADDMASTER;
            if (entMaster.BUDGETCHARGE >= 0)
            {
                msgs.Add("费用总预算必须小于0!");
            }
            return msgs;
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
                this.OrderEntity.SetObjValue(EntityFieldName.OwnerPostName, create.Post.Text);

                OrderEntity.SetObjValue("Entity.BUDGETARYMONTH", new DateTime(DataCore.SystemDateTime.Year, DataCore.SystemDateTime.Month, 1));

            }
            else if (this.EditForm.OperationType == OperationTypes.Browse)
            {
                SetPropertyChanged();
                return;
            }
            GetOrderDetail();
        }

        private void GetOrderDetail()
        {
            this.ShowProcess();
            // 预算月份
            DateTime dtBudgetDate = (DateTime)this.OrderEntity.GetObjValue("Entity.BUDGETARYMONTH");
            string dataValue = dtBudgetDate.AddMonths(-1).ToString("yyyy-MM-dd");
            QueryExpression qeMonth = QueryExpressionHelper.Equal("BUDGETCHECKDATE", dataValue);
            qeMonth.QueryType = typeof(T_FB_DEPTBUDGETADDDETAIL).Name;

            // 预算部门

            QueryExpression qeDept = this.OrderEntity.GetQueryExpression(FieldName.OwnerDepartmentID);
            qeDept.QueryType = typeof(T_FB_DEPTBUDGETADDDETAIL).Name;
            qeDept.RelatedExpression = qeMonth;

            fbService.QueryFBEntities(qeDept);
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
            //beyond
            // 清除预算明细
            this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETADDDETAIL).Name).Clear();
            this.OrderEntity.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name).Clear();

            // 添加预算明细
            e.Result.ToList().ForEach(item =>
            {
                T_FB_DEPTBUDGETADDDETAIL deptDetail = item.Entity as T_FB_DEPTBUDGETADDDETAIL;
                deptDetail.T_FB_DEPTBUDGETADDMASTER = this.OrderEntity.Entity as T_FB_DEPTBUDGETADDMASTER;

                item.FBEntityState = FBEntityState.Added;

                #region beyond
                item.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name).ToList().ForEach(fbpersondetail =>
                {
                    T_FB_PERSONBUDGETADDDETAIL persondetail = fbpersondetail.Entity as T_FB_PERSONBUDGETADDDETAIL;
                    persondetail.CREATEUSERID = this.OrderEntity.LoginUser.Value.ToString();
                    persondetail.CREATEUSERNAME = this.OrderEntity.LoginUser.Text;
                    persondetail.UPDATEUSERID = this.OrderEntity.LoginUser.Value.ToString();
                    persondetail.UPDATEUSERNAME = this.OrderEntity.LoginUser.Text;
                    persondetail.T_FB_DEPTBUDGETADDDETAIL = item.Entity as T_FB_DEPTBUDGETADDDETAIL;

                    fbpersondetail.FBEntityState = FBEntityState.Added;
                });
                #endregion

                deptDetail.T_FB_SUBJECT.T_FB_BUDGETACCOUNT.Clear();
                deptDetail.T_FB_SUBJECT.T_FB_BUDGETCHECK.Clear();

            });
            this.OrderEntity.FBEntity.AddFBEntities<T_FB_DEPTBUDGETADDDETAIL>(e.Result);
            this.OrderEntity.FBEntity.SetObjValue("Entity.BUDGETCHARGE", 0);
          
        }

        /// <summary>
        /// 编辑时操作
        /// </summary>
        /// <param name="e"></param>
        private void DoForEdit(QueryFBEntitiesCompletedEventArgs e)
        {

            // 清除预算明细
            var details = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETADDDETAIL).Name);
            var personDetails = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name);

            var detailsTemp = details.ToList();

            var resultsTemp = e.Result.ToList();
            Action<ObservableCollection<FBEntity>, ObservableCollection<FBEntity>, T_FB_DEPTBUDGETADDDETAIL> handelDetailPerson = (detailPersons, resultPersons, detail) =>
            {
                detailPersons.CompareFBEntity(resultPersons,
                    (item, itemR) => item.CompareFBEntity<T_FB_PERSONBUDGETADDDETAIL>(itemR, entity => entity.OWNERID, entity1 => entity1.OWNERPOSTID),
                (item, find) =>  /*找到的处理*/
                {
                    var itemEntity = item.Entity as T_FB_PERSONBUDGETADDDETAIL;
                    var findEntity = find.Entity as T_FB_PERSONBUDGETADDDETAIL;
                    itemEntity.USABLEMONEY = findEntity.USABLEMONEY;
                    itemEntity.LIMITBUDGETMONEY = findEntity.LIMITBUDGETMONEY;

                },
                (item) => /*需要删除的item*/
                {
                    personDetails.Add(item);
                    personDetails.Remove(item);
                    detailPersons.Remove(item);
                },

                (itemR) => /*需要新的item*/
                {
                    var findEntity = itemR.Entity as T_FB_PERSONBUDGETADDDETAIL;
                    findEntity.T_FB_DEPTBUDGETADDDETAIL = detail;
                    findEntity.T_FB_SUBJECT = detail.T_FB_SUBJECT;
                    findEntity.CREATEUSERID = this.OrderEntity.LoginUser.Value.ToString();
                    findEntity.CREATEUSERNAME = this.OrderEntity.LoginUser.Text;
                    findEntity.UPDATEUSERID = this.OrderEntity.LoginUser.Value.ToString();
                    findEntity.UPDATEUSERNAME = this.OrderEntity.LoginUser.Text;


                    itemR.FBEntityState = FBEntityState.Added;
                    detailPersons.Add(itemR);
                }
                );
            };
            details.CompareFBEntity(e.Result,
                (item, itemR) => item.CompareFBEntity<T_FB_DEPTBUDGETADDDETAIL>(itemR, entity => entity.T_FB_SUBJECT.SUBJECTID),
                (item, find) =>  /*找到的处理*/
                {
                    var itemEntity = item.Entity as T_FB_DEPTBUDGETADDDETAIL;
                    var findEntity = find.Entity as T_FB_DEPTBUDGETADDDETAIL;
                    itemEntity.AUDITBUDGETMONEY = findEntity.AUDITBUDGETMONEY;
                    itemEntity.PERSONBUDGETMONEY = findEntity.PERSONBUDGETMONEY;
                    itemEntity.USABLEMONEY = findEntity.USABLEMONEY;

                    var d1 = item.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name);
                    var d2 = find.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name);
                    handelDetailPerson(d1, d2, itemEntity);
                },
                (item) => /*需要删除的item*/
                {
                    var d1 = item.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name);
                    var d2 = new ObservableCollection<FBEntity>();
                    handelDetailPerson(d1, d2, item.Entity as T_FB_DEPTBUDGETADDDETAIL);

                    details.Remove(item);
                },

                (itemR) => /*需要新的item*/
                {
                    var deptDetail = itemR.Entity as T_FB_DEPTBUDGETADDDETAIL;
                    deptDetail.T_FB_SUBJECT.T_FB_BUDGETACCOUNT.Clear();
                    deptDetail.T_FB_SUBJECT.T_FB_BUDGETCHECK.Clear();
                    deptDetail.T_FB_DEPTBUDGETADDMASTER = this.OrderEntity.Entity as T_FB_DEPTBUDGETADDMASTER;
                    itemR.FBEntityState = FBEntityState.Added;
                    details.Add(itemR);
                    var d1 = new ObservableCollection<FBEntity>();
                    var d2 = itemR.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name);
                    handelDetailPerson(d1, d2, deptDetail);
                }
            );

            RefreshBudgetData();
        }

       


        protected override void OnLoadDataComplete()
        {
            InitData();
            T_FB_DEPTBUDGETADDMASTER master = this.OrderEntity.Entity as T_FB_DEPTBUDGETADDMASTER;
            //var DTNow = master.BUDGETARYMONTH;
            //DateTime OperDate = System.Convert.ToDateTime(DTNow.Year + "-" + DTNow.Month + "-" + "30" + " 23:59:59");

            //this.OrderMessage = "单据的有效时间为提交审核时所在月份最后一天的23时59分59秒";
        }

        protected override void OnLoadControlComplete()
        {
            this.OrderEntity.OrderPropertyChanged += new EventHandler<OrderPropertyChangedArgs>(OrderEntity_OrderPropertyChanged);
            if (this.EditForm.OperationType != OperationTypes.Browse)
            {
                this.ShowProcess(true);
                this.EditForm.BindingData(new OrderEntity(new T_FB_DEPTBUDGETADDMASTER()));
            }
        }

        void OrderEntity_OrderPropertyChanged(object sender, OrderPropertyChangedArgs e)
        {
            if (e.Result.Contains(EntityFieldName.OwnerDepartmentID))
            {
                GetOrderDetail();
                ChangeCreator();
                ChangeOwnerID();
            }
        }


        private void SetPropertyChanged()
        {
            // 因后台保存是二层结构，所有需要把 A->B->C这种结构变成 A->B, A->C结构
            ObservableCollection<FBEntity> detailPersonList = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name);
            ObservableCollection<FBEntity> detailList = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETADDDETAIL).Name);

            detailList.ToList().ForEach(item =>
            {
                #region 部门变化
                // 部门预算明细变化事件
                item.Entity.PropertyChanged += (senderDept, eDept) =>
                {
                    T_FB_DEPTBUDGETADDDETAIL DeptDetail = senderDept as T_FB_DEPTBUDGETADDDETAIL;
                    #region
                    // 部门预算明细的总预算变化时，统计所有明细的预算
                    if (eDept.PropertyName == "TOTALBUDGETMONEY")
                    {
                        decimal? newTotal = detailList.Sum(itemDept =>
                        {
                            return (decimal?)itemDept.GetObjValue("Entity.TOTALBUDGETMONEY");
                        });

                        this.OrderEntity.SetObjValue("Entity.BUDGETCHARGE", newTotal);
                    }
                  
                    // 改：先填写预算总额，后填个人预算，而部门的公共预算＝预算总额 - 个人预算 2011-12-28
                    // 个人预算或部门预算变化时，统计个人预算明细　+ 部门预算
                    if (eDept.PropertyName == "TOTALBUDGETMONEY" || eDept.PropertyName == "PERSONBUDGETMONEY")
                    {
                        DeptDetail.BUDGETMONEY = DeptDetail.TOTALBUDGETMONEY.Subtract(DeptDetail.PERSONBUDGETMONEY).Value;
                    }

                    #endregion
                };
                #endregion

                #region 个人预算明细变化
                // 个人预算明细变化
                ObservableCollection<FBEntity> obPersonDetail = item.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name);
                obPersonDetail.ToList().ForEach(itemPerson =>
                {
                    T_FB_PERSONBUDGETADDDETAIL persondetail = itemPerson.Entity as T_FB_PERSONBUDGETADDDETAIL;

                    itemPerson.Entity.PropertyChanged += (senderPerson, ePerson) =>
                    {
                        if (ePerson.PropertyName == "BUDGETMONEY")
                        {
                            var totalPerson = obPersonDetail.Sum(itemPersonDetail =>
                            {
                                // 在个人预算中将会部门预算记录，所以需要判断是否统计的对象是个人预算
                                T_FB_PERSONBUDGETADDDETAIL pDetail = itemPersonDetail.Entity as T_FB_PERSONBUDGETADDDETAIL;
                                if (pDetail != null)
                                {

                                    return pDetail.BUDGETMONEY;
                                }
                                else
                                {
                                    return 0;
                                }
                            });

                            var itemDept = item.Entity as T_FB_DEPTBUDGETADDDETAIL;
                            itemDept.PERSONBUDGETMONEY = totalPerson;
                        }
                    };

                    detailPersonList.Add(itemPerson);
                });

                // 因为在个人预算中需要体现部门公共费用栏，则加入部门公共费用(T记录到个人预算集合中
                if (obPersonDetail.Count > 0)
                {
                    var sumEntity = new V_DepartmentSum(item.Entity);
                    var curItemEntity = item.Entity as T_FB_DEPTBUDGETADDDETAIL;
                    var sumFBEntity = sumEntity.ToFBEntity();
                    sumFBEntity.ReadOnly = true;
                    sumEntity.LIMITBUDGETMONEY = curItemEntity.AUDITBUDGETMONEY.Subtract(obPersonDetail.Sum(itemFB =>
                    {
                        var tee = (itemFB.Entity as T_FB_PERSONBUDGETADDDETAIL);
                        if (tee.LIMITBUDGETMONEY == null)
                        {
                            return 0;
                        }
                        else
                        {
                            return tee.LIMITBUDGETMONEY;
                        }
                    }));
                    sumFBEntity.FBEntityState = FBEntityState.Unchanged;
                    obPersonDetail.Insert(0, sumFBEntity);

                }
                else
                {
                    item.HideDetails = true;
                }

                #endregion
            });

            this.EditForm.BindingData();
            this.CloseProcess(false);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshBudgetData()
        {
            ObservableCollection<FBEntity> detailList = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETADDDETAIL).Name);
            decimal? newTotal = 0;
            detailList.ToList().ForEach(item =>
            {
                ObservableCollection<FBEntity> obPersonDetail = item.GetRelationFBEntities(typeof(T_FB_PERSONBUDGETADDDETAIL).Name);
                var totalPerson = obPersonDetail.Sum(itemPersonDetail =>
                {
                    // 在个人预算中将会部门预算记录，所以需要判断是否统计的对象是个人预算
                    T_FB_PERSONBUDGETADDDETAIL pDetail = itemPersonDetail.Entity as T_FB_PERSONBUDGETADDDETAIL;
                    if (pDetail != null)
                    {

                        return pDetail.BUDGETMONEY;
                    }
                    else
                    {
                        return 0;
                    }
                });
                var itemDept = item.Entity as T_FB_DEPTBUDGETADDDETAIL;
                itemDept.PERSONBUDGETMONEY = totalPerson;
                itemDept.BUDGETMONEY = itemDept.TOTALBUDGETMONEY.Subtract(itemDept.PERSONBUDGETMONEY).Value;
                newTotal += itemDept.TOTALBUDGETMONEY;
            });

            this.OrderEntity.SetObjValue("Entity.BUDGETCHARGE", newTotal);
        }
    }
}
