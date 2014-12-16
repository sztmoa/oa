using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.HRM.CustomModel
{
    public class V_RELATIONPOST
    {
        public V_RELATIONPOST()
        { 
        }

        public string PostCode { get; set; }
        public string PostName { get; set; }
        public string RelationPostID { get; set; }
        public TM_SaaS_OA_EFModel.T_HR_POST Post { get; set; }
    }
     
}
