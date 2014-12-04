using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMT.Portal.Common.SmtForm.Framework;
using SMT.Portal.SmtForm;

namespace SMT.Portal.Common.SmtForm
{
    public partial class AjaxForm :PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string MessageID = Request["MessageID"]+"";
            string IsApproval = Request["IsApproval"] + "";
            string error = string.Empty;

            GetFormDetail detail = new GetFormDetail();
            try
            {
                detail.GetForm(MessageID, ref this.pnl1, IsApproval.ToLower() == "true" ? true : false, ref error);
            }
            catch
            {
                Label label = new Label();
                label.Text = error;
                this.pnl1.Controls.Add(label);
            }
        }
    }
}