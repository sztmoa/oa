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
using SMT.SaaS.OA.UI;
using SMT.SaaS.FrameworkUI;
using SMT.SaaS.OA.UI.Class;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.OA.UI.SmtOAPersonOfficeService;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.SAAS.Main.CurrentContext;
using SMT.SaaS.FrameworkUI.SelectCityControl;
using SMT.SaaS.FrameworkUI.SelectPostLevel;
using SMT.Saas.Tools.OrganizationWS;
using SMT.SaaS.OA.UI.UserControls.Travelmanagement;

namespace SMT.SaaS.OA.UI.Views.Travelmanagement
{
    public partial class SolutionManagement
    {

        #region 交通工具标准datagrid事件

        private void DgVechileStandard_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (DGVechileStandard.ItemsSource != null)
            {
                T_OA_TAKETHESTANDARDTRANSPORT Standard = (T_OA_TAKETHESTANDARDTRANSPORT)e.Row.DataContext;
                TravelDictionaryComboBox ComVechile = DGVechileStandard.Columns[1].GetCellContent(e.Row).FindName("ComVechileType") as TravelDictionaryComboBox;
                TravelDictionaryComboBox ComLevel = DGVechileStandard.Columns[2].GetCellContent(e.Row).FindName("ComVechileTypeLeve") as TravelDictionaryComboBox;
                SelectPost EndComPost = DGVechileStandard.Columns[3].GetCellContent(e.Row).FindName("txtSelectPost") as SelectPost;


                DGVechileStandard.SelectedItem = e.Row;
                string dictid = "";
                if (ComVechile != null)
                {
                    foreach (T_SYS_DICTIONARY Region in ComVechile.Items)
                    {
                        if (Region.DICTIONARYVALUE.ToString() == Standard.TYPEOFTRAVELTOOLS)
                        {
                            ComVechile.SelectedItem = Region;
                            dictid = Region.DICTIONARYID;
                            break;
                        }

                    }
                }
                if (ComVechile.SelectedIndex < 0)
                {
                    ComVechile.SelectedIndex = 0;
                }
                if (ComLevel != null)
                {
                    var ents = from ent in ListVechileLevel
                               where ent.T_SYS_DICTIONARY2.DICTIONARYID == dictid
                               select ent;
                    if (ents.Count() > 0)
                    {
                        ComLevel.ItemsSource = ents.ToList();//根据类型绑定级别
                        foreach (T_SYS_DICTIONARY Region in ents)
                        {

                            if (Region.DICTIONARYVALUE.ToString() == Standard.TAKETHETOOLLEVEL)
                            {
                                ComLevel.SelectedItem = Region;
                                break;
                            }

                        }
                    }
                }
                if (ComLevel.SelectedIndex < 0)
                {
                    ComLevel.SelectedIndex = 0;
                }

                if (!string.IsNullOrEmpty(Standard.ENDPOSTLEVEL))
                {
                    //将  岗位值转换为对应的名称
                    //string PostCode = EndComPost.TxtSelectedPost.Text;
                    string PostCode = "";
                    string[] arrstr = Standard.ENDPOSTLEVEL.Split(',');
                    foreach (var d in arrstr)
                    {
                        int i = d.ToInt32();
                        var ents = from n in ListPost
                                   where n.DICTIONARYVALUE == i
                                   select n;
                        if (ents.Count() > 0)
                            PostCode += ents.FirstOrDefault().DICTIONARYNAME.ToString() + ",";
                    }
                    if (!(string.IsNullOrEmpty(PostCode)))
                    {
                        PostCode = PostCode.Substring(0, PostCode.Length - 1);

                    }
                    EndComPost.TxtSelectedPost.Text = PostCode;
                }
                //}
            }


        }
        /// <summary>
        /// 交通工具类型选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComVechileType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tempDetail = DGVechileStandard.SelectedItem as T_OA_TAKETHESTANDARDTRANSPORT;
          
