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

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Data;
using System.Globalization;

using SMT.Saas.Tools.PermissionWS;
using SMT.Saas.Tools.OrganizationWS;

namespace SMT.FBAnalysis.UI
{
    public class DictionaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            List<T_SYS_DICTIONARY> dicts = Application.Current.Resources["SYS_DICTIONARY"] as List<T_SYS_DICTIONARY>;
            if (dicts == null)
                return value;

            var objs = from a in dicts
                       where a.DICTIONARYVALUE.ToString() == value.ToString() && a.DICTIONCATEGORY == parameter.ToString()
                       select a;

            T_SYS_DICTIONARY dict = objs.Count() > 0 ? objs.FirstOrDefault() : null;
            return dict == null ? value : dict.DICTIONARYNAME;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    /// <summary>
    /// 查询分析专用,不要删!!!
    /// </summary>
    public class CustomDictionaryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            List<T_SYS_DICTIONARY> dicts = Application.Current.Resources["SYS_DICTIONARY"] as List<T_SYS_DICTIONARY>;
            if (dicts == null)
                return value;

            //新加的
            if (parameter.ToString() == "FBARepayType")
            {
                string v = value.ToString();
                if (!String.IsNullOrWhiteSpace(v))
                {
                    var strArr = v.Split(',');
                    if (strArr.Length == 2)
                    {
                        value = strArr[1];
                        parameter = strArr[0];
                    }
                }
            }

            var objs = from a in dicts
                       where a.DICTIONARYVALUE.ToString() == value.ToString() && a.DICTIONCATEGORY == parameter.ToString()
                       select a;


