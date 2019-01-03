<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Log_file.aspx.cs" Inherits="Contrast_Web.Log_file" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Erro Log</title>
    <style type="text/css">
        .LabelText
        {
            font-family: Verdana,Arial;
            font-size: 11px;
            font-weight: normal;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblLog" CssClass="LabelText" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
