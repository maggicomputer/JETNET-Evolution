<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Debug.aspx.vb" Inherits="crmWebClient.Debug" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Untitled Page</title>
  <link href="../EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />
  <link href="../EvoStyles/stylesheets/layout/skeleton_grid.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <form id="form1" runat="server">
  <div style="padding-bottom: 20px">
    launch market summary from link :
    <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/MarketSummary.aspx?amod_id=272">marketSummary.aspx?amod_id=272</asp:LinkButton><br />
    launch full text search link :
    <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="~/fullTextSearch.aspx">fullTextSearch.aspx</asp:LinkButton><br />
    launch static folder manager link :
    <asp:LinkButton ID="LinkButton3" runat="server" PostBackUrl="~/staticFolderEditor.aspx?folderID=671">staticFolderEditor.aspx?folderID=671</asp:LinkButton><br />
    launch Aircraft Acquisition View link :
    <asp:LinkButton ID="LinkButton4" runat="server" PostBackUrl="~/aircraftFinder.aspx">aircraftFinder.aspx</asp:LinkButton><br />
    launch Generic Report link :
    <asp:LinkButton ID="LinkButton5" runat="server" PostBackUrl="~/WebSource.aspx?genericReport=true">WebSource.aspx?genericReport=true</asp:LinkButton><br />
    launch homebase edit aircraft :
    <asp:LinkButton ID="LinkButton6" runat="server" PostBackUrl="~/homebaseEditAircraftModel.aspx?AircraftID=197500">homebaseEditAircraftModel.aspx?AircraftID=197500</asp:LinkButton><br />
    launch homebase subscription page :
    <asp:LinkButton ID="LinkButton7" runat="server" PostBackUrl="~/homebaseSubscription.aspx" Enabled="true">homebaseSubscription.aspx</asp:LinkButton><br />
  </div>
  </form>
</body>
</html>
