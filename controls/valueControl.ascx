<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="valueControl.ascx.vb"
    Inherits="crmWebClient.valueControl" %>
<script type="text/javascript" src="https://cdn.rawgit.com/Mikhus/canvas-gauges/gh-pages/download/2.1.4/all/gauge.min.js"></script>

<style>
    .mainHeading.padded_left {
        padding-left: 3px;
    }

    .CLIENTCRMRow, .CLIENTCRMRow.even, .DTFC_LeftBodyWrapper .CLIENTCRMRow td {
        padding: 5px;
        margin: -4px;
        width: 100%;
        display: block;
    }

    .searchPanelContainerDiv .valueSpec.Simplistic .Box {
        margin: auto !important;
    }

    .detailsTab {
        height: 200px;
        overflow-y: auto;
        overflow-x: hidden;
        padding: 0px 5px 0px 10px;
    }

    #closeFolderListingButton {
        display: none;
    }

    .valueTables {
        width: 978px;
    }

        .valueTables .startTable_wrapper, .valueTables .transactionTable_wrapper {
            display: none;
        }

    .refreshGraph {
        margin-right: 25px !important;
    }

    .valueTables .DTFC_LeftBodyWrapper {
        max-height: none !important;
    }

    .valueTables .DTFC_LeftBodyLiner {
        height: auto !important;
    }

    .valueTables th {
        text-align: left !important;
    }

    .valueTables .dataTables_scrollHead {
        width: 100% !important;
    }

    .detailsTab .negMarginTop {
        margin-top: -10px;
    }

    .detailsTab .clearAircraftButton {
        font-size: 9px;
        margin: 0px 5px 5px 0px;
        clear: right;
    }

    .detailsTab div {
        margin: 0px !important;
    }

    .detailsTab .green_text {
        color: #5f9e3a !important;
    }

    .detailsTab .pictureInfo {
        width: 96%;
        padding: 5px;
    }

    .detailsTab .borderTopSep {
        border-top: 2px solid rgb(234, 234, 234);
        padding-top: 5px;
    }

    .modal {
        display: none;
        position: fixed;
        z-index: 1000;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        background: rgba( 255, 255, 255, .8 );
    }

    .loadingTextStyle {
        display: none;
        position: fixed;
        z-index: 900000;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        text-align: center;
        font-size: 16px;
        font-weight: bold;
        vertical-align: middle;
        opacity: 6 !important;
    }
    /* When the body has the loading class, we turn
   the scrollbar off with overflow:hidden */ body.loading {
        overflow: hidden;
    }
        /* Anytime the body has the loading class, our
   modal element will be visible */ body.loading .modal, body.loading .loadingTextStyle {
          /*  display: block;*/
        }

            body.loading .loadingTextStyle .display_block {
                margin: 9px;
            }

            body.loading .loadingTextStyle div {
                position: absolute;
                top: 20%;
                left: 35%;
            }

    .valueTables table.dataTable thead th, .valueTables table.dataTable thead td {
        padding: 5px;
    }


    .dataTables_info {
        margin-right: 15px !important;
        font-size: .8em !important;
    }

    .dataTables_length {
        margin-top: 6px !important;
    }

    .dataTables_filter {
        display: none;
    }

    .dataTables_paginate {
        float: right !important;
    }

    dataTables .select-info {
        display: none !important;
    }

    .valueSummary label {
        margin-left: 5px;
    }

    .subtextNoMargin tr td:first-child {
        width: 110px;
    }

    .formatTable.large, .formatTable.large .sub_text {
        font-size: 14px;
    }

    .formatTable .greenText {
        color: #509c23 !important;
    }

    .valueSpec.Simplistic .subHeader {
        font-size: 14px;
    }

    .valueSpec.aircraftSpec.Simplistic {
        min-height: auto;
    }

    .bx-wrapper {
        margin-top: -8px !important;
    }

        .bx-wrapper .bx-controls-direction a {
            top: 73% !important;
        }

    .bx-wrapper {
        max-height: 250px !important;
    }

    .green_text {
        color: #429810 !important;
    }

    .searchPanelContainerDiv .graphS .ajax__tab_panel {
        margin-right: -11px;
        padding: 3px;
    }

    .searchPanelContainerDiv .graphS .valueSpec.Simplistic .Box {
        width: 98% !important;
        margin-top: 15px;
    }

    .searchPanelContainerDiv .graphS .valueSpec.aircraftSpec.Simplistic {
        padding-top: 6px;
        padding-bottom: 5px;
    }

    .maxWidthValue {
        max-width: 50% !important;
        width: 50% !important;
        display: inline-block;
    }

    .searchPanelContainerDiv .five.columns.disableHeaders {
        width: 34%;
    }

    .searchPanelContainerDiv .header_row .amount {
        height: 25px;
    }

    .searchPanelContainerDiv .seven.columns.remove_margin {
        margin-left: 2% !important;
        width: 64%;
    }

    .searchPanelContainerDiv .viewBoxMargin .subHeader {
        margin-bottom: -15px;
        font-size: 13px;
        padding-top: 3px;
        padding-left: 3px;
    }

    .dataTables_scrollHead {
        width: auto !important;
    }

    .fullWidth {
        width: 100% !important;
    }

    #salePriceGauge, #evaluePriceGauge {
        width: 180px
    }

    @media (max-width: 440px) {
        .searchBox, .welcome_text {
            display: none;
        }

        .valueTabs .row .dark-theme {
            width: 90% !important;
            max-width: 400px;
        }

        .searchPanelContainerDiv .five.columns.disableHeaders {
            width: 100%;
        }

        .searchPanelContainerDiv .seven.columns.remove_margin {
            margin-left: 1% !important; /* width: 82%;*/
            width: 100%;
            max-width: 100%;
            clear: both;
        }
    }
</style>
<link rel="stylesheet" href="/abiFiles/css/jquery.bxslider.css" type="text/css" />

<script type="text/javascript" src="/abiFiles/js/jquery.bxslider.min.js"></script>

<asp:Label runat="server" ID="loadingTextContainer" CssClass="loadingScreenBox" Style="display: none;">
    <span></span>
    <div class="loader">Loading...</div>
    <asp:Label ID="loadingText" runat="server" CssClass="display_none"
        Text="false"></asp:Label>
</asp:Label>
<script>
     google.charts.load('current', { 'packages': ['corechart', 'table'] });
