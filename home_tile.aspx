<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="home_tile.aspx.vb" Inherits="crmWebClient.home_tile"
    MasterPageFile="~/main_site.Master" EnableViewState="true" StylesheetTheme="Evo" %>

<%@ MasterType VirtualPath="~/main_site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="/common/sumoSelect.css" />
    <asp:Label runat="server" ID="evo_scripts">


        <style type="text/css">
            .gray_background .valueSpec.Simplistic {
                background-color: transparent !important;
            }


            .header_row {
                border-bottom: 0px !important;
            }

            .marginTop {
                margin-top: 15px;
            }
            .marginBottom {margin-bottom:15px;}

            .subHeader {
                font-size: 25.5px !important;
            }

            .blue_text {
                padding: 7px 2px 2px 0px;
                padding: 7px 2px 2px 0px;
                display: block;
                text-transform: uppercase;
                color: #5d5e5e !important;
            }

            .Box .aircraft_folder {
                background-image: none;
            }

            .aircraft_folder .header_row {
                color: #333 !important;
                font-weight: bold;
                font-size: 13px;
            }

            .aircraft_folder img {
                padding-right: 4px;
            }

            .overflowBox {
                max-height: 250px;
                overflow: auto;
                clear: both;
            }

            img.float_right.xButton {
                margin-left: 6px;
                margin-right: -4px;
                cursor: pointer;
                position: relative;
                z-index: 1;
            }

            .recentActivities strong.display_block {
                padding-top: 10px;
                padding-bottom: 5px;
                font-size: 14px
            }

            .folderSubHeader {
                float: left;
                width: 38% !important;
            }

            .folderItem img {
                margin-top: -4px;
            }

            .recentActivities a.display_block {
                padding: 5px 0px 5px 15px;
                font-size: 13px;
                color: #4f5050;
            }

            a:hover {
                color: #078fd7 !important;
            }

            .grid-item .searchBox .searchIcon {
                width: 50px !important;
                height: 21px !important;
            }

            .grid-item .searchBox {
                display: block !important;
                width: 100% !important
            }

                .grid-item .searchBox input[type="text"] {
                    width: 98% !important;
                    padding: 2px !important;
                    height: 30px;
                }

            .grid-item .medium_text, .grid-item th a {
                font-size: 13px;
                text-transform: uppercase;
                color: #333 !important;
            }

            table.dataTable td {
                font-size: 14px !important;
            }

            .grid-item .gray_background {
                /*background-color: #7b7b7b;*/
            }

            .folderItem .dark_header {
                padding-top: 2px !important;
            }

            .selectorBoxColor {
                background-color: #eceaea;
                padding: 10px;
                margin-bottom: 10px;
                z-index: 1;
                position: relative;
            }

            .editButtonSVG {
                z-index: 2;
                position: relative;
            }

            .grid-item .gray_background .tiny_text {
                /*color: white;*/
            }

            .aircraft_folder .header_row a, .aircraft_folder .headerFolder, .aircraft_folder table tr:first-child td a {
                font-size: 15px !important;
                text-decoration: none !important;
                font-weight: bold;
                text-transform: uppercase;
            }



            .SumoSelect {
                width: 97% !important;
            }

                .SumoSelect .select-all.partial > span i, .SumoSelect .select-all.selected > span i, .SumoSelect > .optWrapper.multiple > .options li.opt.selected span i {
                    background-color: #0075ff !important;
                }

            .eventsHeader {
                width: 47% !important;
                float: left;
                display: inline !important;
            }

            .eventsPanel {
                float: left;
                padding-left: 20px;
            }
        </style>

        <script type="text/javascript">

            function RunHomeSearch() {
                if ($("#<%=HomeSearchBoxText.ClientID %>").val() != '') {
              callQuickHeaderSearch($("#<%=HomeSearchBoxText.ClientID %>").val(), false);
                }
            }

            function openSmallWindowJS(address, windowname) {
                var rightNow = new Date();
                windowname += rightNow.getTime();
                var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=1150,height=600");
            }

            var options1 = {
                curveType: 'function',
                width: '100%', height: 320,
                chartArea: { width: '87%', height: '80%', top: 10 },
                vAxis: { title: "Clicks", minValue: -1 },
                legend: { position: 'none' }
            };

            var options40 = {
                curveType: 'function',
                width: '100%', height: 320,
                vAxis: { title: "Deliveries", minValue: -1 },
                legend: { position: 'top' }
            };

            function refreshHome() {
                <%= PostBackStr.ToString %>;
            }

        </script>
    </asp:Label>

    <style type="text/css">
        .valueSpec.Simplistic .Box.grid-item .subHeader {
            color: #078fd7 !important
        }

        .grid-item a, .valueSpec .formatTable.blue a, .formatTable a, .valueSpec .grid .formatTable.blue a {
            text-decoration: underline !important;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
        <ProgressTemplate>
            <span></span>
            <div class="loader">Loading...</div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel runat="server" Visible="true" ID="crmPanelVisibility">
        <asp:UpdateProgress ID="UpdateProgress3" AssociatedUpdatePanelID="crm_update_panel"
            runat="server" DisplayAfter="5">
            <ProgressTemplate>
                <div id="Div1" runat="server" class="loadingScreenUpdatePanel">
                    <br />
                    <br />
                    <img src="Images/loading.gif" alt="Loading..." /><br />
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <table width="100%" cellpadding="5" cellspacing="0">
            <tr>
                <td rowspan="3" valign="top">
                    <asp:UpdatePanel runat="server" ID="crm_update_panel" UpdateMode="Conditional" ChildrenAsTriggers="false">
                        <ContentTemplate>
                            <cc1:TabContainer runat="server" ID="crm_tab" Width="100%" Visible="true" CssClass="dark-theme"
                                AutoPostBack="true" OnClientActiveTabChanged="LargeTabActiveTabChanged">
                                <cc1:TabPanel ID="crm_action_panel" runat="server" Visible="true">
                                    <HeaderTemplate>
                                        Action Items
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="padding">
                                            <asp:Panel runat="server" BackColor="#184D7B" ForeColor="White" Font-Size="14pt"
                                                Height="35px" CssClass="no_pad">
                                                <asp:Label ID="today_date" runat="server" Text="Most Recently Edited Companies" CssClass="today_date"></asp:Label>
                                            </asp:Panel>
                                            <asp:Label runat="server" ID="demo_attention_label" Font-Bold="True" ForeColor="Red"
                                                Font-Size="Medium" Visible="False"><p align="center">Please note that this is a demonstration account only.</p> <p align="center"> All data under this account may be viewed by other demonstration users and may be erased at any time.</p></asp:Label>
                                            <br />
                                            <asp:Panel runat="server" BackColor="#C8DAF0" ForeColor="Black" Font-Size="11pt"
                                                Height="25px" ID="calendar_panel">
                                                <asp:Panel runat="server" ID="timeframepanel" CssClass="float_right margin_4">
                                                    <asp:DropDownList runat="server" ID="crm_calendar_timeframe" CssClass="float_right"
                                                        AutoPostBack="True">
                                                        <asp:ListItem Value="1">Next Day</asp:ListItem>
                                                        <asp:ListItem Selected="True" Value="7">Next Week</asp:ListItem>
                                                        <asp:ListItem Value="31">Next Month</asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:Label ID="crm_calendar_timeframe_label" runat="server" CssClass="padding float_right">Show Actions Through:</asp:Label>
                                                </asp:Panel>
                                                <asp:Label ID="main_calendar" runat="server" Text="Calendar" CssClass="today_calendar"></asp:Label>
                                            </asp:Panel>
                                            <asp:Label ID="main_calendar_txt" runat="server"></asp:Label>
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="crm_market_overview_panel" runat="server" Visible="true">
                                    <HeaderTemplate>
                                        Market Overview
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="padding">
                                            <asp:Label ID="crm_market_overview" runat="server"></asp:Label>
                                            <asp:Button runat="server" ID="toggleSales" Text="Show Pre-Owned Sales Summary" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="crm_event_panel" runat="server">
                                    <HeaderTemplate>
                                        Events (Recent)
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="crm_event_update_panel" UpdateMode="Conditional"
                                            ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <asp:Panel runat="server" ID="crm_time_panel" CssClass="display_none light_seafoam_green_header_color">
                                                    <table width="100%" cellpadding="3" cellspacing="0">
                                                        <tr>
                                                            <td align="right" valign="middle">
                                                                <asp:DropDownList ID="crm_event_time" Visible="true" CellPadding="3"
                                                                    runat="server" AutoPostBack="true" Font-Size="9px">
                                                                    <asp:ListItem Value="1">One Day</asp:ListItem>
                                                                    <asp:ListItem Selected="True" Value="7">One Week</asp:ListItem>
                                                                    <asp:ListItem Value="30">One Month</asp:ListItem>
                                                                    <asp:ListItem Value="90">Three Months</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="right" valign="middle">
                                                                <asp:Label ID="Label3" runat="server" Font-Size="9px">Category:</asp:Label>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:DropDownList ID="crm_event_category" Visible="true" CellPadding="3"
                                                                    runat="server" RepeatLayout="flow" AutoPostBack="true" Font-Size="9px">
                                                                    <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                                <asp:Label ID="crm_event_listing" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="crm_wanteds_panel" runat="server">
                                    <HeaderTemplate>
                                        Wanteds (Recent)
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:UpdatePanel runat="server" ID="crm_wanted_update_panel" UpdateMode="Conditional"
                                            ChildrenAsTriggers="false">
                                            <ContentTemplate>
                                                <asp:Label ID="crm_wanted_label" runat="server" Text=""></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="crm_user_activity_panel" runat="server">
                                    <HeaderTemplate>
                                        User Activity
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Label ID="user_activity_label" runat="server" Text=""></asp:Label>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                                <cc1:TabPanel ID="crm_client_db_panel" runat="server">
                                    <HeaderTemplate>
                                        Client Database
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <asp:Label ID="client_database_label" runat="server" Text=""></asp:Label>
                                    </ContentTemplate>
                                </cc1:TabPanel>
                            </cc1:TabContainer>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td valign="top" width="25%">
                    <asp:Panel runat="server" ID="home_right_visible" BackColor="White">
                        <asp:Label ID="home_companies_txt" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="home_contacts_txt" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="home_aircraft_txt" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="home_notes_txt" runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="home_documents_txt" runat="server" Text=""></asp:Label>
                        <br />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Table ID="evo_display_table" runat="server" CellPadding="0" CellSpacing="0"
        Width="100%" CssClass="row valueSpec viewValueExport Simplistic aircraftSpec gray_background">
        <asp:TableHeaderRow>
            <asp:TableCell VerticalAlign="Top">
                <asp:UpdatePanel runat="server" ID="main_home_update_panel" UpdateMode="Conditional"
                    ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <div class="dark_header heightBar">
                        </div>
                        <asp:Button runat="server" ID="makeModelButton" Text="Save Stuff" CssClass="display_none" />
                        <asp:Button runat="server" ID="dashboardSaveButton" Text="Save Dashboard Stuff" CssClass="display_none" />
                        <asp:Label runat="server" ID="queryOutput" ForeColor="Red" Font-Bold="true"></asp:Label>
                        <div class="grid">

                            <asp:Literal runat="server" ID="gridItems"></asp:Literal>
                            <asp:Panel runat="server" ID="BoxContainer"></asp:Panel>

                            <div id="edit-start">
                                <div runat="server" id="modelSelectContainer" style="display: none;" class="selectorBoxColor">
                                    <p>Add models to your fleet or <a href="javascript:void(0)" id="clear_button">clear</a> all selections.</p>
                                    <asp:ListBox SelectionMode="multiple" runat="server" ID="makeModelDynamic" Width="100%" CssClass="makeModelSelectSumo display_block" placeholder="Please select models of interest.."></asp:ListBox>
                                    <br />
                                    <br />
                                </div>
                            </div>

                            <div class="grid-item" runat="server" id="boxCustomizerContainer" visible="false">
                                <div class="Box">
                                    <div class="subHeader">Customize Homepage</div>
                                    <p class="marginTop">Customize the tiles displayed on your homepage below.</p>
                                    <asp:ListBox SelectionMode="multiple" runat="server" ID="dashboardDynamic" Width="100%" CssClass="display_block" placeholder="Please select blocks of interest.."></asp:ListBox>
                                </div>
                            </div>

                            <div class="grid-item" runat="server" id="FleetSummaryContainer" visible="false">
                                <div class="Box">
                                    <img src="images/x.svg" runat="server" id="FleetSummaryContainer_delete" alt="Delete Block" class="float_right xButton" width="20" />
                                    <img src="images/edit.svg" alt="Edit Global Models" class="float_right cursor_pointer editButtonSVG" width="20" id="edit-2" onclick="showEdit(2);" />

                                    <div id="edit-model-2" style="display: none;"></div>
                                    <div class="subHeader" runat="server" id="FleetSummaryHeader">Fleet Summary</div>
                                    <asp:Label ID="fleet_summary_label" runat="server" Text=""></asp:Label><div class="clear"></div>
                                </div>
                            </div>

                            <div class="grid-item" runat="server" id="MarketOverviewContainer" visible="false">
                                <div class="Box">
                                    <img src="images/x.svg" runat="server" id="MarketOverviewContainer_delete" alt="Delete Block" class="float_right xButton" width="20" />
                                    <img src="images/edit.svg" alt="Edit Global Models" class="float_right cursor_pointer editButtonSVG" width="18" id="edit-1" onclick="showEdit(1);" />

                                    <div id="edit-model-1" style="display: none;"></div>
                                    <div class="subHeader" runat="server" id="MarketOverviewHeader">Market Overview</div>
                                    <asp:Label ID="market_overview_label" runat="server" Text=""></asp:Label><div class="clear"></div>
                                </div>
                            </div>

                            <div class="grid-item" id="Events_Container" visible="false" runat="server">
                                <div class="Box">
                                    <img src="images/x.svg" runat="server" id="events_container_delete" alt="Delete Block" class="float_right xButton" width="20" />
                                    <img src="images/edit.svg" alt="Edit Global Models" class="float_right cursor_pointer editButtonSVG" width="18" id="edit-3" onclick="showEdit(3);" />
                                    <div id="edit-model-3" style="display: none;"></div>
                                    <div class="subHeader eventsHeader" runat="server" id="eventsHeader">Events</div>
                                    <asp:Panel runat="server" ID="event_time_panel" CssClass="eventsPanel">
                                        <table width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="top" width="100px">
                                                    <asp:DropDownList ID="event_time" Visible="true" CellPadding="3"
                                                        runat="server" AutoPostBack="true" Font-Size="9px">
                                                        <asp:ListItem Selected="True" Value="7">One Week</asp:ListItem>
                                                        <asp:ListItem Value="30">One Month</asp:ListItem>
                                                        <asp:ListItem Value="90">Three Months</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" valign="middle" width="50px">
                                                    <asp:Label ID="Label1" runat="server" Font-Size="9px" Font-Bold="true">Category:</asp:Label>
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:DropDownList ID="event_category" Visible="true" CellPadding="3"
                                                        runat="server" AutoPostBack="true" Font-Size="9px" Width="90px">
                                                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br />
                                    <asp:Label ID="event_listing_label" runat="server" Text="" Style="max-height: 170px; overflow: auto; display: block; clear: both;"></asp:Label><div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-item" runat="server" id="WantedContainer" visible="false">
                                <div class="Box">
                                    <img src="images/x.svg" runat="server" id="WantedContainer_delete" alt="Delete Block" class="float_right xButton" width="20" />
                                    <img src="images/edit.svg" alt="Edit Global Models" class="float_right cursor_pointer editButtonSVG" width="18" id="edit-4" onclick="showEdit(4);" />
                                    <div id="edit-model-4" style="display: none;"></div>
                                    <asp:Label ID="wanted_listing_label" runat="server" Text="" CssClass="display_block"></asp:Label><div class="clear"></div>
                                </div>
                            </div>

                            <div class="grid-item" id="CompanyAnalyticsContainer" runat="server" visible="false">

                                <div class="Box overflow_hidden">
                                    <img src="images/x.svg" runat="server" id="CompanyAnalyticsContainer_delete" alt="Delete Block" class="float_right xButton" width="20" />
                                    <div class="subHeader marginBottom" runat="server" id="companyAnalyticsHeader">My Company Analytics</div>
                                    <asp:Label ID="company_analytics_label" runat="server" Text=""></asp:Label><div class="clear"></div>
                                    <div id="AircraftAnalyticsContainer" runat="server" visible="false">
                                        <div class="overflow_hidden" style="margin-top: 10px;">
                                            <hr />
                                            <asp:Label ID="aircraft_analytics_label" runat="server" Text=""></asp:Label><div class="clear"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="grid-item overflow_hidden" id="Cumulative_New_Deliveries_Container" runat="server" visible="false">
                                <div class="Box overflow_hidden">
                                    <img src="images/x.svg" runat="server" id="Cumulative_New_Deliveries_Container_delete" alt="Delete Block" class="float_right xButton" width="20" />
                                    <div class="subHeader" runat="server" id="cumulative_deliveries_header">Cumulative Deliveries</div>
                                    <asp:Label ID="cumulative_deliveries_label" runat="server" Text=""></asp:Label>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-item" id="ActionItemsContainer" runat="server" visible="false">
                                <div class="Box">
                                    <img src="images/x.svg" alt="Delete Block" runat="server" id="ActionItemsContainer_delete" class="float_right xButton" width="20" />
                                    <div class="subHeader" runat="server" id="actionItemsHeader">Action Items (30 Days)</div>
                                    <asp:Label ID="evo_action_items" runat="server" Text="" Style="overflow: auto; max-height: 209px; display: block;"></asp:Label><div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-item" visible="false" runat="server" id="ReportsContainer">
                                <div class="Box">
                                    <img src="images/x.svg" alt="Delete Block" runat="server" id="ReportsContainer_delete" class="float_right xButton" width="20" />
                                    <div class="subHeader" runat="server" id="reportHeader">Reports</div>
                                    <asp:Label runat="server" ID="custom_reports_results">
                                    </asp:Label>
                                    <asp:Label ID="custom_reports_label" runat="server" Text="" CssClass="overflowBox display_block"></asp:Label><div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-item" visible="false" runat="server" id="MyAirportsContainer">
                                <div class="Box">
                                    <img src="images/x.svg" alt="Delete Block" runat="server" id="MyAirportsContainer_delete" class="float_right xButton" width="20" />
                                    <div class="subHeader" runat="server" id="myAirportHeader">My Airports</div>
                                    <table width="100%" class="marginTop">
                                        <tr>
                                            <td width="110px">
                                                <asp:DropDownList ID="months_choice" runat="server" Visible="false" AutoPostBack="true">
                                                    <asp:ListItem Value="MTD">Current Month to Date</asp:ListItem>
                                                    <asp:ListItem Value="YTD">Current Year to Date</asp:ListItem>
                                                    <asp:ListItem Value="1" Selected="True">1 Month</asp:ListItem>
                                                    <asp:ListItem Value="3">3 Months</asp:ListItem>
                                                    <asp:ListItem Value="6">6 Months</asp:ListItem>
                                                    <asp:ListItem Value="12">12 Months</asp:ListItem>
                                                    <asp:ListItem Value="24">24 Months</asp:ListItem>
                                                </asp:DropDownList></td>
                                            <td align="left">
                                                <asp:Label ID="my_airports_label" Text="" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label runat="server" ID="modifyListAirport"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>


                                    <div runat="server" id="div_airports_label_table">
                                        <div style="text-align: center; width: 100%;" runat="server" id="airportsResults">
                                            <asp:Label ID="airportsTable" runat="server" Text="" EnableViewState="false"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-item" visible="false" runat="server">
                                <div class="Box">
                                    <img src="images/x.svg" alt="Delete Block" class="float_right xButton" width="20" />
                                    <asp:Label ID="mympm_label" runat="server"><p>&nbsp;</p></asp:Label><div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-item" visible="false" runat="server" id="SearchContainer">
                                <div class="Box">
                                    <img src="images/x.svg" alt="Delete Block" runat="server" id="SearchContainer_delete" class="float_right xButton" width="20" />
                                    <div class="subHeader">Quick Search</div>
                                    <asp:Panel ID="searchBoxVisible" CssClass="searchBox" runat="server">
                                        <asp:TextBox ID="HomeSearchBoxText" runat="server" placeholder="Search" CssClass="tooltip"></asp:TextBox><img src="images/search.svg" class="searchIcon" alt="Search" onclick="RunHomeSearch();return false;" />

                                    </asp:Panel>
                                    <div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-item" visible="false" runat="server">
                                <div class="Box">
                                    <img src="images/x.svg" alt="Delete Block" class="float_right xButton" width="20" />
                                    <asp:Label ID="folder_events_tab_text" runat="server" CssClass="padding"><p>&nbsp;</p></asp:Label><div class="clear"></div>
                                </div>
                            </div>
                            <div class="grid-item recentActivities" visible="false" runat="server" id="RecentItemsContainers">
                                <div class="Box">
                                    <img src="images/x.svg" runat="server" id="RecentItemsContainers_delete" alt="Delete Block" class="float_right xButton" width="20" />
                                    <div class="subHeader" runat="server" id="recentActivityHeader">My Recent Activity</div>
                                    <div class="overflowBox">
                                        <asp:Label ID="recent_aircraft_activity_evo" runat="server" Text=""></asp:Label>
                                        <div class="clear"></div>
                                    </div>
                                </div>
                            </div>
                            <div class="grid-item" visible="false" runat="server" id="FolderContainer">
                                <div class="Box">
                                    <img src="images/x.svg" alt="Delete Block" runat="server" id="FolderContainer_delete" class="float_right xButton" width="20" />
                                    <div class="subHeader folderSubHeader" runat="server" id="folderHeader">Folders</div>
                                    <div class="padding_table folderItem" style="margin-top: 5px;">
                                        <asp:CheckBox runat="server" ID="show_hidden_folders" Text="Show Hidden Folders?"
                                            AutoPostBack="true" EnableViewState="true"
                                            CssClass="tiny_text margin_right float_right" onclick="createCookie('hideHidden', this.checked, 1);" />
                                        <asp:CheckBox ID="hide_shared" runat="server" Text="Hide Shared Folders?" AutoPostBack="true"
                                            EnableViewState="true" CssClass="tiny_text margin_right float_right" onclick="createCookie('hideShared', this.checked, 1);" />
                                    </div>
                                    <div class="overflowBox">

                                        <asp:Panel runat="server" ID="aircraft_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=3" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <a href="FolderMaintenance.aspx?t=3&newStaticFolder=true&fromHome=true" target="new"
                                                class="float_right margin_right">New<img src="images/newsearch.png" alt="New"
                                                    border="0" class="padding_left" /></a>
                                            <asp:TreeView ID="aircraft_projects" runat="server" SkinID="project_tree_view" Width="250px">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="history_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=8" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <asp:TreeView ID="history_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="company_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=1" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <a href="FolderMaintenance.aspx?t=1&newStaticFolder=true&fromHome=true" target="new"
                                                class="float_right margin_right">New<img src="images/newsearch.png" alt="New"
                                                    border="0" class="padding_left" /></a>
                                            <asp:TreeView ID="company_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="contact_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=2" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <asp:TreeView ID="contact_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="event_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=5" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <asp:TreeView ID="event_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="wanted_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=9" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <asp:TreeView ID="wanted_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="performance_specs_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=12" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <asp:TreeView ID="performance_specs_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="operating_costs_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=11" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <asp:TreeView ID="operating_costs_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="marketing_summary_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=13" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <asp:TreeView ID="marketing_summary_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="airport_folder_container" CssClass="aircraft_folder">
                                            <a href="FolderMaintenance.aspx?t=17" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <a href="FolderMaintenance.aspx?t=17&newStaticFolder=true&fromHome=true" target="new"
                                                class="float_right margin_right">New<img src="images/newsearch.png" alt="New"
                                                    border="0" class="padding_left" /></a>
                                            <asp:TreeView ID="airport_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="values_folder_container" CssClass="aircraft_folder display_none">
                                            <a href="FolderMaintenance.aspx?t=16" target="new" class="float_right margin_right">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" width="11px" /></a>
                                            <asp:TreeView ID="values_projects" runat="server" SkinID="project_tree_view">
                                            </asp:TreeView>
                                        </asp:Panel>
                                        <asp:Label ID="no_projects" runat="server" Text="" Visible="false"><br /><p align="center">There are no current projects.</p></asp:Label>
                                        <br class="div_clear" />
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>

                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </asp:TableCell>
        </asp:TableHeaderRow>
    </asp:Table>
    <asp:Table ID="yacht_display_table" runat="server" Visible="false" CellPadding="2"
        CellSpacing="0" Width="100%" CssClass="evo_display_table">
        <asp:TableHeaderRow>
            <asp:TableCell VerticalAlign="Top">
                <asp:UpdatePanel runat="server" ID="yacht_update_panel" UpdateMode="Conditional"
                    ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <cc1:TabContainer runat="server" ID="yacht_summary_tab" Width="100%" ActiveTabIndex="0"
                            CssClass="dark-theme" OnClientActiveTabChanged="LargeTabActiveTabChanged" AutoPostBack="true">
                            <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Summary">
                                <ContentTemplate>
                                    <asp:Label ID="yacht_summary_label" runat="server" Text=""></asp:Label>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="yacht_quick_search_tab" runat="server" HeaderText="Quick Search">
                                <ContentTemplate>
                                    <asp:Panel runat="server" ID="yacht_search_control_panel">
                                        <table width="100%" cellpadding="3" cellspacing="0" class="data_aircraft_grid override_borders">
                                            <tr class="header_row">
                                                <td align="left" valign="top" width="50%">
                                                    <b>Yacht</b>
                                                </td>
                                                <td align="left" valign="top" width="50%">
                                                    <b>Company</b>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <table width="100%" cellpadding="3" cellspacing="0">
                                                        <tr class="display_none">
                                                            <td align="left" valign="top" width="140">Brand/Model:<img src="../images/magnify_small.png" class="padding_left help_cursor"
                                                                border="0" width="9" alt="Type characters describing the Yacht Brand/Model that you desire and Yacht Spot will provide you with a list of models that match your needs. Note that Quick Search only allows for one model selection at a time."
                                                                title="Type characters describing the Yacht Brand/Model that you desire and Yacht Spot will provide you with a list of models that match your needs. Note that Quick Search only allows for one model selection at a time." />
                                                            </td>
                                                            <td align="left" valign="top" colspan="3">
                                                                <asp:TextBox ID="___yt_model_id" runat="server" CssClass="display_none"></asp:TextBox>
                                                                <div class="ui-widget">
                                                                    <asp:DropDownList runat="server" name="YachtModelDynamic" ID="YachtModelDynamic"
                                                                        autofocus="autofocus" autocorrect="off" autocomplete="off">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top" width="160">Yacht Name:
                                                            </td>
                                                            <td align="left" valign="top" colspan="3">
                                                                <asp:TextBox ID="___yacht_name_search" runat="server" Width="45%"></asp:TextBox>
                                                                <asp:CheckBox runat="server" Text="Search Prev. Names" ID="___ypn_previous_name" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top" width="160">Hull #:
                                                            </td>
                                                            <td align="left" valign="top" width="24%">
                                                                <asp:TextBox ID="___hull_MFR_from" runat="server" Width="100%"></asp:TextBox>
                                                            </td>
                                                            <td align="right" valign="top" width="20%">Call Sign:
                                                            </td>
                                                            <td align="left" valign="top" width="30%">
                                                                <asp:TextBox ID="___yt_call_sign" runat="server" Width="100%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan='5' align='left'>
                                                                <table cellspacing='0' cellpadding='0' border='0' width='100%'>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="___for_sale" Text="For Sale?" CssClass="float_left" />&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="___for_lease" Text="For Lease?" CssClass="float_left" />&nbsp;
                                                                        </td>
                                                                        <td>
                                                                            <asp:CheckBox runat="server" ID="___for_charter" Text="For Charter?" CssClass="float_left" />&nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <!--
                            <tr>
                              <td align="left" valign="top">
                                Market Status:
                              </td>
                              <td align="left" valign="top" colspan="3">
                                <asp:DropDownList ID="___yt_market" runat="server" Width="100%">
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
                                </asp:DropDownList>
                              </td>
                            </tr>
                            -->
                                                        <tr>
                                                            <td align="left" valign="top">Year MFR/Dlv:
                                                            </td>
                                                            <td align="left" valign="top" colspan="3">
                                                                <asp:TextBox ID="___yt_year_mfr" runat="server" Width="60px"></asp:TextBox>/
                                <asp:TextBox ID="___yt_year_dlv" runat="server" Width="60px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Length:
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList runat="server" ID="___operator_length" Width="100%">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="___length_to" runat="server" Width="100%">
                                                                </asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="___us_standard" runat="server" Text="US" Checked='true' />
                                                                <asp:CheckBox ID="___metric_standard" runat="server" Text="Metric" />
                                                                <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe1" runat="server" TargetControlID="___us_standard"
                                                                    Key="YesNo" />
                                                                <cc1:MutuallyExclusiveCheckBoxExtender ID="mecbe2" runat="server" TargetControlID="___metric_standard"
                                                                    Key="YesNo" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">Yacht Type:
                                                            </td>
                                                            <td align="left" valign="top" colspan="3">
                                                                <asp:DropDownList ID="___yt_motor_sailing" runat="server" Width="100%">
                                                                    <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                                    <asp:ListItem Value="M">Motor</asp:ListItem>
                                                                    <asp:ListItem Value="S">Sailing</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" valign="top" class="gray_background_color">
                                                    <table width="100%" cellpadding="3" cellspacing="0">
                                                        <tr>
                                                            <td align="left" valign="top" width="130">Company Name:
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="company_name___" runat="server" Width="100%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">Contact Name (First/Last):
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="company_contact_first___" runat="server" Width="45%" CssClass="float_left"></asp:TextBox>
                                                                <asp:TextBox ID="company_contact_last___" runat="server" Width="45%" CssClass="float_right"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">Email Address:
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:TextBox ID="company_email_address___" runat="server" Width="100%"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">Relationship To Yacht:
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:DropDownList ID="company_relationship___" runat="server" Width="100%">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%" cellpadding="3" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="top" width="50%">
                                                    <asp:LinkButton ID="searchYacht" runat="server" CssClass="gray_button float_right"
                                                        OnClientClick="document.body.style.cursor='wait';">Search Yacht</asp:LinkButton>
                                                </td>
                                                <td align="left" valign="top" width="50%">
                                                    <asp:LinkButton ID="searchYachtCompany" runat="server" CssClass="gray_button float_right">Search Company</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Latest News">
                                <ContentTemplate>
                                    <asp:Label ID="yacht_latest_news_label" runat="server" Text=""></asp:Label>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="yacht_index_tab" runat="server" HeaderText="Index" Visible="false">
                                <HeaderTemplate>
                                    Attributes
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div id="yacht_index_wait_div" runat="server" class="loadingScreenPage home_page_margin">
                                        <span>Please wait while the Attributes Tab is loading... </span>
                                        <br />
                                        <br />
                                        <img src="Images/loading.gif" alt="Loading..." /><br />
                                    </div>
                                    <asp:Label ID="yacht_index_tab_label" runat="server"></asp:Label>
                                    <asp:Panel runat="server" ID="yachtIndexPanel" CssClass="display_none">
                                    </asp:Panel>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="yacht_action_items" runat="server" HeaderText="Action Items" Visible="false">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="yacht_action_items_label"><p>There are no current items for display.</p></asp:Label>
                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:TableCell>
            <asp:TableCell VerticalAlign="Top" Width="300px">
                <cc1:TabContainer runat="server" ID="yacht_small_tab" Width="100%" ActiveTabIndex="0"
                    CssClass="dark-theme" OnClientActiveTabChanged="SmallTabActiveTabChanged">
                    <cc1:TabPanel ID="TabPanel6" runat="server" HeaderText="Recent Activity">
                        <ContentTemplate>
                            <asp:Label ID="recent_aircraft_activity_yacht" runat="server" Text=""></asp:Label>
                            <asp:TreeView ID="yacht_company_recent" runat="server" SkinID="project_recent_view"
                                Visible="false" NodeWrap="true" CssClass="vertical_align_top tiny_text aircraft_folder">
                            </asp:TreeView>
                            <asp:TreeView ID="yacht_contact_recent" runat="server" SkinID="project_recent_view"
                                Visible="false" NodeWrap="true" CssClass="vertical_align_top tiny_text aircraft_folder">
                            </asp:TreeView>
                            <asp:TreeView ID="yacht_recent" runat="server" SkinID="project_recent_view" Visible="false"
                                NodeWrap="true" CssClass="vertical_align_top tiny_text aircraft_folder">
                            </asp:TreeView>
                            <br class="div_clear" />
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="TabPanel7" runat="server" HeaderText="Folders">
                        <ContentTemplate>
                            <asp:UpdatePanel runat="server" ID="yacht_folder_update" ChildrenAsTriggers="true"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="small_subbar">
                                        <a href="#" onclick="javascript:load('http://www.jetnetevo.com/help/helpexamples/340.pdf ','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
                                            class="red_button help_button">
                                            <img src="images/info_white.png" alt="" border="0" width="13" /></a>
                                        <asp:CheckBox runat="server" ID="yacht_hidden_folders" Text="Show Hidden Folders?"
                                            AutoPostBack="true" EnableViewState="true" CssClass="tiny_text margin_right white_text float_right"
                                            OnCheckedChanged="change_hidden_folder" />
                                    </div>
                                    <asp:Panel runat="server" ID="Panel4" CssClass="aircraft_folder">
                                        <a href="FolderMaintenance.aspx?t=1" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" /></a>
                                        <asp:TreeView ID="yacht_company_projects" runat="server" SkinID="project_tree_view">
                                        </asp:TreeView>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="Panel2" CssClass="aircraft_folder">
                                        <a href="FolderMaintenance.aspx?t=2" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" /></a>
                                        <asp:TreeView ID="yacht_contact_projects" runat="server" SkinID="project_tree_view">
                                        </asp:TreeView>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="Panel7" CssClass="aircraft_folder">
                                        <a href="FolderMaintenance.aspx?t=10" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" /></a>
                                        <asp:TreeView ID="yacht_projects" runat="server" SkinID="project_tree_view">
                                        </asp:TreeView>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="Panel1" CssClass="aircraft_folder">
                                        <a href="FolderMaintenance.aspx?t=14" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" /></a>
                                        <asp:TreeView ID="yacht_history_projects" runat="server" SkinID="project_tree_view">
                                        </asp:TreeView>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="Panel3" CssClass="aircraft_folder">
                                        <a href="FolderMaintenance.aspx?t=15" target="new" class="float_right margin_right tiny_text">Edit<img src="images/edit.svg" alt="Edit" border="0" class="padding_left" /></a>
                                        <asp:TreeView ID="yacht_event_projects" runat="server" SkinID="project_tree_view">
                                        </asp:TreeView>
                                    </asp:Panel>
                                    <asp:Label ID="Label4" runat="server" Text="" Visible="false"><br /><p align="center">There are no current projects.</p></asp:Label>
                                    <br class="div_clear" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </asp:TableCell>
        </asp:TableHeaderRow>
    </asp:Table>

    <asp:TextBox ID="time" runat="server" Style="display: none;">
    </asp:TextBox>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {

            loadMasonry();

        });


        function loadMasonry() {
            var grid = document.querySelector('.grid');
            var msnry = new Masonry(grid, {
                itemSelector: '.grid-item',
                columnWidth: '.grid-item',
                gutter: 10,
                horizontalOrder: true,
                percentPosition: true
            });
            setTimeout(function () { msnry.layout(); }, 300);

        }

        function DisplayACDetailsWithAnalytics(ACID) {
            window.open("", "myNewWin", "width=1050,height=900,toolbar=0,scrollbars=1");

            my_form = document.createElement('FORM');
            my_form.name = 'myForm';
            my_form.method = 'POST';
            my_form.action = 'DisplayAircraftDetail.aspx?acid=' + ACID + '&analytics=Y';
            my_form.target = "myNewWin";
            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = 'analytics';
            my_tb.value = "true";
            my_form.appendChild(my_tb);
            document.body.appendChild(my_form);
            my_form.submit();
        }


        function SubmitForm(model, lifecycle, for_sale, exclusive, type_code, make) {
            my_form = document.createElement('FORM');
            my_form.name = 'myForm';
            my_form.method = 'POST';
            my_form.action = 'Aircraft_Listing.aspx';

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = 'for_sale';
            my_tb.value = for_sale;
            my_form.appendChild(my_tb);

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = 'complete_search';
            my_tb.value = "Y";
            my_form.appendChild(my_tb);

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = 'model';
            my_tb.value = model;
            my_form.appendChild(my_tb);


            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = 'make';
            my_tb.value = make;
            my_form.appendChild(my_tb);

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = 'type_code';
            my_tb.value = type_code;
            my_form.appendChild(my_tb);

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = 'exclusive';
            my_tb.value = exclusive;
            my_form.appendChild(my_tb);

            my_tb = document.createElement('INPUT');
            my_tb.type = 'HIDDEN';
            my_tb.name = 'lifecycle';
            my_tb.value = lifecycle;
            my_form.appendChild(my_tb);
            document.body.appendChild(my_form);


            my_form.submit();
        }


        function RedrawDatatablesOnSys() {
            setTimeout(reRenderThem, 1800);
        }

        function reRenderThem() {
            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
            $($.fn.dataTable.tables(true)).DataTable().responsive.recalc()
        }

        function selectAllRows(data, selectedRows, tableName) {

            var IDsToUse = '';
            var count = 0;

            data.each(function (value, index) {
                if (IDsToUse.length == 0) {
                    IDsToUse = value[1];
                } else {
                    IDsToUse += ', ' + value[1];
                }
                count += 1;
            });

            //$("#" + selectedRows).val(IDsToUse);

        }


        function CreateTheDatatable(divName, tableName, jQueryTablename) {

            //alert("show datatable");

            var selectedRows = '';

            try {
                if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
                    $("#" + divName).empty();
                };
            }
            catch (err) {

            }

            if ($("#" + tableName).length) {

                //if ((tableName == "companyDataTable") || (tableName == "airportDataTable")) {
                //  selectedRows = "";

                //} else {
                //  selectedRows = "";

                //}

                //jQuery("#" + tableName).css('display', 'block');

                var clone = jQuery("#" + tableName).clone(true);

                jQuery("#" + tableName).css('display', 'none');
                clone[0].setAttribute('id', jQueryTablename);
                clone.appendTo("#" + divName);

                var table = $("#" + jQueryTablename).DataTable({
                    destroy: true,
                    language: { "search": "Filter:" },
                    fixedHeader: true,
                    "initComplete": function (settings, json) {
                        setTimeout(function () {
                            $("#" + jQueryTablename).DataTable().columns.adjust();
                            $("#" + jQueryTablename).DataTable().scroller.measure();

                            var dataRows = $("#" + jQueryTablename).DataTable().rows();
                            selectAllRows(dataRows.data(), selectedRows, tableName);

                        }, 1200)
                    },
                    scrollCollapse: true,
                    stateSave: true,
                    paging: false,
                    columnDefs: [
                        { targets: [1], className: 'display_none' },
                        { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }
                    ],
                    select: { style: 'multi', selector: 'td:first-child' },
                    order: [[3, 'desc']],
                    dom: 'Bftrp',
                    buttons: [
                        { extend: 'csv', exportOptions: { columns: ':visible' } },
                        { extend: 'excel', exportOptions: { columns: ':visible' } },
                        { extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible' } },
                        //{ extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

                        {
                            text: 'Remove Selected Rows', className: 'RemoveRowsValue',
                            action: function (e, dt, node, config) {

                                dt.rows({ selected: true }).remove().draw(false);
                                selectAllRows(dt.rows({ selected: false }).data(), selectedRows, tableName);

                            }
                        },

                        {
                            text: 'Keep Selected Rows', className: 'KeepTableRow',
                            action: function (e, dt, node, config) {

                                dt.draw();
                                selectAllRows(dt.rows({ selected: true }).data(), selectedRows, tableName);
                                dt.rows({ selected: false }).remove().draw(false);
                                dt.rows('.selected').deselect();

                            }
                        },

                        {
                            text: 'Reload Table', className: 'RefreshTableValue',
                            action: function (e, dt, node, config) {

                                //$("#" + selectedRows).val('');
                                ChangeTheMouseCursorOnItemParentDocument('cursor_wait');

                            }
                        }
                    ]
                });
            }

            $(".RefreshTableValue").addClass('display_none');
            $(".KeepTableRow").addClass('display_none');

            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
        };



        function showEdit(editNumber) {
            if (!$("#edit-model-" + editNumber).is(":visible")) {
                var myCustomizeBox = $("#<%= modelSelectContainer.ClientID %>").detach();
                $("#edit-model-" + editNumber).append(myCustomizeBox.show());
            }
            $('#edit-model-' + editNumber).slideToggle();
            loadMasonry();

        }

        function resetEdit(editNumber) {
            var myCustomizeBox = $("#<%= modelSelectContainer.ClientID %>").detach();
            $("#edit-start").append(myCustomizeBox.show());
        }

    </script>
</asp:Content>
