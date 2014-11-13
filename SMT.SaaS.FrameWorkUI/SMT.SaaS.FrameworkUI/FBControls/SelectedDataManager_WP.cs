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

using System.Collections;
using SMT.Saas.Tools.FBServiceWS;
using System.Xml.Linq;
using System.Reflection;
using System.Globalization;
using SMT.Saas.Tools.WpServiceWS;
using SMT.SaaS.FrameworkUI.Common;
using SMT.SaaS.FrameworkUI.ChildWidow;
using System.Collections.ObjectModel;


namespace SMT.SaaS.FrameworkUI.FBControls
{
    public partial class SelectedDataManager
    {
        WPServicesClient WpService = null;

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
            if (e.Error == null)
            {
            }
            else
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "系统错误，请联系管理员!");
            }
        }

        void WpService_GetTripSubjectCompleted(object sender, GetTripSubjectCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    //OnQueryCompleted(null);
                    return;
                }

                if (e.Result.Count == 0)
                {
                    //OnQueryCompleted(null);
                    return;
                }
                List<FBEntity> listall = new List<FBEntity>();
                foreach (var item in e.Result)
                {

                    if (item.SubjectName == "业务差旅费")
                    {
                        continue;
                    }
                    T_FB_EXTENSIONORDERDETAIL detail = new T_FB_EXTENSIONORDERDETAIL();
                    detail.EXTENSIONORDERDETAILID = Guid.NewGuid().ToString();
                    SMT.Saas.Tools.FBServiceWS.T_FB_SUBJECT subject = new SMT.Saas.Tools.FBServiceWS.T_FB_SUBJECT();
                    subject.SUBJECTID = item.SubjectID;
                    subject.SUBJECTCODE = item.SubjectCode;
                    subject.SUBJECTNAME = item.SubjectName;
                    subject.OWNERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
                    subject.OWNERPOSTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
                    subject.OWNERDEPARTMENTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
                    subject.OWNERCOMPANYID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                    detail.T_FB_SUBJECT = subject;
                    detail.USABLEMONEY = item.UseMoney;
                    detail.APPLIEDMONEY = item.PaidMoney;
                    if (detail.T_FB_EXTENSIONALORDERReference == null)
                    {

                    }
                    FBEntity queryEntity = new FBEntity();
                    queryEntity = ToFBEntity(detail);
                    listall.Add(queryEntity);
                    //detail.T_F
                }
                OriginalItems = listall;
                OnGetUnSelectedItems();
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), e.Error.Message.ToString(), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }

        }
        private FBEntity ToFBEntity(EntityObject t)
        {
            FBEntity fbEntity = new FBEntity();
            fbEntity.Entity = t;
            fbEntity.CollectionEntity = new ObservableCollection<RelationManyEntity>();
            fbEntity.ReferencedEntity = new ObservableCollection<RelationOneEntity>();
            return fbEntity;
        }

    }
}
