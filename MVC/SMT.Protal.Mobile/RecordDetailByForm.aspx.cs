using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Text;
using System.Collections;
using System.IO;
using System.Globalization;
using System.Web.UI.HtmlControls;
using SMT.Portal.Common.SmtForm.Framework;
using SMT.Portal.Common.SmtForm.BLL;
using SMT.Portal.Common.SmtForm.Logic;
using System.Xml.Linq;
using SMT.SaaS.Services;


namespace SMT.Portal.Common.SmtForm
{
    public partial class RecordDetailByForm : PageBase
    {
        public string MessageID = string.Empty;
        public string billText;
        public string SystemCode = string.Empty;
        public string PERSONALRECORDID = string.Empty;
        string error = "";
        bool IsApproval = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            MessageID = Request["MessageID"] + "";
            //MessageID = "d0235c96-6473-4ed0-a34d-c156fba9b115";
            SystemCode = Request["SystemCode"] + "";
            //SystemCode = "OA";
            PERSONALRECORDID = Request["PERSONALRECORDID"] + "";

            string xml = Get_Record_DetailByForm(SystemCode + "_" + MessageID);
            //string xml = MobileService.Get_Record_DetailByForm(PERSONALRECORDID);

            ObjBill bill = null;
            BillView billView = new BillView();

            if (null != xml && xml.Length > 0)
            {
                try
                {
                    bill = billView.CreateView(this.pnl1, xml, IsApproval, ref error);
                    if (bill != null)
                    {
                        billText = bill.Text;
                    }
                }
                catch 
                {
                    Response.Write(error);
                }
                
            }
            else
            {
                Response.Write("对不起，当前数据版本不对，请在电脑上查看");
            }

        }

