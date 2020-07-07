<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Aircraft_Edit_Template.ascx.vb"
  Inherits="crmWebClient.Aircraft_Edit_Template" %>
<link rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:portrait)"
  href="common/ipad-portrait.css" />
<link rel="stylesheet" media="all and (min-device-width: 481px) and (m172.20.101.100ax-device-width: 1024px) and (orientation:landscape)"
  href="common/ipad-landscape.css" />
<link rel="stylesheet" media="all and (min-device-width: 1025px)" href="common/regular.css" />

<div class="container">
<asp:Panel ID="synch" runat="server" CssClass="valueSpec viewValueExport Simplistic aircraftSpec plain" Visible="false">
      <asp:Label ID="sync_edit_text" runat="server"></asp:Label>
<div class="Box">
<div class="subHeader">Client Aircraft Synchronization</div><br />
  <p align="left" class="nonflyout_info_box">
    This facilility is used to automatically copy data from a JETNET aircraft record
    to a corresponding Client Aircraft record.
    <br />
    <br />
    Please note that if you choose to synchronize an area with the corresponding Jetnet
    Aircraft, the client side information will be removed.</p>
  <asp:Label ID="synch_note" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
  <table width="100%" cellpadding="0" cellspacing="0" class="formatTable blue">
    <tr>
      <td align="left" valign="top">
        &nbsp;<b><u>Aircraft Areas to Synchronize:</u></b>
        <div style="padding-left: 10px; padding-top: 10px;">
          <asp:CheckBoxList ID="synch_list" runat="server" AutoPostBack="true" CellPadding="3">
            <asp:ListItem>General/Location/Status</asp:ListItem>
            <asp:ListItem>Features</asp:ListItem>
            <asp:ListItem>Engine</asp:ListItem>
            <asp:ListItem>Avionics</asp:ListItem>
            <asp:ListItem>Usage</asp:ListItem>
            <asp:ListItem>Maintenance</asp:ListItem>
            <asp:ListItem>Equipment</asp:ListItem>
            <asp:ListItem>Interior/Exterior</asp:ListItem>
            <asp:ListItem>Cockpit</asp:ListItem>
            <asp:ListItem>APU</asp:ListItem>
            <asp:ListItem>Aircraft Relationships</asp:ListItem>
          </asp:CheckBoxList>
        </div>
      </td>
      <td align="right" valign="top">
         <asp:Button runat="server" CausesValidation="true" Text="Begin Synchronization"
          ID="synchronize_buttonFunction" Visible="false" />
      </td>
    </tr>
  </table>
  </div>
</asp:Panel>
<asp:Panel ID="subpanel_folder" runat="server" BackColor="White" CssClass="edit_panel"
  Visible="false">
  <h4 align="right">
    Subfolder:</h4>
  <asp:DropDownList ID="add_folder_cbo" runat="server" CssClass="float_right" Visible="false"
    Style="margin-top: 5px; margin-left: 4px;">
  </asp:DropDownList>
  <br clear="all" />
  <br clear="all" />
