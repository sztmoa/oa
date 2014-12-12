using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    /// <summary>
    /// 帐务查询列表。
    /// </summary>
    public class V_AcountsList
    {
        /// <summary>
        /// 单据编号。
        /// </summary>
        //private string id;
        ///// <summary>
        ///// 借款1。
        ///// </summary>
        //private double borrow1;
        ///// <summary>
        ///// 普通1。
        ///// </summary>
        //private double general1;
        ///// <summary>
        ///// 备用金1。
        ///// </summary>
        //private double tillmoney1;
        ///// <summary>
        ///// 专项1。
        ///// </summary>
        //private double special1;
        ///// <summary>
        ///// 还款。
        ///// </summary>
        //private double repayment;
        ///// <summary>
        ///// 普通2。
        ///// </summary>
        //private double general2;
        ///// <summary>
        ///// 备用金2。
        ///// </summary>
        //private double tillmoney2;
        ///// <summary>
        ///// 专项2。
        ///// </summary>
        //private double special2;

        public List<V_AcountsList> GetTestData()
        {
            List<V_AcountsList> list = new List<V_AcountsList>();
            V_AcountsList[] acouts = new V_AcountsList[2];
            acouts[0] = new V_AcountsList();
            acouts[0].ID = "";
            acouts[0].Borrow1 = 100.00;
            acouts[0].General1 = 110.00;
            acouts[0].Tillmoney1 = 120.00;
            acouts[0].Special1 = 130.00;
            acouts[0].Repayment = 140.00;
            acouts[0].General2 = 110.00;
            acouts[0].Tillmoney2 = 120.00;
            acouts[0].Special2 = 130.00;

            acouts[1] = new V_AcountsList();
            acouts[1].ID = "";
            acouts[1].Borrow1 = 100.00;
            acouts[1].General1 = 110.00;
            acouts[1].Tillmoney1 = 120.00;
            acouts[1].Special1 = 130.00;
            acouts[1].Repayment = 140.00;
            acouts[1].General2 = 110.00;
            acouts[1].Tillmoney2 = 120.00;
            acouts[1].Special2 = 130.00;

            list.Add(acouts[0]);
            list.Add(acouts[1]);

            return list;
        }

        /// <summary>
        /// 单据编号。
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 借款1。
        /// </summary>
        public double Borrow1 { get; set; }
        /// <summary>
        /// 普通1。
        /// </summary>
        public double General1 { get; set; }
        /// <summary>
        /// 备用金1。
        /// </summary>
        public double Tillmoney1 { get; set; }
        /// <summary>
        /// 专项1。
        /// </summary>
        public double Special1 { get; set; }
        /// <summary>
        /// 还款。
        /// </summary>
        public double Repayment { get; set; }
        /// <summary>
        /// 普通2。
        /// </summary>
        public double General2 { get; set; }
        /// <summary>
        /// 备用金2。
        /// </summary>
        public double Tillmoney2 { get; set; }
        /// <summary>
        /// 专项2。
        /// </summary>
        public double Special2 { get; set; }
    }
}
