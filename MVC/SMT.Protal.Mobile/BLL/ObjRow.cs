using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SMT.Portal.Common.SmtForm.Framework;

namespace SMT.Portal.Common.SmtForm.BLL
{
    public class ObjRow
    {
        string _name;
        /// <summary>
        /// ObjectList名称
        /// </summary>
        [XmlAttribute(AttributeName = "Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _id;
        /// <summary>
        /// ObjectList ID
        /// </summary>
        [XmlAttribute(AttributeName = "Id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        string _text;
        /// <summary>
        /// ObjectList文本值
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
        ObjField[] _objFieldList = null;
        [XmlElement(ElementName = "Detail")]
        public ObjField[] ObjFieldList
        {
            get { return _objFieldList; }
            set { _objFieldList = value; }
        }

        /// <summary>
        /// 嵌套的明细数据
        /// </summary>
        ObjSubList[] _subsubList;
        public ObjSubList[] SubSubList
        {
            get { return _subsubList; }
            set { _subsubList = value; }
        }

     
        /// <summary>
        /// 字段集合列表
        /// </summary>
        ListBase<ObjField> _objFieldDict;
        public ListBase<ObjField> GetObjFieldDict()
        {
            if (_objFieldDict == null)
            {
                _objFieldDict = new ListBase<ObjField>();
                _objFieldDict.KeyFieldName = "Name";

                //将数组转换成集合
                foreach (ObjField obj in _objFieldList)
                {
                    _objFieldDict.Add(obj);
                }

            }
            return _objFieldDict;
        }

        public string GetObjFieldValue(string fileName)
        {
            var result = string.Empty;
            var field = GetObjFieldDict()[fileName];
            if ( field != null)
            {
                result = field.DataValue;
            }
            return result;

        }

        /// <summary>
        /// 判断此行是否为空行
        /// </summary>
        /// <returns></returns>
        public bool CheckEmpty()
        {
            bool result = true;
            if (_objFieldList != null)
            {
                foreach (var item in _objFieldList)
                {
                    if (!string.IsNullOrEmpty(item.DataValue))
                    {
                        result =  false;
                        break;
                    }
                }
            }
            return result;
        }
    }
}