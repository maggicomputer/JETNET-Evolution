<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ContactQuickEntry.ascx.vb"
  Inherits="crmWebClient.ContactQuickEntry" %>
<asp:Panel ID="contact_edit" runat="server" CssClass="valueSpec viewValueExport Simplistic aircraftSpec plain">
  <h2 class="mainHeading" style="margin-bottom: 0px;">
    Contact Quick Entry</h2>
  <div class="row remove_margin">
    <div class="six columns">
      <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Contact_Edit"
        DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
      <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="Company_Edit"
        DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
      <div class="Box">
        <table width="100%" cellpadding="4" cellspacing="0" class="formatTable blue">
          <tr>
            <td align="left" valign="top">
            </td>
            <td align="left" valign="top">
              First
            </td>
            <td align="center" valign="top">
              M.
            </td>
            <td align="left" valign="top">
              Last
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Name:
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="firstname" runat="server" Width="100%" MaxLength="15" onchange="javascript:update_fields('comp_name', 'firstname');"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator3" runat="server" ControlToValidate="firstname" Font-Bold="True"
                ErrorMessage="First Name is Required" ValidationGroup="Contact_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
            </td>
            <td align="center" valign="top">
              <asp:TextBox ID="middle" runat="server" Width="10" MaxLength="1" onchange="javascript:update_fields('comp_name', 'firstname');"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="lastname" runat="server" Width="98%" MaxLength="25" onchange="javascript:update_fields('comp_name', 'firstname');"></asp:TextBox><asp:RequiredFieldValidator
                ID="RequiredFieldValidator4" runat="server" ControlToValidate="lastname" Font-Bold="True"
                ErrorMessage="Last Name is Required" ValidationGroup="Contact_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Title:
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="contact_title" runat="server" Width="100%" MaxLength="40"></asp:TextBox>
            </td>
            <td align="left" valign="top" colspan="2">
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Email
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="Email" runat="server" Width="100%" MaxLength="70" onchange="javascript:update_fields('comp_email', 'Email');"></asp:TextBox>
            </td>
            <td align="left" valign="top" colspan="2">
            </td>
          </tr>
        </table>
      </div><br />
      <div class="Box">
        <div class="subHeader padding_left">
          Phone Numbers</div>
        <br />
        <table width="100%" cellpadding="4" cellspacing="0">
          <tr>
            <td align="left" valign="top">
              Phone Type<br />
              <asp:DropDownList ID="type1" runat="server" Width="100%" onchange="javascript:update_fields('cphone_type1', 'type1');">
              </asp:DropDownList>
              <asp:CustomValidator ID="CustomValidator1" runat="server" OnServerValidate="TextValidate"
                ControlToValidate="phone1" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                ValidationGroup="Contact_Edit" Text="" Display="None">
              </asp:CustomValidator>
            </td>
            <td align="left" valign="top">
              Phone #<br />
              <asp:TextBox ID="phone1" runat="server" MaxLength="28" Width="100%" onchange="javascript:update_fields('cphone1', 'phone1');"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:DropDownList ID="type2" runat="server" Width="100%" onchange="javascript:update_fields('cphone_type2', 'type2');">
              </asp:DropDownList>
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="phone2" runat="server" MaxLength="28" Width="100%" onchange="javascript:update_fields('cphone2', 'phone2');"></asp:TextBox>
              <asp:CustomValidator ID="CustomValidator3" runat="server" OnServerValidate="TextValidate"
                ControlToValidate="phone2" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                ValidationGroup="Contact_Edit" Text="" Display="None">
              </asp:CustomValidator>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:DropDownList ID="type3" runat="server" Width="100%" onchange="javascript:update_fields('cphone_type3', 'type3');">
              </asp:DropDownList>
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="phone3" runat="server" MaxLength="28" Width="100%" onchange="javascript:update_fields('cphone3', 'phone3');"></asp:TextBox>
              <asp:CustomValidator ID="CustomValidator5" runat="server" OnServerValidate="TextValidate"
                ControlToValidate="phone3" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                ValidationGroup="Contact_Edit" Text="" Display="None">
              </asp:CustomValidator>
            </td>
          </tr>
        </table>
      </div>
      <asp:Panel runat="server" ID="contactTypeAircraft" Visible="false">
        <div class="Box">
          <div class="subHeader">
            Contact Relationship to selected Aircraft</div>
          <br />
          <table width="100%" cellpadding="4" cellspacing="0">
            <tr>
              <td align="left" valign="top">
                Relationship:
              </td>
              <td align="left" valign="top">
                <asp:DropDownList runat="server" ID="contactRelationship">
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="contactRelationshipRequired" ControlToValidate="contactRelationship"
                  ErrorMessage="Contact Relationship is Required" Font-Bold="True" ValidationGroup="Contact_Edit"
                  Text="" Display="None"></asp:RequiredFieldValidator>
              </td>
              <td align="left" valign="top">
                Priority:
              </td>
              <td align="left" valign="top">
                <asp:DropDownList runat="server" ID="contactRelationshipPriority">
                  <asp:ListItem Value="" Selected="True">NONE</asp:ListItem>
                  <asp:ListItem Value="1">PRIMARY</asp:ListItem>
                  <asp:ListItem Value="2">SECONDARY</asp:ListItem>
                  <asp:ListItem Value="3">OTHER</asp:ListItem>
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="contactRelationshipPriority"
                  ErrorMessage="Contact Relationship Priority is Required" Font-Bold="True" ValidationGroup="Contact_Edit"
                  Text="" Display="None"></asp:RequiredFieldValidator>
              </td>
            </tr>
          </table>
        </div>
      </asp:Panel><br />
      <div class="Box">
        <div class="subHeader">
          Notes</div>
        <br />
        <table width="100%" cellpadding="4" cellspacing="0" class="formatTable blue">
          <tr>
            <td align="left" colspan="4">
              Note About this Contact: <em>
                <asp:CheckBox ID="enter_as_note" runat="server" Text="Enter as a journal note with todays date?"
                  CssClass="float_right" Checked="true" /></em>
            </td>
          </tr>
          <tr>
            <td align="left" colspan="4">
              <asp:CheckBox ID="attach_note_to_aircraft" Text="Attach this Note to an Aircraft?"
                runat="server" CssClass="float_right" Font-Italic="true" AutoPostBack="true" /><br
                  class="clear" />
            </td>
          </tr>
          <tr>
            <td align="left" colspan="4">
              <asp:CheckBox ID="attach_prospect_aircraft" Text="Add this Company as a prospect for this Aircraft?"
                runat="server" CssClass="float_right" Font-Italic="true" Visible="false" /><br class="clear" />
            </td>
          </tr>
          <tr>
            <td align="left" valign="top" colspan="4">
              <asp:TextBox ID="contact_notes" runat="server" TextMode="MultiLine" Width="100%"
                Height="103px" MaxLength="21845"></asp:TextBox>
            </td>
          </tr>
        </table>
      </div>
    </div>
    <div class="six columns">
      <asp:Panel ID="Panel1" runat="server" CssClass="Box">
        <asp:RadioButtonList ID="company_instructions" runat="server" RepeatDirection="Horizontal"
          AutoPostBack="true" RepeatColumns="3">
          <asp:ListItem Value="auto" Selected="True">Auto Create Company</asp:ListItem>
          <asp:ListItem Value="enter_new">Enter New Company Information</asp:ListItem>
        </asp:RadioButtonList>
      </asp:Panel><br />
      <asp:Panel runat="server" ID="company_panel">
        <div class="Box">
          <table width="100%" cellpadding="4" cellspacing="0" class="formatTable blue">
            <tr>
              <td align="left" valign="top" width="25%">
                <asp:Label ID="comp_name_label" runat="server" Text="Company Name:" ForeColor="#888888"></asp:Label>
              </td>
              <td align="left" valign="top" width="75%">
                <asp:Label ID="comp_name_lbl" runat="server"></asp:Label>
                <asp:TextBox ID="comp_name" runat="server" Height="20px" Width="97%" MaxLength="50"
                  CssClass="display_none"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                    runat="server" ControlToValidate="comp_name" ErrorMessage="Company Name is Required"
                    Font-Bold="True" ValidationGroup="Contact_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                <asp:Label ID="comp_address_label" runat="server" Text="Address:" ForeColor="#888888"></asp:Label>
              </td>
              <td align="left" valign="top" class="style2">
                <asp:Label ID="comp_address_lbl" runat="server"></asp:Label>
                <asp:TextBox ID="comp_address" runat="server" Width="97%" MaxLength="50" CssClass="display_none"></asp:TextBox><asp:RequiredFieldValidator
                  ID="RequiredFieldValidator2" runat="server" ControlToValidate="comp_address" Font-Bold="True"
                  ErrorMessage="Company Address is Required" ValidationGroup="Contact_Edit" Text=""
                  Display="None" Enabled="false"></asp:RequiredFieldValidator>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                <asp:Label ID="comp_city_label" runat="server" Text="City, State, ZIP:" ForeColor="#888888"></asp:Label>
              </td>
              <td align="left" valign="top" class="style4">
                <asp:Label ID="comp_city_lbl" runat="server"></asp:Label>
                <asp:TextBox ID="comp_city" runat="server" Style="margin-bottom: 0px" Width="60%"
                  MaxLength="50" CssClass="display_none"></asp:TextBox>
                <asp:Label ID="comp_state_lbl" runat="server"></asp:Label>
                <asp:TextBox ID="comp_state" runat="server" Width="5%" MaxLength="2" CssClass="display_none"></asp:TextBox>
                <asp:Label ID="comp_zip_lbl" runat="server"></asp:Label>
                <asp:TextBox ID="comp_zip" runat="server" Width="15%" MaxLength="10" CssClass="display_none"></asp:TextBox>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                <asp:Label ID="comp_country_label" runat="server" Text="Country:" ForeColor="#888888"></asp:Label>
              </td>
              <td align="left" valign="top" class="style4">
                <asp:Label ID="comp_country_lbl" runat="server"></asp:Label>
                <asp:TextBox ID="comp_country" runat="server" Width="97%" MaxLength="25" CssClass="display_none"></asp:TextBox><asp:RequiredFieldValidator
                  ID="RequiredFieldValidator12" runat="server" ControlToValidate="comp_country" Font-Bold="True"
                  Enabled="false" ErrorMessage="Company Country is Required" ValidationGroup="Contact_Edit"
                  Text="" Display="None"></asp:RequiredFieldValidator>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                <asp:Label ID="comp_email_label" runat="server" Text="Email Address:" ForeColor="#888888"></asp:Label>
              </td>
              <td align="left" valign="top" class="style2">
                <asp:Label ID="comp_email_lbl" runat="server"></asp:Label>
                <asp:TextBox ID="comp_email" runat="server" Height="16px" Width="97%" MaxLength="70"
                  CssClass="display_none"></asp:TextBox>
              </td>
            </tr>
          </table>
        </div><br />
        <div class="Box">
          <div class="subHeader">
            Phone Numbers</div>
          <br />
          <table width="100%" cellpadding="4" cellspacing="0">
            <tr>
              <td align="left" valign="top">
                <asp:Label ID="comp_phone_type_label" runat="server" Text="Phone Type<br />" ForeColor="#888888"></asp:Label>
              </td>
              <td align="left" valign="top">
                <asp:Label ID="comp_phone_label" runat="server" Text="Phone #<br />" ForeColor="#888888"></asp:Label>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top" width="30%">
                <asp:CustomValidator ID="CustomValidator7" runat="server" OnServerValidate="TextValidate"
                  ControlToValidate="cphone1" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                  ValidationGroup="Contact_Edit" Text="" Display="None">
                </asp:CustomValidator>
                <asp:DropDownList ID="cphone_type1" runat="server" Width="100%" CssClass="display_none">
                </asp:DropDownList>
                <asp:Label ID="cphone_type1_lbl" runat="server"></asp:Label>
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="cphone1" runat="server" MaxLength="28" Width="97%" CssClass="display_none"></asp:TextBox>
                <asp:Label ID="cphone1_lbl" runat="server"></asp:Label>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                <asp:CustomValidator ID="CustomValidator8" runat="server" OnServerValidate="TextValidate"
                  ControlToValidate="cphone2" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                  ValidationGroup="Contact_Edit" Text="" Display="None">
                </asp:CustomValidator>
                <asp:DropDownList ID="cphone_type2" runat="server" Width="100%" CssClass="display_none">
                </asp:DropDownList>
                <asp:Label ID="cphone_type2_lbl" runat="server"></asp:Label>
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="cphone2" runat="server" MaxLength="28" Width="97%" CssClass="display_none"></asp:TextBox>
                <asp:Label ID="cphone2_lbl" runat="server"></asp:Label>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                <asp:CustomValidator ID="CustomValidator9" runat="server" OnServerValidate="TextValidate"
                  ControlToValidate="cphone3" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                  ValidationGroup="Contact_Edit" Text="" Display="None">
                </asp:CustomValidator>
                <asp:DropDownList ID="cphone_type3" runat="server" Width="100%" CssClass="display_none">
                </asp:DropDownList>
                <asp:Label ID="cphone_type3_lbl" runat="server"></asp:Label>
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="cphone3" runat="server" MaxLength="28" Width="97%" CssClass="display_none"></asp:TextBox>
                <asp:Label ID="cphone3_lbl" runat="server"></asp:Label>
              </td>
            </tr>
          </table>
        </div>
      </asp:Panel><br />
      <asp:Panel runat="server" ID="attach_note_to_aircraft_panel" Visible="false" CssClass="padding Box margin-top"
        Width="98%">
        <div class="subHeader">
          Aircraft Information</div>
        <br />
        <asp:Table runat="server" ID="Aircraft_Display_Table" CellPadding="3" Width="100%">
          <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="50%" ID="addAircraftNote">
              <asp:CheckBox runat="server" ID="aircraft_prospects_checkbox" Text="Aircraft Prospects"
                CausesValidation="false" Checked="true" AutoPostBack="true" />
              <asp:CheckBox runat="server" ID="aircraft_search_checkbox" Text="Search Aircraft"
                CausesValidation="false" AutoPostBack="true" />
              <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe1" runat="server" TargetControlID="aircraft_search_checkbox"
                Key="YesNo" />
              <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe2" runat="server" TargetControlID="aircraft_prospects_checkbox"
                Key="YesNo" />
              <br />
              <br />
              <asp:DropDownList runat="server" ID="aircraft_note" Width="99%" AutoPostBack="true">
                <asp:ListItem Value="0||0">Please Select an Aircraft</asp:ListItem>
              </asp:DropDownList>
              <asp:TextBox ID="jetnet_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="client_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="jetnet_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
              <asp:TextBox ID="client_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="left" VerticalAlign="Top" RowSpan="3" Width="50%">
              <asp:Panel ID="aircraft_display_information_panel" runat="server" Visible="false">
                <asp:Label runat="server" ID="aircraft_information"></asp:Label>
              </asp:Panel>
              <asp:Panel runat="server" ID="aircraft_display_search_panel" Visible="false">
                <table width="100%" cellpadding="2" cellspacing="0" class="notes_pnl float_right"
                  border="0">
                  <tr>
                    <td align="left" valign="top">
                    </td>
                    <td align="right" valign="top">
                      <b>Search Parameters</b>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Ser #/Reg #<br />
                      Make/Model:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="serial_number_text" Width="99%" />
                      <asp:RequiredFieldValidator runat="server" ID="required_serial" ControlToValidate="serial_number_text"
                        ValidationGroup="AircraftSearch" ErrorMessage="Please type in Serial # before hitting search"
                        Display="Static"></asp:RequiredFieldValidator>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                    </td>
                    <td align="right" valign="top">
                      <asp:ImageButton ID="ac_search_button" runat="server" ImageUrl="~/images/search_button.jpg"
                        OnClick="SearchAircraftButton" ValidationGroup="AircraftSearch" CausesValidation="true" />
                    </td>
                  </tr>
                </table>
              </asp:Panel>
            </asp:TableCell>
          </asp:TableRow>
          <asp:TableRow ID="aircraft_display_dropdown">
            <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                
            </asp:TableCell>
          </asp:TableRow>
        </asp:Table>
      </asp:Panel>
    </div>
  </div>
  <div class="row">
    <div class="twelve columns padding_left">
      <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
        class="button float_left">Close</a>
      <asp:LinkButton ID="save_quick_entryLB" runat="server" CssClass="button float_right mobile_float_right"
        Text="Save" CausesValidation="true" AlternateText="Update"
        ValidationGroup="Contact_Edit" />
    </div>
  </div>
