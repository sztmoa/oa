using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Threading;

namespace SMT.Portal.Common.SmtForm.Framework
{
    public class Functions
    {
        #region -- 其他方法 --
        public static string Input(string requestKey)
        {
            return HttpContext.Current.Request[requestKey];
        }

        public static bool IsNotNull(string str)
        {
            if (str != null && str != "")
                return true;
            else
                return false;
        }



        //生成随机数
        public static char MakeRndNum()
        {
            System.Random random1 = new Random();
            return (char)random1.Next(65, 91);
        }

        //生成随机字符，包括英文和数字
        public static string MakeRndStr(int i)
        {
            string Rndstr = "";
            Thread.Sleep(10);
            System.Random Rnd = new Random();
            int j = 0;
            while (j < i)
            {
                int FileRnd = Rnd.Next(48, 90);
                if ((FileRnd >= 48 && FileRnd <= 57) || (FileRnd >= 65 && FileRnd <= 90))
                {
                    j++;
                    Rndstr += ((char)FileRnd);
                }
            }
            return Rndstr;
        }

        //生成随机字符，只有数字
        public static string MakeRndNum(int i)
        {
            string Rndstr = "";
            Thread.Sleep(10);
            System.Random Rnd = new Random();
            int j = 0;
            while (j < i)
            {
                int FileRnd = Rnd.Next(48, 57);
                if ((FileRnd >= 48 && FileRnd <= 57))
                {
                    j++;
                    Rndstr += ((char)FileRnd);
                }
            }
            return Rndstr;
        }


        //生成随机字符，只有英文
        public static string MakeRndChar(int i)
        {
            string Rndstr = "";
            Thread.Sleep(10);
            System.Random Rnd = new Random();
            int j = 0;
            while (j < i)
            {
                int FileRnd = Rnd.Next(65, 90);
                if ((FileRnd >= 65 && FileRnd <= 90))
                {
                    j++;
                    Rndstr += ((char)FileRnd);
                }
            }
            return Rndstr;
        }

        public static string GeneratePassword()
        {
            int length = 8;
            string pword = "";
            for (int x = 0; x <= length; x++)
            {
                if (RandomNumber(1, 6) <= 3)
                {// alphabet
                    pword += RandomCharacter();
                }
                else
                {// number
                    pword += RandomNumber(1, 9).ToString();
                }
            }
            return pword;
        }// end generate password

