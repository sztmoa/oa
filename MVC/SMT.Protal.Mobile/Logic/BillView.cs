using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Xml;
using SMT.Portal.Common.SmtForm.BLL;
using SMT.Portal.Common.SmtForm.Framework;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data;
using System.Drawing;
using SMT.SaaS.Services.SmtMoblieWS;
using System.Text.RegularExpressions;

using SMT.SaaS.Services.PublicInterfaceWS;

namespace SMT.Portal.Common.SmtForm.Logic
{
    public class BillView
    {
        private ObjTemplate _template;
        public ObjTemplate Template
        {
            get
            {
                if (_template == null && HttpContext.Current.Request.Cookies["Template"] != null)
                {
                    _template = CacheData.CacheConfig.GetTemplate()[HttpContext.Current.Request.Cookies["Template"].Values["Template"].ToString()];
                }
                else
                {
                    _template = CacheData.CacheConfig.GetTemplate()["Template1"];
                }
                return _template;
            }
        }
        private ObjViewmode _viewmode;
        public ObjViewmode Viewmode
        {
            get
            {
                if (_viewmode == null && HttpContext.Current.Request.Cookies["Viewmode"] != null)
                {

                    _viewmode = CacheData.CacheConfig.GetViewmode()[HttpContext.Current.Request.Cookies["Viewmode"].Values["Viewmode"].ToString()];
                }
                else
                {
                    _viewmode = CacheData.CacheConfig.GetViewmode()["Phone"];
                }
                return _viewmode;
            }
        }

        /// <summary>
        /// 创建table
        /// </summary>
        private HtmlTable _tableCtrl;
        public HtmlTable TableCtrl
        {
            get
            {
                if (_tableCtrl == null)
                {
                    _tableCtrl = new HtmlTable();
                    _tableCtrl.ID = "html";
                    _tableCtrl.CellPadding = 3;
                    _tableCtrl.CellSpacing = 0;
                    _tableCtrl.Width = "100%";
                }
                return _tableCtrl;
            }
        }

        private bool _isCreateConsultation = false;
        public bool IsCreateConsultation
        {
            get { return _isCreateConsultation; }
            set { _isCreateConsultation = value; }
        }

        public ObjBill CreateView(Panel pn, string xml, bool IsApproval, ref string error)
        {
            if (null != xml && xml.Length > 0)
            {
                ObjBill bill = BillData.CreateBill(xml, ref error);

                CreatForm(bill, IsApproval);
                pn.Controls.Add(TableCtrl);
                //HtmlTable table = CreateConsultationList(bill.ConsultationLists);
                //table.ID = "tbConsultationList";
                //table.Attributes["class"] = "maintable";
                //table.CellPadding = 0;
                //table.CellSpacing = 0;
                //pn.Controls.Add(table);

                return bill;
            }
            return null;
        }

        private void CreatForm(ObjBill bill, bool IsApproval)
        {
            //创建头部显示标题
            {
                HtmlGenericControl DivTop = new HtmlGenericControl("div");
                DivTop.Attributes.Add("class", "tabpanelheaddiv");
                HtmlGenericControl span = new HtmlGenericControl("span");
                span.Attributes.Add("style", "height:30px;line-height:30px;");
                span.InnerText = bill.Text;
                DivTop.Controls.Add(span);
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell td = CreateTD(1, null, null, "");
                td.Attributes.Add("style", "padding:0px;margin:0px;");
                td.Controls.Add(DivTop);
                tr.Controls.Add(td);
                //Label label = new Label();
                //label.Text = text;
                //td.Controls.Add(label);
                //tr.Controls.Add(td);
                TableCtrl.Controls.Add(tr);
                //TableCtrl.Controls.Add(CreateTop(bill.Text));
            }

            //创建主表
            CreateDetails(bill.DetailLists);

            //创建rtf内容
            CreateRtf(bill);

            int sum = 0;
            //创建从表
            CreateSubList(bill.SubLists, bill);

            //创建附件列表
            CreateAttachList(bill.AttachLists);

            //创建审核列表

            if (IsApproval == true)
                CreateApprovalList(bill.ApprovalLists);

            //if (IsCreateConsultation)
            //{
            //    CreateConsultationList(bill.ConsultationLists);
            //}
        }

        private void CreateSum(int sum)
        {

        }

        public void CreateRtf(ObjBill bill)
        {
            if (!String.IsNullOrEmpty(bill.RtfData))
            {
                HtmlTableRow trrtf = new HtmlTableRow();
                HtmlTableCell tdrtf = new HtmlTableCell();
                tdrtf.Attributes.Add("class", "billTopTd");
                Label label = new Label();
                label.Text = "内容";
                tdrtf.Controls.Add(label);

                trrtf.Controls.Add(tdrtf);
                TableCtrl.Controls.Add(trrtf);

                HtmlTableRow trHd = new HtmlTableRow();
                HtmlTableCell tdHd = new HtmlTableCell();

                HtmlInputHidden inputHd = new HtmlInputHidden();
                inputHd.ID = "hdGuid";
                inputHd.Value = bill.RtfData;
                tdHd.Controls.Add(inputHd);
                HtmlGenericControl div = new HtmlGenericControl("div");
                string rtfContent = bill.RtfData.Equals("System.Byte[]") ? bill.FormID : bill.RtfData;
                div.InnerHtml = GetRtf(bill.FormID);
                div.ID = "divContext";
                tdHd.Controls.Add(div);
                /*
                Panel panel = new Panel();
                panel.ID = "divContext";
                
                Label lab = new Label();
                lab.Text = "正在加载内容...";
                
                HtmlImage img = new HtmlImage();
                img.Src = Template.ImageUrl + "loading.gif";

                panel.Controls.Add(lab);
                panel.Controls.Add(img);
                 
                tdHd.Controls.Add(panel);*/

                trHd.Controls.Add(tdHd);

                TableCtrl.Controls.Add(trHd);

                //td3.Controls.Add(panel);
            }
        }

