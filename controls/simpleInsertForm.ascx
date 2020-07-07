<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="simpleInsertForm.ascx.vb"
  Inherits="crmWebClient.simpleInsertForm" %>

<script type="text/javascript">
  //function ChangeCalendarView(sender,args){
  //  sender._switchMode("years", true);
  //}

  function checkDateSimple(sender, args) {

    var txtDateID = document.getElementById("<%= txtDateID.ClientID %>");
    var selectedDate = formatDateTime(cDate(txtDateID.value), vbShortDate);

    var rightNow = formatDateTime(new Date(), vbShortDate);


    var isNote = false;

    //alert("rightNow : " + rightNow);
    //alert("selectedDate : " + selectedDate);

    if (isNote) {
      //alert("note : isNote : " + isNote);
      //alert("check : " + (cDate(selectedDate, vbShortDate) > cDate(rightNow, vbShortDate)) );
      if (cDate(selectedDate, vbShortDate) > cDate(rightNow, vbShortDate)) {
        alert("You cannot select a day later than today!");
        sender._selectedDate = new Date();
        // set the date back to the current date
        sender._textbox.set_Value(sender._selectedDate.format(sender._format))
      }
    }
    else {
      //alert("action : isNote : " + isNote);
      //alert("check : " + (cDate(selectedDate, vbShortDate) < cDate(rightNow, vbShortDate)) );
      if (cDate(selectedDate, vbShortDate) < cDate(rightNow, vbShortDate)) {
        alert("You cannot select a day earlier than today!");
        sender._selectedDate = new Date();
        // set the date back to the current date
        sender._textbox.set_Value(sender._selectedDate.format(sender._format))
      }
    }
  }
 
</script>
<style type="text/css">
.ajax__calendar_container, .ajax__calendar_body{width:230px;}
</style>
<link href="EvoStyles/stylesheets/tableThemes.css" type="text/css" rel="stylesheet" />
<div class="DetailsBrowseTable">
  <span class="backgroundShade"><a href="#" class="gray_button float_right noBefore"
    onclick="javascript:window.close();"><img src="/images/x.svg" alt="Close" /></a></span><div class="clear">
    </div>
