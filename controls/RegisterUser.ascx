<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="RegisterUser.ascx.vb" Inherits="crmWebClient._RegisterUser" EnableViewState="false"%>

  <asp:Panel ID="registerPanel" runat="server" Width="225px">
    <table border="0" cellpadding="0" cellspacing="0" style="border-collapse: collapse;"
      width="100%">
      <tr>
        <td>
          <table id="outerTableRegister" cellpadding="2" cellspacing="0" width="100%">
            <tr>
              <td colspan="2" align="center">
                <asp:Label ID="reglbl" runat="server" Font-Bold="True">Please Register your install of Jetnet CRM Web Client</asp:Label>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top" style="white-space: nowrap;">
                <asp:Label ID="LabelSubID" runat="server" AssociatedControlID="TextSubID">Subscription ID : </asp:Label>
              </td>
              <td align="left">
                <asp:TextBox ID="TextSubID" runat="server" Width="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Subscription ID is required."
                  ToolTip="Subscription ID is required." ControlToValidate="TextSubID" ValidationGroup="Register1">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Subscription ID needs to be a NUMBER."
                  ToolTip="Subscription ID needs to be a NUMBER." ControlToValidate="TextSubID" ValidationGroup="Register1"
                  ValidationExpression="^\d+$">*</asp:RegularExpressionValidator>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top" style="white-space: nowrap;">
                <asp:Label ID="LabelUserID" runat="server" AssociatedControlID="TextUserID">User Name : </asp:Label>
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="TextUserID" runat="server" Width="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="User Name is required."
                  ToolTip="User Name is required." ControlToValidate="TextUserID" ValidationGroup="Register1">*</asp:RequiredFieldValidator>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top" style="white-space: nowrap;">
                <asp:Label ID="LabelPwd" runat="server" AssociatedControlID="TextPswd">Password : </asp:Label>
              </td>
              <td align="left">
                <asp:TextBox ID="TextPswd" runat="server" TextMode="Password" Width="100px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Password is required."
                  ToolTip="Password is required." ControlToValidate="TextPswd" ValidationGroup="Register1">*</asp:RequiredFieldValidator>
              </td>
            </tr>
            <tr>
              <td align="center" colspan="2" style="color: Red;">
                <asp:Literal ID="FailureText1" runat="server" EnableViewState="False"></asp:Literal>
              </td>
            </tr>
            <tr>
              <td colspan="2" align="right" >
                <asp:Button ID="RegisterButton" runat="server" CommandName="Register" OnClick="RegisterButton_Click" Text="Register"
                  ValidationGroup="Register1" />
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </asp:Panel>

