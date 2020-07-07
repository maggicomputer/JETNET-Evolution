<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="MarketSummary.aspx.vb"
    Inherits="crmWebClient.MarketSummary" MasterPageFile="~/EvoStyles/EvoTheme.Master"
    StylesheetTheme="Evo" %>

<%@ MasterType VirtualPath="~/EvoStyles/EvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
        <ProgressTemplate>
            <span></span>
            <div class="loader">Loading...</div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <!--
  <input type="hidden" name="marketSumDirection" value="" id="marketSumDirectionID" />
  <input type="hidden" name="marketControlPanelSelection" value="" id="marketControlPanelSelectionID" />
  -->
    <asp:UpdatePanel runat="server" ID="Summary_Criteria" Visible="true" ChildrenAsTriggers="true"
        UpdateMode="Always">
        <ContentTemplate>
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td align="left" valign="top" class="dark_header" width="100%">
                        <asp:Table ID="Table1" runat="server" Width="100%" CellPadding="0" CellSpacing="0"
                            CssClass="padding_table">
                            <asp:TableRow>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="20" ID="help_text" CssClass="evoHelp">
                <a href="#" class="help_cursor" onclick="javascript:load('#','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"><img src="images/help-circle.svg" alt="Help" /></a>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="80" ID="search_expand_text">
                                    <asp:Panel ID="Control_Panel" runat="server" Width="100%">
                                        <asp:Image ID="ControlImage" runat="server" ImageUrl="../Images/search_expand.jpg" />
                                    </asp:Panel>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" ID="results_text">
                                    <asp:Label ID="criteria_results" runat="server" Text="Label"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="50" ID="sort_by_text">Sort By:</asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="70" ID="sort_by_dropdown">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="sort_dropdown" runat="server" CssClass="ul_top sort_dropdown_width">
                                            <asp:ListItem>Model/Ser#</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="sort_submenu_dropdown" runat="server" CssClass="ul_bottom sort_dropdown"
                                            DisplayMode="LinkButton">
                                            <asp:ListItem>Model/Ser#</asp:ListItem>
                                            <asp:ListItem>List Date</asp:ListItem>
                                            <asp:ListItem>AFTT</asp:ListItem>
                                            <asp:ListItem>Status</asp:ListItem>
                                        </asp:BulletedList>
                                    </div>
                                </asp:TableCell>
                                <asp:TableCell HorizontalAlign="left" VerticalAlign="middle" Width="75" ID="action_dropdown">
                                    <div class="action_dropdown_container">
                                        <asp:BulletedList ID="actions_dropdown" runat="server" CssClass="ul_top">
                                            <asp:ListItem>Actions</asp:ListItem>
                                        </asp:BulletedList>
                                        <asp:BulletedList ID="actions_submenu_dropdown" runat="server" CssClass="ul_bottom market_dropdown_width"
                                            DisplayMode="HyperLink">
                                            <asp:ListItem Value="javascript:SubMenuDrop(3,0, 'MARKET SUMMARIES');">Save As - New Folder</asp:ListItem>
                                            <asp:ListItem Value="javascript:SubMenuDrop(6,5,market_selection);">Aircraft Upgrade TO Report</asp:ListItem>
                                            <asp:ListItem Value="javascript:SubMenuDrop(6,12,market_selection);">Aircraft Upgrade FROM Report</asp:ListItem>
                                            <asp:ListItem Value="javascript:SubMenuDrop(5,0,'MARKET SUMMARY');">JETNET Export/Report</asp:ListItem>
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
                                            Visible="false" />&#160;
                    <asp:ImageButton ID="previous" ImageUrl="../images/previous_listing.png" Visible="false"
                        runat="server" />&#160;
                    <asp:Label ID="record_count" runat="server">Showing 25 - 50</asp:Label>&#160;
                    <asp:ImageButton ID="next_" ImageUrl="../images/next_listing.png" runat="server" />&#160;
                    <asp:ImageButton ID="next_all" ImageUrl="~/images/next_all.png" runat="server" /></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </td>
                </tr>
            </table>
            <cc1:CollapsiblePanelExtender ID="PanelCollapseEx" runat="server" TargetControlID="Collapse_Panel"
                Collapsed="true" ExpandControlID="Control_Panel" ImageControlID="ControlImage"
                CollapsedText="New Search" ExpandedText="Hide Search" ExpandedImage="../Images/search_collapse.jpg"
                CollapsedImage="../Images/search_expand.jpg" CollapseControlID="Control_Panel"
                Enabled="True">
            </cc1:CollapsiblePanelExtender>
            <asp:Panel ID="Collapse_Panel" runat="server" Height="0px" Width="100%" CssClass="collapse">
                <asp:Label runat="server" ID="close_current_folder" Font-Bold="true" ForeColor="Red"
                    Visible="false"><br /><br /><p align="center" class="medium_text">You must Close Current Folder before starting a New Search.</p><br /><br /></asp:Label>
                <asp:Table ID="Table2" Width="100%" CellPadding="3" CellSpacing="0" runat="server">
                    <asp:TableRow>
                        <asp:TableCell Width="33%" HorizontalAlign="Left" VerticalAlign="Top" CssClass="model_search_box"
                            RowSpan="2">
                            <asp:Panel runat="server" ID="model_search_box" CssClass="makeStyle">
                                <div id="ProdWarningID" style="visibility: hidden; text-align: center; width: 100%;">
                                    <font color="Red" size="2.5"><b>Please select only one aircraft product to perform searches</b></font>
                                </div>
                                <evo:viewTMMDropDowns ID="ViewTMMDropDowns" runat="server" />

                                <script language="javascript" type="text/javascript">

                                    marketSearchButton = "<%= summary_search.ClientID.Trim%>";                             
                 refreshTypeMakeModelByCheckBox("", "", <%= isHeliOnlyProduct.tostring.tolower%>,<%= productCodeCount.tostring%>);

                                </script>
                            </asp:Panel>
                        </asp:TableCell>
                        <asp:TableCell Width="67%" HorizontalAlign="Left" VerticalAlign="top">
                            <asp:Panel runat="server" ID="summary_search_box" CssClass="summary_type_search_box">
                                <asp:Table runat="server" ID="summary_type" CellPadding="2" CellSpacing="0" Width="100%">
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                            <div id="WarningID" style="visibility: hidden; text-align: center; width: 100%;">
                                                <font color="Red" size="2.5"><b>You Must Select The Type Of Summary</b></font>
                                            </div>
                                            <asp:CheckBox ID="chkAvailableID" runat="server" Checked="true" Text="Market Summary <em>(Aircraft Available For Sale)</em>" />&nbsp;&nbsp;
                      <asp:CheckBox ID="chkTransactionsID" runat="server" Checked="true" Text="Market Activity Summary <em>(Transactions)</em>" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top">
                                            <evo:marketPickDate ID="MarketSummaryPickDateID" runat="server" />

                                            <script language="javascript" type="text/javascript">
                         fillStartDateJS("", <%= isHeliOnlyProduct.tostring.tolower%>,<%= isBusinessOnlyProduct.tostring.tolower%>,<%= isCommercialOnlyProduct.tostring.tolower%>);
                                            </script>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                            <asp:Panel runat="server" ID="transaction_search_box" CssClass="transaction_search_box remove_margin">
                                <br />
                                <asp:Table runat="server" ID="transaction_search" CellPadding="2" CellSpacing="0"
                                    Width="100%">
                                    <asp:TableRow>
                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="40px">Type:</asp:TableCell>
                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="110px">
                                            <asp:ListBox ID="transaction_type_lb" runat="server" SelectionMode="Multiple" Rows="5">
                                                <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                                <asp:ListItem Value="WS">Whole</asp:ListItem>
                                                <asp:ListItem Value="FS">Fractional</asp:ListItem>
                                                <asp:ListItem Value="SS">Share</asp:ListItem>
                                                <asp:ListItem Value="DP">Delivery Position</asp:ListItem>
                                                <asp:ListItem Value="LA, LO, LT">Leases</asp:ListItem>
                                                <asp:ListItem Value="FC">Foreclosures</asp:ListItem>
                                                <asp:ListItem Value="SZ">Seizures</asp:ListItem>
                                                <asp:ListItem Value="OM">Off Markets</asp:ListItem>
                                                <asp:ListItem Value="MA">On Markets</asp:ListItem>
                                            </asp:ListBox>
                                        </asp:TableCell>
                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Width="90px">
                                            <asp:DropDownList ID="transaction_from" runat="server" Width="99%">
                                                <asp:ListItem Value="from" Selected="True">From</asp:ListItem>
                                                <asp:ListItem Value="not from">Not From</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:TextBox ID="folder_name" runat="server" CssClass="display_none"></asp:TextBox>
                                        </asp:TableCell>
                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="left">
                                            <asp:ListBox ID="transaction_from_lb" runat="server" SelectionMode="Multiple" Rows="5"
                                                Width="99%">
                                                <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                                <asp:ListItem Value="CH">Charter Company</asp:ListItem>
                                                <asp:ListItem Value="DB">Dealer</asp:ListItem>
                                                <asp:ListItem Value="DS">Distributor</asp:ListItem>
                                                <asp:ListItem Value="EU">Retail</asp:ListItem>
                                                <asp:ListItem Value="FB">Fixed Base Operator</asp:ListItem>
                                                <asp:ListItem Value="FI">Financial Institution</asp:ListItem>
                                                <asp:ListItem Value="LS">Leasing Company</asp:ListItem>
                                                <asp:ListItem Value="MC">Management Company</asp:ListItem>
                                                <asp:ListItem Value="MF">Manufacturer</asp:ListItem>
                                                <asp:ListItem Value="FY">Ferrying Company</asp:ListItem>
                                                <asp:ListItem Value="RE">Reverse Exchange Company</asp:ListItem>
                                                <asp:ListItem Value="FB">Program Holder</asp:ListItem>
                                                <asp:ListItem Value="AD">Awaiting Documentation</asp:ListItem>
                                                <asp:ListItem Value="AL">Airlines</asp:ListItem>
                                            </asp:ListBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="left" Wrap="false" ColumnSpan="2">
                                            <asp:CheckBox ID="chkNewToMarketID" runat="server" Text="Sales of New Aircraft Only" /><br />
                                            <asp:CheckBox ID="chkUsedMarketID" runat="server" Text="Sales of Used Aircraft Only" />
                                        </asp:TableCell>
                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="left">
                                            <asp:DropDownList ID="transaction_to" runat="server" Width="99%">
                                                <asp:ListItem Value="to" Selected="True">To</asp:ListItem>
                                                <asp:ListItem Value="not to">Not To</asp:ListItem>
                                            </asp:DropDownList>
                                        </asp:TableCell>
                                        <asp:TableCell VerticalAlign="Top" HorizontalAlign="left">
                                            <asp:ListBox ID="transaction_to_lb" runat="server" SelectionMode="Multiple" Rows="5" Width="99%">
                                                <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                                <asp:ListItem Value="CH">Charter Company</asp:ListItem>
                                                <asp:ListItem Value="DB">Dealer</asp:ListItem>
                                                <asp:ListItem Value="DS">Distributor</asp:ListItem>
                                                <asp:ListItem Value="EU">Retail</asp:ListItem>
                                                <asp:ListItem Value="FB">Fixed Base Operator</asp:ListItem>
                                                <asp:ListItem Value="FI">Financial Institution</asp:ListItem>
                                                <asp:ListItem Value="LS">Leasing Company</asp:ListItem>
                                                <asp:ListItem Value="MC">Management Company</asp:ListItem>
                                                <asp:ListItem Value="MF">Manufacturer</asp:ListItem>
                                                <asp:ListItem Value="FY">Ferrying Company</asp:ListItem>
                                                <asp:ListItem Value="RE">Reverse Exchange Company</asp:ListItem>
                                                <asp:ListItem Value="FB">Program Holder</asp:ListItem>
                                                <asp:ListItem Value="IT">Internal</asp:ListItem>
                                                <asp:ListItem Value="AD">Awaiting Documentation</asp:ListItem>
                                                <asp:ListItem Value="AL">Airlines</asp:ListItem>
                                            </asp:ListBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:Panel>
                            <asp:Panel runat="server" DefaultButton="summary_search" ID="summarySearchButton"
                                CssClass="display_block search_seperator" BackColor="#dce3e8" Width="100%" HorizontalAlign="Right">
                                <asp:Button ID="summary_search" runat="server" Text="Search" CssClass="button-darker button_width"
                                    UseSubmitBehavior="false" /><br />
                                <asp:Button ID="reset" runat="server" Text="Clear Selections" CssClass="font-weight-normal button_width" />
                                <div class="div_clear">
                                </div>
                            </asp:Panel>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
            <asp:Label runat="server" ID="FolderInformation" Visible="false" CssClass="FolderNameBar help_cursor"></asp:Label>
            <asp:Panel ID="summary_control_block" runat="server" Visible="false">
                <div style="width: 100%; overflow: auto; vertical-align: top;">
                    <asp:Table ID="summary_control_panel_table" BorderColor="#949494" BorderStyle="Solid"
                        BorderWidth="1" CellPadding="2" CellSpacing="0" GridLines="None" Width="100%" runat="server">
                        <asp:TableRow ID="scp_tr_1" CssClass="header_row">
                            <asp:TableCell HorizontalAlign="Center" VerticalAlign="Middle" Width="100%"><font size="2.5"><b>MARKET SUMMARY - CONTROL PANEL</b></font>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="scp_tr_2">
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Middle" Width="100%">
                                Display&nbsp;<asp:Label ID="simpleReportTitle" runat="server" Visible="true" Text=""></asp:Label>&nbsp;during&nbsp;a&nbsp;<asp:DropDownList
                                    ID="range_span_DropDownList" runat="server" AutoPostBack="true">
                                </asp:DropDownList>
                                &nbsp;
                <asp:DropDownList ID="time_scale_DropDownList" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="year" Value="years"></asp:ListItem>
                    <asp:ListItem Text="quarter" Value="quarters"></asp:ListItem>
                    <asp:ListItem Text="month" Value="months"></asp:ListItem>
                </asp:DropDownList>
                                &nbsp;timeframe&nbsp;starting&nbsp;on&nbsp;
                <asp:DropDownList ID="start_date_DropDownList" runat="server" AutoPostBack="true">
                </asp:DropDownList>
                                <!--
                &nbsp;or&nbsp;
                <asp:LinkButton ID="marketSumDirectionPrevious" runat="server" Text="shift time span back one" PostBackUrl="" OnClientClick="javascript:setPrevious();"></asp:LinkButton>
                <asp:LinkButton ID="marketSumDirectionNext" runat="server" Text=" or shift time span forward one" PostBackUrl="" OnClientClick="javascript:setNext();"></asp:LinkButton>
                -->
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow ID="scp_tr_3">
                            <asp:TableCell HorizontalAlign="Left" VerticalAlign="Middle" Width="100%">
                                <br />
                                <asp:Label ID="hasTransactionSummary" runat="server" Visible="true" Text=""></asp:Label>&nbsp;a
                report of market activity (transactions) summarized by&nbsp;
                <asp:DropDownList ID="summary_type_DropDownList" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="Seller/Purchaser" Value="trans_type"></asp:ListItem>
                    <asp:ListItem Text="Purchaser" Value="trans_destination"></asp:ListItem>
                    <asp:ListItem Text="Seller" Value="trans_source"></asp:ListItem>
                </asp:DropDownList>
                                &nbsp;including&nbsp;
                <asp:ListBox ID="tx_types_DropDownList" runat="server" AutoPostBack="true" SelectionMode="Multiple"></asp:ListBox>
                                &nbsp;transactions.
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </div>
                <hr width='100%' size='1' style='margin: 10px 0px 10px 0px;'>
            </asp:Panel>
            <asp:Panel ID="available_summary_block" runat="server" Visible="false">
                <asp:Literal ID="available_summary" runat="server" Visible="true" Text=""></asp:Literal>
            </asp:Panel>
            <asp:Panel ID="transaction_summary_block" runat="server" Visible="false">
                <asp:Literal ID="transaction_summary" runat="server" Visible="true" Text=""></asp:Literal>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:CheckBox ID="logo_check" runat="server" Visible="False" Checked="True" Text=" Include My Company Logo in Header of Report " ToolTip="Include Logo" />
    <script type="text/javascript">



</script>
</asp:Content>
