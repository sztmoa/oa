
// 内容摘要: ToolManager管理提示控件
using System.Collections.Generic;

namespace SMT.SAAS.Platform.Controls.ToolTips
{
    public class ToolManager
    {
        private static List<ToolTips> _ListTips;

        /// <summary>
        /// 提示控件集合
        /// </summary>
        /// <returns></returns>
        public static List<ToolTips> ListTips()
        {
            if (_ListTips == null)
            {
                _ListTips = new List<ToolTips>();
            }
            return _ListTips;
        }


        public static bool IsVisibility 
        {
            set
            {
                SetVisibility(value);                 
            } 
        
        }

        /// <summary>
        /// 是否显示已有的提示控件
        /// </summary>
        /// <param name="isshow"></param>
        private static void SetVisibility(bool isshow)
        {
            if (_ListTips != null)
            {
                foreach (var item in _ListTips)
                {
                    item.IsVisibility = isshow;
                }
            }
        }
    }
}