</script>
 <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='100%' runat="server"
    class="DetailsBrowseTable">
    <asp:TableRow>
      <asp:TableCell HorizontalAlign="right" VerticalAlign="middle">
            <div class="backgroundShade">              <asp:Literal runat="server" ID="buttons"></asp:Literal>
                <a href="#" onclick="javascript:window.close();" class="gray_button float_left noBefore"><img src="images/x.svg" alt="Help" /></a>
              </div>
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
<div class="row  remove_margin valueTabs">
    <asp:Panel ID="Collapse_Panel" runat="server" CssClass="display_inline">
        <asp:Label runat="server" ID="close_current_folder" Font-Bold="true" ForeColor="Red"
            Visible="false"><br /><br /><p align="center" class="medium_text">You must Close Current Folder before starting a New Search.</p><br /><br /></asp:Label>
        <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
            <ProgressTemplate>
                <span></span>
                <div class="loader">Loading...</div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:Table ID="Table2" Width="100%" CellPadding="0" CellSpacing="0" runat="server">
            <asp:TableRow>
                <asp:TableCell HorizontalAlign="Left" VerticalAlign="Top" CssClass="remove_margin">
                    <div class="row searchPanelContainerDiv">
                        <div class="five columns disableHeaders maxwidth">
                            <cc1:TabContainer ID="tabs_top_left" runat="server" Width="100%" CssClass="dark-theme"
                                Height="200px">
                                <cc1:TabPanel ID="tabs_top_left_1" runat="server">
                                    <HeaderTemplate>
                                        <asp:Label runat="server" ID="tabs_top_left_1_header">My Aircraft</asp:Label>
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Panel runat="server" ID="searchPanelToggle" CssClass="valuesSearchPanel">
                                            <asp:UpdatePanel runat="server" ID="modelUpdatePanel" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:TextBox runat="server" ID="acIDText" CssClass="display_none"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="variantThere" CssClass="display_none"></asp:TextBox>
                                                    <div class="row">
                                                        <div class="three columns removeLeftMargin displayNoneMobile">
                                                            <label>
                                                                Model:</label>
                                                        </div>
                                                        <div class="nine columns">
                                                            <asp:DropDownList runat="server" Width="102%" ID="modelList" CssClass="chosen-select"
                                                                AutoPostBack="true" data-placeholder="Please Pick a Model">
                                                            </asp:DropDownList>
                                                            <div class="mobile_display_on_cell mobileChosenSpacer">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div class="row div_clear toggleSmallScreen">
                                                <div class="three columns removeLeftMargin">
                                                    <label>
                                                        DLV Year(s):</label>
                                                </div>
                                                <div class="two columns">
                                                    <asp:TextBox runat="server" ID="hiddenYear_start" CssClass="display_none"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="year_start" CssClass="amount float_right"></asp:TextBox>
                                                </div>
                                                <div class="five columns">
                                                    <div id="slider-range">
                                                    </div>
                                                </div>
                                                <div class="two columns">
                                                    <asp:TextBox runat="server" ID="hiddenYear_end" CssClass="display_none"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="year_end" CssClass="amount float_left"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row toggleSmallScreen">
                                                <div class="three columns removeLeftMargin">
                                                    <label>
                                                        AFTT:</label>
                                                </div>
                                                <div class="two columns">
                                                    <asp:TextBox runat="server" ID="hiddenAftt_start" CssClass="display_none"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="aftt_start" CssClass="amount float_right"></asp:TextBox>
                                                </div>
                                                <div class="five columns">
                                                    <div id="aftt-range">
                                                    </div>
                                                </div>
                                                <div class="two columns">
                                                    <asp:TextBox runat="server" ID="hiddenAftt_end" CssClass="display_none"></asp:TextBox>
                                                    <asp:TextBox runat="server" ID="aftt_end" CssClass="amount float_left"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="row toggleSmallScreen">
                                                <div class="four columns removeLeftMargin">
                                                    <label>
                                                        Registration:</label>
                                                </div>
                                                <div class="four columns removeLeftMargin">
                                                    <asp:DropDownList ID="aircraft_registration" runat="server" CssClass="chosen-select"
                                                        Width="100%">
                                                        <asp:ListItem Text="US (Domestic)" Value="N"></asp:ListItem>
                                                        <asp:ListItem Text="International" Value="I"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="Worldwide" Value="Worldwide"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <div class="mobile_display_on_cell mobileChosenSpacer">
                                                    </div>
                                                </div>
                                                <div class="four columns">
                                                    <label>
                                                        &nbsp;</label>
                                                </div>
                                                <div class="four columns removeLeftMargin">
                                                    <asp:UpdatePanel runat="server" ID="loadWhatUpdate" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="loadWhatAC" runat="server" CssClass="chosen-select" Width="100%"
                                                                AutoPostBack="true" onchange="$('body').addClass('loading');">
                                                                <asp:ListItem Text="For Sale Market" Value="Y"></asp:ListItem>
                                                                <asp:ListItem Text="All In Operation Aircraft" Value="All" Selected="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="mobile_display_on_cell mobileChosenSpacer">
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </div>
                                            <div class="row toggleSmallScreen">
                                                <asp:Label runat="server" ID="evaluesTextAircraft" Visible="false"></asp:Label>
                                            </div>
                                            <div class="row removeMarginBottom">
                                                <div class="sevenhalf columns removeLeftMargin">
                                                    <asp:Label runat="server" ID="variantModelText" CssClass="display_none" ForeColor="Red">
                  *Variant Models Loaded, Including: </asp:Label>
                                                </div>
                                                <div class="clearfix">
                                                </div>
                                                <asp:Label runat="server" ID="FolderInformation" Visible="false" CssClass="FolderNameBar help_cursor"></asp:Label>
                                                <asp:Label runat="server" ID="StaticFolderNewSearchLabel"></asp:Label>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tabs_top_left_2" runat="server" HeaderText="Details" Visible="false">
                                    <ContentTemplate>
                                        <div class="valueSpec Simplistic aircraftSpec viewBoxMargin">
                                            <div class="detailsTab">
                                                <div class="flex-even">
                                                    <asp:LinkButton runat="server" ID="viewAircraft" Visible="true" CssClass="float_left clearAircraftButton">View Selected Aircraft</asp:LinkButton>
                                                    <asp:LinkButton runat="server" ID="removeAircraft" Visible="true" CssClass="float_left clearAircraftButton">Clear Selected Aircraft</asp:LinkButton>
                                                </div>
                                                <asp:Label ID="aircraft_information" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="status_information" runat="server" Text=""></asp:Label>
                                                <div class="Box">
                                                    <div class="row">
                                                        <asp:Label ID="picture_information" runat="server" Text=""></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tabs_top_left_3" runat="server" HeaderText="Variants">
                                    <ContentTemplate>
                                        <div>
                                            <div class="row">
                                                <asp:UpdatePanel runat="server" ID="variantUpdatePanel">
                                                    <ContentTemplate>
                                                        <asp:ListBox runat="server" Width="100%" ID="VariantList" data-placeholder="Please Pick Model(s)"
                                                            AutoPostBack="false" SelectionMode="Multiple" Height="167px"></asp:ListBox>
                                                        <asp:Button runat="server" ID="runVariants" Text="Include Variants" OnClientClick="$('body').addClass('loading');" />
                                                        <asp:Button runat="server" ID="removeVariants" Text="Remove Variants" CssClass="display_none"
                                                            OnClientClick="$('body').addClass('loading');" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>
                        <div class="seven columns remove_margin">
                            <asp:DropDownList ID="acKeepRemove" runat="server" CssClass="float_right display_none"
                                Width="100%">
                                <asp:ListItem Value="keep">keep</asp:ListItem>
                                <asp:ListItem Selected="True" Value="remove">remove</asp:ListItem>
                            </asp:DropDownList>
                            <cc1:TabContainer ID="tabs_top_right" runat="server" Width="100%" CssClass="dark-theme graphS"
                                Height="260px">
                                <cc1:TabPanel ID="tabs_top_right_1" runat="server" HeaderText="Search Summary">
                                    <HeaderTemplate>
                                        Value Summary
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="valueSliderGraphUpdate" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="bxslider" runat="server" id="sliderBX">
                                                    <div class="child">
                                                        <div class="valueSpec Simplistic aircraftSpec viewBoxMargin ContainerBoxSummary">
                                                            <asp:UpdatePanel runat="server" ID="tabs_top_right_1_update_panel" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <img src="/images/valuesLogo.png" class="valueLogo" />
                                                                    <div class="Box">
                                                                        <div class="valueSummary formatTable blue airframeTable">
                                                                            <div class="row removeLeftMargin header_row">
                                                                                <div class="columns three removeLeftMargin dateColumn">
                                                                                    <asp:TextBox runat="server" ID="dateValueText" TextMode="MultiLine" Rows="1" CssClass="amount mobileAlignLeft mobileDateHeight label gray"
                                                                                        Enabled="false">&nbsp;</asp:TextBox>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <span class="label gray">LOW ($K)</span>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <span class="label gray">AVG ($K)</span>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <span class="label gray">HIGH ($K)</span>
                                                                                </div>
                                                                                <div class="columns two removeLeftMargin text-right">
                                                                                    <span class="label gray">#AC</span>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row removeLeftMargin">
                                                                                <div class="columns three removeLeftMargin  dateColumn">
                                                                                    <b>Aircraft on Market</b>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin">
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin">
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin">
                                                                                </div>
                                                                                <div class="columns two removeLeftMargin">
                                                                                </div>
                                                                            </div>
                                                                            <div class="row removeLeftMargin">
                                                                                <div class="columns three removeLeftMargin dateColumn">
                                                                                    <label>
                                                                                        Asking<span class="displayNoneMobile"> Price</span></label>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="lowest_aircraft_on_market" CssClass="amount float_right"></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="average_aircraft_on_market" CssClass="amount float_right"></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="highest_aircraft_on_market" CssClass="amount float_right"></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns two removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="count_aircraft_on_market" CssClass="amount float_right"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row removeLeftMargin">
                                                                                <div class="columns three removeLeftMargin displayBlockMobile dateColumn">
                                                                                    <b>Aircraft Sales</b>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin">
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin">
                                                                                </div>
                                                                                <div class="columns two removeLeftMargin">
                                                                                </div>
                                                                                <div class="columns two removeLeftMargin">
                                                                                </div>
                                                                            </div>
                                                                            <div class="row removeLeftMargin">
                                                                                <div class="columns three removeLeftMargin dateColumn">
                                                                                    <label>
                                                                                        Asking<span class="displayNoneMobile"> Price</span></label>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="lowest_asking_sales" CssClass="amount float_right"></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="average_asking_sales" CssClass="amount float_right"></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="highest_asking_sales" CssClass="amount float_right"></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns two removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="count_asking_sales" CssClass="amount float_right"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row removeLeftMargin">
                                                                                <div class="columns three removeLeftMargin dateColumn">
                                                                                    <label class=" red_text">
                                                                                        Sale Price</label>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="lowest_sale_aircraft_sales" CssClass="amount float_righ red_text"></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="average_sale_aircraft_sales" CssClass="amount float_right red_text"></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="highest_sale_aircraft_sales" CssClass="amount float_right red_text"></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns two removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="count_sale_aircraft_sales" CssClass="amount float_right red_text"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class='row removeLeftMargin evalue_blue' runat="server" id="avgEvaluesRow" visible="false">
                                                                                <div class="columns three removeLeftMargin dateColumn" runat="server" id="evalues1" visible="false">
                                                                                    <a href="javascript:void(0);" title="Asset Insight Estimated Value - Click to Learn More"
                                                                                        class='text_underline'
                                                                                        onclick="javascript:load('/help/documents/809.pdf','','scrollbars=yes,menubar=no,height=900,width=1150,resizable=yes,toolbar=no,location=no,status=no');">
                                                                                        <b>
                                                                                            <%= crmWebClient.Constants.eValues_Refer_Name %></b></a>
                                                                                </div>
                                                                                <div runat="server" id="evalues2" visible="false" class="columns three removeLeftMargin text-right">
                                                                                    <asp:TextBox runat="server" ID="evalues_low" CssClass='amount float_right'></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right" runat="server" id="evalues3" visible="false">
                                                                                    <asp:TextBox runat="server" ID="evalues_avg" CssClass='amount float_right'></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns three removeLeftMargin text-right" runat="server" id="evalues4" visible="false">
                                                                                    <asp:TextBox runat="server" ID="evalues_high" CssClass='amount float_right'></asp:TextBox>
                                                                                </div>
                                                                                <div class="columns two removeLeftMargin text-right cursor"
                                                                                    onclick="$find('<%= tabs_bottom.ClientID %>').set_activeTabIndex(8);" runat="server" id="evalues5" visible="false">
                                                                                    <asp:TextBox runat="server" ID="evalues_count" CssClass='amount float_right' Style="text-decoration: underline; cursor: pointer;"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="clear"></div>
                                                                            <br class="clear" />
                                                                            <asp:Button ID="valueSummaryRefreshButton" runat="server" Text="Refresh Values" CssClass="float_right"
                                                                                Style="display: none;" OnClick="RunValueVintageTabClick" OnClientClick="clearBoxes();$('body').addClass('loading');" />
                                                                        </div>
                                                                        <div class="optionalBox" runat="server" id="optionalEvaluesBox" visible="false">
                                                                            <div class="Box removeLeftMargin" style="height: 180px; overflow: hidden">
                                                                                <table cellpadding="0" cellspacing="0" class="formatTable blue large" width="100%">
                                                                                    <tr class="noBorder">
                                                                                        <td align="left" valign="top"><span class="subHeader">Sale Price Average</span></td>
                                                                                        <td align="left" valign="top"><span class="subHeader">Evalue Average</span></td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <canvas id="salePriceGauge"></canvas>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <canvas id="evaluePriceGauge"></canvas>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </div>
                                                                        </div>
                                                                        <br clear="all" />
                                                                        <br clear="all" />
                                                                    </div>
                                                                </ContentTemplate>
                                                                <Triggers>
                                                                    <asp:AsyncPostBackTrigger ControlID="valuesByYearVintageButton" />
                                                                </Triggers>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                    <!--slide1 child-->
                                                    <div runat="server" id="slide2" class="child" visible="false">
                                                        <asp:Literal runat="server" ID="topTabCurrentMarketValuation"></asp:Literal>
                                                    </div>
                                                    <!--slide2 child-->
                                                    <div runat="server" id="slide3" class="child" visible="false">
                                                        <asp:Literal runat="server" ID="topTabValuationByMFRYear"></asp:Literal>
                                                    </div>
                                                    <!--slide3 child-->
                                                    <div class="child" runat="server" id="slide4" visible="false">
                                                        <asp:Literal runat="server" ID="topTabValuationByMonth"></asp:Literal>
                                                    </div>
                                                    <!--slide4 child-->
                                                    <div class="child" runat="server" id="slide5" visible="false">
                                                        <asp:Literal runat="server" ID="topTabValuationByAFTT"></asp:Literal>
                                                    </div>
                                                    <!--slide5 child-->
                                                    <div class="child" runat="server" id="slide6" visible="false">
                                                        <asp:Literal runat="server" ID="topTabResidualValues"></asp:Literal>
                                                    </div>
                                                    <!--slide6 child-->
                                                </div>
                                                <!--bxslider-->
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tabs_top_right_2" runat="server" HeaderText="My Sales">
                                    <ContentTemplate>
                                        <div class="valueSpec Simplistic aircraftSpec viewBoxMargin ">
                                            <div class="Box">
                                                <br />
                                                <div class="removeLeftMargin">
                                                    <div class="row">
                                                        <div class="three columns removeLeftMargin">
                                                            Start Date:
                                                        </div>
                                                        <div class="three columns removeLeftMargin mobile_top_padding">
                                                            <asp:TextBox runat="server" ID="start_date" CssClass="" Width="70px" Style="margin-top: -2px;"></asp:TextBox>
                                                        </div>
                                                        <div class="three columns removeLeftMargin">
                                                            <span class="float_right mobile_float_left mobile_top_padding">End Date:</span>
                                                        </div>
                                                        <div class="three columns removeLeftMargin mobile_top_padding">
                                                            <asp:TextBox runat="server" ID="end_date" CssClass="" Width="70px" Style="margin-top: -12px;"></asp:TextBox>
                                                        </div>
                                                        <div class="seven columns display_none">
                                                            <span class="float_left">Start Date:</span>
                                                            <div style="width: 225px; margin-left: 106px; display: none;">
                                                                <div id="date_slider">
                                                                </div>
                                                            </div>
                                                            <span class="float_left removeLeftMargin">End Date:</span>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="three columns removeLeftMargin">
                                                            New/Used:
                                                        </div>
                                                        <div class="six columns removeLeftMargin">
                                                            <asp:DropDownList runat="server" ID="newUsed" CssClass="chosen-select" Width="100%">
                                                                <asp:ListItem Value="N">New (First Owner)</asp:ListItem>
                                                                <asp:ListItem Value="U" Selected="True">Used/Pre-Owned</asp:ListItem>
                                                                <asp:ListItem Value="">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="mobile_display_on_cell mobileChosenSpacer">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="three columns removeLeftMargin">
                                                            Market:
                                                        </div>
                                                        <div class="six columns removeLeftMargin">
                                                            <asp:DropDownList ID="ac_market" runat="server" CssClass="chosen-select float_right"
                                                                Width="100%">
                                                                <asp:ListItem Value="All" Selected="True">All</asp:ListItem>
                                                                <asp:ListItem Value="Y">For Sale</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="mobile_display_on_cell mobileChosenSpacer">
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="three columns removeLeftMargin">
                                                            Display:
                                                        </div>
                                                        <div class="six columns removeLeftMargin">
                                                            <asp:DropDownList runat="server" ID="salePriceDropdown" CssClass="chosen-select"
                                                                Width="100%">
                                                                <asp:ListItem Value="W">Only Sales with Sale Prices</asp:ListItem>
                                                                <asp:ListItem Value="O">Only Sales without Sale Prices</asp:ListItem>
                                                                <asp:ListItem Value="" Selected="True">All Sales</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <div class="mobile_display_on_cell mobileChosenSpacer">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br clear="all" />
                                                <br />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tabs_top_right_3" runat="server" HeaderText="Model Summary">
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="tabs_top_right_3_update_panel" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="valueSpec Simplistic aircraftSpec viewBoxMargin ">
                                                    <div class="Box">
                                                        <br />
                                                        <div class="row removeLeftMargin">
                                                            <div class="columns eight removeLeftMargin">
                                                                <asp:Label runat="server" ID="modelSummaryText" CssClass="valueControl formatTable blue airframeTable"></asp:Label>
                                                            </div>
                                                            <div class="columns four removeLeftMargin">
                                                                <asp:Image ID="modelImage" runat="server" Style="max-width: 130px;" ImageUrl="/images/spacer.gif" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="tabs_top_right_4" runat="server" HeaderText="My Graphs">
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="tabs_top_right_4_update_panel" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:TextBox runat="server" ID="startIDs" Style="display: none;"></asp:TextBox>
                                                <asp:TextBox runat="server" ID="graphWhat" Style="display: none;"></asp:TextBox>
                                                <div class="row removeLeftMargin remove_margin">
                                                    <div class="eight columns text_align_center remove_margin">
                                                        <div id="startGraph">
                                                        </div>
                                                    </div>
                                                    <div class="four columns remove_margin">
                                                        <asp:Button runat="server" CssClass="float_left refreshGraph" ID="closeGraphs" Text="Close" />
                                                    </div>
                                                    <div class="four columns remove_margin">
                                                        <asp:Button runat="server" CssClass="float_right" Style="display: none;" ID="refreshGraphs"
                                                            Text="Refresh Graph" OnClientClick="$('body').addClass('loading');" />
                                                    </div>
                                                    <asp:Button runat="server" CssClass="float_right display_none" ID="createStartGraphs"
                                                        Text="Refresh Graph" OnClientClick="$('body').addClass('loading');" />
                                                    <asp:Button runat="server" CssClass="float_right display_none" Style="display: none;"
                                                        ID="createStartTransGraphs" Text="Refresh Graph" OnClientClick="$('body').addClass('loading');" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </div>
                    </div>
                    <br />
                    <div class="clearfix">
                    </div>
                    <div class="row">
                        <asp:TextBox runat="server" ID="evalueIDs" CssClass="display_none"></asp:TextBox>
                        <asp:TextBox runat="server" ID="currentACIDs" CssClass="display_none"></asp:TextBox>
                        <asp:TextBox runat="server" ID="salesACIDs" CssClass="display_none"></asp:TextBox>
                        <cc1:TabContainer ID="tabs_bottom" CssClass="dark-theme" Width="100%" runat="server"
                            ActiveTabIndex="1">
                            <cc1:TabPanel runat="server" ID="tabs_bottom_1" HeaderText="Aircraft on Market">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_1_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="currentTabButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="currentRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Panel runat="server" ID="currentAircraftPanelToggle">
                                                <asp:Label runat="server" ID="modelAirframeTypeCode" CssClass="display_none"></asp:Label>
                                                <asp:Label runat="server" ID="ModelTypeCode" CssClass="display_none"></asp:Label>
                                                <asp:Label runat="server" ID="ModelWeightClass" CssClass="display_none"></asp:Label>
                                                <asp:Label runat="server" ID="currentAircraftText" CssClass="valueSearchTable"><div class="cwContainer" width="100%"><table id="startTable" class="refreshable"></table></div></asp:Label>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tabs_bottom_2" HeaderText="Sales">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_2_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="salesTabButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="salesRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Panel runat="server" ID="transactionAircraftPanelToggle">
                                                <asp:Label runat="server" ID="transactionAircraftText" CssClass="valueSearchTable"><div class="cwContainer" width="100%"><table id="transactionTable" width="100%" class="refreshable"></table></div></asp:Label>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tabs_bottom_3" HeaderText="Values by Year/Vintage">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_3_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="valuesByYearVintageButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="vtgRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Label runat="server" ID="valueYearVintageLabel" CssClass="valueSearchTable"></asp:Label><hr
                                                class="valueSeperator" />
                                            <div class="row">
                                                <div class="six columns text_align_center">
                                                    <strong>Avg Asking vs Selling Price By Year Mfr ($k)</strong>
                                                    <div id="graph1">
                                                    </div>
                                                </div>
                                                <div class="six columns text_align_center">
                                                    <strong>Avg Selling Price By Year Mfr ($k)</strong>
                                                    <div id="graph2">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="twelve columns text_align_center">
                                                    <asp:Literal runat="server" ID="valueYearVintageMFRGraph"></asp:Literal>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tabs_bottom_4" HeaderText="Values by History" Visible="true">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_4_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="valuesByQuarterButton" />
                                            <asp:AsyncPostBackTrigger ControlID="FirstTimeValuesByQuarterButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="QuarterRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Label runat="server" ID="valueTrendsByQuarterLabel" CssClass="valueSearchTable"></asp:Label><hr
                                                class="valueSeperator" />
                                            <div class="row">
                                                <div class="six columns text_align_center">
                                                    <strong class="displayNoneMobile">Avg Asking vs Selling Price ($k)<span> - (For Asking
                            with Sold)</span></strong>
                                                    <div id="graphQuarter1Div">
                                                    </div>
                                                </div>
                                                <div class="six columns text_align_center">
                                                    <strong>Avg Asking vs Selling Price ($k) <span class="displayNoneMobile">- (All Asking/Sold
                            Prices)</span></strong>
                                                    <div id="graphQuarter2Div">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="six columns text_align_center">
                                                    <strong>Avg Asking Price ($k) <span class="displayNoneMobile">- (All Asking Prices)</span></strong>
                                                    <div id="graphQuarter3Div">
                                                    </div>
                                                </div>
                                                <div class="six columns text_align_center">
                                                    <strong>Avg Sold Price ($k) <span class="displayNoneMobile">- (All Sold Prices)</span></strong>
                                                    <div id="graphQuarter4Div">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="six columns text_align_center">
                                                    <strong>Avg. Sold Price % of Asking Price <span class="displayNoneMobile">- (For Asking
                            with Sold)</span></strong>
                                                    <div id="graphQuarter5Div">
                                                    </div>
                                                </div>
                                                <div class="six columns text_align_center">
                                                    <strong>Variance of Sold Price from Asking Price <span class="displayNoneMobile">-
                            (For Asking with Sold)</span></strong>
                                                    <div id="graphQuarter6Div">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="twelve columns text_align_center">
                                                    <asp:Literal runat="server" ID="valuesByQuarterMonthGraph"></asp:Literal>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tabs_bottom_5" HeaderText="Values by AFTT">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_5_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="valuesByAFTTButton" />
                                            <asp:AsyncPostBackTrigger ControlID="FirstTimeValuesByAFTTButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="afttRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Label runat="server" ID="valueTrendsByAFTTLabel" CssClass="valueSearchTable"></asp:Label><hr
                                                class="valueSeperator" />
                                            <div class="row">
                                                <div class="six columns text_align_center">
                                                    <strong>Avg Asking vs Selling Price by AFTT ($k)</strong>
                                                    <div id="graphAFTT1Div">
                                                    </div>
                                                </div>
                                                <div class="six columns text_align_center">
                                                    <strong>Avg Selling Price by AFTT ($k)</strong>
                                                    <div id="graphAFTT2Div">
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:Literal runat="server" ID="valueAfttGraphAfttTab"></asp:Literal>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tabs_bottom_6" HeaderText="Values by Weight Class">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_6_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="valuesByWeightClassButton" />
                                            <asp:AsyncPostBackTrigger ControlID="FirstTimeValuesByWeightClassButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="WeightRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Label runat="server" ID="valueTrendsByWeightLabel" CssClass="valueSearchTable"></asp:Label><hr
                                                class="valueSeperator" />
                                            <div class="row">
                                                <div class="six columns text_align_center">
                                                    <strong>Weight Class - Percentage of Asking Price (%)</strong>
                                                    <div id="graphWeight1Div">
                                                    </div>
                                                </div>
                                                <div class="six columns text_align_center">
                                                    <strong>Weight Class - Variance of Asking Price (%)</strong>
                                                    <div id="graphWeight2Div">
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tabs_bottom_8" HeaderText="Valuation" Visible="false">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_8_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="valuesValuationButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="valueValuationRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Literal runat="server" ID="currentMarketValueGraph"></asp:Literal>
                                            <asp:Literal runat="server" ID="estimatesMfrYearGraph"></asp:Literal>
                                            <asp:Literal runat="server" ID="estimatesMonthGraph"></asp:Literal>
                                            <asp:Literal runat="server" ID="estimatesAFTTGraph"></asp:Literal>
                                            <asp:Literal runat="server" ID="estimatesResidualGraph"></asp:Literal>
                                            <asp:Label runat="server" ID="value_estimates_label"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tabs_bottom_9" HeaderText="Residual Values" Visible="false">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_9_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="valuesResidualButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="valueResidualsRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Literal runat="server" ID="estimatesResidualTabGraph"></asp:Literal>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tabs_bottom_10" HeaderText="" Visible="false">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_10_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="valuesEvaluesButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="valueEvaluesRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Literal runat="server" ID="evalues_data"><div class="cwContainer"><table id="evaluesTable" class="refreshable"></table></div></asp:Literal>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel runat="server" ID="tabs_bottom_7" HeaderText="My Value History" Visible="false">
                                <ContentTemplate>
                                    <asp:UpdatePanel ID="tabs_bottom_7_update_panel" runat="server" UpdateMode="Conditional">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="valuesHistoryButton" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox runat="server" ID="valueHistoryRan" Text="false" CssClass="display_none"></asp:TextBox>
                                            <asp:Label runat="server" ID="values_label" CssClass="valueSpec viewValueExport Simplistic aircraftSpec aircraftContainer"></asp:Label>
                                            <asp:Chart ID="valuation_chart" runat="server" ImageStorageMode="UseImageLocation"
                                                ImageType="Jpeg" Visible="False">
                                                <Series>
                                                    <asp:Series>
                                                    </asp:Series>
                                                </Series>
                                                <ChartAreas>
                                                    <asp:ChartArea Name="ChartArea1">
                                                    </asp:ChartArea>
                                                </ChartAreas>
                                            </asp:Chart>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <!-- <canvas id="chart_div_canvas"></canvas>
                <input type="button" value="Open as image in a new window" id="chart_div_button" />

                <script type="text/javascript">
                  jQuery('#chart_div_button').click(function() {
                    canvg(document.getElementById('chart_div_canvas'), jQuery('#valueHistoryGraph1 div div').html());
                    //jQuery('#valueHistoryGraph1').hide();
                    var canvas = document.getElementById('chart_div_canvas');
                    var img = canvas.toDataURL("image/png");
                    window.open(img);
                  });
                </script>-->
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </div>
                    <asp:Button ID="valuesEvaluesButton" runat="server" Text="Runs Evalues Tab Load"
                        CssClass="display_none" OnClick="RunEvaluesTabClick" />
                    <asp:Button ID="currentTabButton" runat="server" Text="Runs Current Tab Load" CssClass="display_none"
                        OnClick="RunCurrentTab" />
                    <asp:Button ID="salesTabButton" runat="server" Text="Runs Sales Tab Load" CssClass="display_none"
                        OnClick="RunSalesTab" />
                    <asp:Button ID="FirstTimeValuesByWeightClassButton" runat="server" Text="Runs Value By Weight Tab Load First Time on Tab Swap"
                        CssClass="display_none" />
                    <asp:Button ID="valuesByWeightClassButton" runat="server" Text="Runs Value By Weight Tab Load"
                        CssClass="display_none" />
                    <asp:Button ID="valuesByQuarterButton" runat="server" Text="Runs Value By Quarter Tab Load"
                        CssClass="display_none" />
                    <asp:Button ID="FirstTimeValuesByQuarterButton" runat="server" Text="Runs Value By Quarter Tab Load on Tab Swap"
                        CssClass="display_none" />
                    <asp:Button ID="valuesByYearVintageButton" runat="server" Text="Runs ValueVintage Tab Load"
                        CssClass="display_none" OnClick="RunValueVintageTabClick" />
                    <asp:Button ID="FirstTimeValuesByAFTTButton" runat="server" Text="Runs AFTT Tab Load First Time On Tab Swap"
                        CssClass="display_none" />
                    <asp:Button ID="valuesByAFTTButton" runat="server" Text="Runs AFTT Tab Load" CssClass="display_none" />
                    <asp:Button runat="server" ID="valuesHistoryButton" OnClick="RunMyHistoryTabClick"
                        Text="Runs Valuation Tab Load on First Time on Tab Swap" CssClass="display_none" />
                    <asp:Button runat="server" ID="valuesValuationButton" OnClick="RunValuationTabClick"
                        Text="Runs Valuation Tab Load on First Time on Tab Swap" CssClass="display_none" />
                    <asp:Button runat="server" ID="valuesResidualButton" OnClick="RunResidualTabClick"
                        Text="Runs Residual Tab Load on First Time on Tab Swap" CssClass="display_none" />
                    <asp:Button runat="server" ID="runFirstQuery" OnClick="RunPageLoad" Text="Run Page Load"
                        CssClass="display_none" />
                    <asp:Button runat="server" ID="runEvaluesSwap" OnClick="RunEvalSwap" Text="Run Page Load and top tap"
                        CssClass="display_none" />
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
</div>
<div class="modal">
    <!-- Place at bottom of page -->
