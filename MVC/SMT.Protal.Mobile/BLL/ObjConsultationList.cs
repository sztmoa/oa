using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SMT.Portal.Common.SmtForm.BLL
{
    public class ObjConsultationList
    {
        string _content;
        /// <summary>
        /// Object中Name
        /// </summary>
        [XmlAttribute(AttributeName = "Content")]
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        DateTime _consultationDate;

        [XmlAttribute(AttributeName = "ConsultationDate")]
        public DateTime ConsultationDate
        {
            get { return _consultationDate; }
            set { _consultationDate = value; }
        }

        string _consultationUserName;
        [XmlAttribute(AttributeName = "ConsultationUserName")]
        public string ConsultationUserName
        {
            get { return _consultationUserName; }
            set { _consultationUserName = value; }
        }

        string _flag;
        [XmlAttribute(AttributeName = "Flag")]
        public string Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        string replyContent;
        [XmlAttribute(AttributeName = "ReplyContent")]
        public string ReplyContent
        {
            get { return replyContent; }
            set { replyContent = value; }
        }

        DateTime _replyDate;
        [XmlAttribute(AttributeName = "ReplyDate")]
        public DateTime ReplyDate
        {
            get { return _replyDate; }
            set { _replyDate = value; }
        }

        string _replyUserName;
        [XmlAttribute(AttributeName = "ReplyUserName")]
        public string ReplyUserName
        {
            get { return _replyUserName; }
            set { _replyUserName = value; }
        }
        string _consultationID;
        [XmlAttribute(AttributeName = "ConsultationID")]
        public string ConsultationID
        {
            get { return _consultationID; }
            set { _consultationID = value; }
        }

    }
}