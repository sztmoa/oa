using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 对应表单数据XML中的附件列表AttachList
    /// </summary>
    public class ObjAttach
    {
        string _name;
        /// <summary>
        /// 附件名称
        /// </summary>
        [XmlAttribute(AttributeName = "Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _url;
        /// <summary>
        /// 下载链接
        /// </summary>
        [XmlAttribute(AttributeName = "Url")]
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

    }
}