using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SMT.Portal.Common.SmtForm.Framework;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 控制附件列表
    /// </summary>
    public class ObjAttachList
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
        /// 附件列表数组
        /// </summary>
        ObjAttach[] _attachList = null;
        [XmlElement(ElementName = "AttachLists")]
        public ObjAttach[] AttachList
        {
            get { return _attachList; }
            set { _attachList = value; }
        }


        /// <summary>
        /// 附件列表集合
        /// </summary>
        ListBase<ObjAttach> _attachDict;
        public ListBase<ObjAttach> GetAttachDict()
        {
            if (_attachDict == null)
            {
                _attachDict = new ListBase<ObjAttach>();
                _attachDict.KeyFieldName = "Name";
                foreach (ObjAttach obj in _attachList)
                {
                    _attachDict.Add(obj);
                }

            }
            return _attachDict;
        }
    }
}