<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="evoProjectConversion.aspx.vb" Inherits="crmWebClient.evoProjectConversion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
<form runat="server" >
The previous version of Evolution (www.jetnetevo.com) provided the ability to save search results as “Projects”.  The new version of Evolution that you are currently using saves these same search results as active “Folders”. The purpose of this page is to support users in the conversion of their previously stored “Projects” into “Folders”. Please note the following:
<ul> 
<li>Once a Project is converted to a Folder, the user may no longer make any changes to that Project in the previous version of Evolution.</li>
<li>Not all Projects can be automatically converted to the new version of Evolution.</li>
<li>When the Project conversion process runs it will identify any Projects (by Name) that it was not able to automatically convert.  Such Projects will have to be re-created in the new version by hand as active Folders.</li>
<li>Once the Project conversion to Folders is complete, you should be able to view and access the Folders from the Home page far right tab labeled as "Folders"</li>
</ul> 

<asp:Label ID="count_line" runat="server" visible="true"></asp:Label>
<asp:label ID="click_line" runat="server" Visible="false">Click on the button desired below to convert your Projects to Folders.</asp:label>
 <br />
    <asp:Button ID="run_ac_export" runat="server"  Text="Convert Aircraft Projects" Visible="false" />
      <asp:Button ID="run_event_export" runat="server"  Text="Convert Event Projects" Visible="false" /> 
        <asp:Button ID="run_market_summary_export" runat="server"  Text="Convert Market Summary Projects" Visible="false" /> 
      <br />
      <asp:Label ID="results_label"  runat="server" Visible="false" ></asp:Label>
    </form>
</body>
</html>
