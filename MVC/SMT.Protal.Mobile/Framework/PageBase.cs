using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using SMT.Portal.Common.SmtForm.BLL;
using System.Net;
using System.IO;
using SMT.SaaS.Common;

using SMT.SaaS.Services.SmtMoblieWS;

namespace SMT.Portal.Common.SmtForm.Framework
{
   
    public class PageBase:System.Web.UI.Page
    {
        //日志锁
        //public bool NeedLogin = true;

        private ObjTemplate _template;
        public ObjTemplate Template
        {
            get 
            {
                if (_template == null && this.Request.Cookies["Template"] != null)
                {
                    _template = CacheData.CacheConfig.GetTemplate()[this.Request.Cookies["Template"].Values["Template"].ToString()];
                }
                else 
                {
                    _template =CacheData.CacheConfig.GetTemplate()["Template1"];
                }
                return _template;
            }
        }
        private ObjViewmode _viewmode;
        public ObjViewmode Viewmode
        {
            get
            {
                if (_viewmode == null && this.Request.Cookies["Viewmode"] != null)
                {

                    _viewmode = CacheData.CacheConfig.GetViewmode()[this.Request.Cookies["Viewmode"].Values["Viewmode"].ToString()];
                }
                else 
                {
                    _viewmode = CacheData.CacheConfig.GetViewmode()["Phone"];
                }
                return _viewmode;
            }
        }

        /// <summary>
        /// 待办的列表的标识字符
        /// </summary>
        private static string openwait = "open";
        public static string OpenWait
        {
            get { return openwait; }
            set { openwait = value; }
        }
        /// <summary>
        /// 已办的列表的标识字符
        /// </summary>
        private static string closewait = "close";
        public static string CloseWait
        {
            get { return closewait; }
            set { closewait = value; }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //private T_SYS_USER _currentUser;
        //public T_SYS_USER CurrentUser
        //{
        //    get
        //    {
        //        if (_currentUser == null && this.Request.Cookies["Loginname"] != null)
        //        {
        //            //System.Net.ServicePointManager.Expect100Continue = false;
        //            //By DK:
        //            //此方法登录不记录日志
        //            _currentUser= this.MobileService.UserLogin(this.Request.Cookies["Loginname"].Values["userName"], CookieHelper.DecryptQueryString(this.Request.Cookies["Loginname"].Values["pwd"]));
        //            if (_currentUser != null)
        //            {
        //                _currentUser.PASSWORD = CookieHelper.DecryptQueryString(this.Request.Cookies["Loginname"].Values["pwd"]);
        //            }
                    
        //        }
                
        //        return _currentUser;
        //    }

        //}


        /// <summary>
        /// 用户跳过登录界面记录地址
        /// </summary>
        public static string LoginUrl { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Header != null)
            {
                Literal li = new Literal();
                li.Text = "\r\n<link type='text/css' rel='stylesheet' href='"+this.Template.CssFile+"' />\r\n";

                //string fileName = "Template.css";
                //CreateFile(fileName);

                //li.Text = "\r\n<link type='text/css' rel='stylesheet' href='" + fileName + "' />\r\n";
                this.Page.Header.Controls.Add(li);
            }           
        }
        protected override void OnError(EventArgs e)
        {
            
        }
        //创建文件存储css内容
        private void CreateFile(string fileName)
        {
            string requestUrl = this.Request.Url.ToString();
            string url = requestUrl.Substring(0, requestUrl.LastIndexOf("/")) + this.Template.CssFile;

            WebClient client = new WebClient();
            byte[] bytes = client.DownloadData(url);

            string path = Request.PhysicalApplicationPath + fileName;
            if (!File.Exists(path))
            {
                File.Create(path);
            }

            FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            fileStream.Write(bytes, 0, bytes.Count());
            fileStream.Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            ////判断用户是否登录及页面是否需要判断用户登录
            //if (this.CurrentUser == null && this.NeedLogin)
            //{
            //    LoginUrl = Server.UrlEncode(Request.RawUrl);
            //    Response.Redirect("~/Login.aspx" + (Session["LoginParam"] != null ? Session["LoginParam"].ToString() : ""), true);
            //}
            //LoginUrl = null;
            base.OnLoad(e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
           
        }

        

    }
}