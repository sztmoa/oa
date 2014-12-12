using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT_FB_EFModel;
using System.Data.Objects.DataClasses;

namespace SMT.FB.BLL
{
    class BudgetTransferBLL : BaseBLL
    {
        public const string FieldName_TransferType = "TRANSFERTYPE";
        public enum TransferType
        {
            Department=1, Company=2
        }
        public List<T> GetT_FB_TRANSFERAPPLYMASTER<T>(QueryExpression qe, TransferType type)
        {
            QueryExpression qeTransferType = QueryExpression.Equal(FieldName_TransferType, Convert.ToString((int)type));
            qeTransferType.QueryType = typeof(T_FB_TRANSFERAPPLYMASTER).Name;
            qeTransferType.RelatedExpression = qe;

            List<EntityObject> list = this.GetEntities(qeTransferType);
            List<T> listResult = new List<T>();
            list.ForEach(item =>
                {
                    T detail = (T)Activator.CreateInstance(typeof(T), item) ;
                    listResult.Add(detail);
                });
            return listResult;
        }

        public List<FBEntity> GetCompanyTransferMaster(QueryExpression qe)
        {
            List<CompanyTransferMaster> list = GetT_FB_TRANSFERAPPLYMASTER<CompanyTransferMaster>(qe, TransferType.Company);
            return list.ToFBEntityList();
        }

        public List<FBEntity> GetDepartmentTransferMaster(QueryExpression qe)
        {

            List<DepartmentTransferMaster> list = GetT_FB_TRANSFERAPPLYMASTER<DepartmentTransferMaster>(qe, TransferType.Department);
            return list.ToFBEntityList();
        }

    }
}
