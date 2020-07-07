<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="sandbox.aspx.vb" Inherits="crmWebClient.sandbox"
    MasterPageFile="~/Mobile.Master" %>

<%@ MasterType VirtualPath="~/Mobile.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="header" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Table ID="display_table" runat="server" Width="100%" CellPadding="5" CellSpacing="0" BackColor="White">
        <asp:TableHeaderRow>
            <asp:TableCell ID="home_calendar" RowSpan="3" VerticalAlign="Top">
                <asp:LinkButton ID="skip_main_menu" runat="server" ForeColor="Black" Font-Bold="true">Skip to Main Menu</asp:LinkButton><br /><br />
                <asp:Panel ID="Panel1" runat="server" BackColor="#657C92" ForeColor="White" Font-Size="14"
                    Height="35" CssClass="no_pad">
                    <asp:Label ID="today_date" runat="server" Text="Most Recently Edited Companies" CssClass="today_date"></asp:Label>
                </asp:Panel>
                <br />

                    <asp:Label ID="action_items" runat="server" Text="Calendar" CssClass="today_calendar"></asp:Label>

            </asp:TableCell>
        </asp:TableHeaderRow>
    </asp:Table>
    <asp:Label ID="main_menu" runat="server" Text="">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left" valign="top">
               <a href="Mobile_Listing.aspx?type=1"><img class="icon" src="images/company_button.png" alt="Company" title="Company" border="0" /></a>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <a href="Mobile_Listing.aspx?type=2"><img class="icon" src="images/contact_button.png" alt="Contact" title="Contact" border="0" /></a>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <a href="Mobile_Listing.aspx?type=3"><img class="icon" src="images/aircraft_button.png" alt="Aircraft" title="Aircraft" border="0" /></a>
            </td>
        </tr>
         <tr>
            <td align="left" valign="top">
                <a href="Mobile_Listing.aspx?type=10"><img class="icon" src="images/market_button.png" alt="Market" title="Market" border="0" /></a>
            </td>
        </tr>
         <tr>
            <td align="left" valign="top">
                <a href="Mobile_Listing.aspx?type=8"><img class="icon" src="images/transactions_button.png" alt="Transactions" title="Transactions" border="0" /></a>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                 <a href="Mobile_Listing.aspx?type=6"><img class="icon" src="images/notes_button.png" alt="Notes" title="Notes" border="0" /></a>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                 <a href="Mobile_Listing.aspx?type=4"><img class="icon" src="images/action_button.png" alt="Actions" title="Actions" border="0" /></a>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <a href="Mobile_Listing.aspx?type=11"><img class="icon" src="images/opportunities_button.png" alt="Opportunities" title="Opportunities" border="0" /></a>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <a href="Mobile_Listing.aspx?type=7"><img class="icon" src="images/document_button.png" alt="Documents" title="Documents" border="0" /></a>
            </td>
        </tr>
    </table>
    </asp:Label>
</asp:Content>
