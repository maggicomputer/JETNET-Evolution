<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="LogonUser.ascx.vb" Inherits="crmWebClient._LogonUser"
  EnableViewState="false" %>
<asp:Panel ID="logonPanel" runat="server">
  <table border="0" cellpadding="3" cellspacing="0" style="border-collapse: collapse;"
    width="100%">
    <tr>
      <td>
        <table id="outerTableLogon" cellpadding="2" cellspacing="0" width="100%">
          <tr>
            <td colspan="2" align="left">
              <asp:Label ID="logonlbl" runat="server" Font-Bold="True" ForeColor="Black">Please Login to JETNET CRM</asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" valign="middle" colspan="2">
              <asp:TextBox ID="UserName" runat="server" Width="99%" Height="18" placeholder="Email:"></asp:TextBox>
              <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                Display="none" ErrorMessage="User Name is required." ToolTip="User Name is required."
                ValidationGroup="Login1">*</asp:RequiredFieldValidator>
              <asp:RegularExpressionValidator ID="regexpName" runat="server" ErrorMessage="Incorrect Username."
                ControlToValidate="UserName" ValidationGroup="Login1" Display="none" Enabled="false"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
            </td>
          </tr>
          <tr>
            <td align="left" valign="middle" colspan="2">
              <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="99%" Height="18"
                placeholder="Password:"></asp:TextBox>
            </td>
          </tr>
           <tr>
            <td align="right" valign="middle" colspan="2">
              <asp:Button ID="LoginButton" runat="server" CommandName="Login" OnClick="LoginButton_Click"
                Text="Log In" ValidationGroup="Login1" CausesValidation="true" />
            </td>
          </tr>
          <tr>
            <td align="left" valign="middle">
              <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me" onclick="javascript:AlertUser();"
                ToolTip=" 'click' Remember me next time? to remember your 'log on' information " />
            </td>
            <td align="right" valign="middle">
              <asp:LinkButton ID="ForgotPassword" runat="server" Text="Forgot Password?" Visible="false"
                AutoPostBack="true" ToolTip=""></asp:LinkButton>
            </td>
          </tr>
          <tr>
            <td colspan="2" align="left" valign="middle">
              <asp:CheckBox ID="Autologin" runat="server" Text="Auto Logon to JETNET - Online"
                Visible="false" ToolTip=" 'click' Auto Logon to allow your subscription to avoid showing 'logon' screen at start up." />
            </td>
          </tr>
          <tr>
            <td align="left" valign="middle" colspan="2" style="color: #ff4545;
              padding: 2px; text-align: center;">
              <asp:ValidationSummary ID="ValidationSummary" HeaderText="The following errors have occured:"
                DisplayMode="BulletList" EnableClientScript="false" runat="server" ValidationGroup="Login1" />
              <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
            </td>
          </tr>
         
        </table>
      </td>
    </tr>
  </table>
  <cc1:ModalPopupExtender ID="PopupDemoWarning" runat="server" TargetControlID="error_DemoWarning"
    PopupControlID="DemoWarningText" BackgroundCssClass="modalBackground" DropShadow="true"
    CancelControlID="CancelButton" RepositionMode="None" />
  <asp:Panel ID="DemoWarningText" runat="server" Style="display: none">
    <asp:Label runat="server" ID="demoWarningTextSwap"></asp:Label>
    <div align="center" style="padding-top: 4px; padding-right: 4px;">
      <asp:Button ID="OkButtonDemo" runat="server" Text="I agree" OnClientClick="hideDemoBox();" />
      <asp:Button ID="CancelButtonDemo" runat="server" Text="I don't agree" />
    </div>
  </asp:Panel>
  <asp:Button ID="error_DemoWarning" runat="server" Text="Button" Style="display: none;" />
  <cc1:ModalPopupExtender ID="MPE2" runat="server" TargetControlID="sendPassword" PopupControlID="forgotPasswordPopUp"
    BackgroundCssClass="modalBackground" DropShadow="true" RepositionMode="None" />
  <asp:Panel ID="forgotPasswordPopUp" runat="server" Style="display: none; padding-top: 8px;
    width: 360px; text-align: center;" HorizontalAlign="Center" BorderColor="black"
    BorderStyle="Solid" BackColor="LightGray">
    <p align="left" style="font-size: 16px; font-weight: bold; padding: 2px; color: black;
      text-align: center;">
      JETNET forgot user password</p>
    <p align="left" style="color: black; text-align: center; padding-left: 2px;">
      'Click' OK to have your password sent to
    </p>
    <asp:TextBox runat="server" ID="emailAddress" Height="22" Width="220" Text="username@email.com"
      Style="border: 1px solid black; padding-left: 4px;" EnableViewState="false"></asp:TextBox>
    <div align="center" style="padding-top: 4px; padding-right: 4px;">
      <asp:Button ID="btnOk" runat="server" Text="OK" BackColor="LightBlue" OnClientClick="hidePasswordBox();" />
      <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
    </div>
  </asp:Panel>
  <asp:Button ID="sendPassword" runat="server" Text="Button" Style="display: none;" />

  <script language="javascript" type="text/javascript">

    function hideDemoBox() {
      var obj = document.getElementById("<%= DemoWarningText.clientID %>")
      if ((typeof (obj) != "undefined") && (obj != null)) {
        obj.style.display = 'none'
      }
    }

    function hidePasswordBox() {
      var obj = document.getElementById("<%= forgotPasswordPopUp.clientID %>")
      if ((typeof (obj) != "undefined") && (obj != null)) {
        obj.style.display = 'none'
      }
    }
     
  </script>

</asp:Panel>
