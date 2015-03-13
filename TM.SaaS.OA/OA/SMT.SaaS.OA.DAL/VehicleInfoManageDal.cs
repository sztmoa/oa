using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TM_SaaS_OA_EFModel;
using SMT.Foundation.Core;
using System.Data.Objects;

namespace SMT.SaaS.OA.DAL
{
    public class VehicleInfoManageDal : CommDaL<T_OA_VEHICLE>
    {
        //TM_SaaS_OA_EFModelContext objModelContext = new TM_SaaS_OA_EFModelContext();
        public ObjectQuery<T_OA_VEHICLE> GetVehicleInfoList()
        {
            return GetObjects<T_OA_VEHICLE>();
        }
    }
}
