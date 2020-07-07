<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="validateUser.ascx.vb"
    Inherits="crmWebClient.validateUser" %>
<asp:Panel ID="registerPanel" runat="server" Width="200px">
    <table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse;"
        width="100%">
        <tr>
            <td align="center">
                <asp:Label ID="Label1" runat="server" Font-Bold="true">Jetnet CRM Client User Validation Failure.</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" style="color: Red;">
                <asp:Literal ID="FailureText1" runat="server" EnableViewState="False"></asp:Literal>
            </td>
        </tr>
    </table>
</asp:Panel>
