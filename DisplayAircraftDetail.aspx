<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="DisplayAircraftDetail.aspx.vb"
    Inherits="crmWebClient.DisplayAircraftDetail" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master" %>

<%@ MasterType VirtualPath="~/EvoStyles/EmptyEvoTheme.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAfbkfuHT2WoFs7kl-KlLqVYqWTtzMfDiE&sensor=false"></script>


    <script type="text/javascript">
        var data;
        var data_bar;
        var options = {
            curveType: 'function',
            vAxis: { title: "Clicks", maxValue: 20, minValue: -1 },
            legend: { position: 'top' }
        };

        function drawVisualization() {
            if (document.getElementById('visualization')) {
                // Create and draw the visualization.
                new google.visualization.LineChart(document.getElementById('visualization')).draw(data, options);
            }
        }

        function drawBarVisualization() {
            if (document.getElementById('visualization_bar')) {
                // Create and draw the visualization.
                new google.visualization.ColumnChart(document.getElementById('visualization_bar')).draw(data_bar, options);
            }
        }

        function openSmallWindowJS(address, windowname) {
            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1100,resizable=yes,toolbar=no,location=no,status=no");
        }

    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart', 'table'] });
    </script>
    <asp:TextBox runat="server" ID="parent_page_name" Text="" Style="display: none;" />
    <asp:TextBox runat="server" ID="parent_check_page_name" Text="AIRCRAFT_LISTING" Style="display: none;" />
    <asp:Panel runat="server" ID="history_background">
    </asp:Panel>
    <div id="divLoading" class="loadingScreenBox" style="display: none;">
        <span></span>
        <div class="loader">Loading...</div>
    </div>
    <div id="toggle_vis" class="aircraftContainer">
        <div id="outerDivAcDetailsID" class="valueSpec viewValueExport Simplistic aircraftSpec" runat="server">
            <div class="sixteen columns">
                <div class="row remove_margin">
                    <asp:Table ID="browseTable" CellSpacing="0" CellPadding="3" Width='98%' runat="server"
                        class="DetailsBrowseTable">
                        <asp:TableRow>
                            <asp:TableCell HorizontalAlign="left" VerticalAlign="top">
                                <div class="backgroundShade">
                                    <asp:UpdatePanel ID="control_update_panel" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="dropdownSettings-sub">
                                                <asp:LinkButton ID="LinkButton1" runat="server"><img src="images/menu.svg" alt="Menu" /></asp:LinkButton>
                                                <div class="dropdown-content-sub">
                                                    <div class="row">
                                                        <div class="six columns">
                                                            <a href="#"><strong>VIEW</strong></a>
                                                            <ul>
                                                                <li>
                                                                    <asp:LinkButton ID="view_notes" runat="server" Visible="false"
                                                                        OnClick="ViewAircraftNotes">Notes/Actions</asp:LinkButton></li>
                                                                <li>
                                                                    <asp:LinkButton ID="view_folders" runat="server"
                                                                        Visible="true" OnClick="ViewAircraftFolders">Folders</asp:LinkButton></li>
                                                                <li>
                                                                    <asp:LinkButton ID="view_aircraft_events" runat="server"
                                                                        OnClick="ViewAircraftEvents">Events</asp:LinkButton></li>
                                                                <li>
                                                                    <asp:LinkButton ID="map_this_aircraft" runat="server"
                                                                        OnClick="ViewAircraftMap">Map</asp:LinkButton></li>
                                                                <li>
                                                                    <asp:LinkButton ID="view_operator_history" Visible="false" runat="server" OnClick="ViewOperatorHistory">Operator History</asp:LinkButton></li>
                                                                <li>
                                                                    <asp:LinkButton ID="view_current_aircraft" Visible="false" runat="server">View Current Aircraft</asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </div>
                                                        <div class="six columns" id="view_ac_insight" runat="server" visible="false">
                                                            <a href="#" runat="server" id="intelDrop"><strong>Intel</strong></a>
                                                            <ul>
                                                                <li runat="server" visible="false" id="viewOwnershipToggle">
                                                                    <asp:LinkButton OnClick="ViewAircraftOwnership" ID="ownership_link" runat="server"></asp:LinkButton>
                                                                </li>
                                                                <li runat="server" visible="false" id="viewUtilToggle">
                                                                    <asp:Literal ID="util_link" runat="server"></asp:Literal>
                                                                </li>
                                                                <li runat="server" visible="false" id="viewAnalyticsToggle">
                                                                    <asp:LinkButton OnClick="ViewAircraftAnalytics" ID="analytics_link" runat="server"></asp:LinkButton>
                                                                </li>
                                                                <li runat="server" visible="false" id="viewProspectorToggle">
                                                                    <asp:Literal runat="server" ID="prospectorLink"></asp:Literal></li>
                                                                <li runat="server" visible="false" id="viewProspectToggle">
                                                                    <asp:LinkButton OnClick="ViewProspects" ID="ViewProspectsLink" runat="server">Prospects</asp:LinkButton></li>
                                                                <li runat="server" id="mobileTellChanges"><a href="javascript:void(0);" id="tellJetnetAboutChangesLinkIntel">Report Aircraft
                            Changes</a></li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <asp:Literal ID="ac_help_text" runat="server"><img src="images/help-circle.svg" alt="Help" /></asp:Literal>
                                            <a href="#" onclick="javascript:window.close();" class="float_right">
                                                <img src="images/x.svg" alt="Close" /></a>
                                            <div class="dropdownSettings-sub" runat="server" id="AddMenuItem" visible="false">
                                                <asp:LinkButton runat="server"><img src="images/edit.svg" alt="Edit" /></asp:LinkButton>
                                                <div class="dropdown-content-sub" style="right: 50px;">
                                                    <div class="row">
                                                        <div class="twelve columns">
                                                            <ul>

                                                                <li runat="server" id="edit_company_link" visible="false">Edit Company</li>
                                                                <asp:Literal runat="server" ID="viewOther" Visible="false"></asp:Literal>
                                                                <li runat="server" id="new_company_link" visible="false"><a href="#" onclick="javascript:window.open('/edit.aspx?action=new&amp;type=company&amp;Listing=1&amp;from=companyDetails');">New Company</a></li>

                                                                <li runat="server" id="Add_Note_Top" visible="false"></li>
                                                                <li runat="server" id="Add_Action_Top" visible="false"></li>
                                                                <li runat="server" id="Add_Prospect_Top" visible="false"></li>
                                                            </ul>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="dropdownSettings-sub" id="VALUES_UL" runat="server" visible="false">
                                                <a href="#" runat="server" id="Values_Drop"
                                                    visible="false">
                                                    <img src="images/dollar-sign.svg" alt="Values" /></a>
                                                <div class="dropdown-content-sub" style="min-width: 240px;">
                                                    <a href="#"><strong>VALUES</strong></a>
                                                    <ul>
                                                        <li runat="server" id="View_Values" visible="false">
                                                            <asp:Literal ID="ViewValuesViewLink" runat="server" Visible="false">Values View</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="AC_Estimates">
                                                            <asp:Literal ID="ViewACEstimatesLink" runat="server">Estimates for Aircraft</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="AC_Model_Estimates">
                                                            <asp:Literal ID="ViewACModelYearLink" runat="server">Estimates for Model By Year</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="AC_Model_Time">
                                                            <asp:Literal ID="ViewACModelTimeLink" runat="server">Estimates for Model By Time</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="AC_Current_Market">
                                                            <asp:Literal ID="ViewACCurrentMarket" runat="server">Current Market by Model</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="AC_Model_Residual_By_MFR">
                                                            <asp:Literal ID="ViewACResidualByMFR" runat="server">Residual Estimates By Model MFR Year</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="AC_Model_Residual">
                                                            <asp:Literal ID="ViewACResidual" runat="server">Residual Estimates for MY AC</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="AC_Model_AFTT">
                                                            <asp:Literal ID="ViewACModelAFTT" runat="server">Model Estimates by AFTT</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="AC_Assett_Summary">
                                                            <asp:Literal ID="ViewACAssett" runat="server">Assett Insight Summary</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="eValues_Toggle">
                                                            <asp:Literal ID="eValues_Toggle_Button" runat="server">Toggle eValues</asp:Literal>
                                                        </li>
                                                        <li runat="server" visible="false" id="eValues_Update_Estimate">
                                                            <asp:Literal ID="eValues_update_estimate_button" runat="server">Update eValues Estimate</asp:Literal>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="dropdownSettings-sub">
                                                <a href="#">
                                                    <img src="images/download.svg" alt="Download Exports/Reports" /></a>
                                                <div class="dropdown-content-sub">
                                                    <a href="#"><strong>EXPORT/REPORTS</strong></a>
                                                    <ul id="cssExportMenu" runat="server">
                                                        <li>
                                                            <asp:Label ID="single_spec_link" runat="server"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="condensed_spec_link" runat="server"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="full_spec_link" runat="server"></asp:Label></li>
                                                        <li>
                                                            <asp:Label ID="market_report_link" runat="server"></asp:Label></li>
                                                    </ul>
                                                    <div class="clearfix"></div>
                                                </div>
                                            </div>


                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>

                                <table class="notesRecord">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="PreviousACSwap" Visible="false"><input id="previousAC" type="button" value="<"  class="display_none" tooltip="Click to View the Previous Aircraft" /></asp:Label></td>
                                        <td>
                                            <asp:Panel runat="server" CssClass="NotesHeader" BackColor="#000000" ForeColor="White"
                                                ID="recordsOf">
                                                <asp:Label ID="browseTableTitle" runat="server" Text=""></asp:Label>
                                                <asp:Label runat="server" ID="browse_label">Record
                    <asp:Label ID="currentRecLabel" runat="server" Text="1"></asp:Label>
                                                    of
                    <asp:Label ID="totalRecLabel" runat="server" Text="1"></asp:Label>
                                                </asp:Label>
                                            </asp:Panel>
                                        </td>
                                        <td>
                                            <asp:Label runat="server" ID="NextACSwap"><input id="nextAC" type="button" value="&#9658;" class="display_none" tooltip="Click to View the Next Aircraft"/></asp:Label>
                                        </td>
                                    </tr>
                                </table>

                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                    <asp:UpdateProgress ID="splashScreen" runat="server" AssociatedUpdatePanelID="" DisplayAfter="500" class="loadingScreenBox">
                        <ProgressTemplate>
                            <span></span>
                            <div class="loader">Loading...</div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <asp:Label runat="server" ID="headerTextTitle"></asp:Label>
                <asp:Label runat="server" ID="historyHeaderTitle"></asp:Label>
                <div class="grid">
                    <!--Block 0 History Block-->
                    <asp:Label runat="server" ID="history__label" Visible="false" CssClass="grid-item"></asp:Label>
                    <!--Block 1 ID/Status/Airframe Block-->
                    <asp:Panel CssClass="grid-item" runat="server" ID="idContainer">
                        <asp:Label runat="server" ID="identification_label"></asp:Label>
                        <asp:Label runat="server" ID="status_label" Style="margin-top: 10px; display: block;"></asp:Label>
                        <asp:Label runat="server" ID="airframe_label" Style="margin-top: 10px; display: block;"></asp:Label>
                    </asp:Panel>
                    <!--Block 2 Events Block-->
                    <asp:UpdatePanel ID="events_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="eventContainer" CssClass="display_none">
                                <div class="grid-item">
                                    <div class="Box">
                                        <font class='subHeader padding_left'>Events</font>
                                        <div style="text-align: right; padding-right: 8px;">
                                            <asp:Label ID="newWindow" runat="server"></asp:Label>
                                        </div>
                                        <asp:Label ID="events_label" runat="server"></asp:Label>
                                        <br />
                                        <asp:LinkButton runat="server" ID="closeEvents" CssClass="float_right padding" OnClick="ViewAircraftEvents"
                                            Visible="false">Close Events</asp:LinkButton><a name="eventsView" class="blockAnchor"></a><div
                                                class="div_clear">
                                            </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--Block 3 Map Block-->
                    <asp:UpdatePanel ID="map_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <a name="map" class="blockAnchor"></a>
                            <asp:Panel runat="server" ID="mapContainer" CssClass="display_none">
                                <div class="grid-item">
                                    <div class="Box text_align_center">
                                        <div class='subHeader padding_left'>
                                            Map
                                        </div>
                                        <br />
                                        <div id="map_canvas" style="width: 96%; height: 550px; margin-left: auto; margin-right: auto;">
                                        </div>
                                        <br />
                                        <asp:LinkButton runat="server" ID="closeMap" CssClass="float_right padding" OnClick="ViewAircraftMap"
                                            Visible="false">Close Map</asp:LinkButton><div class="div_clear">
                                            </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--Block 4 Folders Block-->
                    <asp:UpdatePanel ID="folders_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="foldersContainer" CssClass="display_none">
                                <div class="grid-item">
                                    <div class="Box">
                                        <div class='subHeader padding_left'>
                                            FOLDERS
                                        </div>
                                        <br />
                                        <asp:Label ID="folders_label" runat="server"></asp:Label>
                                        <asp:LinkButton runat="server" ID="closeFolders" CssClass="float_right padding" OnClick="ViewAircraftFolders"
                                            Visible="false">Close Folders</asp:LinkButton><div class="div_clear">
                                            </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--Block Operator History Block-->
                    <asp:UpdatePanel ID="operator_history_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="operator_history_panel" CssClass="display_none">
                                <div class="grid-item">
                                    <div class="Box text_align_center">
                                        <div class='subHeader padding_left'>
                                            Operator History
                                        </div>
                                        <br />
                                        <asp:Label ID="operator_history_label" runat="server" CssClass="formatTable blue flightActivity"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--Block 5 Analytics Block-->
                    <asp:UpdatePanel ID="analytic_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="analyticContainer" runat="server" CssClass="display_none">
                                <div class="grid-item">
                                    <a name="analytics" class="blockAnchor"></a><a name="ownership" class="blockAnchor"></a>
                                    <div class="Box">
                                        <font class='subHeader padding_left'>Analytics</font>
                                        <div class="formatTable blue">
                                            <div class="header_row medium_text padding ">
                                                <b>
                                                    <asp:Label ID="clicks_label" runat="server" Text="Clicks per Month (Last 12 Months)"></asp:Label>
                                                </b>
                                            </div>
                                        </div>
                                        <div id="visualization" class="resizeChart">
                                        </div>
                                        <br />
                                        <br />
                                        <div>
                                            <asp:Label ID="analytic_label" runat="server" CssClass="panel_no_height"></asp:Label>
                                        </div>
                                        <hr />
                                        <asp:Panel runat="server" ID="toggle_for_sale_analytics">
                                            <br />
                                            <br />
                                            <div class="formatTable blue">
                                                <div class="header_row medium_text padding ">
                                                    <b>My Aircraft vs. Others of This Model (Since Listing Date)</b>
                                                </div>
                                            </div>
                                            <div id="visualization_bar" class="resizeChart">
                                            </div>
                                        </asp:Panel>
                                        <asp:LinkButton runat="server" ID="closeAnalytics" CssClass="float_right padding"
                                            OnClick="ViewAircraftAnalytics" Visible="false">Close Analytics</asp:LinkButton><div
                                                class="div_clear">
                                            </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--Block 6 Picture Block-->
                    <asp:Label ID="aircraft_picture_slideshow" runat="server" Text="" class="grid-item slideShowContainer"></asp:Label>
                    <!--Block 7 Company/Contact Block-->
                    <asp:UpdatePanel ID="contactUpdatePanel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Label ID="aircraft_contacts_label" runat="server" CssClass="grid-item"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--Block 8 Notes/Action Block-->
                    <asp:UpdatePanel ID="notes_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:panel class="grid-item" runat="server" visible="false" id="notesContainerItem"><a name="notes" class="blockAnchor"></a>
                                <div class="Box removeTopPadding">
                                    <asp:Panel runat="server" ID="notesPanel">

                                        <table class="formatTable blue" width="100%">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <div class="subHeader">
                                                        Notes
                              <asp:Label ID="notes_add_new" runat="server" CssClass="float_right smallLink upperCase display_inline_block"></asp:Label>
                                                    </div>
                                                    <asp:Label ID="notes_label" runat="server" Text=""></asp:Label>
                                                    <asp:Label ID="notes_all_label" runat="server" Text=""></asp:Label>
                                                    <asp:LinkButton runat="server" ID="notes_view_all" CssClass="float_left padding text_underline"
                                                        Visible="false" OnClientClick="document.body.style.cursor='wait';"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="actionPanel">
                                        <hr />
                                        <table class="formatTable blue" width="100%">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <div class="subHeader">
                                                        ACTION ITEMS<asp:Label ID="action_add_new" runat="server" CssClass="float_right smallLink"></asp:Label>
                                                    </div>
                                                    <asp:Label ID="action_label" runat="server"></asp:Label>
                                                    <asp:LinkButton runat="server" ID="closeNotes" CssClass="float_right padding" OnClick="ViewAircraftNotes"
                                                        Visible="false">Close Notes/Actions</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </asp:Panel>
                                </div>
                            </asp:panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <!--Block 9 Engines (put APU in this area as separate block) Block-->
                    <asp:Panel runat="server" CssClass="grid-item">
                        <asp:Label runat="server" ID="engine_label" Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="apu__label" Visible="false" Style="margin-top: 10px; display: block;"></asp:Label>
                        <asp:Label ID="propeller_tab_label" runat="server" Visible="false" Style="margin-top: 10px; display: block;"></asp:Label>
                    </asp:Panel>
                    <!--Block 10 Avoinics Block-->
                    <asp:Label runat="server" ID="avionics_label" CssClass="grid-item" Visible="false"></asp:Label>
                    <!--Block 11 Maintenance Block-->
                    <asp:Label runat="server" ID="maintenance_label" CssClass="grid-item" Visible="false"></asp:Label>
                    <!--Block 12 Features Block-->
                    <asp:Label ID="features_label" runat="server" CssClass="grid-item" Visible="false"></asp:Label>
                    <!--Block 13 Interior (put exterior in a separate block) Block-->
                    <asp:Panel runat="server" CssClass="grid-item">
                        <asp:Label runat="server" ID="interior_label" Visible="false"></asp:Label>
                        <asp:Label runat="server" ID="exterior_label" Visible="false" Style="margin-top: 10px; display: block;"></asp:Label>
                    </asp:Panel>
                    <!--Block 14 Additional Equipment Block-->
                    <asp:Label runat="server" ID="equipment_label" CssClass="grid-item" Visible="false"></asp:Label>
                    <!--Block 15 History Block-->
                    <asp:Label runat="server" ID="history_label" CssClass="grid-item"></asp:Label>
                    <!--Block 16 Valuation/Residual Block-->
                    <asp:UpdatePanel ID="valuesUpdatePanel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                                <asp:Label runat="server" ID="values_label" CssClass="grid-item"></asp:Label>
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
                    <!--Block 17 Flight Activity Block-->
                    <asp:Panel runat="server" ID="flightContainer" CssClass="display_none" Visible="false">
                        <div class="grid-item">
                            <a name="flight" class="blockAnchor"></a>
                            <div class="Box text_align_center">
                                <div class='subHeader padding_left'>
                                    Recent Flight Activity
                                </div>
                                <br />
                                <asp:Label ID="aircraft_flight_tab_label" runat="server" CssClass="formatTable blue flightActivity"></asp:Label>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Label ID="lease_tab_label" runat="server" CssClass="grid-item"></asp:Label>
                    <asp:Label ID="attributes_label" runat="server" CssClass="grid-item" Visible="false"></asp:Label>
                    <asp:Label runat="server" ID="cockpit_label" CssClass="grid-item" Visible="false"></asp:Label>
                    <asp:Label runat="server" ID="custom_label" CssClass="grid-item" Visible="false"></asp:Label>

                    <asp:TextBox ID="aircraftPageTitle" runat="server" CssClass="display_none"></asp:TextBox>
                    <asp:TextBox ID="aircraft_model" runat="server" CssClass="display_none"></asp:TextBox>
                    <asp:TextBox ID="jetnet_aircraft_id" runat="server" CssClass="display_none"></asp:TextBox>
                    <asp:TextBox ID="DOM" runat="server" CssClass="display_none"></asp:TextBox>
                    <asp:TextBox ID="Latitude" runat="server" CssClass="display_none"></asp:TextBox>
                    <asp:TextBox ID="Longitude" runat="server" CssClass="display_none"></asp:TextBox>


                    <cc1:TabContainer ID="aircraft_stats" runat="server" Visible="false" CssClass="dark-theme">
                        <cc1:TabPanel ID="stats_tab" runat="server" Visible="true">
                            <ContentTemplate>
                                <asp:Label ID="aircraft_information_label" runat="server"></asp:Label>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                    <asp:UpdatePanel ID="ProspectUpdate" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="prospectsContainer" CssClass="display_none">
                                <div class="grid-item">
                                    <div class="Box">
                                        <div class='subHeader padding_left'>
                                            PROSPECTS
                                        </div>
                                        <br />
                                        <asp:Label ID="prospects_label" runat="server"></asp:Label>
                                        <asp:LinkButton runat="server" ID="closeProspects" CssClass="float_right padding"
                                            OnClick="ViewProspects" Visible="false">Close Prospects</asp:LinkButton><div class="div_clear">
                                            </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="ownership_update_panel" runat="server" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel ID="ownership_panel" runat="server" CssClass="display_none">
                                <div class="grid-item">
                                    <div class="Box">
                                        <asp:Label ID="ownership_label" runat="server" CssClass="panel_no_height" Text="test"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <cc1:TabContainer ID="aircraft_appraisal_container" runat="server" Visible="false"
                        CssClass="blue-theme" AutoPostBack="false">
                        <cc1:TabPanel ID="aircraft_appraisal_tab" runat="server" HeaderText="APPRAISALS"
                            Visible="true">
                            <HeaderTemplate>
                                APPRAISALS
                
                
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:Label ID="aircraft_appraisal_label" runat="server"></asp:Label>

                                <asp:Label ID="appraisal_add_new" runat="server"></asp:Label>



                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                    <asp:UpdatePanel ID="prospects_update_panel" runat="server" Visible="false" ChildrenAsTriggers="false"
                        UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:Panel runat="server" ID="prospects_panel">
                                <span class="grid-item">
                                    <div class="Box removeTopPadding">
                                        <table class="formatTable blue" width="100%">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <div class="subHeader">
                                                        Prospects
                              <asp:Label ID="new_prospects_add" runat="server" CssClass="float_right smallLink"></asp:Label>
                                                    </div>
                                                    <asp:Label runat="server" ID="prospects_label2" CssClass="valueSpec viewValueExport Simplistic aircraftSpec"></asp:Label>
                                                    <asp:Label ID="prospects_all_label" runat="server" Text=""></asp:Label>
                                                    <asp:LinkButton runat="server" ID="prospects_view_all" CssClass="float_left padding"
                                                        Visible="false" OnClientClick="document.body.style.cursor='wait';"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </span>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Panel ID="edit_eValues" runat="server" Visible="false">
                        <div class="grid-item">
                            <div class="Box text_align_center">
                                <div class='subHeader padding_left'>
                                    EVALUE ESTIMATOR
                                </div>
                                <div class="Box  removeTopPadding">
                                    <table id='Aircraft_Identification_Status' class="formatTable" cellspacing='0' cellpadding='2'
                                        border="0" style="width: 80%;">
                                        <tr class='underlineRow'>
                                            <td colspan='2'>
                                                <table width='450'>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="estimator_label1" runat="server" CssClass="formatTable blue flightActivity"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr class='underlineRow' valign="bottom">
                                            <td nowrap='nowrap'>
                                                <font class='sub_text'><b>Data as Of: </b></font>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="estimator_as_of_date" runat="server" Width="65"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class='underlineRow' valign="bottom">
                                            <td nowrap='nowrap'>
                                                <font class='sub_text'><b>AFTT: </b></font>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="estimator_aftt" runat="server" Width="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class='underlineRow' valign="bottom">
                                            <td nowrap='nowrap'>
                                                <font class='sub_text'><b>Landings/Cycles: </b></font>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="estimator_landings" runat="server" Width="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class='underlineRow' valign="top">
                                            <td nowrap='nowrap'>
                                                <font class='sub_text'><b>Airframe Maintenance<br />
                            Program</b></font>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="estimator_airframe_program" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr class='underlineRow' valign="top">
                                            <td nowrap='nowrap'>
                                                <font class='sub_text'><b>Other Value<br />Related Changes</font>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="estimator_extra_info" runat="server" Width="270" Height="40" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr class='underlineRow' valign="bottom">
                                            <td align="left" colspan='2'>
                                                <asp:CheckBox ID="estimator_verify" runat="server" />
                                                By checking this box you acknowledge that you have verified the data above being
                          submitted to JETNET or believe the data was acquired from a reliable source.
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='2' align="right">
                                                <asp:Label ID="estimator_result" runat="server"></asp:Label>
                                                <asp:Button ID="estimator_submit" runat="server" Text="Submit eValue Update Request"
                                                    Width="199" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='2' align="left">
                                                <asp:Label ID="estimator_post_text" runat="server" Visible="false" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='2' align='left'>
                                                <asp:Label runat="server" ID="assett_click_label" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Button ID="cancel_update" runat="server" Text="Cancel" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <div id="statusBeforeAppend">
                    </div>
                    <div id="mobileAppend">
                    </div>
                </div>
                <br class="div_clear" />
                <br class="div_clear" />
                <asp:Label ID="aircraft_details_bottom" runat="server" Text=""></asp:Label>
            </div>
            <asp:Button runat="server" ID="contactUpdateButton" Text="updateContact" CssClass="display_none" />
            <asp:Button runat="server" ID="ContactUpdateCurrent" Text="updateContactCurrent" CssClass="display_none" />
            <span id="TellJetnetChangesContainer">
                <asp:Panel runat="server" ID="TellJetnetAboutChanges" Visible="false" class="sticky_bottom_position">
                    <a href="#" id="closeTellJetnetChanges">X</a>
                    <img src="images/arrowsCircle.png" width="36px" />
                    <a id="tellJetnetAboutChangesLink">TELL JETNET ABOUT CHANGES TO THIS AIRCRAFT</a>
                </asp:Panel>
            </span>
            <asp:Panel runat="server" Visible="false" ID="TellJetnetAboutChangesForm">
                <div id="notifyJetnetDialog" style="display: none;">
                    <iframe frameborder="0" width="100%" height="400px" id="notifyIframe" runat="server"></iframe>
                </div>
                <asp:Literal runat="server" ID="includeJqueryTheme"></asp:Literal>
                <asp:Label ID="amod_id" runat="server" Text="0" Visible="false"></asp:Label>
            </asp:Panel>
        </div>
    </div>
    <link href="common/contentslider.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="common/contentslider.js">
    /***********************************************
    * Featured Content Slider- (c) Dynamic Drive DHTML code library (www.dynamicdrive.com)
    * This notice MUST stay intact for legal use
    * Visit Dynamic Drive at http://www.dynamicdrive.com/ for this script and 100s more
    ***********************************************/
    </script>

    <script type="text/javascript" src="common/stepcarousel.js">
    /***********************************************
    * Step Carousel Viewer script- (c) Dynamic Drive DHTML code library (www.dynamicdrive.com)
    * Visit http://www.dynamicDrive.com for hundreds of DHTML scripts
    * This notice must stay intact for legal use
    ***********************************************/
    </script>

    <asp:Literal runat="server" ID="slideshow_script" Visible="false">
        <script type="text/javascript">

            featuredcontentslider.init({
                id: "slider1",  //id of main slider DIV
                contentsource: ["inline", ""],  //Valid values: ["inline", ""] or ["ajax", "path_to_file"]
                toc: "#increment",  //Valid values: "#increment", "markup", ["label1", "label2", etc]
                nextprev: ["", ""],  //labels for "prev" and "next" links. Set to "" to hide.
                revealtype: "click", //Behavior of pagination links to reveal the slides: "click" or "mouseover"
                enablefade: [true, 0.1],  //[true/false, fadedegree]
                autorotate: [true, 10000],  //[true/false, pausetime]
                onChange: function (previndex, curindex) {  //event handler fired whenever script changes slide
                    //previndex holds index of last slide viewed b4 current (1=1st slide, 2nd=2nd etc)
                    //curindex holds index of currently shown slide (1=1st slide, 2nd=2nd etc)
                }
            })

        </script> </asp:Literal>
    <asp:Literal ID="step_script" runat="server" Visible="false">
        <script type="text/javascript">
            stepcarousel.setup({
                galleryid: 'mygallery', //id of carousel DIV
                beltclass: 'belt', //class of inner "belt" DIV containing all the panel DIVs
                panelclass: 'panel', //class of panel DIVs each holding content
                autostep: { enable: true, moveby: 1, pause: 3000 },
                panelbehavior: { speed: 500, wraparound: false, wrapbehavior: 'slide', persist: false },
                defaultbuttons: { enable: true, moveby: 1, leftnav: ['images/previous.png', -2, 50], rightnav: ['images/next.png', -13, 50] },
                statusvars: ['statusA', 'statusB', 'statusC'], //register 3 variables that contain current panel (start), current panel (last), and total panels
                contenttype: ['inline'] //content setting ['inline'] or ['ajax', 'path_to_external_file']
            })

        </script> </asp:Literal>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script language="javascript" type="text/javascript">
        jQuery('.viewTransCompanies').click(function () {
            jQuery('#<%= ContactUpdateCurrent.ClientID %>').click();
        });

        jQuery('.viewCurrentCompanies').click(function () {
            jQuery('#<%= contactUpdateButton.ClientID %>').click();
        });

        var map;
        //This is going to run on window load.
        window.onload = function () {
            //document.getElementById("divLoading").className = "display_none";
            //document.getElementById("toggle_vis").className = "display_block";
        }
        function removeVis() {
            document.getElementById("divLoading").style.display = 'none';
        }
        function ToggleVis() {
            //document.getElementById("toggle_vis").className = "display_none";

            //             if (document.getElementById("prev_button_slide") != null) {
            //                document.getElementById("prev_button_slide").className = "display_none";
            //             }
            //             if (document.getElementById("next_button_slide") != null) {
            //                document.getElementById("next_button_slide").className = "display_none";
            //              }
            ToggleButtons();
            document.getElementById("divLoading").style.display = 'block';
        }


    </script>
    <asp:Literal runat="server" ID="mobileAdditionScript" Visible="false"></asp:Literal>
    <script type="text/javascript">


        window.onload = function () {
            setTimeout(function(){loadMasonry(); }, 400);

            var parent = '';

            if ((window.opener) && (window.opener.location.href)) {
                parent = String(window.opener.location);
                parent = parent.toUpperCase();
            }

            document.getElementById("<%= parent_page_name.clientID %>").value = parent;
            var n = parent.indexOf(document.getElementById("<%= parent_check_page_name.clientID %>").value);
            var hist = parent.indexOf("H=1");
            var even = parent.indexOf("E=1");


            var invis = false

            if (n == -1) {
                invis = true;
            }

            if (hist != -1) {
                //   invis = true;
            }

            if (even != -1) {
                invis = true;
            }


            if (invis == true) {

                var nextElement = document.getElementById("nextAC");
                if (nextElement != null) {
                    document.getElementById("nextAC").style.cssText = 'display:none !important';
                }
                //check for previous
                var previousElement = document.getElementById("previousAC");
                if (previousElement != null) {
                    document.getElementById("previousAC").style.cssText = 'display:none !important';
                }
                //check if browse element exists.
                var browseElement = document.getElementById("<%= browse_label.clientID %>");
                if (browseElement != null) {
                    document.getElementById("<%= browse_label.clientID %>").style.cssText = 'display:none !important';
                }
            }


        }


        function swapHistoryToggle() {
            if (jQuery(".hideHistory")[0]) {
                jQuery(".hideHistory")
                    .removeClass("hideHistory")
                    .addClass("showHistory");
                jQuery("#viewAllHistoryButton").text("VIEW HISTORY (1 YEAR)");
                jQuery("#viewAllHistoryButton").focus();
            } else {
                jQuery(".showHistory")
                    .removeClass("showHistory")
                    .addClass("hideHistory");
                jQuery("#viewAllHistoryButton").text("VIEW ALL HISTORY");
                jQuery("#viewAllHistoryButton").focus();

            }
            if (jQuery(".noRowsShow")[0]) {
                jQuery(".noRowsShow")
                    .removeClass("noRowsShow")
                    .addClass("noRowsHide");
            } else {
                jQuery(".noRowsHide")
                    .removeClass("noRowsHide")
                    .addClass("noRowsShow");
            }
            loadMasonry();
        }

    </script>

</asp:Content>
