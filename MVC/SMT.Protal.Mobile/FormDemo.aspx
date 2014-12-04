<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormDemo.aspx.cs" Inherits="SMT.Portal.Common.SmtForm.FormDemo" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
   <script type="text/javascript" language="javascript" src="Scripts/Global.js"></script>
   <script type="text/javascript" language="javascript" src="Scripts/Ajax.js"></script>
    <link href="Styles/Template.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" language="javascript">
    var g = new global();
    function GetFormContentByPC(MessageID, IsApproval) {
        var xmlhttp = createxmlhttp();
        var xmlString = "MessageID=" + escape(MessageID) + "&IsApproval=" + escape(IsApproval);
        var url = "AjaxForm.aspx";
        g.returnajax(xmlhttp, xmlString, url, updatePage);
        function updatePage() {
            if (xmlhttp.readyState == 4) {
                if (xmlhttp.status == 200) {
                    var res = xmlhttp.responseText;
                    document.getElementById("tbform").innerHTML = res;
                    
                }
                else { res = "Server Error:" + xmlhttp.statusText; }
            }
        }
    }
</script>
</head>
<body onload="javascript:GetFormContentByPC(112453,true);">
<div id="tbform">
</div>
</body>
</html>