            TravelDictionaryComboBox vechiletype = sender as TravelDictionaryComboBox;
            if (vechiletype.SelectedIndex > 0)
            {
                T_SYS_DICTIONARY VechileTypeObj = vechiletype.SelectedItem as T_SYS_DICTIONARY;
                if (tempDetail != null)
                { 
                tempDetail.TYPEOFTRAVELTOOLS = VechileTypeObj.DICTIONARYVALUE.ToString();
                }

                if (DGVechileStandard.SelectedItem != null)
                {
                    if (DGVechileStandard.Columns[1].GetCellContent(DGVechileStandard.SelectedItem) != null)
                    {
                        TravelDictionaryComboBox ComLevel = DGVechileStandard.Columns[2].GetCellContent(DGVechileStandard.SelectedItem).FindName("ComVechileTypeLeve") as TravelDictionaryComboBox;

                        var ListObj = from ent in ListVechileLevel
                                      where ent.T_SYS_DICTIONARY2.DICTIONARYID == VechileTypeObj.DICTIONARYID
                                      select ent;
                        if (ListObj.Count() > 0)
                        {

                            if (ListObj != null)
                            {
                                //ListObj.ToList().Insert(0, nuldict);
                                ComLevel.ItemsSource = ListObj.ToList();
                                ComLevel.SelectedIndex = 0;
                                if (tempDetail != null)
                                {
                                    T_SYS_DICTIONARY LevelObj = ComLevel.SelectedItem as T_SYS_DICTIONARY;
                                    tempDetail.TAKETHETOOLLEVEL = LevelObj.DICTIONARYVALUE.ToString();
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region 选择岗位级别
        private void txtSelectPost_SelectClick(object sender, EventArgs e)
        {
            var tempDetail = DGVechileStandard.SelectedItem as T_OA_TAKETHESTANDARDTRANSPORT;


            SelectPost txt = (SelectPost)sender;
            string StrOld = txt.TxtSelectedPost.Text.ToString();

            PostList SelectCity = new PostList(StrOld, AAA);
            string citycode = "";
            SelectCity.SelectedClicked += (obj, ea) =>
            {
                AAA = "";
                string StrPost = SelectCity.Result.Keys.FirstOrDefault();
                if (!string.IsNullOrEmpty(StrPost))
                    txt.TxtSelectedPost.Text = StrPost.Substring(0, StrPost.Length - 1);
                citycode = SelectCity.Result[SelectCity.Result.Keys.FirstOrDefault()].ToString();
                if (!string.IsNullOrEmpty(citycode))
                    citycode = citycode.Substring(0, citycode.Length - 1);

                if (txt.Name == "txtSelectPost")
                {
                    StandardList[DGVechileStandard.SelectedIndex].ENDPOSTLEVEL = citycode;
                    tempDetail.ENDPOSTLEVEL = citycode;
                    AAA = citycode;
                }
            };
            var windows = SMT.SAAS.Controls.Toolkit.Windows.ProgramManager.ShowProgram(Utility.GetResourceStr("CITY"), "", "123", SelectCity, false, false, null);
            if (SelectCity is PostList)
            {
                (SelectCity as PostList).Close += (o, args) =>
                {
                    windows.Close();
                };
            }
        }
        #endregion


        #region 出差方案选择
        private void cmbSolution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            travelObj = cmbSolution.SelectedItem as T_OA_TRAVELSOLUTIONS;
            if (travelObj != null)
            {

                if (isChange) client.GetVechileStandardAndPlaneLineAsync(travelObj.TRAVELSOLUTIONSID, RefPlaneList, RefvechileList);
                DetailSolutionInfo(travelObj);
                //client.GetTravleSolutionSetBySolutionIDCompleted += new EventHandler<GetTravleSolutionSetBySolutionIDCompletedEventArgs>(client_GetTravleSolutionSetBySolutionIDCompleted);
                loadbar.Start();
                if (!isChange && isDefaultSolution)
                {
                    client.GetTravleSolutionSetBySolutionIDAsync(travelObj.TRAVELSOLUTIONSID, "DefaultSolution");
                    client.GetVechileStandardAndPlaneLineAsync(travelObj.TRAVELSOLUTIONSID, RefPlaneList, RefvechileList, "DefaultSolution");
                    isDefaultSolution = false;
                }
                else
                {
                    client.GetTravleSolutionSetBySolutionIDAsync(travelObj.TRAVELSOLUTIONSID);
                }
                SetEnabled();
            }
        }
        #endregion
    }
}