</div>

<script src="/common/moment-with-locales.js"></script>

<script type="text/javascript">

    function clearBoxes() {
        $('#<%= QuarterRan.ClientID %>').val('false');
        $('#<%= vtgRan.clientID %>').val('false');
        $('#<%= WeightRan.clientID %>').val('false');
        $('#<%= valueValuationRan.clientID %>').val('false');
        $('#<%= valueResidualsRan.clientID %>').val('false');
        $find('<%= tabs_bottom.clientID %>').set_activeTabIndex(1);
    }

    function SetLoadingText(textToSet) {
        $("#<%= loadingTextContainer.ClientID %>").css("display", "block");
       // $('#<%'= loadingText.clientID %>').text(textToSet);
    }
    var startWindow;
    var yearIndex = 0;
    var afttIndex = 0;
    var regIndex = 0;
    var forSaleIndex = 0;
    var newACFlag = 0;
    var transDateIndex = 0;
    var salePriceIndex = 0;

    var doNotFilter = false;
    var x = 0;
    function customFilter(settings) {

        var tabIndex = $find("<%=tabs_bottom.clientID %>"); // name of tabContainer
        var i = tabIndex._activeTabIndex;

        if (settings.nTable.getAttribute('id') == 'startTable') {
            yearIndex = 4;
            afttIndex = 5;
            regIndex = 2;
            forSaleIndex = 20;
            newACFlag = 0;
            transDateIndex = 0;
            salePriceIndex = 0;
            if (i == 0) { doNotFilter = false } else {
                doNotFilter = true;
            }

        } else if (settings.nTable.getAttribute('id') == 'evaluesTable') {

            yearIndex = 4;
            afttIndex = 5;
            regIndex = 2;
            forSaleIndex = 20;
            newACFlag = 0;
            transDateIndex = 0;
            salePriceIndex = 0;

            if (i == 8) { doNotFilter = false } else {
                doNotFilter = true;
            }

        } else if (settings.nTable.getAttribute('id') == 'vintageTable') {
            doNotFilter = true;
        } else if (settings.nTable.getAttribute('id') == 'afttTable') {
            doNotFilter = true;
        } else if (settings.nTable.getAttribute('id') == 'quarterTable') {
            doNotFilter = true;
        } else if (settings.nTable.getAttribute('id') == 'weightTable') {
            doNotFilter = true;
        } else {
            if (i == 1) { doNotFilter = false } else {
                doNotFilter = true;
            }
            yearIndex = 3;
            afttIndex = 5;
            transDateIndex = 4;
            regIndex = 2;
            forSaleIndex = 9;
            newACFlag = 16;
            salePriceIndex = 8;
        }
    }
    //var dtApi = null;
    /* Custom filtering function which will search data in column four between two values */
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            //We need to set booleans for the filter return:
            var yearFilter = true;
            var afttFilter = true;
            var regFilter = true;
            var marketFilter = true;
            var checkFilter = true;
            var displayFilter = true;
            var saleDateFilter = true;
            var newFilter = true;

            customFilter(settings);

            if (doNotFilter == false) { //If this is true, we don't need to run any of these.
                //console.log('This means we filtered check');
                //Here we go with filtering:
                //        var CheckedCol = $(data[0]).filter(':checked').val()

                //        if (CheckedCol == true) {
                //          checkFilter = true;
                //        }
                //        else { checkFilter = false; }
                //        if (!dtApi) {
                //          dtApi = new $.fn.dataTable.Api(settings); 
                //        }
                var row = $.fn.dataTable.Api(settings).row(dataIndex).nodes();
                var KeepRemove = $('#<%= acKeepRemove.clientID %>').val();
                checkFilter = ($(row).hasClass('gone') ? false : true);

                switch (KeepRemove) {
                    case "remove":
                        if ($(row).hasClass('remove')) {
                            $(row).removeClass('remove');
                            $(row).removeClass('keep');
                            $(row).addClass('gone');
                            checkFilter = false;
                        }
                        break;
                    default:
                        if ($(row).hasClass('keep')) {
                            $(row).removeClass('remove');
                            $(row).removeClass('keep');
                            $(row).removeClass('gone');
                            checkFilter = true;
                        } else {
                            $(row).removeClass('remove');
                            $(row).removeClass('keep');
                            $(row).addClass('gone');
                            checkFilter = false;
                        };
                }


                //$.fn.dataTable.Api(settings).row(dataIndex).deselect();
                //  

                //checkFilter = true;
                //console.log('check filter' + checkFilter);

                //Year filtering takes place here.
                //Since the yearFilter defaults to true, this only really becomes important if it hits false. 
                var yearMin = parseInt($('#<%= year_start.clientID %>').val(), 10);
                var yearMax = parseInt($('#<%= year_end.clientID %>').val(), 10);
                var yearCol = parseFloat(data[yearIndex]) || 0; // use data for the age column
                //console.log(yearCol);
                if ((isNaN(yearMin) && isNaN(yearMin)) ||
                    (isNaN(yearMin) && yearCol <= yearMax) ||
                    (yearMin <= yearCol && isNaN(yearMax)) ||
                    (yearMin <= yearCol && yearCol <= yearMax)) {
                    yearFilter = true;
                } else {
                    yearFilter = false;
                }

                // console.log('year filter' + yearFilter);
                //Aftt filtering is happening here.
                var afttMin = parseInt($('#<%= aftt_start.clientID %>').val(), 10);
                var afttMax = parseInt($('#<%= aftt_end.clientID %>').val(), 10);
                var afttCol = parseFloat(data[afttIndex]) || 0; // use data for the age column
                //console.log(afttCol);
                if ((isNaN(afttMin) && isNaN(afttMax)) ||
                    (isNaN(afttMin) && afttCol <= afttMax) ||
                    (afttMin <= afttCol && isNaN(afttMax)) ||
                    (afttMin <= afttCol && afttCol <= afttMax)) {
                    afttFilter = true;
                } else {
                    afttFilter = false;
                }

                //console.log('aftt filter' + afttFilter);

                //Registration Filtering is here.
                var value = $('#<%= aircraft_registration.clientID %>').val(); //checked_radio.val();
                var regNo = data[regIndex] || ''; // use data for the reg column
                //console.log(regNo);
                switch (value) {
                    case "N":
                        if (regNo.substring(0, 1) == "N") {
                            regFilter = true;
                        } else {
                            regFilter = false;
                        }
                        break;
                    case "I":
                        if (regNo.substring(0, 1) != "N") {
                            regFilter = true;
                        } else {
                            regFilter = false;
                        }
                        break;
                    default:
                        regFilter = true;
                } //End Switch


                // console.log('reg filter' + regFilter);
                //Market filtering
                var value = $('#<%= ac_market.clientID %>').val(); //checked_radio.val();
                var forSale = data[forSaleIndex] || ''; // use data for the reg column

                //console.log(forSale);
                switch (value) {
                    case "Y":
                        if (forSale.substring(0, 1) == "Y") {
                            marketFilter = true;

                        } else {
                            marketFilter = false;
                        }
                        break;
                    default:
                        marketFilter = true;
                } //End Switch

                //console.log('market filter' + yearFilter);
                //These are all the sales price table filters.
                if (settings.nTable.getAttribute('id') == 'transactionTable') { //Transaction table if start

                    //Starting with the display dropdown.
                    var value = $('#<%= salePriceDropdown.clientID %>').val();
                    var salePriceCol = data[salePriceIndex] || ''; // use data for the reg column
                    //console.log(data[salePriceIndex]);
                    //console.log(salePriceCol.length);
                    switch (value) {
                        case "W":
                            if (salePriceCol.length == 0) {
                                displayFilter = false;
                            } else {
                                displayFilter = true;
                            }
                            break;
                        case "O":
                            if (salePriceCol.length == 0) {
                                displayFilter = true;
                            } else {
                                displayFilter = false;
                            }
                            break;
                        default:
                            displayFilter = true;
                    } //end switch

                    //Sales date boxes.
                    var dateMin = new Date($('#<%= start_date.clientID %>').val());
                    var dateMax = new Date($('#<%= end_date.clientID %>').val());
                    var dateCol = moment(data[transDateIndex], "MM/DD/YY") || '';
                    //console.log(dateCol);
                    if (dateCol.isBetween(dateMin, dateMax, 'days', '[]')) {
                        saleDateFilter = true;
                    } else {
                        saleDateFilter = false;
                    }


                    ///New used dropdown.
                    var value = $('#<%= newUsed.clientID %>').val(); //checked_radio.val();
                    var newFlagCol = data[newACFlag] || ''; // use data for the reg column
                    //console.log(newFlagCol);
                    switch (value) {
                        case "N":
                            if (newFlagCol == "NEWFIRSTOWNER") {
                                newFilter = true;
                            } else {
                                newFilter = false;
                            }
                            break;
                        case "U":
                            if (newFlagCol == "NEWFIRSTOWNER") {
                                newFilter = false;
                            } else {
                                newFilter = true;
                            }
                            break;
                        default:
                            newFilter = true;
                    } //End switch

                } //Ending the transaction filters.

            } //End the do not filter function

            //This is where we need to set up the return.
            //console.log(checkFilter);
            //console.log('\n Year: ' + yearFilter + ' Aftt:' + afttFilter + ' Reg:' + regFilter + ' Market:' + marketFilter + ' SaleDate:' + saleDateFilter + ' New:' + newFilter);
            if (checkFilter && yearFilter && afttFilter && regFilter && marketFilter && displayFilter && saleDateFilter && newFilter) {
                //  console.log('\n Year: ' + yearFilter + ' check filter' + checkFilter + ' Aftt:' + afttFilter + ' Reg:' + regFilter + ' Market:' + marketFilter + ' SaleDate:' + saleDateFilter + ' New:' + newFilter + ' return true');
                return true;
            } else {
                // console.log('\n Year: ' + yearFilter + ' Aftt:' + afttFilter + ' Reg:' + regFilter + ' Market:' + marketFilter + ' SaleDate:' + saleDateFilter + ' New:' + newFilter + ' return false');
                return false;
            }


        });                                   //Entire function end.




    $(document).ready(function () {
        $find('<%=tabs_top_right_4.ClientID%>')._hide();
        $find('<%=tabs_top_left_3.ClientID%>')._hide();
        window.scrollTo(0, 0);

        //    var mw = $(".ContainerBoxSummary").width() - 20;
        //    hideShowGraphs(mw);

        var groups = {};
        $("select option[OptionGroup]").each(function () {
            groups[$.trim($(this).attr("OptionGroup"))] = true;
        });
        $.each(groups, function (c) {
            $("select option[OptionGroup='" + c + "']").wrapAll('<optgroup label="' + c + '">');
        });


    });

    window.name = "ValueMaster";

    function SubMenuDropValue(reportID, folder_type) {


        my_form = document.createElement('FORM');
        my_form.method = 'POST';
        my_form.target = "_blank"
        // alert(folder_type);

        //folders maintenance popup  

        my_form.action = 'FolderMaintenance.aspx';
        my_form.name = 'folderForm';

        //Appending the type of folder, either Aircraft or History.
        my_tb = document.createElement('INPUT');
        my_tb.type = 'HIDDEN';
        my_tb.name = "TYPE_OF_FOLDER";
        my_tb.value = folder_type//.innerHTML;
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
                //if (elem[i].value != '') {
                if ((elem[i].id.indexOf("tabs_top_right_tabs_top_right_1") == -1) && (elem[i].id.indexOf("tabs_bottom_tabs_bottom_") == -1)) {
                    var re = new RegExp("ctl[A-Za-z0-9]*_ContentPlaceHolder[A-Za-z0-9]_", "g");
                    var re2 = new RegExp("Value_View1_tabs_top_left_tabs_top_left_[A-Za-z0-9]*_", "g");
                    var re3 = new RegExp("Value_View1_tabs_top_right_tabs_top_right_[A-Za-z0-9]*_", "g");
                    var re4 = new RegExp("Value_View1_", "g");

                    var rep = elem[i].id;
                    var temp = rep.replace(re, "");


                    temp = temp.replace(re2, "");
                    temp = temp.replace(re3, "");
                    temp = temp.replace(re4, "");
                    my_tb = document.createElement('INPUT');
                    my_tb.type = 'HIDDEN';
                    my_tb.name = temp;

                    //If it has a checked value that's not undefined, go ahead and 
                    //Pass that, if not, pass the value

                    if (elem[i].type == 'checkbox') {
                        my_tb.value = elem[i].checked;
                        //alert(temp + " : " + elem[i].value);
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

                    my_form.appendChild(my_tb);
                    // }
                }
            }
        }
        document.body.appendChild(my_form);
        my_form.submit();


    }

    function setUpSliderInitial() {
        bxSliderVar = $('.bxslider').bxSlider({
            auto: false, responsive: true, slideSelector: '.child',
            autoControls: false,
            stopAutoOnClick: true,
            pager: false,
            onSliderLoad: function (index) {
                if (index === 0) {
                    $('.bx-prev').addClass("display_none");
                };
                if (index === 5) {
                    $('.bx-next').addClass("display_none");
                };
            },
            onSlideAfter: function (slide, oldIndex, newIndex) {
                if (newIndex === 5) {
                    $('.bx-next').addClass("display_none");
                }
                else {
                    $('.bx-prev').removeClass("display_none");
                    $('.bx-next').removeClass("display_none");
                    if (newIndex === 0) {
                        $('.bx-prev').addClass("display_none");
                    };
                }
            },
            infiniteLoop: false
        }); $('.bx-prev').addClass("display_none");
    }
    function setUpSlider() {
        setTimeout(function () {
            setUpSliderInitial()
        }, 3000);
    }


    function SubMenuDropValue2(reportID, folder_type) {

        my_form = document.createElement('FORM');
        my_form.method = 'GET';
        my_form.target = "_blank"
        // alert(folder_type);

        //folders maintenance popup  

        my_form.action = 'viewtopdf.aspx';
        my_form.name = 'folderForm';

        //Appending the type of folder, either Aircraft or History. 
        my_tb = document.createElement('INPUT');
        my_tb.type = 'HIDDEN';
        my_tb.name = "viewID";
        my_tb.value = "998"
        my_form.appendChild(my_tb);

        var str = '';
        var elem = document.getElementById('aspnetForm').elements;
        for (var i = 0; i < elem.length; i++) {
            if (elem[i].type != 'hidden' && elem[i].type != 'submit') {
                //if (elem[i].value != '') {
                if ((elem[i].id.indexOf("tabs_top_right_tabs_top_right_1") == -1) && (elem[i].id.indexOf("tabs_bottom_tabs_bottom_") == -1)) {
                    var re = new RegExp("ctl[A-Za-z0-9]*_ContentPlaceHolder[A-Za-z0-9]_", "g");
                    var re2 = new RegExp("Value_View1_tabs_top_left_tabs_top_left_[A-Za-z0-9]*_", "g");
                    var re3 = new RegExp("Value_View1_tabs_top_right_tabs_top_right_[A-Za-z0-9]*_", "g");
                    var re4 = new RegExp("Value_View1_", "g");
                    var re5 = new RegExp("ContentPlaceHolder1_", "g");
                    var re6 = new RegExp("hiddenYear_start", "g");
                    var re7 = new RegExp("hiddenYear_end", "g");
                    var re8 = new RegExp("hiddenAftt_start", "g");
                    var re9 = new RegExp("hiddenAftt_end", "g");

                    var rep = elem[i].id;
                    var temp = rep.replace(re, "");


                    temp = temp.replace(re2, "");
                    temp = temp.replace(re3, "");
                    temp = temp.replace(re4, "");
                    temp = temp.replace(re5, "");
                    temp = temp.replace(re6, "hidden_year_start");
                    temp = temp.replace(re7, "hidden_year_end");
                    temp = temp.replace(re8, "hidden_aftt_start");
                    temp = temp.replace(re9, "hidden_aftt_end");
                    my_tb = document.createElement('INPUT');
                    my_tb.type = 'HIDDEN';
                    my_tb.name = temp;

                    //If it has a checked value that's not undefined, go ahead and 
                    //Pass that, if not, pass the value

                    if (elem[i].type == 'checkbox') {
                        my_tb.value = elem[i].checked;
                        //alert(temp + " : " + elem[i].value);
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
                    }

                    my_form.appendChild(my_tb);
                    // }
                }
            }
        }

        //Let's do a test to see if sliders moved:

        //year_start=2001&year_end=2014&aftt_start=0&aftt_end=10439

        my_tb = document.createElement('INPUT');
        my_tb.name = "afttFilter";

        if ($("#<%= aftt_start.clientID %>").val() !== $("#<%= hiddenAftt_start.clientID %>").val() || $("#<%= aftt_end.clientID %>").val() !== $("#<%= hiddenAftt_end.clientID %>").val()) {
            my_tb.value = "true";
        } else {
            my_tb.value = "false";
        }

        my_form.appendChild(my_tb);

        my_tb = document.createElement('INPUT');
        my_tb.name = "yearFilter";

        if (($("#<%= year_start.clientID %>").val() !== $("#<%= hiddenYear_start.clientID %>").val() || $("#<%= year_end.clientID %>").val() !== $("#<%= hiddenYear_end.clientID %>").val())) {
            my_tb.value = "true";
        } else {
            my_tb.value = "false";
        }

        my_form.appendChild(my_tb);

        document.body.appendChild(my_form);
        my_form.submit();


    }

    $(window).resize(function () {
        setTimeout(function () {
            var mw = $(".ContainerBoxSummary").width() - 20;
            //$(".cwContainer").width(cw);
            hideShowGraphs(mw);
        }, 700);
    });

    function hideShowGraphs(amountAvailable) {

        if (Number(amountAvailable) >= 770) {
            //alert('made it');
            //we have room for the graph.
            //show graph
            //set maxwidth for summary
            $('.valueSummary').addClass("maxWidthValue float_left");
            $('.valueSummary').removeClass("fullWidth");
            $('.optionalBox').removeClass("display_none");
            $('.optionalBox').addClass("float_right");
            $('.optionalBox').css("width", "45% !important;");
        } else {
            $('.optionalBox').addClass("display_none");
            $('.valueSummary').removeClass("maxWidthValue");
            $('.valueSummary').addClass("fullWidth");
        }

    }
</script>

