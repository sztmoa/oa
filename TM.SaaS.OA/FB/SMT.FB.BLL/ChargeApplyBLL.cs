using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SMT_FB_EFModel;
using System.Data.Objects.DataClasses;


namespace SMT.FB.BLL
{
    public class ChargeApplyBLL 
    {
        public static void Test(EntityObject obj)
        {
            T_FB_TRAVELEXPAPPLYMASTER bb = obj as T_FB_TRAVELEXPAPPLYMASTER;
            BaseBll<T_FB_TRAVELEXPAPPLYMASTER> b = new BaseBll<T_FB_TRAVELEXPAPPLYMASTER>();
            T_FB_TRAVELEXPAPPLYMASTER t = b.GetTable().First();
            T_FB_TRAVELEXPAPPLYDETAIL t1 = new T_FB_TRAVELEXPAPPLYDETAIL();

            T_FB_TRAVELEXPAPPLYDETAIL t2 = bb.T_FB_TRAVELEXPAPPLYDETAIL.ToList()[0];
            t.T_FB_TRAVELEXPAPPLYDETAIL.Add(t1);
            t.T_FB_TRAVELEXPAPPLYDETAIL.Add(t2);
            b.dal.GetDataContext().SaveChanges();
        }
        //public ChargeApplyOrder GetChargeApplyOrder(string orderID)
        //{


        //    FBCommonBLL<T_FB_CHARGEAPPLYMASTER> b1 = new FBCommonBLL<T_FB_CHARGEAPPLYMASTER>();
            
        //    QueryExpression qe = new QueryExpression();
        //    qe.PropertyName = "CHARGEAPPLYMASTERID";
        //    qe.PropertyValue = orderID;
        //    qe.Operation = QueryExpression.Operations.Equal;
        //    qe.QueryType = typeof(T_FB_CHARGEAPPLYMASTER).Name;
            
        //    T_FB_CHARGEAPPLYMASTER t = b1.QueryTable(qe)[0];

        //    FBCommonBLL<T_FB_CHARGEAPPLYDETAIL> b2 = new FBCommonBLL<T_FB_CHARGEAPPLYDETAIL>();

        //    BaseBll<T_FB_CHARGEAPPLYDETAIL> bb = new BaseBll<T_FB_CHARGEAPPLYDETAIL>();
        //    var items = from item in bb.GetTable()
        //                where item.T_FB_CHARGEAPPLYMASTER.CHARGEAPPLYMASTERID == orderID
        //                select item;

        //    List<T_FB_CHARGEAPPLYDETAIL> t2 = items.ToList();
        //    ChargeApplyOrder c = new ChargeApplyOrder();
        //    c.Entity = t;
        //    c.ChargeApplyList = t2;
        //    return c;
        //}
    }
}
