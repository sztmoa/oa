using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using EFMysql;

namespace Asd.Award
{
    public partial class AwardPrintSZ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                using (TMAwardEntities context = new TMAwardEntities())
                {
                    var dt = from ent in context.TmpAward
                             select ent;
                    this.bindingData(dt);
                }

                //this.table0.Visible = false;
                //this.table1.Visible = false;
                //this.table2.Visible = false;
                //this.table3.Visible = false;
                //this.table4.Visible = false;
                //this.table5.Visible = false;
            }
        }

        private void bindingData(IQueryable<TmpAward> dt)
        {
            foreach (var item in dt)
            {
                    switch (item.Level)
                    {
                        case "0":
                            BuildTableRow(this.table0, item);
                            break;
                        case "1":
                            BuildTableRow(this.table1, item);
                            break;
                        case "2":
                            BuildTableRow(this.table2, item);
                            break;
                        case "3":
                            BuildTableRow(this.table3, item);
                            break;
                        case "4":
                            BuildTableRow(this.table4, item);
                            break;
                        case "5":
                            BuildTableRow(this.table5, item);
                            break;
                    }
            }
        }

        public static void BuildTableRow(HtmlTable table, TmpAward item)
        {
            var tr = new HtmlTableRow();
            var cell = new HtmlTableCell();
            cell.InnerText = item.TicketNO;
            tr.Cells.Add(cell);
            table.Rows.Add(tr);
        }
    }
}