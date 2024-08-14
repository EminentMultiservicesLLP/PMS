<%@ Page  Async="true" Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="PMS.Reports.ReportViewer" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
                  <%--<asp:Button ID="Button1" runat="server" Text="Print" BorderStyle="Solid" ToolTip="Print" OnClick="Button1_Click" />--%>  
        <asp:ScriptManager ID="ScriptManager1" runat="server" ScriptMode="Release"></asp:ScriptManager>
        <rsweb:ReportViewer id="ReportViewer1" runat="server" asyncrendering="true"  ShowPrintButton="true" sizetoreportcontent="True" width="99%"
         backcolor="White" font-names="Verdana" font-size="8pt" waitmessagefont-names="Verdana" waitmessagefont-size="14pt">

        </rsweb:ReportViewer> 
    </div>
    </form>
</body>
</html>
