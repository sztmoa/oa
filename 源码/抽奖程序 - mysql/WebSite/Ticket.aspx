<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="Ticket.aspx.cs" Inherits="Asd.Award.Ticket" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        
    <label>开始号码：</label>
    <asp:TextBox ID="txtSZ" runat="server"></asp:TextBox>
    <label>结束号码：</label>
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        
    <asp:Button ID="btnsz" runat="server" onclick="btnsz_Click" Text="修改总人数" />
    </div>
<%--    <asp:Button ID="btnbj" runat="server" Text="修改北京总人数" onclick="btnbj_Click" />
    <asp:TextBox ID="txtBJ" runat="server"></asp:TextBox>--%>
  
</asp:Content>
