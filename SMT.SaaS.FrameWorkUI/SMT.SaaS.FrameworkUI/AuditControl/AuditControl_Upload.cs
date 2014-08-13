using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SMT.SaaS.FrameworkUI.Common;
using System.Windows.Data;
using SMT.Saas.Tools.FlowWFService;
using SMT.SaaS.FrameworkUI.KPIControl;
using SMT.SaaS.FrameworkUI.ChildWidow;
using System.Text;
using SMT.Saas.Tools;
using System.Reflection;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace SMT.SaaS.FrameworkUI.AuditControl
{
    public partial class AuditControl : UserControl
    {
        public static void InitFileLoad(string strApplicationID, FormTypes action, FileUpLoad.FileControl control, bool AllowDelete)
        {
            SMT.FileUpLoad.Classes.UserConfig uc = new SMT.FileUpLoad.Classes.UserConfig();
            uc.CompanyCode = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            uc.SystemCode = "WF";
            uc.ModelCode = "FLOW_FLOWRECORDDETAIL_T";
            uc.UserID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            uc.ApplicationID = strApplicationID;
            uc.NotShowThumbailChckBox = true;
            if (action == FormTypes.Browse || action == FormTypes.Audit)
            {
                uc.NotShowUploadButton = true;
                uc.NotShowDeleteButton = true;
                uc.NotAllowDelete = true;
            }
            if (!AllowDelete)
            {
                uc.NotShowDeleteButton = true;
            }
            uc.Multiselect = true;
            uc.Filter = "所有文件 (*.*)|*.*";
            //uc.Filter = "图片文件(*.jpg,*.gif,*.bmp)|*.jpg;*.gif;*.bmp";
            uc.MaxConcurrentUploads = 5;
            uc.MaxSize = "20.MB";
            uc.CreateName = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
            uc.PageSize = 20;
            control.Init(uc);
        }
    }
}
