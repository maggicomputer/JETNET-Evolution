<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Aircraft_Edit_Transactions_Tab.ascx.vb"
  Inherits="crmWebClient.Aircraft_Edit_Transactions_Tab" %>
<link rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:portrait)"
  href="common/ipad-portrait.css" />
<link rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:landscape)"
  href="common/ipad-landscape.css" />
<link rel="stylesheet" media="all and (min-device-width: 1025px)" href="common/regular.css" />
<div class="container">
  <asp:Panel ID="aircraft_edit" runat="server" CssClass="valueSpec viewValueExport Simplistic aircraftSpec plain">

    <script language="javascript" type="text/javascript">
      function textMaxLength(obj, maxLength, evt) {
        var charCode = (evt.which) ? evt.which : event.keyCode
        var max = maxLength - 0;
        var text = obj.value;
        if (text.length > max) {
          var ignoreKeys = [8, 46, 37, 38, 39, 40, 35, 36];
          for (i = 0; i < ignoreKeys.length; i++) {
            if (charCode == ignoreKeys[i]) {
              return true;
            }
          }
          return false;
        } else {
          return true;
        }
      } </script>

    <asp:Literal runat="server" ID="includeJqueryTheme"></asp:Literal>
    <asp:Label runat="server" ID="title_change"> <h4 align="right">Transactions Edit</h4></asp:Label>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Trans_Edit"
      DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="subject"
      ErrorMessage="*Subject Required" Font-Bold="True" ValidationGroup="Trans_Edit"
      Text="" Display="None"></asp:RequiredFieldValidator>
    <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="trans_date"
      ErrorMessage="Date must be in dd/mm/yyyy format." Operator="DataTypeCheck" Type="Date"
      Font-Bold="True" ValidationGroup="Trans_Edit" Text="" Display="None"></asp:CompareValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="airframe_total_landings"
      ErrorMessage="*Airframe Total Landings is Required" Font-Bold="True" ValidationGroup="Trans_Edit"
      Text="" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="airframe_total_hours"
      ErrorMessage="*Airframe Total Hours is Required" Font-Bold="True" ValidationGroup="Trans_Edit"
      Text="" Display="None"></asp:RequiredFieldValidator>
    <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToValidate="airframe_total_hours"
      ErrorMessage="*Airframe Total Hours must be a Number" Operator="DataTypeCheck"
      Type="Integer" Font-Bold="True" ValidationGroup="Trans_Edit" Text="" Display="None"></asp:CompareValidator>
    <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToValidate="airframe_total_landings"
      ErrorMessage="*Airframe Total Landings must be a Number" Operator="DataTypeCheck"
      Type="Integer" Font-Bold="True" ValidationGroup="Trans_Edit" Text="" Display="None"></asp:CompareValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="estimated_price"
      ErrorMessage="Take Price is Required" Font-Bold="True" ValidationGroup="Trans_Edit"
      Text="" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Asking Price is Required"
      Font-Bold="True" ControlToValidate="asking" ValidationGroup="Trans_Edit" Text=""
      Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Sold Price is Required"
      Font-Bold="True" ControlToValidate="sold_price" ValidationGroup="Trans_Edit" Text=""
      Display="None"></asp:RequiredFieldValidator>
    <asp:Label ID="ref_adding" runat="server" Font-Bold="True" ForeColor="Red" Text="*Please scroll down to complete your record by adding Transaction References."
      Visible="False"></asp:Label>
    <div class="row">
      <div class="six columns float_left remove_margin">
        <div class="Box remove_margin">
          <table cellpadding="4" cellspacing="0" width="100%" class="formatTable blue">
            <tr class="noBorder">
              <td colspan="4" align="left">
                <div class="subHeader">
                  History Information</div>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Transaction Date
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="trans_date" runat="server" Width="65" CssClass="float_left"></asp:TextBox>
                <asp:Image runat="server" ID="cal_image3" ImageUrl="~/images/final.jpg" CssClass="float_left" />
                <asp:TextBox ID="clitrans_subcategory_code" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="clitrans_subcat_code_part1" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="clitrans_subcat_code_part2" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="clitrans_subcat_code_part3" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="jetnet_ac_id" runat="server" Text="0" Style="display: none;"></asp:TextBox><br />
                <asp:TextBox ID="client_ac_id" runat="server" Text="0" Style="display: none;"></asp:TextBox>
                <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="trans_date"
                  Format="d" PopupButtonID="cal_image3" />
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Type:
              </td>
              <td align="left" valign="top">
                <asp:DropDownList ID="typed" runat="server" Width="180" AutoPostBack="true" Visible="true">
                </asp:DropDownList>
              </td>
              <td align="left" valign="top">
              </td>
              <td align="left" valign="top">
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Internal?
              </td>
              <td align="left" valign="top">
                <asp:RadioButtonList ID="ac_internal" runat="server" RepeatDirection="Horizontal">
                  <asp:ListItem id="internal_yes" runat="server" Value="Y" Text="Yes" />
                  <asp:ListItem id="internal_no" runat="server" Value="N" Text="No" Selected="True" />
                </asp:RadioButtonList>
              </td>
            </tr>
            <tr runat="server" id="retailTransToggle">
              <td align="left" valign="top">
                Retail Transaction?
              </td>
              <td align="left" valign="top">
                <asp:RadioButtonList ID="clitrans_retail_flag_rad" runat="server" RepeatDirection="Horizontal">
                  <asp:ListItem id="retail_yes" runat="server" Value="Y" Text="Yes" />
                  <asp:ListItem id="retail_no" runat="server" Value="N" Text="No" Selected="True" />
                </asp:RadioButtonList>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Deal Type:
              </td>
              <td align="left" valign="top">
                <asp:DropDownList ID="deal_type" runat="server" Width="180" AutoPostBack="false"
                  Visible="true">
                </asp:DropDownList>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Subject<br />
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="subject" runat="server" Width="280" Rows="3" MaxLength="200" onkeypress="return textMaxLength(this, '200', event);"
                  TextMode="MultiLine"></asp:TextBox><br />
                <br />
              </td>
            </tr>
          </table>
        </div>
      </div>
      <div class="six columns float_right remove_margin">
        <div class="Box remove_margin">
          <table cellpadding="4" cellspacing="0" width="100%" class="formatTable blue">
            <tr class="noBorder">
              <td colspan="4" align="left">
                <div class="subHeader">
                  Identification</div>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Model
              </td>
              <td align="left" valign="top">
                <asp:DropDownList ID="model_cbo" runat="server" AutoPostBack="true">
                </asp:DropDownList>
                <asp:Panel ID="model_listing" runat="server" Visible="true">
                  <table class="search_pnl" cellpadding="4" cellspacing="0">
                    <tr>
                      <td align="left" valign="top" width="34">
                        Make/Model:
                      </td>
                      <td align="left" valign="top" colspan="2">
                        <asp:Label ID="ac_make" runat="server" />
                        <asp:Label ID="ac_model" runat="server" />
                      </td>
                    </tr>
                    <tr>
                      <td align="left" valign="top">
                        Make Type:
                      </td>
                      <td align="left" valign="top" width="35%">
                        <asp:Label ID="ac_make_type" runat="server" />
                      </td>
                      <td align="right" valign="top">
                        Airframe Type:
                      </td>
                      <td align="left" valign="top">
                        <asp:Label ID="airframe_type" runat="server" />
                      </td>
                    </tr>
                    <tr>
                      <td align="left" valign="top">
                        Mfr Name:
                      </td>
                      <td align="left" valign="top" colspan="3">
                        <asp:Label ID="ac_manu_name" runat="server" />
                      </td>
                    </tr>
                  </table>
                </asp:Panel>
                <asp:TextBox ID="journ_id" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="journ_ac_id" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="journ_date" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="subcategory" runat="server" Style="display: none;"></asp:TextBox>
                <asp:TextBox ID="journ_jetnet_amod_id" runat="server" Style="display: none;"></asp:TextBox>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Serial #/Reg #
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="serial_nbr" runat="server" MaxLength="15" Width="100"></asp:TextBox>/<asp:TextBox
                  ID="reg_nbr" runat="server" MaxLength="12" Width="80"></asp:TextBox>
              </td>
            </tr>
            <tr>
              <td align="left" valign='top' width="100">
                Year MFR
              </td>
              <td align="left" valign="top">
                <asp:DropDownList ID="year_mfr" runat="server">
                </asp:DropDownList>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                Country of Registration
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="country_reg" runat="server" MaxLength="25" Width="100"></asp:TextBox>
              </td>
            </tr>
          
            <tr>
              <td align="left" valign="top">
                Customer Notes
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="customer_note" runat="server" TextMode="MultiLine" Width="300" Rows="7"
                  MaxLength="250" onkeypress="return textMaxLength(this, '250', event);"></asp:TextBox>
              </td>
              <td align="left" valign="top">
              </td>
              <td align="left" valign="top">
              </td>
            </tr>
          </table>
        </div>
      </div>

      <div class="six columns float_left remove_margin" style="margin-top:10px !important;">
        <div class="Box remove_margin">
          <table class="formatTable blue" cellpadding="4" cellspacing="0" width="100%">
            <tr class="noBorder">
              <td colspan="4" align="left">
                <div class="subHeader">
                  Status</div>
              </td>
            </tr>
            <tr>
              <td align="left" valign='top'>
                <table width="100%" cellpadding="3" cellspacing="0">
                  <tr>
                    <td align="left" valign="top" width="195">
                      New?
                    </td>
                    <td align="left" valign="top">
                      <asp:RadioButtonList ID="new_list" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem id="new_yes" runat="server" Value="Y" Text="Yes" />
                        <asp:ListItem id="new_no" runat="server" Value="N" Text="No" Selected="True" />
                      </asp:RadioButtonList>
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
            <tr>
              <td align="left" valign="top">
                <asp:Label ID="Label1" runat="server"><div class="for_sale"><b>For Sale</b></div></asp:Label><br />
                <table width="100%" cellspacing="0" cellpadding="3" border="0" style="background-color: #CBF0B5;
                  border: 2px solid #98E36A;">
                  <tr>
                    <td align="left" valign="top">
                      For Sale?
                    </td>
                    <td align="left" valign="top">
                      <asp:RadioButtonList ID="for_sale" runat="server" RepeatDirection="Horizontal" AutoPostBack="true">
                        <asp:ListItem id="yes" runat="server" Value="Y" Text="Yes" />
                        <asp:ListItem id="no" runat="server" Value="N" Text="No" Selected="True" />
                      </asp:RadioButtonList>
                    </td>
                  </tr>
                  <tr>
                    <td align="left" valign="top">
                      <span class="help_cursor" title="Describe attributes of this aircraft directly impacting the price such as inspections, maintenance, damage, low/high hours, etc.">
                        Value/Price Description&nbsp;<img src='../images/magnify_small.png' alt="Describe attributes of this aircraft directly impacting the price such as inspections, maintenance, damage, low/high hours, etc." /></span>
                    </td>
                    <td align="left" valign="top">
                      <asp:TextBox runat="server" ID="clitrans_value_description_text" TextMode="MultiLine"
                        Rows="3" Width="100%"></asp:TextBox>
                    </td>
                  </tr>
                  <asp:Panel Visible="true" ID="for_sale_first" runat="server">
                    <tr>
                      <asp:Panel ID="date_listed_panel" runat="server" Visible="false">
                        <td align="left" valign="top">
                          Date Listed
                        </td>
                        <td align="left" valign="top">
                          <asp:TextBox ID="date_listed" runat="server" value="" Width="66" MaxLength="15"></asp:TextBox>
                          <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="date_listed"
                            Format="d" PopupButtonID="cal_image2" />
                          <asp:Image runat="server" ID="cal_image2" ImageUrl="~/images/final.jpg" />
                        </td>
                      </asp:Panel>
                    </tr>
                    <tr>
                      <td align="left" valign="top">
                        <asp:Label runat="server" ID="asking_lbl" Visible="true"> Asking Wordage<br /><em>Select Price if price is known</em></asp:Label>
                      </td>
                      <td align="left" valign="top">
                        <asp:DropDownList ID="asking_wordage" runat="server" Width="180" AutoPostBack="true"
                          Visible="true">
                          <asp:ListItem Value="" Selected="True">Please Select One</asp:ListItem>
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
                    </tr>
                  </asp:Panel>
                  <asp:Panel ID="for_sale_second" Visible="true" runat="server">
                    <tr>
                      <asp:Panel ID="price_vis" runat="Server" Visible="false">
                        <td align="left" valign="top">
                          <asp:Label runat="server" ID="ask_lbl">Asking Price</asp:Label>
                        </td>
                        <td align="left" valign="top">
                          <asp:TextBox ID="asking" runat="server" value="0" Width="90"></asp:TextBox>
                        </td>
                      </asp:Panel>
                    </tr>
                    <tr>
                      <td align="left" valign="top">
                        Take Price
                      </td>
                      <td align="left" valign="top">
                        <asp:TextBox ID="estimated_price" runat="server" value="0" Width="90"></asp:TextBox>
                      </td>
                    </tr>
                    <tr>
                      <td align="left" valign="top">
                        <asp:Label runat="server" ID="sold_lbl">Sold Price</asp:Label>
                      </td>
                      <td align="left" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="0">
                          <tr>
                            <td align="left" valign="top">
                              <asp:TextBox ID="sold_price" runat="server" value="0" Width="90"></asp:TextBox>
                            </td>
                            <td align="left" valign="top">
                              <asp:RadioButtonList ID="sold_price_type" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem id="sold_price_type1" runat="server" Value="F" Text="Firm" />
                                <asp:ListItem id="sold_price_type2" runat="server" Value="E" Text="Estimated" Selected="True" />
                              </asp:RadioButtonList>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </asp:Panel>
                </table>
                <br />
                <asp:Panel ID="share_box" runat="server" Visible="false">
                  <table width="100%">
                    <tr>
                      <td align="left" valign="top" rowspan="6" colspan="2">
                        <asp:Label ID="Label2" runat="server"><div class="shareAgreementHeader"><b>Share My Transaction Data with JETNET</b></div></asp:Label><br />
                        <table width="100%" cellspacing="0" cellpadding="3" border="0" style="background-color: #F6CECE;
                          border: 2px solid #ae0303;">
                          <tr>
                            <td align="left" valign="top">
                              <asp:Label runat="server" ID="share_label_box" Text=""></asp:Label>
                            </td>
                          </tr>
                          <tr>
                            <td>
                              &nbsp;
                            </td>
                          </tr>
                          <tr>
                            <td>
                              <b>Send This Transaction Data to JETNET</b><asp:CheckBox ID="send_check" runat="server" />
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
                  </table>
                </asp:Panel>
              </td>
            </tr>
          </table>
        </div>
      </div>
      <div class="six columns float_right remove_margin" style="margin-top:10px !important;">
      <div class="Box remove_margin">
        <table cellpadding="4" cellspacing="0" width="100%" class="formatTable blue">
          <tr class="noBorder">
            <td colspan="4" align="left">
              <div class="subHeader">
                Airframe</div>
            </td>
          </tr>
            <tr>
              <td align="left" valign="top">
                Airframe Total Hrs/Total Landings
              </td>
              <td align="left" valign="top">
                <asp:TextBox ID="airframe_total_hours" runat="server" value="0" MaxLength="10" Width="80"></asp:TextBox>/<asp:TextBox
                  ID="airframe_total_landings" runat="server" value="0" MaxLength="10" Width="80"></asp:TextBox>
              </td>
            </tr>
        </table>
        </div>
      </div>
    </div>
    <div class="row">
    <div class="Box">
      <table cellpadding="4" cellspacing="0" width="100%" class="formatTable blue">
        <asp:Panel Visible="false" runat="server">
          <tr>
            <td colspan="4" align="left">
              <div class="subHeader padding_left">
                Airport Information</div>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Airport IATA Code
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="iata_code" runat="server" Width="30" MaxLength="4"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              Airport ICAO Code
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="icao_code" runat="server" Width="40" MaxLength="4"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Airport Name
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="airport_name" runat="server" MaxLength="100"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              Airport State
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="airport_state" runat="server" Width="30" MaxLength="2"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Airport Country
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="airport_country" runat="server" MaxLength="50"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              Airport City
            </td>
            <td align="left" valign="top">
              <asp:TextBox ID="aiport_city" runat="server" MaxLength="50"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Private Airport?
            </td>
            <td align="left" valign="top">
              <asp:RadioButtonList ID="airport_private" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem id="private_yes" runat="server" Value="Y" Text="Yes" />
                <asp:ListItem id="private_no" runat="server" Value="N" Text="No" Selected="True" />
              </asp:RadioButtonList>
            </td>
            <td align="left" valign="top">
            </td>
            <td align="left" valign="top">
            </td>
          </tr>
          <tr>
            <td colspan="4" align="left">
              <div class="subHeader padding_left">
                Status Flags</div>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Exclusive?
            </td>
            <td align="left" valign="top">
              <asp:RadioButtonList ID="ac_exclusive" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem id="exclusive_yes" runat="server" Value="Y" Text="Yes" />
                <asp:ListItem id="exclusive_no" runat="server" Value="N" Text="No" Selected="True" />
              </asp:RadioButtonList>
            </td>
            <td align="left" valign="top">
            </td>
            <td align="left" valign="top">
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Lifecycle?
            </td>
            <td align="left" valign="top">
              <asp:RadioButtonList ID="lifecycle_list" runat="server" RepeatDirection="vertical">
                <asp:ListItem id="lifecycle_in_production" runat="server" Value="1" Text="In Production" />
                <asp:ListItem id="lifecycle_new" runat="server" Value="2" Text="New (at MFR)" />
                <asp:ListItem id="lifecycle_operation" runat="server" Value="3" Text="In Operation" />
                <asp:ListItem id="lifecycle_retired" runat="server" Value="4" Text="Retired" />
              </asp:RadioButtonList>
            </td>
            <td align="left" valign="top">
              Ownership
            </td>
            <td align="left" valign="top">
              <asp:RadioButtonList ID="ownership_list" runat="server" RepeatDirection="vertical">
                <asp:ListItem id="ownership_whole" runat="server" Value="W" Text="Wholly Owned" />
                <asp:ListItem id="ownership_co" runat="server" Value="N" Text="Co-Owned" />
                <asp:ListItem id="ownership_fraction" runat="server" Value="F" Text="Fractionally Owned" />
              </asp:RadioButtonList>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top">
              Usage?
            </td>
            <td align="left" valign="top">
              <asp:RadioButtonList ID="usage" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem id="usage_yes" runat="server" Value="Y" Text="Option" />
                <asp:ListItem id="usage_no" runat="server" Value="N" Text="Option" Selected="True" />
              </asp:RadioButtonList>
              <asp:TextBox runat="server" ID="hiddenJetnetAssumeIDRedirect" Style="display: none;"></asp:TextBox>
            </td>
            <td align="left" valign="top">
            </td>
            <td align="left" valign="top">
            </td>
          </tr>
        </asp:Panel>
        <tr>
          <td align="left">
            <asp:Panel runat="server" ID="reference_info" Visible="false">
              <div class="subHeader padding_left">
                COMPANIES/CONTACTS</div>
              <br />
            </asp:Panel>
            <asp:DataGrid runat="server" ID="datagrid2" CellPadding="3" Width="100%" OnEditCommand="MyDataGrid_Edit"
              Visible="true" OnUpdateCommand="MyDataGrid_Update" AllowPaging="false" PageSize="60"
              OnDeleteCommand="MyDataGrid_Delete" OnItemCommand="dispDetails" OnCancelCommand="MyDataGrid_Cancel"
              CssClass="formatTable blue" AllowSorting="True" GridLines="None" AllowCustomPaging="True"
              AutoGenerateColumns="false">
              <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Bold="True" Font-Underline="True"
                ForeColor="White" />
              <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" />
              <HeaderStyle Font-Bold="True" ForeColor="White" Wrap="False" HorizontalAlign="left"
                VerticalAlign="Middle" Height="20px"></HeaderStyle>
              <Columns>
                <asp:TemplateColumn HeaderText="Company">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <asp:TextBox runat="server" ID="company_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clitcref_client_comp_id") %>'
                      Style="display: none;" />
                    <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "clitcref_id") %>'
                      Style="display: none;" />
                    <%#displayCompany(DataBinder.Eval(Container.DataItem, "clitcref_client_comp_id"), True)%>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:LinkButton CssClass="float_right" ID="comp_search" runat="server" Font-Italic="true"
                      Font-Size="Smaller" CommandName="search">Search&nbsp;&nbsp;</asp:LinkButton>
                    <asp:TextBox runat="server" ID="company_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clitcref_client_comp_id") %>'
                      Style="display: none;" />
                    <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "clitcref_id") %>'
                      Style="display: none;" />
                    <asp:DropDownList runat="server" ID="company" Width="150" OnSelectedIndexChanged="swap_company"
                      AutoPostBack="true">
                      <asp:ListItem Selected="True">Please Select One..</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <br />
                    <asp:Panel runat="server" ID="company_search_panel" Visible="false">
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
                            Company:
                          </td>
                          <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="Name" Width="164" />
                          </td>
                        </tr>
                        <tr>
                          <td align="left" valign="top">
                          </td>
                          <td align="right" valign="top">
                            <asp:LinkButton CssClass="float_right" ID="company_search_button" runat="server"
                              Font-Italic="true" Font-Size="Smaller" CommandName="search_me"><img src="images/search_button.jpg" alt="Search"  border="0"/></asp:LinkButton>
                          </td>
                        </tr>
                      </table>
                    </asp:Panel>
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Contact">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <asp:TextBox runat="server" ID="contact_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clitcref_client_contact_id") %>'
                      Style="display: none;" />
                    <%#displayContact(DataBinder.Eval(Container.DataItem, "clitcref_client_contact_id"), True)%>
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox runat="server" ID="contact_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clitcref_client_contact_id") %>'
                      Style="display: none;" />
                    <asp:DropDownList runat="server" ID="contact" Width="150">
                      <asp:ListItem Selected="True">Please Select One..</asp:ListItem>
                    </asp:DropDownList>
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Relationship">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#whatRelationship(DataBinder.Eval(Container.DataItem, "clitcref_contact_type"))%>
                    <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clitcref_contact_type") %>'
                      Style="display: none;" />
                  </ItemTemplate>
                  <EditItemTemplate>
                    <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clitcref_contact_type") %>'
                      Style="display: none;" />
                    <asp:DropDownList runat="server" ID="contact_type">
                      <asp:ListItem Selected="True">Please Select One..</asp:ListItem>
                    </asp:DropDownList>
                  </EditItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                  <ItemTemplate>
                    <asp:LinkButton ID="LinkButton1" CommandName="Delete" Text="Delete" runat="server" /></ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                  <ItemTemplate>
                    <asp:LinkButton ID="Cancel" CommandName="Cancel" Text="Cancel" runat="server" Visible="false" /></ItemTemplate>
                </asp:TemplateColumn>
              </Columns>
            </asp:DataGrid>
            <asp:DataGrid runat="server" ID="datagrid1" BorderColor="White" BorderStyle="None"
              CellPadding="3" BackColor="White" Width="100%" Visible="true" AllowPaging="false"
              PageSize="60" CssClass="formatTable blue" AllowSorting="True" Font-Names="tahoma"
              AllowCustomPaging="True" AutoGenerateColumns="false" GridLines="None">
              <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" Font-Bold="True" Font-Underline="True"
                ForeColor="White" />
              <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" />
              <HeaderStyle Font-Bold="True" Font-Underline="True" ForeColor="White" Wrap="False"
                HorizontalAlign="left" VerticalAlign="Middle" Height="20px"></HeaderStyle>
              <Columns>
                <asp:TemplateColumn HeaderText="Company">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#displayCompany(DataBinder.Eval(Container.DataItem, "tacref_comp_id"), False)%>
                  </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Contact">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#displayContact(DataBinder.Eval(Container.DataItem, "tacref_contact_id"), False)%>
                  </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="Relationship">
                  <ItemTemplate>
                    <itemstyle width="180px" horizontalalign="center" verticalalign="top" />
                    <headerstyle width="180px" />
                    <%#whatRelationship(DataBinder.Eval(Container.DataItem, "tacref_contact_type"))%>
                  </ItemTemplate>
                </asp:TemplateColumn>
              </Columns>
            </asp:DataGrid>
          </td>
        </tr>
        <tr>
          <td align="left" valign="top">
            <asp:Label ID="attention" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
            <asp:LinkButton ID="insert_row" runat="server" CssClass="float_right" Font-Bold="true">ADD REFRENCE</asp:LinkButton><br />
            <asp:Panel ID="new_row" Visible="false" runat="server">
              <table width="800" cellpadding="3">
                <tr>
                  <td align="left" valign="top">
                    <b><u>Company</u></b>
                  </td>
                  <td align="left" valign="top">
                    <b><u>Relationship</u></b>
                  </td>
                  <td align="left" valign="top">
                    &nbsp;
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" width="550">
                    <asp:ListBox runat="server" ID="row_company" Width="550" Visible="false" OnSelectedIndexChanged="insert_row_change"
                      AutoPostBack="true">
                      <asp:ListItem Selected="True" Value="">Please Select One..</asp:ListItem>
                    </asp:ListBox>
                    <asp:LinkButton ID="comp_search_row" runat="server" Font-Italic="true" Font-Size="Smaller"
                      CommandName="search" Visible="false" CausesValidation="false">Search Companies&nbsp;&nbsp;</asp:LinkButton>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="row_company"
                      ErrorMessage="Company is required." ValidationGroup="Trans_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
                    <asp:Panel Visible="false" runat="server" ID="contact_drop">
                      <br />
                      <b><u>Contact</u></b><br />
                      <br />
                      <asp:ListBox runat="server" ID="row_contact" Width="550" AutoPostBack="false">
                        <asp:ListItem Selected="True" Value="0">Please Select One..</asp:ListItem>
                      </asp:ListBox>
                      <br clear="all" />
                      <br />
                      <br />
                    </asp:Panel>
                    <asp:Panel runat="server" ID="company_search_panel_row">
                      <table width="75%" align="left" cellpadding="3" cellspacing="0" class="notes_pnl"
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
                            Company:
                          </td>
                          <td align="left" valign="top">
                            <asp:TextBox runat="server" ID="row_Name" Width="164" />
                          </td>
                        </tr>
                        <tr>
                          <td align="left" valign="top">
                          </td>
                          <td align="right" valign="top">
                            <asp:LinkButton CssClass="float_right" ID="company_search_button" runat="server"
                              Font-Italic="true" Font-Size="Smaller" CommandName="search_me" CausesValidation="false"><img src="images/search_button.jpg" alt="Search"  border="0"/></asp:LinkButton>
                          </td>
                        </tr>
                      </table>
                    </asp:Panel>
                  </td>
                  <td align="left" valign="top">
                    <asp:DropDownList runat="server" ID="row_contact_type" Width="150">
                      <asp:ListItem Selected="True" Value="">Please Select One..</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="row_contact_type"
                      ErrorMessage="Relationship is required." ValidationGroup="Trans_Edit" Text="" Display="None"></asp:RequiredFieldValidator>
                  </td>
                  <td align="left" valign="top" width="40">
                    <asp:LinkButton ID="cancel" runat="server" CssClass="float_right" Font-Bold="true">Cancel</asp:LinkButton>
                  </td>
                  <td align="left" valign="top" width="40">
                    <asp:LinkButton ID="save_row" runat="server" CssClass="float_right" Font-Bold="true"
                      ValidationGroup="Trans_Edit">Save</asp:LinkButton>
                  </td>
                </tr>
              </table>
            </asp:Panel>
          </td>
        </tr>
        <tr>
          <td align="left">
          </td>
        </tr>
      </table>
      </div>
     </div>
      <asp:Panel ID="buttons" runat="server">
        <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Right">
          <asp:Label ID="update_text" runat="server" Font-Italic="True"></asp:Label>
        </asp:Panel>
        <table width="100%" cellpadding="4" cellspacing="0">
          <tr>
            <td align="left" valign="top">
              <a href="javascript: window.opener.location.href = window.opener.location.href; self.close();"
                class="button">Close</a>
            </td>
            <td align="right" valign="top">
              <asp:Button runat="server" ID="removeButton" Visible="false" CausesValidation="false"
                CssClass="button" Text="Remove" />
              <asp:Button runat="server" CausesValidation="true" ID="updateButton" ValidationGroup="Trans_Edit"
                Text="Save" CssClass="button" />
            </td>
          </tr>
        </table>
      </asp:Panel>

      <script>
        function showPopup(val, option) {
          if (option == 1) {
            if (val == '0' || val == '') {
              alert('Note that removing your sale price for this transaction from the Marketplace Manager DOES NOT automatically remove the data from display to JETNET customers. If you would like this transaction data removed from JETNET, please contact your JETNET representative at 1-800-553-8638.');
            }
          } else {
            return confirm('Note that removing your sale price for this transaction from the Marketplace Manager DOES NOT automatically remove the data from display to JETNET customers. If you would like this transaction data removed from JETNET, please contact your JETNET representative at 1-800-553-8638.');

          }
        }
    
      </script>

  </asp:Panel>
</div>