</div>
<div class="aircraftContainer">
<div class="sixteen columns">
  <div class="row remove_margin">
    <div class="twelve columns remove_margin">
      <asp:Label ID="headerLabel" runat="server" Text="" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"
        Visible="true"></asp:Label></div>
  </div>
  <div class="row remove_margin">
    <div class="six columns remove_margin">
      <asp:Label ID="ModelID" runat="server" Text="" CssClass="display_none"></asp:Label>
      <asp:Label ID="YachtModelID" runat="server" Text="" CssClass="display_none"></asp:Label>
      <asp:Label ID="aircraft_information" runat="server" Text="" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"
        Visible="false"></asp:Label>
         <asp:Label ID="yacht_information" runat="server" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"></asp:Label>
      <cc1:TabContainer ID="yacht_container_tab" runat="server" CssClass="dark-theme" Visible="false"
        Width="100%" AutoPostBack="false">
        <cc1:TabPanel ID="yacht_features_tab" runat="server" HeaderText="Features">
          <ContentTemplate>
            <table width="100%" cellpadding="3" cellspacing="0" class="alt_row border">
              <tr>
                <td align="left" valign="top">
                 
                </td>
              </tr>
            </table>
          </ContentTemplate>
        </cc1:TabPanel>
      </cc1:TabContainer>
      <asp:Label ID="company_information" runat="server" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"></asp:Label>
      <cc1:TabContainer ID="company_container_tab" runat="server" CssClass="dark-theme"
        Visible="false" Width="100%" AutoPostBack="false">
        <cc1:TabPanel ID="company_features_tab" runat="server" HeaderText="Features">
          <ContentTemplate>
            <table width="100%" cellpadding="3" cellspacing="0" class="alt_row border">
              <tr>
                <td align="left" valign="top">
                </td>
              </tr>
            </table>
          </ContentTemplate>
        </cc1:TabPanel>
      </cc1:TabContainer>
    </div>
    <div class="six columns remove_margin" style="margin-left:2% !important;">
      <div class="valueSpec viewValueExport Simplistic aircraftSpec">
        <div class="Box removeTopPadding">
          <table class="formatTable large blue" width="100%" cellpadding="0" cellspacing="0">
            <tr class="noBorder">
              <td align="left" valign="top">
                <div class="subHeader padded_left" runat="server" id="recordHeading">
                  Record Information</div>
              </td>
            </tr>
            <tr class="noBorder">
              <td align="left" valign="middle" style="margin-left: 10px" class="remove_padding">
                <asp:Label ID="itemErrorLblID" runat="server" Font-Bold="True" ForeColor="Red" Font-Size="Large" CssClass="display_none"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Select User Name.<br />"
                  Display="Dynamic" SetFocusOnError="True" Font-Bold="True" ControlToValidate="userNameList"></asp:RequiredFieldValidator>
                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="txtDateID"
                  Display="Dynamic" ErrorMessage="Invalid Date.<br />" Operator="DataTypeCheck" Type="Date"
                  Font-Bold="True"></asp:CompareValidator>
                <cc1:ValidatorCalloutExtender ID="CompareValidator1_ValidatorCalloutExtender" runat="server"
                  Enabled="True" TargetControlID="CompareValidator1">
                </cc1:ValidatorCalloutExtender>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                <asp:Label ID="action_to_note_warning" runat="server" ForeColor="Red"></asp:Label>
                <table width="100%" cellpadding="0" cellspacing="0">
                  <tr>
                    <td align="left" valign="top" colspan="2"  class="remove_padding">
                      <asp:UpdatePanel runat="server" ID="update_notes_date">
                        <ContentTemplate>
                          <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="0" CellSpacing="0">
                            <asp:TableRow>
                              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3"  cssclass="remove_padding">
                                <asp:RadioButtonList runat="server" ID="current_or_previous_date" RepeatDirection="Horizontal"
                                  Visible="false" AutoPostBack="true">
                                  <asp:ListItem Value="previous">Previous Date</asp:ListItem>
                                  <asp:ListItem Selected="True" Value="current">Current Date</asp:ListItem>
                                </asp:RadioButtonList>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell HorizontalAlign="Left" VerticalAlign="middle" Width="45px"  cssclass="remove_padding">
                                                           &nbsp;&nbsp;Date/Time:
                              </asp:TableCell>
                              <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ID="current_date_label"
                                CssClass="display_none">
                                <asp:Label runat="server" ID="entryDate"></asp:Label>
                              </asp:TableCell>
                              <asp:TableCell HorizontalAlign="Left" VerticalAlign="bottom" ID="previous_date_text">
                                <asp:TextBox ID="txtDateID" runat="server" Width="60px"></asp:TextBox>
                                <asp:ImageButton runat="server" ID="cal_image" ImageUrl="../images/final.jpg" AlternateText="Click here to display calendar" Visible="false" />
                                &nbsp;&nbsp;<asp:Label runat="server" ID="entryTimeLbl" Text="Time : " Visible="true"></asp:Label>
                                <asp:DropDownList ID="entryTime" runat="server" Visible="true">
                                  <asp:ListItem Value="0">12:00 AM</asp:ListItem>
                                  <asp:ListItem Value="1">1:00 AM</asp:ListItem>
                                  <asp:ListItem Value="2">2:00 AM</asp:ListItem>
                                  <asp:ListItem Value="3">3:00 AM</asp:ListItem>
                                  <asp:ListItem Value="4">4:00 AM</asp:ListItem>
                                  <asp:ListItem Value="5">5:00 AM</asp:ListItem>
                                  <asp:ListItem Value="6">6:00 AM</asp:ListItem>
                                  <asp:ListItem Value="7">7:00 AM</asp:ListItem>
                                  <asp:ListItem Value="8">8:00 AM</asp:ListItem>
                                  <asp:ListItem Value="9">9:00 AM</asp:ListItem>
                                  <asp:ListItem Value="10">10:00 AM</asp:ListItem>
                                  <asp:ListItem Value="11">11:00 AM</asp:ListItem>
                                  <asp:ListItem Value="12">12:00 PM</asp:ListItem>
                                  <asp:ListItem Value="13">1:00 PM</asp:ListItem>
                                  <asp:ListItem Value="14">2:00 PM</asp:ListItem>
                                  <asp:ListItem Value="15">3:00 PM</asp:ListItem>
                                  <asp:ListItem Value="16">4:00 PM</asp:ListItem>
                                  <asp:ListItem Value="17">5:00 PM</asp:ListItem>
                                  <asp:ListItem Value="18">6:00 PM</asp:ListItem>
                                  <asp:ListItem Value="19">7:00 PM</asp:ListItem>
                                  <asp:ListItem Value="20">8:00 PM</asp:ListItem>
                                  <asp:ListItem Value="21">9:00 PM</asp:ListItem>
                                  <asp:ListItem Value="22">10:00 PM</asp:ListItem>
                                  <asp:ListItem Value="23">11:00 PM</asp:ListItem>
                                </asp:DropDownList>
                              </asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                          <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateID" Enabled="false"
                            PopupButtonID="cal_image" Format="d" OnClientDateSelectionChanged="checkDateSimple">
                          </cc1:CalendarExtender>
                        </ContentTemplate>
                      </asp:UpdatePanel>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="middle">
                      <asp:Label runat="server" ID="statUsedLbl" Text="Status:"></asp:Label>
                    </td>
                    <td align="left" valign="middle">
                      <asp:RadioButtonList ID="statUsed" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Text="Active" Value="P" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Completed" Value="A"></asp:ListItem>
                        <asp:ListItem Text="Dismissed" Value="D"></asp:ListItem>
                      </asp:RadioButtonList>
                    </td>
                  </tr>
                </table>
                <asp:Table ID="editItemTable" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                  <asp:TableRow ID="TableRow1" runat="server">
                    <asp:TableCell ID="TableCell1" HorizontalAlign="Left" VerticalAlign="Top" runat="server">
                      <asp:TextBox ID="notes_edit" runat="server" TextMode="MultiLine" Rows="12" Width="100%"
                        TabIndex="1"></asp:TextBox>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow2" runat="server">
                    <asp:TableCell ID="TableCell2" HorizontalAlign="Left" VerticalAlign="Top" runat="server">
                      <asp:Label runat="server" ID="pertaining_to_lbl"></asp:Label>&nbsp;&nbsp;<asp:DropDownList
                        ID="userNameList" runat="server" AppendDataBoundItems="True" DataTextField="cliuser_full_name"
                        DataValueField="cliuser_id">
                        <asp:ListItem>Select CRM User</asp:ListItem>
                      </asp:DropDownList>
                    </asp:TableCell>
                  </asp:TableRow>
                  <asp:TableRow ID="TableRow3" runat="server">
                    <asp:TableCell ID="TableCell3" HorizontalAlign="Right" VerticalAlign="Top" runat="server">
                      <asp:LinkButton ID="add_note_btn" runat="server" CssClass="button float_right mobile_float_left"
                        OnClientClick="javascript:return EncodeNoteText();">Save</asp:LinkButton>
                      <asp:LinkButton ID="remove_note" runat="server" CssClass="button float_left mobile_float_left"
                        Visible="False" CausesValidation="False">Remove</asp:LinkButton>
                      <asp:Label runat="server" ID="invis_note_text" Visible="false"></asp:Label>
                    </asp:TableCell>
                  </asp:TableRow>
                </asp:Table>
              </td>
            </tr>
          </table>
        </div>
      </div>
    </div>
    <div class="clearfix"></div><br />
  </div>
