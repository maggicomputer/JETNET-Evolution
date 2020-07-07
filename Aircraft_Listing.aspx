<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Aircraft_Listing.aspx.vb"
    Inherits="crmWebClient.Aircraft_Listing" MasterPageFile="~/EvoStyles/EvoTheme.Master"
    StylesheetTheme="Evo" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/EvoStyles/EvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style>
        .ie8 .action_dropdown_container:hover {
            z-index: 40000 !important;
        }
    </style>

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
        var bShowInactiveCountriesBase = false;

        var bIsBaseCompany = false;
        var bIsViewCompany = false;
        var bShowInactiveCountriesCompany = false;

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="DataGridShadowContainer PerformanceListingTable AircraftPage">
        <asp:Panel runat="server" ID="Aircraft_Criteria" Visible="true" class="fixPosition">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left" valign="top" class="dark_header" width="100%">
                        <asp:UpdatePanel runat="server" ID="folderInformationUpdate" UpdateMode="Conditional"
                            ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="2" CellSpacing="0">
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="help_text" CssClass="evoHelp mobile_vertical_align_bottom mobile_display_off_cell">
                        <a href="help.aspx?t=2&s=1" target="_blank" class="display_none">Help</a>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="search_expand_text">
                                            <asp:Panel ID="Control_Panel" runat="server" Width="100%">
                                                <asp:Image ID="ControlImage" runat="server" ImageUrl="../Images/spacer.gif" CssClass="mobile_display_off_cell controlImage" />
                                                <a href="javascript:void(0);" id="controlLink" runat="server" class="display_none">
                                                    <i class="fa fa-chevron-left" aria-hidden="true"></i></a>
                                            </asp:Panel>
                                            <asp:Label runat="server" ID="StaticFolderNewSearchLabel"></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="results_text" Wrap="false"
                                            CssClass="mobile_padding mobileAlignRight">
                                            <asp:Label ID="criteria_results" runat="server" Text=""></asp:Label>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="center" VerticalAlign="middle" ID="sort_by_text"
                                            CssClass="mobile_display_off_cell">
                        Sort By: 
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="sort_by_dropdown"
                                            CssClass="mobile_display_block mobileAlignRight">
                                            <div class="action_dropdown_container">
                                                <asp:BulletedList ID="sort_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                                    <asp:ListItem>Ser #</asp:ListItem>
                                                </asp:BulletedList>
                                                <asp:BulletedList ID="sort_submenu_dropdown" runat="server" CssClass="ul_bottom sort_dropdown"
                                                    OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                                    <asp:ListItem>Ser #</asp:ListItem>
                                                    <asp:ListItem>Model/Ser#</asp:ListItem>
                                                    <asp:ListItem>List Date</asp:ListItem>
                                                    <asp:ListItem>AFTT</asp:ListItem>
                                                    <asp:ListItem>Status</asp:ListItem>
                                                </asp:BulletedList>
                                            </div>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="center" VerticalAlign="middle" Wrap="false" ID="per_page_text"
                                            CssClass="mobile_display_off_cell">
                        Per Page:&nbsp;
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="per_page_dropdown_"
                                            CssClass="mobile_display_off_cell">
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
                                                    <asp:ListItem Value="200">200</asp:ListItem>
                                                    <asp:ListItem Value="300">300</asp:ListItem>
                                                    <asp:ListItem Value="400">400</asp:ListItem>
                                                    <asp:ListItem Value="500">500</asp:ListItem>
                                                </asp:BulletedList>
                                            </div>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="view_dropdown_"
                                            CssClass="mobile_display_off_cell">
                                            <div class="action_dropdown_container">
                                                <asp:BulletedList ID="view_dropdown" runat="server" CssClass="ul_top thumnail_view_bullet">
                                                    <asp:ListItem></asp:ListItem>
                                                </asp:BulletedList>
                                                <asp:BulletedList ID="view_submenu_dropdown" runat="server" CssClass="ul_bottom thumbnail special"
                                                    OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                                    <asp:ListItem>Listing </asp:ListItem>
                                                    <asp:ListItem>Gallery</asp:ListItem>
                                                </asp:BulletedList>
                                            </div>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="75" ID="action_dropdown"
                                            CssClass="mobile_display_off_cell">
                                            <div class="action_dropdown_container">
                                                <asp:BulletedList ID="actions_dropdown" runat="server" CssClass="ul_top">
                                                    <asp:ListItem>Actions</asp:ListItem>
                                                </asp:BulletedList>
                                                <asp:BulletedList ID="actions_submenu_dropdown" runat="server" CssClass="ul_bottom ac_action_dropdown"
                                                    DisplayMode="HyperLink" OnClick="submenu_dropdown_Click">
                                                </asp:BulletedList>
                                            </div>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="TableCell1" CssClass="mobile_display_off_cell">
                                            <div class="action_dropdown_container">
                                                <asp:BulletedList ID="folders_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                                    <asp:ListItem>Folders</asp:ListItem>
                                                </asp:BulletedList>
                                                <asp:BulletedList ID="folders_submenu_dropdown" runat="server" CssClass="ul_bottom folder_dropdown"
                                                    DisplayMode="HyperLink">
                                                    <asp:ListItem>Project 1</asp:ListItem>
                                                    <asp:ListItem>Project 2</asp:ListItem>
                                                </asp:BulletedList>
                                            </div>
                                            <div class="fleetAnalyzerIcon">
                                                <asp:Literal runat="server" ID="fleetAnalyzerContainer" Visible="false"></asp:Literal>
                                            </div>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="65" ID="go_to_text_2">
                        Go To:&nbsp;
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="go_to_dropdown_cell_2">
                                            <div class="action_dropdown_container">
                                                <asp:BulletedList ID="go_to_dropdown_2" runat="server" CssClass="ul_top per_page_width">
                                                    <asp:ListItem>1</asp:ListItem>
                                                </asp:BulletedList>
                                                <asp:BulletedList ID="go_to_submenu_dropdown_2" runat="server" CssClass="ul_bottom per_page_dropdown"
                                                    OnClick="submenu_dropdown_Click" DisplayMode="LinkButton">
                                                </asp:BulletedList>
                                            </div>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="Center" VerticalAlign="middle" ID="TableCell2" CssClass="mobile_display_off_cell"
                                            Visible="false">
                   <a href="MarketSummary.aspx?previousSummary=true" class="underline cursor" style="color:White;">Back&nbsp;to<br />Market&nbsp;Summary</a>
                                        </asp:TableCell>
                                        <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" ID="results_text_"
                                            CssClass="mobile_display_off_cell">
                                            <asp:Label ID="paging" runat="server" CssClass="criteria_text">
                                                <asp:ImageButton ID="previous_all" ImageUrl="../images/previous_all.png" runat="server"
                                                    Visible="false" OnClick="MoveNext" OnClientClick="javascript: ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />&nbsp;<asp:ImageButton
                                                        ID="previous" ImageUrl="../images/previous_listing.png" Visible="false" runat="server"
                                                        OnClick="MoveNext" OnClientClick="javascript: ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />&nbsp;<asp:Label
                                                            ID="record_count" runat="server">Showing 25 - 50</asp:Label>&nbsp;<asp:ImageButton
                                                                ID="next_" ImageUrl="../images/next_listing.png" runat="server" OnClick="MoveNext"
                                                                OnClientClick="javascript: ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />&nbsp;<asp:ImageButton
                                                                    ID="next_all" ImageUrl="~/images/next_all.png" runat="server" OnClick="MoveNext"
                                                                    OnClientClick="javascript: ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" /></asp:Label>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
                <ProgressTemplate>
                    <span></span>
                    <div class="loader">Loading...</div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Panel ID="Collapse_Panel" runat="server" Width="100%" CssClass="collapse">
                <asp:Label runat="server" ID="close_current_folder" Font-Bold="true" ForeColor="Red"
                    Visible="false"><br /><br /><p align="center" class="medium_text">You must Close Current Folder before starting a New Search.</p><br /><br /></asp:Label>
                <asp:Table ID="Table2" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                    <asp:TableRow CssClass="mobile_display_on_cell mobileWhiteBackground modelRow" ID="mobileModelRow">
                        <asp:TableCell CssClass="mobile_display_on_cell" ID="mobileModelCell" ColumnSpan="2">
                            <asp:Panel runat="server" ID="MobileSearchVisible" Visible="false">
                                <asp:DropDownList runat="server" AutoPostBack="false" ID="makeModelDynamic" CssClass="chosen-select"
                                    Width="100%">
                                    <asp:ListItem Value="">Please pick a Model</asp:ListItem>
                                </asp:DropDownList>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="33%" HorizontalAlign="Left" VerticalAlign="Top" CssClass="model_search_box collapseSearchTable"
                            RowSpan="2">
                            <asp:Panel runat="server" ID="model_search_box">
                                <asp:Panel ID="wanted_make_model_panel" runat="server">
                                    <evo:viewTMMDropDowns ID="ViewTMMDropDowns" runat="server" />

                                    <script language="javaScript" type="text/javascript">
                    //alert("refresh type make model");
                    refreshTypeMakeModelByCheckBox("", "", <%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);
                                    </script>
                                </asp:Panel>
                                <asp:Panel runat="server" CssClass="padding cursor display_none" ID="advanced_control_panel">
                                    <asp:Image ID="advanced_image" runat="server" ImageUrl="../Images/expand.jpg" />&nbsp;<span
                                        class="text_underline">Advanced Search</span>
                                </asp:Panel>
                            </asp:Panel>
                        </asp:TableCell>
                        <asp:TableCell Width="67%" HorizontalAlign="Left" VerticalAlign="bottom" ID="tableCellToggle"
                            CssClass="collapseSearchTable mobileWhiteBackground">
                            <asp:Table ID="Table4" runat="server" Width="100%" CellPadding="3" CellSpacing="0">
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="50" ID="serText">
                           <a href="#" class="mobileSearchHeader" onclick="javascript:load('MasterLists.aspx?helplist=serial','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');">Serial #</a>: 
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="50" CssClass="mobile_display_off_cell">
                           From/To:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="210px">
                                        <asp:TextBox ID="ac_ser_no_from" runat="server" Width="90px" TabIndex="1">
                                        </asp:TextBox><span class="displayNoneMobile">/</span>
                                        <asp:TextBox ID="ac_ser_no_to" runat="server" Width="90px" TabIndex="1" CssClass="displayNoneMobile">
                                        </asp:TextBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" RowSpan="2" CssClass="light_border mobile_display_off_cell">
                                        <asp:CheckBox ID="do_not_search_ac_alt_ser_no" runat="server" Text="Don't Search Alt. Ser#"
                                            Font-Size="9px" TabIndex="2" /><br />
                                        <asp:CheckBox ID="do_not_search_ac_prev_reg_no" runat="server" Text="Don't Search Prev. Reg#"
                                            Font-Size="9px" TabIndex="3" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                             <a href="#" class="mobileSearchHeader" onclick="javascript:load('MasterLists.aspx?helplist=registration','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');">Reg #</a>:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="30px" CssClass="mobile_display_off_cell">
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="90px">
                                        <asp:TextBox ID="ac_reg_no" runat="server" Width="90px" TabIndex="4" Rows="1" Height="12px"
                                            TextMode="MultiLine">
                                        </asp:TextBox>&nbsp;
                    <asp:CheckBox ID="ac_reg_no_exact_match" runat="server" Text="Exact Match" Font-Size="9px"
                        TabIndex="5" CssClass="mobile_display_off_cell" />
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow ID="mobileSearchRadioToggle">
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" CssClass="mobileSearchHeader mobile_display_on_cell">
                            Status:
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="30px" CssClass="mobile_display_off_cell">
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" CssClass="mobile_display_on_cell">
                                        <asp:RadioButtonList runat="server" ID="mobileStatus" CssClass="mobile_display_on_cell float_left mobileRadio">
                                            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                            <asp:ListItem Value="For Sale"></asp:ListItem>
                                            <asp:ListItem Value="Not For Sale"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ID="Aircraft_Search_Box_toggle" runat="server" Width="50%" HorizontalAlign="Left"
                            VerticalAlign="top" CssClass="market_search_box">
                            <asp:TextBox ID="comp_folder_name" runat="server" CssClass="display_none"></asp:TextBox>
                            <asp:TextBox ID="static_folder" runat="server" CssClass="display_none"></asp:TextBox>
                            <asp:TextBox ID="static_folder_ac_ids" runat="server" CssClass="display_none"></asp:TextBox>
                            <asp:Table ID="transaction_box" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3" VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:CheckBox ID="transaction_retail" runat="server" Text="" AutoPostBack="true"
                                            TabIndex="6" /><asp:Label runat="server" ID="retail_transaction_label">Retail Transactions</asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="25">Type:</asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Width="150" Height="60">
                                        <asp:ListBox ID="journ_subcat_code_part1" runat="server" Width="100%" Rows="4" SelectionMode="multiple"
                                            TabIndex="7">
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="right" Width="60" Height="60">
                                        <asp:DropDownList ID="journ_subcat_code_part2_operator" runat="server" Width="60"
                                            TabIndex="8">
                                            <asp:ListItem>From</asp:ListItem>
                                            <asp:ListItem>Not From</asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left" Height="60">
                                        <asp:ListBox ID="journ_subcat_code_part2" runat="server" Width="100%" Rows="4" SelectionMode="multiple"
                                            TabIndex="9"></asp:ListBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3" VerticalAlign="top" HorizontalAlign="Left" Height="60">
                                        <asp:DropDownList ID="journ_subcat_code_part3_operator" runat="server" Width="60"
                                            CssClass="float_right" TabIndex="10">
                                            <asp:ListItem>To</asp:ListItem>
                                            <asp:ListItem>Not To</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:TextBox runat="server" ID="journ_id" Style="display: none"></asp:TextBox>
                                        <asp:CheckBox ID="journ_newac_flag" runat="server" Text="Sales of New Aircraft Only"
                                            CssClass="display_block" TabIndex="11" />
                                        <asp:CheckBox ID="jcat_used_retail_sales_flag" runat="server" Text="Sales of Used Aircraft Only"
                                            TabIndex="12" CssClass="display_block" />
                                        <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe1" runat="server" TargetControlID="journ_newac_flag"
                                            Key="YesNo" />
                                        <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe2" runat="server" TargetControlID="jcat_used_retail_sales_flag"
                                            Key="YesNo" />
                                        <asp:CheckBox runat="server" ID="journ_exclude_internal_transactions" Text="Exclude Internal Transactions" />
                                    </asp:TableCell>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="right" Width="140" Height="60">
                                        <asp:ListBox ID="journ_subcat_code_part3" runat="server" Width="100%" Rows="4" SelectionMode="multiple"
                                            TabIndex="13"></asp:ListBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell ColumnSpan="3" runat="server" VerticalAlign="Top" HorizontalAlign="Left">
                                        Transaction Date:
                    <asp:DropDownList ID="journ_date_operator" runat="server" Width="83px" CssClass="margin_right display_none"
                        TabIndex="14" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'journ_date', 'input');">
                    </asp:DropDownList>
                                        <asp:TextBox ID="journ_date" runat="server" Width="140px" TabIndex="15" CausesValidation="true"></asp:TextBox>&nbsp;<asp:Image
                                            ImageUrl="~/images/magnify_small.png" runat="server" AlternateText="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;"
                                            ToolTip="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;" />
                                        <asp:CustomValidator runat="server" ControlToValidate="journ_date" ID="VALIDATE_TransactionDate"
                                            Font-Size="9px" ErrorMessage="*Incorrect Format" Font-Bold="true" ValidationGroup="Numeric"
                                            SetFocusOnError="true" CssClass="float_right" ClientValidationFunction="validateDate"
                                            Text="*Incorrect Format" OnServerValidate="IsValidDateEntry" Display="Static" Enabled="true"
                                            EnableClientScript="true"></asp:CustomValidator>
                                    </asp:TableCell>
                                    <asp:TableCell RowSpan="2" ColumnSpan="3" ID="TableCell3" runat="server" VerticalAlign="Top"
                                        HorizontalAlign="right">
                                        <asp:UpdatePanel runat="server" ID="acHistSearchUpdate" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="transaction_search" />
                                                <asp:AsyncPostBackTrigger ControlID="Button1" />
                                            </Triggers>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Panel runat="server" DefaultButton="transaction_search" ID="searchTransaction_button">
                                            <asp:Button ID="transaction_search" runat="server" Text="Search" CssClass="button_width button-darker"
                                                UseSubmitBehavior="true" TabIndex="16" CausesValidation="true" ValidationGroup="Numeric"
                                                OnClientClick="javascript:FillStateHiddenValue(1);performCheck(); ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" /><br />
                                            <asp:Button ID="Button1" runat="server" Text="Clear Selections" OnClick="Reset_Form"
                                                CssClass="button_width font-weight-normal" TabIndex="17" CausesValidation="false"
                                                OnClientClick="javascript: ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="market_search_box" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell ID="aerodex_toggle" HorizontalAlign="left" VerticalAlign="top" Width="43%"
                                        RowSpan="2">
                                        <span class="extra">Market Status:</span>
                                        <asp:ListBox ID="market" runat="server" Width="100%" Rows="13" SelectionMode="multiple"
                                            TabIndex="6">
                                            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                            <asp:ListItem Value="For Sale">For Sale</asp:ListItem>
                                            <asp:ListItem Value="For Sale/Lease">For Sale/Lease</asp:ListItem>
                                            <asp:ListItem Value="For Sale/Trade">For Sale/Trade</asp:ListItem>
                                            <asp:ListItem Value="For Sale on Exclusive">For Sale on Exclusive</asp:ListItem>
                                            <asp:ListItem Value="For Sale Not on Exclusive">For Sale Not on Exclusive</asp:ListItem>
                                            <asp:ListItem Value="Not For Sale">Not For Sale</asp:ListItem>
                                            <asp:ListItem Value="Lease">Lease</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="23%" CssClass="displayNoneMobile">
                                        <span class="extra"><a href="#" onclick="javascript:load('MasterLists.aspx?helplist=lifecycle','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');">Lifecycle</a>:</span>
                                        <asp:ListBox ID="ac_lifecycle_stage" runat="server" Width="100%" Rows="7" TabIndex="7"
                                            SelectionMode="Multiple">
                                            <asp:ListItem Value="">All</asp:ListItem>
                                            <asp:ListItem Value="1">In Production</asp:ListItem>
                                            <asp:ListItem Value="2">New-With MFR</asp:ListItem>
                                            <asp:ListItem Value="3" Selected="True">In Operation</asp:ListItem>
                                            <asp:ListItem Value="4">Retired</asp:ListItem>
                                            <asp:ListItem Value="5">In Storage</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%" CssClass="displayNoneMobile">
                                        <span class="extra">Ownership:</span>
                                        <asp:ListBox ID="ac_ownership_type" runat="server" Width="100%" Rows="7" TabIndex="8"
                                            SelectionMode="Multiple">
                                            <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                            <asp:ListItem Value="W">Wholly Owned</asp:ListItem>
                                            <asp:ListItem Value="S">Shared</asp:ListItem>
                                            <asp:ListItem Value="F">Fractional</asp:ListItem>
                                        </asp:ListBox>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="left" VerticalAlign="top" Width="33%" CssClass="displayNoneMobile">
                                        Previously Owned:
                    <asp:DropDownList ID="ac_previously_owned_flag" runat="server" Width="100%" TabIndex="9">
                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:DropDownList>
                                        Lease Status:
                    <asp:DropDownList ID="lease_status" runat="server" Width="100%" TabIndex="10">
                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                        <asp:ListItem Value="Y">Leased</asp:ListItem>
                        <asp:ListItem Value="N">Not Leased</asp:ListItem>
                    </asp:DropDownList>
                                    </asp:TableCell>
                                    <asp:TableCell HorizontalAlign="right" VerticalAlign="top" Width="33%" CssClass="mobileWhiteBackground">
                                        <asp:UpdatePanel runat="server" ID="acSearchUpdate" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="acsearch" EventName="click" />
                                                <asp:AsyncPostBackTrigger ControlID="reset" EventName="click" />
                                            </Triggers>
                                            <ContentTemplate>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Panel runat="server" DefaultButton="acsearch" ID="acSearchButton">
                                            <asp:Button ID="acsearch" runat="server" Text="Search" CssClass="button-darker button_width"
                                                TabIndex="11" CausesValidation="true" ValidationGroup="Numeric" OnClientClick="javascript:FillStateHiddenValue(1);performCheck(); ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />
                                            <asp:Button ID="reset" runat="server" Text="Clear Selections" OnClick="Reset_Form"
                                                CssClass="font-weight-normal button_width displayNoneMobile" TabIndex="12" CausesValidation="false"
                                                OnClientClick="javascript: ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />
                                        </asp:Panel>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table runat="server" Width="100%" CellPadding="3" CellSpacing="0" ID="aircraftShowTable"
                                runat="server" Visible="false">
                                <asp:TableRow>
                                    <asp:TableCell Width="50">Show:</asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList runat="server" ID="aircraftShowNotes" onchange="toggleNotesDateToggle(this)">
                                            <asp:ListItem Text="Aircraft without Notes" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Aircraft with Notes" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Aircraft with or without Notes" Value="0" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                        <span id="placerHold" runat="server">
                                            <img src="images/spacer.gif" width="55" height="17" /></span><span id="aircraftNotesDateToggle"
                                                class="display_none" runat="server">
                                                <asp:CompareValidator ID="CompareValidator4" runat="server" ControlToValidate="notesDate"
                                                    ErrorMessage="&nbsp;*" Operator="DataTypeCheck" Type="Date" Text="&nbsp;*" Display="static"
                                                    ToolTip="*Valid Date Needed" Font-Bold="true" Font-Size="14px" ValidationGroup="Numeric" />Since:&nbsp;<asp:TextBox
                                                        runat="server" ID="notesDate" Width="75"></asp:TextBox>
                                                <cc1:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="notesDate"
                                                    Format="d" PopupButtonID="cal_image" />
                                                <asp:Image runat="server" ID="cal_image" ImageUrl="~/images/final.jpg" Visible="true" /></span>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <asp:Table ID="event_search_box" runat="server" Width="100%">
                                <asp:TableRow>
                                    <asp:TableCell VerticalAlign="Top" HorizontalAlign="Left">
                                        <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="conditional">
                                            <ContentTemplate>
                                                <evo:eventCategoryTypeDropdowns ID="eventCategoryTypeDropdowns" runat="server" />

                                                <script type="text/javascript" language="JavaScript">
                                                    refreshEventCombosJS("", "", eventCatType, eventCatCode);
                                                </script>

                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:Table ID="Table12" Width="100%" CellPadding="3" CellSpacing="0" runat="server"
                                            CssClass="seperator_search_box">
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="6">
                                Find Events that have occurred in the last:
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow CssClass="eventTimes">
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
                                                <asp:TableCell HorizontalAlign="Left" VerticalAlign="top" CssClass="mobile_display_off_cell">
                                                    <asp:Button runat="server" ID="clearTimesMarket" CssClass="remove_margin displayNoneMobile"
                                                        Text="Clear Times" OnClientClick="resetMarketTimes();return false;" />

                                                </asp:TableCell>
                                                <asp:TableCell HorizontalAlign="right" VerticalAlign="bottom" RowSpan="2">
                                                    <asp:TextBox runat="server" ID="event_type_text" CssClass="display_none"></asp:TextBox>
                                                    <asp:UpdatePanel runat="server" ID="EventSearchUpdatePanel" UpdateMode="Conditional">
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="events_search" />
                                                            <asp:AsyncPostBackTrigger ControlID="Button2" />
                                                        </Triggers>
                                                        <ContentTemplate>
                                                            <asp:Panel runat="server" DefaultButton="events_search" ID="eventsSearchButton">
                                                                <asp:Button ID="events_search" runat="server" Text="Search" CssClass="button_width button-darker"
                                                                    UseSubmitBehavior="true" TabIndex="12" CausesValidation="true" ValidationGroup="Numeric"
                                                                    OnClientClick="javascript:if (Page_ClientValidate()) {FillStateHiddenValue(1);FillEventType();performCheck(); ChangeTheMouseCursorOnItemParentDocument('cursor_wait');} else {return false;}" /><br />
                                                                <asp:Button ID="Button2" runat="server" Text="Clear Selections" OnClick="Reset_Form"
                                                                    CssClass="button_width font-weight-normal displayNoneMobile" TabIndex="13" CausesValidation="false"
                                                                    OnClientClick="javascript: ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell HorizontalAlign="left" VerticalAlign="top" ColumnSpan="5">
                                                    <asp:Button runat="server" ID="eventAlertMaintenanceButton" CssClass="float_right" Style="display: none;" Text="Schedule Event Alert" OnClientClick="SubMenuDropAircraft(3, 0, true);return false;" />
                                                    <asp:CompareValidator ID="CompareValidator3" runat="server" ErrorMessage="*Months must be less or equal to 120. "
                                                        ControlToValidate="events_months" ValueToCompare="120" Type="Integer" Operator="LessThanEqual"
                                                        ValidationGroup="Numeric" Display="Dynamic"></asp:CompareValidator>
                                                    <asp:CompareValidator runat="server" ControlToValidate="events_months" ID="Validate_EventMonths"
                                                        ValidationGroup="Numeric" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"
                                                        ErrorMessage="*Incorrect Format (Months must be a number)<br />">
                                                    </asp:CompareValidator>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="events_months"
                                                        Display="Dynamic" ValidationGroup="numeric" ErrorMessage="*Incorrect Format (Months must be a number)<br />" />
                                                    <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="*Days must be less or equal to 365. "
                                                        ControlToValidate="event_days" ValueToCompare="365" Type="Integer" Operator="LessThanEqual"
                                                        ValidationGroup="Numeric" Display="Dynamic"></asp:CompareValidator>
                                                    <asp:CompareValidator runat="server" ControlToValidate="event_days" ID="Validate_EventDays"
                                                        ValidationGroup="Numeric" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"
                                                        ErrorMessage="*Incorrect Format (Days must be a number)<br />">
                                                    </asp:CompareValidator>
                                                    <asp:RequiredFieldValidator runat="server" ID="requiredDays" ControlToValidate="event_days"
                                                        Display="Dynamic" ValidationGroup="numeric" ErrorMessage="*Incorrect Format (Days must be a number)<br />" />
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="event_hours"
                                                        Display="Dynamic" ValidationGroup="numeric" ErrorMessage="*Incorrect Format (Hours must be a number)<br />" />
                                                    <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="*Hours must be less or equal to 168. "
                                                        ControlToValidate="event_hours" ValueToCompare="168" Type="Integer" Operator="LessThanEqual"
                                                        ValidationGroup="Numeric" Display="Dynamic"></asp:CompareValidator>
                                                    <asp:CompareValidator runat="server" ControlToValidate="event_hours" ID="Validate_EventHours"
                                                        ValidationGroup="Numeric" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"
                                                        ErrorMessage="*Incorrect Format (Hours must be a number)<br />">
                                                    </asp:CompareValidator>
                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="event_minutes"
                                                        Display="Dynamic" ValidationGroup="numeric" ErrorMessage="*Incorrect Format (Minutes must be a number)<br />" />
                                                    <asp:CompareValidator runat="server" ErrorMessage="*Minutes must be less or equal to 60. "
                                                        ValidationGroup="Numeric" ControlToValidate="event_minutes" ValueToCompare="60"
                                                        Type="Integer" Operator="LessThanEqual" Display="Dynamic"></asp:CompareValidator>
                                                    <asp:CompareValidator runat="server" ControlToValidate="event_minutes" ID="Validate_EventMinutes"
                                                        ValidationGroup="Numeric" Display="Dynamic" Operator="DataTypeCheck" Type="Integer"
                                                        ErrorMessage="*Incorrect Format (Minutes must be a number)<br />">
                                                    </asp:CompareValidator>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="2">
                            <asp:UpdatePanel runat="server" ID="ac_advanced_update" UpdateMode="Conditional"
                                ChildrenAsTriggers="false">
                                <ContentTemplate>
                                    <cc1:TabContainer ID="ac_advanced_search" runat="server" Visible="true" CssClass="dark-theme"
                                        AutoPostBack="false">
                                        <cc1:TabPanel ID="location" runat="server" HeaderText="Location" Visible="true" Style="display: none; visibility: hidden;">
                                            <ContentTemplate>
                                                <asp:Panel ID="Panel2" Width="624px" runat="server" CssClass="region_panel">
                                                    <evo:viewCCSTDropDowns ID="viewCCSTDropDownsAirport" runat="server" />
                                                </asp:Panel>

                                                <script language="javascript" type="text/javascript">
                                                    checkRadioButtons(bIsBaseBase, bIsViewBase, companyRegion, baseRegion, viewRegion, companyCountry, baseCountry, viewCountry, companyState, baseState, viewState, companyTimeZone, viewTimeZone);
                                                </script>
                                                <asp:Panel runat="server" ID="location_dynamic_panel">
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="equip" runat="server" HeaderText="Equip/Maint" Visible="true" Style="display: none; visibility: hidden;">
                                            <ContentTemplate>
                                                <table width="100%" cellpadding="4" cellspacing="0" class="data_aircraft_grid">
                                                    <tr>
                                                        <td align="left" valign="top" class="data_aircraft_grid_cell light_seafoam_green_header_color"
                                                            colspan="4">
                                                            <b>Maintenance Regulation</b>&nbsp;&nbsp;<a href='https://www.jetnetevolution.com/help/documents/1034.pdf' target='_blank'><img src='images/magnify_small.png' width='9' alt='Maintenance Regulation' title='Maintenance Regulation' /></a>
                                                        </td>
                                                        <td align="left" valign="top" class="data_aircraft_grid_cell light_seafoam_green_header_color"
                                                            colspan="4"></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top" width="70" class="alt_row">US:
                                                        </td>
                                                        <td align="left" valign="top" width="160" class="alt_row">
                                                            <asp:DropDownList ID="COMPARE_us_ac_maintained" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="us_ac_maintained" Width="180px" ToolTip="Maintenance Regulation Name (US)"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" valign="top" width="70" class="alt_row">Foreign:
                                                        </td>
                                                        <td align="left" valign="top" class="alt_row">
                                                            <asp:DropDownList ID="COMPARE_foreign_ac_maintained" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="foreign_ac_maintained" Width="180px" ToolTip="Maintenance Regulation Name (FOREIGN)"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" valign="top" colspan="4" class="alt_row"></td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top" class="data_aircraft_grid_cell light_seafoam_green_header_color"
                                                            colspan="4">
                                                            <b>Engine Maintenance Program (EMP)</b> <a href="#" onclick="javascript:load('MasterLists.aspx?helplist=emp','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;">
                                                                <img src="../images/magnify_small.png" class="padding_left help_cursor" border="0"
                                                                    width="9" alt="Click for More Information." title="Click for More Information." /></a>
                                                        </td>
                                                        <td align="left" valign="top" class="light_border data_aircraft_grid_cell light_seafoam_green_header_color"
                                                            colspan="4">
                                                            <b>Engine Management Program (EMGP)</b> <a href="#" onclick="javascript:load('MasterLists.aspx?helplist=emgp','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;">
                                                                <img src="../images/magnify_small.png" class="padding_left help_cursor" border="0"
                                                                    width="9" alt="Click for More Information." title="Click for More Information." /></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top" width="70" class="light_border">Provider:
                                                        </td>
                                                        <td align="left" valign="top" width="160">
                                                            <asp:DropDownList ID="COMPARE_emp_provider_name" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="emp_provider_name" Width="180px" ToolTip="Engine Maintenance Program (EMP) Provider Name"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" valign="top" width="70">Name:
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:DropDownList ID="COMPARE_emp_program_name" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="emp_program_name" Width="180px" ToolTip="Engine Maintenance Program (EMP) Provider Name"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" valign="top" class="alt_row light_border">Provider:
                                                        </td>
                                                        <td align="left" valign="top" class="alt_row">
                                                            <asp:DropDownList ID="COMPARE_emgp_provider_name" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="emgp_provider_name" Width="180px" ToolTip="Engine Management Program (EMGP) Provider Name"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" valign="top" class="alt_row">Name:
                                                        </td>
                                                        <td align="left" valign="top" class="alt_row">
                                                            <asp:DropDownList ID="COMPARE_emgp_program_name" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="emgp_program_name" Width="180px" ToolTip="Engine Management Program (EMGP) Program Name"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top" class="data_aircraft_grid_cell light_seafoam_green_header_color"
                                                            colspan="4">
                                                            <b>Airframe Maintenance Program (AMP)</b> <a href="#" onclick="javascript:load('MasterLists.aspx?helplist=amp','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;">
                                                                <img src="../images/magnify_small.png" class="padding_left help_cursor" border="0"
                                                                    width="9" alt="Click for More Information." title="Click for More Information." /></a>
                                                        </td>
                                                        <td align="left" valign="top" class="light_border data_aircraft_grid_cell light_seafoam_green_header_color"
                                                            colspan="4">
                                                            <b>Airframe Maintenance Tracking Program (AMTP)</b> <a href="#" onclick="javascript:load('MasterLists.aspx?helplist=amtp','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;">
                                                                <img src="../images/magnify_small.png" class="padding_left help_cursor" border="0"
                                                                    width="9" alt="Click for More Information." title="Click for More Information." /></a>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top" class="alt_row">Provider:
                                                        </td>
                                                        <td align="left" valign="top" class="alt_row">
                                                            <asp:DropDownList ID="COMPARE_amp_provider_name" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="amp_provider_name" Width="100%" ToolTip="Airframe Maintenance Provider"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" valign="top" class="alt_row">Name:
                                                        </td>
                                                        <td align="left" valign="top" class="alt_row">
                                                            <asp:DropDownList ID="COMPARE_amp_program_name" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="amp_program_name" Width="100%" ToolTip="Airframe Maintenance Program Name"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" valign="top" class="light_border">Provider:
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:DropDownList ID="COMPARE_amtp_provider_name" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="amtp_provider_name" Width="100%" ToolTip="Airframe Maintenance Tracking Program (AMTP) Provider Name"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" valign="top">Name:
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:DropDownList ID="COMPARE_amtp_program_name" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:DropDownList runat="server" ID="amtp_program_name" Width="100%" ToolTip="Airframe Maintenance Tracking Program (AMTP) Program Name"
                                                                ValidationGroup="String">
                                                                <asp:ListItem></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="8" align="left" valign="top" class="light_border data_aircraft_grid_cell light_seafoam_green_header_color">
                                                            <b>INSPECTIONS/MAINTENANCE ITEMS</b> <a href="#" onclick="javascript:void(0);">
                                                                <img src="../images/magnify_small.png" class="padding_left help_cursor" border="0"
                                                                    width="9" alt="Click for More Information." title="Click for More Information." /></a><br />
                                                            <table cellpadding="4" cellspacing="0" border="0" class="data_aircraft_grid">
                                                                <tr>
                                                                    <td align="left" valign="top" colspan="5" class="alt_row">
                                                                        <div style="text-align: center">
                                                                            Maintenance items/inspections are sometimes reported to JETNET as being completed
                                      without providing the specific date. In these situations, the words "as reported"
                                      are provided in the notes to clarify the date is not the actual date. In addition,
                                      in some situations only a year or month are provided for the completed item. To
                                      ensure these items are included in typical searches include the first day of the
                                      month or year in your date range. The checkbox labeled as "Include As Reported/Estimated
                                      Dates" can be used to include these items as if they were actual dates.
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="bottom" width="23%">Maintenance&nbsp;Item/Inspection&nbsp;:&nbsp;
                                    <asp:ListBox ID="maintenance_item" runat="server" Rows="1" ToolTip="Maintenance Item"
                                        ValidationGroup="String" SelectionMode="Single">
                                        <asp:ListItem Value="" Selected="True"></asp:ListItem>
                                    </asp:ListBox>
                                                                    </td>
                                                                    <td align="left" valign="bottom">Type&nbsp;:<br />
                                                                        <asp:ListBox ID="acmaint_date" runat="server" Rows="1" ToolTip="Maintenance Item Date"
                                                                            ValidationGroup="String" SelectionMode="Single">
                                                                            <asp:ListItem Value="CW">C/W</asp:ListItem>
                                                                            <asp:ListItem Value="DUE">Due</asp:ListItem>
                                                                        </asp:ListBox>
                                                                    </td>
                                                                    <td align="left" valign="bottom">Units&nbsp;:<br />
                                                                        <asp:ListBox ID="acmaint_time" runat="server" Rows="1" ToolTip="Maintenance Item Time"
                                                                            ValidationGroup="String" SelectionMode="Single">
                                                                            <asp:ListItem Value="DATE">Date</asp:ListItem>
                                                                            <asp:ListItem Value="HOURS">Hours</asp:ListItem>
                                                                        </asp:ListBox>
                                                                    </td>
                                                                    <td align="left" valign="bottom">Conditions&nbsp;:<br />
                                                                        <asp:DropDownList ID="COMPARE_acmaint_value" runat="server" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'acmaint_value', 'input');">
                                                                            <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                                            <asp:ListItem Value="Less Than">Less Than</asp:ListItem>
                                                                            <asp:ListItem Value="Greater Than">Greater Than</asp:ListItem>
                                                                            <asp:ListItem Value="Between">Between</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        &nbsp;
                                    <asp:TextBox ID="acmaint_value" runat="server" Width="52%" ToolTip="Maintenance Item Value"
                                        Rows="1" Height="12px" ValidationGroup="String" TextAlign="right"></asp:TextBox>&nbsp;<asp:Image
                                            ID="Image1" ImageUrl="~/images/magnify_small.png" runat="server" AlternateText="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;"
                                            ToolTip="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;" />
                                                                    </td>
                                                                    <td align="center" valign="bottom">
                                                                        <asp:CheckBox ID="acmaint_chk" ToolTip="Include As Reported/Estimated Dates" Text="Include As Reported/Estimated Dates"
                                                                            runat="server" Font-Size="10px" Checked="true" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="bottom">
                                                                        <asp:ListBox ID="maintenance_item1" runat="server" Rows="1" ToolTip="Available Maintenance Items"
                                                                            ValidationGroup="String" SelectionMode="Single">
                                                                            <asp:ListItem Value="" Selected="True"></asp:ListItem>
                                                                        </asp:ListBox>
                                                                    </td>
                                                                    <td align="left" valign="bottom">
                                                                        <asp:ListBox ID="acmaint_date1" runat="server" Rows="1" ToolTip="Maintenance Item Date"
                                                                            ValidationGroup="String" SelectionMode="Single">
                                                                            <asp:ListItem Value="CW">C/W</asp:ListItem>
                                                                            <asp:ListItem Value="DUE">Due</asp:ListItem>
                                                                        </asp:ListBox>
                                                                    </td>
                                                                    <td align="left" valign="bottom">
                                                                        <asp:ListBox ID="acmaint_time1" runat="server" Rows="1" ToolTip="Maintenance Item Time"
                                                                            ValidationGroup="String" SelectionMode="Single">
                                                                            <asp:ListItem Value="DATE">Date</asp:ListItem>
                                                                            <asp:ListItem Value="HOURS">Hours</asp:ListItem>
                                                                        </asp:ListBox>
                                                                    </td>
                                                                    <td align="left" valign="bottom">
                                                                        <asp:DropDownList ID="COMPARE_acmaint_value1" runat="server" onchange="javascript:ClearAssociatedBox($(this).find(':selected').val(),'acmaint_value1', 'input');">
                                                                            <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                                            <asp:ListItem Value="Less Than">Less Than</asp:ListItem>
                                                                            <asp:ListItem Value="Greater Than">Greater Than</asp:ListItem>
                                                                            <asp:ListItem Value="Between">Between</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        &nbsp;
                                    <asp:TextBox ID="acmaint_value1" runat="server" Width="52%" ToolTip="Maintenance Item Value"
                                        Rows="1" Height="12px" ValidationGroup="String" TextAlign="right"></asp:TextBox>&nbsp;<asp:Image
                                            ID="Image2" ImageUrl="~/images/magnify_small.png" runat="server" AlternateText="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;"
                                            ToolTip="&ldquo;mm/dd/yyyy&rdquo;, for Between Use &ldquo;mm/dd/yyyy:mm/dd/yyyy&rdquo;" />
                                                                    </td>
                                                                    <td align="center" valign="bottom">
                                                                        <asp:CheckBox ID="acmaint_chk1" ToolTip="Include As Reported/Estimated Dates" Text="Include As Reported/Estimated Dates"
                                                                            runat="server" Font-Size="10px" Checked="true" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Panel runat="server" ID="equip_dynamic_panel">
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="company_contact" runat="server" HeaderText="Company/Contact" Visible="true"
                                            Style="display: none; visibility: hidden;">
                                            <ContentTemplate>
                                                <asp:Table ID="Table3" Width="100%" CellPadding="5" CellSpacing="0" runat="server"
                                                    CssClass="data_aircraft_grid">
                                                    <asp:TableRow>
                                                        <asp:TableCell CssClass="header_row" ColumnSpan="5"><b>COMPANY/CONTACT DEMOGRAPHICS</b></asp:TableCell>
                                                    </asp:TableRow>
                                                    <asp:TableRow>
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" Width="230">
                                                            Company Name:
                              <asp:CheckBox ID="comp_alt_name" Text="Search Alternate Name" runat="server" Font-Size="10px"
                                  CssClass="display_none" Checked="true" />
                                                            <asp:DropDownList ID="COMPARE_comp_name" runat="server" CssClass="display_none">
                                                                <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="comp_name" runat="server" Width="100%" Rows="1" Height="12px" TextMode="MultiLine"
                                                                ToolTip="Company Name" ValidationGroup="String"></asp:TextBox>
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
                                                            Relationships to Aircraft:<br class="clear" />
                                                            <asp:CheckBox ID="comp_not_in_selected" Text="Not in Selected Relationship" runat="server"
                                                                Font-Size="10px" /><br class="clear" />
                                                            <asp:ListBox ID="cref_contact_type" runat="server" Width="100%" SelectionMode="Multiple"
                                                                ToolTip="Relationships to Aircraft" ValidationGroup="String">
                                                                <asp:ListItem>All</asp:ListItem>
                                                            </asp:ListBox>
                                                        </asp:TableCell>
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" BackColor="#DAE1E8" RowSpan="3"
                                                            Width="43%">
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
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" BackColor="#DAE1E8" RowSpan="3">
                                                            Contact Title Group:
                              <asp:DropDownList ID="COMPARE_contact_title" runat="server" CssClass="display_none">
                                  <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                              </asp:DropDownList>
                                                            <asp:ListBox ID="contact_title" runat="server" Width="100%" SelectionMode="Multiple"
                                                                ToolTip="Contact Title" ValidationGroup="String">
                                                                <asp:ListItem>All</asp:ListItem>
                                                            </asp:ListBox>
                                                            <br />
                                                            Phone Number:
                              <asp:DropDownList ID="COMPARE_comp_phone_office" runat="server" CssClass="display_none">
                                  <asp:ListItem Value="Begins With">Begins With</asp:ListItem>
                              </asp:DropDownList>
                                                            <asp:TextBox ID="comp_phone_office" runat="server" Width="100" ToolTip="Phone" ValidationGroup="String"
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
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" ColumnSpan="2">
                                                            <span class="float_left padded_right">Company Folder:&nbsp;</span>
                                                            <span class="float_left padded_left">
                                                                <asp:DropDownList ID="COMPARE_cref_comp_id" runat="server" CssClass="display_none">
                                                                    <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <asp:DropDownList ID="cref_comp_id" runat="server" Width="100%" ToolTip="Company Folder"
                                                                    ValidationGroup="String">
                                                                    <asp:ListItem Value="">Please Select One</asp:ListItem>
                                                                </asp:DropDownList></span>
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
                                                            Fractional Percent:
                              <asp:DropDownList ID="COMPARE_cref_owner_percent" runat="server">
                                  <asp:ListItem Value="Equals">Equals</asp:ListItem>
                                  <asp:ListItem Value="Less Than">Less Than</asp:ListItem>
                                  <asp:ListItem Value="Greater Than">Greater Than</asp:ListItem>
                                  <asp:ListItem Value="Between">Between</asp:ListItem>
                              </asp:DropDownList>
                                                            &nbsp;
                              <asp:TextBox ID="cref_owner_percent" runat="server" Width="22%" ToolTip="Fractional Percent"
                                  Rows="1" Height="12px" ValidationGroup="String" TextAlign="right"></asp:TextBox><br />
                                                        </asp:TableCell>
                                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" BackColor="#DAE1E8">
                                                            Fractional&nbsp;Programs&nbsp;:
                              <asp:ListBox ID="lbFractionalProgram" runat="server" Rows="5" ToolTip="Available Fractional Programs"
                                  ValidationGroup="String" SelectionMode="Multiple">
                                  <asp:ListItem Value="0" Selected="True">All</asp:ListItem>
                              </asp:ListBox>
                                                        </asp:TableCell>
                                                    </asp:TableRow>
                                                </asp:Table>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="AttrTab" runat="server" HeaderText="Attributes" Visible="false"
                                            Style="display: none; visibility: hidden;">
                                            <ContentTemplate>
                                                <asp:Button runat="server" ID="TestLoadAttributes" CssClass="display_none" />
                                                <asp:TextBox runat="server" ID="attrBoolRan" CssClass="display_none"></asp:TextBox>
                                                <asp:Panel runat="server" ID="AttributesPanel">
                                                </asp:Panel>
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                        <cc1:TabPanel ID="Custom_MPM" runat="server" Visible="false" HeaderText="Custom">
                                            <ContentTemplate>
                                                <asp:Table runat="server" ID="advanced_search_categories_table" Visible="false">
                                                    <asp:TableRow>
                                                        <asp:TableCell ColumnSpan="7" HorizontalAlign="Left" VerticalAlign="Top">
                                                            <strong><u>Aircraft Custom Data:</u></strong>&nbsp;&nbsp;<asp:ImageButton ID="infoButton1"
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
                                            </ContentTemplate>
                                        </cc1:TabPanel>
                                    </cc1:TabContainer>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
        </asp:Panel>
        <asp:UpdatePanel runat="server" ID="listingUpdatePanel" UpdateMode="Conditional"
            ChildrenAsTriggers="false">
            <ContentTemplate>
                <div class="valueSpec aircraftListing Simplistic aircraftSpec Box">
                    <asp:Label ID="aircraft_attention" runat="server" Text="" CssClass="red_text emphasis_text text_align_center small_to_medium_text mobileTopPaddingAttention"></asp:Label>
                    <asp:Label runat="server" ID="FolderInformation" Visible="false" CssClass="FolderNameBar help_cursor"></asp:Label>
                    <asp:DataList ID="mobileDataList" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                        AutoGenerateColumns="False" GridLines="horizontal" BorderColor="#eeeeee" AllowPaging="false"
                        CssClass="mGrid">
                        <ItemStyle VerticalAlign="Top" Width="50%" />
                        <ItemTemplate>
                            <div class="boxed_item_padding">
                                <h1 class="dataListH1 float_left div_clear">
                                    <%#TrimName(DataBinder.Eval(Container.DataItem, "ac_year"), DataBinder.Eval(Container.DataItem, "amod_make_name"), DataBinder.Eval(Container.DataItem, "amod_model_name"), DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), DataBinder.Eval(Container.DataItem, "ac_id"))%></h1>
                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_picture_id")), "<img src='" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/aircraft/") & DataBinder.Eval(Container.DataItem, "ac_id") & "-0-" & DataBinder.Eval(Container.DataItem, "ac_picture_id") & ".jpg' alt='AC Picture' width='220' class='border float_left cursor' onclick=""javascript:SubmitTransactionDocumentForm('" & DataBinder.Eval(Container.DataItem, "amod_make_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "amod_model_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString & "'," & DataBinder.Eval(Container.DataItem, "ac_id").ToString & ",0,'');""/>", IIf(DataBinder.Eval(Container.DataItem, "amod_airframe_type_code ").ToString = "F", "<img src='images/jet_no_image.jpg' width='220' class='border float_left' />", "<img src='images/helo_no_image.jpg' width='220' class='border float_left' />"))%>
                                <div class="float_right halfScreen">
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_reg_no")), "<span class=""float_left"">" & DataBinder.Eval(Container.DataItem, "ac_reg_no") & "</span>", "")%>
                                    <%#showEstAFTT(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs")), DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_year")), DataBinder.Eval(Container.DataItem, "ac_year"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_times_as_of_date")), DataBinder.Eval(Container.DataItem, "ac_times_as_of_date"), ""), True, False)%>
                                    <%#crmWebClient.clsGeneral.clsGeneral.MobileDisplayStatus(DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_status"), DataBinder.Eval(Container.DataItem, "ac_delivery"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "ac_list_date"), DataBinder.Eval(Container.DataItem, "ac_asking"), False, Now())%>
                                    <asp:Label ID="company_information" runat="server" Text=''></asp:Label>
                                    <%#DisplayBaseInfo(DataBinder.Eval(Container.DataItem, "ac_aport_country"), DataBinder.Eval(Container.DataItem, "ac_aport_state"))%>
                                    <asp:Label ID="Label1" runat="server" Text='<%#(DisplayMobileCompanies(DataBinder.Eval(Container.DataItem, "ac_id")))%>'></asp:Label>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>


                    <asp:Repeater ID="ResultsSearchData" runat="server" Visible="false">
                        <AlternatingItemTemplate>
                            <tr class="alt_row">
                                <td class="alt_row" valign="top" align="left">
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_picture_id")), "<a href =""javascript:SubmitTransactionDocumentForm('" & DataBinder.Eval(Container.DataItem, "amod_make_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "amod_model_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString & "'," & DataBinder.Eval(Container.DataItem, "ac_id").ToString & ",0,'');"" class=""cursor""><i class=""fa fa-camera"" alt='Pictures' /></i></a>", "")%>
                                </td>
                                <td class="alt_row" valign="top" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%><br />
                                    <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
                                </td>
                                <td class="alt_row" valign="top" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%><br />
                                    <%#DataBinder.Eval(Container.DataItem, "ac_year")%>
                                </td>
                                <td class="alt_row" valign="top" align="left">
                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), "text_underline", "")%>
                                </td>
                                <td valign="top" class="alt_row" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_no")%>
                                    <%   If Trim(Session.Item("useFAAFlightData")) <> "" And Trim(Session.Item("useFAAFlightData")) <> "ARGUS" And HttpContext.Current.Session.Item("localPreferences").AerodexStandard = False Then %>
                                    <% else %>
                                    <br />
                                    <%#IIf(Trim(Session.Item("useFAAFlightData")) = "FAA", "<br /><a href='#' onclick=""javascript:load('FAAFlightData.aspx?acid=" & DataBinder.Eval(Container.DataItem, "ac_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;""  title='Flight Data' ><i class=""fa fa-plane"" alt='Flight Activity Data (Last 90 Days)' /></i></a>", "")%>
                                    <%#DisplayEvalueIcon(DataBinder.Eval(Container.DataItem, "ac_id"), DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "AVGEvalue"))%>
                                    <% end if %>
                                </td>
                                <%  If Session.Item("localSubscription").crmAerodexFlag = False Then %>
                                <td class="alt_row" valign="top" align="left">
                                    <%#IIf(DataBinder.Eval(Container.DataItem, "ac_forsale_flag").ToString = "Y", "<span class='green_background'>" & DataBinder.Eval(Container.DataItem, "ac_status") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_asking_price")), IIf(DataBinder.Eval(Container.DataItem, "ac_asking").ToString = "Price", "<br /><span class=""emphasis_text"">" & crmWebClient.clsGeneral.clsGeneral.ConvertIntoThousands(DataBinder.Eval(Container.DataItem, "ac_asking_price")) & "</span>", ""), "<br /><span class=""emphasis_text"">" & DataBinder.Eval(Container.DataItem, "ac_asking").ToString & "</span>") & ShowDom(DataBinder.Eval(Container.DataItem, "ac_list_date")) & "</span>", "<span>" & DataBinder.Eval(Container.DataItem, "ac_status") & "</span>")%>
                                    <span class="<%= Session.Item("localUser").crmUser_Evalues_CSS %>">
                                        <%#DisplayEValuesData(DataBinder.Eval(Container.DataItem, "AVGEvalue"))%></span>
                                </td>
                                <% end if %>
                                <td class="alt_row" valign="top" align="left">
                                    <asp:Label ID="company_information" runat="server" Text='<%#(crmWebClient.CompanyFunctions.FindEvolutionACCompanies(masterPage.aclsData_Temp, DataBinder.Eval(Container.DataItem, "ac_id")))%>'></asp:Label>
                                </td>
                                <td class="alt_row" valign="top" align="left">
                                    <%#showEstAFTT(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs")), DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_year")), DataBinder.Eval(Container.DataItem, "ac_year"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_times_as_of_date")), DataBinder.Eval(Container.DataItem, "ac_times_as_of_date"), ""), False, True)%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs")), "" & DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs") & "", "")%><br />
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs")), "" & DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs") & "", "")%><br />
                                </td>
                                <td class="alt_row" valign="top" align="left">
                                    <%#crmWebClient.clsGeneral.clsGeneral.Show_Evo_Event_Ac_Listing(DataBinder.Eval(Container.DataItem, "ac_last_event"), DataBinder.Eval(Container.DataItem, "ac_last_aerodex_event"))%>
                                </td>
                                <%   If Session.Item("localSubscription").crmServerSideNotes_Flag Or Session.Item("localSubscription").crmCloudNotes_Flag Then %>
                                <td class="alt_row" valign="top" align="left">
                                    <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True Or Session.Item("localSubscription").crmCloudNotes_Flag = True, AircraftBuildNote(DataBinder.Eval(Container.DataItem, "ac_id"), "AC"), "")%>
                                </td>
                                <% end if  %>
                            </tr>
                            <%#DisplayClientAircraftRow(DataBinder.Eval(Container.DataItem, "ac_id"), DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"), DataBinder.Eval(Container.DataItem, "amod_model_name"), DataBinder.Eval(Container.DataItem, "amod_make_name"), DataBinder.Eval(Container.DataItem, "amod_id"))%>
                        </AlternatingItemTemplate>
                        <HeaderTemplate>
                            <div class=" gray_background_color">
                                <table width="98%" class="formatTable blue datagrid" cellpadding="0" cellspacing="0"
                                    style="border-collapse: collapse !important; border-spacing: 0px;">
                                    <tr>
                                        <td class="mobile_display_off_cell label gray" valign="top" align="left"></td>
                                        <td class="mobile_display_off_cell label gray" valign="top" align="left">MAKE<br />
                                            MODEL
                                        </td>
                                        <td class="mobile_display_off_cell label gray" width="90px" valign="top" align="left">YEAR MFG<br />
                                            YEAR DLV
                                        </td>
                                        <td class="mobile_display_off_cell label gray" width="70px" valign="top" align="left">SERIAL<br />
                                            NUMBER
                                        </td>
                                        <td class="mobile_display_off_cell label gray" width="70px" valign="top" align="left">REG<br />
                                            NUMBER
                                        </td>
                                        <%  If Session.Item("localSubscription").crmAerodexFlag = False Then %>
                                        <td class="mobile_display_off_cell label gray" width="85px" valign="top" align="left">STATUS<br />
                                            PRICE
                                        </td>
                                        <% end if %>
                                        <td class="mobile_display_off_cell label gray" valign="top" align="left">COMPANY
                                        </td>
                                        <td class="mobile_display_off_cell label gray" width="100px" valign="top" align="left">AFTT / <a href='javascript:void();' onclick='openEstAFTTHelp();' class='text_underline'>EST AFTT</a><br />
                                            ENGINE TT<br />
                                            SMOH
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                        <%   If Session.Item("localSubscription").crmServerSideNotes_Flag Or Session.Item("localSubscription").crmCloudNotes_Flag Then %>
                                        <td>&nbsp;
                                        </td>
                                        <% end if  %>
                                    </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td valign="top" align="left">
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_picture_id")), "<a href =""javascript:SubmitTransactionDocumentForm('" & DataBinder.Eval(Container.DataItem, "amod_make_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "amod_model_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString & "'," & DataBinder.Eval(Container.DataItem, "ac_id").ToString & ",0,'');"" class=""cursor""><i class=""fa fa-camera"" alt='Pictures' /></i></a>", "")%>
                                </td>
                                <td valign="top" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%><br />
                                    <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
                                </td>
                                <td valign="top" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%><br />
                                    <%#DataBinder.Eval(Container.DataItem, "ac_year")%>
                                </td>
                                <td valign="top" align="left">
                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), "text_underline", "")%>
                                </td>
                                <td valign="top" align="left">
                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_no")%>
                                    <%   If Trim(Session.Item("useFAAFlightData")) <> "" And Trim(Session.Item("useFAAFlightData")) <> "ARGUS" And HttpContext.Current.Session.Item("localPreferences").AerodexStandard = False Then %>
                                    <% else %>
                                    <br />
                                    <%#IIf(Trim(Session.Item("useFAAFlightData")) = "FAA", "<br /><a href='#' onclick=""javascript:load('FAAFlightData.aspx?acid=" & DataBinder.Eval(Container.DataItem, "ac_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;""  title='Flight Data' ><i class=""fa fa-plane"" alt='Flight Activity Data (Last 90 Days)' /></i></a>", "")%>
                                    <%#DisplayEvalueIcon(DataBinder.Eval(Container.DataItem, "ac_id"), DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "AVGEvalue"))%>
                                    <% end if %>
                                </td>
                                <%  If Session.Item("localSubscription").crmAerodexFlag = False Then %>
                                <td valign="top" align="left">
                                    <%#IIf(DataBinder.Eval(Container.DataItem, "ac_forsale_flag").ToString = "Y", "<span class='green_background'>" & DataBinder.Eval(Container.DataItem, "ac_status") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_asking_price")), IIf(DataBinder.Eval(Container.DataItem, "ac_asking").ToString = "Price", "<br /><span class=""emphasis_text"">" & crmWebClient.clsGeneral.clsGeneral.ConvertIntoThousands(DataBinder.Eval(Container.DataItem, "ac_asking_price")) & "</span>", ""), "<br /><span class=""emphasis_text"">" & DataBinder.Eval(Container.DataItem, "ac_asking").ToString & "</span>") & ShowDom(DataBinder.Eval(Container.DataItem, "ac_list_date")) & "</span>", "<span>" & DataBinder.Eval(Container.DataItem, "ac_status") & "</span>")%>
                                    <span class="<%= Session.Item("localUser").crmUser_Evalues_CSS %>">
                                        <%#DisplayEValuesData(DataBinder.Eval(Container.DataItem, "AVGEvalue"))%></span>
                                </td>
                                <% end if %>
                                <td valign="top" align="left">
                                    <asp:Label ID="company_information" runat="server" Text='<%#(crmWebClient.CompanyFunctions.FindEvolutionACCompanies(masterPage.aclsData_Temp, DataBinder.Eval(Container.DataItem, "ac_id")))%>'></asp:Label>
                                </td>
                                <td valign="top" align="left">
                                    <%#showEstAFTT(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs")), DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_year")), DataBinder.Eval(Container.DataItem, "ac_year"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_times_as_of_date")), DataBinder.Eval(Container.DataItem, "ac_times_as_of_date"), ""), False, True)%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs")), "" & DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs") & "", "")%><br />
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs")), "" & DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs") & "", "")%><br />
                                </td>
                                <td valign="top" align="left">
                                    <%#crmWebClient.clsGeneral.clsGeneral.Show_Evo_Event_Ac_Listing(DataBinder.Eval(Container.DataItem, "ac_last_event"), DataBinder.Eval(Container.DataItem, "ac_last_aerodex_event"))%>
                                </td>
                                <%   If Session.Item("localSubscription").crmServerSideNotes_Flag Or Session.Item("localSubscription").crmCloudNotes_Flag Then %>
                                <td valign="top" align="left">
                                    <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True Or Session.Item("localSubscription").crmCloudNotes_Flag = True, AircraftBuildNote(DataBinder.Eval(Container.DataItem, "ac_id"), "AC"), "")%>
                                </td>
                                <% end if  %>
                            </tr>
                            <%#DisplayClientAircraftRow(DataBinder.Eval(Container.DataItem, "ac_id"), DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"), DataBinder.Eval(Container.DataItem, "amod_model_name"), DataBinder.Eval(Container.DataItem, "amod_make_name"), DataBinder.Eval(Container.DataItem, "amod_id"))%>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table></div>
                        </FooterTemplate>
                    </asp:Repeater>
                    <asp:DataGrid runat="server" ID="AircraftSearchDataGrid" AutoGenerateColumns="false"
                        Width="100%" Visible="false" AllowCustomPaging="false" AllowPaging="true" CssClass="formatTable blue datagrid"
                        AlternatingItemStyle-CssClass="alt_row">
                        <Columns>
                            <asp:TemplateColumn HeaderText="" HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_picture_id")), "<a href =""javascript:SubmitTransactionDocumentForm('" & DataBinder.Eval(Container.DataItem, "amod_make_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "amod_model_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString & "'," & DataBinder.Eval(Container.DataItem, "ac_id").ToString & ",0,'');"" class=""cursor""><i class=""fa fa-camera"" alt='Pictures' /></i></a>", "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="MAKE<br />MODEL" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%><br />
                                    <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="YEAR MFG<br />YEAR DLV" ItemStyle-Width="60" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="bottom">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%><br />
                                    <%#DataBinder.Eval(Container.DataItem, "ac_year")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="SERIAL<br />NUMBER" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), "text_underline", "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="REG<br />NUMBER" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_no")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="REG<br />NUMBER" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_no")%>
                                    <br />
                                    <%#IIf(Trim(Session.Item("useFAAFlightData")) = "FAA", "<br /><a href='#' onclick=""javascript:load('FAAFlightData.aspx?acid=" & DataBinder.Eval(Container.DataItem, "ac_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;""  title='Flight Data' ><i class=""fa fa-plane"" alt='Flight Activity Data (Last 90 Days)' /></i></a>", "")%>
                                    <%#DisplayEvalueIcon(DataBinder.Eval(Container.DataItem, "ac_id"), DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "AVGEvalue"))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="STATUS<br />PRICE" Visible="false" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#IIf(DataBinder.Eval(Container.DataItem, "ac_forsale_flag").ToString = "Y", "<span class='green_background'>" & DataBinder.Eval(Container.DataItem, "ac_status") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_asking_price")), IIf(DataBinder.Eval(Container.DataItem, "ac_asking").ToString = "Price", "<br /><span class=""emphasis_text"">" & crmWebClient.clsGeneral.clsGeneral.no_zero(DataBinder.Eval(Container.DataItem, "ac_asking_price"), "", True) & "</span>", ""), "<br /><span class=""emphasis_text"">" & DataBinder.Eval(Container.DataItem, "ac_asking").ToString & "</span>") & ShowDom(DataBinder.Eval(Container.DataItem, "ac_list_date")) & "</span>", "<span>" & DataBinder.Eval(Container.DataItem, "ac_status") & "</span>")%>
                                    <span class="<%= Session.Item("localUser").crmUser_Evalues_CSS %>">
                                        <%#DisplayEValuesData(DataBinder.Eval(Container.DataItem, "AVGEvalue"))%></span>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="DELIVERY<br />LISTED" Visible="false" ItemStyle-Width="70"
                                HeaderStyle-CssClass="label gray" HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "ac_delivery")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_list_date")), "<br />" & crmWebClient.clsGeneral.clsGeneral.datenull(DataBinder.Eval(Container.DataItem, "ac_list_date")) & ShowDom(DataBinder.Eval(Container.DataItem, "ac_list_date")), "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="COMPANY" HeaderStyle-CssClass="label gray" HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:Label ID="company_information" runat="server" Text='<%#(crmWebClient.CompanyFunctions.FindEvolutionACCompanies(masterPage.aclsData_Temp, DataBinder.Eval(Container.DataItem, "ac_id")))%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderStyle-CssClass="label gray" HeaderStyle-VerticalAlign="Middle"
                                HeaderText="AFTT / <a href='javascript:void();' onclick='openEstAFTTHelp();' class='text_underline'>EST AFTT</a><br />ENGINE TT<br />SMOH">
                                <ItemStyle Width="120px" />
                                <ItemTemplate>
                                    <%#showEstAFTT(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")), DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs")), DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_year")), DataBinder.Eval(Container.DataItem, "ac_year"), ""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_times_as_of_date")), DataBinder.Eval(Container.DataItem, "ac_times_as_of_date"), ""), False, True)%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs")), "" & DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs") & "", "")%><br />
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs")), "" & DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs") & "", "")%>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs") & "", "")%><br />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="">
                                <ItemTemplate>
                                    <%#crmWebClient.clsGeneral.clsGeneral.Show_Evo_Event_Ac_Listing(DataBinder.Eval(Container.DataItem, "ac_last_event"), DataBinder.Eval(Container.DataItem, "ac_last_aerodex_event"))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="" Visible="false">
                                <ItemTemplate>
                                    <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True Or Session.Item("localSubscription").crmCloudNotes_Flag = True, AircraftBuildNote(DataBinder.Eval(Container.DataItem, "ac_id"), "AC"), "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <asp:DataGrid runat="server" ID="TransactionSearchDataGrid" AutoGenerateColumns="false"
                        Width="100%" Visible="false" AllowCustomPaging="false" AllowPaging="true" CssClass="formatTable blue datagrid"
                        AlternatingItemStyle-CssClass="alt_row">
                        <Columns>
                            <asp:TemplateColumn HeaderText="MAKE<br />MODEL" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%><br />
                                    <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="YEAR MFG<br />YEAR DLV" ItemStyle-Width="70" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%><br />
                                    <%#DataBinder.Eval(Container.DataItem, "ac_year")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="SERIAL<br />NUMBER" ItemStyle-Width="70" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full"), "text_underline", "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="REG<br />NUMBER" ItemStyle-Width="70" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_no").ToString%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="TRANS DATE" ItemStyle-Width="70" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_date")), crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), True, crmWebClient.clsGeneral.clsGeneral.FormatDateShorthand(DataBinder.Eval(Container.DataItem, "journ_date")), "", ""), "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="DESCRIPTION" ItemStyle-Width="295" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), True, IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "jcat_subcategory_name")), DataBinder.Eval(Container.DataItem, "jcat_subcategory_name") & " - ", "") & DataBinder.Eval(Container.DataItem, "journ_subject").ToString, "", "")%><%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_customer_note")), IIf(Not String.IsNullOrEmpty(Trim(DataBinder.Eval(Container.DataItem, "journ_customer_note").ToString)), "&nbsp;&nbsp;(<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), False, "", "", "") & " class='help_cursor error_text no_text_underline' title='" & DataBinder.Eval(Container.DataItem, "journ_customer_note") & "' alt='" & DataBinder.Eval(Container.DataItem, "journ_customer_note") & "'>Note</a>)", ""), "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="LISTED" ItemStyle-Width="70" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_list_date")), crmWebClient.clsGeneral.clsGeneral.FormatDateShorthand(DataBinder.Eval(Container.DataItem, "ac_list_date")), "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="ASKING" ItemStyle-Width="88" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DisplayHistoryDataGridAsking(DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_status").ToString, DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "ac_asking"))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="SALE" ItemStyle-Width="38" HeaderStyle-CssClass="label gray"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#IIf(DataBinder.Eval(Container.DataItem, "journ_subcat_code_part1") = "WS", ShowHistoryLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "journ_id"), DataBinder.Eval(Container.DataItem, "ac_id"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), True, DataBinder.Eval(Container.DataItem, "ac_sale_price"), "", DataBinder.Eval(Container.DataItem, "ac_sale_price_display_flag")), "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                    <asp:DataGrid runat="server" ID="EventsDataGrid" AutoGenerateColumns="false" Width="100%"
                        Visible="false" AllowCustomPaging="false" AllowPaging="true" CssClass="formatTable blue datagrid"
                        AlternatingItemStyle-CssClass="alt_row">
                        <Columns>
                            <asp:TemplateColumn HeaderText="MAKE" HeaderStyle-CssClass="label gray" HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name").ToString%>
                                    <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="YEAR" ItemStyle-CssClass="mobile_display_off_cell"
                                HeaderStyle-CssClass="mobile_display_off_cell label gray" FooterStyle-CssClass="mobile_display_off_cell"
                                HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "ac_year").ToString%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="SER #" ItemStyle-CssClass="mobile_display_off_cell"
                                HeaderStyle-CssClass="mobile_display_off_cell label gray" FooterStyle-CssClass="mobile_display_off_cell">
                                <ItemTemplate>
                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString, "text_underline", "")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="REG #" ItemStyle-CssClass="mobile_display_off_cell"
                                HeaderStyle-CssClass="mobile_display_off_cell label gray" FooterStyle-CssClass="mobile_display_off_cell">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_no").ToString%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="SER<br />REG" ItemStyle-CssClass="mobile_display_on_cell"
                                HeaderStyle-CssClass="mobile_display_on_cell label gray" FooterStyle-CssClass="mobile_display_on_cell">
                                <ItemTemplate>
                                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString, "", "")%><br />
                                    <%#DataBinder.Eval(Container.DataItem, "ac_reg_no").ToString%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="ACTIVITY<br /><em>Date/Time</em>" HeaderStyle-HorizontalAlign="Center"
                                HeaderStyle-CssClass="label gray" HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "apev_entry_date").ToString%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="DESCRIPTION" HeaderStyle-CssClass="label gray" HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container.DataItem, "apev_subject").ToString%>
                                    <%#LinkOutEventsCompanies(DataBinder.Eval(Container.DataItem, "apev_description"), DataBinder.Eval(Container.DataItem, "priorev_comp_id"), DataBinder.Eval(Container.DataItem, "priorev_contact_id"))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>

                    <div class="grid">
                        <asp:DataList ID="TransactionSearchDataList" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                            AutoGenerateColumns="False" GridLines="none" AllowPaging="true" CCssClass="formatTable blue gray_background_color">
                            <ItemStyle CssClass="grid-item" />
                            <ItemTemplate>
                                <div class="Box">
                                    <div class="row">
                                        <div class="columns seven remove_margin">
                                            <h2 class="mainHeading">
                                                <strong>
                                                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>
                                                    <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
                                                </strong>S/N
                    <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString, "", "")%>
                    </h1> <span class="li"><span class="label">Year Mfr/Dlv:</span>
                        <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%>/<%#DataBinder.Eval(Container.DataItem, "ac_year")%></span><span class="li"><span class="label">Reg #:</span><span class="emphasisColor mediumText">
                            <%#DataBinder.Eval(Container.DataItem, "ac_reg_no")%></span></span>
                                                <%#crmWebClient.clsGeneral.clsGeneral.DisplayStatusListingDateEvoACListing(DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_status").ToString, DataBinder.Eval(Container.DataItem, "ac_delivery"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "ac_list_date"), DataBinder.Eval(Container.DataItem, "ac_asking"), True, DataBinder.Eval(Container.DataItem, "journ_date"))%>
                                        </div>
                                        <div class="columns five ">
                                            <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_date")), "<span class=""li""><span class=""label"">Transaction Date:</span> " & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_date")), crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), True, crmWebClient.clsGeneral.clsGeneral.FormatDateShorthand(DataBinder.Eval(Container.DataItem, "journ_date")), "", ""), "") & "</span>", "")%>
                                            <span class="li">
                                                <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), True, IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "jcat_subcategory_name")), DataBinder.Eval(Container.DataItem, "jcat_subcategory_name") & " - ", "") & DataBinder.Eval(Container.DataItem, "journ_subject").ToString, "", "")%><%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "journ_customer_note")), IIf(Not String.IsNullOrEmpty(Trim(DataBinder.Eval(Container.DataItem, "journ_customer_note").ToString)), "&nbsp;&nbsp;(<a " & crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, DataBinder.Eval(Container.DataItem, "journ_id"), False, "", "help_cursor", "") & " title='" & DataBinder.Eval(Container.DataItem, "journ_customer_note") & "' alt='" & DataBinder.Eval(Container.DataItem, "journ_customer_note") & "'  class='help_cursor error_text no_text_underline'>Note</a>)", ""), "")%></span>
                                        </div>
                                    </div>
                                    <div class="row expandedLinks">
                                        <div class="columns seven">
                                            <span class="float_left">
                                                <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, "DETAILS", "", "")%>
                                            </span><span class="float_right">
                                                <%#IIf(DataBinder.Eval(Container.DataItem, "journ_subcat_code_part1") = "WS", ShowHistoryLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "journ_id"), DataBinder.Eval(Container.DataItem, "ac_id"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), False, DataBinder.Eval(Container.DataItem, "ac_sale_price"), "Sale Price:", DataBinder.Eval(Container.DataItem, "ac_sale_price_display_flag")), "")%></span>
                                        </div>
                                        <div class="columns five">
                                            <div class="float_right">
                                                <ul class="cssMenu">
                                                    <li><a href="#" class="expand_more">MORE</a>
                                                        <ul>
                                                            <li><a href="#"><a href="#" onclick="javascript:load('view_template.aspx?amod_id=<%#DataBinder.Eval(Container.DataItem, "amod_id")%>','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;">About This Model</a></li>
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
                        <asp:DataList ID="AircraftSearchDataList" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal"
                            AutoGenerateColumns="False" GridLines="none" AllowPaging="true" CssClass="formatTable blue gray_background_color">
                            <ItemStyle CssClass="grid-item" />
                            <ItemTemplate>
                                <div class="Box">
                                    <div class="row remove_margin">
                                        <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_picture_id")), "<div class=""columns five displayNoneMobile""><img src='" & IIf(HttpContext.Current.Session.Item("jetnetWebSiteType") <> crmWebClient.eWebSiteTypes.LOCAL, HttpContext.Current.Session.Item("jetnetFullHostName").ToString & HttpContext.Current.Session.Item("AircraftPicturesFolderVirtualPath") & "/", "https://www.testjetnetevolution.com/pictures/aircraft/") & DataBinder.Eval(Container.DataItem, "ac_id") & "-0-" & DataBinder.Eval(Container.DataItem, "ac_picture_id") & ".jpg' alt='AC Picture' width='100%' class='border float_left cursor displayNoneMobile' onclick=""javascript:SubmitTransactionDocumentForm('" & DataBinder.Eval(Container.DataItem, "amod_make_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "amod_model_name").ToString & "','" & DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString & "'," & DataBinder.Eval(Container.DataItem, "ac_id").ToString & ",0,'');""/></div>", IIf(DataBinder.Eval(Container.DataItem, "amod_airframe_type_code ").ToString = "F", "<div class=""columns five displayNoneMobile""><img src='images/jet_no_image.jpg' width='100%' class='border float_left displayNoneMobile toggleSmallScreen' /></div>", "<div class=""columns five displayNoneMobile""><img src='images/helo_no_image.jpg' width='100%' class='border float_left displayNoneMobile toggleSmallScreen' /></div>"))%>
                                        <div class="float_right columns seven remove_margin">
                                            <h2 class='mainHeading'>
                                                <strong>
                                                    <%#DataBinder.Eval(Container.DataItem, "amod_make_name")%>
                                                    <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), DataBinder.Eval(Container.DataItem, "amod_model_name"), True)%>
                                                </strong>S/N
                      <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, DataBinder.Eval(Container.DataItem, "ac_ser_no_full").ToString, "", "")%></h1>
                      <span class="li"><span class="label">Year Mfr/Dlv:</span>
                          <%#DataBinder.Eval(Container.DataItem, "ac_mfr_year")%>/<%#DataBinder.Eval(Container.DataItem, "ac_year")%></span><span class="li "><span class="label">Reg #:</span><span class="emphasisColor mediumText">
                              <%#DataBinder.Eval(Container.DataItem, "ac_reg_no")%></span></span>
                                                <%#crmWebClient.clsGeneral.clsGeneral.DisplayStatusListingDateEvoACListing(DataBinder.Eval(Container.DataItem, "ac_forsale_flag"), DataBinder.Eval(Container.DataItem, "ac_status"), DataBinder.Eval(Container.DataItem, "ac_delivery"), DataBinder.Eval(Container.DataItem, "ac_asking_price"), DataBinder.Eval(Container.DataItem, "ac_list_date"), DataBinder.Eval(Container.DataItem, "ac_asking"), False,  Now())%>
                                                <span class="<%= Session.Item("localUser").crmUser_Evalues_CSS %>">
                                                    <%#DisplayEValuesData(DataBinder.Eval(Container.DataItem, "AVGEvalue"))%></span>
                                                <asp:Label ID="company_information" runat="server" Text='<%#(crmWebClient.CompanyFunctions.FindEvolutionACCompanies(Masterpage.aclsData_Temp, DataBinder.Eval(Container.DataItem, "ac_id")))%>'></asp:Label>
                                                <%#DisplayClientAircraft(DataBinder.Eval(Container.DataItem, "ac_id"), DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"))%>
                                        </div>
                                        <div class="clear_left">
                                        </div>
                                        <div class="columns five remove_margin expandedLinks">
                                            <div class="float_left">
                                                <%#IIf(Session.Item("localSubscription").crmServerSideNotes_Flag = True Or Session.Item("localSubscription").crmCloudNotes_Flag = True, AircraftBuildNote(DataBinder.Eval(Container.DataItem, "ac_id"), "AC"), "")%><%#crmWebClient.clsGeneral.clsGeneral.Show_Evo_Event_Ac_Listing(DataBinder.Eval(Container.DataItem, "ac_last_event"), DataBinder.Eval(Container.DataItem, "ac_last_aerodex_event"))%>
                                                <% If Trim(Session.Item("useFAAFlightData")) <> "" And Trim(Session.Item("useFAAFlightData")) <> "ARGUS" And HttpContext.Current.Session.Item("localPreferences").AerodexStandard = False Then%>
                                                <%#IIf(Trim(Session.Item("useFAAFlightData")) = "FAA", "<a href='#' class='no_text_underline' onclick=""javascript:load('FAAFlightData.aspx?acid=" & DataBinder.Eval(Container.DataItem, "ac_id").ToString & "','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');return false;""  title='Flight Data'><i class=""fa fa-plane"" alt='Flight Activity Data (Last 90 Days)' /></i></a>", "")%>
                                                <%#DisplayEvalueIcon(DataBinder.Eval(Container.DataItem, "ac_id"),DataBinder.Eval(Container.DataItem, "amod_id"),DataBinder.Eval(Container.DataItem, "AVGEvalue"))%>
                                                <% End If%>
                                                <div class="float_left">
                                                    <ul class="cssMenu">
                                                        <li><a href="#" class="expand_more text_underline">MORE</a>
                                                            <ul>
                                                                <li>
                                                                    <%#crmWebClient.DisplayFunctions.WriteModelLink(DataBinder.Eval(Container.DataItem, "amod_id"), "About This Model", True)%></li>
                                                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_aport_iata_code")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_aport_icao_code")), IIf(Session.Item("isMobile"), "", "<li>" & crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, "Map This Aircraft", "", "&map=1") & "</li>"), "")%>
                                                            </ul>
                                                        </li>
                                                    </ul>
                                                </div>
                                                <br class="div_clear" />
                                            </div>
                                            <span class="float_right">
                                                <%#crmWebClient.DisplayFunctions.WriteDetailsLink(DataBinder.Eval(Container.DataItem, "ac_id"), 0, 0, 0, True, "DETAILS", "remove_padding text_underline", "")%>
                                            </span>
                                            <div class="clearfix border_bottom">
                                            </div>
                                            <div class=" mobileAFTT">
                                                <asp:Label ID="lbl_aftt_estaftt" runat="server" Text='<%#showEstAFTT(IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs")),DataBinder.Eval(Container.DataItem, "ac_airframe_tot_hrs"),""), IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs")),DataBinder.Eval(Container.DataItem, "ac_est_airframe_hrs"),""),IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_year")),DataBinder.Eval(Container.DataItem, "ac_year"),""),IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_times_as_of_date")),DataBinder.Eval(Container.DataItem, "ac_times_as_of_date"),""), true, false)%>'></asp:Label>
                                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs")), "<span class='li_no_bullet' style='padding:0px !important;'><span class=""label"">Eng TT</span>: " & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs")), "" & DataBinder.Eval(Container.DataItem, "ac_engine_1_tot_hrs") & "", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_2_tot_hrs") & "", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_3_tot_hrs") & "", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_4_tot_hrs") & "", "") & "</span>", "")%>
                                                <%#IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs")) Or Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs")), "<span class='li_no_bullet' style='padding:0px !important;'><span class=""label"">Eng SMOH</span>: " & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs")), "" & DataBinder.Eval(Container.DataItem, "ac_engine_1_soh_hrs") & "", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_2_soh_hrs") & "", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_3_soh_hrs") & "", "") & IIf(Not IsDBNull(DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs")), " / " & DataBinder.Eval(Container.DataItem, "ac_engine_4_soh_hrs") & "", "") & "</span>", "")%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                        <asp:Label runat="server" ID="page_type" CssClass="display_none"></asp:Label>

                    </div>

                </div>
                <asp:Panel runat="server" ID="Aircraft_Bottom_Paging" Visible="false">
                    <asp:Table runat="server" Width="100%" CellPadding="6" CellSpacing="0" border="0"
                        CssClass="dark_header">
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
            <img src="images/spacer.gif" alt="" height="15" />
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
                            <asp:TableCell HorizontalAlign="right" VerticalAlign="middle" Width="180">
                                <asp:Label ID="bottom_paging" runat="server" CssClass="criteria_text criteria_spacer">
                                    <asp:ImageButton ID="bottom_previous_all" ImageUrl="../images/previous_all.png" runat="server"
                                        Visible="false" OnClick="MoveNext" OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />&nbsp;<asp:ImageButton
                                            ID="bottom_previous" ImageUrl="../images/previous_listing.png" Visible="false"
                                            runat="server" OnClick="MoveNext" OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />&nbsp;<asp:Label
                                                ID="bottom_record_count" runat="server">Showing 25 - 50</asp:Label>&nbsp;<asp:ImageButton
                                                    ID="bottom_next_" ImageUrl="../images/next_listing.png" runat="server" OnClick="MoveNext"
                                                    OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" />&nbsp;<asp:ImageButton
                                                        ID="bottom_next_all" ImageUrl="~/images/next_all.png" runat="server" OnClick="MoveNext"
                                                        OnClientClick="javascript:ChangeTheMouseCursorOnItemParentDocument('cursor_wait');" /></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="below_form">

    <script type="text/javascript">


        function setPortfolioView(portfolioID) {
            my_form = document.createElement('FORM');
            window.open('', 'result' + portfolioID, '');
            my_form.method = 'POST';
            my_form.target = 'result' + portfolioID

            my_form.name = 'mappingForm';
            my_form.action = 'userPortfolio.aspx';

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = "REPORT_ID";
            my_tb.value = portfolioID;
            my_form.appendChild(my_tb);

            document.body.appendChild(my_form);
            my_form.submit();
        }

        function setFlightActivityView(portfolioID, portfolioName) {
            my_form = document.createElement('FORM');
            window.open('', 'result' + portfolioID, '');
            my_form.method = 'POST';
            my_form.target = 'result' + portfolioID

            my_form.name = 'mappingForm';
            my_form.action = 'view_template.aspx?noMaster=false&ViewID=28&ViewName=Flight Activity (Operator/Airport)';

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = "acfolder";
            my_tb.value = portfolioID;
            my_form.appendChild(my_tb);

            document.body.appendChild(my_form);

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = "acfoldername";
            my_tb.value = portfolioName;
            my_form.appendChild(my_tb);

            document.body.appendChild(my_form);
            my_form.submit();
        }
        function SubMenuDropAircraft(x, reportID, eventAlert) {
            var folder_type;
            folder_type = document.getElementById("<%= page_type.ClientID %>");

            my_form = document.createElement('FORM');
            my_form.method = 'POST';
            my_form.target = "_blank"


            switch (x) {
                case 4:
                    //Map Form
                    my_form.name = 'mappingForm';
                    my_form.action = 'MapItems.aspx';
                    document.body.appendChild(my_form);
                    my_form.submit();
                    break;
                case 12:
                    //Filter popup
                    if (folder_type.innerHTML == 'HISTORY') {
                        window.location = 'SearchSummary.aspx?h=1&filter=true'; //redirects to homepage
                    } else if (folder_type.innerHTML == 'EVENTS') {
                        window.location = 'SearchSummary.aspx?e=1&filter=true'; //redirects to homepage
                    } else {
                        window.location = 'SearchSummary.aspx?filter=true';
                    }
                    break;
                case 2:
                    //Summary popup
                    if (folder_type.innerHTML == 'HISTORY') {
                        window.open('SearchSummary.aspx?h=1', '_blank');
                    } else if (folder_type.innerHTML == 'EVENTS') {
                        window.open('SearchSummary.aspx?e=1', '_blank');
                    } else {
                       window.open('SearchSummary.aspx', '_blank');
                    }
                    //my_form.submit();
                    break;
                case 5:
                    var URL = "PDF_Creator.aspx?export_type=" + folder_type.innerHTML;
                    window.open(URL, '_blank');
                    break;
                case 7:
                    var URL = "view_template.aspx?ViewID=18&ViewName=Prospect Management&noMaster=false&UseAircraftSearch=Y"
                    window.open(URL, '_blank');
                    break;
                case 6:
                    my_form.action = 'Aircraft_Listing.aspx';
                    my_form.name = 'folderForm';
                case 3:
                    //folders maintenance popup  
                    //if (eventAlert == false) {
                        my_form.action = 'FolderMaintenance.aspx';
                        my_form.name = 'folderForm';
                    //} else {
                    //    my_form.action = 'EventAlertMaintenance.aspx';
                    //    my_form.name = 'folderForm';
                    //}

                    //Appending the type of folder, either Aircraft or History.
                    my_tb = document.createElement('INPUT');
                    my_tb.type = 'HIDDEN';
                    my_tb.name = "TYPE_OF_FOLDER";
                    my_tb.value = folder_type.innerHTML;
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
                    var postToForm = true;
                    var elem = document.getElementById('aspnetForm').elements;
                    for (var i = 0; i < elem.length; i++) {
                        postToForm = true;

                        if ((elem[i].type != 'hidden' || elem[i].name == 'hasModelFilter' || elem[i].name == 'radEventsValue') && elem[i].type != 'submit') {
                            if (elem[i].value != '') {
                                if ((elem[i].className == 'display_none' || elem[i].className == 'display_none includes')) {
                                    //ignore this because if it has these classes, then they only have one option that's defaulted
                                    //so we don't need to save these. Though there's no reason you can't,
                                    //This only clears up the saved row in the database some.
                                    //alert(elem[i].className);
                                    //However adding a catch to double check and make sure there's only 1 option.

                                    if (elem[i].length == 1) { //Must have only 1 option to not be added:
                                        postToForm = false;
                                    }
                                }

                                if (postToForm == true) {
                                    var appendMyField = true;
                                    var re = new RegExp("ctl[A-Za-z0-9]*_ContentPlaceHolder[A-Za-z0-9]_", "g");
                                    var re2 = new RegExp("ac_advanced_search_TAB[A-Za-z0-9]*_", "g");
                                    var re3 = new RegExp("ViewTMMDropDowns_", "g");
                                    var re4 = new RegExp("ac_advanced_search_company_contact_", "g");
                                    var re5 = new RegExp("ac_advanced_search_location_", "g");
                                    var re6 = new RegExp("ac_advanced_search_equip_", "g");
                                    var re7 = new RegExp("ac_advanced_search_AttrTab_", "g");
                                    var re8 = new RegExp("ContentPlaceHolder1_", "g");
                                    var rep = elem[i].id;
                                    var temp = rep.replace(re, "");
                                    //alert(elem[i].name);

                                    temp = temp.replace(re2, "");
                                    temp = temp.replace(re3, "");
                                    temp = temp.replace(re4, "");
                                    temp = temp.replace(re5, "");
                                    temp = temp.replace(re6, "");
                                    temp = temp.replace(re7, "")
                                    temp = temp.replace(re8, "")

                                    my_tb = document.createElement('INPUT');
                                    my_tb.type = 'HIDDEN';
                                    my_tb.name = temp;

                                    //If it has a checked value that's not undefined, go ahead and 
                                    //Pass that, if not, pass the value

                                    if (elem[i].type == 'checkbox') {
                                        if (elem[i].id.indexOf("ac_advanced_search_AttrTab_") >= 0) {
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
                                    } else if (elem[i].type == 'select-one') {
                                        // because of the way that the company custom build advanced search tab
                                        // uses dropdowns filled with equals/begins with that are hidden, which
                                        // get sent even if the textbox/selectbox/etc value are filled in
                                        // this checks and makes sure it has a value pair 
                                        my_tb.value = elem[i].value;
                                        if (elem[i].value != "") {
                                            if (elem[i].id.indexOf("COMPARE_") > -1) {
                                                if (elem.length >= i + 1) {
                                                    var rep2 = elem[i + 1].id;
                                                    var temp2 = rep2.replace(re, "");
                                                    temp2 = temp2.replace(re2, "");
                                                    temp2 = "COMPARE_" + temp2;
                                                    //alert(temp + " | " + temp2);
                                                    if (temp == temp2) {
                                                        if (elem[i + 1].value == "") {
                                                            my_tb.value = ""; //elem[i].value;
                                                            my_tb.name = ""; //temp;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    } else if (elem[i].type == 'radio') {

                                        my_tb.value = elem[i].checked; //elem[i].value;
                                        var n = temp.indexOf("radEventsID");
                                        //Let's set up a special case for the radio button list.
                                        //This is used during the events Type Of Search and the only radio button list we're using.
                                        //If we add another one, we can extend this to work for multiple.

                                        if (n == 0) {
                                            //This means that it's the correct radio box.
                                            //Next let's check and see if it's checked
                                            //Otherwise we honestly don't care about saving it.
                                            if (elem[i].checked == true) {
                                                //And finally, we're going to associate the value we're looking for
                                                //And with the events_type_of_search item.
                                                //I need to go ahead, in this radio button list case for the event search type
                                                //And overwrite the name of the box we're sending.
                                                my_tb.name = "radEventsID";
                                                // alert(elem[i].value + " " + temp + " : " + elem[i].checked);
                                                my_tb.value = elem[i].value;
                                            }
                                        }

                                        // alert(temp + " : " + elem[i].checked);
                                    } else {
                                        if ((rep.indexOf("ac_reg_no") >= 1)) {
                                            //Replacing = with -
                                            var oldVal = elem[i].value;
                                            var reEqual = new RegExp("=", "g"); //global find
                                            var findReplace = oldVal.replace(reEqual, "-");
                                            my_tb.value = findReplace
                                        } else {
                                            my_tb.value = elem[i].value;
                                        }
                                    }
                                    if (appendMyField == true) {
                                        my_form.appendChild(my_tb);
                                    }
                                }
                            }
                        } //end if on class
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
                    my_tb.value = folder_type.innerHTML;
                    my_form.appendChild(my_tb);
                    document.body.appendChild(my_form);
                    my_form.submit();

            }


        }

        function resetMarketTimes() {
            $("#<%= events_months.ClientID %>").val('0');
            $("#<%= event_days.ClientID %>").val('1');
            $("#<%= event_hours.ClientID %>").val('0');
            $("#<%= event_minutes.ClientID %>").val('0');
        }

        function openEstAFTTHelp() {
            load('/help/documents/612.pdf', '', '');
            return false;
        }
        function validateBetween(sender, args) {
            //So I decided that I was going to create a custom validator and add a custom attribute called data to it.
            //This stores the name of the textbox you're validating. We will use it to find the associated select box by removing the name (data attribute)
            //From the sender.controltovalidate and we'll end up with the begining string that asp.net generates for the control ID.
            //We will then be able to add COMPARE_dataname to it and we should have the select box.
            //This will allow us to check and see if the val() = 'BETWEEN' and require a : if it does.
            var senderName = sender.getAttribute("data")
            var controlToVal = sender.controltovalidate
            var selectBoxName = controlToVal.replace(senderName, "");
            selectBoxName = selectBoxName + 'COMPARE_' + senderName

            var valWeCareAbout = $("#" + selectBoxName + "").val()
            if (valWeCareAbout == 'Between') {
                if (args.Value.indexOf(":") == -1 && args.Value.indexOf(";") == -1) {
                    args.IsValid = false;
                    return false;
                }
            }
            args.IsValid = true;
            return;
        }
        function FillEventType() {

            var result = $("#cboEventsTypeCodesID option:selected").map(function () {
                return $(this).text();
            }).get().join(', ');

            $('#<%=event_type_text.ClientID%>').val(result);
        }
        function performCheck() {
            var isValid = false;

               isValid = Page_ClientValidate('Numeric');
            
            return isValid;
        }

        //function loadMasonry() {
        //    alert('test');
        //    var grid = document.querySelector('.grid');
        //    var msnry = new Masonry(grid, {
        //        itemSelector: '.grid-item',
        //        columnWidth: '.grid-item',
        //        gutter: 10,
        //        horizontalOrder: true,
        //        percentPosition: true
        //    });
        //}
    </script>

</asp:Content>
