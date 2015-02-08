<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NumberSet.aspx.cs" Inherits="Asd.Award.NumberSet" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <label>开始号码：</label>
    <asp:TextBox ID="txtStartNumber" runat="server"></asp:TextBox>
    <label>结束号码：</label>
    <asp:TextBox ID="txtEndNumber" runat="server"></asp:TextBox>
        
    <label>总人数：</label>
    <label id="numberSum" runat="server"></label>
    <asp:Button ID="btnsz" runat="server" onclick="btnsz_Click" Text="修改总人数" />
    </div>
    </form>
</body>
</html>
