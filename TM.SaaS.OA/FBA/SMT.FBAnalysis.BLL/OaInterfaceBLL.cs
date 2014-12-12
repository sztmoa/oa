
/*
 * 文件名：OaInterfaceBLL.cs
 * 作  用：OA 系统接口业务逻辑类
 * 创建人：吕超
 * 创建时间：2011-01-27 15:33:04
 * 修改人：
 * 修改时间：
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT_FB_EFModel;
using SMT.FBAnalysis.DAL;

namespace SMT.FBAnalysis.BLL
{
    public class OaInterfaceBLL
    {
        /// <summary>
        /// 根据扩展单编号查询申请单编号。
        /// </summary>
        /// <param name="orderID">扩展单编号。</param>
        /// <returns>返回申请单编号。</returns>
        public string GetApplyOrderID(string orderID)
        {
            string strRes = string.Empty;
            try
            {
                OaInterfaceDAL dal = new OaInterfaceDAL();

                strRes = dal.GetApplyOrderID(orderID);
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.ToString());
            }

            return strRes;
        }
    }
}
