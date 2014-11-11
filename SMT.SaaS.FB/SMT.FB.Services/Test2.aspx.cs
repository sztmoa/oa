using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMT_FB_EFModel;
using SMT.FB.DAL;
using SMT.FB.BLL;

namespace SMT.FB.Services
{
    public partial class Test2 : System.Web.UI.Page
    {
        protected List<EntityInfo> EntityInfoList { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            return;
            FBService fbsv = new FBService();
            fbsv.CreatePersonMoneyAssignInfo("bac05c76-0f5b-40ae-b73b-8be541ed35ed", "24a358f9-8539-4faa-aee6-d5cbc8ea450d", "d9d1b478-5e29-435f-bf94-77afc9536d1d");
           
            //FBCommonService fbCommonService = new FBCommonService();
            //EntityInfoList = fbCommonService.GetEntityInfoList();
            
            //if (!IsPostBack)
            //{                
            //    this.ddlOrderType.DataTextField = "Type";
            //    this.ddlOrderType.DataValueField = "Type";
            //    this.ddlOrderType.DataSource = EntityInfoList;
            //    this.DataBind();

            //}
        }
        

        protected void btnTest_Click(object sender, EventArgs e)
        {
            FBCommonService s = new FBCommonService();
            QueryExpression qe = new QueryExpression() { QueryType = typeof(T_FB_COMPANYBUDGETAPPLYMASTER).Name };
            var data = s.QueryFBEntities(qe);
            this.GridView1.DataSource = data;
            DataBind();
        }

        protected void btnContenList_Click(object sender, EventArgs e)
        {
            string count = "0";// DBContext.contenxtCache.Count.ToString();
            this.listDetail.Text = string.Format("一共有: {0} 个", count);
        }

        protected void btnUpdateOrder_Click(object sender, EventArgs e)
        {
            FBService FBClient = new FBService();
            string strMsg = "成功!";
            // s.UpdateExtensionOrder("Travel", "e8577fe8-7fe6-4e5e-8697-2d00504069b1", "1", ref ms);
            var fbResult = FBClient.UpdateExtensionOrder("Travel", this.txtOrderID.Text,txtCheckState.Text, ref strMsg);
            this.lbMsg.Text = strMsg;
        }

        protected void btnDeptDetail_Click(object sender, EventArgs e)
        {
            // 预算月份
            //DateTime dtBudgetDate = DateTime.Parse();
            //string dataValue = dtBudgetDate.ToString("yyyy-MM-dd");
            QueryExpression qeMonth = QueryExpression.Equal("BUDGETARYMONTH", this.txtMonth.Text);
            qeMonth.QueryType = typeof(T_FB_DEPTBUDGETAPPLYDETAIL).Name;

            // 预算部门
            QueryExpression qeDept = QueryExpression.Equal("OWNERDEPARTMENTID", this.txtDeptID.Text);
            qeDept.QueryType = typeof(T_FB_DEPTBUDGETAPPLYDETAIL).Name;
            qeDept.RelatedExpression = qeMonth;

            FBCommonService fbService = new FBCommonService();

            var resultList = fbService.QueryFBEntities(qeDept);

            this.GridView1.DataSource = resultList.ToEntityList<T_FB_DEPTBUDGETAPPLYDETAIL>();
            DataBind();
        }


        protected void btnGenXml_Click(object sender, EventArgs e)
        {
         
            string typeName = "";
            string keyName = "";
            var orderID = this.orderIDForXmlQuery.Text.Split(':')[0];
            var entityInfo = EntityInfoList.First(item => item.Type == this.ddlOrderType.SelectedValue);
            typeName = entityInfo.Type;
            keyName = entityInfo.KeyName;
            var codeName = entityInfo.CodeName;
            string qName = codeName;
            Guid tempGuidID = Guid.Empty;
            
            if (Guid.TryParse(orderID, out tempGuidID))
            {
                qName = keyName;
            }

            QueryExpression qe = QueryExpression.Equal(qName, orderID);
            qe.QueryType = typeName;
            SubjectBLL sbll = new SubjectBLL();
            var data = qe.Query(sbll);
            var order = data.FirstOrDefault();
            if (order != null)
            {
                this.orderIDForXmlQuery.Text += ":" + order.GetValue(keyName);
                FBEntity dataFBEntity = sbll.GetFBEntityByEntityKey(order.EntityKey);
                FBEntity fbEntity = new FBEntity();
                VirtualAudit auditEntity = new VirtualAudit{ModelCode = typeName};
                fbEntity.Entity = auditEntity;
                fbEntity.ReferencedEntity.Add(new RelationOneEntity(){FBEntity = dataFBEntity});
                AuditBLL bll = new AuditBLL();
                xmlConent.Value = bll.GetAuditXmlForMobile(auditEntity.ModelCode, dataFBEntity);
            }
            else
            {
                Response.Write("没有可操作的数据");
            }
        }
        
    }
}