        public string GetRtf(string strGuid)
        {
           
            PublicServiceClient publicClient = SMT.SaaS.Common.WCFServiceProvider.Instance.GetService<PublicServiceClient>("IPublicService");
            try
            {

                string result = publicClient.GetContentFormatImgToUrl(strGuid);

                if (!string.IsNullOrEmpty(result))
                {
                    int styleIndex = result.IndexOf("<style type=\"text/css\">");
                    int styleLast = result.LastIndexOf("</style>");
                    if (styleIndex == -1 || styleLast == -1)
                    {
                        return result;
                    }
                    else
                    {
                        string style = result.Substring(styleIndex, styleLast + 8 - styleIndex);

                        int bodyIndex = result.IndexOf("<body>");
                        int bodyLast = result.LastIndexOf("</body>");
                        string body = result.Substring(bodyIndex + 6, bodyLast - bodyIndex - 6);
                        body = Regex.Replace(body, "href=\"(.*?)\"", "href=\"#\"", RegexOptions.IgnoreCase);
                        body = Regex.Replace(body, "href='(.*?)'", "href='#'", RegexOptions.IgnoreCase);
                        if (body == "" || body == null)
                        {
                            body = "无内容";
                        }
                        return style + body;
                    }
                }
                else
                {
                    return "无内容";
                }

            }
            catch (Exception ex)
            {
                return "error:" + ex.Message;
            }

        }
        /// <summary>
        /// 根据主表数据创建控件，设置样式，并添加至table
        /// </summary>
        /// <param name="objDetailList">主表数据数组</param>
        public void CreateDetails(ObjDetailList[] objDetailList)
        {
            //创建table
            HtmlTable tb = CreateTable(0, 0, 0, "100%", null, "MobileTabTable grid");

            if (null != objDetailList && objDetailList.Length > 0)
            {
                foreach (ObjDetailList detailList in objDetailList)
                {
                    //判断pad或者phone，确定在页面显示几列
                    int column = int.Parse(this.Viewmode.Name == "Pad" ?
                        detailList.PadRepeatCount : detailList.PhoneRepeatCount);

                    int count = 0;
                    HtmlTableRow tr = null;
                    int m = 0;
                    int visibleCount = 0;
                    int tdcount = 100;
                    ListBase<Field> fieldDict = detailList.GetFieldDict();
                    if (null != fieldDict && fieldDict.Count > 0)
                    {

                        foreach (Field field in fieldDict)
                        {
                            if (field.ControlType == Constant.CtrlType.Rtf)
                            {

                            }

                            if (field.IsVisible && field.ControlType != Constant.CtrlType.Rtf)
                            {
                                if (count == visibleCount - 1)
                                {
                                    field.Colspan = (column - 1 - m % column) + 1;
                                }

                                if (tdcount >= column)
                                {
                                    tr = CreateTR(null, "16px", null);
                                    tdcount = 0;
                                }
                                tdcount = tdcount + field.Colspan;
                                {
                                    string cssName = "billField";
                                    if (!string.IsNullOrEmpty(field.CssName))
                                    {
                                        cssName = field.CssName;
                                    }

                                    if (field.IsShowCaption)    //是否显示标题
                                    {
                                        HtmlTableCell td1 = CreateTD(1, null, null, "billFormIdText");
                                        td1.Controls.Add((Control)field.CaptionControl);
                                        td1.Width = (100 / (column * 4)) + "%";
                                        ((Label)field.CaptionControl).ToolTip = field.FieldName;
                                        tr.Controls.Add(td1);

                                        HtmlTableCell td3 = CreateTD(1, null, null, cssName);
                                        td3.ColSpan = (field.Colspan - 1) * 2 + 1;
                                        td3.Width = (((100 / (column * 4)) * 3) + (100 / (column * 4)) * (field.Colspan - 1)) + "%";

                                        if (field.ControlType == Constant.CtrlType.Rtf)
                                        {

                                            //Panel panel = new Panel();
                                            //panel.ID = "divContext";

                                            //Label lab = new Label();
                                            //lab.Text = "Load...";

                                            //HtmlImage img = new HtmlImage();
                                            //img.Src = Template.ImageUrl + "loading.gif";

                                            //panel.Controls.Add(lab);
                                            //panel.Controls.Add(img);
                                            //td3.Controls.Add(panel);
                                        }
                                        else
                                        {
                                            td3.Controls.Add((Control)field.MainControl[0]);
                                        }


                                        tr.Controls.Add(td3);
                                    }
                                    else
                                    {
                                        HtmlTableCell td3 = CreateTD(1, null, null, cssName);
                                        td3.ColSpan = (field.Colspan - 1) * 2 + 1 + 1;
                                        td3.Width = ((((100 / (column * 4)) * 3) + (100 / (column * 4)) * (field.Colspan - 1)) + (100 / (column * 4))) + "%"; ;
                                        Literal li = new Literal();
                                        li.Text = field.CaptionName;
                                        td3.Controls.Add(li);

                                        td3.Controls.Add((Control)field.MainControl[0]);
                                        tr.Controls.Add(td3);
                                    }

                                    tb.Controls.Add(tr);
                                }
                            }

                            count++;
                            m = m + field.Colspan;
                        }
                    }
                }


                TableCtrl.Rows.Add(AddTable(tb));
            }
        }



