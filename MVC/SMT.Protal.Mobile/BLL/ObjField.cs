using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 对应字段的显示
    /// </summary>
    public class ObjField
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


        string _dataType;
        /// <summary>
        /// 数据类型
        /// </summary>
        [XmlAttribute(AttributeName = "DataType")]
        public string DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }


        string _dataValue;
        /// <summary>
        /// 字段的数据值
        /// </summary>
        [XmlAttribute(AttributeName = "DataValue")]
        public string DataValue
        {
            get { return _dataValue; }
            set { _dataValue = value; }
        }

        int _orderno;
        /// <summary>
        /// 显示的顺序
        /// </summary>
        [XmlAttribute(AttributeName = "Orderno")]
        public int Orderno
        {
            get { return _orderno; }
            set { _orderno = value; }
        }


        int _colspan;
        /// <summary>
        /// 占几列
        /// </summary>
        [XmlAttribute(AttributeName = "Colspan")]
        public int Colspan
        {
            get { return _colspan; }
            set { _colspan = value; }
        }



        bool _isVisible = true;
        /// <summary>
        /// 是否可见
        /// </summary>
        [XmlAttribute(AttributeName = "IsVisible")]
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; }
        }

        bool _isShowCaption = true;
        /// <summary>
        /// 是否显示标题名称
        /// </summary>
        [XmlAttribute(AttributeName = "IsShowCaption")]
        public bool IsShowCaption
        {
            get { return _isShowCaption; }
            set { _isShowCaption = value; }
        }

        /// <summary>
        /// 控制样式名称
        /// </summary>
        string _cssName;
        [XmlAttribute(AttributeName = "CssName")]
        public string CssName
        {
            get { return _cssName; }
            set { _cssName = value; }
        }
        /// <summary>
        /// 显示在表格中的列位置
        /// </summary>
        string _columnOrder;
        [XmlAttribute(AttributeName = "ColumnOrder")]
        public string ColumnOrder
        {
            get { return _columnOrder; }
            set { _columnOrder = value; }
        }
        /// <summary>
        /// 字体颜色
        /// </summary>
        string _color;
        [XmlAttribute(AttributeName = "Color")]
        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
            }
        }
        /// <summary>
        /// 提示文字
        /// </summary>
        string _tooltip;
        [XmlAttribute(AttributeName = "Tooltip")]
        public string Tooltip
        {
            get
            {
                return _tooltip;
            }
            set
            {
                _tooltip = value;
            }
        }

        /// <summary>
        /// 是否是链接
        /// </summary>
        bool _IsLink;
        [XmlAttribute(AttributeName = "IsLink")]
        public bool IsLink
        {
            get
            {
                return _IsLink;
            }
            set
            {
                _IsLink = value;
            }
        }

        public string GetFormatValue(string format = "")
        {

            var result = this.DataValue;

            if (format == "Money")
            {
                decimal d = 0;
                decimal.TryParse(this.DataValue, out d);
                result = string.Format("{0:N}", d);
            }
            else if ( !string.IsNullOrEmpty(format))
            {
                result = string.Format(format, result);
            }
            return result;
            
        }
    }
}