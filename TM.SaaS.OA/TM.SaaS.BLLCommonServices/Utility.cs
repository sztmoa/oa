using System;
using System.Collections.Generic;
using System.Text;

namespace TM.SaaS.BLLCommonServices
{
    public class Utility
    {
        /// <summary>
        /// 添加"我的单据"
        /// </summary>
        /// <param name="entity">源实体</param>
        public static void SubmitMyRecord<TEntity>(object entity)
        {
            //SubmitMyRecord<TEntity>(entity, "0");
        }

         /// <summary>
        /// 删除"我的单据"
        /// </summary>
        /// <param name="entity">源实体</param>
        public static void RemoveMyRecord<TEntity>(object entity)
        {

        }
    }
}
