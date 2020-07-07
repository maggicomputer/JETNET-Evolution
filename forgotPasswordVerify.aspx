<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" CodeBehind="forgotPasswordVerify.aspx.vb" Inherits="crmWebClient.forgotPasswordVerify" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

  <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
  <link rel="Stylesheet" type="text/css" href="https://ajax.aspnetcdn.com/ajax/jquery.ui/1.12.1/themes/smoothness/jquery-ui.css" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  <asp:Panel ID="contentClass" runat="server" Width="100%" HorizontalAlign="Center"
    CssClass="valueViewPDFExport remove_padding">
    <div id="searchPanelContainerDiv" runat="server" class="center_outer_div" width="1050">
      <div style="padding: 28px; text-align: center; width: 95%;">
        <strong>
          <asp:Label ID="user_email_text" runat="server" Text="Password Change for User" Font-Size="Large" ForeColor="CadetBlue"></asp:Label>
        </strong>
        <br />
        <br />
        Please Change your password to a Password of your choice.<br />
        Your new password should be a minimum of 8 characters and must contain *at least*,
        <br />
        one NUMBER, one LOWER case and one UPPER case, and one SPECIAL character (<em>!@#$%^&*_+=-</em>).
      </div>
      <table id="change_password_table" style="padding: 4px; border-spacing: 6px; text-align: left; width: 100%;">
        <tr>
          <td style="vertical-align: top; text-align: left; padding: 4px; width: 20%;" rowspan="3" colspan="2">
            <img src="images/tools.jpg" alt="Change Password" />
          </td>
        </tr>
        <tr>
          <td style="vertical-align: top; text-align: left; padding: 4px;">New Password:</td>
          <td style="vertical-align: top; text-align: left; padding: 4px;">
            <asp:TextBox ID="newPasswordID" runat="server" TextMode="Password" Text="" MaxLength="24" ></asp:TextBox>
            &nbsp;&nbsp;<asp:Image ID="actinfo_password_mouseover_img" Height="15px" runat="server"
              ImageUrl="/images/info.png" />
          </td>
        </tr>
        <tr>
          <td style="vertical-align: top; text-align: left; padding: 4px;">Confirm Password:</td>
          <td style="vertical-align: top; text-align: left; padding: 4px;">
            <asp:TextBox ID="confirmPasswordID" runat="server" TextMode="Password" Text=""></asp:TextBox>
          </td>
        </tr>
      </table>

      <div style="text-align: right; padding-right: 16px; padding-bottom: 6px;">
        <asp:LinkButton ID="changeBtn" runat="server" CssClass="button-darker" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Changing Password', 'Changing Password ... Please Wait ...');return true;" PostBackUrl="~/forgotPasswordVerify.aspx" Text="Change" />
        <asp:LinkButton ID="tryAgainBtn" runat="server" CssClass="button-darker" OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Resending Password', 'Resending Password Change Email ... Please Wait ...');return true;" PostBackUrl="~/forgotPasswordVerify.aspx" Text="Try Again" />
      </div>

      <asp:Label ID="forgot_email_response" runat="server" ForeColor="Red" Font-Size="Large" Height="26"></asp:Label><br /><br />
      <asp:Label ID="login_link" runat="server" Height="26" Visible="false"></asp:Label>

    </div>
    <div id="DivLoadingMessage">
    </div>
  </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

  <script type="text/javascript">
    function ShowLoadingMessage(DivTag, Title, Message) {
      $("#" + DivTag).html(Message);
      $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
    }

    function CloseLoadingMessage(DivTag) {
      $("#" + DivTag).dialog("close");
    }

    function showChangeButton() {

      var newPwd = document.getElementById("<%= newPasswordID.ClientID.ToString %>").value;
      var confirmPwd = document.getElementById("<%= confirmPasswordID.ClientID.ToString %>").value;
      var changeBtn = document.getElementById("<%= changeBtn.ClientID.ToString %>");

      if (confirmPwd != '' && newPwd != '') {

        if (newPwd == confirmPwd) {
          changeBtn.style.visibility = "visible";
        } else {
          changeBtn.style.visibility = "hidden";
        }
      } else {
        changeBtn.style.visibility = "hidden";
      }
    }

    function validatePassword() {
      var txttext = document.getElementById("<%= newPasswordID.ClientID.ToString %>").value;
      var regex = /^(?=(.*[A-Z]){1,})(?=(.*[a-z]){1,})(?=(.*[\d]){1,})(?=(.*[\W]){1,})(?!.*\s).{8,25}$/;

      if (eval(regex.test(txttext)) == false && txttext != '') {
        alert('Your new password should be a minimum of 8 characters in length and must contain *at least* one number, one LOWER case and one UPPER case, and one SPECIAL character ( !@#$%^&*()_+=- )" ...');
        document.getElementById("<%= newPasswordID.ClientID.ToString%>").style.color = "red";
      }
    }


  </script>

</asp:Content>
