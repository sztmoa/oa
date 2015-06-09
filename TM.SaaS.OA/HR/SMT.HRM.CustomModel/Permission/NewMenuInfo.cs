using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SMT.HRM.CustomModel.Permission
{
    [DataContract]
    [Serializable]
    public class NewMenuInfo
    {

        [DataMember]
        public string MenuID { get; set; }
        [DataMember]
        public string MenuCode { get; set; }
        [DataMember]
        public string MenuName { get; set; }
        [DataMember]
        public decimal ListNumber { get; set; }
        [DataMember]
        public string MenuIconPath { get; set; }
        [DataMember]
        public string MenuUrl { get; set; }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public string SystemType { get; set; }
        [DataMember]
        public string ParentID { get; set; }
        [DataMember]
        public List<NewMenuInfo> SubMenuInfoList { get; set; }
        [DataMember]
        public NewMenuInfo ParentMenu { get; set; }
        [DataMember]
        public int Level { get; set; }
        [DataMember]
        public string ShortCutId { get; set; }
    }
}