</asp:Panel>
<asp:Panel ID="aircraft_edit" runat="server" CssClass="valueSpec viewValueExport Simplistic aircraftSpec plain">
  <div style="margin-left: 15px; margin-right: 15px;">
    <asp:Label ID="aircraft_edit_text" runat="server"><h2 class="mainHeading" align="right"><strong>Aircraft Name</strong> Edit</h2></asp:Label>
    <p align="left">
      &nbsp;<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="AC_Edit"
        DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
      <p>
        <asp:CompareValidator ID="CompareValidator1" runat="server" 
          ControlToValidate="date_listed" Display="None" Enabled="false" 
          ErrorMessage="Date Listed must be in Date format." Font-Bold="true" 
          ForeColor="Red" Operator="DataTypeCheck" SetFocusOnError="True" Text="" 
          Type="Date" ValidationGroup="AC_Edit"></asp:CompareValidator>
        <asp:CompareValidator ID="CompareValidator2" runat="server" 
          ControlToValidate="date_purchased" Display="None" Enabled="true" 
          ErrorMessage="Date Purchased must be in Date format." Font-Bold="true" 
          ForeColor="Red" Operator="DataTypeCheck" SetFocusOnError="True" Text="" 
          Type="Date" ValidationGroup="AC_Edit"></asp:CompareValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
          ControlToValidate="asking_price" Display="None" 
          ErrorMessage="*Please Enter an Asking Price." Font-Bold="true" ForeColor="Red" 
          Text="" ValidationGroup="AC_Edit"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
          ControlToValidate="est_price" Display="None" 
          ErrorMessage="*Please enter a Take Price." Font-Bold="true" ForeColor="Red" 
          Text="" ValidationGroup="AC_Edit"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
          ControlToValidate="broker_price" Display="None" 
          ErrorMessage="*Please enter a Broker Price." Font-Bold="true" ForeColor="Red" 
          Text="" ValidationGroup="AC_Edit"></asp:RequiredFieldValidator>
      </p>
      <div class="row">
        <div class="columns sixteen">
          <div class="Box">
            <table cellpadding="0" cellspacing="0" class="formatTable blue" width="100%">
              <tr class="noBorder">
                <td align="left" colspan="4" valign="top">
                  <div class="subHeader">
                    Identification</div>
                </td>
              </tr>
              <tr>
                <td align="left" colspan="4" valign="top">
                  <table cellpadding="4" cellspacing="0" width="100%">
                    <tr>
                      <td align="left" valign="top" width="25%">
                        <asp:Label ID="model_text" runat="Server">Model</asp:Label>
                      </td>
                      <td align="left" valign="top" width="25%">
                        <asp:DropDownList ID="model_cbo" runat="server" alt="Model" Width="90%">
                        </asp:DropDownList>
                      </td>
                      <td align="left" valign="top" width="25%">
                        Year Mfr/Delivered
                      </td>
                      <td align="left" valign="top" width="25%">
                        <asp:TextBox ID="year_manufactured" runat="server" alt="Year MFR" MaxLength="4" 
                          Width="40"></asp:TextBox>
                        /<asp:TextBox ID="year_dlv" runat="server" alt="Year DLV" MaxLength="4" 
                          Width="40"></asp:TextBox>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr class="noBorder">
                <td align="left" colspan="2" valign="top">
                  <asp:Panel ID="model_listing" runat="server" Style="display: none;">
                    <table cellpadding="4" cellspacing="0" class="search_pnl">
                      <tr>
                        <td align="left" colspan="4">
                          <h3>
                            Custom Aircraft:</h3>
                          <asp:Label ID="notCustomACLink" runat="server">&gt;Not a Custom Aircraft?</asp:Label>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top">
                          Make
                        </td>
                        <td align="left" valign="top" width="35%">
                          <asp:TextBox ID="ac_make" runat="server" alt="Make" MaxLength="20"></asp:TextBox>
                        </td>
                        <td align="right" valign="top" width="25%">
                          Model
                        </td>
                        <td align="left" valign="top">
                          <asp:TextBox ID="ac_model" runat="server" alt="Model" MaxLength="20"></asp:TextBox>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top">
                          Make Type
                        </td>
                        <td align="left" valign="top" width="35%">
                          <asp:DropDownList ID="ac_make_type" runat="server">
                          </asp:DropDownList>
                        </td>
                        <td align="right" valign="top" width="25%">
                          Airframe Type
                        </td>
                        <td align="left" valign="top">
                          <asp:DropDownList ID="Airframe_type" runat="server">
                          </asp:DropDownList>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top">
                          Manufacturer Name
                        </td>
                        <td align="left" colspan="3" valign="top">
                          <asp:TextBox ID="ac_manu_name" runat="server" MaxLength="50" Width="250"></asp:TextBox>
                          <asp:TextBox ID="jetnet_amod_id" runat="server" Style="display: none;" 
                            Width="250"></asp:TextBox>
                        </td>
                      </tr>
                    </table>
                  </asp:Panel>
                </td>
              </tr>
              <tr>
                <td align="left" colspan="2" valign="top" width="50%">
                  <table cellpadding="4" cellspacing="0" width="100%">
                    <tr>
                      <td align="left" valign="top" width="50%">
                        Serial #/Alternate Serial #
                      </td>
                      <td align="left" valign="top" width="50%">
                        <asp:TextBox ID="serial" runat="server" alt="Ser #" MaxLength="15" Width="100"></asp:TextBox>
                        &nbsp;
                        <asp:TextBox ID="alternate_serial" runat="server" alt="Alt Ser #" 
                          MaxLength="15" Width="100"></asp:TextBox>
                        <asp:TextBox ID="jetnet_ac" runat="server" Style="display: none;"></asp:TextBox>
                        <asp:TextBox ID="serial_sort" runat="server" Style="display: none;"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td align="left" valign="top">
                        Country of Registration
                      </td>
                      <td align="left" valign="top">
                        <asp:TextBox ID="reg_country" runat="server" alt="Country of Registration" 
                          MaxLength="25" Width="100"></asp:TextBox>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td align="left" colspan="2" valign="top">
                  <table cellpadding="4" width="100%">
                    <tr>
                      <td align="left" valign="top" width="50%">
                        Reg #/Previous Reg #
                      </td>
                      <td align="left" valign="top" width="50%">
                        <asp:TextBox ID="reg" runat="server" alt="Reg #" MaxLength="12" Width="80"></asp:TextBox>
                        &nbsp;<asp:TextBox ID="previous_registration" runat="server" alt="Previous Reg #" 
                          MaxLength="12" Width="80"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td align="left" valign="top" width="50%">
                        Date Purchased
                      </td>
                      <td align="left" valign="top" width="50%">
                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" Format="d" 
                          PopupButtonID="cal_image2" TargetControlID="date_purchased" />
                        <asp:TextBox ID="date_purchased" runat="server" alt="Date Purchased" 
                          MaxLength="15" Width="79"></asp:TextBox>
                        <asp:Image ID="cal_image2" runat="server" ImageUrl="~/images/final.jpg" />
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="columns sixteen">
          <div class="Box">
            <table cellpadding="4" cellspacing="0" class="formatTable blue" width="100%">
              <tr>
                <td align="left" colspan="4" valign="top">
                  <div class="subHeader">
                    Airport/Location Information</div>
                </td>
              </tr>
              <tr>
                <td align="left" colspan="2" valign="top">
                  <table cellpadding="4" cellspacing="0" width="100%">
                    <tr>
                      <td align="left" valign="top" width="50%">
                        Airport IATA Code/ICAO Code
                      </td>
                      <td align="left" valign="top" width="50%">
                        <asp:TextBox ID="iata_code" runat="server" alt="IATA Code" MaxLength="4" 
                          Width="40"></asp:TextBox>
                        &nbsp;<asp:TextBox ID="icao_code" runat="server" alt="ICAO Code" MaxLength="4" 
                          Width="40"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td align="left" valign="top" width="50%">
                        Airport Name
                      </td>
                      <td align="left" valign="top" width="50%">
                        <asp:TextBox ID="airport_name" runat="server" alt="Airport Name" 
                          MaxLength="100" Width="150"></asp:TextBox>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
              <tr>
                <td align="left" colspan="2" valign="top">
                  <table cellpadding="4" cellspacing="0" width="100%">
                    <tr>
                      <td align="left" valign="top" width="50%">
                        Private Airport?
                      </td>
                      <td align="left" valign="top" width="50%">
                        <asp:RadioButtonList ID="airport_private" runat="server" alt="Private Airport" 
                          RepeatDirection="Horizontal">
                          <asp:ListItem ID="private_yes" runat="server" alt="Private Airport - Yes" 
                            Text="Yes" Value="Y" />
                          <asp:ListItem ID="private_no" runat="server" alt="Private Airport - Yes" 
                            Selected="True" Text="No" Value="N" />
                        </asp:RadioButtonList>
                      </td>
                    </tr>
                    <tr>
                      <td align="left" valign="top" width="50%">
                        Airport State/City<br />
                        <br />
                        Airport Country
                      </td>
                      <td align="left" valign="top" width="50%">
                        <asp:TextBox ID="airport_state" runat="server" alt="Airport State" 
                          MaxLength="2" Width="10%"></asp:TextBox>
                        &nbsp;<asp:TextBox ID="aiport_city" runat="server" alt="Airport City" MaxLength="50" 
                          Width="70%"></asp:TextBox>
                        &nbsp;<br />
                        <br />
                        <asp:TextBox ID="airport_country" runat="server" alt="Airport Country" 
                          MaxLength="50" Width="85%"></asp:TextBox>
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="six columns float_left">
          <div class="Box">
            <table cellpadding="4" cellspacing="0" class="formatTable blue" width="100%">
              <tr>
                <td align="left" valign="top" width="50%">
                  <div class="subHeader">
                    Status</div>
                  <table cellpadding="3" cellspacing="0" width="100%">
                    <tr>
                      <td align="left" valign="top" width="25%">
                        Lifecycle?
                      </td>
                      <td align="left" valign="top" width="25%">
                        <asp:DropDownList ID="lifecycle_list" runat="server" alt="Lifecycle" 
                          Width="120">
                          <asp:ListItem Value="1">In Production</asp:ListItem>
                          <asp:ListItem Value="2">New (At MFR)</asp:ListItem>
                          <asp:ListItem Value="3">In Operation</asp:ListItem>
                          <asp:ListItem Value="4">Retired</asp:ListItem>
                        </asp:DropDownList>
                      </td>
                    </tr>
                    <tr>
                      <td align="left" valign="top" width="50%">
                        Ownership
                      </td>
                      <td align="left" valign="top" width="50%">
                        <asp:DropDownList ID="ownership_list" runat="server" alt="Ownership" 
                          Width="120">
                          <asp:ListItem Selected="True" Value="W">Wholly Owned</asp:ListItem>
                          <asp:ListItem Value="C">Co-Owned</asp:ListItem>
                          <asp:ListItem Value="F">Fractionally-Owned</asp:ListItem>
                        </asp:DropDownList>
                      </td>
                    </tr>
                    <asp:Panel ID="aerodex_first" runat="server">
                      <tr>
                        <td align="left" valign="top">
                          New?
                        </td>
                        <td align="left" valign="top">
                          <asp:RadioButtonList ID="new_list" runat="server" alt="New" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem ID="new_yes" runat="server" alt="New - Yes" Text="Yes" 
                              Value="Y" />
                            <asp:ListItem ID="new_no" runat="server" alt="New - No" Selected="True" 
                              Text="No" Value="N" />
                          </asp:RadioButtonList>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top">
                          Lease?
                        </td>
                        <td align="left" valign="top">
                          <asp:RadioButtonList ID="ac_lease" runat="server" alt="Lease" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem ID="lease_yes" runat="server" alt="Lease - Yes" Text="Yes" 
                              Value="Y" />
                            <asp:ListItem ID="lease_no" runat="server" alt="Lease - No" Selected="True" 
                              Text="No" Value="N" />
                          </asp:RadioButtonList>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top">
                          Exclusive?
                        </td>
                        <td align="left" valign="top">
                          <asp:RadioButtonList ID="ac_exclusive" runat="server" alt="Exclusive" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem ID="exclusive_yes" runat="server" alt="Exclusive - Yes" 
                              Text="Yes" Value="Y" />
                            <asp:ListItem ID="exclusive_no" runat="server" alt="Exclusive - Yes" 
                              Selected="True" Text="No" Value="N" />
                          </asp:RadioButtonList>
                        </td>
                      </tr>
                    </asp:Panel>
                  </table>
                </td>
              </tr>
            </table>
          </div>
        </div>
        <div class="six columns float_right">
          <div class="Box">
            <table cellpadding="4" cellspacing="0" class="formatTable blue" width="100%">
              <tr>
                <td align="left" valign="top" width="50%">
                  <div class="subHeader">
                    Airframe</div>
                  <table cellpadding="3" cellspacing="0" width="100%">
                    <tr>
                      <td align="left" colspan="2" valign="top">
                        Times/Values <span class="tiny">
                        <br />
                        Current As Of</span>:
                      </td>
                      <td align="left" valign="top" width="140">
                        <asp:TextBox ID="ac_date_engine_times_as_of" runat="server" 
                          alt="Times/Values Current as of" CssClass="float_left" Width="60px" />
                        <cc1:CalendarExtender ID="CalendarExtender3" runat="server" Format="d" 
                          PopupButtonID="ac_date_times_calendar_image" 
                          TargetControlID="ac_date_engine_times_as_of" />
                        <asp:Image ID="ac_date_times_calendar_image" runat="server" 
                          CssClass="float_left" ImageUrl="~/images/final.jpg" />
                        <asp:CompareValidator ID="CompareValidator3" runat="server" 
                          ControlToValidate="ac_date_engine_times_as_of" Display="Dynamic" Enabled="true" 
                          ErrorMessage="&lt;br clear='all' /&gt;Please Enter a Date" Height="19px" 
                          Operator="DataTypeCheck" SetFocusOnError="True" Type="Date" Width="120px"></asp:CompareValidator>
                      </td>
                    </tr>
                    <tr>
                      <td align="left" colspan="2" valign="top">
                        Air Frame Total Time <span class="tiny">(AFTT)</span>:
                      </td>
                      <td align="left" valign="top">
                        <asp:TextBox ID="ac_airframe_total_hours" runat="server" alt="AFTT" 
                          Width="120px" />
                        <asp:CompareValidator ID="CompareValidator4" runat="server" 
                          ControlToValidate="ac_airframe_total_hours" Display="Dynamic" 
                          ErrorMessage="&lt;br clear='all' /&gt;Please Enter a number" 
                          Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                      </td>
                    </tr>
                    <tr>
                      <td align="left" colspan="2" valign="top">
                        Landings/Cycles:
                      </td>
                      <td align="left" valign="top" width="140">
                        <asp:TextBox ID="ac_airframe_total_landings" runat="server" 
                          alt="Landings/Cycles" Width="120px" />
                        <asp:CompareValidator ID="CompareValidator5" runat="server" 
                          ControlToValidate="ac_airframe_total_landings" Display="Dynamic" 
                          ErrorMessage="&lt;br clear='all'/&gt;Please Enter a number" 
                          Operator="DataTypeCheck" Type="Integer"></asp:CompareValidator>
                        <br />
                      </td>
                    </tr>
                  </table>
                </td>
              </tr>
            </table>
          </div>
        </div>
      </div>
      <div class="row">
        <div class="six columns float_left">
          <div class="Box">
            <table class="formatTable blue" width="100%">
              <tr>
                <td align="left" colspan="2" valign="top">
                  <asp:Label ID="for_sale_header" runat="server">
                <div class="subHeader">For Sale</div></asp:Label>
                  <asp:Panel ID="aerodex_second" runat="server">
                    <table cellpadding="3" cellspacing="0" width="100%">
                      <tr>
                        <td align="left" valign="top" width="33%">
                          For Sale?
                        </td>
                        <td align="left" valign="top" width="35%">
                          <asp:RadioButtonList ID="ac_sale" runat="server" alt="For Sale" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem ID="sale_yes" runat="server" alt="For Sale - Yes" Text="Yes" 
                              Value="Y" />
                            <asp:ListItem ID="sale_no" runat="server" alt="For Sale - No" Selected="True" 
                              Text="No" Value="N" />
                          </asp:RadioButtonList>
                        </td>
                        <td align="left" valign="top" width="20%">
                          Status?
                        </td>
                        <td align="left" valign="top">
                          <asp:DropDownList ID="ac_status_for_sale" runat="server" alt="Status">
                            <asp:ListItem Selected="True" Value="">Please Choose One</asp:ListItem>
                            <asp:ListItem Value="Deal">Deal</asp:ListItem>
                            <asp:ListItem Value="For Sale">For Sale</asp:ListItem>
                            <asp:ListItem Value="For Sale/Best Deal">For Sale/Best Deal</asp:ListItem>
                            <asp:ListItem Value="For Sale/Lease">For Sale/Lease</asp:ListItem>
                            <asp:ListItem Value="For Sale/Off Market">For Sale/Off Market</asp:ListItem>
                            <asp:ListItem Value="For Sale/Possible">For Sale/Possible</asp:ListItem>
                            <asp:ListItem Value="For Sale/Trade">For Sale/Trade</asp:ListItem>
                            <asp:ListItem Value="For Sale/Share">For Sale/Share</asp:ListItem>
                            <asp:ListItem Value="Other">Other</asp:ListItem>
                            <asp:ListItem Value="Sale Pending">Sale Pending</asp:ListItem>
                            <asp:ListItem Value="Unconfirmed">Unconfirmed</asp:ListItem>
                          </asp:DropDownList>
                          <asp:DropDownList ID="ac_status_not_for_sale" runat="server" alt="Status">
                            <asp:ListItem Selected="True" Value="">Please Choose One</asp:ListItem>
                            <asp:ListItem Value="Not For Sale">Not For Sale</asp:ListItem>
                          </asp:DropDownList>
                          <asp:DropDownList ID="ac_status_not_for_sale_withdrawn" runat="server" 
                            alt="Status">
                            <asp:ListItem Selected="True" Value="">Please Choose One</asp:ListItem>
                            <asp:ListItem Value="Withdrawn from Use">Withdrawn from Use</asp:ListItem>
                            <asp:ListItem Value="Withdrawn from Use – Display">Withdrawn from Use – Display</asp:ListItem>
                            <asp:ListItem Value="Withdrawn from Use – Stored">Withdrawn from Use – Stored</asp:ListItem>
                            <asp:ListItem Value="Withdrawn from Use – Tech School">Withdrawn from Use – Tech School</asp:ListItem>
                            <asp:ListItem Value="Written Off">Written Off</asp:ListItem>
                            <asp:ListItem Value="Written Accident">Written Accident</asp:ListItem>
                            <asp:ListItem Value="Written Damage">Written Damage</asp:ListItem>
                            <asp:ListItem Value="Written Display">Written Display</asp:ListItem>
                            <asp:ListItem Value="Written - Fire">Written - Fire</asp:ListItem>
                            <asp:ListItem Value="Written - War Casualty">Written - War Casualty</asp:ListItem>
                            <asp:ListItem Value="Stolen">Stolen</asp:ListItem>
                            <asp:ListItem Value="Other">Other</asp:ListItem>
                          </asp:DropDownList>
                          <asp:TextBox ID="ac_status_hold" runat="server" Style="display: none;" 
                            Width="40"></asp:TextBox>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top">
                          <span class="help_cursor" 
                            title="Describe attributes of this aircraft directly impacting the price such as inspections, maintenance, damage, low/high hours, etc.">
                          Value/Price Description&nbsp;<img 
                            alt="Describe attributes of this aircraft directly impacting the price such as inspections, maintenance, damage, low/high hours, etc." 
                            src="../images/magnify_small.png" /></span>
                        </td>
                        <td align="left" colspan="3" valign="top">
                          <asp:TextBox ID="cliaircraft_value_description_text" runat="server" 
                            alt="Value/Price Description" Rows="3" TextMode="MultiLine" Width="100%"></asp:TextBox>
                        </td>
                      </tr>
                      <tr>
                        <td colspan="4" valign="top">
                          <asp:Panel ID="date_listed_panel" runat="server">
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                              <tr>
                                <td align="left" valign="top" width="32%">
                                  Date Listed
                                </td>
                                <td align="left" valign="top">
                                  <cc1:CalendarExtender ID="CalendarExtender2" runat="server" Format="d" 
                                    PopupButtonID="cal_image" TargetControlID="date_listed" />
                                  <asp:TextBox ID="date_listed" runat="server" alt="Date Listed" MaxLength="15" 
                                    Width="70"></asp:TextBox>
                                  <asp:Image ID="cal_image" runat="server" ImageUrl="~/images/final.jpg" />
                                </td>
                                <td align="left" valign="top" width="23%">
                                  <asp:Label ID="DOMWord" runat="server" Text="DOM" CssClass="help_cursor" Font-Underline="true" ToolTip="Days on Market"></asp:Label>
                                </td>
                                <td align="left" valign="top">
                                  <asp:Label ID="DOMlisted" runat="server" Text=""></asp:Label>
                                </td>
                              </tr>
                              <tr>
                                <td align="left" valign="top">
                                  Asking Wordage
                                  <br />
                                  <em class="tiny">Select Price if price is known.</em>
                                </td>
                                <td align="left" valign="top">
                                  <asp:DropDownList ID="asking_wordage" runat="server" alt="Asking Wordage" 
                                    Width="105">
                                    <asp:ListItem Value="">Please Select One</asp:ListItem>
                                    <asp:ListItem Value="Make Offer">Make Offer</asp:ListItem>
                                    <asp:ListItem Value="Price">Price</asp:ListItem>
                                    <asp:ListItem Value="Auction">Auction</asp:ListItem>
                                    <asp:ListItem Value="Sale/Trade">Sale/Trade</asp:ListItem>
                                    <asp:ListItem Value="Sale/Lease">Sale/Lease</asp:ListItem>
                                    <asp:ListItem Value="Sale/Share">Sale/Share</asp:ListItem>
                                    <asp:ListItem Value="Sealed Bid">Sealed Bid</asp:ListItem>
                                    <asp:ListItem Value="Lease">Lease</asp:ListItem>
                                    <asp:ListItem Value="Lease Only">Lease Only</asp:ListItem>
                                    <asp:ListItem Value="Trade">Trade</asp:ListItem>
                                    <asp:ListItem Value="Other">Other</asp:ListItem>
                                  </asp:DropDownList>
                                </td>
                                <td align="left" valign="top">
                                  <asp:Label ID="ask_lbl" runat="server">Asking Price</asp:Label>
                                </td>
                                <td align="left" valign="top" width="100">
                                  <asp:TextBox ID="asking_price" runat="server" alt="Asking Price" MaxLength="20" 
                                    Width="100">0.00</asp:TextBox>
                                </td>
                              </tr>
                              <tr>
                                <td align="left" valign="top">
                                  <asp:Label ID="est_label" runat="server">Take Price</asp:Label>
                                </td>
                                <td align="left" colspan="3" valign="top">
                                  <asp:TextBox ID="est_price" runat="server" alt="Take Price" Height="16px" 
                                    MaxLength="20" Width="100px">0.00</asp:TextBox>
                                </td>
                              </tr>
                              <tr>
                                <td align="left" valign="top">
                                  <asp:Label ID="broker_lbl" runat="server">Broker (Estimated Value) Price</asp:Label>
                                </td>
                                <td align="left" colspan="3" valign="top">
                                  <asp:TextBox ID="broker_price" runat="server" alt="Broker Price" Height="16px" 
                                    MaxLength="20" Width="100px">0.00</asp:TextBox>
                                </td>
                              </tr>
                              <tr>
                                <td align="left" valign="top">
                                </td>
                                <td align="left" colspan="3" valign="top">
                                  <asp:TextBox ID="delivery" runat="server" alt="AC Delivery" MaxLength="45" 
                                    Style="display: none;" Width="120"></asp:TextBox>
                                </td>
                              </tr>
                            </table>
                          </asp:Panel>
                        </td>
                      </tr>
                    </table>
                  </asp:Panel>
                  <br clear="all" />
                  <br />
                  <asp:Panel ID="OffMarketDueToSale" runat="server" Style="padding: 5px;
                  background-color: #fafafa; border: 1px solid #ccc;" Visible="false">
                    <p>
                      This aircraft may no longer be on market due to sale. Please select the related 
                      transaction below to automatically create a client sold record and move the 
                      asking price, take price, and value related notes to the client sold record then 
                      click on the &quot;Off Market Due to Sale&quot; button to complete this action and remove 
                      the aircraft from market.</p>
                    <asp:Label ID="applicableTransactions" runat="server"></asp:Label>
                    <asp:Button ID="changeIntoTransaction" runat="server" CssClass="float_right" 
                      Text="Off Market Due to Sale" />
                    <div class="div_clear">
                    </div>
                  </asp:Panel>
                </td>
              </tr>
            </table>
          </div>
        </div>
        <div class="six columns float_left">
          <div class="Box">
            <asp:Table ID="Table1" runat="server" CellPadding="4" CellSpacing="0" 
              CssClass="formatTable blue" Width="100%">
              <asp:TableRow>
                <asp:TableCell ColumnSpan="2">    
      <div class="subHeader">
        Custom Aircraft Data</div></asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat1row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat1_text" runat="server" Text="Category 1" MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat1" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat2row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat2_text" runat="server" Text="Category 2" MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat2" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat3row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat3_text" runat="server" Text="Category 3" MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat3" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat4row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat4_text" runat="server" Text="Category 4" MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat4" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat5row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat5_text" runat="server" Text="Category 5" MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat5" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat6row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat6_text" runat="server" Text="Category 6" MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat6" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat7row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat7_text" runat="server" Text="Category 7" MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat7" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat8row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat8_text" runat="server" Text="Category 8" MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat8" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat9row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat9_text" runat="server" Text="Category 9" MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat9" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
              <asp:TableRow ID="cat10row" Visible="false">
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:Label ID="ac_cat10_text" runat="server" Text="Category 10" 
                MaxLength="100"></asp:Label>
              </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                <asp:TextBox ID="ac_cat10" runat="server" Width="100%" TextMode="MultiLine" 
                MaxLength="150"></asp:TextBox>
              </asp:TableCell>
              </asp:TableRow>
            </asp:Table>
          </div>
        </div>
      </div>
      <p>
      </p>
      <p>
      </p>
      <p>
      </p>
      <p>
      </p>
      <p>
      </p>
      <p>
      </p>
      <p>
      </p>
      <p>
      </p>
      <p>
      </p>
    </p>
  </div>
