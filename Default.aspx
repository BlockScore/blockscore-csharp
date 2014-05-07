<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TestBlockScore.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="StyleSheet1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">

    <center>
    <div>
    
    Sample integrations to the Blockscore API.  
    <br />
    
    <br />Click one of the buttons below to run a sample verification:
    <br />

    
        <asp:Button ID=btnverifyus runat=server Text="Verify US" 
            onclick="btnverifyus_Click" />&nbsp;
        <asp:Button ID=btnverifyinternational runat=server Text="Verify International" 
            onclick="btnverifyinternational_Click"/>

    <br />
    <br />
    <asp:Panel ID=pnlverifications runat=server Visible=false>
    API Call /verifications
    <table style="width:800px; vertical-align:top; border:1px solid gray;">
        <tr>
            <td>/verifications Request:</td>
            <td>/verifications Response:</td>
        </tr>
        <tr valign=top>
            <td><asp:Label ID=lblverificationrequest runat=server></asp:Label></td>
            <td><asp:Label ID=lblverificationresponse runat=server></asp:Label></td>
        </tr>
    </table>    
    </asp:Panel>

    <asp:Panel id=pnlquestions runat=server Visible=false>
    <br />
    <br />
    API Call /questions
    <table style="width:800px; vertical-align:top; border:1px solid gray;">
        <tr>
            <td>/questions Request:</td>
            <td>/questions Response:</td>
        </tr>
        <tr valign=top>
            <td><asp:Label ID=lblquestionrequest runat=server></asp:Label></td>
            <td><asp:Label ID=lblquestionresponse runat=server></asp:Label></td>
        </tr>
    </table>    
    
    <br />
    <br />
    API Call /questions/score
    <table style="width:800px; vertical-align:top; border:1px solid gray;">
        <tr>
            <td>/questions/score Request:</td>
            <td>/questions/score Response:</td>
        </tr>
        <tr valign=top>
            <td><asp:Label ID=lblquestionscorerequest runat=server></asp:Label></td>
            <td><asp:Label ID=lblquestionscoreresponse runat=server></asp:Label></td>
        </tr>
    </table>    
    </asp:Panel>

    </div>
    </center>
    </form>
</body>
</html>
