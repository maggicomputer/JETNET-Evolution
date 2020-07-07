<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Company_Edit_Template.ascx.vb"
  Inherits="crmWebClient.Company_Edit_Template" %>
<style type="text/css">
  .style1
  {
    width: 552px;
  }
  .style2
  {
    width: 509px;
  }
  .style3
  {
    width: 496px;
  }
  .style4
  {
    width: 644px;
  }
  .style5
  {
    width: 684px;
  }
  .style6
  {
    width: 779px;
  }
</style>
<link rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:portrait)"
  href="common/ipad-portrait.css" />
<link rel="stylesheet" media="all and (min-device-width: 481px) and (max-device-width: 1024px) and (orientation:landscape)"
  href="common/ipad-landscape.css" />
<link rel="stylesheet" media="all and (min-device-width: 1025px)" href="common/regular.css" />
<asp:Panel runat="server" ID="connect_company_table" CssClass="edit_panel" BackColor="white"
  Visible="true">
  <asp:Label runat="server" ID="Attention_connect" ForeColor="Red"></asp:Label>
  <table class="body_width" cellspacing="4" cellpadding="5" border="0">
    <tr>
      <td align="left" valign="top" width="25%" bgcolor="#F5F5F5" style="border-right: 1px solid #dddddd">
        <h4>
          Client Company</h4>
        <br />
        <asp:Label ID="connect_main_company" runat="server" Text="Label"></asp:Label>
      </td>
      <td align="left" valign="top" width="50%">
        <h4>
          Search Jetnet Companies to Relate</h4>
        <table width="100%" cellpadding="0" cellspacing="0">
          <tr>
            <td align="left" valign="top" width="100%">
              <table width="100%" cellpadding="4" cellspacing="2" border="0">
                <tr>
                  <td align="left" valign="top">
                    Name:
                  </td>
                  <td align="left" valign="top" colspan="5">
                    <asp:TextBox ID="connect_company" runat="server" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Address:
                  </td>
                  <td align="left" valign="top" colspan="5">
                    <asp:TextBox ID="connect_address" runat="server" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    City:
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="connect_city" runat="server" Width="100%"></asp:TextBox>
                  </td>
                  <td align="left" valign="top">
                    <asp:Label runat="server" ID="state_connect_label" Visible="false">State</asp:Label>
                  </td>
                  <td align="left" valign="top">
                    <asp:DropDownList ID="connect_state" runat="server" Width="75px" Visible="false" />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Country:
                  </td>
                  <td align="left" valign="top">
                    <asp:DropDownList ID="connect_country" runat="server" Width="100%" AutoPostBack="True">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    Zip/Postal:
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="connect_zip" runat="server" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="right" valign="top" colspan="6">
                    <asp:ImageButton ID="connect_company_search" ImageUrl="~/images/search_button.jpg"
                      runat="server" />&nbsp;&nbsp;
                  </td>
                </tr>
              </table>
            </td>
          </tr>
          <tr>
            <td align="left" valign="top" colspan="2">
              <br />
              <br />
              <asp:DropDownList Enabled="false" ID="connect_company_list" runat="server" Width="100%"
                AutoPostBack="true">
              </asp:DropDownList>
            </td>
          </tr>
        </table>
      </td>
      <td align="left" valign="top" bgcolor="#F5F5F5" style="border-left: 1px solid #dddddd">
        <h4>
          Jetnet Company Details</h4>
        <br clear="all" />
        <asp:Label ID="connect_company_details" runat="server" Text=""></asp:Label>
        <p align="center">
          <asp:ImageButton ID="connect_remove" OnClientClick="return confirm('Are you sure you'd like to remove this relationship?.');"
            ImageUrl="~/images/remove.gif" AlternateText="Remove Relationship" runat="server"
            Visible="false" /></p>
        <img src="images/spacer.gif" alt="" width="250" height="1" />
      </td>
    </tr>
    <tr>
      <td align="right" valign="top" colspan="3">
        <asp:ImageButton ID="connect_me" OnClientClick="return confirm('Clicking on this will connect the two companies.');"
          ImageUrl="~/images/connect_companies.jpg" AlternateText="Connect Companies" runat="server"
          Visible="false" />
      </td>
    </tr>
  </table>
