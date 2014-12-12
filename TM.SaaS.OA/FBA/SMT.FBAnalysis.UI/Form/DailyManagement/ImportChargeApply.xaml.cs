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
using SMT.SaaS.FrameworkUI;
using SMT.SaaS.FrameworkUI.Common;
using SMT.FBAnalysis.ClientServices.DailyManagementWS;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.FBAnalysis.UI.CommonForm;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI.Validator;
using SMT.Saas.Tools.PersonnelWS;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.MobileXml;
using SMT.SAAS.ClientServices;
using SMT.SAAS.ClientUtility;
using System.Text.RegularExpressions;

namespace SMT.FBAnalysis.UI.Form
{
    public delegate void SaveFileHandler(int bytesRead, byte[] buffer);
    public delegate void UploadFileHandler(int bytesRead);

    public partial class ImportChargeApply : BaseForm, IEntityEditor
    {
        DailyManagementServicesClient client = new DailyManagementServicesClient();
        public OpenFileDialog OpenFileDialog = null;
        ObservableCollection<V_ChargeApplyReport> ListBalance = new ObservableCollection<V_ChargeApplyReport>();
        private FormTypes FormTypesAction;//操作定义 增加、修改、审核、查看、重新提交
        private FormTypes types;
        //AttendanceServiceClient clientAtt;
        byte[] byExport;

        private List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        public ImportChargeApply()
        {
            InitializeComponent();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            client.ImportAttendMonthlyBalanceFromCSVCompleted += new EventHandler<ImportAttendMonthlyBalanceFromCSVCompletedEventArgs>(client_ImportAttendMonthlyBalanceFromCSVCompleted);
            client.UptChargeApplyIsPayedCompleted += new EventHandler<UptChargeApplyIsPayedCompletedEventArgs>(client_UptChargeApplyIsPayedCompleted);
        }

        /// <summary>
        /// 设置当前页面状态
        /// </summary>
        private void RefreshFormType(FormTypes formtype, RefreshedTypes refreshedType)
        {
            EntityBrowser entBrowser = this.FindParentByType<EntityBrowser>();
            entBrowser.FormType = formtype;
            types = formtype;
            FormTypesAction = formtype;
            RefreshUI(refreshedType);
            RefreshUI(RefreshedTypes.All);
        }

        void client_UptChargeApplyIsPayedCompleted(object sender, UptChargeApplyIsPayedCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result>=0)
                {
                    tbFileName.Text = string.Empty;
                    RefreshFormType(FormTypes.Edit, RefreshedTypes.AuditInfo);
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("保存成功"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                }
                else
                {
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), Utility.GetResourceStr("保存失败"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                }
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERRORINFO"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
        }

        void client_ImportAttendMonthlyBalanceFromCSVCompleted(object sender, ImportAttendMonthlyBalanceFromCSVCompletedEventArgs e)
        {
            if (e.Error == null)
            {

                if (e.Result != null)
                {
                    ListBalance = e.Result;
                    if (ListBalance != null && ListBalance.Count > 0)
                    {
                        this.LoadData(ListBalance.ToList());
                    }
                }
                else
                {
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), "没有导入的数据",
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                    return;
                }
            }
            else
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("CAUTION"), "导入月费用报销记录失败",
                Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }
        }
        /// <summary>
        /// 选择上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BrowseFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog = new OpenFileDialog();

            OpenFileDialog.Multiselect = false;
            OpenFileDialog.Filter = "csv Files (*.csv)|*.csv;";

            if (OpenFileDialog.ShowDialog() != true)
            {
                return;
            }
            tbFileName.Text = OpenFileDialog.File.Name;
            txtUploadResMsg.Text = string.Empty;
            ImportBalance();
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            ImportBalance();
        }

        #region IEntityEditor
        public string GetTitle()
        {
            return Utility.GetResourceStr("PEOPLECHARGEAPPLY");
        }

        public string GetStatus()
        {
            if (string.IsNullOrEmpty(txtUploadResMsg.Text))
                return "上传中";
            else
                return "上传完毕";
        }


        public List<NavigateItem> GetLeftMenuItems()
        {
            List<NavigateItem> items = new List<NavigateItem>();
            return items;
        }

        public void DoAction(string actionType)
        {
            switch (actionType)
            {
                case "0":
                    UptChargeApplyIsPayed();
                    break;
                case "1":
                    //UptChargeApplyIsPayed();
                    break;
            }
        }

        private void LoadData(List<V_ChargeApplyReport> ListPensions)
        {
            if (ListPensions != null && ListPensions.Count > 0)
            {
                ListPensions.RemoveAt(0);
                DtGrid.ItemsSource = ListPensions;
            }
        }

        public List<ToolbarItem> GetToolBarItems()
        {
            List<ToolbarItem> items = new List<ToolbarItem>();

            ToolbarItem item = new ToolbarItem
            {
                DisplayType = ToolbarItemDisplayTypes.Image,
                Key = "0",
                Title = Utility.GetResourceStr("SAVE"),
                ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_save.png"
            };

            items.Add(item);
            return items;
        }

        public event UIRefreshedHandler OnUIRefreshed;
        public void RefreshUI(RefreshedTypes type)
        {
            if (OnUIRefreshed != null)
            {
                UIRefreshedEventArgs args = new UIRefreshedEventArgs();
                args.RefreshedType = type;
                OnUIRefreshed(this, args);
            }
        }
        #endregion
        /// <summary>
        /// 更新报销状态
        /// </summary>
        private void UptChargeApplyIsPayed()
        {
            if (string.IsNullOrEmpty(tbFileName.Text.Trim()))
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("请选择文件"), Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                return;
            }
            if (ListBalance != null && ListBalance.Count > 0)
            {
                ListBalance.RemoveAt(0);
                client.UptChargeApplyIsPayedAsync(ListBalance);
            }
        }
        private void ImportBalance()
        {
            string strMsg = string.Empty;
            try
            {
                System.IO.Stream Stream = (System.IO.Stream)OpenFileDialog.File.OpenRead();

                byte[] Buffer = new byte[Stream.Length];
                Stream.Read(Buffer, 0, (int)Stream.Length);

                Stream.Dispose();
                Stream.Close();

                SMT.FBAnalysis.ClientServices.DailyManagementWS.UploadFileModel UploadFile = new SMT.FBAnalysis.ClientServices.DailyManagementWS.UploadFileModel();
                UploadFile.FileName = OpenFileDialog.File.Name;
                UploadFile.File = Buffer;

                client.ImportAttendMonthlyBalanceFromCSVAsync(UploadFile, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, strMsg);

                strMsg = string.Empty;
            }
            catch (Exception ex)
            {
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(ex.Message.ToString()));
            }
        }
    }
}
