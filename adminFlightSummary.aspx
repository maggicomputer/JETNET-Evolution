<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master"
  CodeBehind="adminFlightSummary.aspx.vb" Inherits="crmWebClient.adminFlightSummary" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <asp:UpdatePanel ID="flight_summary_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
        <strong>Flight Data Summary</strong>
        <asp:Table ID="menuTable" CellPadding="4" CellSpacing="0" Width="100%" CssClass="buttonsTable"
          runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="left" VerticalAlign="top" Style="padding-right: 4px;">
              <asp:Label ID="flightSummaryDataGraphlbl" runat="server" Text=""></asp:Label>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow>
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="left" VerticalAlign="top" Style="padding-right: 4px;">
              <asp:Label ID="flightSummaryDataDatelbl" runat="server" Text=""></asp:Label>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>

