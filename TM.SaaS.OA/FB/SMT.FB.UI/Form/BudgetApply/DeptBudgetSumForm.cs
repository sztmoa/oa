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
using SMT.SaaS.FrameworkUI;

namespace SMT.FB.UI.Form.BudgetApply
{
    /// <summary>
    /// 部门汇总
    /// </summary>
    public class DeptBudgetSumForm : FBPage
    {
        FBEntityService fbService;

        /// <summary>
        /// 汇总类型 0科目，1部门
        /// </summary>
        /// 
        private int _SumType=0;
        public int SumType 
        {
            get
            {
                return this._SumType;
            }set
            {
                this._SumType=value;
            }
        }

        public DeptBudgetSumForm(OrderEntity orderEntity)
            : base(orderEntity)
        {
            this.EditForm.Saving += new EventHandler<SavingEventArgs>(EditForm_Saving);
        }

        void EditForm_Saving(object sender, SavingEventArgs e)
        {
           
            e.SaveFBEntity.CollectionEntity.Clear();
        }

        protected override void OnLoadControlComplete()
        {
            base.OnLoadControlComplete();
            DetailGrid grid = this.EditForm.FindControl("OrderGrid") as DetailGrid;
            if (grid != null)
            {
                grid.P1.Visibility = System.Windows.Visibility.Visible;
                List<ToolbarItem> list = new List<ToolbarItem>();
                ToolbarItem item = new ToolbarItem
                {
                    DisplayType = ToolbarItemDisplayTypes.Image,
                    Key = "S1",
                    Title = "按科目查看",
                    ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/18_addView.png",


                };
                list.Add(item);
                item = new ToolbarItem
                {
                    DisplayType = ToolbarItemDisplayTypes.Image,
                    Key = "S2",
                    Title = "按公司部门单据查看",
                    ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/18_addView.png"
                };
                list.Add(item);

                grid.AddToolBarItems(list);
            }
            var dGrid = grid;
            dGrid.ADGrid.LoadingRow += (object sender, DataGridRowEventArgs e) =>
            {
                if ((this.EditForm.OperationType == OperationTypes.Add
                    || this.EditForm.OperationType == OperationTypes.Edit
                    || this.EditForm.OperationType == OperationTypes.ReSubmit) && SumType == 1)
                {
                    var con = dGrid.ADGrid.Columns[7].GetCellContent(e.Row) as StackPanel;
                    Action a2 = () =>
                    {
                        Label label = new Label();
                        label.Content = "已打回";
                        con.Children.Clear();
                        con.Children.Add(label);
                    };

                    Action a1 = () =>
                    {
                        ImageButton myButton = new ImageButton();
                        myButton.Margin = new Thickness(0);
                        myButton.AddButtonAction("/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/ico_16_delete.png", "打回");
                        myButton.Tag = e.Row.DataContext;
                        myButton.Click += (oo, ee) =>
                        {
                            Control c = oo as Control;
                            var entity = c.Tag as FBEntity;
                            Action action = () =>
                            {

                                // dGrid.Delete(new List<FBEntity> { entity });
                                var saveEntity = entity.Entity.ToFBEntity();
                                saveEntity.SetObjValue("Entity.CHECKSTATES", 4);
                                saveEntity.FBEntityState = FBEntityState.Modified;
                                FBEntityService fbs = new FBEntityService();
                                fbs.SetVisitUser(saveEntity);
                                fbs.FBService.SaveCompleted += (ooo, eee) =>
                                {
                                    this.CloseProcess();
                                    if (eee.Error != null)
                                    {
                                        CommonFunction.ShowErrorMessage("操作失败, " + eee.Error.Message);

                                    }
                                    else if (eee.Result.Exception != null)
                                    {
                                        CommonFunction.ShowErrorMessage(eee.Result.Exception);
                                    }
                                    else
                                    {
                                        a2();
                                    }
                                };
                                this.ShowProcess();
                                fbs.FBService.SaveAsync(saveEntity);
                                // none;
                            };
                            var personName = entity.GetObjValue("Entity.T_FB_DEPTBUDGETAPPLYMASTER.OWNERDEPARTMENTNAME");

                            var msg = "你确定要打回 [" + personName + "] 的部门月度预算吗?";
                            CommonFunction.AskDelete(msg, action);
                        };
                        con.Children.Clear();
                        con.Children.Add(myButton);
                    };

                    var cs = e.Row.DataContext.GetObjValue("Entity.CHECKSTATES") as decimal?;
                    if (cs.Equal(4))
                    {
                        a2();
                    }
                    else
                    {
                        a1();
                    }
                }
            };     


            grid.deatilGridBar.ItemClicked += new EventHandler<ToolBar.ToolBarItemClickArgs>(deatilGridBar_ItemClicked);
            deatilGridBar_ItemClicked(grid, new ToolBar.ToolBarItemClickArgs("S1"));
        }

