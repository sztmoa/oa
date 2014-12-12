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

namespace SMT.LM.FileUpLoad.Test
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            //SMT.FileUpLoad.Classes. FileList.Init("LM", "CUSTOME", "Fuyicheng", "d027ed66-48c5-46cb-b24d-866b17704728");
            // SMT.FileUpLoad.FileControl ss = new SMT.FileUpLoad.FileControl(bol);        
            SMT.FileUpLoad.FileControl fc = (SMT.FileUpLoad.FileControl)sMTFileUpload1.FindName("FileList");
            // ((Button)ss.FindName("btnUp")).Visibility = Visibility.Collapsed;
            SMT.FileUpLoad.Classes.UserConfig uc = new SMT.FileUpLoad.Classes.UserConfig();
            uc.CompanyCode = "aa";
            uc.IsLimitCompanyCode = false;
            uc.SystemCode = "OA";
            uc.ModelCode = "考勤管理";
            // uc.UserID = "LONGKC";
            uc.ApplicationID = "smt-90156487";
            // uc.NotShowDeleteButton = true;
            uc.NotShowThumbailChckBox = true;

            uc.Multiselect = true;
            uc.Filter = "所有文件 (*.*)|*.*";
            //uc.Filter = "图片文件(*.jpg,*.gif,*.bmp)|*.jpg;*.gif;*.bmp";
            uc.MaxConcurrentUploads = 5;
            uc.MaxSize = "10.MB";
            uc.CreateName = "龙康才";
            uc.OnlyShowMySelf = true;
            uc.PageSize = 10;

            // uc.NotAllowDelete = true;
            // uc.NotShowUploadButton = true;
            // uc.NotAllowDownload = true;
            // uc.NotAllowDelete = true;
            // uc.NotShowUploadButton = true;
            // uc.NotAllowDownload = true;
            fc.Init(uc);
            // fc.GetFileListByCompanyCode(uc.CompanyCode);
            // fc.GetFileListByApplicationID(uc.ApplicationID);

            // ss.DeleteFileByApplicationID(uc.CompanyCode, uc.SystemCode, uc.ModelCode, uc.ApplicationID);

        }
    }
}
