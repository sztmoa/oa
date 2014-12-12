using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SMT.FBAnalysis.BLL;
using SMT.FBAnalysis.DAL;

namespace SMT.FBAnalysis.Service
{
    public partial class TerstWebForm : System.Web.UI.Page
    {
        #region 借款单据更新专用变量
        string strOrcConn = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString();
        string strBorrowUpdate = "Update t_fb_borrowapplydetail t set t.unrepaymoney = {0} where t.borrowapplydetailid='{1}'";
        string strChageUpdate = "Update t_fb_chargeapplydetail t set t.repaymoney = {0} where t.chargeapplydetailid='{1}'";
        string strRepaySearch = "select * from repayorder t where t.checkstates = 2 and t.borrowapplydetailid = '{0}' order by t.borrowapplydetailid, t.updatedate";
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
          
        }

        #region 借款单据更新专用方法
        private void BindUnRepayRd()
        {
            string strSql = "select d.borrowapplydetailid, d.borrowapplymasterid, t.borrowapplymastercode,"
                + " (case t.repaytype when 1 then '普通借款' when 2 then '备用金借款' when 3 then '专项借款'"
                + " End) repaytypename, t.updatedate, s.subjectname, d.chargetype, d.usablemoney, d.borrowmoney,"
                + " d.unrepaymoney, d.ROWID from t_fb_borrowapplydetail d inner join t_fb_borrowapplymaster t"
                + " on d.borrowapplymasterid = t.borrowapplymasterid inner join t_fb_subject s on d.subjectid = s.subjectid "
                + " where t.checkstates = 2";

            BorrowApplyMasterDAL dalBorrow = new BorrowApplyMasterDAL();
            DataTable dt = dalBorrow.GetUnRepayRdList(strSql);
            if (dt == null)
            {
                return;
            }

            gvUnRepayList.DataSource = dt;
            gvUnRepayList.DataBind();
        }

        private void BindRepayRd(string strID)
        {
            string strSql = string.Format(string.Copy(strRepaySearch), strID);

            RepayApplyMasterDAL dalRepay = new RepayApplyMasterDAL();
            DataTable dtRepay = dalRepay.GetRepayRdList(strSql);
            if (dtRepay == null)
            {
                return;
            }

            gvRepayList.DataSource = dtRepay;
            gvRepayList.DataBind();
        }
        #endregion

        /// <summary>
        /// 测试手机审单服务接口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTest_Click(object sender, EventArgs e)
        {
            //停止使用，仅在需要测试时打开
            //if (btnTest.Enabled)
            //{
            //    return;
            //}
            DailyUpdateCheckStateService target = new DailyUpdateCheckStateService(); // TODO: 初始化为适当的值
            string strEntityName = txtEntityName.Text; // TODO: 初始化为适当的值
            string strEntityKeyName = txtEntityKeyName.Text; // TODO: 初始化为适当的值
            string strEntityKeyValue = txtEntityKeyValue.Text; // TODO: 初始化为适当的值
            string strCheckState = txtCheckState.Text; // TODO: 初始化为适当的值         

            if (string.IsNullOrWhiteSpace(strEntityName) || string.IsNullOrWhiteSpace(strEntityKeyName) || string.IsNullOrWhiteSpace(strEntityKeyValue) || string.IsNullOrWhiteSpace(strCheckState))
            {
                return;
            }

            int actual;
            actual = target.UpdateCheckState(strEntityName, strEntityKeyName, strEntityKeyValue, strCheckState);
        }

        #region 借款单据更新专用按钮事件
        protected void btnShowUnRepayRd_Click(object sender, EventArgs e)
        {
            BindUnRepayRd();
        }