        public static int RandomNumber(int numDice, int diceType)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] b = new byte[1];
            rng.GetBytes(b);
            Random r = new Random(b[0]);
            int maxValue = numDice * diceType;
            int tmpTotal = 0;
            for (int i = 0; i < numDice; i++)
            {
                tmpTotal += r.Next(1, diceType);
            }
            if (tmpTotal > maxValue) { tmpTotal = maxValue; } // Double check the max.
            if (tmpTotal < numDice) { tmpTotal = numDice; }; // Double Check the min.
            return tmpTotal;
        } // end dice function

        public static string RandomCharacter()
        {
            String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            int x = RandomNumber(1, chars.Length);
            return chars.Substring(x, 1);
        }



        /// <summary>
        /// 按字节数获取字符串，中文算两个字节
        /// </summary>
        /// <param name="strSrc"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetStrByByteCount(string strSrc, int count)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
            char[] stringChar = strSrc.ToCharArray();
            StringBuilder sb = new StringBuilder();
            int nLength = 0;
            bool isCut = false;
            for (int i = 0; i < stringChar.Length; i++)
            {
                if (regex.IsMatch((stringChar[i]).ToString()))
                {
                    sb.Append(stringChar[i]);
                    nLength += 2;
                }
                else
                {
                    sb.Append(stringChar[i]);
                    nLength = nLength + 1;
                }

                if (nLength > count)
                {
                    isCut = true;
                    break;
                }
            }
            if (isCut)
                return sb.ToString() + "...";
            else
                return sb.ToString();
        }


        //生成模式窗口，如果不采用此Iframe方式，在IE5.0下有些操作会出错
        public static void ModalDialogFrame(string Title)
        {
            if (HttpContext.Current.Request["modaldialogframetype"] != "iframe")
            {
                string newurl = "";
                if (HttpContext.Current.Request.Url.ToString().IndexOf("?") != -1)
                {
                    newurl = HttpContext.Current.Request.Url.ToString() + "&modaldialogframetype=iframe";
                }
                else
                {
                    newurl = HttpContext.Current.Request.Url.ToString() + "?modaldialogframetype=iframe";
                }

                HttpContext.Current.Response.Write("<title>" + Title + "</title><style> BODY { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; FONT-SIZE: 12px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px; FONT-FAMILY: 'verdana','Arial', 'Helvetica', 'sans-serif'; BACKGROUND-COLOR: #ffffff; TEXT-ALIGN: center } </style> <iframe src='" + newurl + "' height='100%' width='100%' marginwidth='0' marginheight='0' frameborder='0' scrolling='no' vspace='0' hspace='0'  style='OVERFLOW-Y:no;OVERFLOW-X:no'  id='ModalFrame'></iframe>");
                HttpContext.Current.Response.End();
            }

        }




        //模式窗口使用Iframe
        public static void ModalDialogFrame()
        {
            if (HttpContext.Current.Request["modaldialogframetype"] != "iframe")
            {
                string newurl = "";
                if (HttpContext.Current.Request.Url.ToString().IndexOf("?") != -1)
                {
                    newurl = HttpContext.Current.Request.Url.ToString() + "&modaldialogframetype=iframe";
                }
                else
                {
                    newurl = HttpContext.Current.Request.Url.ToString() + "?modaldialogframetype=iframe";
                }
                HttpContext.Current.Response.Write("<style> BODY { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; FONT-SIZE: 12px; PADDING-BOTTOM: 0px; MARGIN: 0px; PADDING-TOP: 0px; FONT-FAMILY: 'verdana','Arial', 'Helvetica', 'sans-serif'; BACKGROUND-COLOR: #ffffff; TEXT-ALIGN: center } </style> <iframe src='" + newurl + "' height='100%' width='100%' marginwidth='0' marginheight='0' frameborder='0' scrolling='no' vspace='0' hspace='0'  style='OVERFLOW-Y:no;OVERFLOW-X:no' ></iframe>");
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// sql参数过滤
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ParamFilter(object param)
        {
            if (param != null)
            {
                string resultParam = ((string)param).Replace("'", "''");
                return resultParam;
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 拼接字符串
        /// </summary>
        /// <param name="resultStr">结果字串</param>
        /// <param name="addStr">要加入的字串</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string LinkString(string resultStr, string addStr, string separator)
        {
            if (resultStr != "")
            {
                resultStr += separator;
            }
            resultStr += addStr;
            return resultStr;
        }


        public static string FormatDate(System.DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm");
        }


        #endregion

        #region -- Html编码解码 --
        public static string HtmlEncode(string str)
        {
            if (str != null)
            {
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("  ", " &nbsp;");
                //s_str = s_str.Replace("'","&#39;");
                str = str.Replace("\r\n", "<br>");
            }
            return str;
        }



        public static string HtmlDecode(string str)
        {
            if (str != null)
            {
                str = str.Replace("&nbsp;", " ");
                str = str.Replace("<br>", "\r\n");
                str = str.Replace("&lt;", "<");
                str = str.Replace("&gt;", ">");
                //s_str = s_str.Replace("&#39;","'");
            }

            return str;
        }

        /// <summary>
        /// 用于不需要对多文本输入框html编码，但需要对其进行换行处理时
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>替换后的返回值 </returns>
        public static string TextAreaEncode(string str)
        {
            if (str != null)
            {
                str = str.Replace("  ", " &nbsp;");
                str = str.Replace("\r\n", "<br>");
            }
            return str;
        }

        /// <summary>
        /// 用于不需要对多文本输入框html编码，但需要对其进行换行处理时
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>替换后的返回值 </returns>
        public static string TextAreaDecode(string str)
        {
            if (str != null)
            {
                str = str.Replace("&nbsp;", "  ");
                str = str.Replace("<br>", "\r\n");
                str = str.Replace("<BR>", "\r\n");
            }
            return str;
        }

        #endregion

        #region -- 加密解密 --


        /// <summary> 
        /// Encrypt the string 
        /// Attention:key must be 8 bits 
        /// </summary> 
        /// <param name="strText">string</param> 
        /// <param name="strEncrKey">key</param> 
        /// <returns></returns> 
        public static string DesEncrypt(string strText, string strEncrKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            try
            {
                byKey = Encoding.UTF8.GetBytes(strEncrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());


            }
            catch (Exception error)
            {
                return "error:" + error.Message + "\r";
            }
        }

        /// <summary> 
        /// Decrypt string 
        /// Attention:key must be 8 bits 
        /// </summary> 
        /// <param name="strText">Decrypt string</param> 
        /// <param name="sDecrKey">key</param> 
        /// <returns>output string</returns> 
        public static string DesDecrypt(string strText, string sDecrKey)
        {
            byte[] byKey = null;
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = new Byte[strText.Length];
            try
            {
                byKey = Encoding.UTF8.GetBytes(sDecrKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = new UTF8Encoding();
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception error)
            {
                return "error:" + error.Message + "\r";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PasswordString"></param>
        /// <returns></returns>
        public static string EncryptPassword(string PasswordString)
        {
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordString, "MD5");
        }
        #endregion


        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="FileLength">文件长度</param>
        /// <returns></returns>
        public static string GetFileSize(int FileLength)
        {
            if (FileLength > 0)
            {
                if (FileLength > 0x100000)
                {
                    return string.Format("{0:F2}M", ((float)FileLength) / 1048576f);
                }
                return string.Format("{0:F2}K", ((float)FileLength) / 1024f);
            }
            return string.Empty;

        }

        /// <summary>
        /// 检测URL文件是否存在   //不包括localhost
        /// </summary>
        /// <param name="sURL"></param>
        /// <returns></returns>
        public static bool UrlExist(string sURL)
        {
            bool bExists = false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);

            request.Method = "HEAD";
            request.AllowAutoRedirect = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode == HttpStatusCode.OK)
                bExists = true;

            response.Close();
            return bExists;
        }


        public static IList ConvertEnumToList(Type enumType)
        {
            ArrayList list = new ArrayList();

            foreach (object obj in System.Enum.GetValues(enumType))
            {
                ListItem listitem = new ListItem(System.Enum.GetName(enumType, obj), System.Enum.Format(enumType, System.Enum.Parse(enumType, obj.ToString()), "d"));

                list.Add(listitem);
            }
            return list;
        }


        public static string GetIPAddress()
        {
            {
                string result = String.Empty;

                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (result != null && result != String.Empty)
                {
                    //可能有代理 
                    if (result.IndexOf(".") == -1)     //没有“.”肯定是非IPv4格式 
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。 
                            result = result.Replace(" ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {
                                if (IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];     //找到不是内网的地址 
                                }
                            }
                        }
                        else if (IsIPAddress(result)) //代理即是IP格式 
                            return result;
                        else
                            result = null;     //代理中的内容 非IP，取IP 
                    }

                }

                string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (null == result || result == String.Empty)
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (result == null || result == String.Empty)
                    result = HttpContext.Current.Request.UserHostAddress;

                return result;
            }

        }
        /// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }


        //将IP 地址转化为数字
        private static long IPtoNum(string Ip)
        {
            string[] stringip = new string[4];
            stringip = Ip.Split('.');
            long ipnum = Convert.ToInt64((stringip[0])) * 16777216 + Convert.ToInt64(stringip[1]) * 65536 + Convert.ToInt64(stringip[2]) * 256 + Convert.ToInt64(stringip[3]);
            return ipnum;
        }
        public static string RichTextEncode(string str)
        {
            if (str != null)
            {
                str = str.Replace("<script>", "&lt;script&gt;");
                str = str.Replace("'", "''");
            }
            return str;
        }
        public static string RichTextDecode(string str)
        {
            if (str != null)
            {

            }
            return str;
        }


        /// <summary>
        /// 循环遍历子控件
        /// </summary>
        /// <param name="sourceControl"></param>
        /// <param name="controlID"></param>
        /// <returns></returns>
        public static Control LoopingControls(Control parentControl, string id)
        {
            System.Web.UI.Control control = null;
            //先使用 FindControl 去查找指定的子控件
            control = parentControl.FindControl(id);
            if (control != null && control.ID != id) control = null;
            //如果未找到则往下层递归方式去查找
            if (control == null)
            {
                foreach (System.Web.UI.Control oChildCtrl in parentControl.Controls)
                {
                    //以递归方式回调原函数
                    control = LoopingControls(oChildCtrl, id);
                    //如果找到指定控件则退出循环
                    if (control != null && control.ID == id) break;
                }
            }
            return control;
        }



        /// <summary>
        /// 显示对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="continueExecute"></param>
        public static void ShowMessage(Page page, string message, bool continueExecute)
        {

            if (continueExecute == false)
            {
                page.RegisterClientScriptBlock("topMessage", "<script>top.ShowMessage('" + message + "','',window);history.back(-1);</script>");
                HttpContext.Current.Response.End();
            }
            else
            {
                page.RegisterClientScriptBlock("topMessage", "<script>top.ShowMessage('" + message + "','',window);</script>");
            }
            //Response.End();
        }


        /// <summary>
        /// 过滤特殊字符
        /// </summary>
        /// <param name="value">要过滤的字符</param>
        /// <returns>返回过滤后的字符</returns>
        public static string Filter(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = Regex.Replace(value, @";", string.Empty);
            value = Regex.Replace(value, @"'", string.Empty);
            value = Regex.Replace(value, @"&", string.Empty);
            value = Regex.Replace(value, @"%20", string.Empty);
            value = Regex.Replace(value, @"--", string.Empty);
            value = Regex.Replace(value, @"==", string.Empty);
            value = Regex.Replace(value, @"<", string.Empty);
            value = Regex.Replace(value, @">", string.Empty);
            value = Regex.Replace(value, @"%", string.Empty);

            return value;
        }

        /// <summary>
        /// 把参数转化成整型数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(object value)
        {
            int result = 0;

            if (value != null && value.ToString().Length >= 1)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }
        /// <summary>
        /// 把参数转化成长整型数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static long ToLong(object value)
        {
            long result = 0;

            if (value != null && value.ToString().Length >= 1)
            {
                result = Convert.ToInt64(value);
            }

            return result;
        }
        /// <summary>
        /// 把参数转化成字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToStr(object value)
        {
            string result = "";

            if (value != null && (value.ToString().Length >= 1))
            {
                result = value.ToString();
            }

            return result;
        }

        /// <summary>
        /// 把参数转化成日期时间类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(object value)
        {
            DateTime result = DateTime.MinValue;

            if (value != null && value != DBNull.Value && (value.GetType() == typeof(string) && value.ToString() != ""))
            {
                result = Convert.ToDateTime(value);
            }

            return result;
        }

        /// <summary>
        /// 把参数转化成布尔类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ToBool(object val)
        {
            if (val == null)
            {
                return false;
            }
            if (!(val.ToString() == "1") && !(val.ToString().ToLower() == "true"))
            {
                return (val.ToString().ToLower() == "y");
            }
            return true;
        }

        /// <summary>
        /// 把参数转化成Decimal类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(object value)
        {
            decimal result = 0M;

            if (value != null && value.ToString().Length >= 1)
            {
                result = Convert.ToDecimal(value);
            }

            return result;
        }

        /// <summary>
        /// 把参数转化成Decimal类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double ToDouble(object value)
        {
            Double result = 0D;

            if (value != null && value.ToString().Length >= 1)
            {
                result = Convert.ToDouble(value);
            }

            return result;
        }

        public static bool getbool(object val)
        {
            if (val == null)
            {
                return false;
            }
            if (!(val.ToString() == "1") && !(val.ToString().ToLower() == "true"))
            {
                return (val.ToString().ToLower() == "y");
            }
            return true;
        }
        /// <summary>
        /// 用来截取小数点后nleng位
        /// </summary>
        /// <param name="sString">sString原字符串。</param>
        /// <param name="nLeng">nLeng长度。</param>
        /// <returns>处理后的字符串。</returns>
        public static string VarStr(string sString, int nLeng)
        {
            int index = sString.IndexOf(".");
            if (index == -1 || index + 2 >= sString.Length)
                return sString;
            else
                return sString.Substring(0, (index + nLeng + 1));
        }

        /// <summary>
        /// 用来截取字符串
        /// </summary>
        /// <param name="sString">sString原字符串。</param>
        /// <param name="nLeng">要截取的长度</param>
        /// <returns>处理后的字符串。</returns>
        public static string VarStrSub(string sString, int nLeng)
        {
            if (sString.Length <= nLeng)
                return sString;

            return sString.Substring(0, nLeng);
        }


        /// <summary>
        /// 验证一个字符串是否为数字
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>true表示为数字，false表示不是数字</returns>
        public static bool IsNumber(string str)
        {
            string pattern = @"^[1-9]\d*$";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(str);

            if (!match.Success) return false;
            return true;
        }

      

        /**/
        ///<summary>   
        ///移除文本中 HTML标签   shl 
        ///</summary>   
        ///<param name="HTMLStr">HTMLStr</param>   
        public static string ParseTags(string HTMLStr)
        {
            if (null != HTMLStr && HTMLStr.Length > 0)
            {
                HTMLStr = System.Text.RegularExpressions.Regex.Replace(HTMLStr, "\n", "");
                HTMLStr = System.Text.RegularExpressions.Regex.Replace(HTMLStr, "\t", "");
                HTMLStr = System.Text.RegularExpressions.Regex.Replace(HTMLStr, "&nbsp;", "");
                HTMLStr = System.Text.RegularExpressions.Regex.Replace(HTMLStr, "<[^>]*>", "");
            }
            return HTMLStr;
        }

    }
}