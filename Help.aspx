<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Help.aspx.vb" Inherits="crmWebClient.Help" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

  <div id="outerDiv" class="valueSpec viewValueExport Simplistic aircraftSpec gray_background" runat="server">
    <table border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;">
      <tr>
        <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">
          <asp:Label ID="close_window_only" runat="server" CssClass="float_right criteria_text"></asp:Label>

          <cc1:TabContainer runat="server" ID="tab_container_ID" Width="100%" ActiveTabIndex="0"
            BorderStyle="None" CssClass="dark-theme" AutoPostBack="true">
            <cc1:TabPanel ID="features_tab" runat="server" HeaderText="RELEASE NOTES/FEATURES">
              <ContentTemplate>
                <asp:Label ID="features_label" runat="server" Text=""></asp:Label>
                <asp:Button ID="export_notes" runat="server" Text="Export Notes" Visible="false"></asp:Button>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="bulletin_tab" runat="server" HeaderText="BULLETIN BOARD">
              <ContentTemplate>
                <asp:Label ID="bulletin_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="help_panel" runat="server" HeaderText="HELP">
              <ContentTemplate>
                <asp:Label ID="help_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="news_panel" runat="server" HeaderText="JETNET NEWS">
              <ContentTemplate>
                <asp:Label ID="news_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="calendar_panel" runat="server" HeaderText="JETNET CALENDAR">
              <ContentTemplate>
                <asp:Label ID="calendar_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="resources_panel" runat="server" HeaderText="Model Resources">
              <ContentTemplate>
                <asp:Label ID="resources_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="ac_glossary_panel" runat="server" HeaderText="Aircraft Glossary" Visible="false">
              <ContentTemplate>
                <asp:TextBox ID="ac_glossary_text" Width='200' runat="server"></asp:TextBox><asp:Button ID="search_ac" Text="Search Glossary" runat="server" autopostback="true"></asp:Button>
                <asp:Label ID="ac_glossary_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="yacht_glossary_panel" runat="server" HeaderText="Yacht Glossary" Visible="false">
              <ContentTemplate>
                <asp:TextBox ID="yacht_glossary_text" Width='200' runat="server"></asp:TextBox><asp:Button ID="search_yacht" runat="server" Text="Search Glossary" autopostback="true"></asp:Button>
                <asp:Label ID="yacht_glossary_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="individual_panel" runat="server" Visible="false">
              <ContentTemplate>
                <asp:Label ID="individual_label" runat="server" Text=""></asp:Label>
                <asp:Label ID="invis_label" runat="server" Text=""></asp:Label>
              </ContentTemplate>
            </cc1:TabPanel>
          </cc1:TabContainer>

        </td>
      </tr>
    </table>
  </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">
</asp:Content>
