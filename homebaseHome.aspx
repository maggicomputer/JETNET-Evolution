<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/HomebaseTheme.Master" CodeBehind="homebaseHome.aspx.vb"
  Inherits="crmWebClient.homebaseHome" %>

<%@ MasterType VirtualPath="~/EvoStyles/HomebaseTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript">
    google.charts.load('current', { 'packages': ['corechart'] });

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
            <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="80%" CssClass="buttonsTable" runat="server">
              <asp:TableRow>
                <asp:TableCell ID="TableCell0" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Edit Aircraft
              <br />
                  <asp:LinkButton ID="btnHomebaseAircraft" runat="server" CssClass="gray_button" Text="Edit Aircraft" ToolTip="Edit Aircraft."
                    Visible="true" Width="164" PostBackUrl=""></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell9" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Edit Aircraft Model
              <br />
                  <asp:LinkButton ID="btnHomebaseModel" runat="server" CssClass="gray_button" Text="Edit Model" ToolTip="Edit Model." Visible="true"
                    Width="164" PostBackUrl="~/homebaseEditAircraftModel.aspx" OnClientClick="window.document.forms[0].target='_new'; setTimeout(function(){window.document.forms[0].target='';}, 500);"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Edit Company
              <br />
                  <asp:LinkButton ID="btnHomebaseCompany" runat="server" CssClass="gray_button" Text="Edit Company" ToolTip="Edit Company"
                    Visible="true" Width="164" PostBackUrl=""></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell3" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Edit Contact
              <br />
                  <asp:LinkButton ID="btnHomebaseContact" runat="server" CssClass="gray_button" Text="Edit Contact" ToolTip="Edit Contact"
                    Visible="true" Width="164" PostBackUrl=""></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell5" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Edit Attributes
              <br />
                  <asp:LinkButton ID="btnHomebaseAttributes" runat="server" CssClass="gray_button" Text="Edit Attributes" ToolTip="Edit Attributes" Visible="true"
                    Width="164" PostBackUrl="~/Attributes.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell8" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Displays a list of active clients for the Marketplace Manager (including those with Cloud Notes Plus).
              <br />
                  <asp:LinkButton ID="btnMPM" runat="server" CssClass="gray_button" Text="MPM" ToolTip=" Displays a list of active clients for the Marketplace Manager (including those with Cloud Notes Plus)."
                    Visible="true" Width="164" PostBackUrl="~/adminMPM.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell4" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Displays a summary of tasks submitted or documented for development relating to JETNET’s systems and products along with
              priorities.
              <br />
                  <asp:LinkButton ID="btnHomebaseDevelopment" runat="server" CssClass="gray_button" Text="Development" ToolTip="Displays a summary of tasks submitted or documented for development relating to JETNET’s systems and products along with priorities."
                    Visible="true" Width="164" PostBackUrl="~/adminDeveloper.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell6" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Used to generate some simple reports from Homebase.
              <br />
                  <asp:LinkButton ID="btnHomebaseReports" runat="server" CssClass="gray_button" Text="Reports" ToolTip="Used to generate some simple reports from Homebase."
                    Visible="true" Width="164" PostBackUrl="~/adminSummary.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Flight Data Research.
              <br />
                  <asp:LinkButton ID="btnHomebaseFAAFlight" runat="server" CssClass="gray_button" Text="Flight Data" ToolTip="Flight Data Research."
                    Visible="true" Width="164" PostBackUrl="~/homebaseFlightResearch.aspx"></asp:LinkButton>
                </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow>
                <asp:TableCell ID="TableCell7" runat="server" HorizontalAlign="left" VerticalAlign="middle" Style="padding-right: 4px;">
                  Sale Price Data Collection Summary.
              <br />
                  <asp:LinkButton ID="btnHomebaseSPDCS" runat="server" CssClass="gray_button" Text="Sale Price DCS" ToolTip="Sale Price Data Collection Summary."
                    Visible="true" Width="164" PostBackUrl="~/homebaseSPDCS.aspx"></asp:LinkButton>
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
