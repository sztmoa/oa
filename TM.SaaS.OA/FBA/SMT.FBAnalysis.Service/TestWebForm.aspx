<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestWebForm.aspx.cs" Inherits="SMT.FBAnalysis.Service.TerstWebForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <p>
            实体名：<asp:TextBox ID="txtEntityName" runat="server"></asp:TextBox><br />
            主键名：<asp:TextBox ID="txtEntityKeyName" runat="server"></asp:TextBox><br />
            主键值：<asp:TextBox ID="txtEntityKeyValue" runat="server"></asp:TextBox><br />
            审核状态：<asp:TextBox ID="txtCheckState" runat="server"></asp:TextBox><br />
            <asp:Button ID="btnTest" runat="server" OnClick="btnTest_Click" Text="测试手机引擎服务" /></p>
        <p>
            <asp:Button ID="btnShowUnRepayRd" runat="server" Enabled="false" Visible="false" OnClick="btnShowUnRepayRd_Click"
                Text="显示未还清借款单" /><span style="padding-left: 20px;"></span>
            <asp:Button ID="btnUpdateUnRepayRd" runat="server" Enabled="false" Visible="false" Text="更新借款单" OnClick="btnUpdateUnRepayRd_Click" />
        </p>
    </div>
    <p>
    </p>
    <div>
        <asp:Literal ID="Literal1" runat="server" Visible="false" >借款单据</asp:Literal>
        <asp:GridView ID="gvUnRepayList" runat="server" Visible="false" AutoGenerateColumns="False"
            OnRowCommand="gvUnRepayList_RowCommand" DataKeyNames="BORROWAPPLYDETAILID" 
            onrowdatabound="gvUnRepayList_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="查看">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbnShowRepayRd" runat="server"
                            CommandArgument='<%# ((GridViewRow)Container).RowIndex %>' 
                            CommandName="ShowRepay">检查还款</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="BORROWAPPLYDETAILID" HeaderText="借款单据明细ID" />
                <asp:BoundField DataField="BORROWAPPLYMASTERCODE" HeaderText="借款单据编号" />
                <asp:BoundField DataField="REPAYTYPENAME" HeaderText="借款类型" />
                <asp:BoundField DataField="SUBJECTNAME" HeaderText="预算科目" />
                <asp:BoundField DataField="CHARGETYPE" HeaderText="费用类型" />
                <asp:BoundField DataField="BORROWMONEY" HeaderText="借款金额" />
                <asp:BoundField DataField="UNREPAYMONEY" HeaderText="未还款金额" />
                <asp:BoundField DataField="UPDATEDATE" HeaderText="终审时间" />
            </Columns>
        </asp:GridView>
    </div>
    <p>
        <asp:Literal ID="Literal4" runat="server" Visible="false" ></asp:Literal>
    </p>
    <div>
        <asp:Literal ID="Literal2" runat="server" Visible="false" >还款单据</asp:Literal>
        <asp:GridView ID="gvRepayList" runat="server" Visible="false" AutoGenerateColumns="False" 
            onrowdatabound="gvRepayList_RowDataBound">
            <Columns>
                <asp:BoundField DataField="REPAYAPPLYDETAILID" HeaderText="还款单据明细ID"/>
                <asp:BoundField DataField="REPAYAPPLYCODE" HeaderText="还款单据编号" />
                <asp:BoundField DataField="ORDERTYPE" HeaderText="还款类型" />
                <asp:BoundField DataField="SUBJECTNAME" HeaderText="预算科目" />
                <asp:BoundField DataField="REPAYMONEY" HeaderText="还款金额" />
                <asp:BoundField DataField="CHARGEMONEY" HeaderText="报销金额" />
                <asp:BoundField DataField="CHECKSTATES" HeaderText="审核状态" />
                <asp:BoundField DataField="UPDATEDATE" HeaderText="终审时间" />
            </Columns>
        </asp:GridView>
    </div>  
    <p>
        <asp:Literal ID="Literal3" runat="server"></asp:Literal>
    </p>
    <p>
        <asp:Literal ID="Literal5" runat="server"></asp:Literal>
    </p>
    </form>
</body>
</html>
