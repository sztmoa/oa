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

using System.Reflection;
using SMT.SaaS.FrameworkUI.Common;
using SMT.SAAS.Utility;
using SMT.SaaS.FrameworkUI;
using SMT.SaaS.FrameworkUI.ChildWidow;
using System.Collections.Generic;
using System.Text;

namespace SMT.FBAnalysis.UI
{
    public class CommonTools
    {
        public class MultiValuesItem<T> where T : class
        {
            public List<T> Values { get; set; }
            public string Text { get; set; }
        }

        public static void InitComonConverter()
        {
            if (Application.Current.Resources["CustomDictionaryConverter"] == null)
            {
                Application.Current.Resources.Add("CustomDictionaryConverter", new SMT.FBAnalysis.UI.CustomDictionaryConverter());
            }

            if (Application.Current.Resources["CustomDateConverter"] == null)
            {
                Application.Current.Resources.Add("CustomDateConverter", new SMT.FBAnalysis.UI.CustomDateConverter());
            }

            if (Application.Current.Resources["CheckConverter"] == null)
            {
                Application.Current.Resources.Add("CheckConverter", new SMT.FBAnalysis.UI.CheckConverter());
            }

            if (Application.Current.Resources["CompanyInfoConverter"] == null)
            {
                Application.Current.Resources.Add("CompanyInfoConverter", new SMT.FBAnalysis.UI.CompanyInfoConverter());
            }
        }

        public static string GetResourceStr(string message)
        {
            string rslt = SMT.SaaS.Globalization.Localization.ResourceMgr.GetString(message, SMT.SaaS.Globalization.Localization.UiCulture);
            return string.IsNullOrEmpty(rslt) ? message : rslt;
        }
        public static string GetResourceStr(string message, string parameter)
        {
            string rslt = SMT.SaaS.Globalization.Localization.GetString(message, parameter);

            return string.IsNullOrEmpty(rslt) ? message : rslt;
        }

        /// <summary>
        /// 确认信息
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public static void ShowCustomMessage(MessageTypes messageType, string title, string message)
        {
            ComfirmWindow.ConfirmationBox(title, message, GetResourceStr("CONFIRMBUTTON"));
        }

        /// <summary>
        ///CheckBox CBAll = sender as CheckBox;
        ///var grid = Utility.FindParentControl<DataGrid>(CBAll);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T FindParentControl<T>(DependencyObject item) where T : class
        {
            if (item != null)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(item);
                T parentGrid = parent as T;
                return (parentGrid != null) ? parentGrid : FindParentControl<T>(parent);
            }
            return null;
        }

        public static void ShowFBForm(string strAssemblyName, string strFormCode, string strFormId)
        {
            if (string.IsNullOrWhiteSpace(strFormCode) || string.IsNullOrWhiteSpace(strFormId))
            {
                return;
            }

            try
            {
                string AssemblyName = "SMT.FB.UI";
                string PublicClass = "SMT.FB.UI.Common.CommonFunction";
                string ProcessName = "ShowEditForm";
                string FormType = "BROWSE";
                string defaultVersion = " , Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                if (strAssemblyName != AssemblyName)
                {
                    AssemblyName = "SMT.FBAnalysis.UI";
                    PublicClass = "SMT.FBAnalysis.UI.Common.Utility";
                    ProcessName = "CreateFormFromEngine";
                    FormType = "VIEW";
                }
                string PageParameter = strFormCode; //例： "T_FB_BORROWAPPLYMASTER"
                string ApplicationOrder = strFormId;

                switch (strFormCode)
                {
                    case "T_FB_CHARGEAPPLYMASTER":
                        AssemblyName = "SMT.FBAnalysis.UI";
                        PublicClass = "SMT.FBAnalysis.UI.Common.Utility";
                        ProcessName = "CreateFormFromEngine";
                        FormType = "VIEW";
                        PageParameter = "SMT.FBAnalysis.UI.Form.ChargeApplyForm";
                        break;
                    case "T_FB_REPAYAPPLYMASTER":
                        AssemblyName = "SMT.FBAnalysis.UI";
                        PublicClass = "SMT.FBAnalysis.UI.Common.Utility";
                        ProcessName = "CreateFormFromEngine";
                        FormType = "VIEW";
                        PageParameter = "SMT.FBAnalysis.UI.Form.RepayApplyForm";
                        break;
                    case "T_FB_BORROWAPPLYMASTER":
                        AssemblyName = "SMT.FBAnalysis.UI";
                        PublicClass = "SMT.FBAnalysis.UI.Common.Utility";
                        ProcessName = "CreateFormFromEngine";
                        FormType = "VIEW";
                        PageParameter = "SMT.FBAnalysis.UI.Form.BorrowAppForm";
                        break;
                    case "SMT.FBAnalysis.UI.Form.ChargeApplyForm":
                    case "SMT.FBAnalysis.UI.Form.BorrowAppForm":
                    case "SMT.FBAnalysis.UI.Form.RepayApplyForm":
                        break;
                    default:
                        AssemblyName = "SMT.FB.UI";
                        PublicClass = "SMT.FB.UI.Common.CommonFunction";
                        ProcessName = "ShowEditForm";
                        FormType = "BROWSE";
                        PageParameter = strFormCode;
                        break;
                }

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
                Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), "当前单据丢失或读取错误，请联系管理员！" + ex.ToString());
            }
        }
    }
}
