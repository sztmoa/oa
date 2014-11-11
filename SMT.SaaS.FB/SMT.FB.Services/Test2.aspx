<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test2.aspx.cs" Inherits="SMT.FB.Services.Test2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        function xmlConent_onclick() {

        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Button ID="btnTest" runat="server" onclick="btnTest_Click" Text="测试" />
    <asp:Button ID="btnContenList" runat="server" onclick="btnContenList_Click" Text="列出连接" />
    </div>
    <div>
    <asp:Label Text="预算日期 : " style="width: 79px"  runat="server"/>
    <asp:TextBox ID="txtMonth" runat="server" Text="2012-01-01" />
    <asp:Label Text="部门ID : " style="width: 68px" runat="server" />
    
    <asp:TextBox ID="txtDeptID" runat="server" 
            Text="fb0b4d07-edff-4a43-967c-51402379484c" Width="277px" />
    <asp:Button ID="btnDeptDetail" runat="server" Text="列出部门预算" 
            onclick="btnDeptDetail_Click" />
    <asp:Label Text="[查部门明细的操作]" runat="server" />
    </div>
    
    <div>
        <asp:Label ID="listDetail" runat="server" />
    </div>
    <div>
    <asp:GridView ID="GridView1" runat="server" Height="245px" Width="759px" AutoGenerateColumns="true">
    </asp:GridView>    
    </div>
    <div>
    <div>
    <asp:TextBox ID="txtOrderID" runat="server" />
    <asp:TextBox ID="txtCheckState" runat="server" />
    <asp:Button ID="btnUpdateOrder" runat="server" onclick="btnUpdateOrder_Click" />
    </div>
    <div>
    <asp:Label ID="lbMsg" runat="server" />
    </div>
    </div>
    <div style="margin-top:20px;">
        <div>查单据生成的元数据</div>
        <div><span> 实体类型：</span><asp:DropDownList style="margin-left:20px;width:300px" ID="ddlOrderType" runat="server" /></div>
        
        <div><span>单据编号或ＩＤ：</span><span style="margin-left:20px;"><asp:TextBox type="text" runat="server"  id="orderIDForXmlQuery" style="width:607px" />
        </span><span style="margin-left:20px;">
            <asp:Button ID="btnGenXml" runat="server" 
                Text="生成元数据xml" Height="21px" onclick="btnGenXml_Click"  /></span>
                <div>
                    <textarea id="xmlConent" name="xmlConent" runat="server" 
                        style="overflow:scroll; height: 579px; width: 89%;"></textarea></div>
                </div>
                <textarea id="TextArea1" cols="20" name="S1" rows="2"></textarea>
    </div>
    </form>
</body>
</html>