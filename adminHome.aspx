<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master"
  CodeBehind="adminHome.aspx.vb" Inherits="crmWebClient.adminHome" ValidateRequest="false" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript">

    function openSmallWindowJS(address, windowname) {

      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1100,resizable=yes,toolbar=no,location=no,status=no");

      return true;
    }

  </script>
  <style type="text/css">
    .TrendBox .trendSmall {
      padding-left: 5px;
      font-size: 12px;
    }

    .TrendBox .trendLarge {
      font-size: 26px;
    }

    .TrendBox .fa {
      padding-right: 3px;
    }

    .TrendBox .trendArrow {
      font-size: 15px;
    }

    #table_9_wrapper table {
      margin: 0 auto;
      width: 100%;
      clear: both;
      border-collapse: collapse;
      table-layout: fixed;
      word-wrap: break-word;
    }

    /* clearfix */
    .grid:after {
      content: '';
      display: block;
      clear: both;
    }

    div[id^='chart_div']
    /* ---- grid-item ---- */
    .grid-item {
      float: left;
    }

    .viewValueExport.aircraftSpec .mainHeading {
      padding-top: 0px;
      clear: both;
    }

    .valueSpec.Simplistic .formatTable th, .dataTables_wrapper .dataTables_filter {
      font-size: 12px;
    }

    .formatTable.blue.small a {
      font-size: 10px;
    }

    .valueSpec.Simplistic .subHeader {
      font-size: 16px !important;
    }

    .dataTables_wrapper .dataTables_info {
      font-size: 10px;
      text-align: left;
      padding-left: 0px;
      margin-bottom: 10px;
    }

    .flex-shrink {
      flex: 1, 0, auto;
    }

    .displayTableCell {
      display: inline-block !important;
      margin: 5px !important;
    }

    .removeLeftMargin {
      margin-left: 0px;
      margin-right: 2%;
    }

    table.dataTable th {
      padding-left: 5px !important;
      padding-right: 18px !important;
    }

    .rowWrapper {
      display: flex;
    }

    .dataTables_scrollHead {
      width: 100% !important;
    }

    table.dataTable tfoot th {
      font-size: 15px !important;
      padding: 6px !important;
    }
  </style>

  <script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>
  <script type="text/javascript" src="/common/moment-with-locales.js"></script>

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
  <div class="row remove_margin">

    <asp:UpdatePanel ID="admin_home_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
        <div class="row" runat="server" id="mainMenuRow">
          <div class="eight columns Box">
            <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="80%" CssClass="buttonsTable"
              runat="server">
              <asp:TableRow>
                <asp:TableCell ID="TableCell0" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Summary of customers that are currently OnLine
              <br />
                  <asp:LinkButton ID="btnOnlineNow" runat="server" CssClass="gray_button" Text="Online Now"
                    ToolTip="Summary of customers that are currently logged in" Visible="true" Width="164"
                    PostBackUrl="~/adminOnline.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Displays a summary subscribers from various perspectives
              <br />
                  <asp:LinkButton ID="btnSubOverview" runat="server" CssClass="gray_button" Text="Subscriber Overview"
                    ToolTip="Displays a summary subscribers from various perspectives" Visible="true"
                    Width="164" PostBackUrl="~/adminOnline.aspx?type_to_show=all"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Provides tools for quick lookup of subscriptions, individual subscribers, etc.
              <br />
                  <asp:LinkButton ID="btnLookUpSubscribers" runat="server" CssClass="gray_button" Text="Subscribers"
                    ToolTip="Provides tools for quick lookup of subscriptions, individual subscribers, etc."
                    Visible="true" Width="164" PostBackUrl="~/adminSubscribers.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell3" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Displays a list of active clients for the Marketplace Manager (including those with
              Cloud Notes Plus).
              <br />

                  <asp:LinkButton ID="btnMPM" runat="server" CssClass="gray_button" Text="MPM" ToolTip=" Displays a list of active clients for the Marketplace Manager (including those with Cloud Notes Plus)."
                    Visible="true" Width="164" PostBackUrl="~/adminMPM.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell4" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Displays a summary of tasks submitted or documented for development relating to
              JETNET’s systems and products along with priorities.
              <br />
                  <asp:LinkButton ID="btnDevelopment" runat="server" CssClass="gray_button" Text="Development"
                    ToolTip="Displays a summary of tasks submitted or documented for development relating to JETNET’s systems and products along with priorities."
                    Visible="true" Width="164" PostBackUrl="~/adminDeveloper.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell5" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Used to manage help, release notes, and bulletin board announcements for Evolution
              and Marketplace Manager.
              <br />
                  <asp:LinkButton ID="btnHelp" runat="server" CssClass="gray_button" Text="Help" ToolTip="Used to manage help, release notes, and bulletin board announcements for Evolution and Marketplace Manager."
                    Visible="true" Width="164" PostBackUrl="~/adminHelp.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell6" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Used to generate some simple reports available relating to subscribers and user
              activities.
              <br />
                  <asp:LinkButton ID="btnReports" runat="server" CssClass="gray_button" Text="Reports"
                    ToolTip="Used to generate some simple reports available relating to subscribers and user activities."
                    Visible="true" Width="164" PostBackUrl="~/adminSummary.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell7" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Provides deeper insight into model user interest, features, maintenance items, dealers,
              and sale prices.
              <br />
                  <asp:DropDownList ID="ddlModelIntel" runat="server" ToolTip="Select Model" AutoPostBack="false">
                  </asp:DropDownList>
                  <br />
                  <asp:LinkButton ID="btnModelIntel" runat="server" CssClass="gray_button" Text="View Model Intelligence"
                    ToolTip="Provides deeper insight into model user interest, features, maintenance items, dealers, and sale prices."
                    Visible="true" Width="164"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell8" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Provides ability to manage the settings on background images used on various Jetnet
              applications.
              <br />
                  <asp:LinkButton ID="btnBackground" runat="server" CssClass="gray_button" Text="Background"
                    ToolTip="Provides ability to manage the settings on background images used on various Jetnet applications."
                    Visible="true" Width="164" PostBackUrl="~/adminBackground.aspx">
                  </asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell9" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Sale Price Data Collection Summary.
              <br />
                  <asp:LinkButton ID="btnSPDCS" runat="server" CssClass="gray_button" Text="Sale Price DCS"
                    ToolTip="Sale Price Data Collection Summary." Visible="true" Width="164" PostBackUrl="~/homebaseSPDCS.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell10" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Summarizes the number of flights per day for the current calendar year for each data source and displays totals.
              <br />
                  <asp:LinkButton ID="btnFlightSummary" runat="server" CssClass="gray_button" Text="Flight Data Summary"
                    ToolTip="Summarizes the number of flights per day for the current calendar year for each data source and displays totals."
                    Visible="true" Width="164" PostBackUrl="~/adminFlightSummary.aspx">
                  </asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell11" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Summarizes the Asset Insight Process as used by JETNET.
              <br />
                  <asp:LinkButton ID="LinkButton1" runat="server" CssClass="gray_button" Text="eValue Dashboard"
                    ToolTip="Summarizes the Asset Insight Process as used by JETNET."
                    Visible="true" Width="164" PostBackUrl="~/adminAssetInsight.aspx">
                  </asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell12" runat="server" HorizontalAlign="left" VerticalAlign="middle"
                  Style="padding-right: 4px;">
                  Summarizes Evolution Users web site usage by areas.
              <br />
                  <asp:LinkButton ID="LinkButton2" runat="server" CssClass="gray_button" Text="User Analytics"
                    ToolTip="Summarizes Evolution Users web site usage by areas."
                    Visible="true" Width="164" PostBackUrl="~/adminUserInsight.aspx">
                  </asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
            </asp:Table>
          </div>
        </div>
        <asp:Literal runat="server" ID="moduleLiteral"></asp:Literal>
      </ContentTemplate>
    </asp:UpdatePanel>

  </div>
  <asp:Button runat="server" ID="refreshPage" Text="Refresh Page" OnClientClick="javascript:fnRefreshPage();return false;" CssClass="display_none" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
  <script>
    function fnRefreshPage() {
      location.reload(true);
    }

    function loadMasonry() {
      var grid = document.querySelector('.grid');
      var msnry = new Masonry(grid, {
        itemSelector: '.grid-item',
        columnWidth: '.grid-item',
        gutter: 10,
        horizontalOrder: true,
        percentPosition: true
      });

    }
  </script>
</asp:Content>
