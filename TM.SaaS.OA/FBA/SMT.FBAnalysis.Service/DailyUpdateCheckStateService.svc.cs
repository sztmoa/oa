using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SMT_FB_EFModel;
using SMT.FBAnalysis.BLL;
using System.ServiceModel.Activation;
using SMT.Foundation.Log;


namespace SMT.FBAnalysis.Service
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“DailyUpdateCheckStateService”。
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DailyUpdateCheckStateService 
    {
        public void DoWork()
        {
        }
        /// <summary>
        /// 修改实体审核状态
        /// </summary>
        /// <param name="strEntityName">实体名</param>
        /// <param name="EntityKeyName">主键名</param>
        /// <param name="EntityKeyValue">主键值</param>
        /// <param name="CheckState">审核状态</param>
        [OperationContract]
        public int UpdateCheckState(string strEntityName, string EntityKeyName, string EntityKeyValue, string CheckState)
        {
            // 在此处添加操作实现
            //Type  bb = strEntityName
            CommBll<T_FB_BORROWAPPLYMASTER> Combll = new CommBll<T_FB_BORROWAPPLYMASTER>();
            string msg = "strEntityName：{0}，EntityKeyName：{1}，EntityKeyValue：{2}，CheckState：{3}";
            Tracer.Debug(string.Format(msg, strEntityName, EntityKeyName, EntityKeyValue, CheckState));
            return Combll.UpdateCheckState(strEntityName, EntityKeyName, EntityKeyValue, CheckState);

        }
    }
}
