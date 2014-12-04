using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SMT.Portal.Common.SmtForm.BLL
{
    /// <summary>
    /// 对应表单数据XML中的审核列表ApprovalList
    /// </summary>
    public class ObjApproval
    {
        string _approver;
        /// <summary>
        /// 审核人
        /// </summary>
        [XmlAttribute(AttributeName = "Approver")]
        public string Approver
        {
            get { return _approver; }
            set { _approver = value; }
        }
        string _flag;
        /// <summary>
        /// 审核人
        /// </summary>
        [XmlAttribute(AttributeName = "Flag")]
        public string Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        DateTime _approvalTime;
        /// <summary>
        /// 审核时间
        /// </summary>        
        [XmlAttribute(AttributeName = "ApprovalTime")]
        public DateTime ApprovalTime
        {
            get { return _approvalTime; }
            set { _approvalTime = value; }
        }


        string _approvalState;
        /// <summary>
        /// 审核状态
        /// </summary>
        [XmlAttribute(AttributeName = "ApprovalState")]
        public string ApprovalState
        {
            get { return _approvalState; }
            set { _approvalState = value; }
        }
        string _flowType;
        /// <summary>
        /// 审核状态
        /// </summary>
        [XmlAttribute(AttributeName = "FlowType")]
        public string FlowType
        {
            get { return _flowType; }
            set { _flowType = value; }
        }
        string _approvalRemark;
        /// <summary>
        /// 审核意见
        /// </summary>
        [XmlAttribute(AttributeName = "ApprovalRemark")]
        public string ApprovalRemark
        {
            get { return _approvalRemark; }
            set { _approvalRemark = value; }
        }
    }
}