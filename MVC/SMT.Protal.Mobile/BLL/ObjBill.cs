using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using SMT.Portal.Common.SmtForm.Framework;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 对应表单数据XML中的Form表单
    /// </summary>
    [XmlRoot("Bill")]
    public class ObjBill
    {
        /// <summary>
        /// Form标题
        /// </summary>
        string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// FormID
        /// </summary>
        string _formID;
        public string FormID
        {
            get { return _formID; }
            set { _formID = value; }
        }

        /// <summary>
        /// Form 代码
        /// </summary>
        string _modelCode;
        public string ModelCode
        {
            get { return _modelCode; }
            set { _modelCode = value; }
        }

        string _messageStatus;
        public string MessageStatus
        {
            get { return _messageStatus; }
            set { _messageStatus = value; }
        }

        string _orderType;
        public string OrderType
        {
            get { return _orderType; }
            set { _orderType = value; }
        }

        string _consultationID;
        public string ConsultationID
        {
            get { return _consultationID; }
            set{_consultationID = value;}
        }
        /// <summary>
        /// Form中业务对象
        /// </summary>
        ObjDetailList[] _detailLists = null;
        [XmlElement(ElementName = "DetailLists")]
        public ObjDetailList[] DetailLists
        {
            get { return _detailLists; }
            set { _detailLists = value; }
        }

        /// <summary>
        /// Form中从表象集合
        /// </summary>
        ObjSubList[] _subLists = null;
        [XmlElement(ElementName = "SubLists")]
        public ObjSubList[] SubLists
        {
            get { return _subLists; }
            set { _subLists = value; }
        }

        ObjConsultationList[] _consultationLists = null;
        [XmlElement(ElementName = "ConsultationList")]
        public ObjConsultationList[] ConsultationLists
        {
            get { return _consultationLists; }
            set { _consultationLists = value; }
        }

        //private ListBase<ObjSubList> _SubListsDict;


        /// <summary>
        /// 获取Form中的从表对象
        /// </summary>
        /// <returns>从表对象集合列表</returns>
        public ListBase<ObjSubList> GetSubListsDict()
        {
            ListBase<ObjSubList> _SubListsDict = new ListBase<ObjSubList>();
            _SubListsDict.KeyFieldName = "Name";

            if (null != _subLists && _subLists.Length > 0)
            {
                foreach (ObjSubList obj in _subLists)
                {
                    _SubListsDict.Add(obj);
                }
            }
            return _SubListsDict;
        }

        /// <summary>
        /// Form中附件列表AttachList
        /// </summary>
        ObjAttachList[] _attachLists = null;
        [XmlElement(ElementName = "Attachs")]
        public ObjAttachList[] AttachLists
        {
            get { return _attachLists; }
            set { _attachLists = value; }
        }


        /// <summary>
        /// Form中审核列表ApprovalList
        /// </summary>
        ObjApprovalList[] _approvalLists = null;
        [XmlElement(ElementName = "Approvas")]
        public ObjApprovalList[] ApprovalLists
        {
            get { return _approvalLists; }
            set { _approvalLists = value; }
        }

        private string _rtfData;
        public string RtfData
        {
            get
            {
                return _rtfData;
            }
            set
            {
                _rtfData = value;
            }
        }
    }
}