using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SMT.FB.UI.Common
{
    public class ExportToCSV
    {
        /// <summary>        
        /// 导出DataGrid数据到Excel        
        /// </summary>        
        /// <param name="withHeaders">是否需要表头</param>        
        /// <param name="grid">DataGrid</param>        
        /// <returns>Excel内容字符串</returns>        
        public static string ExportDataGrid(bool withHeaders, DataGrid grid)
        {
            System.Reflection.PropertyInfo propInfo;
            System.Windows.Data.Binding binding;
            var strBuilder = new System.Text.StringBuilder();
            DictionaryConverter dicConverter = new DictionaryConverter();
            var source = (grid.ItemsSource as System.Collections.IList);
            if (source == null) return "";
            var headers = new List<string>();
            grid.Columns.ForEach(col =>
            {
                if (col is DataGridBoundColumn)
                {
                    string strHeader = ConvertDic(col.Header.ToString());
                    headers.Add(FormatCsvField(strHeader));
                }
            });
            strBuilder.Append(String.Join(",", headers.ToArray())).Append("\r\n");
            foreach (Object data in source)
            {
                var csvRow = new List<string>();
                foreach (DataGridColumn col in grid.Columns)
                {
                    try
                    {
                        if (col is DataGridBoundColumn)
                        {
                            binding = (col as DataGridBoundColumn).Binding;
                            string colPath = binding.Path.Path;
                            string[] arr = colPath.Split('.');
                            string dicCategory = Convert.ToString(binding.ConverterParameter);//如有绑定字典值，则为字典类别
                            propInfo = data.GetType().GetProperty(colPath);
                            object ob = data;
                            if (arr.Length > 1)
                            {
                                ob = data.GetObjValue(arr[0]);
                                propInfo = data.GetObjValue(arr[0]).GetType().GetProperty(arr[1]);
                            }
                            if (propInfo != null)
                            {
                                object obj = propInfo.GetValue(ob, null) == null ? null : propInfo.GetValue(ob, null).ToString();
                                obj = dicConverter.Convert(obj, null, dicCategory, null);
                                string value = Convert.ToString(obj);
                                csvRow.Add(FormatCsvField(value));
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                strBuilder.Append(String.Join(",", csvRow.ToArray())).Append("\r\n");
            }
            return strBuilder.ToString();
        }

        #region 导出CSV
        /// <summary>        
        /// CSV格式化        
        /// </summary>        
        /// <param name="data">数据</param>        
        /// <returns>格式化数据</returns>        
        private static string FormatCsvField(string data)
        {
            return String.Format("\"{0}\"", data.Replace("\"", "\"\"\"").Replace("\n", "").Replace("\r", ""));
        }

        /// <summary>
        /// 根据绑定字典值转换成相应名称(可加判断扩展)
        /// </summary>
        /// <param name="strDic">字典值</param>
        /// <returns>转换后名称</returns>
        private static string ConvertDic(string strDic)
        {
            string strName=string.Empty;
            try
            {
                strName = GetResourceStr(strDic);
            }
            catch (Exception)
            {
                 strName=strDic;
            }
            return strName;
        }
        public static string GetResourceStr(string message)
        {
            string rslt = SMT.SaaS.Globalization.Localization.ResourceMgr.GetString(message, SMT.SaaS.Globalization.Localization.UiCulture);
            return string.IsNullOrEmpty(rslt) ? message : rslt;
        }
         /// <summary>       
        /// /// 导出DataGrid数据到Excel        
        /// </summary>        
        /// <param name="withHeaders">是否需要表头</param>  
        /// /// <param name="grid">DataGrid</param>      
        /// /// <param name="dataBind"></param>        
        /// <returns>Excel内容字符串</returns>      
        public static string ExportDataGrid(bool withHeaders, DataGrid grid, bool dataBind)
        {
            var strBuilder = new System.Text.StringBuilder();
            var source = (grid.ItemsSource as System.Collections.IList);
            if (source == null) return "";
            var headers = new List<string>();
            grid.Columns.ForEach(col =>
            {
                if (col is DataGridTemplateColumn)
                {
                    headers.Add(col.Header != null ? FormatCsvField(col.Header.ToString()) : string.Empty);
                }
            });
            strBuilder.Append(String.Join(",", headers.ToArray())).Append("\r\n");
            foreach (Object data in source)
            {
                var csvRow = new List<string>();
                foreach (DataGridColumn col in grid.Columns)
                {
                    if (col is DataGridTemplateColumn)
                    {
                        FrameworkElement cellContent = col.GetCellContent(data);
                        TextBlock block;
                        if (cellContent.GetType() == typeof(Grid))
                        {
                            block = cellContent.FindName("TempTextblock") as TextBlock;
                        }
                        else
                        {
                            block = cellContent as TextBlock;
                        }
                        if (block != null)
                        {
                            csvRow.Add(FormatCsvField(block.Text));
                        }
                    }
                }
                strBuilder.Append(String.Join(",", csvRow.ToArray())).Append("\r\n");
            }
            return strBuilder.ToString();
        }

        /// <summary>        
        /// 导出DataGrid数据到Excel为CVS文件     
        /// 使用utf8编码 中文是乱码 改用Unicode编码 
        /// </summary>       
        /// <param name="withHeaders">是否带列头</param>
        /// <param name="grid">DataGrid</param>     
        public static void ExportDataGridSaveAs(DataGrid grid)
        {
            string data = ExportDataGrid(true, grid);
            var sfd = new SaveFileDialog
            {
                DefaultExt = "csv",
                Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*",
                FilterIndex = 1
            };
            if (sfd.ShowDialog() == true)
            {
                using (Stream stream = sfd.OpenFile())
                {
                    using (var writer = new StreamWriter(stream, System.Text.Encoding.Unicode))
                    {
                        data = data.Replace(",", "\t");
                        writer.Write(data);
                        writer.Close();
                    }
                    stream.Close();
                    MessageBox.Show("导出成功！");
                }
            }
        }


        /// <summary>        
        /// 导出DataGrid数据到Excel为CVS文件     
        /// 使用utf8编码 中文是乱码 改用Unicode编码 
        /// </summary>       
        /// <param name="withHeaders">是否带列头</param>
        /// <param name="grid">DataGrid</param>     
        public static void ExportDataGridWithDataSourceSaveAs(DataGrid grid, object DataSource)
        {
            try
            {              
                var sfd = new SaveFileDialog
                {
                    DefaultExt = "csv",
                    Filter = "CSV Files (*.csv)|*.csv|All files (*.*)|*.*",
                    FilterIndex = 1
                };
                if (sfd.ShowDialog() == true)
                {
                    using (Stream stream = sfd.OpenFile())
                    {
                        using (var writer = new StreamWriter(stream, System.Text.Encoding.Unicode))
                        {
                            string data = ExportDataGridWithDataSource(true, grid, DataSource);
                            data = data.Replace(",", "\t");
                            writer.Write(data);
                            writer.Close();
                        }
                        stream.Close();
                        MessageBox.Show("导出成功！");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>        
        /// 导出DataGrid数据到Excel        
        /// </summary>        
        /// <param name="withHeaders">是否需要表头</param>        
        /// <param name="grid">DataGrid</param>        
        /// <returns>Excel内容字符串</returns>        
        public static string ExportDataGridWithDataSource(bool withHeaders, DataGrid grid,object DataSource)
        {
            System.Reflection.PropertyInfo propInfo;
            System.Windows.Data.Binding binding;
            var strBuilder = new System.Text.StringBuilder();
            DictionaryConverter dicConverter = new DictionaryConverter();
            var source = DataSource as System.Collections.IList;
            if (source == null) return "";
            var headers = new List<string>();
            grid.Columns.ForEach(col =>
            {
                if (col is DataGridBoundColumn)
                {
                    string strHeader = ConvertDic(col.Header.ToString());
                    headers.Add(FormatCsvField(strHeader));
                }
            });
            strBuilder.Append(String.Join(",", headers.ToArray())).Append("\r\n");
            foreach (Object data in source)
            {
                var csvRow = new List<string>();
                foreach (DataGridColumn col in grid.Columns)
                {
                    try
                    {
                        if (col is DataGridBoundColumn)
                        {
                            binding = (col as DataGridBoundColumn).Binding;
                            string colPath = binding.Path.Path.Replace("Entity.","");
                            string[] arr = colPath.Split('.');
                            string dicCategory = Convert.ToString(binding.ConverterParameter);//如有绑定字典值，则为字典类别
                            propInfo = data.GetType().GetProperty(colPath);
                            object ob = data;
                            if (arr.Length > 1)
                            {
                                ob = data.GetObjValue(arr[0]);
                                propInfo = data.GetObjValue(arr[0]).GetType().GetProperty(arr[1]);
                            }
                            if (propInfo != null)
                            {
                                object obj = propInfo.GetValue(ob, null) == null ? null : propInfo.GetValue(ob, null).ToString();
                                obj = dicConverter.Convert(obj, null, dicCategory, null);
                                string value = Convert.ToString(obj);
                                if(colPath=="ACCOUNTOBJECTTYPE")
                                {
                                    if (value == "1") value = "年度预算";
                                    if (value == "2") value = "月度预算";
                                    if (value == "3") value = "个人预算";
                                }
                                csvRow.Add(FormatCsvField(value));
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                strBuilder.Append(String.Join(",", csvRow.ToArray())).Append("\r\n");
            }
            return strBuilder.ToString();
        }

        #endregion
    
    }
}