        #region 我的单据详细信息 PC 使用
        /// <summary>
        /// 我的单据详细信息
        /// </summary>
        /// <param name="FormidSystemcode">表单ID和systemcode，格式：SystemCode_FormID</param>
        /// <returns></returns>
        public static string Get_Record_DetailByForm(string FormidSystemcode)
        {
            try
            {
                #region 获取单据代码块

                DateTime start = DateTime.Now;
                EngineService engineService = new EngineService();
                SMT.SaaS.Services.EngineWS.T_FLOW_ENGINEMSGLIST detail = engineService.PendingDetailTasksByOrderNodeCodeForMobile(FormidSystemcode);
                if (detail == null)
                {
                    throw new Exception("调用我的单据Get_Record_DetailByForm方法中globalClient.PendingDetailTasksByOrderNodeCodeForMobile" +
                    "没有找到【" + FormidSystemcode + "】的数据");
                }
                DateTime end = DateTime.Now;
                //Common.CallWcfLog("PendingDetailTasksByOrderNodeCodeForMobile", "调用与返回", "我的单据详细信息", start, end);
                string OrderType = "";//是否是咨询单
                if (detail.ORDERNODECODE.IndexOf("Consultati") > -1)
                {
                    OrderType = "Consultation";
                }
                #region 过滤需要编辑的单据
                if (detail.FLOWXML == "" || detail.FLOWXML == null)
                {
                    return GetXmlIsValid("1", "该单据需要在电脑上编辑提交审核。");
                }
                #endregion

                #region 没有Version节点表示为测试数据。
                if ((from c in XElement.Parse(detail.APPXML).Elements("Version") select c).FirstOrDefault() == null)
                {
                    //return GetXmlIsValid("2", "该单据数据版本低，请在电脑上审核。");
                    return GetXmlIsValid("2", "该单据数据版本低，请在电脑上查看。");
                }
                #endregion
                var xmlroots = (from c in XElement.Parse(detail.APPXML).Elements("Object") select c).FirstOrDefault();
                string modelCodeValue = xmlroots.Attribute("Name").Value;
                string modelNameValue = xmlroots.Attribute("Description").Value;
                string formid = xmlroots.Attribute("id").Value;
                bool isAttach = false;//是否有附件(true有附件，false没有附件)
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                stringBuilder.Append("<System>");
                stringBuilder.Append("<Result Code=\"0\"><![CDATA[成功]]></Result>");
                stringBuilder.Append("<Form  FormID=\"" + formid + "\" ModelCode=\"" + modelCodeValue + "\" MESSAGESTATUS=\"" +
                    detail.MESSAGESTATUS.ToLower() + "\" OrderType=\"" + OrderType.ToLower() + "\" Text=\"" + modelNameValue + "\" ></Form>");
                stringBuilder.Append("<Object Name=\"" + modelCodeValue + "\" Text=\"" + modelNameValue + "\">");
                #region 主表数据
                var xmlitems = from c in XElement.Parse(detail.APPXML).Elements("Object").Elements("Attribute") select c;//主表
                for (int i = 0; i < xmlitems.Count(); i++)
                {
                    string name = xmlitems.ElementAt(i).Attribute("Name").Value;
                    string text = xmlitems.ElementAt(i).Attribute("Description").Value;
                    string datatype = xmlitems.ElementAt(i).Attribute("DataType").Value;
                    string datatext = EscapeXML(xmlitems.ElementAt(i).Attribute("DataText").Value);
                    string datavalue = EscapeXML(xmlitems.ElementAt(i).Attribute("DataValue").Value);
                    string isencryption = "0";//是否是需要解密(0:表示不存在IsEncryption属性；true表示存在IsEncryption属性并把值加密；false表示存在IsEncryption属性值没有加密)
                    string value = "";//值
                    string color = xmlitems.ElementAt(i).Attribute("Color") == null ? "" : xmlitems.ElementAt(i).Attribute("Color").Value;
                    string tooltip = xmlitems.ElementAt(i).Attribute("Tooltip") == null ? "" : xmlitems.ElementAt(i).Attribute("Tooltip").Value;

                    if (datatext == string.Empty)
                    {
                        value = datavalue;
                    }
                    else
                    {
                        value = datatext;
                    }
                    if (datatype.ToLower() == "datetime")
                    {
                        value = ChangeTime(value);
                    }
                    if (xmlitems.ElementAt(i).Attribute("IsEncryption") != null)
                    {
                        isencryption = xmlitems.ElementAt(i).Attribute("IsEncryption").Value.ToLower();
                    }

                    if (datatype.ToLower() != "attachmentlist" && isencryption == "0")
                    {
                        stringBuilder.Append("<Attribute  Name=\"" + name + "\" Text=\"" + text + "\" DataType=\"" + datatype +
                            "\" DataValue=\"" + value + "\"></Attribute>");
                    }
                    else if (datatype.ToLower() != "attachmentlist" && isencryption == "true")
                    {
                        stringBuilder.Append("<Attribute  Name=\"" + name + "\" Text=\"" + text + "\" DataType=\"" + datatype +
                            "\" DataValue=\"" + ReplaceString(value) + "\"></Attribute>");
                    }
                    else if (datatype.ToLower() != "attachmentlist" && isencryption == "false")
                    {
                        stringBuilder.Append("<Attribute  Name=\"" + name + "\" Text=\"" + text + "\" DataType=\"" + datatype +
                            "\" DataValue=\"" + value + "\"></Attribute>");
                    }
                    if (datatype.ToLower() == "attachmentlist" && isencryption == "0")
                    {
                        isAttach = true;
                    }
                }
                #endregion
                #region 从表数据
                var objectlists = from c in XElement.Parse(detail.APPXML).Elements("Object").Elements("ObjectList") select c;//从表信息
                for (int i = 0; i < objectlists.Count(); i++)
                {
                    stringBuilder.Append("<ObjectList Name=\"" + objectlists.ElementAt(i).Attribute("Name").Value + "\"  Text=\"" +
                        objectlists.ElementAt(i).Attribute("Description").Value + "\">");
                    var objects = from obj in objectlists.ElementAt(i).Elements("Object") select obj;
                    for (int j = 0; j < objects.Count(); j++)
                    {
                        stringBuilder.Append("<Object Name=\"" + objects.ElementAt(j).Attribute("Name").Value + "\"   id=\"" +
                            objects.ElementAt(j).Attribute("id").Value + "\" Text=\"" + objects.ElementAt(j).Attribute("Description").Value +
                            "\">");
                        var attributes = from att in objects.ElementAt(j).Elements("Attribute") select att;
                        for (int k = 0; k < attributes.Count(); k++)
                        {
                            string name = attributes.ElementAt(k).Attribute("Name").Value;
                            string text = attributes.ElementAt(k).Attribute("Description").Value;
                            string datatype = attributes.ElementAt(k).Attribute("DataType").Value;
                            string datavalue = attributes.ElementAt(k).Attribute("DataValue").Value;
                            string datatext = attributes.ElementAt(k).Attribute("DataText").Value;
                            string isencryption = "0";//是否是需要解密(0:表示不存在IsEncryption属性；true表示存在IsEncryption属性并把值加密；false表示存在IsEncryption属性值没有加密)
                            string value = "";//值
                            string color = attributes.ElementAt(k).Attribute("Color") == null ? "" :
                                attributes.ElementAt(k).Attribute("Color").Value;
                            string tooltip = attributes.ElementAt(k).Attribute("Tooltip") == null ? "" :
                                attributes.ElementAt(k).Attribute("Tooltip").Value;

                            if (datatext == string.Empty)
                            {
                                value = datavalue;
                            }
                            else
                            {
                                value = datatext;
                            }
                            if (datatype.ToLower() == "datetime")
                            {
                                value = ChangeTime(value);
                            }
                            if (attributes.ElementAt(k).Attribute("IsEncryption") != null)
                            {
                                isencryption = attributes.ElementAt(k).Attribute("IsEncryption").Value.ToLower();
                            }

                            stringBuilder.Append("<Attribute ");
                            stringBuilder.Append("Name=\"" + name + "\" ");
                            stringBuilder.Append("Text=\"" + text + "\" ");
                            stringBuilder.Append("DataType=\"" + datatype + "\" ");
                            stringBuilder.Append("DataValue=\"" + (isencryption == "true" ? ReplaceString(value) :
                                value) + "\" ");
                            if (!string.IsNullOrEmpty(color))
                            {
                                stringBuilder.Append(" Color=\"" + color + "\" ");
                            }
                            if (!string.IsNullOrEmpty(tooltip))
                            {
                                stringBuilder.Append(" Tooltip=\"" + tooltip + "\" ");
                            }
                            stringBuilder.Append("></Attribute>");
                        }
                        #region 从表的从表
                        var aol = from att in objects.ElementAt(j).Elements("ObjectList") select att;
                        for (int a = 0; a < aol.Count(); a++)
                        {
                            stringBuilder.Append("<ObjectList Name=\"" + aol.ElementAt(a).Attribute("Name").Value + "\"  Text=\"" + aol.ElementAt(a).Attribute("Description").Value + "\">");
                            var aolo = from obj in aol.ElementAt(a).Elements("Object") select obj;
                            for (int b = 0; b < aolo.Count(); b++)
                            {
                                stringBuilder.Append("<Object Name=\"" + aolo.ElementAt(b).Attribute("Name").Value + "\"   id=\"" + aolo.ElementAt(b).Attribute("id").Value + "\" Text=\"" + aolo.ElementAt(b).Attribute("Description").Value + "\">");
                                var aoloatt = from objatt in aolo.ElementAt(b).Elements("Attribute") select objatt;
                                for (int c = 0; c < aoloatt.Count(); c++)
                                {
                                    string name = aoloatt.ElementAt(c).Attribute("Name").Value;
                                    string text = aoloatt.ElementAt(c).Attribute("Description").Value;
                                    string datatype = aoloatt.ElementAt(c).Attribute("DataType").Value;
                                    string datavalue = aoloatt.ElementAt(c).Attribute("DataValue").Value;
                                    string datatext = aoloatt.ElementAt(c).Attribute("DataText").Value;
                                    string isencryption = "0";//是否是需要解密(0:表示不存在IsEncryption属性；true表示存在IsEncryption属性并把值加密；false表示存在IsEncryption属性值没有加密)
                                    string value = "";//值
                                    if (datatext == string.Empty)
                                    {
                                        value = datavalue;
                                    }
                                    else
                                    {
                                        value = datatext;
                                    }
                                    if (datatype.ToLower() == "datetime")
                                    {
                                        value = ChangeTime(value);
                                    }
                                    if (aoloatt.ElementAt(c).Attribute("IsEncryption") != null)
                                    {
                                        isencryption = aoloatt.ElementAt(c).Attribute("IsEncryption").Value.ToLower();
                                    }
                                    switch (isencryption)
                                    {
                                        case "0":
                                            stringBuilder.Append("<Attribute Name=\"" + name + "\"  Text=\"" + text + "\" DataType=\"" + datatype + "\" DataValue=\"" + value + "\" />");
                                            break;
                                        case "true":
                                            stringBuilder.Append("<Attribute Name=\"" + name + "\"  Text=\"" + text + "\" DataType=\"" + datatype + "\" DataValue=\"" + ReplaceString(value) + "\" />");
                                            break;
                                        case "false":
                                            stringBuilder.Append("<Attribute Name=\"" + name + "\"  Text=\"" + text + "\" DataType=\"" + datatype + "\" DataValue=\"" + value + "\" />");
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                stringBuilder.Append("</Object>");
                            }
                            stringBuilder.Append("</ObjectList>");
                        }
                        #endregion

                        stringBuilder.Append("</Object>");
                    }
                    stringBuilder.Append("</ObjectList>");
                }

                #endregion
                stringBuilder.Append("</Object>");
                #region 取附件
                if (isAttach == true)
                {
                    #region new 2012-08-30 添加
                    stringBuilder.Append(GetAttachList(formid));
                    #endregion

                }
                #endregion
                stringBuilder.Append("</System>");
                return stringBuilder.ToString();
                #endregion
            }
            catch (Exception ex)
            {
                return GetXmlIsValid("3", "该单据数据出现异常，异常消息：" + ex.Message + "\n  FormidSystemcode:" + FormidSystemcode);
            }
        }


        public static string GetXmlIsValid(string flag, string word)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            stringBuilder.Append("<System>");
            stringBuilder.Append("<Result Code=\"" + flag + "\"><![CDATA[" + word + "]]></Result>");
            stringBuilder.Append("</System>");
            return stringBuilder.ToString();
            //return "";
        }

        #region 时间格式转换
        /// <summary>
        /// 时间格式转换
        /// </summary>
        /// <param name="value">值</param>
        /// <returns>string</returns>
        public static string ChangeTime(string value)
        {
            string str = "";
            try
            {
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                {
                    string contime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    if (contime.IndexOf("00:00:00") > -1)
                    {
                        str = dt.ToString("yyyy-MM-dd HH:mm:ss").Replace("00:00:00", "");
                    }
                    else
                    {
                        str = dt.ToString("yyyy-MM-dd HH:mm");
                    }

                }
                else
                {
                    str = value;
                }
                //string contime = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss");
                //if (contime.IndexOf("00:00:00") > -1)
                //{
                //    str = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm:ss").Replace("00:00:00", "");
                //}
                //else
                //{
                //    str = Convert.ToDateTime(value).ToString("yyyy-MM-dd HH:mm");
                //}
            }
            catch
            {
                str = value;
            }
            return str;
        }

        #endregion
       

        #region 替换解密的\0字符
        /// <summary>
        /// 替换解密的\0字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceString(string value)
        {
            return Encryption.Encrypt.AESDecrypt(value).Replace("\0", "");
        }
        #endregion

        public static string EscapeXML(string str)
        {

            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            /*
            &lt;=<   
            &gt;=>   
            &amp;=& 
            &apos;='
            &quot;="              
             */
            return str.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;").Replace("'", "&apos;").Replace("\"", "&quot;");
        }

        #endregion
        #region 取附件列表
        /// <summary>
        /// 取附件列表
        /// </summary>
        /// <param name="formid">表单ID</param>
        /// <returns>string</returns>
        public static string GetAttachList(string formid)
        {
            FileUploadService fileuploadClient = new FileUploadService();
            StringBuilder stringBuilder = new StringBuilder();
            var attach = fileuploadClient.GetFileListByOnlyApplicationID(formid);
            stringBuilder.Append("<AttachList>");
            if (attach.FileList != null)
            {
                for (int i = 0; i < attach.FileList.Count(); i++)
                {
                    string path = attach.FileList[i].FILEURL;
                    string filename = path.Substring(path.LastIndexOf('\\') + 1);
                    string filepath = HttpUtility.UrlEncode(attach.FileList[i].FILEURL + "\\" + attach.FileList[i].FILENAME);
                    string url = attach.DownloadUrl + "?filename=" + filepath;//文件地址
                    //sb.Append("<a href=\"" + url + "\" >" + filename + "</a>");
                    stringBuilder.Append("<Atrribute Name=\"" + attach.FileList[i].FILENAME + "\" Url=\"" + url + "\" />");
                }
            }
            stringBuilder.Append("</AttachList>");
            return stringBuilder.ToString();
        }
        #endregion
    }
}