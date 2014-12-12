
/*
 * 文件名：PersonAccountBLL.cs
 * 作  用：T_FB_PERSONACCOUNT 业务逻辑类
 * 创建人：朱磊
 * 创建时间：2010-12-15 11:47:04
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
    public class PersonAccountBLL:BaseBll<T_FB_PERSONACCOUNT>
    {
        public PersonAccountBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_PERSONACCOUNT信息
        /// </summary>
        /// <param name="strPersonAccountId">主键索引</param>
        /// <returns></returns>
        public T_FB_PERSONACCOUNT GetPersonAccountByID(string strPersonAccountId)
        {
            if (string.IsNullOrEmpty(strPersonAccountId))
            {
                return null;
            }

            PersonAccountDAL dalPersonAccount = new PersonAccountDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();
            
            if (!string.IsNullOrEmpty(strPersonAccountId))
            {
                strFilter.Append(" PERSONACCOUNTID == @0");
                objArgs.Add(strPersonAccountId);
            }

            T_FB_PERSONACCOUNT entRd = dalPersonAccount.GetPersonAccountRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取T_FB_PERSONACCOUNT信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_PERSONACCOUNT> GetAllPersonAccountRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            PersonAccountDAL dalPersonAccount = new PersonAccountDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " PERSONACCOUNTID ";
            }

            var q = dalPersonAccount.GetPersonAccountRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }


        public static List<T_FB_REPAYAPPLYDETAIL> GetAllPersonAccountRdListByMultSearchForMobile(string strFilter, List<object> objArgs, string strSortKey, string employeeID)
        {
            List<T_FB_REPAYAPPLYDETAIL> repDtlobj = new List<T_FB_REPAYAPPLYDETAIL>();
            try
            {
                PersonAccountDAL dalPersonAccount = new PersonAccountDAL();
                string strOrderBy = string.Empty;

                if (!string.IsNullOrEmpty(strSortKey))
                {
                    strOrderBy = strSortKey;
                }
                else
                {
                    strOrderBy = " PERSONACCOUNTID ";
                }

                var q = dalPersonAccount.GetPersonAccountRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
                Tracer.Debug("GetAllPersonAccountRdListByMultSearchForMobile- q的数量："+ q.Count());
                if (q.Count() > 0)
                {
                    T_FB_PERSONACCOUNT PerEntity = new T_FB_PERSONACCOUNT();
                    PerEntity = q.FirstOrDefault();
                    if (PerEntity.SIMPLEBORROWMONEY > 0)
                    {
                        T_FB_REPAYAPPLYDETAIL repay = new T_FB_REPAYAPPLYDETAIL();
                        repay.BORROWMONEY = PerEntity.SIMPLEBORROWMONEY.Value;
                        repay.CREATEDATE = DateTime.Now;
                        repay.CREATEUSERID = employeeID;
                        repay.REMARK = "";
                        repay.REPAYAPPLYDETAILID = System.Guid.NewGuid().ToString();
                        repay.REPAYMONEY = 0;
                        repay.REPAYTYPE = 1;
                        repay.UPDATEDATE = DateTime.Now;
                        repay.UPDATEUSERID = employeeID;
                        repDtlobj.Add(repay);
                    }
                    if (PerEntity.BACKUPBORROWMONEY > 0)
                    {
                        T_FB_REPAYAPPLYDETAIL repay = new T_FB_REPAYAPPLYDETAIL();
                        repay.BORROWMONEY = PerEntity.BACKUPBORROWMONEY.Value;
                        repay.CREATEDATE = DateTime.Now;
                        repay.CREATEUSERID = employeeID;
                        repay.REMARK = "";
                        repay.REPAYAPPLYDETAILID = System.Guid.NewGuid().ToString();
                        repay.REPAYMONEY = 0;
                        repay.REPAYTYPE = 2;
                        repay.UPDATEDATE = DateTime.Now;
                        repay.UPDATEUSERID = employeeID;
                        repDtlobj.Add(repay);
                    }
                    if (PerEntity.SPECIALBORROWMONEY > 0)
                    {
                        T_FB_REPAYAPPLYDETAIL repay = new T_FB_REPAYAPPLYDETAIL();
                        repay.BORROWMONEY = PerEntity.SPECIALBORROWMONEY.Value;
                        repay.CREATEDATE = DateTime.Now;
                        repay.CREATEUSERID = employeeID;
                        repay.REMARK = "";
                        repay.REPAYAPPLYDETAILID = System.Guid.NewGuid().ToString();
                        repay.REPAYMONEY = 0;
                        repay.REPAYTYPE = 3;
                        repay.UPDATEDATE = DateTime.Now;
                        repay.UPDATEUSERID = employeeID;
                        repDtlobj.Add(repay);
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetAllPersonAccountRdListByMultSearchForMobile-出现错误：" + ex.ToString());
            }
            return repDtlobj;
        }

        /// <summary>
        /// 根据条件，获取T_FB_PERSONACCOUNT信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_PERSONACCOUNT信息</returns>
        public IQueryable<T_FB_PERSONACCOUNT> GetPersonAccountRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllPersonAccountRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_PERSONACCOUNT>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion

        #region 操作数据  zl
        /// <summary>
        /// 增加PersonAccount表数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool AddPersonAccount(T_FB_PERSONACCOUNT entity)
        {
            bool re;
            try
            {
                dal.BeginTransaction();
                re = Add(entity);
                if (!re)
                {
                    dal.RollbackTransaction();
                    return false;
                }

                dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                string ErrInfo = this.GetType().ToString() + "：AddPersonAccount，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                dal.RollbackTransaction();
                return false;
            }
        }

        /// <summary>
        /// 借款单审核通过后更新T_FB_PERSONACCOUNT表数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UptPersonAccountByBorr(T_FB_BORROWAPPLYMASTER entity)
        {
            bool re;
            string ErrInfo = "";
            T_FB_PERSONACCOUNT entPerson=new T_FB_PERSONACCOUNT();
            try
            {
                IQueryable<T_FB_PERSONACCOUNT> qAccount = from a in dal.GetObjects()
                                                          where
                                                              a.OWNERID == entity.OWNERID &&
                                                              a.OWNERCOMPANYID == entity.OWNERCOMPANYID
                                                          select a;

                if(qAccount == null || qAccount.Count() == 0)
                {
                    entPerson.PERSONACCOUNTID = System.Guid.NewGuid().ToString();
                    if(entity.REPAYTYPE==1)
                    {
                        entPerson.BACKUPBORROWMONEY = 0;
                        entPerson.SIMPLEBORROWMONEY = entity.TOTALMONEY;
                        entPerson.SPECIALBORROWMONEY = 0;
                    }
                    else if(entity.REPAYTYPE==2)
                    {
                        entPerson.BACKUPBORROWMONEY = entity.TOTALMONEY;
                        entPerson.SIMPLEBORROWMONEY = 0;
                        entPerson.SPECIALBORROWMONEY = 0;
                    }
                    else if(entity.REPAYTYPE==3)
                    {
                        entPerson.BACKUPBORROWMONEY = 0;
                        entPerson.SIMPLEBORROWMONEY = 0;
                        entPerson.SPECIALBORROWMONEY = entity.TOTALMONEY;
                    }
                    entPerson.BORROWMONEY = entPerson.BACKUPBORROWMONEY + entPerson.SIMPLEBORROWMONEY +
                                            entPerson.SPECIALBORROWMONEY;
                    
                    entPerson.CREATECOMPANYID = entity.CREATECOMPANYID;
                    entPerson.CREATEDATE = DateTime.Now;
                    entPerson.CREATEDEPARTMENTID = entity.CREATEDEPARTMENTID;
                    entPerson.CREATEPOSTID = entity.CREATEPOSTID;
                    entPerson.CREATEUSERID = entity.CREATEUSERID;
                    entPerson.NEXTREPAYDATE = entity.PLANREPAYDATE;
                    entPerson.OWNERCOMPANYID = entity.OWNERCOMPANYID;
                    entPerson.OWNERDEPARTMENTID = " ";
                    entPerson.OWNERID = entity.OWNERID;
                    entPerson.OWNERPOSTID = " ";
                    entPerson.REMARK = "";
                    entPerson.UPDATEDATE = DateTime.Now;
                    entPerson.UPDATEUSERID = entity.UPDATEUSERID;

                    re = AddPersonAccount(entPerson);
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单ID：" + entity.BORROWAPPLYMASTERID + ", 借款审核通过时更新PersonAccount表，新增总账记录，金额：" + entity.TOTALMONEY;
                    if(!re)
                    {
                        ErrInfo += "， 失败！";
                        Tracer.Debug(ErrInfo);
                        return false;
                    }
                    CreatePersonAccountWaterFlow(entPerson, entity);  //写流水
                }
                else
                {
                    T_FB_PERSONACCOUNT obj = qAccount.FirstOrDefault();
                    if(entity.REPAYTYPE==1)
                    {
                        obj.SIMPLEBORROWMONEY += entity.TOTALMONEY;
                        ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单ID：" + entity.BORROWAPPLYMASTERID + ", 借款审核通过时更新PersonAccount表，普通借款，增加金额" + entity.TOTALMONEY;
                    }
                    else if(entity.REPAYTYPE==2)
                    {
                        obj.BACKUPBORROWMONEY += entity.TOTALMONEY;
                        ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单ID：" + entity.BORROWAPPLYMASTERID + ", 借款审核通过时更新PersonAccount表，备用金借款，增加金额" + entity.TOTALMONEY;  
                    }
                    else if(entity.REPAYTYPE==3)
                    {
                        obj.SPECIALBORROWMONEY += entity.TOTALMONEY;
                        ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单ID：" + entity.BORROWAPPLYMASTERID + ", 借款审核通过时更新PersonAccount表，专项借款，增加金额" + entity.TOTALMONEY;  
                    }
                    obj.BORROWMONEY += entity.TOTALMONEY;
                    obj.NEXTREPAYDATE = entity.PLANREPAYDATE;
                    obj.UPDATEUSERID = entity.UPDATEUSERID;
                    obj.UPDATEDATE = DateTime.Now;

                    re = Update(obj);
                    if(!re)
                    {
                        ErrInfo += "， 失败！";
                        Tracer.Debug(ErrInfo);
                        return false;
                    }
                    CreatePersonAccountWaterFlow(obj, entity);  //写流水
                }
                ErrInfo += "， 成功！";
                Tracer.Debug(ErrInfo);
                return true;
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：UptPersonAccountByBorr，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        /// <summary>
        /// 还款审核中时检查还款金额是否大于借款余额
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <param name="dCheckState"></param>
        /// <returns></returns>
        public string CheckRepMoneyForRepay(T_FB_REPAYAPPLYMASTER entity, List<T_FB_REPAYAPPLYDETAIL> detailList, decimal dCheckState)
        {
            string sResult = "";
            string ErrInfo = "";
            T_FB_PERSONACCOUNT entPerson = new T_FB_PERSONACCOUNT();
            try
            {
                IQueryable<T_FB_PERSONACCOUNT> qAccount = from a in dal.GetObjects()
                                                          where
                                                              a.OWNERID == entity.OWNERID &&
                                                              a.OWNERCOMPANYID == entity.OWNERCOMPANYID
                                                          select a;

                if(qAccount != null && qAccount.Count() > 0)
                {
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 还款单ID：" + entity.REPAYAPPLYMASTERID;
                    T_FB_PERSONACCOUNT obj = qAccount.FirstOrDefault();
                    foreach (T_FB_REPAYAPPLYDETAIL rep in detailList)
                    {
                        if (dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approving) || dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approved))
                        {
                            if (rep.REPAYTYPE == 1)
                            {
                                if(obj.SIMPLEBORROWMONEY < rep.REPAYMONEY)
                                {
                                    ErrInfo += " 明细ID：" + rep.REPAYAPPLYDETAILID + ", 还普通借款审核中时还款金额 " + rep.REPAYMONEY + " 大于借款余额 " + obj.SIMPLEBORROWMONEY + " 审核终止 ||";
                                    Tracer.Debug(ErrInfo);
                                    sResult = "还款额度不能大于借款余额，请审核不通过。";
                                    return sResult;
                                }
                            }
                            else if (rep.REPAYTYPE == 2)
                            {
                                if(obj.BACKUPBORROWMONEY < rep.REPAYMONEY)
                                {
                                    ErrInfo += " 明细ID：" + rep.REPAYAPPLYDETAILID + ", 还备用金借款审核中时还款金额 " + rep.REPAYMONEY + " 大于借款余额 " + obj.BACKUPBORROWMONEY + " 审核终止 ||";
                                    Tracer.Debug(ErrInfo);
                                    sResult = "还款额度不能大于借款余额，请审核不通过。";
                                    return sResult;
                                }
                            }
                            else if (rep.REPAYTYPE == 3)
                            {
                                if(obj.SPECIALBORROWMONEY < rep.REPAYMONEY)
                                {
                                    ErrInfo += " 明细ID：" + rep.REPAYAPPLYDETAILID + ", 还专项借款审核中时还款金额 " + rep.REPAYMONEY + " 大于借款余额 " + obj.SPECIALBORROWMONEY + " 审核终止 ||";
                                    Tracer.Debug(ErrInfo);
                                    sResult = "还款额度不能大于借款余额，请审核不通过。";
                                    return sResult;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 还款单ID：" + entity.REPAYAPPLYMASTERID + ", PersonAccount表中找不到相关数据。";
                    Tracer.Debug(ErrInfo);
                    sResult = "没有找到相应的借款总账数据，审核终止！";
                    return sResult;
                }
                return "";
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：CheckRepMoneyForRepay，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                sResult = "还款金额检查程序异常，审核终止！";
                return sResult;  
            }
        }

        /// <summary>
        /// 还款单审核通过后更新T_FB_PERSONACCOUNT表数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UptPersonAccountByRepa(T_FB_REPAYAPPLYMASTER entity, List<T_FB_REPAYAPPLYDETAIL> detailList, decimal dCheckState)
        {
            bool re;
            string ErrInfo = "";
            T_FB_PERSONACCOUNT entPerson = new T_FB_PERSONACCOUNT();
            try
            {
                IQueryable<T_FB_PERSONACCOUNT> qAccount = from a in dal.GetObjects()
                                                          where
                                                              a.OWNERID == entity.OWNERID &&
                                                              a.OWNERCOMPANYID == entity.OWNERCOMPANYID
                                                          select a;

                if(qAccount != null && qAccount.Count() > 0)
                {
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 还款单ID：" + entity.REPAYAPPLYMASTERID;
                    T_FB_PERSONACCOUNT obj = qAccount.FirstOrDefault();
                    foreach (T_FB_REPAYAPPLYDETAIL rep in detailList)
                    {
                        if (dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approved))  //终审通过
                        {
                            if(rep.REPAYMONEY == 0)
                            {
                                continue;
                            }
                            if (rep.REPAYTYPE == 1)
                            {
                                obj.SIMPLEBORROWMONEY -= rep.REPAYMONEY;
                                ErrInfo += " 明细ID：" + rep.REPAYAPPLYDETAILID + ", 还款终审通过时更新PersonAccount表，还普通借款，扣减金额" + rep.REPAYMONEY + "||";
                            }
                            else if (rep.REPAYTYPE == 2)
                            {
                                obj.BACKUPBORROWMONEY -= rep.REPAYMONEY;
                                ErrInfo += " 明细ID：" + rep.REPAYAPPLYDETAILID + ", 还款终审通过时更新PersonAccount表，还备用金借款，扣减金额" + rep.REPAYMONEY + "||";
                            }
                            else if(rep.REPAYTYPE == 3)
                            {
                                obj.SPECIALBORROWMONEY -= rep.REPAYMONEY;
                                ErrInfo += " 明细ID：" + rep.REPAYAPPLYDETAILID + ", 还款终审通过时更新PersonAccount表，还专项借款，扣减金额" + rep.REPAYMONEY + "||";
                            }
                            obj.BORROWMONEY -= rep.REPAYMONEY;
                        }
                        obj.UPDATEUSERID = entity.UPDATEUSERID;
                        obj.UPDATEDATE = DateTime.Now;

                        re = Update(obj);
                        if (!re)
                        {
                            ErrInfo += "， 失败！";
                            Tracer.Debug(ErrInfo);
                            return false;
                        }
                        CreatePersonAccountWaterFlow(obj, rep);  //写流水
                    }   
                }
                else
                {
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 还款单ID：" + entity.REPAYAPPLYMASTERID + ", PersonAccount表中找不到相关数据。";
                    Tracer.Debug(ErrInfo);
                    return false;
                }
                ErrInfo += "， 成功！";
                Tracer.Debug(ErrInfo);
                return true;
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：UptPersonAccountByRepa，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        /// <summary>
        /// 报销冲借款审核中时检查冲款金额是否大于借款余额
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="detailList"></param>
        /// <param name="dCheckState"></param>
        /// <returns></returns>
        public string CheckRepMoneyForCharge(T_FB_CHARGEAPPLYMASTER entity, List<T_FB_CHARGEAPPLYREPAYDETAIL> detailList, decimal dCheckState)
        {
            string sResult = "";
            string ErrInfo = "";
            T_FB_PERSONACCOUNT entPerson = new T_FB_PERSONACCOUNT();
            try
            {
                IQueryable<T_FB_PERSONACCOUNT> qAccount = from a in dal.GetObjects()
                                                          where
                                                              a.OWNERID == entity.OWNERID &&
                                                              a.OWNERCOMPANYID == entity.OWNERCOMPANYID
                                                          select a;

                if (qAccount != null && qAccount.Count() > 0)
                {
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 报销单ID：" + entity.CHARGEAPPLYMASTERID;
                    T_FB_PERSONACCOUNT obj = qAccount.FirstOrDefault();
                    foreach (T_FB_CHARGEAPPLYREPAYDETAIL rep in detailList)
                    {
                        if (dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approving) || dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approved))
                        {
                            if (rep.REPAYTYPE == 1)
                            {
                                if (obj.SIMPLEBORROWMONEY < rep.REPAYMONEY)
                                {
                                    ErrInfo += " 明细ID：" + rep.CHARGEAPPLYREPAYDETAILID + ", 报销冲普通借款审核中时还款金额 " + rep.REPAYMONEY + " 大于借款余额 " + obj.SIMPLEBORROWMONEY + " 审核终止 ||";
                                    Tracer.Debug(ErrInfo);
                                    sResult = "冲借款金额不能大于借款余额，请修改后再操作。";
                                    return sResult;
                                }
                            }
                            else if (rep.REPAYTYPE == 2)
                            {
                                if (obj.BACKUPBORROWMONEY < rep.REPAYMONEY)
                                {
                                    ErrInfo += " 明细ID：" + rep.CHARGEAPPLYREPAYDETAILID + ", 报销冲备用金借款审核中时还款金额 " + rep.REPAYMONEY + " 大于借款余额 " + obj.BACKUPBORROWMONEY + " 审核终止 ||";
                                    Tracer.Debug(ErrInfo);
                                    sResult = "冲借款金额不能大于借款余额，请修改后再操作。";
                                    return sResult;
                                }
                            }
                            else if (rep.REPAYTYPE == 3)
                            {
                                if (obj.SPECIALBORROWMONEY < rep.REPAYMONEY)
                                {
                                    ErrInfo += " 明细ID：" + rep.CHARGEAPPLYREPAYDETAILID + ", 报销冲专项借款审核中时还款金额 " + rep.REPAYMONEY + " 大于借款余额 " + obj.SPECIALBORROWMONEY + " 审核终止 ||";
                                    Tracer.Debug(ErrInfo);
                                    sResult = "冲借款金额不能大于借款余额，请修改后再操作。";
                                    return sResult;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 报销单ID：" + entity.CHARGEAPPLYMASTERID + ", PersonAccount表中找不到相关数据。";
                    Tracer.Debug(ErrInfo);
                    sResult = "没有找到相应的借款总账数据，审核终止！";
                    return sResult;
                }
                return "";
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：CheckRepMoneyForCharge，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                sResult = "还款金额检查程序异常，审核终止！";
                return sResult;
            }
        }

        /// <summary>
        /// 冲借款报销单审核通过时扣掉T_FB_PERSONACCOUNT表借款金额
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="checkstate"></param>
        /// <returns></returns>
        public bool UptPersonAccountByChar(T_FB_CHARGEAPPLYMASTER entity, List<T_FB_CHARGEAPPLYREPAYDETAIL> detailList, decimal dCheckState)
        {
            bool re;
            string ErrInfo = "";
            T_FB_PERSONACCOUNT entPerson = new T_FB_PERSONACCOUNT();

            try
            {
                IQueryable<T_FB_PERSONACCOUNT> qAccount = from a in dal.GetObjects()
                                                            where
                                                                a.OWNERID == entity.OWNERID &&
                                                                a.OWNERCOMPANYID == entity.OWNERCOMPANYID
                                                            select a;

                if (qAccount != null && qAccount.Count() > 0)
                {
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 报销单ID：" + entity.CHARGEAPPLYMASTERID;
                    T_FB_PERSONACCOUNT obj = qAccount.FirstOrDefault();
                    foreach (T_FB_CHARGEAPPLYREPAYDETAIL rep in detailList)
                    {
                        if (dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.Approved))  //终审通过
                        {
                            if(rep.REPAYMONEY == 0)
                            {
                                continue;
                            }
                            if (rep.REPAYTYPE == 1)
                            {
                                obj.SIMPLEBORROWMONEY -= rep.REPAYMONEY;
                                ErrInfo += " 冲借款明细ID：" + rep.CHARGEAPPLYREPAYDETAILID + ", 报销冲借款终审通过时更新PersonAccount表，冲普通借款，扣减金额" + rep.REPAYMONEY + "||";
                            }
                            else if (rep.REPAYTYPE == 2)
                            {
                                obj.BACKUPBORROWMONEY -= rep.REPAYMONEY;
                                ErrInfo += " 冲借款明细ID：" + rep.CHARGEAPPLYREPAYDETAILID + ", 报销冲借款终审通过时更新PersonAccount表，冲备用金借款，扣减金额" + rep.REPAYMONEY + "||";
                            }
                            else if(rep.REPAYTYPE == 3)
                            {
                                obj.SPECIALBORROWMONEY -= rep.REPAYMONEY;
                                ErrInfo += " 冲借款明细ID：" + rep.CHARGEAPPLYREPAYDETAILID + ", 报销冲借款终审通过时更新PersonAccount表，冲专项借款，扣减金额" + rep.REPAYMONEY + "||";
                            }
                            obj.BORROWMONEY -= rep.REPAYMONEY;
                        }
                        //else if (dCheckState == Convert.ToDecimal(FBAEnums.CheckStates.UnApproved)) //审核不通过
                        //{
                        //    if (rep.REPAYTYPE == 1)
                        //    {
                        //        obj.SIMPLEBORROWMONEY += rep.REPAYMONEY;
                        //        ErrInfo += " 冲借款明细ID：" + rep.CHARGEAPPLYREPAYDETAILID + ", 报销冲借款审核不通过时更新PersonAccount表，冲普通借款，增加金额" + rep.REPAYMONEY + "||";
                        //    }
                        //    else if (rep.REPAYTYPE == 2)
                        //    {
                        //        obj.BACKUPBORROWMONEY += rep.REPAYMONEY;
                        //        ErrInfo += " 冲借款明细ID：" + rep.CHARGEAPPLYREPAYDETAILID + ", 报销冲借款审核不通过时更新PersonAccount表，冲备用金借款，增加金额" + rep.REPAYMONEY + "||";
                        //    }
                        //    else if(rep.REPAYTYPE == 3)
                        //    {
                        //        obj.SPECIALBORROWMONEY += rep.REPAYMONEY;
                        //        ErrInfo += " 冲借款明细ID：" + rep.CHARGEAPPLYREPAYDETAILID + ", 报销冲借款审核不通过时更新PersonAccount表，冲专项借款，增加金额" + rep.REPAYMONEY + "||";
                        //    }
                        //    obj.BORROWMONEY += rep.REPAYMONEY;
                        //}
                        obj.UPDATEUSERID = entity.UPDATEUSERID;
                        obj.UPDATEDATE = DateTime.Now;

                        re = Update(obj);
                        if (!re)
                        {
                            ErrInfo += "， 失败！";
                            Tracer.Debug(ErrInfo);
                            return false;
                        }
                        CreatePersonAccountWaterFlow(obj, rep);  //写流水
                    }
                }
                else
                {
                    ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 报销单ID：" + entity.CHARGEAPPLYMASTERID + ", PersonAccount表中找不到相关数据。";
                    Tracer.Debug(ErrInfo);
                    return false;
                }
                ErrInfo += "， 成功！";
                Tracer.Debug(ErrInfo);
                return true;
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：UptPersonAccountByChar，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                return false;
            }
        }

        /// <summary>
        /// 写PersonAccount的流水账   2012.1.16
        /// </summary>
        /// <param name="perEnt"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool CreatePersonAccountWaterFlow(T_FB_PERSONACCOUNT perEnt, EntityObject entity)
        {
            T_FB_CHARGEAPPLYREPAYDETAIL chaDet = new T_FB_CHARGEAPPLYREPAYDETAIL();
            T_FB_BORROWAPPLYMASTER bor = new T_FB_BORROWAPPLYMASTER();
            T_FB_REPAYAPPLYDETAIL repDet = new T_FB_REPAYAPPLYDETAIL();
            WfPersonAccountBLL bllWfPer = new WfPersonAccountBLL();
            string masid = entity.EntityKey.ToString();
            string tab = "";
            string Log = "";
            bool n = true;
            string state = "";
            try
            {
                T_FB_WFPERSONACCOUNT wf = new T_FB_WFPERSONACCOUNT();
                wf.WFPERSONACCOUNTID = Guid.NewGuid().ToString();
                wf.BACKUPBORROWMONEY = perEnt.BACKUPBORROWMONEY;
                wf.BORROWMONEY = perEnt.BORROWMONEY;
                wf.CREATECOMPANYID = perEnt.CREATECOMPANYID;
                wf.CREATEDATE = perEnt.CREATEDATE;
                wf.CREATEDEPARTMENTID = perEnt.CREATEDEPARTMENTID;
                wf.CREATEPOSTID = perEnt.CREATEPOSTID;
                wf.CREATEUSERID = perEnt.CREATEUSERID;
                wf.NEXTREPAYDATE = perEnt.NEXTREPAYDATE;
                wf.OWNERCOMPANYID = perEnt.OWNERCOMPANYID;
                wf.OWNERDEPARTMENTID = perEnt.OWNERDEPARTMENTID;
                wf.OWNERID = perEnt.OWNERID;
                wf.OWNERPOSTID = perEnt.OWNERPOSTID;
                wf.PERSONACCOUNTID = perEnt.PERSONACCOUNTID;
                wf.REMARK = perEnt.REMARK;
                wf.SIMPLEBORROWMONEY = perEnt.SIMPLEBORROWMONEY;
                wf.SPECIALBORROWMONEY = perEnt.SPECIALBORROWMONEY;
                wf.TRIGGERBY = "";
                wf.UPDATEDATE = DateTime.Now;
                wf.UPDATEUSERID = perEnt.UPDATEUSERID;
                switch (entity.GetType().Name)
                {
                    case "T_FB_CHARGEAPPLYREPAYDETAIL":
                        chaDet = entity as T_FB_CHARGEAPPLYREPAYDETAIL;
                        if(chaDet != null)
                        {
                            if (chaDet.REPAYMONEY == 0)
                            {
                                return true;
                            }
                            masid = chaDet.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID;
                            tab = "T_FB_CHARGEAPPLYMASTER";
                            wf.OPERATIONMONEY = chaDet.REPAYMONEY;
                            wf.ORDERCODE = chaDet.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERCODE;
                            wf.ORDERDETAILID = chaDet.CHARGEAPPLYREPAYDETAILID;
                            wf.ORDERID = masid;
                            wf.ORDERTYPE = tab;
                            if (chaDet.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == 1)
                            {
                                state = "Approving";
                            }
                            else if (chaDet.T_FB_CHARGEAPPLYMASTER.CHECKSTATES == 2)
                            {
                                state = "Approved";
                            }
                            wf.TRIGGEREVENT = state;
                            n = bllWfPer.AddWfPersonAccount(wf);
                            if(!n)
                            {
                                Log = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 报销单冲借款明细ID：" + chaDet.CHARGEAPPLYREPAYDETAILID + " ，添加T_FB_WFPERSONACCOUNT表数据异常。";
                                Tracer.Debug(Log);
                            }
                            else
                            {
                                Log = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 报销单冲借款明细ID：" + chaDet.CHARGEAPPLYREPAYDETAILID + " ，添加T_FB_WFPERSONACCOUNT表数据成功。";
                                Tracer.Debug(Log);
                            } 
                        }
                        else
                        {
                            Log = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + "CreatePersonAccountWaterFlow 异常，没有找到相关报销数据。";
                            Tracer.Debug(Log);
                        }
                        break;
                    case "T_FB_BORROWAPPLYMASTER":
                        bor = entity as T_FB_BORROWAPPLYMASTER;
                        if(bor != null)
                        {
                            if (bor.TOTALMONEY == 0)
                            {
                                return true;
                            }
                            masid = bor.BORROWAPPLYMASTERID;
                            tab = "T_FB_BORROWAPPLYMASTER";
                            wf.OPERATIONMONEY = bor.TOTALMONEY;
                            wf.ORDERCODE = bor.BORROWAPPLYMASTERCODE;
                            wf.ORDERDETAILID = masid;
                            wf.ORDERID = masid;
                            wf.ORDERTYPE = tab;
                            if (bor.CHECKSTATES == 1)
                            {
                                state = "Approving";
                            }
                            else if (bor.CHECKSTATES == 2)
                            {
                                state = "Approved";
                            }
                            wf.TRIGGEREVENT = state;
                            n = bllWfPer.AddWfPersonAccount(wf);
                            if (!n)
                            {
                                Log = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单ID：" + bor.BORROWAPPLYMASTERID + " ，添加T_FB_WFPERSONACCOUNT表数据异常。";
                                Tracer.Debug(Log);
                            }
                            else
                            {
                                Log = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单ID：" + bor.BORROWAPPLYMASTERID + " ，添加T_FB_WFPERSONACCOUNT表数据成功。";
                                Tracer.Debug(Log);
                            }
                        }
                        else
                        {
                            Log = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + "CreatePersonAccountWaterFlow 异常，没有找到相关报销数据。";
                            Tracer.Debug(Log);
                        }
                        break;
                    case "T_FB_REPAYAPPLYDETAIL":
                        repDet = entity as T_FB_REPAYAPPLYDETAIL;
                        if(repDet != null)
                        {
                            if (repDet.REPAYMONEY == 0)
                            {
                                return true;
                            }
                            masid = repDet.T_FB_REPAYAPPLYMASTER.REPAYAPPLYMASTERID;
                            tab = "T_FB_REPAYAPPLYMASTER";
                            wf.OPERATIONMONEY = repDet.REPAYMONEY;
                            wf.ORDERCODE = repDet.T_FB_REPAYAPPLYMASTER.REPAYAPPLYCODE;
                            wf.ORDERDETAILID = repDet.REPAYAPPLYDETAILID;
                            wf.ORDERID = masid;
                            wf.ORDERTYPE = tab;
                            if (repDet.T_FB_REPAYAPPLYMASTER.CHECKSTATES == 1)
                            {
                                state = "Approving";
                            }
                            else if (repDet.T_FB_REPAYAPPLYMASTER.CHECKSTATES == 2)
                            {
                                state = "Approved";
                            }
                            wf.TRIGGEREVENT = state;
                            n = bllWfPer.AddWfPersonAccount(wf);
                            if (!n)
                            {
                                Log = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 还款单明细ID：" + repDet.REPAYAPPLYDETAILID + " ，添加T_FB_WFPERSONACCOUNT表数据异常。";
                                Tracer.Debug(Log);
                            }
                            else
                            {
                                Log = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 还款单明细ID：" + repDet.REPAYAPPLYDETAILID + " ，添加T_FB_WFPERSONACCOUNT表数据成功。";
                                Tracer.Debug(Log);
                            }
                            
                        }
                        else
                        {
                            Log = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + "CreatePersonAccountWaterFlow 异常，没有找到相关报销数据。";
                            Tracer.Debug(Log);
                        }
                        break;
                }      
                return true;
            }
            catch (Exception ex)
            {
                Tracer.Debug(ex.ToString());
                return false;
            }
        }

        #endregion

        #region  转移数据专用 zl
        /// <summary>
        /// 把借款表中审核通过的还未还清的单据的借款余额写到T_FB_PERSONACCOUNT表中
        /// </summary>
        /// <returns></returns>
        public bool TransPersonAccountByBorr()
        {
            bool re;
            string ErrInfo = "";
            DateTime dt = new DateTime(2012,1,1);
            try
            {
                //dal.BeginTransaction();
                //审核通过的还未还清的借款数据
                IQueryable<T_FB_BORROWAPPLYDETAIL> borObj =
                from a in dal.GetObjects<T_FB_BORROWAPPLYDETAIL>().Include("T_FB_BORROWAPPLYMASTER")
                where a.T_FB_BORROWAPPLYMASTER.CHECKSTATES == 2 &&
                      a.T_FB_BORROWAPPLYMASTER.ISREPAIED == 0 &&
                      a.UNREPAYMONEY > 0
                      //a.T_FB_BORROWAPPLYMASTER.UPDATEDATE < dt
                select a;

                foreach (T_FB_BORROWAPPLYDETAIL borDet in borObj)
                {
                    IQueryable<T_FB_PERSONACCOUNT> qAccount = from a in dal.GetObjects()
                                                              where
                                                                  a.OWNERID == borDet.T_FB_BORROWAPPLYMASTER.OWNERID &&
                                                                  a.OWNERCOMPANYID == borDet.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID
                                                              select a;
                    if (qAccount == null || qAccount.Count() == 0)
                    {
                        T_FB_PERSONACCOUNT entPerson = new T_FB_PERSONACCOUNT();
                        entPerson.PERSONACCOUNTID = System.Guid.NewGuid().ToString();
                        if (borDet.T_FB_BORROWAPPLYMASTER.REPAYTYPE == 1)
                        {
                            entPerson.BACKUPBORROWMONEY = 0;
                            entPerson.SIMPLEBORROWMONEY = borDet.UNREPAYMONEY;
                            entPerson.SPECIALBORROWMONEY = 0;
                        }
                        else if (borDet.T_FB_BORROWAPPLYMASTER.REPAYTYPE == 2)
                        {
                            entPerson.BACKUPBORROWMONEY = borDet.UNREPAYMONEY;
                            entPerson.SIMPLEBORROWMONEY = 0;
                            entPerson.SPECIALBORROWMONEY = 0;
                        }
                        else if (borDet.T_FB_BORROWAPPLYMASTER.REPAYTYPE == 3)
                        {
                            entPerson.BACKUPBORROWMONEY = 0;
                            entPerson.SIMPLEBORROWMONEY = 0;
                            entPerson.SPECIALBORROWMONEY = borDet.UNREPAYMONEY;
                        }
                        entPerson.BORROWMONEY = entPerson.BACKUPBORROWMONEY + entPerson.SIMPLEBORROWMONEY +
                                                entPerson.SPECIALBORROWMONEY;

                        entPerson.CREATECOMPANYID = borDet.T_FB_BORROWAPPLYMASTER.CREATECOMPANYID;
                        entPerson.CREATEDATE = DateTime.Now;
                        entPerson.CREATEDEPARTMENTID = borDet.T_FB_BORROWAPPLYMASTER.CREATEDEPARTMENTID;
                        entPerson.CREATEPOSTID = borDet.T_FB_BORROWAPPLYMASTER.CREATEPOSTID;
                        entPerson.CREATEUSERID = borDet.T_FB_BORROWAPPLYMASTER.CREATEUSERID;
                        entPerson.NEXTREPAYDATE = borDet.T_FB_BORROWAPPLYMASTER.PLANREPAYDATE;
                        entPerson.OWNERCOMPANYID = borDet.T_FB_BORROWAPPLYMASTER.OWNERCOMPANYID;
                        entPerson.OWNERDEPARTMENTID = " ";
                        entPerson.OWNERID = borDet.T_FB_BORROWAPPLYMASTER.OWNERID;
                        entPerson.OWNERPOSTID = " ";
                        entPerson.REMARK = "";
                        entPerson.UPDATEDATE = DateTime.Now;
                        entPerson.UPDATEUSERID = borDet.T_FB_BORROWAPPLYMASTER.UPDATEUSERID;

                        re = AddPersonAccount(entPerson);
                        ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单明细ID：" + borDet.BORROWAPPLYDETAILID + ", 新增总账记录，金额：" + borDet.UNREPAYMONEY;
                        if (!re)
                        {
                            ErrInfo += "， 失败！";
                            Tracer.Debug(ErrInfo);
                            //dal.RollbackTransaction();
                            return false;
                        }
                    }
                    else
                    {
                        T_FB_PERSONACCOUNT obj = qAccount.FirstOrDefault();
                        if (borDet.T_FB_BORROWAPPLYMASTER.REPAYTYPE == 1)
                        {
                            obj.SIMPLEBORROWMONEY += borDet.UNREPAYMONEY;
                            ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单明细ID：" + borDet.BORROWAPPLYDETAILID + ", 普通借款，增加金额" + borDet.UNREPAYMONEY;
                        }
                        else if (borDet.T_FB_BORROWAPPLYMASTER.REPAYTYPE == 2)
                        {
                            obj.BACKUPBORROWMONEY += borDet.UNREPAYMONEY;
                            ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单明细ID：" + borDet.BORROWAPPLYDETAILID + ", 用金借款，增加金额" + borDet.UNREPAYMONEY;
                        }
                        else if (borDet.T_FB_BORROWAPPLYMASTER.REPAYTYPE == 3)
                        {
                            obj.SPECIALBORROWMONEY += borDet.UNREPAYMONEY;
                            ErrInfo = this.GetType().ToString() + "," + System.DateTime.Now.ToString() + " 借款单明细ID：" + borDet.BORROWAPPLYDETAILID + ", 专项借款，增加金额" + borDet.UNREPAYMONEY;
                        }
                        obj.BORROWMONEY += borDet.UNREPAYMONEY;
                        obj.NEXTREPAYDATE = borDet.T_FB_BORROWAPPLYMASTER.PLANREPAYDATE;
                        obj.UPDATEUSERID = borDet.T_FB_BORROWAPPLYMASTER.UPDATEUSERID;
                        obj.UPDATEDATE = DateTime.Now;

                        re = Update(obj);
                        if (!re)
                        {
                            ErrInfo += "， 失败！";
                            Tracer.Debug(ErrInfo);
                            //dal.RollbackTransaction();
                            return false;
                        }
                    }
                }
                //dal.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                ErrInfo = this.GetType().ToString() + "：TransPersonAccountByBorr，" + System.DateTime.Now.ToString() + "，" + ex.Message;
                Tracer.Debug(ErrInfo);
                //dal.RollbackTransaction();
                return false;
            }  
        }

        #endregion


        

    }
}

