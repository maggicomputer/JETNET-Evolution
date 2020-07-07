<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master"
  CodeBehind="adminUserInsight.aspx.vb" Inherits="crmWebClient.adminUserInsight" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <link rel="Stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.24/themes/smoothness/jquery-ui.css" />
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
  <div style="text-align: left; padding-top: 8px;">
    <script type="text/javascript" src="http://www.google.com/jsapi?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE"></script>
    <asp:Panel ID="user_insight_panel" runat="server">
      <strong>User Insight Summary</strong>
      <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable"
        runat="server">
        <asp:TableRow>
          <asp:TableCell ID="TableCell_left" runat="server" HorizontalAlign="left" VerticalAlign="top" Style="padding-right: 4px; width: 30%;" RowSpan="2">
            <asp:Label ID="insightArealbl" runat="server" Text="What area would like to profile?"></asp:Label><br />
            <asp:ListBox ID="insightAreaddl" runat="server" Rows="1" AutoPostBack="False">
              <asp:ListItem Text="Choose area to Profile" Value=""></asp:ListItem>
              <asp:ListItem Text="Aircraft Acquisition View" Value="acquisition"></asp:ListItem>
              <asp:ListItem Text="Aircraft Comparison Report" Value="aircraft comparison report"></asp:ListItem>
              <asp:ListItem Text="Aircraft Seller/Purchaser Report" Value="aircraft seller/ purchaser report"></asp:ListItem>
              <asp:ListItem Text="Aircraft Prospector" Value="aircraft prospector"></asp:ListItem>
              <asp:ListItem Text="Charter Intelligence" Value="charter intelligence"></asp:ListItem>
              <asp:ListItem Text="Company Contact Labels Report" Value="company contact labels report"></asp:ListItem>
              <asp:ListItem Text="Company Details Report" Value="company details report"></asp:ListItem>
              <asp:ListItem Text="Dealer/brokers" Value="dealer/brokers"></asp:ListItem>
              <asp:ListItem Text="Financial documents" Value="financial documents"></asp:ListItem>
              <asp:ListItem Text="Financial market trends" Value="financial market trends"></asp:ListItem>
              <asp:ListItem Text="Flight activity" Value="flight activity"></asp:ListItem>
              <asp:ListItem Text="Fractional program" Value="fractional program"></asp:ListItem>
              <asp:ListItem Text="Leased aircraft" Value="leased aircraft"></asp:ListItem>
              <asp:ListItem Text="Location of fleet" Value="location of fleet"></asp:ListItem>
              <asp:ListItem Text="Logged In" Value="logged in"></asp:ListItem>
              <asp:ListItem Text="Manufacturer" Value="manufacturer"></asp:ListItem>
              <asp:ListItem Text="Market at a glance" Value="market at a glance"></asp:ListItem>
              <asp:ListItem Text="Model comparison" Value="model comparison"></asp:ListItem>
              <asp:ListItem Text="Model market list" Value="model market list"></asp:ListItem>
              <asp:ListItem Text="Model market summary" Value="model market summary"></asp:ListItem>
              <asp:ListItem Text="Market Report" Value="market report"></asp:ListItem>
              <asp:ListItem Text="Market Summary Report" Value="market summary report"></asp:ListItem>
              <asp:ListItem Text="Notes center" Value="notes center"></asp:ListItem>
              <asp:ListItem Text="Operator/aircraft summary" Value="operator/aircraft summary"></asp:ListItem>
              <asp:ListItem Text="Portfolio manager" Value="portfolio manager"></asp:ListItem>
              <asp:ListItem Text="Residual Value Market Forecast" Value="residual value market forecast"></asp:ListItem>
              <asp:ListItem Text="Route Analysis" Value="route analysis"></asp:ListItem>
              <asp:ListItem Text="Sales price index (SPI)" Value="sales price index"></asp:ListItem>
              <asp:ListItem Text="Statistical analysis reports (STAR)" Value="statistical analysis reports"></asp:ListItem>
              <asp:ListItem Text="eValues" Value="values"></asp:ListItem>
            </asp:ListBox>&nbsp;<asp:Button ID="go" runat="server" Text="Go" CssClass="button-darker button_width" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'User Insight', 'Loading ... Please Wait ...');return true;" PostBackUrl="~/adminUserInsight.aspx?task=" />
          </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
          <asp:TableCell ID="TableCell_right" runat="server" HorizontalAlign="left" VerticalAlign="top" Style="padding-right: 4px;">

            <asp:Label ID="user_insight_DataGraphlbl" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="user_insight_DataDatelbl" runat="server" Text=""></asp:Label>

          </asp:TableCell>
        </asp:TableRow>

      </asp:Table>
      <div id="DivLoadingMessage" style="display: none;">
      </div>
    </asp:Panel>
  </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

  <script type="text/javascript">

    function ShowLoadingMessage(DivTag, Title, Message) {
      $("#" + DivTag).html(Message);
      $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
    }

    function CloseLoadingMessage(DivTag) {
      $("#" + DivTag).dialog("close");
    }

  </script>

</asp:Content>