        /// <summary>
        /// 根据从表数据创建控件，设置样式，并添加至table
        /// </summary>
        /// <param name="objSubList"></param>
        public void CreateSubList(ObjSubList[] objSubList, ObjBill bill)
        {
            if (null != objSubList && objSubList.Length > 0)
            {
                int dgID = 1;
                foreach (ObjSubList subList in objSubList)
                {
                    //如果是空行则退出
                    if (subList.RowList.Length == 0)
                    {
                        continue;
                    }
                    //DataTable dt = subList.GetDataTable();
                    #region Verticle
                    if (subList.Align == "verticle")
                    {
                        if (null != subList.ColumnList && subList.ColumnList.Length > 0)
                        {
                            {
                                HtmlTableRow tr = new HtmlTableRow();
                                HtmlTableCell td = CreateTD(1, null, null, "billTopTd");

                                Label label = new Label();
                                label.Text = subList.Text;
                                td.Controls.Add(label);

                                tr.Controls.Add(td);
                                TableCtrl.Controls.Add(tr);
                            }
                            HtmlTable tbSubList = new HtmlTable();
                            tbSubList.Width = "100%";
                            tbSubList.BgColor = "#999999";
                            tbSubList.Border = 0;
                            tbSubList.CellSpacing = 1;
                            tbSubList.CellPadding = 3;
                            foreach (ObjRow row in subList.RowList)
                            {
                                int n = 0;
                                HtmlTableRow tr = null;
                                int columncount = 0;
                                foreach (ObjColumn column in subList.ColumnList[0].ColumnList)
                                {
                                    if (column.IsVisible)
                                    { columncount++; }
                                }
                                string name = "";
                                foreach (ObjColumn column in subList.ColumnList[0].ColumnList)
                                {
                                    if (column.IsVisible)
                                    {
                                        if (n == 0)
                                        {
                                            tr = new HtmlTableRow();
                                            HtmlTableCell td = new HtmlTableCell();
                                            td.InnerText = row.GetObjFieldValue(column.Name);
                                            name = td.InnerText;
                                            td.BgColor = "white";
                                            td.Width = "25%";
                                            td.RowSpan = columncount - 1;
                                            td.Align = "center";
                                            td.Style.Add("font-size", "14px");
                                            td.Style.Add("font-weight", "bold");
                                            tr.Cells.Add(td);
                                        }
                                        else
                                        {
                                            if (n > 1)
                                            {
                                                tr = new HtmlTableRow();
                                            }
                                            HtmlTableCell td = new HtmlTableCell();
                                            td.InnerText = column.Text;
                                            td.BgColor = "#f1f1f1";
                                            td.Width = "25%";
                                            tr.Cells.Add(td);

                                            HtmlTableCell tdcontent = new HtmlTableCell();
                                            tdcontent.InnerText = row.GetObjFieldValue(column.Name);
                                            tdcontent.BgColor = "white";
                                            tdcontent.Width = "25%";

                                            tr.Cells.Add(tdcontent);

                                            if (n == 1)
                                            {
                                                HtmlTableCell tdview = new HtmlTableCell();
                                                HtmlTable tbDetail = new HtmlTable();
                                                tbDetail.Width = "100%";
                                                tbDetail.BgColor = "#999999";
                                                tbDetail.Border = 0;
                                                tbDetail.CellSpacing = 1;
                                                tbDetail.CellPadding = 3;


                                                HtmlTableRow trDetail1 = new HtmlTableRow();
                                                HtmlTableCell tdDetail1 = new HtmlTableCell();
                                                tdDetail1.Align = "left";
                                                tdDetail1.BgColor = "#e1e1e1";
                                                tdDetail1.Style.Add("font-weight", "bold");
                                                tdDetail1.InnerHtml = name + "薪资详情";
                                                tdDetail1.Height = "35";
                                                trDetail1.Cells.Add(tdDetail1);

                                                HtmlTableCell tdDetail4 = new HtmlTableCell();
                                                tdDetail4.Align = "right";
                                                tdDetail4.BgColor = "#e1e1e1";
                                                tdDetail4.Style.Add("font-weight", "bold");
                                                tdDetail4.InnerHtml = @"<input type=""button"" value=""关闭"" onclick=""javascript:closeDetail();"" onmousemove=""javascript:MousemoveAudit(this.id);""

                                    onmouseout=""javascript:MouseoutAudit(this.id);"" id=""btn_close"" name=""btn_close"" class=""billAuditYes"" />";
                                                tdDetail4.Height = "35";
                                                trDetail1.Cells.Add(tdDetail4);


                                                tbDetail.Rows.Add(trDetail1);

                                                foreach (ObjColumn detailcolumn in subList.ColumnList[0].ColumnList)
                                                {
                                                    HtmlTableRow trDetail2 = new HtmlTableRow();
                                                    HtmlTableCell tdDetail2 = new HtmlTableCell();
                                                    tdDetail2.Align = "left";
                                                    tdDetail2.Width = "50%";
                                                    tdDetail2.BgColor = "#f1f1f1";
                                                    tdDetail2.InnerHtml = detailcolumn.Text;
                                                    trDetail2.Cells.Add(tdDetail2);

                                                    HtmlTableCell tdDetail3 = new HtmlTableCell();
                                                    tdDetail3.Align = "left";
                                                    tdDetail2.Width = "50%";
                                                    tdDetail3.BgColor = "#ffffff";
                                                    tdDetail3.InnerHtml = row.GetObjFieldValue(detailcolumn.Name);
                                                    trDetail2.Cells.Add(tdDetail3);


                                                    tbDetail.Rows.Add(trDetail2);
                                                }
                                                tdview.RowSpan = columncount - 1;
                                                tdview.InnerHtml = "<a href=\"javascript:showDetail('virdetail" + row.Id + "')\">详请</a>";
                                                HtmlGenericControl div = new HtmlGenericControl();
                                                div.TagName = "div";
                                                div.Style.Add("display", "none");
                                                div.ID = "virdetail" + row.Id;
                                                div.Controls.Add(tbDetail);
                                                tdview.Controls.Add(div);
                                                tdview.BgColor = "white";
                                                tdview.Width = "25%";
                                                tdview.Align = "center";
                                                tr.Cells.Add(tdview);
                                            }
                                        }

                                        if (n > 0)
                                        {
                                            tbSubList.Rows.Add(tr);
                                        }
                                        n++;
                                    }

                                }
                            }
                            {
                                int n = 0;
                                HtmlTableRow tr = null;
                                int columncount = 0;
                                foreach (Field field in bill.DetailLists[0].GetFieldDict())
                                {
                                    if (!String.IsNullOrEmpty(field.ColumnOrder) && Convert.ToInt32(field.ColumnOrder) > 0)
                                    {
                                        columncount++;
                                    }
                                }
                                foreach (Field field in bill.DetailLists[0].GetFieldDict())
                                {

                                    if (!String.IsNullOrEmpty(field.ColumnOrder) && Convert.ToInt32(field.ColumnOrder) > 0)
                                    {
                                        if (n == 0)
                                        {
                                            tr = new HtmlTableRow();
                                            HtmlTableCell td = new HtmlTableCell();
                                            td.InnerText = "总计";
                                            td.BgColor = "white";
                                            td.Width = "25%";
                                            td.RowSpan = columncount;
                                            td.Align = "center";
                                            td.Style.Add("font-size", "14px");
                                            td.Style.Add("font-weight", "bold");
                                            td.Style.Add("border-top", "1px black solid");
                                            tr.Cells.Add(td);


                                        }

                                        {
                                            if (n > 0)
                                            {
                                                tr = new HtmlTableRow();
                                            }
                                            HtmlTableCell td = new HtmlTableCell();
                                            td.InnerText = field.CaptionName.TrimEnd('：');
                                            td.BgColor = "#f1f1f1";
                                            td.Width = "25%";
                                            if (n == 0)
                                            {
                                                td.Style.Add("border-top", "1px black solid");
                                            }
                                            tr.Cells.Add(td);

                                            HtmlTableCell tdcontent = new HtmlTableCell();
                                            tdcontent.Controls.Add((Control)field.MainControl[0]);
                                            tdcontent.BgColor = "white";
                                            tdcontent.Width = "25%";
                                            if (n == 0)
                                            {
                                                tdcontent.Style.Add("border-top", "1px black solid");


                                            }
                                            tr.Cells.Add(tdcontent);

                                            if (n == 0)
                                            {
                                                HtmlTableCell tdview = new HtmlTableCell();
                                                tdview.InnerHtml = "&nbsp;";
                                                tdview.BgColor = "white";
                                                tdview.Width = "25%";
                                                tdview.Align = "center";
                                                tdview.RowSpan = columncount;
                                                if (n == 0)
                                                {
                                                    tdview.Style.Add("border-top", "1px black solid");
                                                }
                                                tr.Cells.Add(tdview);
                                            }

                                        }

                                        //if (n > 0)
                                        {
                                            tbSubList.Rows.Add(tr);
                                        }
                                        n++;
                                    }

                                }
                            }

                            HtmlTableRow tra = new HtmlTableRow();
                            HtmlTableCell tda = CreateTD(1, "100%", null, null);
                            tda.Controls.Add(tbSubList);
                            tra.Cells.Add(tda);
                            TableCtrl.Rows.Add(tra);
                        }
                    }
                    #endregion
                    else
                    {
                        if (null != subList.ColumnList && subList.ColumnList.Length > 0)
                        {
                            if (subList.RowList.Length == 0)
                            {
                                continue;
                            }
                            if (subList.RowList[0] == null)
                            {
                                continue;
                            }
                            if (subList.HideWhenEmpty == "1" && subList.CheckEmpty())
                            {
                                continue;
                            }
                            {
                                //HtmlTableRow tr = new HtmlTableRow();
                                //HtmlTableCell td = CreateTD(1, null, null, "billTopTd");

                                //Label label = new Label();
                                //label.Text = subList.Text;
                                //td.Controls.Add(label);

                                //tr.Controls.Add(td);
                                //TableCtrl.Controls.Add(tr);
                                //从表头
                                HtmlGenericControl DivTop = new HtmlGenericControl("div");
                                DivTop.Attributes.Add("class", "tabpanelheaddiv");
                                HtmlGenericControl span = new HtmlGenericControl("span");
                                span.Attributes.Add("style", "height:30px;line-height:30px;");
                                span.InnerText = subList.Text;
                                DivTop.Controls.Add(span);

                                HtmlTableRow tr = new HtmlTableRow();
                                HtmlTableCell td = CreateTD(1, null, null, "");
                                td.Attributes.Add("style", "padding:3px 0px 0px 0px;margin:0px;");
                                td.Controls.Add(DivTop);
                                tr.Controls.Add(td);
                                TableCtrl.Controls.Add(tr);
                            }
                            {
                                HtmlTable tbSubList = new HtmlTable();
                                tbSubList.Width = "100%";
                                tbSubList.BgColor = "#999999";
                                tbSubList.Border = 0;
                                tbSubList.CellSpacing = 1;
                                tbSubList.CellPadding = 3;
                                tbSubList.Attributes.Add("class", "audit ClassTableNew MobileTabGrid");
                                //tbSubList.Attributes.Add("style", "border: 1px solid #5F5F5F;");
                                HtmlTableRow tr = new HtmlTableRow();
                                tr.Attributes.Add("class", "auditHeader");
                                {
                                    
                                    HtmlTableCell td = new HtmlTableCell("th");
                                    td.Attributes.Add("style", "padding:3px 0px 0px 0px;margin:0px;");
                                    td.InnerHtml = "<div></div>";
                                    //td.BgColor = "#E1EFF7";
                                    td.Width = "3%";
                                    tr.Cells.Add(td);
                                }
                                double sum = 0;
                                bool isSum = false;
                                string sumName = string.Empty;
                                List<string> sumList = new List<string>();
                                HtmlTableRow sumRow = new HtmlTableRow();
                                var sumTD = new HtmlTableCell() { InnerText = "" };
                                sumTD.Attributes.Add("class", "Align_Right");
                                sumRow.Cells.Add(sumTD);

                                sumRow.Attributes.Add("class", "ClassTableNewSum");
                                int cellIndex = 1;
                                int cellCols = 0;
                                int cellColsOut = 0;
                                foreach (ObjColumn column in subList.ColumnList[0].ColumnList)
                                {
                                    if (column.IsVisible)
                                    {
                                        HtmlTableCell td = new HtmlTableCell("th");
                                        td.Attributes.Add("Title",column.ToolTip);
                                        td.InnerHtml = "<div>" + column.Text + "</div>";
                                        // td.Attributes.Add("class", "ui-state-default ui-th-column ui-th-ltr");
                                        // td.BgColor = "#E1EFF7";
                                        td.Width = column.Width;
                                        tr.Cells.Add(td);
                                        if (column.ColumnSum != null)
                                        {
                                            if (column.ColumnSum.ToLower() == "true")
                                            {
                                                sumName = column.Name;
                                                isSum = true;
                                                cellCols++;
                                                cellColsOut = 1;
                                                sumList.Add(column.Name);
                                            }
                                            else
                                            {
                                                cellCols = 0;
                                            }
                                        }
                                        else
                                        {
                                            cellCols = 0;
                                        }
                                        cellIndex++;
                                        if (cellCols > 1)
                                        {
                                            sumRow.Cells[sumRow.Cells.Count - 1].ColSpan = (cellIndex - sumRow.Cells.Count) + 1;
                                        }
                                        else
                                        {
                                            sumRow.Cells.Add(new HtmlTableCell() { InnerText = "" });
                                        }

                                       
                                    }

                                }
                                tbSubList.Rows.Add(tr);
                                int i = 0;
                                bool hassub = false;
                                foreach (ObjRow row in subList.RowList)
                                {
                                    //忽略空行
                                    if (row == null)
                                    {
                                        continue;
                                    }

                                    HtmlTableRow trcontent = new HtmlTableRow();
                                    trcontent.Attributes.Add("class", "auditRow");
                                    {
                                        HtmlTableCell td = new HtmlTableCell();
                                        if (row.SubSubList != null && row.SubSubList.Length > 0)
                                        {
                                            td.InnerHtml = "<img src='" + this.Template.ImageUrl + "plus.gif' id='subimg" + i + "'>";
                                            td.RowSpan = 1;
                                            td.ID = "tdsub" + i;

                                        }
                                        else
                                        {
                                            td.InnerHtml = "&nbsp;";
                                            td.RowSpan = 1;
                                            td.ID = "tbsub" + i;
                                        }
                                        td.Align = "center";
                                        td.VAlign = "top";
                                        td.BgColor = "white";
                                        td.Width = "3%";
                                        trcontent.Cells.Add(td);
                                    }
                                    if (row.SubSubList != null && row.SubSubList.Length > 0)
                                    {
                                        trcontent.Attributes.Add("onclick", @"showsub(""tr" + i + @""",""subimg" + i + @""",""tdsub" + i + @""")");
                                    }
                                    int rowIndex = 1;
                                    cellIndex = 1;
                                    foreach (ObjColumn column in subList.ColumnList[0].ColumnList)
                                    {
                                        if (column.IsVisible)
                                        {
                                            HtmlTableCell tdcontent = new HtmlTableCell();
                                            
                                            ObjField field = row.GetObjFieldDict()[column.Name];
                                            if ( field != null)
                                            {
                                                string rowValue = field.DataValue;
                                                var tempValue = rowValue;
                                                if (!string.IsNullOrEmpty(column.Format))
                                                {
                                                    tempValue = field.GetFormatValue(column.Format);
                                                }
                                                tdcontent.InnerText = tempValue;
                                                

                                                if (!string.IsNullOrEmpty(field.Color))
                                                {
                                                    tdcontent.Style.Add("color", field.Color);
                                                }
                                                if (!string.IsNullOrEmpty(field.Tooltip))
                                                {
                                                    tdcontent.Attributes.Add("title", field.Tooltip);
                                                }
                                                var align = "Left";
                                                if (!string.IsNullOrEmpty(column.Align))
                                                {
                                                    align = column.Align;
                                                }
                                                tdcontent.Attributes.Add("class", "Align_" + align);
                                                if (isSum)
                                                {
                                                    if (sumList.Contains(column.Name))
                                                    {
                                                        double tryValue = 0;
                                                        if (double.TryParse(rowValue, out tryValue))
                                                        {
                                                            sum += tryValue;
                                                        }
                                                        sumRow.Cells[cellIndex].InnerText = string.Format("合计:{0}", sum);
                                                    }
                                                    else
                                                    {
                                                        cellIndex++;
                                                    }

                                                }
                                            }
                                            tdcontent.BgColor = "white";
                                            trcontent.Cells.Add(tdcontent);
                                           
                                            rowIndex++;
                                        }

                                    }
                                    tbSubList.Rows.Add(trcontent);
                                    #region 如果有嵌套的明细数据
                                    //如果有嵌套的明细数据
                                    if (row.SubSubList != null && row.SubSubList.Length > 0)
                                    {
                                        hassub = true;
                                        foreach (ObjSubList subsubList in row.SubSubList)
                                        {
                                            HtmlTable tbSubSubList = new HtmlTable();
                                            tbSubSubList.Width = "100%";
                                            tbSubSubList.BgColor = "#999999";
                                            tbSubSubList.Border = 0;
                                            tbSubSubList.CellSpacing = 1;
                                            tbSubSubList.CellPadding = 3;
                                            tbSubSubList.Attributes.Add("style", "margin: 5px 2%;width: 96%;");
                                            HtmlTableRow trSub = new HtmlTableRow();
                                            //设置DataGrid控件的列
                                            foreach (ObjColumn column in subsubList.ColumnList[0].ColumnList)
                                            {
                                                if (column.IsVisible)
                                                {
                                                    
                                                    HtmlTableCell tdsub = new HtmlTableCell("th");
                                                    tdsub.Attributes.Add("Title", column.ToolTip);
                                                    tdsub.InnerText = column.Text;
                                                    tdsub.BgColor = "#E1EFF7";
                                                    tdsub.Width = column.Width;
                                                    trSub.Cells.Add(tdsub);
                                                }

                                            }
                                            tbSubSubList.Rows.Add(trSub);
                                            foreach (ObjRow rowsub in subsubList.RowList)
                                            {
                                                HtmlTableRow trcontentsub = new HtmlTableRow();
                                                foreach (ObjColumn column in subsubList.ColumnList[0].ColumnList)
                                                {
                                                    if (column.IsVisible)
                                                    {
                                                        HtmlTableCell tdcontent = new HtmlTableCell();
                                                        tdcontent.InnerText = rowsub.GetObjFieldValue(column.Name);
                                                        tdcontent.BgColor = "white";
                                                        trcontentsub.Cells.Add(tdcontent);
                                                    }

                                                }

                                                tbSubSubList.Rows.Add(trcontentsub);
                                            }
                                            HtmlTableRow trsubsub = new HtmlTableRow();
                                            trsubsub.ID = "tr" + i;
                                            trsubsub.Style.Add("display", "none");
                                            HtmlTableCell tdsubsub = CreateTD(1, "100%", null, null);
                                            tdsubsub.BgColor = "#dddddd";
                                            tdsubsub.ColSpan = subList.ColumnList[0].ColumnList.Count(item => item.IsVisible);
                                            tdsubsub.Controls.Add(tbSubSubList);
                                            trsubsub.Cells.Add(tdsubsub);
                                            tbSubList.Rows.Add(trsubsub);
                                        }

                                    }
                                    #endregion
                                    if (hassub == false)
                                    {
                                        //tbSubList.Rows[0].Cells[0].Visible = false;
                                        //trcontent.Cells[0].Visible = false;
                                    }
                                    i++;
                                }

                                HtmlTableRow tra = new HtmlTableRow();
                                HtmlTableCell tda = CreateTD(1, "100%", null, null);
                                tda.Attributes.Add("style", "margin:0px;padding:3px 0px 1px 0px;");
                                //添加合计
                                if (isSum)
                                {
                                    tbSubList.Rows.Add(sumRow);
                                }
                                tda.Controls.Add(tbSubList);
                                tra.Cells.Add(tda);
                                TableCtrl.Rows.Add(tra);

                            }

                        }

                    }





                    dgID++;
                }
            }

        }


