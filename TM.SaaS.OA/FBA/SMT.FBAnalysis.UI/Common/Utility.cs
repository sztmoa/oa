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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SMT.SaaS.FrameworkUI;
using SMT.SAAS.Main.CurrentContext;
using SMT.Saas.Tools.PermissionWS;
using SMT.Saas.Tools.PersonnelWS;
using SMT.Saas.Tools.FlowDesignerWS;
using SMT.SAAS.Controls.Toolkit.Windows;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace SMT.FBAnalysis.UI.Common
{
    public class Utility
    {
        /// <summary>
        /// 将字典值绑定到 combox中
        /// </summary>
        /// <param name="cbx"></param>
        /// <param name="category"></param>
        /// <param name="defalutValue"></param>
        public static void CbxItemBinder(ComboBox cbx, string category, string defalutValue)
        {
            List<T_SYS_DICTIONARY> dicts = Application.Current.Resources["SYS_DICTIONARY"] as List<T_SYS_DICTIONARY>;
            var ents = dicts.Where(s => s.DICTIONCATEGORY == category).OrderBy(s => s.DICTIONARYVALUE);
            List<T_SYS_DICTIONARY> tempDicts = ents.ToList();

            if (ents == null)
            {
                return;
            }

            if (ents.Count() == 0)
            {
                return;
            }

            T_SYS_DICTIONARY nuldict = new T_SYS_DICTIONARY();
            nuldict.DICTIONARYNAME = Utility.GetResourceStr("ALL");
            nuldict.DICTIONARYVALUE = 5;
            tempDicts.Insert(0, nuldict);

            cbx.ItemsSource = tempDicts;
            cbx.DisplayMemberPath = "DICTIONARYNAME";
            if (defalutValue != "")
            {
                foreach (var item in cbx.Items)
                {
                    T_SYS_DICTIONARY dict = item as T_SYS_DICTIONARY;
                    if (dict != null)
                    {
                        if (dict.DICTIONARYVALUE.ToString() == defalutValue)
                        {
                            cbx.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取多语言转换
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string GetResourceStr(string message)
        {
            string rslt = SMT.SaaS.Globalization.Localization.ResourceMgr.GetString(message, SMT.SaaS.Globalization.Localization.UiCulture);
            return string.IsNullOrEmpty(rslt) ? message : rslt;
        }
        /// <summary>
        /// 获取多语言转换
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static string GetResourceStr(string message, string parameter)
        {
            string rslt = SMT.SaaS.Globalization.Localization.GetString(message, parameter);

            return string.IsNullOrEmpty(rslt) ? message : rslt;
        }

        public static object XmlToContractObject(string xml, Type type)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(xml)))
            {
                DataContractSerializer dataContractSerializer = new DataContractSerializer(type);
                return dataContractSerializer.ReadObject(ms);
            }
        }

        private static string FormID;
        private static string FormName;
        private static string FormType;
        /// <summary>
        /// 门户创建待审批的单据（通过引擎） 2010-6-24
        /// </summary>
        /// <param name="FormID"></param>
        /// <param name="FormName"></param>
        public static void CreateFormFromEngine(string StrFormID, string StrFormName, string StrFormType)
        {
            FormID = StrFormID;
            FormName = StrFormName;
            FormType = StrFormType;

            //ChecResource();
            FormTypes CurrentAction = FormTypes.Resubmit;
            if (!string.IsNullOrEmpty(FormType))
            {
                switch (FormType.ToUpper())
                {
                    case "AUDIT":
                        CurrentAction = FormTypes.Audit;
                        break;
                    case "ADD":
                        CurrentAction = FormTypes.New;
                        break;
                    case "EDIT":
                        CurrentAction = FormTypes.Edit;
                        break;
                    case "VIEW":
                        CurrentAction = FormTypes.Browse;
                        break;
                    case "RESUBMIT":
                        CurrentAction = FormTypes.Resubmit;
                        break;
                }
            }

            Type t = Type.GetType(FormName);
            // SMT.SaaS.OA.UI.UserControls..BusinessApplicationsForm
            Object[] parameters = new Object[2];    // 定义构造函数需要的参数
            parameters[0] = CurrentAction;
            parameters[1] = FormID;// "5d572f2d-c0e4-49ca-960e-6bd45bfb97a9";

            object form = Activator.CreateInstance(t, parameters);
            if (form != null)
            {
                EntityBrowser entBrowser = new EntityBrowser(form);
                entBrowser.FormType = CurrentAction;
                //MessageBox.Show(CurrentAction.ToString() +"  " +FormType+ FormID);
                entBrowser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, FormID);
            }
            //CheckPermission(FormName);

        }

        /// <summary>
        /// 门户创建待审批的单据（针对新平台） 2012-8-3
        /// </summary>
        /// <param name="FormID"></param>
        /// <param name="FormName"></param>
        public static void CreateFormFromMvcPlat(string StrFormID, string StrFormName, string StrFormType)
        {
            FormID = StrFormID;
            FormName = StrFormName;
            FormType = StrFormType;

            //ChecResource();
            FormTypes CurrentAction = FormTypes.Audit;
            if (!string.IsNullOrEmpty(FormType))
            {
                switch (FormType.ToUpper())
                {
                    case "AUDIT":
                        CurrentAction = FormTypes.Audit;
                        break;
                    case "ADD":
                        CurrentAction = FormTypes.New;
                        break;
                    case "EDIT":
                        CurrentAction = FormTypes.Edit;
                        break;
                    case "VIEW":
                        CurrentAction = FormTypes.Browse;
                        break;
                }
            }

            Type t = Type.GetType(FormName);
            // SMT.SaaS.OA.UI.UserControls..BusinessApplicationsForm
            Object[] parameters = new Object[2];    // 定义构造函数需要的参数
            parameters[0] = CurrentAction;
            parameters[1] = FormID;// "5d572f2d-c0e4-49ca-960e-6bd45bfb97a9";

            object form = Activator.CreateInstance(t, parameters);
            if (form != null)
            {
                EntityBrowser entBrowser = new EntityBrowser(form);
                entBrowser.FormType = CurrentAction;
                entBrowser.ShowMvcPlat<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { });
            }
            //CheckPermission(FormName);

        }

        public static void CreateFormFromEngine(string StrFormID, string StrFormName, string StrFormType, Border parent)
        {
            try
            {
                FormID = StrFormID;
                FormName = StrFormName;
                FormType = StrFormType;

                //ChecResource();

                FormTypes CurrentAction = FormTypes.Audit;
                if (!string.IsNullOrEmpty(FormType))
                {
                    switch (FormType.ToUpper())
                    {
                        case "AUDIT":
                            CurrentAction = FormTypes.Audit;
                            break;
                        case "ADD":
                            CurrentAction = FormTypes.New;
                            break;
                        case "EDIT":
                            CurrentAction = FormTypes.Edit;
                            break;
                        case "VIEW":
                            CurrentAction = FormTypes.Browse;
                            break;
                        case "RESUBMIT":
                            CurrentAction = FormTypes.Resubmit;
                            break;
                    }
                }

                Type t = Type.GetType(FormName);

                Object[] parameters = new Object[2];    // 定义构造函数需要的参数
                parameters[0] = CurrentAction;
                parameters[1] = FormID;

                object form = Activator.CreateInstance(t, parameters);

                if (form == null)
                {
                    return;
                }

                EntityBrowser entBrowser = new EntityBrowser(form);
                if (entBrowser == null)
                {
                    return;
                }

                entBrowser.FormType = CurrentAction;
                if (parent == null)
                {
                    return;
                }

                parent.Child = entBrowser;
            }
            catch
            {
                SMT.SaaS.FrameworkUI.ChildWidow.ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"),
                Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                return;
            }
        }

        public static void LoadDictss()
        {
            ServiceClient FlowDesigner = new ServiceClient();
            FlowDesigner.GetModelNameInfosComboxCompleted += (o, e) =>
            {
                List<FLOW_MODELDEFINE_T> dicts = new List<FLOW_MODELDEFINE_T>();
                dicts = e.Result == null ? null : e.Result.ToList();
                if (Application.Current.Resources["FLOW_MODELDEFINE_T"] == null)
                {
                    Application.Current.Resources.Add("FLOW_MODELDEFINE_T", dicts);
                }
            };
            //TODO: 获取模块
            FlowDesigner.GetModelNameInfosComboxAsync();
        }

        //private static void ChecResource()
        //{
        //    if (Application.Current.Resources["FLOW_MODELDEFINE_T"] == null)
        //    {
        //        Utility.LoadDictss();
        //    }
        //    if (!Application.Current.Resources.Contains("ResourceConveter"))
        //    {
        //        Application.Current.Resources.Add("ResourceConveter", new SMT.SaaS.Globalization.ResourceConveter());
        //    }
        //}


        /// <summary>
        /// 加载权限
        /// </summary>
        /// <param name="FormName"></param>
        private static void CheckPermission(string FormName)
        {
            PermissionServiceClient clientPerm = new PermissionServiceClient();

            clientPerm.GetUserPermissionByUserToUICompleted += new EventHandler<GetUserPermissionByUserToUICompletedEventArgs>(clientPerm_GetUserPermissionByUserToUICompleted);

            string strUserId = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.SysUserID;
            //ApplicationCache.CacheMenuList
            clientPerm.GetUserPermissionByUserToUIAsync(strUserId);

        }

        static void clientPerm_GetUserPermissionByUserToUICompleted(object sender, GetUserPermissionByUserToUICompletedEventArgs e)
        {
            if (e.Error == null)
            {
                if (e.Result == null)
                {
                    return;
                }
                List<V_UserPermissionUI> entPermList = e.Result.ToList();
                foreach (var fent in entPermList)
                {
                    SMT.SaaS.LocalData.V_UserPermissionUI tps = new SMT.SaaS.LocalData.V_UserPermissionUI();
                    tps = SMT.SAAS.Main.CurrentContext.Common.CloneObject<V_UserPermissionUI, SMT.SaaS.LocalData.V_UserPermissionUI>(fent, tps);
                    SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.PermissionInfoUI.Add(tps);
                }
                ShowForm();
            }
        }

        /// <summary>
        /// 打开Form表单
        /// </summary>
        private static void ShowForm()
        {
            FormTypes CurrentAction = FormTypes.Audit;
            if (!string.IsNullOrEmpty(FormType))
            {
                switch (FormType.ToUpper())
                {
                    case "AUDIT":
                        CurrentAction = FormTypes.Audit;
                        break;
                    case "ADD":
                        CurrentAction = FormTypes.New;
                        break;
                    case "EDIT":
                        CurrentAction = FormTypes.Edit;
                        break;
                    case "VIEW":
                        CurrentAction = FormTypes.Browse;
                        break;
                }
            }

            Type t = Type.GetType(FormName);
            // SMT.SaaS.OA.UI.UserControls..BusinessApplicationsForm
            Object[] parameters = new Object[2];    // 定义构造函数需要的参数
            parameters[0] = CurrentAction;
            parameters[1] = FormID;// "5d572f2d-c0e4-49ca-960e-6bd45bfb97a9";

            object form = Activator.CreateInstance(t, parameters);
            if (form != null)
            {
                EntityBrowser entBrowser = new EntityBrowser(form);
                entBrowser.FormType = CurrentAction;
                entBrowser.TitleContent = "出差申请";
                entBrowser.Show<string>(DialogMode.Default, SMT.SAAS.Main.CurrentContext.Common.ParentLayoutRoot, "", (result) => { }, true, FormID);

                if (FormName == "SMT.SaaS.OA.UI.UserControls.BusinessApplicationsForm")
                {
                    WindowsManager.MaxWindow(entBrowser.ParentWindow);
                    //entBrowser.ParentWindow.Height = 900;
                    //entBrowser.ParentWindow.Width = 1145;
                }
                else if (FormName == "SMT.SaaS.OA.UI.UserControls.BusinessApplicationsForm")
                {

                    WindowsManager.MaxWindow(entBrowser.ParentWindow);
                    //entBrowser.ParentWindow.Height = 900;
                    //entBrowser.ParentWindow.Width = 1060;
                }
                else if (FormName == "SMT.SaaS.OA.UI.UserControls.BusinessApplicationsForm")
                {
                    WindowsManager.MaxWindow(entBrowser.ParentWindow);
                    //entBrowser.ParentWindow.Height = 900;
                    //entBrowser.ParentWindow.Width = 1180;
                }
                else
                {
                    entBrowser.ParentWindow.Height = 900;
                    entBrowser.ParentWindow.Width = 900;
                }
            }
        }

        /// <summary>
        /// 显示报销外部单据  add zl 2012.1.6
        /// </summary>
        /// <param name="strAssemblyName"></param>
        /// <param name="strFormCode"></param>
        /// <param name="strFormId"></param>
        public static void ShowExtenForm(string strOrderType, string strFormId)
        {
            if (string.IsNullOrWhiteSpace(strOrderType) || string.IsNullOrWhiteSpace(strFormId))
            {
                return;
            }

            try
            {
                string AssemblyName = String.Empty;
                string PublicClass = String.Empty;
                string ProcessName = String.Empty;
                string FormType = String.Empty;
                string defaultVersion = " , Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                string PageParameter = String.Empty;
                if (strOrderType == "CCBX")
                {
                    AssemblyName = "SMT.SaaS.OA.UI";
                    PublicClass = "SMT.SaaS.OA.UI.Utility";
                    ProcessName = "CreateFormFromEngine";
                    FormType = "Browse";
                    PageParameter = "SMT.SaaS.OA.UI.UserControls.BusinessApplicationsForm";
                }

                string ApplicationOrder = strFormId;
                StringBuilder typeString = new StringBuilder();
                typeString.Append(PublicClass);
                typeString.Append(", ");
                typeString.Append(AssemblyName);
                typeString.Append(defaultVersion);

                Type type = Type.GetType(typeString.ToString());

                Type[] types = new Type[] { typeof(string), typeof(string), typeof(string) };
                MethodInfo method = type.GetMethod(ProcessName, types);
                method.Invoke(null, BindingFlags.Static | BindingFlags.InvokeMethod, null, new object[] { ApplicationOrder, PageParameter, FormType }, null);
            }
            catch (Exception ex)
            {
                SMT.SaaS.FrameworkUI.Common.Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "当前单据丢失或读取错误，请联系管理员！" + ex.ToString());
                return;
            }
        }


        /// <summary>
        /// 根据PostId从岗位列表缓存内取出岗位名称
        /// </summary>
        /// <param name="strPostId"></param>
        /// <returns></returns>
        public static string GetPostNameByPostId(string strPostId)
        {
            string strRes = string.Empty;
            List<SMT.Saas.Tools.OrganizationWS.T_HR_POST> entPostList = App.Current.Resources["SYS_PostInfo"] as List<SMT.Saas.Tools.OrganizationWS.T_HR_POST>;

            if (entPostList == null)
            {
                return strRes;
            }

            if (entPostList.Count() == 0)
            {
                return strRes;
            }

            SMT.Saas.Tools.OrganizationWS.T_HR_POST entPost = entPostList.Where(c => c.POSTID == strPostId).FirstOrDefault();

            if (entPost == null)
            {
                return strRes;
            }

            if (entPost.T_HR_POSTDICTIONARY == null)
            {
                return strRes;
            }

            strRes = entPost.T_HR_POSTDICTIONARY.POSTNAME;

            return strRes;
        }




        #region 新上传控件
        public static void InitFileLoad(string strModelCode, string strApplicationID, FormTypes action, FileUpLoad.FileControl control)
        {
            InitFileLoad(strModelCode, strApplicationID, action, control, true);
        }
        public static void InitFileLoad(string strModelCode, string strApplicationID, FormTypes action, FileUpLoad.FileControl control, bool AllowDelete)
        {
            SMT.FileUpLoad.Classes.UserConfig uc = new SMT.FileUpLoad.Classes.UserConfig();
            uc.CompanyCode = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            uc.SystemCode = "FB";
            uc.ModelCode = strModelCode;
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
            uc.MaxSize = "1000.MB";
            uc.CreateName = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeName;
            uc.PageSize = 4;
            control.Init(uc);
        }

        public static void InitFileLoad(FormTypes action, FileUpLoad.FileControl control, string StrApplicationId, bool AllowDelete)
        {
            SMT.FileUpLoad.Classes.UserConfig uc = new SMT.FileUpLoad.Classes.UserConfig();
            uc.CompanyCode = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            uc.SystemCode = "FB";
            uc.IsLimitCompanyCode = false;
            uc.UserID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            uc.ApplicationID = StrApplicationId;

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
            //uc.CreateName = Common.CurrentLoginUserInfo.EmployeeName;
            uc.PageSize = 20;
            control.Init(uc);
        }
        #endregion


    }
}
