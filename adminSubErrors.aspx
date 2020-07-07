<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyHomebaseTheme.Master" CodeBehind="adminSubErrors.aspx.vb" Inherits="crmWebClient.adminSubErrors" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyHomebaseTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
  <script type="text/javascript">
  </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="">
    <ProgressTemplate>
      <div id="divLoading" runat="server" style="text-align: center; font-weight: bold; background-color: #eeeeee; filter: alpha(opacity=90);
        opacity: 0.9; width: 395px; height: 295px; text-align: center; padding: 75px; position: absolute; border: 1px solid #003957;
        z-index: 10; margin-left: 225px;">
        <span>Please wait ... </span>
        <br />
        <br />
        <img src="/images/loading.gif" alt="Loading..." /><br />
      </div>
    </ProgressTemplate>
  </asp:UpdateProgress>
  <div style="text-align: center; padding-left: 20px; padding-right: 20px;">
    <asp:UpdatePanel ID="subscriber_display_panel" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
        <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" CssClass="DetailsBrowseTable" runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="right" VerticalAlign="middle" Style="padding-right: 4px;"
              Width="23%">
            </asp:TableCell>
            <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="right" VerticalAlign="middle" Style="padding-right: 4px;"
              Width="23%">
              <asp:LinkButton ID="close_button" runat="server" OnClientClick="javascript:window.close();" CssClass="float_right"><img src="/images/x.svg" alt="Close" /></asp:LinkButton>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>

        <div class="valueSpec Simplistic aircraftSpec Box">
            <asp:DropDownList ID="days_drop" runat="server" AutoPostBack="true">
                <asp:ListItem Value="1">Today</asp:ListItem>
                <asp:ListItem Value="2">2 Days</asp:ListItem>
                <asp:ListItem Value="7">7 Days</asp:ListItem>
                <asp:ListItem Value="30">30 Days</asp:ListItem>
                <asp:ListItem Value="180">180 Days</asp:ListItem>
                <asp:ListItem Value="365">365 Days</asp:ListItem>
            </asp:DropDownList>
                
                Month Summary By: 
            <asp:DropDownList ID="sum_by" runat="server" AutoPostBack="true">
                <asp:ListItem Value="">All History</asp:ListItem>
                <asp:ListItem Value="subislog_message">Message</asp:ListItem>
                <asp:ListItem Value="subislog_tcpip">Ip Addresses/Location</asp:ListItem>
                <asp:ListItem Value="export_counts">Export Counts</asp:ListItem>
                <asp:ListItem Value="export_details">Export Details</asp:ListItem>
            </asp:DropDownList>
          <asp:Label ID="subscriber_data_list_display" runat="server" Text="" Width="95%"></asp:Label>
        </div>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
</asp:Content>
