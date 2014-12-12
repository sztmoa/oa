using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace SMT.FBAnalysis.Service
{
    /// <summary>
    /// 日常管理服务，主要包括：个人费用申请、个人费用报销、个人费用还款
    /// 主要由朱磊、勒中玉、刘建兴参与开发
    /// 
    /// </summary>
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class DailyManagementServices
    {
       

        #region 个人费用申请
        //调用DailyManagement下的BorrowApply.cs
        #endregion

        #region 个人费用报销 
        //调用DailyManagement下的ChargeApply.cs
        #endregion

        #region 个人还款 
        //调用DailyManagement下的RepayApply.cs
        #endregion

        // 在此处添加更多操作并使用 [OperationContract] 标记它们
    }
}