        /// <summary>
        /// 根据附件列表数据创建控件，设置样式，并添加至table
        /// </summary>
        /// <param name="objAttachList">附件列表</param>
        public void CreateAttachList(ObjAttachList[] objAttachList)
        {
            HtmlTable tb = CreateTable(0, 0, 0, "100%", null, null);
            int cols = 0;
            if (null != objAttachList && objAttachList.Length > 0)
            {
                foreach (ObjAttachList objAttach in objAttachList)
                {
                    { //创建显示标题
                        if (objAttach.AttachList != null && objAttach.AttachList.Length > 0)
                            TableCtrl.Controls.Add(CreateTop("附件"));
                    }
                    HtmlTableRow tr = new HtmlTableRow();
                    foreach (ObjAttach obj in objAttach.AttachList)
                    {
                        string name = obj.Name;
                        string url = obj.Url;
                        //三列自动换行
                        if ((cols % 3) == 0)
                        {
                            tr = new HtmlTableRow();
                            tr.Height = "20px";
                            tb.Controls.Add(tr);
                        }
                       
                        HtmlTableCell td = CreateTD(1, null, null, "billLink");

                        //创建链接
                        HtmlAnchor anchor = new HtmlAnchor();
                        anchor.HRef = url;
                        anchor.Target = "_bank";
                        anchor.InnerHtml = name;

                        td.Controls.Add(anchor);
                        tr.Controls.Add(td);
                        cols++;
                    }
                    
                }

            }

            TableCtrl.Rows.Add(AddTable(tb));
        }


