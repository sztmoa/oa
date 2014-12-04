using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMT.Portal.Common.SmtForm.Framework;

using System.Text.RegularExpressions;
using SMT.SaaS.Common;
using SMT.SaaS.Services.SmtMoblieWS;
using SMT.SaaS.Services;

namespace SMT.Portal.Common.SmtForm
{
    public partial class Context : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string strGuid = Request["Guid"] + "";
            
            try
            {
                string result = GetContent(strGuid);

                if (!string.IsNullOrEmpty(result))
                {
                    int styleIndex = result.IndexOf("<style type=\"text/css\">");
                    int styleLast = result.LastIndexOf("</style>");
                    string style = result.Substring(styleIndex, styleLast + 8 - styleIndex);

                    int bodyIndex = result.IndexOf("<body>");
                    int bodyLast = result.LastIndexOf("</body>");
                    string body = result.Substring(bodyIndex + 6, bodyLast - bodyIndex - 6);
                    body = Regex.Replace(body, "href=\"(.*?)\"", "href=\"#\"", RegexOptions.IgnoreCase);
                    body = Regex.Replace(body, "href='(.*?)'", "href='#'", RegexOptions.IgnoreCase);
                    if (body == "" || body == null)
                    {
                        body = "无内容";
                    }
                    Response.Write(style + body);
                }
                else
                {
                    Response.Write("error");
                }

            }
            catch
            {
                Response.Write("error");
            }
        }

        #region 获取富文本框内容
        /// <summary>
        /// 获取富文本框内容
        /// </summary>
        /// <param name="formid">表单ID</param>
        /// <returns>string</returns>
        public static string GetContent(string formid)
        {
            string result = "";
            PublicService pservice = new PublicService();
            byte[] content = pservice.GetContent(formid);

            if (content != null)
            {
                //result = System.Text.Encoding.Default.GetString(content);
                result = System.Text.Encoding.UTF8.GetString(content);
            }
            return result;
        }
        #endregion
    }
}