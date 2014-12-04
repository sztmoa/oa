using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SMT.Portal.Common.SmtForm.Framework;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 控制列集合的显示
    /// </summary>
    public class ObjColumnList
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
        /// 文本值
        /// </summary>
        [XmlAttribute(AttributeName = "Text")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// 字段数组
        /// </summary>
        ObjColumn[] _ColumnList = null;
        [XmlElement(ElementName = "Column")]
        public ObjColumn[] ColumnList
        {
            get { return _ColumnList; }
            set { _ColumnList = value; }
        }

        /// <summary>
        /// 集合
        /// </summary>
        ListBase<ObjColumn> _columnDict;
        public ListBase<ObjColumn> GetColumnDict()
        {
            if (_columnDict == null)
            {
                _columnDict = new ListBase<ObjColumn>();
                _columnDict.KeyFieldName = "Name";
                foreach (ObjColumn obj in _ColumnList)
                {
                    _columnDict.Add(obj);
                }

            }
            return _columnDict;
        }
    }
}