        /// <summary>
        /// 根据审核列表创建控件，设置样式，并添加至table
        /// </summary>
        /// <param name="objApprovalList">审核列表</param>
        public void CreateApprovalList(ObjApprovalList[] objApprovalList)
        {
            foreach (ObjApprovalList oaList in objApprovalList)
            {
                HtmlTable tb = CreateTable(0, 0, 0, "100%", null, null);

                if (null != objApprovalList && objApprovalList.Length > 0)
                {

                    foreach (ObjApprovalList objApproval in objApprovalList)
                    {
                        {//创建显示头部
                            if (null != objApproval.ApprovalList && objApproval.ApprovalList.Length > 0)
                            {
                                HtmlTableRow tr1 = CreateTop(oaList.Text);
                                tr1.Cells[0].ID = "tdApproveTitle";
                                //tr1.Cells[0].Attributes.Add("style", "display:none");
                                TableCtrl.Controls.Add(tr1);
                            }
                            else
                            {
                                HtmlTableRow tr1 = CreateTop(oaList.Text);
                                tr1.Cells[0].ID = "tdApproveTitle";
                                tr1.Cells[0].Attributes.Add("style", "display:none");
                                TableCtrl.Controls.Add(tr1);
                            }

                        }

                        foreach (ObjApproval obj in objApproval.ApprovalList)
                        {
                            string Approver = obj.Approver;
                            string ApprovalRemark = obj.ApprovalRemark;
                            //DK:日期统一格式
                            string ApprovalTime = obj.ApprovalTime.ToString("yyyy-MM-dd HH:mm");
                            string ApprovalState = obj.ApprovalState;
                            string flag = obj.Flag;
                            HtmlTableRow tr1 = new HtmlTableRow();

                            HtmlTableCell td1 = CreateTD(1, null, null, "billAuditPerson");
                            if (flag == "0")
                            {
                                td1.InnerText = "提单人";
                            }
                            else
                            {
                                td1.InnerText = "审核人";
                            }


                            HtmlTableCell td2 = CreateTD(1, "2%", null, null);

                            HtmlTableCell td3 = CreateTD(1, null, null, "billAuditName");
                            td3.InnerText = Approver;

                            HtmlTableCell td4 = CreateTD(1, null, null, "billAuditTime");
                            td4.InnerText = ApprovalTime;
                            td4.Width = "40%";
                            td4.Align = "center";
                            tr1.Controls.Add(td1);
                            tr1.Controls.Add(td2);
                            tr1.Controls.Add(td3);
                            tr1.Controls.Add(td4);
                            tb.Controls.Add(tr1);

                            if (flag == "1")
                            {
                                HtmlTableRow tr2 = new HtmlTableRow();
                                HtmlTableCell tdTwo1 = CreateTD(1, null, null, "billAuditPerson");
                                tdTwo1.InnerText = "审核结果";

                                HtmlTableCell tdTwo2 = CreateTD(1, "2%", null, null);

                                HtmlTableCell tdTwo3 = CreateTD(2, "80%", null, null);
                                tdTwo3.Align = "left";
                                tdTwo3.ColSpan = 2;
                                tdTwo3.InnerText = ApprovalState;

                                tr2.Controls.Add(tdTwo1);
                                tr2.Controls.Add(tdTwo2);
                                tr2.Controls.Add(tdTwo3);
                                tb.Controls.Add(tr2);

                                HtmlTableRow tr3 = new HtmlTableRow();

                                HtmlTableCell tdThree1 = CreateTD(1, null, null, "billAuditPerson");
                                tdThree1.InnerText = "审核意见";

                                HtmlTableCell tdThree2 = CreateTD(1, "2%", null, null);

                                HtmlTableCell tdThree3 = CreateTD(2, "80%", null, "smt_autobreak");
                                tdThree3.Align = "left";
                                tdThree3.InnerText = ApprovalRemark;
                                tdThree3.ColSpan = 2;
                                tr3.Controls.Add(tdThree1);
                                tr3.Controls.Add(tdThree2);
                                tr3.Controls.Add(tdThree3);
                                tb.Controls.Add(tr3);
                            }


                            tb.Controls.Add(CreateBottomLine());
                        }
                    }

                }
                tb.ID = "tbApprove";
                HtmlTableRow tr = AddTable(tb);
                tr.Cells[0].ID = "tdApprove";
                TableCtrl.Rows.Add(tr);
            }

        }