            T_SYS_DICTIONARY dict = objs.Count() > 0 ? objs.FirstOrDefault() : null;
            return dict == null ? value : dict.DICTIONARYNAME;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class CompanyInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            if (parameter == null)
                return value;
            switch (parameter.ToString())
            {
                case "Company":
                    List<T_HR_COMPANY> dictc = Application.Current.Resources["SYS_CompanyInfo"] as List<T_HR_COMPANY>;
                    if (dictc == null)
                        return value;
                    var objc = from a in dictc
                               where a.COMPANYID == value.ToString()
                               select a.CNAME;
                    return objc.Count() > 0 ? objc.FirstOrDefault() : value;
                case "Department":
                    List<T_HR_DEPARTMENT> dictd = Application.Current.Resources["SYS_DepartmentInfo"] as List<T_HR_DEPARTMENT>;
                    if (dictd == null)
                        return value;
                    var objd = from a in dictd
                               where a.DEPARTMENTID == value.ToString()
                               select a.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                    return objd.Count() > 0 ? objd.FirstOrDefault() : value;
                case "Post":
                    List<T_HR_POST> dictp = Application.Current.Resources["SYS_PostInfo"] as List<T_HR_POST>;
                    if (dictp == null)
                        return value;
                    var objp = from a in dictp
                               where a.POSTID == value.ToString()
                               select a.T_HR_POSTDICTIONARY.POSTNAME;
                    return objp.Count() > 0 ? objp.FirstOrDefault() : value;
                case "OrgDynamic":
                    return GetValueByID(value);

            }
            return null;
        }

        private object GetValueByID(object value)
        {
            string strRes = string.Empty;
            List<T_HR_COMPANY> dictc = Application.Current.Resources["SYS_CompanyInfo"] as List<T_HR_COMPANY>;
            if (dictc != null)
            {
                var objc = from a in dictc
                           where a.COMPANYID == value.ToString()
                           select a.CNAME;

                if (objc.Count() > 0)
                {
                    strRes = objc.FirstOrDefault();
                }
            }

            if (string.IsNullOrWhiteSpace(strRes))
            {
                List<T_HR_DEPARTMENT> dictd = Application.Current.Resources["SYS_DepartmentInfo"] as List<T_HR_DEPARTMENT>;
                if (dictd != null)
                {
                    var objd = from a in dictd
                               where a.DEPARTMENTID == value.ToString()
                               select a.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME;
                    if (objd.Count() > 0)
                    {
                        strRes = objd.FirstOrDefault();
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(strRes))
            {
                List<T_HR_POST> dictp = Application.Current.Resources["SYS_PostInfo"] as List<T_HR_POST>;
                if (dictp != null)
                {
                    var objp = from a in dictp
                               where a.POSTID == value.ToString()
                               select a.T_HR_POSTDICTIONARY.POSTNAME;
                    if (objp.Count() > 0)
                    {
                        strRes = objp.FirstOrDefault();
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(strRes))
            {
                return value;
            }

            return strRes;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class FlowInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            switch (parameter.ToString())
            {
                case "Flow":
                    List<T_HR_COMPANY> dictc = Application.Current.Resources["SYS_FlowInfo"] as List<T_HR_COMPANY>;
                    if (dictc == null)
                        return value;
                    var objc = from a in dictc
                               where a.COMPANYID == value.ToString()
                               select a.CNAME;
                    return objc.Count() > 0 ? objc.FirstOrDefault() : value;

            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class CustomDateConverter : IValueConverter
    {
        #region IValueConverter 成员

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(value.ToString()))
            {
                return value;
            }

            if (parameter == null)
            {
                return null;
            }

            object retValue = null;
            switch (parameter.ToString())
            {
                case "DATE":    //日期                    
                    retValue = GetDate(value);
                    break;
                case "TIME":    //小时：分
                    retValue = GetHourMin(value);
                    break;
                case "DATETIME":    //日期 小时：分
                    retValue = GetDateTime(value);
                    break;
                case "DATETIMES":    //日期 小时：分：秒
                    retValue = GetDateTimes(value);
                    break;
                default:
                    retValue = value;
                    break;
            }
            return retValue;
        }

        /// <summary>
        /// 日期格式化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetDate(object value)
        {
            DateTime dt;
            DateTime.TryParse(value.ToString(), out dt);

            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetHourMin(object value)
        {
            DateTime dtTemp = new DateTime();
            DateTime.TryParse(value.ToString(), out dtTemp);

            return dtTemp.ToString("HH:mm");
        }

        /// <summary>
        /// 日期与时间格式化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetDateTime(object value)
        {
            DateTime dt;
            DateTime.TryParse(value.ToString(), out dt);

            return dt.ToString("yyyy-MM-dd HH:mm");
        }
        /// <summary>
        /// 日期与时间格式化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetDateTimes(object value)
        {
            DateTime dt;
            DateTime.TryParse(value.ToString(), out dt);

            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value.ToString();
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, out resultDateTime))
            {
                return resultDateTime;
            }
            return value;
        }

        #endregion
    }

    public class CheckConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            object objRes = null;

            switch (parameter.ToString())
            {
                case "CN":
                    objRes = value.ToString() == "0" ? "否" : "是";
                    break;
                case "EN":
                    objRes = value.ToString() == "0" ? "Flase" : "True";
                    break;
                default:
                    objRes = value.ToString() == "0" ? false : true;
                    break;
            }

            return objRes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class RepayTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            object objRes = null;
            switch (value.ToString().Trim())
            {
                case "1":
                    objRes = "普通借款";
                    break;
                case "2":
                    objRes = "备用金借款";
                    break;
                default:
                    objRes = "专项借款";
                    break;
            }
            return objRes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

    }

    public class AccountObjectType : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            object objRes = null;
            switch (value.ToString().Trim())
            {
                case "3":
                    objRes = "个人预算费用";
                    break;
                default:
                    objRes = "部门预算费用";
                    break;
            }
            return objRes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class SubjectUsableMoneyFormat : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            decimal a = 0;
            decimal.TryParse(value.ToString(), out a);

            if (a == 999999 || a == 99999999)
            {
                return "不受月度预算限制";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class ChargeTpyeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            object objRes = null;
            switch (value.ToString().Trim())
            {
                case "1":
                    objRes = "个人预算费用";
                    break;
                default:
                    objRes = "部门预算费用";
                    break;
            }
            return objRes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class PercentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            object objRes = null;
            decimal dValue = 0;
            decimal.TryParse(value.ToString(), out dValue);
            dValue = decimal.Round(dValue * 100, 2);
            string[] strlist = dValue.ToString().Split('.');
            string strTemp = string.Empty;
            strTemp = strlist[0];


            if (strlist.Length == 2)
            {
                strTemp += "." + strlist[1].PadRight(2, '0');
            }
            else
            {
                strTemp += ".00";
            }

            objRes = strTemp + "%";
            return objRes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class FinanceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            object objRes = null;
            decimal dValue = 0;
            decimal.TryParse(value.ToString(), out dValue);
            objRes = dValue.ToString("N");
            return objRes;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}
