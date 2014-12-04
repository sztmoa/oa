<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecordDetailByForm.aspx.cs" Inherits="SMT.Portal.Common.SmtForm.RecordDetailByForm" %>
<script type="text/javascript">
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
</script>
<table id="tbl_global" class="maintable" border="0" cellpadding="0" cellspacing="0">
            <tr id="tbl_center" valign="top" runat="server">
                <td class="bg">
                    <table class="PageTable" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <div id="maintd">
                                    <asp:Panel ID="pnl1" runat="server">
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>