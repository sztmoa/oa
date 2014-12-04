using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 对应Config.xml文档中的Viewmode
    /// </summary>
    public class ObjViewmode
    {
        /// <summary>
        /// 查看名称
        /// </summary>
        private string _name = null;
        [XmlAttribute(AttributeName = "Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 文本
        /// </summary>
        private string _text = null;
        [XmlAttribute(AttributeName = "Text")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// 占几列
        /// </summary>
        private string _column = null;
        [XmlAttribute(AttributeName = "Column")]
        public string Column
        {
            get { return _column; }
            set { _column = value; }
        }


    }
}