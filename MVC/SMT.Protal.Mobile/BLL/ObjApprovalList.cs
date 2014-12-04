using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SMT.Portal.Common.SmtForm.Framework;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 控制审核列表
    /// </summary>
    public class ObjApprovalList
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

        /// <summary>
        /// 列表数组
        /// </summary>
        ObjApproval[] _approvalList = null;
        [XmlElement(ElementName = "ApprovalLists")]
        public ObjApproval[] ApprovalList
        {
            get { return _approvalList; }
            set { _approvalList = value; }
        }

        /// <summary>
        /// 列表集合
        /// </summary>
        ListBase<ObjApproval> _approvalDict ;
        public ListBase<ObjApproval> GetApprovalDict()
        {
            if(_approvalDict==null)
            {
                _approvalDict = new ListBase<ObjApproval>();
                _approvalDict.KeyFieldName = "Approver";
                foreach (ObjApproval obj in _approvalList)
                {
                    _approvalDict.Add(obj);
                }
                   
            }
            return _approvalDict;
        }
    }
}