</asp:Panel>
<asp:Panel ID="buttons" runat="server" BackColor="#eeeeee">
  <asp:Panel ID="Panel1" runat="server" HorizontalAlign="center">
    <asp:Label ID="update_text" runat="server" Font-Italic="True"></asp:Label>
  </asp:Panel>
  <table width="100%" cellpadding="4" cellspacing="0">
    <tr>
      <td align="left" valign="top">
        <asp:Label runat="server" ID="mobile_close">
                <a href="javascript: self.close ()" class="button">Close</a>
        </asp:Label>
        <asp:Button runat="server" Text="Remove" ID="deleteFunction" OnClientClick="return confirm('Are you sure you would like to remove this Aircraft?');"
          Visible="true" />
      </td>
      <td align="right" valign="top">
      </td>
      <td align="right" valign="top">
        <asp:Button runat="server" CausesValidation="true" Text="Save" ValidationGroup="AC_Edit"
          ID="updateFunction" />
        <asp:TextBox runat="server" ID="logGenerated" CssClass="display_none"></asp:TextBox>
      </td>
    </tr>
  </table>

  <script type="text/javascript">



    //We need to loop through each form on page load and go ahead and add an attribute of the old value, we're going to use the title attribute.
    function runJQuery() {
      //$(document).ready(function() {
      var $inputs = jQuery('#<%= aircraft_edit.clientID %> :input');
      var values = {};
      $inputs.each(function() {

        if ($(this).attr('type') != 'radio') {
          jQuery(this).attr('title', jQuery(this).val());
        } else {
          var isRadio = $(this).prop('checked');
          jQuery(this).attr('title', isRadio);

        }
      });
      //});
    }

    $("form :input").change(function() {
      var ValueToCompare = '';
      if ($(this).attr('type') != 'radio') {
        //not a radio button
        ValueToCompare = jQuery(this).val()
      } else {
        //Is a radio button
        ValueToCompare = String($(this).prop('checked'));
        var otherOption = '';
        otherOption = jQuery(this).attr("id")
        var ending = otherOption.substring(otherOption.length - 2);
        if (ending == '_0') {
          ending = '_1';
        } else {
          ending = '_0';
        }
        otherOption = otherOption.replace('_0', '');
        otherOption = otherOption.replace('_1', '');
        otherOption = otherOption + ending;

        //Since the other option needs to be marked back as false, we do this:
        jQuery("#" + otherOption).attr("data", false);
        //alert('this means OTHER data changed attribute removed');
      }
      //alert(jQuery(this).attr("title") + "!=" + ValueToCompare);

      if (String(jQuery(this).attr("title")) != ValueToCompare) {
        //$(this).closest('form').data('changed', true);
        //alert('this means data changed attribute added');
        jQuery(this).attr("data", true);
      } else {
        //mark it as not changed
        //$(this).closest('form').data('changed', false)
        jQuery(this).attr("data", false);
        // alert('this means data changed attribute removed');
      }

    });
    $('#<%= updateFunction.clientID %>').click(function() {
      GenerateLog();
    });

    function GenerateLog() {
      var alertLog = "";
      var DataChanged = false;
      $('form input').each(function(i, v) {
        // Access input like this:

        if ($(this).attr("data") == 'true') {
          if ($(this).attr('type') != 'radio') {
            alertLog = alertLog + $(this).attr('alt');
            alertLog = alertLog + " changed from '" + $(this).attr('title') + "' to '" + $(this).val() + "'"
            alertLog = alertLog + "*";
            DataChanged = true;
          } else {
            var replaceName = jQuery(this).attr('id');
            var TempChangeDisplay = "";

            if (replaceName.indexOf("_0") != -1) {
              TempChangeDisplay = " - Previous value was No, Changed to Yes";
            } else {
              TempChangeDisplay = " - Previous value was Yes, Changed to No";
            }
            replaceName = replaceName.replace('Aircraft_Edit_Template1_', '');
            replaceName = replaceName.replace('_0', '');
            replaceName = replaceName.replace('_1', '');
            replaceName = replaceName.replace('_', ' ');
            alertLog = alertLog + replaceName + TempChangeDisplay;
            alertLog = alertLog + "*";
            DataChanged = true;
          }
        }

      });

      $('form textarea').each(function(i, v) {
        // Access textarea like this:
        if ($(this).attr("data") == 'true') {
          alertLog = alertLog + $(this).attr('alt');
          alertLog = alertLog + " changed from '" + $(this).attr('title') + "' to '" + $(this).val() + "'"
          alertLog = alertLog + "*";
          DataChanged = true;
        }

      });

      $('form select').each(function(i, v) {
        // Access select box like this:
        if ($(this).attr("data") == 'true') {
          alertLog = alertLog + $(this).attr('alt');
          alertLog = alertLog + " changed from '" + $(this).attr('title') + "' to '" + $(this).val() + "'"
          alertLog = alertLog + "*";
          DataChanged = true;
        }

      });
      if (DataChanged) {
        //      var re = new RegExp("/*/", "g");
        //      
        //      var alertDisplay = '';
        //      alertDisplay = alertLog.replace(re, '\n');
        //alert('Data has been changed:\n' + alertLog);
        jQuery('#<%= logGenerated.clientID %>').val(alertLog);
        //return false;
      }
    }
  </script>

  <br clear="all" />
</asp:Panel>
</div>