</asp:Panel>
<asp:Panel runat="server" ID="identify_main" CssClass="edit_panel" BackColor="white"
  Visible="false">
  <asp:Label runat="server" ID="attention_parent" ForeColor="Red"></asp:Label>
  <table class="body_width" cellspacing="4" cellpadding="5" border="0">
    <tr>
      <td align="left" valign="top" width="25%" bgcolor="#F5F5F5" style="border-right: 1px solid #dddddd">
        <h4>
          SELECTED COMPANY</h4>
        <br />
        <asp:Label ID="child_company_text" runat="server" Text="Label"></asp:Label>
      </td>
      <td align="left" valign="top" width="50%">
        <h4>
          SEARCH TO ASSOCIATE MAIN LOCATION</h4>
        <p class="nonflyout_info_box">
          The purpose of this form is to identify a main location for the selected company.
          Simply type in the main company name and click search to obtain a list of client
          companies meeting your search criteria in the drop down. Then select the main company
          location and click on the "Associate Main Location"</p>
        <table width="100%" cellpadding="0" cellspacing="0">
          <tr>
            <td align="left" valign="top" width="80%">
              <asp:TextBox ID="parent_search_text" runat="server" Width="100%"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              <asp:ImageButton ID="parent_search" ImageUrl="~/images/search_button.jpg" runat="server" />
            </td>
          </tr>
          <tr>
            <td align="left" valign="top" colspan="2">
              <br />
              <br />
              <asp:DropDownList Enabled="false" ID="parent_list" runat="server" Width="100%" AutoPostBack="true">
              </asp:DropDownList>
            </td>
          </tr>
        </table>
      </td>
      <td align="left" valign="top" bgcolor="#F5F5F5" style="border-left: 1px solid #dddddd"
        class="25%">
        <h4>
          MAIN LOCATION DETAILS</h4>
        <br clear="all" />
        <asp:Label ID="parent_company_details" runat="server" Text="" Width="100%"></asp:Label>
        <img src="images/spacer.gif" alt="" width="290" height="1" />
      </td>
    </tr>
    <tr>
      <td align="right" valign="top" colspan="3">
        <asp:ImageButton ID="add_parent" OnClientClick="return confirm('Clicking on this will associate the parent company with the child company.');"
          ImageUrl="~/images/add_parent.jpg" AlternateText="Combine Companies" runat="server"
          Visible="false" />
      </td>
    </tr>
  </table>
</asp:Panel>
<asp:Panel runat="server" ID="company_combine_table" CssClass="edit_panel" BackColor="white"
  Visible="false">
  <asp:Label runat="server" ID="Attention" ForeColor="Red"></asp:Label>
  <table class="body_width" cellspacing="4" cellpadding="5" border="0">
    <tr>
      <td align="left" valign="top" width="25%" bgcolor="#F5F5F5" style="border-right: 1px solid #dddddd">
        <h4>
          Main Company</h4>
        <br />
        <asp:Label ID="company_combine_details" runat="server" Text="Label"></asp:Label>
      </td>
      <td align="left" valign="top" width="50%">
        <h4>
          Search to Combine</h4>
        <p class="nonflyout_info_box">
          The purpose of this tool is to perminently move all of the notes, action items,
          aircraft references, and contacts from one client company record to another and
          perminently remove the company being copies. This tool should be used when the CRM
          has a duplicate company record.<br />
          <br />
          It is critical that the company displayed on the left hand side of the screen as
          "MAIN COMPANY" is the company you desire to keep and the company displayed to the
          right as "SECONDARY COMPANY" is the one you wish to copy and remove.<br />
          <br />
          It is also critical any company phone numbers be moved by hand to the main company
          prior to combining companies.</p>
        <table width="100%" cellpadding="0" cellspacing="0">
          <tr>
            <td align="left" valign="top" width="80%">
              <asp:TextBox ID="search_combine_text" runat="server" Width="100%"></asp:TextBox>
            </td>
            <td align="left" valign="top">
              <asp:ImageButton ID="search_combine" ImageUrl="~/images/search_button.jpg" runat="server" />
            </td>
          </tr>
          <tr>
            <td align="left" valign="top" colspan="2">
              <br />
              <br />
              <asp:DropDownList Enabled="false" ID="combine_company_list" runat="server" Width="100%"
                AutoPostBack="true">
              </asp:DropDownList>
            </td>
          </tr>
        </table>
      </td>
      <td align="left" valign="top" bgcolor="#F5F5F5" style="border-left: 1px solid #dddddd"
        width="25%">
        <h4>
          Secondary Company Details</h4>
        <br clear="all" />
        <asp:Label ID="combining_company_details" runat="server" Text=""></asp:Label>
        <img src="images/spacer.gif" alt="" width="290" height="1" />
      </td>
    </tr>
    <tr>
      <td align="right" valign="top" colspan="3">
        <asp:ImageButton ID="combine_me" OnClientClick="return confirm('Clicking on this will remove the secondary Company and combine it with the main Company.');"
          ImageUrl="~/images/combine_companies.jpg" AlternateText="Combine Companies" runat="server"
          Visible="false" />
      </td>
    </tr>
  </table>
