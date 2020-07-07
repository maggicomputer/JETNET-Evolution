<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Aircraft_Edit_Propeller_Tab.ascx.vb" Inherits="crmWebClient.Aircraft_Edit_Propeller_Tab" %>
 <asp:Panel ID="aircraft_edit" runat="server" BackColor="White" CssClass="edit_panel"
    Width="1182px">
    <h4 align="right">
        Propeller Edit</h4>
    <br />
    <br />

    <asp:Table ID="Table1" runat="server" CellSpacing="3" CellPadding="7" GridLines="Both">
        <asp:TableRow runat="server" CssClass="dark_blue">
            <asp:TableCell  runat="server" ColumnSpan="2">
        &nbsp;
            </asp:TableCell>
            <asp:TableCell runat="server" VerticalAlign="Top">
            <strong>Serial #:</strong>
            </asp:TableCell>
            <asp:TableCell runat="server" VerticalAlign="Top">
            <strong>TTSNEW Hrs</strong><br /><span class="tiny">(Total Time Since New)</span>
            </asp:TableCell>
            <asp:TableCell runat="server" VerticalAlign="Top">
            <strong>SOH/SCOR Hrs </strong><br /><span class="tiny">(Since Overhaul)</span>
            </asp:TableCell>
            <asp:TableCell runat="server" VerticalAlign="Top">
            <strong>SOH/SCOR Mth/Year </strong><br /><span class="tiny">(Since Overhaul)</span>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow runat="server" CssClass="alt_row">
            <asp:TableCell ID="TableCell3" runat="server" ColumnSpan="2">
       Propeller 1:
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox runat="server" id="prop_1_ser" Width="50"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox runat="server" ID="prop_1_ttsnew" Width="50"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox ID="prop_1_soh" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox ID="prop_1_sohyrs" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow4" runat="server">
            <asp:TableCell runat="server" ColumnSpan="2">
        Propeller 2:
            </asp:TableCell>
            <asp:TableCell  runat="server">
                <asp:TextBox ID="prop_2_ser" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox ID="prop_2_ttsnew" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox ID="prop_2_soh" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell  runat="server">
                <asp:TextBox ID="prop_2_sohyrs" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow ID="TableRow5" runat="server" CssClass="alt_row">
            <asp:TableCell runat="server" ColumnSpan="2">
       Propeller 3:
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox ID="prop_3_ser" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox ID="prop_3_ttsnew" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox ID="prop_3_soh" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell runat="server">
                <asp:TextBox ID="prop_3_sohyrs" runat="server" Width="50"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Panel ID="buttons" runat="server" BackColor="White">
        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Right">
            <asp:Label ID="update_text" runat="server" Font-Italic="True"></asp:Label>
        </asp:Panel>
        <table width="100%" cellpadding="4" cellspacing="0">
            <tr>
                <td align="left" valign="top">
                    <a href="javascript: self.close ()">
                        <img src="images/cancel.gif" alt="Cancel" border="0" /></a>
                </td>
                <td align="right" valign="top">
                    <asp:ImageButton ID="update" CausesValidation="true" runat="server" ImageUrl="~/images/update.gif"
                        AlternateText="Update" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Panel>