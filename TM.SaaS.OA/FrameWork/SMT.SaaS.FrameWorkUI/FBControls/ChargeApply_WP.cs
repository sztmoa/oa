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
using SMT.Saas.Tools.WpServiceWS;
using System.Collections.Generic;
using SMT.Saas.Tools.FBServiceWS;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.SaaS.FrameworkUI.Common;
using System.Collections.ObjectModel;

namespace SMT.SaaS.FrameworkUI.FBControls
{
    public partial class ChargeApplyControl
    {
        WPServicesClient WpService=null;

        WPServicesClient WpServiceClient
        {
            get
            {
                if (WpService == null)
                {
                    WpService = new WPServicesClient();
                    WpService.GetTripSubjectCompleted += WpService_GetTripSubjectCompleted;
                    WpService.TripSubjectSaveCompleted += WpService_TripSubjectSaveCompleted;
                }
                return WpService;
            }
        }

        void WpService_TripSubjectSaveCompleted(object sender, TripSubjectSaveCompletedEventArgs e)
        {
            List<string> listResult = new List<string>();
            if (e.Error == null)
            {
                OnSaveCompleted(listResult);
            }
            else
            {
                listResult.Add(e.Error.ToString());
                OnSaveCompleted(listResult);
            }
        }

        void WpService_GetTripSubjectCompleted(object sender, GetTripSubjectCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    OnQueryCompleted(null);
                    return;
                }

                if (e.Result.Count == 0)
                {
                    OnQueryCompleted(null);
                    return;
                }
                T_FB_EXTENSIONALORDER Extentity = new T_FB_EXTENSIONALORDER();
                Extentity.T_FB_EXTENSIONORDERDETAIL = new ObservableCollection<T_FB_EXTENSIONORDERDETAIL>();  
                //特殊科目出差报销业务差旅费
                RelationManyEntity TravelEntity = new RelationManyEntity();
                TravelEntity.FBEntities = new ObservableCollection<FBEntity>();
                TravelEntity.EntityType = "T_FB_EXTENSIONORDERDETAIL_Travel";
                //其他在报销控件里面选择的费用科目
                RelationManyEntity chargeEntity = new RelationManyEntity();
                chargeEntity.FBEntities = new ObservableCollection<FBEntity>();
                chargeEntity.EntityType = "T_FB_EXTENSIONORDERDETAIL";
                foreach(var item in e.Result)
                {
                    T_FB_EXTENSIONORDERDETAIL detail = new T_FB_EXTENSIONORDERDETAIL();
                    detail.EXTENSIONORDERDETAILID = Guid.NewGuid().ToString();  
                    SMT.Saas.Tools.FBServiceWS.T_FB_SUBJECT subject=new SMT.Saas.Tools.FBServiceWS.T_FB_SUBJECT();
                    if(string.IsNullOrEmpty(item.NormID))
                    {
                        MessageBox.Show("工作计划获取科目Id失败，请联系管理员！");
                    }
                    subject.SUBJECTID = item.NormID;
                    subject.SUBJECTCODE = item.SubjectCode;
                    subject.SUBJECTNAME = item.SubjectName;
                    subject.OWNERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                    subject.OWNERPOSTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
                    subject.OWNERDEPARTMENTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
                    subject.OWNERCOMPANYID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                    detail.T_FB_SUBJECT = subject;
                    detail.USABLEMONEY = item.UseMoney;
                    detail.APPLIEDMONEY = item.PaidMoney;
                    detail.REMARK = item.NormName;
                    Extentity.T_FB_EXTENSIONORDERDETAIL.Add(detail);

                    if(item.SubjectName=="业务差旅费")
                    {                      
                        TravelEntity.FBEntities.Add(ToFBEntity(detail));
                    }else
                    {
                        chargeEntity.FBEntities.Add(ToFBEntity(detail));
                    }
                    //detail.T_F
                }

                FBEntity queryEntity = new FBEntity();
                queryEntity = ToFBEntity(Extentity);
                queryEntity.CollectionEntity.Add(TravelEntity);
                queryEntity.CollectionEntity.Add(chargeEntity);

                OnQueryCompleted(queryEntity);
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), e.Error.Message.ToString(), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }

        }


    }
}
