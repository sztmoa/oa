using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT.FBAnalysis.DAL;
using SMT.Foundation.Log;

namespace SMT.FBAnalysis.BLL
{
    public class CommBll<TEntity>
    {
        /// <summary>
        /// 修改实体审核状态
        /// </summary>
        /// <param name="strEntityName">实体名</param>
        /// <param name="EntityKeyName">主键名</param>
        /// <param name="EntityKeyValue">主键值</param>
        /// <param name="CheckState">审核状态</param>
        public int UpdateCheckState(string strEntityName, string EntityKeyName, string EntityKeyValue, string CheckState)
        {
            using (CommDal<TEntity> dal = new CommDal<TEntity>())
            {
                int intResult=0;
                Tracer.Debug("进入了COMMONBLL，实体名：" + strEntityName +  "，DateTime:" + System.DateTime.Now.ToString());
                Tracer.Debug("实体ID名："+EntityKeyName + "实体主键值："+ EntityKeyValue);
                Tracer.Debug("审核的状态："+ CheckState);
                switch (strEntityName)
                {
                    case "T_FB_BORROWAPPLYMASTER"://借款申请
                        BorrowApplyMasterBLL bll = new BorrowApplyMasterBLL();
                        intResult = bll.GetBorrowApplyForMobile(EntityKeyValue,CheckState);
                        break;
                    case "T_FB_CHARGEAPPLYMASTER":
                        ChargeApplyMasterBLL Chargebll = new ChargeApplyMasterBLL();
                        intResult = Chargebll.GetChargeApplyForMobile(EntityKeyValue,CheckState);
                        break;
                    case "T_FB_REPAYAPPLYMASTER":
                        RepayApplyMasterBLL Repaybll = new RepayApplyMasterBLL();
                        intResult = Repaybll.GetRepayApplyForMobile(EntityKeyValue,CheckState);
                        break;
                    default:
                       intResult = dal.UpdateCheckState(strEntityName, EntityKeyName, EntityKeyValue, CheckState);
                       break;
                }
                

                return intResult;
            }

        }
    }
}
