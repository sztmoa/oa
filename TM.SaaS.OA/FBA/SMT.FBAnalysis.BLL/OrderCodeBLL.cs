
/*
 * 文件名：OrderCodeBLL.cs
 * 作  用：T_FB_ORDERCODE 业务逻辑类
 * 创建人：吴鹏
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

using TM_SaaS_OA_EFModel;
using SMT.FBAnalysis.DAL;

namespace SMT.FBAnalysis.BLL
{
    public class OrderCodeBLL
    {
        public OrderCodeBLL()
        { }

        #region 获取数据

        /// <summary>
        /// 获取T_FB_ORDERCODE信息
        /// </summary>
        /// <param name="strOrderCodeId">主键索引</param>
        /// <returns></returns>
        public T_FB_ORDERCODE GetOrderCodeByID(string strOrderCodeId)
        {
            if (string.IsNullOrEmpty(strOrderCodeId))
            {
                return null;
            }

            OrderCodeDAL dalOrderCode = new OrderCodeDAL();
            StringBuilder strFilter = new StringBuilder();
            List<string> objArgs = new List<string>();

            if (!string.IsNullOrEmpty(strOrderCodeId))
            {
                strFilter.Append(" TABLENAME == @0");
                objArgs.Add(strOrderCodeId);
            }

            T_FB_ORDERCODE entRd = dalOrderCode.GetOrderCodeRdByMultSearch(strFilter.ToString(), objArgs.ToArray());
            return entRd;
        }

        /// <summary>
        /// 根据条件，获取T_FB_ORDERCODE信息
        /// </summary>
        /// <param name="strVacName"></param>
        /// <param name="strVacYear"></param>
        /// <param name="strCountyType"></param>
        /// <param name="strSortKey"></param>
        /// <returns></returns>
        public static IQueryable<T_FB_ORDERCODE> GetAllOrderCodeRdListByMultSearch(string strFilter, List<object> objArgs, string strSortKey)
        {
            OrderCodeDAL dalOrderCode = new OrderCodeDAL();
            string strOrderBy = string.Empty;

            if (!string.IsNullOrEmpty(strSortKey))
            {
                strOrderBy = strSortKey;
            }
            else
            {
                strOrderBy = " TABLENAME ";
            }

            var q = dalOrderCode.GetOrderCodeRdListByMultSearch(strOrderBy, strFilter, objArgs.ToArray());
            return q;
        }

        /// <summary>
        /// 根据条件，获取T_FB_ORDERCODE信息,并进行分页
        /// </summary>
        /// <param name="strFilter">查询条件</param>
        /// <param name="objArgs">查询参数</param>
        /// <param name="strSortKey">排序字段</param>
        /// <param name="pageIndex">当前页索引</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="pageCount">总页数</param>
        /// <returns>T_FB_ORDERCODE信息</returns>
        public IQueryable<T_FB_ORDERCODE> GetOrderCodeRdListByMultSearch(string strFilter, List<object> objArgs,
            string strSortKey, int pageIndex, int pageSize, ref int pageCount)
        {
            var q = GetAllOrderCodeRdListByMultSearch(strFilter, objArgs, strSortKey);

            return Utility.Pager<T_FB_ORDERCODE>(q, pageIndex, pageSize, ref pageCount);
        }

        #endregion

        #region 操作数据
        /// <summary>
        /// 获取单据编号
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //public static string GetAutoOrderCode(EntityObject entity)
        //{
        //    string strRes = string.Empty;
        //    string tablename = GetTableName(entity);

        //    if (string.IsNullOrWhiteSpace(tablename))
        //    {
        //        return strRes;
        //    }

        //    OrderCodeDAL dalOrderCode = new OrderCodeDAL();

        //    var ents = from n in dalOrderCode.GetObjects<T_FB_ORDERCODE>()
        //               where n.TABLENAME == tablename
        //               select n;

        //    T_FB_ORDERCODE orderCode = ents.FirstOrDefault();

        //    if (orderCode == null)
        //    {
        //        return strRes;
        //    }

        //    DateTime CurrentDate = orderCode.CURRENTDATE.Value;
        //    if (CurrentDate.Date != System.DateTime.Now.Date)
        //    {
        //        orderCode.CURRENTDATE = System.DateTime.Now.Date;
        //        orderCode.RUNNINGNUMBER = 1;

        //    }

        //    string shortName = orderCode.PRENAME;
        //    decimal curNumber = orderCode.RUNNINGNUMBER.Value;
        //    string strDate = orderCode.CURRENTDATE.Value.ToString("yyyyMMdd");
        //    string code = shortName + "_" + strDate + curNumber.ToString().PadLeft(6, '0');

        //    orderCode.RUNNINGNUMBER = curNumber + 1;
        //    dalOrderCode.Update(orderCode);

        //    return code;
        //}



        /// <summary>
        /// 获取单据编号
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public string GetAutoOrderCode(EntityObject entity)
        {
            string strRes = string.Empty;
            string tablename = GetTableName(entity);

            if (string.IsNullOrWhiteSpace(tablename))
            {
                return strRes;
            }

            OrderCodeDAL dalOrderCode = new OrderCodeDAL();

            var ents = from n in dalOrderCode.GetObjects<T_FB_ORDERCODE>()
                       where n.TABLENAME == tablename
                       select n;

            T_FB_ORDERCODE orderCode = ents.FirstOrDefault();

            if (orderCode == null)
            {
                return strRes;
            }
            string shortName = orderCode.PRENAME;
            DateTime CurrentDate = orderCode.CURRENTDATE.Value;
            if (CurrentDate.Date != System.DateTime.Now.Date)
            {
                orderCode.CURRENTDATE = System.DateTime.Now.Date;
                orderCode.RUNNINGNUMBER = 1;

                lock (this)
                {
                    string strclear = @"declare n number(10); tsql   varchar2(1000); begin select " 
                                        + tablename + "_SEQ.nextval   into   n   from   dual; "
                                        + " n:=-(n);"
                                        + " tsql:= 'alter   sequence " + tablename + "_SEQ   increment   by '||   n;"
                                        + " execute   immediate   tsql; "
                                        + "  select " + tablename + "_SEQ.nextval   into   n   from   dual; "
                                        + "  tsql:= 'alter   sequence " + tablename + "_SEQ   increment   by   1 '; "
                                        + "  execute   immediate   tsql; "
                                        + "  end; ";
                    dalOrderCode.ExecuteCustomerSql(strclear);
                    //string strclear = " DROP SEQUENCE T_FB_CHARGEAPPLYMASTER_SEQ ";
                    //dalOrderCode.ExecuteCustomerSql(strclear);
                    //strclear = " create sequence T_FB_CHARGEAPPLYMASTER_SEQ minvalue 0 maxvalue 999999999999999999999999999 start with 1 increment by 1 cache 20 ";
                    //dalOrderCode.ExecuteCustomerSql(strclear);

                }

            }


            decimal curNumber = 0;
            string strNextSqense = @" select " + tablename + "_SEQ.nextval from dual";
            curNumber = decimal.Parse(dalOrderCode.ExecuteCustomerSql(strNextSqense).ToString());



            string strDate = orderCode.CURRENTDATE.Value.ToString("yyyyMMdd");
            string code = shortName + "_" + strDate + curNumber.ToString().PadLeft(6, '0');

            orderCode.RUNNINGNUMBER = Convert.ToInt32(curNumber + 1);
            dalOrderCode.Update(orderCode);

            return code;
        }

        /// <summary>
        /// 获取传入实体的表名/实体类名
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static string GetTableName(EntityObject entity)
        {
            string tableName = entity.GetType().Name;
            return tableName;
        }
        #endregion
    }
}

