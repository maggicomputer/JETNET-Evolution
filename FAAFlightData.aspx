<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="FAAFlightData.aspx.vb"
    Inherits="crmWebClient.FAAFlightData" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE&sensor=false&libraries=geometry">
    </script>

    <style type='text/css' media='screen'>
        .hiddenBox {
            position: absolute;
            top: 0px;
            right: -55px;
            z-index: 12000;
        }

        .paddedBox {
            padding: 10px;
        }

        .containerElement {
            position: relative;
            /* display: block;*/
            width: 255px;
            /*z-index: 10000;*/
        }

        #imagePreview {
            width: 150px;
            display: none;
            border: 1px solid #eee;
        }

        #map {
            float: left;
            width: 740px;
            height: 400px;
        }

        #message {
            position: absolute;
            padding: 10px;
            background: #555;
            color: #fff;
            width: 75px;
        }

        #list {
            float: left;
            width: 200px;
            background: #eee;
            list-style: none;
            padding: 0;
        }

            #list li {
                padding: 10px;
            }

                #list li:hover {
                    background: #555;
                    color: #fff;
                    cursor: pointer;
                    cursor: hand;
                }

        A.underline {
            font-family: Arial, Times, Verdana, Geneva, Helvetica, sans-serif;
            text-decoration: underline;
            cursor: pointer;
        }
        /*faaFlightSearch only*/ .faaFlightSearch .valueSpec.Simplistic .Box {
            margin-left: auto;
            margin-right: auto;
            width: 96%;
            margin-top: 10px;
            padding: 15px;
        }

        .faaFlightSearch .faaFlightSearchPanel {
            margin-left: auto;
            margin-right: auto;
            width: 96%;
            margin-top: 10px;
        }

        .faaFlightSearch .valueSpec.Simplistic, .faaFlightSearch .valueSpec {
            background-color: Transparent;
            background-image: none;
            min-height: 200px;
        }

        .faaFlightSearch .dark-theme .ajax__tab_body {
            background-color: #eee;
        }

        .faaFlightSearch .noPaddingFirstLevelTD td {
            padding: 0px;
        }

            .faaFlightSearch .noPaddingFirstLevelTD td td {
                padding: 4px;
            }

        .faaFlightSearch .formatTable th strong, .faaFlightSearch .formatTable th .label {
            display: block;
            padding-bottom: 15px;
        }

        .faaFlightSearch .faaFlightSearchPanel {
            margin-top: -10px;
        }

            .faaFlightSearch .faaFlightSearchPanel .searchButtonFAA {
                margin-top: -1px;
                padding: 4px 8px;
            }

        .faaFlightSearch .viewValueExport .mainHeading span {
            /*margin-top:10px;*/
            font-size: 18px;
            display: inline-block;
            padding-right: 10px;
        }

        .faaFlightSearch .viewValueExport .mainHeading {
            line-height: 1.1em;
            font-size: 26px;
        }

        .faaFlightSearch .blue .mainHeading span strong {
            color: #000;
        }

        .searchBoxFAA {
            background-color: rgba(80, 80, 80, 0.9);
            padding: 20px;
            -webkit-box-shadow: 2px 2px 4px 1px #121212;
            box-shadow: 2px 2px 4px 1px #121212;
        }

        .searchOptionHeader {
            float: left;
            color: #fff;
            font-weight: bold;
            text-transform: uppercase;
            margin: -14px 0px 0px 0px;
            padding: 0px;
            font-size: 1.5em;
        }

        .closeXPanel {
            float: right;
            z-index: 100000;
            position: absolute;
            bottom: 23px;
            right: 5px;
            font-weight: bold;
            font-size: 1.5em !important;
        }

        .mainBox {
            width: 97% !important;
            margin-bottom: 10px !important;
        }

        '
        .mainBox .mainHeading span {
            display: inline-block;
            padding-right: 10px;
            padding-left: 0px;
        }

        .mainBox .padding {
            padding-left: 0px;
        }

        .dateSearchBoxes {
            font-size: 18px;
            margin-left: -7px;
            color: #676767;
        }

        .dataTables_scrollHead {
            width: 100% !important;
        }

        .dataTables_wrapper .dataTables_length {
            float: right;
        }

        #ctl00_pageSizing {
            overflow-x: hidden;
        }

        .dataTables_scrollBody thead td {
            padding-bottom: 0px !important;
            padding-top: 0px !important;
        }

        .dataTables_scrollBody .formatTable.blue thead {
            margin-bottom: 10px;
        }

        .dataTable thead {
            font-weight: bold;
        }

        .valueSpec.Simplistic .formatTable.dataTable th {
            padding: 10px 18px;
            background-color: #eee;
            font-size: 12px !important;
            text-transform: none;
            vertical-align: middle;
        }

        .endBox {
            width: 100%;
            font-size: 15px;
            text-transform: uppercase;
            color: #676767;
        }

        tfoot tr th {
            padding: 10px !important;
        }

        .toFromLabel {
            font-size: 15px;
            display: block;
            padding-right: 8px;
            padding-left: 8px;
            padding-bottom: 4px;
            padding-top: 5px;
            text-transform: uppercase;
        }

        .startBox {
            width: 100%;
            font-size: 15px;
            color: #078fd7 !important;
            font-weight: bold;
            text-transform: uppercase;
        }

        .standalone_page .DetailsBrowseTable a.display_none {
            display: none !important;
        }

        .standalone_page .DetailsBrowseTable {
            max-width: 100% !important;
            width: 100% !important;
        }
        .dataTables_scrollHead{width:auto !important;}
    </style>

    <script language="javascript" type="text/javascript">

        function openSmallWindowJS(address, windowname) {

            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = open(address, windowname, "menubar,scrollbars=1,resizable,width=1150,height=600");

            return true;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart', 'table'] });
    </script>
    <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
        <ProgressTemplate>
            <span></span>
            <div class="loader">Loading...</div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="text-align: center;" class="faaFlightSearch">
        <asp:UpdatePanel ID="flight_data_update" runat="server" ChildrenAsTriggers="True"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" CssClass="DetailsBrowseTable"
                    runat="server">
                    <asp:TableRow>
                        <asp:TableCell ID="TableCell2" runat="server" HorizontalAlign="right" VerticalAlign="middle"
                            Style="padding-right: 4px;">

                        </asp:TableCell>
                        <asp:TableCell ID="TableCell3" runat="server" HorizontalAlign="right" VerticalAlign="middle"
                            Style="padding-right: 4px;">
                            <div class="backgroundShade">
                                <a href="#" onclick="javascript:window.close();" class="float_right">
                                    <img src="images/x.svg" alt="Close" /></a>
                                <a href="#" onclick="javascript:load('help.aspx','','scrollbars=yes,menubar=no,height=900,width=1050,resizable=yes,toolbar=no,location=no,status=no');"
                                    class="float_right help_cursor" title="Show Flight Data Help">
                                    <img src="images/help-circle.svg" alt="Help" /></a>
                                <a class="float_right display_none" href="javascript:void(0);" runat="server"
                                    id="exportFlightsLink">
                                    <img src="images/download.svg" alt="Download" /></a>
                                <asp:Label runat="server" ID="viewMapLinkLabel"></asp:Label>
                                </a>
                            </div>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <div class="aircraftContainer">
                    <div class="sixteen columns tabContainerBottomBox">
                        <div class="valueSpec viewValueExport Simplistic blue">
                            <div class="Box mainBox">
                                <asp:CheckBox runat="server" ID="exportUtilization" Checked="false" Text="Export Flights"
                                    CssClass="display_none" />
                                <table cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" valign="top">
                                            <asp:Label ID="company_name_label" runat="server" Visible="false" Text=""></asp:Label>
                                            <asp:Label runat="server" ID="aircraftInformationText"></asp:Label><asp:Panel runat="server"
                                                ID="dateSearchBoxes" CssClass="display_inline dateSearchBoxes">
                                                FROM&nbsp;
                        <asp:TextBox ID="faa_start_date" runat="server" Width="75"></asp:TextBox>
                                                TO&nbsp;
                        <asp:TextBox ID="faa_end_date" runat="server" Width="75"></asp:TextBox>
                                            </asp:Panel>
                                            <div class="float_right">
                                                <asp:Button runat="server" ValidationGroup="routeSearch" ID="searchFlight" Text="Search"
                                                    Style="margin-top: 0px;" />
                                                <input type="reset" id="resetButtonRoutes" style="margin-top: 0px;" onclick="resetRoutes();"
                                                    value="Clear Selections" />
                                            </div>
                                            <asp:Label runat="server" ID="acDisplayData"></asp:Label>
                                            <div class="clear">
                                            </div>
                                            <br />
                                            <asp:Panel ID="flight_search_options" runat="server" Width="100%" Visible="true"
                                                CssClass="mobile_padding faaFlightSearchPanel">
                                                <table cellpadding="0" cellspacing="0" align="left">
                                                    <tr>
                                                        <td width="55" valign="top" align="left" id="ownerLabelCell" runat="server">
                                                            <asp:Label ID="owner_text" runat="server" Text="Selections:"></asp:Label>
                                                        </td>
                                                        <td nowrap="nowrap" valign="top" align="left" width="100" runat="server" id="ownerWidth">
                                                            <asp:DropDownList ID="DropDownList_owner" runat="server" Width="100px">
                                                                <asp:ListItem Text="Lifetime" Value="lifetime"></asp:ListItem>
                                                                <asp:ListItem Text="Current Owner" Value="current"></asp:ListItem>
                                                                <asp:ListItem Text="Custom Date" Value="" Selected="True"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:Panel ID="routes_search_panel" runat="server" Visible="false" CssClass="float_left">
                                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="left" valign="top" width="55">
                                                                            <asp:Label ID="route_label" runat="server" Text="Route: "></asp:Label>
                                                                        </td>
                                                                        <td align="left" valign="top" width="305">
                                                                            <asp:DropDownList ID="route_selection" runat="server" AutoPostBack="true" Width="305px"
                                                                                Style="margin-bottom: 2px; margin-top: -2px;">
                                                                                <asp:ListItem Text="Both Directions" Value="0"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                        <td nowrap="nowrap" valign="top" align="left" width="330">
                                                            <asp:Panel ID="date_search_box" runat="server" Visible="false">
                                                                <table cellpadding='0' cellspacing='0' width="100%">
                                                                    <tr>
                                                                        <td nowrap="nowrap" valign="top" align="center" width="77"></td>
                                                                        <td valign="top" align="left"></td>
                                                                        <td nowrap="nowrap" valign="top" align="center" width="76" id="endDateWidth" runat="server"></td>
                                                                        <td valign="top" align="left"></td>
                                                                        <td valign="top" align="left" width="57">
                                                                            <asp:Button ID="search_date_range" runat="server" Text="Search" CssClass="searchButtonFAA display_none"
                                                                                AutoPostBack="true" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="route_analysis_panel" runat="server" Width="100%" Visible="false"
                                                CssClass="mobile_padding faaFlightSearchPanel">
                                                <table cellpadding="0" cellspacing="0" align="left" width="100%">
                                                    <tr>
                                                        <td nowrap="nowrap" valign="top" align="right">
                                                            <asp:ValidationSummary runat="server" ID="valSummary" ShowMessageBox="true" ValidationGroup="routeSearch"
                                                                ShowSummary="false" />
                                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td align="left" valign="top" width="48%">
                                                                        <asp:TextBox runat="server" ID="route" Width="100%" CssClass="startBox" placeholder="Origin:"></asp:TextBox><br />
                                                                        <asp:TextBox runat="server" ID="route_id" Width="100%" CssClass="display_none"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator runat="server" ID="routeIdVal" ControlToValidate="route_id"
                                                                            ValidationGroup="routeSearch" Display="None" ErrorMessage="Please pick a valid route"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <td align="center" valign="top" width="38">
                                                                        <asp:Label runat="server" ID="toFromLabel" CssClass="toFromLabel" Visible="false"></asp:Label>
                                                                        <img src="images/compare_arrows.png" width="30" id="swap" />
                                                                    </td>
                                                                    <td align="left" valign="top" width="48%">
                                                                        <asp:TextBox runat="server" ID="destination" Width="100%" CssClass="endBox" placeholder="Destination:"></asp:TextBox><br />
                                                                        <asp:TextBox runat="server" ID="destination_id" Width="100%" CssClass="display_none"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="destination_id"
                                                                            Display="None" ValidationGroup="routeSearch" ErrorMessage="Please pick a valid destination"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:CheckBox runat="server" ID="checkboxBoth" Text="Include flights for both directions?"
                                                                CssClass="float_left" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:DropDownList ID="DropDownList_timeframe" runat="server" AutoPostBack="true"
                                                Width="130px" CssClass="display_none">
                                                <asp:ListItem Text="3 Months" Value="90_days" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="12 Months" Value="last_year"></asp:ListItem>
                                                <asp:ListItem Text="Since Current Owner" Value="current"></asp:ListItem>
                                                <asp:ListItem Text="Lifetime" Value="all"></asp:ListItem>
                                                <asp:ListItem Text="Custom" Value="date_search"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="DropDownList_timeframe_dup" runat="server" AutoPostBack="true"
                                                Width="130px" Visible="false">
                                                <asp:ListItem Text="3 Months" Value="90_days" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="12 Months" Value="last_year"></asp:ListItem>
                                                <asp:ListItem Text="Since Current Owner" Value="current"></asp:ListItem>
                                                <asp:ListItem Text="Lifetime" Value="all"></asp:ListItem>
                                                <asp:ListItem Text="Custom" Value="date_search"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="right" width='50%' runat="server" id="flightAwareCell" visible="false">
                                            <asp:Label runat="server" ID="flight_aware_label" Visible="false" Width="900" Text="<img src='/pictures/company/flight_aware.jpg' align='right'>"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <cc1:TabContainer ID="main_tab_container" runat="server" CssClass="dark-theme" AutoPostBack="true"
                            Style="margin-left: auto; margin-right: auto; text-align: left;">
                            <cc1:TabPanel ID="flight_activity_tab" runat="server" Visible="true" HeaderText="Flights">
                                <HeaderTemplate>
                                    Flights
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div runat="server" id="flightDataContainer" class="MaxWidthRemove">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr valign='top'>
                                                <td width='550'>
                                                    <div class="specialTableContainer">
                                                        <asp:Literal ID="flight_data" runat="server"></asp:Literal>
                                                        <table id="flightData" class="refreshable">
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="origins_tab" runat="server" Visible="true" HeaderText="Origins">
                                <ContentTemplate>
                                    <div class="valueSpec viewValueExport Simplistic blue">
                                        <!-- <div style="vertical-align: top; height: 370px; overflow: auto; text-align: center;">-->
                                        <p>
                                            <asp:Literal ID="origins_tab_data" runat="server"></asp:Literal>
                                        </p>
                                        <!--  </div>  -->
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="destinations_tab" runat="server" Visible="true" HeaderText="Destinations">
                                <ContentTemplate>
                                    <div class="valueSpec viewValueExport Simplistic blue">
                                        <!--  <div style="vertical-align: top; height: 370px; overflow: auto; text-align: center;">-->
                                        <p>
                                            <asp:Literal ID="destinations_tab_data" runat="server"></asp:Literal>
                                        </p>
                                    </div>
                                    <!--  </div>-->
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="airframe_estimates_tab" runat="server" Visible="true" HeaderText="Airframe Estimates">
                                <ContentTemplate>
                                    <div class="valueSpec viewValueExport Simplistic blue">
                                        <!--  <div style="vertical-align: top; height: 370px; overflow: auto;">-->
                                        <p>
                                            <asp:Literal ID="airframe_estimates_tab_data" runat="server"></asp:Literal>
                                        </p>
                                    </div>
                                    <!--  </div>-->
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="activity_tab" runat="server" Visible="true" HeaderText="Utilization">
                                <ContentTemplate>
                                    <div class="valueSpec viewValueExport Simplistic blue">
                                        <asp:UpdatePanel ID="chart_panel" runat="server">
                                            <ContentTemplate>
                                                <div class="Box">
                                                    <asp:Label runat="server" Visible="false" ID="Flights_total_label" Text=""></asp:Label>
                                                    <asp:Label runat="server" Visible="true" ID="mapHeader" CssClass="subHeader"></asp:Label>
                                                    <div id="chart_div_survey" style="width: 96%; height: 350px; text-align: center; overflow: hidden; margin-left: 0px; margin-bottom: 0px;">
                                                    </div>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel ID="chart_text" runat="server">
                                            <ContentTemplate>
                                                <asp:Label runat="server" ID="chart_label"></asp:Label>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="routes_tab" runat="server" Visible="true" HeaderText="Routes">
                                <ContentTemplate>
                                    <div class="valueSpec viewValueExport Simplistic blue">
                                        <!--   <div style="vertical-align: top; height: 370px; overflow: auto;">-->
                                        <p>
                                            <asp:Label ID="routes_label" runat="server" Text=""></asp:Label>
                                        </p>
                                    </div>
                                    <!--  </div>-->
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="pairs_tab" runat="server" Visible="true" HeaderText="City Pairs">
                                <ContentTemplate>
                                    <div class="valueSpec viewValueExport Simplistic blue">
                                        <!--  <div style="vertical-align: top; height: 370px; overflow: auto;">-->
                                        <p>
                                            <asp:Label ID="city_pairs_label" runat="server" Text=""></asp:Label>
                                        </p>
                                    </div>
                                    <!--   </div>-->
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="aircraft_tab" runat="server" Visible="false" HeaderText="Aircraft">
                                <ContentTemplate>
                                    <div class="valueSpec viewValueExport Simplistic blue">
                                        <p>
                                            <asp:Label runat="server" ID="aircraft_tab_label" Text=""></asp:Label>
                                        </p>
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="operators_tab" runat="server" Visible="false" HeaderText="Operators">
                                <ContentTemplate>
                                    <div class="valueSpec viewValueExport Simplistic blue">
                                        <p>
                                            <asp:Label runat="server" ID="operators_tab_label" Text=""></asp:Label>
                                        </p>
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="live_flights_tab" runat="server" Visible="false" HeaderText="Live Flights">
                                <ContentTemplate>
                                    <div class="valueSpec viewValueExport Simplistic blue">
                                        <p>
                                            <asp:Label runat="server" ID="live_flights_label" Text=""></asp:Label>
                                        </p>
                                    </div>
                                </ContentTemplate>
                            </cc1:TabPanel>
                            <cc1:TabPanel ID="map_tab" runat="server" Visible="true" HeaderText="Flight Map">
                                <ContentTemplate>

                                    <div class="Box">
                                        <div class="row">
                                            <asp:CheckBox ID="search_flight_checkbox" runat="server" Visible="false" Text="" AutoPostBack="true" />
                                            <asp:Label runat="server" ID="search_label" Visible="false" Text="Display Comparable Refuel/Tech Stop for IAT/ICAO: "></asp:Label>
                                            <asp:TextBox ID="iata_icao_search" runat="server" Visible="false" Width="60"></asp:TextBox>
                                            <asp:Panel ID="map_panel" runat="server">
                                                <div id="map_canvas" style="width: 100%; height: 396px; text-align: center; margin-left: 0px; margin-right: 0px;">
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <br />
                                        <table width="95%" cellspacing="1" cellpadding="5" border="0">
                                            <tr>
                                                <td width="50%">
                                                    <div class="Box">
                                                        <asp:Label runat="server" ID="below_graph_label"></asp:Label>
                                                    </div>
                                                </td>
                                                <td width="50%">
                                                    <div class="Box">
                                                        <asp:Label runat="server" ID="below_graph_label2" Visible="false"></asp:Label>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
                                        <br />
                                    </div>

                                    <script type="text/javascript" language="javascript">

                                        function initialize() {
                                            var mapOptions = {
                                                zoom: 4,
                                                center: new google.maps.LatLng(39.2323, -95.8887),
                                                mapTypeId: google.maps.MapTypeId.ROADMAP
                                            };

                                            var mapDiv = document.getElementById("map_canvas");
                                            var map = new google.maps.Map(mapDiv, mapOptions);

                                            var marker = new google.maps.Marker({
                                                position: new google.maps.LatLng(39.2323, -95.8887),
                                                map: map,
                                                title: 'United States'
                                            });

                                        }

                                        //Building the tab Map 
                                        function BuildAirportMap(airport_name, latitude, longitude) {

                                            if (Number(latitude) == 0 && Number(longitude) == 0) { //not initalizing map, do not ignore this
                                                initialize();
                                                return false;
                                            }

                                            //Setting up the new options for the map.
                                            var mapOptions = {
                                                zoom: 2,
                                                center: new google.maps.LatLng(latitude, longitude),
                                                mapTypeId: google.maps.MapTypeId.ROADMAP
                                            }

                                            var mapDiv = document.getElementById("map_canvas");
                                            var map = new google.maps.Map(mapDiv, mapOptions);

                                            //finding the map.    
                                            if ((map != null) && (typeof (map) != "undefined")) {

                                                google.maps.event.clearListeners(window, 'resize');
                                                AddMarker(latitude, longitude, airport_name, 1, map);

                                            }
                                        }

                                        function AddListener(marker, title, counter, map_id) { //adding listener on click event. Basically adds a popup window with predetermined text on click event of marker.
                                            // alert("add list");
                                            var infowindow = new google.maps.InfoWindow();

                                            //Then go ahead and add the listener marker to the map.
                                            google.maps.event.addListener(marker, 'click', (function (marker, counter) {
                                                return function () {
                                                    infowindow.setContent(title);
                                                    infowindow.open(map_id, marker);
                                                }
                                            })(marker, counter));
                                        }

                                        function AddGeodesicLine(map, lat1, long1, lat2, long2) {

                                            var flightPathCoordinates = [new google.maps.LatLng(lat1, long1),
                                            new google.maps.LatLng(lat2, long2)];

                                            var line = new google.maps.Polyline({
                                                path: flightPathCoordinates,
                                                strokeColor: "#FF0000",
                                                strokeOpacity: 1.0,
                                                strokeWeight: 2,
                                                geodesic: true,
                                                map: map
                                            });
                                        }

                                        function AddGeodesicLine_Blue(map, lat1, long1, lat2, long2) {

                                            var flightPathCoordinates = [new google.maps.LatLng(lat1, long1),
                                            new google.maps.LatLng(lat2, long2)];

                                            var line = new google.maps.Polyline({
                                                path: flightPathCoordinates,
                                                strokeColor: "#0000FF",
                                                strokeOpacity: 1.0,
                                                strokeWeight: 2,
                                                geodesic: true,
                                                map: map
                                            });
                                        }

                                        function AddMarker(latitude, longitude, title, counter, map) {

                                            if ((map != null) && (typeof (map) != "undefined")) {

                                                //creating the marker for the map based on previously estabilished lat/long
                                                var marker = new google.maps.Marker({
                                                    position: new google.maps.LatLng(latitude, longitude),
                                                    map: map,
                                                    title: title
                                                });

                                                google.maps.event.clearListeners(marker, 'click');

                                                //adding a listener to map.
                                                AddListener(marker, title, counter, map);

                                            }

                                        }

                                    </script>

                                </ContentTemplate>
                            </cc1:TabPanel>
                        </cc1:TabContainer>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="jsonDiv">
    </div>

    <script>

        //    function readCookie(name) {
        //      var nameEQ = name + "=";
        //      var ca = document.cookie.split(';');
        //      for (var i = 0; i < ca.length; i++) {
        //        var c = ca[i];
        //        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        //        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        //      }
        //      return "";
        //    }


        //    function toggleCookie() {
        //      var cook = readCookie('showdiv');
        //      if (cook == 'false')
        //        $('#togglePanelJQ').hide();
        //      else
        //        $('#togglePanelJQ').show();
        //    }
        function resetRoutes() {
            $("#<%= route.clientID %>").attr("value", "");
            $("#<%= route_id.clientID %>").attr("value", "");
            $("#<%= destination.clientID %>").attr("value", "");
            $("#<%= destination_id.clientID %>").attr("value", "");
            $("#<%= checkboxBoth.clientID %>").removeAttr('checked');
        }
        function swap() {
            var route = $("#<%= route.clientID %>").val();
            var route_id = $("#<%= route_id.clientID %>").val();

            $("#<%= route.clientID %>").val($("#<%= destination.clientID %>").val());
            $("#<%= route_id.clientID %>").val($("#<%= destination_id.clientID %>").val());

            $("#<%= destination.clientID %>").val(route);
            $("#<%= destination_id.clientID %>").val(route_id);
        }

    //    function alignHeader() {
    //      var tabContainer = $find('<%=main_tab_container.ClientID %>');
        //      var index = tabContainer.get_activeTabIndex();
        //      $("#textToChangeOnTab").text(tabContainer.get_tabs()[index]._header.innerText)
        //    }
        //

        function setUpAutoComplete() {
            $("#swap").click(function () {
                swap();
            });

            $("#<%= route.clientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "GET",
                        url: "JSONresponse.aspx/Airport?term=" + request.term,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: {},
                        success: function (data) {
                            var json = data.d;
                            var obj = JSON.parse(json);
                            response(obj);
                        }
                    });
                },
                minLength: 3,
                select: function (event, ui) {
                    $("#<%= route.clientID %>").val(ui.item.label);
                    $("#<%= route_id.clientID %>").val(ui.item.value);
                    return false;
                }
            });

            $("#<%= destination.clientID %>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "GET",
                        url: "JSONresponse.aspx/Airport?term=" + request.term,
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: {},
                        success: function (data) {
                            var json = data.d;
                            var obj = JSON.parse(json);
                            response(obj);
                        }
                    });
                },
                minLength: 3,
                select: function (event, ui) {
                    $("#<%= destination.clientID %>").val(ui.item.label);
                    $("#<%= destination_id.clientID %>").val(ui.item.value);
                    return false;
                }
            });
        }

        function setUpLinkHover() {
            $("#link").hover(function () {
                $("#imagePreview").show();
            });

            $("#link").mouseleave(function () {
                $("#imagePreview").hide();
            });
        }
        function setUpLinks() { }
    </script>
    <div class="div_clear"></div><br />
</asp:Content>
