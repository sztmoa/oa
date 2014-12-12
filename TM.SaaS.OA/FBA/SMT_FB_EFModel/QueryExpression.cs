using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System;


namespace SMT_FB_EFModel
{
    public class QueryExpression : VisitUserBase
    {
        public QueryExpression()
        {
            IsUnCheckRight = true;
        }
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string QueryType { get; set; }
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string PropertyName { set; get; }
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string PropertyValue { set; get; }
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [DefaultValue(Operations.Equal)]
        public Operations Operation { set; get; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [DefaultValue(Operations.Equal)]
        public RelationType RelatedType { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public QueryExpression RelatedExpression { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public PageExpression Pager
        {
            get;
            set;
        }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public OrderByExpression OrderByExpression
        {
            get;
            set;
        }

        public enum Operations
        {
            Equal, Like, NotEqual, NotLike, GreaterThan, LessThan, GreaterThanOrEqual, LessThanOrEqual
        }

        public enum RelationType
        {
            And, Or
        }
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Include { get; set; }
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string[] OrderBy { get; set; }

        public bool IsUnCheckRight { get; set; }
        public List<QueryExpression> ToList()
        {
            QueryExpression qe = this;
            List<QueryExpression> listQE = new List<QueryExpression>();
            while (qe != null)
            {
                listQE.Add(qe);
                qe = qe.RelatedExpression;
            }
            return listQE;
        }
        public static QueryExpression Equal(string propertyName, string propertyValue)
        {
            QueryExpression qe = new QueryExpression();
            qe.PropertyName = propertyName;
            qe.PropertyValue = propertyValue;
            qe.RelatedType = RelationType.And;
            return qe;
        }
        public static QueryExpression NotEqual(string propertyName, string propertyValue)
        {
            QueryExpression qe = new QueryExpression();
            qe.PropertyName = propertyName;
            qe.PropertyValue = propertyValue;
            qe.RelatedType = RelationType.And;
            qe.Operation = Operations.NotEqual;
            return qe;
        }

    }

    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public class PageExpression
    {
        public PageExpression()
        {
            this.PageSize = 25;
            this.PageIndex = 1;
            this.PreRowCount = 100;
        }
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public int PageSize { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public int PageCount { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public int PageIndex { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public int PreRowCount { get; set; }
   
    }

    [global::System.Runtime.Serialization.DataContractAttribute(IsReference = true)]
    [global::System.Serializable()]
    public class OrderByExpression
    {
        
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string PropertyName { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public string PropertyType { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public OrderByType OrderByType { get; set; }

        [global::System.Runtime.Serialization.DataMemberAttribute()]
        public OrderByExpression ThenOrderByExpression { get; set; }

    }
    public enum OrderByType
    {
        Asc, Dsc
    }   
}
