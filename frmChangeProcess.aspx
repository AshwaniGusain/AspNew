<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="frmChangeProcess.aspx.cs"
    Inherits="Contrast_Web.frmChangeProcess" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">


</script>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Delivery Details</title>
    <%--<link href="Stylesheet/Stylesheet.css" type="text/css" rel="Stylesheet" />--%>
    <style type="text/css">
        .PanelText
        {
            font-family: Verdana,Arial;
            font-size: 13px;
            font-weight: bold;
        }
        .LabelText
        {
            font-family: Verdana,Arial;
            font-size: 11px;
            font-weight: normal;
        }
        .LabelText a:hover
        {
            font-family: Verdana,Arial;
            font-size: 11px;
            font-weight: normal;
        }
        .LabelText a
        {
            font-family: Verdana,Arial;
            font-size: 11px;
            font-weight: normal;
        }
        .scroll_Grid
        {
            width: 100%;
            height: 800px;
            padding: 2px;
            overflow: auto;
            background: white;
            border: 1px solid #ccc;
        }
        .scroll_panel
        {
            width: 100%;
            padding: 2px;
            overflow: auto;
            background: white;
            border: 1px solid #ccc;
            text-align: center;
        }
        .gvwHeader
        {
            border: 1px solid black;
            background-color: #294a77; /*color:Blue; HeaderStyle-ForeColor="White" HeaderStyle-BorderColor="Red" HeaderStyle-BorderWidth="1px" HeaderStyle-Font-Bold="false" HeaderStyle-Font-Size="Smaller"*/
        }
        .gvwHeader th
        {
            border: 1px solid black;
            font-size: 11px;
        }
        .gvwHeader th a
        {
            font-size: 11px;
        }
        .popup_window
        {
            position: absolute;
            width: 500px;
            height: 400px;
            border: 5px solid #333;
            background: #FFF;
            display: none;
            left: 50%;
            margin: 0px 0 0 -250px;
        }
        .popup_content
        {
            height: 300px;
            overflow: auto;
            font-size: 12px;
            font-family: Verdana,Arial;
            padding: 14px;
        }
        .btnClose
        {
            text-align: center;
        }
        .popup_header
        {
            background: #294a77;
            padding: 8px;
            color: #FFF;
            font-family: Verdana,Arial;
            font-size: 11px;
        }
        .wrapper
        {
            position: relative;
        }
        .updateProgress
        {
            font-size: 35px;
            position:absolute;
            top: 25%;
            left: 50%;
            margin-left: -60px;
            margin-right: -20px;
            text-decoration: underline;
            -moz-text-decoration-color: red; /* Code for Firefox */
            text-decoration-top:20%;
            
        }
         .modal
        {
            position: fixed;
          z-index: 999;
          height: 100%;
          width: 100%;
          top: 0;
          background-color: Black;
          filter: alpha(opacity=30);
          opacity: 0.8;
         -moz-opacity: 0.2;
        }
        .style1
        {
            width: 184px;
        }
        .style2
        {
            width: 92px;
        }
    </style>

    <script language="javascript" type="text/javascript">
        function close_popup(){
            document.getElementById("popup_window").style.display = "none";   
        }
        
        function Display_log(FileName, isError) {            
            try{
            var log_file_link =FileName;// "\\\\Win7159\\s280_dd\\Log\\MRW_" + Journal + "_" + Article.trim() + "_" + OrderId + ".txt"; 
                alert(log_file_link); 
                alert(isError);
                if (isError >= 0) {
                    alert("No Error");
                    return;
                }  
                var output = '';                      
                var fso = new ActiveXObject("Scripting.FileSystemObject");
                          
                var fh = fso.OpenTextFile(log_file_link, 1, false, 0);                       
                while(fh.AtEndOfStream==false){               
                    output += fh.ReadLine() + "<br>";             
                }     
                alert(output);                     
                document.getElementById("popup_window").style.display = "block";
                document.getElementById("popup_content").innerHTML = output;
                return false;    
            }
            catch(err){
            alert(err);
                alert("Log file not found");
            }                        
                        
            /*var args = new Object; args.window = window;
            var returnValue = "";

            returnValue = showModalDialog('Log_file.aspx?OrderId=' + OrderId, null, 'status:no;dialogWidth:500px;dialogHeight:400px;dialogHide:true;help:no;', args);
            return*/
        }                      
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <br />
    <%--<h2>
        Delivery Details</h2>
    <hr />--%>
    <div id="popup_window" class="popup_window">
        <div class="popup_header">
            Log File
        </div>
        <div id="popup_content" class="popup_content">
        </div>
        <center>
            <input type="button" value="Close" onclick="close_popup()" /></center>
    </div>
    <div>
        <asp:Panel ID="Panel2" GroupingText="Search Details" CssClass="PanelText" runat="server"
            Style="width: 90%" text-align="center">
            <table width="100%">
                <tr>
                    <td class="LabelText" align="right">
                        Stage:
                    </td>
                    <td align="left">
                        <asp:DropDownList ID="ddlStage" runat="server" Width="150px" class="LabelText">
                            <asp:ListItem Text="ALL" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td class="LabelText" align="right">
                        Process:
                    </td>
                    <td align="left" class="style1">
                        <asp:DropDownList ID="ddlProcess" runat="server" Width="150px" class="LabelText">
                            <asp:ListItem Text="ALL" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td align="left" class="style2">
                        <asp:CheckBox ID="chkFTP" runat="server" AutoPostBack="True" 
                            oncheckedchanged="chkFTP_CheckedChanged" Text="IsFTP" />
                    </td>
                    <td>
                        <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Panel ID="Panel3" GroupingText="Delivery Details" CssClass="PanelText" runat="server">
            <div class="scroll_Grid" id="grid_view">
                <%--<table width="100%">
            <tr>
                <td>--%>
                <div style="padding: 4px">
                    <asp:LinkButton ID="Export_Link" runat="server" OnClick="Export_Link_Click">Export to Excel</asp:LinkButton>
                </div>
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:GridView ID="GVChangeProcess" runat="server" AllowSorting="true" 
                    AutoGenerateColumns="False" 
                    DataKeyNames="ID,ZipDestinationPath,ZipFileName,stagedisplayname,article,journal" 
                    EmptyDataRowStyle-BackColor="AliceBlue" EmptyDataText="No Records Found!!!" 
                    EnableTheming="false" HeaderStyle-CssClass="gvwHeader" 
                    HeaderStyle-ForeColor="White" onrowdatabound="GVChangeProcess_RowDataBound">
                    <EmptyDataRowStyle BackColor="AliceBlue" />
                    <Columns>
                        <asp:TemplateField HeaderText="SlNo." ItemStyle-CssClass="LabelText" 
                            ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Journal" HeaderText="Journal" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="Article" HeaderText="Article" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="OrderId" HeaderText="Order ID" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="OrderType" HeaderText="Order Type" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="VersionNo" HeaderText="Version Id" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ReceivedDate" 
                            DataFormatString="{0:dd-MMM-yyyy hh:mm}" HeaderText="Received Date" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="DueDate" DataFormatString="{0:dd-MMM-yyyy hh:m}" 
                            HeaderText="Due Date" ItemStyle-CssClass="LabelText" 
                            ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="StageDisplayName" HeaderText="Stage" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ProcessName" HeaderText="Status" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="DispatchDate" HeaderText="Dispatch Date" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="ZipFileName" HeaderText="FileName" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="Remarks" HeaderText="Remarks" 
                            ItemStyle-CssClass="LabelText" ItemStyle-HorizontalAlign="Left" 
                            ItemStyle-Width="20%" />
                        <asp:TemplateField HeaderText="Reinitiate">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkChangeProcess" runat="server" AutoPostBack="false" 
                                    OnCheckedChanged="chkChangeProcess_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Upload To FTP">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkupload" runat="server" Enabled="False" 
                                    oncheckedchanged="chkupload_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
               
                   
                <%--</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" />
                </td>
            </tr>
        </table>--%>
            </div>
            <div>
             <center>
             
             &nbsp;&nbsp;
             <asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
                 <ContentTemplate>
                 <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_Click" 
                         Visible="False" />
                 <asp:Button ID="btnupload" runat="server" onclick="btnupload_Click" 
                     Text="Upload" Visible="False" />
                     </ContentTemplate>
                     </asp:UpdatePanel>
                 <asp:Button ID="TestBTN" runat="server" onclick="TestBTN_Click" Text="Test" 
                     Visible="False" />
             </center>
            </div>
        </asp:Panel>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                <div class="modal">
        <div class="updateProgress">
            <img alt="" src="App_Themes/ajax-loader-arrows.gif" /> 
                  Uploading in Process...
        </div>
        </div>
        </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    </form>
</body>
</html>
