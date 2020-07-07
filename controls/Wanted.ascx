<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Wanted.ascx.vb" Inherits="crmWebClient.Wanted" %>

<script type="text/javascript">
  function checkDate(sender, args) {
    if (sender._selectedDate > new Date()) {
      alert("You cannot select a day later than today.");
      sender._selectedDate = new Date();
      // set the date back to the current date
      sender._textbox.set_Value(sender._selectedDate.format(sender._format))
    }
  }

  function FitPic() {
    window.resizeTo(920, 686);
    self.focus();
  }; 

</script>

<asp:Label ID="mobile_style" runat="server" Text="" Visible="false"> <link href="common/style.css" rel="stylesheet" type="text/css" /></asp:Label>
<asp:Panel runat="server" ID="edit_table">
  <div class="row remove_margin">
    <div class="six columns remove_margin">
      <div class="Box">
        <div class="subHeader">
          Aircraft Information:
        </div>
        <table width="100%" cellpadding="3" cellspacing="0">
          <tr>
            <td align="left" valign="top">
              <asp:CheckBox ID="aircraft_related" runat="server" Text="Aircraft Related to Company"
                Checked="true" AutoPostBack="true" Visible="false" />
            </td>
          </tr>
          <tr>
            <td align="left" valign="top" colspan="3">
              <asp:CheckBox runat="server" ID="add_prospect_automatically_checkbox" Text="Add this company as a Prospect for this Aircraft?"
                CssClass="display_none" />
              <asp:LinkButton ID="AC_Search_Vis" runat="server" Visible="false">Click for AC Search</asp:LinkButton>
              <asp:Panel runat="server" ID="ac_search" Visible="false">
                <table width="100%" align="center" cellpadding="3" cellspacing="0" class="notes_pnl"
                  border="0">
                  <tr>
                    <td align="left" valign="top">
                    </td>
                    <td align="right" valign="top">
                      <b>Search Parameters</b>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top" width="130">
                      <asp:Label ID="ac_search_text" runat="server">Ser #/Reg #/Make/Model:</asp:Label>
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="serial" Width="100%" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                    </td>
                    <td align="right" valign="top">
                      <asp:ImageButton ID="ac_search_button" runat="server" ImageUrl="~/images/search_button.jpg" />
                    </td>
                  </tr>
                </table>
              </asp:Panel>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:DropDownList ID="aircraft_name" runat="server" AutoPostBack="true" Width="350">
              </asp:DropDownList>
              <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="You must choose an Aircraft Model"
                ValueToCompare="0||0" ControlToValidate="aircraft_name" Operator="NotEqual"></asp:CompareValidator>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Label ID="aircraft_info" runat="server" Text=""></asp:Label>
            </td>
          </tr>
        </table>
      </div>
      <!----Company----->
      <div class="Box">
        <div class="subHeader">
          Company Information:
        </div>
        <table width="100%" cellpadding="3" cellspacing="0">
          <tr>
            <td align="left" valign="top">
              <asp:CheckBox ID="company_related" runat="server" Text="Company Related to Aircraft"
                Checked="true" AutoPostBack="true" />
            </td>
          </tr>
          <tr>
            <td colspan="3" align="left" valign="top">
              <asp:LinkButton ID="company_search_vis" runat="server" Visible="false">Click for Company Search</asp:LinkButton>
              <asp:Panel runat="server" ID="company_search" Visible="false">
                <table width="95%" align="center" cellpadding="3" cellspacing="0" class="notes_pnl"
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
                      Company Name:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="Name" Width="164" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      First/Last Name:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="first_name" Width="78" /><asp:TextBox runat="server"
                        ID="last_name" Width="79" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Email Address:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="email_address" Width="164" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Phone Number:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="phone_number" Width="164" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                    </td>
                    <td align="right" valign="top">
                      <asp:ImageButton ID="company_search_button" runat="server" ImageUrl="~/images/search_button.jpg" />
                    </td>
                  </tr>
                </table>
              </asp:Panel>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:DropDownList ID="company_name" runat="server" Width="350" AutoPostBack="true">
              </asp:DropDownList>
              <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="You must choose a Company."
                ValueToCompare="0||0" ControlToValidate="company_name" Operator="NotEqual"></asp:CompareValidator>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Label ID="company_info" runat="server" Text=""></asp:Label>
            </td>
          </tr>
        </table>
      </div>
      <!---Contact--->
      <div class="Box">
        <div class="subHeader">
          Contact Information:
        </div>
        <table width="100%" cellpadding="3" cellspacing="0">
          <tr>
            <td align="left" valign="top">
              <asp:CheckBox ID="contact_related" Visible="false" runat="server" Text="Contacts Related to Company"
                Checked="true" AutoPostBack="true" />
            </td>
          </tr>
          <tr>
            <td colspan="3" align="left" valign="top">
              <asp:LinkButton ID="contact_search_vis" runat="server" Visible="false">Click for Contact Search</asp:LinkButton>
              <asp:Panel runat="server" ID="contact_search" Visible="false">
                <table width="90%" align="center" cellpadding="3" cellspacing="0" class="notes_pnl"
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
                      First Name:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="first" Width="110" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      Last Name:
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="last" Width="110" />
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                    </td>
                    <td align="right" valign="top">
                      <asp:ImageButton ID="contact_search_button" runat="server" ImageUrl="~/images/search_button.jpg" />
                    </td>
                  </tr>
                </table>
              </asp:Panel>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:Label ID="contact_info" runat="server" Text=""></asp:Label>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              <asp:DropDownList ID="contact_name" runat="server" AutoPostBack="true" Width="350">
                <asp:ListItem Value="">PLEASE SELECT A COMPANY</asp:ListItem>
              </asp:DropDownList>
            </td>
          </tr>
        </table>
      </div>
      <!---Display Portion Boxes-->
      <br />
      <br />
    </div>
    <div class="six columns">
      <asp:TextBox ID="jetnet_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
      <asp:TextBox ID="action" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
      <asp:TextBox ID="client_ac" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
      <asp:TextBox ID="jetnet_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
      <asp:TextBox ID="client_comp" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
      <asp:TextBox ID="jetnet_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
      <asp:TextBox ID="client_contact" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
      <asp:TextBox ID="jetnet_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
      <asp:TextBox ID="client_mod" runat="server" Width="70" Text="0" Style="display: none;"></asp:TextBox>
      <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Wanted_Edit"
        DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
      <div class="Box">
        <asp:Panel runat="server" ID="action_view" Visible="false">
          <table width="100%" cellpadding="4" cellspacing="0" class="formatTable blue">
            <tr>
              <td align="left" valign="top">
                Date:
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="dated" runat="server" Width="70"></asp:TextBox>
                <asp:Image runat="server" ID="cal_image" ImageUrl="images/final.jpg" />
                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="dated"
                  Format="d" PopupButtonID="cal_image" />
                &nbsp;&nbsp;Time:&nbsp;&nbsp;
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Priority:
              </td>
              <td align="left" valign="top">
                <asp:DropDownList ID="priority" runat="server">
                </asp:DropDownList>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Status:
              </td>
              <td align="left" valign="top">
                <asp:RadioButtonList ID="statused" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                  <asp:ListItem Text="Active" Value="P" Selected="True"></asp:ListItem>
                  <asp:ListItem Text="Completed" Value="C"></asp:ListItem>
                  <asp:ListItem Text="Dismissed" Value="D"></asp:ListItem>
                </asp:RadioButtonList>
              </td>
            </tr>
          </table>
        </asp:Panel>
        <div>
          <asp:Table ID="Table1" runat="server" CssClass="formatTable blue">
            <asp:TableRow>
              <asp:TableCell ColumnSpan="2" HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="notes_edit" runat="server" TextMode="MultiLine" Width="440" Height="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Enabled="true" runat="server"
                  ControlToValidate="notes_edit" ErrorMessage="Text is Required" ValidationGroup="Wanted_Edit"
                  Text="" Display="None"></asp:RequiredFieldValidator>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            Wanted Entered By:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="pertaining_to" runat="server" Width="120">
                </asp:DropDownList>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Style="display: none;">
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            Category:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="notes_cat" runat="server" Width="120">
                </asp:DropDownList>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                           Year Range:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:DropDownList ID="wanted_year_start" runat="server" Width="55">
                </asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="wanted_year_end" runat="server" Width="55">
                </asp:DropDownList>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                           Max AFTT/Max Price:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:TextBox ID="wanted_max_aftt" runat="server" Width="50"></asp:TextBox>&nbsp;
                <asp:TextBox ID="wanted_max_price" runat="server" Width="50"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator4" runat="server" ErrorMessage="Max AFTT must be numeric."
                  Operator="DataTypeCheck" ControlToValidate="wanted_max_aftt" Type="Double" ValidationGroup="Wanted_Edit"
                  Text="" Display="None"></asp:CompareValidator>
                <asp:CompareValidator ID="CompareValidator5" runat="server" ErrorMessage="Max Price must be a valid Currency. "
                  Operator="DataTypeCheck" ControlToValidate="wanted_max_price" Type="Currency" ValidationGroup="Wanted_Edit"
                  Text="" Display="None"></asp:CompareValidator>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                           History of Damage?:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:RadioButtonList runat="server" ID="wanted_damage_hist" RepeatDirection="Horizontal">
                  <asp:ListItem Value="Y" Text="Yes" />
                  <asp:ListItem Value="N" Text="No" />
                </asp:RadioButtonList>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                            Current Damage?:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                <asp:RadioButtonList runat="server" ID="wanted_damage_cur" RepeatDirection="Horizontal">
                  <asp:ListItem Value="Y" Text="Yes" />
                  <asp:ListItem Value="N" Text="No" />
                </asp:RadioButtonList>
              </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                           Listed Date:
              </asp:TableCell>
              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="notesdate">
                <asp:RadioButtonList ID="curprev" runat="server" AutoPostBack="true" RepeatDirection="Horizontal">
                  <asp:ListItem Value="P" Text="Previous Date"></asp:ListItem>
                </asp:RadioButtonList>
                <br />
                Date/Time:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="current"
                  Visible="false"></asp:Label>
                <asp:TextBox ID="note_date" runat="server" Width="80" Style="margin-left: 2px;"></asp:TextBox><asp:Image
                  runat="server" ID="note_date_image" ImageUrl="../images/final.jpg" />
                <asp:DropDownList ID="time" runat="server" Style="display: none;">
                </asp:DropDownList>
                &nbsp;&nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Enabled="false" runat="server"
                  ControlToValidate="note_date" ErrorMessage="Date is Required"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="note_date"
                  ErrorMessage="Enter a valid start date" Operator="DataTypeCheck" Type="Date" ValidationGroup="Wanted_Edit"
                  Text="" Display="None" />
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="note_date"
                  PopupButtonID="note_date_image" OnClientDateSelectionChanged="checkDate" Format="MM/dd/yyyy" />
                <br />
                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="You cannot select a day later than today"
                  OnServerValidate="checkDate" ControlToValidate="note_date" ValidationGroup="Wanted_Edit"
                  Text="" Display="None"></asp:CustomValidator>
              </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
          <br clear="all" />
          <br clear="all" />
          <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
            class="button float_left">Close</a>
            <asp:LinkButton ID="add_noteLB" runat="server" CssClass="button float_right mobile_float_right"
              Text="Save" CausesValidation="true" ValidationGroup="Wanted_Edit" />
            <asp:LinkButton ID="removeNoteLB" OnClientClick="return confirm('Are you sure you want to Remove this Note?');"
              runat="server" CssClass="button float_left mobile_float_left" Visible="False" CausesValidation="False">Remove</asp:LinkButton>
                <hr />
            <!--Upload area-->
            <asp:Panel ID="upload_area" runat="server" Visible="false">
              <asp:Label ID="existing_docs" runat="server"></asp:Label><br clear="all" />
              <p id="upload-area">
                <asp:FileUpload ID="FileUpload1" runat="server" /></p>
            </asp:Panel>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    FitPic();
  </script>

</asp:Panel>