        public HtmlTable CreateConsultationList(ObjConsultationList[] objConsultationList)
        {
            HtmlTable tb = CreateTable(0, 0, 0, "100%", null, null);

            if (null != objConsultationList && objConsultationList.Length > 0)
            {
                //HtmlTableRow trHeader = new HtmlTableRow();
                //HtmlTableCell tdHeader = CreateTD(3, null, null, "billTopTd");
                //tdHeader.InnerText = "";
                //trHeader.Controls.Add(tdHeader);
                //tb.Controls.Add(trHeader);

                foreach (ObjConsultationList objConsultation in objConsultationList)
                {
                    string ConsultationUserName = objConsultation.ConsultationUserName;
                    string ConsultationDate = objConsultation.ConsultationDate.ToString("yyyy-MM-dd HH:mm");
                    string Content = objConsultation.Content;

                    string ReplyContent = objConsultation.ReplyContent;
                    string ReplyDate = objConsultation.ReplyDate.ToString("yyyy-MM-dd HH:mm");
                    string ReplyUserName = objConsultation.ReplyUserName;

                    HtmlTableRow tr1 = new HtmlTableRow();
                    HtmlTableCell tdOne1 = CreateTD(1, null, null, "billAuditPerson");
                    tdOne1.InnerText = "咨询时间";

                    HtmlTableCell tdOne2 = CreateTD(1, "2%", null, null);

                    HtmlTableCell tdOne3 = CreateTD(1, "80%", null, null);
                    tdOne3.Align = "left";
                    tdOne3.InnerText = ConsultationDate;

                    tr1.Controls.Add(tdOne1);
                    tr1.Controls.Add(tdOne2);
                    tr1.Controls.Add(tdOne3);
                    tb.Controls.Add(tr1);

                    HtmlTableRow tr2 = new HtmlTableRow();
                    HtmlTableCell tdTwo1 = CreateTD(1, null, null, "billAuditPerson");
                    tdTwo1.InnerText = "咨询人";

                    HtmlTableCell tdTwo2 = CreateTD(1, "2%", null, null);

                    HtmlTableCell tdTwo3 = CreateTD(1, "80%", null, null);
                    tdTwo3.Align = "left";
                    tdTwo3.InnerText = ConsultationUserName;

                    tr2.Controls.Add(tdTwo1);
                    tr2.Controls.Add(tdTwo2);
                    tr2.Controls.Add(tdTwo3);
                    tb.Controls.Add(tr2);

                    HtmlTableRow tr3 = new HtmlTableRow();
                    HtmlTableCell tdThree1 = CreateTD(1, null, null, "billAuditPerson");
                    tdThree1.InnerText = "咨询内容";

                    HtmlTableCell tdThree2 = CreateTD(1, "2%", null, null);

                    HtmlTableCell tdThree3 = CreateTD(1, "80%", null, null);
                    tdThree3.Align = "left";
                    tdThree3.InnerText = Content;

                    tr3.Controls.Add(tdThree1);
                    tr3.Controls.Add(tdThree2);
                    tr3.Controls.Add(tdThree3);
                    tb.Controls.Add(tr3);

                    HtmlTableRow tr4 = new HtmlTableRow();
                    HtmlTableCell tdFour1 = CreateTD(1, null, null, "billAuditPerson");
                    tdFour1.InnerText = "回复时间";

                    HtmlTableCell tdFour2 = CreateTD(1, "2%", null, null);

                    HtmlTableCell tdFour3 = CreateTD(1, "80%", null, null);
                    tdFour3.Align = "left";
                    tdFour3.InnerText = ReplyDate;

                    tr4.Controls.Add(tdFour1);
                    tr4.Controls.Add(tdFour2);
                    tr4.Controls.Add(tdFour3);
                    tb.Controls.Add(CreateBottomLine());
                    tb.Controls.Add(tr4);


                    HtmlTableRow tr5 = new HtmlTableRow();
                    HtmlTableCell tdFive1 = CreateTD(1, null, null, "billAuditPerson");
                    tdFive1.InnerText = "回复人";

                    HtmlTableCell tdFive2 = CreateTD(1, "2%", null, null);

                    HtmlTableCell tdFive3 = CreateTD(1, "80%", null, null);
                    tdFive3.Align = "left";
                    tdFive3.InnerText = ReplyUserName;

                    tr5.Controls.Add(tdFive1);
                    tr5.Controls.Add(tdFive2);
                    tr5.Controls.Add(tdFive3);
                    tb.Controls.Add(tr5);

                    HtmlTableRow tr6 = new HtmlTableRow();
                    HtmlTableCell tdSix1 = CreateTD(1, null, null, "billAuditPerson");
                    tdSix1.InnerText = "回复内容";

                    HtmlTableCell tdSix2 = CreateTD(1, "2%", null, null);

                    HtmlTableCell tdSix3 = CreateTD(1, "80%", null, null);
                    tdSix3.Align = "left";
                    tdSix3.InnerText = ReplyContent;

                    tr6.Controls.Add(tdSix1);
                    tr6.Controls.Add(tdSix2);
                    tr6.Controls.Add(tdSix3);
                    tb.Controls.Add(tr6);
                }

                tb.Controls.Add(CreateBottomLine());
            }

            tb.ID = "tbConsultationList";
            HtmlTableRow tr = AddTable(tb);
            tr.Cells[0].ID = "tdConsultationList";
            TableCtrl.Rows.Add(tr);

            return tb;
        }