</asp:Panel>
<div class="container">
  <div style="margin-left: 15px; margin-right: 15px;margin-top:15px;">
    <asp:Panel ID="company_edit_table" runat="server" CssClass="valueSpec viewValueExport Simplistic aircraftSpec plain"
      Visible="true"><asp:Label runat="server" ID="companyLabelHeader"><h2 class="mainHeading remove_margin"><strong>Company</strong> Add</h2></asp:Label>
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Company_Edit" 
          DisplayMode="BulletList" EnableClientScript="true" HeaderText="There are problems with the following fields:" />
        <div class="row">
          <div class="sixteen columns">
            <asp:Panel ID="subpanel_folder" runat="server" BackColor="White" CssClass="edit_panel"
              Visible="false">
              <h4 align="right">
                Subfolder:</h4>
              <asp:DropDownList ID="add_folder_cbo" runat="server" CssClass="float_right" Style="margin-top: 5px;
                margin-left: 4px;" Visible="false">
              </asp:DropDownList>
              <br clear="all" />
              <br clear="all" />
            </asp:Panel>
          </div>
        </div>
        <div class="row">
          <div class="sixteen columns">
            <asp:Panel ID="company_edit" runat="server" CssClass="Box">
              <table cellpadding="0" cellspacing="0" width="100%" class="formatTable blue">
                <tr class="noBorder">
                  <td colspan="2">
                    <asp:Label ID="edit_tag" runat="server" align="right"><div 
                      class="subHeader">Company Information</div><br /></asp:Label>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" width="25%">
                    Company Name:
                  </td>
                  <td align="left" class="style2" valign="top" width="75%">
                    <asp:TextBox ID="comp_name" runat="server" Height="20px" MaxLength="50" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="comp_name"
                      Display="None" ErrorMessage="Company Name is Required" Font-Bold="True" Text=""
                      ValidationGroup="Company_Edit"></asp:RequiredFieldValidator>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Company Alt Name:
                  </td>
                  <td align="left" class="style4" valign="top">
                    <asp:TextBox ID="comp_alt_name" runat="server" Height="21px" MaxLength="40" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Address:
                  </td>
                  <td align="left" class="style2" valign="top">
                    <asp:TextBox ID="comp_address" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="comp_address"
                      Display="None" Enabled="false" ErrorMessage="Address is Required" Font-Bold="True"
                      Text="" ValidationGroup="Company_Edit"></asp:RequiredFieldValidator>
                  </td>
                </tr>
                <tr>
                  <td align="left" class="style1" valign="top">
                  </td>
                  <td align="left" class="style3" valign="top">
                    <asp:TextBox ID="comp_address2" runat="server" MaxLength="50" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    City, State, ZIP:
                  </td>
                  <td align="left" class="style4" valign="top">
                    <asp:TextBox ID="comp_city" runat="server" MaxLength="50" Style="margin-bottom: 0px"
                      Width="60%"></asp:TextBox>
                    <asp:TextBox ID="comp_state" runat="server" MaxLength="2" Width="5%"></asp:TextBox>
                    <asp:TextBox ID="comp_zip" runat="server" MaxLength="10" Width="15%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Country:
                  </td>
                  <td align="left" class="style4" valign="top">
                    <asp:TextBox ID="comp_country" runat="server" MaxLength="25" Width="100%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="comp_country"
                      Display="None" Enabled="false" ErrorMessage="Country is Required" Font-Bold="True"
                      Text="" ValidationGroup="Company_Edit"></asp:RequiredFieldValidator>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Email Address:
                  </td>
                  <td align="left" class="style2" valign="top">
                    <asp:TextBox ID="comp_email" runat="server" Height="16px" MaxLength="70" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Web Address:
                  </td>
                  <td align="left" class="style4" valign="top">
                    <asp:TextBox ID="comp_web" runat="server" MaxLength="70" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Agency Type:
                  </td>
                  <td align="left" colspan="3" valign="top">
                    <asp:RadioButtonList ID="comp_agency_type" runat="server" RepeatDirection="Horizontal">
                      <asp:ListItem ID="civilian" runat="server" Text="Civilian" Value="C" />
                      <asp:ListItem ID="government" runat="server" Text="Government" Value="G" />
                      <asp:ListItem ID="other" runat="server" Text="Other" Value="O" />
                      <asp:ListItem ID="unknown" runat="server" Text="Unknown" Value="U" />
                    </asp:RadioButtonList>
                    <asp:TextBox ID="jetnet_comp_id" runat="server" Style="display: none;"></asp:TextBox>
                    <asp:TextBox ID="main_loc" runat="server" Style="display: none;"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Status:
                  </td>
                  <td align="left" rowspan="3" valign="top">
                    <asp:RadioButtonList ID="company_status" runat="server" RepeatDirection="Horizontal"
                      Visible="true">
                      <asp:ListItem ID="company_active" runat="server" Selected="True" Text="Active" Value="A" />
                      <asp:ListItem ID="company_inactive" runat="server" Text="Inactive" Value="B" />
                    </asp:RadioButtonList>
                  </td>
                </tr>
              </table>
            </asp:Panel>
          </div>
        </div>
        <div class="row">
          <div class="sixteen columns">
            <asp:Panel ID="phone" runat="server" CssClass="Box">
              <table cellpadding="0" cellspacing="0" width="100%" class="formatTable blue">
                <tr class="noBorder">
                  <td colspan="2">
                    <div class="subHeader">
                      Phone Numbers</div>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    Phone Type<br />
                  </td>
                  <td align="left" valign="top">
                    Phone #<br />
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" width="30%">
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="phone1"
                      Display="None" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                      OnServerValidate="TextValidate" Text="" ValidationGroup="Company_Edit">
                    </asp:CustomValidator>
                    <asp:DropDownList ID="type1" runat="server" Width="100%">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="phone1" runat="server" MaxLength="28" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="phone2"
                      Display="None" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                      OnServerValidate="TextValidate" Text="" ValidationGroup="Company_Edit">
                    </asp:CustomValidator>
                    <asp:DropDownList ID="type2" runat="server" Width="100%">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="phone2" runat="server" MaxLength="28" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:CustomValidator ID="CustomValidator3" runat="server" ControlToValidate="phone3"
                      Display="None" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                      OnServerValidate="TextValidate" Text="" ValidationGroup="Company_Edit">
                    </asp:CustomValidator>
                    <asp:DropDownList ID="type3" runat="server" Width="100%">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="phone3" runat="server" MaxLength="28" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:CustomValidator ID="CustomValidator4" runat="server" ControlToValidate="phone4"
                      Display="None" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                      OnServerValidate="TextValidate" Text="" ValidationGroup="Company_Edit">
                    </asp:CustomValidator>
                    <asp:DropDownList ID="type4" runat="server" Width="100%">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="phone4" runat="server" MaxLength="28" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:CustomValidator ID="CustomValidator5" runat="server" ControlToValidate="phone5"
                      Display="None" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                      OnServerValidate="TextValidate" Text="" ValidationGroup="Company_Edit">
                    </asp:CustomValidator>
                    <asp:DropDownList ID="type5" runat="server" Width="100%">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="phone5" runat="server" MaxLength="28" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:CustomValidator ID="CustomValidator6" runat="server" ControlToValidate="phone6"
                      Display="None" ErrorMessage="A Phone Type is Required if a Phone Number is Entered"
                      OnServerValidate="TextValidate" Text="" ValidationGroup="Company_Edit">
                    </asp:CustomValidator>
                    <asp:DropDownList ID="type6" runat="server" Width="100%">
                    </asp:DropDownList>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="phone6" runat="server" MaxLength="28" Width="100%"></asp:TextBox>
                  </td>
                </tr>
              </table>
            </asp:Panel>
          </div>
        </div>
        <br />
        <div class="row">
          <div class="sixteen columns">
            <asp:Panel ID="Panel2" runat="server"   CssClass="Box">
              <table cellpadding="0" cellspacing="0" width="100%" class="formatTable blue">
                <tr class="noBorder">
                  <td colspan="2">
                    <div class="subHeader">
                      Description</div>
                  </td>
                </tr>
                <tr>
                  <td align="left" class="style6" valign="top">
                    <asp:TextBox ID="comp_description" runat="server" Height="100px" MaxLength="21845"
                      TextMode="MultiLine" Width="100%"></asp:TextBox>
                  </td>
                </tr>
              </table>
            </asp:Panel>
          </div>
        </div>
        <br />
        <div class="row">
          <div class="sixteen columns">
            <asp:Panel ID="Panel3" runat="server"  CssClass="Box">
              <table cellpadding="0" cellspacing="0" width="100%" class="formatTable blue">
                <tr class="noBorder">
                  <td colspan="2">
                    <div class="subHeader">
                      Categories</div>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:Label ID="comp_cat1_text" runat="server" MaxLength="100" Text="Category 1"></asp:Label>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="comp_cat1" runat="server" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:Label ID="comp_cat2_text" runat="server" MaxLength="100" Text="Category 2"></asp:Label>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="comp_cat2" runat="server" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:Label ID="comp_cat3_text" runat="server" MaxLength="100" Text="Category 3"></asp:Label>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="comp_cat3" runat="server" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:Label ID="comp_cat4_text" runat="server" MaxLength="100" Text="Category 4"></asp:Label>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="comp_cat4" runat="server" Width="100%"></asp:TextBox>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:Label ID="comp_cat5_text" runat="server" MaxLength="100" Text="Category 5"></asp:Label>
                  </td>
                  <td align="left" valign="top">
                    <asp:TextBox ID="comp_cat5" runat="server" Width="100%"></asp:TextBox>
                  </td>
                </tr>
              </table>
            </asp:Panel>
          </div>
        </div><br />
        <div class="row">
          <div class="sixteen columns">
            <asp:Panel ID="buttons" runat="server">
              <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Right">
                <asp:Label ID="update_text" runat="server" Font-Italic="True"></asp:Label>
              </asp:Panel>
              <table cellpadding="4" cellspacing="0" width="100%">
                <tr>
                  <td align="left" valign="top">
                  
                    <asp:Label ID="mobile_close" runat="server">
                               <a href="javascript: self.close ()" class="button float_left">Close</a>
                    </asp:Label>
                    <asp:Button runat="server" Text="Remove" ID="deleteFunction" CssClass="float_left"
                      OnClientClick="return confirm('Are you sure you would like to remove this Company?');"
                      Visible="true" />
                    <asp:Button runat="server" CssClass="float_right" CausesValidation="true" Text="Save"
                      ValidationGroup="Company_Edit" ID="updateFunction" />
                  </td>
                </tr>
              </table>
            </asp:Panel>
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
      </p>
    </asp:Panel>
  </div>
