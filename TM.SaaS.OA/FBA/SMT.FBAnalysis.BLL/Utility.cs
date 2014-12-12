using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Objects.DataClasses;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Reflection;
using System.Data;
using System.Xml;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Mapping;
using System.Configuration;
using System.Runtime.InteropServices;
using EngineWS = SMT.SaaS.BLLCommonServices.EngineConfigWS;
using System.Linq.Dynamic;
using SMT.FBAnalysis.DAL;
using SMT.Foundation.Log;

namespace SMT.FBAnalysis.BLL
{
    public class Utility
    {     
        public static string SerializeObject(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer =
                        new XmlSerializer(obj.GetType());
                serializer.Serialize(ms, obj);
                ms.Position = 0;

                using (StreamReader reader = new StreamReader(ms))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 克隆对像
        /// </summary>
        /// <param name="source">要被克隆的对像</param>
        /// <returns>克隆的新对像</returns>
        public static T CloneObject<T>(object source) where T : class
        {
            if (source == null)
            {
                return null;
            }

            string str = SerializeObject(source);
            T tmpObj = DeserializeObject<T>(str);
            return tmpObj;
        }
        public static T DeserializeObject<T>(string objString)
        {
            //List<Person> persons = DeserializeObject<List<Person>>( jsonString )
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(objString)))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                return (T)serializer.Deserialize(ms);
            }
        }
        public static object DeserializeObject(string objString, Type type)
        {
            //List<Person> persons = DeserializeObject<List<Person>>( jsonString )
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(objString)))
            {
                XmlSerializer serializer = new XmlSerializer(type);

                return serializer.Deserialize(ms);
            }
        }

        public static object GetLookupData(FBAEnums.BLLPrefixNames entity, string modelCode, string userID, int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount)
        {
            object ents = null;
            Type type = Type.GetType("SMT.FBAnalysis.BLL." + entity.ToString() + "BLL");

            if (type != null)
            {
                ILookupEntity bll = (ILookupEntity)Activator.CreateInstance(type);
                ents = bll.GetLookupData(modelCode, userID, pageIndex, pageSize, sort, filterString, paras, ref pageCount);
            }
            return ents;
        }

        /// <summary>
        /// 对IQueryable对像分页处理
        /// </summary>
        /// <typeparam name="T">需要分页对像类型</typeparam>
        /// <param name="ents">需要分页的IQueryable对像,必需是有排过序的</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录条数</param>
        /// <param name="pageCount">页数</param>
        /// <returns>IQueryable对像</returns>
        public static IQueryable<T> Pager<T>(IQueryable<T> ents, int pageIndex, int pageSize, ref int pageCount)
        {
            int count = ents.Count();
            pageCount = count / pageSize;
            int tmp = count % pageSize;

            pageCount = pageCount + (tmp > 0 ? 1 : 0);
            if (pageIndex > pageCount)
                pageIndex = 2;

            ents = ents.ToList().AsQueryable().Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return ents;
        }

        /// <summary>
        /// 对List对像分页处理
        /// </summary>
        /// <typeparam name="T">需要分页对像类型</typeparam>
        /// <param name="ents">需要分页的List对像,必需是有排过序的</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页记录条数</param>
        /// <param name="pageCount">页数</param>
        /// <returns>List对像</returns>
        public static List<T> PagerList<T>(List<T> ents, int pageIndex, int pageSize, ref int pageCount)
        {
            if (ents == null)
            {
                return ents;
            }

            int count = ents.Count();
            pageCount = count / pageSize;
            int tmp = count % pageSize;

            pageCount = pageCount + (tmp > 0 ? 1 : 0);
            if (pageIndex > pageCount)
                pageIndex = 2;

            ents = ents.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return ents;
        }

        public static void CloneEntity<T>(T sourceObj, T targetObj) where T : class
        {
            Type a = sourceObj.GetType();
            PropertyInfo[] infos = a.GetProperties();
            foreach (PropertyInfo prop in infos)
            {
                //System.Data.Objects.DataClasses.
                if (prop.PropertyType.BaseType == typeof(EntityReference)
                    || prop.PropertyType.BaseType == typeof(RelatedEnd)
                    || prop.PropertyType == typeof(System.Data.EntityState)
                    || prop.PropertyType == typeof(System.Data.EntityKey)
                    || prop.PropertyType.BaseType == typeof(System.Data.Objects.DataClasses.EntityObject))
                    continue;
                if (sourceObj is EntityObject)
                {
                    EntityObject ent = sourceObj as EntityObject;

                    if (ent != null && ent.EntityKey != null && ent.EntityKey.EntityKeyValues != null && ent.EntityKey.EntityKeyValues.Count() > 0)
                    {
                        bool isKeyField = false;
                        foreach (var key in ent.EntityKey.EntityKeyValues)
                        {
                            if (key.Key == prop.Name)
                            {
                                isKeyField = true;
                                break;
                            }
                        }
                        if (isKeyField)
                            continue;
                    }
                }
                //prop.Name
                object value = prop.GetValue(sourceObj, null);
                try
                {
                    prop.SetValue(targetObj, value, null);
                }
                catch (Exception ex)
                {
                    string e = ex.Message;
                }
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="strMsg"></param>
        public static void SaveLog(string strMsg)
        {
            if (!string.IsNullOrWhiteSpace(strMsg))
            {
                Tracer.Debug(strMsg);
            }
        }

        public static string GetResourceStr(string message)
        {
            string rslt = SMT.SaaS.Globalization.Localization.ResourceMgr.GetString(message, SMT.SaaS.Globalization.Localization.UiCulture);
            return string.IsNullOrEmpty(rslt) ? message : rslt;
        }
        public static string GetResourceStr(string message, string parameter)
        {
            string rslt = SMT.SaaS.Globalization.Localization.GetString(message, parameter);

            return string.IsNullOrEmpty(rslt) ? message : rslt;
        }

        public static void RefreshEntity(EntityObject entity)
        {
            var rs = (entity as IEntityWithRelationships).RelationshipManager.GetAllRelatedEnds();

            foreach (IRelatedEnd re in rs)
            {
                List<EntityObject> list = new List<EntityObject>();
                foreach (var item in re)
                {
                    list.Add(item as EntityObject);
                }
                list.ForEach(p =>
                {
                    if (re.GetType().BaseType == typeof(EntityReference))
                    {
                        EntityKey eKey = p.EntityKey;
                        if (eKey != null)
                        {
                            (re as EntityReference).EntityKey = eKey;
                            re.Remove(p);
                        }
                    }

                });
            }
        }

        /// <summary>
        /// 将数据转为指定html格式，并以流的形式返回，以便导出成指定格式的文件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="dt"></param>
        public static byte[] OutFileStream(string title, DataTable dt)
        {
            string strCustomerBodyHeader = string.Empty;

            return OutFileStream(title, dt, strCustomerBodyHeader);
        }

        public static byte[] OutFileStream(string strCustomerBody)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetHeader().ToString());
            sb.Append(strCustomerBody);
            byte[] by = Encoding.UTF8.GetBytes(sb.ToString());

            return by;
        }

        /// <summary>
        /// 将数据转为指定html格式，并以流的形式返回，以便导出成指定格式的文件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="dt"></param>
        public static byte[] OutFileStream(string title, DataTable dt, string strCustomerBodyHeader)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetHeader().ToString());
            sb.Append(GetBody(title, dt, strCustomerBodyHeader).ToString());
            byte[] by = Encoding.UTF8.GetBytes(sb.ToString());

            return by;
        }

        /// <summary>
        /// 获取EXCEL头文件
        /// </summary>
        /// <returns></returns>
        public static StringBuilder GetHeader()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:x=\"urn:schemas-microsoft-com:office:excel\" xmlns=\"http://www.w3.org/TR/REC-html40\">\n\r");
            sb.Append("<head>\n\r");
            sb.Append("<meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">\n\r");
            sb.Append("<meta name=ProgId content=Excel.Sheet>\n\r");
            sb.Append("<meta name=Generator content=\"Microsoft Excel 11\">\n\r");

            sb.Append(" <xml>\n\r");
            sb.Append("<x:ExcelWorkbook>\n\r");
            sb.Append("<x:ExcelWorksheets>\n\r");
            sb.Append("<x:ExcelWorksheet>\n\r");
            sb.Append("<x:Name>Sheet1</x:Name>\n\r");
            sb.Append("<x:WorksheetOptions>\n\r");
            sb.Append("<x:Print>\n\r");
            sb.Append("</x:Print>\n\r");
            sb.Append("</x:WorksheetOptions>\n\r");
            sb.Append("</x:ExcelWorksheet>\n\r");
            sb.Append("</x:ExcelWorksheets>\n\r");
            sb.Append("</x:ExcelWorkbook>\n\r");
            sb.Append(" </xml>\n\r");
            sb.Append("<style>\n\r");
            sb.Append("td {font-size:12px;}\n\r");
            sb.Append(".title {font-size:14px; font-weight:bold;height:30px;}\n\r");
            sb.Append(".thead{font-weight:bold;}\n\r");
            sb.Append(".style0{mso-number-format:General;text-align:general;vertical-align:middle;white-space:normal;" +
                              "mso-rotate:0;mso-background-source:auto;mso-pattern:auto;color:windowtext;" +
                              "font-weight:400;font-style:normal;text-decoration:none;font-family:宋体;" +
                              "mso-generic-font-family:auto;mso-font-charset:134;border:none;" +
                              "mso-protection:locked visible;mso-style-name:常规;mso-style-id:0;" +
                              "font-size:9.0pt;border:.5pt solid black;}\n\r");
            sb.Append(".x1281{mso-style-parent:style0;mso-number-format:\"\\@\";border:.5pt solid black;font-weight:bold;}\n\r");
            sb.Append(".x1282{mso-style-parent:style0;mso-number-format:\"\\@\";border:.5pt solid black;}\n\r");
            sb.Append(".x0{mso-style-parent:style0;mso-number-format:\"0_ \";text-align:right;border:.5pt solid black;}\n\r");
            sb.Append(".x1{mso-style-parent:style0;mso-number-format:\"0\\.0_ \";text-align:right;border:.5pt solid black;}\n\r");
            sb.Append(".x2{mso-style-parent:style0;mso-number-format:\"0\\.00_ \";text-align:right;border:.5pt solid black;}\n\r");
            sb.Append(".x3{mso-style-parent:style0;mso-number-format:\"0\\.000_ \";text-align:right;border:.5pt solid black;}\n\r");
            sb.Append(".x4{mso-style-parent:style0;mso-number-format:\"0\\.0000_ \";text-align:right;border:.5pt solid black;}\n\r");
            sb.Append(".x5{mso-style-parent:style0;mso-number-format:\"0\\.00000_ \";text-align:right;border:.5pt solid black;}\n\r");
            sb.Append("</style>\n\r");
            sb.Append("</head>\n\r");
            return sb;
        }

        /// <summary>
        /// 获取EXCEL内容
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static StringBuilder GetBody(string title, DataTable dt, string strCustomerBodyHeader)
        {
            StringBuilder s = new StringBuilder();
            s.Append("<body>\n\r");
            s.Append("<table ID=\"Table0\" BORDER=0 CELLSPACING=1 CELLPADDING=3 width=100% align=center>\n\r");
            s.Append("<tr>");
            int cols = dt.Columns.Count;
            s.Append("<td colspan=\"" + cols + "\" align=center class=\"title\">" + title + "</td>");
            s.Append("</tr>\n\r");
            s.Append("</table>\n\r");
            s.Append("<table border=0 cellspacing=0 CELLPADDING=3 width=100% align=center>");
            if (string.IsNullOrWhiteSpace(strCustomerBodyHeader))
            {
                s.Append("<tr>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    s.Append("<td class='x1281'>" + dt.Columns[i].Caption.ToString().Replace("*" + i, "") + "</td>");
                }
                s.Append("</tr>");
            }
            else
            {
                s.Append(strCustomerBodyHeader);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    s.Append("<td class='" + GetCss(dt.Rows[i][j].ToString(), dt.Columns[j].DataType.Name) + "'>" + dt.Rows[i][j].ToString() + "</td>");
                }
                s.Append("</tr>");
            }
            s.Append("</table>");
            s.Append("</body></html>");
            return s;
        }

        /// <summary>
        /// 不带头部名称的内容
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strCustomerBodyHeader"></param>
        /// <returns></returns>
        public static StringBuilder GetBodyWithNoTitle(DataTable dt, string strCustomerBodyHeader)
        {
            StringBuilder s = new StringBuilder();            
            s.Append("<table border=0 cellspacing=0 CELLPADDING=3 width=100% align=center>");
            if (string.IsNullOrWhiteSpace(strCustomerBodyHeader))
            {
                s.Append("<tr>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    s.Append("<td class='x1281'>" + dt.Columns[i].Caption.ToString().Replace("*" + i, "") + "</td>");
                }
                s.Append("</tr>");
            }
            else
            {
                s.Append(strCustomerBodyHeader);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                s.Append("<tr>");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    s.Append("<td class='" + GetCss(dt.Rows[i][j].ToString(), dt.Columns[j].DataType.Name) + "'>" + dt.Rows[i][j].ToString() + "</td>");
                }
                s.Append("</tr>");
            }
            s.Append("</table>");
            return s;
        }

        /// <summary>
        /// 获取单元格样式
        /// </summary>
        /// <param name="str"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static string GetCss(string str, string typename)
        {
            string tempStr = "x1282";
            if (!str.Equals("") && ("Int32,Decimal,Double".IndexOf(typename) >= 0))
            {

                int m = 0;
                if (str.LastIndexOf(".") >= 0)
                    m = str.Length - str.LastIndexOf('.') - 1;
                if (m >= 0) tempStr = "x" + m;
            }
            return tempStr;
        }

        public static string ObjListToXml<T>(T objectdata, string SystemCode, string currentUserName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<System>");
            Type objtype = objectdata.GetType();
            sb.AppendLine("<Name>" + SystemCode + "</Name>");
            sb.AppendLine("<Object Name=\"Approval\" Description=\"\">");
            PropertyInfo[] propinfos = objtype.GetProperties();
            foreach (PropertyInfo propinfo in propinfos)
            {
                if (propinfo.Name.ToUpper() != "CHECKSTATE")
                {
                    sb.AppendLine("<Attribute Name=\"" + propinfo.Name + "\" Description=\"" + "" + "\" DataType=\"" + "" + "\" DataValue=\"" + propinfo.GetValue(objectdata, null) + "\"/>");
                }
            }
            sb.AppendLine("<Attribute Name=\"" + "CURRENTEMPLOYEENAME" + "\" Description=\"" + "提交者" + "\" DataType=\"" + "" + "\" DataValue=\"" + currentUserName + "\"/>");
            sb.AppendLine("</Object>");
            sb.AppendLine("</System>");
            return sb.ToString();

        }
        /// <summary>
        /// 删除指定地址下文件
        /// </summary>
        /// <param name="strPhysicalPath"></param>
        public static void DeleteUploadFile(string strPhysicalPath)
        {
            if (Directory.Exists(strPhysicalPath)) //如果存在这个文件夹删除之   
            {
                foreach (string d in Directory.GetFileSystemEntries(strPhysicalPath))
                {
                    if (File.Exists(d))
                    {
                        File.Delete(d); //直接删除其中的文件                          
                        break;
                    }
                }
            }
        }
    }
}
