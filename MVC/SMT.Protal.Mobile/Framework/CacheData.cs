using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using SMT.Portal.Common.SmtForm.BLL;

namespace SMT.Portal.Common.SmtForm.Framework
{
    public static class CacheData
    {     
        /// <summary>
        /// 返回反序列号的文件内容
        /// </summary>
        private static ObjConfig _cacheConfig;
        public static ObjConfig CacheConfig
        {
            get
            {
                if (_cacheConfig == null)
                {
                    _cacheConfig  = ConfigManager.Get(HttpContext.Current.Server.MapPath("Config.xml"));
                }
                return _cacheConfig;
            }
        }

   
    }
}