        /// <summary>
        /// 创建一个TD
        /// </summary>
        /// <param name="colspan">占几列</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="cssName">样式名称</param>
        /// <returns>反应TD</returns>
        private HtmlTableCell CreateTD(int colspan, string width, string height, string cssName)
        {
            HtmlTableCell td = new HtmlTableCell();

            if (colspan > 0)
                td.ColSpan = colspan;
            if (!string.IsNullOrEmpty(width))
                td.Width = width;
            if (!string.IsNullOrEmpty(height))
                td.Height = height;
            if (!string.IsNullOrEmpty(cssName))
                td.Attributes.Add("class", cssName);

            return td;
        }

        /// <summary>
        /// 创建TR
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="cssName">样式名称</param>
        /// <returns>TR</returns>
        private HtmlTableRow CreateTR(string width, string height, string cssName)
        {
            HtmlTableRow tr = new HtmlTableRow();

            if (!string.IsNullOrEmpty(width))
                tr.Attributes.Add("width", width);
            if (!string.IsNullOrEmpty(height))
                tr.Height = height;
            if (!string.IsNullOrEmpty(cssName))
                tr.Attributes.Add("class", cssName);

            return tr;
        }

        /// <summary>
        /// 创建一个Table
        /// </summary>
        /// <param name="cellPadding">单元格边框与内容距离</param>
        /// <param name="cellSpacing">单元格之间距离</param>
        /// <param name="border">边框粗细</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="cssName">样式名</param>
        /// <returns></returns>
        private HtmlTable CreateTable(int cellPadding,
            int cellSpacing, int border, string width, string height, string cssName)
        {
            HtmlTable table = new HtmlTable();

            table.CellPadding = cellPadding;
            table.CellSpacing = cellSpacing;
            table.Border = border;

            if (!string.IsNullOrEmpty(width))
                table.Width = width;
            if (!string.IsNullOrEmpty(height))
                table.Height = height;
            if (!string.IsNullOrEmpty(cssName))
                table.Attributes.Add("class", cssName);

            return table;
        }

