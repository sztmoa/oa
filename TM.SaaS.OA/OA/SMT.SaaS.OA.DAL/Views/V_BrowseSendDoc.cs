using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT_OA_EFModel;

namespace SMT.SaaS.OA.DAL.Views
{
    /// <summary>
    /// 事项审批查看类
    /// </summary>
    public class V_BrowseSendDoc
    {
        public string SENDDOCID { get; set; }
        public string SENDDOCTITLE { get; set; }
        public string ISDISTRIBUTE { get; set; }
        public string ISREDDOC { get; set; }
        public string NUM { get; set; }
        public string OWNERCOMPANYID { get; set; }
        public string OWNERDEPARTMENTID { get; set; }
        public string OWNERID { get; set; }
        public string OWNERPOSTID { get; set; }
        public string PRIORITIES { get; set; }
        public string SENDDOCTYPE { get; set; }
        public string GRADED { get; set; }
        public DateTime PUBLISHDATE { get; set; }
        public string CHECKSTATE { get; set; }
        /// <summary>
        /// 发送总人数
        /// </summary>
        public string SENDTOTAL { get; set; }
        /// <summary>
        /// 已查看人数
        /// </summary>
        public string VIEWDCOUNT { get; set; }
        /// <summary>
        /// 未查看人数
        /// </summary>
        public string UNVIEWCOUNT { get; set; }
        
    }
}
