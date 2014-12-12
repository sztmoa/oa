using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SMT.FBAnalysis.BLL
{
    public class FBAEnums
    {
        public enum BLLPrefixNames
        {
            Execution
        }
        /// <summary>
        /// 窗体类型
        /// </summary>
        public enum FormTypes
        {
            /// <summary>
            /// 新增
            /// </summary>
            New,
            /// <summary>
            /// 编辑
            /// </summary>
            Edit,
            /// <summary>
            /// 浏览
            /// </summary>
            Browse,
            /// <summary>
            /// 审核
            /// </summary>
            Audit,
            /// <summary>
            /// 重新提交
            /// </summary>
            Resubmit
        }
        public enum CheckStates
        {
            /// <summary>
            /// 未提交
            /// </summary>
            UnSubmit,
            /// <summary>
            /// 审核中
            /// </summary>
            Approving,
            /// <summary>
            /// 审核通过
            /// </summary>
            Approved,
            /// <summary>
            /// 审核未通过
            /// </summary>
            UnApproved,
            /// <summary>
            /// 待审核
            /// </summary>
            WaittingApproval,
            /// <summary>
            /// 所有
            /// </summary>
            All
        }
        public enum EditStates
        {
            /// <summary>
            /// 未生效
            /// </summary>
            UnActived,
            /// <summary>
            /// 已生效
            /// </summary>
            Actived,
            /// <summary>
            /// 撤消中
            /// </summary>
            PendingCanceled,
            /// <summary>
            /// 已撤消
            /// </summary>
            Canceled,
            /// <summary>
            /// 删除状态
            /// </summary>
            Deleted
        }
    }
}