</div>
<asp:Panel ID="synch" runat="server" BackColor="White" CssClass="edit_panel" Visible="false">
  <h4 align="right">
    Client Company Synchronization</h4>
  <p align="left" class="nonflyout_info_box">
    This facilility is used to automatically copy data from a JETNET aircraft record
    to a corresponding Client Company record.
    <br />
    <br />
    Please note that if you choose to synchronize an area with the corresponding Jetnet
    Company, the client side information will be removed.</p>
  <asp:Label ID="synch_note" runat="server" Text="" ForeColor="Red" Font-Bold="true"></asp:Label>
  <table width="100%" cellpadding="0" cellspacing="0">
    <tr>
      <td align="left" valign="top">
        &nbsp;<b><u>Company Areas to Synchronize:</u></b>
        <div style="padding-left: 10px; padding-top: 10px;">
          <asp:CheckBoxList ID="synch_list" runat="server" AutoPostBack="true" CellPadding="3">
            <asp:ListItem>General/Location/Status</asp:ListItem>
            <asp:ListItem>Phone Numbers</asp:ListItem>
            <asp:ListItem>Contacts</asp:ListItem>
          </asp:CheckBoxList>
        </div>
      </td>
      <td align="right" valign="top">
        <asp:ImageButton ID="synchronize_button" Visible="false" runat="server" ImageUrl="~/images/begin_synch.jpg" />
      </td>
    </tr>
  </table>
</asp:Panel>
