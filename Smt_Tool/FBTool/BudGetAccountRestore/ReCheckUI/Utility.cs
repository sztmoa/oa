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


namespace ReCheckUI
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
            if (ents == null)
            {
                return null;
            }
            int count = ents.Count();
            pageCount = count / pageSize;
            int tmp = count % pageSize;

            pageCount = pageCount + (tmp > 0 ? 1 : 0);
            if (pageIndex > pageCount)
                pageIndex = 2;

            ents = ents.ToList().AsQueryable().Skip((pageIndex - 1) * pageSize).Take(pageSize);
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
        /// 获取每周上班天数
        /// </summary>
        /// <param name="iWorkMode"></param>
        /// <param name="iWorkDays"></param>
        public static void GetWorkDays(int iWorkMode, ref List<int> iWorkDays)
        {
            // 摘要: Sunday = 0, Monday = 1, Tuesday = 2, Wednesday = 3, Thursday = 4, Friday = 5, Saturday = 6.
            switch (iWorkMode)
            {
                case 1:
                    iWorkDays.Add(1);
                    break;
                case 2:
                    iWorkDays.Add(1);
                    iWorkDays.Add(2);
                    break;
                case 3:
                    iWorkDays.Add(1);
                    iWorkDays.Add(2);
                    iWorkDays.Add(3);
                    break;
                case 4:
                    iWorkDays.Add(1);
                    iWorkDays.Add(2);
                    iWorkDays.Add(3);
                    iWorkDays.Add(4);
                    break;
                case 5:
                    iWorkDays.Add(1);
                    iWorkDays.Add(2);
                    iWorkDays.Add(3);
                    iWorkDays.Add(4);
                    iWorkDays.Add(5);
                    break;
                case 6:
                    iWorkDays.Add(1);
                    iWorkDays.Add(2);
                    iWorkDays.Add(3);
                    iWorkDays.Add(4);
                    iWorkDays.Add(5);
                    iWorkDays.Add(6);
                    break;
                case 7:
                    iWorkDays.Add(0);
                    iWorkDays.Add(1);
                    iWorkDays.Add(2);
                    iWorkDays.Add(3);
                    iWorkDays.Add(4);
                    iWorkDays.Add(5);
                    iWorkDays.Add(6);
                    break;
            }
        }

        /// <summary>
        /// 将数据转为指定html格式，并以流的形式返回，以便导出成指定格式的文件
        /// </summary>
        /// <param name="title"></param>
        /// <param name="dt"></param>
        public static byte[] OutFileStream(string title, DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetHeader().ToString());
            sb.Append(GetBody(title, dt).ToString());
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
            sb.Append("<<x:Name>Sheet1</x:Name>\n\r");
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
        public static StringBuilder GetBody(string title, DataTable dt)
        {
            StringBuilder s = new StringBuilder();
            s.Append("<body>\n\r");
            s.Append("<table ID=\"Table0\" BORDER=0 CELLSPACING=1 CELLPADDING=3 width=100% align=center>\n\r");
            s.Append("<tr>");
            int cols = dt.Columns.Count;
            if (cols > 12) cols = 12;
            s.Append("<td colspan=\"" + cols + "\" align=center class=\"title\">" + title + "</td>");
            s.Append("</tr>\n\r");
            s.Append("</table>\n\r");
            s.Append("<table border=0 cellspacing=0 CELLPADDING=3 width=100% align=center>");
            s.Append("<tr>");
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                s.Append("<td class='x1281'>" + dt.Columns[i].Caption.ToString().Replace("*" + i, "") + "</td>");
            }
            s.Append("</tr>");
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
        ///   <summary>      
        ///   返回两个日期之间的时间间隔（y：年份间隔、M：月份间隔、d：天数间隔、h：小时间隔、m：分钟间隔、s：秒钟间隔、ms：微秒间隔）      
        ///   </summary>      
        ///   <param   name="Date1">开始日期</param>      
        ///   <param   name="Date2">结束日期</param>      
        ///   <param   name="Interval">间隔标志</param>      
        ///   <returns>返回间隔标志指定的时间间隔</returns>      
        public static int DateDiff(System.DateTime Date1, System.DateTime Date2, string Interval)
        {
            double dblYearLen = 365;//年的长度，365天      
            double dblMonthLen = (365 / 12);//每个月平均的天数      
            System.TimeSpan objT;
            objT = Date2.Subtract(Date1);
            switch (Interval)
            {
                case "y"://返回日期的年份间隔      
                    return System.Convert.ToInt32(objT.Days / dblYearLen);
                case "M"://返回日期的月份间隔      
                    return System.Convert.ToInt32(objT.Days / dblMonthLen);
                case "d"://返回日期的天数间隔      
                    return objT.Days;
                case "h"://返回日期的小时间隔      
                    return objT.Hours;
                case "m"://返回日期的分钟间隔      
                    return objT.Minutes;
                case "s"://返回日期的秒钟间隔      
                    return objT.Seconds;
                case "ms"://返回时间的微秒间隔      
                    return objT.Milliseconds;
                default:
                    break;
            }
            return 0;
        }

        /// <summary>
        /// 引擎需要的XML形式的实体字符串转化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectdata"></param>
        /// <param name="SystemCode"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="strMsg"></param>
        public static void SaveLog(string strMsg)
        {
            if (!string.IsNullOrWhiteSpace(strMsg))
            {
                //Tracer.Debug(strMsg);
            }
        }
    }
}
