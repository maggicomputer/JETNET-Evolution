<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="YachtListing.aspx.vb"
    Inherits="crmWebClient.YachtListing" MasterPageFile="~/EvoStyles/YachtTheme.Master"
    StylesheetTheme="Evo" EnableEventValidation="false" %>

<%@ Register Src="~/controls/yachtTypeSizeBrandModel.ascx" TagName="tabTSBMDropDowns"
    TagPrefix="yacht" %>
<%@ MasterType VirtualPath="~/EvoStyles/YachtTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        //These functions are there for the popout link to the view. This means it's opening up the model market summary.
        function SetViewName() {
            return "Model Market Summary";
        }
        function SetViewID() {
            return 1;
        }

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
        /*Smart Phones*/
        @media (max-width: 599px) {
            .dataListSeperator, .mGrid img.mainImage {
                width: 100%;
            }
        }

        /*Tablet Portrait*/
        @media (min-width: 600px) {
            .dataListSeperator, .mGrid img.mainImage {
                width: 100%;
            }
        }

        /*Tablet Landscape*/
        @media (min-width: 900px) {
            .dataListSeperator, .mGrid img.mainImage {
                width: 100%;
            }
        }

        @media (min-width: 1200px) {
            .dataListSeperator, .mGrid img.mainImage {
                width: 100%;
            }
        }

        @media (min-width: 1800px) {
            .dataListSeperator, .mGrid img.mainImage {
                width: 48%;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="imgDisplayFolder" runat="server" Visible="false"></asp:Label>
    <div class="DataGridShadowContainer">
        <asp:Panel runat="server" ID="Yacht_Criteria">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left" valign="top" class="dark_header" width="100%">
                        <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
                            CssClass="padding_table">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="20" ID="help_text">
                        
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="80" ID="search_expand_text">
                                    <asp:Panel ID="Control_Panel" runat="server" Width="100%">
                                        <asp:Image ID="ControlImage" runat="server" ImageUrl="../Images/search_expand.jpg" />
                                    </asp:Panel>
                                    <asp:Label runat="server" ID="StaticFolderNewSearchLabel"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="results_text">
                                    <asp:Label ID="criteria_results" runat="server" Text="Label">798 Results</asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="sort_by_text">
                        Sort By: 
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="sort_by_dropdown">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="sort_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                            <asp:ListItem>Brand</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="sort_submenu_dropdown" runat="server" CssClass="ul_bottom sort_dropdown"
                                            OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                            <asp:ListItem>Year</asp:ListItem>
                                            <asp:ListItem>Name</asp:ListItem>
                                            <asp:ListItem>Hull #</asp:ListItem>
                                            <asp:ListItem>Brand/Model</asp:ListItem>
                                            <asp:ListItem>Model</asp:ListItem>
                                            <asp:ListItem>LOA</asp:ListItem>
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="per_page_text">
                        Per Page:&nbsp;
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="per_page_dropdown_">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="per_page_dropdown" runat="server" CssClass="ul_top per_page_width">
                                            <asp:ListItem Value="10">10</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="per_page_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown"
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
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="go_to_text">
                        Go To:&nbsp;
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="go_to_dropdown_">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="go_to_dropdown" runat="server" CssClass="ul_top per_page_width">
                                            <asp:ListItem>1</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="go_to_submenu_dropdown" runat="server" CssClass="ul_bottom per_page_dropdown"
                                            OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="45" ID="view_dropdown_">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="view_dropdown" runat="server" CssClass="ul_top thumnail_view_bullet">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="view_submenu_dropdown" runat="server" CssClass="ul_bottom thumbnail"
                                            OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                            <asp:ListItem>Listing</asp:ListItem>
                                            <asp:ListItem>Gallery</asp:ListItem>
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="75" ID="action_dropdown">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="actions_dropdown" runat="server" CssClass="ul_top">
                                            <asp:ListItem>Actions</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="actions_submenu_dropdown" runat="server" CssClass="ul_bottom yacht_action_dropdown"
                                            DisplayMode="HyperLink" OnClick="submenu_dropdown_Click">
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="TableCell1">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="folders_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                            <asp:ListItem>Folders</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="folders_submenu_dropdown" runat="server" CssClass="ul_bottom folder_dropdown"
                                            DisplayMode="HyperLink">
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="180" ID="results_text_">
                                    <asp:Label ID="paging" runat="server" CssClass="criteria_text criteria_spacer">
                                        <asp:ImageButton ID="previous_all" ImageUrl="../images/previous_all.png" runat="server"
                                            Visible="false" />&nbsp;<asp:ImageButton ID="previous" ImageUrl="../images/previous_listing.png"
                                                Visible="false" runat="server" />&nbsp;<asp:Label ID="record_count" runat="server">Showing 25 - 50</asp:Label>&nbsp;<asp:ImageButton
                                                    ID="next_" ImageUrl="../images/next_listing.png" runat="server" />&nbsp;<asp:ImageButton
                                                        ID="next_all" ImageUrl="~/images/next_all.png" runat="server" /></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
            </table>
            <cc1:CollapsiblePanelExtender ID="PanelCollapseEx" runat="server" TargetControlID="Collapse_Panel"
                Collapsed="true" ExpandControlID="Control_Panel" ImageControlID="ControlImage"
                ExpandedImage="../Images/search_collapse.jpg" CollapsedImage="../Images/search_expand.jpg"
                CollapseControlID="Control_Panel" Enabled="True" CollapsedText="New Search" ExpandedText="Hide Search">
            </cc1:CollapsiblePanelExtender>
            <asp:Panel ID="Collapse_Panel" runat="server" Height="0px" Width="100%" CssClass="collapse">
                <asp:Label runat="server" ID="close_current_folder" Font-Bold="true" ForeColor="Red"
                    Visible="false"><br /><br /><p align="center" class="medium_text">You must Close Current Folder before starting a New Search.</p><br /><br /></asp:Label>
                <asp:Table ID="Table2" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                    <asp:TableRow>
                        <asp:TableCell Width="50%" HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:Panel runat="server" ID="yacht_model_box" CssClass="yacht_model_search_box">
                                <asp:Panel ID="Panel2" runat="server" Width="60%">
                                    <yacht:tabTSBMDropDowns ID="tabTSBMDropDowns" runat="server" />
                                </asp:Panel>

                                <script language="javascript" type="text/javascript">
                                    refreshYachtTypeSizeBrandModel("", "");
                                </script>
                                <asp:Table ID="Table10" runat="server" Width="100%" CellPadding="3">
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="150px">
                         Search Mfr/Brand/Model Name:</asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                                            <asp:TextBox ID="search_yt_mfr_brand" runat="server" Width="70%">
                                            </asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                        </asp:TableCell>
                        <asp:TableCell Width="50%" HorizontalAlign="Left" VerticalAlign="Top">
                            <asp:Table ID="Table4" runat="server" Width="100%" CellPadding="3">
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                                        Yacht Name:</asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="115px">
                                        <asp:TextBox ID="yacht_name_search" runat="server" Width="100%">
                                        </asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="3">
                                        <asp:CheckBox runat="server" Text="Search Prev. Names" ID="ypn_previous_name" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                                      Flag:</asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="3">
                                        <asp:DropDownList runat="server" Width="100%" ID="country_registration">
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="113">
                                Length:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:DropDownList runat="server" ID="operator_length" Width="100%">
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="50px">
                                        <asp:TextBox ID="length_to" runat="server" Width="100%">
                                        </asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:CheckBox ID="us_standard" runat="server" Text="US" Checked="true" />
                                        <asp:CheckBox ID="metric_standard" runat="server" Text="Metric" />
                                        <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe1" runat="server" TargetControlID="us_standard"
                                            Key="YesNo" />
                                        <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe2" runat="server" TargetControlID="metric_standard"
                                            Key="YesNo" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="100">
                                     Class:          
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:DropDownList runat="server" ID="yacht_class" Width="100%" CssClass="float_left">
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Call Sign:</asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:TextBox ID="yt_call_sign" runat="server" Width="100%"> 
                                        </asp:TextBox>
                                        <asp:TextBox ID="folder_name" runat="server" CssClass="display_none">
                                        </asp:TextBox>
                                        <asp:TextBox ID="static_folder" runat="server" CssClass="display_none">
                                        </asp:TextBox>
                                        <asp:TextBox ID="static_folder_ids" runat="server" CssClass="display_none">
                                        </asp:TextBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Year Delivered:</asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:DropDownList runat="server" ID="operator_year_dlv" Width="100%">
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:TextBox runat="server" ID="yt_year_dlv" Width="100%">
                                        </asp:TextBox>
                                        <asp:TextBox ID="yt_id" runat="server" Width="76px" CssClass="display_none"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">Year Manufactured:</asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:DropDownList ID="operator_year_mfr" runat="server" Width="100%">
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        <asp:TextBox runat="server" ID="yt_year_mfr" Width="100%">
                                        </asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="event_box" Width="100%" CellPadding="3" CellSpacing="0" runat="server"
                                CssClass="market_search_box">
                                <asp:TableRow runat="server" ID="event_yacht_row" Visible="false">
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3">
                                        <asp:UpdatePanel runat="server" ID="event_update_panel">
                                            <ContentTemplate>
                                                <asp:Table runat="server" Width="100%" CellPadding="3" CellSpacing="0" CssClass="padding_market">
                                                    <asp:TableRow>
                                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                                            <asp:Table runat="server" Width="100%" CellPadding="3" CellSpacing="0">
                                                                <asp:TableRow>
                                                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="120px">
                                                                        Category:<br />
                                                                        <asp:ListBox Rows="4" ID="market_category" runat="server" Width="100%" SelectionMode="Multiple"
                                                                            AutoPostBack="true">
                                                                            <asp:ListItem>All</asp:ListItem>
                                                                        </asp:ListBox>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                                                        Type:<br />
                                                                        <asp:ListBox Rows="4" ID="market_type" runat="server" Width="100%" SelectionMode="Multiple">
                                                                            <asp:ListItem>All</asp:ListItem>
                                                                        </asp:ListBox>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableFooterRow>
                                    <asp:TableCell runat="server" ID="aerodex_toggle_checkboxes" ColumnSpan="4">
                                        <asp:CheckBox runat="server" ID="for_sale" Text="For Sale?" CssClass="float_left" />
                                        <asp:CheckBox runat="server" ID="for_lease" Text="For Lease?" CssClass="float_left" />
                                        <asp:CheckBox runat="server" ID="for_charter" Text="For Charter?" CssClass="float_left"
                                            AutoPostBack="true" />
                                    </asp:TableCell>
                                </asp:TableFooterRow>
                                <asp:TableRow>
                                    <asp:TableCell runat="server" ColumnSpan="4">
                                        For Sale/Charter Restrictions:
                    <asp:DropDownList runat="server" ID="US_waters" Width="246px" CssClass="float_right">
                        <asp:ListItem Value="">None</asp:ListItem>
                        <asp:ListItem Value="N">Exclude Yachts Not Available for sale/charter to US residents while in US waters</asp:ListItem>
                        <asp:ListItem Value="Y">Not Available for sale/charter to US residents while in US waters
                        </asp:ListItem>
                    </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="4" Visible="true"
                                        ID="price_range_toggle">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="top" width="145">Asking Price:
                                                </td>
                                                <td align="left" valign="top" width="90">
                                                    <asp:DropDownList runat="server" ID="operator_asking_price" Width="100%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="right" valign="top" width="81">
                                                    <asp:TextBox runat="server" ID="price_range" Width="81px"></asp:TextBox>
                                                </td>
                                                <td align="right" valign="top">
                                                    <asp:DropDownList runat="server" ID="price_range_currency" Width="100%">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" width="145">Days on Market
                                                </td>
                                                <td align="left" valign="top" width="98">
                                                    <asp:DropDownList runat="server" ID="operator_days_on_market" Width="100%">
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="right" valign="top" width="81">
                                                    <asp:TextBox runat="server" ID="days_on_market" Width="81px"></asp:TextBox>
                                                </td>
                                                <td align="right" valign="top"></td>
                                            </tr>
                                        </table>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ID="aerodex_toggle" HorizontalAlign="left" VerticalAlign="top" Width="30%"
                                        CssClass="">
                                        Market Status:
                    <asp:ListBox ID="yt_market" runat="server" Width="100%" Rows="5" Font-Size="10px"
                        SelectionMode="multiple">
                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                        <asp:ListItem Value="Available">Available</asp:ListItem>
                        <asp:ListItem Value="Auction">Auction</asp:ListItem>
                        <asp:ListItem Value="Deposit">Deposit</asp:ListItem>
                        <asp:ListItem Value="Lease Pending">Lease Pending</asp:ListItem>
                        <asp:ListItem Value="Sale Pending">Sale Pending</asp:ListItem>
                        <asp:ListItem Value="Sealed Bid">Sealed Bid</asp:ListItem>
                        <asp:ListItem Value="See Notes">See Notes</asp:ListItem>
                        <asp:ListItem Value="Unconfirmed">Unconfirmed</asp:ListItem>
                        <asp:ListItem Value="Not For Sale">Not For Sale</asp:ListItem>
                        <asp:ListItem Value="Not Available">Not Available</asp:ListItem>
                        <asp:ListItem Value="Not For Charter">Not For Charter</asp:ListItem>
                    </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell ID="lifecycle_cell" HorizontalAlign="left" VerticalAlign="top">
                                        <span class="extra">Lifecycle:</span>
                                        <asp:ListBox ID="yt_lifecycle_id" runat="server" Width="100%" Rows="3" Font-Size="10px"
                                            SelectionMode="Multiple"></asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell ID="event_toggle_on" ColumnSpan="2" Visible="false">
                                        <asp:Table ID="event_table" Width="250" CellPadding="3" CellSpacing="0" runat="server"
                                            Visible="false" CssClass="lighter_gray_search">
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="4">
                                Find Events that have occurred in the last:
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="50">
                                                    <asp:TextBox ID="events_months" runat="server" Width="50" Text="0" TabIndex="8"></asp:TextBox>
                                                    <br />
                                                    Month(s):
                                                </asp:TableCell>
                                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="50">
                                                    <asp:TextBox ID="event_days" runat="server" Width="50" Text="1" TabIndex="9"></asp:TextBox>
                                                    <br />
                                                    Day(s):
                                                </asp:TableCell>
                                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="50">
                                                    <asp:TextBox ID="event_hours" runat="server" Width="50" Text="0" TabIndex="10"></asp:TextBox>
                                                    <br />
                                                    Hour(s):
                                                </asp:TableCell>
                                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="50">
                                                    <asp:TextBox ID="event_minutes" runat="server" Width="50" Text="0" TabIndex="11"></asp:TextBox>
                                                    <br />
                                                    Minute(s):
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="4">
                                                    <asp:CompareValidator runat="server" ControlToValidate="events_months" ID="Validate_EventMonths"
                                                        ValidationGroup="Numeric" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"
                                                        ErrorMessage="*Incorrect Format (Months must be a number)<br />">
                                                    </asp:CompareValidator>
                                                    <asp:CompareValidator runat="server" ControlToValidate="event_days" ID="Validate_EventDays"
                                                        ValidationGroup="Numeric" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"
                                                        ErrorMessage="*Incorrect Format (Days must be a number)<br />">
                                                    </asp:CompareValidator>
                                                    <asp:CompareValidator runat="server" ControlToValidate="event_hours" ID="Validate_EventHours"
                                                        ValidationGroup="Numeric" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"
                                                        ErrorMessage="*Incorrect Format (Hours must be a number)<br />">
                                                    </asp:CompareValidator>
                                                    <asp:CompareValidator runat="server" ControlToValidate="event_minutes" ID="Validate_EventMinutes"
                                                        ValidationGroup="Numeric" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"
                                                        ErrorMessage="*Incorrect Format (Minutes must be a number)<br />">
                                                    </asp:CompareValidator>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:TableCell>
                                    <asp:TableCell ID="history_toggle_on" ColumnSpan="2" Visible="false">
                                        <asp:Table runat="server" ID="history_table" CellPadding="3" CellSpacing="0">
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                                    Transaction Date:<br />
                                                    <asp:DropDownList ID="journ_date_operator" runat="server" Width="83px" CssClass="margin_right"
                                                        TabIndex="14" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'journ_date', 'input');">
                                                        <asp:ListItem></asp:ListItem>
                                                        <asp:ListItem>Equals</asp:ListItem>
                                                        <asp:ListItem>Less Than</asp:ListItem>
                                                        <asp:ListItem>Greater Than</asp:ListItem>
                                                        <asp:ListItem>Between</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="journ_date" runat="server" Width="100px" TabIndex="15"></asp:TextBox>&nbsp;<asp:Image
                                                        ID="Image1" ImageUrl="~/images/magnify_small.png" runat="server" AlternateText="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;"
                                                        ToolTip="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;" />
                                                    <asp:CustomValidator runat="server" ControlToValidate="journ_date" ID="VALIDATE_TransactionDate"
                                                        ErrorMessage="&nbsp;&nbsp;*Incorrect Format" Font-Bold="true" ValidationGroup="Numeric"
                                                        SetFocusOnError="true" ClientValidationFunction="validateDate" Text="&nbsp;&nbsp;*Incorrect Format"
                                                        Display="Static" Enabled="true"></asp:CustomValidator>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                                    Transaction Type:<br />
                                                    <asp:DropDownList ID="journ_trans_type" runat="server" Width="83px" CssClass="margin_right"
                                                        TabIndex="15">
                                                        <asp:ListItem Value="">All Sales</asp:ListItem>
                                                        <asp:ListItem Value="YC">Charter</asp:ListItem>
                                                        <asp:ListItem Value="YD">Delivery Position Sale</asp:ListItem>
                                                        <asp:ListItem Value="YF">Fractional Sale</asp:ListItem>
                                                        <asp:ListItem Value="YS">Full Sale</asp:ListItem>
                                                        <asp:ListItem Value="YL">Lease</asp:ListItem>
                                                        <asp:ListItem Value="YZ">Seizure</asp:ListItem>
                                                    </asp:DropDownList>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="right" VerticalAlign="top" Width="100px" CssClass="padding_market">
                                        <asp:Button ID="search" runat="server" Text="Search" CssClass="button_width button-darker"
                                            OnClientClick="javascript:FillStateHiddenValue(2);" />
                                        <br />
                                        <asp:Button ID="reset_form" runat="server" Text="Clear Selections" CssClass="button_width font-weight-normal"
                                            OnClick="Reset_Page" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3">
                                   
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <cc1:TabContainer ID="yacht_advanced_search" runat="server" Visible="true" CssClass="dark-theme"
                    AutoPostBack="false">
                    <cc1:TabPanel ID="company_contact_tab" HeaderText="Company/Contact" runat="server">
                        <ContentTemplate>
                            <asp:Table ID="Table3" Width="100%" CellPadding="5" CellSpacing="0" runat="server"
                                CssClass="data_aircraft_grid">
                                <asp:TableRow>
                                    <asp:TableCell CssClass="header_row" ColumnSpan="5"><b>COMPANY/CONTACT DEMOGRAPHICS</b></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="230">
                                        <asp:CheckBox ID="comp_alt_name" Text="Search Alternate Name" runat="server" Font-Size="10px"
                                            CssClass="display_none" Checked="true" />
                                        Company Name:
                    <asp:DropDownList ID="COMPARE_comp_name" runat="server" CssClass="display_none">
                        <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                    </asp:DropDownList>
                                        <asp:TextBox ID="comp_name" runat="server" Width="100%" Rows="1" Height="12px" TextMode="MultiLine"
                                            ToolTip="Company Name" ValidationGroup="String"></asp:TextBox><br class="clear" />
                                        <asp:CheckBox runat="server" ID="comp_active_flag" ToolTip="Exclude Inactive Companies"
                                            Font-Size="10px" Text="Exclude Companies no longer active?" />
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                        Agency Type:
                    <asp:DropDownList ID="COMPARE_comp_agency_type" runat="server" CssClass="display_none">
                        <asp:ListItem Value="Equals">Equals</asp:ListItem>
                    </asp:DropDownList>
                                        <asp:DropDownList ID="comp_agency_type" runat="server" Width="100%" ToolTip="Agency Type"
                                            ValidationGroup="String">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                            <asp:ListItem Value="C">Civilian</asp:ListItem>
                                            <asp:ListItem Value="G">Government</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="170" RowSpan="2">
                                        Relationships to Yacht:<br class="clear" />
                                        <asp:CheckBox ID="comp_not_in_selected" Text="Not in Selected Relationship" runat="server"
                                            Font-Size="10px" /><br class="clear" />
                                        <asp:ListBox ID="yr_contact_type" runat="server" Width="100%" SelectionMode="Multiple"
                                            ToolTip="Relationships to Yacht" ValidationGroup="String">
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" BackColor="#DAE1E8" RowSpan="2">
                                        Contact First Name:
                    <asp:DropDownList ID="COMPARE_contact_first_name" runat="server" CssClass="display_none">
                        <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                    </asp:DropDownList>
                                        <asp:TextBox ID="contact_first_name" runat="server" Width="100%" ToolTip="Contact First Name"
                                            Rows="1" Height="12px" TextMode="MultiLine" ValidationGroup="String"></asp:TextBox><br />
                                        Contact Last Name:
                    <asp:DropDownList ID="COMPARE_contact_last_name" runat="server" CssClass="display_none">
                        <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                    </asp:DropDownList>
                                        <asp:TextBox ID="contact_last_name" runat="server" Width="100%" ToolTip="Contact Last Name"
                                            Rows="1" Height="12px" TextMode="MultiLine" ValidationGroup="String"></asp:TextBox>
                                        Email Address:
                    <asp:DropDownList ID="COMPARE_comp_email_address" runat="server" CssClass="display_none">
                        <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                    </asp:DropDownList>
                                        <asp:TextBox ID="comp_email_address" runat="server" Width="100%" ToolTip="Email Address"
                                            Rows="1" Height="12px" TextMode="MultiLine" ValidationGroup="String"></asp:TextBox><br />
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" BackColor="#DAE1E8" RowSpan="2">
                                        Contact Title Group:
                    <asp:DropDownList ID="COMPARE_contact_title" runat="server" CssClass="display_none">
                        <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                    </asp:DropDownList>
                                        <asp:ListBox ID="contact_title" runat="server" Width="100%" SelectionMode="Multiple"
                                            ToolTip="Contact Title" ValidationGroup="String">
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:ListBox>
                                        Phone Number:
                    <asp:DropDownList ID="COMPARE_comp_phone_office" runat="server" CssClass="display_none">
                        <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                    </asp:DropDownList>
                                        <asp:TextBox ID="comp_phone_office" runat="server" Width="100%" ToolTip="Phone" ValidationGroup="String"
                                            Rows="1" Height="12px" TextMode="MultiLine"></asp:TextBox><br />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="top" width="48">Address:
                                                </td>
                                                <td align="left" valign="top" colspan="2">
                                                    <asp:DropDownList ID="COMPARE_comp_address1" runat="server" CssClass="display_none">
                                                        <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="comp_address1" runat="server" Width="90%" ToolTip="Company Address"
                                                        ValidationGroup="String"></asp:TextBox>
                                                </td>
                                                <td align="left" valign="top" width="79">Business Type:
                                                </td>
                                                <td align="left" valign="top" width="160" rowspan="2">
                                                    <asp:ListBox ID="cref_business_type" runat="server" Width="100%" Rows="4" ToolTip="Business Type"
                                                        ValidationGroup="String" SelectionMode="Multiple">
                                                        <asp:ListItem>All</asp:ListItem>
                                                    </asp:ListBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">City:
                                                </td>
                                                <td align="left" valign="top" width="80">
                                                    <asp:DropDownList ID="COMPARE_comp_city" runat="server" CssClass="display_none">
                                                        <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="comp_city" runat="server" Width="100%" ToolTip="Company City" ValidationGroup="String"
                                                        Rows="1" Height="12px" TextMode="MultiLine"></asp:TextBox><br />
                                                </td>
                                                <td align="center" valign="top">Postal Code:
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:DropDownList ID="COMPARE_comp_zip_code" runat="server" CssClass="display_none">
                                                        <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="comp_zip_code" runat="server" Width="90%" ToolTip="Company Zip Code"
                                                        Rows="1" Height="12px" TextMode="MultiLine" ValidationGroup="String"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="2" ColumnSpan="3">
                                        <asp:Panel ID="Panel1" Width="624px" runat="server" CssClass="region_panel">
                                            <evo:viewCCSTDropDowns ID="viewCCSTDropDowns" runat="server" />
                                        </asp:Panel>

                                        <script language="javascript" type="text/javascript">
                                            checkRadioButtons(bIsBaseCompany, bIsViewCompany, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);
                                        </script>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" BackColor="#DAE1E8">
                                                              
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" BackColor="#DAE1E8">
                                                             
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="general_tab" HeaderText="General" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="0" class="data_aircraft_grid">
                                <tr>
                                    <td valign="top" align="left" width="160px">
                                        <span class="extra">Ownership:</span><br />
                                        <asp:ListBox ID="yt_ownership_type" runat="server" Width="100%" Rows="4" Font-Size="10px"
                                            SelectionMode="Multiple">
                                            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                            <asp:ListItem Value="W">Wholly Owned</asp:ListItem>
                                            <asp:ListItem Value="S">Shared</asp:ListItem>
                                            <asp:ListItem Value="F">Fractional</asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                    <td valign="top" align="left" width="307px">
                                        <span class="extra">Manufacturer/Brand:</span><br />
                                        <asp:ListBox ID="ym_mfr_comp_id" runat="server" Rows="7" Width="100%" Font-Size="10px"
                                            AutoPostBack="false" SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                    <td valign="top" align="left" class="display_none">
                                        <span class="extra">Brand:</span><br />
                                        <asp:ListBox ID="brand_listbox" runat="server" Rows="7" Font-Size="10px" SelectionMode="Multiple"
                                            Width="100%"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="location_tab" HeaderText="Location" runat="server">
                        <ContentTemplate>
                            <asp:Table ID="Table8" runat="server" Width="100%" CellPadding="5" CellSpacing="0"
                                CssClass="data_aircraft_grid">
                                <asp:TableRow CssClass="header_row">
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" ColumnSpan="7" CssClass="data_aircraft_grid_cell light_seafoam_green_header_color"><b>HULL</b></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="113px">
                          Port:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="130px">
                                        <asp:ListBox runat="server" ID="port" Width="100%">
                                            <asp:ListItem>Registered</asp:ListItem>
                                            <asp:ListItem>Home</asp:ListItem>
                                            <asp:ListItem>Lying</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="113px">
                          Country:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="130px">
                                        <asp:ListBox runat="server" ID="ListBox3" Width="100%">
                                            <asp:ListItem>Please select one</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="113px">Region:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="130px">
                                        <asp:ListBox runat="server" ID="ListBox4" Width="100%">
                                            <asp:ListItem>Please select one</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="3">
                 
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="Table9" runat="server" Width="100%" CellPadding="5" CellSpacing="0"
                                CssClass="data_aircraft_grid">
                                <asp:TableRow CssClass="header_row">
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%"><b>FIELD</b></asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="15%"><b>CONDITION</b></asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="15%"><b>VALUE</b></asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="38%"><b>FORMAT</b></asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="12%"></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">Home Port ID</asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:DropDownList ID="COMPARE_yt_home_port_id" runat="server" Width="100%" CssClass="margin_right"
                                            TabIndex="14" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'home_port', 'input');">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Equals</asp:ListItem>
                                            <asp:ListItem>Less Than</asp:ListItem>
                                            <asp:ListItem>Greater Than</asp:ListItem>
                                            <asp:ListItem>Between</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:TextBox ID="yt_home_port_id" runat="server" Width="100%" TabIndex="15" ValidationGroup="Numeric"
                                            ToolTip="Home Port ID"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">"nnnn", for Between use "nnnn: nnnn"</asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left"><asp:RegularExpressionValidator runat="server" ControlToValidate="yt_home_port_id"  ValidationExpression="^[\d,:\s\n]+$" Text="*Incorrect Format" ErrorMessage="*Incorrect Format"  Font-Bold="true"></asp:RegularExpressionValidator></asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">Lying Port ID</asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:DropDownList ID="COMPARE_yt_lying_port_id" runat="server" Width="100%" CssClass="margin_right"
                                            TabIndex="14" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'yt_lying_port_id', 'input');">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Equals</asp:ListItem>
                                            <asp:ListItem>Less Than</asp:ListItem>
                                            <asp:ListItem>Greater Than</asp:ListItem>
                                            <asp:ListItem>Between</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:TextBox ID="yt_lying_port_id" runat="server" Width="100%" TabIndex="15" ValidationGroup="Numeric"
                                            ToolTip="Lying Port ID"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">"nnnn", for Between use "nnnn: nnnn"</asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="yt_lying_port_id"
                                            ValidationExpression="^[\d,:\s\n]+$" Text="*Incorrect Format" ErrorMessage="*Incorrect Format"
                                            Font-Bold="true"></asp:RegularExpressionValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">Registered Port ID</asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:DropDownList ID="COMPARE_yt_port_registered_id" runat="server" Width="100%"
                                            CssClass="margin_right" TabIndex="14" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'registered_port', 'input');">
                                            <asp:ListItem></asp:ListItem>
                                            <asp:ListItem>Equals</asp:ListItem>
                                            <asp:ListItem>Less Than</asp:ListItem>
                                            <asp:ListItem>Greater Than</asp:ListItem>
                                            <asp:ListItem>Between</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:TextBox ID="yt_port_registered_id" runat="server" Width="100%" TabIndex="15"
                                            ValidationGroup="Numeric" ToolTip="Registered Port ID"></asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">"nnnn", for Between use "nnnn: nnnn"</asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="yt_port_registered_id"
                                            ValidationExpression="^[\d,:\s\n]+$" Text="*Incorrect Format" ErrorMessage="*Incorrect Format"
                                            Font-Bold="true"></asp:RegularExpressionValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="hull_tab" HeaderText="Hull/Dimensions" runat="server">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="0" class="data_aircraft_grid">
                                <tr class="header_row">
                                    <td valign="top" align="left" colspan="7" class="data_aircraft_grid_cell light_seafoam_green_header_color">
                                        <b>HULL</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" align="top" width="113">Hull # From/To:
                                    </td>
                                    <td align="left" valign="top" width="130px">
                                        <asp:TextBox ID="hull_MFR_from" runat="server">
                                        </asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" width="2">/
                                    </td>
                                    <td align="left" valign="top" width="130px">
                                        <asp:TextBox ID="hull_MFR_to" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="left" valign="top" colspan="3">
                                        <asp:CheckBox runat="server" ID="search_alt_hull" Text="Search Alt Hull #" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="maintenance" runat="server" HeaderText="Maintenance" Visible="true"
                        Style="display: none; visibility: hidden;">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="0" class="data_aircraft_grid">
                                <tr>
                                    <td align="left" valign="top" class="data_aircraft_grid_cell light_seafoam_green_header_color"
                                        colspan="3">
                                        <b>Compliance</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" width="20%" class="alt_row">Compliance Type:
                                    </td>
                                    <td align="left" valign="top" class="alt_row" width="15%">
                                        <span class="lighter_gray_text">Equals</span>
                                    </td>
                                    <td align="left" valign="top" class="alt_row">
                                        <asp:DropDownList ID="COMPARE_yt_compliance_type" runat="server" CssClass="display_none">
                                            <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList runat="server" ID="yt_compliance_type" Width="180px" ToolTip="Compliance Type"
                                            ValidationGroup="String">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel runat="server" ID="maintenance_dynamic_panel">
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="power" runat="server" HeaderText="Power" Visible="true" Style="display: none; visibility: hidden;">
                        <ContentTemplate>
                            <table width="100%" cellpadding="5" cellspacing="0" class="data_aircraft_grid">
                                <tr>
                                    <td align="left" valign="top" class="data_aircraft_grid_cell light_seafoam_green_header_color"
                                        colspan="4">
                                        <b>Engine Model</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top" width="20%" class="alt_row">Manufacturer:
                                    </td>
                                    <td align="left" valign="top" class="alt_row" width="30%">
                                        <asp:ListBox runat="server" ID="yt_engine_manufacturer" AutoPostBack="true" Width="100%"
                                            SelectionMode="Multiple"></asp:ListBox>
                                    </td>
                                    <td align="left" valign="top" width="20%" class="alt_row">Model:
                                    </td>
                                    <td align="left" valign="top" class="alt_row" width="30%">
                                        <asp:ListBox runat="server" ID="yt_engine_model" Width="100%" SelectionMode="Multiple">
                                            <asp:ListItem>Please Select a Manufacturer</asp:ListItem>
                                        </asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel runat="server" ID="power_dynamic_panel">
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="AttrTab" runat="server" HeaderText="Attributes" Visible="false"
                        Style="display: none; visibility: hidden;">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="AttributesPanel">
                            </asp:Panel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="charter_tab" runat="server" HeaderText="Charter" Visible="false">
                        <ContentTemplate>
                            <asp:Table ID="Table6" runat="server" Width="100%" CellPadding="5" CellSpacing="0"
                                CssClass="data_aircraft_grid">
                                <asp:TableRow VerticalAlign="Top">
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="160px">
                                        <span class="extra">Charter Availability/Season:</span><br />
                                        <asp:ListBox ID="yt_charter_availability" runat="server" Width="100%" Rows="4" Font-Size="10px"
                                            SelectionMode="Multiple">
                                            <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                            <asp:ListItem Value="Winter">Winter Season</asp:ListItem>
                                            <asp:ListItem Value="Summer">Summer Season</asp:ListItem>
                                        </asp:ListBox>
                                        <br />
                                        <span class="extra">Charter Duration/Timeframe:</span><br />
                                        <asp:DropDownList ID="COMPARE_yt_charter_duration" runat="server" CssClass="display_none">
                                            <asp:ListItem Value="Includes">Includes</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="yt_charter_duration" runat="server" Width="100%">
                                            <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                            <asp:ListItem Value="Week">Weekly</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="307px">
                                        <span class="extra">Charter Location:</span><br />
                                        <asp:DropDownList ID="COMPARE_yt_confidential_notes" runat="server" CssClass="display_none">
                                            <asp:ListItem Value="Includes">Includes</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:ListBox ID="yt_confidential_notes" runat="server" Rows="7" Width="100%" Font-Size="10px"
                                            ValidationGroup="String" SelectionMode="Multiple">
                                            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="Table7" runat="server" Width="100%" CellPadding="5" CellSpacing="0">
                                <asp:TableRow VerticalAlign="Top" CssClass="header_row" Visible="false">
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="20%"><b>Field</b>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="15%"><b>Condition</b>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="15%"><b>Value</b>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="38%">
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="12%">
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" ColumnSpan="5"><a href="#" onclick="javascript:$find('<%= yacht_advanced_search.ClientID %>').set_activeTabIndex(4);">To View other Charter Fields, Click here</a>
                  &nbsp;&nbsp;&nbsp;&nbsp;<a href="#" onclick="javascript:$find('<%= yacht_advanced_search.ClientID %>').set_activeTabIndex(11);">To View other Charter Attribute Fields, Click here</a>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </asp:Panel>
        </asp:Panel>
        <asp:Label runat="server" ID="FolderInformation" Visible="false" CssClass="FolderNameBar help_cursor"></asp:Label>
        <asp:DataList ID="YachtDataList" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
            AutoGenerateColumns="False" GridLines="Both" BorderColor="#eeeeee" AllowPaging="true"
            CssClass="mGrid">
            <ItemStyle VerticalAlign="Top" Width="33%" />
            <ItemTemplate>
                <div class="boxed_item_padding">
                    <%'#image_yes_no(DataBinder.Eval(Container.DataItem, "ytpic_seq_no"),DataBinder.Eval(Container.DataItem, "yt_id"))%>
                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ytpic_id")), "<img src='" + Me.imgDisplayFolder.Text + "/" + DataBinder.Eval(Container.DataItem, "yt_id") & "-0-" & DataBinder.Eval(Container.DataItem, "ytpic_id") & ".jpg' alt='Yacht Picture' width='50%' class='border float_left mainImage'/>", "<img src='images/yacht_no_image.jpg' width='50%' class='border float_left mainImage' />")%>
                    <div class="float_right dataListSeperator">
                        <h1 class="dataListYacht">
                            <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid= <%#DataBinder.Eval(Container.DataItem, "yt_id")%>&jid=<%#DataBinder.Eval(Container.DataItem, "yt_journ_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                                <%#DataBinder.Eval(Container.DataItem, "ym_brand_name")%>
                                <%#DataBinder.Eval(Container.DataItem, "ym_model_name")%><br />
                                <%#DataBinder.Eval(Container.DataItem, "yt_yacht_name")%>
                            </a>
                        </h1>
                        <span class="li"><span class="label">Year:</span>
                            <%#DataBinder.Eval(Container.DataItem, "yt_year_mfr")%></span>
                        <%#crmWebClient.clsGeneral.clsGeneral.DisplayStatusListingDateEvoYachtListing(DataBinder.Eval(Container.DataItem, "yt_forsale_flag"), DataBinder.Eval(Container.DataItem, "yt_forsale_status"), DataBinder.Eval(Container.DataItem, "yt_asking_price"), DataBinder.Eval(Container.DataItem, "yt_forsale_list_date"), True, DataBinder.Eval(Container.DataItem, "yt_for_lease_flag"), DataBinder.Eval(Container.DataItem, "yt_for_charter_flag"), DataBinder.Eval(Container.DataItem, "yt_asking_price_wordage"), DataBinder.Eval(Container.DataItem, "yt_id"))%>
                        <span class="li"><span class="label">Length:</span>
                            <%#iif(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), DataBinder.Eval(Container.DataItem, "yt_length_overall_meters").ToString & " (m)","")%>
                            <%#iif(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), " / " &  FormatNumber(crmwebclient.conversionfunctions.ConvertMeterToFeet(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), 2, TriState.UseDefault, TriState.UseDefault, TriState.False) & " (f)","")%></span>
                        <span class="li"><span class="label">Hull #:</span>
                            <%#DataBinder.Eval(Container.DataItem, "yt_hull_mfr_nbr")%></span> <span class="li">
                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yl_lifecycle_name")), DataBinder.Eval(Container.DataItem, "yl_lifecycle_name") & ",", "")%>
                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yls_lifecycle_status")), DataBinder.Eval(Container.DataItem, "yls_lifecycle_status"), "")%></span>
                        <asp:Label ID="Label1" runat="server" Text='<%#(crmWebClient.CompanyFunctions.FindEvolutionACCompanies(Master.aclsData_Temp, DataBinder.Eval(Container.DataItem, "yt_id")))%>'></asp:Label>
                        <div class="ac_action_bar">
                            <%#IIf(Session.Item("localSubscription").crmCloudNotes_Flag = True, crmWebClient.DisplayFunctions.BuildNote(DataBinder.Eval(Container.DataItem, "yt_id"), Master.aclsData_Temp, "YACHT"), "")%>
                            <span class="float_right"><a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid= <%#DataBinder.Eval(Container.DataItem, "yt_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">Full Details</a></span><div class="float_right">
                                <!--<ul class="cssMenu"> 
                    <li><a href="#" class="expand_more">More</a>
                      <ul>
                        <li><a href="#"><a href="#">About This Model</a></li>
                      </ul>
                    </li> 
                  </ul>
                   -->
                            </div>
                            <br class="div_clear" />
                        </div>
                    </div>
                    <%#crmWebClient.YachtFunctions.DisplayYachtConfidentialNotes(DataBinder.Eval(Container.DataItem, "yt_for_charter_flag"), DataBinder.Eval(Container.DataItem, "yt_id"), "float_left yacht_spot_charter_notes light_blue_background", False, DataBinder.Eval(Container.DataItem, "yt_forsale_flag"))%>
                </div>
            </ItemTemplate>
        </asp:DataList>
        <asp:DataGrid runat="server" ID="YachtDataGrid" AutoGenerateColumns="false" Width="100%"
            Visible="false" AllowCustomPaging="false" AllowPaging="true">
            <Columns>
                <asp:TemplateColumn HeaderText="" ItemStyle-Width="15">
                    <ItemTemplate>
                        <%#If(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ytpic_id")), "<img src='images/camera.png' alt='Yacht Pic' />", "")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="BRAND/MODEL" ItemStyle-Width="130">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "ym_brand_name")%>
            /
            <%#DataBinder.Eval(Container.DataItem, "ym_model_name")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="NAME" ItemStyle-Width="100">
                    <ItemTemplate>
                        <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid=<%#DataBinder.Eval(Container.DataItem, "yt_id")%>&jid=<%#DataBinder.Eval(Container.DataItem, "yt_journ_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                            <%#DataBinder.Eval(Container.DataItem, "yt_yacht_name")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="HULL #" ItemStyle-Width="67" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid=<%#DataBinder.Eval(Container.DataItem, "yt_id")%>&jid=<%#DataBinder.Eval(Container.DataItem, "yt_journ_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                            <%#DataBinder.Eval(Container.DataItem, "yt_hull_mfr_nbr")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="MFR" ItemStyle-Width="35" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "yt_year_mfr")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="STATUS" ItemStyle-Width="98">
                    <ItemTemplate>
                        <%#crmWebClient.clsGeneral.clsGeneral.DisplayStatusListingDateEvoYachtListing(DataBinder.Eval(Container.DataItem, "yt_forsale_flag"), DataBinder.Eval(Container.DataItem, "yt_forsale_status"), DataBinder.Eval(Container.DataItem, "yt_asking_price"), DataBinder.Eval(Container.DataItem, "yt_forsale_list_date"), False, DataBinder.Eval(Container.DataItem, "yt_for_lease_flag"), DataBinder.Eval(Container.DataItem, "yt_for_charter_flag"), DataBinder.Eval(Container.DataItem, "yt_asking_price_wordage"), DataBinder.Eval(Container.DataItem, "yt_id"))%>
                        <span class="li">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yl_lifecycle_name")), DataBinder.Eval(Container.DataItem, "yl_lifecycle_name") & ",", "")%>
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yls_lifecycle_status")), DataBinder.Eval(Container.DataItem, "yls_lifecycle_status"), "")%></span>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="COMPANIES" ItemStyle-Width="200">
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%#(crmWebClient.CompanyFunctions.FindEvolutionACCompanies(Master.aclsData_Temp, DataBinder.Eval(Container.DataItem, "yt_id")))%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="LENGTH" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%#iif(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), DataBinder.Eval(Container.DataItem, "yt_length_overall_meters").ToString & " (m)","")%>
                        <%#iif(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), "<br />" &  FormatNumber(crmwebclient.conversionfunctions.ConvertMeterToFeet(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), 2, TriState.UseDefault, TriState.UseDefault, TriState.False) & " (f)","")%></span>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="" Visible="false" ItemStyle-Width="15">
                    <ItemTemplate>
                        <%#IIf(Session.Item("localSubscription").crmCloudNotes_Flag = True, crmWebClient.DisplayFunctions.BuildNote(DataBinder.Eval(Container.DataItem, "yt_id"), Master.aclsData_Temp, "YACHT"), "")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:DataList ID="HistoryDataList" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
            AutoGenerateColumns="False" GridLines="Both" BorderColor="#eeeeee" AllowPaging="true"
            CssClass="mGrid">
            <ItemStyle VerticalAlign="Top" Width="33%" />
            <ItemTemplate>
                <table width="100%" cellpadding="3" cellspacing="0" class="boxed_item_padding">
                    <tr>
                        <td align="left" valign="top" class="dataListSeperator no_bottom_border">
                            <h1 class="dataListYacht">
                                <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid= <%#DataBinder.Eval(Container.DataItem, "yt_id")%>&jid=<%#DataBinder.Eval(Container.DataItem, "yt_journ_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                                    <%#DataBinder.Eval(Container.DataItem, "ym_brand_name")%>
                                    <%#DataBinder.Eval(Container.DataItem, "ym_model_name")%><br />
                                    <%#DataBinder.Eval(Container.DataItem, "yt_yacht_name")%>
                                </a>
                            </h1>
                            <span class="li"><span class="label">Year:</span>
                                <%#DataBinder.Eval(Container.DataItem, "yt_year_mfr")%></span>
                            <%#crmWebClient.clsGeneral.clsGeneral.DisplayStatusListingDateEvoYachtListing(DataBinder.Eval(Container.DataItem, "yt_forsale_flag"), DataBinder.Eval(Container.DataItem, "yt_forsale_status"), DataBinder.Eval(Container.DataItem, "yt_asking_price"), DataBinder.Eval(Container.DataItem, "yt_forsale_list_date"), True, DataBinder.Eval(Container.DataItem, "yt_for_lease_flag"), DataBinder.Eval(Container.DataItem, "yt_for_charter_flag"), DataBinder.Eval(Container.DataItem, "yt_asking_price_wordage"), 0)%>
                            <span class="li"><span class="label">Length:</span>
                                <%#iif(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), DataBinder.Eval(Container.DataItem, "yt_length_overall_meters").ToString & " (m)","")%>
                                <%#iif(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), " / " &  FormatNumber(crmwebclient.conversionfunctions.ConvertMeterToFeet(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), 2, TriState.UseDefault, TriState.UseDefault, TriState.False) & " (f)","")%></span>
                            <span class="li"><span class="label">Hull #:</span>
                                <%#DataBinder.Eval(Container.DataItem, "yt_hull_mfr_nbr")%></span>
                            <div class="ac_action_bar">
                                <%#IIf(Session.Item("localSubscription").crmCloudNotes_Flag = True, crmWebClient.DisplayFunctions.BuildNote(DataBinder.Eval(Container.DataItem, "yt_id"), Master.aclsData_Temp, "YACHT"), "")%>
                                <span class="float_right"><a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid= <%#DataBinder.Eval(Container.DataItem, "yt_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">Full Details</a></span><div class="float_right">
                                    <!--
                    <ul class="cssMenu">
                      <li><a href="#" class="expand_more">More</a>
                        <ul>
                          <li><a href="#"><a href="#">About This Model</a></li>
                        </ul>
                      </li>
                    </ul>
                    -->
                                </div>
                                <br class="div_clear" />
                            </div>
                        </td>
                        <td align="left" valign="top" class="dataListSeperatorHistory no_bottom_border">
                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_date")), "<span class=""li""><span class=""label"">Transaction Date:</span> " & crmWebClient.clsGeneral.clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "journ_date")) & "</span>", "")%>
                            <span class="li">
                                <%#DataBinder.Eval(Container.DataItem, "journ_subject").ToString%></span>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
        <asp:DataGrid runat="server" ID="HistoryDataGrid" AutoGenerateColumns="false" Width="100%"
            Visible="false" AllowCustomPaging="false" AllowPaging="true">
            <Columns>
                <asp:TemplateColumn HeaderText="BRAND/MODEL" ItemStyle-Width="130">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "ym_brand_name")%>
            /
            <%#DataBinder.Eval(Container.DataItem, "ym_model_name")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="NAME" ItemStyle-Width="100">
                    <ItemTemplate>
                        <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid=<%#DataBinder.Eval(Container.DataItem, "yt_id")%>&jid=<%#DataBinder.Eval(Container.DataItem, "yt_journ_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                            <%#DataBinder.Eval(Container.DataItem, "yt_yacht_name")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="HULL #" ItemStyle-Width="67" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid=<%#DataBinder.Eval(Container.DataItem, "yt_id")%>&jid=<%#DataBinder.Eval(Container.DataItem, "yt_journ_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                            <%#DataBinder.Eval(Container.DataItem, "yt_hull_mfr_nbr")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="MFR" ItemStyle-Width="35" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "yt_year_mfr")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="TRANS DATE" ItemStyle-Width="70">
                    <ItemTemplate>
                        <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_date")), crmWebClient.clsGeneral.clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "journ_date")), "")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DESCRIPTION" ItemStyle-Width="400">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "journ_subject").ToString%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="LENGTH" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%#iif(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), DataBinder.Eval(Container.DataItem, "yt_length_overall_meters").ToString & " (m)","")%>
                        <%#iif(Not IsDBNull(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), "<br />" &  FormatNumber(crmwebclient.conversionfunctions.ConvertMeterToFeet(DataBinder.Eval(Container.DataItem, "yt_length_overall_meters")), 2, TriState.UseDefault, TriState.UseDefault, TriState.False) & " (f)","")%></span>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="" Visible="false" ItemStyle-Width="15">
                    <ItemTemplate>
                        <%#IIf(Session.Item("localSubscription").crmCloudNotes_Flag = True, crmWebClient.DisplayFunctions.BuildNote(DataBinder.Eval(Container.DataItem, "yt_id"), Master.aclsData_Temp, "YACHT"), "")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:DataGrid runat="server" ID="YachtEventsDataGrid" AutoGenerateColumns="false"
            Width="100%" Visible="false" AllowCustomPaging="false" AllowPaging="true">
            <Columns>
                <asp:TemplateColumn HeaderText="BRAND/MODEL" ItemStyle-Width="130">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "ym_brand_name")%>
            /
            <%#DataBinder.Eval(Container.DataItem, "ym_model_name")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="NAME" ItemStyle-Width="100">
                    <ItemTemplate>
                        <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid=<%#DataBinder.Eval(Container.DataItem, "yt_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                            <%#DataBinder.Eval(Container.DataItem, "yt_yacht_name")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="HULL #" ItemStyle-Width="67" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <a href="#" onclick="javascript:load('DisplayYachtDetail.aspx?yid=<%#DataBinder.Eval(Container.DataItem, "yt_id")%>','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');return false;">
                            <%#DataBinder.Eval(Container.DataItem, "yt_hull_mfr_nbr")%>
                        </a>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="MFR" ItemStyle-Width="35" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "yt_year_mfr")%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="ACTIVITY<br /><em>Date/Time</em>" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "apev_entry_date").ToString%>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="DESCRIPTION">
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container.DataItem, "apev_subject").ToString%>
                        <%#crmWebClient.DisplayFunctions.LinkOutEventsCompanies(DataBinder.Eval(Container.DataItem, "apev_description"), DataBinder.Eval(Container.DataItem, "ype_comp_id"), DataBinder.Eval(Container.DataItem, "ype_contact_id"), Master)%>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
        <asp:Panel runat="server" ID="bottom_yacht_search">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left" valign="top" class="dark_header" width="100%">
                        <asp:Table ID="bottom_next_prev" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
                            CssClass="padding_table">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="180" ID="results_text_2">
                                    <asp:Label ID="paging2" runat="server" CssClass="criteria_text criteria_spacer">
                                        <asp:ImageButton ID="previous_all2" ImageUrl="../images/previous_all.png" runat="server"
                                            Visible="false" />&nbsp;<asp:ImageButton ID="previous2" ImageUrl="../images/previous_listing.png"
                                                Visible="false" runat="server" />&nbsp;<asp:Label ID="record_count2" runat="server">Showing 25 - 50</asp:Label>&nbsp;<asp:ImageButton
                                                    ID="next_2" ImageUrl="../images/next_listing.png" runat="server" />&nbsp;<asp:ImageButton
                                                        ID="next_all2" ImageUrl="~/images/next_all.png" runat="server" /></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <asp:Label ID="yacht_attention" runat="server" Text="" CssClass="red_text emphasis_text text_align_center small_to_medium_text"></asp:Label>
    </div>

    <script type="text/javascript">
        //Javascript to scroll to first selected item in each multi select list.
        $('select').each(function () {
            $(this).prop('selected', 'selected');
        });

        //Automatically submit on enter press
        $(function () {
            $('textarea').on('keyup', function (e) {
                if (e.keyCode == 13) {
                    $("#<%= search.clientID %>").click();
                }
            });
        });


        function SubMenuYachtDrop(x, reportID, folder_type) {


            my_form = document.createElement('FORM');
            my_form.method = 'POST';
            my_form.target = "_blank"
            // alert(folder_type);

            switch (x) {
                case 4:
                    //Map Form
                    my_form.name = 'mappingForm';
                    my_form.action = 'MapItems.aspx';
                    document.body.appendChild(my_form);
                    my_form.submit();
                    break;
                case 2:
                    //Summary popup
                    if (folder_type == 'COMPANY') {
                        window.location = 'SearchSummary.aspx?sub_type=C'; //redirects to homepage
                    } else {
                        my_form.name = 'exportForm';
                        my_form.action = 'evo_exporter.aspx';
                        my_tb = document.createElement('INPUT');
                        my_tb.type = 'HIDDEN';
                        my_tb.name = 'type';
                        my_tb.value = "summary";
                        my_form.appendChild(my_tb);

                        //Appending the type of folder, either Aircraft or History.
                        my_tb = document.createElement('INPUT');
                        my_tb.type = 'HIDDEN';
                        my_tb.name = "export_type";
                        my_tb.value = folder_type//.innerHTML;
                        my_form.appendChild(my_tb);
                        document.body.appendChild(my_form);
                        my_form.submit();
                    }
                    break;
                case 5:
                    load("PDF_Creator.aspx?export_type=" + folder_type, "", "scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no");
                    break;
                case 6:
                    load("STAR_ToFromReport.aspx?starReport=" + reportID + "&marketSelection=" + folder_type, "", "scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no");
                    break;
                case 3:
                    //folders maintenance popup  

                    my_form.action = 'FolderMaintenance.aspx';
                    my_form.name = 'folderForm';

                    //Appending the type of folder, either Aircraft or History.
                    my_tb = document.createElement('INPUT');
                    my_tb.type = 'HIDDEN';
                    my_tb.name = "TYPE_OF_FOLDER";
                    my_tb.value = folder_type//.innerHTML;
                    //alert(folder_type);
                    my_form.appendChild(my_tb);

                    //this parameter means that this is an update instead of insert.
                    if (reportID != 0) {
                        my_tb = document.createElement('INPUT');
                        my_tb.type = 'HIDDEN';
                        my_tb.name = "REPORT_ID";
                        my_tb.value = reportID;
                        my_form.appendChild(my_tb);
                    }

                    var str = '';
                    var elem = document.getElementById('aspnetForm').elements;
                    for (var i = 0; i < elem.length; i++) {
                        if (elem[i].type != 'hidden' && elem[i].type != 'submit') {
                            if (elem[i].value != '') {
                                if (elem[i].className == 'display_none' || elem[i].className == 'display_none includes') {
                                    //ignore this because if it has these classes, then they only have one option that's defaulted
                                    //so we don't need to save these. Though there's no reason you can't,
                                    //This only clears up the saved row in the database some.
                                    //alert(elem[i].className);
                                } else { //submit this.
                                    var appendMyField = true;
                                    var re = new RegExp("ctl[A-Za-z0-9]*_ContentPlaceHolder[A-Za-z0-9]_", "g");
                                    var re2 = new RegExp("Criteria_Bar[A-Za-z0-9]*_", "g");

                                    var re3 = new RegExp("yacht_advanced_search_AttrTab_", "g");
                                    var re4 = new RegExp("yacht_advanced_search_general_tab_", "g");
                                    var re5 = new RegExp("yacht_advanced_search_company_contact_tab_", "g");
                                    var re6 = new RegExp("yacht_advanced_search_maintenance_", "g");
                                    var re7 = new RegExp("yacht_advanced_search_TAB[A-Za-z0-9]*_", "g");
                                    var re8 = new RegExp("yacht_advanced_search_hull_tab_", "g");
                                    var re9 = new RegExp("yacht_advanced_search_location_tab_", "g");
                                    var re10 = new RegExp("yacht_advanced_search_charter_tab_", "g");

                                    var rep = elem[i].id;
                                    var temp = rep.replace(re, "");
                                    temp = temp.replace(re2, "");
                                    temp = temp.replace(re3, "");
                                    temp = temp.replace(re4, "");
                                    temp = temp.replace(re5, "");
                                    temp = temp.replace(re6, "");
                                    temp = temp.replace(re7, "");
                                    temp = temp.replace(re8, "");
                                    temp = temp.replace(re9, "");
                                    temp = temp.replace(re10, "");

                                    my_tb = document.createElement('INPUT');
                                    my_tb.type = 'HIDDEN';
                                    my_tb.name = temp;

                                    //If it has a checked value that's not undefined, go ahead and 
                                    //Pass that, if not, pass the value

                                    if (elem[i].type == 'checkbox') {
                                        if (elem[i].id.indexOf("yacht_advanced_search_AttrTab_") >= 0) {
                                            if (elem[i].checked == true) { //If the actual attribute isn't checked, there is 
                                                //No real reason to store it. The default is unchecked so it would basically be storing the default setting.
                                                my_tb.name = temp;
                                                my_tb.value = elem[i].checked;
                                                appendMyField = true;
                                            } else {
                                                appendMyField = false;
                                            }
                                        } else {
                                            my_tb.value = elem[i].checked;
                                            //alert(temp + " : " + elem[i].value);
                                        }
                                    } else if (elem[i].type == 'select-multiple') {
                                        //var opt = document.getElementById('' + elem[i].id + '').options
                                        //alert(elem[i].id);
                                        var SelBranchVal = "";
                                        var x = 0;
                                        for (x = 0; x < elem[i].length; x++) {
                                            if (elem[i][x].selected) {
                                                //Add seperator just not for 1st entry.
                                                if (SelBranchVal != "") {
                                                    SelBranchVal = SelBranchVal + "##"
                                                }
                                                SelBranchVal = SelBranchVal + elem[i][x].value;
                                            }
                                        }
                                        //alert(SelBranchVal);
                                        my_tb.value = SelBranchVal; //elem[i].value;
                                    } else if (elem[i].type == 'radio') {
                                        my_tb.value = elem[i].checked;
                                    } else {
                                        my_tb.value = elem[i].value;
                                        //alert(temp + " : " + elem[i].checked);
                                    }

                                    if (appendMyField == true) {
                                        my_form.appendChild(my_tb);
                                    }
                                }
                            }
                        }
                    }
                    document.body.appendChild(my_form);
                    my_form.submit();
                    break;
                default:
                    //Evo Exporter popup
                    my_form.name = 'exportForm';
                    my_form.action = 'evo_exporter.aspx';
                    my_tb = document.createElement('INPUT');
                    my_tb.type = 'HIDDEN';
                    my_tb.name = 'type';
                    my_tb.value = "";
                    my_form.appendChild(my_tb);

                    //Appending the type of folder, either Aircraft or History.
                    my_tb = document.createElement('INPUT');
                    my_tb.type = 'HIDDEN';
                    my_tb.name = "export_type";
                    my_tb.value = folder_type//.innerHTML;
                    my_form.appendChild(my_tb);
                    document.body.appendChild(my_form);
                    my_form.submit();
            }


        }
    </script>

</asp:Content>
