<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Company_Listing.aspx.vb"
    Inherits="crmWebClient.Company_Listing" MasterPageFile="~/EvoStyles/EvoTheme.Master"
    StylesheetTheme="Evo" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/EvoStyles/EvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



    <script language="javascript" type="text/javascript">
        var bIsBaseView = false;
        var bIsViewView = false;
        var bShowInactiveCountriesView = false;

        var bIsBaseBase = false;
        var bIsViewBase = false;
        var bShowInactiveCountriesBase = false;

        var bIsBaseCompany = false;
        var bIsViewCompany = false;
        var bShowInactiveCountriesCompany = false;
    </script>

    <style>
        .aircraftListing.valueSpec.Simplistic .formatTable .CLIENTCRMRow td, li.folderClientRow {
            background-color: #ffece7 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" class="loadingScreenBox">
        <ProgressTemplate>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:Panel runat="server" Visible="true" ID="Company_Criteria" CssClass="PerformanceListingTable">
        <cc1:CollapsiblePanelExtender ID="CompanyPanelEx" runat="server" TargetControlID="Company_Collapse_Panel"
            Collapsed="true" ExpandControlID="Company_Control_Panel" ImageControlID="Company_Image"
            ExpandedImage="../Images/spacer.gif" CollapsedImage="../Images/search_expand.jpg"
            CollapseControlID="Company_Control_Panel" Enabled="True" CollapsedText="New Search"
            ExpandedText="Hide Search">
        </cc1:CollapsiblePanelExtender>
        <div class="fixPosition">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left" valign="top" class="dark_header" width="100%">
                        <asp:Table ID="Table3" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
                            CssClass="padding_table">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="company_help_text"
                                    CssClass="evoHelp displayNoneMobile">
                        <a href="#" class="display_none">Help</a>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="90" ID="company_search_expand_text">
                                    <asp:Panel ID="Company_Control_Panel" runat="server" Width="100%">
                                        <asp:Image ID="Company_Image" runat="server" ImageUrl="../Images/search_expand.jpg"
                                            CssClass="mobile_display_off_cell" />
                                        <a href="javascript:void(0);" id="controlLink" runat="server" class="display_none">
                                            <i class="fa fa-chevron-left" aria-hidden="true"></i></a>
                                    </asp:Panel>
                                    <asp:Label runat="server" ID="StaticFolderNewSearchLabel"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="company_results_text"
                                    CssClass="mobile_padding">
                                    <asp:Label ID="company_criteria_results" runat="server" Text=""></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="company_sort_by_text"
                                    CssClass="mobile_display_off_cell">
                        Sort By: 
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="company_sort_by_dropdown"
                                    CssClass="mobile_top_padding">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="company_sort_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                            <asp:ListItem>Company</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="company_sort_submenu_dropdown" runat="server" CssClass="ul_bottom sort_dropdown"
                                            OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                            <asp:ListItem>Name</asp:ListItem>
                                            <asp:ListItem>Address</asp:ListItem>
                                            <asp:ListItem>City</asp:ListItem>
                                            <asp:ListItem>State</asp:ListItem>
                                            <asp:ListItem>Country</asp:ListItem>
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="company_per_page_text"
                                    CssClass="mobile_display_off_cell">
                        Per Page:&nbsp;
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="company_per_page_dropdown_"
                                    CssClass="mobile_display_off_cell">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="company_per_page_dropdown" runat="server" CssClass="ul_top per_page_width">
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="company_per_page_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown"
                                            OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                            <asp:ListItem Value="20">20</asp:ListItem>
                                            <asp:ListItem Value="30">30</asp:ListItem>
                                            <asp:ListItem Value="40">40</asp:ListItem>
                                            <asp:ListItem Value="50">50</asp:ListItem>
                                            <asp:ListItem Value="60">60</asp:ListItem>
                                            <asp:ListItem Value="70">70</asp:ListItem>
                                            <asp:ListItem Value="80">80</asp:ListItem>
                                            <asp:ListItem Value="90">90</asp:ListItem>
                                            <asp:ListItem Value="100">100</asp:ListItem>
                                            <asp:ListItem Value="200">200</asp:ListItem>
                                            <asp:ListItem Value="300">300</asp:ListItem>
                                            <asp:ListItem Value="400">400</asp:ListItem>
                                            <asp:ListItem Value="500">500</asp:ListItem>
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="45" ID="company_view_dropdown_"
                                    CssClass="displayNoneMobile mobile_padding fullWidthMobile">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="company_view_dropdown" runat="server" CssClass="ul_top thumnail_view_bullet">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="company_view_submenu_dropdown" runat="server" CssClass="ul_bottom thumbnail"
                                            OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                            <asp:ListItem>Listing</asp:ListItem>
                                            <asp:ListItem>Gallery</asp:ListItem>
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="75" ID="TableCell11"
                                    CssClass="mobile_display_off_cell">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="company_actions_dropdown" runat="server" CssClass="ul_top">
                                            <asp:ListItem>Actions</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="company_actions_submenu_dropdown" runat="server" CssClass="ul_bottom ac_action_dropdown"
                                            DisplayMode="HyperLink" OnClick="submenu_dropdown_Click">
                                            <asp:ListItem Value="javascript:SubMenuDrop(3,0, 'COMPANY');">Save As - New Folder</asp:ListItem>
                                            <asp:ListItem Value="javascript:SubMenuDrop(1,0,'COMPANY');">Custom Export</asp:ListItem>
                                            <asp:ListItem Value="javascript:SubMenuDrop(5,0,'COMPANY');">JETNET Export/Report</asp:ListItem>
                                            <asp:ListItem Value="javascript:SubMenuDrop(2,0,'COMPANY');">Summary</asp:ListItem>
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" CssClass="mobile_display_off_cell">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="folders_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                            <asp:ListItem>Folders</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="folders_submenu_dropdown" runat="server" CssClass="ul_bottom folder_dropdown"
                                            DisplayMode="HyperLink">
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="180" ID="company_paging"
                                    CssClass="mobile_vertical_align_bottom mobile_display_off_cell">
                                    <asp:Label ID="Label2" runat="server" CssClass="criteria_text criteria_spacer">
                                        <asp:ImageButton ID="company_previous_all" ImageUrl="../images/previous_all.png"
                                            runat="server" CssClass="display_none" CommandName="previous_all" OnClick="MoveNext" />&nbsp;<asp:ImageButton
                                                ID="company_previous" ImageUrl="../images/previous_listing.png" CssClass="display_none"
                                                runat="server" CommandName="previous" OnClick="MoveNext" />&nbsp;<asp:Label ID="company_record_count"
                                                    runat="server">Showing 25 - 50</asp:Label>&nbsp;<asp:ImageButton ID="company_next"
                                                        CssClass="display_none" ImageUrl="../images/next_listing.png" runat="server" CommandName="next"
                                                        OnClick="MoveNext" />&nbsp;<asp:ImageButton CssClass="display_none" ID="company_next_all"
                                                            ImageUrl="~/images/next_all.png" runat="server" CommandName="next_all" OnClick="MoveNext" /></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Company_Collapse_Panel" runat="server" Height="0px" Width="100%" CssClass="collapse">
                <asp:Label runat="server" ID="close_current_folder" Font-Bold="true" ForeColor="Red"
                    Visible="false"><br /><br /><p align="center" class="medium_text">You must Close Current Folder before starting a New Search.</p><br /><br /></asp:Label>
                <asp:Table ID="Table4" Width="100%" CellPadding="3" CellSpacing="0" runat="server"
                    CssClass="mobileWhiteBackground">
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="200" ID="companyNameCell">
                            <span class="companyMobileHalf">Company Name:</span>
                            <span class="companyMobileHalf2">
                                <asp:TextBox ID="company_name" runat="server" Width="100%" Rows="1" Height="12px" TextMode="MultiLine"></asp:TextBox>
                            </span>
                            <asp:CheckBox runat="server" ID="chkShowInactiveCompany" Visible="false" Text="Show Inactive Companies?" /><br />
                            <asp:CheckBox runat="server" ID="chkShowHiddenCompany" Visible="false" Text="Show Hidden Companies?" />
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" CssClass="displayNoneMobile">
                            Agency Type:
              <asp:DropDownList ID="company_agency_type" runat="server" Width="100%">
                  <asp:ListItem Value="">All</asp:ListItem>
                  <asp:ListItem Value="C">Civilian</asp:ListItem>
                  <asp:ListItem Value="G">Government</asp:ListItem>
              </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="170" RowSpan="2"
                            CssClass="displayNoneMobile">
                            <asp:Label runat="server" ID="relationship_text">Relationships to Aircraft:</asp:Label>
                            <br class="clear" />
                            <asp:CheckBox ID="comp_not_in_selected" Text="Not in Selected Relationship" runat="server" />
                            <asp:ListBox ID="company_relationship" runat="server" Width="100%" SelectionMode="Multiple"
                                Rows="6">
                                <asp:ListItem>All</asp:ListItem>
                            </asp:ListBox>
                            <asp:Panel runat="server" ID="typeOfSearch" Visible="false" Style="margin-top: 20px">
                                Search In:
                <asp:DropDownList runat="server" ID="searchTypeDropdown">
                    <asp:ListItem Value="C">Client Data Only</asp:ListItem>
                    <asp:ListItem Value="" Selected="True">JETNET Data Only</asp:ListItem>
                    <asp:ListItem Value="B">JETNET &amp; Client Data</asp:ListItem>
                </asp:DropDownList>
                            </asp:Panel>
                        </asp:TableCell>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2" BackColor="#CFCFCF"
                            CssClass="displayNoneMobile">
                            <asp:Label ID="comp_contacts_yacht_label" runat="server" Visible="false"></asp:Label>
                            <asp:CheckBox ID="comp_product_helicopter_flag" runat="server" Checked="true" Text="Helicopter" />
                            <asp:CheckBox ID="comp_product_business_flag" runat="server" Checked="true" Text="Business" />
                            <asp:CheckBox ID="comp_product_commercial_flag" runat="server" Checked="true" Text="Commercial" />
                            <asp:CheckBox ID="comp_product_yacht_flag" runat="server" Checked="true" Text="Yacht" />

                            <asp:Label runat="server" ID="product_spacer"><br />
              <hr class="remove_margin" /></asp:Label>
                            <asp:CheckBox ID="company_contact_info" runat="server" Text="Display Contact Info" />
                            <asp:CheckBox ID="company_aircraft_sales" runat="server" Text="Only Aircraft Sales Professionals" /><br />
                            <asp:CheckBox runat="server" ID="chkShowHiddenContact" Visible="false" Text="Show Hidden Contacts?" />
                            <asp:CheckBox runat="server" ID="chkShowInactiveContact" Visible="false" Text="Show Inactive Contacts?" /><br />
                            <asp:CheckBox ID="goto_subscriberSearch" runat="server" Checked="false" Text="Search Customers Only" AutoPostBack="true" Visible="false" />

                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                            <table width="100%" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td align="left" valign="top" width="41" class="displayNoneMobile">Address:
                                    </td>
                                    <td align="left" valign="top" colspan="2" class="displayNoneMobile">
                                        <asp:TextBox ID="company_address" runat="server" Width="96%"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" width="76" runat="server" id="businessTypeCell">Business Type:
                                    </td>
                                    <td align="left" valign="top" width="160" class="displayNoneMobile">
                                        <asp:ListBox ID="company_business" runat="server" Width="100%" Rows="4" SelectionMode="Multiple">
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                    <td align="left" valign="top" width="110" runat="server" id="businessTypeAnswerCell"
                                        class="mobile_display_on_cell">
                                        <asp:DropDownList runat="server" ID="mobile_company_business" Width="100%">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">City:
                                    </td>
                                    <td align="left" valign="top" width="90">
                                        <asp:TextBox ID="comp_city" runat="server" Width="96%" Rows="1" Height="12px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td align="center" valign="top" width="68" class="displayNoneMobile">Postal Code:
                                    </td>
                                    <td align="left" valign="top" colspan="2" class="displayNoneMobile">
                                        <asp:TextBox ID="comp_zip_code" runat="server" Width="96%" Rows="1" Height="12px"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="mobile_display_on_cell">State:
                                    </td>
                                    <td align="left" valign="top" width="90" class="mobile_display_on_cell">
                                        <asp:DropDownList runat="server" ID="mobileStateOptions" Width="100%">
                                            <asp:ListItem>ALL</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="mobile_display_on_cell">Country:
                                    </td>
                                    <td align="left" valign="top" width="90" class="mobile_display_on_cell">
                                        <asp:DropDownList runat="server" ID="mobileCountryOptions" Width="100%">
                                            <asp:ListItem>ALL</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="mobile_display_on_cell">Relationship To A/C:
                                    </td>
                                    <td align="left" valign="top" colspan="2" class="mobile_display_on_cell">
                                        <div id="mobileRadioButtonAnswer">
                                            <asp:RadioButtonList runat="server" ID="mobileYesNoDropDown" RepeatDirection="Horizontal">
                                                <asp:ListItem>Yes</asp:ListItem>
                                                <asp:ListItem Selected="True">No</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td id="mobileHide1" align="left" valign="top" class="display_none">Type:
                                    </td>
                                    <td id="mobileHide2" align="left" valign="top" colspan="2" class="display_none">
                                        <asp:DropDownList ID="mobile_company_relationship" runat="server" Width="97%" CssClass="margin_4">
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="left" valign="top" width="70" colspan="12" class="displayNoneMobile">
                                        <table id="company_block">
                                            <tr>
                                                <td align="left" valign="top" width="90">
                                                    <a href="/help/documents/650.pdf" target="_blank">Certifications</a>:
                                                </td>
                                                <td align="left" valign="top" width="300" class="displayNoneMobile">
                                                    <asp:ListBox ID="comp_certifications" runat="server" Width="100%" Rows="4" SelectionMode="Multiple">
                                                        <asp:ListItem>All</asp:ListItem>
                                                    </asp:ListBox>
                                                </td>
                                                <td align="left" valign="top" width="70" class="displayNoneMobile">
                                                    <a href="/help/documents/650.pdf" target="_blank">Memberships/Accreditations</a>:
                                                </td>
                                                <td align="left" valign="top" width="190" class="displayNoneMobile">
                                                    <asp:ListBox ID="comp_member_accred" runat="server" Width="100%" Rows="4" SelectionMode="Multiple">
                                                        <asp:ListItem>All</asp:ListItem>
                                                    </asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <asp:TextBox ID="comp_id" runat="server" CssClass="display_none"></asp:TextBox>
                            <asp:TextBox ID="clicomp_id" runat="server" CssClass="display_none"></asp:TextBox>
                            <asp:TextBox ID="contact_id" runat="server" CssClass="display_none"></asp:TextBox>
                            <asp:TextBox ID="comp_folder_name" runat="server" CssClass="display_none"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell ID="fleet_cell" HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2"
                            CssClass="seperator_top_bottom lighter_blue_search displayNoneMobile">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="top">Aircraft Fleet (Owner/Operator):
                    <asp:UpdatePanel runat="server" ID="fleet_update_panel">
                        <ContentTemplate>
                            <table width="100%" cellpadding="3" cellspacing="0">
                                <tr>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="company_fleet" runat="server">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem>Operator</asp:ListItem>
                                            <asp:ListItem>Owner</asp:ListItem>
                                            <asp:ListItem>Co-Owner</asp:ListItem>
                                            <asp:ListItem>Fractional Owner</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:DropDownList ID="company_condition" runat="server" AutoPostBack="true">
                                            <asp:ListItem Value=""></asp:ListItem>
                                            <asp:ListItem>Equals</asp:ListItem>
                                            <asp:ListItem>Less Than</asp:ListItem>
                                            <asp:ListItem>Greater Than</asp:ListItem>
                                            <asp:ListItem>Between</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" valign="top">
                                        <asp:TextBox ID="company_fleet_value" runat="server" Width="100%"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="TBWE2" runat="server" TargetControlID="company_fleet_value"
                                            WatermarkText="nnnn" WatermarkCssClass="watermarked" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="company_fleet_value"
                                            ValidationExpression="^\d*[0-9](\:\d*[0-9])?$" runat="server" ErrorMessage="nnnn or nnnn:nnnn only"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="vertical-align: top; text-align: left;">
                                        <asp:Label ID="service_usedLabel" runat="server" Text="Services&nbsp;Used:" Visible="false"></asp:Label>
                                        <br />
                                        <asp:ListBox ID="service_used" runat="server" Width="115" Rows="4" SelectionMode="Multiple" Visible="false">
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                                        <asp:Panel runat="server" ID="yacht_panel" CssClass="lighter_blue_search displayNoneMobile" Visible="false">
                                            <asp:UpdatePanel runat="server" ID="yacht_update_panel">
                                                <ContentTemplate>
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" valign="top">Yacht Fleet:
                                <asp:DropDownList ID="yacht_fleet" runat="server">
                                    <asp:ListItem Selected="True" Value="">Search Yacht Company Directory</asp:ListItem>
                                    <asp:ListItem Value="Show Yacht Owners">Companies Owning Yachts</asp:ListItem>
                                    <asp:ListItem Value="Show Companies Related to Yachts">Companies Related to Yachts</asp:ListItem>
                                    <asp:ListItem Value="Show Companies Not Owning Yachts">Companies Not Owning Yachts</asp:ListItem>
                                    <asp:ListItem Value="Show Companies Not Related to Yachts">Companies Not Related to Yachts</asp:ListItem>
                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                        <asp:TableCell ID="yacht_ac_cell" HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2"
                            CssClass="seperator_top_bottom lighter_blue_search displayNoneMobile">
                            <asp:Panel runat="server" ID="ac_panel" CssClass="lighter_blue_search" Visible="false">
                                <asp:UpdatePanel runat="server" ID="ac_update_panel">
                                    <ContentTemplate>
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="top">Aircraft Fleet:
                          <asp:DropDownList ID="ac_fleet" runat="server">
                              <asp:ListItem Selected="True" Value="">Search Aircraft Company Directory</asp:ListItem>
                              <asp:ListItem Value="Show AC Owners">Companies Owning Any Aircraft</asp:ListItem>
                          </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow CssClass="displayNoneMobile">
                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="3" ColumnSpan="3">
                            <asp:Panel ID="Panel1" Width="100%" runat="server" CssClass="region_panel">
                                <evo:viewCCSTDropDowns ID="viewCCSTDropDowns" runat="server" />
                            </asp:Panel>

                            <script language="javascript" type="text/javascript">
                                checkRadioButtons(bIsBaseCompany, bIsViewCompany, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);
                            </script>

                        </asp:TableCell>
                        <asp:TableCell runat="server" CssClass="lighter_gray_search">
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left" valign="top" class="lighter_gray_search">Contact First Name:
                    <asp:TextBox ID="company_contact_first" runat="server" Width="97%"></asp:TextBox><br />
                                        Contact Last Name:
                    <asp:TextBox ID="company_contact_last" runat="server" Width="97%"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" class="lighter_gray_search">Contact Title Group:
                    <asp:ListBox ID="company_contact_title" runat="server" Width="97%" SelectionMode="Multiple">
                        <asp:ListItem>All</asp:ListItem>
                    </asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" class="lighter_gray_search">Email Address:
                    <asp:TextBox ID="company_email_address" runat="server" Width="97%" Rows="1" Height="12px"
                        TextMode="MultiLine"></asp:TextBox><br />
                                    </td>
                                    <td align="left" valign="top" class="lighter_gray_search">Phone Number:
                    <asp:TextBox ID="company_phone" runat="server" Width="97%" Rows="1" Height="12px"
                        TextMode="MultiLine"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Company ID:
                    <asp:TextBox ID="company_id_text" runat="server" TextMode="MultiLine" Rows="1"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="right" ID="searchCell" VerticalAlign="Top" ColumnSpan="2">
                            <asp:Button ID="company_search" runat="server" Text="Search" OnClientClick="javascript:FillStateHiddenValue(2);"
                                CssClass="button_width button-darker" /><br />
                            <asp:Button ID="reset_form" runat="server" Text="Clear Selections" CssClass="button_width font-weight-normal displayNoneMobile" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>

                <asp:Panel runat="server" ID="customer_targets_panel" CssClass="mobileWhiteBackground" Visible="false">
                    <table id="customer_targets_table" border="0" style="padding: 4px; border-spacing: 6px; text-align: left; width: 45%;">
                        <tr>
                            <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 18px;" colspan="4">
                                <asp:CheckBox ID="chkSearchCustomerTargets" runat="server" Text="Search Customer Targets" />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Previous Customer:<br />
                                <asp:DropDownList ID="targets_previous_customer" runat="server">
                                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="YesNo" Text="Yes/No" Selected="true"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Prospect:<br />
                                <asp:DropDownList ID="targets_prospect" runat="server">
                                    <asp:ListItem Value="Yes" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="No" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="YesNo" Text="Yes/No" Selected="true"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Services Used:<br />
                                <asp:ListBox ID="targets_services_used" runat="server" Width="115" Rows="4">
                                    <asp:ListItem Value="" Text=""></asp:ListItem>
                                </asp:ListBox>
                            </td>
                            <td style="vertical-align: top; text-align: left; padding-left: 8px; padding-top: 8px;">Customer Segments:<br />
                                <asp:ListBox ID="targets_customer_segments" runat="server" Width="145" Rows="4" SelectionMode="Multiple">
                                    <asp:ListItem Value="" Text="None" Selected="true"></asp:ListItem>
                                    <asp:ListItem Value="jet" Text="Jet Dealers"></asp:ListItem>
                                    <asp:ListItem Value="turbo" Text="Turbo Props/Piston Dealers"></asp:ListItem>
                                    <asp:ListItem Value="heli" Text="Helicopter Dealers"></asp:ListItem>
                                </asp:ListBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>

                <asp:Panel runat="server" ID="company_custom_fields" Visible="false">
                    <asp:Table ID="company_custom" runat="server">
                        <asp:TableRow ID="advanced_search_categories">
                            <asp:TableCell ColumnSpan="7" HorizontalAlign="Left" VerticalAlign="Top">
                                <strong><u>Company Custom Data:</u></strong>&nbsp;&nbsp;<asp:ImageButton ID="infoButton1"
                                    runat="server" Height="15" ImageUrl="~/images/info.png" Visible="true" />
                                <table width="100%" cellpadding="3" cellspacing="0">
                                    <tr>
                                        <td align="left" valign="top" width="150">
                                            <asp:Label runat="server" ID="custom_pref_name1"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text1" Width="98%"></asp:TextBox>
                                        </td>
                                        <td align="left" valign="top" width="150">
                                            <asp:Label runat="server" ID="custom_pref_name2"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text2" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:Label runat="server" ID="custom_pref_name3"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text3" Width="98%"></asp:TextBox>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label runat="server" ID="custom_pref_name4"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text4" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:Label runat="server" ID="custom_pref_name5"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text5" Width="98%"></asp:TextBox>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label runat="server" ID="custom_pref_name6"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text6" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:Label runat="server" ID="custom_pref_name7"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text7" Width="98%"></asp:TextBox>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label runat="server" ID="custom_pref_name8"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text8" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:Label runat="server" ID="custom_pref_name9"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text9" Width="98%"></asp:TextBox>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:Label runat="server" ID="custom_pref_name10"></asp:Label>
                                        </td>
                                        <td align="left" valign="top">
                                            <asp:TextBox runat="server" ID="custom_pref_text10" Width="98%"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>

            </asp:Panel>
        </div>
    </asp:Panel>
    <asp:Label runat="server" ID="FolderInformation" Visible="false" CssClass="FolderNameBar help_cursor"></asp:Label>
    <asp:Label runat="server" ID="page_type" CssClass="display_none"></asp:Label>
    <div id="divTabLoading" runat="server" class="loadingScreenBox" style="display: none;" align="center">
        <span></span>
        <div class="loader">Loading...</div>
    </div>
    <div class="DataGridShadowContainer CompanyPage">
        <div class="valueSpec aircraftListing Simplistic aircraftSpec">
            <asp:DataGrid runat="server" ID="Results" AutoGenerateColumns="false" Width="100%"
                AllowCustomPaging="false" AllowPaging="true" Visible="false" CssClass="formatTable blue datagrid"
                AlternatingItemStyle-CssClass="alt_row">
                <Columns>
                    <asp:TemplateColumn HeaderText="COMPANY" ItemStyle-CssClass="mobile_display_off_cell"
                        HeaderStyle-CssClass="mobile_display_off_cell label gray">
                        <ItemTemplate>
                            <%#"<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, False, "", "", IIf(DataBinder.Eval(Container.DataItem, "source") = "CLIENT", "&source=CLIENT", "")) & " title=""" & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name_type")), DataBinder.Eval(Container.DataItem, "comp_alternate_name_type").ToString & ": ", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name")), DataBinder.Eval(Container.DataItem, "comp_alternate_name").ToString, "") & """>" & DataBinder.Eval(Container.DataItem, "comp_name").ToString & "</a>"%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="ADDRESS" ItemStyle-CssClass="mobile_display_off_cell"
                        HeaderStyle-CssClass="mobile_display_off_cell label gray">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "comp_address1")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="COMPANY" ItemStyle-CssClass="mobile_display_on_cell"
                        HeaderStyle-CssClass="mobile_display_on_cell label gray">
                        <ItemTemplate>
                            <%#"<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, False, "", "",  iif(DataBinder.Eval(Container.DataItem, "source") = "CLIENT", "&source=CLIENT","")) & " title=""" & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name_type")), DataBinder.Eval(Container.DataItem, "comp_alternate_name_type").ToString & ": ", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name")), DataBinder.Eval(Container.DataItem, "comp_alternate_name").ToString, "") & """>" & DataBinder.Eval(Container.DataItem, "comp_name").ToString & "</a>"%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="CITY" ItemStyle-CssClass="mobile_display_off_cell"
                        HeaderStyle-CssClass="mobile_display_off_cell label gray">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "comp_city")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="STATE" ItemStyle-CssClass="mobile_display_off_cell"
                        HeaderStyle-CssClass="mobile_display_off_cell label gray">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "comp_state")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="COUNTRY" ItemStyle-CssClass="mobile_display_off_cell"
                        HeaderStyle-CssClass="mobile_display_off_cell label gray">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "comp_country")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="LAST CHANGE" Visible="false" HeaderStyle-CssClass="label gray">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "comp_action_date")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="PHONE" ItemStyle-CssClass="mobile_display_off_cell"
                        HeaderStyle-CssClass="mobile_display_off_cell label gray">
                        <ItemTemplate>
                            <%#IIf(DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString <> "", "<span class=""li_no_bullet"">" & DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString & "</span>", "")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="PAST CUST" Visible="false" ItemStyle-CssClass="mobile_display_off_cell"
                        HeaderStyle-CssClass="mobile_display_off_cell label gray">
                        <ItemTemplate>
                            <%# IIf(chkSearchCustomerTargets.Checked, crmWebClient.clsGeneral.clsGeneral.FormatDateShorthand(DataBinder.Eval(Container.DataItem, "PASTCUSTEND")), "")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="PROSPECT" Visible="false" ItemStyle-CssClass="mobile_display_off_cell"
                        HeaderStyle-CssClass="mobile_display_off_cell label gray">
                        <ItemTemplate>
                            <%#IIf(chkSearchCustomerTargets.Checked, DataBinder.Eval(Container.DataItem, "PROSPECT"), "")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="SERVICES" Visible="false" ItemStyle-CssClass="mobile_display_off_cell"
                        HeaderStyle-CssClass="mobile_display_off_cell label gray">
                        <ItemTemplate>
                            <%#IIf(chkSearchCustomerTargets.Checked, DataBinder.Eval(Container.DataItem, "SERVICESUSED"), "")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="CONTACT" Visible="false" HeaderStyle-CssClass="label gray">
                        <ItemTemplate>
                            <%#DisplayContactInfoListing(DataBinder.Eval(Container.DataItem, "comp_id"), DataBinder.Eval(Container.DataItem, "contact_id"), DataBinder.Eval(Container.DataItem, "contact_sirname"), DataBinder.Eval(Container.DataItem, "contact_first_name"), DataBinder.Eval(Container.DataItem, "contact_last_name"), DataBinder.Eval(Container.DataItem, "contact_title"), False, DataBinder.Eval(Container.DataItem, "contact_middle_initial"))%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn HeaderText="TITLE" Visible="false" HeaderStyle-CssClass="label gray">
                        <ItemTemplate>
                            <%#DataBinder.Eval(Container.DataItem, "contact_title")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                    <asp:TemplateColumn Visible="false" HeaderStyle-CssClass="label gray">
                        <ItemTemplate>
                            <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True, crmWebClient.DisplayFunctions.BuildNote(DataBinder.Eval(Container.DataItem, "comp_id"), masterPage.aclsData_Temp, "COMP"), "")%>
                        </ItemTemplate>
                    </asp:TemplateColumn>
                </Columns>
            </asp:DataGrid>

            <asp:Repeater ID="ResultsSearchData" runat="server" Visible="true">
                <AlternatingItemTemplate>
                    <tr class="<%# IIf(DataBinder.Eval(Container.DataItem, "source").ToString = "CLIENT", "CLIENTCRMRow","") %>">
                        <td class="mobile_display_off_cell alt_row">
                            <%#"<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, False, "", "", IIf(DataBinder.Eval(Container.DataItem, "source") = "CLIENT", "&source=CLIENT", "")) & "  class='text_underline' title=""" & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name_type")), DataBinder.Eval(Container.DataItem, "comp_alternate_name_type").ToString & ": ", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name")), DataBinder.Eval(Container.DataItem, "comp_alternate_name").ToString, "") & """>" & DataBinder.Eval(Container.DataItem, "comp_name").ToString & "</a>"%>
                        </td>
                        <td class="mobile_display_off_cell alt_row">
                            <%#DataBinder.Eval(Container.DataItem, "comp_address1")%>
                        </td>
                        <td class="mobile_display_on_cell alt_row">
                            <%#"<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, False, "", "",  iif(DataBinder.Eval(Container.DataItem, "source") = "CLIENT", "&source=CLIENT","")) & " title=""" & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name_type")), DataBinder.Eval(Container.DataItem, "comp_alternate_name_type").ToString & ": ", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name")), DataBinder.Eval(Container.DataItem, "comp_alternate_name").ToString, "") & """>" & DataBinder.Eval(Container.DataItem, "comp_name").ToString & "</a>"%>
                        </td>
                        <td class="mobile_display_off_cell alt_row">
                            <%#DataBinder.Eval(Container.DataItem, "comp_city")%>
                        </td>
                        <td class="mobile_display_off_cell alt_row">
                            <%#DataBinder.Eval(Container.DataItem, "comp_state")%>
                        </td>
                        <td class="mobile_display_off_cell alt_row">
                            <%#DataBinder.Eval(Container.DataItem, "comp_country")%>
                        </td>
                        <td class="mobile_display_off_cell alt_row">
                            <%#IIf(DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString <> "", "<span class=""li_no_bullet"">" & DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString & "</span>", "")%>
                        </td>
                        <%#IIf(chkSearchCustomerTargets.Checked, "<td class=""mobile_display_off_cell alt_row"">" + crmWebClient.clsGeneral.clsGeneral.FormatDateShorthand(DataBinder.Eval(Container.DataItem, "PASTCUSTEND")) + "</td>", "") %>
                        <%#IIf(chkSearchCustomerTargets.Checked, "<td class=""mobile_display_off_cell alt_row"">" + DataBinder.Eval(Container.DataItem, "PROSPECT") + "</td>", "") %>
                        <%#IIf(chkSearchCustomerTargets.Checked, "<td class=""mobile_display_off_cell alt_row"">" + DataBinder.Eval(Container.DataItem, "SERVICESUSED") + "</td>", "") %>
                        <%#IIf(company_contact_info.Checked = True, "<td class=""alt_row"">" & DisplayContactInfoListing(DataBinder.Eval(Container.DataItem, "comp_id"), DataBinder.Eval(Container.DataItem, "contact_id"), DataBinder.Eval(Container.DataItem, "contact_sirname"), DataBinder.Eval(Container.DataItem, "contact_first_name"), DataBinder.Eval(Container.DataItem, "contact_last_name"), DataBinder.Eval(Container.DataItem, "contact_title"), False, DataBinder.Eval(Container.DataItem, "contact_middle_initial")) & "</td>", "")%>
                        <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True, "<td class=""alt_row"">" & crmWebClient.DisplayFunctions.BuildNote(DataBinder.Eval(Container.DataItem, "comp_id"), masterPage.aclsData_Temp, "COMP") & "</td>", "")%>
                    </tr>
                    <%'#DisplayClientCompany(DataBinder.Eval(Container.DataItem, "comp_id"), False, DataBinder.Eval(Container.DataItem, "contact_id"), DataBinder.Eval(Container.DataItem, "contact_id"))%>
                </AlternatingItemTemplate>
                <HeaderTemplate>
                    <div class=" gray_background_color">
                        <table width="100%" class="formatTable blue datagrid" cellpadding="0" cellspacing="0"
                            style="border-collapse: collapse !important; border-spacing: 0px;">
                            <tr>
                                <td class="mobile_display_off_cell label gray">COMPANY
                                </td>
                                <td class="mobile_display_off_cell label gray">ADDRESS
                                </td>
                                <td class="mobile_display_on_cell label gray">COMPANY
                                </td>
                                <td class="mobile_display_off_cell label gray">CITY
                                </td>
                                <td class="mobile_display_off_cell label gray">STATE
                                </td>
                                <td class="mobile_display_off_cell label gray">COUNTRY
                                </td>
                                <td class="mobile_display_off_cell label gray">PHONE
                                </td>
                                <%#IIf(chkSearchCustomerTargets.Checked, "<td class=""mobile_display_off_cell label gray"">PAST CUST</td>", "") %>
                                <%#IIf(chkSearchCustomerTargets.Checked, "<td class=""mobile_display_off_cell label gray"">PROSPECT</td>", "") %>
                                <%#IIf(chkSearchCustomerTargets.Checked, "<td class=""mobile_display_off_cell label gray"">SERVICES</td>", "") %>
                                <%#IIf(company_contact_info.Checked = True, "<td class=""mobile_display_off_cell label gray"">CONTACT</td>", "")%>
                                <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True, "<td class=""label gray""></td>", "")%>
                            </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr class="<%# IIf(DataBinder.Eval(Container.DataItem, "source").ToString = "CLIENT", "CLIENTCRMRow","") %>">
                        <td class="mobile_display_off_cell">
                            <%#"<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, False, "", "", IIf(DataBinder.Eval(Container.DataItem, "source") = "CLIENT", "&source=CLIENT", "")) & "  class='text_underline' title=""" & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name_type")), DataBinder.Eval(Container.DataItem, "comp_alternate_name_type").ToString & ": ", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name")), DataBinder.Eval(Container.DataItem, "comp_alternate_name").ToString, "") & """>" & DataBinder.Eval(Container.DataItem, "comp_name").ToString & "</a>"%>
                        </td>
                        <td class="mobile_display_off_cell">
                            <%#DataBinder.Eval(Container.DataItem, "comp_address1")%>
                        </td>
                        <td class="mobile_display_on_cell">
                            <%#"<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, False, "", "",  iif(DataBinder.Eval(Container.DataItem, "source") = "CLIENT", "&source=CLIENT","")) & " title=""" & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name_type")), DataBinder.Eval(Container.DataItem, "comp_alternate_name_type").ToString & ": ", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name")), DataBinder.Eval(Container.DataItem, "comp_alternate_name").ToString, "") & """>" & DataBinder.Eval(Container.DataItem, "comp_name").ToString & "</a>"%>
                        </td>
                        <td class="mobile_display_off_cell">
                            <%#DataBinder.Eval(Container.DataItem, "comp_city")%>
                        </td>
                        <td class="mobile_display_off_cell">
                            <%#DataBinder.Eval(Container.DataItem, "comp_state")%>
                        </td>
                        <td class="mobile_display_off_cell">
                            <%#DataBinder.Eval(Container.DataItem, "comp_country")%>
                        </td>
                        <td class="mobile_display_off_cell">
                            <%#IIf(DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString <> "", "<span class=""li_no_bullet"">" & DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString & "</span>", "")%>
                        </td>
                        <%#IIf(chkSearchCustomerTargets.Checked, "<td class=""mobile_display_off_cell"">" + crmWebClient.clsGeneral.clsGeneral.FormatDateShorthand(DataBinder.Eval(Container.DataItem, "PASTCUSTEND")) + "</td>", "") %>
                        <%#IIf(chkSearchCustomerTargets.Checked, "<td class=""mobile_display_off_cell"">" + DataBinder.Eval(Container.DataItem, "PROSPECT") + "</td>", "") %>
                        <%#IIf(chkSearchCustomerTargets.Checked, "<td class=""mobile_display_off_cell"">" + DataBinder.Eval(Container.DataItem, "SERVICESUSED") + "</td>", "") %>
                        <%#IIf(company_contact_info.Checked = True, "<td>" & DisplayContactInfoListing(DataBinder.Eval(Container.DataItem, "comp_id"), DataBinder.Eval(Container.DataItem, "contact_id"), DataBinder.Eval(Container.DataItem, "contact_sirname"), DataBinder.Eval(Container.DataItem, "contact_first_name"), DataBinder.Eval(Container.DataItem, "contact_last_name"), DataBinder.Eval(Container.DataItem, "contact_title"), False, DataBinder.Eval(Container.DataItem, "contact_middle_initial")) & "</td>", "")%>
                        <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True, "<td>" & crmWebClient.DisplayFunctions.BuildNote(DataBinder.Eval(Container.DataItem, "comp_id"), masterPage.aclsData_Temp, "COMP") & "</td>", "")%>
                    </tr>
                    <%'#DisplayClientCompany(DataBinder.Eval(Container.DataItem, "comp_id"), False, DataBinder.Eval(Container.DataItem, "contact_id"))%>
                </ItemTemplate>
                <FooterTemplate>
                    </table></div>
                </FooterTemplate>
            </asp:Repeater>
            <div class="grid">
                <asp:DataList ID="ResultsSearchDataList" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                    AutoGenerateColumns="False" GridLines="none" AllowPaging="true" Visible="true"
                    CssClass="formatTable blue gray_background_color">
                    <ItemStyle CssClass="grid-item" />
                    <ItemTemplate>
                        <div class="Box">
                            <div class="row  <%# IIf(DataBinder.Eval(Container.DataItem, "source").ToString = "CLIENT", "CLIENTCRMRow","") %>">
                                <div class="columns five remove_margin">
                                    <%#DisplayCompanyLogo(DataBinder.Eval(Container.DataItem, "comp_logo_flag").ToString, DataBinder.Eval(Container.DataItem, "comp_id"))%>
                                </div>
                                <div class="columns seven remove_margin float_right">
                                    <span class="float_right mobileWidth" style="width: 100%;">
                                        <h2 class='mainHeading'>
                                            <%#"<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, False, "", "",  iif(DataBinder.Eval(Container.DataItem, "source") = "CLIENT", "&source=CLIENT","")) & " title=""" & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name_type")), DataBinder.Eval(Container.DataItem, "comp_alternate_name_type").ToString & ": ", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "comp_alternate_name")), DataBinder.Eval(Container.DataItem, "comp_alternate_name").ToString, "") & """>" & DataBinder.Eval(Container.DataItem, "comp_name").ToString & "</a>"%>
                                        </h2>
                                        <%#IIf(DataBinder.Eval(Container.DataItem, "comp_address1").ToString <> "", "<span class='li displayNoneMobile'>" & DataBinder.Eval(Container.DataItem, "comp_address1").ToString & "</span>", "")%>
                                        <%#IIf(DataBinder.Eval(Container.DataItem, "comp_address2").ToString <> "", "<span class='li displayNoneMobile'>" & DataBinder.Eval(Container.DataItem, "comp_address2").ToString & "</span>", "")%>
                                        <%#IIf(DataBinder.Eval(Container.DataItem, "comp_city").ToString <> "" Or DataBinder.Eval(Container.DataItem, "comp_state").ToString <> "" Or DataBinder.Eval(Container.DataItem, "comp_country").ToString <> "", "<span class='li'>" & IIf(DataBinder.Eval(Container.DataItem, "comp_city").ToString <> "", DataBinder.Eval(Container.DataItem, "comp_city").ToString & ", ", "") & DataBinder.Eval(Container.DataItem, "comp_state").ToString & " " & IIf(Session.Item("isMobile") = False, DataBinder.Eval(Container.DataItem, "comp_country").ToString, Replace(DataBinder.Eval(Container.DataItem, "comp_country").ToString, "United States", "US")) & "</span>", "")%>
                                        <%#IIf(DataBinder.Eval(Container.DataItem, "comp_zip_code").ToString <> "", "<span class='li displayNoneMobile'>" & DataBinder.Eval(Container.DataItem, "comp_zip_code").ToString & "</span>", "")%>
                                        <%#IIf(DataBinder.Eval(Container.DataItem, "comp_email_address").ToString <> "", "<span class='li displayNoneMobile'><a href='mailto:" & DataBinder.Eval(Container.DataItem, "comp_email_address").ToString & "'>" & DataBinder.Eval(Container.DataItem, "comp_email_address").ToString & "</a></span>", "")%>
                                        <%#IIf(DataBinder.Eval(Container.DataItem, "comp_web_address").ToString <> "", "<span class='" & IIf(Session.Item("isMobile") = True, "", "li") & "'><a href='http://www." & Replace(Replace(DataBinder.Eval(Container.DataItem, "comp_web_address").ToString, "http://", ""), "www.", "") & "' target='new' title='http://www." & Replace(Replace(DataBinder.Eval(Container.DataItem, "comp_web_address").ToString, "http://", ""), "www.", "") & "'>" & IIf(Session.Item("isMobile") = True, "<i class=""fa fa-globe"" aria-hidden=""true""></i>", DataBinder.Eval(Container.DataItem, "comp_web_address").ToString) & "</a></span>", "")%>
                                        <%#IIf(DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString <> "", IIf(Session.Item("isMobile") = True, "<a href=""tel:" & DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString & """ title=""" & DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString & """><i class=""fa fa-phone"" aria-hidden=""true""></i></a>", "<span class=""li""><span class='label'>Phone: </span>" & DataBinder.Eval(Container.DataItem, "comp_phone_office").ToString) & "</span>", "")%>
                                        <%#IIf(DataBinder.Eval(Container.DataItem, "comp_phone_fax").ToString <> "", "<span class=""li displayNoneMobile""><span class='label'>Fax: </span>" & DataBinder.Eval(Container.DataItem, "comp_phone_fax").ToString & "</span>", "")%>
                                        <%#DisplayContactInfoListing(DataBinder.Eval(Container.DataItem, "comp_id"), DataBinder.Eval(Container.DataItem, "contact_id"), DataBinder.Eval(Container.DataItem, "contact_sirname"), DataBinder.Eval(Container.DataItem, "contact_first_name"), DataBinder.Eval(Container.DataItem, "contact_last_name"), DataBinder.Eval(Container.DataItem, "contact_title"), True, DataBinder.Eval(Container.DataItem, "contact_middle_initial"))%>
                                    </span>
                                    <br class="div_clear" />
                                    <%'#DisplayClientCompany(DataBinder.Eval(Container.DataItem, "comp_id"), True, DataBinder.Eval(Container.DataItem, "contact_id"))%>
                                </div>
                                <div class="clear_left">
                                </div>
                                <div class="columns five remove_margin expandedLinks">
                                    <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True, crmWebClient.DisplayFunctions.BuildNote(DataBinder.Eval(Container.DataItem, "comp_id"), masterPage.aclsData_Temp, "COMP"), "")%>
                                    <div class="float_left">
                                        <ul class="cssMenu">
                                            <li><a href="#" class="expand_more text_underline">MORE</a>
                                                <ul>
                                                    <li>
                                                        <%#crmWebClient.DisplayFunctions.WriteDetailsLink(0, DataBinder.Eval(Container.DataItem, "comp_id"), 0, 0, True, "Map Company", "",  iif(DataBinder.Eval(Container.DataItem, "source") = "CLIENT", "&source=CLIENT","") & "&map=1")%></li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    <br class="div_clear" />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            <div class="clear"></div>
        </div>
        <asp:Label ID="company_attention" runat="server" Text="" CssClass="red_text emphasis_text text_align_center small_to_medium_text mobileTopPaddingAttention"></asp:Label>
        <asp:Panel runat="server" ID="Company_Bottom_Paging" Visible="false">
            <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="6" CellSpacing="0"
                border="0" CssClass="dark_header">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
            <img src="images/spacer.gif" alt="" height="15" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="company_go_to_text"
                        CssClass="mobile_display_off_cell">
                        Go To:&nbsp;
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="company_go_to_dropdown_"
                        CssClass="mobile_display_off_cell">
                        <div class="action_dropdown_container">
                            <asp:BulletedList ID="company_go_to_dropdown" runat="server" CssClass="ul_top per_page_width">
                                <asp:ListItem>1</asp:ListItem>
                            </asp:BulletedList>
                            <asp:BulletedList ID="company_go_to_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown"
                                OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                            </asp:BulletedList>
                        </div>
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="180">
                        <asp:Label ID="bottom_paging" runat="server" CssClass="criteria_text criteria_spacer">
                            <asp:ImageButton ID="bottom_previous_all" ImageUrl="../images/previous_all.png" runat="server"
                                CommandName="previous_all" CssClass="display_none" OnClick="MoveNext" OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />&nbsp;<asp:ImageButton
                                    ID="bottom_previous" ImageUrl="../images/previous_listing.png" CssClass="display_none"
                                    runat="server" OnClick="MoveNext" OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"
                                    CommandName="previous" />&nbsp;<asp:Label ID="bottom_record_count" runat="server">Showing 25 - 50</asp:Label>&nbsp;<asp:ImageButton
                                        ID="bottom_next_" ImageUrl="../images/next_listing.png" runat="server" OnClick="MoveNext"
                                        OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"
                                        CommandName="next" CssClass="display_none" />&nbsp;<asp:ImageButton ID="bottom_next_all"
                                            ImageUrl="~/images/next_all.png" runat="server" OnClick="MoveNext" OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');"
                                            CommandName="next_all" CssClass="display_none" /></asp:Label>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>

    <script type="text/javascript">



        //Automatically submit on enter press
        $(function () {
            
            $('textarea').on('keyup', function (e) {
                if (e.keyCode == 13) {
                    $("#<%= company_search.ClientID %>").click();
                }
            });
        });
    </script>

</asp:Content>
