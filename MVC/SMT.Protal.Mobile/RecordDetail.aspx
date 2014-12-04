<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordDetail.aspx.cs" Inherits="SMT.Mobile.Web.RecordDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, minimum-scale=1, maximum-scale=1" />
    <script type="text/javascript" language="javascript" src="Scripts/Global.js"></script>
    <script type="text/javascript" language="javascript" src="Scripts/Ajax.js"></script>
</head>
<script language="javascript" type="text/javascript">
    var uName;
    var g = new global();
    changewidth();
    showmaskdiv();
    document.getElementById("maskshow").innerHTML = '<div style="margin-top:50px;">       请稍等...&nbsp;&nbsp;&nbsp;&nbsp;<img src="<%= this.Template.ImageUrl%>loading.gif" /></div>    </div>';
    function changewidth() {
        document.getElementById("maskshow").style.left = ((document.body.offsetWidth - 250) / 2) + "px";
        document.getElementById("masktimeout").style.left = ((document.body.offsetWidth - 250) / 2) + "px";

        document.getElementById("maskbg").style.height = document.body.scrollHeight + "px";
        if (document.getElementById("tblbutton") != null) {
            document.getElementById("tblbutton").width = document.body.offsetWidth;
        }
    }
    function LoadContext() {
        if (document.getElementById("hdGuid") != null) {
            var hdGuid = document.getElementById("hdGuid").value;
            if (hdGuid != null) {
                var xmlhttp = createxmlhttp();

                var xmlString = "Guid=" + escape(hdGuid);
                var url = "Context.aspx";
                g.returnajax(xmlhttp, xmlString, url, updatePage);

                function updatePage() {
                    if (xmlhttp.readyState == 4) {
                        if (xmlhttp.status == 200) {
                            var res = xmlhttp.responseText;

                            document.getElementById("divContext").innerHTML = res;
                        }
                        else { res = "Server Error:" + xmlhttp.statusText; }
                    }
                }
            }
        }
    }
    function showsub(name, imgname, tdname) {
        var obj = document.getElementById(name);
        var objimg = document.getElementById(imgname);
        var objtd = document.getElementById(tdname);
        if (obj.style.display == 'none') {
            obj.style.display = '';
            objtd.rowSpan = 2;
            objimg.src = "<%=this.Template.ImageUrl %>min.gif";
        }
        else {
            obj.style.display = 'none';
            objtd.rowSpan = 1;
            objimg.src = "<%=this.Template.ImageUrl %>plus.gif";
        }
    }
    window.onresize = function () { changewidth() };
    function doBack() {
        document.location = "<%=this.Url %>";
    }

    function showDetail(id) {

        document.getElementById("maskbg").style.display = "block";
        var obj = document.getElementById(id);
        document.getElementById("maskView").style.display = "block";
        document.getElementById("maskView").innerHTML = obj.innerHTML;
        document.getElementById("maskView").style.top = ((window.innerHeight - document.getElementById("maskView").scrollHeight) / 2) + "px";
        document.getElementById("maskView").style.left = ((window.innerWidth - document.getElementById("maskView").scrollWidth) / 2) + "px";
    }
    function closeDetail() {
        document.getElementById("maskbg").style.display = "none";
        document.getElementById("maskView").style.display = "none";
        document.getElementById("maskView").innerHTML = "";
    }
</script>
<body onload="changewidth();">
    <div id="alldiv">
        <div id="maskbg">
        </div>
        <div id="maskshow">
            <div style="margin-top: 50px;">
                请稍等...&nbsp;&nbsp;&nbsp;&nbsp;<img src="<%= this.Template.ImageUrl%>loading.gif" /></div>
        </div>
        <%--        <div id="maskView">
            
        </div>--%>
        <div id="masktimeout">
            <div style="margin-top: 50px;">
                已超时，请重试<br />
                <input type="button" value="关闭" onclick="hidemaskdiv()" onmousemove="javascript:MousemoveAudit(this.id);"
                    onmouseout="javascript:MouseoutAudit(this.id);" id="btn_AuditNo" name="btn_AuditNo"
                    class="billAuditYes" /></div>
        </div>
        <form id="audit" runat="server">
        <input type="hidden" id="hdUserName" />
        <table id="tbl_global" class="maintable" border="0" cellpadding="0" cellspacing="0">
            <tr class="top">
                <td class="toptd">
                    <table border="0" style="width: 100%;" cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="topleft">
                                <a href="<%=this.Url %>">
                                    <img src="<%=this.Template.ImageUrl %>Back.png" alt="" class="topImage" /></a>
                            </td>
                            <td class="topFont">
                                我的单据
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="tbl_center" valign="top" runat="server">
                <td class="bg">
                    <table class="PageTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div id="maintd">
                                    <asp:Panel ID="pnl1" runat="server">
                                    </asp:Panel>
                                    <%=html %>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div id="divUser" style="display: none; position: absolute; z-index: 20; width: 100%;
            background-color: white;">
        </div>
        </form>
    </div>
</body>
</html>
