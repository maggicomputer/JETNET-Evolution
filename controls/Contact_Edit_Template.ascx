<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Contact_Edit_Template.ascx.vb"
  Inherits="crmWebClient.Contact_Edit_Template" %>
<link rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:portrait)"
  href="common/ipad-portrait.css" />
<link rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:landscape)"
  href="common/ipad-landscape.css" />
<link rel="stylesheet" media="all and (min-device-width: 1025px)" href="common/regular.css" />
<asp:Panel ID="subpanel_folder" runat="server" BackColor="White" CssClass="edit_panel"
  Visible="false">
  <h4 align="right">
    Subfolders:</h4>
  <asp:Label runat="server" ID="folders_atten" ForeColor="Red" Font-Bold="true" Visible="false"></asp:Label>
  <asp:Label runat="server" ID="folders" Visible="false"></asp:Label>
  <br clear="all" />
  <br clear="all" />
</asp:Panel>
<div class="container">
  <div style="margin-left: 15px; margin-right: 15px; margin-top: 15px;">
    <div class="valueSpec viewValueExport Simplistic aircraftSpec plain">
      <asp:Label ID="edit_cont_tag" runat="server" align="right"><h2 class="mainHeading remove_margin"><strong>Contact</strong> Edit</h2></asp:Label>
      <div class="row">
        <div class="columns seven">
          <asp:Panel ID="contact_edit" runat="server" CssClass="Box">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Contact_Edit"
              DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
            <div class="subHeader">
              Contact Information</div>
            <br />
            <table class="formatTable blue" cellpadding="4" cellspacing="0" width="100%">
              <tr>
                <td align="left" valign="top">
                  Prefix&nbsp;
                </td>
                <td align="left" valign="top">
                  <asp:DropDownList ID="sirname" runat="server" Width="100%">
                  </asp:DropDownList>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  First Name
                </td>
                <td align="left" valign="top"><asp:TextBox runat="server" ID="company_id" CssClass="display_none"></asp:TextBox>
                  <asp:TextBox runat="server" ID="jetnet_contact_id" CssClass="display_none"></asp:TextBox>
                  <asp:TextBox ID="firstname" runat="server" Width="100%" MaxLength="15"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator3" runat="server" ControlToValidate="firstname" Font-Bold="True"
                    ErrorMessage="First Name is Required" ValidationGroup="Contact_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Last Name
                </td>
                <td align="left" valign="top">
                  <asp:TextBox ID="lastname" runat="server" Width="98%" MaxLength="25"></asp:TextBox><asp:RequiredFieldValidator
                    ID="RequiredFieldValidator4" runat="server" ControlToValidate="lastname" Font-Bold="True"
                    ErrorMessage="Last Name is Required" ValidationGroup="Contact_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Preferred Name
                </td>
                <td align="left" valign="top">
                  <asp:TextBox ID="pref" runat="server" Width="100%" MaxLength="20"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Suffix
                </td>
                <td align="left" valign="top">
                  <asp:DropDownList ID="suffix" runat="server" Width="30%">
                  </asp:DropDownList>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Title
                </td>
                <td align="left" valign="top">
                  <asp:TextBox ID="contact_title" runat="server" Width="100%" MaxLength="40"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Status
                </td>
                <td align="left" valign="top">
                  <asp:RadioButtonList ID="contact_status" runat="server" Visible="true" RepeatDirection="Horizontal">
                    <asp:ListItem id="contact_active" runat="server" Value="A" Text="Active" Selected="True" />
                    <asp:ListItem id="contact_inactive" runat="server" Value="B" Text="Inactive" />
                  </asp:RadioButtonList>
                  <asp:TextBox ID="comp_id" runat="server" Style="display: none;"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Middle Initial
                </td>
                <td align="left" valign="top">
                  <asp:TextBox ID="middle" runat="server" Width="100%" MaxLength="1"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Email
                </td>
                <td align="left" valign="top">
                  <asp:TextBox ID="Email" runat="server" Width="98%" MaxLength="70"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Email List
                </td>
                <td align="left" valign="top">
                  <asp:CheckBox ID="CheckBox1" runat="server" />
                  &nbsp;
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Contact Notes
                </td>
                <td align="left" valign="top">
                  <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Width="100%" Height="103px"
                    MaxLength="21845"></asp:TextBox>
                </td>
              </tr>
            </table>
          </asp:Panel>
        </div>
        <div class="columns five">
          <asp:Panel ID="phone" runat="server" CssClass="Box">
            <div class="subHeader">
              Phone Numbers</div>
            <br />
            <table class="formatTable blue" width="100%" cellpadding="4" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  Phone Type<br />
                  <asp:DropDownList ID="type1" runat="server">
                  </asp:DropDownList>
                  <asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="TextValidate"
                    ControlToValidate="phone1" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                    ValidationGroup="Contact_Edit" Text="" Display="None">
                  </asp:CustomValidator>
                </td>
                <td align="left" valign="top">
                  Phone #<br />
                  <asp:TextBox ID="phone1" runat="server" MaxLength="28"></asp:TextBox>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Phone Type<br />
                  <asp:DropDownList ID="type2" runat="server">
                  </asp:DropDownList>
                </td>
                <td align="left" valign="top">
                  Phone #<br />
                  <asp:TextBox ID="phone2" runat="server" MaxLength="28"></asp:TextBox>
                  <asp:CustomValidator ID="CustomValidator2" runat="server" OnServerValidate="TextValidate"
                    ControlToValidate="phone2" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                    ValidationGroup="Contact_Edit" Text="" Display="None">
                  </asp:CustomValidator>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Phone Type<br />
                  <asp:DropDownList ID="type3" runat="server">
                  </asp:DropDownList>
                </td>
                <td align="left" valign="top">
                  Phone #<br />
                  <asp:TextBox ID="phone3" runat="server" MaxLength="28"></asp:TextBox>
                  <asp:CustomValidator ID="CustomValidator3" runat="server" OnServerValidate="TextValidate"
                    ControlToValidate="phone3" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                    ValidationGroup="Contact_Edit" Text="" Display="None">
                  </asp:CustomValidator>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Phone Type<br />
                  <asp:DropDownList ID="type4" runat="server">
                  </asp:DropDownList>
                </td>
                <td align="left" valign="top">
                  Phone #<br />
                  <asp:TextBox ID="phone4" runat="server" MaxLength="28"></asp:TextBox>
                  <asp:CustomValidator ID="CustomValidator4" runat="server" OnServerValidate="TextValidate"
                    ControlToValidate="phone4" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                    ValidationGroup="Contact_Edit" Text="" Display="None">
                  </asp:CustomValidator>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Phone Type<br />
                  <asp:DropDownList ID="type5" runat="server">
                  </asp:DropDownList>
                </td>
                <td align="left" valign="top">
                  Phone #<br />
                  <asp:TextBox ID="phone5" runat="server" MaxLength="28"></asp:TextBox>
                  <asp:CustomValidator ID="CustomValidator5" runat="server" OnServerValidate="TextValidate"
                    ControlToValidate="phone5" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                    ValidationGroup="Contact_Edit" Text="" Display="None">
                  </asp:CustomValidator>
                </td>
              </tr>
              <tr>
                <td align="left" valign="top">
                  Phone Type<br />
                  <asp:DropDownList ID="type6" runat="server">
                  </asp:DropDownList>
                </td>
                <td align="left" valign="top">
                  Phone #<br />
                  <asp:TextBox ID="phone6" runat="server" MaxLength="28"></asp:TextBox>
                  <asp:CustomValidator ID="CustomValidator6" runat="server" OnServerValidate="TextValidate"
                    ControlToValidate="phone6" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                    ValidationGroup="Contact_Edit" Text="" Display="None">
                  </asp:CustomValidator>
                </td>
              </tr>
            </table>
          </asp:Panel>
        </div>
      </div>
      <div class="row">
        <div class="columns twelve">
          <asp:Panel ID="buttons" runat="server">
            <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Right">
              <asp:Label ID="update_text" runat="server" Font-Italic="True"></asp:Label>
            </asp:Panel>
            <table width="100%" cellpadding="4" cellspacing="0">
              <tr>
                <td align="left" valign="top">
                  <asp:Label ID="mobile_close" runat="server">
                               <a href="javascript: self.close ()" class="button float_left">Close</a>
                  </asp:Label>
                  <asp:Button runat="server" Text="Remove" ID="deleteFunction" CssClass="float_left"
                    OnClientClick="return confirm('Are you sure you would like to remove this Contact?');"
                    Visible="true" />
                </td>
                <td align="right" valign="top">
                  <asp:Button runat="server" ID="updateButton" ValidationGroup="Contact_Edit" Text="Save" />
                </td>
              </tr>
            </table>
          </asp:Panel>
        </div>
      </div>
    </div>
  </div>