        void deatilGridBar_ItemClicked(object sender, ToolBar.ToolBarItemClickArgs e)
        {
            if (e.Key == "S1")
            {
                this.SumType = 0;
            }
            else
            {
                this.SumType = 1;
            }
            //this.InitData();
            DetailGrid grid = this.EditForm.FindControl("OrderGrid") as DetailGrid;
            if (grid != null)
            {
                if (this.SumType == 0)
                {
                    int i = 1;
                    if (grid.ADGrid.Columns[0].GetType() == typeof(DataGridIconColumn))
                    {
                        i = 2;
                    }
                    grid.ADGrid.Columns[i-1].Visibility = Visibility.Visible;
                    grid.ADGrid.Columns[i].Visibility = Visibility.Visible;
                    grid.ADGrid.Columns[i + 3].Visibility = Visibility.Visible;

                    grid.ADGrid.Columns[i + 1].Visibility = Visibility.Collapsed;
                    grid.ADGrid.Columns[i + 2].Visibility = Visibility.Collapsed;
                    grid.ADGrid.Columns[i + 4].Visibility = Visibility.Collapsed;
                    grid.ADGrid.Columns[i + 5].Visibility = Visibility.Collapsed;

                    grid.ItemsSource = this.OrderEntity.GetRelationFBEntities(typeof(V_SubjectDepartmentSum).Name);
                }
                else
                {
                    int i = 1;
                    if (grid.ADGrid.Columns[0].GetType() == typeof(DataGridIconColumn))
                    {
                        i = 2;
                    }
                    grid.ADGrid.Columns[i-1].Visibility = Visibility.Collapsed;
                    grid.ADGrid.Columns[i].Visibility = Visibility.Collapsed;

                    grid.ADGrid.Columns[i + 2].Visibility = Visibility.Visible;
                    grid.ADGrid.Columns[i + 1].Visibility = Visibility.Visible;
                    grid.ADGrid.Columns[i + 3].Visibility = Visibility.Visible;
                    grid.ADGrid.Columns[i + 4].Visibility = Visibility.Visible;
                    grid.ADGrid.Columns[i + 5].Visibility = Visibility.Visible;

                    grid.ItemsSource = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETSUMDETAIL).Name);
                }
            }
        }
        protected override void OnLoadDataComplete()
        {
            var details = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETSUMDETAIL).Name);

            this.OrderEntity.FBEntity.OrderDetailBy<T_FB_DEPTBUDGETSUMDETAIL>(item =>
                {
                    return item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERDEPARTMENTID;
                });

            List<T_FB_DEPTBUDGETAPPLYDETAIL> listDetail = new List<T_FB_DEPTBUDGETAPPLYDETAIL>();

            details.ToList().ForEach(item =>
            {
                var cDetail = (item.Entity as T_FB_DEPTBUDGETSUMDETAIL).T_FB_DEPTBUDGETAPPLYDETAIL;
                item.AddFBEntities<T_FB_DEPTBUDGETAPPLYDETAIL>(cDetail.ToFBEntityList());
                item.OrderDetailBy<T_FB_DEPTBUDGETAPPLYDETAIL>(detail =>
                    {
                        return detail.T_FB_SUBJECT.SUBJECTCODE;
                    });
                listDetail.AddRange(cDetail);
            });

           var detailSum = from item in listDetail
                            group item by item.T_FB_SUBJECT into p
                            select new V_SubjectDepartmentSum
                            {
                                T_FB_SUBJECT = p.Key,
                                BUDGETMONEY = p.Sum(sumItem => sumItem.TOTALBUDGETMONEY.Value),
                                T_FB_DEPTBUDGETAPPLYDETAIL = listDetail.Where(de => de.T_FB_SUBJECT == p.Key).ToList()
                            };
            var detailsTwo = this.OrderEntity.GetRelationFBEntities(typeof(V_SubjectDepartmentSum).Name);

            foreach (var ds in detailSum)
            {
                FBEntity fbDetail = ds.ToFBEntity();
                fbDetail.AddFBEntities<T_FB_DEPTBUDGETAPPLYDETAIL>(ds.T_FB_DEPTBUDGETAPPLYDETAIL.ToFBEntityList());

                fbDetail.OrderDetailBy<T_FB_DEPTBUDGETAPPLYDETAIL>(item =>
                {
                    return item.T_FB_DEPTBUDGETAPPLYMASTER.OWNERDEPARTMENTID;
                });
                detailsTwo.Add(fbDetail);
            }

            this.OrderEntity.FBEntity.OrderDetailBy<V_SubjectDepartmentSum>(item =>
            {
                return item.T_FB_SUBJECT.SUBJECTCODE;
            });

            T_FB_DEPTBUDGETSUMMASTER master = this.OrderEntity.Entity as T_FB_DEPTBUDGETSUMMASTER;
            this.OrderMessage = "单据的截止提交时间为：" + master.BUDGETARYMONTH.AddDays(-1).ToLongDateString();
        }
        protected override void OnAuditing(object sender, AuditEventArgs e)
        {
            try
            {
                this.OrderEntity.FBEntity.CollectionEntity.Clear();

                var posts = (this.OrderEntity.LoginUser as LoginUserData).PostInfos;
                EmployeerData ownerInfo = this.OrderEntity.GetOwnerInfo();

                var finds = posts.Where(item => item.Company.Value.ToString() == ownerInfo.Company.Value.ToString());
                if (finds.Count() > 0)
                {
                    var user = finds.FirstOrDefault();
                    AuditControl ac = sender as AuditControl;
                    ac.AuditEntity.CreateCompanyID = user.Company.Value.ToString();
                    ac.AuditEntity.CreateDepartmentID = user.Department.Value.ToString();
                    ac.AuditEntity.CreatePostID = user.Post.Value.ToString();
                }


            }
            catch (Exception ex)
            {
                e.Result = AuditEventArgs.AuditResult.Cancel;
                CommonFunction.ShowErrorMessage("提交或审核异常, " + ex.ToString());
            }
        }

        protected override bool AuditCheck()
        {
            var details = this.OrderEntity.GetRelationFBEntities(typeof(T_FB_DEPTBUDGETSUMDETAIL).Name);
            var findsT = details.FindAll(item => (item.Entity as T_FB_DEPTBUDGETSUMDETAIL).CHECKSTATES.Equal(4));
            if (findsT.Count > 0)
            {
                Action action = () =>
                {
                    if (details.Count == findsT.Count)
                    {
                        CommonFunction.ShowErrorMessage("不存在可提交的明细单据，请确认！");
                        return;
                    }
                    base.InnerSubmit();
                };
                var msg = "系统将会自动从汇总明细单据中移除已打回明细单据并提交，请确认继续提交？";
                CommonFunction.AskDelete(msg, action);
                return false;
            }

            return true;
        }
        
    }
}
