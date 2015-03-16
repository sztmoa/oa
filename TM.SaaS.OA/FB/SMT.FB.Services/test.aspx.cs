using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TM_SaaS_OA_EFModel;
using SMT.FB.BLL;
using SMT.SaaS.BLLCommonServices.PersonnelWS;

namespace SMT.FB.Services
{
    public partial class test : System.Web.UI.Page
    {
        protected List<EntityInfo> EntityInfoList { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            //return;
            FBCommonService fbCommonService = new FBCommonService();
            EntityInfoList = fbCommonService.GetEntityInfoList();
            if (!IsPostBack)
            {

                this.ddlOrderType.DataTextField = "Type";
                this.ddlOrderType.DataValueField = "Type";
                this.ddlOrderType.DataSource = EntityInfoList;
                this.DataBind();

                BindMobileDpd(dpdMobileEntityList, "Type", "Type", EntityInfoList);
            }

           
         
        }

        /// <summary>
        /// 绑定DropDownList
        /// </summary>
        /// <param name="dpdMobileEntityList"></param>
        /// <param name="strDisplayText"></param>
        /// <param name="strBindValue"></param>
        /// <param name="EntityInfoList"></param>
        private void BindMobileDpd(DropDownList dpd, string strDisplayText, string strBindValue, List<EntityInfo> EntityInfoList)
        {
            if (EntityInfoList == null)
            {
                return;
            }

            if (EntityInfoList.Count() == 0)
            {
                return;
            }

            dpd.Items.Clear();

            dpd.DataTextField = strDisplayText;
            dpd.DataValueField = strBindValue;
            dpd.DataSource = EntityInfoList;
            dpd.DataBind();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {
            FBService fs = new FBService();
            string comID = this.TextBox1.Text;
            string deptID = this.TextBox2.Text;
            var data = fs.FetchSalaryBudget(comID, deptID);
            DataList1.DataSource = data;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string xml =  TextBox3.Text;
            xml = "<?xml version='1.0' encoding='utf-8' ?>" + xml;
            FBService fs = new FBService();
            fs.UpdateSalaryBudget(xml);

        }

        protected void btnGetTravelExpApplyMasterCode_Click(object sender, EventArgs e)
        {
            IOaService service = new OaServerce();
            this.lblExpApplyMasterCode.Text = service.GetTravelExpApplyMasterCode(this.txtExtensionalOrderID.Text.Trim());
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            AuditOrder(CheckStates.Approved);
        }

        private void AuditOrder(CheckStates cs)
        {
            string[] orderids = this.txtOrderID.Text.Split(',');
            orderids.ToList().ForEach(item =>
            {
                AuditOrder(item, cs);
            });
        }

        private void AuditOrder(string orderID, CheckStates cs)
        {
            string typeName = "";
            string keyName = "";

            var entityInfo = EntityInfoList.First(item => item.Type == this.ddlOrderType.SelectedValue);
            typeName = entityInfo.Type;
            keyName = entityInfo.KeyName;
            QueryExpression qe = QueryExpression.Equal(keyName, orderID);
            qe.QueryType = typeName;
            using (BudgetAccountBLL bll = new BudgetAccountBLL())
            {
                var data = qe.Query(bll);
                var order = data.FirstOrDefault();
                if (order != null)
                {
                   
                    try
                    {
                        bll.BeginTransaction();
                        bll.UpdateAccount(order.ToFBEntity(), cs);
                        bll.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        bll.RollbackTransaction();
                        SMT.Foundation.Log.Tracer.Error("手动审核异常：" + ex.ToString());
                        Response.Write(ex.ToString());
                    }
                    Response.Write("操作成功");
                }
                else
                {
                    Response.Write("没有可操作的数据");
                }


            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {
            AuditOrder(CheckStates.UnApproved);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            AuditOrder(CheckStates.Approving);
        }

        protected void Button5_Click(object sender, EventArgs e)
        {
            BaseBLL bll = new BaseBLL();
            var data = bll.InnerGetEntities<T_FB_CHARGEAPPLYMASTER>(new QueryExpression());
            var b = from item in data
                    orderby item.UPDATEDATE
                    select item;
            var c = from item in b
                    where item.TOTALMONEY > 0
                    select item;
            var d = c.Take(5).ToList();

            
        }

        protected void btnShow0_Click(object sender, EventArgs e)
        {
            FBService service = new FBService();
            service.CloseBudget();
        }

        /// <summary>
        /// 活动经费测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFunds_Click(object sender, EventArgs e)
        {
          
            //BudgetAccountBLL bll = new BudgetAccountBLL();
            //string strCompanyID = "bac05c76-0f5b-40ae-b73b-8be541ed35ed";
            //string strAssignOwnerID = "24a358f9-8539-4faa-aee6-d5cbc8ea450d";
            //bll.CreatePersonMoneyAssignInfo(strCompanyID, strAssignOwnerID);

            //FBService fb = new FBService();
            //T_HR_EMPLOYEEPOSTCHANGE personChange = new T_HR_EMPLOYEEPOSTCHANGE();
            //string str = string.Empty;
            //personChange.FROMPOSTID = "6631a391-dd20-4033-a769-71c3c6263801";
            //personChange.T_HR_EMPLOYEE = new T_HR_EMPLOYEE();
            //personChange.T_HR_EMPLOYEE.EMPLOYEEID = "df5acddf-d902-49e3-84f0-4be2a900c5f0";

            //personChange.TOCOMPANYID = "3cd50b8b-8288-465b-826f-58d1dbe43464";
            //personChange.TODEPARTMENTID = "df5acddf-d902-49e3-84f0-4be2a900c5f0";
            //personChange.TOPOSTID = "de006da5-ab43-4ee1-ac7a-f9d49dddb5c0";
            //fb.HRPersonPostChanged(personChange,ref str);

            //QueryExpression qe = new QueryExpression();
            //qe.QueryType = "T_FB_EXTENSIONALORDER";
            //qe.PropertyName = "ORDERID";
            //qe.PropertyValue = "692eea24-c4b2-4e82-af28-aab1d86b2e14";

            //QueryExpression a = QueryExpression.Equal("OWNERID", "1111");
            //QueryExpression b = QueryExpression.Equal("OWNERDEPARTMENTID", "2222");
            //b.RelatedExpression = a;
            //qe.RelatedExpression = b;
            //qe.Include = new string[] { ("T_FB_EXTENSIONORDERDETAIL") };
            //qe.Operation = QueryExpression.Operations.Equal;
            //FBService ss = new FBService();
            //ss.QueryFBEntities(qe);

        }
        /// <summary>
        /// 手机提交审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMobileApproving_Click(object sender, EventArgs e)
        {
            string strModelCode = string.Empty, orderID = string.Empty, strCheckStates = string.Empty, strMsg = string.Empty;
            int i = 0;
            
            var entityInfo = EntityInfoList.First(item => item.Type == dpdMobileEntityList.SelectedValue);
            strModelCode = entityInfo.Type;

            if (string.IsNullOrWhiteSpace(txtMobileEntityID.Text))
            {
                return;
            }

            orderID = txtMobileEntityID.Text;
            strCheckStates = ((int)CheckStates.Approving).ToString();

            FBService svc = new FBService();
            i = svc.UpdateCheckState(strModelCode, orderID, strCheckStates, ref strMsg);

            if (i < 0)
            {
                Response.Write("手机测试服务操作失败：错误消息为：" + strMsg);
                return;
            }
            Response.Write("手机测试服务操作成功");
        }

        /// <summary>
        /// 手机审核通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMobileApproved_Click(object sender, EventArgs e)
        {
            string strModelCode = string.Empty, orderIDs = string.Empty, strCheckStates = string.Empty, strMsg = string.Empty;
            int i = 0;

            var entityInfo = EntityInfoList.First(item => item.Type == dpdMobileEntityList.SelectedValue);
            strModelCode = entityInfo.Type;

            if (string.IsNullOrWhiteSpace(txtMobileEntityID.Text))
            {
                return;
            }

            orderIDs = txtMobileEntityID.Text;
            string[] allid=orderIDs.Split(',');

            foreach (string orderID in allid)
            {                
                strCheckStates = ((int)CheckStates.Approved).ToString();

                FBService svc = new FBService();
                i = svc.UpdateCheckState(strModelCode, orderID, strCheckStates, ref strMsg);

                if (i < 0)
                {
                    Response.Write("手机测试服务操作失败：错误消息为：" + strMsg + "\r\n");
                    return;
                }
                Response.Write(orderID + "手机测试服务操作成功");
            }
        }

        /// <summary>
        /// 手机审核不通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnMobileUnApproved_Click(object sender, EventArgs e)
        {
            string strModelCode = string.Empty, orderID = string.Empty, strCheckStates = string.Empty, strMsg = string.Empty;
            int i = 0;

            var entityInfo = EntityInfoList.First(item => item.Type == dpdMobileEntityList.SelectedValue);
            strModelCode = entityInfo.Type;

            if (string.IsNullOrWhiteSpace(txtMobileEntityID.Text))
            {
                return;
            }

            orderID = txtMobileEntityID.Text;
            strCheckStates = ((int)CheckStates.UnApproved).ToString();

            FBService svc = new FBService();
            i = svc.UpdateCheckState(strModelCode, orderID, strCheckStates, ref strMsg);

            if (i < 0)
            {
                Response.Write("手机测试服务操作失败：错误消息为：" + strMsg);
                return;
            }
            Response.Write("手机测试服务操作成功");
        }

        protected void testGetPersonMoneyAssign_Click(object sender, EventArgs e)
        {
            SMT.FB.BLL.BudgetAccountBLL bll = new BudgetAccountBLL();
            var masters = bll.GetPersonMoneyAssign(this.inputASSIGNCOMPANYID.Value, this.inputOWNERID.Value,"");

            List<FBEntity> resultList = new List<FBEntity>();
            if (masters != null)
            {
                FBEntity fbResult = masters.ToFBEntity();

                //处理岗位信息一栏
                List<T_FB_PERSONMONEYASSIGNDETAIL> rlist = masters.T_FB_PERSONMONEYASSIGNDETAIL.ToList();
                // rlist = obll.UpdatePostInfo(rlist);
                //按公司、部门排序
                rlist = rlist.OrderBy(c => c.OWNERCOMPANYID).ThenBy(c => c.OWNERDEPARTMENTID).ToList();

                fbResult.AddFBEntities<T_FB_PERSONMONEYASSIGNDETAIL>(rlist.ToFBEntityList());
                resultList.Add(fbResult);
                this.GridView1.DataSource = masters.T_FB_PERSONMONEYASSIGNDETAIL.ToList();
                this.DataBind();
            }
        }
        protected void testGetPersonMoneyAssignAA_Click(object sender, EventArgs e)
        {
            FBService s = new FBService();
            s.CreatePersonMoneyAssignInfo(this.inputASSIGNCOMPANYID.Value, this.inputOWNERID.Value, this.inputCreateID.Value);
        }
    }
}