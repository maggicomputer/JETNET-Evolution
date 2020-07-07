<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="adminCurrentUsers.aspx.vb" Inherits="crmWebClient.adminCurrentUsers" MasterPageFile="~/EvoStyles/CustomerAdminTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/CustomerAdminTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <asp:UpdatePanel ID="admin_current_users_panel" runat="server" ChildrenAsTriggers="True"
      UpdateMode="Conditional">
      <ContentTemplate>
        <table>
          <tr>
            <td>
              Name:<asp:TextBox ID="name_search" runat="server"></asp:TextBox>
            </td>
            <td>
              <asp:Button runat="server" Text="Search" ID="search_button" />
            </td>
            <td>
              Message Type:
              <asp:DropDownList ID="msg_type" runat="server" AutoPostBack="true">
                <asp:ListItem Value="">All</asp:ListItem>
                <asp:ListItem Value="UserLog">User Login</asp:ListItem>
                <asp:ListItem Value="UserSearch">User Search</asp:ListItem>
                <asp:ListItem Value="UserStatistics">User Statistics</asp:ListItem>
                <asp:ListItem Value="UserError">User Error</asp:ListItem>
                <asp:ListItem Value="UserLogout">User Logout</asp:ListItem>
                <asp:ListItem Value="UserDisplayView">User Display View</asp:ListItem>
              </asp:DropDownList>
            </td>
          </tr>
        </table>
        <table id="usersDataTable" width="100%" cellpadding="2" cellspacing="0" border="0">
          <tr valign="top">
            <td valign="top">
              <cc1:TabContainer ID="left_tab_container" runat="server" Visible="true" CssClass="dark-theme" Width="100%">
                <cc1:TabPanel runat="server" ID="left_side_panel" Visible="true">
                  <ContentTemplate>
                    <asp:Label ID="left_side_text" runat="server"></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </td>
            <td valign="top">
              <cc1:TabContainer ID="right_tab_container" runat="server" Visible="true" CssClass="dark-theme"
                AutoPostBack="false">
                <cc1:TabPanel runat="server" ID="right_side_panel" HeaderText="" Visible="true">
                  <HeaderTemplate>
                    Current User Info
                  </HeaderTemplate>
                  <ContentTemplate>
                    <asp:Label ID="right_side_text" runat="server"></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel runat="server" ID="all_user_panel" HeaderText="" Visible="false">
                  <HeaderTemplate>
                    All Licenses
                  </HeaderTemplate>
                  <ContentTemplate>
                    <asp:Label ID="all_user_text" runat="server"></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </td>
          </tr>
          <tr valign="top">
            <td colspan="2" valign="top">
              <cc1:TabContainer ID="bottom_tab_container" runat="server" Visible="true" CssClass="dark-theme">
                <cc1:TabPanel runat="server" ID="bottom_tab_panel" Visible="true">
                  <ContentTemplate>
                    <asp:Label ID="bottom_tab_text" runat="server"></asp:Label>
                  </ContentTemplate>
                </cc1:TabPanel>
              </cc1:TabContainer>
            </td>
          </tr>
        </table>
        <asp:Label ID="invisible_label" runat="server" Visible="false"></asp:Label>
      </ContentTemplate>
    </asp:UpdatePanel>
  </div>
</asp:Content>
