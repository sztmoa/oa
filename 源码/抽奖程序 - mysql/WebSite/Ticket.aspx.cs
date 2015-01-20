using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EFMysql;

namespace Asd.Award
{
    public partial class Ticket : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadData();
            }
        }

        private void loadData()
        {
            List<TmpTicket> data = new List<TmpTicket>();
            using (TMAwardEntities context = new TMAwardEntities())
            {
                data =(from ent in  context.TmpTicket
                          select ent).ToList();
            }
            var sz=data.FirstOrDefault(c => c.TicketNO == "深圳");
            if (sz != null)
            {
                txtSZ.Text = sz.TicketCount;
            }
            var bj=data.FirstOrDefault(c => c.TicketNO == "北京");
            if (bj != null)
            {
                //txtBJ.Text = bj.TicketCount;
            }
        }
        /// <summary>
        /// 修改深圳票数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnsz_Click(object sender, EventArgs e)
        {
            try
            {
                using (TMAwardEntities context = new TMAwardEntities())
                {
                    int ticketCount = Convert.ToInt32(txtSZ.Text);
                    var dt = (from ent in context.TmpTicket
                             where ent.TicketNO == "深圳"
                             select ent).FirstOrDefault();
                    if (dt == null)
                    {
                        dt = new TmpTicket();
                        dt.TicketCount = ticketCount.ToString();
                        dt.TicketNO = "深圳";
                        context.AddToTmpTicket(dt);
                        context.SaveChanges();
                    }
                    else
                    {
                        dt.TicketCount = ticketCount.ToString();
                        context.SaveChanges();
                    }
                }
                loadData();
            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('" + ex.Message +"')</script>");
            }
        }
        /// <summary>
        /// 修改北京票数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void btnbj_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (TMAwardEntities context = new TMAwardEntities())
        //        {
        //            int ticketCount = Convert.ToInt32(txtBJ.Text);
        //            var dt = (from ent in context.TmpTicket
        //                      where ent.TicketNO == "北京"
        //                      select ent).FirstOrDefault();
        //            if (dt == null)
        //            {
        //                dt = new TmpTicket();
        //                dt.TicketCount = ticketCount.ToString();
        //                dt.TicketNO = "北京";
        //                context.AddToTmpTicket(dt);
        //                context.SaveChanges();
        //            }
        //            else
        //            {
        //                dt.TicketCount = ticketCount.ToString();
        //                context.SaveChanges();
        //            }
        //        }
        //        loadData();
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write("<script>alert('" + ex.Message + "')</script>");
        //    }
        //}
    }
}