using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT_FB_EFModel;
using SMT.FBAnalysis.CustomModel;
using SMT.Foundation.Log;

namespace SMT.FBAnalysis.DAL
{
    public class OaInterfaceDAL : CommDal<T_FB_DEPTTRANSFERMASTER>
    {
        public OaInterfaceDAL()
        {
        }

        /// <summary>
        /// 根据扩展单编号查询申请单编号。
        /// </summary>
        /// <param name="orderID">扩展单编号。</param>
        /// <returns>返回申请单编号。</returns>
        public string GetApplyOrderID(string orderID)
        {
            string id = string.Empty;
            id = this.GetChargeapplymasterID(orderID);
            if (id.Equals(string.Empty))
            {
                id = this.GetTravelExpApplyMasterCode(orderID);
            }

            return id;
        }

        /// <summary>
        /// 根据扩展单编号查询申请单编号。
        /// </summary>
        /// <param name="orderID">扩展单编号。</param>
        /// <returns>返回申请单编号。</returns>
        private string GetChargeapplymasterID(string orderID)
        {
            string id = string.Empty;
            try
            {
               

                var q = from b in GetObjects<T_FB_CHARGEAPPLYMASTER>().Include("T_FB_EXTENSIONALORDER")
                        where b.T_FB_EXTENSIONALORDER.ORDERID == orderID
                        orderby b.UPDATEDATE descending
                        select b.CHARGEAPPLYMASTERCODE;

                if (q == null)
                {
                    return id;
                }

                if (q.Count() == 0)
                {
                    return id; 
                }

                id = q.FirstOrDefault();
            }
            catch (Exception e)
            {
                Tracer.Debug(e.ToString());
            }

            return id;
        }

        /// <summary>
        /// 根据扩展单编号查询申请单编号。
        /// </summary>
        /// <param name="orderID">扩展单编号。</param>
        /// <returns>返回申请单编号。</returns>
        private string GetTravelExpApplyMasterCode(string orderID)
        {
            string code = "";
            try
            {
                List<ExtensionalOrderCode> codeList = new List<ExtensionalOrderCode>();

                var q = from b in GetObjects<T_FB_TRAVELEXPAPPLYDETAIL>().Include("T_FB_TRAVELEXPAPPLYMASTER")
                        join c in GetObjects<T_FB_EXTENSIONALORDER>() on b.T_FB_TRAVELEXPAPPLYMASTER.T_FB_EXTENSIONALORDER.EXTENSIONALORDERID equals c.EXTENSIONALORDERID
                        where c.ORDERID == orderID
                        select new ExtensionalOrderCode
                        {
                            Code = b.T_FB_TRAVELEXPAPPLYMASTER.TRAVELEXPAPPLYMASTERCODE
                        };
                codeList = q.ToList();

                if (codeList != null && codeList.Count > 0)
                {
                    code = codeList[0].Code;
                }
            }
            catch (Exception e)
            {
                Tracer.Debug(e.InnerException.Message);
            }
            return code;
        }
    }

    public class ExtensionalOrderCode
    {
        public string Code { get; set; }
    }
}
