using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SMT.Portal.Common.SmtForm.Framework;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 控制业务主对象集合
    /// </summary>
    public class ObjDetailList
    {
        string _name;
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute(AttributeName = "Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _id;
        /// <summary>
        /// id
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        string _text;
        /// <summary>
        /// 文本
        /// </summary>
        [XmlAttribute(AttributeName = "Text")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }


        string _phoneRepeatCount="2";
        /// <summary>
        /// phone字段占几列
        /// </summary>
        [XmlAttribute(AttributeName = "PhoneRepeatCount")]
        public string PhoneRepeatCount
        {
            get { return "2"; }
            set { _phoneRepeatCount = value; }
        }


        string _padRepeatCount = "2";
        /// <summary>
        /// pad字段占几列
        /// </summary>
        [XmlAttribute(AttributeName = "PadRepeatCount")]
        public string PadRepeatCount
        {
            get { 
                return _padRepeatCount; 
            }
            set { _padRepeatCount = value; }
        }


        ObjField[] _objFieldList = null;
        [XmlElement(ElementName = "Detail")]
        public ObjField[] ObjFieldList
        {
            get { return _objFieldList; }
            set { _objFieldList = value; }
        }


        ListBase<Field> _fieldDict;
        public ListBase<Field> GetFieldDict()
        {

            if (_fieldDict == null)
            {
                _fieldDict = new ListBase<Field>();
                _fieldDict.KeyFieldName = "Name";

                foreach (ObjField obj in GetObjFieldDict())
                {
                    Field field = new Field();
                    field.FieldName = obj.Name;
                    if (obj.IsLink)
                    {
                        obj.DataType = "Link";
                    }
                    field.ControlType = ConvertControlType(obj.DataType);
                    field.Value = obj.DataValue;
                    field.CaptionName = obj.Text + "：";
                    field.Colspan = obj.Colspan;
                    field.CssName = obj.CssName;
                    field.IsVisible = obj.IsVisible;
                    field.ColumnOrder = obj.ColumnOrder;
                    field.IsShowCaption = obj.IsShowCaption;
                    
                    _fieldDict.Add(field);
                }
                //_fieldDict.Sort("Orderno", true);
            }
            return _fieldDict;
        }

        public void Load()
        {
            if (_objFieldDict == null)
            {
                _objFieldDict = new ListBase<ObjField>();
                _objFieldDict.KeyFieldName = "Name";

                foreach (ObjField obj in _objFieldList)
                {
                    _objFieldDict.Add(obj);
                }
                _objFieldDict.Sort("Orderno", true);
            }
        }
        ListBase<ObjField> _objFieldDict;
        public ListBase<ObjField> GetObjFieldDict()
        {

            if (_objFieldDict == null)
            {
                _objFieldDict = new ListBase<ObjField>();
                _objFieldDict.KeyFieldName = "Name";

                foreach (ObjField obj in _objFieldList)
                {
                    _objFieldDict.Add(obj);
                }
                _objFieldDict.Sort("Orderno", true);
            }
            return _objFieldDict;


        }

        /// <summary>
        /// 根据控件字符串返回控件类型
        /// </summary>
        /// <param name="control_text">控件字符串</param>
        /// <returns>控件类型</returns>
        private Constant.CtrlType ConvertControlType(string control_text)
        {
            switch (control_text)
            {
                case "TextBox":
                    return Constant.CtrlType.TextBox;
                case "DropDownList":
                    return Constant.CtrlType.DropDownList;
                case "TextArea":
                    return Constant.CtrlType.TextArea;
                case "Label":
                    return Constant.CtrlType.Label;
                case "ListBox":
                    return Constant.CtrlType.ListBox;
                case "RTF":
                    return Constant.CtrlType.Rtf;
                case "Link" :
                    return Constant.CtrlType.Link;
                default: return Constant.CtrlType.Label;
            }
        }
    }
}