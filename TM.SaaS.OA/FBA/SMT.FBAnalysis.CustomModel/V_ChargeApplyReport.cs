using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.CustomModel
{
    public class V_ChargeApplyReport
    {
        /// <summary>
        /// 单据编号
        /// </summary>
        public string CHARGEAPPLYMASTERCODE { get; set; }
        /// <summary>
        /// 帐号
        /// </summary>
        public string BANKCARDNUMBER { get; set; }
        /// <summary>
        /// 户名
        /// </summary>
        public string EMPLOYEECNAME { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string TOTALMONEY { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string BANKID { get; set; }
        /// <summary>
        /// 开户地
        /// </summary>
        public string BANKADDRESS { get; set; }
    }
}
