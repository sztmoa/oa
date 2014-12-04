using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMT.Portal.Common.SmtForm.BLL;
using System.IO;
using System.Xml.Serialization;

namespace SMT.Portal.Common.SmtForm.Framework
{
     public class ConfigManager
    {
        private static ObjConfig _objConfig = null;
        private ConfigManager() { }
 
         /// <summary>
         /// 根据path反序列xml
         /// </summary>
         /// <param name="path">文件路径</param>
        /// <returns>返回ObjConfig的对象</returns>
        public static ObjConfig Get(string path)
        {
            if (_objConfig == null)
            {
                FileStream fs = null;
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ObjConfig));
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    _objConfig = (ObjConfig)xs.Deserialize(fs);
                    fs.Close();
                    return _objConfig;
                }
                catch(Exception ex)
                {
                    if (fs != null)
                        fs.Close();
                    throw ex;
                }
 
            }
            else
            {
                return _objConfig;
            }
        }

        /// <summary>
        /// 根据path和ObjConfig序列化
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="ObjConfig">操作Config.xml对象</param>
        public static void Set(string path, ObjConfig ObjConfig)
        {
            if (ObjConfig == null)
                throw new Exception("Parameter ObjConfig is null!");
            
            FileStream fs = null;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(ObjConfig));
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                xs.Serialize(fs, ObjConfig);
                _objConfig = null;
                fs.Close();
            }
            catch(Exception ex)
            {
                if (fs != null)
                    fs.Close();
                throw ex;
            }
        }
    }

}