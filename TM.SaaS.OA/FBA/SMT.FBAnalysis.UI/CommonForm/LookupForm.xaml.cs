using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections;
using System.Reflection;
using System.Windows.Data;
using System.Collections.ObjectModel;

using SMT.FBAnalysis.ClientServices.FBAnalysisWS;
using SMT.SaaS.FrameworkUI.Helper;
using SMT.SaaS.FrameworkUI;

namespace SMT.FBAnalysis.UI
{
    public partial class LookupForm : System.Windows.Controls.Window
    {
        private FBAnalysisServiceClient client = new FBAnalysisServiceClient();

        public FBAEnumsBLLPrefixNames EntityName { get; set; }
        public Type EntityType { get; set; }
        public Dictionary<string, string> ColumnNames { get; set; }
        public string OrderBy { get; set; }
        public string filter { get; set; }
        public string modelCode { get; set; }
        ObservableCollection<object> paras = new ObservableCollection<object>();
        private SMTLoading loadbar = new SMTLoading();

        public object SelectedObj
        {
            get
            {
                try
                {
                    ObservableCollection<object> selObjs = new ObservableCollection<object>();

                    if (DtGrid.ItemsSource != null)
                    {
                        foreach (var obj in DtGrid.ItemsSource)
                        {
                            var cell = DtGrid.Columns[0].GetCellContent(obj);
                            if (cell == null)
                            {
                                continue;
                            }

                            CheckBox cbx = cell.FindName("checkbox") as CheckBox;
                            if (cbx == null)
                            {
                                continue;
                            }

                            if (cbx.IsChecked == null)
                            {
                                continue;
                            }

                            if (cbx.IsChecked.Value == true)
                            {
                                selObjs.Add(obj);
                            }
                        }
                    }

                    return selObjs;
                }
                catch (Exception ex)
                {
                    SMT.SaaS.FrameworkUI.Common.Utility.ShowCustomMessage(SMT.SaaS.FrameworkUI.MessageTypes.Error, this.TitleContent.ToString(), ex.ToString());
                    return null;
                }
            }
        }

        public event EventHandler SelectedClick;
        public LookupForm(FBAEnumsBLLPrefixNames entName, Type type, Dictionary<string, string> cols)
        {
            InitializeComponent();

            EntityName = entName;
            EntityType = type;
            ColumnNames = cols;
            client.GetLookupOjbectsCompleted += new EventHandler<GetLookupOjbectsCompletedEventArgs>(client_GetLookupOjbectsCompleted);

            this.TitleContent = SMT.FBAnalysis.UI.Common.Utility.GetResourceStr(entName.ToString().ToUpper());
            BindData();
        }
        public LookupForm(FBAEnumsBLLPrefixNames entName, Type type, Dictionary<string, string> cols, string strmodelCode, string filter, System.Collections.ObjectModel.ObservableCollection<object> paras)
        {
            InitializeComponent();

            EntityName = entName;
            EntityType = type;
            ColumnNames = cols;
            modelCode = strmodelCode;
            client.GetLookupOjbectsCompleted += new EventHandler<GetLookupOjbectsCompletedEventArgs>(client_GetLookupOjbectsCompleted);
            DtGrid.LoadingRow += new EventHandler<DataGridRowEventArgs>(DtGrid_LoadingRow);

            this.TitleContent = SMT.FBAnalysis.UI.Common.Utility.GetResourceStr(entName.ToString().ToUpper());
            this.filter = filter;
            this.paras = paras;
            BindData();
        }

        private IEnumerable dataSource;

        public IEnumerable DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        void client_GetLookupOjbectsCompleted(object sender, GetLookupOjbectsCompletedEventArgs e)
        {
            loadbar.Stop();
            if (e.Error == null)
            {
                //绑定系统类型
                if (string.IsNullOrEmpty(e.Result))
                {
                    return;
                }

                string rslt = e.Result.ToString();
                object objs = SMT.FBAnalysis.UI.Common.Utility.XmlToContractObject(rslt, EntityType);

                DtGrid.ItemsSource = (IEnumerable)objs;
                DataSource = (IEnumerable)objs;
                dataPager.PageCount = e.pageCount;
                if (DtGrid.Columns.Count() > 1)
                {
                    DtGrid.Columns.RemoveAt(1);
                }
                if (ColumnNames == null)
                {
                    return;
                }

                if (ColumnNames.Count == 0)
                {
                    return;
                }

                //显示用户指定的列
                foreach (var col in ColumnNames)
                {
                    string colName = GetLocalName(col.Key);
                    if (string.IsNullOrEmpty(colName))
                    {
                        continue;
                    }

                    DataGridTextColumn txtCol = new DataGridTextColumn();
                    txtCol.Header = colName;
                    txtCol.Binding = GetColumnBinding(col.Value);
                    txtCol.IsReadOnly = true;
                    DtGrid.Columns.Add(txtCol);
                }
            }
        }

        void DtGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            int index = e.Row.GetIndex();
            var cell = DtGrid.Columns[0].GetCellContent(e.Row).FindName("checkbox") as CheckBox;
            if (cell != null)
            {
                cell.IsChecked = false;
            }
        }

        /// <summary>
        /// 对绑定值进行转换
        /// </summary>
        /// <param name="strColumnValue"></param>
        /// <returns></returns>
        private static Binding GetColumnBinding(string strColumnValue)
        {
            Binding binding = new Binding();
            if (string.IsNullOrWhiteSpace(strColumnValue))
            {
                binding = new Binding(strColumnValue);
                return binding;
            }

            string[] strlist = strColumnValue.Split(',');
            if (strlist.Length != 3)
            {
                binding = new Binding(strColumnValue);
                return binding;
            }

            switch (strlist[2].ToString().ToUpper())
            {
                case "CustomDictionaryConverter":
                    //CustomDictionaryConverter dic = new CustomDictionaryConverter();
                    //binding = new Binding(strlist[0].ToString()) { Converter = dic, ConverterParameter = strlist[1].ToString() };
                    break;
                case "CUSTOMDATECONVERTER":
                    SMT.FBAnalysis.UI.CustomDateConverter dat = new SMT.FBAnalysis.UI.CustomDateConverter();
                    binding = new Binding(strlist[0].ToString()) { Converter = dat, ConverterParameter = strlist[1].ToString() };
                    break;
            }

            return binding;
        }

        private string GetLocalName(string columnName)
        {
            string localName = string.Empty;
            localName = SMT.FBAnalysis.UI.Common.Utility.GetResourceStr(columnName);
            return localName;

        }
        private void BindData()
        {

            int pageCount = 0;

            client.GetLookupOjbectsAsync(EntityName, modelCode, SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID, dataPager.PageIndex, dataPager.PageSize, string.IsNullOrEmpty(OrderBy) ? "OWNERID" : OrderBy,
                filter, paras, pageCount);
            loadbar.Start();
            
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (DtGrid.SelectedItem == null)
            {
                SMT.SaaS.FrameworkUI.Common.Utility.ShowCustomMessage(SMT.SaaS.FrameworkUI.MessageTypes.Error, this.TitleContent.ToString(), SMT.FBAnalysis.UI.Common.Utility.GetResourceStr("PLEASESELECT", this.TitleContent.ToString()));
                return;
            }

            this.Close();

            if (SelectedClick != null)
                SelectedClick(sender, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void GridPager_Click(object sender, RoutedEventArgs e)
        {
            BindData();
        }
    }
}

