<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AjaxForm.aspx.cs" Inherits="SMT.Portal.Common.SmtForm.AjaxForm" %>
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
 <table class="smt_PageTable" cellpadding="0" cellspacing="0">
       <tr>
          <td>
             <div id="smt_maintd">
                <asp:Panel ID="pnl1" runat="server"></asp:Panel>
             </div>
          </td>
       </tr>
  </table>
