using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMT.Portal.Common.SmtForm.Framework
{
    public class Common
    {
        /// <summary>
        /// 分解字符串获取对应的页面的名称
        /// </summary>
        /// <param name="strapp"></param>
        /// <returns></returns>
        public static string GetModelCode(string strapp)
        {
            string str = strapp;
            string modelcode = "";
            string[] asplit = str.Split('Ё');
            foreach (var item in asplit)
            {
                string name = item.Split('|')[0];
                string value = item.Split('|')[1];
                if (name == "ModelCode")
                {
                    modelcode = value;
                }
            }
            return modelcode;
        }
    }
}