        /// <summary>
        /// 将一个table添加至TR
        /// </summary>
        /// <param name="tb">要添加的table</param>
        /// <returns>TR</returns>
        private HtmlTableRow AddTable(HtmlTable tb)
        {
            HtmlTableRow tr = new HtmlTableRow();

            HtmlTableCell td = new HtmlTableCell();
            td.Attributes.Add("style", "padding:0px;margin:0px;");
            td.Controls.Add(tb);
            tr.Cells.Add(td);
            return tr;
        }

        /// <summary>
        /// 创建显示标题
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private HtmlTableRow CreateTop(string text)
        {
            //                   <div class="tabpanelheaddiv"><span style="height:30px;
            //line-height:30px;">审核信息</span></div>



            HtmlTableRow tr = new HtmlTableRow();
            HtmlTableCell td = CreateTD(1, null, null, "billTopTd");

            Label label = new Label();
            label.Text = text;
            td.Controls.Add(label);

            tr.Controls.Add(td);
            return tr;
        }

        /// <summary>
        /// 创建底部显示
        /// </summary>
        /// <returns></returns>
        private HtmlTableRow CreateBottomLine()
        {
            HtmlTableRow tr = CreateTR(null, "1px", null);
            HtmlTableCell td = CreateTD(4, null, null, "billBottomLine");

            tr.Controls.Add(td);
            return tr;
        }
    }
}