        protected void btnUpdateUnRepayRd_Click(object sender, EventArgs e)
        {
            BorrowApplyMasterDAL dalBorrow = new BorrowApplyMasterDAL();
            RepayApplyMasterDAL dalRepay = new RepayApplyMasterDAL();
            ChargeApplyMasterDAL dalCharge = new ChargeApplyMasterDAL();

            foreach (GridViewRow row in gvUnRepayList.Rows)
            {
                string strId = Server.HtmlDecode(row.Cells[1].Text).ToString();
                string strBorrow = Server.HtmlDecode(row.Cells[6].Text).ToString();
                string strUnRepay = Server.HtmlDecode(row.Cells[7].Text).ToString();
                if (string.IsNullOrWhiteSpace(strId) || string.IsNullOrWhiteSpace(strBorrow) || string.IsNullOrWhiteSpace(strUnRepay))
                {
                    continue;
                }                

                string strSql_Repay = string.Format(string.Copy(strRepaySearch), strId);

                DataTable dtRepay = dalRepay.GetRepayRdList(strSql_Repay);
                if (dtRepay == null)
                {
                    continue;
                }                

                decimal dRepayMoney = 0;
                decimal dUpdateMoney = 0;
                decimal dBorrow = 0;
                decimal dUnRepay = 0;
                decimal.TryParse(strBorrow, out dBorrow);
                decimal.TryParse(strUnRepay, out dUnRepay);
                foreach (DataRow item in dtRepay.Rows)
                {
                    decimal dRepayTemp = 0, dChage = 0;
                    string strRepayId = string.Empty, strOrderType = string.Empty;

                    if (item["ORDERTYPE"] == null || item["REPAYMONEY"] == null || item["CHARGEMONEY"] == null)
                    {
                        continue;
                    }

                    strRepayId = item["REPAYAPPLYDETAILID"].ToString();
                    strOrderType = item["ORDERTYPE"].ToString();

                    if (strOrderType == "Repay")
                    {
                        decimal.TryParse(item["REPAYMONEY"].ToString(), out dRepayTemp);
                    }
                    else if (strOrderType == "Charge")
                    {
                        decimal.TryParse(item["REPAYMONEY"].ToString(), out dRepayTemp);
                        decimal.TryParse(item["CHARGEMONEY"].ToString(), out dChage);

                        decimal dTemp = dBorrow - dRepayMoney;
                        bool bUpdate = false;
                        if (dTemp >= dChage)
                        {
                            if (dRepayTemp < dChage)
                            {
                                dRepayTemp = dChage;
                                bUpdate = true;
                            }
                        }
                        else
                        {
                            if (dRepayTemp != dTemp)
                            {
                                dRepayTemp = dTemp;
                                bUpdate = true;
                            }
                        }

                        if (bUpdate)
                        {
                            string strUpdateChage = string.Format(string.Copy(strChageUpdate), dRepayTemp, strRepayId);
                            dalCharge.UpdateBySql(strUpdateChage);
                            Literal5.Text += "已更新的冲借款" + strRepayId + ";";
                        } 
                    }
                    dRepayMoney += dRepayTemp;
                }

                if (dRepayMoney == 0)
                {
                    Literal4.Text += "该借款" + strId + "不存在还款;";
                }

                if (dRepayMoney > dBorrow)
                {
                    dRepayMoney = dBorrow;
                    Literal4.Text += "还款超出借款" + strId + ";";
                }

                dUpdateMoney = dBorrow - dRepayMoney;
                if (dUpdateMoney == dUnRepay)
                {
                    continue;
                }
                string strUpdateBorrow = string.Format(string.Copy(strBorrowUpdate), dUpdateMoney, strId);
                dalBorrow.UpdateBySql(strUpdateBorrow);
                Literal3.Text += "已更新的借款" + strId + ";";
            }
        }

        protected void cbxUnRepayID_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbx = sender as CheckBox;
            if (!cbx.Checked)
            {
                return;
            }

            string strId = cbx.ToolTip;
            if (string.IsNullOrWhiteSpace(strId))
            {
                return;
            }

            BindRepayRd(strId);
        }

        protected void gvUnRepayList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowRepay")
            {
                int index = Convert.ToInt32(e.CommandArgument);

                GridViewRow row = gvUnRepayList.Rows[index];
                string strId = Server.HtmlDecode(row.Cells[1].Text).ToString();
                if (string.IsNullOrWhiteSpace(strId))
                {
                    return;
                }

                BindRepayRd(strId);
            }
        }

        protected void gvUnRepayList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Visible = false;
            }
        }

        protected void gvRepayList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
        }

        protected void gvRepayAndChargeList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Visible = false;
            }
        }
        #endregion
    }
}