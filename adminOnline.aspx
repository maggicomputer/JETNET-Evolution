<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="adminOnline.aspx.vb" Inherits="crmWebClient.adminOnline" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
  <script type="text/javascript">
    google.charts.load('current', { 'packages': ['corechart'] });
  </script>


  <script type="text/javascript">
    var bDontClose = false;

    function ActiveTabChanged(sender, args) { }

    function openSmallWindowJS(address, windowname) {

      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=1200,height=600");

      return true;
    }

  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold; background-color: #eeeeee; filter: alpha(opacity=90); opacity: 0.9; width: 395px; height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957; z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>

  <asp:UpdatePanel ID="admin_online_subscriber_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
    <ContentTemplate>
      <asp:LinkButton ID="btnRefresh" runat="server" Text="Refresh Display" PostBackUrl="~/adminOnline.aspx" Visible="false" Font-Bold="true"></asp:LinkButton>

      <asp:DropDownList ID="location_drop" runat="server" Visible="false" AutoPostBack="true">
        <asp:ListItem Text="All">All</asp:ListItem>
        <asp:ListItem Text="EMEA">EMEA</asp:ListItem>
        <asp:ListItem Text="Non EMEA">Non EMEA</asp:ListItem>
      </asp:DropDownList>
      <asp:Label ID="admin_online_display" runat="server" ForeColor="Black"></asp:Label>

    </ContentTemplate>
  </asp:UpdatePanel>

</asp:Content>

