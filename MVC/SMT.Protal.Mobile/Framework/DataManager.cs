using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Serialization;

namespace SMT.Portal.Common.SmtForm.Framework
{
    public class DataManager<T>
    {
         private DataManager() { }


        /// <summary>
        /// 反序列号对象
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
         public static T Get(string path)
        {
            T _obj = default(T);
            if (_obj == null)
            {
                FileStream fs = null;
                try
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    _obj = (T)xs.Deserialize(fs);
                    fs.Close();
                    return _obj;
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
                return _obj;
            }
        }
 
        /// <summary>
        /// 序列化对象到指定的文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="Obj">要序列的对象</param>
        public static void Set(string path, T Obj)
        {
            if (Obj == null)
                throw new Exception("Parameter ObjConfig is null!");
            
            FileStream fs = null;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                xs.Serialize(fs, Obj);
                
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