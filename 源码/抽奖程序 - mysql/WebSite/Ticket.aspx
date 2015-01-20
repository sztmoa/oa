<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ticket.aspx.cs" Inherits="Asd.Award.Ticket" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Button ID="btnsz" runat="server" onclick="btnsz_Click" Text="修改总人数" />
    <asp:TextBox ID="txtSZ" runat="server"></asp:TextBox>
    <br />
<%--    <asp:Button ID="btnbj" runat="server" Text="修改北京总人数" onclick="btnbj_Click" />
    <asp:TextBox ID="txtBJ" runat="server"></asp:TextBox>--%>
  
</asp:Content>
