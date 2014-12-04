using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 对应字段的显示属性
    /// </summary>
    public class ObjColumn
    {
        string _name;
        /// <summary>
        /// 字段名称
        /// </summary>
        [XmlAttribute(AttributeName = "Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _text;
        /// <summary>
        /// 字段文本值
        /// </summary>
        [XmlAttribute(AttributeName = "Text")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        bool _isVisible;
        /// <summary>
        /// 字段是否可见
        /// </summary>
        [XmlAttribute(AttributeName = "IsVisible")]
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        string _width;
        /// <summary>
        /// 字段宽度
        /// </summary>
        [XmlAttribute(AttributeName = "Width")]
        public string Width
        {
            get { return _width; }
            set { _width = value; }
        }

        /// <summary>
        /// 列是否合计
        /// </summary>
        string _columnSum;
        [XmlAttribute(AttributeName = "ColumnSum")]
        public string ColumnSum
        {
            get
            {
                return _columnSum;
            }
            set
            {
                _columnSum = value;
            }
        }

        string _ToolTip;
        [XmlAttribute(AttributeName = "ToolTip")]
        public string ToolTip
        {
            set
            {
                _ToolTip = value;
            }
            get
            {
                if ( string.IsNullOrEmpty(_ToolTip ))
                {
                    return this.Text;
                }
                return _ToolTip;
            }
        }

        [XmlAttribute(AttributeName = "Align")]
        public string Align
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "Format")]
        public string Format
        {
            get;
            set;
        }
    }
}