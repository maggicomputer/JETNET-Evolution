<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="eventsManager.aspx.vb" Inherits="crmWebClient.eventsManager" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <script language="javascript" type="text/javascript">
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
  <div style="text-align: left;">
    <asp:UpdatePanel ID="events_manager" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
      <ContentTemplate>
        <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" CssClass="buttonsTable" runat="server">
          <asp:TableRow>
            <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="right" VerticalAlign="middle" Style="padding-right: 4px;"
              Width="23%">
          <a href="#" onclick="javascript:load('help.aspx','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
            class="red_button help_button float_left" title="Show Market Transactions Help">
            <img src="images/info_white.png" border="0" width="16" alt="Help" title="Show Events Manager Help"/>
          </a>
            </asp:TableCell>
            <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="right" VerticalAlign="middle" Style="padding-right: 4px;"
              Width="23%">
              <asp:LinkButton ID="close_button" runat="server" Text="Close" OnClientClick="javascript:window.close();" CssClass="gray_button float_right"></asp:LinkButton>
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
        <div class="NotesHeader" style="margin-bottom: 3px;">
        </div>
        <br />
        <asp:Literal ID="debug_output" runat="server"></asp:Literal>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>