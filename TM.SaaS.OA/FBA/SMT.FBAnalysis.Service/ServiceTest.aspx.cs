using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SMT.FBAnalysis.Service
{
    public partial class ServiceTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGetChargeApplyMasterCode_Click(object sender, EventArgs e)
        {
            OaInterface oaInterface = new OaInterface();
            this.lblChargeApplyMasterCode.Text = oaInterface.GetApplyOrderID(this.txtExtensionalOrderID.Text.Trim());
        }
    }
}