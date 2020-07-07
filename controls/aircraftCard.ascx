<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="aircraftCard.ascx.vb"
  Inherits="crmWebClient.aircraftCard" EnableViewState="true" %>
<asp:Label ID="next_prev_text" runat="server" Text="" CssClass="float_right" Width="50"></asp:Label>
<cc1:TabContainer ID="ac_info_container" runat="server" Width="100%" CssClass="dark-theme"
  Visible="true" Height="190px">
  <cc1:TabPanel ID="info_tab" runat="server" HeaderText="GENERAL INFO">
    <ContentTemplate>
      <table width="100%" cellspacing="0" cellpadding="0" class="card_info">
        <tr>
          <td align="left" valign="top" width="45%">
            <asp:Label runat="server" Text="" ID="contact_info" Width="100%" CssClass="card_overflow"></asp:Label>
          </td>
          <td align="left" valign="top">
            <asp:Label runat="server" Text="" ID="contact_right" Width="100%" CssClass="card_overflow"></asp:Label>
          </td>
        </tr>
        <tr>
          <td align="center" valign="top" colspan="2">
            <asp:Label ID="synch_date_comp" runat="server" CssClass="float_left" Width="70px"></asp:Label>
            <asp:Panel runat="server" ID="switch_view" CssClass="float_left">
              <asp:Label runat="server" ID="email_ac" CssClass="float_left"></asp:Label>
              <asp:Label runat="server" ID="edit_view">
                <% If Session.Item("crmUserLogon") = True Then%><% If Session.Item("OtherID") = "0" And Session.Item("ListingSource") = "JETNET" Then%><asp:ImageButton
                  runat="server" ID="create_client_company" ImageUrl="~/images/create_client.jpg"
                  AlternateText="Edit" OnClientClick="javascript:load('edit.aspx?action=edit&type=aircraft','','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;" /><% ElseIf Session.Item("ListingSource") = "CLIENT" Then%><asp:ImageButton
                    runat="server" ID="edit" AlternateText="Edit" ImageUrl="~/images/edit_card.jpg"
                    OnClientClick="javascript:load('edit.aspx?action=edit&type=aircraft','','scrollbars=yes,menubar=no,height=900,width=1030,resizable=yes,toolbar=no,location=no,status=no');return false;" /><% End If%><% End If%></asp:Label>
            </asp:Panel>
            <asp:Label ID="edit_comp" runat="server"></asp:Label>
            <asp:Label ID="full_page" runat="server" CssClass="float_right" Width="90px"></asp:Label>
          </td>
        </tr>
      </table>
    </ContentTemplate>
  </cc1:TabPanel>
  <cc1:TabPanel ID="custom_data_tab" runat="server" HeaderText="CUSTOM DATA" Visible="false">
    <ContentTemplate>
      <table width="100%" cellspacing="0" cellpadding="0" class="card_info">
        <tr>
          <td align="left" valign="top">
            <asp:Label CssClass="card_overflow display_block" runat="server" ID="custom_data_information"></asp:Label>
          </td>
        </tr>
      </table>
    </ContentTemplate>
  </cc1:TabPanel>
</cc1:TabContainer>
