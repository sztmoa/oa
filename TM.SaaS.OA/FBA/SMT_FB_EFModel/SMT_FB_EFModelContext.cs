using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using System.Data.Objects.DataClasses;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Data.Objects;
using System.Threading;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Reflection;

namespace SMT_FB_EFModel
{
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public class VisitUserBase
    {
        [DataMember]
        public string VisitUserID { get; set; }
        [DataMember]
        public string VisitModuleCode { get; set; }
        [DataMember]
        public string VisitAction { get; set; }
        [DataMember]
        public bool IsGetFullData { get; set; }
    }

    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public class ServiceResult
    {
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string Exception { get; set; }
    }

    [DataContract]
    public class SaveResult : ServiceResult
    {
        [DataMember]
        public FBEntity FBEntity { get; set; }
        [DataMember]
        public bool Successful { get; set; }
    }

    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public class QueryResult : ServiceResult
    {
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public PageExpression Pager { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public List<FBEntity> Result { get; set; }
    }

  
}