</asp:Panel>

<script type="text/javascript">
  function update_fields(company_field, contact_field) {

    if (company_field == "comp_name") {
      var comp_name_answer;
      comp_name_answer = "";
      if (document.getElementById("ContactQuickEntry1_firstname").value != "") {
        comp_name_answer = document.getElementById("ContactQuickEntry1_firstname").value
      }

      if (document.getElementById("ContactQuickEntry1_middle").value != "") {
        comp_name_answer = comp_name_answer + " " + document.getElementById("ContactQuickEntry1_middle").value
      }

      if (document.getElementById("ContactQuickEntry1_lastname").value != "") {
        comp_name_answer = comp_name_answer + " " + document.getElementById("ContactQuickEntry1_lastname").value
      }

      document.getElementById("ContactQuickEntry1_comp_name").value = comp_name_answer
      document.getElementById("ContactQuickEntry1_comp_name_lbl").innerHTML = comp_name_answer
    } else {
      document.getElementById("ContactQuickEntry1_" + company_field + "").value = document.getElementById("ContactQuickEntry1_" + contact_field + "").value;
      document.getElementById("ContactQuickEntry1_" + company_field + "_lbl").innerHTML = document.getElementById("ContactQuickEntry1_" + contact_field + "").value;
    }

    if (document.getElementById("ContactQuickEntry1_company_instructions_0").checked == true) {
      document.getElementById("ContactQuickEntry1_" + company_field + "_lbl").className = "";
      document.getElementById("ContactQuickEntry1_" + company_field).className = "display_none";
    } else {
      document.getElementById("ContactQuickEntry1_" + company_field + "_lbl").className = "display_none";
      document.getElementById("ContactQuickEntry1_" + company_field).className = "";
    }

  }
</script>

