<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="companyCard.ascx.vb"
    Inherits="crmWebClient._companyCard" %>
<asp:Label ID="next_prev_text" runat="server" Text="" CssClass="float_right" Width="70"></asp:Label>
<cc1:TabContainer ID="company_info_container" runat="server" Width="100%" CssClass="dark-theme"
    Visible="true" Height="190px">
    <cc1:TabPanel ID="company_info_tab" runat="server" HeaderText="GENERAL INFO">
        <ContentTemplate>
            <table width="100%" cellspacing="0" cellpadding="0" class="card_info">
                <tr>
                    <td align="left" valign="top" width="60%">
                        <asp:Label runat="server" Text="" ID="contact_info" Width="100%" CssClass="card_overflow"></asp:Label>
                    </td>
                    <td align="left" valign="top" width="40%">
                        <asp:Label runat="server" Text="" ID="contact_right" Width="100%" CssClass="card_overflow"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right" valign="top" colspan="2">
                        <asp:Label ID="synch_date_comp" runat="server" CssClass="float_left" Width="140px"></asp:Label><asp:Panel
                            runat="server" ID="switch_view" CssClass="float_left">
                        </asp:Panel>
                        <asp:Label runat="server" ID="email_company" CssClass="float_left"></asp:Label>
                                           <asp:Label ID="full_page" runat="server" CssClass="float_left padding_left" Width="90px"></asp:Label>
                        <asp:Panel ID="edit_company" runat="server">
                            <% If Session.Item("OtherID") = "0" And Session.Item("ListingSource") = "JETNET" Then%><img
                                alt="" src="images/create_client.jpg" onclick="javascript:load('edit.aspx?type=company&comp_ID=<%= session.item("ListingID") %>&source=JETNET','','scrollbars=yes,menubar=no,height=620,width=1050,resizable=yes,toolbar=no,location=no,status=no');" />
                            <% Else%>
                            <img alt="" src="images/edit_card.jpg" onclick="javascript:load('edit.aspx?type=company&action=edit&comp_ID=<%= session.item("ListingID") %>&source=CLIENT','','scrollbars=yes,menubar=no,height=600,width=1050,resizable=yes,toolbar=no,location=no,status=no');" /><% End If%></asp:Panel>

                    </td>
                </tr>
            </table>
            <br clear="all" />
        </ContentTemplate>
    </cc1:TabPanel>
</cc1:TabContainer>