</div>
</div>
<script type="text/javascript">
  function EncodeNoteText() {
    /*This pattern for regular expression says the following:
    Look for < OR > or || or &&.
    It is followed up by gi which basically tells it to not stop at the first match (g is for global)
    And the i means that its case insensitive.
    'I realize that the case insensitive does not matter with what its looking up, however if this is ever extended
    'to look for certain words, it will matter.*/
    var pattern = /<|>|\|\||&&/gi;
    var value = document.getElementById("<% = notes_edit.ClientID %>").value;
    var matchText = value.match(pattern);
    var matchTextStringDisplay = '';

    if (matchText != null) {
      for (var i = 0, len = matchText.length; i < len; i++) {
        if (matchTextStringDisplay.indexOf(matchText[i]) == -1) {
          if (matchTextStringDisplay != '') {
            matchTextStringDisplay += ', ';
          }
          matchTextStringDisplay += matchText[i];

        } //end index of search
      } //end for 
      //user alert with string of characters they used, none repeating.
      alert("The following characters are considered invalid and are not allowed to be submitted: " + matchTextStringDisplay);
      return false; //stops submittal
    } else { //The return was fine.
      return true; //doesn't stop submittal.
    } //end matchText =! null
  }

  $(document).ready(function() {
    $('#<%=notes_edit.clientID %>').focus();
  });
</script>

