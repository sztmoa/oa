using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Data;
using SMT.Portal.Common.SmtForm.Framework;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 对应表单数据XML中的从表ObjectList
    /// </summary>
    public class ObjSubList
    {
        string _name;
        /// <summary>
        /// Object中Name
        /// </summary>
        [XmlAttribute(AttributeName = "Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        string _id;
        /// <summary>
        /// Object中id
        /// </summary>
        [XmlAttribute(AttributeName = "id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        string _text;
        /// <summary>
        /// Object中Text
        /// </summary>
        [XmlAttribute(AttributeName = "Text")]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        string _align;
        /// <summary>
        /// Object中Text
        /// </summary>
        [XmlAttribute(AttributeName = "Align")]
        public string Align
        {
            get { return _align; }
            set { _align = value; }
        }

        string _HideWhenEmpty;
        [XmlAttribute(AttributeName = "HideWhenEmpty")]
        public string HideWhenEmpty
        {
            get { return _HideWhenEmpty; }
            set { _HideWhenEmpty = value; }
        }


        /// <summary>
        /// 行数据数组
        /// </summary>
        ObjRow[] _rowList = null;
        [XmlElement(ElementName = "Row")]
        public ObjRow[] RowList
        {
            get { return _rowList; }
            set { _rowList = value; }
        }

     

        ListBase<ObjRow> _rowDict;
        public ListBase<ObjRow> GetRowDict()
        {
            if (_rowDict == null)
            {
                _rowDict = new ListBase<ObjRow>();
                _rowDict.KeyFieldName = "Id";
                if (_rowList != null)
                {
                    //将行数组添加至集合
                    foreach (ObjRow obj in _rowList)
                    {
                        _rowDict.Add(obj);
                    }
                }

            }
            return _rowDict;
        }

        /// <summary>
        /// 列数据数组
        /// </summary>
        ObjColumnList[] _columnList = null;
        [XmlElement(ElementName = "ColumnLists")]
        public ObjColumnList[] ColumnList
        {
            get { return _columnList; }
            set { _columnList = value; }
        }
        /// <summary>
        /// 列数据数组
        /// </summary>
        ObjColumnList[] _subColumnList = null;
        [XmlElement(ElementName = "SubColumnLists")]
        public ObjColumnList[] SubColumnList
        {
            get { return _subColumnList; }
            set { _subColumnList = value; }
        }




        ListBase<ObjColumnList> _columnListDict;
        public ListBase<ObjColumnList> GetColumnListDict()
        {
            if (_columnListDict == null)
            {
                _columnListDict = new ListBase<ObjColumnList>();
                _columnListDict.KeyFieldName = "Id";

                //将列数组添加至集合
                foreach (ObjColumnList obj in _columnList)
                {
                    _columnListDict.Add(obj);
                }
            }
            return _columnListDict;
        }


        ListBase<ObjColumnList> _subColumnListDict;
        public ListBase<ObjColumnList> GetSubColumnListDict()
        {
            if (_subColumnListDict == null)
            {
                _subColumnListDict = new ListBase<ObjColumnList>();
                _subColumnListDict.KeyFieldName = "Id";

                //将列数组添加至集合
                foreach (ObjColumnList obj in _subColumnList)
                {
                    _subColumnListDict.Add(obj);
                }
            }
            return _subColumnListDict;
        }
         

         /// <summary>
         /// 判断此行是否为空行
         /// </summary>
         /// <returns></returns>
         public bool CheckEmpty()
         {
             bool result = true;
             if (_rowList != null)
             {
                 foreach (var item in _rowList)
                 {
                     if (!item.CheckEmpty())
                     {
                         result = false;
                         break;
                     }
                 }
             }
             return result;
         }
    }
}