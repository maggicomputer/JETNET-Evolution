<%@ Page Title="" Language="vb" AutoEventWireup="true" MasterPageFile="~/EvoStyles/EmptyEvoTheme.Master"
    CodeBehind="aircraftFinder.aspx.vb" Inherits="crmWebClient.aircraftFinder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="common/aircraft_model.css" type="text/css" rel="stylesheet" />


    <script type="text/javascript" src="/common/moment-with-locales.js"></script>

    <script type="text/javascript">

        function openSmallWindowJS(address, windowname) {
            var rightNow = new Date();
            windowname += rightNow.getTime();
            var Place = window.open(address, windowname, "scrollbars=yes,menubar=yes,height=800,width=1050,resizable=yes,toolbar=no,location=no,status=no");
            return true;
        }

    </script>

    <style type="text/css">
        select {
            margin-left: 9px;
            padding: 2px;
        }

        .dataTables_scrollHead {
            width: auto !important;
        }

        .bxslider {
            width: 100%;
        }

        .ui-state-default, .ui-widget-content .ui-state-default, .ui-widget-header .ui-state-default {
            border: 1px solid #d3d3d3;
            background: #078fd7 50% 50% repeat-x;
            font-weight: normal;
            color: #555555;
        }

        .searchPanelContainerDiv .amountFinder:focus {
            background: transparent !important;
            border: 0;
            text-align: right;
            margin-top: -5px;
            font-size: 12px;
            width: 70px;
            box-shadow: 0 0 0 #ffffff;
            -webkit-box-shadow: 0 0 0 #ffffff;
        }

        .searchPanelContainerDiv .chosen-container {
            position: relative !important;
        }

        .searchSummary li {
            font-weight: bold;
            text-transform: uppercase;
        }

            .searchSummary li ul li {
                font-weight: normal;
                text-transform: none;
                border-bottom: 1px solid #eee;
            }

        .editLink {
            padding-left: 1em;
            text-transform: lowercase !important;
            font-weight: normal !important;
            cursor: pointer;
            color: #078fd7 !important;
        }

        td.select-checkbox {
            color: #444 !important;
        }

        input, div, a {
            color: transparent !important;
            text-shadow: 0 0 0 #444 !important;
            &:focus

        {
            outline: none !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel runat="server" ID="contentClass" CssClass="valueViewPDFExport remove_padding mainHolder">
        <div id="searchPanelContainerDiv" runat="server" class="center_outer_div" width="1050">
            <asp:Table ID="buttonsTable" CellPadding="3" CellSpacing="0" Width="100%" CssClass="DetailsBrowseTable"
                runat="server">
                <asp:TableRow>
                    <asp:TableCell ID="TableCell1" runat="server" HorizontalAlign="right" VerticalAlign="middle"
                        Style="padding-right: 4px;" Width="23%">
            <div class="backgroundShade">
              <a href="#" onclick="javascript:load('https://www.jetnet.com/help/documents/865.pdf','','');" class="help_cursor"><img src="images/help-circle.svg" alt="Help" /></a> <span class="float_right">
                  <a href="#" onclick="javascript:window.close();">
                    <img src="images/x.svg" alt="Close" /></a> </span>
            </div>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <div class="valueSpec viewValueExport Simplistic aircraftSpec plain">
                <table border="0" cellpadding="3" cellspacing="0" width="98%" align="center">
                    <tr>
                        <td height="30%" width="100%">
                            <br />
                            <div class="row">
                                <div class="seven columns Box">
                                    <div class="bxslider" runat="server" id="sliderBX">
                                        <div class="child">
                                            <div class="row">
                                                <div class="columns twelve">
                                                    <h2>
                                                        <strong>
                                                            <asp:Label ID="Label5" runat="server" Text="AIRCRAFT TYPE"></asp:Label>
                                                        </strong>
                                                        <asp:Label ID="Label6" runat="server" Text="PREFRENCES"></asp:Label>
                                                    </h2>
                                                    <asp:Panel ID="Panel5" runat="server" CssClass="searchPanelContainerDiv" Style="padding-left: 10px; clear: both">
                                                        <asp:TextBox runat="server" ID="selected_type_rows" CssClass="display_none"></asp:TextBox>
                                                        <div style="text-align: center; width: 100%;" runat="server" id="aircraftTypeResultsDiv">
                                                            <asp:Label ID="aircraftTypeTable" runat="server"></asp:Label>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="child">
                                            <div class="row">
                                                <div class="columns twelve">
                                                    <h2>
                                                        <strong>
                                                            <asp:Label ID="section_title1" runat="server" Text="MISSION/TRIP"></asp:Label>
                                                        </strong>
                                                        <asp:Label ID="section_sub_label1" runat="server" Text="REQUIREMENTS"></asp:Label>
                                                    </h2>
                                                    <asp:Panel ID="Panel0" runat="server" CssClass="searchPanelContainerDiv" Style="padding-left: 10px; clear: both">
                                                        <div class="row div_clear toggleSmallScreen">
                                                            <div class="twelve columns float_left">
                                                                <label>
                                                                    Range: Must fly a minimum of</label><asp:DropDownList runat="server" ID="minimumRangeDropdown">
                                                                        <asp:ListItem Value="0">Not Specified</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                <label>
                                                                    (nm) non-stop.</label>
                                                            </div>
                                                            <!--<div class="five columns removeLeftMargin">
                                <div id="slider-range">
                                </div>

                              </div>
                              <div class="three columns removeLeftMargin">
                                <label>
                                  (nm)
                                </label>
                              </div>-->
                                                        </div>
                                                        <div class="row toggleSmallScreen">
                                                            <div class="twelve columns float_left">
                                                                <label>
                                                                    Passengers: Must carry a minimum of</label><asp:DropDownList runat="server" ID="minimumPassengersDropdown">
                                                                        <asp:ListItem Value="0">Not Specified</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                <label>
                                                                    passengers in addition to crew.</label>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <h2>
                                                        <strong>
                                                            <asp:Label ID="section_title2" runat="server" Text="PURCHASE PRICE/USE"></asp:Label>
                                                        </strong>
                                                        <asp:Label ID="section_sub_label2" runat="server" Text="PREFERENCES"></asp:Label>
                                                    </h2>
                                                    <asp:Panel ID="Panel1" runat="server" CssClass="searchPanelContainerDiv" Style="padding-left: 10px; clear: both">
                                                        <div id="price_range_div" class="row toggleSmallScreen">
                                                            <div class="twelve columns float_left">
                                                                <label>
                                                                    Price: Must have maximum asking price (or estimated value) less than
                                                                </label>
                                                                <asp:DropDownList runat="server" ID="maximumPriceDropdown">
                                                                    <asp:ListItem Value="0">Not specified</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label>
                                                                    k dollars.</label>
                                                            </div>
                                                        </div>
                                                        <div class="row toggleSmallScreen">
                                                            <div class="twelve columns removeLeftMargin">
                                                                <label>
                                                                    Market Status: Include aircraft</label>
                                                                <asp:DropDownList runat="server" ID="market_status">
                                                                    <asp:ListItem>For Sale and Not For Sale</asp:ListItem>
                                                                    <asp:ListItem Value="Y">For Sale Only</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="row toggleSmallScreen">
                                                            <div class="twelve columns float_left">
                                                                <label>
                                                                    Age: Only include aircraft manufactured from</label><asp:DropDownList runat="server"
                                                                        ID="yearDropdownStart">
                                                                        <asp:ListItem Value="0">Any</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                <label>
                                                                    through</label><asp:DropDownList runat="server" ID="yearDropdownEnd">
                                                                        <asp:ListItem Value="0">Any</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                <label>
                                                                    year</label>.
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <asp:Panel ID="Panel2" runat="server" CssClass="searchPanelContainerDiv" Style="padding-left: 10px; clear: both">
                                                        <div class="row toggleSmallScreen">
                                                            <div class="twelve columns float_left">
                                                                <label>
                                                                    Airframe Hours: Include aircraft with a maximum of</label>
                                                                <asp:DropDownList runat="server" ID="maximumAFTTDropdown">
                                                                    <asp:ListItem Value="0">Any</asp:ListItem>
                                                                </asp:DropDownList>
                                                                <label>
                                                                    airframe hours.</label>
                                                            </div>
                                                        </div>
                                                        <div class="row toggleSmallScreen">
                                                            <div class="twelve columns float_left">
                                                                <label>
                                                                    Include Aircraft with location</label>
                                                                <asp:DropDownList ID="aircraft_registration" runat="server">
                                                                    <asp:ListItem Text="Anywhere in US" Value="N"></asp:ListItem>
                                                                    <asp:ListItem Text="Anywhere International" Value="I"></asp:ListItem>
                                                                    <asp:ListItem Selected="True" Text="Anywhere" Value="Worldwide"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="child">
                                            <div class="row">
                                                <div class="columns twelve">
                                                    <h2>
                                                        <strong>
                                                            <asp:Label ID="Label1" runat="server" Text="FEATURE/COMFORT"></asp:Label>
                                                        </strong>
                                                        <asp:Label ID="Label2" runat="server" Text="PREFERENCES"></asp:Label>
                                                    </h2>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="columns twelve">
                                                    <asp:Panel ID="Panel3" runat="server" CssClass="searchPanelContainerDiv" Style="padding-left: 15px; clear: both">
                                                        <label>
                                                            Check all that apply</label>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<div
                                                            class="checkboxMenu">
                                                            <div class="checkboxDiv">
                                                                <asp:CheckBoxList ID="comfortFeatursCBL" runat="server" RepeatColumns="3" Width="100%"
                                                                    RepeatDirection="Horizontal" CellPadding="2">
                                                                    <asp:ListItem Text="Comfort Feature"></asp:ListItem>
                                                                </asp:CheckBoxList>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="child">
                                            <div class="row">
                                                <div class="columns twelve">
                                                    <h2>
                                                        <strong>
                                                            <asp:Label ID="Label3" runat="server" Text="OPERATION/MAINTENANCE"></asp:Label>
                                                        </strong>
                                                        <asp:Label ID="Label4" runat="server" Text="REQUIREMENTS"></asp:Label>
                                                    </h2>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="columns twelve">
                                                    <asp:Panel ID="Panel4" runat="server" CssClass="searchPanelContainerDiv" Style="padding-left: 15px; clear: both">
                                                        <div class="row">
                                                            <div class="six columns removeLeftMargin">
                                                                <asp:CheckBox runat="server" ID="airframe_maintenance_program" Text="On Airframe Maintenance Program?" />
                                                            </div>
                                                            <div class="six columns removeLeftMargin">
                                                                <asp:CheckBox runat="server" ID="engine_maintenance_program" Text="On Engine Maintenance Program?" />
                                                            </div>
                                                        </div>
                                                        <div class="row" style="visibility: hidden" runat="server" id="maintenancePanel">
                                                            <div class="six columns removeLeftMargin">
                                                                &nbsp;
                                                            </div>
                                                            <div class="six columns removeLeftMargin">
                                                                <asp:DropDownList runat="server" ID="engine_maintenance_dropdown">
                                                                    <asp:ListItem Value="">Any</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="five columns Box">
                                    <asp:UpdatePanel runat="server" ID="topUpdatePanel">
                                        <ContentTemplate>
                                            <input type="button" value="Find Aircraft" class="button_width float_right" onclick="clickSearch();" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <h2>
                                        <strong>PREFERENCES</strong></h2>
                                    <ul class="searchSummary">
                                        <li>Aircraft Type <a id="type_edit" title="Edit Aircraft Type Preferences" data="0"
                                            class="editLink">edit</a>
                                            <ul>
                                                <li id="typeText">Not Specified.</li>
                                            </ul>
                                        </li>
                                        <li>Mission/Trip <a id="trip_edit" class="editLink" title="Edit Mission/Trip Requirements"
                                            data="1">edit</a>
                                            <ul>
                                                <li id="rangeText">Minimum Range not specified.</li>
                                                <li id="paxText">Minimum Passengers not specified.</li>
                                            </ul>
                                        </li>
                                        <li>Purchase Price/Use <a id="purchase_edit" title="Edit Purchase Price/Use Preferences"
                                            data="1" class="editLink">edit</a>
                                            <ul>
                                                <li id="statusText"></li>
                                                <li id="priceText" class="display_none"></li>
                                                <li id="yearText"></li>
                                                <li id="afttText"></li>
                                                <li id="locationText"></li>
                                            </ul>
                                        </li>
                                        <li>Feature/Comfort <a id="feature_edit" title="Edit Feature/Comfort Preferences"
                                            data="2" class="editLink">edit</a>
                                            <ul>
                                                <li id="featuresText">Not Specified.</li>
                                            </ul>
                                        </li>
                                        <li>Operation/Maintenance <a id="operation_edit" title="Edit Operation/Maintenance Requirements"
                                            data="3" class="editLink">edit</a>
                                            <ul>
                                                <li id="airframeText">Airframe Maintenance Program: Not Specified.</li>
                                                <li id="engineText">Engine Maintenance Program: Not Specified.</li>
                                            </ul>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div style='max-height: 470px; overflow: auto;'>
                                <asp:Label ID="english_summary" runat="server" Text=""></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:UpdatePanel runat="server" ID="tabContainerBottomUpdate" ChildrenAsTriggers="true"
                                UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="Box">
                                        <asp:Button ID="findAC" runat="server" Text="Find Aircraft" CssClass="display_none"
                                            OnClientClick="javascript:ShowLoadingMessage('DivLoadingMessage', 'Finding Aircraft', 'Searching ... Please Wait ...');return true;" />
                                        <asp:TextBox runat="server" ID="selected_aircraft_rows" CssClass="display_none"></asp:TextBox>
                                        <div runat="server" id="div_aircraft_results_table" class="sixteen columns removeLeftMargin">
                                            <div style="text-align: center; width: 100%;" runat="server" id="acSearchResultsDiv">
                                                <asp:Label ID="acSearchResultsTable" runat="server" Text=""></asp:Label>
                                                <div id="acSearchResultsContainer">
                                                    <table id="searchDataTable" cellpadding="0" cellspacing="0" border="0" width="100%"></table>
                                                    <div id="searchInnerTable" align="left" valign="middle" style="max-height: 470px; overflow: auto;">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br clear="all" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="DivLoadingMessage" style="display: none;">
        </div>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="below_form" runat="server">

    <script type="text/javascript">

        function setUpSliderInitial() {

            bxSliderVar = $('.bxslider').bxSlider({
                auto: false, responsive: true, slideSelector: '.child',
                autoControls: false,
                stopAutoOnClick: true,
                pagerCustom: '.customControls',
                pager: false,
                onSliderLoad: function (index) {
                    if (index === 0) {
                        $('.bx-prev').addClass("display_none");
                    };
                    if (index === 3) {
                        $('.bx-next').addClass("display_none");
                    };
                },
                onSlideAfter: function (slide, oldIndex, newIndex) {
                    if (newIndex === 3) {
                        $('.bx-next').addClass("display_none");
                        $('.bx-prev').removeClass("display_none");
                    } else {
                        $('.bx-prev').removeClass("display_none");
                        $('.bx-next').removeClass("display_none");
                        if (newIndex === 0) {
                            $('.bx-prev').addClass("display_none");
                        };
                    }
                },
                infiniteLoop: false
            }); $('.bx-prev').addClass("display_none");

            $('#type_edit,#trip_edit,#purchase_edit,#feature_edit,#operation_edit').click(function () {
                bxSliderVar.goToSlide(parseInt($(this).attr('data')));
            });
        }
        function setUpSlider() {
            setTimeout(function () {
                setUpSliderInitial()
            }, 3000);
        }


        function BuildSummary() {


            if (parseInt($("#<%= minimumPassengersDropdown.clientID %> option:selected").val()) == 0) {
                $('#paxText').text('Minimum Passengers not specified.');
            } else {
                $('#paxText').text('Passengers: Must carry a minimum of ' + $("#<%= minimumPassengersDropdown.clientID %> option:selected").val() + ' passengers in addition to crew.');
            }
            if (parseInt($("#<%= minimumRangeDropdown.clientID %> option:selected").val()) == 0) {
                $('#rangeText').text('Minimum Range not specified.');
            } else {
                $('#rangeText').text('Range: Must fly a minimum of ' + $("#<%= minimumRangeDropdown.clientID %> option:selected").val() + ' (nm) non-stop.');
            }
            if (parseInt($("#<%= maximumAFTTDropdown.clientID %> option:selected").val()) == 0) {
                $('#afttText').text('Maximum AFTT not specified.');
            } else {
                $('#afttText').text('Maximum AFTT of ' + $("#<%= maximumAFTTDropdown.clientID %> option:selected").text() + ' hrs.');
            }

            if ($("#<%= aircraft_registration.clientID %> option:selected").val() == 'Worldwide') {
                $('#locationText').text('Location not specified.');
            } else {
                $('#locationText').text('Location ' + $("#<%= aircraft_registration.clientID %> option:selected").text() + '.');
            }


            if ($('#<%=market_status.clientID %> option:selected').val() == 'Y') {
                $('#statusText').text('Only For Sale Aircraft.');
            } else {
                $('#statusText').text('Market Status not specified.');
            }

            if (parseInt($("#<%= maximumPriceDropdown.clientID %> option:selected").val()) > 0) {
                $('#statusText').removeClass('display_none');
                var DisplayPrice = $("#<%= maximumPriceDropdown.clientID %> option:selected").text();
                $('#statusText').text('Price less than  ' + AddCommas(DisplayPrice) + 'k dollars.');
            } else {
                $('#statusText').text('Price not specified.');
            }
            var featuresListStr = '';
            $('#<%=comfortFeatursCBL.clientID %> input:checked').each(function () {
                if (featuresListStr != '') {
                    featuresListStr += ', '
                }
                featuresListStr += $("label[for='" + this.id + "']").text();
            });

            if (featuresListStr != '') {
                $(featuresText).text(featuresListStr);
            } else {
                $(featuresText).text('Not Specified.');
            }

            if ($('#<%=airframe_maintenance_program.clientID %>').prop('checked')) {
                $("#airframeText").text('On an Airframe Maintenance Program.');
            }

            if ($('#<%=engine_maintenance_program.clientID %>').prop('checked')) {
                var DisplayProgram = $("#<%= engine_maintenance_dropdown.clientID %> option:selected").text();
                if (DisplayProgram !== '') {
                    DisplayProgram = ' - ' + DisplayProgram
                }
                $("#engineText").text('On an Engine Maintenance Program' + DisplayProgram + '.');
            }

            if (parseInt($("#<%= yearDropdownStart.clientID %> option:selected").val()) == 0 && parseInt($("#<%= yearDropdownEnd.clientID %> option:selected").val()) == 0) {
                $('#yearText').text('Age Range not specified.');
            } else if (parseInt($("#<%= yearDropdownStart.clientID %> option:selected").val()) > 0 && parseInt($("#<%= yearDropdownEnd.clientID %> option:selected").val()) > 0) {
                if (parseInt($("#<%= yearDropdownStart.clientID %> option:selected").val()) >= parseInt($("#<%= yearDropdownEnd.clientID %> option:selected").val())) {
                    alert('Your start age has been automatically adjusted to be lower than the end age.');
                    $("#<%= yearDropdownStart.clientID %>").val(parseInt($("#<%= yearDropdownEnd.clientID %> option:selected").val()) - 1);
                $('#yearText').text('Age Range between ' + $("#<%= yearDropdownStart.clientID %> option:selected").val() + '-' + $("#<%= yearDropdownEnd.clientID %> option:selected").val() + '.');
            } else {
                $('#yearText').text('Age Range between ' + $("#<%= yearDropdownStart.clientID %> option:selected").val() + '-' + $("#<%= yearDropdownEnd.clientID %> option:selected").val() + '.');
                }
            } else if (parseInt($("#<%= yearDropdownStart.clientID %> option:selected").val()) > 0) {
                $('#yearText').text('Age greater than ' + $("#<%= yearDropdownStart.clientID %> option:selected").val() + '.');
            } else if (parseInt($("#<%= yearDropdownEnd.clientID %> option:selected").val()) > 0) {
                $('#yearText').text('Age less than ' + $("#<%= yearDropdownEnd.clientID %> option:selected").val() + '.');
            }
        }

        var startWindow;

        function ActiveTabChanged(sender, args) {

            var nextTab = sender.get_activeTab().get_id();

            if (nextTab.indexOf("finder_preferences") > 0) {
                //alert("finder preferences");
                swapChosenDropdowns();
            }

        }

        function swapChosenDropdowns() {
            $(".chosen-select").chosen("destroy");
            $(".chosen-select").chosen({ no_results_text: "No results found.", disable_search_threshold: 10 });
        };

        function ShowLoadingMessage(DivTag, Title, Message) {
            $("#" + DivTag).html(Message);
            $("#" + DivTag).dialog({ modal: true, title: Title, width: 395, height: 75, resizable: false });
        }

        function CloseLoadingMessage(DivTag) {
            $("#" + DivTag).dialog("close");
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
                if (typeof rowSelected !== "undefined") {
                    if (IDsToUse.length == 0) {
                        IDsToUse = value[1];
                    } else {
                        IDsToUse += ', ' + value[1];
                    }
                } else {
                    IDsToUse = value[1];
                }
                count += 1;
            });

            $("#" + selectedRows).val(IDsToUse);

        }

        function setRowSelected(data, selectedRows, tableName) {

            if (selectedRows != '') {

                //alert("sel:" + selectedRows);

                var rowSelected = null;

                rowSelected = selectedRows.split(", ");

                if (typeof rowSelected !== "undefined") {

                    data.each(function (value, index) {

                        for (var i = 0; i < rowSelected.length; i++) {

                            if (value[1] == rowSelected[i]) {

                                var row = $("#" + tableName).DataTable().row(rowSelected[i]).node();

                                //alert("idx:" + index + "row:" + row);

                                $(row).addClass("selected");

                            }

                        }

                    });

                }
            }
        }

        function CreateTheDatatable(divName, tableName, jQueryTablename) {

            var selectedRows = '';

            try {
                if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
                    $("#" + divName).empty();
                };
            }
            catch (err) {
                //alert("make datatable error");
            }

            if ($("#" + tableName).length) {

                selectedRows = "<%= selected_type_rows.clientID %>";

                //jQuery("#" + tableName).css('display', 'block');

                var clone = jQuery("#" + tableName).clone(true);

                jQuery("#" + tableName).css('display', 'none');
                clone[0].setAttribute('id', jQueryTablename);
                clone.appendTo("#" + divName);



                var table = $("#" + jQueryTablename).DataTable({
                    ordering: false,
                    destroy: true,
                    fixedHeader: true,
                    "initComplete": function (settings, json) {
                        setTimeout(function () {
                            $("#" + jQueryTablename).DataTable().columns.adjust();
                            $("#" + jQueryTablename).DataTable().scroller.measure();

                            var dataRows = $("#" + jQueryTablename).DataTable().rows();
                            var selectedValues = $("#" + selectedRows).val();

                            setRowSelected(dataRows.data(), selectedValues, jQueryTablename);

                        }, 1200)
                    },
                    scrollCollapse: true,
                    stateSave: true,
                    paging: false,
                    rowId: [1],
                    columnDefs: [
                        { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] },
                        { targets: [1], className: 'display_none', name: 'typeIdx' }
                    ],
                    select: { style: 'multi', selector: 'td:first-child' },
                    order: [[4, 'asc']],
                    dom: 'Btrp',
                    rowID: 'typeIdx',
                    buttons: []
                });
            }

            $("#" + jQueryTablename).on('select.dt deselect.dt', function (e, dt, type, indexes) {
                var rows = dt.rows({ selected: true }).indexes();
                var data = dt.cells(rows, 1).data();
                var dataDisplay = dt.cells(rows, 2).data();
                var IDsToUse = '';
                var dataDisplayText = '';
                data.each(function (value, index) {
                    if (IDsToUse.length == 0) {
                        IDsToUse = value;
                    } else {
                        IDsToUse += ', ' + value;
                    }
                });

                dataDisplay.each(function (value, index) {
                    if (dataDisplayText.length == 0) {
                        dataDisplayText = value;
                    } else {
                        dataDisplayText += ', ' + value;
                    }
                });
                if (dataDisplayText != '') {
                    $('#typeText').html(dataDisplayText);
                } else {
                    $('#typeText').text('Not Specified.');
                }
                $("#" + selectedRows).val(IDsToUse);
            });

            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();
        };

        function CreateSearchTable(divName, tableName, jQueryTablename) {

            var selectedRows = '';

            try {
                if ($.fn.DataTable.isDataTable("#" + jQueryTablename)) {
                    $("#" + divName).empty();
                };

            }
            catch (err) {

            }

            switch (tableName) {
                case "searchDataTable":
                    {
                        dynamicDataSet = dataSetAC;
                        columnSetArray = [
                            { title: "SEL", width: "20px", data: "SEL" },
                            { title: "MAKE", data: "MAKE" },
                            { title: "MODEL", data: "MODEL" },
                            { title: "SER #", data: "SERNO" },
                            { title: "REG #", data: "REGNO" },
                            { title: "STATUS", data: "STATUS" },
                            { title: "ASKING", data: "ASKING" },
                            { title: "YEARMFG", data: "YEARMFG" },
                            { title: "PAX", data: "PAX" },
                            { title: "RANGE", data: "RANGE" },
                            { title: "AFTT", data: "AFTT" }
                        ];
                    }
                    break;
            }

            if (typeof $("#" + tableName) !== "undefined") {
                if ($("#" + tableName).length) {

                    selectedRows = "<%= selected_aircraft_rows.clientID %>";

                    //jQuery("#" + tableName).css('display', 'block');

                    var clone = jQuery("#" + tableName).clone(true);

                    jQuery("#" + tableName).css('display', 'none');
                    clone[0].setAttribute('id', jQueryTablename);
                    clone.appendTo("#" + divName);

                    var cw = $('.mainHolder').width() - 40;
                    $("#searchInnerTable").width(cw);

                    var table = $("#" + jQueryTablename).DataTable({
                        destroy: true,
                        data: dynamicDataSet,
                        columns: columnSetArray,
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
                        scroller: true,
                        deferRender: true,
                        stateSave: true,
                        paging: true,
                        processing: true,
                        autoWidth: false,
                        scrollY: 390,
                        scrollX: cw,
                        pageLength: 100,
                        columnDefs: [
                            //{ targets: [1], className: 'display_none' },
                            { orderable: false, className: 'select-checkbox', width: '10px', targets: [0] }
                        ],
                        select: { style: 'multi', selector: 'td:first-child' },
                        order: [[2, 'asc']],
                        dom: 'Bfitrp',
                        buttons: [
                            { extend: 'csv', exportOptions: { columns: ':visible' } },
                            { extend: 'excel', exportOptions: { columns: ':visible' } },
                            { extend: 'pdf', orientation: 'landscape', pageSize: 'A2', exportOptions: { columns: ':visible' } },
                            { extend: 'colvis', text: 'Columns', collectionLayout: 'fixed two-column', postfixButtons: ['colvisRestore'] },

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
            }

            $(".RefreshTableValue").addClass('display_none');
            //$(".KeepTableRow").addClass('display_none');

            $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            $($.fn.dataTable.tables(true)).DataTable().scroller.measure();

        };

        function clickSearch() {
            $('#<%= findAC.clientID %>').click();
            return false;
        }
        function AddCommas(t) { return String(t).replace(/(\d)(?=(\d{3})+$)/g, "$1,") }
    </script>

    <link rel="stylesheet" href="/abiFiles/css/jquery.bxslider.css" type="text/css" />

    <script type="text/javascript" src="/abiFiles/js/jquery.bxslider.min.js"></script>

</asp:Content>
