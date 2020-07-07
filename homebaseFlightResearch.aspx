<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="homebaseFlightResearch.aspx.vb"
  Inherits="crmWebClient.homebaseFlightResearch" MasterPageFile="~/EvoStyles/HomebaseTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/HomebaseTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script type="text/javascript">
    var bDontClose = false;

    function ActiveTabChanged(sender, args) { }

    function openSmallWindowJS(address, windowname) {

      var rightNow = new Date();
      windowname += rightNow.getTime();
      var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=900,height=600");

      return true;
    }
  
  </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold;
        background-color: #eeeeee; filter: alpha(opacity=90); opacity: 0.9; width: 395px;
        height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957;
        z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div style="text-align: left; padding-top: 8px;">
    <asp:UpdatePanel ID="faa_flight_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
        <strong>FAA Flight Research</strong>
        <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable"
          runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="left" VerticalAlign="middle"
              Style="padding-right: 4px;" ColumnSpan="2">
              <div style="text-align: right;">
                Enter Aircraft Registration Number
                <asp:TextBox ID="reg_no" runat="server" Width="200"></asp:TextBox>&nbsp;&nbsp;
                <asp:LinkButton ID="runTaskBtn" runat="server" PostBackUrl="~/homebaseFlightResearch.aspx?task=run"
                  Text="<strong>Generate Report</strong>"></asp:LinkButton>
              </div>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="left" VerticalAlign="middle">
              <asp:Label ID="FAAResearchDetailsLbl" runat="server" Text=""></asp:Label>
            </asp:TableCell>
            <asp:TableCell ID="TableCell3" runat="server" HorizontalAlign="left" VerticalAlign="top">
              <asp:Label ID="AircraftDetailsLbl" runat="server" Text=""></asp:Label>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell4" runat="server" HorizontalAlign="left" VerticalAlign="middle"
              Style="padding-right: 24px;" ColumnSpan="2">
              <div style="text-align: right;">
                <asp:Label ID="flightAwareLink" runat="server" Text=""></asp:Label>
              </div>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell5" runat="server" HorizontalAlign="left" VerticalAlign="middle">
              <asp:Label ID="faaDataDatelbl" runat="server" Text=""></asp:Label>
            </asp:TableCell>
            <asp:TableCell ID="TableCell6" runat="server" HorizontalAlign="left" VerticalAlign="middle" Width="75%">
              <asp:Label ID="faaDataGraphlbl" runat="server" Text=""></asp:Label>
            </asp:TableCell>          
           </asp:TableRow>
        </asp:Table>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
