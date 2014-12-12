/*
 * 文件名：OaInterface.svc.cs
 * 作  用：OA查询单号专用
 * 创建人：吕超
 * 创建时间：2011-01-27 15:33:04
 * 修改人：
 * 修改时间：
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SMT.FBAnalysis.BLL;
using System.ServiceModel.Activation;
using SMT_FB_EFModel;

namespace SMT.FBAnalysis.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“OaInterface”。
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OaInterface
    {
        [OperationContract]
        public void DoWork()
        {
        }        

        /// <summary>
        /// 根据扩展单编号查询申请单编号。
        /// </summary>
        /// <param name="orderID">扩展单编号。</param>
        /// <returns>返回申请单编号。</returns>
        [OperationContract]
        public string GetApplyOrderID(string orderID)
        {
            if (string.IsNullOrWhiteSpace(orderID))
            {
                return string.Empty;
            }

            OaInterfaceBLL bll = new OaInterfaceBLL();

            return bll.GetApplyOrderID(orderID);
        }


        /// <summary>
        /// 根据扩展单编号查询申请单编号。
        /// </summary>
        /// <param name="orderID">扩展单编号。</param>
        /// <returns>返回申请单编号。</returns>
        [OperationContract]
        public string SaveChargeRdByExtenOrder(string strFormType, T_FB_EXTENSIONALORDER entTemp)
        {
            string strRes = string.Empty;
            try
            {
                if (entTemp == null || string.IsNullOrWhiteSpace(strFormType))
                {
                    return strRes;
                }

                ExtensionalOrderBLL bllExtensionalOrder = new ExtensionalOrderBLL();
                bllExtensionalOrder.SaveExtensionalOrderRd(entTemp, strFormType);

                OaInterfaceBLL bll = new OaInterfaceBLL();

                strRes = bll.GetApplyOrderID(entTemp.ORDERID);
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.Message);
            }

            return strRes;
        }
    }
}
