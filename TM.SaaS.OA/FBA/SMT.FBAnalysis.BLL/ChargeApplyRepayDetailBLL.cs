/*
 * 文件名：ChargeApplyRepayDetailBLL.cs
 * 作  用：T_FB_CHARGEAPPLYREPAYDETAIL 业务逻辑类
 * 创建人：朱磊
 * 创建时间：2011-12-30
 * 修改人：
 * 修改时间：
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using SMT_FB_EFModel;
using SMT.FBAnalysis.DAL;
using SMT.Foundation.Log;

namespace SMT.FBAnalysis.BLL
{
    public class ChargeApplyRepayDetailBLL : BaseBll<T_FB_CHARGEAPPLYREPAYDETAIL>
    {
        public ChargeApplyRepayDetailBLL()
        { }

        #region 获取数据
        /// <summary>
        /// 根据主表ID获取报销冲借款明细数据
        /// </summary>
        /// <param name="strChargeApplyMasterId"></param>
        /// <returns></returns>
        public List<T_FB_CHARGEAPPLYREPAYDETAIL> GetChargeApplyRepayDetailByMasterID(string strChargeApplyMasterId)
        {
            if (string.IsNullOrEmpty(strChargeApplyMasterId))
            {
                return null;
            }

            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            List<T_FB_CHARGEAPPLYREPAYDETAIL> chaRepList = new List<T_FB_CHARGEAPPLYREPAYDETAIL>();

            if (!string.IsNullOrEmpty(strChargeApplyMasterId))
            {
                strFilter.Append(" T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID == @0");
                objArgs.Add(strChargeApplyMasterId);
            }

            try
            {
                IQueryable<T_FB_CHARGEAPPLYREPAYDETAIL> entRds = GetChargeApplyRepayDetailRdList("UPDATEDATE",strFilter.ToString(),objArgs.ToArray());
                if (entRds == null)
                {
                    return null;
                }
                foreach (T_FB_CHARGEAPPLYREPAYDETAIL da in entRds)
                {
                    if (da.T_FB_CHARGEAPPLYMASTER.CHECKSTATES != 2)
                    {
                        var a = from i in dal.GetObjects<T_FB_PERSONACCOUNT>()
                                where i.OWNERCOMPANYID == da.T_FB_CHARGEAPPLYMASTER.OWNERCOMPANYID
                                && i.OWNERID == da.T_FB_CHARGEAPPLYMASTER.OWNERID
                                select i;
                        if (da.REPAYTYPE == 1)
                        {
                            da.BORROWMONEY = a.FirstOrDefault().SIMPLEBORROWMONEY.Value;
                        }
                        if (da.REPAYTYPE == 2)
                        {
                            da.BORROWMONEY = a.FirstOrDefault().BACKUPBORROWMONEY.Value;
                        }
                        if (da.REPAYTYPE == 3)
                        {
                            da.BORROWMONEY = a.FirstOrDefault().SPECIALBORROWMONEY.Value;
                        }
                    }
                    chaRepList.Add(da);
                }
                return chaRepList;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetChargeApplyRepayDetailByMasterID，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        /// <summary>
        /// 根据条件获取报销冲借款明细数据
        /// </summary>
        /// <param name="strOrderBy"></param>
        /// <param name="strFilter"></param>
        /// <param name="objArgs"></param>
        /// <returns></returns>
        public IQueryable<T_FB_CHARGEAPPLYREPAYDETAIL> GetChargeApplyRepayDetailRdList(string strOrderBy, string strFilter, params object[] objArgs)
        {
            try
            {
                var q = from v in dal.GetObjects<T_FB_CHARGEAPPLYREPAYDETAIL>().Include("T_FB_CHARGEAPPLYMASTER")
                        select v;

                if (objArgs.Count() > 0 && !string.IsNullOrEmpty(strFilter))
                {
                    q = q.Where(strFilter, objArgs);
                }

                return q.OrderBy(strOrderBy);
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：GetChargeApplyRepayDetailRdList，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
            }
            return null;
        }

        #endregion

        #region 删除数据
        /// <summary>
        /// 删除费用报销冲借款明细表数据   add by zl
        /// </summary>
        /// <param name="chargeMasterID"></param>
        /// <returns></returns>
        public bool DelChargeApplyRepayDetail(string chargeMasterID)
        {
            bool re = true;

            try
            {
                var entitys = from ent in dal.GetObjects().Include("T_FB_CHARGEAPPLYMASTER")
                              where ent.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID == chargeMasterID
                              select ent;

                if (entitys.Count() > 0)
                {
                    foreach (T_FB_CHARGEAPPLYREPAYDETAIL obj in entitys)
                    {
                        re = Delete(obj);
                        if (!re)
                        {
                            break;
                        }
                    }
                }

                return re;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：DelChargeApplyRepayDetail，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        #endregion

        #region 更新数据
        /// <summary>
        /// 更新报销冲借款明细数据
        /// </summary>
        /// <param name="strChargeMasterID"></param>
        /// <param name="detailList"></param>
        /// <returns></returns>
        public bool UpdateChargeApplyRepayDetail(string strChargeMasterID, List<T_FB_CHARGEAPPLYREPAYDETAIL> detailList)
        {
            bool bRes = false;
            try
            {
                if (string.IsNullOrWhiteSpace(strChargeMasterID))
                {
                    return bRes;
                }

                ChargeApplyMasterBLL masterBLL = new ChargeApplyMasterBLL();
                T_FB_CHARGEAPPLYMASTER entMaster = masterBLL.GetChargeApplyMasterByID(strChargeMasterID);

                bRes = DelChargeApplyRepayDetail(strChargeMasterID);

                foreach (T_FB_CHARGEAPPLYREPAYDETAIL item in detailList)
                {
                    if (item.EntityKey != null)
                    {
                        item.EntityKey = null;
                    }

                    item.CHARGEAPPLYREPAYDETAILID = System.Guid.NewGuid().ToString();
                    if (item.T_FB_CHARGEAPPLYMASTER == null)
                    {
                        item.T_FB_CHARGEAPPLYMASTER = entMaster;
                    }

                    item.T_FB_CHARGEAPPLYMASTER.EntityKey = new System.Data.EntityKey("SMT_FB_EFModelContext.T_FB_CHARGEAPPLYMASTER", "CHARGEAPPLYMASTERID", entMaster.CHARGEAPPLYMASTERID);
                    Add(item);
                }
                bRes = true;
            }
            catch (Exception ex)
            {
                Tracer.Debug(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "调用函数 UpdateChargeApplyRepayDetail 出现异常，异常信息为：" + ex.ToString());
                bRes = false;
            }

            return bRes;
        }

        #endregion
    }
}
