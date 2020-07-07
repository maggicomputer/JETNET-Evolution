<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="myCRM.aspx.vb" Inherits="crmWebClient.myCRM" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>My Manager</title>
  <meta http-equiv="Content-type" content="text/html;charset=UTF-8" />
  <!--Created Stylesheet-->
  <link href="/EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />
  <link href="common/MyCRM_style.css" rel="stylesheet" type="text/css" />
  <!--Created Stylesheet-->
  <link href="/EvoStyles/stylesheets/additional_styles.css" rel="stylesheet" type="text/css" />
  <!-- Header Alternate Styles-->
  <link href="/EvoStyles/stylesheets/header_styles.css" rel="stylesheet" type="text/css" />
  <!--Grid/Layout Styles-->
  <link href="/EvoStyles/stylesheets/layout/base_html_elements.css" rel="stylesheet"
    type="text/css" />
  <link href="/EvoStyles/stylesheets/layout/skeleton_grid.css" rel="stylesheet" type="text/css" />
  <asp:Literal ID="background_image_style" runat="server">
     <style type="text/css">
       body
       {
         background-image: url('/images/background/1.jpg' );
       }
     </style>
    
  </asp:Literal>
</head>
<body>
  <form id="form1" runat="server">
  <div class="FixedHeaderBar" runat="server" id="fixedBar">
  </div>
  <div class="container">
    <cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true"
      EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <div class="sixteen columns headerHeight">
      <div class="one-third column">
        <img src="images/JETNET_MarketplaceMan.png" alt="" class="evolution_logo" />
      </div>
      <div class="eight columns padding_top">
        <asp:Label ID="PageText" runat="server" CssClass="logo_text_title padding_table display_none"></asp:Label>
      </div>
    </div>
    <div class="headerHeightPadding">
    </div>
    <div class="sixteen columns white_background_color content_border">
      <div id="container" width="670px" runat="server" class="tabs_container">
        <br />
        <asp:Label ID="main_attention" runat="server" class="attention"></asp:Label>
        <cc1:TabContainer runat="server" Width="670px" ID="tab_container_ID" EnableViewState="true"
          CssClass="dark-theme">
          <cc1:TabPanel ID="my_account" runat="server" HeaderText="Account">
            <ContentTemplate>
              <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                  <td align="left" valign="top">
                    <table width="100%" cellspacing="0" cellpadding="0">
                      <tr>
                        <td align="left" valign="top" width="190">
                          <h1>
                            Summary of Account Info</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <asp:Table ID="actinfo_subscription_information_table" runat="server" Width="100%"
                            CellPadding="3">
                            <asp:TableRow>
                              <asp:TableCell ID="actinfo_subscription_information_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server" CssClass="subheading" ColumnSpan="5">SUBSCRIPTION INFORMATION
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell RowSpan="4" HorizontalAlign="Left" VerticalAlign="top" Width="60"><img src="images/person.jpg" alt="Information" /></asp:TableCell>
                              <asp:TableCell ID="actinfo_contact_name_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server">
                                <asp:Label runat="server" ID="actinfo_contact_name"></asp:Label>&nbsp;
                              </asp:TableCell>
                              <asp:TableCell ID="actinfo_client_name_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server">
                                <asp:Label runat="server" ID="actinfo_client_name"></asp:Label>&nbsp;
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell ID="subscription_username_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server">
                                <asp:Label runat="server" ID="subscription_username">User Name: <em>Amanda</em></asp:Label></asp:TableCell>
                              <asp:TableCell ID="subscription_email_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server" ColumnSpan="3">
                                <asp:Label runat="server" ID="subscription_email">Email: <em>demo@jetnet.com</em></asp:Label></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell ID="subscription_admin_account_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server">
                                <asp:Label runat="server" ID="subscription_admin_account">Administrator Account: <em>False</em></asp:Label></asp:TableCell>
                              <asp:TableCell ID="subscription_demo_account_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server" ColumnSpan="3">
                                <asp:Label runat="server" ID="subscription_demo_account">Demo Account: <em>False</em></asp:Label></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell ID="act_timezoneinfo" HorizontalAlign="Left" VerticalAlign="Top" runat="server"
                                ColumnSpan="4">
                                Timezone:
                                <asp:DropDownList ID="actinfo_timezone" runat="server">
                                </asp:DropDownList>
                              </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell ID="tier_space_td" HorizontalAlign="Left" VerticalAlign="Top" runat="server"
                                ColumnSpan="4"><hr /></asp:TableCell></asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell RowSpan="5" HorizontalAlign="Left" VerticalAlign="top" Width="60"><img src="images/tiers.jpg" alt="Tiers" /></asp:TableCell>
                              <asp:TableCell ID="subscription_tier_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server">
                                <asp:Label runat="server" ID="subscription_tier">Tier Level: <em>All</em></asp:Label></asp:TableCell>
                              <asp:TableCell ID="subscription_business_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server" ColumnSpan="3">
                                <asp:Label runat="server" ID="subscription_business">Business: <em>True</em></asp:Label></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell ID="subscription_commercial_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server">
                                <asp:Label runat="server" ID="subscription_commercial">Commercial: <em>True</em></asp:Label></asp:TableCell>
                              <asp:TableCell ID="subscription_helicopter_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server" ColumnSpan="3">
                                <asp:Label runat="server" ID="subscription_helicopter">Helicopter: <em>True</em></asp:Label></asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell ID="subscription_aerodex_row" HorizontalAlign="Left" VerticalAlign="Top"
                                runat="server" ColumnSpan="4">
                                <asp:Label runat="server" ID="subscription_aerodex">Aerodex: <em>False</em></asp:Label></asp:TableCell>
                            </asp:TableRow>
                          </asp:Table>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <table width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                              <td align="left" valign="top" width="145">
                                <h2>
                                  Change Password</h2>
                              </td>
                              <td align="left" valign="top">
                                <div class="seperator_line">
                                  &nbsp;</div>
                              </td>
                            </tr>
                          </table>
                          <p class="nonflyout_info_box">
                            Password must be 8-15 and must contain at least one number and one character.</p>
                          <asp:Label ID="password_attention" runat="server" class="attention"></asp:Label>
                          <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="password_confirm_txt"
                            ControlToCompare="password_txt" ErrorMessage="Passwords do not match." />
                          <asp:Table ID="password_table" runat="server" Width="100%" CellPadding="3">
                            <asp:TableRow>
                              <asp:TableCell RowSpan="3" HorizontalAlign="Left" VerticalAlign="top" Width="60"><img src="images/tools.jpg" alt="Tools" /></asp:TableCell>
                              <asp:TableCell Width="120">Old Password:   </asp:TableCell>
                              <asp:TableCell>
                                <asp:TextBox ID="old_password_txt" runat="server" TextMode="Password" Text="test"></asp:TextBox>
                              </asp:TableCell>
                              <asp:TableCell>
                                                            
                              </asp:TableCell>
                              </asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell Width="120">New Password:   </asp:TableCell>
                              <asp:TableCell>
                                <asp:TextBox ID="password_txt" runat="server" TextMode="Password" Text="test"></asp:TextBox>&nbsp;&nbsp;<asp:Image ID="actinfo_password_mouseover_img" Height="15px" runat="server"
                        ImageUrl="/images/info.png" />
                              </asp:TableCell>
                              <asp:TableCell>
                                                            
                              </asp:TableCell></asp:TableRow>
                            <asp:TableRow>
                              <asp:TableCell>Confirm Password:    </asp:TableCell>
                              <asp:TableCell ColumnSpan="2">
                                <asp:TextBox ID="password_confirm_txt" runat="server" TextMode="Password" Text="test"></asp:TextBox>
                              </asp:TableCell></asp:TableRow>
                          </asp:Table>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <table width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                              <td align="left" valign="top" width="104">
                                <h2>
                                  Note Settings</h2>
                              </td>
                              <td align="left" valign="top">
                                <div class="seperator_line">
                                  &nbsp;</div>
                              </td>
                            </tr>
                          </table>
                          <asp:CheckBox runat="server" ID="automaticNoteLog" Text="Turn on automatic logging of notes for changes made to Aircraft." />
                          <p class="attention nonflyout_info_box">
                            *Note that when this feature is set to on, note records will automatically be created
                            and stored for changes made to via the primary aircraft form.</p>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </ContentTemplate>
          </cc1:TabPanel>
          <cc1:TabPanel ID="my_display" runat="server" HeaderText="Display">
            <ContentTemplate>
              <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <table width="100%" cellspacing="0" cellpadding="0">
                      <tr>
                        <td align="left" valign="top" width="140">
                          <h1>
                            Records Per Page</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="60">
                                <img src="images/number.jpg" alt="Number of Records" />
                              </td>
                              <td align="left" valign="top">
                                <p>
                                  To provide you with the maximum search and display speed over the web, search results
                                  will return sets of information to you based on your needs. The number of records
                                  per page identifies the number of records that will be returned in each data set
                                  without requiring you to click "Next Page".
                                </p>
                                <p class="nonflyout_info_box">
                                  Note that increasing the Number of Records per Page may slow the display of information.</p>
                                Number of Records Per Page:&nbsp;
                                <asp:TextBox ID="mydisplay_records_per_page_txt" runat="server" Width="40"></asp:TextBox>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <table width="100%" cellspacing="0" cellpadding="0">
                      <tr>
                        <td align="left" valign="top" width="250">
                          <h1>
                            Display and Edit Aircraft Values</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="60">
                                <img src="images/number.jpg" alt="Relationships" />
                              </td>
                              <td align="left" valign="top">
                                Display and Edit Aircraft Values:&nbsp;
                                <asp:DropDownList ID="mydisplay_value_format" runat="server" AutoPostBack="true"
                                  OnSelectedIndexChanged="run_value_label">
                                  <asp:ListItem Text="Display In Thousads" Value="T">Display In Thousads</asp:ListItem>
                                  <asp:ListItem Text="Display In Millions" Value="M">Display In Millions</asp:ListItem>
                                  <asp:ListItem Text="Full Number Display" Value="F">Full Number Display</asp:ListItem>
                                </asp:DropDownList>
                                &nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="format_label" Text=""></asp:Label>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <table width="100%" cellspacing="0" cellpadding="0">
                      <tr>
                        <td align="left" valign="top" width="180">
                          <h1>
                            Relationship to Aircraft</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="60">
                                <img src="images/relationship.jpg" alt="Relationships" />
                              </td>
                              <td align="left" valign="top">
                                <p>
                                  Please select the check box below if you would like to enable your currently selected
                                  "Relationship to Aircraft" selection to be saved as <b>YOUR default "Relationship
                                    to Aircraft"</b> for aircraft searches.
                                  <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                      <td align="left" valign="top" colspan="2">
                                        <asp:DropDownList ID="types_of_owners" runat="server" Width="160">
                                          <asp:ListItem Text="All Companies" Value="All Companies"></asp:ListItem>
                                          <asp:ListItem Text="All Owners" Value="All Owners" Selected="True"></asp:ListItem>
                                          <asp:ListItem Text="Whole Owners" Value="Whole Owners"></asp:ListItem>
                                          <asp:ListItem Text="Operators" Value="Operators"></asp:ListItem>
                                        </asp:DropDownList>
                                      </td>
                                    </tr>
                                  </table>
                                </p>
                                <br />
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top">
                    <asp:Table runat="server" ID="myCookiePreferences">
                      <asp:TableRow>
                        <asp:TableCell ID="display_blank_fields_on_aircraft" HorizontalAlign="left" VerticalAlign="top"
                          runat="server">
                          <table width="100%" cellspacing="0" cellpadding="0">
                            <tr>
                              <td align="left" valign="top" width="230">
                                <h1>
                                  Show Blank Fields for Aircraft</h1>
                              </td>
                              <td align="left" valign="top">
                                <div class="seperator_line">
                                  &nbsp;</div>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top" colspan="2">
                                <table width="100%" cellpadding="0" cellspacing="0">
                                  <tr>
                                    <td align="left" valign="top" width="60">
                                      <img src="images/tools.jpg" alt="Show Blank Fields for Aircraft" width="70" />
                                    </td>
                                    <td align="left" valign="top">
                                      <p>
                                        Evolution allows users to display aircraft details in two different formats<br />
                                        (1) Display Aircraft in Condensed Format showing all critical fields and those with
                                        information filled in<br />
                                        (2) Display Aircraft in Expanded Format showing all fields even if blank providing
                                        more of a template for filling in specifications.
                                      </p>
                                      Format:&nbsp;
                                      <asp:DropDownList ID="display_no_blank_fields_on_aircraft_ddl" runat="server">
                                        <asp:ListItem Value="CF">Display Aircraft in Condensed Format (Do Not Display Blank Fields)</asp:ListItem>
                                        <asp:ListItem Value="EF">Display Aircraft in Expanded Format (Display Blank Fields)</asp:ListItem>
                                      </asp:DropDownList>
                                    </td>
                                  </tr>
                                </table>
                              </td>
                            </tr>
                          </table>
                        </asp:TableCell>
                      </asp:TableRow>
                    </asp:Table>
                  </td>
                </tr>
              </table>
            </ContentTemplate>
          </cc1:TabPanel>
          <cc1:TabPanel ID="my_models" runat="server" HeaderText="Models">
            <ContentTemplate>
              <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <table width="100%" cellspacing="0" cellpadding="0">
                      <tr>
                        <td align="left" valign="top" width="150">
                          <h1>
                            Default Model(s)</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <asp:Panel ID="model_toggle" runat="server" Enabled="false" CssClass="unavailable">
                            <asp:Label ID="model_attention" runat="server" class="attention"></asp:Label>
                            <table width="100%" cellpadding="0" cellspacing="0">
                              <tr>
                                <td align="left" valign="top" width="60" rowspan="3">
                                  <img src="images/autologin.jpg" alt="Default Regions" />
                                </td>
                                <td align="left" valign="top" colspan="2">
                                  <p>
                                    Choose default market preferences Models.</p>
                                </td>
                              </tr>
                              <tr>
                                <td align="left" valign="top" colspan="2">
                                  <p align="left" class="nonflyout_info_box">
                                    Use this form to identify the aircraft models that you will use as a default throughout
                                    the system as your primary aircraft market. Note that the Market Time default will
                                    be used for all users of your system and is only editable by Administrators.</p>
                                  <table width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                      <td align="left" valign="top">
                                        Default to display activity for:
                                        <asp:DropDownList ID="market_time" runat="server">
                                          <asp:ListItem Value="7">7 Days</asp:ListItem>
                                          <asp:ListItem Value="31">One Month</asp:ListItem>
                                          <asp:ListItem Value="93">Three Months</asp:ListItem>
                                          <asp:ListItem Value="186">Six Months</asp:ListItem>
                                          <asp:ListItem Value="279">Nine Months</asp:ListItem>
                                          <asp:ListItem Value="365">Twelve Months</asp:ListItem>
                                        </asp:DropDownList>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top" colspan="2">
                                        <table width="100%">
                                          <tr>
                                            <td align="left" valign="top">
                                              Available Models<br />
                                              <asp:ListBox ID="market_pref_models" runat="server" Rows="10" Width="250px" SelectionMode="multiple">
                                              </asp:ListBox>
                                            </td>
                                            <td align="left" valign="top">
                                              &nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td align="left" valign="top">
                                              Selected Model Preferences<br />
                                              <asp:ListBox ID="selected_models" runat="server" Rows="10" Width="250px" SelectionMode="multiple">
                                              </asp:ListBox>
                                            </td>
                                          </tr>
                                        </table>
                                        <asp:Button ID="removeall" Text="<<" OnClick="RemoveAllBtn_Click" runat="server"
                                          Width="30px" CommandArgument="market" />
                                        <asp:Button ID="removeone" Text="<" OnClick="RemoveBtn_Click" runat="server" Width="26px"
                                          CommandArgument="market" />
                                        <asp:Button ID="addone" Text=">" OnClick="AddBtn_Click" runat="server" Width="29px"
                                          CommandArgument="market" />
                                        <asp:Button ID="addall" Text=">>" OnClick="AddAllBtn_Click" runat="server" Width="33px"
                                          CommandArgument="market" />
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
                  </td>
                </tr>
              </table>
            </ContentTemplate>
          </cc1:TabPanel>
          <cc1:TabPanel ID="my_feature_codes" runat="server" HeaderText="Features">
            <ContentTemplate>
              <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <table width="100%" cellspacing="0" cellpadding="0">
                      <tr>
                        <td align="left" valign="top" width="220">
                          <h1>
                            FEATURE CODE MAINTENANCE</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="60" rowspan="3">
                                <img src="images/autologin.jpg" alt="Default Regions" />
                              </td>
                              <td align="left" valign="top" colspan="2">
                                Add Feature Codes for Use within your Program.
                                <table width="100%" cellpadding="0" cellspacing="0">
                                  <tr>
                                    <td colspan="4" align="left">
                                      <p align="right">
                                        <asp:LinkButton ID="add_new" CommandName="Add" Text="Add New Row" runat="server" /></p>
                                      <asp:Panel runat="server" CssClass="gray" Visible="false" ID="new_row">
                                        <table width="100%" cellpadding="3" cellspacing="0">
                                          <tr>
                                            <td align="left" valign="top" width="420">
                                              <b><u>Name</u></b>
                                            </td>
                                            <td align="left" valign="top" width="90">
                                              <b><u>Type</u></b>
                                            </td>
                                            <td align="left" valign="top">
                                            </td>
                                          </tr>
                                          <tr>
                                            <td align="left" valign="top">
                                              <asp:TextBox ID="clickfeat_name" Width="420px" runat="server" MaxLength="60" />
                                            </td>
                                            <td align="left" valign="top" width="90">
                                              <asp:TextBox ID="clikfeat_type" Width="90px" runat="server" MaxLength="3" />
                                            </td>
                                            <td align="left" valign="top">
                                              <asp:LinkButton ID="insert" CommandName="insert" Text="Insert" runat="server" />
                                            </td>
                                          </tr>
                                        </table>
                                      </asp:Panel>
                                    </td>
                                  </tr>
                                  <tr>
                                    <td align="left" valign="top">
                                      <asp:DataGrid runat="server" ID="datagrid_feature_code" CellPadding="3" horizontal-align="left"
                                        EnableViewState="true" ShowFooter="false" BackColor="White" Font-Size="8pt" Width="100%"
                                        OnCancelCommand="MyDataGrid_Cancel" OnEditCommand="MyDataGrid_Edit" OnDeleteCommand="MyDataGrid_Delete"
                                        AllowPaging="false" PageSize="25" Visible="true" BorderStyle="None" AllowSorting="True"
                                        AutoGenerateColumns="false" BorderColor="Gray">
                                        <PagerStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="#204763" Font-Bold="True"
                                          Font-Underline="True" ForeColor="White" Mode="NumericPages" NextPageText="Next"
                                          PrevPageText="Previous" />
                                        <AlternatingItemStyle BackColor="#eeeeee" />
                                        <ItemStyle BorderStyle="None" VerticalAlign="Top" BorderColor="Gray" Font-Size="8pt" />
                                        <HeaderStyle BackColor="#A8C1DD" Font-Bold="True" Font-Size="8pt" Font-Underline="True"
                                          ForeColor="Black" Wrap="False" HorizontalAlign="Left" VerticalAlign="Middle"></HeaderStyle>
                                        <Columns>
                                          <asp:TemplateColumn HeaderText="Name" ItemStyle-HorizontalAlign="left">
                                            <ItemTemplate>
                                              <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "clikfeat_name")), (DataBinder.Eval(Container.DataItem, "clikfeat_name")), "")%>
                                              <asp:TextBox runat="server" ID="id_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_name") %>'
                                                Visible="true" Width="430px" MaxLength="60" Style="display: none;" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                              <asp:TextBox runat="server" ID="id" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_name") %>'
                                                Visible="true" Width="430px" MaxLength="60" />
                                              <asp:TextBox runat="server" ID="id_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_name") %>'
                                                Visible="true" Width="430px" MaxLength="60" Style="display: none;" />
                                            </EditItemTemplate>
                                          </asp:TemplateColumn>
                                          <asp:TemplateColumn HeaderText="Code" ItemStyle-HorizontalAlign="left">
                                            <ItemTemplate>
                                              <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "clikfeat_type")), (DataBinder.Eval(Container.DataItem, "clikfeat_type")), "")%>
                                              <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_type") %>'
                                                Visible="true" Width="40px" MaxLength="3" Style="display: none;" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                              <asp:TextBox runat="server" ID="type" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_type") %>'
                                                Visible="true" Width="40px" MaxLength="3" />
                                              <asp:TextBox runat="server" ID="type_hidden" Text='<%# DataBinder.Eval(Container.DataItem, "clikfeat_type") %>'
                                                Visible="true" Width="40px" MaxLength="3" Style="display: none;" />
                                            </EditItemTemplate>
                                          </asp:TemplateColumn>
                                          <asp:TemplateColumn>
                                            <ItemTemplate>
                                              <asp:LinkButton ID="feature_code_delete" CommandName="Delete" Text="Delete" runat="server" /></ItemTemplate>
                                          </asp:TemplateColumn>
                                        </Columns>
                                      </asp:DataGrid>
                                    </td>
                                  </tr>
                                </table>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </ContentTemplate>
          </cc1:TabPanel>
          <cc1:TabPanel ID="my_company" runat="server" HeaderText="Company Preferences">
            <ContentTemplate>
              <table width="100%" cellpadding="3" cellspacing="0">
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <p class="nonflyout_info_box attention">
                      The following preferences will be applied to all users of this system and should
                      only be modified by a system administrator.</p>
                    <table width="100%" cellspacing="0" cellpadding="0">
                      <tr>
                        <td align="left" valign="top" width="165">
                          <h1>
                            Company Categories</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="60">
                                <img src="images/category_watermark.jpg" alt="Categories" />
                              </td>
                              <td align="left" valign="top">
                                <asp:Label ID="preference_attention" runat="server" class="attention"></asp:Label>
                                <asp:Panel ID="preference_toggle" runat="server">
                                  <table width="450" cellpadding="4" cellspacing="0">
                                    <tr>
                                      <td align="left" valign="top">
                                        &nbsp;
                                      </td>
                                      <td align="left" valign="top">
                                        <strong>Category Name</strong>
                                      </td>
                                      <td align="left" valign="top">
                                        <strong>Use?</strong>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Category #1:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="pref_id" runat="server" Width="310" Style="display: none;" />
                                        <asp:TextBox ID="pref_1" runat="server" Width="310" MaxLength="60" Enabled="false"   />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="pref_1_use" runat="server" />
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Category #2:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="pref_2" runat="server" Width="310" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="pref_2_use" runat="server" />
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Category #3:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="pref_3" runat="server" Width="310" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="pref_3_use" runat="server" />
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Category #4:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="pref_4" runat="server" Width="310" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="pref_4_use" runat="server" />
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Category #5:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="pref_5" runat="server" Width="310" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="pref_5_use" runat="server" />
                                      </td>
                                    </tr>
                                  </table>
                                </asp:Panel>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" width="165">
                          <h1>
                            Aircraft Custom Fields</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="60">
                                <img src="images/category_watermark.jpg" alt="Custom Fields" />
                              </td>
                              <td align="left" valign="top">
                                <asp:Label ID="Label1" runat="server" class="attention"></asp:Label>
                                <asp:Panel ID="aircraft_preference_toggle" runat="server">
                                  <p>
                                    Enter a Name/Label for each custom aircraft data field that you desire and check
                                    the box to the right of the name if you wish to have it applied in the system. At
                                    any point where a given field is no longer used simply uncheck the box to the right
                                    of the name. <span class="red">Note: Do not reuse fields for a different purpose in
                                      the future since data stored in each given field would still have previous values.</span></p>
                                  <table width="650" cellpadding="4" cellspacing="0">
                                    <tr>
                                      <td align="left" valign="top">
                                        &nbsp;
                                      </td>
                                      <td align="left" valign="top">
                                        <strong>Custom Field Name</strong>
                                      </td>
                                      <td align="left" valign="top">
                                        <strong>Use?</strong>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top" width="150">
                                        Custom Field #1:
                                      </td>
                                      <td align="left" valign="top" width="160">
                                        <asp:TextBox ID="ac_category_1" runat="server" Width="250" MaxLength="60" Enabled="false"  />  
                                      </td>
                                      <td align="left" valign="top" nowrap="nowrap" width="320">
                                        <asp:CheckBox ID="ac_category_1_use" runat="server" />
                                        &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_1"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_1"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_1"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_1"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label1" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete1" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete1" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Custom Field #2:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="ac_category_2" runat="server" Width="250" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="ac_category_2_use" runat="server" />
                                        &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_2"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_2"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_2"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_2"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label2" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete2" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete2" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Custom Field #3:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="ac_category_3" runat="server" Width="250" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="ac_category_3_use" runat="server" />
                                        &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_3"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_3"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_3"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_3"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label3" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete3" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete3" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Custom Field #4:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="ac_category_4" runat="server" Width="250" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="ac_category_4_use" runat="server" />
                                        &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_4"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_4"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_4"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_4"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label4" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete4" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete4" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Custom Field #5:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="ac_category_5" runat="server" Width="250" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="ac_category_5_use" runat="server" />
                                        &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_5"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_5"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_5"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_5"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label5" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete5" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete5" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Custom Field #6:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="ac_category_6" runat="server" Width="250" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="ac_category_6_use" runat="server" />
                                        &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_6"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_6"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_6"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_6"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label6" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete6" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete6" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Custom Field #7:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="ac_category_7" runat="server" Width="250" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="ac_category_7_use" runat="server" />
                                         &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_7"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_7"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_7"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_7"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label7" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete7" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete7" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Custom Field #8:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="ac_category_8" runat="server" Width="250" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="ac_category_8_use" runat="server" />
                                        &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_8"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_8"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_8"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_8"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label8" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete8" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete8" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Custom Field #9:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="ac_category_9" runat="server" Width="250" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="ac_category_9_use" runat="server" />
                                        &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_9"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_9"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_9"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_9"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label9" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete9" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete9" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                    <tr>
                                      <td align="left" valign="top">
                                        Custom Field #10:
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:TextBox ID="ac_category_10" runat="server" Width="250" MaxLength="60" Enabled="false"  />
                                      </td>
                                      <td align="left" valign="top">
                                        <asp:CheckBox ID="ac_category_10_use" runat="server" />
                                         &nbsp;&nbsp;  
                                        <asp:ImageButton id="edit_ac_10"  AlternateText="Edit" ImageUrl="~/images/edit_icon.png" runat="server" />
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="deleteq_ac_10"  AlternateText="Delete" ImageUrl="~/images/red_x.gif" runat="server" />  
                                        &nbsp;&nbsp;
                                        <asp:ImageButton id="updateq_ac_10"  AlternateText="Update" ImageUrl="~/images/update.gif" runat="server" Visible="false"  />
                                        &nbsp;&nbsp;       
                                        <asp:ImageButton id="cancel_ac_10"  AlternateText="Cancel" ImageUrl="~/images/cancel.gif" runat="server" Visible="false" />
                                        &nbsp;&nbsp; 
                                        <asp:Label runat="server" ID="deleteq_label10" Text="Delete?" Visible="false"></asp:Label>
                                        &nbsp;&nbsp; 
                                        <asp:LinkButton ID="yes_delete10" runat="server" Text="Yes" Visible="false" ></asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="no_delete10" runat="server" Text="No" Visible="false" ></asp:LinkButton>
                                      </td>
                                    </tr>
                                  </table>
                                </asp:Panel>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" width="165">
                          <h1>
                            Company Settings</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                      <tr>
                        <td align="left" valign="top" colspan="2">
                          <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                              <td align="left" valign="top" width="60">
                                <img src="images/number.jpg" alt="Categories" />
                              </td>
                              <td align="left" valign="top">
                                <asp:Panel ID="maximum_export" runat="server">
                                  <asp:CompareValidator ID="maximum_compare" runat="server" ControlToValidate="maximum_records_export"
                                    Operator="DataTypeCheck" Type="Double" ErrorMessage="* Maximum Records Must be Numeric"></asp:CompareValidator>
                                  <table width="450" cellpadding="4" cellspacing="0">
                                    <tr>
                                      <td align="left" valign="top" colspan="2">
                                        Maximum # of Client Records in Single Export:&nbsp;
                                        <asp:TextBox ID="maximum_records_export" runat="server" Width="50" MaxLength="10"
                                          Text="0" />
                                        <p class="nonflyout_info_box">
                                          "0" indicates unlimited export of client records</p>
                                      </td>
                                    </tr>
                                  </table>
                                </asp:Panel>
                              </td>
                            </tr>
                          </table>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
              </table>
            </ContentTemplate>
          </cc1:TabPanel>
          <cc1:TabPanel ID="my_support" runat="server" HeaderText="Support">
            <ContentTemplate>
              <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <table width="100%" cellspacing="0" cellpadding="0">
                      <tr>
                        <td align="left" valign="top" width="235">
                          <h1>
                            Additional Support Information</h1>
                        </td>
                        <td align="left" valign="top">
                          <div class="seperator_line">
                            &nbsp;</div>
                        </td>
                      </tr>
                    </table>
                  </td>
                </tr>
                <tr>
                  <td align="left" valign="top" colspan="2">
                    <asp:Label ID="mysupport_attention" runat="server" Text="" CssClass="attention"></asp:Label>
                    <asp:Table ID="actinfo_subscriber_information_table" runat="server" Width="100%"
                      CellPadding="3">
                      <asp:TableRow>
                        <asp:TableCell ID="actinfo_subscriber_information_row" HorizontalAlign="Left" VerticalAlign="Top"
                          runat="server" CssClass="subheading" ColumnSpan="4">
                          SUBSCRIBER EMAIL: [<asp:Label runat="server" ID="actinfo_subscriber_information_email">419946</asp:Label>]
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="top" Width="60"><img src="images/support.jpg" alt="Support" /></asp:TableCell>
                        <asp:TableCell ID="actinfo_email_address_row" HorizontalAlign="left" VerticalAlign="Top"
                          runat="server" ColumnSpan="3">
                          If you have a specific question or issue to report to JETNET please enter a description
                          of your issue in the box below and click on the 'Submit Customer Support Issue'
                          button. Staff from our customer support center will then research your issue and
                          respond as quickly as possible.<br />
                          <br />
                          <asp:TextBox ID="actinfo_phone_textbox" Width="570" runat="server" value="" CssClass="unwatermarked" /><br />
                          <asp:TextBox ID="actinfo_email_textbox" runat="server" TextMode="MultiLine" Width="570"
                            Rows="10" CssClass="unwatermarked"></asp:TextBox>
                          <cc1:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="actinfo_phone_textbox"
                            WatermarkText="Type Phone # Here" WatermarkCssClass="watermarked" />
                          <cc1:TextBoxWatermarkExtender ID="TBWE1" runat="server" TargetControlID="actinfo_email_textbox"
                            WatermarkText="Type Comments Here" WatermarkCssClass="watermarked" />
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="top" ColumnSpan="4">
                          <p align="right">
                            <asp:Button ID="myact_email_button" runat="server" Text="Submit Customer Support Issue" /></p>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell ID="TableCell1" HorizontalAlign="Left" VerticalAlign="Top" runat="server"
                          CssClass="subheading" ColumnSpan="4">
                                                CONTACT JETNET
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="top" Width="60"><img src="images/support.jpg" alt="Support" /></asp:TableCell>
                        <asp:TableCell ID="TableCell2" HorizontalAlign="left" VerticalAlign="Top" runat="server"
                          ColumnSpan="3">
                                                <table cellpadding="3" cellspacing="0">
                                                    <tr>
							                            <td align="left" valign="top" width="300">101 First Street, 2<sup>nd</sup> Floor</td>
							                            <td align="left" valign="top">Phone: <a href="#">(315)-797-4420</a></td>
						                            </tr>
						                            <tr>
							                            <td align="left" valign="top">Utica,&nbsp;NY&nbsp;13501-1222</td>
							                            <td align="left" valign="top">Toll Free: <a href="#">(800)-553-8638</a></td>
						                            </tr>
						                            <tr>
							                            <td  align="left" valign="top">United States</td>
							                            <td align="left" valign="top">Fax: <a href="#">(315)-797-4798</a></td>
						                            </tr>
						                            <tr>
							                            <td colspan="2" align="left" valign="top"><a href="#">customerservice@jetnet.com</a></td>
						                            </tr>
						                            <tr>
							                            <td colspan="2" align="left" valign="top"><a href="#">www.jetnet.com</a></td>
						                            </tr>
                                                </table>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell ID="TableCell3" HorizontalAlign="Left" VerticalAlign="Top" runat="server"
                          CssClass="subheading" ColumnSpan="4">
                                                OPERATIONS SUPPORT
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="top" Width="60"><img src="images/support.jpg" alt="Support" /></asp:TableCell>
                        <asp:TableCell ID="TableCell4" HorizontalAlign="left" VerticalAlign="Top" runat="server"
                          ColumnSpan="3">
                          <table width="100%" cellpadding="3" cellspacing="0">
                            <tr>
                              <td align="left" valign="top">
                                AeroWebTech Support Team
                              </td>
                              <td align="right" valign="top">
                                <asp:Label runat="server" ID="adminLink" Visible="false"><a href="crmAdministration.aspx" target="_blank">Administration Tools</a></asp:Label>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top" colspan="2">
                                Email: <a href="support@aerowebtech.com">support@aerowebtech.com</a>
                              </td>
                            </tr>
                            <tr>
                              <td align="left" valign="top" colspan="2">
                                Phone: 315-542-6132<br />
                              </td>
                            </tr>
                          </table>
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell ID="TableCell5" HorizontalAlign="Left" VerticalAlign="Top" runat="server"
                          CssClass="subheading" ColumnSpan="4">
                                                CONTACT YOUR JETNET REPRESENTATIVE
                        </asp:TableCell>
                      </asp:TableRow>
                      <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="top" Width="60"><img src="images/support.jpg" alt="Support" /></asp:TableCell>
                        <asp:TableCell ID="TableCell6" HorizontalAlign="left" VerticalAlign="Top" runat="server"
                          ColumnSpan="3">
                          <table width="100%" cellpadding="3" cellspacing="0">
                            <tr>
                              <td align="left" valign="top">
                                <asp:Label ID="jetnet_rep_name" runat="server"></asp:Label>
                                <br />
                                <br />
                                <asp:Label ID="jetnet_rep_phone" runat="server"></asp:Label><br />
                                <asp:Label ID="jetnet_rep_email" runat="server"></asp:Label>
                              </td>
                              <td align="right" valign="top">
                                <asp:Image ID="jetnet_rep" runat="server" Width="150" CssClass="border" />
                              </td>
                            </tr>
                          </table>
                        </asp:TableCell>
                      </asp:TableRow>
                    </asp:Table>
                  </td>
                </tr>
              </table>
            </ContentTemplate>
          </cc1:TabPanel>
        </cc1:TabContainer>
        <asp:Panel runat="server" ID="buttons" CssClass="button_bottom">
          <table width="100%" cellpadding="3" cellspacing="1">
            <tr>
              <td align="left" valign="top">
                <asp:Button ID="cancel_button" runat="server" Text="Cancel" OnClientClick="javascript:window.close();" />
              </td>
              <td align="right" valign="top">
                <asp:Button ID="save_button" runat="server" Text="Save" />
              </td>
            </tr>
          </table>
        </asp:Panel>
      </div>
    </div>
  </div>
  </form>
</body>
</html>
