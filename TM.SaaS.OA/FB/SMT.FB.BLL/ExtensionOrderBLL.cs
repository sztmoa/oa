using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TM_SaaS_OA_EFModel;
using System.Xml.Linq;
using System.Data.Objects.DataClasses;
using SMT.Foundation.Log;

namespace SMT.FB.BLL
{
    public class ExtensionOrderBLL : FBCommonBLL
    {

        public List<FBEntity> QueryDefault(QueryExpression qe)
        {
            List<EntityObject> list = BaseGetEntities(qe);
            return list.ToFBEntityList();

        }
    }
}
