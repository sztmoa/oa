using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 对应Config.xml文档中的Template
    /// </summary>
    public class ObjTemplate
    {
        /// <summary>
        /// 模板名称
        /// </summary>
        private string _name = null;
        [XmlAttribute(AttributeName = "Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
 
        /// <summary>
        /// 模板对应的文件
        /// </summary>
        private string _cssFile = null;
        [XmlAttribute(AttributeName = "CssFile")]
        public string CssFile
        {
            get { return _cssFile; }
            set { _cssFile = value; }
        }

        /// <summary>
        /// 对应的图片路径
        /// </summary>
        private string _imageUrl = null;
        [XmlAttribute(AttributeName = "ImageUrl")]
        public string ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        /// <summary>
        /// 对应的图片文件
        /// </summary>
        private string _cssImageUrl = null;
        [XmlAttribute(AttributeName = "CssImageUrl")]
        public string CssImageUrl
        {
            get { return _cssImageUrl; }
            set { _cssImageUrl = value; }
        }
    }
}