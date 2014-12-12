<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ServiceTest.aspx.cs" Inherits="SMT.FBAnalysis.Service.ServiceTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="Label1" runat="server" Text ="扩展单 ID 号：" /><asp:TextBox ID="txtExtensionalOrderID" runat="server" />
    <asp:Button ID="btnGetChargeApplyMasterCode" runat="server" Text="获取" 
            onclick="btnGetChargeApplyMasterCode_Click" />
    <asp:Label ID="lblChargeApplyMasterCode" runat="server" />
    </div>
    </form>
</body>
</html>
